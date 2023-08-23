using System.Collections.Generic;
using AttendanceService.Model;

namespace AttendanceService.HAL
{
    public interface IAttendanceDevice : IDevice
    {

        void GetLogs();
    }
}
