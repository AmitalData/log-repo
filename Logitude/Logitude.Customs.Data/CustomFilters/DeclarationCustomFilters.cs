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

       public IQueryable<Declaration> GetFilteredQuery(QueryOperations operations, IQueryable<Declaration> queryableData, int tenant)
       {
           List<QueryFilterItem> queryFilters = operations.QueryFilterItems;



           foreach (QueryFilterItem item in queryFilters)
           {
               if (item.FieldName == "DeclarationWithoutRelease")
               {
                    //queryableData = queryableData.Where(d => (d.HatraDate == null));
                    queryableData = queryableData.Where(d => (d.IsClose == false));

                }

               if (item.FieldName == "PaidDeclarationWithoutRelease")
               {
                   queryableData = queryableData.Where(d => (d.PaymentDate != null) && (d.HatraDate == null));
               }

                if (item.FieldName == "CourierPendingReasonList")
                {
                    //&& (x.CourierPendingReasonList== item.FieldValue || x.CourierPendingReasonList.Contains("," + item.FieldValue+","))
                    List<string> declarations = queryableData.Select(x => (string)x.Id ).ToList();
                    var repoDeclarationCourierStatus = new DeclarationCourierStatusRepository(tenant);
                    var pocoDeclarationCourierStatus = repoDeclarationCourierStatus
                        .GetDeclarationsByPendings(declarations, tenant, item.FieldValue.ToString());
                        //.Where(x=> x.CourierPendingReasonList != null ).ToList();

                 //   pocoDeclarationCourierStatus = pocoDeclarationCourierStatus.Where(x=> x.CourierPendingReasonList.Split(',').Contains(item.FieldValue)).ToList();
                    var decs = pocoDeclarationCourierStatus.Select(x => (string) x.DeclarationId).ToList();
                    queryableData = queryableData.Where(d => decs.Contains( d.Id));
                }
                //     queryableData = queryableData.Where(d => (d.AmendmentDontDisplayInList == false));

            }

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

    }
}
