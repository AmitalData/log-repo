using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Horse
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public int? YearOfBirth { get; set; }
        public string Color { get; set; }
        public string Breed { get; set; }
        public string Discipline { get; set; }
        public string TravelBehavior { get; set; }
        public string MicochipNumber { get; set; }
        public string PassportNumber { get; set; }
        public string CountryOfBirthId { get; set; }
        public string CurrentStable { get; set; }
        public string Owner { get; set; }
        public string Remarks { get; set; }
        public bool Inactive { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string SearchFields { get; set; }
        public string GenderCode { get; set; }

        public virtual Country CountryOfBirth { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }
        public virtual HorseGender HorseGender { get; set; }
    }
}
