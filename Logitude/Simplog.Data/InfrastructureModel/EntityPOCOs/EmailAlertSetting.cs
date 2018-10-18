using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
   public class EmailAlertSetting
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }

        public string ObjectTableId { get; set; }
        public bool InActive { get; set; }
        public string SettingLevelCode  { get; set; }
        public string To { get; set; }
        public int IndexOrder { get; set; }

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
    }
}
