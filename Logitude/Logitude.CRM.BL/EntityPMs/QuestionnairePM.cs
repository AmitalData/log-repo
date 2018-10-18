using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityPMs
{
    public partial class QuestionnairePM
    {
        [DataMember]
        public bool IsCopy { get; set; }

    }
}
