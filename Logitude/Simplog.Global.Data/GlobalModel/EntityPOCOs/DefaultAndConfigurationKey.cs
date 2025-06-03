using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class DefaultAndConfigurationKey
    {
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string SetType1 { get; set; }
        [Key]
        public string SetKey { get; set; }
        public string ShortDescription { get; set; }
        public string FullDesctiption { get; set; }
        public string SetType2 { get; set; }
    }
}
