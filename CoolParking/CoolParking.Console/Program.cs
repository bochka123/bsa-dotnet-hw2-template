using CoolParking.BL.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Services;

string _logFilePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log";
ILogService logService = new LogService(_logFilePath);
ParkingService parkingService = new ParkingService(new TimerService(), new TimerService(), logService);
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
            Console.WriteLine("Enter \"Current\" to get info about earned money for current period");
            Console.WriteLine("Enter \"Free\" to get info about free places on parking");
            Console.WriteLine("Enter \"HistoryBefore\" to get info about transactions for current period");
            Console.WriteLine("Enter \"History\" to get info about transactions in log");
            Console.WriteLine("Enter \"Vehicles\" to get info about vehicles on parking");
            Console.WriteLine("Enter \"Put\" to put your vehicle on our parking");
            Console.WriteLine("Enter \"Take\" to take your vehicle from our parking");
            Console.WriteLine("Enter \"TopUp\" to top up balance of your vehicle from our parking");
            Console.WriteLine("Enter \"Quit\" to quit the program");
            break;
        case "Balance":
            Console.WriteLine(parkingService.GetBalance());
            break;
        case "Current":
            Console.WriteLine(parkingService.GetBalanceBeforeLog());
            break;
        case "Free":
            Console.WriteLine($"{parkingService.GetFreePlaces()}/{parkingService.GetCapacity()}");
            break;
        case "HistoryBefore":
            foreach (var transaction in parkingService.GetLastParkingTransactions())
            {
                Console.WriteLine(transaction.ToString());
            }
            break;
        case "History":
            Console.WriteLine(parkingService.ReadFromLog());
            break;
        case "Vehicles":
            foreach (var car in parkingService.GetVehicles())
            {
                Console.WriteLine(car.ToString());
            }
            break;
        case "Put":
            VehicleType type = VehicleType.PassengerCar;
            Console.WriteLine("Enter type of your vehicle (PassengerCar, Truck, Bus, Motorcycle)");
            bool check = true;
            while (check) {
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
            while (check) {
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
                parkingService.AddVehicle(new Vehicle(Vehicle.GenerateRandomRegistrationPlateNumber(), type, balance));
                Console.WriteLine("You put your vehicle on our parking successfuly");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "Take":
            Console.WriteLine("Enter Id of your car");
            string id = Console.ReadLine();
            try
            {
                parkingService.RemoveVehicle(id);
                Console.WriteLine("You removed your vehicle on our parking successfuly");
            }
            catch(Exception ex)
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
                parkingService.TopUpVehicle(vehicleId, balance1);
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