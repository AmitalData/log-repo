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
    public class APPaymentAPiHelper
    {
        public static APPaymentList ApplyFilters(APPaymentList entityList, int tenant)
        {
            return entityList;
        }

        public static IQueryable<APPayment> ApplyFilters(IQueryable<APPayment> entityPocos, int tenant)
        {
            return entityPocos;
        }

        public static void AddFilters(QueryOperations queryOperations, int tenant)
        {
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
        }
    }
}