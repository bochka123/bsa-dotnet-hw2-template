// TODO: implement class TimerService from the ITimerService interface.
//       Service have to be just wrapper on System Timers.
using CoolParking.BL.Interfaces;

using System.Timers;

namespace CoolParking.BL.Services;

public class TimerService : ITimerService
{
    public double Interval { get; set; }

    public event ElapsedEventHandler Elapsed;

    private Timer _timer;

    public void Dispose()
    {
        this.Dispose();
    }

    public void Start()
    {
        _timer = new Timer();
        _timer.Interval = Interval;
        _timer.Elapsed += Elapsed;
        _timer.Enabled = true;
    }

    public void Stop()
    {
        _timer.Stop();
    }
}
