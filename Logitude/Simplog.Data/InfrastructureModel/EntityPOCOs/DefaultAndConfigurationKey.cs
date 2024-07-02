using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class DefaultAndConfigurationKey
    {
        [Key, Column(Order = 0)]
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }        
        public string SetType1 { get; set; }
        [Key, Column(Order = 1)]
        public string SetKey { get; set; }
        public string ShortDescription { get; set; }
        public string FullDesctiption { get; set; }
        public string SetType2 { get; set; }
    }
}
