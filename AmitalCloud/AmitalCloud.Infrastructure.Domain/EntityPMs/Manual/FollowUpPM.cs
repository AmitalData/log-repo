using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{

    [CustomValidation(typeof(IClassLevelValidator), "ValidateClass")]
    public partial class FollowUpPM
    {
        //[Key]
        //public string Id { get; set; }
        //public string ShipmentId { get; set; }
        //public string JobId { get; set; }
        //public int Tenant { get; set; }

        //public string ExternalDocumentId { get; set; }
        //private DateTime? date;
        //public DateTime? Date
        //{
        //    get
        //    {
        //        return date;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            date = value.Value.Date;
        //        }
        //    }
        //}
        //public bool IsNew { get; set; }
        //public string Notes { get; set; }
        //public string DoneNote { get; set; }
        //public DateTime? DoneDateTime { get; set; }
        //public bool Done { get; set; }

        //public string InternalDocumentId { get; set; }
        //public string LegType { get; set; }
        //public bool Deleted { get; set; }
        //public string EntityDateId { get; set; }

        //[CustomValidation(typeof(IValidationClass), "ValidateClass")]
        //public string EventTypeId { get; set; }

        //public string EventTypeFollowUpName { get; set; }
        //public bool ManualActivatedFollowUp { get; set; }
        //public string OwnerUserId { get; set; }
        //public string OwnerUserName { get; set; }
        //public string Area { get; set; }
        //public string DocumentTypeId { get; set; }
        //public string AutomationId { get; set; }

    }
}
