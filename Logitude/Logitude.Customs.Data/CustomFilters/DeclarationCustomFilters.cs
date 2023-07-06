using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.Utils;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.CustomFilters
{
    public class DeclarationCustomFilters
    {
        public IQueryable<Declaration> GetFilteredQuery(QueryOperations operations, IQueryable<Declaration> queryableData, int tenant, ICustomContext context)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.FieldName == "DeclarationWithoutRelease")
                {
                    //queryableData = queryableData.Where(d => (d.HatraDate == null));
                    queryableData = queryableData.Where(d => (d.IsClose == false));
                }

                if (item.FieldName == "ExportDecWithoutRelease")
                {
                    queryableData = queryableData.Where(d => (d.IsExportClosed == false && d.IsClose==false));

                }

                if (item.FieldName == "PaidDeclarationWithoutRelease")
                {
                    queryableData = queryableData.Where(d => (d.PaymentDate != null) && (d.HatraDate == null));
                }

                if (item.FieldName == "CourierPendingReasonList")
                {
                    queryableData = queryableData.Join(context.DeclarationCourierStatuses, x => x.Id, x => x.DeclarationId, (dec, sta) => new { dec = dec, sta = sta })
                        .Where(x => x.sta.Tenant == tenant && ("," + x.sta.CourierPendingReasonList + ",").Contains("," + item.FieldValue.ToString() + ","))
                        .Select(x => x.dec);
                }
            }

            return queryableData;
        }
        public IQueryable<DeclarationList> GetFilteredQueryList(QueryOperations operations, IQueryable<DeclarationList> queryableData, int tenant, ICustomContext context)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.FieldName == "DeclarationWithoutRelease")
                {
                    //queryableData = queryableData.Where(d => (d.HatraDate == null));
                    queryableData = queryableData.Where(d => (d.IsClose == false));
                }

                if (item.FieldName == "ExportDecWithoutRelease")
                {
                    queryableData = queryableData.Where(d => (d.IsExportClosed == false && d.IsClose == false));

                }

                if (item.FieldName == "PaidDeclarationWithoutRelease")
                {
                    queryableData = queryableData.Where(d => (d.PaymentDate != null) && (d.HatraDate == null));
                }

                if (item.FieldName == "CourierPendingReasonList")
                {
                    queryableData = queryableData.Join(context.DeclarationCourierStatuses, x => x.Id, x => x.DeclarationId, (dec, sta) => new { dec = dec, sta = sta })
                        .Where(x => x.sta.Tenant == tenant && ("," + x.sta.CourierPendingReasonList + ",").Contains("," + item.FieldValue.ToString() + ","))
                        .Select(x => x.dec);
                }
            }

            queryableData = queryableData.Where(x => x.Tenant == tenant);
            return queryableData;
        }

        public IQueryable<Declaration> GetFreelancerDeclarations(QueryOperations operations, IQueryable<Declaration> queryableData, int tenant)
        {
            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            if (frlUtil.user.IsFreelancer)
            {
                List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
                if (customersIds.Count > 0)
                {
                    queryableData = queryableData.Where(d => customersIds.Contains(d.CustomerId));
                }
            }

            return queryableData;
        }
        public IQueryable<DeclarationList> GetFreelancerDeclarationsList(QueryOperations operations, IQueryable<DeclarationList> queryableData, int tenant)
        {
            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            if (frlUtil.user.IsFreelancer)
            {
                List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
                if (customersIds.Count > 0)
                {
                    queryableData = queryableData.Where(d => customersIds.Contains(d.CustomerId));
                }
            }

            return queryableData;
        }

    }
}
