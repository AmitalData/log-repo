using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class CustomsVendorPM
    {
        [DataMember]
        public int LastLineNumber { get; set; }

        [DataMember]
        public DateTime DeadlineToFix { get; set; }

        [DataMember]
        public string NotesForCustomsAgent { get; set; }

        [DataMember]
        public int? SubstituteVendorID { get; set; }

        [DataMember]
        public string CustomsRequestsSheetId { get; set; }//task10151
        
    }
}
