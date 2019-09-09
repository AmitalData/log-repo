using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class VatType
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }  
        public string Code { get; set; }       
        public string EnglishName { get; set; }  
        public string LocalName { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string Description { get; set; }
        public string LocalDescription { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public bool IsMultiPercentage { get; set; }
        public double? RecognizedPercentage { get; set; }
    }
}