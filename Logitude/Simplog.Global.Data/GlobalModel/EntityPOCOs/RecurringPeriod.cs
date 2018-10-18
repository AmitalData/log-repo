using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class RecurringPeriod
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

        //public List<TenantManagement> TenantManagements { get; set; }
    }
}
