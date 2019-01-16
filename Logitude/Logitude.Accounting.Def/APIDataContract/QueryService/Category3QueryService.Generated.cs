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
   public partial class Category3QueryService
   {
   
		IAccountingContext  context;
		//Category3Service service; 
		
		Logitude.Accounting.BL.EntityQueryServices.Category3QueryService query; 

        public Category3QueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new Category3Service(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.Category3QueryService(tenant);
        }

		
		public Category3 GetCategory3ById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Category3 with Id " + Id + " doesn't exist");

				return Category3DataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Category3 Category3DataMapping(Category3PM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Category3(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.Inactive = MyEntityPM.Inactive;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public Category3PM Category3DataMappingAndValidatin(Category3 MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new Category3PM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("Category3 with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					temp.EnglishName = MyEntity.EnglishName;
					temp.LocalName = MyEntity.LocalName;
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