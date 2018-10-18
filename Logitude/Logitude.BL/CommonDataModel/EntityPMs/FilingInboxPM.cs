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
    public class FilingInboxPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Sender { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Subject { get; set; }
        public bool IsDeleted { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? CreateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public DateTime? UpdateDate { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string UpdatedByUserId { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BodyDocumentId { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EmailBody { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SenderName { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string SearchFields { get; set; }

        private List<FilingInboxAttachmentPM> filingInboxAttachments;
        [DataMember]
        public virtual List<FilingInboxAttachmentPM> FilingInboxAttachments
        {
            get
            {
                if (filingInboxAttachments == null)
                {
                    filingInboxAttachments = new List<FilingInboxAttachmentPM>();
                }
                return filingInboxAttachments;
            }
            set { filingInboxAttachments = value; }
        }

    }
}
