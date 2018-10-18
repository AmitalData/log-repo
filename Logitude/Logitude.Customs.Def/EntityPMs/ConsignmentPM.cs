using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class ConsignmentPM
    {
        [DataMember]
        public int ConsignmentPackagLastLineNumber { get; set; }
        [DataMember]
        public int ConsignmentInternalTransitionLastLineNumber { get; set; }
        [DataMember]
        public string ConsignmentPackagesActiveIds { get; set; } //Yuval Chalup 29.03.2015
    }
}
