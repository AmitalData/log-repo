using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class BusinessHoursHolidayList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public string HolidayName { get; set; }

        public bool IsRecurring { get; set; }

        public bool Inactive { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string CreatedByUserId { get; set; }

        public string UpdatedByUserId { get; set; }

        public string BusinessHourId { get; set; }
    }
}
