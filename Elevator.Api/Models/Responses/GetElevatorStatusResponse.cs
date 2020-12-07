using Elevator.Api.Enums;

namespace Elevator.Api.Models.Responses
{
    public class GetElevatorStatusResponse
    {
        public ElevatorStatus  ElevatorStatus {get; set; }
        public DoorStatus DoorStatus { get; set; }
        public int CurrentFloor { get; set; }
        public int DestinationFloor { get; set; }
    }
}