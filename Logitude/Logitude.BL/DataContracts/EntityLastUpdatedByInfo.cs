using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class EntityLastUpdatedByInfo
    {
        [Key]
        public string  UserName { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
