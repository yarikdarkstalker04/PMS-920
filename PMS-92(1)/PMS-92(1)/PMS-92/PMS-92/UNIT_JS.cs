using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_92
{
    class UNIT_JS
    {
        public string request_collection_js { get; set; }
        public string adres_js { get; set; }

        public UNIT_JS(string rqsjs, string adrsjs)
        {
            request_collection_js = rqsjs;
            adres_js = adrsjs;
        }
    }
}
