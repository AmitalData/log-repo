using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ProductTypeList
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string QuotationDefaultTemplateId { get; set; }
        public string DefaultTemplate { get; set; }
    }
}