using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;

namespace WebFreight.Web.Controllers.CommonDataModel.ApiHelpers
{
    public class ExternalLinkAPiHelper
    {
        public static ExternalLinkList ApplyFilters(ExternalLinkList entityList, int tenant)
        {
            return entityList;
        }

        public static IQueryable<ExternalLink> ApplyFilters(IQueryable<ExternalLink> entityPocos, int tenant)
        {
            return entityPocos;
        }

        public static void AddFilters(QueryOperations queryOperations, int tenant)
        {
            //BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
        }
    }
}