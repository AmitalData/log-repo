using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{

    public class DWHSetting
    {
        [Key]
        public int Tenant { get; set; }
        public int? ParentTenant { get; set; }
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Catalog { get; set; }
        
    }
}
