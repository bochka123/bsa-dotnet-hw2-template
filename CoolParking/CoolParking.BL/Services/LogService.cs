// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.
using CoolParking.BL.Interfaces;
using System.IO;
using System.Reflection;

namespace CoolParking.BL.Services;

public class LogService : ILogService
{
    public string LogPath { get; }
    public LogService(string logFilePath)
    {
        LogPath = logFilePath;
    }

    public string Read()
    {
        var file = new StreamReader(LogPath);
        string result = file.ReadToEnd();
        file.Close();
        return result;
    }

    public void Write(string logInfo)
    {
        var file = new StreamWriter(LogPath, true);
        file.WriteLine(logInfo);
        file.Close();
    }
}
