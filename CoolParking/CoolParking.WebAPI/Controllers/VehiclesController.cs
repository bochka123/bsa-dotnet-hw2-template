using Microsoft.AspNetCore.Mvc;
using CoolParking.BL.Services;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System.Reflection;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private string _logFilePath { get => $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log"; }
        private readonly ITimerService _withdrawTimer;
        private readonly ITimerService _logTimer;
        private readonly ILogService _logService;
        private readonly IParkingService _parkingService;
        public VehiclesController()
        {
            _withdrawTimer = new TimerService();
            _logTimer = new TimerService();
            _logService = new LogService(_logFilePath);
            _parkingService = new ParkingService(_withdrawTimer, _logTimer, _logService);
        }
        // GET: api/Vehicles
        [HttpGet]
        public string GetAll()
        {
            var allVehicles = _parkingService.GetVehicles();
            string results = JsonConvert.SerializeObject(allVehicles);
            return results;
        }
        // GET: api/Vehicles/id
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            if (!Regex.Match(id, @"[A-Z]{2}-\d{4}-[A-Z]{2}").Success)
                return BadRequest();
            string vehicle;
            try
            {
                vehicle = JsonConvert.SerializeObject(_parkingService.GetById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        // POST: api/Vehicles
        [HttpPost]
        public IActionResult AddVehicle(string vehicleJson)
        {
            if (vehicleJson == null)
                return BadRequest();
            #pragma warning disable CS8600
            Vehicle vehicle = JsonConvert.DeserializeObject<Vehicle>(vehicleJson);
            #pragma warning restore CS8600
            try
            {
                _parkingService.AddVehicle(vehicle);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return StatusCode(201, JsonConvert.SerializeObject(vehicle));
        }
        // DELETE: api/Vehicles/id
        [HttpDelete("{id}")]
        public IActionResult RemoveVehicle(string id)
        {
            if (!Regex.Match(id, @"[A-Z]{2}-\d{4}-[A-Z]{2}").Success)
                return BadRequest();
            try
            {
                _parkingService.RemoveVehicle(id);
            }
            catch (Exception)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
