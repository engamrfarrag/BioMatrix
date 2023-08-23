using AttendanceService.Dispatcher;
using AttendanceService.HAL;
using AttendanceService.Model;
using AttendanceService.Providers;
using AttendanceService.Util;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceService.Managers
{
    public class DevicesManager
    {
        static List<IManagedAttendencDevice> _managedAttendencDevices = new List<IManagedAttendencDevice>();
        readonly ILogsDispatcher _logsDispatcher;
        public DevicesManager(IDevicesProvider provider, ILogsDispatcher dispatcher)
        {
            while(_managedAttendencDevices.Count == 0) _managedAttendencDevices = provider.GetDevices();
            this._logsDispatcher = dispatcher;
            this.ConfigMangedDevices();
            
        }


        private void ConfigMangedDevices()
        {
            if (_managedAttendencDevices.Count == 0) return;
            _managedAttendencDevices.ToList().ForEach(device => ConfigDevice(device));
        }
        private void ConfigDevice(IManagedAttendencDevice device)
        {
            device.LogsArrived += Device_LogsArrived;
            device.Connect();
            var timer = new System.Threading.Timer(CheckState, device, 60 * 1000, 60 * 3 * 1000);
        }

        private void Device_LogsArrived(IDevice sender, List<LogInfo> logs)
        {
            this._logsDispatcher.EventDispatch(sender, logs);

        }
        private void CheckState(object state)
        {
            IManagedAttendencDevice zk = (IManagedAttendencDevice)state;
            bool status = Network.PingTheDevice(zk.IpAddress);
            if (status && zk.State == Status.Disconnected)
            {
                zk.Connect();
            }
            else if (!status && zk.State == Status.Connected)
            {
                zk.Disconnect();
            }
        }
    }
}
