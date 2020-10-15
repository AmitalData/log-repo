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
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.QuoteModel;

 namespace Logitude.BL.QuoteModel.APIDataContract.ApiV1
{ 
   public partial class QuotePackageQueryService
   {
   
		QuotePackageQuery query; 

        public QuotePackageQueryService(int tenant)
        {
		
			query = new QuotePackageQuery(tenant);
        }

		
		public List<QuotePackage> QuotePackageDataMapping(List<QuotePackagePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<QuotePackage>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new QuotePackage(); 
				   temp.Id = item.Id; 

			  
				   if(item.PackageTypeId != null)
				   {
					   PackageTypeQueryService PackageTypeService0 = new PackageTypeQueryService(Tenant);
					   					   temp.PackageType = PackageTypeService0.GetPackageTypeById(item.PackageTypeId,Tenant); 
			       
					   				   }
				   
				   temp.Quantity = item.Quantity;
				   temp.GrossWeight = item.GrossWeight;
				   temp.Volume = item.Volume;
				   temp.Height = item.Height;
				   temp.Width = item.Width;
				   temp.Length = item.Length;
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

		public List<QuotePackagePM> QuotePackageDataMappingAndValidatin(List<QuotePackage> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<QuotePackagePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new QuotePackagePM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("QuotePackage with Id " + item.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("QuotePackage with provided key doesn't exist");
						
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
			
					
                    
					if(!IsUpdate)// && item.Quantity != null)
					{							//throw new ApplicationException("Quantity Can't be update"); 
							temp.Quantity = item.Quantity;

										}  

					
                    
					if(!IsUpdate)// && item.GrossWeight != null)
					{							//throw new ApplicationException("GrossWeight Can't be update"); 
							temp.GrossWeight = item.GrossWeight;

										}  

					
                    
					if(!IsUpdate)// && item.Volume != null)
					{							//throw new ApplicationException("Volume Can't be update"); 
							temp.Volume = item.Volume;

										}  

					
                    
					if(!IsUpdate)// && item.Height != null)
					{							//throw new ApplicationException("Height Can't be update"); 
							temp.Height = item.Height;

										}  

					
                    
					if(!IsUpdate)// && item.Width != null)
					{							//throw new ApplicationException("Width Can't be update"); 
							temp.Width = item.Width;

										}  

					
                    
					if(!IsUpdate)// && item.Length != null)
					{							//throw new ApplicationException("Length Can't be update"); 
							temp.Length = item.Length;

										}  

					
                    
					if(!IsUpdate)// && item.VolumetricWeight != null)
					{							//throw new ApplicationException("VolumetricWeight Can't be update"); 
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