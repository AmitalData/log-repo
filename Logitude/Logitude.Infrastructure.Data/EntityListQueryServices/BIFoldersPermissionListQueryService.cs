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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class BIFoldersPermissionListQueryService
    {
	    private IQueryable<BIFoldersPermissionList> GetIqueryableList(IQueryable<BIFoldersPermission> iQueryable)
        {
		IQueryable<BIFoldersPermissionList> query = (from a in iQueryable
                                            select new BIFoldersPermissionList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          FolderId = a.FolderId,
					
					                          UserId = a.UserId,
					
		                    	            });
            return query;
		}

		private IQueryable<BIFoldersPermission> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BIFoldersPermission> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<BIFoldersPermission> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<BIFoldersPermission> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	