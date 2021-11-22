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

		
		public Quote GetQuoteById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Quote with Id " + Id + " doesn't exist");

				return QuoteDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Quote GetQuoteByQuoteNumber(string QuoteNumber,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePMByQuoteNumber(QuoteNumber, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Quote with QuoteNumber " + QuoteNumber + " doesn't exist");

				return QuoteDataMapping(temp,Tenant,ComputingPartnerName);
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
				   temp.StartDate = MyEntityPM.StartDate;
				   temp.Volume = MyEntityPM.Volume; 

			  
				   if(MyEntityPM.AgentContactId != null)
				   {
					   ContactQueryService ContactService0 = new ContactQueryService(Tenant);
					   					   temp.AgentContact = ContactService0.GetContactById(MyEntityPM.AgentContactId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.AgentId != null)
				   {
					   CardQueryService CardService1 = new CardQueryService(Tenant);
					   					   temp.Agent = CardService1.GetCardById(MyEntityPM.AgentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.BranchId != null)
				   {
					   BranchQueryService BranchService2 = new BranchQueryService(Tenant);
					   					   temp.Branch = BranchService2.GetBranchById(MyEntityPM.BranchId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DepartmentId != null)
				   {
					   DepartmentQueryService DepartmentService3 = new DepartmentQueryService(Tenant);
					   					   temp.Department = DepartmentService3.GetDepartmentById(MyEntityPM.DepartmentId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ChargeableWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService4 = new WeightUnitQueryService(Tenant);
					   					   temp.ChargeableWeightUnit = WeightUnitService4.GetWeightUnitByCode(MyEntityPM.ChargeableWeightUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipperContactId != null)
				   {
					   ContactQueryService ContactService5 = new ContactQueryService(Tenant);
					   					   temp.ShipperContact = ContactService5.GetContactById(MyEntityPM.ShipperContactId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipperId != null)
				   {
					   CardQueryService CardService6 = new CardQueryService(Tenant);
					   					   temp.Shipper = CardService6.GetCardById(MyEntityPM.ShipperId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ConsigneeContactId != null)
				   {
					   ContactQueryService ContactService7 = new ContactQueryService(Tenant);
					   					   temp.ConsigneeContact = ContactService7.GetContactById(MyEntityPM.ConsigneeContactId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ConsigneeId != null)
				   {
					   CardQueryService CardService8 = new CardQueryService(Tenant);
					   					   temp.Consignee = CardService8.GetCardById(MyEntityPM.ConsigneeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService9 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService9.GetUserById(MyEntityPM.CreatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.UpdatedByUserId != null)
				   {
					   UserQueryService UserService10 = new UserQueryService(Tenant);
					   					   temp.UpdatedByUser = UserService10.GetUserById(MyEntityPM.UpdatedByUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.CustomerContactId != null)
				   {
					   ContactQueryService ContactService11 = new ContactQueryService(Tenant);
					   					   temp.CustomerContact = ContactService11.GetContactById(MyEntityPM.CustomerContactId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.CustomerId != null)
				   {
					   CardQueryService CardService12 = new CardQueryService(Tenant);
					   					   temp.Customer = CardService12.GetCardById(MyEntityPM.CustomerId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DeliveryAddressId != null)
				   {
					   AddressQueryService AddressService13 = new AddressQueryService(Tenant);
					   					   temp.DeliveryAddress = AddressService13.GetAddressById(MyEntityPM.DeliveryAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DimensionsUnitCode != null)
				   {
					   DimensionsUnitQueryService DimensionsUnitService14 = new DimensionsUnitQueryService(Tenant);
					   					   temp.DimensionsUnit = DimensionsUnitService14.GetDimensionsUnitByCode(MyEntityPM.DimensionsUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.DirectionId != null)
				   {
					   DirectionQueryService DirectionService15 = new DirectionQueryService(Tenant);
					   					   temp.Direction = DirectionService15.GetDirectionById(MyEntityPM.DirectionId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.TransportModeId != null)
				   {
					   TransportModeQueryService TransportModeService16 = new TransportModeQueryService(Tenant);
					   					   temp.TransportMode = TransportModeService16.GetTransportModeById(MyEntityPM.TransportModeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.FromPortId != null)
				   {
					   PortQueryService PortService17 = new PortQueryService(Tenant);
					   					   temp.FromPort = PortService17.GetPortById(MyEntityPM.FromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ToPortId != null)
				   {
					   PortQueryService PortService18 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService18.GetPortById(MyEntityPM.ToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.GrossWeightUnitCode != null)
				   {
					   WeightUnitQueryService WeightUnitService19 = new WeightUnitQueryService(Tenant);
					   					   temp.GrossWeightUnit = WeightUnitService19.GetWeightUnitByCode(MyEntityPM.GrossWeightUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.IncotermId != null)
				   {
					   IncotermQueryService IncotermService20 = new IncotermQueryService(Tenant);
					   					   temp.Incoterm = IncotermService20.GetIncotermById(MyEntityPM.IncotermId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.MainCarriageCarrierId != null)
				   {
					   CardQueryService CardService21 = new CardQueryService(Tenant);
					   					   temp.MainCarriageCarrier = CardService21.GetCardById(MyEntityPM.MainCarriageCarrierId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.MoveTypeId != null)
				   {
					   MoveTypeQueryService MoveTypeService22 = new MoveTypeQueryService(Tenant);
					   					   temp.MoveType = MoveTypeService22.GetMoveTypeById(MyEntityPM.MoveTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PickUpAddressId != null)
				   {
					   AddressQueryService AddressService23 = new AddressQueryService(Tenant);
					   					   temp.PickUpAddress = AddressService23.GetAddressById(MyEntityPM.PickUpAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.QuoteTypeCode != null)
				   {
					   QuoteTypeQueryService QuoteTypeService24 = new QuoteTypeQueryService(Tenant);
					   					   temp.QuoteType = QuoteTypeService24.GetQuoteTypeByCode(MyEntityPM.QuoteTypeCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.SaleCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService25 = new CurrencyQueryService(Tenant);
					   					   temp.SaleCurrency = CurrencyService25.GetCurrencyById(MyEntityPM.SaleCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.SalesmanUserId != null)
				   {
					   UserQueryService UserService26 = new UserQueryService(Tenant);
					   					   temp.SalesmanUser = UserService26.GetUserById(MyEntityPM.SalesmanUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ShipmentTypeId != null)
				   {
					   ShipmentTypeQueryService ShipmentTypeService27 = new ShipmentTypeQueryService(Tenant);
					   					   temp.ShipmentType = ShipmentTypeService27.ShipmentTypeCustomDataMapping(MyEntityPM.ShipmentTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.StageId != null)
				   {
					   QuoteStageQueryService QuoteStageService28 = new QuoteStageQueryService(Tenant);
					   					   temp.Stage = QuoteStageService28.GetQuoteStageById(MyEntityPM.StageId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.ValueOfGoodsCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService29 = new CurrencyQueryService(Tenant);
					   					   temp.ValueOfGoodsCurrency = CurrencyService29.GetCurrencyById(MyEntityPM.ValueOfGoodsCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.VolumeUnitCode != null)
				   {
					   VolumeUnitQueryService VolumeUnitService30 = new VolumeUnitQueryService(Tenant);
					   					   temp.VolumeUnit = VolumeUnitService30.GetVolumeUnitByCode(MyEntityPM.VolumeUnitCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				if(MyEntityPM.QuoteCharges != null && MyEntityPM.QuoteCharges.Count > 0)
				{
					 QuoteChargeQueryService QuoteChargeService31 = new QuoteChargeQueryService(Tenant);
					 temp.QuoteCharges = QuoteChargeService31.QuoteChargeDataMapping(MyEntityPM.QuoteCharges,Tenant,ComputingPartnerName);
				}

							  

			  
				   if(MyEntityPM.PackageType1Id != null)
				   {
					   PackageTypeQueryService PackageTypeService31 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType1 = PackageTypeService31.GetPackageTypeById(MyEntityPM.PackageType1Id,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PackageType2Id != null)
				   {
					   PackageTypeQueryService PackageTypeService32 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType2 = PackageTypeService32.GetPackageTypeById(MyEntityPM.PackageType2Id,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PackageType3Id != null)
				   {
					   PackageTypeQueryService PackageTypeService33 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType3 = PackageTypeService33.GetPackageTypeById(MyEntityPM.PackageType3Id,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PackageType4Id != null)
				   {
					   PackageTypeQueryService PackageTypeService34 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType4 = PackageTypeService34.GetPackageTypeById(MyEntityPM.PackageType4Id,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PackageType5Id != null)
				   {
					   PackageTypeQueryService PackageTypeService35 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType5 = PackageTypeService35.GetPackageTypeById(MyEntityPM.PackageType5Id,Tenant,ComputingPartnerName); 
			       
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
					 temp.QuotePackages = QuotePackageService36.QuotePackageDataMapping(MyEntityPM.QuotePackages,Tenant,ComputingPartnerName);
				}

							 
				   temp.StageDate = MyEntityPM.LastStageDate;
				   temp.SameOrFixed = MyEntityPM.SameOrFixed;
				   temp.PickupCity = MyEntityPM.PickupCity; 

			  
				   if(MyEntityPM.PickupCountryId != null)
				   {
					   CountryQueryService CountryService36 = new CountryQueryService(Tenant);
					   					   temp.PickupCountry = CountryService36.GetCountryById(MyEntityPM.PickupCountryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.PickupZipCode = MyEntityPM.PickupZipCode;
				   temp.DeliveryCity = MyEntityPM.DeliveryCity; 

			  
				   if(MyEntityPM.DeliveryCountryId != null)
				   {
					   CountryQueryService CountryService37 = new CountryQueryService(Tenant);
					   					   temp.DeliveryCountry = CountryService37.GetCountryById(MyEntityPM.DeliveryCountryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.DeliveryZipCode = MyEntityPM.DeliveryZipCode;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public QuotePM QuoteDataMappingAndValidatin(Quote MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Quote with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
                    
					if(!IsUpdate)// && MyEntity.AcceptedDate != null)
					{							//throw new ApplicationException("AcceptedDate Can't be update"); 
							temp.AcceptedDate = MyEntity.AcceptedDate;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.AgentReference1))
					{							//throw new ApplicationException("AgentReference1 Can't be update"); 
							temp.AgentReference1 = MyEntity.AgentReference1;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.AgentReference2))
					{							//throw new ApplicationException("AgentReference2 Can't be update"); 
							temp.AgentReference2 = MyEntity.AgentReference2;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ChargeableWeight != null)
					{							//throw new ApplicationException("ChargeableWeight Can't be update"); 
							temp.ChargeableWeight = MyEntity.ChargeableWeight;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ConsigneeReference1))
					{							//throw new ApplicationException("ConsigneeReference1 Can't be update"); 
							temp.ConsigneeReference1 = MyEntity.ConsigneeReference1;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ConsigneeReference2))
					{							//throw new ApplicationException("ConsigneeReference2 Can't be update"); 
							temp.ConsigneeReference2 = MyEntity.ConsigneeReference2;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.CostTotalAmountInLocalCurrency != null)
					{							//throw new ApplicationException("CostTotalAmountInLocalCurrency Can't be update"); 
							temp.CostTotalAmountInLocalCurrency = MyEntity.CostTotalAmountInLocalCurrency;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.CostTotalAmountInSaleCurrency != null)
					{							//throw new ApplicationException("CostTotalAmountInSaleCurrency Can't be update"); 
							temp.CostTotalAmountInSaleCurrency = MyEntity.CostTotalAmountInSaleCurrency;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.CustomerReference1))
					{							//throw new ApplicationException("CustomerReference1 Can't be update"); 
							temp.CustomerReference1 = MyEntity.CustomerReference1;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.CustomerReference2))
					{							//throw new ApplicationException("CustomerReference2 Can't be update"); 
							temp.CustomerReference2 = MyEntity.CustomerReference2;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.DeclinedDate != null)
					{							//throw new ApplicationException("DeclinedDate Can't be update"); 
							temp.DeclinedDate = MyEntity.DeclinedDate;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.DeliveryLocation))
					{							//throw new ApplicationException("DeliveryLocation Can't be update"); 
							temp.DeliveryLocation = MyEntity.DeliveryLocation;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.DepartureFrequency))
					{							//throw new ApplicationException("DepartureFrequency Can't be update"); 
							temp.DepartureFrequency = MyEntity.DepartureFrequency;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.DescriptionOfGoods))
					{							//throw new ApplicationException("DescriptionOfGoods Can't be update"); 
							temp.DescriptionOfGoods = MyEntity.DescriptionOfGoods;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.DimFactor != null)
					{							//throw new ApplicationException("DimFactor Can't be update"); 
							temp.DimFactor = MyEntity.DimFactor;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.EstimateProfit != null)
					{							//throw new ApplicationException("EstimateProfit Can't be update"); 
							temp.EstimateProfit = MyEntity.EstimateProfit;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.EstimateProfitInSaleCurrency != null)
					{							//throw new ApplicationException("EstimateProfitInSaleCurrency Can't be update"); 
							temp.EstimateProfitInSaleCurrency = MyEntity.EstimateProfitInSaleCurrency;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ExpirationDate != null)
					{							//throw new ApplicationException("ExpirationDate Can't be update"); 
							temp.ExpirationDate = MyEntity.ExpirationDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.GrossWeight != null)
					{							//throw new ApplicationException("GrossWeight Can't be update"); 
							temp.GrossWeight = MyEntity.GrossWeight;

										}  

					
                    
					if(!IsUpdate)// && (MyEntity.IncludeDelivery != temp.IncludeDelivery))
					{							//throw new ApplicationException("IncludeDelivery Can't be update"); 
							temp.IncludeDelivery = MyEntity.IncludeDelivery;

										}  

					
                    
					if(!IsUpdate)// && (MyEntity.IncludePickUp != temp.IncludePickUp))
					{							//throw new ApplicationException("IncludePickUp Can't be update"); 
							temp.IncludePickUp = MyEntity.IncludePickUp;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.LastVersionNumber != null)
					{							//throw new ApplicationException("LastVersionNumber Can't be update"); 
							temp.LastVersionNumber = MyEntity.LastVersionNumber;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.CreateDate != null)
					{							//throw new ApplicationException("CreateDate Can't be update"); 
							temp.OpenDate = MyEntity.CreateDate;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ProductCode))
					{							//throw new ApplicationException("ProductCode Can't be update"); 
							temp.ProductCode = MyEntity.ProductCode;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.SaleTotalAmountInSaleCurrency != null)
					{							//throw new ApplicationException("SaleTotalAmountInSaleCurrency Can't be update"); 
							temp.SaleTotalAmountInSaleCurrency = MyEntity.SaleTotalAmountInSaleCurrency;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Notes))
					{							//throw new ApplicationException("Notes Can't be update"); 
							temp.Notes = MyEntity.Notes;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.UpdateDate != null)
					{							//throw new ApplicationException("UpdateDate Can't be update"); 
							temp.UpdateDate = MyEntity.UpdateDate;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.QuoteNumber))
					{							//throw new ApplicationException("QuoteNumber Can't be update"); 
							temp.QuoteNumber = MyEntity.QuoteNumber;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.SentDate != null)
					{							//throw new ApplicationException("SentDate Can't be update"); 
							temp.SentDate = MyEntity.SentDate;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ShipperPickAddressId))
					{							//throw new ApplicationException("ShipperPickAddressId Can't be update"); 
							temp.ShipperPickAddressId = MyEntity.ShipperPickAddressId;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ShipperReference1))
					{							//throw new ApplicationException("ShipperReference1 Can't be update"); 
							temp.ShipperReference1 = MyEntity.ShipperReference1;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ShipperReference2))
					{							//throw new ApplicationException("ShipperReference2 Can't be update"); 
							temp.ShipperReference2 = MyEntity.ShipperReference2;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.StageDueDate != null)
					{							//throw new ApplicationException("StageDueDate Can't be update"); 
							temp.StageDueDate = MyEntity.StageDueDate;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Subject))
					{							//throw new ApplicationException("Subject Can't be update"); 
							temp.Subject = MyEntity.Subject;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.TEU != null)
					{							//throw new ApplicationException("TEU Can't be update"); 
							temp.TEU = MyEntity.TEU;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.TotalContainers))
					{							//throw new ApplicationException("TotalContainers Can't be update"); 
							temp.TotalContainers = MyEntity.TotalContainers;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.TransitTime))
					{							//throw new ApplicationException("TransitTime Can't be update"); 
							temp.TransitTime = MyEntity.TransitTime;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.ValueOfGoods != null)
					{							//throw new ApplicationException("ValueOfGoods Can't be update"); 
							temp.ValueOfGoods = MyEntity.ValueOfGoods;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.StartDate != null)
					{							//throw new ApplicationException("StartDate Can't be update"); 
							temp.StartDate = MyEntity.StartDate;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.Volume != null)
					{							//throw new ApplicationException("Volume Can't be update"); 
							temp.Volume = MyEntity.Volume;

										}  

					
					ContactQueryService AgentContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.AgentContact != null)
					{
						var myAgentContactPM = AgentContactContactService.ContactDataMappingAndValidatin(MyEntity.AgentContact,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAgentContactPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("AgentContact Can't be update"); 
								temp.AgentContactId = myAgentContactPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService AgentCardService = new CardQueryService(Tenant);
					if(MyEntity.Agent != null)
					{
						var myAgentPM = AgentCardService.CardDataMappingAndValidatin(MyEntity.Agent,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAgentPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Agent Can't be update"); 
								temp.AgentId = myAgentPM.Id;
						  
							}  

							
						} 

					}
			
					
					BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
					if(MyEntity.Branch != null)
					{
						var myBranchPM = BranchBranchService.BranchDataMappingAndValidatin(MyEntity.Branch,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myBranchPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Branch Can't be update"); 
								temp.BranchId = myBranchPM.Id;
						  
							}  

							
						} 

					}
			
					
					DepartmentQueryService DepartmentDepartmentService = new DepartmentQueryService(Tenant);
					if(MyEntity.Department != null)
					{
						var myDepartmentPM = DepartmentDepartmentService.DepartmentDataMappingAndValidatin(MyEntity.Department,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDepartmentPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Department Can't be update"); 
								temp.DepartmentId = myDepartmentPM.Id;
						  
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
			
					
					ContactQueryService ShipperContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.ShipperContact != null)
					{
						var myShipperContactPM = ShipperContactContactService.ContactDataMappingAndValidatin(MyEntity.ShipperContact,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myShipperContactPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ShipperContact Can't be update"); 
								temp.ShipperContactId = myShipperContactPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService ShipperCardService = new CardQueryService(Tenant);
					if(MyEntity.Shipper != null)
					{
						var myShipperPM = ShipperCardService.CardDataMappingAndValidatin(MyEntity.Shipper,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myShipperPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Shipper Can't be update"); 
								temp.ShipperId = myShipperPM.Id;
						  
							}  

							
						} 

					}
			
					
					ContactQueryService ConsigneeContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.ConsigneeContact != null)
					{
						var myConsigneeContactPM = ConsigneeContactContactService.ContactDataMappingAndValidatin(MyEntity.ConsigneeContact,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myConsigneeContactPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ConsigneeContact Can't be update"); 
								temp.ConsigneeContactId = myConsigneeContactPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService ConsigneeCardService = new CardQueryService(Tenant);
					if(MyEntity.Consignee != null)
					{
						var myConsigneePM = ConsigneeCardService.CardDataMappingAndValidatin(MyEntity.Consignee,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myConsigneePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Consignee Can't be update"); 
								temp.ConsigneeId = myConsigneePM.Id;
						  
							}  

							
						} 

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
			
					
					UserQueryService UpdatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.UpdatedByUser != null)
					{
						var myUpdatedByUserPM = UpdatedByUserUserService.UserDataMappingAndValidatin(MyEntity.UpdatedByUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myUpdatedByUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("UpdatedByUser Can't be update"); 
								temp.UpdatedByUserId = myUpdatedByUserPM.Id;
						  
							}  

							
						} 

					}
			
					
					ContactQueryService CustomerContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.CustomerContact != null)
					{
						var myCustomerContactPM = CustomerContactContactService.ContactDataMappingAndValidatin(MyEntity.CustomerContact,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCustomerContactPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("CustomerContact Can't be update"); 
								temp.CustomerContactId = myCustomerContactPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService CustomerCardService = new CardQueryService(Tenant);
					if(MyEntity.Customer != null)
					{
						var myCustomerPM = CustomerCardService.CardDataMappingAndValidatin(MyEntity.Customer,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCustomerPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Customer Can't be update"); 
								temp.CustomerId = myCustomerPM.Id;
						  
							}  

							
						} 

					}
			
					
					AddressQueryService DeliveryAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.DeliveryAddress != null)
					{
						var myDeliveryAddressPM = DeliveryAddressAddressService.AddressDataMappingAndValidatin(MyEntity.DeliveryAddress,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDeliveryAddressPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("DeliveryAddress Can't be update"); 
								temp.DeliveryAddressId = myDeliveryAddressPM.Id;
						  
							}  

							
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
			
					
					IncotermQueryService IncotermIncotermService = new IncotermQueryService(Tenant);
					if(MyEntity.Incoterm != null)
					{
						var myIncotermPM = IncotermIncotermService.IncotermDataMappingAndValidatin(MyEntity.Incoterm,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myIncotermPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Incoterm Can't be update"); 
								temp.IncotermId = myIncotermPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService MainCarriageCarrierCardService = new CardQueryService(Tenant);
					if(MyEntity.MainCarriageCarrier != null)
					{
						var myMainCarriageCarrierPM = MainCarriageCarrierCardService.CardDataMappingAndValidatin(MyEntity.MainCarriageCarrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainCarriageCarrierPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("MainCarriageCarrier Can't be update"); 
								temp.MainCarriageCarrierId = myMainCarriageCarrierPM.Id;
						  
							}  

							
						} 

					}
			
					
					MoveTypeQueryService MoveTypeMoveTypeService = new MoveTypeQueryService(Tenant);
					if(MyEntity.MoveType != null)
					{
						var myMoveTypePM = MoveTypeMoveTypeService.MoveTypeDataMappingAndValidatin(MyEntity.MoveType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMoveTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("MoveType Can't be update"); 
								temp.MoveTypeId = myMoveTypePM.Id;
						  
							}  

							
						} 

					}
			
					
					AddressQueryService PickUpAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.PickUpAddress != null)
					{
						var myPickUpAddressPM = PickUpAddressAddressService.AddressDataMappingAndValidatin(MyEntity.PickUpAddress,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPickUpAddressPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PickUpAddress Can't be update"); 
								temp.PickUpAddressId = myPickUpAddressPM.Id;
						  
							}  

							
						} 

					}
			
					
					QuoteTypeQueryService QuoteTypeQuoteTypeService = new QuoteTypeQueryService(Tenant);
					if(MyEntity.QuoteType != null)
					{
						var myQuoteTypePM = QuoteTypeQuoteTypeService.QuoteTypeDataMappingAndValidatin(MyEntity.QuoteType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myQuoteTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("QuoteType Can't be update"); 
								temp.QuoteTypeCode = myQuoteTypePM.Code;
						  
							}  

							
						} 

					}
			
					
					CurrencyQueryService SaleCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.SaleCurrency != null)
					{
						var mySaleCurrencyPM = SaleCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.SaleCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySaleCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("SaleCurrency Can't be update"); 
								temp.SaleCurrencyId = mySaleCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
					UserQueryService SalesmanUserUserService = new UserQueryService(Tenant);
					if(MyEntity.SalesmanUser != null)
					{
						var mySalesmanUserPM = SalesmanUserUserService.UserDataMappingAndValidatin(MyEntity.SalesmanUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySalesmanUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("SalesmanUser Can't be update"); 
								temp.SalesmanUserId = mySalesmanUserPM.Id;
						  
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
			
					
					QuoteStageQueryService StageQuoteStageService = new QuoteStageQueryService(Tenant);
					if(MyEntity.Stage != null)
					{
						var myStagePM = StageQuoteStageService.QuoteStageDataMappingAndValidatin(MyEntity.Stage,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStagePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Stage Can't be update"); 
								temp.StageId = myStagePM.Id;
						  
							}  

							
						} 

					}
			
					
					CurrencyQueryService ValueOfGoodsCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.ValueOfGoodsCurrency != null)
					{
						var myValueOfGoodsCurrencyPM = ValueOfGoodsCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.ValueOfGoodsCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myValueOfGoodsCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ValueOfGoodsCurrency Can't be update"); 
								temp.ValueOfGoodsCurrencyId = myValueOfGoodsCurrencyPM.Id;
						  
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
			
					 

					if(MyEntity.QuoteCharges != null && MyEntity.QuoteCharges.Count > 0)
					{
						QuoteChargeQueryService QuoteChargeService38 = new QuoteChargeQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("QuoteCharges Can't be update"); 
								temp.QuoteCharges = QuoteChargeService38.QuoteChargeDataMappingAndValidatin(MyEntity.QuoteCharges,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
					PackageTypeQueryService PackageType1PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType1 != null)
					{
						var myPackageType1PM = PackageType1PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType1,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPackageType1PM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PackageType1 Can't be update"); 
								temp.PackageType1Id = myPackageType1PM.Id;
						  
							}  

							
						} 

					}
			
					
					PackageTypeQueryService PackageType2PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType2 != null)
					{
						var myPackageType2PM = PackageType2PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType2,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPackageType2PM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PackageType2 Can't be update"); 
								temp.PackageType2Id = myPackageType2PM.Id;
						  
							}  

							
						} 

					}
			
					
					PackageTypeQueryService PackageType3PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType3 != null)
					{
						var myPackageType3PM = PackageType3PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType3,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPackageType3PM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PackageType3 Can't be update"); 
								temp.PackageType3Id = myPackageType3PM.Id;
						  
							}  

							
						} 

					}
			
					
					PackageTypeQueryService PackageType4PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType4 != null)
					{
						var myPackageType4PM = PackageType4PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType4,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPackageType4PM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PackageType4 Can't be update"); 
								temp.PackageType4Id = myPackageType4PM.Id;
						  
							}  

							
						} 

					}
			
					
					PackageTypeQueryService PackageType5PackageTypeService = new PackageTypeQueryService(Tenant);
					if(MyEntity.PackageType5 != null)
					{
						var myPackageType5PM = PackageType5PackageTypeService.PackageTypeDataMappingAndValidatin(MyEntity.PackageType5,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPackageType5PM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PackageType5 Can't be update"); 
								temp.PackageType5Id = myPackageType5PM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && MyEntity.PackageType1Quantity != null)
					{							//throw new ApplicationException("PackageType1Quantity Can't be update"); 
							temp.PackageType1Quantity = MyEntity.PackageType1Quantity;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.PackageType2Quantity != null)
					{							//throw new ApplicationException("PackageType2Quantity Can't be update"); 
							temp.PackageType2Quantity = MyEntity.PackageType2Quantity;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.PackageType3Quantity != null)
					{							//throw new ApplicationException("PackageType3Quantity Can't be update"); 
							temp.PackageType3Quantity = MyEntity.PackageType3Quantity;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.PackageType4Quantity != null)
					{							//throw new ApplicationException("PackageType4Quantity Can't be update"); 
							temp.PackageType4Quantity = MyEntity.PackageType4Quantity;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.PackageType5Quantity != null)
					{							//throw new ApplicationException("PackageType5Quantity Can't be update"); 
							temp.PackageType5Quantity = MyEntity.PackageType5Quantity;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.PackagesQuantity != null)
					{							//throw new ApplicationException("PackagesQuantity Can't be update"); 
							temp.NumberOfPackages = MyEntity.PackagesQuantity;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.VolumetricWeight != null)
					{							//throw new ApplicationException("VolumetricWeight Can't be update"); 
							temp.VolumetricWeight = MyEntity.VolumetricWeight;

										}  

					 

					if(MyEntity.QuotePackages != null && MyEntity.QuotePackages.Count > 0)
					{
						QuotePackageQueryService QuotePackageService38 = new QuotePackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("QuotePackages Can't be update"); 
								temp.QuotePackages = QuotePackageService38.QuotePackageDataMappingAndValidatin(MyEntity.QuotePackages,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    
					if(!IsUpdate)// && MyEntity.StageDate != null)
					{							//throw new ApplicationException("StageDate Can't be update"); 
							temp.LastStageDate = MyEntity.StageDate;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.SameOrFixed))
					{							//throw new ApplicationException("SameOrFixed Can't be update"); 
							temp.SameOrFixed = MyEntity.SameOrFixed;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PickupCity))
					{							//throw new ApplicationException("PickupCity Can't be update"); 
							temp.PickupCity = MyEntity.PickupCity;

										}  

					
					CountryQueryService PickupCountryCountryService = new CountryQueryService(Tenant);
					if(MyEntity.PickupCountry != null)
					{
						var myPickupCountryPM = PickupCountryCountryService.CountryDataMappingAndValidatin(MyEntity.PickupCountry,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPickupCountryPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PickupCountry Can't be update"); 
								temp.PickupCountryId = myPickupCountryPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PickupZipCode))
					{							//throw new ApplicationException("PickupZipCode Can't be update"); 
							temp.PickupZipCode = MyEntity.PickupZipCode;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.DeliveryCity))
					{							//throw new ApplicationException("DeliveryCity Can't be update"); 
							temp.DeliveryCity = MyEntity.DeliveryCity;

										}  

					
					CountryQueryService DeliveryCountryCountryService = new CountryQueryService(Tenant);
					if(MyEntity.DeliveryCountry != null)
					{
						var myDeliveryCountryPM = DeliveryCountryCountryService.CountryDataMappingAndValidatin(MyEntity.DeliveryCountry,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myDeliveryCountryPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("DeliveryCountry Can't be update"); 
								temp.DeliveryCountryId = myDeliveryCountryPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.DeliveryZipCode))
					{							//throw new ApplicationException("DeliveryZipCode Can't be update"); 
							temp.DeliveryZipCode = MyEntity.DeliveryZipCode;

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