using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class GeneralLockPM
    {
        [Key]
        [Column("GeneralKey", Order = 1)]
        public string GeneralKey { get; set; }
        [Key]
        [Column("Tenant", Order = 2)]
        public int Tenant { get; set; }

        public DateTime  CreatedAt { get; set; }
		public string EntityId1 { get; set; }
        public string ObjectTableId1 { get; set; }
		public string EntityId2 { get; set; }
		public string ObjectTableId2 { get; set; }
		public string UserId { get; set; }


		[ForeignKey("ObjectTableId1")]
		public virtual ObjectTable ObjectTable1 { get; set; }
		[ForeignKey("ObjectTableId2")]
		public virtual ObjectTable ObjectTable2 { get; set; }

		[ForeignKey("UserId")]
		public virtual User UsedByUser { get; set; }
		public string SessionId { get; set; }
		public string UserName { get; set; }

	}
}
