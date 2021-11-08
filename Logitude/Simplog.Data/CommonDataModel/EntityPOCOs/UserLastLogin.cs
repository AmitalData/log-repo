using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class UserLastLogin
    {
        [Key]
        public string Id { get; set; }
        [Key]
        public string ComputerId { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public int Tenant { get; set; }

        [Key]
        public string WorkEnvironment { get; set; }


        public virtual User User { get; set; }

    }
}