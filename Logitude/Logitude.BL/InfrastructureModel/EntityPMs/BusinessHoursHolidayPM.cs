using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public  class BusinessHoursHolidayPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }
        [DataMember]
        public int Day { get; set; }
        [DataMember]
        public int Month { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]   
        public string HolidayName { get; set; }
        [DataMember]
        public bool IsRecurring { get; set; }
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

        public string BusinessHourId { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }

        [Include]
        [Association("BusinessHoursHolidayBusinessHour", "BusinessHourId", "Id", IsForeignKey = true)]
        public virtual BusinessHourPM BusinessHour { get; set; }

    }
}
