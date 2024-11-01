using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Services
{
    public class EntityCustomFieldServiceArgs
    {
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableName { get; set; }
        public string Type { get; set; }
        public List<object> Entities { get; set; }
        public string KeyName { get; set; }
    }
}
