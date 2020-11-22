using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
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

        public List<BIReportFolderList> GetPermittedFoldersList(int tenant, string loggedUserId)
        {
            bool isCustomerCareUser = false;
            FoldersPermissionParams foldersPermissionParams = GetBIReportFolderPermissionParams(tenant,null);
            UserQuery userQuery = new UserQuery(tenant);
            UserPM user = userQuery.GetSinglePM(loggedUserId, tenant);
            if (user != null && user.IsCustomerCare) isCustomerCareUser = true;
            List<BIReportFolderList> result = repository.GetFoldersByPermittedUser(foldersPermissionParams, isCustomerCareUser);
            return result;
        }

        public bool CheckIfUserHasFolderPermission(int tenant, string folderId)
        {
            FoldersPermissionParams foldersPermissionParams = GetBIReportFolderPermissionParams(tenant, folderId);
            bool result = repository.CheckIfUserHasFolderPermission(foldersPermissionParams);
            return result;
        }

        private FoldersPermissionParams GetBIReportFolderPermissionParams(int tenant, string folderId)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            string loggedCotactId = contactQuery.GetContactIdByLoggedEmail(tenant);

            BIFoldersPermissionQueryService biFoldersPermissionQueryService = new BIFoldersPermissionQueryService(tenant);
            List<string> permittedFolders = biFoldersPermissionQueryService.GetFoldersIdsByPermittedUserId(loggedCotactId);

            FoldersPermissionParams foldersPermissionParams = new FoldersPermissionParams
            {
                LoggedUserId = loggedCotactId,
                PermittedFolders = permittedFolders,
                Tenant = tenant,
                FolderId = folderId
            };
            return foldersPermissionParams;
        }
    }
}
