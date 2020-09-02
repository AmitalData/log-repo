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
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ShipmentPM DirectDataMappingAndValidatin(Direct MyEntity,int Tenant,string ComputingPartnerName = "")
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
							temp.ShipmentTypeId = myShipmentTypePM.Id;
						}
						 
					}
			
					
					DirectionQueryService DirectionDirectionService = new DirectionQueryService(Tenant);
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
					temp.ShipperReference2 = MyEntity.ShipperReference2;
					CardQueryService ConsigneeCardService = new CardQueryService(Tenant);
					if(MyEntity.Consignee != null)
					{
						var myConsigneePM = ConsigneeCardService.CardDataMappingAndValidatin(MyEntity.Consignee,Tenant,ComputingPartnerName);
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
						var myCustomerPM = CustomerCardService.CardDataMappingAndValidatin(MyEntity.Customer,Tenant,ComputingPartnerName);
												if(myCustomerPM != null)
						{
							temp.CustomerId = myCustomerPM.Id;
						}
						 
					}
			
					
					PortQueryService FromPortPortService = new PortQueryService(Tenant);
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
			
					
					IncotermQueryService IncotermIncotermService = new IncotermQueryService(Tenant);
					if(MyEntity.Incoterm != null)
					{
						var myIncotermPM = IncotermIncotermService.IncotermDataMappingAndValidatin(MyEntity.Incoterm,Tenant,ComputingPartnerName);
												if(myIncotermPM != null)
						{
							temp.IncotermId = myIncotermPM.Id;
						}
						 
					}
			
					
					temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods; 

					if(MyEntity.AirPackages != null && MyEntity.AirPackages.Count > 0)
					{
						AirPackageQueryService AirPackageService20 = new AirPackageQueryService(Tenant);
						temp.ShipmentPackages = AirPackageService20.AirPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.AirPackages,Tenant,ComputingPartnerName);
					}

								  

					if(MyEntity.OceanOrInlandPackages != null && MyEntity.OceanOrInlandPackages.Count > 0)
					{
						OceanOrInlandPackageQueryService OceanOrInlandPackageService20 = new OceanOrInlandPackageQueryService(Tenant);
						temp.ShipmentPackages = OceanOrInlandPackageService20.OceanOrInlandPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.OceanOrInlandPackages,Tenant,ComputingPartnerName);
					}

								  

					if(MyEntity.Containers != null && MyEntity.Containers.Count > 0)
					{
						ContainerQueryService ContainerService20 = new ContainerQueryService(Tenant);
						temp.ShipmentPackages = ContainerService20.ContainerCustomDataMappingAndValidatin(MyEntity,MyEntity.Containers,Tenant,ComputingPartnerName);
					}

								 
					temp.AWBCommodityItemNumber = MyEntity.Commodity;
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
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
			
					
					temp.TEU = MyEntity.TEU;
					temp.NumberOfPackages = MyEntity.NumberOfPackages;
					temp.GrossWeight = MyEntity.GrossWeight;
					temp.Volume = MyEntity.Volume;
					temp.VolumetricWeight = MyEntity.VolumetricWeight;
					temp.ChargeableWeight = MyEntity.ChargeableWeight;
					temp.Master = MyEntity.Master;
					temp.ShipmentNumber = MyEntity.ShipmentNumber;
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
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
						DeliveryQueryService DeliveryService20 = new DeliveryQueryService(Tenant);
						temp.ShipmentDeliveries = DeliveryService20.DeliveryDataMappingAndValidatin(MyEntity.Deliveries,Tenant,ComputingPartnerName);
					}

								  

					if(MyEntity.PickUps != null && MyEntity.PickUps.Count > 0)
					{
						PickUpQueryService PickUpService20 = new PickUpQueryService(Tenant);
						temp.ShipmentPickUps = PickUpService20.PickUpDataMappingAndValidatin(MyEntity.PickUps,Tenant,ComputingPartnerName);
					}

								 
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					
					temp.IsOperationalClosed = MyEntity.IsOperationalClosed;
					VesselQueryService VesselVesselService = new VesselQueryService(Tenant);
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
					temp.ValueOfGoods = MyEntity.ValueOfGoods;
					CurrencyQueryService ValueOfGoodsCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.ValueOfGoodsCurrency != null)
					{
						var myValueOfGoodsCurrencyPM = ValueOfGoodsCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.ValueOfGoodsCurrency,Tenant,ComputingPartnerName);
												if(myValueOfGoodsCurrencyPM != null)
						{
							temp.ValueOfGoodsCurrencyId = myValueOfGoodsCurrencyPM.Id;
						}
						 
					}
			
					
					temp.MainCarriageCarrierNumber = MyEntity.MainCarriageCarrierNumber;
					CardQueryService MainCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageCarrier != null)
					{
						var myMainCarriageCarrierPM = MainCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier,Tenant,ComputingPartnerName);
												if(myMainCarriageCarrierPM != null)
						{
							temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
						}
						 
					}
			
					 

					if(MyEntity.Receivables != null && MyEntity.Receivables.Count > 0)
					{
						ReceivableQueryService ReceivableService20 = new ReceivableQueryService(Tenant);
						temp.ShipmentReceivables = ReceivableService20.ReceivableDataMappingAndValidatin(MyEntity.Receivables,Tenant,ComputingPartnerName);
					}

								  

					if(MyEntity.Payables != null && MyEntity.Payables.Count > 0)
					{
						PayableQueryService PayableService20 = new PayableQueryService(Tenant);
						temp.ShipmentPayables = PayableService20.PayableDataMappingAndValidatin(MyEntity.Payables,Tenant,ComputingPartnerName);
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