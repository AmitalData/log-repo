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
 public  class AutomationPM
    {
       [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ObjectTableId { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string ResultCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Inactive { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        public DateTime? UpdateDate { get; set; }

        [DataMember]
        public string CreatedByUserId { get; set; }

        [DataMember]
        public string UpdatedByUserId { get; set; }

        [DataMember]
        public string DocumentTypeId { get; set; }

        [DataMember]
        public string TemplateId { get; set; }

        [DataMember]
        public string CreatedByUserName { get; set; }

        [DataMember]
        public string UpdatedByUserName { get; set; }

        [DataMember]
        public string From { get; set; }

        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string AutomationXML { get; set; }

        [DataMember]
        public int Version { get; set; }

       [DataMember]
        public int Order { get; set; }

       [DataMember]
       public AutomatedBackup AutomatedDataBackup { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public List<AutomationResultEmailRecipientPM> AutomationResultEmailRecipientLists { get; set; }
        

       [DataMember]
       public bool IsChangeAutomationXaml { get; set; }
    }
}
