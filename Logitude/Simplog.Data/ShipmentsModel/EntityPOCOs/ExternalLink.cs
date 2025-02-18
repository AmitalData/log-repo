using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ExternalLink
    {
        [Key]
        public string Id { get; set; }
        public string Ref { get; set; }
        public string Link { get; set; }
    }
}
