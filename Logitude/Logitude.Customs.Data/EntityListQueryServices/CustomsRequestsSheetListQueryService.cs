using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.DataContracts;
using Simplog.Data.InfrastructureModel;
using System.Runtime.Remoting.Contexts;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class CustomsRequestsSheetListQueryService
    {
        private IQueryable<CustomsRequestsSheetList> GetIqueryableList(IQueryable<CustomsRequestsSheet> iQueryable)
        {
            IQueryable<CustomsRequestsSheetList> query = (from a in iQueryable
                                                          select new CustomsRequestsSheetList()
                                                          {
                                                              Id = a.Id,

                                                              AnswerCreateDate = a.AnswerCreateDate,
                                                              IsDCA = a.IsDCA,
                                                              CorrelationId = a.CorrelationId,
                                                              CustomFileNo = a.CustomFileNo,
                                                              EntityId1 = a.EntityId1,
                                                              EntityId2 = a.EntityId2,
                                                              EntityReference = a.EntityReference,
                                                              InterfaceTypeCode = a.InterfaceTypeCode,
                                                              ObjectTableId1 = a.ObjectTableId1,
                                                              ObjectTableId2 = a.ObjectTableId2,
                                                              RequestComminicationId = a.RequestComminicationId,
                                                              RequestCreateDate = a.RequestCreateDate,
                                                              RequestDescription = a.RequestDescription,
                                                              RequestOwnerId = a.RequestOwnerId,
                                                              RequestStatusCode = a.RequestStatusCode,
                                                              Tenant = a.Tenant,
                                                              SearchFields = a.SearchFields,
                                                              InterfaceTypeName = a.InterfaceManagement != null ? a.InterfaceManagement.Description : null,
                                                              RequestOwnerName = !string.IsNullOrEmpty(a.User.Contact.LocalName) ? a.User.Contact.LocalName : a.User.Contact.EnglishName,
                                                              RequestStatusName = a.CustomsRequestsSheetStatus != null ? a.CustomsRequestsSheetStatus.LocalName : null,
                                                              IsRestored = a.IsRestored,

                                                          });
            return query;
        }

        private IQueryable<CustomsRequestsSheet> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsRequestsSheet> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<PriorityRequestsSheetSummary> GetStatisticsByCourierDeclarations(int tenant,string courierMasterId)
        {


            //var lastweek = DateTime.Now.Date.AddDays(-7);
            /*var query1 = (from b in context.CourierDeclarations
                          where b.Tenant == tenant && b.CourierMasterId == courierMasterId
                          select b.DeclarationId).ToList();*/

            var query = (from a in context.RequestSheetInQueueMesViews
                         join c in context.CourierDeclarations on
                          a.EntityId1 equals c.DeclarationId 
                          where a.Tenant == tenant && c.CourierMasterId == courierMasterId
                          select new CustomsRequestsSheetList()
                          {
                              ObjectTableId1 = a.ObjectTableId1,
                              EntityId1 = a.EntityId1,
                              InterfaceTypeCode = a.InterfaceTypeCode,
                              InterfaceTypeName = a.InterfaceTypeName,
                              TenantPriority = a.TenantPriority
                          });
            var query2 = (from a in context.RequestSheetInQueueMesViews
                         join c in context.CourierDeclarations on
                          a.EntityId1 equals c.CourierMasterId
                         where a.Tenant == tenant && c.CourierMasterId == courierMasterId
                         select new CustomsRequestsSheetList()
                         {
                             ObjectTableId1 = a.ObjectTableId1,
                             EntityId1 = a.EntityId1,
                             InterfaceTypeCode = a.InterfaceTypeCode,
                             InterfaceTypeName = a.InterfaceTypeName,
                             TenantPriority = a.TenantPriority
                         }).Distinct();
            var resultQuery = query.Concat(query2);

            /*IQueryable < CustomsRequestsSheetList > query = (from a in context.CustomsRequestsSheets
                                                              where a.Tenant == tenant &&  (a.RequestStatusCode == "5" || a.RequestStatusCode == "1" ||
                                                              a.RequestStatusCode == "2" || a.RequestStatusCode == "21")
                                                              && a.RequestCreateDate >= lastweek
                                                              select new CustomsRequestsSheetList()
                                                              {
                                                                  ObjectTableId1=a.ObjectTableId1,
                                                                  EntityId1 = a.EntityId1,
                                                                  InterfaceTypeCode = a.InterfaceTypeCode,
                                                                  InterfaceTypeName = a.InterfaceManagement != null ? a.InterfaceManagement.Description : null,
                                                              });*/


            var qGroupIt = resultQuery.GroupBy(q =>new { q.InterfaceTypeName, q.InterfaceTypeCode, q.TenantPriority }).Select(g => new PriorityRequestsSheetSummary
            {
                Id = new Guid(),
                count = g.Select(x => x.InterfaceTypeCode).Count(),
                totalCount=g.Select(x => x.InterfaceTypeCode).Count(),
                InterfaceTypeName = g.Key.InterfaceTypeName,
                InterfaceTypeCode = g.Key.InterfaceTypeCode,
                TenantPriority = g.Key.TenantPriority
            });
            return qGroupIt.Where(r => r.count > 0).ToList();
        }


    }


}
