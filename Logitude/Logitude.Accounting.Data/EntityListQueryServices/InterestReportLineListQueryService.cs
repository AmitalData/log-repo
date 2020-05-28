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

    public partial class InterestReportLineListQueryService
    {
	    private IQueryable<InterestReportLineList> GetIqueryableList(IQueryable<InterestReportLine> iQueryable)
        {
		IQueryable<InterestReportLineList> query = (from a in iQueryable
                                            select new InterestReportLineList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          InterestReportId = a.InterestReportId,
					
					                          InterestTransactionId = a.InterestTransactionId,
					
		                    	            });
            return query;
		}

		private IQueryable<InterestReportLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestReportLine> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<InterestReportLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestReportLine> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	