using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.Def.EntityPMs
{
    //[CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class QuoteFollowUpPM
    {
        [Key]
        public string Id { get; set; }
        public string ShipmentId { get; set; }
        public string JobId { get; set; }
        public int Tenant { get; set; }

        public string ExternalDocumentId { get; set; }
        private DateTime? date;
        public DateTime? Date
        {
            get
            {
                return date;
            }
            set
            {
                if (value != null)
                {
                    date = value.Value.Date;
                }
            }
        }
        public bool IsNew { get; set; }
        public string Note { get; set; }
        public string DoneNote { get; set; }
        public DateTime? DoneDateTime { get; set; }
        public bool Done { get; set; }

        public string InternalDocumentId { get; set; }
        public string LegType { get; set; }
        public bool Deleted { get; set; }
        public string EntityDateId { get; set; }

        public string QuoteId { get; set; }
        //[CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string EventTypeId { get; set; }

        public string EventTypeFollowUpName { get; set; }
        public bool ManualActivatedFollowUp { get; set; }
        public string OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string Area { get; set; }
        public string DocumentTypeId { get; set; }
        public string AutomationId { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
