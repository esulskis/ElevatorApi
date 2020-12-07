using Elevator.Api.Database.Entities;
using LiteDB;

namespace Elevator.Api.Database
{
    public interface IElevatorDBContext
    {
        ILiteCollection<Entities.ElevatorEntity> Elevators { get; } 
        ILiteCollection<ElevatorLogEntity> ElevatorLogs { get; } 
    }
}