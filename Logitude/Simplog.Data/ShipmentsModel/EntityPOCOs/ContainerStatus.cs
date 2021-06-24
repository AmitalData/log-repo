using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ContainerStatus
    {
        [Key]
        public string  Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}
