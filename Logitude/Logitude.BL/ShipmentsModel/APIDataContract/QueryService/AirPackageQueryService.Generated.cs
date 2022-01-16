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
   public partial class AirPackageQueryService
   {
   
		ShipmentPackageQuery query; 

        public AirPackageQueryService(int tenant)
        {
		
			query = new ShipmentPackageQuery(tenant);
        }

		
		public List<AirPackage> AirPackageDataMapping(List<ShipmentPackagePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<AirPackage>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new AirPackage(); 
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
				   temp.Reference1 = item.Reference1;
				   temp.Reference2 = item.Reference2;
				   temp.Reference3 = item.Reference3;
				   temp.CommodityNumber = item.CommodityNumber;
				   temp.Reference4 = item.Reference4;
				   temp.Notes = item.Notes;
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

		public List<ShipmentPackagePM> AirPackageDataMappingAndValidatin(List<AirPackage> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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

					 

					
                    							
						temp.Reference1 = item.Reference1;

					 

					
                    							
						temp.Reference2 = item.Reference2;

					 

					
                    							
						temp.Reference3 = item.Reference3;

					 

					
                    							
						temp.CommodityNumber = item.CommodityNumber;

					 

					
                    							
						temp.Reference4 = item.Reference4;

					 

					
                    
					if(!IsUpdate|| string.IsNullOrEmpty(item.Id))
					{							
						temp.Notes = item.Notes;

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