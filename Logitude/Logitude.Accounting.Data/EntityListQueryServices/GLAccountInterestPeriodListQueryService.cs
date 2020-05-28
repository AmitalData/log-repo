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

    public partial class GLAccountInterestPeriodListQueryService
    {
	    private IQueryable<GLAccountInterestPeriodList> GetIqueryableList(IQueryable<GLAccountInterestPeriod> iQueryable)
        {
		IQueryable<GLAccountInterestPeriodList> query = (from a in iQueryable
                                            select new GLAccountInterestPeriodList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          LineNumber = a.LineNumber,
					
					                          GLAccountId = a.GLAccountId,
					
					                          PeriodStartDate = a.PeriodStartDate,
					
					                          StandardInterestRateBaseId = a.StandardInterestRateBaseId,
					
					                          StandardAddInterestPercent = a.StandardAddInterestPercent,
					
					                          ExceptionalInterestRateBaseId = a.ExceptionalInterestRateBaseId,
					
					                          ExceptionalAddInterestPercent = a.ExceptionalAddInterestPercent,
					
					                          CreditInterestRateBaseId = a.CreditInterestRateBaseId,
					
					                          CreditAddInterestPercent = a.CreditAddInterestPercent,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          UpdateDateTime = a.UpdateDateTime,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          CreateDateTime = a.CreateDateTime,
					
		                    	            });
            return query;
		}

		private IQueryable<GLAccountInterestPeriod> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccountInterestPeriod> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<GLAccountInterestPeriod> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccountInterestPeriod> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	