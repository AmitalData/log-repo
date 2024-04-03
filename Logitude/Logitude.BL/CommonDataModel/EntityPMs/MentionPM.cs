using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class MentionPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SearchFields { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool InActive { get; set; }
    }
}
