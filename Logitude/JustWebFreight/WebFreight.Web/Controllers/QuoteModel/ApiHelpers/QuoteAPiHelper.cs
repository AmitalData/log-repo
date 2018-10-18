using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityLists;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel;

namespace WebFreight.Web.Controllers.QuoteModel.ApiHelpers
{
    public class QuoteAPiHelper
    {
        public static QuoteList ApplyFilters(QuoteList entityList, int tenant)
        {
            if (entityList != null)
            {
                QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
                entityList = filter.RunFilter(entityList);
            }

            return entityList;
        }

        public static IQueryable<Quote> ApplyFilters(IQueryable<Quote> iQueryable_Data, int tenant)
        {
            QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
            iQueryable_Data = filter.RunFilter(iQueryable_Data);

            return iQueryable_Data;
        }

        public static IQueryable<QuoteList> ApplyFilters(IQueryable<QuoteList> iQueryable_Data, int tenant)
        {
            QuoteBusinessUnitFilter filter = new QuoteBusinessUnitFilter(tenant);
            iQueryable_Data = filter.RunFilter(iQueryable_Data);

            return iQueryable_Data;
        }

        public static void AddFilters(QueryOperations queryOperations, int tenant)
        {
            BranchPermitionsFilter.AddUserBranchRestrictionFilters(queryOperations, tenant);
            ProductPermitionsFilter.AddUserProductRestrictionFilters(queryOperations, tenant);
        }
    }
}