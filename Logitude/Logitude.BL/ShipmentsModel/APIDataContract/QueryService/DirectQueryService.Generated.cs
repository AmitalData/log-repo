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
   public partial class DirectQueryService
   {
   
		IShipmentsContext  context;
		//ShipmentService service; 
		
		ShipmentQuery query; 

        public DirectQueryService(int tenant)
        {
				    context = ShipmentsContext.GetContext(tenant); 
			//service = new ShipmentService(context, tenant); 
			query = new ShipmentQuery(tenant);
        }

		
		public Direct GetDirectById(string Id,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Shipment with Id " + Id + " doesn't exist");

				return DirectDataMapping(temp,Tenant,ComputingPartnerName);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Direct GetDirectByShipmentNumber(string ShipmentNumber,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePMByShipmentNumber(ShipmentNumber,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Shipment with ShipmentNumber " + ShipmentNumber + " doesn't exist");

				return DirectDataMapping(temp,Tenant,ComputingPartnerName);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Direct DirectDataMapping(ShipmentPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Direct(); 
				   temp.Id = MyEntityPM.Id; 

			  
				   if(MyEntityPM.ShipmentTypeId != null)
				   {
					   ShipmentTypeQueryService ShipmentTypeService0 = new ShipmentTypeQueryService(Tenant);
					   					   temp.ShipmentType = ShipmentTypeService0.ShipmentTypeCustomDataMapping(MyEntityPM.ShipmentTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DirectionId != null)
				   {
					   DirectionQueryService DirectionService1 = new DirectionQueryService(Tenant);
					   					   temp.Direction = DirectionService1.GetDirectionById(MyEntityPM.DirectionId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService2 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService2.GetTransportModeById(MyEntityPM.TransportModeId,Tenant,ComputingPartnerName); 
			       
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

			  
				   if(MyEntityPM.CustomerId != null)
				   {
					   CardQueryService CardService5 = new CardQueryService(Tenant);
					   					   temp.Customer = CardService5.GetCardById(MyEntityPM.CustomerId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
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
				    

			  
				   if(MyEntityPM.IncotermId != null)
				   {
					   IncotermQueryService IncotermService11 = new IncotermQueryService(Tenant);
					   					   temp.Incoterm = IncotermService11.GetIncotermById(MyEntityPM.IncotermId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.DescriptionOfGoods = MyEntityPM.DescriptionOfGoods;
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 AirPackageQueryService AirPackageService12 = new AirPackageQueryService(Tenant);
					 temp.AirPackages = AirPackageService12.AirPackageCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 OceanOrInlandPackageQueryService OceanOrInlandPackageService12 = new OceanOrInlandPackageQueryService(Tenant);
					 temp.OceanOrInlandPackages = OceanOrInlandPackageService12.OceanOrInlandPackageCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 ContainerQueryService ContainerService12 = new ContainerQueryService(Tenant);
					 temp.Containers = ContainerService12.ContainerCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant,ComputingPartnerName);
				}

							 
				   temp.Commodity = MyEntityPM.AWBCommodityItemNumber; 

			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService12 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService12.GetBranchById(MyEntityPM.BranchId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DepartmentId != null)
				   {
					   DepartmentQueryService DepartmentService13 = new DepartmentQueryService(Tenant);
					   					   temp.Department = DepartmentService13.GetDepartmentById(MyEntityPM.DepartmentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.TEU = MyEntityPM.TEU;
				   temp.NumberOfPackages = MyEntityPM.NumberOfPackages;
				   temp.GrossWeight = MyEntityPM.GrossWeight;
				   temp.Volume = MyEntityPM.Volume;
				   temp.VolumetricWeight = MyEntityPM.VolumetricWeight;
				   temp.ChargeableWeight = MyEntityPM.ChargeableWeight;
				   temp.Master = MyEntityPM.Master;
				   temp.ShipmentNumber = MyEntityPM.ShipmentNumber; 

			  
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
				 
				   
				   temp.IsOperationalClosed = MyEntityPM.IsOperationalClosed; 

			  
				   if(MyEntityPM.MainCarriageVesselId != null)
				   {
					   VesselQueryService VesselService16 = new VesselQueryService(Tenant);
					   					   temp.Vessel = VesselService16.GetVesselById(MyEntityPM.MainCarriageVesselId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.MainCarriageATA = MyEntityPM.MainCarriageATA;
				   temp.MainCarriageATD = MyEntityPM.MainCarriageATD;
				   temp.IsAccountingClosed = MyEntityPM.IsAccountingClosed;
				   temp.ValueOfGoods = MyEntityPM.ValueOfGoods; 

			  
				   if(MyEntityPM.ValueOfGoodsCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService17 = new CurrencyQueryService(Tenant);
					   					   temp.ValueOfGoodsCurrency = CurrencyService17.GetCurrencyById(MyEntityPM.ValueOfGoodsCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.MainCarriageCarrierNumber = MyEntityPM.MainCarriageCarrierNumber; 

			  
				   if(MyEntityPM.MainCarriageCarrierId != null)
				   {
					   CardQueryService CardService18 = new CardQueryService(Tenant);
					   					   temp.MainCarriageCarrier = CardService18.GetCardById(MyEntityPM.MainCarriageCarrierId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				if(MyEntityPM.ShipmentReceivables != null && MyEntityPM.ShipmentReceivables.Count > 0)
				{
					 ReceivableQueryService ReceivableService19 = new ReceivableQueryService(Tenant);
					 temp.Receivables = ReceivableService19.ReceivableDataMapping(MyEntityPM.ShipmentReceivables,Tenant,ComputingPartnerName);
				}

							 
				if(MyEntityPM.ShipmentPayables != null && MyEntityPM.ShipmentPayables.Count > 0)
				{
					 PayableQueryService PayableService19 = new PayableQueryService(Tenant);
					 temp.Payables = PayableService19.PayableDataMapping(MyEntityPM.ShipmentPayables,Tenant,ComputingPartnerName);
				}

							  

			  
				   if(MyEntityPM.DimensionsUnitCode != null)
				   {
					   DimensionsUnitQueryService DimensionsUnitService19 = new DimensionsUnitQueryService(Tenant);
					   					   temp.DimensionsUnit = DimensionsUnitService19.GetDimensionsUnitByCode(MyEntityPM.DimensionsUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.OrderNumberOfPackages = MyEntityPM.BookingNumberOfPackages;
				   temp.OrderGrossWeight = MyEntityPM.OrderGrossWeight;
				   temp.OrderVolume = MyEntityPM.BookingVolume;
				   temp.OrderChargeableWeight = MyEntityPM.OrderChargeableWeight;
				   temp.OrderIsDangerouseGoods = MyEntityPM.OrderIsDangerouseGoods;
				   temp.MainHarmonize = MyEntityPM.MainHarmonize; 

			  
				   if(MyEntityPM.SalesmanUserId != null)
				   {
					   UserQueryService UserService20 = new UserQueryService(Tenant);
					   					   temp.Salesman = UserService20.GetUserById(MyEntityPM.SalesmanUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.AccountManagerUserId != null)
				   {
					   UserQueryService UserService21 = new UserQueryService(Tenant);
					   					   temp.AccountManager = UserService21.GetUserById(MyEntityPM.AccountManagerUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.SpecialServicesTypeId != null)
				   {
					   SpecialServicesTypeQueryService SpecialServicesTypeService22 = new SpecialServicesTypeQueryService(Tenant);
					   					   temp.SpecialServicesType = SpecialServicesTypeService22.GetSpecialServicesTypeById(MyEntityPM.SpecialServicesTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipperNotExporterId != null)
				   {
					   CardQueryService CardService23 = new CardQueryService(Tenant);
					   					   temp.ShipperNotExporter = CardService23.GetCardById(MyEntityPM.ShipperNotExporterId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.AgentId != null)
				   {
					   CardQueryService CardService24 = new CardQueryService(Tenant);
					   					   temp.Agent = CardService24.GetCardById(MyEntityPM.AgentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.CustomAgentImportId != null)
				   {
					   CardQueryService CardService25 = new CardQueryService(Tenant);
					   					   temp.CustomAgentImport = CardService25.GetCardById(MyEntityPM.CustomAgentImportId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ReleasingAgentId != null)
				   {
					   CardQueryService CardService26 = new CardQueryService(Tenant);
					   					   temp.ReleasingAgent = CardService26.GetCardById(MyEntityPM.ReleasingAgentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.FreightForwarderId != null)
				   {
					   CardQueryService CardService27 = new CardQueryService(Tenant);
					   					   temp.FreightForwarder = CardService27.GetCardById(MyEntityPM.FreightForwarderId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.MasterDate = MyEntityPM.MAWBOBLDate;
				   temp.Ratio = MyEntityPM.Ratio;
				if(MyEntityPM.MainCarriageLegs != null && MyEntityPM.MainCarriageLegs.Count > 0)
				{
					 MainCarriageLegQueryService MainCarriageLegService28 = new MainCarriageLegQueryService(Tenant);
					 temp.MainCarriageLegs = MainCarriageLegService28.MainCarriageLegDataMapping(MyEntityPM.MainCarriageLegs,Tenant,ComputingPartnerName);
				}

							 
				   temp.ConcurrencyGUID = MyEntityPM.ConcurrencyGUID;
				   temp.CustomerReference1 = MyEntityPM.CustomerReference1; 

			  
				   if(MyEntityPM.StatusId != null)
				   {
					   EntityStatusQueryService EntityStatusService28 = new EntityStatusQueryService(Tenant);
					   					   temp.Status = EntityStatusService28.GetEntityStatusById(MyEntityPM.StatusId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.BookingConfirmationNumber = MyEntityPM.BookingConfirmationNumber;
				   temp.EstimatedFinalArrivalDate = MyEntityPM.EstimatedFinalArrivalDate;
				   temp.ActualFinalArrivalDate = MyEntityPM.ActualFinalArrivalDate;
				   temp.HouseNo = MyEntityPM.House; 

			  
				   if(MyEntityPM.MainCarriageFromPartnerId != null)
				   {
					   CardQueryService CardService29 = new CardQueryService(Tenant);
					   					   temp.MainCarriageFromPartner = CardService29.GetCardById(MyEntityPM.MainCarriageFromPartnerId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.MainCarriageToPartnerId != null)
				   {
					   CardQueryService CardService30 = new CardQueryService(Tenant);
					   					   temp.MainCarriageToPartner = CardService30.GetCardById(MyEntityPM.MainCarriageToPartnerId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.MainCarriageETA = MyEntityPM.MainCarriageETA;
				   temp.MainCarriageETD = MyEntityPM.MainCarriageETD;
				   temp.TruckNumber = MyEntityPM.TruckNumber;
				   temp.IsHTSMissing = MyEntityPM.IsHTSMissing;
				   temp.PlannedCargoReadyDate = MyEntityPM.PlannedCargoReadyDate;
				   temp.ApprovedCargoReadyDate = MyEntityPM.ApprovedCargoReadyDate; 

			  
				   if(MyEntityPM.HandlerUserId != null)
				   {
					   UserQueryService UserService31 = new UserQueryService(Tenant);
					   					   temp.HandlerUser = UserService31.GetUserById(MyEntityPM.HandlerUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Notify1Reference = MyEntityPM.Notify1Reference;
				   temp.Notify1Reference2 = MyEntityPM.Notify1Reference2;
				   temp.ShipperNotExporterReference1 = MyEntityPM.ShipperNotExporterReference1;
				   temp.ShipperNotExporterReference2 = MyEntityPM.ShipperNotExporterReference2;
				   temp.CustomsClearanceDate = MyEntityPM.CustomsClearanceDate; 

			  
				   if(MyEntityPM.InlandDomesticFromTypeCode != null)
				   {
					   PickUpDeliveryFromToTypeQueryService PickUpDeliveryFromToTypeService32 = new PickUpDeliveryFromToTypeQueryService(Tenant);
					   					   temp.InlandDomesticFromTypeCode = PickUpDeliveryFromToTypeService32.GetPickUpDeliveryFromToTypeByCode(MyEntityPM.InlandDomesticFromTypeCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.InlandDomesticToTypeCode != null)
				   {
					   PickUpDeliveryFromToTypeQueryService PickUpDeliveryFromToTypeService33 = new PickUpDeliveryFromToTypeQueryService(Tenant);
					   					   temp.InlandDomesticToTypeCode = PickUpDeliveryFromToTypeService33.GetPickUpDeliveryFromToTypeByCode(MyEntityPM.InlandDomesticToTypeCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.InlandDomesticFromCountryId != null)
				   {
					   CountryQueryService CountryService34 = new CountryQueryService(Tenant);
					   					   temp.InlandDomesticFromCountry = CountryService34.GetCountryById(MyEntityPM.InlandDomesticFromCountryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.InlandDomesticToCountryId != null)
				   {
					   CountryQueryService CountryService35 = new CountryQueryService(Tenant);
					   					   temp.InlandDomesticToCountry = CountryService35.GetCountryById(MyEntityPM.InlandDomesticToCountryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.InlandDomesticFromZipCode = MyEntityPM.InlandDomesticFromZipCode;
				   temp.InlandDomesticToZipCode = MyEntityPM.InlandDomesticToZipCode;
				   temp.InlandDomesticFromCity = MyEntityPM.InlandDomesticFromCity;
				   temp.InlandDomesticToCity = MyEntityPM.InlandDomesticToCity; 

			  
				   if(MyEntityPM.MainCarriageFromPortId != null)
				   {
					   PortQueryService PortService36 = new PortQueryService(Tenant);
					   					   temp.MainCarriageFromPort = PortService36.GetPortById(MyEntityPM.MainCarriageFromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.MainCarriageToPortId != null)
				   {
					   PortQueryService PortService37 = new PortQueryService(Tenant);
					   					   temp.MainCarriageToPort = PortService37.GetPortById(MyEntityPM.MainCarriageToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.CutoffDate = MyEntityPM.CutoffDate;
				   temp.IsDangerous = MyEntityPM.IsDangerous;
				   temp.HAWBDate = MyEntityPM.HAWBDate; 

			  
				   if(MyEntityPM.OnCarriageTransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService38 = new TransportModeQueryService(Tenant);
					   					   temp.OnCarriageTransportMode = TransportModeService38.GetTransportModeById(MyEntityPM.OnCarriageTransportModeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OnCarriageFromPortId != null)
				   {
					   PortQueryService PortService39 = new PortQueryService(Tenant);
					   					   temp.OnCarriageFromPort = PortService39.GetPortById(MyEntityPM.OnCarriageFromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OnCarriageToPortId != null)
				   {
					   PortQueryService PortService40 = new PortQueryService(Tenant);
					   					   temp.OnCarriageToPort = PortService40.GetPortById(MyEntityPM.OnCarriageToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.OnCarriageCarrierId != null)
				   {
					   CardQueryService CardService41 = new CardQueryService(Tenant);
					   					   temp.OnCarriageCarrier = CardService41.GetCardById(MyEntityPM.OnCarriageCarrierId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.OnCarriageCarrierNumber = MyEntityPM.OnCarriageCarrierNumber;
				   temp.OnCarriageATD = MyEntityPM.OnCarriageATD;
				   temp.OnCarriageATA = MyEntityPM.OnCarriageATA;
				   temp.OnCarriageETD = MyEntityPM.OnCarriageETD;
				   temp.OnCarriageETA = MyEntityPM.OnCarriageETA; 

			  
				   if(MyEntityPM.PreCarriageTransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService42 = new TransportModeQueryService(Tenant);
					   					   temp.PreCarriageTransportMode = TransportModeService42.GetTransportModeById(MyEntityPM.PreCarriageTransportModeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PreCarriageFromPortId != null)
				   {
					   PortQueryService PortService43 = new PortQueryService(Tenant);
					   					   temp.PreCarriageFromPort = PortService43.GetPortById(MyEntityPM.PreCarriageFromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PreCarriageToPortId != null)
				   {
					   PortQueryService PortService44 = new PortQueryService(Tenant);
					   					   temp.PreCarriageToPort = PortService44.GetPortById(MyEntityPM.PreCarriageToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PreCarriageCarrierId != null)
				   {
					   CardQueryService CardService45 = new CardQueryService(Tenant);
					   					   temp.PreCarriageCarrier = CardService45.GetCardById(MyEntityPM.PreCarriageCarrierId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.PreCarriageCarrierNumber = MyEntityPM.PreCarriageCarrierNumber;
				   temp.PreCarriageATD = MyEntityPM.PreCarriageATD;
				   temp.PreCarriageATA = MyEntityPM.PreCarriageATA;
				   temp.PreCarriageETD = MyEntityPM.PreCarriageETD;
				   temp.PreCarriageETA = MyEntityPM.PreCarriageETA; 

			  
				   if(MyEntityPM.ConsigneeNotImporterId != null)
				   {
					   CardQueryService CardService46 = new CardQueryService(Tenant);
					   					   temp.ConsigneeNotImporter = CardService46.GetCardById(MyEntityPM.ConsigneeNotImporterId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.DeclarationNumber = MyEntityPM.DeclarationNumber;
				   temp.DeclarationDate = MyEntityPM.DeclarationDate;
				   temp.BookingConfirmationNotes = MyEntityPM.BookingConfirmationNotes;
				   temp.FreightRelease = MyEntityPM.FreightRelease;
				   temp.TerminalAvailable = MyEntityPM.TerminalAvailable; 

			  
				   if(MyEntityPM.Notify1Id != null)
				   {
					   CardQueryService CardService47 = new CardQueryService(Tenant);
					   					   temp.Notify1 = CardService47.GetCardById(MyEntityPM.Notify1Id,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ShipmentPM DirectDataMappingAndValidatin(Direct MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
						temp = query.GetSinglePMByShipmentNumber(MyEntity.ShipmentNumber, Tenant);
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

						 								//throw new ApplicationException("ShipmentType Can't be update"); 
								temp.ShipmentTypeId = myShipmentTypePM.Id;
						  

							
						} 

					}
			
					
					DirectionQueryService DirectionDirectionService = new DirectionQueryService(Tenant);
					if(MyEntity.Direction != null)
					{
						var myDirectionPM = DirectionDirectionService.DirectionDataMappingAndValidatin(MyEntity.Direction,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDirectionPM != null)
						{ 

						 								//throw new ApplicationException("Direction Can't be update"); 
								temp.DirectionId = myDirectionPM.Id;
						  

							
						} 

					}
			
					
					TransportModeQueryService TransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.TransportMode != null)
					{
						var myTransportModePM = TransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.TransportMode,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTransportModePM != null)
						{ 

						 								//throw new ApplicationException("TransportMode Can't be update"); 
								temp.TransportModeId = myTransportModePM.Id;
						  

							
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

						 								//throw new ApplicationException("FromPort Can't be update"); 
								temp.FromPortId = myFromPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService ToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.ToPort != null)
					{
						var myToPortPM = ToPortPortService.PortDataMappingAndValidatin(MyEntity.ToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myToPortPM != null)
						{ 

						 								//throw new ApplicationException("ToPort Can't be update"); 
								temp.ToPortId = myToPortPM.Id;
						  

							
						} 

					}
			
					
					WeightUnitQueryService GrossWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.GrossWeightUnit != null)
					{
						var myGrossWeightUnitPM = GrossWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.GrossWeightUnit,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myGrossWeightUnitPM != null)
						{ 

						 								//throw new ApplicationException("GrossWeightUnit Can't be update"); 
								temp.GrossWeightUnitCode = myGrossWeightUnitPM.Code;
						  

							
						} 

					}
			
					
					WeightUnitQueryService ChargeableWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.ChargeableWeightUnit != null)
					{
						var myChargeableWeightUnitPM = ChargeableWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.ChargeableWeightUnit,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myChargeableWeightUnitPM != null)
						{ 

						 								//throw new ApplicationException("ChargeableWeightUnit Can't be update"); 
								temp.ChargeableWeightUnitCode = myChargeableWeightUnitPM.Code;
						  

							
						} 

					}
			
					
					VolumeUnitQueryService VolumeUnitVolumeUnitService = new VolumeUnitQueryService(Tenant);
					if(MyEntity.VolumeUnit != null)
					{
						var myVolumeUnitPM = VolumeUnitVolumeUnitService.VolumeUnitDataMappingAndValidatin(MyEntity.VolumeUnit,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myVolumeUnitPM != null)
						{ 

						 								//throw new ApplicationException("VolumeUnit Can't be update"); 
								temp.VolumeUnitCode = myVolumeUnitPM.Code;
						  

							
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
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.DescriptionOfGoods))
					{							//throw new ApplicationException("DescriptionOfGoods Can't be update"); 
							temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;

										}  

					 

					if(MyEntity.AirPackages != null && MyEntity.AirPackages.Count > 0)
					{
						AirPackageQueryService AirPackageService48 = new AirPackageQueryService(Tenant);
						 								//throw new ApplicationException("AirPackages Can't be update"); 
								temp.ShipmentPackages = AirPackageService48.AirPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.AirPackages,Tenant,ComputingPartnerName);

					 

						
					}

								  

					if(MyEntity.OceanOrInlandPackages != null && MyEntity.OceanOrInlandPackages.Count > 0)
					{
						OceanOrInlandPackageQueryService OceanOrInlandPackageService48 = new OceanOrInlandPackageQueryService(Tenant);
						 								//throw new ApplicationException("OceanOrInlandPackages Can't be update"); 
								temp.ShipmentPackages = OceanOrInlandPackageService48.OceanOrInlandPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.OceanOrInlandPackages,Tenant,ComputingPartnerName);

					 

						
					}

								  

					if(MyEntity.Containers != null && MyEntity.Containers.Count > 0)
					{
						ContainerQueryService ContainerService48 = new ContainerQueryService(Tenant);
						 								//throw new ApplicationException("Containers Can't be update"); 
								temp.ShipmentPackages = ContainerService48.ContainerCustomDataMappingAndValidatin(MyEntity,MyEntity.Containers,Tenant,ComputingPartnerName);

					 

						
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

					
                    							//throw new ApplicationException("NumberOfPackages Can't be update"); 
							temp.NumberOfPackages = MyEntity.NumberOfPackages;

					 

					
                    
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

					
                    							//throw new ApplicationException("ChargeableWeight Can't be update"); 
							temp.ChargeableWeight = MyEntity.ChargeableWeight;

					 

					
                    							//throw new ApplicationException("Master Can't be update"); 
							temp.Master = MyEntity.Master;

					 

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ShipmentNumber))
					{							//throw new ApplicationException("ShipmentNumber Can't be update"); 
							temp.ShipmentNumber = MyEntity.ShipmentNumber;

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
			
					 

					if(MyEntity.Deliveries != null && MyEntity.Deliveries.Count > 0)
					{
						DeliveryQueryService DeliveryService48 = new DeliveryQueryService(Tenant);
						 								//throw new ApplicationException("Deliveries Can't be update"); 
								temp.ShipmentDeliveries = DeliveryService48.DeliveryDataMappingAndValidatin(MyEntity.Deliveries,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								  

					if(MyEntity.PickUps != null && MyEntity.PickUps.Count > 0)
					{
						PickUpQueryService PickUpService48 = new PickUpQueryService(Tenant);
						 								//throw new ApplicationException("PickUps Can't be update"); 
								temp.ShipmentPickUps = PickUpService48.PickUpDataMappingAndValidatin(MyEntity.PickUps,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					
                    
					if(!IsUpdate)// && (MyEntity.IsOperationalClosed != temp.IsOperationalClosed))
					{							//throw new ApplicationException("IsOperationalClosed Can't be update"); 
							temp.IsOperationalClosed = MyEntity.IsOperationalClosed;

										}  

					
					VesselQueryService VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Vessel != null)
					{
						var myVesselPM = VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Vessel,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myVesselPM != null)
						{ 

						 								//throw new ApplicationException("Vessel Can't be update"); 
								temp.MainCarriageVesselId = myVesselPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("MainCarriageATA Can't be update"); 
							temp.MainCarriageATA = MyEntity.MainCarriageATA;

					 

					
                    							//throw new ApplicationException("MainCarriageATD Can't be update"); 
							temp.MainCarriageATD = MyEntity.MainCarriageATD;

					 

					
                    
					if(!IsUpdate)// && (MyEntity.IsAccountingClosed != temp.IsAccountingClosed))
					{							//throw new ApplicationException("IsAccountingClosed Can't be update"); 
							temp.IsAccountingClosed = MyEntity.IsAccountingClosed;

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
			
					
                    							//throw new ApplicationException("MainCarriageCarrierNumber Can't be update"); 
							temp.MainCarriageCarrierNumber = MyEntity.MainCarriageCarrierNumber;

					 

					
					CardQueryService MainCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageCarrier != null)
					{
						var myMainCarriageCarrierPM = MainCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainCarriageCarrierPM != null)
						{ 

						 								//throw new ApplicationException("MainCarriageCarrier Can't be update"); 
								temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
						  

							
						} 

					}
			
					 

					if(MyEntity.Receivables != null && MyEntity.Receivables.Count > 0)
					{
						ReceivableQueryService ReceivableService48 = new ReceivableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("Receivables Can't be update"); 
								temp.ShipmentReceivables = ReceivableService48.ReceivableDataMappingAndValidatin(MyEntity.Receivables,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.Payables != null && MyEntity.Payables.Count > 0)
					{
						PayableQueryService PayableService48 = new PayableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("Payables Can't be update"); 
								temp.ShipmentPayables = PayableService48.PayableDataMappingAndValidatin(MyEntity.Payables,Tenant,ComputingPartnerName,IsUpdate);

					 
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
			
					
                    
					if(!IsUpdate)// && MyEntity.MasterDate != null)
					{							//throw new ApplicationException("MasterDate Can't be update"); 
							temp.MAWBOBLDate = MyEntity.MasterDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.Ratio != null)
					{							//throw new ApplicationException("Ratio Can't be update"); 
							temp.Ratio = MyEntity.Ratio;

										}  

					 

					if(MyEntity.MainCarriageLegs != null && MyEntity.MainCarriageLegs.Count > 0)
					{
						MainCarriageLegQueryService MainCarriageLegService48 = new MainCarriageLegQueryService(Tenant);
						 								//throw new ApplicationException("MainCarriageLegs Can't be update"); 
								temp.MainCarriageLegs = MainCarriageLegService48.MainCarriageLegDataMappingAndValidatin(MyEntity.MainCarriageLegs,Tenant,ComputingPartnerName,IsUpdate);

					 

						
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
			
					
                    							//throw new ApplicationException("BookingConfirmationNumber Can't be update"); 
							temp.BookingConfirmationNumber = MyEntity.BookingConfirmationNumber;

					 

					
                    
					if(!IsUpdate)// && MyEntity.EstimatedFinalArrivalDate != null)
					{							//throw new ApplicationException("EstimatedFinalArrivalDate Can't be update"); 
							temp.EstimatedFinalArrivalDate = MyEntity.EstimatedFinalArrivalDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ActualFinalArrivalDate != null)
					{							//throw new ApplicationException("ActualFinalArrivalDate Can't be update"); 
							temp.ActualFinalArrivalDate = MyEntity.ActualFinalArrivalDate;

										}  

					
                    							//throw new ApplicationException("HouseNo Can't be update"); 
							temp.House = MyEntity.HouseNo;

					 

					
					CardQueryService MainCarriageFromPartnerCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageFromPartner != null)
					{
						var myMainCarriageFromPartnerPM = MainCarriageFromPartnerCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageFromPartner,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainCarriageFromPartnerPM != null)
						{ 

						 								//throw new ApplicationException("MainCarriageFromPartner Can't be update"); 
								temp.MainCarriageFromPartnerId = myMainCarriageFromPartnerPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService MainCarriageToPartnerCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageToPartner != null)
					{
						var myMainCarriageToPartnerPM = MainCarriageToPartnerCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageToPartner,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainCarriageToPartnerPM != null)
						{ 

						 								//throw new ApplicationException("MainCarriageToPartner Can't be update"); 
								temp.MainCarriageToPartnerId = myMainCarriageToPartnerPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("MainCarriageETA Can't be update"); 
							temp.MainCarriageETA = MyEntity.MainCarriageETA;

					 

					
                    							//throw new ApplicationException("MainCarriageETD Can't be update"); 
							temp.MainCarriageETD = MyEntity.MainCarriageETD;

					 

					
                    							//throw new ApplicationException("TruckNumber Can't be update"); 
							temp.TruckNumber = MyEntity.TruckNumber;

					 

					
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

					 

					
					PickUpDeliveryFromToTypeQueryService InlandDomesticFromTypeCodePickUpDeliveryFromToTypeService = new PickUpDeliveryFromToTypeQueryService(Tenant);
					if(MyEntity.InlandDomesticFromTypeCode != null)
					{
						var myInlandDomesticFromTypeCodePM = InlandDomesticFromTypeCodePickUpDeliveryFromToTypeService.PickUpDeliveryFromToTypeDataMappingAndValidatin(MyEntity.InlandDomesticFromTypeCode,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myInlandDomesticFromTypeCodePM != null)
						{ 

						 								//throw new ApplicationException("InlandDomesticFromTypeCode Can't be update"); 
								temp.InlandDomesticFromTypeCode = myInlandDomesticFromTypeCodePM.Code;
						  

							
						} 

					}
			
					
					PickUpDeliveryFromToTypeQueryService InlandDomesticToTypeCodePickUpDeliveryFromToTypeService = new PickUpDeliveryFromToTypeQueryService(Tenant);
					if(MyEntity.InlandDomesticToTypeCode != null)
					{
						var myInlandDomesticToTypeCodePM = InlandDomesticToTypeCodePickUpDeliveryFromToTypeService.PickUpDeliveryFromToTypeDataMappingAndValidatin(MyEntity.InlandDomesticToTypeCode,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myInlandDomesticToTypeCodePM != null)
						{ 

						 								//throw new ApplicationException("InlandDomesticToTypeCode Can't be update"); 
								temp.InlandDomesticToTypeCode = myInlandDomesticToTypeCodePM.Code;
						  

							
						} 

					}
			
					
					CountryQueryService InlandDomesticFromCountryCountryService = new CountryQueryService(Tenant);
					if(MyEntity.InlandDomesticFromCountry != null)
					{
						var myInlandDomesticFromCountryPM = InlandDomesticFromCountryCountryService.CountryDataMappingAndValidatin(MyEntity.InlandDomesticFromCountry,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myInlandDomesticFromCountryPM != null)
						{ 

						 								//throw new ApplicationException("InlandDomesticFromCountry Can't be update"); 
								temp.InlandDomesticFromCountryId = myInlandDomesticFromCountryPM.Id;
						  

							
						} 

					}
			
					
					CountryQueryService InlandDomesticToCountryCountryService = new CountryQueryService(Tenant);
					if(MyEntity.InlandDomesticToCountry != null)
					{
						var myInlandDomesticToCountryPM = InlandDomesticToCountryCountryService.CountryDataMappingAndValidatin(MyEntity.InlandDomesticToCountry,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myInlandDomesticToCountryPM != null)
						{ 

						 								//throw new ApplicationException("InlandDomesticToCountry Can't be update"); 
								temp.InlandDomesticToCountryId = myInlandDomesticToCountryPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("InlandDomesticFromZipCode Can't be update"); 
							temp.InlandDomesticFromZipCode = MyEntity.InlandDomesticFromZipCode;

					 

					
                    							//throw new ApplicationException("InlandDomesticToZipCode Can't be update"); 
							temp.InlandDomesticToZipCode = MyEntity.InlandDomesticToZipCode;

					 

					
                    							//throw new ApplicationException("InlandDomesticFromCity Can't be update"); 
							temp.InlandDomesticFromCity = MyEntity.InlandDomesticFromCity;

					 

					
                    							//throw new ApplicationException("InlandDomesticToCity Can't be update"); 
							temp.InlandDomesticToCity = MyEntity.InlandDomesticToCity;

					 

					
					PortQueryService MainCarriageFromPortPortService = new PortQueryService(Tenant);
					if(MyEntity.MainCarriageFromPort != null)
					{
						var myMainCarriageFromPortPM = MainCarriageFromPortPortService.PortDataMappingAndValidatin(MyEntity.MainCarriageFromPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainCarriageFromPortPM != null)
						{ 

						 								//throw new ApplicationException("MainCarriageFromPort Can't be update"); 
								temp.MainCarriageFromPortId = myMainCarriageFromPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService MainCarriageToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.MainCarriageToPort != null)
					{
						var myMainCarriageToPortPM = MainCarriageToPortPortService.PortDataMappingAndValidatin(MyEntity.MainCarriageToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainCarriageToPortPM != null)
						{ 

						 								//throw new ApplicationException("MainCarriageToPort Can't be update"); 
								temp.MainCarriageToPortId = myMainCarriageToPortPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("CutoffDate Can't be update"); 
							temp.CutoffDate = MyEntity.CutoffDate;

					 

					
                    							//throw new ApplicationException("IsDangerous Can't be update"); 
							temp.IsDangerous = MyEntity.IsDangerous;

					 

					
                    							//throw new ApplicationException("HAWBDate Can't be update"); 
							temp.HAWBDate = MyEntity.HAWBDate;

					 

					
					TransportModeQueryService OnCarriageTransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.OnCarriageTransportMode != null)
					{
						var myOnCarriageTransportModePM = OnCarriageTransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.OnCarriageTransportMode,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnCarriageTransportModePM != null)
						{ 

						 								//throw new ApplicationException("OnCarriageTransportMode Can't be update"); 
								temp.OnCarriageTransportModeId = myOnCarriageTransportModePM.Id;
						  

							
						} 

					}
			
					
					PortQueryService OnCarriageFromPortPortService = new PortQueryService(Tenant);
					if(MyEntity.OnCarriageFromPort != null)
					{
						var myOnCarriageFromPortPM = OnCarriageFromPortPortService.PortDataMappingAndValidatin(MyEntity.OnCarriageFromPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnCarriageFromPortPM != null)
						{ 

						 								//throw new ApplicationException("OnCarriageFromPort Can't be update"); 
								temp.OnCarriageFromPortId = myOnCarriageFromPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService OnCarriageToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.OnCarriageToPort != null)
					{
						var myOnCarriageToPortPM = OnCarriageToPortPortService.PortDataMappingAndValidatin(MyEntity.OnCarriageToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnCarriageToPortPM != null)
						{ 

						 								//throw new ApplicationException("OnCarriageToPort Can't be update"); 
								temp.OnCarriageToPortId = myOnCarriageToPortPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService OnCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.OnCarriageCarrier != null)
					{
						var myOnCarriageCarrierPM = OnCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.OnCarriageCarrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myOnCarriageCarrierPM != null)
						{ 

						 								//throw new ApplicationException("OnCarriageCarrier Can't be update"); 
								temp.OnCarriageCarrierId = myOnCarriageCarrierPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("OnCarriageCarrierNumber Can't be update"); 
							temp.OnCarriageCarrierNumber = MyEntity.OnCarriageCarrierNumber;

					 

					
                    							//throw new ApplicationException("OnCarriageATD Can't be update"); 
							temp.OnCarriageATD = MyEntity.OnCarriageATD;

					 

					
                    							//throw new ApplicationException("OnCarriageATA Can't be update"); 
							temp.OnCarriageATA = MyEntity.OnCarriageATA;

					 

					
                    							//throw new ApplicationException("OnCarriageETD Can't be update"); 
							temp.OnCarriageETD = MyEntity.OnCarriageETD;

					 

					
                    							//throw new ApplicationException("OnCarriageETA Can't be update"); 
							temp.OnCarriageETA = MyEntity.OnCarriageETA;

					 

					
					TransportModeQueryService PreCarriageTransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.PreCarriageTransportMode != null)
					{
						var myPreCarriageTransportModePM = PreCarriageTransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.PreCarriageTransportMode,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreCarriageTransportModePM != null)
						{ 

						 								//throw new ApplicationException("PreCarriageTransportMode Can't be update"); 
								temp.PreCarriageTransportModeId = myPreCarriageTransportModePM.Id;
						  

							
						} 

					}
			
					
					PortQueryService PreCarriageFromPortPortService = new PortQueryService(Tenant);
					if(MyEntity.PreCarriageFromPort != null)
					{
						var myPreCarriageFromPortPM = PreCarriageFromPortPortService.PortDataMappingAndValidatin(MyEntity.PreCarriageFromPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreCarriageFromPortPM != null)
						{ 

						 								//throw new ApplicationException("PreCarriageFromPort Can't be update"); 
								temp.PreCarriageFromPortId = myPreCarriageFromPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService PreCarriageToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.PreCarriageToPort != null)
					{
						var myPreCarriageToPortPM = PreCarriageToPortPortService.PortDataMappingAndValidatin(MyEntity.PreCarriageToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreCarriageToPortPM != null)
						{ 

						 								//throw new ApplicationException("PreCarriageToPort Can't be update"); 
								temp.PreCarriageToPortId = myPreCarriageToPortPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService PreCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.PreCarriageCarrier != null)
					{
						var myPreCarriageCarrierPM = PreCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.PreCarriageCarrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPreCarriageCarrierPM != null)
						{ 

						 								//throw new ApplicationException("PreCarriageCarrier Can't be update"); 
								temp.PreCarriageCarrierId = myPreCarriageCarrierPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("PreCarriageCarrierNumber Can't be update"); 
							temp.PreCarriageCarrierNumber = MyEntity.PreCarriageCarrierNumber;

					 

					
                    							//throw new ApplicationException("PreCarriageATD Can't be update"); 
							temp.PreCarriageATD = MyEntity.PreCarriageATD;

					 

					
                    							//throw new ApplicationException("PreCarriageATA Can't be update"); 
							temp.PreCarriageATA = MyEntity.PreCarriageATA;

					 

					
                    							//throw new ApplicationException("PreCarriageETD Can't be update"); 
							temp.PreCarriageETD = MyEntity.PreCarriageETD;

					 

					
                    							//throw new ApplicationException("PreCarriageETA Can't be update"); 
							temp.PreCarriageETA = MyEntity.PreCarriageETA;

					 

					
					CardQueryService ConsigneeNotImporterCardService = new CardQueryService(Tenant);
					if(MyEntity.ConsigneeNotImporter != null)
					{
						var myConsigneeNotImporterPM = ConsigneeNotImporterCardService.CardDataMappingAndValidatin(MyEntity.ConsigneeNotImporter,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myConsigneeNotImporterPM != null)
						{ 

						 								//throw new ApplicationException("ConsigneeNotImporter Can't be update"); 
								temp.ConsigneeNotImporterId = myConsigneeNotImporterPM.Id;
						  

							
						} 

					}
			
					
                    							//throw new ApplicationException("DeclarationNumber Can't be update"); 
							temp.DeclarationNumber = MyEntity.DeclarationNumber;

					 

					
                    							//throw new ApplicationException("DeclarationDate Can't be update"); 
							temp.DeclarationDate = MyEntity.DeclarationDate;

					 

					
                    							//throw new ApplicationException("BookingConfirmationNotes Can't be update"); 
							temp.BookingConfirmationNotes = MyEntity.BookingConfirmationNotes;

					 

					
                    							//throw new ApplicationException("FreightRelease Can't be update"); 
							temp.FreightRelease = MyEntity.FreightRelease;

					 

					
                    							//throw new ApplicationException("TerminalAvailable Can't be update"); 
							temp.TerminalAvailable = MyEntity.TerminalAvailable;

					 

					
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
			
										   
					return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}
