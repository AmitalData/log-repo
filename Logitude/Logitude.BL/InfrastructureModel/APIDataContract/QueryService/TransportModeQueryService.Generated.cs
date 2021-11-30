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
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;

 namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{ 
   public partial class TransportModeQueryService
   {
   
		TransportModeQuery query; 

        public TransportModeQueryService(int tenant)
        {
		
			query = new TransportModeQuery(tenant);
        }

		
		public TransportMode GetTransportModeById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("TransportMode with Id " + Id + " doesn't exist");

				return TransportModeDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public TransportMode TransportModeDataMapping(TransportModePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
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

		public TransportModePM TransportModeDataMappingAndValidatin(TransportMode MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new TransportModePM();								  
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePM(MyEntity.Code);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("TransportMode with Code " + MyEntity.Code + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Code))
					    {
					        throw new ApplicationException("TransportMode with provided key doesn't exist");
						
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