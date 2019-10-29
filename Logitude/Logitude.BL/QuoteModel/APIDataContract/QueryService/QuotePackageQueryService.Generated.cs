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

		public List<QuotePackagePM> QuotePackageDataMappingAndValidatin(List<QuotePackage> MyEntity,int Tenant,string ComputingPartnerName = "")
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
						var myPackageTypePM = PackageTypePackageTypeService.PackageTypeDataMappingAndValidatin(item.PackageType,Tenant,ComputingPartnerName);
												if(myPackageTypePM != null)
						{
							temp.PackageTypeId = myPackageTypePM.Id;
						}
						 
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
		 
   }
}