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

    public partial class DeclarationStatusListQueryService
    {
	    private IQueryable<DeclarationStatusList> GetIqueryableList(IQueryable<DeclarationStatus> iQueryable)
        {
		IQueryable<DeclarationStatusList> query = (from a in iQueryable
                                            select new DeclarationStatusList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationStatus> iQueryable, int tenant)
        {
			return iQueryable;
		}
	}


}
	