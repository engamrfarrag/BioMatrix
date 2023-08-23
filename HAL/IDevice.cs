using System;

namespace AttendanceService.HAL
{
    public interface IDevice
    {
        string IpAddress { get; }
        string Password { get; }
        int Port { get; }
        DateTime LastLog { get; }
        int Id { get; }
    }
}
