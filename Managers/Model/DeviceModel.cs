using AttendanceService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceService.Managers.Model
{
    internal class DeviceModel
    {
        public DeviceModel(int id) { 
            this.Id = id;
        }
        public int Id { get; }
        public DateTime LastLog { get; set; }
        public Queue<LogInfo> Logs { get; set; }
        public bool IsReady { get; set; } 
    }
}
