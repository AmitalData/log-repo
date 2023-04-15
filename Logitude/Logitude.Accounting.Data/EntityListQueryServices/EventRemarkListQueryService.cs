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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class EventRemarkListQueryService
    {
	    private IQueryable<EventRemarkList> GetIqueryableList(IQueryable<EventRemark> iQueryable)
        {
		IQueryable<EventRemarkList> query = (from a in iQueryable
                                            select new EventRemarkList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          EventTypeId = a.EventTypeId,
					
					                          PartnerTypeId = a.PartnerTypeId,
					
					                          IsChoose = a.IsChoose,
					
		                    	            });
            return query;
		}

		private IQueryable<EventRemark> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<EventRemark> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<EventRemark> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<EventRemark> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	