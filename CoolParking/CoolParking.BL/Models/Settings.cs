﻿// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.
using System.Collections.Generic;

namespace CoolParking.BL.Models;

public static class Settings
{
    public static decimal Balance { get => 0; }
    public static int Capacity { get => 10; }
    public static double PeriodOfPayment { get => 5; }
    public static double PeriodOfWritingToLog { get => 60; }
    public static IDictionary<VehicleType, decimal> Tariffs { get => new Dictionary<VehicleType, decimal>() {
            { VehicleType.PassengerCar, 2 },
            { VehicleType.Truck, 5 },
            { VehicleType.Bus, 3.5m },
            { VehicleType.Motorcycle, 1 },
        }; 
    }
    public static decimal PenaltyFactor { get => 2.5m; }
}