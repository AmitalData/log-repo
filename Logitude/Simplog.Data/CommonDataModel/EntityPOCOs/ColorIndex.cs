using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
  public  class ColorIndex
    {

        [Key]
        public int IndexNumber { get; set; }

        public string Color { get; set; }
    }
}
