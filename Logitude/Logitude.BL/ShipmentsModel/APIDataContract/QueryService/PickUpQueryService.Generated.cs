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
   public partial class PickUpQueryService
   {
   
		ShipmentPickUpQuery query; 

        public PickUpQueryService(int tenant)
        {
		
			query = new ShipmentPickUpQuery(tenant);
        }

		
		public List<PickUp> PickUpDataMapping(List<ShipmentPickUpPM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<PickUp>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new PickUp(); 
				   temp.Id = item.Id;
				   temp.ATD = item.ATD;
				   temp.ATA = item.ATA;
				   temp.ETD = item.ETD;
				   temp.ETA = item.ETA; 

			  
				   if(item.FromPortId != null)
				   {
					   PortQueryService PortService0 = new PortQueryService(Tenant);
					   					   temp.FromPort = PortService0.GetPortById(item.FromPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(item.ToPortId != null)
				   {
					   PortQueryService PortService1 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService1.GetPortById(item.ToPortId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(item.FromPartnerCardId != null)
				   {
					   CardQueryService CardService2 = new CardQueryService(Tenant);
					   					   temp.FromPartnerCard = CardService2.GetCardById(item.FromPartnerCardId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(item.ToPartnerCardId != null)
				   {
					   CardQueryService CardService3 = new CardQueryService(Tenant);
					   					   temp.ToPartnerCard = CardService3.GetCardById(item.ToPartnerCardId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(item.CarrierId != null)
				   {
					   CardQueryService CardService4 = new CardQueryService(Tenant);
					   					   temp.Carrier = CardService4.GetCardById(item.CarrierId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.TruckNumber = item.TruckNumber;
				   temp.Driver = item.Driver;
				   temp.TrailerNumber = item.TrailerNumber;
				   temp.TransportModeCode = item.TransportModeCode;
				   temp.Notes = item.Notes;
				   temp.TruckerNumber = item.CarrierNumber;
				   temp.PickUpReference = item.PickUpDeliveryNumber;
				if(item.ShipmentPickUpDeliveryPackages != null && item.ShipmentPickUpDeliveryPackages.Count > 0)
				{
					 PackageQueryService PackageService5 = new PackageQueryService(Tenant);
					 temp.Packages = PackageService5.PackageDataMapping(item.ShipmentPickUpDeliveryPackages,Tenant,ComputingPartnerName);
				}

							  

			  
				   if(item.PickUpDeliveryFromTypeCode != null)
				   {
					   PickUpDeliveryFromToTypeQueryService PickUpDeliveryFromToTypeService5 = new PickUpDeliveryFromToTypeQueryService(Tenant);
					   					   temp.FromType = PickUpDeliveryFromToTypeService5.GetPickUpDeliveryFromToTypeByCode(item.PickUpDeliveryFromTypeCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(item.PickUpDeliveryToTypeCode != null)
				   {
					   PickUpDeliveryFromToTypeQueryService PickUpDeliveryFromToTypeService6 = new PickUpDeliveryFromToTypeQueryService(Tenant);
					   					   temp.ToType = PickUpDeliveryFromToTypeService6.GetPickUpDeliveryFromToTypeByCode(item.PickUpDeliveryToTypeCode,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.FromCity = item.FromAddressCity;
				   temp.FromZipCode = item.FromAddressZipCode; 

			  
				   if(item.FromAddressCountryId != null)
				   {
					   CountryQueryService CountryService7 = new CountryQueryService(Tenant);
					   					   temp.FromCountry = CountryService7.GetCountryById(item.FromAddressCountryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ToCity = item.ToAddressCity;
				   temp.ToZipCode = item.ToAddressZipCode; 

			  
				   if(item.ToAddressCountryId != null)
				   {
					   CountryQueryService CountryService8 = new CountryQueryService(Tenant);
					   					   temp.ToCountry = CountryService8.GetCountryById(item.ToAddressCountryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ChangeSetOp = item.ChangeSet;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<ShipmentPickUpPM> PickUpDataMappingAndValidatin(List<PickUp> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ShipmentPickUpPM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ShipmentPickUpPM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("ShipmentPickUp with Id " + item.Id + " doesn't exist");
					} 
				 
										 
					if(IsUpdate == true)
					{
					    
						
					      temp.ChangeSetOp = ChangeSetOperation.Update; 
					}
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ShipmentPickUp with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
					}
                    							
						temp.ATD = item.ATD;

					 

					
                    							
						temp.ATA = item.ATA;

					 

					
                    							
						temp.ETD = item.ETD;

					 

					
                    							
						temp.ETA = item.ETA;

					 

					
					PortQueryService FromPortPortService = new PortQueryService(Tenant);
					if(item.FromPort != null)
					{
						var myFromPortPM = FromPortPortService.PortDataMappingAndValidatin(item.FromPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFromPortPM != null)
						{ 

						 								
								temp.FromPortId = myFromPortPM.Id;
						  

							
						} 

					}
			
					
					PortQueryService ToPortPortService = new PortQueryService(Tenant);
					if(item.ToPort != null)
					{
						var myToPortPM = ToPortPortService.PortDataMappingAndValidatin(item.ToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myToPortPM != null)
						{ 

						 								
								temp.ToPortId = myToPortPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService FromPartnerCardCardService = new CardQueryService(Tenant);
					if(item.FromPartnerCard != null)
					{
						var myFromPartnerCardPM = FromPartnerCardCardService.CardDataMappingAndValidatin(item.FromPartnerCard,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFromPartnerCardPM != null)
						{ 

						 								
								temp.FromPartnerCardId = myFromPartnerCardPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService ToPartnerCardCardService = new CardQueryService(Tenant);
					if(item.ToPartnerCard != null)
					{
						var myToPartnerCardPM = ToPartnerCardCardService.CardDataMappingAndValidatin(item.ToPartnerCard,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myToPartnerCardPM != null)
						{ 

						 								
								temp.ToPartnerCardId = myToPartnerCardPM.Id;
						  

							
						} 

					}
			
					
					CardQueryService CarrierCardService = new CardQueryService(Tenant);
					if(item.Carrier != null)
					{
						var myCarrierPM = CarrierCardService.CardDataMappingAndValidatin(item.Carrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCarrierPM != null)
						{ 

						 								
								temp.CarrierId = myCarrierPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.TruckNumber = item.TruckNumber;

					 

					
                    							
						temp.Driver = item.Driver;

					 

					
                    							
						temp.TrailerNumber = item.TrailerNumber;

					 

					
                    
					if(!IsUpdate)
					{							
						temp.TransportModeCode = item.TransportModeCode;

										}  

					
                    							
						temp.Notes = item.Notes;

					 

					
                    							
						temp.CarrierNumber = item.TruckerNumber;

					 

					
                    
					if(!IsUpdate)
					{							
						temp.PickUpDeliveryNumber = item.PickUpReference;

										}  

					 

					if(item.Packages != null && item.Packages.Count > 0)
					{
						PackageQueryService PackageService9 = new PackageQueryService(Tenant);
						 								
							temp.ShipmentPickUpDeliveryPackages = PackageService9.PackageDataMappingAndValidatin(item.Packages,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 
					PickUpDeliveryFromToTypeQueryService FromTypePickUpDeliveryFromToTypeService = new PickUpDeliveryFromToTypeQueryService(Tenant);
					if(item.FromType != null)
					{
						var myFromTypePM = FromTypePickUpDeliveryFromToTypeService.PickUpDeliveryFromToTypeDataMappingAndValidatin(item.FromType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFromTypePM != null)
						{ 

						 								
								temp.PickUpDeliveryFromTypeCode = myFromTypePM.Code;
						  

							
						} 

					}
			
					
					PickUpDeliveryFromToTypeQueryService ToTypePickUpDeliveryFromToTypeService = new PickUpDeliveryFromToTypeQueryService(Tenant);
					if(item.ToType != null)
					{
						var myToTypePM = ToTypePickUpDeliveryFromToTypeService.PickUpDeliveryFromToTypeDataMappingAndValidatin(item.ToType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myToTypePM != null)
						{ 

						 								
								temp.PickUpDeliveryToTypeCode = myToTypePM.Code;
						  

							
						} 

					}
			
					
                    							
						temp.FromAddressCity = item.FromCity;

					 

					
                    							
						temp.FromAddressZipCode = item.FromZipCode;

					 

					
					CountryQueryService FromCountryCountryService = new CountryQueryService(Tenant);
					if(item.FromCountry != null)
					{
						var myFromCountryPM = FromCountryCountryService.CountryDataMappingAndValidatin(item.FromCountry,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFromCountryPM != null)
						{ 

						 								
								temp.FromAddressCountryId = myFromCountryPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.ToAddressCity = item.ToCity;

					 

					
                    							
						temp.ToAddressZipCode = item.ToZipCode;

					 

					
					CountryQueryService ToCountryCountryService = new CountryQueryService(Tenant);
					if(item.ToCountry != null)
					{
						var myToCountryPM = ToCountryCountryService.CountryDataMappingAndValidatin(item.ToCountry,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myToCountryPM != null)
						{ 

						 								
								temp.ToAddressCountryId = myToCountryPM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.ChangeSet = item.ChangeSetOp;

					 

										   
						MyList.Add(temp);
					}
						
					return MyList;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }


						   
   }
}