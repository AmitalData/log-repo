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
   public partial class ARInvoiceQueryService
   {
   
		IInvoiceContext  context;
		//ARInvoiceService service; 
		
		ARInvoiceQuery query; 

        public ARInvoiceQueryService(int tenant)
        {
				    context = InvoiceContext.GetContext(tenant); 
			//service = new ARInvoiceService(context, tenant); 
			query = new ARInvoiceQuery(tenant);
        }

		
		public ARInvoice GetARInvoiceById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ARInvoice with Id " + Id + " doesn't exist");

				return ARInvoiceDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public ARInvoice ARInvoiceDataMapping(ARInvoicePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ARInvoice(); 
				   temp.Id = MyEntityPM.Id; 

			  
				   if(MyEntityPM.ARInvoiceTypeCode != null)
				   {
					   ARInvoiceTypeQueryService ARInvoiceTypeService0 = new ARInvoiceTypeQueryService(Tenant);
					   					   temp.InvoiceType = ARInvoiceTypeService0.GetARInvoiceTypeByCode(MyEntityPM.ARInvoiceTypeCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.BillToId != null)
				   {
					   CardQueryService CardService1 = new CardQueryService(Tenant);
					   					   temp.BillTo = CardService1.CardCustomDataMapping(MyEntityPM.BillToId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.InvoiceNumber = MyEntityPM.InvoiceNumber;
				   temp.InvoiceDate = MyEntityPM.InvoiceDate;
				   temp.PrintDate = MyEntityPM.PrintDate;
				   temp.IsPrinted = MyEntityPM.IsPrinted;
				   temp.MainEntityReference = MyEntityPM.MainEntityReference;
				   temp.IsConstituentInvoice = MyEntityPM.IsConstituentInvoice;
				   temp.IsConsolidationInvoice = MyEntityPM.IsConsolidationInvoice; 

			  
				   if(MyEntityPM.InvoiceCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService2 = new CurrencyQueryService(Tenant);
					   					   temp.InvoiceCurrency = CurrencyService2.CurrencyCustomDataMapping(MyEntityPM.InvoiceCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.AmountInLocalCurrency = MyEntityPM.AmountInLocalCurrency;
				   temp.CancelledByARInvoice = MyEntityPM.CancelledByARInvoiceId; 

			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService3 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService3.UserCustomDataMapping(MyEntityPM.CreatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.VATNumber = MyEntityPM.VatNumber; 

			  
				   if(MyEntityPM.BillToAddressId != null)
				   {
					   AddressQueryService AddressService4 = new AddressQueryService(Tenant);
					   					   temp.BillToAddress = AddressService4.AddressCustomDataMapping(MyEntityPM.BillToAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.PrintNotes = MyEntityPM.PrintNotes; 

			  
				   if(MyEntityPM.IssuedByUserId != null)
				   {
					   UserQueryService UserService5 = new UserQueryService(Tenant);
					   					   temp.IssuedByUser = UserService5.UserCustomDataMapping(MyEntityPM.IssuedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.InvoiceCurrencyExchangeRate = MyEntityPM.InvoiceCurrencyExchangeRate;
				if(MyEntityPM.InvoiceLines != null && MyEntityPM.InvoiceLines.Count > 0)
				{
					 ARInvoiceLineQueryService ARInvoiceLineService6 = new ARInvoiceLineQueryService(Tenant);
					 temp.ARInvoiceLines = ARInvoiceLineService6.ARInvoiceLineCustomDataMapping(MyEntityPM,MyEntityPM.InvoiceLines,Tenant,ComputingPartnerName);
				}

							 
				   temp.DueDate = MyEntityPM.DueDate;
				   temp.SubTotalInInvoiceCurrency = MyEntityPM.SubTotalInInvoiceCurrency;
				   temp.SubTotalInLocalCurrency = MyEntityPM.SubTotalInLocalCurrency;
				   temp.AmountInInvoiceCurrency = MyEntityPM.AmountInInvoiceCurrency;
				   temp.IsDraft = MyEntityPM.IsDraft;
				   temp.ProfitCurrencyExchangeRate = MyEntityPM.ProfitCurrencyExchangeRate;
				   temp.AmountInProfitCurrency = MyEntityPM.AmountInProfitCurrency; 

			  
				   if(MyEntityPM.TransferStatusCode != null)
				   {
					   ARInvoiceTransferStatusQueryService ARInvoiceTransferStatusService6 = new ARInvoiceTransferStatusQueryService(Tenant);
					   					   temp.TransferStatus = ARInvoiceTransferStatusService6.GetARInvoiceTransferStatusByCode(MyEntityPM.TransferStatusCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService7 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService7.BranchCustomDataMapping(MyEntityPM.BranchId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.LocalCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService8 = new CurrencyQueryService(Tenant);
					   					   temp.LocalCurrency = CurrencyService8.CurrencyCustomDataMapping(MyEntityPM.LocalCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.IsMultiCurrency = MyEntityPM.IsMultiCurrency;
				   temp.CreditARInvoice = MyEntityPM.CreditARInvoice;
				   temp.ExternalAccountingEntityId = MyEntityPM.ExternalAccountingEntityId;
				   temp.BillToGLAccount = MyEntityPM.BillToGLAccountId; 

			  
				   if(MyEntityPM.StatusCode != null)
				   {
					   ARInvoiceStatusQueryService ARInvoiceStatusService9 = new ARInvoiceStatusQueryService(Tenant);
					   					   temp.Status = ARInvoiceStatusService9.GetARInvoiceStatusByCode(MyEntityPM.StatusCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ARInvoicePM ARInvoiceDataMappingAndValidatin(ARInvoice MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new ARInvoicePM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("ARInvoice with Id " + MyEntity.Id + " doesn't exist");
					} 
										 
					if(IsUpdate == true)
					{
					    
					      temp.NewConcurrencyGUID = Guid.NewGuid().ToString(); 
						
					}
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("ARInvoice with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
					ARInvoiceTypeQueryService InvoiceTypeARInvoiceTypeService = new ARInvoiceTypeQueryService(Tenant);
					if(MyEntity.InvoiceType != null)
					{
						var myInvoiceTypePM = InvoiceTypeARInvoiceTypeService.ARInvoiceTypeDataMappingAndValidatin(MyEntity.InvoiceType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myInvoiceTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ARInvoiceTypeCode = myInvoiceTypePM.Code;
						  
							}  

							
						} 

					}
			
					
					CardQueryService BillToCardService = new CardQueryService(Tenant);
					if(MyEntity.BillTo != null)
					{
						var myBillToPM = BillToCardService.CardCustomDataMappingAndValidatin(MyEntity.BillTo,Tenant);
						
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
						temp.InvoiceNumber = MyEntity.InvoiceNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.InvoiceDate = MyEntity.InvoiceDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PrintDate = MyEntity.PrintDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsPrinted = MyEntity.IsPrinted;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.MainEntityReference = MyEntity.MainEntityReference;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsConstituentInvoice = MyEntity.IsConstituentInvoice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsConsolidationInvoice = MyEntity.IsConsolidationInvoice;

										}  

					
					CurrencyQueryService InvoiceCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.InvoiceCurrency != null)
					{
						var myInvoiceCurrencyPM = InvoiceCurrencyCurrencyService.CurrencyCustomDataMappingAndValidatin(MyEntity.InvoiceCurrency,Tenant);
						
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
						temp.AmountInLocalCurrency = MyEntity.AmountInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CancelledByARInvoiceId = MyEntity.CancelledByARInvoice;

										}  

					
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserCustomDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant);
						
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
						temp.VatNumber = MyEntity.VATNumber;

										}  

					
					AddressQueryService BillToAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.BillToAddress != null)
					{
						var myBillToAddressPM = BillToAddressAddressService.AddressCustomDataMappingAndValidatin(MyEntity.BillToAddress,Tenant);
						
						if(myBillToAddressPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.BillToAddressId = myBillToAddressPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.PrintNotes = MyEntity.PrintNotes;

										}  

					
					UserQueryService IssuedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.IssuedByUser != null)
					{
						var myIssuedByUserPM = IssuedByUserUserService.UserCustomDataMappingAndValidatin(MyEntity.IssuedByUser,Tenant);
						
						if(myIssuedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.IssuedByUserId = myIssuedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.InvoiceCurrencyExchangeRate = MyEntity.InvoiceCurrencyExchangeRate;

										}  

					 

					if(MyEntity.ARInvoiceLines != null && MyEntity.ARInvoiceLines.Count > 0)
					{
						ARInvoiceLineQueryService ARInvoiceLineService10 = new ARInvoiceLineQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.InvoiceLines = ARInvoiceLineService10.ARInvoiceLineCustomDataMappingAndValidatin(MyEntity,MyEntity.ARInvoiceLines,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)
					{							
						temp.DueDate = MyEntity.DueDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SubTotalInInvoiceCurrency = MyEntity.SubTotalInInvoiceCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SubTotalInLocalCurrency = MyEntity.SubTotalInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AmountInInvoiceCurrency = MyEntity.AmountInInvoiceCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsDraft = MyEntity.IsDraft;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ProfitCurrencyExchangeRate = MyEntity.ProfitCurrencyExchangeRate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AmountInProfitCurrency = MyEntity.AmountInProfitCurrency;

										}  

					
					ARInvoiceTransferStatusQueryService TransferStatusARInvoiceTransferStatusService = new ARInvoiceTransferStatusQueryService(Tenant);
					if(MyEntity.TransferStatus != null)
					{
						var myTransferStatusPM = TransferStatusARInvoiceTransferStatusService.ARInvoiceTransferStatusDataMappingAndValidatin(MyEntity.TransferStatus,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransferStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.TransferStatusCode = myTransferStatusPM.Code;
						  
							}  

							
						} 

					}
			
					
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchCustomDataMappingAndValidatin(MyEntity.Branch,Tenant);
						
						if(myBranchPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.BranchId = myBranchPM.Id;
						  
							}  

							
						} 

					}
			
					
					CurrencyQueryService LocalCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.LocalCurrency != null)
					{
						var myLocalCurrencyPM = LocalCurrencyCurrencyService.CurrencyCustomDataMappingAndValidatin(MyEntity.LocalCurrency,Tenant);
						
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
						temp.Tenant = MyEntity.Tenant;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsMultiCurrency = MyEntity.IsMultiCurrency;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CreditARInvoice = MyEntity.CreditARInvoice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ExternalAccountingEntityId = MyEntity.ExternalAccountingEntityId;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.BillToGLAccountId = MyEntity.BillToGLAccount;

										}  

					
					ARInvoiceStatusQueryService StatusARInvoiceStatusService = new ARInvoiceStatusQueryService(Tenant);
					if(MyEntity.Status != null)
					{
						var myStatusPM = StatusARInvoiceStatusService.ARInvoiceStatusDataMappingAndValidatin(MyEntity.Status,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
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