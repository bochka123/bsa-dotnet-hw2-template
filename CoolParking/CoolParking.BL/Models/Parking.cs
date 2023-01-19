// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.
using System.Collections.Generic;

namespace CoolParking.BL.Models;

public class Parking
{
    public decimal Balance { get; set;  }
    public int Capacity { get; }
    public IList<Vehicle> Vehicles { get; set; }
    private static Parking _instance;
    private Parking()
    {
        Capacity = Settings.Capacity;
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