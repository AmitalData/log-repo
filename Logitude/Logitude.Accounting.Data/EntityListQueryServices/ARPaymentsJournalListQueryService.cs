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

    public partial class ARPaymentsJournalListQueryService
    {
	    private IQueryable<ARPaymentsJournalList> GetIqueryableList(IQueryable<ARPaymentsJournal> iQueryable)
        {
		IQueryable<ARPaymentsJournalList> query = (from a in iQueryable
                                            select new ARPaymentsJournalList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          PaymentId = a.PaymentId,
					
					                          IsVoided = a.IsVoided,
					
		                    	            });
            return query;
		}

		private IQueryable<ARPaymentsJournal> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ARPaymentsJournal> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<ARPaymentsJournal> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ARPaymentsJournal> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	