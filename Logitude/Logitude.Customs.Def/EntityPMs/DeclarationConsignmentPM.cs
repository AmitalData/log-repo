using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public class DeclarationConsignmentPM
    {
        [Key]
        [DataMember]
        public string DeclarationId { get; set; }
        [Key]
        [DataMember]
        public int? ConsignmentNumber { get; set; }
        [DataMember]
        public int? SequenceNumeric { get; set; }
        [DataMember]
        public string ManifestNumber { get; set; }
        
    }
}
