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
        parking.Vehicles = new List<Vehicle>();
    }
    public void AddVehicle(Vehicle vehicle)
    {
        if (parking.Vehicles.Count >= parking.Capacity)
            throw new InvalidOperationException();
        foreach (Vehicle vehicle1 in GetVehicles())
        {
            if (vehicle.Id == vehicle1.Id)
                throw new ArgumentException();
        }
        if (vehicle == null)
            throw new ArgumentNullException();
        parking.Vehicles.Add(vehicle);
    }

    public void Dispose()
    {
        if (!disposed)
        {
            parking.Balance = 0;
            parking.Vehicles.Clear();
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

    public TransactionInfo[] GetLastParkingTransactions()
    {
        throw new NotImplementedException();
    }

    public ReadOnlyCollection<Vehicle> GetVehicles()
    {
        ReadOnlyCollection<Vehicle> vehicles = new ReadOnlyCollection<Vehicle>(parking.Vehicles);
        return vehicles;
    }

    public string ReadFromLog()
    {
        throw new NotImplementedException();
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
            throw new ArgumentException();
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
            throw new ArgumentException();
        currentVehicle.Balance += sum;
    }
}