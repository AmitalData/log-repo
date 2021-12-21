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

		
		public Master GetMasterById(string Id,int Tenant, string include, string ComputingPartnerName = "")
        { 
		    try
            {
								
				var temp = query.GetSinglePM(Id, Tenant, include);
				 if (temp == null)
                    throw new ApplicationException("Shipment with Id " + Id + " doesn't exist");

				return MasterDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Master GetMasterByShipmentNumber(string ShipmentNumber,int Tenant, string include, string ComputingPartnerName = "")
        { 
		    try
            {
								
				var temp = query.GetSinglePMByShipmentNumber(ShipmentNumber, Tenant, include);
				 if (temp == null)
                    throw new ApplicationException("Shipment with ShipmentNumber " + ShipmentNumber + " doesn't exist");

				return MasterDataMapping(temp,Tenant,ComputingPartnerName);
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
					   					   temp.Direction = DirectionService0.GetDirectionById(MyEntityPM.DirectionId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService1 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService1.GetTransportModeById(MyEntityPM.TransportModeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipmentTypeId != null)
				   {
					   ShipmentTypeQueryService ShipmentTypeService2 = new ShipmentTypeQueryService(Tenant);
					   					   temp.ShipmentType = ShipmentTypeService2.ShipmentTypeCustomDataMapping(MyEntityPM.ShipmentTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipperId != null)
				   {
					   CardQueryService CardService3 = new CardQueryService(Tenant);
					   					   temp.Shipper = CardService3.GetCardById(MyEntityPM.ShipperId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ShipperReference1 = MyEntityPM.ShipperReference1;
				   temp.ShipperReference2 = MyEntityPM.ShipperReference2; 

			  
				   if(MyEntityPM.ConsigneeId != null)
				   {
					   CardQueryService CardService4 = new CardQueryService(Tenant);
					   					   temp.Consignee = CardService4.GetCardById(MyEntityPM.ConsigneeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ConsigneeReference1 = MyEntityPM.ConsigneeReference1;
				   temp.ConsigneeReference2 = MyEntityPM.ConsigneeReference2; 

			  
				   if(MyEntityPM.AgentId != null)
				   {
					   CardQueryService CardService5 = new CardQueryService(Tenant);
					   					   temp.Agent = CardService5.GetCardById(MyEntityPM.AgentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.AgentReference1 = MyEntityPM.AgentReference1;
				   temp.AgentReference2 = MyEntityPM.AgentReference2; 

			  
				   if(MyEntityPM.FromPortId != null)
				   {
					   PortQueryService PortService6 = new PortQueryService(Tenant);
					   					   temp.FromPort = PortService6.GetPortById(MyEntityPM.FromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ToPortId != null)
				   {
					   PortQueryService PortService7 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService7.GetPortById(MyEntityPM.ToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.GrossWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService8 = new WeightUnitQueryService(Tenant);
					   					   temp.GrossWeightUnit = WeightUnitService8.GetWeightUnitByCode(MyEntityPM.GrossWeightUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ChargeableWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService9 = new WeightUnitQueryService(Tenant);
					   					   temp.ChargeableWeightUnit = WeightUnitService9.GetWeightUnitByCode(MyEntityPM.ChargeableWeightUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.VolumeUnitCode != null)
				   {
					   VolumeUnitQueryService VolumeUnitService10 = new VolumeUnitQueryService(Tenant);
					   					   temp.VolumeUnit = VolumeUnitService10.GetVolumeUnitByCode(MyEntityPM.VolumeUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.DescriptionOfGoods = MyEntityPM.DescriptionOfGoods;
				   temp.Commodity = MyEntityPM.AWBCommodityItemNumber; 

			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService11 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService11.GetBranchById(MyEntityPM.BranchId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DepartmentId != null)
				   {
					   DepartmentQueryService DepartmentService12 = new DepartmentQueryService(Tenant);
					   					   temp.Department = DepartmentService12.GetDepartmentById(MyEntityPM.DepartmentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.MasterNumber = MyEntityPM.Master; 

			  
				   if(MyEntityPM.MainCarriageCarrierId != null)
				   {
					   CardQueryService CardService13 = new CardQueryService(Tenant);
					   					   temp.MainCarriageCarrier = CardService13.GetCardById(MyEntityPM.MainCarriageCarrierId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.MainCarriageCarrierNumber = MyEntityPM.MainCarriageCarrierNumber; 

			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService14 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService14.GetUserById(MyEntityPM.CreatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				if(MyEntityPM.ShipmentDeliveries != null && MyEntityPM.ShipmentDeliveries.Count > 0)
				{
					 DeliveryQueryService DeliveryService15 = new DeliveryQueryService(Tenant);
					 temp.Deliveries = DeliveryService15.DeliveryDataMapping(MyEntityPM.ShipmentDeliveries,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ShipmentPickUps != null && MyEntityPM.ShipmentPickUps.Count > 0)
				{
					 PickUpQueryService PickUpService15 = new PickUpQueryService(Tenant);
					 temp.PickUps = PickUpService15.PickUpDataMapping(MyEntityPM.ShipmentPickUps,Tenant,ComputingPartnerName);
				}

							  

				
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				temp.CustomFields = customFieldService.CustomFieldCustomDataMapping(MyEntityPM, Tenant);
				 
				   
				if(MyEntityPM.ShipmentConsoleShipments != null && MyEntityPM.ShipmentConsoleShipments.Count > 0)
				{
					 HouseQueryService HouseService16 = new HouseQueryService(Tenant);
					 temp.Houses = HouseService16.HouseCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentConsoleShipments,Tenant,ComputingPartnerName);
				}

							 
				   temp.IsOperationalClosed = MyEntityPM.IsOperationalClosed; 

			  
				   if(MyEntityPM.MainCarriageVesselId != null)
				   {
					   VesselQueryService VesselService16 = new VesselQueryService(Tenant);
					   					   temp.Vessel = VesselService16.GetVesselById(MyEntityPM.MainCarriageVesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.MainCarriageATA = MyEntityPM.MainCarriageATA;
				   temp.MainCarriageATD = MyEntityPM.MainCarriageATD;
				   temp.IsAccountingClosed = MyEntityPM.IsAccountingClosed;
				if(MyEntityPM.ShipmentReceivables != null && MyEntityPM.ShipmentReceivables.Count > 0)
				{
					 ReceivableQueryService ReceivableService17 = new ReceivableQueryService(Tenant);
					 temp.Receivables = ReceivableService17.ReceivableDataMapping(MyEntityPM.ShipmentReceivables,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ShipmentPayables != null && MyEntityPM.ShipmentPayables.Count > 0)
				{
					 PayableQueryService PayableService17 = new PayableQueryService(Tenant);
					 temp.Payables = PayableService17.PayableDataMapping(MyEntityPM.ShipmentPayables,Tenant,ComputingPartnerName);
				}

							  

			  
				   if(MyEntityPM.DimensionsUnitCode != null)
				   {
					   DimensionsUnitQueryService DimensionsUnitService17 = new DimensionsUnitQueryService(Tenant);
					   					   temp.DimensionsUnit = DimensionsUnitService17.GetDimensionsUnitByCode(MyEntityPM.DimensionsUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.OrderNumberOfPackages = MyEntityPM.BookingNumberOfPackages;
				   temp.OrderGrossWeight = MyEntityPM.OrderGrossWeight;
				   temp.OrderVolume = MyEntityPM.BookingVolume;
				   temp.OrderChargeableWeight = MyEntityPM.OrderChargeableWeight;
				   temp.OrderIsDangerouseGoods = MyEntityPM.OrderIsDangerouseGoods;
				   temp.MainHarmonize = MyEntityPM.MainHarmonize; 

			  
				   if(MyEntityPM.SalesmanUserId != null)
				   {
					   UserQueryService UserService18 = new UserQueryService(Tenant);
					   					   temp.Salesman = UserService18.GetUserById(MyEntityPM.SalesmanUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.AccountManagerUserId != null)
				   {
					   UserQueryService UserService19 = new UserQueryService(Tenant);
					   					   temp.AccountManager = UserService19.GetUserById(MyEntityPM.AccountManagerUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.SpecialServicesTypeId != null)
				   {
					   SpecialServicesTypeQueryService SpecialServicesTypeService20 = new SpecialServicesTypeQueryService(Tenant);
					   					   temp.SpecialServicesType = SpecialServicesTypeService20.GetSpecialServicesTypeById(MyEntityPM.SpecialServicesTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipperNotExporterId != null)
				   {
					   CardQueryService CardService21 = new CardQueryService(Tenant);
					   					   temp.ShipperNotExporter = CardService21.GetCardById(MyEntityPM.ShipperNotExporterId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.CustomAgentImportId != null)
				   {
					   CardQueryService CardService22 = new CardQueryService(Tenant);
					   					   temp.CustomAgentImport = CardService22.GetCardById(MyEntityPM.CustomAgentImportId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ReleasingAgentId != null)
				   {
					   CardQueryService CardService23 = new CardQueryService(Tenant);
					   					   temp.ReleasingAgent = CardService23.GetCardById(MyEntityPM.ReleasingAgentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.FreightForwarderId != null)
				   {
					   CardQueryService CardService24 = new CardQueryService(Tenant);
					   					   temp.FreightForwarder = CardService24.GetCardById(MyEntityPM.FreightForwarderId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.MasterDate = MyEntityPM.MAWBOBLDate;
				   temp.Ratio = MyEntityPM.Ratio;
				if(MyEntityPM.MainCarriageLegs != null && MyEntityPM.MainCarriageLegs.Count > 0)
				{
					 MainCarriageLegQueryService MainCarriageLegService25 = new MainCarriageLegQueryService(Tenant);
					 temp.MainCarriageLegs = MainCarriageLegService25.MainCarriageLegDataMapping(MyEntityPM.MainCarriageLegs,Tenant,ComputingPartnerName);
				}

							 
				   temp.ShipmentNumber = MyEntityPM.ShipmentNumber;
				   temp.ConcurrencyGUID = MyEntityPM.ConcurrencyGUID; 

			  
				   if(MyEntityPM.StatusId != null)
				   {
					   EntityStatusQueryService EntityStatusService25 = new EntityStatusQueryService(Tenant);
					   					   temp.Status = EntityStatusService25.GetEntityStatusById(MyEntityPM.StatusId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.BookingConfirmationNumber = MyEntityPM.BookingConfirmationNumber;
				   temp.EstimatedFinalArrivalDate = MyEntityPM.EstimatedFinalArrivalDate;
				   temp.ActualFinalArrivalDate = MyEntityPM.ActualFinalArrivalDate;
				   temp.IsHTSMissing = MyEntityPM.IsHTSMissing;
				   temp.PlannedCargoReadyDate = MyEntityPM.PlannedCargoReadyDate;
				   temp.ApprovedCargoReadyDate = MyEntityPM.ApprovedCargoReadyDate; 

			  
				   if(MyEntityPM.HandlerUserId != null)
				   {
					   UserQueryService UserService26 = new UserQueryService(Tenant);
					   					   temp.HandlerUser = UserService26.GetUserById(MyEntityPM.HandlerUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Notify1Reference = MyEntityPM.Notify1Reference;
				   temp.Notify1Reference2 = MyEntityPM.Notify1Reference2;
				   temp.ShipperNotExporterReference1 = MyEntityPM.ShipperNotExporterReference1;
				   temp.ShipperNotExporterReference2 = MyEntityPM.ShipperNotExporterReference2;
				   temp.CustomsClearanceDate = MyEntityPM.CustomsClearanceDate; 

			  
				   if(MyEntityPM.Notify1Id != null)
				   {
					   CardQueryService CardService27 = new CardQueryService(Tenant);
					   					   temp.Notify1 = CardService27.GetCardById(MyEntityPM.Notify1Id,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				if(MyEntityPM.EventList != null && MyEntityPM.EventList.Count > 0)
				{
					 EventQueryService EventService28 = new EventQueryService(Tenant);
					 temp.EventList = EventService28.EventCustomDataMapping(MyEntityPM,MyEntityPM.EventList,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.AddManualEvents != null && MyEntityPM.AddManualEvents.Count > 0)
				{
					 EventQueryService EventService28 = new EventQueryService(Tenant);
					 temp.AddManualEvents = EventService28.EventCustomDataMapping(MyEntityPM,MyEntityPM.AddManualEvents,Tenant,ComputingPartnerName);
				}

							  

			  
				   if(MyEntityPM.UnassignedShipperAddressId != null)
				   {
					   AddressQueryService AddressService28 = new AddressQueryService(Tenant);
					   					   temp.UnassignedShipperAddress = AddressService28.AddressCustomDataMapping(MyEntityPM.UnassignedShipperAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.UnassignedConsigneeAddressId != null)
				   {
					   AddressQueryService AddressService29 = new AddressQueryService(Tenant);
					   					   temp.UnassignedConsigneeAddress = AddressService29.AddressCustomDataMapping(MyEntityPM.UnassignedConsigneeAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ShipmentPM MasterDataMappingAndValidatin(Master MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new ShipmentPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.ShipmentNumber))
					{
						temp = query.GetSinglePMByShipmentNumber(MyEntity.ShipmentNumber, Tenant  );
					} 					   
					if(temp == null)
					{   
					    throw new ApplicationException("Shipment with ShipmentNumber " + MyEntity.ShipmentNumber + " doesn't exist");
					} 
										 
					if(IsUpdate == true)
					{
					    
					      temp.NewConcurrencyGUID = Guid.NewGuid().ToString(); 
						
					}
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Shipment with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
					DirectionQueryService DirectionDirectionService = new DirectionQueryService(Tenant);
					if(MyEntity.Direction != null)
					{
						var myDirectionPM = DirectionDirectionService.DirectionDataMappingAndValidatin(MyEntity.Direction,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDirectionPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.DirectionId = myDirectionPM.Id;
						  
							}  

							
						} 

					}
			
					
					TransportModeQueryService TransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.TransportMode != null)
					{
						var myTransportModePM = TransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.TransportMode,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransportModePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.TransportModeId = myTransportModePM.Id;
						  
							}  

							
						} 

					}
			
					
					ShipmentTypeQueryService ShipmentTypeShipmentTypeService = new ShipmentTypeQueryService(Tenant);
					if(MyEntity.ShipmentType != null)
					{
						var myShipmentTypePM = ShipmentTypeShipmentTypeService.ShipmentTypeCustomDataMappingAndValidatin(MyEntity.ShipmentType,Tenant);
						
						if(myShipmentTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ShipmentTypeId = myShipmentTypePM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService ShipperCardService = new CardQueryService(Tenant);
					if(MyEntity.Shipper != null)
					{
						var myShipperPM = ShipperCardService.CardDataMappingAndValidatin(MyEntity.Shipper,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myShipperPM != null)
						{ 

						 								
								temp.ShipperId = myShipperPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.ShipperReference1 = MyEntity.ShipperReference1;

					 

					
                    							
						temp.ShipperReference2 = MyEntity.ShipperReference2;

					 

					
					CardQueryService ConsigneeCardService = new CardQueryService(Tenant);
					if(MyEntity.Consignee != null)
					{
						var myConsigneePM = ConsigneeCardService.CardDataMappingAndValidatin(MyEntity.Consignee,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myConsigneePM != null)
						{ 

						 								
								temp.ConsigneeId = myConsigneePM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.ConsigneeReference1 = MyEntity.ConsigneeReference1;

					 

					
                    							
						temp.ConsigneeReference2 = MyEntity.ConsigneeReference2;

					 

					
					CardQueryService AgentCardService = new CardQueryService(Tenant);
					if(MyEntity.Agent != null)
					{
						var myAgentPM = AgentCardService.CardDataMappingAndValidatin(MyEntity.Agent,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAgentPM != null)
						{ 

						 								
								temp.AgentId = myAgentPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.AgentReference1 = MyEntity.AgentReference1;

					 

					
                    							
						temp.AgentReference2 = MyEntity.AgentReference2;

					 

					
					PortQueryService FromPortPortService = new PortQueryService(Tenant);
					if(MyEntity.FromPort != null)
					{
						var myFromPortPM = FromPortPortService.PortDataMappingAndValidatin(MyEntity.FromPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFromPortPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.FromPortId = myFromPortPM.Id;
						  
							}  

							
						} 

					}
			
					
					PortQueryService ToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.ToPort != null)
					{
						var myToPortPM = ToPortPortService.PortDataMappingAndValidatin(MyEntity.ToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myToPortPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ToPortId = myToPortPM.Id;
						  
							}  

							
						} 

					}
			
					
					WeightUnitQueryService GrossWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.GrossWeightUnit != null)
					{
						var myGrossWeightUnitPM = GrossWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.GrossWeightUnit,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myGrossWeightUnitPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.GrossWeightUnitCode = myGrossWeightUnitPM.Code;
						  
							}  

							
						} 

					}
			
					
					WeightUnitQueryService ChargeableWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.ChargeableWeightUnit != null)
					{
						var myChargeableWeightUnitPM = ChargeableWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.ChargeableWeightUnit,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myChargeableWeightUnitPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ChargeableWeightUnitCode = myChargeableWeightUnitPM.Code;
						  
							}  

							
						} 

					}
			
					
					VolumeUnitQueryService VolumeUnitVolumeUnitService = new VolumeUnitQueryService(Tenant);
					if(MyEntity.VolumeUnit != null)
					{
						var myVolumeUnitPM = VolumeUnitVolumeUnitService.VolumeUnitDataMappingAndValidatin(MyEntity.VolumeUnit,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myVolumeUnitPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.VolumeUnitCode = myVolumeUnitPM.Code;
						  
							}  

							
						} 

					}
			
					
                    							
						temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;

					 

					
                    
					if(!IsUpdate)
					{							
						temp.AWBCommodityItemNumber = MyEntity.Commodity;

										}  

					
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myBranchPM != null)
						{ 

						 								
								temp.BranchId = myBranchPM.Id;
						  

							
						} 

					}
			
					
					DepartmentQueryService DepartmentDepartmentService = new DepartmentQueryService(Tenant);
					if(MyEntity.Department != null)
					{
						var myDepartmentPM = DepartmentDepartmentService.DepartmentDataMappingAndValidatin(MyEntity.Department,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDepartmentPM != null)
						{ 

						 								
								temp.DepartmentId = myDepartmentPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.Master = MyEntity.MasterNumber;

					 

					
					CardQueryService MainCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageCarrier != null)
					{
						var myMainCarriageCarrierPM = MainCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainCarriageCarrierPM != null)
						{ 

						 								
								temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.MainCarriageCarrierNumber = MyEntity.MainCarriageCarrierNumber;

					 

					
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCreatedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CreatedByUserId = myCreatedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					 

					if(MyEntity.Deliveries != null && MyEntity.Deliveries.Count > 0)
					{
						DeliveryQueryService DeliveryService30 = new DeliveryQueryService(Tenant);
						 								
							temp.ShipmentDeliveries = DeliveryService30.DeliveryDataMappingAndValidatin(MyEntity.Deliveries,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								  

					if(MyEntity.PickUps != null && MyEntity.PickUps.Count > 0)
					{
						PickUpQueryService PickUpService30 = new PickUpQueryService(Tenant);
						 								
							temp.ShipmentPickUps = PickUpService30.PickUpDataMappingAndValidatin(MyEntity.PickUps,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					 

					if(MyEntity.Houses != null && MyEntity.Houses.Count > 0)
					{
						HouseQueryService HouseService30 = new HouseQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ShipmentConsoleShipments = HouseService30.HouseCustomDataMappingAndValidatin(MyEntity,MyEntity.Houses,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)
					{							
						temp.IsOperationalClosed = MyEntity.IsOperationalClosed;

										}  

					
					VesselQueryService VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Vessel != null)
					{
						var myVesselPM = VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Vessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myVesselPM != null)
						{ 

						 								
								temp.MainCarriageVesselId = myVesselPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.MainCarriageATA = MyEntity.MainCarriageATA;

					 

					
                    							
						temp.MainCarriageATD = MyEntity.MainCarriageATD;

					 

					
                    
					if(!IsUpdate)
					{							
						temp.IsAccountingClosed = MyEntity.IsAccountingClosed;

										}  

					 

					if(MyEntity.Receivables != null && MyEntity.Receivables.Count > 0)
					{
						ReceivableQueryService ReceivableService30 = new ReceivableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ShipmentReceivables = ReceivableService30.ReceivableDataMappingAndValidatin(MyEntity.Receivables,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.Payables != null && MyEntity.Payables.Count > 0)
					{
						PayableQueryService PayableService30 = new PayableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ShipmentPayables = PayableService30.PayableDataMappingAndValidatin(MyEntity.Payables,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
					DimensionsUnitQueryService DimensionsUnitDimensionsUnitService = new DimensionsUnitQueryService(Tenant);
					if(MyEntity.DimensionsUnit != null)
					{
						var myDimensionsUnitPM = DimensionsUnitDimensionsUnitService.DimensionsUnitDataMappingAndValidatin(MyEntity.DimensionsUnit,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDimensionsUnitPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.DimensionsUnitCode = myDimensionsUnitPM.Code;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.BookingNumberOfPackages = MyEntity.OrderNumberOfPackages;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.OrderGrossWeight = MyEntity.OrderGrossWeight;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.BookingVolume = MyEntity.OrderVolume;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.OrderChargeableWeight = MyEntity.OrderChargeableWeight;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.OrderIsDangerouseGoods = MyEntity.OrderIsDangerouseGoods;

										}  

					
                    							
						temp.MainHarmonize = MyEntity.MainHarmonize;

					 

					
					UserQueryService SalesmanUserService = new UserQueryService(Tenant);
					if(MyEntity.Salesman != null)
					{
						var mySalesmanPM = SalesmanUserService.UserDataMappingAndValidatin(MyEntity.Salesman,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySalesmanPM != null)
						{ 

						 								
								temp.SalesmanUserId = mySalesmanPM.Id;
						  

							
						} 

					}
			
					
					UserQueryService AccountManagerUserService = new UserQueryService(Tenant);
					if(MyEntity.AccountManager != null)
					{
						var myAccountManagerPM = AccountManagerUserService.UserDataMappingAndValidatin(MyEntity.AccountManager,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAccountManagerPM != null)
						{ 

						 								
								temp.AccountManagerUserId = myAccountManagerPM.Id;
						  

							
						} 

					}
			
					
					SpecialServicesTypeQueryService SpecialServicesTypeSpecialServicesTypeService = new SpecialServicesTypeQueryService(Tenant);
					if(MyEntity.SpecialServicesType != null)
					{
						var mySpecialServicesTypePM = SpecialServicesTypeSpecialServicesTypeService.SpecialServicesTypeDataMappingAndValidatin(MyEntity.SpecialServicesType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySpecialServicesTypePM != null)
						{ 

						 								
								temp.SpecialServicesTypeId = mySpecialServicesTypePM.Id;
						  

							
						} 

					}
			
					
					CardQueryService ShipperNotExporterCardService = new CardQueryService(Tenant);
					if(MyEntity.ShipperNotExporter != null)
					{
						var myShipperNotExporterPM = ShipperNotExporterCardService.CardDataMappingAndValidatin(MyEntity.ShipperNotExporter,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myShipperNotExporterPM != null)
						{ 

						 								
								temp.ShipperNotExporterId = myShipperNotExporterPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService CustomAgentImportCardService = new CardQueryService(Tenant);
					if(MyEntity.CustomAgentImport != null)
					{
						var myCustomAgentImportPM = CustomAgentImportCardService.CardDataMappingAndValidatin(MyEntity.CustomAgentImport,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCustomAgentImportPM != null)
						{ 

						 								
								temp.CustomAgentImportId = myCustomAgentImportPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService ReleasingAgentCardService = new CardQueryService(Tenant);
					if(MyEntity.ReleasingAgent != null)
					{
						var myReleasingAgentPM = ReleasingAgentCardService.CardDataMappingAndValidatin(MyEntity.ReleasingAgent,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myReleasingAgentPM != null)
						{ 

						 								
								temp.ReleasingAgentId = myReleasingAgentPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService FreightForwarderCardService = new CardQueryService(Tenant);
					if(MyEntity.FreightForwarder != null)
					{
						var myFreightForwarderPM = FreightForwarderCardService.CardDataMappingAndValidatin(MyEntity.FreightForwarder,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFreightForwarderPM != null)
						{ 

						 								
								temp.FreightForwarderId = myFreightForwarderPM.Id;
						  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.MAWBOBLDate = MyEntity.MasterDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Ratio = MyEntity.Ratio;

										}  

					 

					if(MyEntity.MainCarriageLegs != null && MyEntity.MainCarriageLegs.Count > 0)
					{
						MainCarriageLegQueryService MainCarriageLegService30 = new MainCarriageLegQueryService(Tenant);
						 								
							temp.MainCarriageLegs = MainCarriageLegService30.MainCarriageLegDataMappingAndValidatin(MyEntity.MainCarriageLegs,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 
                    
					if(!IsUpdate)
					{							
						temp.ShipmentNumber = MyEntity.ShipmentNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ConcurrencyGUID = MyEntity.ConcurrencyGUID;

										}  

					
					EntityStatusQueryService StatusEntityStatusService = new EntityStatusQueryService(Tenant);
					if(MyEntity.Status != null)
					{
						var myStatusPM = StatusEntityStatusService.EntityStatusDataMappingAndValidatin(MyEntity.Status,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.StatusId = myStatusPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.BookingConfirmationNumber = MyEntity.BookingConfirmationNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.EstimatedFinalArrivalDate = MyEntity.EstimatedFinalArrivalDate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ActualFinalArrivalDate = MyEntity.ActualFinalArrivalDate;

										}  

					
                    							
						temp.IsHTSMissing = MyEntity.IsHTSMissing;

					 

					
                    							
						temp.PlannedCargoReadyDate = MyEntity.PlannedCargoReadyDate;

					 

					
                    							
						temp.ApprovedCargoReadyDate = MyEntity.ApprovedCargoReadyDate;

					 

					
					UserQueryService HandlerUserUserService = new UserQueryService(Tenant);
					if(MyEntity.HandlerUser != null)
					{
						var myHandlerUserPM = HandlerUserUserService.UserDataMappingAndValidatin(MyEntity.HandlerUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myHandlerUserPM != null)
						{ 

						 								
								temp.HandlerUserId = myHandlerUserPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.Notify1Reference = MyEntity.Notify1Reference;

					 

					
                    							
						temp.Notify1Reference2 = MyEntity.Notify1Reference2;

					 

					
                    							
						temp.ShipperNotExporterReference1 = MyEntity.ShipperNotExporterReference1;

					 

					
                    							
						temp.ShipperNotExporterReference2 = MyEntity.ShipperNotExporterReference2;

					 

					
                    							
						temp.CustomsClearanceDate = MyEntity.CustomsClearanceDate;

					 

					
					CardQueryService Notify1CardService = new CardQueryService(Tenant);
					if(MyEntity.Notify1 != null)
					{
						var myNotify1PM = Notify1CardService.CardDataMappingAndValidatin(MyEntity.Notify1,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myNotify1PM != null)
						{ 

						 								
								temp.Notify1Id = myNotify1PM.Id;
						  

							
						} 

					}
			
					 

					if(MyEntity.EventList != null && MyEntity.EventList.Count > 0)
					{
						EventQueryService EventService30 = new EventQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.EventList = EventService30.EventCustomDataMappingAndValidatin(MyEntity,MyEntity.EventList,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.AddManualEvents != null && MyEntity.AddManualEvents.Count > 0)
					{
						EventQueryService EventService30 = new EventQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.AddManualEvents = EventService30.EventCustomDataMappingAndValidatin(MyEntity,MyEntity.AddManualEvents,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
					AddressQueryService UnassignedShipperAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.UnassignedShipperAddress != null)
					{
						var myUnassignedShipperAddressPM = UnassignedShipperAddressAddressService.AddressCustomDataMappingAndValidatin(MyEntity.UnassignedShipperAddress,Tenant);
						
						if(myUnassignedShipperAddressPM != null)
						{ 

						 								
								temp.UnassignedShipperAddressId = myUnassignedShipperAddressPM.Id;
						  

							
						} 

					}
			
					
					AddressQueryService UnassignedConsigneeAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.UnassignedConsigneeAddress != null)
					{
						var myUnassignedConsigneeAddressPM = UnassignedConsigneeAddressAddressService.AddressCustomDataMappingAndValidatin(MyEntity.UnassignedConsigneeAddress,Tenant);
						
						if(myUnassignedConsigneeAddressPM != null)
						{ 

						 								
								temp.UnassignedConsigneeAddressId = myUnassignedConsigneeAddressPM.Id;
						  

							
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