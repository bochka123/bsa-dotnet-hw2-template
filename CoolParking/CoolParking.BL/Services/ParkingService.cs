// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Timers;
using System.Linq;

namespace CoolParking.BL.Services;

public class ParkingService : IParkingService
{
    private readonly ITimerService _withdrawTimer;
    private readonly ITimerService _logTimer;
    private readonly ILogService _logService;
    private Parking parking;
    private bool disposed = false;

    public ParkingService(ITimerService withdrawTimer, ITimerService logTimer, ILogService logService)
    {
        _withdrawTimer = withdrawTimer;
        _logTimer = logTimer;
        _logService = logService;
        parking = Parking.GetInstance();
        withdrawTimer.Interval = Settings.PeriodOfPayment;
        logTimer.Interval = Settings.PeriodOfWritingToLog;
        withdrawTimer.Elapsed += Payment;
        logTimer.Elapsed += WriteToLog;
        withdrawTimer.Start();
        logTimer.Start();
    }
    public void AddVehicle(Vehicle vehicle)
    {
        if (parking.Vehicles.Count >= parking.Capacity)
            throw new InvalidOperationException("There are no free places on parking");
        foreach (Vehicle vehicle1 in GetVehicles())
        {
            if (vehicle.Id == vehicle1.Id)
                throw new ArgumentException("There is vehicle with such Id");
        }
        if (vehicle == null)
            throw new ArgumentNullException("Exception because of null value");
        parking.Vehicles.Add(vehicle);
    }

    public void Dispose()
    {
        if (!disposed)
        {
            lock(parking.TransactionsBeforeLog)
            {
                parking.TransactionsBeforeLog.Clear();
            }
            parking.Balance = 0;
            parking.Vehicles.Clear();
            parking.BalanceBeforeLog = 0;
        }
    }
    public decimal GetBalance()
    {
        return parking.Balance;
    }

    public int GetCapacity()
    {
        return parking.Capacity;
    }

    public int GetFreePlaces()
    {
        return parking.Capacity - parking.Vehicles.Count;
    }
    public Vehicle GetById(string id)
    {
        var vehicles = GetVehicles();
        foreach (var vehicle in vehicles)
        {
            if (vehicle.Id.Equals(id))
            {
                return vehicle;
            }
        }
        throw new ArgumentException();
    }
    public TransactionInfo[] GetLastParkingTransactions()
    {
        lock (parking.TransactionsBeforeLog)
        {
            return parking.TransactionsBeforeLog.ToArray();
        }
    }

    public ReadOnlyCollection<Vehicle> GetVehicles()
    {
        ReadOnlyCollection<Vehicle> vehicles = new ReadOnlyCollection<Vehicle>(parking.Vehicles);
        return vehicles;
    }

    public string ReadFromLog()
    {
        var result = _logService.Read();
        if (result == null)
            return "Log file is empty";
        return result;
    }

    public void RemoveVehicle(string vehicleId)
    {
        Vehicle currentVehicle = null;
        foreach (Vehicle vehicle in GetVehicles())
        {
            if (vehicle.Id == vehicleId)
            {
                currentVehicle = vehicle;
            }
        }
        if (currentVehicle == null)
            throw new ArgumentException("There is no car with such id");
        if (currentVehicle.Balance < 0)
            throw new InvalidOperationException("Balance of the car is negative, make it positive to take the car");
        parking.Vehicles.Remove(currentVehicle);
    }

    public void TopUpVehicle(string vehicleId, decimal sum)
    {
        if (sum < 0)
            throw new ArgumentException("Sum is less than zero");
        Vehicle currentVehicle = null;
        foreach (Vehicle vehicle in GetVehicles())
        {
            if (vehicle.Id.Equals(vehicleId))
            {
                currentVehicle = vehicle;
                break;
            }
        }
        if (currentVehicle == null)
            throw new ArgumentException("There is no vehicle with such Id");
        currentVehicle.Balance += sum;
    }
    public void Payment(object source, ElapsedEventArgs e)
    {
        foreach (Vehicle vehicle in GetVehicles())
        {
            decimal price = parking.Tarrifs[vehicle.VehicleType];
            if (vehicle.Balance < price)
            {
                if (vehicle.Balance < 0)
                {
                    price *= parking.PenaltyFactor;
                }
                else
                {
                    decimal diff = price - vehicle.Balance;
                    price = diff * parking.PenaltyFactor + vehicle.Balance;
                }
            }
            TransactionInfo transaction = new TransactionInfo();
            transaction.Sum = price;
            transaction.VehicleId = vehicle.Id;
            transaction.DateTime = DateTime.Now;
            lock (parking.TransactionsBeforeLog)
            {
                parking.TransactionsBeforeLog.Add(transaction);
            }
            vehicle.Balance -= price;
            parking.Balance += price;
            parking.BalanceBeforeLog += price;
        }
    }
    public void WriteToLog(object source, ElapsedEventArgs e)
    {
        string transactions = String.Empty;
        lock (parking.TransactionsBeforeLog)
        {
            foreach (var transaction in parking.TransactionsBeforeLog.ToArray())
                transactions += $"{transaction}\n";
        }
        _logService.Write(transactions);
        lock (parking.TransactionsBeforeLog)
        {
            parking.TransactionsBeforeLog.Clear();
        }
        parking.BalanceBeforeLog = 0;
    }
    public decimal GetBalanceBeforeLog()
    {
        return parking.BalanceBeforeLog;
    }
}