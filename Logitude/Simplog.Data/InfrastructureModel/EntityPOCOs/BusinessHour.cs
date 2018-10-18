using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class BusinessHour
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool Is247 { get; set; }

        public string Code { get; set; }

        public string CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        public string UpdatedByUserId { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public bool IsMondayEnabeled { get; set; }
        public bool IsTuesdayEnabeled { get; set; }
        public bool IsWednesdayEnabeled { get; set; }
        public bool IsThursdayEnabeled { get; set; }
        public bool IsFridayEnabeled { get; set; }
        public bool IsSaturdayEnabeled { get; set; }
        public bool IsSundayEnabeled { get; set; }

        public TimeSpan MondayFromHour { get; set; }
        public TimeSpan TuesdayFromHour { get; set; }
        public TimeSpan WednesdayFromHour { get; set; }
        public TimeSpan ThursdayFromHour { get; set; }
        public TimeSpan FridayFromHour { get; set; }
        public TimeSpan SaturdayFromHour { get; set; }
        public TimeSpan SundayFromHour { get; set; }

        public TimeSpan MondayToHour { get; set; }
        public TimeSpan TuesdayToHour { get; set; }
        public TimeSpan WednesdayToHour { get; set; }
        public TimeSpan ThursdayToHour { get; set; }
        public TimeSpan FridayToHour { get; set; }
        public TimeSpan SaturdayToHour { get; set; }
        public TimeSpan SundayToHour { get; set; }

        public string SearchFields { get; set; }

    }
}
