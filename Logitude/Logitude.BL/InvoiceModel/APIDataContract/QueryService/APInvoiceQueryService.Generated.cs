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
   public partial class APInvoiceQueryService
   {
   
		IInvoiceContext  context;
		//APInvoiceService service; 
		
		APInvoiceQuery query; 

        public APInvoiceQueryService(int tenant)
        {
				    context = InvoiceContext.GetContext(tenant); 
			//service = new APInvoiceService(context, tenant); 
			query = new APInvoiceQuery(tenant);
        }

		
		public APInvoice GetAPInvoiceById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("APInvoice with Id " + Id + " doesn't exist");

				return APInvoiceDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public APInvoice APInvoiceDataMapping(APInvoicePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new APInvoice(); 
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.InternalNumber = MyEntityPM.InternalNumber; 

			  
				   if(MyEntityPM.VendorId != null)
				   {
					   VendorQueryService VendorService0 = new VendorQueryService(Tenant);
					   					   temp.Vendor = VendorService0.GetVendorById(MyEntityPM.VendorId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.VATNumber = MyEntityPM.VATNumber;
				   temp.InvoiceNumber = MyEntityPM.InvoiceNumber; 

			  
				   if(MyEntityPM.InvoiceCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
					   					   temp.InvoiceCurrency = CurrencyService1.GetCurrencyById(MyEntityPM.InvoiceCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.InvoiceCurrencyExchangeRate = MyEntityPM.InvoiceCurrencyExchangeRate;
				   temp.InvoiceDate = MyEntityPM.InvoiceDate;
				   temp.AccountingDate = MyEntityPM.AccountingDate; 

			  
				   if(MyEntityPM.PaymentTermId != null)
				   {
					   PaymentTermQueryService PaymentTermService2 = new PaymentTermQueryService(Tenant);
					   					   temp.PaymentTerm = PaymentTermService2.GetPaymentTermById(MyEntityPM.PaymentTermId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.DueDate = MyEntityPM.DueDate;
				   temp.ExchangeRateDate = MyEntityPM.ExchangeRateDate; 

			  
				   if(MyEntityPM.LocalCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService3 = new CurrencyQueryService(Tenant);
					   					   temp.LocalCurrency = CurrencyService3.GetCurrencyById(MyEntityPM.LocalCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.InternalNotes = MyEntityPM.InternalNotes;
				   temp.SubTotalInLocalCurrency = MyEntityPM.SubTotalInLocalCurrency;
				   temp.SubTotalInInvoiceCurrency = MyEntityPM.SubTotalInInvoiceCurrency;
				   temp.AmountInLocalCurrency = MyEntityPM.AmountInLocalCurrency; 

			  
				   if(MyEntityPM.StatusCode != null)
				   {
					   APInvoiceStatusQueryService APInvoiceStatusService4 = new APInvoiceStatusQueryService(Tenant);
					   					   temp.Status = APInvoiceStatusService4.GetAPInvoiceStatusByCode(MyEntityPM.StatusCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ProfitCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService5 = new CurrencyQueryService(Tenant);
					   					   temp.ProfitCurrency = CurrencyService5.GetCurrencyById(MyEntityPM.ProfitCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ProfitCurrencyExchangeRate = MyEntityPM.ProfitCurrencyExchangeRate;
				   temp.AmountInProfitCurrency = MyEntityPM.AmountInProfitCurrency; 

			  
				   if(MyEntityPM.UpdatedByUserId != null)
				   {
					   UserQueryService UserService6 = new UserQueryService(Tenant);
					   					   temp.UpdatedByUser = UserService6.GetUserById(MyEntityPM.UpdatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.UpdateDate = MyEntityPM.UpdateDate;
				   temp.AmountDue = MyEntityPM.AmountDue;
				   temp.AmountDueInLocalCurrency = MyEntityPM.AmountDueInLocalCurrency;
				   temp.AmountDueInProfitCurrency = MyEntityPM.AmountDueInProfitCurrency;
				   temp.RefundAmount = MyEntityPM.RefundAmount; 

			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService7 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService7.GetBranchById(MyEntityPM.BranchId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.HouseNumber = MyEntityPM.HouseNumber;
				   temp.MasterNumber = MyEntityPM.MasterNumber;
				   temp.Description = MyEntityPM.Description;
				   temp.AccountingExternalCode = MyEntityPM.AccountingExternalCode;
				   temp.CreditAccount = MyEntityPM.CreditAccount;
				   temp.PaymentTermExternalId = MyEntityPM.PaymentTermExternalId; 

			  
				   if(MyEntityPM.TransferStatusCode != null)
				   {
					   APInvoiceTransferStatusQueryService APInvoiceTransferStatusService8 = new APInvoiceTransferStatusQueryService(Tenant);
					   					   temp.TransferStatus = APInvoiceTransferStatusService8.GetAPInvoiceTransferStatusByCode(MyEntityPM.TransferStatusCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ApprovedDate = MyEntityPM.ApprovedDate; 

			  
				   if(MyEntityPM.ApprovedByUserId != null)
				   {
					   UserQueryService UserService9 = new UserQueryService(Tenant);
					   					   temp.ApprovedByUser = UserService9.GetUserById(MyEntityPM.ApprovedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.IsExternalEntity = MyEntityPM.IsExternalEntity;
				   temp.IsGeneralInvoice = MyEntityPM.IsGeneralInvoice;
				   temp.ExternalAccountingEntityId = MyEntityPM.ExternalAccountingEntityId;
				   temp.Id = MyEntityPM.Id;
				if(MyEntityPM.InvoiceLines != null && MyEntityPM.InvoiceLines.Count > 0)
				{
					 APInvoiceLineQueryService APInvoiceLineService10 = new APInvoiceLineQueryService(Tenant);
					 temp.InvoiceLines = APInvoiceLineService10.APInvoiceLineDataMapping(MyEntityPM.InvoiceLines,Tenant,ComputingPartnerName);
				}

							 
				   temp.AmountInInvoiceCurrency = MyEntityPM.AmountInInvoiceCurrency;
				   temp.InvoiceExpectedAmount = MyEntityPM.InvoiceExpectedAmount;
				   temp.EntityReference = MyEntityPM.MainEntityReference;
				   temp.VendorGLAccount = MyEntityPM.VendorGLAccountId;
				if(MyEntityPM.TotalVATs != null && MyEntityPM.TotalVATs.Count > 0)
				{
					 APInvoiceTotalVATQueryService APInvoiceTotalVATService10 = new APInvoiceTotalVATQueryService(Tenant);
					 temp.TotalVATs = APInvoiceTotalVATService10.APInvoiceTotalVATDataMapping(MyEntityPM.TotalVATs,Tenant,ComputingPartnerName);
				}

							 
				   temp.TotalVATOnly = MyEntityPM.TotalVATOnly;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public APInvoicePM APInvoiceDataMappingAndValidatin(APInvoice MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new APInvoicePM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("APInvoice with Id " + MyEntity.Id + " doesn't exist");
					} 
				 
										 
					if(IsUpdate == true)
					{
					    
					      temp.NewConcurrencyGUID = Guid.NewGuid().ToString(); 
						
					}
                    
					if(!IsUpdate)
					{							
						temp.Tenant = MyEntity.Tenant;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.InternalNumber = MyEntity.InternalNumber;

										}  

					
					VendorQueryService VendorVendorService = new VendorQueryService(Tenant);
					if(MyEntity.Vendor != null)
					{
						var myVendorPM = VendorVendorService.VendorDataMappingAndValidatin(MyEntity.Vendor,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myVendorPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.VendorId = myVendorPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.VATNumber = MyEntity.VATNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.InvoiceNumber = MyEntity.InvoiceNumber;

										}  

					
					CurrencyQueryService InvoiceCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.InvoiceCurrency != null)
					{
						var myInvoiceCurrencyPM = InvoiceCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.InvoiceCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myInvoiceCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.InvoiceCurrencyId = myInvoiceCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.InvoiceCurrencyExchangeRate = MyEntity.InvoiceCurrencyExchangeRate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.InvoiceDate = MyEntity.InvoiceDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AccountingDate = MyEntity.AccountingDate;

										}  

					
					PaymentTermQueryService PaymentTermPaymentTermService = new PaymentTermQueryService(Tenant);
					if(MyEntity.PaymentTerm != null)
					{
						var myPaymentTermPM = PaymentTermPaymentTermService.PaymentTermDataMappingAndValidatin(MyEntity.PaymentTerm,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPaymentTermPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PaymentTermId = myPaymentTermPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.DueDate = MyEntity.DueDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ExchangeRateDate = MyEntity.ExchangeRateDate;

										}  

					
					CurrencyQueryService LocalCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.LocalCurrency != null)
					{
						var myLocalCurrencyPM = LocalCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.LocalCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLocalCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.LocalCurrencyId = myLocalCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.InternalNotes = MyEntity.InternalNotes;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SubTotalInLocalCurrency = MyEntity.SubTotalInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SubTotalInInvoiceCurrency = MyEntity.SubTotalInInvoiceCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AmountInLocalCurrency = MyEntity.AmountInLocalCurrency;

										}  

					
					APInvoiceStatusQueryService StatusAPInvoiceStatusService = new APInvoiceStatusQueryService(Tenant);
					if(MyEntity.Status != null)
					{
						var myStatusPM = StatusAPInvoiceStatusService.APInvoiceStatusDataMappingAndValidatin(MyEntity.Status,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.StatusCode = myStatusPM.Code;
						  
							}  

							
						} 

					}
			
					
					CurrencyQueryService ProfitCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.ProfitCurrency != null)
					{
						var myProfitCurrencyPM = ProfitCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.ProfitCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myProfitCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ProfitCurrencyId = myProfitCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.ProfitCurrencyExchangeRate = MyEntity.ProfitCurrencyExchangeRate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AmountInProfitCurrency = MyEntity.AmountInProfitCurrency;

										}  

					
					UserQueryService UpdatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.UpdatedByUser != null)
					{
						var myUpdatedByUserPM = UpdatedByUserUserService.UserDataMappingAndValidatin(MyEntity.UpdatedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myUpdatedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.UpdatedByUserId = myUpdatedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.UpdateDate = MyEntity.UpdateDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AmountDue = MyEntity.AmountDue;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AmountDueInLocalCurrency = MyEntity.AmountDueInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AmountDueInProfitCurrency = MyEntity.AmountDueInProfitCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.RefundAmount = MyEntity.RefundAmount;

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
			
					
                    
					if(!IsUpdate)
					{							
						temp.HouseNumber = MyEntity.HouseNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.MasterNumber = MyEntity.MasterNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Description = MyEntity.Description;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AccountingExternalCode = MyEntity.AccountingExternalCode;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CreditAccount = MyEntity.CreditAccount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PaymentTermExternalId = MyEntity.PaymentTermExternalId;

										}  

					
					APInvoiceTransferStatusQueryService TransferStatusAPInvoiceTransferStatusService = new APInvoiceTransferStatusQueryService(Tenant);
					if(MyEntity.TransferStatus != null)
					{
						var myTransferStatusPM = TransferStatusAPInvoiceTransferStatusService.APInvoiceTransferStatusDataMappingAndValidatin(MyEntity.TransferStatus,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransferStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.TransferStatusCode = myTransferStatusPM.Code;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.ApprovedDate = MyEntity.ApprovedDate;

										}  

					
					UserQueryService ApprovedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.ApprovedByUser != null)
					{
						var myApprovedByUserPM = ApprovedByUserUserService.UserDataMappingAndValidatin(MyEntity.ApprovedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myApprovedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ApprovedByUserId = myApprovedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.IsExternalEntity = MyEntity.IsExternalEntity;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsGeneralInvoice = MyEntity.IsGeneralInvoice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ExternalAccountingEntityId = MyEntity.ExternalAccountingEntityId;

										}  

					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("APInvoice with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					} 

					if(MyEntity.InvoiceLines != null && MyEntity.InvoiceLines.Count > 0)
					{
						APInvoiceLineQueryService APInvoiceLineService10 = new APInvoiceLineQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.InvoiceLines = APInvoiceLineService10.APInvoiceLineDataMappingAndValidatin(MyEntity.InvoiceLines,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)
					{							
						temp.AmountInInvoiceCurrency = MyEntity.AmountInInvoiceCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.InvoiceExpectedAmount = MyEntity.InvoiceExpectedAmount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.MainEntityReference = MyEntity.EntityReference;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.VendorGLAccountId = MyEntity.VendorGLAccount;

										}  

					 

					if(MyEntity.TotalVATs != null && MyEntity.TotalVATs.Count > 0)
					{
						APInvoiceTotalVATQueryService APInvoiceTotalVATService10 = new APInvoiceTotalVATQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.TotalVATs = APInvoiceTotalVATService10.APInvoiceTotalVATDataMappingAndValidatin(MyEntity.TotalVATs,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)
					{							
						temp.TotalVATOnly = MyEntity.TotalVATOnly;

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