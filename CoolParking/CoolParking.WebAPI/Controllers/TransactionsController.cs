using Microsoft.AspNetCore.Mvc;
using CoolParking.BL.Services;
using CoolParking.BL.Interfaces;
using System.Reflection;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private string _logFilePath { get => $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log"; }
        private readonly ITimerService _withdrawTimer;
        private readonly ITimerService _logTimer;
        private readonly ILogService _logService;
        private readonly IParkingService _parkingService;
        public TransactionsController()
        {
            _withdrawTimer = new TimerService();
            _logTimer = new TimerService();
            _logService = new LogService(_logFilePath);
            _parkingService = new ParkingService(_withdrawTimer, _logTimer, _logService);
        }
        // GET: api/Transactions/Last
        [HttpGet("Last")]
        public string GetLast()
        {
            return _parkingService.GetLastParkingTransactions().Last().ToString();
        }
        // GET: api/Transactions/All
        [HttpGet("All")]
        public string GetAll()
        {
            return _parkingService.ReadFromLog();
        }
        // PUT: api/Transactions/topUpVehicle
        [HttpPut("topUpVehicle")]
        public IActionResult TopUpVehicle(string id, decimal sum)
        {
            if (!Regex.Match(id, @"[A-Z]{2}-\d{4}-[A-Z]{2}").Success || sum < 0)
                return BadRequest();
            string vehicle;
            try
            {
                _parkingService.TopUpVehicle(id, sum);
                vehicle = JsonConvert.SerializeObject(_parkingService.GetById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
    }
}
