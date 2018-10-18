using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class Region
    {
       [Key]
       public string Id { get; set; }
       public int Tenant {get; set;}
       public string Name { get; set; }
       public string LocalName { get; set; }
       public string SearchFields { get; set; }
       public bool InActive { get; set; }
    }
}
