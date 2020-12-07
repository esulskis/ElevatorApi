using System.Collections.Generic;
using System.Threading.Tasks;
using Elevator.Api.Models;
using Elevator.Api.Models.Requests;
using Elevator.Api.Models.Responses;

namespace Elevator.Api.Services.Interfaces
{
    public interface IElevatorService
    {
        Task<bool> CallElevator(CallElevatorRequest request);
        List<GetElevatorLogsResponse> GetElevatorLogs(GetElevatorLogsRequest request);
        GetElevatorStatusResponse GetElevatorStatus(int elevatorId);
    }
}