using AttendanceService.HAL;
using AttendanceService.HAL.Driver;
using AttendanceService.Util;
using BioMetrixCore.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceService.Providers
{
    internal class DefaultDeviceProvider : IDevicesProvider
    {
        public DefaultDeviceProvider()
        {
           
        }
        public List<IManagedAttendencDevice> GetDevices()
        {
            List<IManagedAttendencDevice> devices = new List<IManagedAttendencDevice>();
            string url = ConfigurationHelper.GetByName("devicesURl") + "?companyId=" + ConfigurationHelper.GetByName("companyId") + "&ids=" + String.Join(",", ConfigurationHelper.GetByNameList("devices"));
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(5);
                    HttpRequestMessage doucmentsubmited = new HttpRequestMessage(HttpMethod.Get, url);
                    HttpResponseMessage response = client.SendAsync(doucmentsubmited).Result;
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    IEnumerable<MachineInfoDto> dto = JsonConvert.DeserializeObject<IEnumerable<MachineInfoDto>>(jsonString);
                    dto.ToList().ForEach(x => devices.Add(new ZkManagedDevice() { IpAddress = x.ip, Id = x.id, Password = x.commKey.ToString(), LastLog = x.lastLog, Port = x.port }));
                }
            }
            catch (Exception) { }
            return devices;

        }

        public void Refresh(IManagedAttendencDevice device)
        {
            return;
        }
    }
}
