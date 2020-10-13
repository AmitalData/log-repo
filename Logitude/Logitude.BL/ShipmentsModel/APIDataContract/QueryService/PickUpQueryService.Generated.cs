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
					   					   temp.FromPort = PortService0.GetPortById(item.FromPortId,Tenant); 
			       
					   				   }
				    

			  
				   if(item.ToPortId != null)
				   {
					   PortQueryService PortService1 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService1.GetPortById(item.ToPortId,Tenant); 
			       
					   				   }
				    

			  
				   if(item.FromPartnerCardId != null)
				   {
					   CardQueryService CardService2 = new CardQueryService(Tenant);
					   					   temp.FromPartnerCard = CardService2.GetCardById(item.FromPartnerCardId,Tenant); 
			       
					   				   }
				    

			  
				   if(item.ToPartnerCardId != null)
				   {
					   CardQueryService CardService3 = new CardQueryService(Tenant);
					   					   temp.ToPartnerCard = CardService3.GetCardById(item.ToPartnerCardId,Tenant); 
			       
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
						var myFromPortPM = FromPortPortService.PortDataMappingAndValidatin(item.FromPort,Tenant,ComputingPartnerName);
						
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
						var myToPortPM = ToPortPortService.PortDataMappingAndValidatin(item.ToPort,Tenant,ComputingPartnerName);
						
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
						var myFromPartnerCardPM = FromPartnerCardCardService.CardDataMappingAndValidatin(item.FromPartnerCard,Tenant,ComputingPartnerName);
						
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
						var myToPartnerCardPM = ToPartnerCardCardService.CardDataMappingAndValidatin(item.ToPartnerCard,Tenant,ComputingPartnerName);
						
						if(myToPartnerCardPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("ToPartnerCard Can't be update"); 
								temp.ToPartnerCardId = myToPartnerCardPM.Id;
						  
							}  

							
						} 

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