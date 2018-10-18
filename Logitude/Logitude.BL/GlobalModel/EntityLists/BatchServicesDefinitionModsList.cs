using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityLists
{
  public  class BatchServicesDefinitionModsList
    {

        [Key]
        public string Code { get; set; }
        public bool InActive { get; set; }
        public int? NumberOfThreads { get; set; } 
        //public string Parameter1 { get; set; }
        //public string Parameter2 { get; set; }
    }
}
