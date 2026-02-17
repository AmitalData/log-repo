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

    public partial class AccountingCompanyTypeListQueryService
    {
	    private IQueryable<AccountingCompanyTypeList> GetIqueryableList(IQueryable<AccountingCompanyType> iQueryable)
        {
		IQueryable<AccountingCompanyTypeList> query = (from a in iQueryable
                                            select new AccountingCompanyTypeList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<AccountingCompanyType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AccountingCompanyType> iQueryable, int tenant)
        {
            return iQueryable;

        }
				private IQueryable<AccountingCompanyType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AccountingCompanyType> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	