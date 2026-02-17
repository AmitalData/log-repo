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
					   					   temp.ChargesType = ChargesTypeService0.GetChargesTypeById(item.ChargesTypeId,Tenant); 
			       
					   				   }
				   			  
				   if(item.CostCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
					   					   temp.CostCurrency = CurrencyService1.GetCurrencyById(item.CostCurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.CostExchangeRate = item.CostExchangeRate;
				   temp.CostIsFixedRate = item.CostIsFixedRate;			  
				   if(item.CostMeasurementId != null)
				   {
					   MeasurementQueryService MeasurementService2 = new MeasurementQueryService(Tenant);
					   					   temp.CostMeasurement = MeasurementService2.GetMeasurementById(item.CostMeasurementId,Tenant); 
			       
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
					   					   temp.SaleMeasurement = MeasurementService3.GetMeasurementById(item.SaleMeasurementId,Tenant); 
			       
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
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<QuoteChargePM> QuoteChargeDataMappingAndValidatin(List<QuoteCharge> MyEntity,int Tenant,string ComputingPartnerName = "")
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
						temp.Id = item.Id;
					}					ChargesTypeQueryService ChargesTypeChargesTypeService = new ChargesTypeQueryService(Tenant);
					if(item.ChargesType != null)
					{
						var myChargesTypePM = ChargesTypeChargesTypeService.ChargesTypeDataMappingAndValidatin(item.ChargesType,Tenant,ComputingPartnerName);
												if(myChargesTypePM != null)
						{
							temp.ChargesTypeId = myChargesTypePM.Id;
						}
						 
					}
			
										CurrencyQueryService CostCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.CostCurrency != null)
					{
						var myCostCurrencyPM = CostCurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.CostCurrency,Tenant,ComputingPartnerName);
												if(myCostCurrencyPM != null)
						{
							temp.CostCurrencyId = myCostCurrencyPM.Id;
						}
						 
					}
			
					
					temp.CostExchangeRate = item.CostExchangeRate;
					temp.CostIsFixedRate = item.CostIsFixedRate;					MeasurementQueryService CostMeasurementMeasurementService = new MeasurementQueryService(Tenant);
					if(item.CostMeasurement != null)
					{
						var myCostMeasurementPM = CostMeasurementMeasurementService.MeasurementDataMappingAndValidatin(item.CostMeasurement,Tenant,ComputingPartnerName);
												if(myCostMeasurementPM != null)
						{
							temp.CostMeasurementId = myCostMeasurementPM.Id;
						}
						 
					}
			
					
					temp.CostQuantity = item.CostQuantity;
					temp.CostTotalAmount = item.CostTotalAmount;
					temp.CostUnitPrice = item.CostUnitPrice;
					temp.IsAllIN = item.IsAllIN;
					temp.SaleExchangeRate = item.SaleExchangeRate;
					temp.SaleIsFixedRate = item.SaleIsFixedRate;					MeasurementQueryService SaleMeasurementMeasurementService = new MeasurementQueryService(Tenant);
					if(item.SaleMeasurement != null)
					{
						var mySaleMeasurementPM = SaleMeasurementMeasurementService.MeasurementDataMappingAndValidatin(item.SaleMeasurement,Tenant,ComputingPartnerName);
												if(mySaleMeasurementPM != null)
						{
							temp.SaleMeasurementId = mySaleMeasurementPM.Id;
						}
						 
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