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

    public partial class JournalAdditionalDataListQueryService
    {
	    private IQueryable<JournalAdditionalDataList> GetIqueryableList(IQueryable<JournalAdditionalData> iQueryable)
        {
		IQueryable<JournalAdditionalDataList> query = (from a in iQueryable
                                            select new JournalAdditionalDataList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          JournalId = a.JournalId,
					
					                          TaxReportId = a.TaxReportId,
					
					                          TaxReportTransmitStatusCode = a.TaxReportTransmitStatusCode,
					
		                    	            });
            return query;
		}

		private IQueryable<JournalAdditionalData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<JournalAdditionalData> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<JournalAdditionalData> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<JournalAdditionalData> iQueryable, int tenant)
        {
			return iQueryable;
		}



		public IQueryable<JournalAdditionalDataList> GetJournalMoreDatasForJournal(string JournalId, int tenant)
		{
			IQueryable<JournalAdditionalDataList> Journallines;



			IQueryable<JournalAdditionalData> q = (from a in context.JournalAdditionalDatas
												 where a.JournalId == JournalId && a.Tenant == tenant
												 select a);

			var res=GetIqueryableList(q);

			return res;
		}
	}


}
	