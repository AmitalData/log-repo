using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class ObjectTableLastUpdate
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string ObjectTableId { get; set; }

        public virtual User UpdatedByUser { get; set; }
        public virtual ObjectTable ObjectTable { get; set; }
    }
}
