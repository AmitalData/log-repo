using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class VatUniquePartnerType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int ViewOrder { get; set; }
        public string SearchFields { get; set; }
    }
}
