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
    public class ContainerAPiHelper
    {
        public static ContainerList ApplyFilters(ContainerList entityList, int tenant)
        {
            return entityList;
        }

        public static IQueryable<Container> ApplyFilters(IQueryable<Container> entityPocos, int tenant)
        {
            return entityPocos;
        }

        public static void AddFilters(QueryOperations queryOperations, int tenant)
        {
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
        }
    }
}