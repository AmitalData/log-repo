
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class AutomationHistoryPM
    {
        [Key]
        public int Version { get; set; }

        [Key]
        public string AutomationsId { get; set; }

        public int Tenant { get; set; }

        [DataMember]
        public string AutomationXML { get; set; }

        [DataMember]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        public AutomatedBackup AutomatedDataBackup { get; set; }

    }
}





