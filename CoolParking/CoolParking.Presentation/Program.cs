using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Newtonsoft.Json;
using System.Net.Http;
using CoolParking.Presentation.ViewModels;
using System.Text;

namespace CoolParking.Presentation
{
    public class Program
    { 
        public async static Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/api/");
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
                        Console.WriteLine("Enter \"Remove\" to remove your vehicle out of our parking");
                        Console.WriteLine("Enter \"TopUp\" to top up balance of your vehicle from our parking");
                        Console.WriteLine("Enter \"Quit\" to quit the program");
                        break;
                    case "Balance":
                        string balance = await client.GetStringAsync("Parking/Balance");
                        Console.WriteLine(balance);
                        break;
                    case "Free":
                        string freeplaces = await client.GetStringAsync("Parking/Freeplaces");
                        string capacity = await client.GetStringAsync("Parking/Capacity");
                        Console.WriteLine($"{freeplaces}/{capacity}");
                        break;
                    case "History":
                        Console.WriteLine(await client.GetStringAsync("Transactions/All"));
                        break;
                    case "GetLastTransaction":
                        Console.WriteLine(await client.GetStringAsync("Transactions/Last"));
                        break;
                    case "Vehicles":
                        string allVehicles = await client.GetStringAsync("Vehicles");
                        Console.WriteLine(allVehicles);
                        break;
                    case "Put":
                        VehicleTypeViewModel type = VehicleTypeViewModel.PassengerCar;
                        Console.WriteLine("Enter type of your vehicle (PassengerCar, Truck, Bus, Motorcycle)");
                        bool check = true;
                        while (check)
                        {
                            string typeString = Console.ReadLine();
                            switch (typeString)
                            {
                                case "PassengerCar":
                                    type = VehicleTypeViewModel.PassengerCar;
                                    check = false;
                                    break;
                                case "Truck":
                                    type = VehicleTypeViewModel.Truck;
                                    check = false;
                                    break;
                                case "Bus":
                                    type = VehicleTypeViewModel.Bus;
                                    check = false;
                                    break;
                                case "Motorcycle":
                                    type = VehicleTypeViewModel.Motorcycle;
                                    check = false;
                                    break;
                                default:
                                    Console.WriteLine("There is no such type :(");
                                    break;
                            }
                        }
                        Console.WriteLine("Enter base balance for your vehicle");
                        check = true;
                        decimal vehicleBalance = 0;
                        while (check)
                        {
                            try
                            {
                                vehicleBalance = Convert.ToDecimal(Console.ReadLine());
                                check = false;
                            }
                            catch
                            {
                                Console.WriteLine("You entered not a number :(");
                            }
                        }
                        try
                        {
                            VehicleViewModel vehicle = new VehicleViewModel(VehicleViewModel.GenerateRandomRegistrationPlateNumber(), type, vehicleBalance);
                            string vehicleJson = JsonConvert.SerializeObject(vehicle);
                            var content = new StringContent(vehicleJson, Encoding.UTF8, "application/json");
                            var result = await client.PostAsync("Vehicles", content);
                            string resultContent = await result.Content.ReadAsStringAsync();
                            Console.WriteLine("You put your vehicle on our parking successfuly\n" + resultContent);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "GetById":
                        Console.WriteLine("Enter id of vehicle");
                        string id = Console.ReadLine();
                        string vehicleById = await client.GetStringAsync($"Vehicles/{id}");
                        Console.WriteLine(vehicleById);
                        break;
                    case "Remove":
                        Console.WriteLine("Enter id of vehicle");
                        string removeId = Console.ReadLine();
                        Console.WriteLine(await client.DeleteAsync($"Vehicles/{removeId}"));
                        break;
                    case "TopUp":
                        Console.WriteLine("Enter Id of your car");
                        string VehicleId = Console.ReadLine();
                        Console.WriteLine("Enter sum you want to top up");
                        bool check1 = true;
                        decimal Sum = 0;
                        while (check1)
                        {
                            try
                            {
                                Sum = Convert.ToDecimal(Console.ReadLine());
                                check1 = false;
                            }
                            catch
                            {
                                Console.WriteLine("You entered not a number :(");
                            }
                        }
                        try
                        {
                            TransactionViewModel transaction = new TransactionViewModel(VehicleId, Sum);
                            await client.PutAsync("Transactions/topUpVehicle", new StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json"));
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
        }
    }
}