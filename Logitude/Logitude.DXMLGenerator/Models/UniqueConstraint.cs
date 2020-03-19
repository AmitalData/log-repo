using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DXMLGenerator.Models
{
    public class UniqueConstraint
    {
        public string TableName { get; set; }

        public string Columns { get; set; }
    }
}