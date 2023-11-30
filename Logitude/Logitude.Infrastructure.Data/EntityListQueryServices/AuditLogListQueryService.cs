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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class AuditLogListQueryService
    {
	    private IQueryable<AuditLogList> GetIqueryableList(IQueryable<AuditLog> iQueryable)
        {
		IQueryable<AuditLogList> query = (from a in iQueryable
                                            select new AuditLogList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          ObjectTableId = a.ObjectTableId,
					
					                          EntityId = a.EntityId,
					
					                          ChangesJson = a.ChangesJson,
					
		                    	            });
            return query;
		}

		private IQueryable<AuditLog> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AuditLog> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<AuditLog> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AuditLog> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	