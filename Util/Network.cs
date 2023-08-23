using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace AttendanceService.Util
{
    internal class Network
    {
        public static bool ValidateIP(string addrString)
        {
            if (IPAddress.TryParse(addrString, out IPAddress address))
                return true;
            else
                return false;
        }

        public static bool PingTheDevice(string ipAdd)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipAdd);
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions() { DontFragment = true };
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                    return true;
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
