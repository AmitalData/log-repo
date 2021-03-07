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
				if(item.ShipmentPickUpDeliveryPackages != null && item.ShipmentPickUpDeliveryPackages.Count > 0)
				{
					 ShipmentPickUpDeliveryPackageQueryService ShipmentPickUpDeliveryPackageService5 = new ShipmentPickUpDeliveryPackageQueryService(Tenant);
					 temp.ShipmentPickUpDeliveryPackages = ShipmentPickUpDeliveryPackageService5.ShipmentPickUpDeliveryPackageDataMapping(item.ShipmentPickUpDeliveryPackages,Tenant,ComputingPartnerName);
				}

				   temp.PickUpReference = item.PickUpDeliveryNumber;					

							 					
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
                    							//throw new ApplicationException("ATD Can't be update"); 
							temp.ATD = item.ATD;

					 

					
                    							//throw new ApplicationException("ATA Can't be update"); 
							temp.ATA = item.ATA;

					 

					
                    							//throw new ApplicationException("ETD Can't be update"); 
							temp.ETD = item.ETD;

					 

					
                    							//throw new ApplicationException("ETA Can't be update"); 
							temp.ETA = item.ETA;

					 

					
					PortQueryService FromPortPortService = new PortQueryService(Tenant);
					if(item.FromPort != null)
					{
						var myFromPortPM = FromPortPortService.PortDataMappingAndValidatin(item.FromPort,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myFromPortPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("FromPort Can't be update"); 
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
							{								//throw new ApplicationException("ToPort Can't be update"); 
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
							{								//throw new ApplicationException("FromPartnerCard Can't be update"); 
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
							{								//throw new ApplicationException("ToPartnerCard Can't be update"); 
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
							{								//throw new ApplicationException("Carrier Can't be update"); 
								temp.CarrierId = myCarrierPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.TruckNumber))
					{							//throw new ApplicationException("TruckNumber Can't be update"); 
							temp.TruckNumber = item.TruckNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Driver))
					{							//throw new ApplicationException("Driver Can't be update"); 
							temp.Driver = item.Driver;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.TrailerNumber))
					{							//throw new ApplicationException("TrailerNumber Can't be update"); 
							temp.TrailerNumber = item.TrailerNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.TransportModeCode))
					{							//throw new ApplicationException("TransportModeCode Can't be update"); 
							temp.TransportModeCode = item.TransportModeCode;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Notes))
					{							//throw new ApplicationException("Notes Can't be update"); 
							temp.Notes = item.Notes;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.TruckerNumber))
					{							//throw new ApplicationException("TruckerNumber Can't be update"); 
							temp.CarrierNumber = item.TruckerNumber;

										}  

					 

					if(item.ShipmentPickUpDeliveryPackages != null && item.ShipmentPickUpDeliveryPackages.Count > 0)
					{
						ShipmentPickUpDeliveryPackageQueryService ShipmentPickUpDeliveryPackageService5 = new ShipmentPickUpDeliveryPackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("ShipmentPickUpDeliveryPackages Can't be update"); 
								temp.ShipmentPickUpDeliveryPackages = ShipmentPickUpDeliveryPackageService5.ShipmentPickUpDeliveryPackageDataMappingAndValidatin(item.ShipmentPickUpDeliveryPackages,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.PickUpReference))
					{							//throw new ApplicationException("PickUpReference Can't be update"); 
							temp.PickUpDeliveryNumber = item.PickUpReference;

						
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
