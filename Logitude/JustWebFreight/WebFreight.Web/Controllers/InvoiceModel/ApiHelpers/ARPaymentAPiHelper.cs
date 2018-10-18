using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Controllers.InvoiceModel.ApiHelpers
{
    public class ARPaymentAPiHelper
    {
        public static ARPaymentList ApplyFilters(ARPaymentList entityList, int tenant)
        {
            return entityList;
        }

        public static IQueryable<ARPayment> ApplyFilters(IQueryable<ARPayment> entityPocos, int tenant)
        {
            return entityPocos;
        }

        public static void AddFilters(QueryOperations queryOperations, int tenant)
        {
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
        }
    }
}