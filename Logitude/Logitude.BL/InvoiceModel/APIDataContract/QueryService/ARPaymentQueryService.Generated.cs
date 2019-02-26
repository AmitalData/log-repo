using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel;

 namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{ 
   public partial class ARPaymentQueryService
   {
   
		IInvoiceContext  context;
		//ARPaymentService service; 
		
		ARPaymentQuery query; 

        public ARPaymentQueryService(int tenant)
        {
				    context = InvoiceContext.GetContext(tenant); 
			//service = new ARPaymentService(context, tenant); 
			query = new ARPaymentQuery(tenant);
        }

		
		public ARPayment GetARPaymentById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ARPayment with Id " + Id + " doesn't exist");

				return ARPaymentDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public ARPayment ARPaymentDataMapping(ARPaymentPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ARPayment(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.PaymentNo = MyEntityPM.PaymentNo;			  
				   if(MyEntityPM.AccountingPaymentMethodId != null)
				   {
					   AccountingPaymentMethodQueryService AccountingPaymentMethodService0 = new AccountingPaymentMethodQueryService(Tenant);
					   					   temp.AccountingPaymentMethod = AccountingPaymentMethodService0.GetAccountingPaymentMethodById(MyEntityPM.AccountingPaymentMethodId,Tenant); 
			       
					   				   }
				   
				   temp.AmountInLocalCurrency = MyEntityPM.AmountInLocalCurrency;
				   temp.AmountInPaymentCurrency = MyEntityPM.AmountInPaymentCurrency;
				   temp.PaidBy = MyEntityPM.PaidBy;
				   temp.PaymentCurrencyExchangeRate = MyEntityPM.PaymentCurrencyExchangeRate;
				   temp.ExchangeRateDate = MyEntityPM.ExchangeRateDate;			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService1 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService1.GetUserById(MyEntityPM.CreatedByUserId,Tenant); 
			       
					   				   }
				   
				   temp.LocalCurrencyCode = MyEntityPM.LocalCurrencyCode;			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService2 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService2.GetBranchById(MyEntityPM.BranchId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.PaymentCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService3 = new CurrencyQueryService(Tenant);
					   					   temp.PaymentCurrency = CurrencyService3.GetCurrencyById(MyEntityPM.PaymentCurrencyId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.BillToId != null)
				   {
					   CardQueryService CardService4 = new CardQueryService(Tenant);
					   					   temp.BillTo = CardService4.GetCardById(MyEntityPM.BillToId,Tenant); 
			       
					   				   }
				   
				   temp.ChequeOrPaymentRef = MyEntityPM.ChequeOrPaymentRef;
				   temp.Bank = MyEntityPM.Bank;
				   temp.BankBranch = MyEntityPM.BankBranch;
				   temp.Account = MyEntityPM.Account;
				   temp.ValueDate = MyEntityPM.ValueDate;
				   temp.RegisterDate = MyEntityPM.RegisterDate;			  
				   if(MyEntityPM.CreditCardTypeId != null)
				   {
					   CreditCardTypeQueryService CreditCardTypeService5 = new CreditCardTypeQueryService(Tenant);
					   					   temp.CreditCardType = CreditCardTypeService5.GetCreditCardTypeById(MyEntityPM.CreditCardTypeId,Tenant); 
			       
					   				   }
				   
				   temp.PaymentCurrencyCode = MyEntityPM.PaymentCurrencyCode;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ARPaymentPM ARPaymentDataMappingAndValidatin(ARPayment MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new ARPaymentPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("ARPayment with Id " + MyEntity.Id + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					temp.PaymentNo = MyEntity.PaymentNo;					AccountingPaymentMethodQueryService AccountingPaymentMethodAccountingPaymentMethodService = new AccountingPaymentMethodQueryService(Tenant);
					if(MyEntity.AccountingPaymentMethod != null)
					{
						var myAccountingPaymentMethodPM = AccountingPaymentMethodAccountingPaymentMethodService.AccountingPaymentMethodDataMappingAndValidatin(MyEntity.AccountingPaymentMethod,Tenant,ComputingPartnerName);
												if(myAccountingPaymentMethodPM != null)
						{
							temp.AccountingPaymentMethodId = myAccountingPaymentMethodPM.Id;
						}
						 
					}
			
					
					temp.AmountInLocalCurrency = MyEntity.AmountInLocalCurrency;
					temp.AmountInPaymentCurrency = MyEntity.AmountInPaymentCurrency;
					temp.PaidBy = MyEntity.PaidBy;
					temp.PaymentCurrencyExchangeRate = MyEntity.PaymentCurrencyExchangeRate;
					temp.ExchangeRateDate = MyEntity.ExchangeRateDate;					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName);
												if(myCreatedByUserPM != null)
						{
							temp.CreatedByUserId = myCreatedByUserPM.Id;
						}
						 
					}
			
					
					temp.LocalCurrencyCode = MyEntity.LocalCurrencyCode;					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName);
												if(myBranchPM != null)
						{
							temp.BranchId = myBranchPM.Id;
						}
						 
					}
			
										CurrencyQueryService PaymentCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.PaymentCurrency != null)
					{
						var myPaymentCurrencyPM = PaymentCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.PaymentCurrency,Tenant,ComputingPartnerName);
												if(myPaymentCurrencyPM != null)
						{
							temp.PaymentCurrencyId = myPaymentCurrencyPM.Id;
						}
						 
					}
			
										CardQueryService BillToCardService = new CardQueryService(Tenant);
					if(MyEntity.BillTo != null)
					{
						var myBillToPM = BillToCardService.CardDataMappingAndValidatin(MyEntity.BillTo,Tenant,ComputingPartnerName);
												if(myBillToPM != null)
						{
							temp.BillToId = myBillToPM.Id;
						}
						 
					}
			
					
					temp.ChequeOrPaymentRef = MyEntity.ChequeOrPaymentRef;
					temp.Bank = MyEntity.Bank;
					temp.BankBranch = MyEntity.BankBranch;
					temp.Account = MyEntity.Account;
					temp.ValueDate = MyEntity.ValueDate;
					temp.RegisterDate = MyEntity.RegisterDate;					CreditCardTypeQueryService CreditCardTypeCreditCardTypeService = new CreditCardTypeQueryService(Tenant);
					if(MyEntity.CreditCardType != null)
					{
						var myCreditCardTypePM = CreditCardTypeCreditCardTypeService.CreditCardTypeDataMappingAndValidatin(MyEntity.CreditCardType,Tenant,ComputingPartnerName);
												if(myCreditCardTypePM != null)
						{
							temp.CreditCardTypeId = myCreditCardTypePM.Id;
						}
						 
					}
			
					
					temp.PaymentCurrencyCode = MyEntity.PaymentCurrencyCode;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}