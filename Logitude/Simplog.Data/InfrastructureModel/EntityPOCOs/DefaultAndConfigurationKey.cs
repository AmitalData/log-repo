using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class DefaultAndConfigurationKey
    {
        [Key]
        public int Tenant { get; set; }
        public DateTime CreateDateTime { get; set; }
        [Key]
        public string SetType { get; set; }
        [Key]
        public string SetKey { get; set; }
        public string ShortDescription { get; set; }
        public string FullDesctiption { get; set; }

    }
}
