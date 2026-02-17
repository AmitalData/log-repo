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

    public partial class TMDayOffTypeListQueryService
    {
	    private IQueryable<TMDayOffTypeList> GetIqueryableList(IQueryable<TMDayOffType> iQueryable)
        {
		IQueryable<TMDayOffTypeList> query = (from a in iQueryable
                                            select new TMDayOffTypeList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<TMDayOffType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TMDayOffType> iQueryable)
        {
            return iQueryable;
        }
				private IQueryable<TMDayOffType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TMDayOffType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	