using System;

namespace AttendanceService.Model
{
    public class LogInfo
    {
        public int MachineNumber { get; set; }
        public int IndRegID { get; set; }
        public DateTime DateTimeRecord { get; set; }
        public override bool Equals(object obj)
        {
            LogInfo other = obj as LogInfo;
            return this.MachineNumber == other.MachineNumber && 
                this.IndRegID == other.IndRegID && 
                this.DateTimeRecord == other.DateTimeRecord ;
        }

        public override int GetHashCode()
        {
            int hashCode = 1652531817;
            hashCode = hashCode * -1521134295 + MachineNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + IndRegID.GetHashCode();
            hashCode = hashCode * -1521134295 + DateTimeRecord.GetHashCode();
            return hashCode;
        }
    }
}
