using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class DeclarationCargoSplitPM
    {
        [DataMember]
        public int DecCargoSplitConLastLineNumber { get; set; }
        public int DecCargoSplitCargoIdentifierLastLineNumber { get; set; }

    }
}
