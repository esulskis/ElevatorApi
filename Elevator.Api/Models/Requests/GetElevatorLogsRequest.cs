using System;

namespace Elevator.Api.Models.Requests
{
    public class GetElevatorLogsRequest
    {
        public int ElevatorId { get; set; }
        public DateTime? DateFrom {get; set; }
        public DateTime? DateTo { get; set; }
    }
}