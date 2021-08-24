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
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityUpdateServices;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;
using Logitude.ShipmentOrderModule.Data;

 namespace Logitude.ShipmentOrderModule.BL.APIDataContract.ApiV1
{ 
   public partial class ShipmentOrderQueryService
   {
   
		IShipmentOrderContext  context;
		//ShipmentOrderService service; 
		
		Logitude.ShipmentOrderModule.BL.EntityQueryServices.ShipmentOrderQueryService query; 

        public ShipmentOrderQueryService(int tenant)
        {
				    context = ShipmentOrderContext.GetContext(tenant); 
			//service = new ShipmentOrderService(context, tenant); 
			query = new Logitude.ShipmentOrderModule.BL.EntityQueryServices.ShipmentOrderQueryService(tenant);
        }

		
		public ShipmentOrder GetShipmentOrderById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ShipmentOrder with Id " + Id + " doesn't exist");

				return ShipmentOrderDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public ShipmentOrder ShipmentOrderDataMapping(ShipmentOrderPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ShipmentOrder(); 
				   temp.Id = MyEntityPM.Id;
				   temp.OrderNumber = MyEntityPM.OrderNumber;			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService0 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService0.GetTransportModeById(MyEntityPM.TransportModeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ConsigneeId != null)
				   {
					   CardQueryService CardService1 = new CardQueryService(Tenant);
					   					   temp.Consignee = CardService1.GetCardById(MyEntityPM.ConsigneeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ShipperId != null)
				   {
					   CardQueryService CardService2 = new CardQueryService(Tenant);
					   					   temp.Shipper = CardService2.GetCardById(MyEntityPM.ShipperId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.AgentId != null)
				   {
					   CardQueryService CardService3 = new CardQueryService(Tenant);
					   					   temp.Agent = CardService3.GetCardById(MyEntityPM.AgentId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.IncotermId != null)
				   {
					   IncotermQueryService IncotermService4 = new IncotermQueryService(Tenant);
					   					   temp.Incoterm = IncotermService4.GetIncotermById(MyEntityPM.IncotermId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.AccountManagerId != null)
				   {
					   UserQueryService UserService5 = new UserQueryService(Tenant);
					   					   temp.AccountManager = UserService5.GetUserById(MyEntityPM.AccountManagerId,Tenant); 
			       
					   				   }
				   
				   temp.PONumber = MyEntityPM.PONumber;
				   temp.Master = MyEntityPM.Master;
				   temp.House = MyEntityPM.House;			  
				   if(MyEntityPM.VesselId != null)
				   {
					   VesselQueryService VesselService6 = new VesselQueryService(Tenant);
					   					   temp.Vessel = VesselService6.GetVesselById(MyEntityPM.VesselId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.CustomsAgentId != null)
				   {
					   CardQueryService CardService7 = new CardQueryService(Tenant);
					   					   temp.CustomsAgent = CardService7.GetCardById(MyEntityPM.CustomsAgentId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.SpecialServicesTypeId != null)
				   {
					   SpecialServicesTypeQueryService SpecialServicesTypeService8 = new SpecialServicesTypeQueryService(Tenant);
					   					   temp.SpecialServicesType = SpecialServicesTypeService8.GetSpecialServicesTypeById(MyEntityPM.SpecialServicesTypeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ForwarderId != null)
				   {
					   CardQueryService CardService9 = new CardQueryService(Tenant);
					   					   temp.Forwarder = CardService9.GetCardById(MyEntityPM.ForwarderId,Tenant); 
			       
					   				   }
				   
				   temp.IsReadyForPickup = MyEntityPM.IsReadyForPickup;
				   temp.DescriptionOfGoods = MyEntityPM.DescriptionOfGoods;
				   temp.TransportModeName = MyEntityPM.TransportModeName;
				   temp.CustomerReferences = MyEntityPM.CustomerReferences;
				   temp.PickupEstimatedDateTime = MyEntityPM.PickupEstimatedDateTime;
				   temp.PickupActualDateTime = MyEntityPM.PickupActualDateTime;
				   temp.BookingConfirmationDate = MyEntityPM.BookingConfirmationDate;
				   temp.ETD = MyEntityPM.ETD;
				   temp.ETA = MyEntityPM.ETA;
				   temp.ATD = MyEntityPM.ATD;
				   temp.ATA = MyEntityPM.ATA;			  
				   if(MyEntityPM.DirectionId != null)
				   {
					   DirectionQueryService DirectionService10 = new DirectionQueryService(Tenant);
					   					   temp.Direction = DirectionService10.GetDirectionById(MyEntityPM.DirectionId,Tenant); 
			       
					   				   }
				   
				   temp.ShipmentNumber = MyEntityPM.ShipmentNumber;
				   temp.SupplyDateTime = MyEntityPM.SupplyDateTime;			  
				   if(MyEntityPM.OriginPortId != null)
				   {
					   PortQueryService PortService11 = new PortQueryService(Tenant);
					   					   temp.OriginPort = PortService11.GetPortById(MyEntityPM.OriginPortId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.DestinationPortId != null)
				   {
					   PortQueryService PortService12 = new PortQueryService(Tenant);
					   					   temp.DestinationPort = PortService12.GetPortById(MyEntityPM.DestinationPortId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.GatewayId != null)
				   {
					   PortQueryService PortService13 = new PortQueryService(Tenant);
					   					   temp.Gateway = PortService13.GetPortById(MyEntityPM.GatewayId,Tenant); 
			       
					   				   }
				   
				   temp.CasualImporterName = MyEntityPM.CasualImporterName;
				   temp.CasualSupplierName = MyEntityPM.CasualSupplierName;			  
				   if(MyEntityPM.ShipmentLevelCode != null)
				   {
					   ShipmentLevelQueryService ShipmentLevelService14 = new ShipmentLevelQueryService(Tenant);
					   					   temp.ShipmentLevel = ShipmentLevelService14.GetShipmentLevelByCode(MyEntityPM.ShipmentLevelCode,Tenant); 
			       
					   				   }
				   
				   temp.PODate = MyEntityPM.PODate;
				   temp.BookingConfirmationNumber = MyEntityPM.BookingConfirmationNumber;
				   temp.CarrierNumber = MyEntityPM.CarrierNumber;			  
				   if(MyEntityPM.CarrierId != null)
				   {
					   CardQueryService CardService15 = new CardQueryService(Tenant);
					   					   temp.Carrier = CardService15.GetCardById(MyEntityPM.CarrierId,Tenant); 
			       
					   				   }
				   
				   temp.CreateDate = MyEntityPM.CreateDate;
				   temp.SecurityKey = MyEntityPM.SecurityKey;
				   temp.IsCancelled = MyEntityPM.IsCancelled;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ShipmentOrderPM ShipmentOrderDataMappingAndValidatin(ShipmentOrder MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new ShipmentOrderPM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("ShipmentOrder with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.OrderNumber = MyEntity.OrderNumber;
					TransportModeQueryService TransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.TransportMode != null)
					{
						var myTransportModePM = TransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.TransportMode,Tenant,ComputingPartnerName);
												if(myTransportModePM != null)
						{
							temp.TransportModeId = myTransportModePM.Id;
						}
						 
					}
			
					
					CardQueryService ConsigneeCardService = new CardQueryService(Tenant);
					if(MyEntity.Consignee != null)
					{
						var myConsigneePM = ConsigneeCardService.CardDataMappingAndValidatin(MyEntity.Consignee,Tenant,ComputingPartnerName);
												if(myConsigneePM != null)
						{
							temp.ConsigneeId = myConsigneePM.Id;
						}
						 
					}
			
					
					CardQueryService ShipperCardService = new CardQueryService(Tenant);
					if(MyEntity.Shipper != null)
					{
						var myShipperPM = ShipperCardService.CardDataMappingAndValidatin(MyEntity.Shipper,Tenant,ComputingPartnerName);
												if(myShipperPM != null)
						{
							temp.ShipperId = myShipperPM.Id;
						}
						 
					}
			
					
					CardQueryService AgentCardService = new CardQueryService(Tenant);
					if(MyEntity.Agent != null)
					{
						var myAgentPM = AgentCardService.CardDataMappingAndValidatin(MyEntity.Agent,Tenant,ComputingPartnerName);
												if(myAgentPM != null)
						{
							temp.AgentId = myAgentPM.Id;
						}
						 
					}
			
					
					IncotermQueryService IncotermIncotermService = new IncotermQueryService(Tenant);
					if(MyEntity.Incoterm != null)
					{
						var myIncotermPM = IncotermIncotermService.IncotermDataMappingAndValidatin(MyEntity.Incoterm,Tenant,ComputingPartnerName);
												if(myIncotermPM != null)
						{
							temp.IncotermId = myIncotermPM.Id;
						}
						 
					}
			
					
					UserQueryService AccountManagerUserService = new UserQueryService(Tenant);
					if(MyEntity.AccountManager != null)
					{
						var myAccountManagerPM = AccountManagerUserService.UserDataMappingAndValidatin(MyEntity.AccountManager,Tenant,ComputingPartnerName);
												if(myAccountManagerPM != null)
						{
							temp.AccountManagerId = myAccountManagerPM.Id;
						}
						 
					}
			
					
					temp.PONumber = MyEntity.PONumber;
					temp.Master = MyEntity.Master;
					temp.House = MyEntity.House;
					VesselQueryService VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Vessel != null)
					{
						var myVesselPM = VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Vessel,Tenant,ComputingPartnerName);
												if(myVesselPM != null)
						{
							temp.VesselId = myVesselPM.Id;
						}
						 
					}
			
					
					CardQueryService CustomsAgentCardService = new CardQueryService(Tenant);
					if(MyEntity.CustomsAgent != null)
					{
						var myCustomsAgentPM = CustomsAgentCardService.CardDataMappingAndValidatin(MyEntity.CustomsAgent,Tenant,ComputingPartnerName);
												if(myCustomsAgentPM != null)
						{
							temp.CustomsAgentId = myCustomsAgentPM.Id;
						}
						 
					}
			
					
					SpecialServicesTypeQueryService SpecialServicesTypeSpecialServicesTypeService = new SpecialServicesTypeQueryService(Tenant);
					if(MyEntity.SpecialServicesType != null)
					{
						var mySpecialServicesTypePM = SpecialServicesTypeSpecialServicesTypeService.SpecialServicesTypeDataMappingAndValidatin(MyEntity.SpecialServicesType,Tenant,ComputingPartnerName);
												if(mySpecialServicesTypePM != null)
						{
							temp.SpecialServicesTypeId = mySpecialServicesTypePM.Id;
						}
						 
					}
			
					
					CardQueryService ForwarderCardService = new CardQueryService(Tenant);
					if(MyEntity.Forwarder != null)
					{
						var myForwarderPM = ForwarderCardService.CardDataMappingAndValidatin(MyEntity.Forwarder,Tenant,ComputingPartnerName);
												if(myForwarderPM != null)
						{
							temp.ForwarderId = myForwarderPM.Id;
						}
						 
					}
			
					
					temp.IsReadyForPickup = MyEntity.IsReadyForPickup;
					temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;
					temp.TransportModeName = MyEntity.TransportModeName;
					temp.CustomerReferences = MyEntity.CustomerReferences;
					temp.PickupEstimatedDateTime = MyEntity.PickupEstimatedDateTime;
					temp.PickupActualDateTime = MyEntity.PickupActualDateTime;
					temp.BookingConfirmationDate = MyEntity.BookingConfirmationDate;
					temp.ETD = MyEntity.ETD;
					temp.ETA = MyEntity.ETA;
					temp.ATD = MyEntity.ATD;
					temp.ATA = MyEntity.ATA;
					DirectionQueryService DirectionDirectionService = new DirectionQueryService(Tenant);
					if(MyEntity.Direction != null)
					{
						var myDirectionPM = DirectionDirectionService.DirectionDataMappingAndValidatin(MyEntity.Direction,Tenant,ComputingPartnerName);
												if(myDirectionPM != null)
						{
							temp.DirectionId = myDirectionPM.Id;
						}
						 
					}
			
					
					temp.ShipmentNumber = MyEntity.ShipmentNumber;
					temp.SupplyDateTime = MyEntity.SupplyDateTime;
					PortQueryService OriginPortPortService = new PortQueryService(Tenant);
					if(MyEntity.OriginPort != null)
					{
						var myOriginPortPM = OriginPortPortService.PortDataMappingAndValidatin(MyEntity.OriginPort,Tenant,ComputingPartnerName);
												if(myOriginPortPM != null)
						{
							temp.OriginPortId = myOriginPortPM.Id;
						}
						 
					}
			
					
					PortQueryService DestinationPortPortService = new PortQueryService(Tenant);
					if(MyEntity.DestinationPort != null)
					{
						var myDestinationPortPM = DestinationPortPortService.PortDataMappingAndValidatin(MyEntity.DestinationPort,Tenant,ComputingPartnerName);
												if(myDestinationPortPM != null)
						{
							temp.DestinationPortId = myDestinationPortPM.Id;
						}
						 
					}
			
					
					PortQueryService GatewayPortService = new PortQueryService(Tenant);
					if(MyEntity.Gateway != null)
					{
						var myGatewayPM = GatewayPortService.PortDataMappingAndValidatin(MyEntity.Gateway,Tenant,ComputingPartnerName);
												if(myGatewayPM != null)
						{
							temp.GatewayId = myGatewayPM.Id;
						}
						 
					}
			
					
					temp.CasualImporterName = MyEntity.CasualImporterName;
					temp.CasualSupplierName = MyEntity.CasualSupplierName;
					ShipmentLevelQueryService ShipmentLevelShipmentLevelService = new ShipmentLevelQueryService(Tenant);
					if(MyEntity.ShipmentLevel != null)
					{
						var myShipmentLevelPM = ShipmentLevelShipmentLevelService.ShipmentLevelDataMappingAndValidatin(MyEntity.ShipmentLevel,Tenant,ComputingPartnerName);
												if(myShipmentLevelPM != null)
						{
							temp.ShipmentLevelCode = myShipmentLevelPM.Code;
						}
						 
					}
			
					
					temp.PODate = MyEntity.PODate;
					temp.BookingConfirmationNumber = MyEntity.BookingConfirmationNumber;
					temp.CarrierNumber = MyEntity.CarrierNumber;
					CardQueryService CarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.Carrier != null)
					{
						var myCarrierPM = CarrierCardService.CardDataMappingAndValidatin(MyEntity.Carrier,Tenant,ComputingPartnerName);
												if(myCarrierPM != null)
						{
							temp.CarrierId = myCarrierPM.Id;
						}
						 
					}
			
					
					temp.CreateDate = MyEntity.CreateDate;
					temp.SecurityKey = MyEntity.SecurityKey;
					temp.IsCancelled = MyEntity.IsCancelled;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}