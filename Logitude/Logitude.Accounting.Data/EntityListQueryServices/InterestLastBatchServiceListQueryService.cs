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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class InterestLastBatchServiceListQueryService
    {
	    private IQueryable<InterestLastBatchServiceList> GetIqueryableList(IQueryable<InterestLastBatchService> iQueryable)
        {
		IQueryable<InterestLastBatchServiceList> query = (from a in iQueryable
                                            select new InterestLastBatchServiceList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateReportsBatchId = a.CreateReportsBatchId,
					
					                          CreateInvoicesBatchId = a.CreateInvoicesBatchId,
					
		                    	            });
            return query;
		}

		private IQueryable<InterestLastBatchService> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestLastBatchService> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<InterestLastBatchService> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestLastBatchService> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	