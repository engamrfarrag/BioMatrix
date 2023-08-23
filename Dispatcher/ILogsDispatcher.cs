using AttendanceService.HAL;
using AttendanceService.Model;
using System.Collections.Generic;

namespace AttendanceService.Dispatcher
{
    public interface ILogsDispatcher
    {
        void EventDispatch(IDevice device, List<LogInfo> logs);

    }
}
