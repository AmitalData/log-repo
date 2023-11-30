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
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
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

		
		public ARPayment GetARPaymentById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ARPayment with Id " + Id + " doesn't exist");

				return ARPaymentDataMapping(temp,Tenant,ComputingPartnerName);
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
					   					   temp.AccountingPaymentMethod = AccountingPaymentMethodService0.AccountingPaymentMethodCustomDataMapping(MyEntityPM.AccountingPaymentMethodId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.AmountInLocalCurrency = MyEntityPM.AmountInLocalCurrency;
				   temp.AmountInPaymentCurrency = MyEntityPM.AmountInPaymentCurrency;
				   temp.PaidBy = MyEntityPM.PaidBy;
				   temp.PaymentCurrencyExchangeRate = MyEntityPM.PaymentCurrencyExchangeRate;
				   temp.ExchangeRateDate = MyEntityPM.ExchangeRateDate; 

			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService1 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService1.GetUserById(MyEntityPM.CreatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.LocalCurrencyCode = MyEntityPM.LocalCurrencyCode; 

			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService2 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService2.GetBranchById(MyEntityPM.BranchId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PaymentCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService3 = new CurrencyQueryService(Tenant);
					   					   temp.PaymentCurrency = CurrencyService3.GetCurrencyById(MyEntityPM.PaymentCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.BillToId != null)
				   {
					   CardQueryService CardService4 = new CardQueryService(Tenant);
					   					   temp.BillTo = CardService4.GetCardById(MyEntityPM.BillToId,Tenant,ComputingPartnerName); 
			       
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
					   					   temp.CreditCardType = CreditCardTypeService5.GetCreditCardTypeById(MyEntityPM.CreditCardTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.PaymentCurrencyCode = MyEntityPM.PaymentCurrencyCode;
				   temp.CreateDate = MyEntityPM.CreateDate;
				if(MyEntityPM.PaymentInvoices != null && MyEntityPM.PaymentInvoices.Count > 0)
				{
					 ARPaymentInvoiceQueryService ARPaymentInvoiceService6 = new ARPaymentInvoiceQueryService(Tenant);
					 temp.PaymentInvoices = ARPaymentInvoiceService6.ARPaymentInvoiceDataMapping(MyEntityPM.PaymentInvoices,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ARPaymentChequeReplicas != null && MyEntityPM.ARPaymentChequeReplicas.Count > 0)
				{
					 ARPaymentChequeQueryService ARPaymentChequeService6 = new ARPaymentChequeQueryService(Tenant);
					 temp.ARPaymentCheques = ARPaymentChequeService6.ARPaymentChequeDataMapping(MyEntityPM.ARPaymentChequeReplicas,Tenant,ComputingPartnerName);
				}

							 
				   temp.BankAccountNumber = MyEntityPM.BankAccountNumber;
				   temp.CancelationNotes = MyEntityPM.CancelationNotes;
				   temp.AccountingCancelationDate = MyEntityPM.AccountingCancelationDate; 

			  
				   if(MyEntityPM.StatusCode != null)
				   {
					   ARPaymentStatusQueryService ARPaymentStatusService6 = new ARPaymentStatusQueryService(Tenant);
					   					   temp.Status = ARPaymentStatusService6.GetARPaymentStatusByCode(MyEntityPM.StatusCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				if(MyEntityPM.ARPaymentBankTranfers != null && MyEntityPM.ARPaymentBankTranfers.Count > 0)
				{
					 ARPaymentBankTranferQueryService ARPaymentBankTranferService7 = new ARPaymentBankTranferQueryService(Tenant);
					 temp.ARPaymentBankTranfers = ARPaymentBankTranferService7.ARPaymentBankTranferDataMapping(MyEntityPM.ARPaymentBankTranfers,Tenant,ComputingPartnerName);
				}

							 					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ARPaymentPM ARPaymentDataMappingAndValidatin(ARPayment MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("ARPayment with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
                    
					if(!IsUpdate)
					{							
						temp.Tenant = MyEntity.Tenant;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PaymentNo = MyEntity.PaymentNo;

										}  

					
					AccountingPaymentMethodQueryService AccountingPaymentMethodAccountingPaymentMethodService = new AccountingPaymentMethodQueryService(Tenant);
					if(MyEntity.AccountingPaymentMethod != null)
					{
						var myAccountingPaymentMethodPM = AccountingPaymentMethodAccountingPaymentMethodService.AccountingPaymentMethodCustomDataMappingAndValidatin(MyEntity.AccountingPaymentMethod,Tenant);
						
						if(myAccountingPaymentMethodPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.AccountingPaymentMethodId = myAccountingPaymentMethodPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.AmountInLocalCurrency = MyEntity.AmountInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AmountInPaymentCurrency = MyEntity.AmountInPaymentCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PaidBy = MyEntity.PaidBy;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PaymentCurrencyExchangeRate = MyEntity.PaymentCurrencyExchangeRate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ExchangeRateDate = MyEntity.ExchangeRateDate;

										}  

					
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCreatedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CreatedByUserId = myCreatedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.LocalCurrencyCode = MyEntity.LocalCurrencyCode;

										}  

					
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myBranchPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.BranchId = myBranchPM.Id;
						  
							}  

							
						} 

					}
			
					
					CurrencyQueryService PaymentCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.PaymentCurrency != null)
					{
						var myPaymentCurrencyPM = PaymentCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.PaymentCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPaymentCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PaymentCurrencyId = myPaymentCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService BillToCardService = new CardQueryService(Tenant);
					if(MyEntity.BillTo != null)
					{
						var myBillToPM = BillToCardService.CardDataMappingAndValidatin(MyEntity.BillTo,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myBillToPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.BillToId = myBillToPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.ChequeOrPaymentRef = MyEntity.ChequeOrPaymentRef;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Bank = MyEntity.Bank;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.BankBranch = MyEntity.BankBranch;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Account = MyEntity.Account;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ValueDate = MyEntity.ValueDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.RegisterDate = MyEntity.RegisterDate;

										}  

					
					CreditCardTypeQueryService CreditCardTypeCreditCardTypeService = new CreditCardTypeQueryService(Tenant);
					if(MyEntity.CreditCardType != null)
					{
						var myCreditCardTypePM = CreditCardTypeCreditCardTypeService.CreditCardTypeDataMappingAndValidatin(MyEntity.CreditCardType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCreditCardTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CreditCardTypeId = myCreditCardTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.PaymentCurrencyCode = MyEntity.PaymentCurrencyCode;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CreateDate = MyEntity.CreateDate;

										}  

					 

					if(MyEntity.PaymentInvoices != null && MyEntity.PaymentInvoices.Count > 0)
					{
						ARPaymentInvoiceQueryService ARPaymentInvoiceService7 = new ARPaymentInvoiceQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.PaymentInvoices = ARPaymentInvoiceService7.ARPaymentInvoiceDataMappingAndValidatin(MyEntity.PaymentInvoices,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.ARPaymentCheques != null && MyEntity.ARPaymentCheques.Count > 0)
					{
						ARPaymentChequeQueryService ARPaymentChequeService7 = new ARPaymentChequeQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ARPaymentChequeReplicas = ARPaymentChequeService7.ARPaymentChequeDataMappingAndValidatin(MyEntity.ARPaymentCheques,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)
					{							
						temp.BankAccountNumber = MyEntity.BankAccountNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CancelationNotes = MyEntity.CancelationNotes;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AccountingCancelationDate = MyEntity.AccountingCancelationDate;

										}  

					
					ARPaymentStatusQueryService StatusARPaymentStatusService = new ARPaymentStatusQueryService(Tenant);
					if(MyEntity.Status != null)
					{
						var myStatusPM = StatusARPaymentStatusService.ARPaymentStatusDataMappingAndValidatin(MyEntity.Status,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.StatusCode = myStatusPM.Code;
						  
							}  

							
						} 

					}
			
					 

					if(MyEntity.ARPaymentBankTranfers != null && MyEntity.ARPaymentBankTranfers.Count > 0)
					{
						ARPaymentBankTranferQueryService ARPaymentBankTranferService7 = new ARPaymentBankTranferQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ARPaymentBankTranfers = ARPaymentBankTranferService7.ARPaymentBankTranferDataMappingAndValidatin(MyEntity.ARPaymentBankTranfers,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 					   
					return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }


						   
   }
}