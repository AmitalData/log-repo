using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CarrierServiceLine
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PartnerTypeId { get; set; }
        public bool Inactive { get; set; }
        public string SearchFields { get; set; }
        public virtual Card Card { get; set; }
        public virtual PartnerType PartnerType { get; set; }
    }
}
