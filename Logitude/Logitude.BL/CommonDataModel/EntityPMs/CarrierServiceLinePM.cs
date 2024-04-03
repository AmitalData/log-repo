using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CarrierServiceLinePM
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
    }
}
