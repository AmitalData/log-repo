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
							{								//throw new ApplicationException("ChargesType Can't be update"); 
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
							{								//throw new ApplicationException("CostCurrency Can't be update"); 
								temp.CostCurrencyId = myCostCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.CostExchangeRate != null)
					{							//throw new ApplicationException("CostExchangeRate Can't be update"); 
							temp.CostExchangeRate = item.CostExchangeRate;

										}  

					
                    
					if(!IsUpdate)// && (item.CostIsFixedRate != temp.CostIsFixedRate))
					{							//throw new ApplicationException("CostIsFixedRate Can't be update"); 
							temp.CostIsFixedRate = item.CostIsFixedRate;

										}  

					
					MeasurementQueryService CostMeasurementMeasurementService = new MeasurementQueryService(Tenant);
					if(item.CostMeasurement != null)
					{
						var myCostMeasurementPM = CostMeasurementMeasurementService.MeasurementDataMappingAndValidatin(item.CostMeasurement,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCostMeasurementPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("CostMeasurement Can't be update"); 
								temp.CostMeasurementId = myCostMeasurementPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.CostQuantity != null)
					{							//throw new ApplicationException("CostQuantity Can't be update"); 
							temp.CostQuantity = item.CostQuantity;

										}  

					
                    
					if(!IsUpdate)// && item.CostTotalAmount != null)
					{							//throw new ApplicationException("CostTotalAmount Can't be update"); 
							temp.CostTotalAmount = item.CostTotalAmount;

										}  

					
                    
					if(!IsUpdate)// && item.CostUnitPrice != null)
					{							//throw new ApplicationException("CostUnitPrice Can't be update"); 
							temp.CostUnitPrice = item.CostUnitPrice;

										}  

					
                    
					if(!IsUpdate)// && (item.IsAllIN != temp.IsAllIN))
					{							//throw new ApplicationException("IsAllIN Can't be update"); 
							temp.IsAllIN = item.IsAllIN;

										}  

					
                    
					if(!IsUpdate)// && item.SaleExchangeRate != null)
					{							//throw new ApplicationException("SaleExchangeRate Can't be update"); 
							temp.SaleExchangeRate = item.SaleExchangeRate;

										}  

					
                    
					if(!IsUpdate)// && (item.SaleIsFixedRate != temp.SaleIsFixedRate))
					{							//throw new ApplicationException("SaleIsFixedRate Can't be update"); 
							temp.SaleIsFixedRate = item.SaleIsFixedRate;

										}  

					
					MeasurementQueryService SaleMeasurementMeasurementService = new MeasurementQueryService(Tenant);
					if(item.SaleMeasurement != null)
					{
						var mySaleMeasurementPM = SaleMeasurementMeasurementService.MeasurementDataMappingAndValidatin(item.SaleMeasurement,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySaleMeasurementPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("SaleMeasurement Can't be update"); 
								temp.SaleMeasurementId = mySaleMeasurementPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.SaleQuantity != null)
					{							//throw new ApplicationException("SaleQuantity Can't be update"); 
							temp.SaleQuantity = item.SaleQuantity;

										}  

					
                    
					if(!IsUpdate)// && item.SaleUnitPrice != null)
					{							//throw new ApplicationException("SaleUnitPrice Can't be update"); 
							temp.SaleUnitPrice = item.SaleUnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.SaleTotalAmount != null)
					{							//throw new ApplicationException("SaleTotalAmount Can't be update"); 
							temp.SaleTotalAmount = item.SaleTotalAmount;

										}  

					
                    
					if(!IsUpdate)// && item.CostContainerType1UnitPrice != null)
					{							//throw new ApplicationException("CostContainerType1UnitPrice Can't be update"); 
							temp.CostContainerType1UnitPrice = item.CostContainerType1UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.SaleContainerType1UnitPrice != null)
					{							//throw new ApplicationException("SaleContainerType1UnitPrice Can't be update"); 
							temp.SaleContainerType1UnitPrice = item.SaleContainerType1UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.CostContainerType2UnitPrice != null)
					{							//throw new ApplicationException("CostContainerType2UnitPrice Can't be update"); 
							temp.CostContainerType2UnitPrice = item.CostContainerType2UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.SaleContainerType2UnitPrice != null)
					{							//throw new ApplicationException("SaleContainerType2UnitPrice Can't be update"); 
							temp.SaleContainerType2UnitPrice = item.SaleContainerType2UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.CostContainerType3UnitPrice != null)
					{							//throw new ApplicationException("CostContainerType3UnitPrice Can't be update"); 
							temp.CostContainerType3UnitPrice = item.CostContainerType3UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.SaleContainerType3UnitPrice != null)
					{							//throw new ApplicationException("SaleContainerType3UnitPrice Can't be update"); 
							temp.SaleContainerType3UnitPrice = item.SaleContainerType3UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.CostContainerType4UnitPrice != null)
					{							//throw new ApplicationException("CostContainerType4UnitPrice Can't be update"); 
							temp.CostContainerType4UnitPrice = item.CostContainerType4UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.SaleContainerType4UnitPrice != null)
					{							//throw new ApplicationException("SaleContainerType4UnitPrice Can't be update"); 
							temp.SaleContainerType4UnitPrice = item.SaleContainerType4UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.CostContainerType5UnitPrice != null)
					{							//throw new ApplicationException("CostContainerType5UnitPrice Can't be update"); 
							temp.CostContainerType5UnitPrice = item.CostContainerType5UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.SaleContainerType5UnitPrice != null)
					{							//throw new ApplicationException("SaleContainerType5UnitPrice Can't be update"); 
							temp.SaleContainerType5UnitPrice = item.SaleContainerType5UnitPrice;

										}  

					
                    
					if(!IsUpdate)// && item.SaleMaxAmount != null)
					{							//throw new ApplicationException("SaleMaxAmount Can't be update"); 
							temp.SaleMaxAmount = item.SaleMaxAmount;

										}  

					
                    
					if(!IsUpdate)// && item.SaleMinAmount != null)
					{							//throw new ApplicationException("SaleMinAmount Can't be update"); 
							temp.SaleMinAmount = item.SaleMinAmount;

										}  

					 

					if(item.PriceBreaks != null && item.PriceBreaks.Count > 0)
					{
						QuotePriceStepsQueryService QuotePriceStepsService4 = new QuotePriceStepsQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("PriceBreaks Can't be update"); 
								temp.QuoteChargePriceSteps = QuotePriceStepsService4.QuotePriceStepsDataMappingAndValidatin(item.PriceBreaks,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.VendorName))
					{							//throw new ApplicationException("VendorName Can't be update"); 
							temp.VendorName = item.VendorName;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.VendorCode))
					{							//throw new ApplicationException("VendorCode Can't be update"); 
							temp.VendorCode = item.VendorCode;

										}  

					
                    
					if(!IsUpdate)// && item.CostRatio != null)
					{							//throw new ApplicationException("CostRatio Can't be update"); 
							temp.CostRatio = item.CostRatio;

										}  

					
                    
					if(!IsUpdate)// && item.SaleRatio != null)
					{							//throw new ApplicationException("SaleRatio Can't be update"); 
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