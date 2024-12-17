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

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class StatusCodeListQueryService
    {
	    private IQueryable<StatusCodeList> GetIqueryableList(IQueryable<StatusCode> iQueryable)
        {
		IQueryable<StatusCodeList> query = (from a in iQueryable
                                            select new StatusCodeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          Status_Code = a.Status_Code,
					
					                          StatusNameHeb = a.StatusNameHeb,
					
					                          StatusNameEng = a.StatusNameEng,
					
		                    	            });
            return query;
		}

		private IQueryable<StatusCode> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<StatusCode> iQueryable, int tenant)
        {
			return iQueryable;
		}
	}


}
	