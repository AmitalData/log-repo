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

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class OccasionStatusListQueryService
    {
	    private IQueryable<OccasionStatusList> GetIqueryableList(IQueryable<OccasionStatus> iQueryable)
        {
		IQueryable<OccasionStatusList> query = (from a in iQueryable
                                            select new OccasionStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<OccasionStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OccasionStatus> iQueryable)
        {
            return iQueryable;
        }
				private IQueryable<OccasionStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<OccasionStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	