using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Args
{
    class CustomReferenceDocumentsMetaDataType
    {
       public DocumentsMetaDataType DREL { get; set; }
        public DocumentsMetaDataType CREF { get; set; }
    }
}
