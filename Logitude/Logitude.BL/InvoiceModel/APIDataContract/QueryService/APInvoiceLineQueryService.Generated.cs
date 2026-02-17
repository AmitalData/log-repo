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
   public partial class APInvoiceLineQueryService
   {
   
		APInvoiceLineQuery query; 

        public APInvoiceLineQueryService(int tenant)
        {
		
			query = new APInvoiceLineQuery(tenant);
        }

		
		public List<APInvoiceLine> APInvoiceLineDataMapping(List<APInvoiceLinePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<APInvoiceLine>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new APInvoiceLine(); 
				   temp.APInvoiceId = item.APInvoiceId;
				   temp.LineNumber = item.LineNumber;
				   temp.Tenant = item.Tenant;			  
				   if(item.ChargesTypeId != null)
				   {
					   ChargesTypeQueryService ChargesTypeService0 = new ChargesTypeQueryService(Tenant);
					   					   temp.ChargesType = ChargesTypeService0.GetChargesTypeById(item.ChargesTypeId,Tenant); 
			       
					   				   }
				   
				   temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
				   temp.LocalCurrencyAmount = item.LocalCurrencyAmount;
				   temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;			  
				   if(item.VatTypeId != null)
				   {
					   VatTypeQueryService VatTypeService1 = new VatTypeQueryService(Tenant);
					   					   temp.VatType = VatTypeService1.VatTypeCustomDataMapping(item.VatTypeId,Tenant); 
			       
					   				   }
				   
				   temp.Notes = item.Notes;
				   temp.VatPercentage = item.VatPercentage;			  
				   if(item.ForiegnCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService2 = new CurrencyQueryService(Tenant);
					   					   temp.ForiegnCurrency = CurrencyService2.GetCurrencyById(item.ForiegnCurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.ForiegnExchangeRate = item.ForiegnExchangeRate;
				   temp.ForiegnCurrencyAmount = item.ForiegnCurrencyAmount;
				   temp.DebitAccount = item.DebitAccount;
				   temp.Description = item.Description;
				   temp.LocalDescription = item.LocalDescription;
				   temp.ChargeTypeGLAccountId = item.ChargeTypeGLAccountId;			  
				   if(item.PrepaidCollectId != null)
				   {
					   PrepaidCollectQueryService PrepaidCollectService3 = new PrepaidCollectQueryService(Tenant);
					   					   temp.PrepaidCollect = PrepaidCollectService3.GetPrepaidCollectById(item.PrepaidCollectId,Tenant); 
			       
					   				   }
				   
				   temp.ExternalVATCard = item.ExternalVATCard;			  
				   if(item.ContainerTypeId != null)
				   {
					   PackageTypeQueryService PackageTypeService4 = new PackageTypeQueryService(Tenant);
					   					   temp.ContainerType = PackageTypeService4.GetPackageTypeById(item.ContainerTypeId,Tenant); 
			       
					   				   }
				   
				   temp.Quantity = item.Quantity;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<APInvoiceLinePM> APInvoiceLineDataMappingAndValidatin(List<APInvoiceLine> MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<APInvoiceLinePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new APInvoiceLinePM();
					if (!string.IsNullOrEmpty(item.APInvoiceId))
					{
						temp = query.GetSingle(item.APInvoiceId,item.LineNumber);
					}
										   
					if(temp == null)
					{   
						throw new ApplicationException("APInvoiceLine with provided keys doesn't exist");
					} 
					temp.APInvoiceId = item.APInvoiceId;
					temp.LineNumber = item.LineNumber;
					temp.Tenant = item.Tenant;
					ChargesTypeQueryService ChargesTypeChargesTypeService = new ChargesTypeQueryService(Tenant);
					if(item.ChargesType != null)
					{
						var myChargesTypePM = ChargesTypeChargesTypeService.ChargesTypeDataMappingAndValidatin(item.ChargesType,Tenant,ComputingPartnerName);
												if(myChargesTypePM != null)
						{
							temp.ChargesTypeId = myChargesTypePM.Id;
						}
						 
					}
			
					
					temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
					temp.LocalCurrencyAmount = item.LocalCurrencyAmount;
					temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
					VatTypeQueryService VatTypeVatTypeService = new VatTypeQueryService(Tenant);
					if(item.VatType != null)
					{
						var myVatTypePM = VatTypeVatTypeService.VatTypeCustomDataMappingAndValidatin(item.VatType,Tenant);
												if(myVatTypePM != null)
						{
							temp.VatTypeId = myVatTypePM.Id;
						}
						 
					}
			
					
					temp.Notes = item.Notes;
					temp.VatPercentage = item.VatPercentage;
					CurrencyQueryService ForiegnCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.ForiegnCurrency != null)
					{
						var myForiegnCurrencyPM = ForiegnCurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.ForiegnCurrency,Tenant,ComputingPartnerName);
												if(myForiegnCurrencyPM != null)
						{
							temp.ForiegnCurrencyId = myForiegnCurrencyPM.Id;
						}
						 
					}
			
					
					temp.ForiegnExchangeRate = item.ForiegnExchangeRate;
					temp.ForiegnCurrencyAmount = item.ForiegnCurrencyAmount;
					temp.DebitAccount = item.DebitAccount;
					temp.Description = item.Description;
					temp.LocalDescription = item.LocalDescription;
					temp.ChargeTypeGLAccountId = item.ChargeTypeGLAccountId;
					PrepaidCollectQueryService PrepaidCollectPrepaidCollectService = new PrepaidCollectQueryService(Tenant);
					if(item.PrepaidCollect != null)
					{
						var myPrepaidCollectPM = PrepaidCollectPrepaidCollectService.PrepaidCollectDataMappingAndValidatin(item.PrepaidCollect,Tenant,ComputingPartnerName);
												if(myPrepaidCollectPM != null)
						{
							temp.PrepaidCollectId = myPrepaidCollectPM.Id;
						}
						 
					}
			
					
					temp.ExternalVATCard = item.ExternalVATCard;
					PackageTypeQueryService ContainerTypePackageTypeService = new PackageTypeQueryService(Tenant);
					if(item.ContainerType != null)
					{
						var myContainerTypePM = ContainerTypePackageTypeService.PackageTypeDataMappingAndValidatin(item.ContainerType,Tenant,ComputingPartnerName);
												if(myContainerTypePM != null)
						{
							temp.ContainerTypeId = myContainerTypePM.Id;
						}
						 
					}
			
					
					temp.Quantity = item.Quantity;					   
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