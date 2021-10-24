using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
        public List<CustomsRequestsSheetSummary> GetStatistics(int tenant)
        {
            var lastweek = DateTime.Now.Date.AddDays(-7);
            IQueryable<CustomsRequestsSheetList> query = (from a in context.CustomsRequestsSheets
                                                          where a.Tenant == tenant && (a.RequestStatusCode == "5" || a.RequestStatusCode == "1" ||
                                                          a.RequestStatusCode == "2" || a.RequestStatusCode == "21" || a.RequestStatusCode == "99")
                                                          && a.RequestCreateDate >= lastweek
                                                          select new CustomsRequestsSheetList()
                                                          {
                                                              InterfaceTypeCode = a.InterfaceTypeCode,
                                                              InterfaceTypeName = a.InterfaceManagement != null ? a.InterfaceManagement.Description : null,
                                                          });


            var qGroupIt = query.GroupBy(q => q.InterfaceTypeName).Select(g => new CustomsRequestsSheetSummary
            {
                Id = new Guid(),
                count = g.Select(x => x.InterfaceTypeCode).Count(),
                InterfaceTypeName = g.Key
            }).ToList();
            return qGroupIt;
        }


    }


}
