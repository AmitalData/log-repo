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

    public partial class JournalExternalReconcileListQueryService
    {
	    private IQueryable<JournalExternalReconcileList> GetIqueryableList(IQueryable<JournalExternalReconcile> iQueryable)
        {
		IQueryable<JournalExternalReconcileList> query = (from a in iQueryable
                                            select new JournalExternalReconcileList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<JournalExternalReconcile> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<JournalExternalReconcile> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<JournalExternalReconcile> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<JournalExternalReconcile> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	