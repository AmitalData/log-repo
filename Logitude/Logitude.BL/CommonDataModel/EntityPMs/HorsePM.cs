using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class HorsePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public int? YearOfBirth { get; set; }
        public string Color { get; set; }
        public string GenderName { get; set; }
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
    }
}
