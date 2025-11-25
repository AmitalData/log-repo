using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityUpdateServicesExt
{
    public interface IExportDocumentHelper
    {
        string ExportDocument2Pdf(
          string documentTypeId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, string documentOutId, int tenant, string documentTypeCopyId, string userId = null, string documentFileName = null

           );
    }
}
