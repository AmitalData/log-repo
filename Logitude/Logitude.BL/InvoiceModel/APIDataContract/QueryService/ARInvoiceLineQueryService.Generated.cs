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
					   					   temp.ChargesType = ChargesTypeService0.ChargesTypeCustomDataMapping(item.ChargesTypeId,Tenant); 
			       
					   				   }
				   			  
				   if(item.ForiegnCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
					   					   temp.ForeignCurrency = CurrencyService1.CurrencyCustomDataMapping(item.ForiegnCurrencyId,Tenant); 
			       
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
					   					   temp.VatType = VatTypeService2.VatTypeCustomDataMapping(item.VatTypeId,Tenant); 
			       
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
					   					   temp.ARInvoiceLineAction = ARInvoiceLineActionService3.GetARInvoiceLineActionByCode(item.LineActionCode,Tenant); 
			       
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

		public List<ARInvoiceLinePM> ARInvoiceLineDataMappingAndValidatin(List<ARInvoiceLine> MyEntity,int Tenant,string ComputingPartnerName = "")
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
					temp.LineNumber = item.LineNumber;
					ChargesTypeQueryService ChargesTypeChargesTypeService = new ChargesTypeQueryService(Tenant);
					if(item.ChargesType != null)
					{
						var myChargesTypePM = ChargesTypeChargesTypeService.ChargesTypeCustomDataMappingAndValidatin(item.ChargesType,Tenant);
												if(myChargesTypePM != null)
						{
							temp.ChargesTypeId = myChargesTypePM.Id;
						}
						 
					}
			
					
					CurrencyQueryService ForeignCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.ForeignCurrency != null)
					{
						var myForeignCurrencyPM = ForeignCurrencyCurrencyService.CurrencyCustomDataMappingAndValidatin(item.ForeignCurrency,Tenant);
												if(myForeignCurrencyPM != null)
						{
							temp.ForiegnCurrencyId = myForeignCurrencyPM.Id;
						}
						 
					}
			
					
					temp.ForiegnExchangeRate = item.ForeignExchangeRate;
					temp.LocalCurrencyAmount = item.LocalCurrencyAmount;
					temp.ForiegnCurrencyAmount = item.ForeignCurrencyAmount;
					temp.LocalDescription = item.LocalDescription;
					temp.Notes = item.Notes;
					temp.ValueDate = item.ValueDate;
					temp.DateForInterest = item.DateForInterest;
					VatTypeQueryService VatTypeVatTypeService = new VatTypeQueryService(Tenant);
					if(item.VatType != null)
					{
						var myVatTypePM = VatTypeVatTypeService.VatTypeCustomDataMappingAndValidatin(item.VatType,Tenant);
												if(myVatTypePM != null)
						{
							temp.VatTypeId = myVatTypePM.Id;
						}
						 
					}
			
					
					temp.Description = item.Description;
					temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
					temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
					temp.VatPercentage = item.VatPercentage;
					temp.UnitPrice = item.UnitPriceInForeignCurrency;
					temp.ExchangeRateDate = item.ExchangeRateDate;
					temp.Quantity = item.Quantity;
					temp.Tenant = item.Tenant;
					temp.GLAccountId = item.GLAccountId;
					ARInvoiceLineActionQueryService ARInvoiceLineActionARInvoiceLineActionService = new ARInvoiceLineActionQueryService(Tenant);
					if(item.ARInvoiceLineAction != null)
					{
						var myARInvoiceLineActionPM = ARInvoiceLineActionARInvoiceLineActionService.ARInvoiceLineActionDataMappingAndValidatin(item.ARInvoiceLineAction,Tenant,ComputingPartnerName);
												if(myARInvoiceLineActionPM != null)
						{
							temp.LineActionCode = myARInvoiceLineActionPM.Code;
						}
						 
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