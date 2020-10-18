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

		
		public APInvoice GetAPInvoiceById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("APInvoice with Id " + Id + " doesn't exist");

				return APInvoiceDataMapping(temp,Tenant);
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
					   					   temp.Vendor = VendorService0.GetVendorById(MyEntityPM.VendorId,Tenant); 
			       
					   				   }
				   
				   temp.VATNumber = MyEntityPM.VATNumber;
				   temp.InvoiceNumber = MyEntityPM.InvoiceNumber; 

			  
				   if(MyEntityPM.InvoiceCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
					   					   temp.InvoiceCurrency = CurrencyService1.GetCurrencyById(MyEntityPM.InvoiceCurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.InvoiceCurrencyExchangeRate = MyEntityPM.InvoiceCurrencyExchangeRate;
				   temp.InvoiceDate = MyEntityPM.InvoiceDate;
				   temp.AccountingDate = MyEntityPM.AccountingDate; 

			  
				   if(MyEntityPM.PaymentTermId != null)
				   {
					   PaymentTermQueryService PaymentTermService2 = new PaymentTermQueryService(Tenant);
					   					   temp.PaymentTerm = PaymentTermService2.GetPaymentTermById(MyEntityPM.PaymentTermId,Tenant); 
			       
					   				   }
				   
				   temp.DueDate = MyEntityPM.DueDate;
				   temp.ExchangeRateDate = MyEntityPM.ExchangeRateDate; 

			  
				   if(MyEntityPM.LocalCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService3 = new CurrencyQueryService(Tenant);
					   					   temp.LocalCurrency = CurrencyService3.GetCurrencyById(MyEntityPM.LocalCurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.InternalNotes = MyEntityPM.InternalNotes;
				   temp.SubTotalInLocalCurrency = MyEntityPM.SubTotalInLocalCurrency;
				   temp.SubTotalInInvoiceCurrency = MyEntityPM.SubTotalInInvoiceCurrency;
				   temp.AmountInLocalCurrency = MyEntityPM.AmountInLocalCurrency; 

			  
				   if(MyEntityPM.StatusCode != null)
				   {
					   APInvoiceStatusQueryService APInvoiceStatusService4 = new APInvoiceStatusQueryService(Tenant);
					   					   temp.Status = APInvoiceStatusService4.GetAPInvoiceStatusByCode(MyEntityPM.StatusCode,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ProfitCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService5 = new CurrencyQueryService(Tenant);
					   					   temp.ProfitCurrency = CurrencyService5.GetCurrencyById(MyEntityPM.ProfitCurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.ProfitCurrencyExchangeRate = MyEntityPM.ProfitCurrencyExchangeRate;
				   temp.AmountInProfitCurrency = MyEntityPM.AmountInProfitCurrency; 

			  
				   if(MyEntityPM.UpdatedByUserId != null)
				   {
					   UserQueryService UserService6 = new UserQueryService(Tenant);
					   					   temp.UpdatedByUser = UserService6.GetUserById(MyEntityPM.UpdatedByUserId,Tenant); 
			       
					   				   }
				   
				   temp.UpdateDate = MyEntityPM.UpdateDate;
				   temp.AmountDue = MyEntityPM.AmountDue;
				   temp.AmountDueInLocalCurrency = MyEntityPM.AmountDueInLocalCurrency;
				   temp.AmountDueInProfitCurrency = MyEntityPM.AmountDueInProfitCurrency;
				   temp.RefundAmount = MyEntityPM.RefundAmount; 

			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService7 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService7.GetBranchById(MyEntityPM.BranchId,Tenant); 
			       
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
					   					   temp.TransferStatus = APInvoiceTransferStatusService8.GetAPInvoiceTransferStatusByCode(MyEntityPM.TransferStatusCode,Tenant); 
			       
					   				   }
				   
				   temp.ApprovedDate = MyEntityPM.ApprovedDate; 

			  
				   if(MyEntityPM.ApprovedByUserId != null)
				   {
					   UserQueryService UserService9 = new UserQueryService(Tenant);
					   					   temp.ApprovedByUser = UserService9.GetUserById(MyEntityPM.ApprovedByUserId,Tenant); 
			       
					   				   }
				   
				   temp.IsExternalEntity = MyEntityPM.IsExternalEntity;
				   temp.IsGeneralInvoice = MyEntityPM.IsGeneralInvoice;
				   temp.ExternalAccountingEntityId = MyEntityPM.ExternalAccountingEntityId;
				   temp.Id = MyEntityPM.Id;
				if(MyEntityPM.InvoiceLines != null && MyEntityPM.InvoiceLines.Count > 0)
				{
					 APInvoiceLineQueryService APInvoiceLineService10 = new APInvoiceLineQueryService(Tenant);
					 temp.InvoiceLines = APInvoiceLineService10.APInvoiceLineDataMapping(MyEntityPM.InvoiceLines,Tenant);
				}

							 
				   temp.AmountInInvoiceCurrency = MyEntityPM.AmountInInvoiceCurrency;
				   temp.InvoiceExpectedAmount = MyEntityPM.InvoiceExpectedAmount;
				   temp.EntityReference = MyEntityPM.MainEntityReference;
				   temp.VendorGLAccount = MyEntityPM.VendorGLAccountId;
				if(MyEntityPM.TotalVATs != null && MyEntityPM.TotalVATs.Count > 0)
				{
					 APInvoiceTotalVATQueryService APInvoiceTotalVATService10 = new APInvoiceTotalVATQueryService(Tenant);
					 temp.TotalVATs = APInvoiceTotalVATService10.APInvoiceTotalVATDataMapping(MyEntityPM.TotalVATs,Tenant);
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
					
                    
					if(!IsUpdate)// && MyEntity.Tenant != null)
					{							//throw new ApplicationException("Tenant Can't be update"); 
							temp.Tenant = MyEntity.Tenant;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.InternalNumber))
					{							//throw new ApplicationException("InternalNumber Can't be update"); 
							temp.InternalNumber = MyEntity.InternalNumber;

										}  

					
					VendorQueryService VendorVendorService = new VendorQueryService(Tenant);
					if(MyEntity.Vendor != null)
					{
						var myVendorPM = VendorVendorService.VendorDataMappingAndValidatin(MyEntity.Vendor,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myVendorPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Vendor Can't be update"); 
								temp.VendorId = myVendorPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.VATNumber))
					{							//throw new ApplicationException("VATNumber Can't be update"); 
							temp.VATNumber = MyEntity.VATNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.InvoiceNumber))
					{							//throw new ApplicationException("InvoiceNumber Can't be update"); 
							temp.InvoiceNumber = MyEntity.InvoiceNumber;

										}  

					
					CurrencyQueryService InvoiceCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.InvoiceCurrency != null)
					{
						var myInvoiceCurrencyPM = InvoiceCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.InvoiceCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myInvoiceCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("InvoiceCurrency Can't be update"); 
								temp.InvoiceCurrencyId = myInvoiceCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.InvoiceCurrencyExchangeRate != null)
					{							//throw new ApplicationException("InvoiceCurrencyExchangeRate Can't be update"); 
							temp.InvoiceCurrencyExchangeRate = MyEntity.InvoiceCurrencyExchangeRate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.InvoiceDate != null)
					{							//throw new ApplicationException("InvoiceDate Can't be update"); 
							temp.InvoiceDate = MyEntity.InvoiceDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.AccountingDate != null)
					{							//throw new ApplicationException("AccountingDate Can't be update"); 
							temp.AccountingDate = MyEntity.AccountingDate;

										}  

					
					PaymentTermQueryService PaymentTermPaymentTermService = new PaymentTermQueryService(Tenant);
					if(MyEntity.PaymentTerm != null)
					{
						var myPaymentTermPM = PaymentTermPaymentTermService.PaymentTermDataMappingAndValidatin(MyEntity.PaymentTerm,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPaymentTermPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PaymentTerm Can't be update"); 
								temp.PaymentTermId = myPaymentTermPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.DueDate != null)
					{							//throw new ApplicationException("DueDate Can't be update"); 
							temp.DueDate = MyEntity.DueDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ExchangeRateDate != null)
					{							//throw new ApplicationException("ExchangeRateDate Can't be update"); 
							temp.ExchangeRateDate = MyEntity.ExchangeRateDate;

										}  

					
					CurrencyQueryService LocalCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.LocalCurrency != null)
					{
						var myLocalCurrencyPM = LocalCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.LocalCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLocalCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("LocalCurrency Can't be update"); 
								temp.LocalCurrencyId = myLocalCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.InternalNotes))
					{							//throw new ApplicationException("InternalNotes Can't be update"); 
							temp.InternalNotes = MyEntity.InternalNotes;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.SubTotalInLocalCurrency != null)
					{							//throw new ApplicationException("SubTotalInLocalCurrency Can't be update"); 
							temp.SubTotalInLocalCurrency = MyEntity.SubTotalInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.SubTotalInInvoiceCurrency != null)
					{							//throw new ApplicationException("SubTotalInInvoiceCurrency Can't be update"); 
							temp.SubTotalInInvoiceCurrency = MyEntity.SubTotalInInvoiceCurrency;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.AmountInLocalCurrency != null)
					{							//throw new ApplicationException("AmountInLocalCurrency Can't be update"); 
							temp.AmountInLocalCurrency = MyEntity.AmountInLocalCurrency;

										}  

					
					APInvoiceStatusQueryService StatusAPInvoiceStatusService = new APInvoiceStatusQueryService(Tenant);
					if(MyEntity.Status != null)
					{
						var myStatusPM = StatusAPInvoiceStatusService.APInvoiceStatusDataMappingAndValidatin(MyEntity.Status,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Status Can't be update"); 
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
							{								//throw new ApplicationException("ProfitCurrency Can't be update"); 
								temp.ProfitCurrencyId = myProfitCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.ProfitCurrencyExchangeRate != null)
					{							//throw new ApplicationException("ProfitCurrencyExchangeRate Can't be update"); 
							temp.ProfitCurrencyExchangeRate = MyEntity.ProfitCurrencyExchangeRate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.AmountInProfitCurrency != null)
					{							//throw new ApplicationException("AmountInProfitCurrency Can't be update"); 
							temp.AmountInProfitCurrency = MyEntity.AmountInProfitCurrency;

										}  

					
					UserQueryService UpdatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.UpdatedByUser != null)
					{
						var myUpdatedByUserPM = UpdatedByUserUserService.UserDataMappingAndValidatin(MyEntity.UpdatedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myUpdatedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("UpdatedByUser Can't be update"); 
								temp.UpdatedByUserId = myUpdatedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.UpdateDate != null)
					{							//throw new ApplicationException("UpdateDate Can't be update"); 
							temp.UpdateDate = MyEntity.UpdateDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.AmountDue != null)
					{							//throw new ApplicationException("AmountDue Can't be update"); 
							temp.AmountDue = MyEntity.AmountDue;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.AmountDueInLocalCurrency != null)
					{							//throw new ApplicationException("AmountDueInLocalCurrency Can't be update"); 
							temp.AmountDueInLocalCurrency = MyEntity.AmountDueInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.AmountDueInProfitCurrency != null)
					{							//throw new ApplicationException("AmountDueInProfitCurrency Can't be update"); 
							temp.AmountDueInProfitCurrency = MyEntity.AmountDueInProfitCurrency;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.RefundAmount != null)
					{							//throw new ApplicationException("RefundAmount Can't be update"); 
							temp.RefundAmount = MyEntity.RefundAmount;

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
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.HouseNumber))
					{							//throw new ApplicationException("HouseNumber Can't be update"); 
							temp.HouseNumber = MyEntity.HouseNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.MasterNumber))
					{							//throw new ApplicationException("MasterNumber Can't be update"); 
							temp.MasterNumber = MyEntity.MasterNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Description))
					{							//throw new ApplicationException("Description Can't be update"); 
							temp.Description = MyEntity.Description;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.AccountingExternalCode))
					{							//throw new ApplicationException("AccountingExternalCode Can't be update"); 
							temp.AccountingExternalCode = MyEntity.AccountingExternalCode;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.CreditAccount))
					{							//throw new ApplicationException("CreditAccount Can't be update"); 
							temp.CreditAccount = MyEntity.CreditAccount;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PaymentTermExternalId))
					{							//throw new ApplicationException("PaymentTermExternalId Can't be update"); 
							temp.PaymentTermExternalId = MyEntity.PaymentTermExternalId;

										}  

					
					APInvoiceTransferStatusQueryService TransferStatusAPInvoiceTransferStatusService = new APInvoiceTransferStatusQueryService(Tenant);
					if(MyEntity.TransferStatus != null)
					{
						var myTransferStatusPM = TransferStatusAPInvoiceTransferStatusService.APInvoiceTransferStatusDataMappingAndValidatin(MyEntity.TransferStatus,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransferStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("TransferStatus Can't be update"); 
								temp.TransferStatusCode = myTransferStatusPM.Code;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.ApprovedDate != null)
					{							//throw new ApplicationException("ApprovedDate Can't be update"); 
							temp.ApprovedDate = MyEntity.ApprovedDate;

										}  

					
					UserQueryService ApprovedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.ApprovedByUser != null)
					{
						var myApprovedByUserPM = ApprovedByUserUserService.UserDataMappingAndValidatin(MyEntity.ApprovedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myApprovedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ApprovedByUser Can't be update"); 
								temp.ApprovedByUserId = myApprovedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && (MyEntity.IsExternalEntity != temp.IsExternalEntity))
					{							//throw new ApplicationException("IsExternalEntity Can't be update"); 
							temp.IsExternalEntity = MyEntity.IsExternalEntity;

										}  

					
                    
					if(!IsUpdate)// && (MyEntity.IsGeneralInvoice != temp.IsGeneralInvoice))
					{							//throw new ApplicationException("IsGeneralInvoice Can't be update"); 
							temp.IsGeneralInvoice = MyEntity.IsGeneralInvoice;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ExternalAccountingEntityId))
					{							//throw new ApplicationException("ExternalAccountingEntityId Can't be update"); 
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
						{								//throw new ApplicationException("InvoiceLines Can't be update"); 
								temp.InvoiceLines = APInvoiceLineService10.APInvoiceLineDataMappingAndValidatin(MyEntity.InvoiceLines,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)// && MyEntity.AmountInInvoiceCurrency != null)
					{							//throw new ApplicationException("AmountInInvoiceCurrency Can't be update"); 
							temp.AmountInInvoiceCurrency = MyEntity.AmountInInvoiceCurrency;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.InvoiceExpectedAmount != null)
					{							//throw new ApplicationException("InvoiceExpectedAmount Can't be update"); 
							temp.InvoiceExpectedAmount = MyEntity.InvoiceExpectedAmount;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.EntityReference))
					{							//throw new ApplicationException("EntityReference Can't be update"); 
							temp.MainEntityReference = MyEntity.EntityReference;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.VendorGLAccount))
					{							//throw new ApplicationException("VendorGLAccount Can't be update"); 
							temp.VendorGLAccountId = MyEntity.VendorGLAccount;

										}  

					 

					if(MyEntity.TotalVATs != null && MyEntity.TotalVATs.Count > 0)
					{
						APInvoiceTotalVATQueryService APInvoiceTotalVATService10 = new APInvoiceTotalVATQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("TotalVATs Can't be update"); 
								temp.TotalVATs = APInvoiceTotalVATService10.APInvoiceTotalVATDataMappingAndValidatin(MyEntity.TotalVATs,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)// && (MyEntity.TotalVATOnly != temp.TotalVATOnly))
					{							//throw new ApplicationException("TotalVATOnly Can't be update"); 
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