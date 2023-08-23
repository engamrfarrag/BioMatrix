using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BioMetrixCore.Model
{
    internal class MachineInfoDto: ISerializable
    {

        public int indx { get; set; }
        public string closeFlag { get; set; }
        public string commKey { get; set; }
        public int companyId { get; set; }
        public int id { get; set; }
        public string ip { get; set; }
        public DateTime lastLog { get; set; }
        public int modelId { get; set; }
        public int ordering { get; set; }
        public int port { get; set; }
        public bool status { get; set; }    

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
