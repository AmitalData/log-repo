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
    public class BusinessHourPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool Is247 { get; set; }
        [DataMember]
        public string CreatedByUserId { get; set; }
        [DataMember]
        public string UpdatedByUserId { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
        [DataMember]
        public DateTime UpdateDate { get; set; }
        [DataMember]
        public bool IsMondayEnabeled { get; set; }
        [DataMember]
        public bool IsTuesdayEnabeled { get; set; }
        [DataMember]
        public bool IsWednesdayEnabeled { get; set; }
        [DataMember]
        public bool IsThursdayEnabeled { get; set; }
        [DataMember]
        public bool IsFridayEnabeled { get; set; }
        [DataMember]
        public bool IsSaturdayEnabeled { get; set; }
        [DataMember]
        public bool IsSundayEnabeled { get; set; }
        [DataMember]
        public TimeSpan?  MondayFromHour { get; set; }
        [DataMember]
        public TimeSpan?  TuesdayFromHour { get; set; }
        [DataMember]
        public TimeSpan?  WednesdayFromHour { get; set; }
        [DataMember]
        public TimeSpan?  ThursdayFromHour { get; set; }
        [DataMember]
        public TimeSpan?  FridayFromHour { get; set; }
        [DataMember]
        public TimeSpan?  SaturdayFromHour { get; set; }
        [DataMember]
        public TimeSpan?  SundayFromHour { get; set; }
        [DataMember]
        public TimeSpan?  MondayToHour { get; set; }
        [DataMember]
        public TimeSpan?  TuesdayToHour { get; set; }
        [DataMember]
        public TimeSpan?  WednesdayToHour { get; set; }
        [DataMember]
        public TimeSpan?  ThursdayToHour { get; set; }
        [DataMember]
        public TimeSpan?  FridayToHour { get; set; }
        [DataMember]
        public TimeSpan?  SaturdayToHour { get; set; }
        [DataMember]
        public TimeSpan?  SundayToHour { get; set; }

        private List<BusinessHoursHolidayPM> businessHoursHolidays;
        [Include]
        [Composition]
        [Association("BusinessHoursHolidayPMBUsinessHourPM", "Id", "BusinessHourId")]
        public virtual List<BusinessHoursHolidayPM> BusinessHoursHolidays
        {
            get
            {
                if (businessHoursHolidays == null)
                {
                    businessHoursHolidays = new List<BusinessHoursHolidayPM>();
                }

                return this.businessHoursHolidays;
            }
            set
            {
                if (value != null)
                {
                    businessHoursHolidays = value;
                }
            }
        }

        [DataMember]
        public string SearchFields { get; set; }

        [DataMember]
        public DateTime?  MondayFromHourDate { get; set; }
        [DataMember]
        public DateTime?  TuesdayFromHourDate { get; set; }
        [DataMember]
        public DateTime?  WednesdayFromHourDate { get; set; }
        [DataMember]
        public DateTime?  ThursdayFromHourDate { get; set; }
        [DataMember]
        public DateTime?  FridayFromHourDate { get; set; }
        [DataMember]
        public DateTime?  SaturdayFromHourDate { get; set; }
        [DataMember]
        public DateTime?  SundayFromHourDate { get; set; }
        [DataMember]
        public DateTime?  MondayToHourDate { get; set; }
        [DataMember]
        public DateTime?  TuesdayToHourDate { get; set; }
        [DataMember]
        public DateTime?  WednesdayToHourDate { get; set; }
        [DataMember]
        public DateTime?  ThursdayToHourDate { get; set; }
        [DataMember]
        public DateTime?  FridayToHourDate { get; set; }
        [DataMember]
        public DateTime?  SaturdayToHourDate { get; set; }
        [DataMember]
        public DateTime?  SundayToHourDate { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
