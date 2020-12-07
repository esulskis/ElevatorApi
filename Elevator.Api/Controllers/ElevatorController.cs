using System;
using System.Linq;
using System.Threading.Tasks;
using Elevator.Api.Config;
using Elevator.Api.Database;
using Elevator.Api.Database.Entities;
using Elevator.Api.Models;
using Elevator.Api.Models.Requests;
using Elevator.Api.Models.Responses;
using Elevator.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Elevator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElevatorController : ControllerBase
    {
        private readonly IElevatorService _elevatorService;
        private readonly AppSettings _appSettings;
        public ElevatorController(IElevatorService elevatorService, IOptions<AppSettings> settings)
        {
            _elevatorService = elevatorService;
            _appSettings = settings.Value;
        }
        
        [HttpGet("CallElevator")]
        public async Task<IActionResult> CallElevator([FromQuery]CallElevatorRequest request)
        {
            if (request.DestinationFloor > _appSettings.MaxFloors)
            {
                return BadRequest($"Max floor {_appSettings.MaxFloors}");
            }
            
            var isSuccess = await _elevatorService.CallElevator(request);
            if (!isSuccess)
            {
                return NotFound("Failed to call elevator");
            }
            return  Ok();
        }
        
        [HttpGet("GetElevatorStatus")]
        public IActionResult GetElevatorStatus([FromQuery] int id)
        {
            var response = _elevatorService.GetElevatorStatus(id);
            return  Ok(response);
        }
        
        [HttpGet("GetElevatorLogInformation")]
        public IActionResult GetElevatorLogInformation([FromQuery] GetElevatorLogsRequest request)
        {
            var response = _elevatorService.GetElevatorLogs(request);
            return  Ok(response);
        }
    }
}