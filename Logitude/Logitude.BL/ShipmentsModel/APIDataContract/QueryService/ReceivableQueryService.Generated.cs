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
   public partial class ReceivableQueryService
   {
   
		ShipmentReceivableQuery query; 

        public ReceivableQueryService(int tenant)
        {
		
			query = new ShipmentReceivableQuery(tenant);
        }

		
		public List<Receivable> ReceivableDataMapping(List<ShipmentReceivablePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<Receivable>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new Receivable(); 
				   temp.Id = item.Id; 

			  
				   if(item.ChargesTypeId != null)
				   {
					   ChargesTypeQueryService ChargesTypeService0 = new ChargesTypeQueryService(Tenant);
					   					   temp.ChargesType = ChargesTypeService0.GetChargesTypeById(item.ChargesTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(item.MeasurementId != null)
				   {
					   MeasurementQueryService MeasurementService1 = new MeasurementQueryService(Tenant);
					   					   temp.Measurement = MeasurementService1.GetMeasurementById(item.MeasurementId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Quantity = item.Quantity;
				   temp.UnitPrice = item.UnitPrice;
				   temp.Amount = item.TotalAmount; 

			  
				   if(item.CurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService2 = new CurrencyQueryService(Tenant);
					   					   temp.Currency = CurrencyService2.GetCurrencyById(item.CurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Rate = item.Rate; 

			  
				   if(item.PrepaidCollectId != null)
				   {
					   PrepaidCollectQueryService PrepaidCollectService3 = new PrepaidCollectQueryService(Tenant);
					   					   temp.PrepaidCollect = PrepaidCollectService3.GetPrepaidCollectById(item.PrepaidCollectId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ChangeSetOp = item.ChangeSet;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<ShipmentReceivablePM> ReceivableDataMappingAndValidatin(List<Receivable> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ShipmentReceivablePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ShipmentReceivablePM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("ShipmentReceivable with Id " + item.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ShipmentReceivable with provided key doesn't exist");
						
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
			
					
					MeasurementQueryService MeasurementMeasurementService = new MeasurementQueryService(Tenant);
					if(item.Measurement != null)
					{
						var myMeasurementPM = MeasurementMeasurementService.MeasurementDataMappingAndValidatin(item.Measurement,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMeasurementPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.MeasurementId = myMeasurementPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.Quantity = item.Quantity;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.UnitPrice = item.UnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.TotalAmount = item.Amount;

										}  

					
					CurrencyQueryService CurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(item.Currency != null)
					{
						var myCurrencyPM = CurrencyCurrencyService.CurrencyDataMappingAndValidatin(item.Currency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CurrencyId = myCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.Rate = item.Rate;

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
			
					
                    							
						temp.ChangeSet = item.ChangeSetOp;

					 

										   
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