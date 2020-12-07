using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elevator.Api.Database;
using Elevator.Api.Database.Entities;
using Elevator.Api.Enums;
using Elevator.Api.Models;
using Elevator.Api.Models.Requests;
using Elevator.Api.Models.Responses;
using Elevator.Api.Services.Interfaces;

namespace Elevator.Api.Services
{
   
    public class ElevatorService : IElevatorService
    {
        private readonly IElevatorDBContext _dbContext;
        public ElevatorService(IElevatorDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CallElevator(CallElevatorRequest request)
        {
            var elevator = GetNearestAvailableElevator(request.DestinationFloor);
            if (elevator == null)
            {
                return false;
            }

            await SendElevator(elevator, request.DestinationFloor);
            return true;
        }

        private ElevatorEntity GetNearestAvailableElevator(int destinationFloor)
        {
            var availableElevators = _dbContext.Elevators
                .Find(x => x.ElevatorStatus == ElevatorStatus.Waiting);

            ElevatorEntity closestElevatorEntity = null;
            var shortestDistance = int.MaxValue;
            foreach (var availableElevator in availableElevators)
            {
                var distance = Math.Abs(availableElevator.CurrentFloor - destinationFloor);
                if (shortestDistance <= distance) continue;
                shortestDistance = distance;
                closestElevatorEntity = availableElevator;
            }
            return closestElevatorEntity;
        }
        
        private async Task SendElevator(ElevatorEntity elevator,int destinationFloor)
        {
            elevator.ElevatorStatus = ElevatorStatus.Moving;
            elevator.DestinationFloor = destinationFloor;
            _dbContext.Elevators.Update(elevator);
            AddElevatorLog(elevator.Id, $"Elevator called to {destinationFloor} floor");
            
            await Task.Delay(2000);
            elevator.DoorStatus = DoorStatus.Closed;
            _dbContext.Elevators.Update(elevator);
            AddElevatorLog(elevator.Id, "Elevator doors closed");
            
            var currentFloor = elevator.CurrentFloor;
            while (currentFloor != destinationFloor)
            {
                await Task.Delay(1000);
                if (currentFloor > destinationFloor)
                {
                    currentFloor--;
                }else
                {
                    currentFloor++;
                }

                elevator.CurrentFloor = currentFloor;
                _dbContext.Elevators.Update(elevator);
                AddElevatorLog(elevator.Id, $"Elavator reached {currentFloor} floor");
            }
            
            await Task.Delay(2000);
            elevator.DoorStatus = DoorStatus.Open;
            elevator.ElevatorStatus = ElevatorStatus.Waiting;
            _dbContext.Elevators.Update(elevator); 
            AddElevatorLog(elevator.Id, "Elevator doors opened");
        }

        private void AddElevatorLog(int elevatorId, string text)
        {
            var elevatorLog = new ElevatorLogEntity
            {
                ElevatorId = elevatorId,
                Text = text,
                Date = DateTime.Now
            };
            _dbContext.ElevatorLogs.Insert(elevatorLog);
        }

        public GetElevatorStatusResponse GetElevatorStatus(int elevatorId)
        {
            var elevator = _dbContext.Elevators.FindOne(x => x.Id == elevatorId);
            return new GetElevatorStatusResponse
            {
                CurrentFloor = elevator.CurrentFloor,
                DestinationFloor = elevator.CurrentFloor,
                DoorStatus = elevator.DoorStatus,
                ElevatorStatus = elevator.ElevatorStatus
            };
        }
        
        public List<GetElevatorLogsResponse> GetElevatorLogs(GetElevatorLogsRequest request)
        {
            var elevatorLogsQuery = _dbContext.ElevatorLogs.Query()
                .Where(x=>x.ElevatorId == request.ElevatorId);
            if (request.DateFrom.HasValue)
            {
                elevatorLogsQuery = elevatorLogsQuery.Where(x => x.Date <= request.DateFrom);
            }

            if (request.DateTo.HasValue)
            {
                elevatorLogsQuery = elevatorLogsQuery.Where(x => x.Date >= request.DateTo);
            }

            return elevatorLogsQuery.Select(x => new GetElevatorLogsResponse
            {
                Date = x.Date,
                LogText = x.Text
            }).ToList();
        }
    }
}