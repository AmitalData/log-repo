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
   public partial class PackageQueryService
   {
   
		ShipmentPickUpDeliveryPackageQuery query; 

        public PackageQueryService(int tenant)
        {
		
			query = new ShipmentPickUpDeliveryPackageQuery(tenant);
        }

		
		public List<Package> PackageDataMapping(List<ShipmentPickUpDeliveryPackagePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<Package>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new Package(); 
				   temp.Id = item.Id; 

			  
				   if(item.PackageTypeId != null)
				   {
					   PackageTypeQueryService PackageTypeService0 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType = PackageTypeService0.GetPackageTypeById(item.PackageTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Quantity = item.Quantity;
				   temp.ContainerNumber = item.ContainerNumber;
				   temp.Volume = item.Volume;
				   temp.Weight = item.Weight;
				   temp.Description = item.Description;
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

		public List<ShipmentPickUpDeliveryPackagePM> PackageDataMappingAndValidatin(List<Package> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ShipmentPickUpDeliveryPackagePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ShipmentPickUpDeliveryPackagePM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("ShipmentPickUpDeliveryPackage with Id " + item.Id + " doesn't exist");
					} 
				 
										 
					if(IsUpdate == true)
					{
					    
						
					      temp.ChangeSetOp = ChangeSetOperation.Update; 
					}
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ShipmentPickUpDeliveryPackage with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
					}
					PackageTypeQueryService PackageTypePackageTypeService = new PackageTypeQueryService(Tenant);
					if(item.PackageType != null)
					{
						var myPackageTypePM = PackageTypePackageTypeService.PackageTypeDataMappingAndValidatin(item.PackageType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPackageTypePM != null)
						{ 

						 								
								temp.PackageTypeId = myPackageTypePM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.Quantity = item.Quantity;

					 

					
                    							
						temp.ContainerNumber = item.ContainerNumber;

					 

					
                    							
						temp.Volume = item.Volume;

					 

					
                    							
						temp.Weight = item.Weight;

					 

					
                    							
						temp.Description = item.Description;

					 

					
                    							
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