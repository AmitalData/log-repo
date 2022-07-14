using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.Data.EntityPOCOs
{
    public class IndexerWaterMark
    {
        public string TableName { get; set; }
        public DateTime? LastUpdateDate { get; set; }

    }
}
