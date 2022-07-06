using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_92
{
    class CONNECTION_JS
    {
        public string TCP_JS { get; set; }
        public string IP_JS { get; set; }

        public CONNECTION_JS(string ip_js, string tcp_js)
        {
            IP_JS = ip_js;
            TCP_JS = tcp_js;
        }
    }
}
