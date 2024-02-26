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

    public partial class ChequeCounterSerialListQueryService
    {
	    private IQueryable<ChequeCounterSerialList> GetIqueryableList(IQueryable<ChequeCounterSerial> iQueryable)
        {
		IQueryable<ChequeCounterSerialList> query = (from a in iQueryable
                                            select new ChequeCounterSerialList()
											{
					                          Tenant = a.Tenant,
					
					                          BankAccountId = a.BankAccountId,
					
					                          SeriesId = a.SeriesId,
					
					                          ChequeCounterBegin = a.ChequeCounterBegin,
					
					                          ChequeCounterEnd = a.ChequeCounterEnd,
					
		                    	            });
            return query;
		}

		private IQueryable<ChequeCounterSerial> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ChequeCounterSerial> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<ChequeCounterSerial> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ChequeCounterSerial> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	