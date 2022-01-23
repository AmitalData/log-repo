using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    
    public class CustomerTeam
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public virtual User CreatedByUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public virtual User UpdatedByUser { get; set; }
        public string SearchFields { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public bool InActive { get; set; }
    }
}