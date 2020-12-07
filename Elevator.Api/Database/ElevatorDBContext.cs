using System.Linq;
using Elevator.Api.Config;
using Elevator.Api.Database.Entities;
using LiteDB;
using Microsoft.Extensions.Options;

namespace Elevator.Api.Database
{
    public class ElevatorDbContext : IElevatorDBContext
    {
        private LiteDatabase _database { get; }
        public ElevatorDbContext(IOptions<AppSettings> setting)
        {
            _database = new LiteDatabase(setting.Value.LiteDbConnectionString);
            Elevators = _database.GetCollection<ElevatorEntity>(Constants.ElevatorDbTables.Elevator);
            ElevatorLogs = _database.GetCollection<ElevatorLogEntity>(Constants.ElevatorDbTables.ElevatorLog);
            var count = _database.GetCollectionNames().Count();
            if (count == 0)
            {
                this.Seed();
            }
        }
        
        public ILiteCollection<ElevatorEntity> Elevators { get; } 
        public ILiteCollection<ElevatorLogEntity> ElevatorLogs { get; } 
        
    }
}