using Microsoft.AspNetCore.Mvc;
using CoolParking.BL.Services;
using CoolParking.BL.Interfaces;
using System.Reflection;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using CoolParking.BL.Models;

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
        public ActionResult<string> GetLast()
        {
            return Ok(_parkingService.GetLastParkingTransactions().Last().ToString());
        }
        // GET: api/Transactions/All
        [HttpGet("All")]
        public ActionResult<string> GetAll()
        {
            return Ok(_parkingService.ReadFromLog());
        }
        // PUT: api/Transactions/topUpVehicle
        [HttpPut("topUpVehicle")]
        public ActionResult<string> TopUpVehicle(string info)
        {
            TransactionInfo transactionInfo = JsonConvert.DeserializeObject<TransactionInfo>(info);
            if (!Regex.Match(transactionInfo.VehicleId, @"[A-Z]{2}-\d{4}-[A-Z]{2}").Success || transactionInfo.Sum < 0)
                return BadRequest();
            string vehicle;
            try
            {
                _parkingService.TopUpVehicle(transactionInfo.VehicleId, transactionInfo.Sum);
                vehicle = JsonConvert.SerializeObject(_parkingService.GetById(transactionInfo.VehicleId));
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
    }
}
