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
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.QuoteModel;

 namespace Logitude.BL.QuoteModel.APIDataContract.ApiV1
{ 
   public partial class QuoteChargeQueryService
   {
   
		QuoteChargeQuery query; 

        public QuoteChargeQueryService(int tenant)
        {
		
			query = new QuoteChargeQuery(tenant);
        }

		
		public List<QuoteCharge> QuoteChargeDataMapping(List<QuoteChargePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<QuoteCharge>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new QuoteCharge(); 
				   temp.Id = item.Id; 

			  
				   if(item.ChargesTypeId != null)
				   {
					   ChargesTypeQueryService ChargesTypeService0 = new ChargesTypeQueryService(Tenant);
					   					   temp.ChargesType = ChargesTypeService0.GetChargesTypeById(item.ChargesTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(item.CostCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
					   					   temp.CostCurrency = CurrencyService1.GetCurrencyById(item.CostCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.CostExchangeRate = item.CostExchangeRate;
				   temp.CostIsFixedRate = item.CostIsFixedRate; 

			  
				   if(item.CostMeasurementId != null)
				   {
					   MeasurementQueryService MeasurementService2 = new MeasurementQueryService(Tenant);
					   					   temp.CostMeasurement = MeasurementService2.GetMeasurementById(item.CostMeasurementId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.CostQuantity = item.CostQuantity;
				   temp.CostTotalAmount = item.CostTotalAmount;
				   temp.CostUnitPrice = item.CostUnitPrice;
				   temp.IsAllIN = item.IsAllIN;
				   temp.SaleExchangeRate = item.SaleExchangeRate;
				   temp.SaleIsFixedRate = item.SaleIsFixedRate; 

			  
				   if(item.SaleMeasurementId != null)
				   {
					   MeasurementQueryService MeasurementService3 = new MeasurementQueryService(Tenant);
					   					   temp.SaleMeasurement = MeasurementService3.GetMeasurementById(item.SaleMeasurementId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.SaleQuantity = item.SaleQuantity;
				   temp.SaleUnitPrice = item.SaleUnitPrice;
				   temp.SaleTotalAmount = item.SaleTotalAmount;
				   temp.CostContainerType1UnitPrice = item.CostContainerType1UnitPrice;
				   temp.SaleContainerType1UnitPrice = item.SaleContainerType1UnitPrice;
				   temp.CostContainerType2UnitPrice = item.CostContainerType2UnitPrice;
				   temp.SaleContainerType2UnitPrice = item.SaleContainerType2UnitPrice;
				   temp.CostContainerType3UnitPrice = item.CostContainerType3UnitPrice;
				   temp.SaleContainerType3UnitPrice = item.SaleContainerType3UnitPrice;
				   temp.CostContainerType4UnitPrice = item.CostContainerType4UnitPrice;
				   temp.SaleContainerType4UnitPrice = item.SaleContainerType4UnitPrice;
				   temp.CostContainerType5UnitPrice = item.CostContainerType5UnitPrice;
				   temp.SaleContainerType5UnitPrice = item.SaleContainerType5UnitPrice;
				   temp.SaleMaxAmount = item.SaleMaxAmount;
				   temp.SaleMinAmount = item.SaleMinAmount;
				if(item.QuoteChargePriceSteps != null && item.QuoteChargePriceSteps.Count > 0)
				{
					 QuotePriceStepsQueryService QuotePriceStepsService4 = new QuotePriceStepsQueryService(Tenant);
					 temp.PriceBreaks = QuotePriceStepsService4.QuotePriceStepsDataMapping(item.QuoteChargePriceSteps,Tenant,ComputingPartnerName);
				}

							 
				   temp.VendorName = item.VendorName;
				   temp.VendorCode = item.VendorCode;
				   temp.CostRatio = item.CostRatio;
				   temp.SaleRatio = item.SaleRatio;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<QuoteChargePM> QuoteChargeDataMappingAndValidatin(List<QuoteCharge> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<QuoteChargePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new QuoteChargePM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("QuoteCharge with Id " + item.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("QuoteCharge with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
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
			
					
					CurrencyQueryService CostCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.CostCurrency != null)
					{
						var myCostCurrencyPM = CostCurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.CostCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCostCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CostCurrencyId = myCostCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.CostExchangeRate = item.CostExchangeRate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostIsFixedRate = item.CostIsFixedRate;

										}  

					
					MeasurementQueryService CostMeasurementMeasurementService = new MeasurementQueryService(Tenant);
					if(item.CostMeasurement != null)
					{
						var myCostMeasurementPM = CostMeasurementMeasurementService.MeasurementDataMappingAndValidatin(item.CostMeasurement,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCostMeasurementPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CostMeasurementId = myCostMeasurementPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.CostQuantity = item.CostQuantity;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostTotalAmount = item.CostTotalAmount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostUnitPrice = item.CostUnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsAllIN = item.IsAllIN;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleExchangeRate = item.SaleExchangeRate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleIsFixedRate = item.SaleIsFixedRate;

										}  

					
					MeasurementQueryService SaleMeasurementMeasurementService = new MeasurementQueryService(Tenant);
					if(item.SaleMeasurement != null)
					{
						var mySaleMeasurementPM = SaleMeasurementMeasurementService.MeasurementDataMappingAndValidatin(item.SaleMeasurement,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySaleMeasurementPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.SaleMeasurementId = mySaleMeasurementPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.SaleQuantity = item.SaleQuantity;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleUnitPrice = item.SaleUnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleTotalAmount = item.SaleTotalAmount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostContainerType1UnitPrice = item.CostContainerType1UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleContainerType1UnitPrice = item.SaleContainerType1UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostContainerType2UnitPrice = item.CostContainerType2UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleContainerType2UnitPrice = item.SaleContainerType2UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostContainerType3UnitPrice = item.CostContainerType3UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleContainerType3UnitPrice = item.SaleContainerType3UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostContainerType4UnitPrice = item.CostContainerType4UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleContainerType4UnitPrice = item.SaleContainerType4UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostContainerType5UnitPrice = item.CostContainerType5UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleContainerType5UnitPrice = item.SaleContainerType5UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleMaxAmount = item.SaleMaxAmount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleMinAmount = item.SaleMinAmount;

										}  

					 

					if(item.PriceBreaks != null && item.PriceBreaks.Count > 0)
					{
						QuotePriceStepsQueryService QuotePriceStepsService4 = new QuotePriceStepsQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.QuoteChargePriceSteps = QuotePriceStepsService4.QuotePriceStepsDataMappingAndValidatin(item.PriceBreaks,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)
					{							
						temp.VendorName = item.VendorName;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.VendorCode = item.VendorCode;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostRatio = item.CostRatio;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleRatio = item.SaleRatio;

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