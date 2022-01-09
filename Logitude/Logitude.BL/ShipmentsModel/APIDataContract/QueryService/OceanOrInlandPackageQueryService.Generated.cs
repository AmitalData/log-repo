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
   public partial class OceanOrInlandPackageQueryService
   {
   
		ShipmentPackageQuery query; 

        public OceanOrInlandPackageQueryService(int tenant)
        {
		
			query = new ShipmentPackageQuery(tenant);
        }

		
		public List<OceanOrInlandPackage> OceanOrInlandPackageDataMapping(List<ShipmentPackagePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<OceanOrInlandPackage>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new OceanOrInlandPackage(); 
				   temp.Id = item.Id; 

			  
				   if(item.PackageTypeId != null)
				   {
					   PackageTypeQueryService PackageTypeService0 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType = PackageTypeService0.GetPackageTypeById(item.PackageTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Length = item.Length;
				   temp.Width = item.Width;
				   temp.Height = item.Height;
				   temp.Pieces = item.Quantity;
				   temp.Volume = item.Volume;
				   temp.GrossWeight = item.Weight;
				   temp.Seal = item.ShipperSeal;
				   temp.Seal2 = item.CarrierSeal;
				   temp.Reference1 = item.Reference1;
				   temp.Reference2 = item.Reference2;
				   temp.Reference3 = item.Reference3;
				   temp.Harmonize = item.Harmonize;
				   temp.Temperature = item.Temperature;
				   temp.Ventilation = item.Ventilation;
				   temp.IsDangerous = item.IsDangerous;
				   temp.ClassNumber = item.ClassNumber;
				   temp.UnNumber = item.UnNumber;
				   temp.PackagingGroup = item.PackagingGroup;
				   temp.IMDGCode = item.IMDGCode;
				   temp.FlashPoint = item.FlashPoint;
				   temp.MaterialDescription = item.MaterialDescription;
				   temp.CommodityNumber = item.CommodityNumber;
				   temp.Reference4 = item.Reference4;
				   temp.Notes = item.Notes;
				if(item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count > 0)
				{
					 InsidePackageQueryService InsidePackageService1 = new InsidePackageQueryService(Tenant);
					 temp.InsidePackages = InsidePackageService1.InsidePackageDataMapping(item.InsideShipmentPackages,Tenant,ComputingPartnerName);
				}

							 
				   temp.ContainerNumber = item.ContainerNumber;
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

		public List<ShipmentPackagePM> OceanOrInlandPackageDataMappingAndValidatin(List<OceanOrInlandPackage> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
					PackageTypeQueryService PackageTypePackageTypeService = new PackageTypeQueryService(Tenant);
					if(item.PackageType != null)
					{
						var myPackageTypePM = PackageTypePackageTypeService.PackageTypeDataMappingAndValidatin(item.PackageType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPackageTypePM != null)
						{ 

						 								
								temp.PackageTypeId = myPackageTypePM.Id;
						  

							
						} 

					}
			
					
                    							
						temp.Length = item.Length;

					 

					
                    							
						temp.Width = item.Width;

					 

					
                    							
						temp.Height = item.Height;

					 

					
                    							
						temp.Quantity = item.Pieces;

					 

					
                    							
						temp.Volume = item.Volume;

					 

					
                    							
						temp.Weight = item.GrossWeight;

					 

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.ShipperSeal = item.Seal;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.CarrierSeal = item.Seal2;

										}  

					
                    							
						temp.Reference1 = item.Reference1;

					 

					
                    							
						temp.Reference2 = item.Reference2;

					 

					
                    							
						temp.Reference3 = item.Reference3;

					 

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.Harmonize = item.Harmonize;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.Temperature = item.Temperature;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.Ventilation = item.Ventilation;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.IsDangerous = item.IsDangerous;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.ClassNumber = item.ClassNumber;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.UnNumber = item.UnNumber;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.PackagingGroup = item.PackagingGroup;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.IMDGCode = item.IMDGCode;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.FlashPoint = item.FlashPoint;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.MaterialDescription = item.MaterialDescription;

										}  

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.CommodityNumber = item.CommodityNumber;

										}  

					
                    							
						temp.Reference4 = item.Reference4;

					 

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.Notes = item.Notes;

										}  

					 

					if(item.InsidePackages != null && item.InsidePackages.Count > 0)
					{
						InsidePackageQueryService InsidePackageService1 = new InsidePackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.InsideShipmentPackages = InsidePackageService1.InsidePackageDataMappingAndValidatin(item.InsidePackages,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
                    							
						temp.ContainerNumber = item.ContainerNumber;

					 

					
                    							
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