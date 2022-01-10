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
   public partial class CommodityQueryService
   {
   
		ShipmentCommodityQuery query; 

        public CommodityQueryService(int tenant)
        {
		
			query = new ShipmentCommodityQuery(tenant);
        }

		
		public List<Commodity> CommodityDataMapping(List<ShipmentCommodityPM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<Commodity>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new Commodity(); 
				   temp.Id = item.Id;
				   temp.DescriptionOfGoods = item.DescriptionOfGoods;
				   temp.ChargeableWeight = item.ChargeableWeight;
				   temp.ChargeRate = item.ChargeRate;
				   temp.ChargeAmount = item.ChargeAmount;
				   temp.CommodityNumber = item.CommodityNumber;
				   temp.NumberOfPackages = item.NumberOfPackages;
				   temp.GrossWeight = item.GrossWeight;
				   temp.Volume = item.Volume;
				   temp.VolumetricWeight = item.VolumetricWeight;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<ShipmentCommodityPM> CommodityDataMappingAndValidatin(List<Commodity> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ShipmentCommodityPM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ShipmentCommodityPM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("ShipmentCommodity with Id " + item.Id + " doesn't exist");
					} 
				 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ShipmentCommodity with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
					}
                    
					if(!IsUpdate)
					{							
						temp.DescriptionOfGoods = item.DescriptionOfGoods;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ChargeableWeight = item.ChargeableWeight;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ChargeRate = item.ChargeRate;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ChargeAmount = item.ChargeAmount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CommodityNumber = item.CommodityNumber;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.NumberOfPackages = item.NumberOfPackages;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.GrossWeight = item.GrossWeight;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Volume = item.Volume;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.VolumetricWeight = item.VolumetricWeight;

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