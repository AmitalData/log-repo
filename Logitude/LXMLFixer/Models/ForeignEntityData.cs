using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXMLFixer.Models
{
    public class ForeignEntityData
    {
        public string ReferencedTable { get; set; }
        public string ReferencedTableSchema { get; set; }
        public string ReferencedColumn { get; set; }
    }
}
