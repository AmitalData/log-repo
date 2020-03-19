using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.DataContracts
{
    public class ServiceResponse
    {
        public object Result { get; set; }
        public int Count { get; set; }
        public long TookMS { get; set; }
    }
}
