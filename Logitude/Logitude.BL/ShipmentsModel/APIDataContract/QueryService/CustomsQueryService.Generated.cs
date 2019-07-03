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
   public partial class CustomsQueryService
   {
   
		IShipmentsContext  context;
		//ShipmentService service; 
		
		ShipmentQuery query; 

        public CustomsQueryService(int tenant)
        {
				    context = ShipmentsContext.GetContext(tenant); 
			//service = new ShipmentService(context, tenant); 
			query = new ShipmentQuery(tenant);
        }

		
		public Customs GetCustomsById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Shipment with Id " + Id + " doesn't exist");

				return CustomsDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Customs GetCustomsByShipmentNumber(string ShipmentNumber,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePMByShipmentNumber(ShipmentNumber,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Shipment with ShipmentNumber " + ShipmentNumber + " doesn't exist");

				return CustomsDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Customs CustomsDataMapping(ShipmentPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Customs(); 
				   temp.Id = MyEntityPM.Id;			  
				   if(MyEntityPM.ShipmentTypeId != null)
				   {
					   ShipmentTypeQueryService ShipmentTypeService0 = new ShipmentTypeQueryService(Tenant);
					   					   temp.ShipmentType = ShipmentTypeService0.ShipmentTypeCustomDataMapping(MyEntityPM.ShipmentTypeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService1 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService1.GetTransportModeById(MyEntityPM.TransportModeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ShipperId != null)
				   {
					   CardQueryService CardService2 = new CardQueryService(Tenant);
					   					   temp.Shipper = CardService2.GetCardById(MyEntityPM.ShipperId,Tenant); 
			       
					   				   }
				   
				   temp.ShipperReference1 = MyEntityPM.ShipperReference1;
				   temp.ShipperReference2 = MyEntityPM.ShipperReference2;			  
				   if(MyEntityPM.ConsigneeId != null)
				   {
					   CardQueryService CardService3 = new CardQueryService(Tenant);
					   					   temp.Consignee = CardService3.GetCardById(MyEntityPM.ConsigneeId,Tenant); 
			       
					   				   }
				   
				   temp.ConsigneeReference1 = MyEntityPM.ConsigneeReference1;
				   temp.ConsigneeReference2 = MyEntityPM.ConsigneeReference2;			  
				   if(MyEntityPM.CustomerId != null)
				   {
					   CardQueryService CardService4 = new CardQueryService(Tenant);
					   					   temp.Customer = CardService4.GetCardById(MyEntityPM.CustomerId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.FromPortId != null)
				   {
					   PortQueryService PortService5 = new PortQueryService(Tenant);
					   					   temp.FromPort = PortService5.GetPortById(MyEntityPM.FromPortId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ToPortId != null)
				   {
					   PortQueryService PortService6 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService6.GetPortById(MyEntityPM.ToPortId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.GrossWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService7 = new WeightUnitQueryService(Tenant);
					   					   temp.GrossWeightUnit = WeightUnitService7.GetWeightUnitByCode(MyEntityPM.GrossWeightUnitCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ChargeableWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService8 = new WeightUnitQueryService(Tenant);
					   					   temp.ChargeableWeightUnit = WeightUnitService8.GetWeightUnitByCode(MyEntityPM.ChargeableWeightUnitCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.VolumeUnitCode != null)
				   {
					   VolumeUnitQueryService VolumeUnitService9 = new VolumeUnitQueryService(Tenant);
					   					   temp.VolumeUnit = VolumeUnitService9.GetVolumeUnitByCode(MyEntityPM.VolumeUnitCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.IncotermId != null)
				   {
					   IncotermQueryService IncotermService10 = new IncotermQueryService(Tenant);
					   					   temp.Incoterm = IncotermService10.GetIncotermById(MyEntityPM.IncotermId,Tenant); 
			       
					   				   }
				   
				   temp.HouseNo = MyEntityPM.House;
				   temp.HouseDate = MyEntityPM.HAWBDate;
				   temp.DescriptionOfGoods = MyEntityPM.DescriptionOfGoods;
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 AirPackageQueryService AirPackageService11 = new AirPackageQueryService(Tenant);
					 temp.AirPackages = AirPackageService11.AirPackageCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant);
				}

							 
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 OceanOrInlandPackageQueryService OceanOrInlandPackageService11 = new OceanOrInlandPackageQueryService(Tenant);
					 temp.OceanOrInlandPackages = OceanOrInlandPackageService11.OceanOrInlandPackageCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant);
				}

							 
				if(MyEntityPM.ShipmentPackages != null && MyEntityPM.ShipmentPackages.Count > 0)
				{
					 ContainerQueryService ContainerService11 = new ContainerQueryService(Tenant);
					 temp.Containers = ContainerService11.ContainerCustomDataMapping(MyEntityPM,MyEntityPM.ShipmentPackages,Tenant);
				}

							 
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
				   
				   temp.TEU = MyEntityPM.TEU;
				   temp.NumberOfPackages = MyEntityPM.NumberOfPackages;
				   temp.GrossWeight = MyEntityPM.GrossWeight;
				   temp.Volume = MyEntityPM.Volume;
				   temp.VolumetricWeight = MyEntityPM.VolumetricWeight;
				   temp.ChargeableWeight = MyEntityPM.ChargeableWeight;
				   temp.ShipmentNumber = MyEntityPM.ShipmentNumber;
				   temp.Master = MyEntityPM.Master;
				   temp.CustomsClearanceDate = MyEntityPM.CustomsClearanceDate;
				   temp.DeclarationNumber = MyEntityPM.DeclarationNumber;			  
				   if(MyEntityPM.MainCarriageCarrierId != null)
				   {
					   CardQueryService CardService13 = new CardQueryService(Tenant);
					   					   temp.MainCarriageCarrier = CardService13.GetCardById(MyEntityPM.MainCarriageCarrierId,Tenant); 
			       
					   				   }
				   				
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				temp.CustomFields = customFieldService.CustomFieldCustomDataMapping(MyEntityPM, Tenant);
				 
				   
				   temp.ShipperName = MyEntityPM.ShipperName;
				   temp.DeclarationXMLData = MyEntityPM.DeclarationXMLData;
				   temp.DeclarationDate = MyEntityPM.DeclarationDate;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ShipmentPM CustomsDataMappingAndValidatin(Customs MyEntity,int Tenant,string ComputingPartnerName = "")
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
						temp.Id = MyEntity.Id;
					}					ShipmentTypeQueryService ShipmentTypeShipmentTypeService = new ShipmentTypeQueryService(Tenant);
					if(MyEntity.ShipmentType != null)
					{
						var myShipmentTypePM = ShipmentTypeShipmentTypeService.ShipmentTypeCustomDataMappingAndValidatin(MyEntity.ShipmentType,Tenant);
												if(myShipmentTypePM != null)
						{
							temp.ShipmentTypeId = myShipmentTypePM.Id;
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
					temp.ConsigneeReference2 = MyEntity.ConsigneeReference2;					CardQueryService CustomerCardService = new CardQueryService(Tenant);
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
			
					
					temp.House = MyEntity.HouseNo;
					temp.HAWBDate = MyEntity.HouseDate;
					temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods; 

					if(MyEntity.AirPackages != null && MyEntity.AirPackages.Count > 0)
					{
						AirPackageQueryService AirPackageService15 = new AirPackageQueryService(Tenant);
						temp.ShipmentPackages = AirPackageService15.AirPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.AirPackages,Tenant,ComputingPartnerName);
					}

								  

					if(MyEntity.OceanOrInlandPackages != null && MyEntity.OceanOrInlandPackages.Count > 0)
					{
						OceanOrInlandPackageQueryService OceanOrInlandPackageService15 = new OceanOrInlandPackageQueryService(Tenant);
						temp.ShipmentPackages = OceanOrInlandPackageService15.OceanOrInlandPackageCustomDataMappingAndValidatin(MyEntity,MyEntity.OceanOrInlandPackages,Tenant,ComputingPartnerName);
					}

								  

					if(MyEntity.Containers != null && MyEntity.Containers.Count > 0)
					{
						ContainerQueryService ContainerService15 = new ContainerQueryService(Tenant);
						temp.ShipmentPackages = ContainerService15.ContainerCustomDataMappingAndValidatin(MyEntity,MyEntity.Containers,Tenant,ComputingPartnerName);
					}

								 
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
			
					
					temp.TEU = MyEntity.TEU;
					temp.NumberOfPackages = MyEntity.NumberOfPackages;
					temp.GrossWeight = MyEntity.GrossWeight;
					temp.Volume = MyEntity.Volume;
					temp.VolumetricWeight = MyEntity.VolumetricWeight;
					temp.ChargeableWeight = MyEntity.ChargeableWeight;
					temp.ShipmentNumber = MyEntity.ShipmentNumber;
					temp.Master = MyEntity.Master;
					temp.CustomsClearanceDate = MyEntity.CustomsClearanceDate;
					temp.DeclarationNumber = MyEntity.DeclarationNumber;					CardQueryService MainCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageCarrier != null)
					{
						var myMainCarriageCarrierPM = MainCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier,Tenant,ComputingPartnerName);
												if(myMainCarriageCarrierPM != null)
						{
							temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
						}
						 
					}
			
					
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					
					temp.ShipperName = MyEntity.ShipperName;
					temp.DeclarationXMLData = MyEntity.DeclarationXMLData;
					temp.DeclarationDate = MyEntity.DeclarationDate;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}