using AttendanceService.Model;
using System;
using System.Collections.Generic;

namespace AttendanceService.HAL
{
    public delegate void Notify(IDevice sender, List<LogInfo> logs);
    public interface IManagedAttendencDevice : ILifeCycle, IAttendanceDevice
    {
        event Notify LogsArrived;
    }
}
