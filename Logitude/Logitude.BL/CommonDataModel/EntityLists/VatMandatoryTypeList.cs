using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class VatMandatoryTypeList
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int ViewOrder { get; set; }
        public string SearchFields { get; set; }
    }
}