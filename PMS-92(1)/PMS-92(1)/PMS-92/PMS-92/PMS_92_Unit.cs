using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PMS_92
{
    class PMS_92_Unit : IUnit
    {
        public List<ushort[]> request_collection { get; set; }
        public byte adres { get; set; }

        public PMS_92_Unit(List<ushort[]> req_coll, byte adrs)
        {
            request_collection = req_coll;
            adres = adrs;
        }

    }
}
