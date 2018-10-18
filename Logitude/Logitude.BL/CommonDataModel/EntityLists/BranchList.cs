using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class BranchList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Notes { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public string ExternalId { get; set; }
        public string Signature { get; set; }
        public string Code { get; set; }
        public string INTTRAId { get; set; }
        public string INTTRAAlias { get; set; }
        public string INTTRAContactId { get; set; }
    }
}