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
					   					   temp.AccountingPaymentMethod = AccountingPaymentMethodService0.AccountingPaymentMethodCustomDataMapping(MyEntityPM.AccountingPaymentMethodId,Tenant); 
			       
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
				   temp.CreateDate = MyEntityPM.CreateDate;
				if(MyEntityPM.PaymentInvoices != null && MyEntityPM.PaymentInvoices.Count > 0)
				{
					 ARPaymentInvoiceQueryService ARPaymentInvoiceService6 = new ARPaymentInvoiceQueryService(Tenant);
					 temp.PaymentInvoices = ARPaymentInvoiceService6.ARPaymentInvoiceDataMapping(MyEntityPM.PaymentInvoices,Tenant);
				}

							 
				if(MyEntityPM.ARPaymentChequeReplicas != null && MyEntityPM.ARPaymentChequeReplicas.Count > 0)
				{
					 ARPaymentChequeQueryService ARPaymentChequeService6 = new ARPaymentChequeQueryService(Tenant);
					 temp.ARPaymentCheques = ARPaymentChequeService6.ARPaymentChequeDataMapping(MyEntityPM.ARPaymentChequeReplicas,Tenant);
				}

							 
				   temp.BankAccountNumber = MyEntityPM.BankAccountNumber;
				   temp.CancelationNotes = MyEntityPM.CancelationNotes;
				   temp.AccountingCancelationDate = MyEntityPM.AccountingCancelationDate; 

			  
				   if(MyEntityPM.StatusCode != null)
				   {
					   ARPaymentStatusQueryService ARPaymentStatusService6 = new ARPaymentStatusQueryService(Tenant);
					   					   temp.Status = ARPaymentStatusService6.GetARPaymentStatusByCode(MyEntityPM.StatusCode,Tenant); 
			       
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
                    
					if(!IsUpdate)// && MyEntity.Tenant != null)
					{							//throw new ApplicationException("Tenant Can't be update"); 
							temp.Tenant = MyEntity.Tenant;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PaymentNo))
					{							//throw new ApplicationException("PaymentNo Can't be update"); 
							temp.PaymentNo = MyEntity.PaymentNo;

										}  

					
					AccountingPaymentMethodQueryService AccountingPaymentMethodAccountingPaymentMethodService = new AccountingPaymentMethodQueryService(Tenant);
					if(MyEntity.AccountingPaymentMethod != null)
					{
						var myAccountingPaymentMethodPM = AccountingPaymentMethodAccountingPaymentMethodService.AccountingPaymentMethodCustomDataMappingAndValidatin(MyEntity.AccountingPaymentMethod,Tenant);
						
						if(myAccountingPaymentMethodPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("AccountingPaymentMethod Can't be update"); 
								temp.AccountingPaymentMethodId = myAccountingPaymentMethodPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.AmountInLocalCurrency != null)
					{							//throw new ApplicationException("AmountInLocalCurrency Can't be update"); 
							temp.AmountInLocalCurrency = MyEntity.AmountInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.AmountInPaymentCurrency != null)
					{							//throw new ApplicationException("AmountInPaymentCurrency Can't be update"); 
							temp.AmountInPaymentCurrency = MyEntity.AmountInPaymentCurrency;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PaidBy))
					{							//throw new ApplicationException("PaidBy Can't be update"); 
							temp.PaidBy = MyEntity.PaidBy;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.PaymentCurrencyExchangeRate != null)
					{							//throw new ApplicationException("PaymentCurrencyExchangeRate Can't be update"); 
							temp.PaymentCurrencyExchangeRate = MyEntity.PaymentCurrencyExchangeRate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ExchangeRateDate != null)
					{							//throw new ApplicationException("ExchangeRateDate Can't be update"); 
							temp.ExchangeRateDate = MyEntity.ExchangeRateDate;

										}  

					
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCreatedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("CreatedByUser Can't be update"); 
								temp.CreatedByUserId = myCreatedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.LocalCurrencyCode))
					{							//throw new ApplicationException("LocalCurrencyCode Can't be update"); 
							temp.LocalCurrencyCode = MyEntity.LocalCurrencyCode;

										}  

					
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myBranchPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Branch Can't be update"); 
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
							{								//throw new ApplicationException("PaymentCurrency Can't be update"); 
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
							{								//throw new ApplicationException("BillTo Can't be update"); 
								temp.BillToId = myBillToPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ChequeOrPaymentRef))
					{							//throw new ApplicationException("ChequeOrPaymentRef Can't be update"); 
							temp.ChequeOrPaymentRef = MyEntity.ChequeOrPaymentRef;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Bank))
					{							//throw new ApplicationException("Bank Can't be update"); 
							temp.Bank = MyEntity.Bank;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.BankBranch))
					{							//throw new ApplicationException("BankBranch Can't be update"); 
							temp.BankBranch = MyEntity.BankBranch;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Account))
					{							//throw new ApplicationException("Account Can't be update"); 
							temp.Account = MyEntity.Account;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ValueDate != null)
					{							//throw new ApplicationException("ValueDate Can't be update"); 
							temp.ValueDate = MyEntity.ValueDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.RegisterDate != null)
					{							//throw new ApplicationException("RegisterDate Can't be update"); 
							temp.RegisterDate = MyEntity.RegisterDate;

										}  

					
					CreditCardTypeQueryService CreditCardTypeCreditCardTypeService = new CreditCardTypeQueryService(Tenant);
					if(MyEntity.CreditCardType != null)
					{
						var myCreditCardTypePM = CreditCardTypeCreditCardTypeService.CreditCardTypeDataMappingAndValidatin(MyEntity.CreditCardType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCreditCardTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("CreditCardType Can't be update"); 
								temp.CreditCardTypeId = myCreditCardTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PaymentCurrencyCode))
					{							//throw new ApplicationException("PaymentCurrencyCode Can't be update"); 
							temp.PaymentCurrencyCode = MyEntity.PaymentCurrencyCode;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.CreateDate != null)
					{							//throw new ApplicationException("CreateDate Can't be update"); 
							temp.CreateDate = MyEntity.CreateDate;

										}  

					 

					if(MyEntity.PaymentInvoices != null && MyEntity.PaymentInvoices.Count > 0)
					{
						ARPaymentInvoiceQueryService ARPaymentInvoiceService7 = new ARPaymentInvoiceQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("PaymentInvoices Can't be update"); 
								temp.PaymentInvoices = ARPaymentInvoiceService7.ARPaymentInvoiceDataMappingAndValidatin(MyEntity.PaymentInvoices,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.ARPaymentCheques != null && MyEntity.ARPaymentCheques.Count > 0)
					{
						ARPaymentChequeQueryService ARPaymentChequeService7 = new ARPaymentChequeQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("ARPaymentCheques Can't be update"); 
								temp.ARPaymentChequeReplicas = ARPaymentChequeService7.ARPaymentChequeDataMappingAndValidatin(MyEntity.ARPaymentCheques,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.BankAccountNumber))
					{							//throw new ApplicationException("BankAccountNumber Can't be update"); 
							temp.BankAccountNumber = MyEntity.BankAccountNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.CancelationNotes))
					{							//throw new ApplicationException("CancelationNotes Can't be update"); 
							temp.CancelationNotes = MyEntity.CancelationNotes;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.AccountingCancelationDate != null)
					{							//throw new ApplicationException("AccountingCancelationDate Can't be update"); 
							temp.AccountingCancelationDate = MyEntity.AccountingCancelationDate;

										}  

					
					ARPaymentStatusQueryService StatusARPaymentStatusService = new ARPaymentStatusQueryService(Tenant);
					if(MyEntity.Status != null)
					{
						var myStatusPM = StatusARPaymentStatusService.ARPaymentStatusDataMappingAndValidatin(MyEntity.Status,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Status Can't be update"); 
								temp.StatusCode = myStatusPM.Code;
						  
							}  

							
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