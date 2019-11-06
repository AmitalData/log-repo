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
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public APInvoicePM APInvoiceDataMappingAndValidatin(APInvoice MyEntity,int Tenant,string ComputingPartnerName = "")
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
					temp.Tenant = MyEntity.Tenant;
					temp.InternalNumber = MyEntity.InternalNumber;
					VendorQueryService VendorVendorService = new VendorQueryService(Tenant);
					if(MyEntity.Vendor != null)
					{
						var myVendorPM = VendorVendorService.VendorDataMappingAndValidatin(MyEntity.Vendor,Tenant,ComputingPartnerName);
												if(myVendorPM != null)
						{
							temp.VendorId = myVendorPM.Id;
						}
						 
					}
			
					
					temp.VATNumber = MyEntity.VATNumber;
					temp.InvoiceNumber = MyEntity.InvoiceNumber;
					CurrencyQueryService InvoiceCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.InvoiceCurrency != null)
					{
						var myInvoiceCurrencyPM = InvoiceCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.InvoiceCurrency,Tenant,ComputingPartnerName);
												if(myInvoiceCurrencyPM != null)
						{
							temp.InvoiceCurrencyId = myInvoiceCurrencyPM.Id;
						}
						 
					}
			
					
					temp.InvoiceCurrencyExchangeRate = MyEntity.InvoiceCurrencyExchangeRate;
					temp.InvoiceDate = MyEntity.InvoiceDate;
					temp.AccountingDate = MyEntity.AccountingDate;
					PaymentTermQueryService PaymentTermPaymentTermService = new PaymentTermQueryService(Tenant);
					if(MyEntity.PaymentTerm != null)
					{
						var myPaymentTermPM = PaymentTermPaymentTermService.PaymentTermDataMappingAndValidatin(MyEntity.PaymentTerm,Tenant,ComputingPartnerName);
												if(myPaymentTermPM != null)
						{
							temp.PaymentTermId = myPaymentTermPM.Id;
						}
						 
					}
			
					
					temp.DueDate = MyEntity.DueDate;
					temp.ExchangeRateDate = MyEntity.ExchangeRateDate;
					CurrencyQueryService LocalCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.LocalCurrency != null)
					{
						var myLocalCurrencyPM = LocalCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.LocalCurrency,Tenant,ComputingPartnerName);
												if(myLocalCurrencyPM != null)
						{
							temp.LocalCurrencyId = myLocalCurrencyPM.Id;
						}
						 
					}
			
					
					temp.InternalNotes = MyEntity.InternalNotes;
					temp.SubTotalInLocalCurrency = MyEntity.SubTotalInLocalCurrency;
					temp.SubTotalInInvoiceCurrency = MyEntity.SubTotalInInvoiceCurrency;
					temp.AmountInLocalCurrency = MyEntity.AmountInLocalCurrency;
					APInvoiceStatusQueryService StatusAPInvoiceStatusService = new APInvoiceStatusQueryService(Tenant);
					if(MyEntity.Status != null)
					{
						var myStatusPM = StatusAPInvoiceStatusService.APInvoiceStatusDataMappingAndValidatin(MyEntity.Status,Tenant,ComputingPartnerName);
												if(myStatusPM != null)
						{
							temp.StatusCode = myStatusPM.Code;
						}
						 
					}
			
					
					CurrencyQueryService ProfitCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.ProfitCurrency != null)
					{
						var myProfitCurrencyPM = ProfitCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.ProfitCurrency,Tenant,ComputingPartnerName);
												if(myProfitCurrencyPM != null)
						{
							temp.ProfitCurrencyId = myProfitCurrencyPM.Id;
						}
						 
					}
			
					
					temp.ProfitCurrencyExchangeRate = MyEntity.ProfitCurrencyExchangeRate;
					temp.AmountInProfitCurrency = MyEntity.AmountInProfitCurrency;
					UserQueryService UpdatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.UpdatedByUser != null)
					{
						var myUpdatedByUserPM = UpdatedByUserUserService.UserDataMappingAndValidatin(MyEntity.UpdatedByUser,Tenant,ComputingPartnerName);
												if(myUpdatedByUserPM != null)
						{
							temp.UpdatedByUserId = myUpdatedByUserPM.Id;
						}
						 
					}
			
					
					temp.UpdateDate = MyEntity.UpdateDate;
					temp.AmountDue = MyEntity.AmountDue;
					temp.AmountDueInLocalCurrency = MyEntity.AmountDueInLocalCurrency;
					temp.AmountDueInProfitCurrency = MyEntity.AmountDueInProfitCurrency;
					temp.RefundAmount = MyEntity.RefundAmount;
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName);
												if(myBranchPM != null)
						{
							temp.BranchId = myBranchPM.Id;
						}
						 
					}
			
					
					temp.HouseNumber = MyEntity.HouseNumber;
					temp.MasterNumber = MyEntity.MasterNumber;
					temp.Description = MyEntity.Description;
					temp.AccountingExternalCode = MyEntity.AccountingExternalCode;
					temp.CreditAccount = MyEntity.CreditAccount;
					temp.PaymentTermExternalId = MyEntity.PaymentTermExternalId;
					APInvoiceTransferStatusQueryService TransferStatusAPInvoiceTransferStatusService = new APInvoiceTransferStatusQueryService(Tenant);
					if(MyEntity.TransferStatus != null)
					{
						var myTransferStatusPM = TransferStatusAPInvoiceTransferStatusService.APInvoiceTransferStatusDataMappingAndValidatin(MyEntity.TransferStatus,Tenant,ComputingPartnerName);
												if(myTransferStatusPM != null)
						{
							temp.TransferStatusCode = myTransferStatusPM.Code;
						}
						 
					}
			
					
					temp.ApprovedDate = MyEntity.ApprovedDate;
					UserQueryService ApprovedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.ApprovedByUser != null)
					{
						var myApprovedByUserPM = ApprovedByUserUserService.UserDataMappingAndValidatin(MyEntity.ApprovedByUser,Tenant,ComputingPartnerName);
												if(myApprovedByUserPM != null)
						{
							temp.ApprovedByUserId = myApprovedByUserPM.Id;
						}
						 
					}
			
					
					temp.IsExternalEntity = MyEntity.IsExternalEntity;
					temp.IsGeneralInvoice = MyEntity.IsGeneralInvoice;
					temp.ExternalAccountingEntityId = MyEntity.ExternalAccountingEntityId;
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
						temp.InvoiceLines = APInvoiceLineService10.APInvoiceLineDataMappingAndValidatin(MyEntity.InvoiceLines,Tenant,ComputingPartnerName);
					}

								 
					temp.AmountInInvoiceCurrency = MyEntity.AmountInInvoiceCurrency;
					temp.InvoiceExpectedAmount = MyEntity.InvoiceExpectedAmount;
					temp.MainEntityReference = MyEntity.EntityReference;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}