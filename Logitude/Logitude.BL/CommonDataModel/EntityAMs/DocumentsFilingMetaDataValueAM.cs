using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityAMs
{
    public class DocumentsFilingMetaDataValueAM
    {
        public CodeProperties DocumentsMetaDataType { get; set; }
        public string MetaDataValue { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
