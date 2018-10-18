using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
  public  class SharedManifestsStatus
    {
        
        [Key]
         public string StatusCode { get; set; }
         public string StatusName	 { get; set; }
         public string SearchFields { get; set; }
    }
}
