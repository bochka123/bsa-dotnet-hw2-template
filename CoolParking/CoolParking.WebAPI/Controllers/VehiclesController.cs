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
        public ActionResult<string> GetAll()
        {
            var allVehicles = _parkingService.GetVehicles();
            string results = JsonConvert.SerializeObject(allVehicles);
            return Ok(results);
        }
        // GET: api/Vehicles/id
        [HttpGet("{id}")]
        public ActionResult<string> GetById(string id)
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
        public ActionResult<string> AddVehicle([FromBody] Vehicle vehicle)
        {
            if (vehicle == null)
                return BadRequest();
            try
            {
                _parkingService.AddVehicle(vehicle);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Created("api/Vehicles", JsonConvert.SerializeObject(vehicle));
        }
        // DELETE: api/Vehicles/id
        [HttpDelete("{id}")]
        public ActionResult<string> RemoveVehicle(string id)
        {
            if (!Regex.Match(id, @"[A-Z]{2}-\d{4}-[A-Z]{2}").Success)
                return BadRequest("Wrong id");
            try
            {
                _parkingService.RemoveVehicle(id);
            }
            catch (Exception)
            {
                return NotFound("No vehicle with such id");
            }
            return StatusCode(204, "You successfuly deleted your vehicle");
        }
    }
}
