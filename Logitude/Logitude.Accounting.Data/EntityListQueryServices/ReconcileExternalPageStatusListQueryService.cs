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

    public partial class ReconcileExternalPageStatusListQueryService
    {
	    private IQueryable<ReconcileExternalPageStatusList> GetIqueryableList(IQueryable<ReconcileExternalPageStatus> iQueryable)
        {
		IQueryable<ReconcileExternalPageStatusList> query = (from a in iQueryable
                                            select new ReconcileExternalPageStatusList()
											{
                     
					                          Code = a.Code,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          EnglishName = a.EnglishName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<ReconcileExternalPageStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ReconcileExternalPageStatus> iQueryable)
        {
            return iQueryable;
        }
				private IQueryable<ReconcileExternalPageStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ReconcileExternalPageStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	