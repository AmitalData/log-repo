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

using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityLists;

namespace Logitude.TimeManagement.Data.EntityListQueryServices
{ 

    public partial class TMOfficeHourListQueryService
    {
	    private IQueryable<TMOfficeHourList> GetIqueryableList(IQueryable<TMOfficeHour> iQueryable)
        {
		IQueryable<TMOfficeHourList> query = (from a in iQueryable
                                            select new TMOfficeHourList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          UserId = a.UserId,
					
					                          WorkDate = a.WorkDate,
					
					                          RecordedEntryTime = a.RecordedEntryTime,
					
					                          RecordedExitTime = a.RecordedExitTime,
					
					                          EntryTime = a.EntryTime,
					
					                          ExitTime = a.ExitTime,
					
					                          Description = a.Description,
					                            
                                              Inactive = a.Inactive
		                    	            });
            return query;
		}

		private IQueryable<TMOfficeHour> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TMOfficeHour> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<TMOfficeHour> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TMOfficeHour> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	