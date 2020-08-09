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

		
		public Direct GetDirectById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Shipment with Id " + Id + " doesn't exist");

				return DirectDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Direct GetDirectByShipmentNumber(string ShipmentNumber,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePMByShipmentNumber(ShipmentNumber,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Shipment with ShipmentNumber " + ShipmentNumber + " doesn't exist");

				return DirectDataMapping(temp,Tenant);
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
					   					   temp.ShipmentType = ShipmentTypeService0.ShipmentTypeCustomDataMapping(MyEntityPM.ShipmentTypeId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DirectionId != null)
				   {
					   DirectionQueryService DirectionService1 = new DirectionQueryService(Tenant);
					   					   temp.Direction = DirectionService1.GetDirectionById(MyEntityPM.DirectionId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService2 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService2.GetTransportModeById(MyEntityPM.TransportModeId,Tenant); 
			       
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

			  
				   if(MyEntityPM.CustomerId != null)
				   {
					   CardQueryService CardService5 = new CardQueryService(Tenant);
					   					   temp.Customer = CardService5.GetCardById(MyEntityPM.CustomerId,Tenant); 
			       
					   				   }
				    

			  
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
				    

			  
				   if(MyEntityPM.IncotermId != null)
				   {
					   IncotermQueryService IncotermService11 = new IncotermQueryService(Tenant);
					   					   temp.Incoterm = IncotermService11.GetIncotermById(MyEntityPM.IncotermId,Tenant); 
			       
					   				   }
				   
				   temp.DescriptionOfGoods = MyEntityPM.DescriptionOfGoods;
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 AirPackageQueryService AirPackageService12 = new AirPackageQueryService(Tenant);
					 temp.AirPackages = AirPackageService12.AirPackageCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant);
				}

							 
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 OceanOrInlandPackageQueryService OceanOrInlandPackageService12 = new OceanOrInlandPackageQueryService(Tenant);
					 temp.OceanOrInlandPackages = OceanOrInlandPackageService12.OceanOrInlandPackageCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant);
				}

							 
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 ContainerQueryService ContainerService12 = new ContainerQueryService(Tenant);
					 temp.Containers = ContainerService12.ContainerCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant);
				}

							 
				   temp.Commodity = MyEntityPM.AWBCommodityItemNumber; 

			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService12 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService12.GetBranchById(MyEntityPM.BranchId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DepartmentId != null)
				   {
					   DepartmentQueryService DepartmentService13 = new DepartmentQueryService(Tenant);
					   					   temp.Department = DepartmentService13.GetDepartmentById(MyEntityPM.DepartmentId,Tenant); 
			       
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
				 
				   
				   temp.IsOperationalClosed = MyEntityPM.IsOperationalClosed; 

			  
				   if(MyEntityPM.MainCarriageVesselId != null)
				   {
					   VesselQueryService VesselService16 = new VesselQueryService(Tenant);
					   					   temp.Vessel = VesselService16.GetVesselById(MyEntityPM.MainCarriageVesselId,Tenant); 
			       
					   				   }
				   
				   temp.MainCarriageATA = MyEntityPM.MainCarriageATA;
				   temp.MainCarriageATD = MyEntityPM.MainCarriageATD;
				   temp.IsAccountingClosed = MyEntityPM.IsAccountingClosed;
				   temp.ValueOfGoods = MyEntityPM.ValueOfGoods; 

			  
				   if(MyEntityPM.ValueOfGoodsCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService17 = new CurrencyQueryService(Tenant);
					   					   temp.ValueOfGoodsCurrency = CurrencyService17.GetCurrencyById(MyEntityPM.ValueOfGoodsCurrencyId,Tenant); 
			       
					   				   }
				   
				   temp.MainCarriageCarrierNumber = MyEntityPM.MainCarriageCarrierNumber; 

			  
				   if(MyEntityPM.MainCarriageCarrierId != null)
				   {
					   CardQueryService CardService18 = new CardQueryService(Tenant);
					   					   temp.MainCarriageCarrier = CardService18.GetCardById(MyEntityPM.MainCarriageCarrierId,Tenant); 
			       
					   				   }
				   
				if(MyEntityPM.ShipmentReceivables != null && MyEntityPM.ShipmentReceivables.Count > 0)
				{
					 ReceivableQueryService ReceivableService19 = new ReceivableQueryService(Tenant);
					 temp.Receivables = ReceivableService19.ReceivableDataMapping(MyEntityPM.ShipmentReceivables,Tenant);
				}

							 
				if(MyEntityPM.ShipmentPayables != null && MyEntityPM.ShipmentPayables.Count > 0)
				{
					 PayableQueryService PayableService19 = new PayableQueryService(Tenant);
					 temp.Payables = PayableService19.PayableDataMapping(MyEntityPM.ShipmentPayables,Tenant);
				}

							  

			  
				   if(MyEntityPM.DimensionsUnitCode != null)
				   {
					   DimensionsUnitQueryService DimensionsUnitService19 = new DimensionsUnitQueryService(Tenant);
					   					   temp.DimensionsUnit = DimensionsUnitService19.GetDimensionsUnitByCode(MyEntityPM.DimensionsUnitCode,Tenant); 
			       
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
					   					   temp.SalesmanUser = UserService20.GetUserById(MyEntityPM.SalesmanUserId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.AccountManagerUserId != null)
				   {
					   UserQueryService UserService21 = new UserQueryService(Tenant);
					   					   temp.AccountManagerUser = UserService21.GetUserById(MyEntityPM.AccountManagerUserId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.SpecialServicesTypeId != null)
				   {
					   SpecialServicesTypeQueryService SpecialServicesTypeService22 = new SpecialServicesTypeQueryService(Tenant);
					   					   temp.SpecialServicesType = SpecialServicesTypeService22.GetSpecialServicesTypeById(MyEntityPM.SpecialServicesTypeId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipperNotExporterId != null)
				   {
					   CardQueryService CardService23 = new CardQueryService(Tenant);
					   					   temp.ShipperNotExporter = CardService23.GetCardById(MyEntityPM.ShipperNotExporterId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.AgentId != null)
				   {
					   CardQueryService CardService24 = new CardQueryService(Tenant);
					   					   temp.Agent = CardService24.GetCardById(MyEntityPM.AgentId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.CustomAgentImportId != null)
				   {
					   CardQueryService CardService25 = new CardQueryService(Tenant);
					   					   temp.CustomAgentImport = CardService25.GetCardById(MyEntityPM.CustomAgentImportId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ReleasingAgentId != null)
				   {
					   CardQueryService CardService26 = new CardQueryService(Tenant);
					   					   temp.ReleasingAgent = CardService26.GetCardById(MyEntityPM.ReleasingAgentId,Tenant); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.FreightForwarderId != null)
				   {
					   CardQueryService CardService27 = new CardQueryService(Tenant);
					   					   temp.FreightForwarder = CardService27.GetCardById(MyEntityPM.FreightForwarderId,Tenant); 
			       
					   				   }
				   
				   temp.MAWBDate = MyEntityPM.MAWBOBLDate;
				   temp.Ratio = MyEntityPM.Ratio;
				if(MyEntityPM.Transshipments != null && MyEntityPM.Transshipments.Count > 0)
				{
					 TransshipmentQueryService TransshipmentService28 = new TransshipmentQueryService(Tenant);
					 temp.Transshipments = TransshipmentService28.TransshipmentDataMapping(MyEntityPM.Transshipments,Tenant);
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

						 
							if(IsUpdate)
							{
								throw new ApplicationException("ShipmentType Can't be update"); 
							}  

							temp.ShipmentTypeId = myShipmentTypePM.Id;
						} 

					}
			
					
					DirectionQueryService DirectionDirectionService = new DirectionQueryService(Tenant);
					if(MyEntity.Direction != null)
					{
						var myDirectionPM = DirectionDirectionService.DirectionDataMappingAndValidatin(MyEntity.Direction,Tenant,ComputingPartnerName);
						
						if(myDirectionPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Direction Can't be update"); 
							}  

							temp.DirectionId = myDirectionPM.Id;
						} 

					}
			
					
					TransportModeQueryService TransportModeTransportModeService = new TransportModeQueryService(Tenant);
					if(MyEntity.TransportMode != null)
					{
						var myTransportModePM = TransportModeTransportModeService.TransportModeDataMappingAndValidatin(MyEntity.TransportMode,Tenant,ComputingPartnerName);
						
						if(myTransportModePM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("TransportMode Can't be update"); 
							}  

							temp.TransportModeId = myTransportModePM.Id;
						} 

					}
			
					
					CardQueryService ShipperCardService = new CardQueryService(Tenant);
					if(MyEntity.Shipper != null)
					{
						var myShipperPM = ShipperCardService.CardDataMappingAndValidatin(MyEntity.Shipper,Tenant,ComputingPartnerName);
						
						if(myShipperPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Shipper Can't be update"); 
							}  

							temp.ShipperId = myShipperPM.Id;
						} 

					}
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("ShipperReference1 Can't be update"); 
					}  

					temp.ShipperReference1 = MyEntity.ShipperReference1;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("ShipperReference2 Can't be update"); 
					}  

					temp.ShipperReference2 = MyEntity.ShipperReference2;
					CardQueryService ConsigneeCardService = new CardQueryService(Tenant);
					if(MyEntity.Consignee != null)
					{
						var myConsigneePM = ConsigneeCardService.CardDataMappingAndValidatin(MyEntity.Consignee,Tenant,ComputingPartnerName);
						
						if(myConsigneePM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Consignee Can't be update"); 
							}  

							temp.ConsigneeId = myConsigneePM.Id;
						} 

					}
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("ConsigneeReference1 Can't be update"); 
					}  

					temp.ConsigneeReference1 = MyEntity.ConsigneeReference1;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("ConsigneeReference2 Can't be update"); 
					}  

					temp.ConsigneeReference2 = MyEntity.ConsigneeReference2;
					CardQueryService CustomerCardService = new CardQueryService(Tenant);
					if(MyEntity.Customer != null)
					{
						var myCustomerPM = CustomerCardService.CardDataMappingAndValidatin(MyEntity.Customer,Tenant,ComputingPartnerName);
						
						if(myCustomerPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Customer Can't be update"); 
							}  

							temp.CustomerId = myCustomerPM.Id;
						} 

					}
			
					
					PortQueryService FromPortPortService = new PortQueryService(Tenant);
					if(MyEntity.FromPort != null)
					{
						var myFromPortPM = FromPortPortService.PortDataMappingAndValidatin(MyEntity.FromPort,Tenant,ComputingPartnerName);
						
						if(myFromPortPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("FromPort Can't be update"); 
							}  

							temp.FromPortId = myFromPortPM.Id;
						} 

					}
			
					
					PortQueryService ToPortPortService = new PortQueryService(Tenant);
					if(MyEntity.ToPort != null)
					{
						var myToPortPM = ToPortPortService.PortDataMappingAndValidatin(MyEntity.ToPort,Tenant,ComputingPartnerName);
						
						if(myToPortPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("ToPort Can't be update"); 
							}  

							temp.ToPortId = myToPortPM.Id;
						} 

					}
			
					
					WeightUnitQueryService GrossWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.GrossWeightUnit != null)
					{
						var myGrossWeightUnitPM = GrossWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.GrossWeightUnit,Tenant,ComputingPartnerName);
						
						if(myGrossWeightUnitPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("GrossWeightUnit Can't be update"); 
							}  

							temp.GrossWeightUnitCode = myGrossWeightUnitPM.Code;
						} 

					}
			
					
					WeightUnitQueryService ChargeableWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.ChargeableWeightUnit != null)
					{
						var myChargeableWeightUnitPM = ChargeableWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.ChargeableWeightUnit,Tenant,ComputingPartnerName);
						
						if(myChargeableWeightUnitPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("ChargeableWeightUnit Can't be update"); 
							}  

							temp.ChargeableWeightUnitCode = myChargeableWeightUnitPM.Code;
						} 

					}
			
					
					VolumeUnitQueryService VolumeUnitVolumeUnitService = new VolumeUnitQueryService(Tenant);
					if(MyEntity.VolumeUnit != null)
					{
						var myVolumeUnitPM = VolumeUnitVolumeUnitService.VolumeUnitDataMappingAndValidatin(MyEntity.VolumeUnit,Tenant,ComputingPartnerName);
						
						if(myVolumeUnitPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("VolumeUnit Can't be update"); 
							}  

							temp.VolumeUnitCode = myVolumeUnitPM.Code;
						} 

					}
			
					
					IncotermQueryService IncotermIncotermService = new IncotermQueryService(Tenant);
					if(MyEntity.Incoterm != null)
					{
						var myIncotermPM = IncotermIncotermService.IncotermDataMappingAndValidatin(MyEntity.Incoterm,Tenant,ComputingPartnerName);
						
						if(myIncotermPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Incoterm Can't be update"); 
							}  

							temp.IncotermId = myIncotermPM.Id;
						} 

					}
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("DescriptionOfGoods Can't be update"); 
					}  

					temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods; 

					if(MyEntity.AirPackages != null && MyEntity.AirPackages.Count > 0)
					{
						AirPackageQueryService AirPackageService28 = new AirPackageQueryService(Tenant);
						  
						if(IsUpdate)
						{
								throw new ApplicationException("AirPackages Can't be update"); 
						}  

						temp.ShipmentPackages = AirPackageService28.AirPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.AirPackages,Tenant,ComputingPartnerName);
						
					}

								  

					if(MyEntity.OceanOrInlandPackages != null && MyEntity.OceanOrInlandPackages.Count > 0)
					{
						OceanOrInlandPackageQueryService OceanOrInlandPackageService28 = new OceanOrInlandPackageQueryService(Tenant);
						  
						if(IsUpdate)
						{
								throw new ApplicationException("OceanOrInlandPackages Can't be update"); 
						}  

						temp.ShipmentPackages = OceanOrInlandPackageService28.OceanOrInlandPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.OceanOrInlandPackages,Tenant,ComputingPartnerName);
						
					}

								  

					if(MyEntity.Containers != null && MyEntity.Containers.Count > 0)
					{
						ContainerQueryService ContainerService28 = new ContainerQueryService(Tenant);
						  
						if(IsUpdate)
						{
								throw new ApplicationException("Containers Can't be update"); 
						}  

						temp.ShipmentPackages = ContainerService28.ContainerCustomDataMappingAndValidatin(MyEntity,MyEntity.Containers,Tenant,ComputingPartnerName);
						
					}

								 
                    
					if(IsUpdate)
					{
							throw new ApplicationException("Commodity Can't be update"); 
					}  

					temp.AWBCommodityItemNumber = MyEntity.Commodity;
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName);
						
						if(myBranchPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Branch Can't be update"); 
							}  

							temp.BranchId = myBranchPM.Id;
						} 

					}
			
					
					DepartmentQueryService DepartmentDepartmentService = new DepartmentQueryService(Tenant);
					if(MyEntity.Department != null)
					{
						var myDepartmentPM = DepartmentDepartmentService.DepartmentDataMappingAndValidatin(MyEntity.Department,Tenant,ComputingPartnerName);
						
						if(myDepartmentPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Department Can't be update"); 
							}  

							temp.DepartmentId = myDepartmentPM.Id;
						} 

					}
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("TEU Can't be update"); 
					}  

					temp.TEU = MyEntity.TEU;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("NumberOfPackages Can't be update"); 
					}  

					temp.NumberOfPackages = MyEntity.NumberOfPackages;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("GrossWeight Can't be update"); 
					}  

					temp.GrossWeight = MyEntity.GrossWeight;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("Volume Can't be update"); 
					}  

					temp.Volume = MyEntity.Volume;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("VolumetricWeight Can't be update"); 
					}  

					temp.VolumetricWeight = MyEntity.VolumetricWeight;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("ChargeableWeight Can't be update"); 
					}  

					temp.ChargeableWeight = MyEntity.ChargeableWeight;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("Master Can't be update"); 
					}  

					temp.Master = MyEntity.Master;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("ShipmentNumber Can't be update"); 
					}  

					temp.ShipmentNumber = MyEntity.ShipmentNumber;
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName);
						
						if(myCreatedByUserPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("CreatedByUser Can't be update"); 
							}  

							temp.CreatedByUserId = myCreatedByUserPM.Id;
						} 

					}
			
					 

					if(MyEntity.Deliveries != null && MyEntity.Deliveries.Count > 0)
					{
						DeliveryQueryService DeliveryService28 = new DeliveryQueryService(Tenant);
						  
						if(IsUpdate)
						{
								throw new ApplicationException("Deliveries Can't be update"); 
						}  

						temp.ShipmentDeliveries = DeliveryService28.DeliveryDataMappingAndValidatin(MyEntity.Deliveries,Tenant,ComputingPartnerName);
						
					}

								  

					if(MyEntity.PickUps != null && MyEntity.PickUps.Count > 0)
					{
						PickUpQueryService PickUpService28 = new PickUpQueryService(Tenant);
						  
						if(IsUpdate)
						{
								throw new ApplicationException("PickUps Can't be update"); 
						}  

						temp.ShipmentPickUps = PickUpService28.PickUpDataMappingAndValidatin(MyEntity.PickUps,Tenant,ComputingPartnerName);
						
					}

								 
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("IsOperationalClosed Can't be update"); 
					}  

					temp.IsOperationalClosed = MyEntity.IsOperationalClosed;
					VesselQueryService VesselVesselService = new VesselQueryService(Tenant);
					if(MyEntity.Vessel != null)
					{
						var myVesselPM = VesselVesselService.VesselDataMappingAndValidatin(MyEntity.Vessel,Tenant,ComputingPartnerName);
						
						if(myVesselPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Vessel Can't be update"); 
							}  

							temp.MainCarriageVesselId = myVesselPM.Id;
						} 

					}
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("MainCarriageATA Can't be update"); 
					}  

					temp.MainCarriageATA = MyEntity.MainCarriageATA;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("MainCarriageATD Can't be update"); 
					}  

					temp.MainCarriageATD = MyEntity.MainCarriageATD;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("IsAccountingClosed Can't be update"); 
					}  

					temp.IsAccountingClosed = MyEntity.IsAccountingClosed;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("ValueOfGoods Can't be update"); 
					}  

					temp.ValueOfGoods = MyEntity.ValueOfGoods;
					CurrencyQueryService ValueOfGoodsCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.ValueOfGoodsCurrency != null)
					{
						var myValueOfGoodsCurrencyPM = ValueOfGoodsCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.ValueOfGoodsCurrency,Tenant,ComputingPartnerName);
						
						if(myValueOfGoodsCurrencyPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("ValueOfGoodsCurrency Can't be update"); 
							}  

							temp.ValueOfGoodsCurrencyId = myValueOfGoodsCurrencyPM.Id;
						} 

					}
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("MainCarriageCarrierNumber Can't be update"); 
					}  

					temp.MainCarriageCarrierNumber = MyEntity.MainCarriageCarrierNumber;
					CardQueryService MainCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageCarrier != null)
					{
						var myMainCarriageCarrierPM = MainCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier,Tenant,ComputingPartnerName);
						
						if(myMainCarriageCarrierPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("MainCarriageCarrier Can't be update"); 
							}  

							temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
						} 

					}
			
					 

					if(MyEntity.Receivables != null && MyEntity.Receivables.Count > 0)
					{
						ReceivableQueryService ReceivableService28 = new ReceivableQueryService(Tenant);
						  
						if(IsUpdate)
						{
								throw new ApplicationException("Receivables Can't be update"); 
						}  

						temp.ShipmentReceivables = ReceivableService28.ReceivableDataMappingAndValidatin(MyEntity.Receivables,Tenant,ComputingPartnerName);
						
					}

								  

					if(MyEntity.Payables != null && MyEntity.Payables.Count > 0)
					{
						PayableQueryService PayableService28 = new PayableQueryService(Tenant);
						  
						if(IsUpdate)
						{
								throw new ApplicationException("Payables Can't be update"); 
						}  

						temp.ShipmentPayables = PayableService28.PayableDataMappingAndValidatin(MyEntity.Payables,Tenant,ComputingPartnerName);
						
					}

								 
					DimensionsUnitQueryService DimensionsUnitDimensionsUnitService = new DimensionsUnitQueryService(Tenant);
					if(MyEntity.DimensionsUnit != null)
					{
						var myDimensionsUnitPM = DimensionsUnitDimensionsUnitService.DimensionsUnitDataMappingAndValidatin(MyEntity.DimensionsUnit,Tenant,ComputingPartnerName);
						
						if(myDimensionsUnitPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("DimensionsUnit Can't be update"); 
							}  

							temp.DimensionsUnitCode = myDimensionsUnitPM.Code;
						} 

					}
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("OrderNumberOfPackages Can't be update"); 
					}  

					temp.BookingNumberOfPackages = MyEntity.OrderNumberOfPackages;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("OrderGrossWeight Can't be update"); 
					}  

					temp.OrderGrossWeight = MyEntity.OrderGrossWeight;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("OrderVolume Can't be update"); 
					}  

					temp.BookingVolume = MyEntity.OrderVolume;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("OrderChargeableWeight Can't be update"); 
					}  

					temp.OrderChargeableWeight = MyEntity.OrderChargeableWeight;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("OrderIsDangerouseGoods Can't be update"); 
					}  

					temp.OrderIsDangerouseGoods = MyEntity.OrderIsDangerouseGoods;
                    
					if(IsUpdate)
					{
							throw new ApplicationException("MainHarmonize Can't be update"); 
					}  

					temp.MainHarmonize = MyEntity.MainHarmonize;
					UserQueryService SalesmanUserUserService = new UserQueryService(Tenant);
					if(MyEntity.SalesmanUser != null)
					{
						var mySalesmanUserPM = SalesmanUserUserService.UserDataMappingAndValidatin(MyEntity.SalesmanUser,Tenant,ComputingPartnerName);
						
						if(mySalesmanUserPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("SalesmanUser Can't be update"); 
							}  

							temp.SalesmanUserId = mySalesmanUserPM.Id;
						} 

					}
			
					
					UserQueryService AccountManagerUserUserService = new UserQueryService(Tenant);
					if(MyEntity.AccountManagerUser != null)
					{
						var myAccountManagerUserPM = AccountManagerUserUserService.UserDataMappingAndValidatin(MyEntity.AccountManagerUser,Tenant,ComputingPartnerName);
						
						if(myAccountManagerUserPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("AccountManagerUser Can't be update"); 
							}  

							temp.AccountManagerUserId = myAccountManagerUserPM.Id;
						} 

					}
			
					
					SpecialServicesTypeQueryService SpecialServicesTypeSpecialServicesTypeService = new SpecialServicesTypeQueryService(Tenant);
					if(MyEntity.SpecialServicesType != null)
					{
						var mySpecialServicesTypePM = SpecialServicesTypeSpecialServicesTypeService.SpecialServicesTypeDataMappingAndValidatin(MyEntity.SpecialServicesType,Tenant,ComputingPartnerName);
						
						if(mySpecialServicesTypePM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("SpecialServicesType Can't be update"); 
							}  

							temp.SpecialServicesTypeId = mySpecialServicesTypePM.Id;
						} 

					}
			
					
					CardQueryService ShipperNotExporterCardService = new CardQueryService(Tenant);
					if(MyEntity.ShipperNotExporter != null)
					{
						var myShipperNotExporterPM = ShipperNotExporterCardService.CardDataMappingAndValidatin(MyEntity.ShipperNotExporter,Tenant,ComputingPartnerName);
						
						if(myShipperNotExporterPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("ShipperNotExporter Can't be update"); 
							}  

							temp.ShipperNotExporterId = myShipperNotExporterPM.Id;
						} 

					}
			
					
					CardQueryService AgentCardService = new CardQueryService(Tenant);
					if(MyEntity.Agent != null)
					{
						var myAgentPM = AgentCardService.CardDataMappingAndValidatin(MyEntity.Agent,Tenant,ComputingPartnerName);
						
						if(myAgentPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("Agent Can't be update"); 
							}  

							temp.AgentId = myAgentPM.Id;
						} 

					}
			
					
					CardQueryService CustomAgentImportCardService = new CardQueryService(Tenant);
					if(MyEntity.CustomAgentImport != null)
					{
						var myCustomAgentImportPM = CustomAgentImportCardService.CardDataMappingAndValidatin(MyEntity.CustomAgentImport,Tenant,ComputingPartnerName);
						
						if(myCustomAgentImportPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("CustomAgentImport Can't be update"); 
							}  

							temp.CustomAgentImportId = myCustomAgentImportPM.Id;
						} 

					}
			
					
					CardQueryService ReleasingAgentCardService = new CardQueryService(Tenant);
					if(MyEntity.ReleasingAgent != null)
					{
						var myReleasingAgentPM = ReleasingAgentCardService.CardDataMappingAndValidatin(MyEntity.ReleasingAgent,Tenant,ComputingPartnerName);
						
						if(myReleasingAgentPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("ReleasingAgent Can't be update"); 
							}  

							temp.ReleasingAgentId = myReleasingAgentPM.Id;
						} 

					}
			
					
					CardQueryService FreightForwarderCardService = new CardQueryService(Tenant);
					if(MyEntity.FreightForwarder != null)
					{
						var myFreightForwarderPM = FreightForwarderCardService.CardDataMappingAndValidatin(MyEntity.FreightForwarder,Tenant,ComputingPartnerName);
						
						if(myFreightForwarderPM != null)
						{ 

						 
							if(IsUpdate)
							{
								throw new ApplicationException("FreightForwarder Can't be update"); 
							}  

							temp.FreightForwarderId = myFreightForwarderPM.Id;
						} 

					}
			
					
                    
					if(IsUpdate)
					{
							throw new ApplicationException("MAWBOBLDate Can't be update"); 
					}  
			
					

					temp.MAWBOBLDate = MyEntity.MAWBDate;

                    

					if(IsUpdate)
					{
							throw new ApplicationException("Ratio Can't be update"); 
					}  


					temp.Ratio = MyEntity.Ratio; 

					if(MyEntity.Transshipments != null && MyEntity.Transshipments.Count > 0)
					{
						TransshipmentQueryService TransshipmentService28 = new TransshipmentQueryService(Tenant);
						temp.Transshipments = TransshipmentService28.TransshipmentDataMappingAndValidatin(MyEntity.Transshipments,Tenant,ComputingPartnerName);
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
