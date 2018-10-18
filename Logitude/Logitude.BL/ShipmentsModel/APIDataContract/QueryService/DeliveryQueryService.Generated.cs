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
				   temp.ETD = item.ETD;
				   temp.ETA = item.ETA;
			  
				   if(item.FromPartnerCardId != null)
				   {
					   CardQueryService CardService0 = new CardQueryService(Tenant);
					   					   temp.FromPartnerCard = CardService0.GetCardById(item.FromPartnerCardId,Tenant); 
			       
					   				   }
			  
				   if(item.FromPortId != null)
				   {
					   PortQueryService PortService1 = new PortQueryService(Tenant);
					   					   temp.FromPort = PortService1.GetPortById(item.FromPortId,Tenant); 
			       
					   				   }
			  
				   if(item.ToPartnerCardId != null)
				   {
					   CardQueryService CardService2 = new CardQueryService(Tenant);
					   					   temp.ToPartnerCard = CardService2.GetCardById(item.ToPartnerCardId,Tenant); 
			       
					   				   }
			  
				   if(item.ToPortId != null)
				   {
					   PortQueryService PortService3 = new PortQueryService(Tenant);
					   					   temp.ToPort = PortService3.GetPortById(item.ToPortId,Tenant); 
			       
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

		public List<ShipmentDeliveryPM> DeliveryDataMappingAndValidatin(List<Delivery> MyEntity,int Tenant,string ComputingPartnerName = "")
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
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = item.Id;
					}
					temp.ETD = item.ETD;
					temp.ETA = item.ETA;
					CardQueryService FromPartnerCardCardService = new CardQueryService(Tenant);
					if(item.FromPartnerCard != null)
					{
						var myFromPartnerCardPM = FromPartnerCardCardService.CardDataMappingAndValidatin(item.FromPartnerCard,Tenant,ComputingPartnerName);
						if(myFromPartnerCardPM != null)
						{
							temp.FromPartnerCardId = myFromPartnerCardPM.Id;
						} 
					}
					PortQueryService FromPortPortService = new PortQueryService(Tenant);
					if(item.FromPort != null)
					{
						var myFromPortPM = FromPortPortService.PortDataMappingAndValidatin(item.FromPort,Tenant,ComputingPartnerName);
						if(myFromPortPM != null)
						{
							temp.FromPortId = myFromPortPM.Id;
						} 
					}
					CardQueryService ToPartnerCardCardService = new CardQueryService(Tenant);
					if(item.ToPartnerCard != null)
					{
						var myToPartnerCardPM = ToPartnerCardCardService.CardDataMappingAndValidatin(item.ToPartnerCard,Tenant,ComputingPartnerName);
						if(myToPartnerCardPM != null)
						{
							temp.ToPartnerCardId = myToPartnerCardPM.Id;
						} 
					}
					PortQueryService ToPortPortService = new PortQueryService(Tenant);
					if(item.ToPort != null)
					{
						var myToPortPM = ToPortPortService.PortDataMappingAndValidatin(item.ToPort,Tenant,ComputingPartnerName);
						if(myToPortPM != null)
						{
							temp.ToPortId = myToPortPM.Id;
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