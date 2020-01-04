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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;

 namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{ 
   public partial class CustomerOpenFilesAmountQueryService
   {
   
		ICommonDataContext  context;
		//CustomerOpenFilesAmountService service; 
		
		CustomerOpenFilesAmountQuery query; 

        public CustomerOpenFilesAmountQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new CustomerOpenFilesAmountService(context, tenant); 
			query = new CustomerOpenFilesAmountQuery(tenant);
        }

		
		public CustomerOpenFilesAmount GetCustomerOpenFilesAmountByCustomerId(string CustomerId,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePMByCustomerId(CustomerId,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("CustomerOpenFilesAmount with CustomerId " + CustomerId + " doesn't exist");

				return CustomerOpenFilesAmountDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public CustomerOpenFilesAmount CustomerOpenFilesAmountDataMapping(CustomerOpenFilesAmountPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new CustomerOpenFilesAmount(); 
				   temp.CustomerId = MyEntityPM.CustomerId;
				   temp.TotalOpenFilesAmount = MyEntityPM.TotalOpenFilesAmount;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.Customer = MyEntityPM.CustomerCode;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public CustomerOpenFilesAmountPM CustomerOpenFilesAmountDataMappingAndValidatin(CustomerOpenFilesAmount MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new CustomerOpenFilesAmountPM();
					if (!string.IsNullOrEmpty(MyEntity.CustomerId))
					{
						temp = query.GetSinglePMByCustomerId(MyEntity.CustomerId, Tenant);
					} 					   
					if(temp == null)
					{   
					    throw new ApplicationException("CustomerOpenFilesAmount with CustomerId " + MyEntity.CustomerId + " doesn't exist");
					} 
					temp.CustomerId = MyEntity.CustomerId;
					temp.TotalOpenFilesAmount = MyEntity.TotalOpenFilesAmount;
					temp.Tenant = MyEntity.Tenant;
					temp.CustomerCode = MyEntity.Customer;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}