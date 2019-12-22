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

    public partial class CashBookLineListQueryService
    {
	    private IQueryable<CashBookLineList> GetIqueryableList(IQueryable<CashBookLine> iQueryable)
        {
            IQueryable<CashBookLineList> query = (from a in iQueryable.Include("ARPaymentCheque")
                                            select new CashBookLineList()
											{
					                          CashBookId = a.CashBookId,
					                          Tenant = a.Tenant,
					                          ARPChequeId = a.ARPChequeId,
                                              ChequeNumber = a.Cheque != null ? a.Cheque.ChequeNumber : null,
					                          IsDeposited = a.IsDeposited,
                                              Currency = a.Cheque.Currency.Code,
                                              DueDate = a.Cheque.ValueDate,
                                              LocalAmount = a.Cheque.LocalAmount,
                                              ForeignAmount = a.Cheque.ForeignAmount,
                                              Bank = a.Cheque.BankAccount,
                                              Branch = a.Cheque.BankBranch,
                                              AccountNumber = a.Cheque.BankId,
                                              ARPaymentNumber = a.Cheque.Payment.PaymentNo,
                                              ARPaymentId = a.Cheque.PaymentId,
                                              ARPChequeStatusCode = a.Cheque.StatusCode,
                                              ARPChequeStatusName = a.Cheque.ARPaymentChequeStatus.LocalName,
                                              SearchFields = a.SearchFields,
                                              
		                    	            });
            return query;
		}

		private IQueryable<CashBookLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CashBookLine> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<CashBookLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CashBookLine> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	