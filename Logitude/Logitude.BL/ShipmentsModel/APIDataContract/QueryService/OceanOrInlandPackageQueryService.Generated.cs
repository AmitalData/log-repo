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
					   					   temp.PackageType = PackageTypeService0.GetPackageTypeById(item.PackageTypeId,Tenant); 
			       
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
					 temp.InsidePackages = InsidePackageService1.InsidePackageDataMapping(item.InsideShipmentPackages,Tenant);
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

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PackageType Can't be update"); 
								temp.PackageTypeId = myPackageTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && item.Length != null)
					{							//throw new ApplicationException("Length Can't be update"); 
							temp.Length = item.Length;

										}  

					
                    
					if(!IsUpdate)// && item.Width != null)
					{							//throw new ApplicationException("Width Can't be update"); 
							temp.Width = item.Width;

										}  

					
                    
					if(!IsUpdate)// && item.Height != null)
					{							//throw new ApplicationException("Height Can't be update"); 
							temp.Height = item.Height;

										}  

					
                    
					if(!IsUpdate)// && item.Pieces != null)
					{							//throw new ApplicationException("Pieces Can't be update"); 
							temp.Quantity = item.Pieces;

										}  

					
                    
					if(!IsUpdate)// && item.Volume != null)
					{							//throw new ApplicationException("Volume Can't be update"); 
							temp.Volume = item.Volume;

										}  

					
                    
					if(!IsUpdate)// && item.GrossWeight != null)
					{							//throw new ApplicationException("GrossWeight Can't be update"); 
							temp.Weight = item.GrossWeight;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Seal))
					{							//throw new ApplicationException("Seal Can't be update"); 
							temp.ShipperSeal = item.Seal;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Seal2))
					{							//throw new ApplicationException("Seal2 Can't be update"); 
							temp.CarrierSeal = item.Seal2;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Reference1))
					{							//throw new ApplicationException("Reference1 Can't be update"); 
							temp.Reference1 = item.Reference1;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Reference2))
					{							//throw new ApplicationException("Reference2 Can't be update"); 
							temp.Reference2 = item.Reference2;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Reference3))
					{							//throw new ApplicationException("Reference3 Can't be update"); 
							temp.Reference3 = item.Reference3;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Harmonize))
					{							//throw new ApplicationException("Harmonize Can't be update"); 
							temp.Harmonize = item.Harmonize;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Temperature))
					{							//throw new ApplicationException("Temperature Can't be update"); 
							temp.Temperature = item.Temperature;

										}  

					
                    
					if(!IsUpdate)// && item.Ventilation != null)
					{							//throw new ApplicationException("Ventilation Can't be update"); 
							temp.Ventilation = item.Ventilation;

										}  

					
                    
					if(!IsUpdate)// && (item.IsDangerous != temp.IsDangerous))
					{							//throw new ApplicationException("IsDangerous Can't be update"); 
							temp.IsDangerous = item.IsDangerous;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.ClassNumber))
					{							//throw new ApplicationException("ClassNumber Can't be update"); 
							temp.ClassNumber = item.ClassNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.UnNumber))
					{							//throw new ApplicationException("UnNumber Can't be update"); 
							temp.UnNumber = item.UnNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.PackagingGroup))
					{							//throw new ApplicationException("PackagingGroup Can't be update"); 
							temp.PackagingGroup = item.PackagingGroup;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.IMDGCode))
					{							//throw new ApplicationException("IMDGCode Can't be update"); 
							temp.IMDGCode = item.IMDGCode;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.FlashPoint))
					{							//throw new ApplicationException("FlashPoint Can't be update"); 
							temp.FlashPoint = item.FlashPoint;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.MaterialDescription))
					{							//throw new ApplicationException("MaterialDescription Can't be update"); 
							temp.MaterialDescription = item.MaterialDescription;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.CommodityNumber))
					{							//throw new ApplicationException("CommodityNumber Can't be update"); 
							temp.CommodityNumber = item.CommodityNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Reference4))
					{							//throw new ApplicationException("Reference4 Can't be update"); 
							temp.Reference4 = item.Reference4;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(item.Notes))
					{							//throw new ApplicationException("Notes Can't be update"); 
							temp.Notes = item.Notes;

										}  

					 

					if(item.InsidePackages != null && item.InsidePackages.Count > 0)
					{
						InsidePackageQueryService InsidePackageService1 = new InsidePackageQueryService(Tenant);
						  
						if(!IsUpdate)
						{								//throw new ApplicationException("InsidePackages Can't be update"); 
								temp.InsideShipmentPackages = InsidePackageService1.InsidePackageDataMappingAndValidatin(item.InsidePackages,Tenant,ComputingPartnerName,IsUpdate);

					 
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