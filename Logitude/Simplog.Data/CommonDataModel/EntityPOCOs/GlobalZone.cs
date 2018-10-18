using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class GlobalZone
    {
        [Key]
        public string Id { get; set; }

        //[Display(Name = "Code")]
        //[Required]
        //[StringLength(2, MinimumLength = 2, ErrorMessage = "The length of the code must be 2!")]
        public string Code { get; set; }
        
        //[Required]
        public int Tenant { get; set; }

        //[Required]
        //[StringLength(40, ErrorMessage = "The maximum length of the english name is 40!")]
        //[Display(Name = "Name")]
        public string EnglishName { get; set; }

        //[Required]
        //[StringLength(40, ErrorMessage = "The maximum length of the local name is 40!")]
        //[Display(Name = "Local Name")]
        public string LocalName { get; set; }

        //[StringLength(250, ErrorMessage = "The maximum length of the remarks is 250!")]
        public string Notes { get; set; }

        //[Required]
        //[Display(Name = "Inactive")]
        public bool InActive { get; set; }
        public string SearchFields { get; set; }

        //public virtual List<Country> Countries { get; set; }

    }
}