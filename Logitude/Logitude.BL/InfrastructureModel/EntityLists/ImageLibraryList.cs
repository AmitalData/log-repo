using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class ImageLibraryList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ImageDetailId { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SecurityId { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public string URL { get; set; }
    }
}
