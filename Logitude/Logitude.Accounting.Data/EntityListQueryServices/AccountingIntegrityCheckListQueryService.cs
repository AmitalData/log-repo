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

    public partial class AccountingIntegrityCheckListQueryService
    {
	    private IQueryable<AccountingIntegrityCheckList> GetIqueryableList(IQueryable<AccountingIntegrityCheck> iQueryable)
        {
            IQueryable<AccountingIntegrityCheckList> query = (from a in iQueryable.Include("IntegrityCheckStatus")
                                                              select new AccountingIntegrityCheckList()
                                                              {

                                                                  Id = a.Id,

                                                                  Tenant = a.Tenant,

                                                                  CreateDateTimeUTC = a.CreateDateTimeUTC,

                                                                  StatusCode = a.StatusCode,

                                                                  HasException = a.HasException,

                                                                  DoneDateTimeUTC = a.DoneDateTimeUTC,

                                                                  StatusName = a.IntegrityCheckStatus.Name,

                                                                  SearchFields = a.SearchFields,
                                                              });
            return query;
		}

		private IQueryable<AccountingIntegrityCheck> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AccountingIntegrityCheck> iQueryable, int tenant)
        {
			return iQueryable;
            //throw new NotImplementedException();
        }
        private IQueryable<AccountingIntegrityCheck> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AccountingIntegrityCheck> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	