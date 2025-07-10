using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Controllers.ShipmentsModel.ApiHelpers
{
    public class ShipmentAPiHelper
    {
        public static ShipmentList ApplyFilters(ShipmentList entityList, int tenant)
        {
            return entityList;
        }

        public static IQueryable<Shipment> ApplyFilters(IQueryable<Shipment> iQueryable_Data, int tenant)
        {
            return iQueryable_Data;
        }

        public static void AddFilters(QueryOperations queryOperations, int tenant)
        {
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);
        }
    }
}