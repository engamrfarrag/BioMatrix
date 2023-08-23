using AttendanceService.HAL;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AttendanceService.Providers
{
    public interface IDevicesProvider
    {
        List<IManagedAttendencDevice> GetDevices();
        void Refresh(IManagedAttendencDevice device);
    }
}
