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
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel;

 namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{ 
   public partial class ContainerDataQueryService
   {
   
		IShipmentsContext  context;
		//ContainerService service; 
		
		ContainerQuery query; 

        public ContainerDataQueryService(int tenant)
        {
				    context = ShipmentsContext.GetContext(tenant); 
			//service = new ContainerService(context, tenant); 
			query = new ContainerQuery(tenant);
        }

		
		public ContainerData GetContainerDataById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Container with Id " + Id + " doesn't exist");

				return ContainerDataDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public ContainerData ContainerDataDataMapping(ContainerPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ContainerData(); 
				   temp.Id = MyEntityPM.Id; 

			  
				   if(MyEntityPM.UpdatedByUserId != null)
				   {
					   UserQueryService UserService0 = new UserQueryService(Tenant);
					   					   temp.UpdatedByUser = UserService0.GetUserById(MyEntityPM.UpdatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ContainerNumber = MyEntityPM.ContainerNumber;
				   temp.DischargeDate = MyEntityPM.DischargeDate;
				   temp.EstimatedEmptyPickupDate = MyEntityPM.EstimatedEmptyPickupDate;
				   temp.ActualEmptyPickupDate = MyEntityPM.ActualEmptyPickupDate;
				   temp.EmptyPickupLocation = MyEntityPM.EmptyPickupLocation;
				   temp.DepartureLocation = MyEntityPM.DepartureLocation;
				   temp.DestinationLocation = MyEntityPM.DestinationLocation;
				   temp.PreCarriageLocation = MyEntityPM.PreCarriageLocation;
				   temp.PreCarriageETD = MyEntityPM.PreCarriageETD;
				   temp.PreCarriageATD = MyEntityPM.PreCarriageATD;
				   temp.POLLocation = MyEntityPM.POLLocation;
				   temp.EstimatedPOLArrival = MyEntityPM.EstimatedPOLArrival;
				   temp.ActualPOLArrival = MyEntityPM.ActualPOLArrival;
				   temp.EstimatedPOLLoaded = MyEntityPM.EstimatedPOLLoaded;
				   temp.ActualPOLLoaded = MyEntityPM.ActualPOLLoaded;
				   temp.EstimatedPOLVesselDeparture = MyEntityPM.EstimatedPOLVesselDeparture;
				   temp.ActualPOLVesselDeparture = MyEntityPM.ActualPOLVesselDeparture;
				   temp.Transshipment1Location = MyEntityPM.Transshipment1Location;
				   temp.EstimatedTrans1VesselArrival = MyEntityPM.EstimatedTrans1VesselArrival;
				   temp.ActualTransshipment1VesselArrival = MyEntityPM.ActualTransshipment1VesselArrival;
				   temp.EstimatedTransshipment1Discharge = MyEntityPM.EstimatedTransshipment1Discharge;
				   temp.ActualTransshipment1Discharge = MyEntityPM.ActualTransshipment1Discharge;
				   temp.EstimatedTransshipment1Loaded = MyEntityPM.EstimatedTransshipment1Loaded;
				   temp.ActualTransshipment1Loaded = MyEntityPM.ActualTransshipment1Loaded;
				   temp.EstimatedTrans1VesselDeparture = MyEntityPM.EstimatedTrans1VesselDeparture;
				   temp.ActualTrans1VesselDeparture = MyEntityPM.ActualTrans1VesselDeparture;
				   temp.Transshipment2Location = MyEntityPM.Transshipment2Location;
				   temp.EstimatedTrans2VesselArrival = MyEntityPM.EstimatedTrans2VesselArrival;
				   temp.ActualTransshipment2VesselArrival = MyEntityPM.ActualTransshipment2VesselArrival;
				   temp.EstimatedTransshipment2Discharge = MyEntityPM.EstimatedTransshipment2Discharge;
				   temp.ActualTransshipment2Discharge = MyEntityPM.ActualTransshipment2Discharge;
				   temp.EstimatedTransshipment2Loaded = MyEntityPM.EstimatedTransshipment2Loaded;
				   temp.ActualTransshipment2Loaded = MyEntityPM.ActualTransshipment2Loaded;
				   temp.EstimatedTrans2VesselDeparture = MyEntityPM.EstimatedTrans2VesselDeparture;
				   temp.ActualTrans2VesselDeparture = MyEntityPM.ActualTrans2VesselDeparture;
				   temp.Transshipment3Location = MyEntityPM.Transshipment3Location;
				   temp.EstimatedTrans3VesselArrival = MyEntityPM.EstimatedTrans3VesselArrival;
				   temp.ActualTransshipment3VesselArrival = MyEntityPM.ActualTransshipment3VesselArrival;
				   temp.EstimatedTransshipment3Discharge = MyEntityPM.EstimatedTransshipment3Discharge;
				   temp.ActualTransshipment3Discharge = MyEntityPM.ActualTransshipment3Discharge;
				   temp.EstimatedTransshipment3Loaded = MyEntityPM.EstimatedTransshipment3Loaded;
				   temp.ActualTransshipment3Loaded = MyEntityPM.ActualTransshipment3Loaded;
				   temp.EstimatedTrans3VesselDeparture = MyEntityPM.EstimatedTrans3VesselDeparture;
				   temp.ActualTrans3VesselDeparture = MyEntityPM.ActualTrans3VesselDeparture;
				   temp.Transshipment4Location = MyEntityPM.Transshipment4Location;
				   temp.EstimatedTrans4VesselArrival = MyEntityPM.EstimatedTrans4VesselArrival;
				   temp.ActualTransshipment4VesselArrival = MyEntityPM.ActualTransshipment4VesselArrival;
				   temp.EstimatedTransshipment4Discharge = MyEntityPM.EstimatedTransshipment4Discharge;
				   temp.ActualTransshipment4Discharge = MyEntityPM.ActualTransshipment4Discharge;
				   temp.EstimatedTransshipment4Loaded = MyEntityPM.EstimatedTransshipment4Loaded;
				   temp.ActualTransshipment4Loaded = MyEntityPM.ActualTransshipment4Loaded;
				   temp.EstimatedTrans4VesselDeparture = MyEntityPM.EstimatedTrans4VesselDeparture;
				   temp.ActualTrans4VesselDeparture = MyEntityPM.ActualTrans4VesselDeparture;
				   temp.Leg1Voyage = MyEntityPM.Leg1Voyage;
				   temp.Leg2Voyage = MyEntityPM.Leg2Voyage;
				   temp.Leg4Voyage = MyEntityPM.Leg4Voyage;
				   temp.Leg5Voyage = MyEntityPM.Leg5Voyage;
				   temp.PODLocation = MyEntityPM.PODLocation;
				   temp.EstimatedPODVesselArrival = MyEntityPM.EstimatedPODVesselArrival;
				   temp.ActualPODVesselArrival = MyEntityPM.ActualPODVesselArrival;
				   temp.EstimatedPODDischarge = MyEntityPM.EstimatedPODDischarge;
				   temp.ActualPODDischarge = MyEntityPM.ActualPODDischarge;
				   temp.EstimatedPODDeparture = MyEntityPM.EstimatedPODDeparture;
				   temp.ActualPODDeparture = MyEntityPM.ActualPODDeparture;
				   temp.OnCarriageLocation = MyEntityPM.OnCarriageLocation;
				   temp.OnCarriageETD = MyEntityPM.OnCarriageETD;
				   temp.OnCarriageATD = MyEntityPM.OnCarriageATD;
				   temp.LIFLocation = MyEntityPM.LIFLocation;
				   temp.EstimatedLIFArrival = MyEntityPM.EstimatedLIFArrival;
				   temp.ActualLIFArrival = MyEntityPM.ActualLIFArrival;
				   temp.EstimatedOnCarriageDeparture = MyEntityPM.EstimatedOnCarriageDeparture;
				   temp.ActualOnCarriageDeparture = MyEntityPM.ActualOnCarriageDeparture;
				   temp.GateIn = MyEntityPM.GateIn;
				   temp.GateOut = MyEntityPM.GateOut;
				   temp.EmptyReturnLocation = MyEntityPM.EmptyReturnLocation;
				   temp.EstimatedEmptyReturn = MyEntityPM.EstimatedEmptyReturn;
				   temp.ActualEmptyReturn = MyEntityPM.ActualEmptyReturn;
				   temp.CustomsReleaseState = MyEntityPM.CustomsReleaseState;
				   temp.CustomsReleaseDate = MyEntityPM.CustomsReleaseDate;
				   temp.CarrierReleaseState = MyEntityPM.CarrierReleaseState;
				   temp.CarrierReleaseDate = MyEntityPM.CarrierReleaseDate;
				   temp.AvailablityDate = MyEntityPM.AvailablityDate;
				   temp.AvailabilityLocation = MyEntityPM.AvailabilityLocation;
				   temp.LastFreeDayDate = MyEntityPM.LastFreeDayDate;
				   temp.FreeDays = MyEntityPM.FreeDays; 

			  
				   if(MyEntityPM.EmptyPickupLocationPortId != null)
				   {
					   PortQueryService PortService1 = new PortQueryService(Tenant);
					   					   temp.EmptyPickupLocationPort = PortService1.GetPortById(MyEntityPM.EmptyPickupLocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OnCarriageLocationPortId != null)
				   {
					   PortQueryService PortService2 = new PortQueryService(Tenant);
					   					   temp.OnCarriageLocationPort = PortService2.GetPortById(MyEntityPM.OnCarriageLocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.EmptyReturnLocationPortId != null)
				   {
					   PortQueryService PortService3 = new PortQueryService(Tenant);
					   					   temp.EmptyReturnLocationPort = PortService3.GetPortById(MyEntityPM.EmptyReturnLocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.AvailabilityLocationPortId != null)
				   {
					   PortQueryService PortService4 = new PortQueryService(Tenant);
					   					   temp.AvailabilityLocationPort = PortService4.GetPortById(MyEntityPM.AvailabilityLocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PreCarriageLocationPortId != null)
				   {
					   PortQueryService PortService5 = new PortQueryService(Tenant);
					   					   temp.PreCarriageLocationPort = PortService5.GetPortById(MyEntityPM.PreCarriageLocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.LIFLocationPortId != null)
				   {
					   PortQueryService PortService6 = new PortQueryService(Tenant);
					   					   temp.LIFLocationPort = PortService6.GetPortById(MyEntityPM.LIFLocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.POLLocationPortId != null)
				   {
					   PortQueryService PortService7 = new PortQueryService(Tenant);
					   					   temp.POLLocationPort = PortService7.GetPortById(MyEntityPM.POLLocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PODLocationPortId != null)
				   {
					   PortQueryService PortService8 = new PortQueryService(Tenant);
					   					   temp.PODLocationPort = PortService8.GetPortById(MyEntityPM.PODLocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.Transshipment1LocationPortId != null)
				   {
					   PortQueryService PortService9 = new PortQueryService(Tenant);
					   					   temp.Transshipment1LocationPort = PortService9.GetPortById(MyEntityPM.Transshipment1LocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.Transshipment2LocationPortId != null)
				   {
					   PortQueryService PortService10 = new PortQueryService(Tenant);
					   					   temp.Transshipment2LocationPort = PortService10.GetPortById(MyEntityPM.Transshipment2LocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.Transshipment3LocationPortId != null)
				   {
					   PortQueryService PortService11 = new PortQueryService(Tenant);
					   					   temp.Transshipment3LocationPort = PortService11.GetPortById(MyEntityPM.Transshipment3LocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.Transshipment4LocationPortId != null)
				   {
					   PortQueryService PortService12 = new PortQueryService(Tenant);
					   					   temp.Transshipment4LocationPort = PortService12.GetPortById(MyEntityPM.Transshipment4LocationPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.TerminalId != null)
				   {
					   CardQueryService CardService13 = new CardQueryService(Tenant);
					   					   temp.Terminal = CardService13.GetCardById(MyEntityPM.TerminalId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.TerminalPhone = MyEntityPM.TerminalPhone;
				   temp.IsClosed = MyEntityPM.IsClosed;
				   temp.ClosedDate = MyEntityPM.ClosedDate;
				   temp.IsCancelled = MyEntityPM.IsCancelled;
				   temp.CancelledDate = MyEntityPM.CancelledDate; 

			  
				   if(MyEntityPM.Leg1VesselId != null)
				   {
					   VesselQueryService VesselService14 = new VesselQueryService(Tenant);
					   					   temp.Leg1Vessel = VesselService14.GetVesselById(MyEntityPM.Leg1VesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.Leg2VesselId != null)
				   {
					   VesselQueryService VesselService15 = new VesselQueryService(Tenant);
					   					   temp.Leg2Vessel = VesselService15.GetVesselById(MyEntityPM.Leg2VesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.Leg3VesselId != null)
				   {
					   VesselQueryService VesselService16 = new VesselQueryService(Tenant);
					   					   temp.Leg3Vessel = VesselService16.GetVesselById(MyEntityPM.Leg3VesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.Leg4VesselId != null)
				   {
					   VesselQueryService VesselService17 = new VesselQueryService(Tenant);
					   					   temp.Leg4Vessel = VesselService17.GetVesselById(MyEntityPM.Leg4VesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.Leg5VesselId != null)
				   {
					   VesselQueryService VesselService18 = new VesselQueryService(Tenant);
					   					   temp.Leg5Vessel = VesselService18.GetVesselById(MyEntityPM.Leg5VesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.OnCarriageGateOut = MyEntityPM.OnCarriageGateOut;
				   temp.PreCarriageGateIn = MyEntityPM.PreCarriageGateIn;
				   temp.Leg3Voyage = MyEntityPM.Leg3Voyage;
				   temp.ShipmentNumber = MyEntityPM.ShipmentNumber;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ContainerPM ContainerDataDataMappingAndValidatin(ContainerData MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new ContainerPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("Container with Id " + MyEntity.Id + " doesn't exist");
					} 
				 
										 
					if(IsUpdate == true)
					{
					    
					      temp.NewConcurrencyGUID = Guid.NewGuid().ToString(); 
						
					}
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Container with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
					UserQueryService UpdatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.UpdatedByUser != null)
					{
						var myUpdatedByUserPM = UpdatedByUserUserService.UserDataMappingAndValidatin(MyEntity.UpdatedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myUpdatedByUserPM != null)
						{ 

						 								
								temp.UpdatedByUserId = myUpdatedByUserPM.Id;
						  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.ContainerNumber = MyEntity.ContainerNumber;

										}  

					
                    							
						temp.DischargeDate = MyEntity.DischargeDate;

					 

					
                    							
						temp.EstimatedEmptyPickupDate = MyEntity.EstimatedEmptyPickupDate;

					 

					
                    							
						temp.ActualEmptyPickupDate = MyEntity.ActualEmptyPickupDate;

					 

					
                    							
						temp.EmptyPickupLocation = MyEntity.EmptyPickupLocation;

					 

					
                    							
						temp.DepartureLocation = MyEntity.DepartureLocation;

					 

					
                    							
						temp.DestinationLocation = MyEntity.DestinationLocation;

					 

					
                    
					if(!IsUpdate)
					{							
						temp.PreCarriageLocation = MyEntity.PreCarriageLocation;

										}  

					
                    							
						temp.PreCarriageETD = MyEntity.PreCarriageETD;

					 

					
                    							
						temp.PreCarriageATD = MyEntity.PreCarriageATD;

					 

					
                    							
						temp.POLLocation = MyEntity.POLLocation;

					 

					
                    							
						temp.EstimatedPOLArrival = MyEntity.EstimatedPOLArrival;

					 

					
                    							
						temp.ActualPOLArrival = MyEntity.ActualPOLArrival;

					 

					
                    							
						temp.EstimatedPOLLoaded = MyEntity.EstimatedPOLLoaded;

					 

					
                    							
						temp.ActualPOLLoaded = MyEntity.ActualPOLLoaded;

					 

					
                    							
						temp.EstimatedPOLVesselDeparture = MyEntity.EstimatedPOLVesselDeparture;

					 

					
                    							
						temp.ActualPOLVesselDeparture = MyEntity.ActualPOLVesselDeparture;

					 

					
                    							
						temp.Transshipment1Location = MyEntity.Transshipment1Location;

					 

					
                    							
						temp.EstimatedTrans1VesselArrival = MyEntity.EstimatedTrans1VesselArrival;

					 

					
                    							
						temp.ActualTransshipment1VesselArrival = MyEntity.ActualTransshipment1VesselArrival;

					 

					
                    							
						temp.EstimatedTransshipment1Discharge = MyEntity.EstimatedTransshipment1Discharge;

					 

					
                    							
						temp.ActualTransshipment1Discharge = MyEntity.ActualTransshipment1Discharge;

					 

					
                    							
						temp.EstimatedTransshipment1Loaded = MyEntity.EstimatedTransshipment1Loaded;

					 

					
                    							
						temp.ActualTransshipment1Loaded = MyEntity.ActualTransshipment1Loaded;

					 

					
                    							
						temp.EstimatedTrans1VesselDeparture = MyEntity.EstimatedTrans1VesselDeparture;

					 

					
                    							
						temp.ActualTrans1VesselDeparture = MyEntity.ActualTrans1VesselDeparture;

					 

					
                    							
						temp.Transshipment2Location = MyEntity.Transshipment2Location;

					 

					
                    							
						temp.EstimatedTrans2VesselArrival = MyEntity.EstimatedTrans2VesselArrival;

					 

					
                    							
						temp.ActualTransshipment2VesselArrival = MyEntity.ActualTransshipment2VesselArrival;

					 

					
                    							
						temp.EstimatedTransshipment2Discharge = MyEntity.EstimatedTransshipment2Discharge;

					 

					
                    							
						temp.ActualTransshipment2Discharge = MyEntity.ActualTransshipment2Discharge;

					 

					
                    							
						temp.EstimatedTransshipment2Loaded = MyEntity.EstimatedTransshipment2Loaded;

					 

					
                    							
						temp.ActualTransshipment2Loaded = MyEntity.ActualTransshipment2Loaded;

					 

					
                    							
						temp.EstimatedTrans2VesselDeparture = MyEntity.EstimatedTrans2VesselDeparture;

					 

					
                    							
						temp.ActualTrans2VesselDeparture = MyEntity.ActualTrans2VesselDeparture;

					 

					
                    							
						temp.Transshipment3Location = MyEntity.Transshipment3Location;

					 

					
                    							
						temp.EstimatedTrans3VesselArrival = MyEntity.EstimatedTrans3VesselArrival;

					 

					
                    							
						temp.ActualTransshipment3VesselArrival = MyEntity.ActualTransshipment3VesselArrival;

					 

					
                    							
						temp.EstimatedTransshipment3Discharge = MyEntity.EstimatedTransshipment3Discharge;

					 

					
                    							
						temp.ActualTransshipment3Discharge = MyEntity.ActualTransshipment3Discharge;

					 

					
                    							
						temp.EstimatedTransshipment3Loaded = MyEntity.EstimatedTransshipment3Loaded;

					 

					
                    							
						temp.ActualTransshipment3Loaded = MyEntity.ActualTransshipment3Loaded;

					 

					
                    							
						temp.EstimatedTrans3VesselDeparture = MyEntity.EstimatedTrans3VesselDeparture;

					 

					
                    							
						temp.ActualTrans3VesselDeparture = MyEntity.ActualTrans3VesselDeparture;

					 

					
                    							
						temp.Transshipment4Location = MyEntity.Transshipment4Location;

					 

					
                    							
						temp.EstimatedTrans4VesselArrival = MyEntity.EstimatedTrans4VesselArrival;

					 

					
                    							
						temp.ActualTransshipment4VesselArrival = MyEntity.ActualTransshipment4VesselArrival;

					 

					
                    							
						temp.EstimatedTransshipment4Discharge = MyEntity.EstimatedTransshipment4Discharge;

					 

					
                    							
						temp.ActualTransshipment4Discharge = MyEntity.ActualTransshipment4Discharge;

					 

					
                    							
						temp.EstimatedTransshipment4Loaded = MyEntity.EstimatedTransshipment4Loaded;

					 

					
                    							
						temp.ActualTransshipment4Loaded = MyEntity.ActualTransshipment4Loaded;

					 

					
                    							
						temp.EstimatedTrans4VesselDeparture = MyEntity.EstimatedTrans4VesselDeparture;

					 

					
                    							
						temp.ActualTrans4VesselDeparture = MyEntity.ActualTrans4VesselDeparture;

					 

					
                    							
						temp.Leg1Voyage = MyEntity.Leg1Voyage;

					 

					
                    							
						temp.Leg2Voyage = MyEntity.Leg2Voyage;

					 

					
                    							
						temp.Leg4Voyage = MyEntity.Leg4Voyage;

					 

					
                    							
						temp.Leg5Voyage = MyEntity.Leg5Voyage;

					 

					
                    							
						temp.PODLocation = MyEntity.PODLocation;

					 

					
                    							
						temp.EstimatedPODVesselArrival = MyEntity.EstimatedPODVesselArrival;

					 

					
                    							
						temp.ActualPODVesselArrival = MyEntity.ActualPODVesselArrival;

					 

					
                    							
						temp.EstimatedPODDischarge = MyEntity.EstimatedPODDischarge;

					 

					
                    							
						temp.ActualPODDischarge = MyEntity.ActualPODDischarge;

					 

					
                    							
						temp.EstimatedPODDeparture = MyEntity.EstimatedPODDeparture;

					 

					
                    							
						temp.ActualPODDeparture = MyEntity.ActualPODDeparture;

					 

					
                    							
						temp.OnCarriageLocation = MyEntity.OnCarriageLocation;

					 

					
                    							
						temp.OnCarriageETD = MyEntity.OnCarriageETD;

					 

					
                    							
						temp.OnCarriageATD = MyEntity.OnCarriageATD;

					 

					
                    							
						temp.LIFLocation = MyEntity.LIFLocation;

					 

					
                    							
						temp.EstimatedLIFArrival = MyEntity.EstimatedLIFArrival;

					 

					
                    							
						temp.ActualLIFArrival = MyEntity.ActualLIFArrival;

					 

					
                    							
						temp.EstimatedOnCarriageDeparture = MyEntity.EstimatedOnCarriageDeparture;

					 

					
                    							
						temp.ActualOnCarriageDeparture = MyEntity.ActualOnCarriageDeparture;

					 

					
                    							
						temp.GateIn = MyEntity.GateIn;

					 

					
                    							
						temp.GateOut = MyEntity.GateOut;

					 

					
                    							
						temp.EmptyReturnLocation = MyEntity.EmptyReturnLocation;

					 

					
                    							
						temp.EstimatedEmptyReturn = MyEntity.EstimatedEmptyReturn;

					 

					
                    							
						temp.ActualEmptyReturn = MyEntity.ActualEmptyReturn;

					 

					
                    							
						temp.CustomsReleaseState = MyEntity.CustomsReleaseState;

					 

					
                    							
						temp.CustomsReleaseDate = MyEntity.CustomsReleaseDate;

					 

					
                    							
						temp.CarrierReleaseState = MyEntity.CarrierReleaseState;

					 

					
                    							
						temp.CarrierReleaseDate = MyEntity.CarrierReleaseDate;

					 

					
                    							
						temp.AvailablityDate = MyEntity.AvailablityDate;

					 

					
                    							
						temp.AvailabilityLocation = MyEntity.AvailabilityLocation;

					 

					
                    							
						temp.LastFreeDayDate = MyEntity.LastFreeDayDate;

					 

					
                    							
						temp.FreeDays = MyEntity.FreeDays;

					 

					
					PortQueryService EmptyPickupLocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.EmptyPickupLocationPort != null)
					{
						var myEmptyPickupLocationPortPM = EmptyPickupLocationPortPortService.PortDataMappingAndValidatin(MyEntity.EmptyPickupLocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myEmptyPickupLocationPortPM != null)
						{ 

						 								
								temp.EmptyPickupLocationPortId = myEmptyPickupLocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService OnCarriageLocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.OnCarriageLocationPort != null)
					{
						var myOnCarriageLocationPortPM = OnCarriageLocationPortPortService.PortDataMappingAndValidatin(MyEntity.OnCarriageLocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnCarriageLocationPortPM != null)
						{ 

						 								
								temp.OnCarriageLocationPortId = myOnCarriageLocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService EmptyReturnLocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.EmptyReturnLocationPort != null)
					{
						var myEmptyReturnLocationPortPM = EmptyReturnLocationPortPortService.PortDataMappingAndValidatin(MyEntity.EmptyReturnLocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myEmptyReturnLocationPortPM != null)
						{ 

						 								
								temp.EmptyReturnLocationPortId = myEmptyReturnLocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService AvailabilityLocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.AvailabilityLocationPort != null)
					{
						var myAvailabilityLocationPortPM = AvailabilityLocationPortPortService.PortDataMappingAndValidatin(MyEntity.AvailabilityLocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAvailabilityLocationPortPM != null)
						{ 

						 								
								temp.AvailabilityLocationPortId = myAvailabilityLocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService PreCarriageLocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.PreCarriageLocationPort != null)
					{
						var myPreCarriageLocationPortPM = PreCarriageLocationPortPortService.PortDataMappingAndValidatin(MyEntity.PreCarriageLocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreCarriageLocationPortPM != null)
						{ 

						 								
								temp.PreCarriageLocationPortId = myPreCarriageLocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService LIFLocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.LIFLocationPort != null)
					{
						var myLIFLocationPortPM = LIFLocationPortPortService.PortDataMappingAndValidatin(MyEntity.LIFLocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLIFLocationPortPM != null)
						{ 

						 								
								temp.LIFLocationPortId = myLIFLocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService POLLocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.POLLocationPort != null)
					{
						var myPOLLocationPortPM = POLLocationPortPortService.PortDataMappingAndValidatin(MyEntity.POLLocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPOLLocationPortPM != null)
						{ 

						 								
								temp.POLLocationPortId = myPOLLocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService PODLocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.PODLocationPort != null)
					{
						var myPODLocationPortPM = PODLocationPortPortService.PortDataMappingAndValidatin(MyEntity.PODLocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPODLocationPortPM != null)
						{ 

						 								
								temp.PODLocationPortId = myPODLocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService Transshipment1LocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.Transshipment1LocationPort != null)
					{
						var myTransshipment1LocationPortPM = Transshipment1LocationPortPortService.PortDataMappingAndValidatin(MyEntity.Transshipment1LocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransshipment1LocationPortPM != null)
						{ 

						 								
								temp.Transshipment1LocationPortId = myTransshipment1LocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService Transshipment2LocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.Transshipment2LocationPort != null)
					{
						var myTransshipment2LocationPortPM = Transshipment2LocationPortPortService.PortDataMappingAndValidatin(MyEntity.Transshipment2LocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransshipment2LocationPortPM != null)
						{ 

						 								
								temp.Transshipment2LocationPortId = myTransshipment2LocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService Transshipment3LocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.Transshipment3LocationPort != null)
					{
						var myTransshipment3LocationPortPM = Transshipment3LocationPortPortService.PortDataMappingAndValidatin(MyEntity.Transshipment3LocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransshipment3LocationPortPM != null)
						{ 

						 								
								temp.Transshipment3LocationPortId = myTransshipment3LocationPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService Transshipment4LocationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.Transshipment4LocationPort != null)
					{
						var myTransshipment4LocationPortPM = Transshipment4LocationPortPortService.PortDataMappingAndValidatin(MyEntity.Transshipment4LocationPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransshipment4LocationPortPM != null)
						{ 

						 								
								temp.Transshipment4LocationPortId = myTransshipment4LocationPortPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService TerminalCardService = new CardQueryService(Tenant);
					if(MyEntity.Terminal != null)
					{
						var myTerminalPM = TerminalCardService.CardDataMappingAndValidatin(MyEntity.Terminal,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTerminalPM != null)
						{ 

						 								
								temp.TerminalId = myTerminalPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.TerminalPhone = MyEntity.TerminalPhone;

					 

					
                    							
						temp.IsClosed = MyEntity.IsClosed;

					 

					
                    							
						temp.ClosedDate = MyEntity.ClosedDate;

					 

					
                    
					if(!IsUpdate)
					{							
						temp.IsCancelled = MyEntity.IsCancelled;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CancelledDate = MyEntity.CancelledDate;

										}  

					
					VesselQueryService Leg1VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Leg1Vessel != null)
					{
						var myLeg1VesselPM = Leg1VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Leg1Vessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLeg1VesselPM != null)
						{ 

						 								
								temp.Leg1VesselId = myLeg1VesselPM.Id;
						  

							
						} 

					}
			
					
					VesselQueryService Leg2VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Leg2Vessel != null)
					{
						var myLeg2VesselPM = Leg2VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Leg2Vessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLeg2VesselPM != null)
						{ 

						 								
								temp.Leg2VesselId = myLeg2VesselPM.Id;
						  

							
						} 

					}
			
					
					VesselQueryService Leg3VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Leg3Vessel != null)
					{
						var myLeg3VesselPM = Leg3VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Leg3Vessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLeg3VesselPM != null)
						{ 

						 								
								temp.Leg3VesselId = myLeg3VesselPM.Id;
						  

							
						} 

					}
			
					
					VesselQueryService Leg4VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Leg4Vessel != null)
					{
						var myLeg4VesselPM = Leg4VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Leg4Vessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLeg4VesselPM != null)
						{ 

						 								
								temp.Leg4VesselId = myLeg4VesselPM.Id;
						  

							
						} 

					}
			
					
					VesselQueryService Leg5VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Leg5Vessel != null)
					{
						var myLeg5VesselPM = Leg5VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Leg5Vessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLeg5VesselPM != null)
						{ 

						 								
								temp.Leg5VesselId = myLeg5VesselPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.OnCarriageGateOut = MyEntity.OnCarriageGateOut;

					 

					
                    							
						temp.PreCarriageGateIn = MyEntity.PreCarriageGateIn;

					 

					
                    							
						temp.Leg3Voyage = MyEntity.Leg3Voyage;

					 

					
                    
					if(!IsUpdate)
					{							
						temp.ShipmentNumber = MyEntity.ShipmentNumber;

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