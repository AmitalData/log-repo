using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.DataContracts
{
    public class PostFilters
    {
        public string EntityId { get; set; }
        public string UserId { get; set; }
        public string ObjectTableId { get; set; }
        public string QueryName { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SubQueryName { get; set; }
        public bool SearchByEntity { get; set; }
        public string Technology { get; set; }
    }
}
