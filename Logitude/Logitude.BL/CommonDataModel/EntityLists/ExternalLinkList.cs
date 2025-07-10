using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ExternalLinkList
    {
        [Key]
        public string Id { get; set; }
        public string Ref { get; set; }
        public string Link { get; set; }
        public int ExpirationDate { get; set; }
        public bool ActivityLog { get; set; }
        public string Params { get; set; }
    }
}