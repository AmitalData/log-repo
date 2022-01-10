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
   public partial class ContainerQueryService
   {
   
		ShipmentPackageQuery query; 

        public ContainerQueryService(int tenant)
        {
		
			query = new ShipmentPackageQuery(tenant);
        }

		
		public List<Container> ContainerDataMapping(List<ShipmentPackagePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<Container>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new Container(); 
				   temp.Id = item.Id; 

			  
				   if(item.PackageTypeId != null)
				   {
					   PackageTypeQueryService PackageTypeService0 = new PackageTypeQueryService(Tenant);
					   					   temp.ContainerType = PackageTypeService0.GetPackageTypeById(item.PackageTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ContainerNumber = item.ContainerNumber;
				   temp.Volume = item.Volume;
				   temp.GrossWeight = item.Weight;
				   temp.Tare = item.Tare;
				   temp.Seal = item.ShipperSeal;
				   temp.Seal2 = item.CarrierSeal;
				   temp.MarksAndNumbers = item.MarksAndNumbers;
				   temp.Reference1 = item.Reference1;
				   temp.Reference2 = item.Reference2;
				   temp.Reference3 = item.Reference3;
				   temp.CommodityNumber = item.CommodityNumber;
				   temp.Pieces = item.Quantity;
				   temp.Reference4 = item.Reference4;
				if(item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0)
				{
					 InsidePackageQueryService InsidePackageService1 = new InsidePackageQueryService(Tenant);
					 temp.InsidePackages = InsidePackageService1.InsidePackageDataMapping(item.InsideShipmentPackages,Tenant,ComputingPartnerName);
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

		public List<ShipmentPackagePM> ContainerDataMappingAndValidatin(List<Container> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<ShipmentPackagePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new ShipmentPackagePM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("ShipmentPackage with Id " + item.Id + " doesn't exist");
					} 
				 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("ShipmentPackage with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
					}
					PackageTypeQueryService ContainerTypePackageTypeService = new PackageTypeQueryService(Tenant);
					if(item.ContainerType != null)
					{
						var myContainerTypePM = ContainerTypePackageTypeService.PackageTypeDataMappingAndValidatin(item.ContainerType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myContainerTypePM != null)
						{ 

						 								
								temp.PackageTypeId = myContainerTypePM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.ContainerNumber = item.ContainerNumber;

					 

					
                    							
						temp.Volume = item.Volume;

					 

					
                    							
						temp.Weight = item.GrossWeight;

					 

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.Tare = item.Tare;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.ShipperSeal = item.Seal;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.CarrierSeal = item.Seal2;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.MarksAndNumbers = item.MarksAndNumbers;

										}  

					
                    							
						temp.Reference1 = item.Reference1;

					 

					
                    							
						temp.Reference2 = item.Reference2;

					 

					
                    							
						temp.Reference3 = item.Reference3;

					 

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.CommodityNumber = item.CommodityNumber;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.Quantity = item.Pieces;

										}  

					
                    							
						temp.Reference4 = item.Reference4;

					 

					 

					if(item.InsidePackages != null && item.InsidePackages.Count > 0)
					{
						InsidePackageQueryService InsidePackageService1 = new InsidePackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.InsideShipmentPackages = InsidePackageService1.InsidePackageDataMappingAndValidatin(item.InsidePackages,Tenant,ComputingPartnerName,IsUpdate);

					 
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