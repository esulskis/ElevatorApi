using System;

namespace Elevator.Api.Database.Entities
{
    public class ElevatorLogEntity
    {
        public int ElevatorId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}