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
   public partial class ShipmentTypeQueryService
   {
   
		ShipmentTypeQuery query; 

        public ShipmentTypeQueryService(int tenant)
        {
		
			query = new ShipmentTypeQuery(tenant);
        }

		
		public ShipmentType GetShipmentTypeById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ShipmentType with Id " + Id + " doesn't exist");

				return ShipmentTypeDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public ShipmentType ShipmentTypeDataMapping(ShipmentTypePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ShipmentType(); 
				   temp.Code = MyEntityPM.Id;
				   temp.Name = MyEntityPM.Name;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ShipmentTypePM ShipmentTypeDataMappingAndValidatin(ShipmentType MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new ShipmentTypePM();								  
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePM(MyEntity.Code);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("ShipmentType with Code " + MyEntity.Code + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Code))
					    {
					        throw new ApplicationException("ShipmentType with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Code;

						//} 

						
					}
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Name))
					{							//throw new ApplicationException("Name Can't be update"); 
							temp.Name = MyEntity.Name;

										}  

										   
					return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}