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
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel;

 namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{ 
   public partial class PayableQueryService
   {
   
		ShipmentPayableQuery query; 

        public PayableQueryService(int tenant)
        {
		
			query = new ShipmentPayableQuery(tenant);
        }

		
		public List<Payable> PayableDataMapping(List<ShipmentPayablePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<Payable>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new Payable(); 
				   temp.Id = item.Id;			  
				   if(item.ChargesTypeId != null)
				   {
					   ChargesTypeQueryService ChargesTypeService0 = new ChargesTypeQueryService(Tenant);
					   					   temp.ChargesType = ChargesTypeService0.GetChargesTypeById(item.ChargesTypeId,Tenant); 
			       
					   				   }
				   			  
				   if(item.MeasurementId != null)
				   {
					   MeasurementQueryService MeasurementService1 = new MeasurementQueryService(Tenant);
					   					   temp.Measurement = MeasurementService1.GetMeasurementById(item.MeasurementId,Tenant); 
			       
					   				   }
				   
				   temp.Quantity = item.Quantity;
				   temp.UnitPrice = item.UnitPrice;			  
				   if(item.CurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService2 = new CurrencyQueryService(Tenant);
					   					   temp.Currency = CurrencyService2.GetCurrencyById(item.CurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.Rate = item.Rate;			  
				   if(item.PrepaidCollectId != null)
				   {
					   PrepaidCollectQueryService PrepaidCollectService3 = new PrepaidCollectQueryService(Tenant);
					   					   temp.PrepaidCollect = PrepaidCollectService3.GetPrepaidCollectById(item.PrepaidCollectId,Tenant); 
			       
					   				   }
				   
				   temp.Amount = item.ExpectedAmount;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<ShipmentPayablePM> PayableDataMappingAndValidatin(List<Payable> MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<ShipmentPayablePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ShipmentPayablePM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("ShipmentPayable with Id " + item.Id + " doesn't exist");
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
			
										MeasurementQueryService MeasurementMeasurementService = new MeasurementQueryService(Tenant);
					if(item.Measurement != null)
					{
						var myMeasurementPM = MeasurementMeasurementService.MeasurementDataMappingAndValidatin(item.Measurement,Tenant,ComputingPartnerName);
												if(myMeasurementPM != null)
						{
							temp.MeasurementId = myMeasurementPM.Id;
						}
						 
					}
			
					
					temp.Quantity = item.Quantity;
					temp.UnitPrice = item.UnitPrice;					CurrencyQueryService CurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.Currency != null)
					{
						var myCurrencyPM = CurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.Currency,Tenant,ComputingPartnerName);
												if(myCurrencyPM != null)
						{
							temp.CurrencyId = myCurrencyPM.Id;
						}
						 
					}
			
					
					temp.Rate = item.Rate;					PrepaidCollectQueryService PrepaidCollectPrepaidCollectService = new PrepaidCollectQueryService(Tenant);
					if(item.PrepaidCollect != null)
					{
						var myPrepaidCollectPM = PrepaidCollectPrepaidCollectService.PrepaidCollectDataMappingAndValidatin(item.PrepaidCollect,Tenant,ComputingPartnerName);
												if(myPrepaidCollectPM != null)
						{
							temp.PrepaidCollectId = myPrepaidCollectPM.Id;
						}
						 
					}
			
					
					temp.ExpectedAmount = item.Amount;					   
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