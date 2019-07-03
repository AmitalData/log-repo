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
   public partial class InsidePackageQueryService
   {
   
		InsideShipmentPackageQuery query; 

        public InsidePackageQueryService(int tenant)
        {
		
			query = new InsideShipmentPackageQuery(tenant);
        }

		
		public List<InsidePackage> InsidePackageDataMapping(List<InsideShipmentPackagePM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<InsidePackage>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new InsidePackage(); 
				   temp.Id = item.Id;
				   temp.Type = item.PackageTypeCode;
				   temp.Quantity = item.Quantity;
				   temp.Width = item.Width;
				   temp.Length = item.Length;
				   temp.Height = item.Height;
				   temp.Volume = item.Volume;
				   temp.GrossWeight = item.Weight;
				   temp.Commodity = item.CommodityNumber;
				   temp.Reference1 = item.Reference1;
				   temp.Reference2 = item.Reference2;
				   temp.Reference3 = item.Reference3;
				   temp.Reference4 = item.Reference4;
				   temp.Description = item.Description;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<InsideShipmentPackagePM> InsidePackageDataMappingAndValidatin(List<InsidePackage> MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<InsideShipmentPackagePM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new InsideShipmentPackagePM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("InsideShipmentPackage with Id " + item.Id + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = item.Id;
					}
					temp.PackageTypeCode = item.Type;
					temp.Quantity = item.Quantity;
					temp.Width = item.Width;
					temp.Length = item.Length;
					temp.Height = item.Height;
					temp.Volume = item.Volume;
					temp.Weight = item.GrossWeight;
					temp.CommodityNumber = item.Commodity;
					temp.Reference1 = item.Reference1;
					temp.Reference2 = item.Reference2;
					temp.Reference3 = item.Reference3;
					temp.Reference4 = item.Reference4;
					temp.Description = item.Description;					   
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