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

    public partial class ExternalReconciliationLineListQueryService
    {
	    private IQueryable<ExternalReconciliationLineList> GetIqueryableList(IQueryable<ExternalReconciliationLine> iQueryable)
        {
		IQueryable<ExternalReconciliationLineList> query = (from a in iQueryable
                                            select new ExternalReconciliationLineList()
											{
                     
					                          LedgerTransactionId = a.LedgerTransactionId,
					
					                          GroupNumber = a.GroupNumber,
					
					                          ExternalPageLineId = a.ExternalPageLineId,
					
		                    	            });
            return query;
		}

		private IQueryable<ExternalReconciliationLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ExternalReconciliationLine> iQueryable, int tenant)
        {
            //throw new NotImplementedException();
            return iQueryable;

        }
        private IQueryable<ExternalReconciliationLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ExternalReconciliationLine> iQueryable, int tenant )
        {
			return iQueryable;
		}
		
			}


}
	