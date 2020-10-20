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

    public partial class CalculatedChartsOfAccountsLineListQueryService
    {
	    private IQueryable<CalculatedChartsOfAccountsLineList> GetIqueryableList(IQueryable<CalculatedChartsOfAccountsLine> iQueryable)
        {
		IQueryable<CalculatedChartsOfAccountsLineList> query = (from a in iQueryable
                                            select new CalculatedChartsOfAccountsLineList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,

											  CreatedByEnglishName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName,

											  CreatedByLocalName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.LocalName,

											  UpdatedByEnglishName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName,

											  UpdatedByLocalName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.LocalName,

											  CreateDateTime = a.CreateDateTime,

											  CalculatedChartsOfAccountsId = a.CalculatedChartsOfAccountsId,

											  GLAccountEnglishName = a.GLAccount == null ? null : a.GLAccount.EnglishName,

											  GLAccountLocalName = a.GLAccount == null ? null : a.GLAccount.LocalName,

											  ChartOfAccountEnglishName = a.ChartOfAccount == null ? null : a.ChartOfAccount.EnglishName,

											  ChartOfAccountLocalName = a.ChartOfAccount == null ? null : a.ChartOfAccount.LocalName,

											  LineTypeEnglishName = a.CalculatedChartsLineType ==null ? null : a.CalculatedChartsLineType.EnglishName,

											  LineTypeLocalName = a.CalculatedChartsLineType == null ? null : a.CalculatedChartsLineType.LocalName,

					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedDateTime = a.UpdatedDateTime,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
		                    	            });
            return query;
		}

		private IQueryable<CalculatedChartsOfAccountsLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CalculatedChartsOfAccountsLine> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<CalculatedChartsOfAccountsLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CalculatedChartsOfAccountsLine> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	