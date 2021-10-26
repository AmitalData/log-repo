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
   public partial class EventTypeQueryService
   {
   
		IWebFreightContext  context;
		//EventTypeService service; 
		
		EventTypeQuery query; 

        public EventTypeQueryService(int tenant)
        {
				    context = WebFreightContext.GetContext(tenant); 
			//service = new EventTypeService(context, tenant); 
			query = new EventTypeQuery(tenant);
        }

		
		public EventType GetEventTypeById(string Id,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("EventType with Id " + Id + " doesn't exist");

				return EventTypeDataMapping(temp,Tenant,ComputingPartnerName);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public EventType EventTypeDataMapping(EventTypePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new EventType(); 
				   temp.Id = MyEntityPM.Id;
				   temp.EventTypeCode = MyEntityPM.Code;
				   temp.EventTypeStatusEntity = MyEntityPM.EnglishName; 

			  
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

		public EventTypePM EventTypeDataMappingAndValidatin(EventType MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
								//throw new ApplicationException("EventTypeCode Can't be update"); 
								temp.Code = MyEntity.EventTypeCode;
								
						
						}  

						
					}
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.EventTypeStatusEntity))
					{							//throw new ApplicationException("EventTypeStatusEntity Can't be update"); 
							temp.EnglishName = MyEntity.EventTypeStatusEntity;

										}  

					
					EntityStatusQueryService EventTypeStatusEntityEntityStatusService = new EntityStatusQueryService(Tenant);
					if(MyEntity.EventTypeStatusEntity != null)
					{
						var myEventTypeStatusEntityPM = EventTypeStatusEntityEntityStatusService.EntityStatusDataMappingAndValidatin(MyEntity.EventTypeStatusEntity,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myEventTypeStatusEntityPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("EventTypeStatusEntity Can't be update"); 
								temp.EntityStatusId = myEventTypeStatusEntityPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && (MyEntity.EventTypeCustomerView != temp.IsCustomerView))
					{							//throw new ApplicationException("EventTypeCustomerView Can't be update"); 
							temp.IsCustomerView = MyEntity.EventTypeCustomerView;

										}  

					
                    
					if(!IsUpdate)// && (MyEntity.EventTypeAgentView != temp.IsAgentView))
					{							//throw new ApplicationException("EventTypeAgentView Can't be update"); 
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