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

    public partial class ARPaymentBankTranferListQueryService
    {
	    private IQueryable<ARPaymentBankTranferList> GetIqueryableList(IQueryable<ARPaymentBankTranfer> iQueryable)
        {
		IQueryable<ARPaymentBankTranferList> query = (from a in iQueryable
                                            select new ARPaymentBankTranferList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          PaymentId = a.PaymentId,
					
					                          LineNumber = a.LineNumber,
					
					                          PaymentRef = a.PaymentRef,
					
					                          ValueDate = a.ValueDate,
					
					                          BankAccountId = a.BankAccountId,
					
					                          CurrencyId = a.CurrencyId,
					
					                          LocalAmount = a.LocalAmount,
					
					                          ForeignAmount = a.ForeignAmount,
					
					                          ExchageRate = a.ExchageRate,
					
		                    	            });
            return query;
		}

		private IQueryable<ARPaymentBankTranfer> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ARPaymentBankTranfer> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<ARPaymentBankTranfer> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ARPaymentBankTranfer> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	