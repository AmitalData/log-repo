using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class GeneralLockList
    {
		[Key]
		public string GeneralKey { get; set; }
		[Key]
		public int Tenant  { get; set; }

		public DateTime CreatedAt { get; set; }
		public string EntityId1 { get; set; }
		public string ObjectTableId1 { get; set; }
		public string EntityId2 { get; set; }
		public string ObjectTableId2 { get; set; }
		public string UserId { get; set; }
		public string SessionId { get; set; }
		public string UserName { get; set; }
		public string ObjectTableName1 { get; set; }
		public string ObjectTableName2 { get; set; }



	}
}
