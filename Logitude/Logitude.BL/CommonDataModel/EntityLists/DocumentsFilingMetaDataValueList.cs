using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
   public class DocumentsFilingMetaDataValueList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentsMetaDataTypeId { get; set; }
        public string MetaDataValue { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
