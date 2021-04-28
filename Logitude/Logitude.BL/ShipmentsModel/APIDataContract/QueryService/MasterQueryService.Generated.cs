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

		
		public Master GetMasterById(string Id,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Shipment with Id " + Id + " doesn't exist");

				return MasterDataMapping(temp,Tenant,ComputingPartnerName);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Master GetMasterByShipmentNumber(string ShipmentNumber,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePMByShipmentNumber(ShipmentNumber,Tenant);				
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
			
					
                    							//throw new ApplicationException("AgentReference1 Can't be update"); 
							temp.AgentReference1 = MyEntity.AgentReference1;

					 

					
                    							//throw new ApplicationException("AgentReference2 Can't be update"); 
							temp.AgentReference2 = MyEntity.AgentReference2;

					 

					
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
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.DescriptionOfGoods))
					{							//throw new ApplicationException("DescriptionOfGoods Can't be update"); 
							temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;

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
			
					
                    							//throw new ApplicationException("MasterNumber Can't be update"); 
							temp.Master = MyEntity.MasterNumber;

					 

					
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
			
					
                    							//throw new ApplicationException("MainCarriageCarrierNumber Can't be update"); 
							temp.MainCarriageCarrierNumber = MyEntity.MainCarriageCarrierNumber;

					 

					
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
						DeliveryQueryService DeliveryService26 = new DeliveryQueryService(Tenant);
						 								//throw new ApplicationException("Deliveries Can't be update"); 
								temp.ShipmentDeliveries = DeliveryService26.DeliveryDataMappingAndValidatin(MyEntity.Deliveries,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								  

					if(MyEntity.PickUps != null && MyEntity.PickUps.Count > 0)
					{
						PickUpQueryService PickUpService26 = new PickUpQueryService(Tenant);
						 								//throw new ApplicationException("PickUps Can't be update"); 
								temp.ShipmentPickUps = PickUpService26.PickUpDataMappingAndValidatin(MyEntity.PickUps,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Shipment");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					 

					if(MyEntity.Houses != null && MyEntity.Houses.Count > 0)
					{
						HouseQueryService HouseService26 = new HouseQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("Houses Can't be update"); 
								temp.ShipmentConsoleShipments = HouseService26.HouseCustomDataMappingAndValidatin(MyEntity,MyEntity.Houses,Tenant,ComputingPartnerName);

					 
						}  

						
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

					 

					if(MyEntity.Receivables != null && MyEntity.Receivables.Count > 0)
					{
						ReceivableQueryService ReceivableService26 = new ReceivableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("Receivables Can't be update"); 
								temp.ShipmentReceivables = ReceivableService26.ReceivableDataMappingAndValidatin(MyEntity.Receivables,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								  

					if(MyEntity.Payables != null && MyEntity.Payables.Count > 0)
					{
						PayableQueryService PayableService26 = new PayableQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("Payables Can't be update"); 
								temp.ShipmentPayables = PayableService26.PayableDataMappingAndValidatin(MyEntity.Payables,Tenant,ComputingPartnerName,IsUpdate);

					 
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
						MainCarriageLegQueryService MainCarriageLegService26 = new MainCarriageLegQueryService(Tenant);
						 								//throw new ApplicationException("MainCarriageLegs Can't be update"); 
								temp.MainCarriageLegs = MainCarriageLegService26.MainCarriageLegDataMappingAndValidatin(MyEntity.MainCarriageLegs,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ShipmentNumber))
					{							//throw new ApplicationException("ShipmentNumber Can't be update"); 
							temp.ShipmentNumber = MyEntity.ShipmentNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ConcurrencyGUID))
					{							//throw new ApplicationException("ConcurrencyGUID Can't be update"); 
							temp.ConcurrencyGUID = MyEntity.ConcurrencyGUID;

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

										   
					return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}