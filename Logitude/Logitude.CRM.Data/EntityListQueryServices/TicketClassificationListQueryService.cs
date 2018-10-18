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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.CustomFilters;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class TicketClassificationListQueryService
    {
	    private IQueryable<TicketClassificationList> GetIqueryableList(IQueryable<TicketClassification> iQueryable)
        {
            TicketClassificationRepository ticketRepository;
            List<TicketClassificationList> query = (from a in iQueryable
                                                          select new TicketClassificationList()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        Name = a.Name,
                                                        Inactive = a.Inactive,
                                                        ParentId = a.ParentId,
                                                        SearchFields = a.SearchFields,
                                                        EmployeeGroupId = a.EmployeeGroupId,
                                                        DefaultSeverityId=a.DefaultSeverityId,
                                                        ManagerUserId = a.EscalationUser == null ? "" : a.EscalationUser.Id,
                                                        EscalationNotify = a.EscalationNotify,

                                                    }).ToList();

           
            foreach (TicketClassificationList item in query)
            {
                if (!string.IsNullOrEmpty(item.ParentId))
                {
                    ticketRepository = new TicketClassificationRepository(item.Tenant);
                    TicketClassification myParent = ticketRepository.GetSingle(item.ParentId, item.Tenant);
                    if (myParent != null)
                    {
                        item.ParentName = myParent.Name;
                    }
                }
            }

            return query.AsQueryable();
		}

		private IQueryable<TicketClassification> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TicketClassification> iQueryable,int tenant)
        {
            return TicketClassificationCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);
		}

		private IQueryable<TicketClassification> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TicketClassification> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}
}
	