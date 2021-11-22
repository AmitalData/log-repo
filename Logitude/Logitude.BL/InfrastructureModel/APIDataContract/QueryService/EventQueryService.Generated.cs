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
   public partial class EventQueryService
   {
   
		IWebFreightContext  context;
		//TraceEventService service; 
		
		TraceEventQuery query; 

        public EventQueryService(int tenant)
        {
				    context = WebFreightContext.GetContext(tenant); 
			//service = new TraceEventService(context, tenant); 
			query = new TraceEventQuery(tenant);
        }

		
		public Event GetEventById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("TraceEvent with Id " + Id + " doesn't exist");

				return EventDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Event EventDataMapping(TraceEventPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Event(); 
				   temp.Id = MyEntityPM.Id; 

			  
				   if(MyEntityPM.EventTypeId != null)
				   {
					   EventTypeQueryService EventTypeService0 = new EventTypeQueryService(Tenant);
					   					   temp.EventType = EventTypeService0.GetEventTypeById(MyEntityPM.EventTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.LogDateTime = MyEntityPM.LogDateTime;
				   temp.EventDateTime = MyEntityPM.EventDateTime;
				   temp.Notes = MyEntityPM.Notes; 

			  
				   if(MyEntityPM.UserId != null)
				   {
					   UserQueryService UserService1 = new UserQueryService(Tenant);
					   					   temp.CreatedBy = UserService1.GetUserById(MyEntityPM.UserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.IsManualEntry = MyEntityPM.IsManualEntry;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public TraceEventPM EventDataMappingAndValidatin(Event MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new TraceEventPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("TraceEvent with Id " + MyEntity.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("TraceEvent with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
					EventTypeQueryService EventTypeEventTypeService = new EventTypeQueryService(Tenant);
					if(MyEntity.EventType != null)
					{
						var myEventTypePM = EventTypeEventTypeService.EventTypeDataMappingAndValidatin(MyEntity.EventType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myEventTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("EventType Can't be update"); 
								temp.EventTypeId = myEventTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.LogDateTime != null)
					{							//throw new ApplicationException("LogDateTime Can't be update"); 
							temp.LogDateTime = MyEntity.LogDateTime;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.EventDateTime != null)
					{							//throw new ApplicationException("EventDateTime Can't be update"); 
							temp.EventDateTime = MyEntity.EventDateTime;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Notes))
					{							//throw new ApplicationException("Notes Can't be update"); 
							temp.Notes = MyEntity.Notes;

										}  

					
					UserQueryService CreatedByUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedBy != null)
					{
						var myCreatedByPM = CreatedByUserService.UserDataMappingAndValidatin(MyEntity.CreatedBy,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCreatedByPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("CreatedBy Can't be update"); 
								temp.UserId = myCreatedByPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && (MyEntity.IsManualEntry != temp.IsManualEntry))
					{							//throw new ApplicationException("IsManualEntry Can't be update"); 
							temp.IsManualEntry = MyEntity.IsManualEntry;

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