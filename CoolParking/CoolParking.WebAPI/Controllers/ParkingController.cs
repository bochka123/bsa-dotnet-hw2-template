using Microsoft.AspNetCore.Mvc;
using CoolParking.BL.Services;
using CoolParking.BL.Interfaces;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private string _logFilePath { get => $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log"; }
        private readonly ITimerService _withdrawTimer;
        private readonly ITimerService _logTimer;
        private readonly ILogService _logService;
        private readonly IParkingService _parkingService;
        public ParkingController()
        {
            _withdrawTimer = new TimerService();
            _logTimer = new TimerService();
            _logService = new LogService(_logFilePath);
            _parkingService = new ParkingService(_withdrawTimer, _logTimer, _logService);
        }
        // GET: api/Parking/Balance
        [HttpGet("Balance")]
        public decimal GetBalance()
        {
            return _parkingService.GetBalance();
        }

        // GET: api/Parking/Capacity
        [HttpGet("Capacity")]
        public int GetCapacity()
        {
            return _parkingService.GetCapacity();
        }

        // GET: api/Parking/FreePlaces
        [HttpGet("FreePlaces")]
        public int GetFreePlaces()
        {
            return _parkingService.GetFreePlaces();
        }
    }
}
