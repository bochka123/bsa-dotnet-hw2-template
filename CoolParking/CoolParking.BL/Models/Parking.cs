// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.
using System.Collections.Generic;

namespace CoolParking.BL.Models;

public class Parking
{
    public decimal Balance { get; set; }
    public int Capacity { get; }
    public IList<Vehicle> Vehicles { get; set; }
    public IList<TransactionInfo> TransactionsBeforeLog { get; set; }
    public decimal BalanceBeforeLog { get; set; }
    public IDictionary<VehicleType, decimal> Tarrifs { get; }
    private static Parking _instance;
    public decimal PenaltyFactor { get; }
    private Parking()
    {
        Capacity = Settings.Capacity;
        Tarrifs = Settings.Tariffs;
        PenaltyFactor = Settings.PenaltyFactor;
        Balance = Settings.Balance;
        BalanceBeforeLog = 0;
        Vehicles = new List<Vehicle>();
        TransactionsBeforeLog = new List<TransactionInfo>();
    }
    public static Parking GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Parking();
        }
        return _instance;
    }
}