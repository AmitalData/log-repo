using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
  public  class DocumentsDataProvider
    {

        [Key]
        public string Code { get; set; }
        public string Name { get; set; }




    }
}
