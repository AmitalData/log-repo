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
   public partial class MasterQueryService
   {
   
		IShipmentsContext  context;
		//ShipmentService service; 
		
		ShipmentQuery query; 

        public MasterQueryService(int tenant)
        {
				    context = ShipmentsContext.GetContext(tenant); 
			//service = new ShipmentService(context, tenant); 
			query = new ShipmentQuery(tenant);
        }

		
		public Master GetMasterById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Shipment with Id " + Id + " doesn't exist");

				return MasterDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Master MasterDataMapping(ShipmentPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Master(); 
				   temp.Id = MyEntityPM.Id;			  
				   if(MyEntityPM.DirectionId != null)
				   {
					   DirectionQueryService DirectionService0 = new DirectionQueryService(Tenant);
					   					   temp.Direction = DirectionService0.GetDirectionById(MyEntityPM.DirectionId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService1 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService1.GetTransportModeById(MyEntityPM.TransportModeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ShipmentTypeId != null)
				   {
					   ShipmentTypeQueryService ShipmentTypeService2 = new ShipmentTypeQueryService(Tenant);
					   					   temp.ShipmentType = ShipmentTypeService2.ShipmentTypeCustomDataMapping(MyEntityPM.ShipmentTypeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ShipperId != null)
				   {
					   CardQueryService CardService3 = new CardQueryService(Tenant);
					   					   temp.Shipper = CardService3.GetCardById(MyEntityPM.ShipperId,Tenant); 
			       
					   				   }
				   
				   temp.ShipperReference1 = MyEntityPM.ShipperReference1;
				   temp.ShipperReference2 = MyEntityPM.ShipperReference2;			  
				   if(MyEntityPM.ConsigneeId != null)
				   {
					   CardQueryService CardService4 = new CardQueryService(Tenant);
					   					   temp.Consignee = CardService4.GetCardById(MyEntityPM.ConsigneeId,Tenant); 
			       
					   				   }
				   
				   temp.ConsigneeReference1 = MyEntityPM.ConsigneeReference1;
				   temp.ConsigneeReference2 = MyEntityPM.ConsigneeReference2;			  
				   if(MyEntityPM.AgentId != null)
				   {
					   CardQueryService CardService5 = new CardQueryService(Tenant);
					   					   temp.Agent = CardService5.GetCardById(MyEntityPM.AgentId,Tenant); 
			       
					   				   }
				   
				   temp.AgentReference1 = MyEntityPM.AgentReference1;
				   temp.AgentReference2 = MyEntityPM.AgentReference2;			  
				   if(MyEntityPM.FromPortId != null)
				   {
					   PortQueryService PortService6 = new PortQueryService(Tenant);
					   					   temp.FromPort = PortService6.GetPortById(MyEntityPM.FromPortId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ToPortId != null)
				   {
					   PortQueryService PortService7 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService7.GetPortById(MyEntityPM.ToPortId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.GrossWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService8 = new WeightUnitQueryService(Tenant);
					   					   temp.GrossWeightUnit = WeightUnitService8.GetWeightUnitByCode(MyEntityPM.GrossWeightUnitCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ChargeableWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService9 = new WeightUnitQueryService(Tenant);
					   					   temp.ChargeableWeightUnit = WeightUnitService9.GetWeightUnitByCode(MyEntityPM.ChargeableWeightUnitCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.VolumeUnitCode != null)
				   {
					   VolumeUnitQueryService VolumeUnitService10 = new VolumeUnitQueryService(Tenant);
					   					   temp.VolumeUnit = VolumeUnitService10.GetVolumeUnitByCode(MyEntityPM.VolumeUnitCode,Tenant); 
			       
					   				   }
				   
				   temp.DescriptionOfGoods = MyEntityPM.DescriptionOfGoods;
				   temp.Commodity = MyEntityPM.AWBCommodityItemNumber;			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService11 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService11.GetBranchById(MyEntityPM.BranchId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.DepartmentId != null)
				   {
					   DepartmentQueryService DepartmentService12 = new DepartmentQueryService(Tenant);
					   					   temp.Department = DepartmentService12.GetDepartmentById(MyEntityPM.DepartmentId,Tenant); 
			       
					   				   }
				   
				   temp.MasterNumber = MyEntityPM.Master;			  
				   if(MyEntityPM.MainCarriageCarrierId != null)
				   {
					   CardQueryService CardService13 = new CardQueryService(Tenant);
					   					   temp.MainCarriageCarrier = CardService13.GetCardById(MyEntityPM.MainCarriageCarrierId,Tenant); 
			       
					   				   }
				   
				   temp.MainCarriageCarrierNumber = MyEntityPM.MainCarriageCarrierNumber;			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService14 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService14.GetUserById(MyEntityPM.CreatedByUserId,Tenant); 
			       
					   				   }
				   
				if(MyEntityPM.ShipmentDeliveries != null && MyEntityPM.ShipmentDeliveries.Count > 0)
				{
					 DeliveryQueryService DeliveryService15 = new DeliveryQueryService(Tenant);
					 temp.Deliveries = DeliveryService15.DeliveryDataMapping(MyEntityPM.ShipmentDeliveries,Tenant);
				}

							 
				if(MyEntityPM.ShipmentPickUps != null && MyEntityPM.ShipmentPickUps.Count > 0)
				{
					 PickUpQueryService PickUpService15 = new PickUpQueryService(Tenant);
					 temp.PickUps = PickUpService15.PickUpDataMapping(MyEntityPM.ShipmentPickUps,Tenant);
				}

							 				
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				temp.CustomFields = customFieldService.CustomFieldCustomDataMapping(MyEntityPM, Tenant);
				 
				   
				if(MyEntityPM.ShipmentConsoleShipments != null && MyEntityPM.ShipmentConsoleShipments.Count > 0)
				{
					 HouseQueryService HouseService16 = new HouseQueryService(Tenant);
					 temp.Houses = HouseService16.HouseCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentConsoleShipments,Tenant);
				}

							 
				   temp.IsOperationalClosed = MyEntityPM.IsOperationalClosed;			  
				   if(MyEntityPM.MainCarriageVesselId != null)
				   {
					   VesselQueryService VesselService16 = new VesselQueryService(Tenant);
					   					   temp.Vessel = VesselService16.GetVesselById(MyEntityPM.MainCarriageVesselId,Tenant); 
			       
					   				   }
				   
				   temp.MainCarriageATA = MyEntityPM.MainCarriageATA;
				   temp.MainCarriageATD = MyEntityPM.MainCarriageATD;
				   temp.IsAccountingClosed = MyEntityPM.IsAccountingClosed;
				if(MyEntityPM.ShipmentReceivables != null && MyEntityPM.ShipmentReceivables.Count > 0)
				{
					 ReceivableQueryService ReceivableService17 = new ReceivableQueryService(Tenant);
					 temp.Receivables = ReceivableService17.ReceivableDataMapping(MyEntityPM.ShipmentReceivables,Tenant);
				}

							 
				if(MyEntityPM.ShipmentPayables != null && MyEntityPM.ShipmentPayables.Count > 0)
				{
					 PayableQueryService PayableService17 = new PayableQueryService(Tenant);
					 temp.Payables = PayableService17.PayableDataMapping(MyEntityPM.ShipmentPayables,Tenant);
				}

							 			  
				   if(MyEntityPM.DimensionsUnitCode != null)
				   {
					   DimensionsUnitQueryService DimensionsUnitService17 = new DimensionsUnitQueryService(Tenant);
					   					   temp.DimensionsUnit = DimensionsUnitService17.GetDimensionsUnitByCode(MyEntityPM.DimensionsUnitCode,Tenant); 
			       
					   				   }
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ShipmentPM MasterDataMappingAndValidatin(Master MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new ShipmentPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("Shipment with Id " + MyEntity.Id + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}					DirectionQueryService DirectionDirectionService = new DirectionQueryService(Tenant);
					if(MyEntity.Direction != null)
					{
						var myDirectionPM = DirectionDirectionService.DirectionDataMappingAndValidatin(MyEntity.Direction,Tenant,ComputingPartnerName);
												if(myDirectionPM != null)
						{
							temp.DirectionId = myDirectionPM.Id;
						}
						 
					}
			
										TransportModeQueryService TransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.TransportMode != null)
					{
						var myTransportModePM = TransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.TransportMode,Tenant,ComputingPartnerName);
												if(myTransportModePM != null)
						{
							temp.TransportModeId = myTransportModePM.Id;
						}
						 
					}
			
										ShipmentTypeQueryService ShipmentTypeShipmentTypeService = new ShipmentTypeQueryService(Tenant);
					if(MyEntity.ShipmentType != null)
					{
						var myShipmentTypePM = ShipmentTypeShipmentTypeService.ShipmentTypeCustomDataMappingAndValidatin(MyEntity.ShipmentType,Tenant);
												if(myShipmentTypePM != null)
						{
							temp.ShipmentTypeId = myShipmentTypePM.Id;
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
			
					
					temp.ShipperReference1 = MyEntity.ShipperReference1;
					temp.ShipperReference2 = MyEntity.ShipperReference2;					CardQueryService ConsigneeCardService = new CardQueryService(Tenant);
					if(MyEntity.Consignee != null)
					{
						var myConsigneePM = ConsigneeCardService.CardDataMappingAndValidatin(MyEntity.Consignee,Tenant,ComputingPartnerName);
												if(myConsigneePM != null)
						{
							temp.ConsigneeId = myConsigneePM.Id;
						}
						 
					}
			
					
					temp.ConsigneeReference1 = MyEntity.ConsigneeReference1;
					temp.ConsigneeReference2 = MyEntity.ConsigneeReference2;					CardQueryService AgentCardService = new CardQueryService(Tenant);
					if(MyEntity.Agent != null)
					{
						var myAgentPM = AgentCardService.CardDataMappingAndValidatin(MyEntity.Agent,Tenant,ComputingPartnerName);
												if(myAgentPM != null)
						{
							temp.AgentId = myAgentPM.Id;
						}
						 
					}
			
					
					temp.AgentReference1 = MyEntity.AgentReference1;
					temp.AgentReference2 = MyEntity.AgentReference2;					PortQueryService FromPortPortService = new PortQueryService(Tenant);
					if(MyEntity.FromPort != null)
					{
						var myFromPortPM = FromPortPortService.PortDataMappingAndValidatin(MyEntity.FromPort,Tenant,ComputingPartnerName);
												if(myFromPortPM != null)
						{
							temp.FromPortId = myFromPortPM.Id;
						}
						 
					}
			
										PortQueryService ToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.ToPort != null)
					{
						var myToPortPM = ToPortPortService.PortDataMappingAndValidatin(MyEntity.ToPort,Tenant,ComputingPartnerName);
												if(myToPortPM != null)
						{
							temp.ToPortId = myToPortPM.Id;
						}
						 
					}
			
										WeightUnitQueryService GrossWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.GrossWeightUnit != null)
					{
						var myGrossWeightUnitPM = GrossWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.GrossWeightUnit,Tenant,ComputingPartnerName);
												if(myGrossWeightUnitPM != null)
						{
							temp.GrossWeightUnitCode = myGrossWeightUnitPM.Code;
						}
						 
					}
			
										WeightUnitQueryService ChargeableWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.ChargeableWeightUnit != null)
					{
						var myChargeableWeightUnitPM = ChargeableWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.ChargeableWeightUnit,Tenant,ComputingPartnerName);
												if(myChargeableWeightUnitPM != null)
						{
							temp.ChargeableWeightUnitCode = myChargeableWeightUnitPM.Code;
						}
						 
					}
			
										VolumeUnitQueryService VolumeUnitVolumeUnitService = new VolumeUnitQueryService(Tenant);
					if(MyEntity.VolumeUnit != null)
					{
						var myVolumeUnitPM = VolumeUnitVolumeUnitService.VolumeUnitDataMappingAndValidatin(MyEntity.VolumeUnit,Tenant,ComputingPartnerName);
												if(myVolumeUnitPM != null)
						{
							temp.VolumeUnitCode = myVolumeUnitPM.Code;
						}
						 
					}
			
					
					temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;
					temp.AWBCommodityItemNumber = MyEntity.Commodity;					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName);
												if(myBranchPM != null)
						{
							temp.BranchId = myBranchPM.Id;
						}
						 
					}
			
										DepartmentQueryService DepartmentDepartmentService = new DepartmentQueryService(Tenant);
					if(MyEntity.Department != null)
					{
						var myDepartmentPM = DepartmentDepartmentService.DepartmentDataMappingAndValidatin(MyEntity.Department,Tenant,ComputingPartnerName);
												if(myDepartmentPM != null)
						{
							temp.DepartmentId = myDepartmentPM.Id;
						}
						 
					}
			
					
					temp.Master = MyEntity.MasterNumber;					CardQueryService MainCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageCarrier != null)
					{
						var myMainCarriageCarrierPM = MainCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier,Tenant,ComputingPartnerName);
												if(myMainCarriageCarrierPM != null)
						{
							temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
						}
						 
					}
			
					
					temp.MainCarriageCarrierNumber = MyEntity.MainCarriageCarrierNumber;					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName);
												if(myCreatedByUserPM != null)
						{
							temp.CreatedByUserId = myCreatedByUserPM.Id;
						}
						 
					}
			
					 

					if(MyEntity.Deliveries != null && MyEntity.Deliveries.Count > 0)
					{
						DeliveryQueryService DeliveryService18 = new DeliveryQueryService(Tenant);
						temp.ShipmentDeliveries = DeliveryService18.DeliveryDataMappingAndValidatin(MyEntity.Deliveries,Tenant,ComputingPartnerName);
					}

								  

					if(MyEntity.PickUps != null && MyEntity.PickUps.Count > 0)
					{
						PickUpQueryService PickUpService18 = new PickUpQueryService(Tenant);
						temp.ShipmentPickUps = PickUpService18.PickUpDataMappingAndValidatin(MyEntity.PickUps,Tenant,ComputingPartnerName);
					}

								 
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					 

					if(MyEntity.Houses != null && MyEntity.Houses.Count > 0)
					{
						HouseQueryService HouseService18 = new HouseQueryService(Tenant);
						temp.ShipmentConsoleShipments = HouseService18.HouseCustomDataMappingAndValidatin(MyEntity,MyEntity.Houses,Tenant,ComputingPartnerName);
					}

								 
					temp.IsOperationalClosed = MyEntity.IsOperationalClosed;					VesselQueryService VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Vessel != null)
					{
						var myVesselPM = VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Vessel,Tenant,ComputingPartnerName);
												if(myVesselPM != null)
						{
							temp.MainCarriageVesselId = myVesselPM.Id;
						}
						 
					}
			
					
					temp.MainCarriageATA = MyEntity.MainCarriageATA;
					temp.MainCarriageATD = MyEntity.MainCarriageATD;
					temp.IsAccountingClosed = MyEntity.IsAccountingClosed; 

					if(MyEntity.Receivables != null && MyEntity.Receivables.Count > 0)
					{
						ReceivableQueryService ReceivableService18 = new ReceivableQueryService(Tenant);
						temp.ShipmentReceivables = ReceivableService18.ReceivableDataMappingAndValidatin(MyEntity.Receivables,Tenant,ComputingPartnerName);
					}

								  

					if(MyEntity.Payables != null && MyEntity.Payables.Count > 0)
					{
						PayableQueryService PayableService18 = new PayableQueryService(Tenant);
						temp.ShipmentPayables = PayableService18.PayableDataMappingAndValidatin(MyEntity.Payables,Tenant,ComputingPartnerName);
					}

								 					DimensionsUnitQueryService DimensionsUnitDimensionsUnitService = new DimensionsUnitQueryService(Tenant);
					if(MyEntity.DimensionsUnit != null)
					{
						var myDimensionsUnitPM = DimensionsUnitDimensionsUnitService.DimensionsUnitDataMappingAndValidatin(MyEntity.DimensionsUnit,Tenant,ComputingPartnerName);
												if(myDimensionsUnitPM != null)
						{
							temp.DimensionsUnitCode = myDimensionsUnitPM.Code;
						}
						 
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