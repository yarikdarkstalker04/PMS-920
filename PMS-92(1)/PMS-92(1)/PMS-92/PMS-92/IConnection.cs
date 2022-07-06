using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PMS_92
{
    interface IConnection
    {
        IPAddress IP { get; set; }
        int TCP { get; set; }

    }
}
