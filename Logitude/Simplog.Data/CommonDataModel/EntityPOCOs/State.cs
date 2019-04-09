using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{    
    public class State
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
        public string QBOTransactionLocationCode { get; set; }
        public string CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
