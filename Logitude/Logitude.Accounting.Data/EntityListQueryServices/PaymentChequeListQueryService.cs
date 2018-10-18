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
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class PaymentChequeListQueryService
    {
	    private IQueryable<PaymentChequeList> GetIqueryableList(IQueryable<PaymentCheque> iQueryable)
        {


            IQueryable<PaymentChequeList> query = (from a in iQueryable.Include("PaymentChequeStatus")
                                            select new PaymentChequeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          InternalNumber = a.InternalNumber,
					
					                          ChequeNumber = a.ChequeNumber,
					
					                          PayToGLAccountId = a.PayToGLAccountId,
					
					                          PayToName = a.PayToName ,
					
					                          BankAccountId = a.BankAccountId,
					
					                          BankAccountGLAccountId = a.BankAccountGLAccountId,
					
					                          LocalAmount = a.LocalAmount,
					
					                          CurrencyId = a.CurrencyId,
					
					                          ForeignAmount = a.ForeignAmount,
					
					                          ExchangeRate = a.ExchangeRate,
					
					                          ValueDate = a.ValueDate,
					
					                          PrintDate = a.PrintDate,
					
					                          ApproveDate = a.ApproveDate,
					
					                          ApprovedByUserId = a.ApprovedByUserId,
					
					                          IsCancelled = a.IsCancelled,
					
					                          CancelledByUserId = a.CancelledByUserId,
					
					                          CancelledDate = a.CancelledDate,
					
					                          CancellationRemarks = a.CancellationRemarks,
					
					                          PaymentChequeStatusCode = a.PaymentChequeStatusCode,
					
					                          EntityId = a.EntityId,
					
					                          ObjectTableId = a.ObjectTableId,
					                          PaymentChequeStatusName= a.PaymentChequeStatus != null? a.PaymentChequeStatus.LocalName:null,
                                              BankAccountName= a.BankAccount != null? a.BankAccount.LocalName :null,
                                              GLAccountNumber = a.PayToGLAccount != null ? a.PayToGLAccount.DisplayNumber : null,
                                            GLAccountName = a.PayToGLAccount != null? a.PayToGLAccount.LocalName : null,
                                           StatusEnglishName= a.PaymentChequeStatus != null? a.PaymentChequeStatus.EnglishName :null,
                                            });
            return query;
		}

		private IQueryable<PaymentCheque> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PaymentCheque> iQueryable, int tenant)
        {
            return iQueryable;

        }
				private IQueryable<PaymentCheque> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<PaymentCheque> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	