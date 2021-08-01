using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{

    public partial class GLAccountPM : EntityPM
    {
        public string Application { get; set; }

        public bool PassedFromAPI { get; set; }

        public string DeductionTypeCode { get; set; }

        public string AssessingOfficeNumber { get; set; }
        [DataMember]
        public string ExternalDisplayNumber { get; set; }


        public string Change2MultiCurrencyNotes { get; set; }
    }
}
