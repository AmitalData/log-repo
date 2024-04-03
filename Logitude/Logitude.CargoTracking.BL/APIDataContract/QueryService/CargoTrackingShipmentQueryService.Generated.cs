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
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.CargoTracking.BL.EntityUpdateServices;
using Logitude.CargoTracking.BL.EntityQueryServices;
using Logitude.CargoTracking.Data;

 namespace Logitude.CargoTracking.BL.APIDataContract.ApiV1
{ 
   public partial class CargoTrackingShipmentQueryService
   {
   
		ICargoTrackingContext  context;
		//CargoTrackingShipmentService service; 
		
		Logitude.CargoTracking.BL.EntityQueryServices.CargoTrackingShipmentQueryService query; 

        public CargoTrackingShipmentQueryService(int tenant)
        {
				    context = CargoTrackingContext.GetContext(tenant); 
			//service = new CargoTrackingShipmentService(context, tenant); 
			query = new Logitude.CargoTracking.BL.EntityQueryServices.CargoTrackingShipmentQueryService(tenant);
        }

		
		public CargoTrackingShipment GetCargoTrackingShipmentById(int Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("CargoTrackingShipment with Id " + Id + " doesn't exist");

				return CargoTrackingShipmentDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public CargoTrackingShipment CargoTrackingShipmentDataMapping(CargoTrackingShipmentPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new CargoTrackingShipment(); 
				   temp.Id = MyEntityPM.Id;
				   temp.House = MyEntityPM.House;
				   temp.ShipmentNumber = MyEntityPM.ShipmentNumber;
				   temp.ArrivalEstimationDate = MyEntityPM.ArrivalEstimationDate;
				   temp.ArrivalDate = MyEntityPM.ArrivalDate;
				   temp.CustomsPaymentDate = MyEntityPM.CustomsPaymentDate;
				   temp.ClearanceDate = MyEntityPM.ClearanceDate;
				   temp.CurrentMilestoneExceptions = MyEntityPM.CurrentMilestoneExceptions;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public CargoTrackingShipmentPM CargoTrackingShipmentDataMappingAndValidatin(CargoTrackingShipment MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new CargoTrackingShipmentPM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id.ToString()))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("CargoTrackingShipment with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id.ToString()))
					{
						temp.Id = MyEntity.Id;
					}
					temp.House = MyEntity.House;
					temp.ShipmentNumber = MyEntity.ShipmentNumber;
					temp.ArrivalEstimationDate = MyEntity.ArrivalEstimationDate;
					temp.ArrivalDate = MyEntity.ArrivalDate;
					temp.CustomsPaymentDate = MyEntity.CustomsPaymentDate;
					temp.ClearanceDate = MyEntity.ClearanceDate;
					temp.CurrentMilestoneExceptions = MyEntity.CurrentMilestoneExceptions;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}