using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    [DataContract]
    public class ApiSearchFilters
    {
        [DataMember]
        public int Skip { get; set; }
        [DataMember]
        public int Take { get; set; }

        [DataMember]
        public string SearchFields { get; set; }
    }
}
