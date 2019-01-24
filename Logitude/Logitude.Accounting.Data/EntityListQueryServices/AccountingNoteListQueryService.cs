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

    public partial class AccountingNoteListQueryService
    {
	    private IQueryable<AccountingNoteList> GetIqueryableList(IQueryable<AccountingNote> iQueryable)
        {
		IQueryable<AccountingNoteList> query = (from a in iQueryable
                                            select new AccountingNoteList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          CardId = a.CardId,
					
					                          Notes = a.Notes,
					
		                    	            });
            return query;
		}

		private IQueryable<AccountingNote> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AccountingNote> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<AccountingNote> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AccountingNote> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	