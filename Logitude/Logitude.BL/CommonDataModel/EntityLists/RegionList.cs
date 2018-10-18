using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class RegionList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public string LocalName { get; set; }
        public bool InActive { get; set; }
    }
}