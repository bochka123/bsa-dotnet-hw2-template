// See https://aka.ms/new-console-template for more information
using CoolParking.BL.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Services;

ITimerService timerService = new TimerService(Settings.PeriodOfPayment);

//string _logFilePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log";
//ILogService logService = new LogService(_logFilePath);
//IParkingService parkingService = new ParkingService(new TimerService(), new TimerService(), logService);
//Console.WriteLine("Welcome to \"Cool Parking\"");
//Console.WriteLine("Enter \"Help\" to get info about all commands");
//while (true)
//{
//    string command = Console.ReadLine();
//    switch (command)
//    {
//        case "Help":
//            Console.WriteLine("Enter \"Help\" to get info about all commands");
//            Console.WriteLine("Enter \"Quit\" to quit the program");
//            break;
//        case "Quit":
//            return;
//        default:
//            Console.WriteLine($"\"{command}\" is not a command :(");
//            break;
//    }
//}