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

    public partial class AccountingEntitiesJournalListQueryService
    {
	    private IQueryable<AccountingEntitiesJournalList> GetIqueryableList(IQueryable<AccountingEntitiesJournal> iQueryable)
        {
		IQueryable<AccountingEntitiesJournalList> query = (from a in iQueryable
                                            select new AccountingEntitiesJournalList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          AccountingEntityId = a.AccountingEntityId,
					
					                          AccountingEntityCode = a.AccountingEntityCode,
					
					                          ChildEntityId = a.ChildEntityId,
					
					                          Action = a.Action,
					
		                    	            });
            return query;
		}

		private IQueryable<AccountingEntitiesJournal> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AccountingEntitiesJournal> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<AccountingEntitiesJournal> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AccountingEntitiesJournal> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	