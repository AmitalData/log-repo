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
								
				var temp = query.GetSinglePM(Id, Tenant, include);
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
								
				var temp = query.GetSinglePMByShipmentNumber(ShipmentNumber, Tenant, include);
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
							{								//throw new ApplicationException("ShipmentType Can't be update"); 
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
							{								//throw new ApplicationException("TransportMode Can't be update"); 
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

						 								//throw new ApplicationException("Shipper Can't be update"); 
								temp.ShipperId = myShipperPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("ShipperReference1 Can't be update"); 
							temp.ShipperReference1 = MyEntity.ShipperReference1;

					 

					
                    							//throw new ApplicationException("ShipperReference2 Can't be update"); 
							temp.ShipperReference2 = MyEntity.ShipperReference2;

					 

					
					CardQueryService ConsigneeCardService = new CardQueryService(Tenant);
					if(MyEntity.Consignee != null)
					{
						var myConsigneePM = ConsigneeCardService.CardDataMappingAndValidatin(MyEntity.Consignee,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myConsigneePM != null)
						{ 

						 								//throw new ApplicationException("Consignee Can't be update"); 
								temp.ConsigneeId = myConsigneePM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("ConsigneeReference1 Can't be update"); 
							temp.ConsigneeReference1 = MyEntity.ConsigneeReference1;

					 

					
                    							//throw new ApplicationException("ConsigneeReference2 Can't be update"); 
							temp.ConsigneeReference2 = MyEntity.ConsigneeReference2;

					 

					
					CardQueryService CustomerCardService = new CardQueryService(Tenant);
					if(MyEntity.Customer != null)
					{
						var myCustomerPM = CustomerCardService.CardDataMappingAndValidatin(MyEntity.Customer,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCustomerPM != null)
						{ 

						 								//throw new ApplicationException("Customer Can't be update"); 
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
							{								//throw new ApplicationException("FromPort Can't be update"); 
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
							{								//throw new ApplicationException("ToPort Can't be update"); 
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
							{								//throw new ApplicationException("GrossWeightUnit Can't be update"); 
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
							{								//throw new ApplicationException("ChargeableWeightUnit Can't be update"); 
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
							{								//throw new ApplicationException("VolumeUnit Can't be update"); 
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

						 								//throw new ApplicationException("Incoterm Can't be update"); 
								temp.IncotermId = myIncotermPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("HouseNo Can't be update"); 
							temp.House = MyEntity.HouseNo;

					 

					
                    							//throw new ApplicationException("HouseDate Can't be update"); 
							temp.HAWBDate = MyEntity.HouseDate;

					 

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.DescriptionOfGoods))
					{							//throw new ApplicationException("DescriptionOfGoods Can't be update"); 
							temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;

										}  

					 

					if(MyEntity.AirPackages != null && MyEntity.AirPackages.Count > 0)
					{
						AirPackageQueryService AirPackageService31 = new AirPackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("AirPackages Can't be update"); 
								temp.ShipmentPackages = AirPackageService31.AirPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.AirPackages,Tenant,ComputingPartnerName);

					 
						}  

						
					}

								  

					if(MyEntity.OceanOrInlandPackages != null && MyEntity.OceanOrInlandPackages.Count > 0)
					{
						OceanOrInlandPackageQueryService OceanOrInlandPackageService31 = new OceanOrInlandPackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("OceanOrInlandPackages Can't be update"); 
								temp.ShipmentPackages = OceanOrInlandPackageService31.OceanOrInlandPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.OceanOrInlandPackages,Tenant,ComputingPartnerName);

					 
						}  

						
					}

								  

					if(MyEntity.Containers != null && MyEntity.Containers.Count > 0)
					{
						ContainerQueryService ContainerService31 = new ContainerQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("Containers Can't be update"); 
								temp.ShipmentPackages = ContainerService31.ContainerCustomDataMappingAndValidatin(MyEntity,MyEntity.Containers,Tenant,ComputingPartnerName);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Commodity))
					{							//throw new ApplicationException("Commodity Can't be update"); 
							temp.AWBCommodityItemNumber = MyEntity.Commodity;

										}  

					
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myBranchPM != null)
						{ 

						 								//throw new ApplicationException("Branch Can't be update"); 
								temp.BranchId = myBranchPM.Id;
						  

							
						} 

					}
			
					
					DepartmentQueryService DepartmentDepartmentService = new DepartmentQueryService(Tenant);
					if(MyEntity.Department != null)
					{
						var myDepartmentPM = DepartmentDepartmentService.DepartmentDataMappingAndValidatin(MyEntity.Department,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDepartmentPM != null)
						{ 

						 								//throw new ApplicationException("Department Can't be update"); 
								temp.DepartmentId = myDepartmentPM.Id;
						  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.TEU != null)
					{							//throw new ApplicationException("TEU Can't be update"); 
							temp.TEU = MyEntity.TEU;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.NumberOfPackages != null)
					{							//throw new ApplicationException("NumberOfPackages Can't be update"); 
							temp.NumberOfPackages = MyEntity.NumberOfPackages;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.GrossWeight != null)
					{							//throw new ApplicationException("GrossWeight Can't be update"); 
							temp.GrossWeight = MyEntity.GrossWeight;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.Volume != null)
					{							//throw new ApplicationException("Volume Can't be update"); 
							temp.Volume = MyEntity.Volume;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.VolumetricWeight != null)
					{							//throw new ApplicationException("VolumetricWeight Can't be update"); 
							temp.VolumetricWeight = MyEntity.VolumetricWeight;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ChargeableWeight != null)
					{							//throw new ApplicationException("ChargeableWeight Can't be update"); 
							temp.ChargeableWeight = MyEntity.ChargeableWeight;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ShipmentNumber))
					{							//throw new ApplicationException("ShipmentNumber Can't be update"); 
							temp.ShipmentNumber = MyEntity.ShipmentNumber;

										}  

					
					DirectionQueryService DirectionDirectionService = new DirectionQueryService(Tenant);
					if(MyEntity.Direction != null)
					{
						var myDirectionPM = DirectionDirectionService.DirectionDataMappingAndValidatin(MyEntity.Direction,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDirectionPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Direction Can't be update"); 
								temp.DirectionId = myDirectionPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && (MyEntity.IsCancelled != temp.IsCancelled))
					{							//throw new ApplicationException("IsCancelled Can't be update"); 
							temp.IsCancelled = MyEntity.IsCancelled;

										}  

					
                    
					if(!IsUpdate)// && (MyEntity.IsOperationalClosed != temp.IsOperationalClosed))
					{							//throw new ApplicationException("IsOperationalClosed Can't be update"); 
							temp.IsOperationalClosed = MyEntity.IsOperationalClosed;

										}  

					
                    
					if(!IsUpdate)// && (MyEntity.IsAccountingClosed != temp.IsAccountingClosed))
					{							//throw new ApplicationException("IsAccountingClosed Can't be update"); 
							temp.IsAccountingClosed = MyEntity.IsAccountingClosed;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.MasterShipmentDataId))
					{							//throw new ApplicationException("MasterShipmentDataId Can't be update"); 
							temp.MasterShipmentDataId = MyEntity.MasterShipmentDataId;

										}  

					
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCreatedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("CreatedByUser Can't be update"); 
								temp.CreatedByUserId = myCreatedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					 

					if(MyEntity.PickUps != null && MyEntity.PickUps.Count > 0)
					{
						PickUpQueryService PickUpService31 = new PickUpQueryService(Tenant);
						 								//throw new ApplicationException("PickUps Can't be update"); 
								temp.ShipmentPickUps = PickUpService31.PickUpDataMappingAndValidatin(MyEntity.PickUps,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								  

					if(MyEntity.Deliveries != null && MyEntity.Deliveries.Count > 0)
					{
						DeliveryQueryService DeliveryService31 = new DeliveryQueryService(Tenant);
						 								//throw new ApplicationException("Deliveries Can't be update"); 
								temp.ShipmentDeliveries = DeliveryService31.DeliveryDataMappingAndValidatin(MyEntity.Deliveries,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					
                    							//throw new ApplicationException("ValueOfGoods Can't be update"); 
							temp.ValueOfGoods = MyEntity.ValueOfGoods;

					 

					
					CurrencyQueryService ValueOfGoodsCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.ValueOfGoodsCurrency != null)
					{
						var myValueOfGoodsCurrencyPM = ValueOfGoodsCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.ValueOfGoodsCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myValueOfGoodsCurrencyPM != null)
						{ 

						 								//throw new ApplicationException("ValueOfGoodsCurrency Can't be update"); 
								temp.ValueOfGoodsCurrencyId = myValueOfGoodsCurrencyPM.Id;
						  

							
						} 

					}
			
					 

					if(MyEntity.Receivables != null && MyEntity.Receivables.Count > 0)
					{
						ReceivableQueryService ReceivableService31 = new ReceivableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("Receivables Can't be update"); 
								temp.ShipmentReceivables = ReceivableService31.ReceivableDataMappingAndValidatin(MyEntity.Receivables,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.Payables != null && MyEntity.Payables.Count > 0)
					{
						PayableQueryService PayableService31 = new PayableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("Payables Can't be update"); 
								temp.ShipmentPayables = PayableService31.PayableDataMappingAndValidatin(MyEntity.Payables,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
					DimensionsUnitQueryService DimensionsUnitDimensionsUnitService = new DimensionsUnitQueryService(Tenant);
					if(MyEntity.DimensionsUnit != null)
					{
						var myDimensionsUnitPM = DimensionsUnitDimensionsUnitService.DimensionsUnitDataMappingAndValidatin(MyEntity.DimensionsUnit,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDimensionsUnitPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("DimensionsUnit Can't be update"); 
								temp.DimensionsUnitCode = myDimensionsUnitPM.Code;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.OrderNumberOfPackages != null)
					{							//throw new ApplicationException("OrderNumberOfPackages Can't be update"); 
							temp.BookingNumberOfPackages = MyEntity.OrderNumberOfPackages;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.OrderGrossWeight != null)
					{							//throw new ApplicationException("OrderGrossWeight Can't be update"); 
							temp.OrderGrossWeight = MyEntity.OrderGrossWeight;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.OrderVolume != null)
					{							//throw new ApplicationException("OrderVolume Can't be update"); 
							temp.BookingVolume = MyEntity.OrderVolume;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.OrderChargeableWeight != null)
					{							//throw new ApplicationException("OrderChargeableWeight Can't be update"); 
							temp.OrderChargeableWeight = MyEntity.OrderChargeableWeight;

										}  

					
                    
					if(!IsUpdate)// && (MyEntity.OrderIsDangerouseGoods != temp.OrderIsDangerouseGoods))
					{							//throw new ApplicationException("OrderIsDangerouseGoods Can't be update"); 
							temp.OrderIsDangerouseGoods = MyEntity.OrderIsDangerouseGoods;

										}  

					
                    							//throw new ApplicationException("MainHarmonize Can't be update"); 
							temp.MainHarmonize = MyEntity.MainHarmonize;

					 

					
					UserQueryService SalesmanUserService = new UserQueryService(Tenant);
					if(MyEntity.Salesman != null)
					{
						var mySalesmanPM = SalesmanUserService.UserDataMappingAndValidatin(MyEntity.Salesman,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySalesmanPM != null)
						{ 

						 								//throw new ApplicationException("Salesman Can't be update"); 
								temp.SalesmanUserId = mySalesmanPM.Id;
						  

							
						} 

					}
			
					
					UserQueryService AccountManagerUserService = new UserQueryService(Tenant);
					if(MyEntity.AccountManager != null)
					{
						var myAccountManagerPM = AccountManagerUserService.UserDataMappingAndValidatin(MyEntity.AccountManager,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAccountManagerPM != null)
						{ 

						 								//throw new ApplicationException("AccountManager Can't be update"); 
								temp.AccountManagerUserId = myAccountManagerPM.Id;
						  

							
						} 

					}
			
					
					SpecialServicesTypeQueryService SpecialServicesTypeSpecialServicesTypeService = new SpecialServicesTypeQueryService(Tenant);
					if(MyEntity.SpecialServicesType != null)
					{
						var mySpecialServicesTypePM = SpecialServicesTypeSpecialServicesTypeService.SpecialServicesTypeDataMappingAndValidatin(MyEntity.SpecialServicesType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySpecialServicesTypePM != null)
						{ 

						 								//throw new ApplicationException("SpecialServicesType Can't be update"); 
								temp.SpecialServicesTypeId = mySpecialServicesTypePM.Id;
						  

							
						} 

					}
			
					
					CardQueryService ShipperNotExporterCardService = new CardQueryService(Tenant);
					if(MyEntity.ShipperNotExporter != null)
					{
						var myShipperNotExporterPM = ShipperNotExporterCardService.CardDataMappingAndValidatin(MyEntity.ShipperNotExporter,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myShipperNotExporterPM != null)
						{ 

						 								//throw new ApplicationException("ShipperNotExporter Can't be update"); 
								temp.ShipperNotExporterId = myShipperNotExporterPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService AgentCardService = new CardQueryService(Tenant);
					if(MyEntity.Agent != null)
					{
						var myAgentPM = AgentCardService.CardDataMappingAndValidatin(MyEntity.Agent,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAgentPM != null)
						{ 

						 								//throw new ApplicationException("Agent Can't be update"); 
								temp.AgentId = myAgentPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService CustomAgentImportCardService = new CardQueryService(Tenant);
					if(MyEntity.CustomAgentImport != null)
					{
						var myCustomAgentImportPM = CustomAgentImportCardService.CardDataMappingAndValidatin(MyEntity.CustomAgentImport,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCustomAgentImportPM != null)
						{ 

						 								//throw new ApplicationException("CustomAgentImport Can't be update"); 
								temp.CustomAgentImportId = myCustomAgentImportPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService ReleasingAgentCardService = new CardQueryService(Tenant);
					if(MyEntity.ReleasingAgent != null)
					{
						var myReleasingAgentPM = ReleasingAgentCardService.CardDataMappingAndValidatin(MyEntity.ReleasingAgent,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myReleasingAgentPM != null)
						{ 

						 								//throw new ApplicationException("ReleasingAgent Can't be update"); 
								temp.ReleasingAgentId = myReleasingAgentPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService FreightForwarderCardService = new CardQueryService(Tenant);
					if(MyEntity.FreightForwarder != null)
					{
						var myFreightForwarderPM = FreightForwarderCardService.CardDataMappingAndValidatin(MyEntity.FreightForwarder,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFreightForwarderPM != null)
						{ 

						 								//throw new ApplicationException("FreightForwarder Can't be update"); 
								temp.FreightForwarderId = myFreightForwarderPM.Id;
						  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.Ratio != null)
					{							//throw new ApplicationException("Ratio Can't be update"); 
							temp.Ratio = MyEntity.Ratio;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ConcurrencyGUID))
					{							//throw new ApplicationException("ConcurrencyGUID Can't be update"); 
							temp.ConcurrencyGUID = MyEntity.ConcurrencyGUID;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.CustomerReference1))
					{							//throw new ApplicationException("CustomerReference1 Can't be update"); 
							temp.CustomerReference1 = MyEntity.CustomerReference1;

										}  

					
					EntityStatusQueryService StatusEntityStatusService = new EntityStatusQueryService(Tenant);
					if(MyEntity.Status != null)
					{
						var myStatusPM = StatusEntityStatusService.EntityStatusDataMappingAndValidatin(MyEntity.Status,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatusPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Status Can't be update"); 
								temp.StatusId = myStatusPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.BookingConfirmationNumber))
					{							//throw new ApplicationException("BookingConfirmationNumber Can't be update"); 
							temp.BookingConfirmationNumber = MyEntity.BookingConfirmationNumber;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.EstimatedFinalArrivalDate != null)
					{							//throw new ApplicationException("EstimatedFinalArrivalDate Can't be update"); 
							temp.EstimatedFinalArrivalDate = MyEntity.EstimatedFinalArrivalDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ActualFinalArrivalDate != null)
					{							//throw new ApplicationException("ActualFinalArrivalDate Can't be update"); 
							temp.ActualFinalArrivalDate = MyEntity.ActualFinalArrivalDate;

										}  

					
                    							//throw new ApplicationException("IsHTSMissing Can't be update"); 
							temp.IsHTSMissing = MyEntity.IsHTSMissing;

					 

					
                    							//throw new ApplicationException("PlannedCargoReadyDate Can't be update"); 
							temp.PlannedCargoReadyDate = MyEntity.PlannedCargoReadyDate;

					 

					
                    							//throw new ApplicationException("ApprovedCargoReadyDate Can't be update"); 
							temp.ApprovedCargoReadyDate = MyEntity.ApprovedCargoReadyDate;

					 

					
					UserQueryService HandlerUserUserService = new UserQueryService(Tenant);
					if(MyEntity.HandlerUser != null)
					{
						var myHandlerUserPM = HandlerUserUserService.UserDataMappingAndValidatin(MyEntity.HandlerUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myHandlerUserPM != null)
						{ 

						 								//throw new ApplicationException("HandlerUser Can't be update"); 
								temp.HandlerUserId = myHandlerUserPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("Notify1Reference Can't be update"); 
							temp.Notify1Reference = MyEntity.Notify1Reference;

					 

					
                    							//throw new ApplicationException("Notify1Reference2 Can't be update"); 
							temp.Notify1Reference2 = MyEntity.Notify1Reference2;

					 

					
                    							//throw new ApplicationException("ShipperNotExporterReference1 Can't be update"); 
							temp.ShipperNotExporterReference1 = MyEntity.ShipperNotExporterReference1;

					 

					
                    							//throw new ApplicationException("ShipperNotExporterReference2 Can't be update"); 
							temp.ShipperNotExporterReference2 = MyEntity.ShipperNotExporterReference2;

					 

					
                    							//throw new ApplicationException("CustomsClearanceDate Can't be update"); 
							temp.CustomsClearanceDate = MyEntity.CustomsClearanceDate;

					 

					
					CardQueryService Notify1CardService = new CardQueryService(Tenant);
					if(MyEntity.Notify1 != null)
					{
						var myNotify1PM = Notify1CardService.CardDataMappingAndValidatin(MyEntity.Notify1,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myNotify1PM != null)
						{ 

						 								//throw new ApplicationException("Notify1 Can't be update"); 
								temp.Notify1Id = myNotify1PM.Id;
						  

							
						} 

					}
			
					 

					if(MyEntity.EventList != null && MyEntity.EventList.Count > 0)
					{
						EventQueryService EventService31 = new EventQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("EventList Can't be update"); 
								temp.EventList = EventService31.EventCustomDataMappingAndValidatin(MyEntity,MyEntity.EventList,Tenant,ComputingPartnerName);

					 
						}  

						
					}

								  

					if(MyEntity.AddManualEvents != null && MyEntity.AddManualEvents.Count > 0)
					{
						EventQueryService EventService31 = new EventQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("AddManualEvents Can't be update"); 
								temp.AddManualEvents = EventService31.EventCustomDataMappingAndValidatin(MyEntity,MyEntity.AddManualEvents,Tenant,ComputingPartnerName);

					 
						}  

						
					}

								 
					AddressQueryService UnassignedShipperAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.UnassignedShipperAddress != null)
					{
						var myUnassignedShipperAddressPM = UnassignedShipperAddressAddressService.AddressCustomDataMappingAndValidatin(MyEntity.UnassignedShipperAddress,Tenant);
						
						if(myUnassignedShipperAddressPM != null)
						{ 

						 								//throw new ApplicationException("UnassignedShipperAddress Can't be update"); 
								temp.UnassignedShipperAddressId = myUnassignedShipperAddressPM.Id;
						  

							
						} 

					}
			
					
					AddressQueryService UnassignedConsigneeAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.UnassignedConsigneeAddress != null)
					{
						var myUnassignedConsigneeAddressPM = UnassignedConsigneeAddressAddressService.AddressCustomDataMappingAndValidatin(MyEntity.UnassignedConsigneeAddress,Tenant);
						
						if(myUnassignedConsigneeAddressPM != null)
						{ 

						 								//throw new ApplicationException("UnassignedConsigneeAddress Can't be update"); 
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