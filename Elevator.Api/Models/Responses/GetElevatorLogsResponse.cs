using System;

namespace Elevator.Api.Models.Responses
{
    public class GetElevatorLogsResponse
    {
        public DateTime Date { get; set; }
        public string LogText { get; set; }
    }
}