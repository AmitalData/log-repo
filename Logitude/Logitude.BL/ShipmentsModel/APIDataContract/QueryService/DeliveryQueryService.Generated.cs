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
   public partial class DeliveryQueryService
   {
   
		ShipmentDeliveryQuery query; 

        public DeliveryQueryService(int tenant)
        {
		
			query = new ShipmentDeliveryQuery(tenant);
        }

		
		public List<Delivery> DeliveryDataMapping(List<ShipmentDeliveryPM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<Delivery>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new Delivery(); 
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
				   temp.DeliveryReference = item.PickUpDeliveryNumber;
				if(item.ShipmentPickUpDeliveryPackages != null && item.ShipmentPickUpDeliveryPackages.Count > 0)
				{
					 PackageQueryService PackageService5 = new PackageQueryService(Tenant);
					 temp.Packages = PackageService5.PackageDataMapping(item.ShipmentPickUpDeliveryPackages,Tenant,ComputingPartnerName);
				}

							 					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<ShipmentDeliveryPM> DeliveryDataMappingAndValidatin(List<Delivery> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ShipmentDeliveryPM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ShipmentDeliveryPM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("ShipmentDelivery with Id " + item.Id + " doesn't exist");
					} 
										 
					if(IsUpdate == true)
					{
					    
						
					      temp.ChangeSetOp = ChangeSetOperation.Update; 
					}
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ShipmentDelivery with provided key doesn't exist");
						
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

						 
							if(!IsUpdate)
							{								
								temp.FromPortId = myFromPortPM.Id;
						  
							}  

							
						} 

					}
			
					
					PortQueryService ToPortPortService = new PortQueryService(Tenant);
					if(item.ToPort != null)
					{
						var myToPortPM = ToPortPortService.PortDataMappingAndValidatin(item.ToPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myToPortPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ToPortId = myToPortPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService FromPartnerCardCardService = new CardQueryService(Tenant);
					if(item.FromPartnerCard != null)
					{
						var myFromPartnerCardPM = FromPartnerCardCardService.CardDataMappingAndValidatin(item.FromPartnerCard,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFromPartnerCardPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.FromPartnerCardId = myFromPartnerCardPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService ToPartnerCardCardService = new CardQueryService(Tenant);
					if(item.ToPartnerCard != null)
					{
						var myToPartnerCardPM = ToPartnerCardCardService.CardDataMappingAndValidatin(item.ToPartnerCard,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myToPartnerCardPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.ToPartnerCardId = myToPartnerCardPM.Id;
						  
							}  

							
						} 

					}
			
					
					CardQueryService CarrierCardService = new CardQueryService(Tenant);
					if(item.Carrier != null)
					{
						var myCarrierPM = CarrierCardService.CardDataMappingAndValidatin(item.Carrier,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCarrierPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CarrierId = myCarrierPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.TruckNumber = item.TruckNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Driver = item.Driver;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.TrailerNumber = item.TrailerNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.TransportModeCode = item.TransportModeCode;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Notes = item.Notes;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CarrierNumber = item.TruckerNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PickUpDeliveryNumber = item.DeliveryReference;

										}  

					 

					if(item.Packages != null && item.Packages.Count > 0)
					{
						PackageQueryService PackageService5 = new PackageQueryService(Tenant);
						 								
							temp.ShipmentPickUpDeliveryPackages = PackageService5.PackageDataMappingAndValidatin(item.Packages,Tenant,ComputingPartnerName,IsUpdate);

					 

						
					}

								 					   
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