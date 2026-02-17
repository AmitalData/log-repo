using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class DWObjectTable
    {
        [Key]
        public string Code { get; set; }
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string TypeCode { get; set; }
        public bool IsClosed { get; set; }
        public string DefaultFilterBy { get; set; }
    }
}
