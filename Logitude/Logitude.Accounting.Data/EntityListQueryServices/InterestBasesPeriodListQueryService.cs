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

    public partial class InterestBasesPeriodListQueryService
    {
	    private IQueryable<InterestBasesPeriodList> GetIqueryableList(IQueryable<InterestBasesPeriod> iQueryable)
        {
		IQueryable<InterestBasesPeriodList> query = (from a in iQueryable
                                            select new InterestBasesPeriodList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          InterestBaseTypeId = a.InterestBaseTypeId,
					
					                          LineNumber = a.LineNumber,
					
					                          InterestBaseStartDate = a.InterestBaseStartDate,
					
					                          InterestRate = a.InterestRate,
					
		                    	            });
            return query;
		}

		private IQueryable<InterestBasesPeriod> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestBasesPeriod> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<InterestBasesPeriod> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestBasesPeriod> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	