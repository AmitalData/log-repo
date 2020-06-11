using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class BIFoldersPermissionQueryService : EntityQueryService<BIFoldersPermission, BIFoldersPermissionKeys, BIFoldersPermissionPM, BIReportFolderPM, BIReportFolderKeys>
    {
        public List<string> GetFoldersIdsByPermittedUserId(string permittedUserId)
        {
            return repository.GetFoldersIdsByPermittedUserId(permittedUserId);
        }
    }
}
