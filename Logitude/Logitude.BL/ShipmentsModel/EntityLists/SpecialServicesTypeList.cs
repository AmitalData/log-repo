using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class SpecialServicesTypeList
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public int Tenant { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
    }
}