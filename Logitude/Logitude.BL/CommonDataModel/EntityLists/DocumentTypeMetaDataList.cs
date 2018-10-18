using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
   public class DocumentTypeMetaDataList
    {
        [Key]
        public string Id { get; set; }

        public string DocumentTypeId { get; set; }

        public string DocumentsMetaDataTypeId { get; set; }

        public int Tenant { get; set; }

        public string DocumentsMetaDataTypeCode { get; set; }

        public string DocumentsMetaDataTypeEnglishName { get; set; }

        public string DocumentsMetaDataTypeLocalName { get; set; }

        public string DocumentsMetaDataTypeFormat { get; set; }

        public bool Mandatory { get; set; }


    }
}
