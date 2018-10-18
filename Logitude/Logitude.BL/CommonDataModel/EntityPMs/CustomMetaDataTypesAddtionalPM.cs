using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
 public   class CustomMetaDataTypesAddtionalPM
    {

        [Key]
        public string Id { get; set; }
        [Key]
        public string Code { get; set; }

        public int Tenant { get; set; }

        public string DocumentsMetaDataTypesCode { get; set; }
    }
}
