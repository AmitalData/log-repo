using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{

    public class DWQuery
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        //public string DWObjectTableCode { get; set; }
        public string SQLString { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }



        [ForeignKey("CreatedByUserId ")]
        public User CreatedBy { get; set; }

        [ForeignKey("UpdateByUserId ")]
        public User UpdateBy { get; set; }


        //[ForeignKey("DWObjectTableCode")]
        //public virtual DWObjectTable DWObjectTable { get; set; }


    }
}
