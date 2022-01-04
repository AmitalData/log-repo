using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;


namespace WebFreight.Web.Controllers.CommonDataModel.ApiHelpers
{
    public class TruckerAPiHelper
    {
        public static TruckerList ApplyFilters(TruckerList entityList, int tenant)
        {
            return entityList;
        }

        public static IQueryable<Trucker> ApplyFilters(IQueryable<Trucker> entityPocos, int tenant)
        {
            return entityPocos;
        }

        public static void AddFilters(QueryOperations queryOperations, int tenant)
        {
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
        }
    }
}