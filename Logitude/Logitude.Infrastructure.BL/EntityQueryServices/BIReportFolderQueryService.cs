using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class BIReportFolderQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, BIReportFolderPM entityPM)
        {
            IInfrastructureContext context = MainContext as InfrastructureContext;
            BIReportFolderKeys biReportFolderKeys = entityKeys as BIReportFolderKeys;
            BIFoldersPermissionQueryService queryService = new BIFoldersPermissionQueryService(context);
            entityPM.PermittedBIFolders = queryService.GetMulti(biReportFolderKeys, true);
        }
    
        public List<BIReportFolderList> GetFoldersByPermittedUser(string loggedUserId, List<string> permittedFolders, int tenant)
        {
            return repository.GetFoldersByPermittedUser(loggedUserId, permittedFolders, tenant);
        }
    }
}
