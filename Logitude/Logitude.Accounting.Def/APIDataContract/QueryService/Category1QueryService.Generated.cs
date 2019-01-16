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
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;

 namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{ 
   public partial class Category1QueryService
   {
   
		IAccountingContext  context;
		//Category1Service service; 
		
		Logitude.Accounting.BL.EntityQueryServices.Category1QueryService query; 

        public Category1QueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new Category1Service(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.Category1QueryService(tenant);
        }

		
		public Category1 GetCategory1ById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Category1 with Id " + Id + " doesn't exist");

				return Category1DataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Category1 Category1DataMapping(Category1PM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Category1(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.Inactive = MyEntityPM.Inactive;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public Category1PM Category1DataMappingAndValidatin(Category1 MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new Category1PM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("Category1 with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					temp.LocalName = MyEntity.LocalName;
					temp.EnglishName = MyEntity.EnglishName;
					temp.Inactive = MyEntity.Inactive;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}