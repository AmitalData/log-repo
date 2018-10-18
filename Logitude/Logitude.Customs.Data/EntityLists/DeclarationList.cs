using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.EntityLists
{
    public partial class DeclarationList
    {
        [DataMember]
        public string DeclarationNumberandVersionId { get; set; }

        [DataMember]
        public string DeclarationVersionId { get; set; }
    }
}
