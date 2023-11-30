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
   public partial class HouseQueryService
   {
   
		IShipmentsContext  context;
		//ShipmentService service; 
		
		ShipmentQuery query; 

        public HouseQueryService(int tenant)
        {
				    context = ShipmentsContext.GetContext(tenant); 
			//service = new ShipmentService(context, tenant); 
			query = new ShipmentQuery(tenant);
        }

		
		public House GetHouseById(string Id,int Tenant, string include, string ComputingPartnerName = "")
        { 
		    try
            {
								
				var temp = query.GetSinglePMWithInclude(Id, Tenant, include);
				 if (temp == null)
                    throw new ApplicationException("Shipment with Id " + Id + " doesn't exist");

				return HouseDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public House GetHouseByShipmentNumber(string ShipmentNumber,int Tenant, string include, string ComputingPartnerName = "")
        { 
		    try
            {
								
				var temp = query.GetSinglePMWithIncludeByShipmentNumber(ShipmentNumber, Tenant, include);
				 if (temp == null)
                    throw new ApplicationException("Shipment with ShipmentNumber " + ShipmentNumber + " doesn't exist");

				return HouseDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public House HouseDataMapping(ShipmentPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new House(); 
				   temp.Id = MyEntityPM.Id; 

			  
				   if(MyEntityPM.ShipmentTypeId != null)
				   {
					   ShipmentTypeQueryService ShipmentTypeService0 = new ShipmentTypeQueryService(Tenant);
					   					   temp.ShipmentType = ShipmentTypeService0.ShipmentTypeCustomDataMapping(MyEntityPM.ShipmentTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService1 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService1.GetTransportModeById(MyEntityPM.TransportModeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipperId != null)
				   {
					   CardQueryService CardService2 = new CardQueryService(Tenant);
					   					   temp.Shipper = CardService2.GetCardById(MyEntityPM.ShipperId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ShipperReference1 = MyEntityPM.ShipperReference1;
				   temp.ShipperReference2 = MyEntityPM.ShipperReference2; 

			  
				   if(MyEntityPM.ConsigneeId != null)
				   {
					   CardQueryService CardService3 = new CardQueryService(Tenant);
					   					   temp.Consignee = CardService3.GetCardById(MyEntityPM.ConsigneeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ConsigneeReference1 = MyEntityPM.ConsigneeReference1;
				   temp.ConsigneeReference2 = MyEntityPM.ConsigneeReference2; 

			  
				   if(MyEntityPM.CustomerId != null)
				   {
					   CardQueryService CardService4 = new CardQueryService(Tenant);
					   					   temp.Customer = CardService4.GetCardById(MyEntityPM.CustomerId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.FromPortId != null)
				   {
					   PortQueryService PortService5 = new PortQueryService(Tenant);
					   					   temp.FromPort = PortService5.GetPortById(MyEntityPM.FromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ToPortId != null)
				   {
					   PortQueryService PortService6 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService6.GetPortById(MyEntityPM.ToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.GrossWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService7 = new WeightUnitQueryService(Tenant);
					   					   temp.GrossWeightUnit = WeightUnitService7.GetWeightUnitByCode(MyEntityPM.GrossWeightUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ChargeableWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService8 = new WeightUnitQueryService(Tenant);
					   					   temp.ChargeableWeightUnit = WeightUnitService8.GetWeightUnitByCode(MyEntityPM.ChargeableWeightUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.VolumeUnitCode != null)
				   {
					   VolumeUnitQueryService VolumeUnitService9 = new VolumeUnitQueryService(Tenant);
					   					   temp.VolumeUnit = VolumeUnitService9.GetVolumeUnitByCode(MyEntityPM.VolumeUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.IncotermId != null)
				   {
					   IncotermQueryService IncotermService10 = new IncotermQueryService(Tenant);
					   					   temp.Incoterm = IncotermService10.GetIncotermById(MyEntityPM.IncotermId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.HouseNo = MyEntityPM.House;
				   temp.HouseDate = MyEntityPM.HAWBDate;
				   temp.DescriptionOfGoods = MyEntityPM.DescriptionOfGoods;
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 AirPackageQueryService AirPackageService11 = new AirPackageQueryService(Tenant);
					 temp.AirPackages = AirPackageService11.AirPackageCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 OceanOrInlandPackageQueryService OceanOrInlandPackageService11 = new OceanOrInlandPackageQueryService(Tenant);
					 temp.OceanOrInlandPackages = OceanOrInlandPackageService11.OceanOrInlandPackageCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 ContainerQueryService ContainerService11 = new ContainerQueryService(Tenant);
					 temp.Containers = ContainerService11.ContainerCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant,ComputingPartnerName);
				}

							 
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
				   
				   temp.TEU = MyEntityPM.TEU;
				   temp.NumberOfPackages = MyEntityPM.NumberOfPackages;
				   temp.GrossWeight = MyEntityPM.GrossWeight;
				   temp.Volume = MyEntityPM.Volume;
				   temp.VolumetricWeight = MyEntityPM.VolumetricWeight;
				   temp.ChargeableWeight = MyEntityPM.ChargeableWeight;
				   temp.ShipmentNumber = MyEntityPM.ShipmentNumber; 

			  
				   if(MyEntityPM.DirectionId != null)
				   {
					   DirectionQueryService DirectionService13 = new DirectionQueryService(Tenant);
					   					   temp.Direction = DirectionService13.GetDirectionById(MyEntityPM.DirectionId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.IsCancelled = MyEntityPM.IsCancelled;
				   temp.IsOperationalClosed = MyEntityPM.IsOperationalClosed;
				   temp.IsAccountingClosed = MyEntityPM.IsAccountingClosed;
				   temp.MasterShipmentDataId = MyEntityPM.MasterShipmentDataId; 

			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService14 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService14.GetUserById(MyEntityPM.CreatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				if(MyEntityPM.ShipmentPickUps != null && MyEntityPM.ShipmentPickUps.Count > 0)
				{
					 PickUpQueryService PickUpService15 = new PickUpQueryService(Tenant);
					 temp.PickUps = PickUpService15.PickUpDataMapping(MyEntityPM.ShipmentPickUps,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ShipmentDeliveries != null && MyEntityPM.ShipmentDeliveries.Count > 0)
				{
					 DeliveryQueryService DeliveryService15 = new DeliveryQueryService(Tenant);
					 temp.Deliveries = DeliveryService15.DeliveryDataMapping(MyEntityPM.ShipmentDeliveries,Tenant,ComputingPartnerName);
				}

							  

				
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				temp.CustomFields = customFieldService.CustomFieldCustomDataMapping(MyEntityPM, Tenant);
				 
				   
				   temp.ValueOfGoods = MyEntityPM.ValueOfGoods; 

			  
				   if(MyEntityPM.ValueOfGoodsCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService16 = new CurrencyQueryService(Tenant);
					   					   temp.ValueOfGoodsCurrency = CurrencyService16.GetCurrencyById(MyEntityPM.ValueOfGoodsCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
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
				    

			  
				   if(MyEntityPM.AgentId != null)
				   {
					   CardQueryService CardService22 = new CardQueryService(Tenant);
					   					   temp.Agent = CardService22.GetCardById(MyEntityPM.AgentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.CustomAgentImportId != null)
				   {
					   CardQueryService CardService23 = new CardQueryService(Tenant);
					   					   temp.CustomAgentImport = CardService23.GetCardById(MyEntityPM.CustomAgentImportId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ReleasingAgentId != null)
				   {
					   CardQueryService CardService24 = new CardQueryService(Tenant);
					   					   temp.ReleasingAgent = CardService24.GetCardById(MyEntityPM.ReleasingAgentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.FreightForwarderId != null)
				   {
					   CardQueryService CardService25 = new CardQueryService(Tenant);
					   					   temp.FreightForwarder = CardService25.GetCardById(MyEntityPM.FreightForwarderId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Ratio = MyEntityPM.Ratio;
				   temp.ConcurrencyGUID = MyEntityPM.ConcurrencyGUID;
				   temp.CustomerReference1 = MyEntityPM.CustomerReference1; 

			  
				   if(MyEntityPM.StatusId != null)
				   {
					   EntityStatusQueryService EntityStatusService26 = new EntityStatusQueryService(Tenant);
					   					   temp.Status = EntityStatusService26.GetEntityStatusById(MyEntityPM.StatusId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.BookingConfirmationNumber = MyEntityPM.BookingConfirmationNumber;
				   temp.EstimatedFinalArrivalDate = MyEntityPM.EstimatedFinalArrivalDate;
				   temp.ActualFinalArrivalDate = MyEntityPM.ActualFinalArrivalDate;
				   temp.IsHTSMissing = MyEntityPM.IsHTSMissing;
				   temp.PlannedCargoReadyDate = MyEntityPM.PlannedCargoReadyDate;
				   temp.ApprovedCargoReadyDate = MyEntityPM.ApprovedCargoReadyDate; 

			  
				   if(MyEntityPM.HandlerUserId != null)
				   {
					   UserQueryService UserService27 = new UserQueryService(Tenant);
					   					   temp.HandlerUser = UserService27.GetUserById(MyEntityPM.HandlerUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Notify1Reference = MyEntityPM.Notify1Reference;
				   temp.Notify1Reference2 = MyEntityPM.Notify1Reference2;
				   temp.ShipperNotExporterReference1 = MyEntityPM.ShipperNotExporterReference1;
				   temp.ShipperNotExporterReference2 = MyEntityPM.ShipperNotExporterReference2;
				   temp.CustomsClearanceDate = MyEntityPM.CustomsClearanceDate; 

			  
				   if(MyEntityPM.Notify1Id != null)
				   {
					   CardQueryService CardService28 = new CardQueryService(Tenant);
					   					   temp.Notify1 = CardService28.GetCardById(MyEntityPM.Notify1Id,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				if(MyEntityPM.EventList != null && MyEntityPM.EventList.Count > 0)
				{
					 EventQueryService EventService29 = new EventQueryService(Tenant);
					 temp.EventList = EventService29.EventCustomDataMapping(MyEntityPM,MyEntityPM.EventList,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.AddManualEvents != null && MyEntityPM.AddManualEvents.Count > 0)
				{
					 EventQueryService EventService29 = new EventQueryService(Tenant);
					 temp.AddManualEvents = EventService29.EventCustomDataMapping(MyEntityPM,MyEntityPM.AddManualEvents,Tenant,ComputingPartnerName);
				}

							  

			  
				   if(MyEntityPM.UnassignedShipperAddressId != null)
				   {
					   AddressQueryService AddressService29 = new AddressQueryService(Tenant);
					   					   temp.UnassignedShipperAddress = AddressService29.AddressCustomDataMapping(MyEntityPM.UnassignedShipperAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.UnassignedConsigneeAddressId != null)
				   {
					   AddressQueryService AddressService30 = new AddressQueryService(Tenant);
					   					   temp.UnassignedConsigneeAddress = AddressService30.AddressCustomDataMapping(MyEntityPM.UnassignedConsigneeAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.FullMaster = MyEntityPM.LongMaster; 

			  
				   if(MyEntityPM.Notify2Id != null)
				   {
					   CardQueryService CardService31 = new CardQueryService(Tenant);
					   					   temp.Notify2 = CardService31.GetCardById(MyEntityPM.Notify2Id,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Notify2Reference = MyEntityPM.Notify2Reference; 

			  
				   if(MyEntityPM.PreForwardingTransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService32 = new TransportModeQueryService(Tenant);
					   					   temp.PreForwardingTransportMode = TransportModeService32.GetTransportModeById(MyEntityPM.PreForwardingTransportModeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PreForwardingFromPortId != null)
				   {
					   PortQueryService PortService33 = new PortQueryService(Tenant);
					   					   temp.PreForwardingFromPort = PortService33.GetPortById(MyEntityPM.PreForwardingFromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PreForwardingToPortId != null)
				   {
					   PortQueryService PortService34 = new PortQueryService(Tenant);
					   					   temp.PreForwardingToPort = PortService34.GetPortById(MyEntityPM.PreForwardingToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.PreForwardingATD = MyEntityPM.PreForwardingATD;
				   temp.PreForwardingATA = MyEntityPM.PreForwardingATA;
				   temp.PreForwardingETD = MyEntityPM.PreForwardingETD;
				   temp.PreForwardingETA = MyEntityPM.PreForwardingETA;
				   temp.PreForwardingCarrierNumber = MyEntityPM.PreForwardingCarrierNumber; 

			  
				   if(MyEntityPM.PreForwardingCarrierId != null)
				   {
					   CardQueryService CardService35 = new CardQueryService(Tenant);
					   					   temp.PreForwardingCarrier = CardService35.GetCardById(MyEntityPM.PreForwardingCarrierId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PreForwardingVesselId != null)
				   {
					   VesselQueryService VesselService36 = new VesselQueryService(Tenant);
					   					   temp.PreForwardingVessel = VesselService36.GetVesselById(MyEntityPM.PreForwardingVesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OnForwardingTransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService37 = new TransportModeQueryService(Tenant);
					   					   temp.OnForwardingTransportMode = TransportModeService37.GetTransportModeById(MyEntityPM.OnForwardingTransportModeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OnForwardingFromPortId != null)
				   {
					   PortQueryService PortService38 = new PortQueryService(Tenant);
					   					   temp.OnForwardingFromPort = PortService38.GetPortById(MyEntityPM.OnForwardingFromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OnForwardingToPortId != null)
				   {
					   PortQueryService PortService39 = new PortQueryService(Tenant);
					   					   temp.OnForwardingToPort = PortService39.GetPortById(MyEntityPM.OnForwardingToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.OnForwardingATD = MyEntityPM.OnForwardingATD;
				   temp.OnForwardingATA = MyEntityPM.OnForwardingATA;
				   temp.OnForwardingETD = MyEntityPM.OnForwardingETD;
				   temp.OnForwardingETA = MyEntityPM.OnForwardingETA;
				   temp.OnForwardingCarrierNumber = MyEntityPM.OnForwardingCarrierNumber; 

			  
				   if(MyEntityPM.OnForwardingCarrierId != null)
				   {
					   CardQueryService CardService40 = new CardQueryService(Tenant);
					   					   temp.OnForwardingCarrier = CardService40.GetCardById(MyEntityPM.OnForwardingCarrierId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OnForwardingVesselId != null)
				   {
					   VesselQueryService VesselService41 = new VesselQueryService(Tenant);
					   					   temp.OnForwardingVessel = VesselService41.GetVesselById(MyEntityPM.OnForwardingVesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OtherPrepaidCollectId != null)
				   {
					   PrepaidCollectQueryService PrepaidCollectService42 = new PrepaidCollectQueryService(Tenant);
					   					   temp.OtherPrepaidCollect = PrepaidCollectService42.GetPrepaidCollectById(MyEntityPM.OtherPrepaidCollectId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.FreightPrepaidCollectId != null)
				   {
					   PrepaidCollectQueryService PrepaidCollectService43 = new PrepaidCollectQueryService(Tenant);
					   					   temp.FreightPrepaidCollect = PrepaidCollectService43.GetPrepaidCollectById(MyEntityPM.FreightPrepaidCollectId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ShipmentPM HouseDataMappingAndValidatin(House MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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

					 

					
					CardQueryService CustomerCardService = new CardQueryService(Tenant);
					if(MyEntity.Customer != null)
					{
						var myCustomerPM = CustomerCardService.CardDataMappingAndValidatin(MyEntity.Customer,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCustomerPM != null)
						{ 

						 								
								temp.CustomerId = myCustomerPM.Id;
						  

							
						} 

					}
			
					
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
			
					
					IncotermQueryService IncotermIncotermService = new IncotermQueryService(Tenant);
					if(MyEntity.Incoterm != null)
					{
						var myIncotermPM = IncotermIncotermService.IncotermDataMappingAndValidatin(MyEntity.Incoterm,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myIncotermPM != null)
						{ 

						 								
								temp.IncotermId = myIncotermPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.House = MyEntity.HouseNo;

					 

					
                    							
						temp.HAWBDate = MyEntity.HouseDate;

					 

					
                    							
						temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;

					 

					 

					if(MyEntity.AirPackages != null && MyEntity.AirPackages.Count > 0)
					{
						AirPackageQueryService AirPackageService44 = new AirPackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ShipmentPackages = AirPackageService44.AirPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.AirPackages,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.OceanOrInlandPackages != null && MyEntity.OceanOrInlandPackages.Count > 0)
					{
						OceanOrInlandPackageQueryService OceanOrInlandPackageService44 = new OceanOrInlandPackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ShipmentPackages = OceanOrInlandPackageService44.OceanOrInlandPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.OceanOrInlandPackages,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.Containers != null && MyEntity.Containers.Count > 0)
					{
						ContainerQueryService ContainerService44 = new ContainerQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ShipmentPackages = ContainerService44.ContainerCustomDataMappingAndValidatin(MyEntity,MyEntity.Containers,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
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
			
					
                    
					if(!IsUpdate)
					{							
						temp.TEU = MyEntity.TEU;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.NumberOfPackages = MyEntity.NumberOfPackages;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.GrossWeight = MyEntity.GrossWeight;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Volume = MyEntity.Volume;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.VolumetricWeight = MyEntity.VolumetricWeight;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ChargeableWeight = MyEntity.ChargeableWeight;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ShipmentNumber = MyEntity.ShipmentNumber;

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
			
					
                    
					if(!IsUpdate)
					{							
						temp.IsCancelled = MyEntity.IsCancelled;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsOperationalClosed = MyEntity.IsOperationalClosed;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.IsAccountingClosed = MyEntity.IsAccountingClosed;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.MasterShipmentDataId = MyEntity.MasterShipmentDataId;

										}  

					
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
			
					 

					if(MyEntity.PickUps != null && MyEntity.PickUps.Count > 0)
					{
						PickUpQueryService PickUpService44 = new PickUpQueryService(Tenant);
						 								
							temp.ShipmentPickUps = PickUpService44.PickUpDataMappingAndValidatin(MyEntity.PickUps,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								  

					if(MyEntity.Deliveries != null && MyEntity.Deliveries.Count > 0)
					{
						DeliveryQueryService DeliveryService44 = new DeliveryQueryService(Tenant);
						 								
							temp.ShipmentDeliveries = DeliveryService44.DeliveryDataMappingAndValidatin(MyEntity.Deliveries,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					
                    							
						temp.ValueOfGoods = MyEntity.ValueOfGoods;

					 

					
					CurrencyQueryService ValueOfGoodsCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.ValueOfGoodsCurrency != null)
					{
						var myValueOfGoodsCurrencyPM = ValueOfGoodsCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.ValueOfGoodsCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myValueOfGoodsCurrencyPM != null)
						{ 

						 								
								temp.ValueOfGoodsCurrencyId = myValueOfGoodsCurrencyPM.Id;
						  

							
						} 

					}
			
					 

					if(MyEntity.Receivables != null && MyEntity.Receivables.Count > 0)
					{
						ReceivableQueryService ReceivableService44 = new ReceivableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ShipmentReceivables = ReceivableService44.ReceivableDataMappingAndValidatin(MyEntity.Receivables,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.Payables != null && MyEntity.Payables.Count > 0)
					{
						PayableQueryService PayableService44 = new PayableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.ShipmentPayables = PayableService44.PayableDataMappingAndValidatin(MyEntity.Payables,Tenant,ComputingPartnerName,IsUpdate);

					 
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
			
					
					CardQueryService AgentCardService = new CardQueryService(Tenant);
					if(MyEntity.Agent != null)
					{
						var myAgentPM = AgentCardService.CardDataMappingAndValidatin(MyEntity.Agent,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAgentPM != null)
						{ 

						 								
								temp.AgentId = myAgentPM.Id;
						  

							
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
						temp.Ratio = MyEntity.Ratio;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ConcurrencyGUID = MyEntity.ConcurrencyGUID;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CustomerReference1 = MyEntity.CustomerReference1;

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
						EventQueryService EventService44 = new EventQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.EventList = EventService44.EventCustomDataMappingAndValidatin(MyEntity,MyEntity.EventList,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.AddManualEvents != null && MyEntity.AddManualEvents.Count > 0)
					{
						EventQueryService EventService44 = new EventQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.AddManualEvents = EventService44.EventCustomDataMappingAndValidatin(MyEntity,MyEntity.AddManualEvents,Tenant,ComputingPartnerName,IsUpdate);

					 
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
			
					
                    
					if(!IsUpdate)
					{							
						temp.LongMaster = MyEntity.FullMaster;

										}  

					
					CardQueryService Notify2CardService = new CardQueryService(Tenant);
					if(MyEntity.Notify2 != null)
					{
						var myNotify2PM = Notify2CardService.CardDataMappingAndValidatin(MyEntity.Notify2,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myNotify2PM != null)
						{ 

						 								
								temp.Notify2Id = myNotify2PM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.Notify2Reference = MyEntity.Notify2Reference;

					 

					
					TransportModeQueryService PreForwardingTransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.PreForwardingTransportMode != null)
					{
						var myPreForwardingTransportModePM = PreForwardingTransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.PreForwardingTransportMode,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreForwardingTransportModePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PreForwardingTransportModeId = myPreForwardingTransportModePM.Id;
						  
							}  

							
						} 

					}
			
					
					PortQueryService PreForwardingFromPortPortService = new PortQueryService(Tenant);
					if(MyEntity.PreForwardingFromPort != null)
					{
						var myPreForwardingFromPortPM = PreForwardingFromPortPortService.PortDataMappingAndValidatin(MyEntity.PreForwardingFromPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreForwardingFromPortPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PreForwardingFromPortId = myPreForwardingFromPortPM.Id;
						  
							}  

							
						} 

					}
			
					
					PortQueryService PreForwardingToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.PreForwardingToPort != null)
					{
						var myPreForwardingToPortPM = PreForwardingToPortPortService.PortDataMappingAndValidatin(MyEntity.PreForwardingToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreForwardingToPortPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PreForwardingToPortId = myPreForwardingToPortPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.PreForwardingATD = MyEntity.PreForwardingATD;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PreForwardingATA = MyEntity.PreForwardingATA;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PreForwardingETD = MyEntity.PreForwardingETD;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PreForwardingETA = MyEntity.PreForwardingETA;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PreForwardingCarrierNumber = MyEntity.PreForwardingCarrierNumber;

										}  

					
					CardQueryService PreForwardingCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.PreForwardingCarrier != null)
					{
						var myPreForwardingCarrierPM = PreForwardingCarrierCardService.CardDataMappingAndValidatin(MyEntity.PreForwardingCarrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreForwardingCarrierPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PreForwardingCarrierId = myPreForwardingCarrierPM.Id;
						  
							}  

							
						} 

					}
			
					
					VesselQueryService PreForwardingVesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.PreForwardingVessel != null)
					{
						var myPreForwardingVesselPM = PreForwardingVesselVesselService.VesselDataMappingAndValidatin(MyEntity.PreForwardingVessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreForwardingVesselPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PreForwardingVesselId = myPreForwardingVesselPM.Id;
						  
							}  

							
						} 

					}
			
					
					TransportModeQueryService OnForwardingTransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.OnForwardingTransportMode != null)
					{
						var myOnForwardingTransportModePM = OnForwardingTransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.OnForwardingTransportMode,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnForwardingTransportModePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.OnForwardingTransportModeId = myOnForwardingTransportModePM.Id;
						  
							}  

							
						} 

					}
			
					
					PortQueryService OnForwardingFromPortPortService = new PortQueryService(Tenant);
					if(MyEntity.OnForwardingFromPort != null)
					{
						var myOnForwardingFromPortPM = OnForwardingFromPortPortService.PortDataMappingAndValidatin(MyEntity.OnForwardingFromPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnForwardingFromPortPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.OnForwardingFromPortId = myOnForwardingFromPortPM.Id;
						  
							}  

							
						} 

					}
			
					
					PortQueryService OnForwardingToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.OnForwardingToPort != null)
					{
						var myOnForwardingToPortPM = OnForwardingToPortPortService.PortDataMappingAndValidatin(MyEntity.OnForwardingToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnForwardingToPortPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.OnForwardingToPortId = myOnForwardingToPortPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.OnForwardingATD = MyEntity.OnForwardingATD;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.OnForwardingATA = MyEntity.OnForwardingATA;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.OnForwardingETD = MyEntity.OnForwardingETD;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.OnForwardingETA = MyEntity.OnForwardingETA;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.OnForwardingCarrierNumber = MyEntity.OnForwardingCarrierNumber;

										}  

					
					CardQueryService OnForwardingCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.OnForwardingCarrier != null)
					{
						var myOnForwardingCarrierPM = OnForwardingCarrierCardService.CardDataMappingAndValidatin(MyEntity.OnForwardingCarrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnForwardingCarrierPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.OnForwardingCarrierId = myOnForwardingCarrierPM.Id;
						  
							}  

							
						} 

					}
			
					
					VesselQueryService OnForwardingVesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.OnForwardingVessel != null)
					{
						var myOnForwardingVesselPM = OnForwardingVesselVesselService.VesselDataMappingAndValidatin(MyEntity.OnForwardingVessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnForwardingVesselPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.OnForwardingVesselId = myOnForwardingVesselPM.Id;
						  
							}  

							
						} 

					}
			
					
					PrepaidCollectQueryService OtherPrepaidCollectPrepaidCollectService = new PrepaidCollectQueryService(Tenant);
					if(MyEntity.OtherPrepaidCollect != null)
					{
						var myOtherPrepaidCollectPM = OtherPrepaidCollectPrepaidCollectService.PrepaidCollectDataMappingAndValidatin(MyEntity.OtherPrepaidCollect,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOtherPrepaidCollectPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.OtherPrepaidCollectId = myOtherPrepaidCollectPM.Id;
						  
							}  

							
						} 

					}
			
					
					PrepaidCollectQueryService FreightPrepaidCollectPrepaidCollectService = new PrepaidCollectQueryService(Tenant);
					if(MyEntity.FreightPrepaidCollect != null)
					{
						var myFreightPrepaidCollectPM = FreightPrepaidCollectPrepaidCollectService.PrepaidCollectDataMappingAndValidatin(MyEntity.FreightPrepaidCollect,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFreightPrepaidCollectPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.FreightPrepaidCollectId = myFreightPrepaidCollectPM.Id;
						  
							}  

							
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