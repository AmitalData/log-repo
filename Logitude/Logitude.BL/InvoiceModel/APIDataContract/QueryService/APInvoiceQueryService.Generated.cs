using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityOtherServices;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logitude.Customs.BL.Messaging.Amital.UnifreightQInvoiceList;

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
				   temp.ConfirmationNumber = MyEntityPM.ConfirmationNumber;
			  
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
					if(MyEntityPM.InvoiceLines?.Any() == true)
					{
						 APInvoiceLineQueryService APInvoiceLineService10 = new APInvoiceLineQueryService(Tenant);
						 temp.InvoiceLines = APInvoiceLineService10.APInvoiceLineDataMapping(MyEntityPM.InvoiceLines,Tenant,ComputingPartnerName);
					}

							 
				   temp.AmountInInvoiceCurrency = MyEntityPM.AmountInInvoiceCurrency;
				   temp.InvoiceExpectedAmount = MyEntityPM.InvoiceExpectedAmount;
				   temp.EntityReference = MyEntityPM.MainEntityReference;
                   temp.ConfirmationNumber = MyEntityPM.ConfirmationNumber;
				   temp.VendorGLAccount = MyEntityPM.VendorGLAccountId;
				if(MyEntityPM.TotalVATs?.Any() == true)
				{
					 APInvoiceTotalVATQueryService APInvoiceTotalVATService10 = new APInvoiceTotalVATQueryService(Tenant);
					 temp.TotalVATs = APInvoiceTotalVATService10.APInvoiceTotalVATDataMapping(MyEntityPM.TotalVATs,Tenant,ComputingPartnerName);
				}

							 
				   temp.TotalVATOnly = MyEntityPM.TotalVATOnly; 

			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService10 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService10.GetUserById(MyEntityPM.CreatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
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

					if (!IsUpdate)
					{
						temp.Tenant = MyEntity.Tenant;


						temp.InternalNumber = MyEntity.InternalNumber;

					}  

					CardPM myVendorPM = null;
					VendorQueryService VendorVendorService = new VendorQueryService(Tenant);
					if(MyEntity.Vendor != null)
					{
						myVendorPM = VendorVendorService.VendorDataMappingAndValidatin(MyEntity.Vendor,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myVendorPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.VendorId = myVendorPM.Id;
						  
							}  

							
						} 

					}



					if (!IsUpdate)
					{
						temp.VendorGLAccountId = MyEntity.VendorGLAccount;

						if (!IsLocalVendor(myVendorPM, Tenant))
						{
							temp.VATNumber = MyEntity.VATNumber; // Take VAT Number from the input message
						}
						else // All the logics are there for the Local Vendors only 
						{
                        MyEntity.VATNumber = MyEntity.VATNumber.Length >= 9
                                                    ? MyEntity.VATNumber.Substring(0, 9)
                                                    : MyEntity.VATNumber;

							if (FeatureToggleHelper.HasFeatureToggle("VPI", Tenant))
							{
								if (!String.IsNullOrWhiteSpace(MyEntity.VATNumber))
								{
									string aPInvoiceVatNumberNormalized = APInvoiceMessageHelper.CheckVATValidation(MyEntity.VATNumber);
									if (MyEntity.VATNumber != "999999999" && MyEntity.VATNumber != "999999998" && MyEntity.VATNumber == aPInvoiceVatNumberNormalized)
									{
										temp.VATNumber = MyEntity.VATNumber;
									}
									else if (myVendorPM != null)
									{
										temp.VATNumber = myVendorPM.VatNumber;
										temp.VATNumber = ModifyVatNumber(temp.VATNumber);
									}
								}
							}
							else
							{
								temp.VATNumber = MyEntity.VATNumber;
							}


							if (string.IsNullOrEmpty(temp.VATNumber) && myVendorPM != null)
							{
								temp.VATNumber = myVendorPM.VatNumber;
								temp.VATNumber = ModifyVatNumber(temp.VATNumber);
							}

							if ((String.IsNullOrWhiteSpace(temp.VATNumber) || temp.VATNumber == "999999999" || temp.VATNumber == "999999998") && temp.VendorGLAccountId != null)
							{
								CardQuery cardQuery = new CardQuery(Tenant);
								var glAccountCards = cardQuery.GetCardsByGLAccountIds(new List<string> { temp.VendorGLAccountId }, Tenant);
								if (glAccountCards.Count != 0)
								{
									var vatNumber = glAccountCards.Count > 1
										? glAccountCards.FirstOrDefault(c => c.VatNumber != null)?.VatNumber
										: glAccountCards[0].VatNumber;

									if (vatNumber != null)
									{
										temp.VATNumber = ModifyVatNumber(vatNumber);
									}
								}

							}
						}  

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



					if (!IsUpdate)
					{
						temp.InvoiceDate = MyEntity.InvoiceDate;


						temp.ConfirmationNumber = MyEntity.ConfirmationNumber;


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



					if (!IsUpdate)
					{
						temp.DueDate = MyEntity.DueDate;


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



					if (!IsUpdate)
					{
						temp.InternalNotes = MyEntity.InternalNotes;


						temp.SubTotalInLocalCurrency = MyEntity.SubTotalInLocalCurrency;


						temp.SubTotalInInvoiceCurrency = MyEntity.SubTotalInInvoiceCurrency;


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



					if (!IsUpdate)
					{
						temp.ProfitCurrencyExchangeRate = MyEntity.ProfitCurrencyExchangeRate;

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



					if (!IsUpdate)
					{
						temp.AmountDue = MyEntity.AmountDue;


						temp.AmountDueInLocalCurrency = MyEntity.AmountDueInLocalCurrency;


						temp.AmountDueInProfitCurrency = MyEntity.AmountDueInProfitCurrency;


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



					if (!IsUpdate)
					{
						temp.HouseNumber = MyEntity.HouseNumber;


						temp.MasterNumber = MyEntity.MasterNumber;


						temp.Description = MyEntity.Description;


						temp.AccountingExternalCode = MyEntity.AccountingExternalCode;


						temp.CreditAccount = MyEntity.CreditAccount;


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



					if (!IsUpdate)
					{
						temp.IsExternalEntity = MyEntity.IsExternalEntity;


						temp.IsGeneralInvoice = MyEntity.IsGeneralInvoice;


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

					//bool differentCurrencies = false;
					bool differentCurrencies = MyEntity.InvoiceLines?
							.Select(line => line.ForiegnCurrency.Code??String.Empty)
							.Distinct()
							.Count() > 1;

					if (MyEntity.InvoiceLines?.Any() == true)
					{
						APInvoiceLineQueryService APInvoiceLineService11 = new APInvoiceLineQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.InvoiceLines = APInvoiceLineService11.APInvoiceLineDataMappingAndValidatin(MyEntity.InvoiceLines,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  
						
					}



					if (!IsUpdate)
					{
						temp.AmountInInvoiceCurrency = MyEntity.AmountInInvoiceCurrency;


						temp.InvoiceExpectedAmount = MyEntity.InvoiceExpectedAmount;


						temp.MainEntityReference = MyEntity.EntityReference;

					}

					if (!String.IsNullOrEmpty(MyEntity.VendorGLAccount) && differentCurrencies) 
					{
						GLAccountQueryService gLAccountListQuery = new GLAccountQueryService(Tenant);
                    bool isMulti = gLAccountListQuery.IsMultiByInternal(MyEntity.VendorGLAccount, Tenant);
                    if (!isMulti) 
						{ 
							throw new ApplicationException("Vendor GLAccount Id " + MyEntity.VendorGLAccount + " is not multi-currency"); 
						}
					}


                    if (MyEntity.TotalVATs?.Any() == true)
					{
						APInvoiceTotalVATQueryService APInvoiceTotalVATService11 = new APInvoiceTotalVATQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.TotalVATs = APInvoiceTotalVATService11.APInvoiceTotalVATDataMappingAndValidatin(MyEntity.TotalVATs,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)
					{							
						temp.TotalVATOnly = MyEntity.TotalVATOnly;

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
			
										   
					return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }

        private bool IsLocalVendor(CardPM myVendorPM, int tenant)
        {
            bool rv = true;
			const string LOCAL_CODE = "IL";
			if (myVendorPM != null)
			{
                rv = false;
                if (myVendorPM.CountryCode == LOCAL_CODE)
				{  
					rv = true; 
				}
				else if (myVendorPM.CountryId != null)
				{
                    CountryQuery countryQuery = new CountryQuery(tenant);
					var country = countryQuery.GetSinglePM(myVendorPM.CountryId, tenant);
					if (country != null)
					{
						rv = country.Code == LOCAL_CODE;
					}
                }
				else
				{
					AddressQuery addressQuery = new AddressQuery(tenant);
					var address = addressQuery.GetAddressByCardId(myVendorPM.Id, tenant);
                    if (address.CountryCode == LOCAL_CODE)
                    {
                        rv = true;
                    }
                    else if (address.CountryId != null)
                    {
                        CountryQuery countryQuery = new CountryQuery(tenant);
                        var country = countryQuery.GetSinglePM(myVendorPM.CountryId, tenant);
                        if (country != null)
                        {
                            rv = country.Code == LOCAL_CODE;
                        }
                    }
                }
			}
			return rv;
        }


        private static string ModifyVatNumber(string vatNumber)
        {
            if (String.IsNullOrWhiteSpace(vatNumber)) vatNumber = "999999998";
            return (vatNumber != null && vatNumber.Length >= 9) ? vatNumber.Substring(0, 9) : vatNumber;

        }


    }
}