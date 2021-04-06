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
   public partial class TransportModeQueryService
   {
   
		Logitude.CargoTracking.BL.EntityQueryServices.CargoTrackingTransportModeQueryService query; 

        public TransportModeQueryService(int tenant)
        {
		
			query = new Logitude.CargoTracking.BL.EntityQueryServices.CargoTrackingTransportModeQueryService(tenant);
        }

		
		public TransportMode GetTransportModeById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("CargoTrackingTransportMode with Id " + Id + " doesn't exist");

				return TransportModeDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public TransportMode TransportModeDataMapping(CargoTrackingTransportModePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new TransportMode(); 
				   temp.Code = MyEntityPM.Id;
				   temp.Name = MyEntityPM.Name;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public CargoTrackingTransportModePM TransportModeDataMappingAndValidatin(TransportMode MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new CargoTrackingTransportModePM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePM(MyEntity.Code,Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("CargoTrackingTransportMode with Code " + MyEntity.Code + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Code;
					}
					temp.Name = MyEntity.Name;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}