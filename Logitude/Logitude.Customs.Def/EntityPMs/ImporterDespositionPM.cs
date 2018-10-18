using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class ImporterDespositionPM
    {
        [DataMember]
        public string VendorName { get; set; }

        [DataMember]
        public string ImporterName { get; set; }

        [DataMember]
        public string ImporterCode { get; set; }
    }
}
