using AttendanceService.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using zkemkeeper;

namespace AttendanceService.HAL.Driver
{
    public class ZkManagedDevice : IManagedAttendencDevice
    {
        public ZkManagedDevice() {
            this.State = Status.Disconnected;
        }
        public Status State { get; private set; }
        public string IpAddress { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int Id { get; set; }
        public Action<IDevice, List<LogInfo>> action;
        public DateTime LastLog { get; set; }

        CZKEM objCZKEM = new CZKEM();
        public event Notify LogsArrived;
        public bool Connect()
        {
            objCZKEM.SetCommPassword(int.Parse(this.Password));
            if (!objCZKEM.Connect_Net(this.IpAddress, this.Port)) return false;
            this.State = Status.Connected;
            GetLogs();
            return true;
        }
        private void register()
        {
            if (objCZKEM.RegEvent(this.Id, 32767))
            {
 
                objCZKEM.OnAttTransactionEx += new _IZKEMEvents_OnAttTransactionExEventHandler(OnAttTransactionEx);

            }
            
        }



        private void OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
        {
            LogInfo info = new LogInfo() { MachineNumber = this.Id, IndRegID = int.Parse(EnrollNumber), DateTimeRecord = new DateTime(Year, Month, Day, Hour, Minute, Second) };
            this.LogsArrived?.Invoke((IDevice)this, new List<LogInfo> { info });

        }
        public void Disconnect()
        {
            this.State = Status.Disconnected;
            objCZKEM.Disconnect();
            objCZKEM.OnAttTransactionEx -= new _IZKEMEvents_OnAttTransactionExEventHandler(OnAttTransactionEx);

        }
        public void GetLogs()
        {
            Task.Run(() => this.GetAllLogsTask());
        }
        private List<LogInfo> MapClientLogs()
        {
            List<LogInfo> lstEnrollData = new List<LogInfo>();
            int dwWorkCode = 0;
                objCZKEM.ReadAllGLogData(this.Id);
                while (objCZKEM.SSR_GetGeneralLogData(this.Id, out string dwEnrollNumber1, out int dwVerifyMode, out int dwInOutMode, out int dwYear, out int dwMonth, out int dwDay, out int dwHour, out int dwMinute, out int dwSecond, ref dwWorkCode))
                {
                    string inputDate = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond).ToString();
                    LogInfo objInfo = new LogInfo() { DateTimeRecord = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond), IndRegID = int.Parse(dwEnrollNumber1), MachineNumber = this.Id };
                    lstEnrollData.Add(objInfo);
                }
            
            return lstEnrollData;
        }

        private void GetAllLogsTask()
        {
            if (!(this.State == Status.Connected)) return;
            this.State = Status.Pending;
            try { LogsArrived?.Invoke((IDevice)this, this.MapClientLogs()); } catch (Exception) { this.Disconnect();return; }
            this.State = Status.Connected;
            this.register();

        }


    }
}
