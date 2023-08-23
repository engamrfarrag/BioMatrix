using AttendanceService.HAL;
using AttendanceService.Managers.Model;
using AttendanceService.Model;
using AttendanceService.Providers;
using AttendanceService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AttendanceService.Dispatcher
{
    public class LogsManager : ILogsDispatcher
    {
       
        readonly static List<DeviceModel> _devicemodel = new List<DeviceModel>();
       
        public void EventDispatch(IDevice device, List<LogInfo> logs)
        {
          var model= this.PrepareDevice(device);
          logs.ForEach(log => {
              if(log.DateTimeRecord > model.LastLog) {
                  model.Logs.Enqueue(log);
              }
             

          });
           Task.Run(() => this.ProccessQueue(model));
        }
        private DeviceModel PrepareDevice(IDevice device)
        {
            DeviceModel model = new DeviceModel(device.Id) { LastLog = device.LastLog, Logs = new Queue<LogInfo>() };
            if (_devicemodel.Any(ml => device.Id == ml.Id))return _devicemodel.Where(devicemodel => device.Id == devicemodel.Id).First();
            _devicemodel.Add(model);
            return model ;
        }
 
        private void ProccessQueue(DeviceModel model) {

            lock (model.Logs)
            {
                while ( model?.Logs?.Count() >0)
                {
                    var log = model.Logs.Dequeue();
                    bool sent = false;
                    while(!sent) sent=SendLog(log);
                    model.LastLog = log.DateTimeRecord;

                }

            }
    
        }
   
        public bool SendLog(LogInfo newLog)
        {
            bool retValue = false;
            string url = ConfigurationHelper.GetByName("logURl") +"?empMachCode="
                            + newLog.IndRegID + "&attDate="
                            + String.Format("{0:yyyy-MM-dd HH:mm:ss}", newLog.DateTimeRecord) + "&machId=" + newLog.MachineNumber;

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(5);
                    HttpRequestMessage doucmentsubmited = new HttpRequestMessage(HttpMethod.Get, url);
                    HttpResponseMessage response = client.SendAsync(doucmentsubmited).Result;
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    if ((int)response.StatusCode == 200) retValue = true;
                }
            }
            catch (Exception) { retValue = false; }
            return retValue;
        }
    }
}
