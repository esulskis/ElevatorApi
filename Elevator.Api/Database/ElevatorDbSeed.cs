using System;
using System.Collections.Generic;
using Elevator.Api.Database.Entities;
using Elevator.Api.Enums;
using Elevator.Api.Models;

namespace Elevator.Api.Database
{
    public static class ElevatorDatabaseSeed
    {
        public static void Seed(this ElevatorDbContext dbContext)
        {
           var elevators = new List<ElevatorEntity>()
           {
               new ElevatorEntity
               {
                   Id = 1,
                   DoorStatus = DoorStatus.Open,
                   ElevatorStatus = ElevatorStatus.Waiting,
                   CurrentFloor = 1
               },
               new ElevatorEntity
               {
                   Id = 2,
                   DoorStatus = DoorStatus.Open,
                   ElevatorStatus = ElevatorStatus.Waiting,
                   CurrentFloor = 5
               }
           };
           dbContext.Elevators.InsertBulk(elevators);
        }
    }
}