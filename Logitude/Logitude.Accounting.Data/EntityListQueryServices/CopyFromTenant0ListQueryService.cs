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

    public partial class CopyFromTenant0ListQueryService
    {
	    private IQueryable<CopyFromTenant0List> GetIqueryableList(IQueryable<CopyFromTenant0> iQueryable)
        {
		IQueryable<CopyFromTenant0List> query = (from a in iQueryable
                                            select new CopyFromTenant0List()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          TableName = a.TableName,
					                          CreatedByUserName=a.CreatedByUserName,
		                    	            });
            return query;
		}

		private IQueryable<CopyFromTenant0> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CopyFromTenant0> iQueryable, int tenant)
        {
            return iQueryable;

        }
        private IQueryable<CopyFromTenant0> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CopyFromTenant0> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	