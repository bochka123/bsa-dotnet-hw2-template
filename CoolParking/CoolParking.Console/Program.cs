using CoolParking.BL.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CoolParking.WebAPI.Controllers;
using Newtonsoft.Json;



ParkingController parkingController = new ParkingController();
VehiclesController vehiclesController = new VehiclesController();
TransactionsController transactionsController = new TransactionsController();
Console.WriteLine("Welcome to \"Cool Parking\"");
Console.WriteLine("Enter \"Help\" to get info about all commands");
while (true)
{
    string command = Console.ReadLine();
    switch (command)
    {
        case "Help":
            Console.WriteLine("Enter \"Help\" to get info about all commands");
            Console.WriteLine("Enter \"Balance\" to get info about current balance");
            Console.WriteLine("Enter \"Free\" to get info about free places on parking");
            Console.WriteLine("Enter \"History\" to get info about transactions in log");
            Console.WriteLine("Enter \"GetLastTransaction\" to get info about transactions in log");
            Console.WriteLine("Enter \"Vehicles\" to get info about vehicles on parking");
            Console.WriteLine("Enter \"GetById\" to get vehicle by id");
            Console.WriteLine("Enter \"Put\" to put your vehicle on our parking");
            Console.WriteLine("Enter \"Take\" to take your vehicle from our parking");
            Console.WriteLine("Enter \"TopUp\" to top up balance of your vehicle from our parking");
            Console.WriteLine("Enter \"Quit\" to quit the program");
            break;
        case "Balance":
            Console.WriteLine(parkingController.GetBalance());
            break;
        case "Free":
            Console.WriteLine($"{parkingController.GetFreePlaces()}/{parkingController.GetCapacity()}");
            break;
        case "History":
            Console.WriteLine(transactionsController.GetAll());
            break;
        case "GetLastTransaction":
            Console.WriteLine(transactionsController.GetLast());
            break;
        case "Vehicles":
            Console.WriteLine(vehiclesController.GetAll());
            break;
        case "Put":
            VehicleType type = VehicleType.PassengerCar;
            Console.WriteLine("Enter type of your vehicle (PassengerCar, Truck, Bus, Motorcycle)");
            bool check = true;
            while (check)
            {
                string typeString = Console.ReadLine();
                switch (typeString)
                {
                    case "PassengerCar":
                        type = VehicleType.PassengerCar;
                        check = false;
                        break;
                    case "Truck":
                        type = VehicleType.Truck;
                        check = false;
                        break;
                    case "Bus":
                        type = VehicleType.Bus;
                        check = false;
                        break;
                    case "Motorcycle":
                        type = VehicleType.Motorcycle;
                        check = false;
                        break;
                    default:
                        Console.WriteLine("There is no such type :(");
                        break;
                }
            }
            Console.WriteLine("Enter base balance for your vehicle");
            check = true;
            decimal balance = 0;
            while (check)
            {
                try
                {
                    balance = Convert.ToDecimal(Console.ReadLine());
                    check = false;
                }
                catch
                {
                    Console.WriteLine("You entered not a number :(");
                }
            }
            try
            {
                Vehicle vehicle = new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), type, balance);
                vehiclesController.AddVehicle(JsonConvert.SerializeObject(vehicle));
                Console.WriteLine("You put your vehicle on our parking successfuly");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "GetById":
            Console.WriteLine("Enter id of vehicle");
            string id = Console.ReadLine();
            Console.WriteLine(vehiclesController.GetById(id));
            break;
        case "Take":
            Console.WriteLine("Enter Id of your car");
            string id1 = Console.ReadLine();
            try
            {
                vehiclesController.RemoveVehicle(id1);
                Console.WriteLine("You removed your vehicle on our parking successfuly");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "TopUp":
            Console.WriteLine("Enter Id of your car");
            string vehicleId = Console.ReadLine();
            Console.WriteLine("Enter sum you want to top up");
            bool check1 = true;
            decimal balance1 = 0;
            while (check1)
            {
                try
                {
                    balance1 = Convert.ToDecimal(Console.ReadLine());
                    check1 = false;
                }
                catch
                {
                    Console.WriteLine("You entered not a number :(");
                }
            }
            try
            {
                transactionsController.TopUpVehicle(vehicleId, balance1);
                Console.WriteLine("You topped up your vehicle on our parking successfuly");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "Quit":
            return;
        default:
            Console.WriteLine($"\"{command}\" is not a command :(");
            break;
    }
}