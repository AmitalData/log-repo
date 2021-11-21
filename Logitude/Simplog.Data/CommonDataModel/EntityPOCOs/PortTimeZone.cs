using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class PortTimeZone
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public bool Inactive { get; set; }
        public string Notes { get; set; }
        public string UTCOffset { get; set; }
        public string UTCDSTOffset { get; set; }
    }
}
