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

    public partial class InterestTransactionListQueryService
    {
	    private IQueryable<InterestTransactionList> GetIqueryableList(IQueryable<InterestTransaction> iQueryable)
        {
		IQueryable<InterestTransactionList> query = (from a in iQueryable
                                            select new InterestTransactionList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDateTime = a.CreateDateTime,
					
					                          UpdateDateTime = a.UpdateDateTime,
					
					                          SearchFields = a.SearchFields,
					
					                          GLAccountId = a.GLAccountId,
					
					                          InterestEntityTypeCode = a.InterestEntityTypeCode,
					
					                          EntityId = a.EntityId,
					
					                          OriginalEntityLineNumber = a.OriginalEntityLineNumber,
					
					                          LocalAmount = a.LocalAmount,
					
					                          ForeignAmount = a.ForeignAmount,
					
					                          CurrencyId = a.CurrencyId,
					
					                          InterestValueDate = a.InterestValueDate,
					
					                          InterestReportId = a.InterestReportId,
					
					                          IsClosed = a.IsClosed,
					
		                    	            });
            return query;
		}

		private IQueryable<InterestTransaction> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestTransaction> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<InterestTransaction> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestTransaction> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	