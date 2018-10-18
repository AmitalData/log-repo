using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Logitude.Customs.Def.EntityPMs
{
    [DataContract]
    public class OpenAccessDeclarationPM
    {
        [DataMember]
        public DeclarationPM declarationPM { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }
    }
}
