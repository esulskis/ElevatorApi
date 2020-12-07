using System;
using Elevator.Api.Enums;

namespace Elevator.Api.Database.Entities
{
    public class ElevatorEntity
    {
        public int Id { get; set; }
        public ElevatorStatus  ElevatorStatus {get; set; }
        public DoorStatus DoorStatus { get; set; }
        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }
    }
}