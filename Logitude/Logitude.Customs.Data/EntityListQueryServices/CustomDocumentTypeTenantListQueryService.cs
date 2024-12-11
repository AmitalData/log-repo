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

    public partial class CustomDocumentTypeTenantListQueryService
    {
	    private IQueryable<CustomDocumentTypeTenantList> GetIqueryableList(IQueryable<CustomDocumentTypeTenant> iQueryable)
        {
		IQueryable<CustomDocumentTypeTenantList> query = (from a in iQueryable
                                            select new CustomDocumentTypeTenantList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<CustomDocumentTypeTenant> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomDocumentTypeTenant> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	