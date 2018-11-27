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
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.QuoteModel;

 namespace Logitude.BL.QuoteModel.APIDataContract.ApiV1
{ 
   public partial class QuoteQueryService
   {
   
		IQuotesContext  context;
		//QuoteService service; 
		
		QuoteQuery query; 

        public QuoteQueryService(int tenant)
        {
				    context = QuotesContext.GetContext(tenant); 
			//service = new QuoteService(context, tenant); 
			query = new QuoteQuery(tenant);
        }

		
		public Quote GetQuoteById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Quote with Id " + Id + " doesn't exist");

				return QuoteDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Quote GetQuoteByQuoteNumber(string QuoteNumber,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePMByQuoteNumber(QuoteNumber,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Quote with QuoteNumber " + QuoteNumber + " doesn't exist");

				return QuoteDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Quote QuoteDataMapping(QuotePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Quote(); 
				   temp.Id = MyEntityPM.Id;
				   temp.AcceptedDate = MyEntityPM.AcceptedDate;
				   temp.AgentReference1 = MyEntityPM.AgentReference1;
				   temp.AgentReference2 = MyEntityPM.AgentReference2;
				   temp.ChargeableWeight = MyEntityPM.ChargeableWeight;
				   temp.ConsigneeReference1 = MyEntityPM.ConsigneeReference1;
				   temp.ConsigneeReference2 = MyEntityPM.ConsigneeReference2;
				   temp.CostTotalAmountInLocalCurrency = MyEntityPM.CostTotalAmountInLocalCurrency;
				   temp.CostTotalAmountInSaleCurrency = MyEntityPM.CostTotalAmountInSaleCurrency;
				   temp.CustomerReference1 = MyEntityPM.CustomerReference1;
				   temp.CustomerReference2 = MyEntityPM.CustomerReference2;
				   temp.DeclinedDate = MyEntityPM.DeclinedDate;
				   temp.DeliveryLocation = MyEntityPM.DeliveryLocation;
				   temp.DepartureFrequency = MyEntityPM.DepartureFrequency;
				   temp.DescriptionOfGoods = MyEntityPM.DescriptionOfGoods;
				   temp.DimFactor = MyEntityPM.DimFactor;
				   temp.EstimateProfit = MyEntityPM.EstimateProfit;
				   temp.EstimateProfitInSaleCurrency = MyEntityPM.EstimateProfitInSaleCurrency;
				   temp.ExpirationDate = MyEntityPM.ExpirationDate;
				   temp.GrossWeight = MyEntityPM.GrossWeight;
				   temp.IncludeDelivery = MyEntityPM.IncludeDelivery;
				   temp.IncludePickUp = MyEntityPM.IncludePickUp;
				   temp.LastVersionNumber = MyEntityPM.LastVersionNumber;
				   temp.CreateDate = MyEntityPM.OpenDate;
				   temp.ProductCode = MyEntityPM.ProductCode;
				   temp.SaleTotalAmountInSaleCurrency = MyEntityPM.SaleTotalAmountInSaleCurrency;
				   temp.Notes = MyEntityPM.Notes;
				   temp.UpdateDate = MyEntityPM.UpdateDate;
				   temp.QuoteNumber = MyEntityPM.QuoteNumber;
				   temp.SentDate = MyEntityPM.SentDate;
				   temp.ShipperPickAddressId = MyEntityPM.ShipperPickAddressId;
				   temp.ShipperReference1 = MyEntityPM.ShipperReference1;
				   temp.ShipperReference2 = MyEntityPM.ShipperReference2;
				   temp.StageDueDate = MyEntityPM.StageDueDate;
				   temp.Subject = MyEntityPM.Subject;
				   temp.TEU = MyEntityPM.TEU;
				   temp.TotalContainers = MyEntityPM.TotalContainers;
				   temp.TransitTime = MyEntityPM.TransitTime;
				   temp.ValueOfGoods = MyEntityPM.ValueOfGoods;
				   temp.Volume = MyEntityPM.Volume;			  
				   if(MyEntityPM.AgentContactId != null)
				   {
					   ContactQueryService ContactService0 = new ContactQueryService(Tenant);
					   					   temp.AgentContact = ContactService0.GetContactById(MyEntityPM.AgentContactId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.AgentId != null)
				   {
					   CardQueryService CardService1 = new CardQueryService(Tenant);
					   					   temp.Agent = CardService1.GetCardById(MyEntityPM.AgentId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService2 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService2.GetBranchById(MyEntityPM.BranchId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.DepartmentId != null)
				   {
					   DepartmentQueryService DepartmentService3 = new DepartmentQueryService(Tenant);
					   					   temp.Department = DepartmentService3.GetDepartmentById(MyEntityPM.DepartmentId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ChargeableWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService4 = new WeightUnitQueryService(Tenant);
					   					   temp.ChargeableWeightUnit = WeightUnitService4.GetWeightUnitByCode(MyEntityPM.ChargeableWeightUnitCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ShipperContactId != null)
				   {
					   ContactQueryService ContactService5 = new ContactQueryService(Tenant);
					   					   temp.ShipperContact = ContactService5.GetContactById(MyEntityPM.ShipperContactId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ShipperId != null)
				   {
					   CardQueryService CardService6 = new CardQueryService(Tenant);
					   					   temp.Shipper = CardService6.GetCardById(MyEntityPM.ShipperId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ConsigneeContactId != null)
				   {
					   ContactQueryService ContactService7 = new ContactQueryService(Tenant);
					   					   temp.ConsigneeContact = ContactService7.GetContactById(MyEntityPM.ConsigneeContactId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ConsigneeId != null)
				   {
					   CardQueryService CardService8 = new CardQueryService(Tenant);
					   					   temp.Consignee = CardService8.GetCardById(MyEntityPM.ConsigneeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService9 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService9.GetUserById(MyEntityPM.CreatedByUserId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.UpdatedByUserId != null)
				   {
					   UserQueryService UserService10 = new UserQueryService(Tenant);
					   					   temp.UpdatedByUser = UserService10.GetUserById(MyEntityPM.UpdatedByUserId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.CustomerContactId != null)
				   {
					   ContactQueryService ContactService11 = new ContactQueryService(Tenant);
					   					   temp.CustomerContact = ContactService11.GetContactById(MyEntityPM.CustomerContactId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.CustomerId != null)
				   {
					   CardQueryService CardService12 = new CardQueryService(Tenant);
					   					   temp.Customer = CardService12.GetCardById(MyEntityPM.CustomerId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.DeliveryAddressId != null)
				   {
					   AddressQueryService AddressService13 = new AddressQueryService(Tenant);
					   					   temp.DeliveryAddress = AddressService13.GetAddressById(MyEntityPM.DeliveryAddressId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.DimensionsUnitCode != null)
				   {
					   DimensionsUnitQueryService DimensionsUnitService14 = new DimensionsUnitQueryService(Tenant);
					   					   temp.DimensionsUnit = DimensionsUnitService14.GetDimensionsUnitByCode(MyEntityPM.DimensionsUnitCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.DirectionId != null)
				   {
					   DirectionQueryService DirectionService15 = new DirectionQueryService(Tenant);
					   					   temp.Direction = DirectionService15.GetDirectionById(MyEntityPM.DirectionId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService16 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService16.GetTransportModeById(MyEntityPM.TransportModeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.FromPortId != null)
				   {
					   PortQueryService PortService17 = new PortQueryService(Tenant);
					   					   temp.FromPort = PortService17.GetPortById(MyEntityPM.FromPortId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ToPortId != null)
				   {
					   PortQueryService PortService18 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService18.GetPortById(MyEntityPM.ToPortId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.GrossWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService19 = new WeightUnitQueryService(Tenant);
					   					   temp.GrossWeightUnit = WeightUnitService19.GetWeightUnitByCode(MyEntityPM.GrossWeightUnitCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.IncotermId != null)
				   {
					   IncotermQueryService IncotermService20 = new IncotermQueryService(Tenant);
					   					   temp.Incoterm = IncotermService20.GetIncotermById(MyEntityPM.IncotermId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.MainCarriageCarrierId != null)
				   {
					   CardQueryService CardService21 = new CardQueryService(Tenant);
					   					   temp.MainCarriageCarrier = CardService21.GetCardById(MyEntityPM.MainCarriageCarrierId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.MoveTypeId != null)
				   {
					   MoveTypeQueryService MoveTypeService22 = new MoveTypeQueryService(Tenant);
					   					   temp.MoveType = MoveTypeService22.GetMoveTypeById(MyEntityPM.MoveTypeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.PickUpAddressId != null)
				   {
					   AddressQueryService AddressService23 = new AddressQueryService(Tenant);
					   					   temp.PickUpAddress = AddressService23.GetAddressById(MyEntityPM.PickUpAddressId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.QuoteTypeCode != null)
				   {
					   QuoteTypeQueryService QuoteTypeService24 = new QuoteTypeQueryService(Tenant);
					   					   temp.QuoteType = QuoteTypeService24.GetQuoteTypeByCode(MyEntityPM.QuoteTypeCode,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.SaleCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService25 = new CurrencyQueryService(Tenant);
					   					   temp.SaleCurrency = CurrencyService25.GetCurrencyById(MyEntityPM.SaleCurrencyId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.SalesmanUserId != null)
				   {
					   UserQueryService UserService26 = new UserQueryService(Tenant);
					   					   temp.SalesmanUser = UserService26.GetUserById(MyEntityPM.SalesmanUserId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ShipmentTypeId != null)
				   {
					   ShipmentTypeQueryService ShipmentTypeService27 = new ShipmentTypeQueryService(Tenant);
					   					   temp.ShipmentType = ShipmentTypeService27.ShipmentTypeCustomDataMapping(MyEntityPM.ShipmentTypeId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.StageId != null)
				   {
					   QuoteStageQueryService QuoteStageService28 = new QuoteStageQueryService(Tenant);
					   					   temp.Stage = QuoteStageService28.GetQuoteStageById(MyEntityPM.StageId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.ValueOfGoodsCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService29 = new CurrencyQueryService(Tenant);
					   					   temp.ValueOfGoodsCurrency = CurrencyService29.GetCurrencyById(MyEntityPM.ValueOfGoodsCurrencyId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.VolumeUnitCode != null)
				   {
					   VolumeUnitQueryService VolumeUnitService30 = new VolumeUnitQueryService(Tenant);
					   					   temp.VolumeUnit = VolumeUnitService30.GetVolumeUnitByCode(MyEntityPM.VolumeUnitCode,Tenant); 
			       
					   				   }
				   
				if(MyEntityPM.QuoteCharges != null && MyEntityPM.QuoteCharges.Count > 0)
				{
					 QuoteChargeQueryService QuoteChargeService31 = new QuoteChargeQueryService(Tenant);
					 temp.QuoteCharges = QuoteChargeService31.QuoteChargeDataMapping(MyEntityPM.QuoteCharges,Tenant);
				}

							 			  
				   if(MyEntityPM.PackageType1Id != null)
				   {
					   PackageTypeQueryService PackageTypeService31 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType1 = PackageTypeService31.GetPackageTypeById(MyEntityPM.PackageType1Id,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.PackageType2Id != null)
				   {
					   PackageTypeQueryService PackageTypeService32 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType2 = PackageTypeService32.GetPackageTypeById(MyEntityPM.PackageType2Id,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.PackageType3Id != null)
				   {
					   PackageTypeQueryService PackageTypeService33 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType3 = PackageTypeService33.GetPackageTypeById(MyEntityPM.PackageType3Id,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.PackageType4Id != null)
				   {
					   PackageTypeQueryService PackageTypeService34 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType4 = PackageTypeService34.GetPackageTypeById(MyEntityPM.PackageType4Id,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.PackageType5Id != null)
				   {
					   PackageTypeQueryService PackageTypeService35 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType5 = PackageTypeService35.GetPackageTypeById(MyEntityPM.PackageType5Id,Tenant); 
			       
					   				   }
				   
				   temp.PackageType1Quantity = MyEntityPM.PackageType1Quantity;
				   temp.PackageType2Quantity = MyEntityPM.PackageType2Quantity;
				   temp.PackageType3Quantity = MyEntityPM.PackageType3Quantity;
				   temp.PackageType4Quantity = MyEntityPM.PackageType4Quantity;
				   temp.PackageType5Quantity = MyEntityPM.PackageType5Quantity;
				   temp.PackagesQuantity = MyEntityPM.NumberOfPackages;
				   temp.VolumetricWeight = MyEntityPM.VolumetricWeight;
				if(MyEntityPM.QuotePackages != null && MyEntityPM.QuotePackages.Count > 0)
				{
					 QuotePackageQueryService QuotePackageService36 = new QuotePackageQueryService(Tenant);
					 temp.QuotePackages = QuotePackageService36.QuotePackageDataMapping(MyEntityPM.QuotePackages,Tenant);
				}

							 
				   temp.StageDate = MyEntityPM.LastStageDate;
				   temp.SameOrFixed = MyEntityPM.SameOrFixed;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public QuotePM QuoteDataMappingAndValidatin(Quote MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new QuotePM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.QuoteNumber))
					{
						temp = query.GetSinglePMByQuoteNumber(MyEntity.QuoteNumber, Tenant);
					} 					   
					if(temp == null)
					{
					    throw new ApplicationException("Quote with QuoteNumber " + MyEntity.QuoteNumber + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.AcceptedDate = MyEntity.AcceptedDate;
					temp.AgentReference1 = MyEntity.AgentReference1;
					temp.AgentReference2 = MyEntity.AgentReference2;
					temp.ChargeableWeight = MyEntity.ChargeableWeight;
					temp.ConsigneeReference1 = MyEntity.ConsigneeReference1;
					temp.ConsigneeReference2 = MyEntity.ConsigneeReference2;
					temp.CostTotalAmountInLocalCurrency = MyEntity.CostTotalAmountInLocalCurrency;
					temp.CostTotalAmountInSaleCurrency = MyEntity.CostTotalAmountInSaleCurrency;
					temp.CustomerReference1 = MyEntity.CustomerReference1;
					temp.CustomerReference2 = MyEntity.CustomerReference2;
					temp.DeclinedDate = MyEntity.DeclinedDate;
					temp.DeliveryLocation = MyEntity.DeliveryLocation;
					temp.DepartureFrequency = MyEntity.DepartureFrequency;
					temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;
					temp.DimFactor = MyEntity.DimFactor;
					temp.EstimateProfit = MyEntity.EstimateProfit;
					temp.EstimateProfitInSaleCurrency = MyEntity.EstimateProfitInSaleCurrency;
					temp.ExpirationDate = MyEntity.ExpirationDate;
					temp.GrossWeight = MyEntity.GrossWeight;
					temp.IncludeDelivery = MyEntity.IncludeDelivery;
					temp.IncludePickUp = MyEntity.IncludePickUp;
					temp.LastVersionNumber = MyEntity.LastVersionNumber;
					temp.OpenDate = MyEntity.CreateDate;
					temp.ProductCode = MyEntity.ProductCode;
					temp.SaleTotalAmountInSaleCurrency = MyEntity.SaleTotalAmountInSaleCurrency;
					temp.Notes = MyEntity.Notes;
					temp.UpdateDate = MyEntity.UpdateDate;
					temp.QuoteNumber = MyEntity.QuoteNumber;
					temp.SentDate = MyEntity.SentDate;
					temp.ShipperPickAddressId = MyEntity.ShipperPickAddressId;
					temp.ShipperReference1 = MyEntity.ShipperReference1;
					temp.ShipperReference2 = MyEntity.ShipperReference2;
					temp.StageDueDate = MyEntity.StageDueDate;
					temp.Subject = MyEntity.Subject;
					temp.TEU = MyEntity.TEU;
					temp.TotalContainers = MyEntity.TotalContainers;
					temp.TransitTime = MyEntity.TransitTime;
					temp.ValueOfGoods = MyEntity.ValueOfGoods;
					temp.Volume = MyEntity.Volume;					ContactQueryService AgentContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.AgentContact != null)
					{
						var myAgentContactPM = AgentContactContactService.ContactDataMappingAndValidatin(MyEntity.AgentContact,Tenant,ComputingPartnerName);
												if(myAgentContactPM != null)
						{
							temp.AgentContactId = myAgentContactPM.Id;
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
			
										WeightUnitQueryService ChargeableWeightUnitWeightUnitService = new WeightUnitQueryService(Tenant);
					if(MyEntity.ChargeableWeightUnit != null)
					{
						var myChargeableWeightUnitPM = ChargeableWeightUnitWeightUnitService.WeightUnitDataMappingAndValidatin(MyEntity.ChargeableWeightUnit,Tenant,ComputingPartnerName);
												if(myChargeableWeightUnitPM != null)
						{
							temp.ChargeableWeightUnitCode = myChargeableWeightUnitPM.Code;
						}
						 
					}
			
										ContactQueryService ShipperContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.ShipperContact != null)
					{
						var myShipperContactPM = ShipperContactContactService.ContactDataMappingAndValidatin(MyEntity.ShipperContact,Tenant,ComputingPartnerName);
												if(myShipperContactPM != null)
						{
							temp.ShipperContactId = myShipperContactPM.Id;
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
			
										ContactQueryService ConsigneeContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.ConsigneeContact != null)
					{
						var myConsigneeContactPM = ConsigneeContactContactService.ContactDataMappingAndValidatin(MyEntity.ConsigneeContact,Tenant,ComputingPartnerName);
												if(myConsigneeContactPM != null)
						{
							temp.ConsigneeContactId = myConsigneeContactPM.Id;
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
			
										UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName);
												if(myCreatedByUserPM != null)
						{
							temp.CreatedByUserId = myCreatedByUserPM.Id;
						}
						 
					}
			
										UserQueryService UpdatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.UpdatedByUser != null)
					{
						var myUpdatedByUserPM = UpdatedByUserUserService.UserDataMappingAndValidatin(MyEntity.UpdatedByUser,Tenant,ComputingPartnerName);
												if(myUpdatedByUserPM != null)
						{
							temp.UpdatedByUserId = myUpdatedByUserPM.Id;
						}
						 
					}
			
										ContactQueryService CustomerContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.CustomerContact != null)
					{
						var myCustomerContactPM = CustomerContactContactService.ContactDataMappingAndValidatin(MyEntity.CustomerContact,Tenant,ComputingPartnerName);
												if(myCustomerContactPM != null)
						{
							temp.CustomerContactId = myCustomerContactPM.Id;
						}
						 
					}
			
										CardQueryService CustomerCardService = new CardQueryService(Tenant);
					if(MyEntity.Customer != null)
					{
						var myCustomerPM = CustomerCardService.CardDataMappingAndValidatin(MyEntity.Customer,Tenant,ComputingPartnerName);
												if(myCustomerPM != null)
						{
							temp.CustomerId = myCustomerPM.Id;
						}
						 
					}
			
										AddressQueryService DeliveryAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.DeliveryAddress != null)
					{
						var myDeliveryAddressPM = DeliveryAddressAddressService.AddressDataMappingAndValidatin(MyEntity.DeliveryAddress,Tenant,ComputingPartnerName);
												if(myDeliveryAddressPM != null)
						{
							temp.DeliveryAddressId = myDeliveryAddressPM.Id;
						}
						 
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
			
										IncotermQueryService IncotermIncotermService = new IncotermQueryService(Tenant);
					if(MyEntity.Incoterm != null)
					{
						var myIncotermPM = IncotermIncotermService.IncotermDataMappingAndValidatin(MyEntity.Incoterm,Tenant,ComputingPartnerName);
												if(myIncotermPM != null)
						{
							temp.IncotermId = myIncotermPM.Id;
						}
						 
					}
			
										CardQueryService MainCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageCarrier != null)
					{
						var myMainCarriageCarrierPM = MainCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier,Tenant,ComputingPartnerName);
												if(myMainCarriageCarrierPM != null)
						{
							temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
						}
						 
					}
			
										MoveTypeQueryService MoveTypeMoveTypeService = new MoveTypeQueryService(Tenant);
					if(MyEntity.MoveType != null)
					{
						var myMoveTypePM = MoveTypeMoveTypeService.MoveTypeDataMappingAndValidatin(MyEntity.MoveType,Tenant,ComputingPartnerName);
												if(myMoveTypePM != null)
						{
							temp.MoveTypeId = myMoveTypePM.Id;
						}
						 
					}
			
										AddressQueryService PickUpAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.PickUpAddress != null)
					{
						var myPickUpAddressPM = PickUpAddressAddressService.AddressDataMappingAndValidatin(MyEntity.PickUpAddress,Tenant,ComputingPartnerName);
												if(myPickUpAddressPM != null)
						{
							temp.PickUpAddressId = myPickUpAddressPM.Id;
						}
						 
					}
			
										QuoteTypeQueryService QuoteTypeQuoteTypeService = new QuoteTypeQueryService(Tenant);
					if(MyEntity.QuoteType != null)
					{
						var myQuoteTypePM = QuoteTypeQuoteTypeService.QuoteTypeDataMappingAndValidatin(MyEntity.QuoteType,Tenant,ComputingPartnerName);
												if(myQuoteTypePM != null)
						{
							temp.QuoteTypeCode = myQuoteTypePM.Code;
						}
						 
					}
			
										CurrencyQueryService SaleCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.SaleCurrency != null)
					{
						var mySaleCurrencyPM = SaleCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.SaleCurrency,Tenant,ComputingPartnerName);
												if(mySaleCurrencyPM != null)
						{
							temp.SaleCurrencyId = mySaleCurrencyPM.Id;
						}
						 
					}
			
										UserQueryService SalesmanUserUserService = new UserQueryService(Tenant);
					if(MyEntity.SalesmanUser != null)
					{
						var mySalesmanUserPM = SalesmanUserUserService.UserDataMappingAndValidatin(MyEntity.SalesmanUser,Tenant,ComputingPartnerName);
												if(mySalesmanUserPM != null)
						{
							temp.SalesmanUserId = mySalesmanUserPM.Id;
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
			
										QuoteStageQueryService StageQuoteStageService = new QuoteStageQueryService(Tenant);
					if(MyEntity.Stage != null)
					{
						var myStagePM = StageQuoteStageService.QuoteStageDataMappingAndValidatin(MyEntity.Stage,Tenant,ComputingPartnerName);
												if(myStagePM != null)
						{
							temp.StageId = myStagePM.Id;
						}
						 
					}
			
										CurrencyQueryService ValueOfGoodsCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.ValueOfGoodsCurrency != null)
					{
						var myValueOfGoodsCurrencyPM = ValueOfGoodsCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.ValueOfGoodsCurrency,Tenant,ComputingPartnerName);
												if(myValueOfGoodsCurrencyPM != null)
						{
							temp.ValueOfGoodsCurrencyId = myValueOfGoodsCurrencyPM.Id;
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
			
					
					if(MyEntity.QuoteCharges != null && MyEntity.QuoteCharges.Count > 0)
					{
						QuoteChargeQueryService QuoteChargeService36 = new QuoteChargeQueryService(Tenant);
						temp.QuoteCharges = QuoteChargeService36.QuoteChargeDataMappingAndValidatin(MyEntity.QuoteCharges,Tenant,ComputingPartnerName);
					}

								 					PackageTypeQueryService PackageType1PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType1 != null)
					{
						var myPackageType1PM = PackageType1PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType1,Tenant,ComputingPartnerName);
												if(myPackageType1PM != null)
						{
							temp.PackageType1Id = myPackageType1PM.Id;
						}
						 
					}
			
										PackageTypeQueryService PackageType2PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType2 != null)
					{
						var myPackageType2PM = PackageType2PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType2,Tenant,ComputingPartnerName);
												if(myPackageType2PM != null)
						{
							temp.PackageType2Id = myPackageType2PM.Id;
						}
						 
					}
			
										PackageTypeQueryService PackageType3PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType3 != null)
					{
						var myPackageType3PM = PackageType3PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType3,Tenant,ComputingPartnerName);
												if(myPackageType3PM != null)
						{
							temp.PackageType3Id = myPackageType3PM.Id;
						}
						 
					}
			
										PackageTypeQueryService PackageType4PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType4 != null)
					{
						var myPackageType4PM = PackageType4PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType4,Tenant,ComputingPartnerName);
												if(myPackageType4PM != null)
						{
							temp.PackageType4Id = myPackageType4PM.Id;
						}
						 
					}
			
										PackageTypeQueryService PackageType5PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType5 != null)
					{
						var myPackageType5PM = PackageType5PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType5,Tenant,ComputingPartnerName);
												if(myPackageType5PM != null)
						{
							temp.PackageType5Id = myPackageType5PM.Id;
						}
						 
					}
			
					
					temp.PackageType1Quantity = MyEntity.PackageType1Quantity;
					temp.PackageType2Quantity = MyEntity.PackageType2Quantity;
					temp.PackageType3Quantity = MyEntity.PackageType3Quantity;
					temp.PackageType4Quantity = MyEntity.PackageType4Quantity;
					temp.PackageType5Quantity = MyEntity.PackageType5Quantity;
					temp.NumberOfPackages = MyEntity.PackagesQuantity;
					temp.VolumetricWeight = MyEntity.VolumetricWeight;
					if(MyEntity.QuotePackages != null && MyEntity.QuotePackages.Count > 0)
					{
						QuotePackageQueryService QuotePackageService36 = new QuotePackageQueryService(Tenant);
						temp.QuotePackages = QuotePackageService36.QuotePackageDataMappingAndValidatin(MyEntity.QuotePackages,Tenant,ComputingPartnerName);
					}

								 
					temp.LastStageDate = MyEntity.StageDate;
					temp.SameOrFixed = MyEntity.SameOrFixed;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}