using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
   public class SpecialServicesType
    {
      [Key]
      public string  Id {get; set;}
      public string  Code {get; set;}
      public string EnglishName {get; set;}
      public string LocalName {get; set;}
      public int Tenant { get; set; }
      public string SearchFields { get; set; }
      public bool InActive { get; set; }
    }
}
