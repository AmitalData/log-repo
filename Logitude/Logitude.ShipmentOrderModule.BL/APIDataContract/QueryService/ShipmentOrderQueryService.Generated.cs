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
				   temp.Tenant = MyEntityPM.Tenant;
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
				   temp.DescriptionofGoods = MyEntityPM.DescriptionofGoods;			  
				   if(MyEntityPM.ShipmentTypeId != null)
				   {
					   ShipmentTypeQueryService ShipmentTypeService6 = new ShipmentTypeQueryService(Tenant);
					   					   temp.ShipmentType = ShipmentTypeService6.GetShipmentTypeById(MyEntityPM.ShipmentTypeId,Tenant); 
			       
					   				   }
				   
				   temp.Master = MyEntityPM.Master;
				   temp.House = MyEntityPM.House;			  
				   if(MyEntityPM.VesselId != null)
				   {
					   VesselQueryService VesselService7 = new VesselQueryService(Tenant);
					   					   temp.Vessel = VesselService7.GetVesselById(MyEntityPM.VesselId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.CustomsAgentId != null)
				   {
					   CardQueryService CardService8 = new CardQueryService(Tenant);
					   					   temp.CustomsAgent = CardService8.GetCardById(MyEntityPM.CustomsAgentId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.SpecialServicesTypeId != null)
				   {
					   SpecialServicesTypeQueryService SpecialServicesTypeService9 = new SpecialServicesTypeQueryService(Tenant);
					   					   temp.SpecialServicesType = SpecialServicesTypeService9.GetSpecialServicesTypeById(MyEntityPM.SpecialServicesTypeId,Tenant); 
			       
					   				   }
				   
				   temp.CustomerRefrences = MyEntityPM.CustomerRefrences;					
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
					temp.Tenant = MyEntity.Tenant;
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
					temp.DescriptionofGoods = MyEntity.DescriptionofGoods;
					ShipmentTypeQueryService ShipmentTypeShipmentTypeService = new ShipmentTypeQueryService(Tenant);
					if(MyEntity.ShipmentType != null)
					{
						var myShipmentTypePM = ShipmentTypeShipmentTypeService.ShipmentTypeDataMappingAndValidatin(MyEntity.ShipmentType,Tenant,ComputingPartnerName);
												if(myShipmentTypePM != null)
						{
							temp.ShipmentTypeId = myShipmentTypePM.Id;
						}
						 
					}
			
					
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
			
					
					temp.CustomerRefrences = MyEntity.CustomerRefrences;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}