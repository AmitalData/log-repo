using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class ObjectTableLastUpdatePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }

        
    }
}
