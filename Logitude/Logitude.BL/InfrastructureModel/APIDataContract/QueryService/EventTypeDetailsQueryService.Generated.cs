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
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
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
   public partial class EventTypeDetailsQueryService
   {
   
		IWebFreightContext  context;
		//EventTypeService service; 
		
		EventTypeQuery query; 

        public EventTypeDetailsQueryService(int tenant)
        {
				    context = WebFreightContext.GetContext(tenant); 
			//service = new EventTypeService(context, tenant); 
			query = new EventTypeQuery(tenant);
        }

		
		public EventTypeDetails GetEventTypeDetailsById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
								
				var temp = query.GetSinglePM(Id, Tenant);
				 if (temp == null)
                    throw new ApplicationException("EventType with Id " + Id + " doesn't exist");

				return EventTypeDetailsDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public EventTypeDetails EventTypeDetailsDataMapping(EventTypePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new EventTypeDetails(); 
				   temp.Id = MyEntityPM.Id;
				   temp.EventTypeCode = MyEntityPM.Code;
				   temp.EventTypeName = MyEntityPM.EnglishName; 

			  
				   if(MyEntityPM.EntityStatusId != null)
				   {
					   EntityStatusQueryService EntityStatusService0 = new EntityStatusQueryService(Tenant);
					   					   temp.EventTypeStatusEntity = EntityStatusService0.GetEntityStatusById(MyEntityPM.EntityStatusId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.EventTypeCustomerView = MyEntityPM.IsCustomerView;
				   temp.EventTypeAgentView = MyEntityPM.IsAgentView;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public EventTypePM EventTypeDetailsDataMappingAndValidatin(EventTypeDetails MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new EventTypePM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("EventType with Id " + MyEntity.Id + " doesn't exist");
					} 
				 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("EventType with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.EventTypeCode))
						{								
							temp.Code = MyEntity.EventTypeCode;
								
						
						}  

						
					}
                    
					if(!IsUpdate)
					{							
						temp.EnglishName = MyEntity.EventTypeName;

										}  

					
					EntityStatusQueryService EventTypeStatusEntityEntityStatusService = new EntityStatusQueryService(Tenant);
					if(MyEntity.EventTypeStatusEntity != null)
					{
						var myEventTypeStatusEntityPM = EventTypeStatusEntityEntityStatusService.EntityStatusDataMappingAndValidatin(MyEntity.EventTypeStatusEntity,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myEventTypeStatusEntityPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.EntityStatusId = myEventTypeStatusEntityPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.IsCustomerView = MyEntity.EventTypeCustomerView;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsAgentView = MyEntity.EventTypeAgentView;

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