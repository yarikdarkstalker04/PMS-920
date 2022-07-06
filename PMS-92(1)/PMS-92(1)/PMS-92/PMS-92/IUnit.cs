using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_92
{
    interface IUnit
    {
        byte adres { get; set; }
        List<ushort[]> request_collection { get; set; }
    }
}
