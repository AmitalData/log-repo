using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ExternalLinkPM
    {
        [Key]
        public string Id { get; set; }
        public string Ref { get; set; }
        public string Link { get; set; }
    }
}
