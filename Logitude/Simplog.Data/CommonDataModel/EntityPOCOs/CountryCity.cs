using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CountryCity
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Notes { get; set; }
        public bool InActive { get; set; }
        public bool AddedManually { get; set; }
        public string SearchFields { get; set; }

        public string CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public string StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }
    }
}
