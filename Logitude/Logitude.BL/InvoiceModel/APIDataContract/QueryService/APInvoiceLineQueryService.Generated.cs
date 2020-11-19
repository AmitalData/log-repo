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
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.APInvoiceId))
					{							//throw new ApplicationException("APInvoiceId Can't be update"); 
							temp.APInvoiceId = item.APInvoiceId;

										}  

					
                    
					if(!IsUpdate)// && item.LineNumber != null)
					{							//throw new ApplicationException("LineNumber Can't be update"); 
							temp.LineNumber = item.LineNumber;

										}  

					
                    
					if(!IsUpdate)// && item.Tenant != null)
					{							//throw new ApplicationException("Tenant Can't be update"); 
							temp.Tenant = item.Tenant;

										}  

					
					ChargesTypeQueryService ChargesTypeChargesTypeService = new ChargesTypeQueryService(Tenant);
					if(item.ChargesType != null)
					{
						var myChargesTypePM = ChargesTypeChargesTypeService.ChargesTypeDataMappingAndValidatin(item.ChargesType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myChargesTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ChargesType Can't be update"); 
								temp.ChargesTypeId = myChargesTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.InvoiceCurrencyAmount != null)
					{							//throw new ApplicationException("InvoiceCurrencyAmount Can't be update"); 
							temp.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)// && item.LocalCurrencyAmount != null)
					{							//throw new ApplicationException("LocalCurrencyAmount Can't be update"); 
							temp.LocalCurrencyAmount = item.LocalCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)// && item.ProfitCurrencyAmount != null)
					{							//throw new ApplicationException("ProfitCurrencyAmount Can't be update"); 
							temp.ProfitCurrencyAmount = item.ProfitCurrencyAmount;

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
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Notes))
					{							//throw new ApplicationException("Notes Can't be update"); 
							temp.Notes = item.Notes;

										}  

					
                    
					if(!IsUpdate)// && item.VatPercentage != null)
					{							//throw new ApplicationException("VatPercentage Can't be update"); 
							temp.VatPercentage = item.VatPercentage;

										}  

					
					CurrencyQueryService ForiegnCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.ForiegnCurrency != null)
					{
						var myForiegnCurrencyPM = ForiegnCurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.ForiegnCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myForiegnCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ForiegnCurrency Can't be update"); 
								temp.ForiegnCurrencyId = myForiegnCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.ForiegnExchangeRate != null)
					{							//throw new ApplicationException("ForiegnExchangeRate Can't be update"); 
							temp.ForiegnExchangeRate = item.ForiegnExchangeRate;

										}  

					
                    
					if(!IsUpdate)// && item.ForiegnCurrencyAmount != null)
					{							//throw new ApplicationException("ForiegnCurrencyAmount Can't be update"); 
							temp.ForiegnCurrencyAmount = item.ForiegnCurrencyAmount;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.DebitAccount))
					{							//throw new ApplicationException("DebitAccount Can't be update"); 
							temp.DebitAccount = item.DebitAccount;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Description))
					{							//throw new ApplicationException("Description Can't be update"); 
							temp.Description = item.Description;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.LocalDescription))
					{							//throw new ApplicationException("LocalDescription Can't be update"); 
							temp.LocalDescription = item.LocalDescription;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.ChargeTypeGLAccountId))
					{							//throw new ApplicationException("ChargeTypeGLAccountId Can't be update"); 
							temp.ChargeTypeGLAccountId = item.ChargeTypeGLAccountId;

										}  

					
					PrepaidCollectQueryService PrepaidCollectPrepaidCollectService = new PrepaidCollectQueryService(Tenant);
					if(item.PrepaidCollect != null)
					{
						var myPrepaidCollectPM = PrepaidCollectPrepaidCollectService.PrepaidCollectDataMappingAndValidatin(item.PrepaidCollect,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPrepaidCollectPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PrepaidCollect Can't be update"); 
								temp.PrepaidCollectId = myPrepaidCollectPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.ExternalVATCard))
					{							//throw new ApplicationException("ExternalVATCard Can't be update"); 
							temp.ExternalVATCard = item.ExternalVATCard;

										}  

					
					PackageTypeQueryService ContainerTypePackageTypeService = new PackageTypeQueryService(Tenant);
					if(item.ContainerType != null)
					{
						var myContainerTypePM = ContainerTypePackageTypeService.PackageTypeDataMappingAndValidatin(item.ContainerType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myContainerTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ContainerType Can't be update"); 
								temp.ContainerTypeId = myContainerTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.Quantity != null)
					{							//throw new ApplicationException("Quantity Can't be update"); 
							temp.Quantity = item.Quantity;

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