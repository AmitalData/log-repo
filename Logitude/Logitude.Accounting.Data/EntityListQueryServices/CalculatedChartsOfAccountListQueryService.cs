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

    public partial class CalculatedChartsOfAccountListQueryService
    {
	    private IQueryable<CalculatedChartsOfAccountList> GetIqueryableList(IQueryable<CalculatedChartsOfAccount> iQueryable)
        {
		IQueryable<CalculatedChartsOfAccountList> query = (from a in iQueryable
                                            select new CalculatedChartsOfAccountList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,

											  EnglishName = a.EnglishName,

											  LocalName = a.LocalName,

											  CreatedByEnglishName = a.CreatedByUser==null? null: a.CreatedByUser.Contact ==null ? null : a.CreatedByUser.Contact.EnglishName,
											
											  CretedByLocalNameName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.LocalName,
											  
											  UpdatedByEnglishName = a.UpdatedByUser==null? null: a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName,

											  UpdatedByLocalName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.LocalName,

											  ChartOfAccountTypeCode = a.ChartOfAccountTypeCode,

											  ChartOfAccountTypeEnglishName = a.ChartOfAccountsType == null ? null : a.ChartOfAccountsType.EnglishName,

											  ChartOfAccountTypeLocalName = a.ChartOfAccountsType == null ? null : a.ChartOfAccountsType.LocalName,

											  IsCancelled = a.IsCancelled,

											  CreateDateTime = a.CreateDateTime,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedDateTime = a.UpdatedDateTime,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
		                    	            });
            return query;
		}

		private IQueryable<CalculatedChartsOfAccount> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CalculatedChartsOfAccount> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<CalculatedChartsOfAccount> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CalculatedChartsOfAccount> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	