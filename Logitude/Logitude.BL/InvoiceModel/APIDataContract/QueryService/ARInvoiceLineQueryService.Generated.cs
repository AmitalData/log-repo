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
   public partial class ARInvoiceLineQueryService
   {
   
		ARInvoiceLineQuery query; 

        public ARInvoiceLineQueryService(int tenant)
        {
		
			query = new ARInvoiceLineQuery(tenant);
        }

		
		public List<ARInvoiceLine> ARInvoiceLineDataMapping(List<ARInvoiceLinePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<ARInvoiceLine>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new ARInvoiceLine(); 
				   temp.Id = item.Id;
				   temp.LineNumber = item.LineNumber; 

			  
				   if(item.ChargesTypeId != null)
				   {
					   ChargesTypeQueryService ChargesTypeService0 = new ChargesTypeQueryService(Tenant);
					   					   temp.ChargesType = ChargesTypeService0.ChargesTypeCustomDataMapping(item.ChargesTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(item.ForiegnCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
					   					   temp.ForeignCurrency = CurrencyService1.CurrencyCustomDataMapping(item.ForiegnCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ForeignExchangeRate = item.ForiegnExchangeRate;
				   temp.LocalCurrencyAmount = item.LocalCurrencyAmount;
				   temp.ForeignCurrencyAmount = item.ForiegnCurrencyAmount;
				   temp.LocalDescription = item.LocalDescription;
				   temp.Notes = item.Notes;
				   temp.ValueDate = item.ValueDate;
				   temp.DateForInterest = item.DateForInterest; 

			  
				   if(item.VatTypeId != null)
				   {
					   VatTypeQueryService VatTypeService2 = new VatTypeQueryService(Tenant);
					   					   temp.VatType = VatTypeService2.VatTypeCustomDataMapping(item.VatTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Description = item.Description;
				   temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
				   temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
				   temp.VatPercentage = item.VatPercentage;
				   temp.UnitPriceInForeignCurrency = item.UnitPrice;
				   temp.ExchangeRateDate = item.ExchangeRateDate;
				   temp.Quantity = item.Quantity;
				   temp.Tenant = item.Tenant;
				   temp.GLAccountId = item.GLAccountId; 

			  
				   if(item.LineActionCode != null)
				   {
					   ARInvoiceLineActionQueryService ARInvoiceLineActionService3 = new ARInvoiceLineActionQueryService(Tenant);
					   					   temp.ARInvoiceLineAction = ARInvoiceLineActionService3.GetARInvoiceLineActionByCode(item.LineActionCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.InvoiceCurrencyExchangeRate = item.InvoiceCurrencyExchangeRate;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<ARInvoiceLinePM> ARInvoiceLineDataMappingAndValidatin(List<ARInvoiceLine> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ARInvoiceLinePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ARInvoiceLinePM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("ARInvoiceLine with Id " + item.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ARInvoiceLine with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
					}
                    
					if(!IsUpdate)// && item.LineNumber != null)
					{							//throw new ApplicationException("LineNumber Can't be update"); 
							temp.LineNumber = item.LineNumber;

										}  

					
					ChargesTypeQueryService ChargesTypeChargesTypeService = new ChargesTypeQueryService(Tenant);
					if(item.ChargesType != null)
					{
						var myChargesTypePM = ChargesTypeChargesTypeService.ChargesTypeCustomDataMappingAndValidatin(item.ChargesType,Tenant);
						
						if(myChargesTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ChargesType Can't be update"); 
								temp.ChargesTypeId = myChargesTypePM.Id;
						  
							}  

							
						} 

					}
			
					
					CurrencyQueryService ForeignCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.ForeignCurrency != null)
					{
						var myForeignCurrencyPM = ForeignCurrencyCurrencyService.CurrencyCustomDataMappingAndValidatin(item.ForeignCurrency,Tenant);
						
						if(myForeignCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ForeignCurrency Can't be update"); 
								temp.ForiegnCurrencyId = myForeignCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.ForeignExchangeRate != null)
					{							//throw new ApplicationException("ForeignExchangeRate Can't be update"); 
							temp.ForiegnExchangeRate = item.ForeignExchangeRate;

										}  

					
                    
					if(!IsUpdate)// && item.LocalCurrencyAmount != null)
					{							//throw new ApplicationException("LocalCurrencyAmount Can't be update"); 
							temp.LocalCurrencyAmount = item.LocalCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)// && item.ForeignCurrencyAmount != null)
					{							//throw new ApplicationException("ForeignCurrencyAmount Can't be update"); 
							temp.ForiegnCurrencyAmount = item.ForeignCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.LocalDescription))
					{							//throw new ApplicationException("LocalDescription Can't be update"); 
							temp.LocalDescription = item.LocalDescription;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Notes))
					{							//throw new ApplicationException("Notes Can't be update"); 
							temp.Notes = item.Notes;

										}  

					
                    
					if(!IsUpdate)// && item.ValueDate != null)
					{							//throw new ApplicationException("ValueDate Can't be update"); 
							temp.ValueDate = item.ValueDate;

										}  

					
                    
					if(!IsUpdate)// && item.DateForInterest != null)
					{							//throw new ApplicationException("DateForInterest Can't be update"); 
							temp.DateForInterest = item.DateForInterest;

										}  

					
					VatTypeQueryService VatTypeVatTypeService = new VatTypeQueryService(Tenant);
					if(item.VatType != null)
					{
						var myVatTypePM = VatTypeVatTypeService.VatTypeCustomDataMappingAndValidatin(item.VatType,Tenant);
						
						if(myVatTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("VatType Can't be update"); 
								temp.VatTypeId = myVatTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Description))
					{							//throw new ApplicationException("Description Can't be update"); 
							temp.Description = item.Description;

										}  

					
                    
					if(!IsUpdate)// && item.InvoiceCurrencyAmount != null)
					{							//throw new ApplicationException("InvoiceCurrencyAmount Can't be update"); 
							temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)// && item.ProfitCurrencyAmount != null)
					{							//throw new ApplicationException("ProfitCurrencyAmount Can't be update"); 
							temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)// && item.VatPercentage != null)
					{							//throw new ApplicationException("VatPercentage Can't be update"); 
							temp.VatPercentage = item.VatPercentage;

										}  

					
                    
					if(!IsUpdate)// && item.UnitPriceInForeignCurrency != null)
					{							//throw new ApplicationException("UnitPriceInForeignCurrency Can't be update"); 
							temp.UnitPrice = item.UnitPriceInForeignCurrency;

										}  

					
                    
					if(!IsUpdate)// && item.ExchangeRateDate != null)
					{							//throw new ApplicationException("ExchangeRateDate Can't be update"); 
							temp.ExchangeRateDate = item.ExchangeRateDate;

										}  

					
                    
					if(!IsUpdate)// && item.Quantity != null)
					{							//throw new ApplicationException("Quantity Can't be update"); 
							temp.Quantity = item.Quantity;

										}  

					
                    
					if(!IsUpdate)// && item.Tenant != null)
					{							//throw new ApplicationException("Tenant Can't be update"); 
							temp.Tenant = item.Tenant;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.GLAccountId))
					{							//throw new ApplicationException("GLAccountId Can't be update"); 
							temp.GLAccountId = item.GLAccountId;

										}  

					
					ARInvoiceLineActionQueryService ARInvoiceLineActionARInvoiceLineActionService = new ARInvoiceLineActionQueryService(Tenant);
					if(item.ARInvoiceLineAction != null)
					{
						var myARInvoiceLineActionPM = ARInvoiceLineActionARInvoiceLineActionService.ARInvoiceLineActionDataMappingAndValidatin(item.ARInvoiceLineAction,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myARInvoiceLineActionPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ARInvoiceLineAction Can't be update"); 
								temp.LineActionCode = myARInvoiceLineActionPM.Code;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.InvoiceCurrencyExchangeRate != null)
					{							//throw new ApplicationException("InvoiceCurrencyExchangeRate Can't be update"); 
							temp.InvoiceCurrencyExchangeRate = item.InvoiceCurrencyExchangeRate;

										}  

										   
						MyList.Add(temp);
					}
						
					return MyList;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}