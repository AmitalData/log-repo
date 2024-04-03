using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class QuoteChargesGroup
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string LocalName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public int ViewOrder { get; set; }
    }
}