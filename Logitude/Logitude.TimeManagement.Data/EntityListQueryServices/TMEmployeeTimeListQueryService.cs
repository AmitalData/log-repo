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

    public partial class TMEmployeeTimeListQueryService
    {
	    private IQueryable<TMEmployeeTimeList> GetIqueryableList(IQueryable<TMEmployeeTime> iQueryable)
        {
		IQueryable<TMEmployeeTimeList> query = (from a in iQueryable
                                            select new TMEmployeeTimeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          EmployeeUserId = a.EmployeeUserId,
					
					                          DateOfWork = a.DateOfWork,
					
					                          Description = a.Description,
					
					                          TimeInMinutes = a.TimeInMinutes,
					
					                          WINumber = a.WINumber,
					
		                    	            });
            return query;
		}

		private IQueryable<TMEmployeeTime> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TMEmployeeTime> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<TMEmployeeTime> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TMEmployeeTime> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	