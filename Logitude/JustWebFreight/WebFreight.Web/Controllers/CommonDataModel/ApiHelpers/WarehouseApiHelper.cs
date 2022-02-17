using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;

namespace WebFreight.Web.Controllers.CommonDataModel.ApiHelpers
{
    public class WarehouseAPiHelper
    {
        public static WarehouseList ApplyFilters(WarehouseList entityList, int tenant)
        {
            return entityList;
        }

        public static IQueryable<Warehouse> ApplyFilters(IQueryable<Warehouse> entityPocos, int tenant)
        {
            return entityPocos;
        }

        public static void AddFilters(QueryOperations queryOperations, int tenant)
        {
            //BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
        }
    }
}