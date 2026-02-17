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
using Logitude.Server.Tools;

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
					   					   temp.ChargesType = ChargesTypeService0.GetChargesTypeById(item.ChargesTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
				   temp.LocalCurrencyAmount = item.LocalCurrencyAmount;
				   temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount; 

			  
				   if(item.VatTypeId != null)
				   {
					   VatTypeQueryService VatTypeService1 = new VatTypeQueryService(Tenant);
					   					   temp.VatType = VatTypeService1.VatTypeCustomDataMapping(item.VatTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Notes = item.Notes;
				   temp.VatPercentage = item.VatPercentage; 

			  
				   if(item.ForiegnCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService2 = new CurrencyQueryService(Tenant);
					   					   temp.ForiegnCurrency = CurrencyService2.GetCurrencyById(item.ForiegnCurrencyId,Tenant,ComputingPartnerName); 
			       
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
					   					   temp.PrepaidCollect = PrepaidCollectService3.GetPrepaidCollectById(item.PrepaidCollectId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ExternalVATCard = item.ExternalVATCard; 

			  
				   if(item.ContainerTypeId != null)
				   {
					   PackageTypeQueryService PackageTypeService4 = new PackageTypeQueryService(Tenant);
					   					   temp.ContainerType = PackageTypeService4.GetPackageTypeById(item.ContainerTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Quantity = item.Quantity;
					temp.ExcludeFromTaxReport = item.ExcludeFromTaxReport;

                    MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<APInvoiceLinePM> APInvoiceLineDataMappingAndValidatin(List<APInvoiceLine> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
				 
					
                    
					if(!IsUpdate)
					{							
						temp.APInvoiceId = item.APInvoiceId;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.LineNumber = item.LineNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Tenant = item.Tenant;

										}  

					
					ChargesTypeQueryService ChargesTypeChargesTypeService = new ChargesTypeQueryService(Tenant);
					if(item.ChargesType != null)
					{
						var myChargesTypePM = ChargesTypeChargesTypeService.ChargesTypeDataMappingAndValidatin(item.ChargesType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myChargesTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ChargesTypeId = myChargesTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.LocalCurrencyAmount = item.LocalCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;

										}  

					
					VatTypeQueryService VatTypeVatTypeService = new VatTypeQueryService(Tenant);
					if(item.VatType != null)
					{
						var myVatTypePM = VatTypeVatTypeService.VatTypeCustomDataMappingAndValidatin(item.VatType,Tenant);
						
						if(myVatTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.VatTypeId = myVatTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.Notes = item.Notes;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.VatPercentage = item.VatPercentage;

										}  

					
					CurrencyQueryService ForiegnCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.ForiegnCurrency != null)
					{
						var myForiegnCurrencyPM = ForiegnCurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.ForiegnCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myForiegnCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ForiegnCurrencyId = myForiegnCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.ForiegnExchangeRate = item.ForiegnExchangeRate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ForiegnCurrencyAmount = item.ForiegnCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.DebitAccount = item.DebitAccount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Description = item.Description;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.LocalDescription = item.LocalDescription;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ChargeTypeGLAccountId = item.ChargeTypeGLAccountId;

										}  

					
					PrepaidCollectQueryService PrepaidCollectPrepaidCollectService = new PrepaidCollectQueryService(Tenant);
					if(item.PrepaidCollect != null)
					{
						var myPrepaidCollectPM = PrepaidCollectPrepaidCollectService.PrepaidCollectDataMappingAndValidatin(item.PrepaidCollect,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPrepaidCollectPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PrepaidCollectId = myPrepaidCollectPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.ExternalVATCard = item.ExternalVATCard;

										}  

					
					PackageTypeQueryService ContainerTypePackageTypeService = new PackageTypeQueryService(Tenant);
					if(item.ContainerType != null)
					{
						var myContainerTypePM = ContainerTypePackageTypeService.PackageTypeDataMappingAndValidatin(item.ContainerType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myContainerTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ContainerTypeId = myContainerTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.Quantity = item.Quantity;

										}


                    if (!IsUpdate)
                    {
                        temp.ExcludeFromTaxReport = item.ExcludeFromTaxReport;

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