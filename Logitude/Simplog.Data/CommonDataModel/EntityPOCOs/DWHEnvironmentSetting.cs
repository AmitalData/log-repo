using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class DWHEnvironmentSetting
    {
        [Key]
        public int Id { get; set; }
        public string FactCodes { get; set; }
    }
}
