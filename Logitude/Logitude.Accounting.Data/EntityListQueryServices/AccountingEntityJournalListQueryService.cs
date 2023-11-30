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

    public partial class AccountingEntityJournalListQueryService
    {
	    private IQueryable<AccountingEntityJournalList> GetIqueryableList(IQueryable<AccountingEntityJournal> iQueryable)
        {
		IQueryable<AccountingEntityJournalList> query = (from a in iQueryable
                                            select new AccountingEntityJournalList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          AccountingEntityId = a.AccountingEntityId,
					
					                          AccountingEntityCode = a.AccountingEntityCode,
					
					                          Action = a.Action,
					
					                          ChildEntityId = a.ChildEntityId,
					
		                    	            });
            return query;
		}

		private IQueryable<AccountingEntityJournal> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AccountingEntityJournal> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<AccountingEntityJournal> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AccountingEntityJournal> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	