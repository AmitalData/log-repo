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
   public partial class PaymentTermQueryService
   {
   
		ICommonDataContext  context;
		//PaymentTermService service; 
		
		PaymentTermQuery query; 

        public PaymentTermQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new PaymentTermService(context, tenant); 
			query = new PaymentTermQuery(tenant);
        }

		
		public PaymentTerm GetPaymentTermById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("PaymentTerm with Id " + Id + " doesn't exist");

				return PaymentTermDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public PaymentTerm PaymentTermDataMapping(PaymentTermPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new PaymentTerm(); 
				   temp.Id = MyEntityPM.Id;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.Days = MyEntityPM.Days;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public PaymentTermPM PaymentTermDataMappingAndValidatin(PaymentTerm MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new PaymentTermPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("PaymentTerm with Id " + MyEntity.Id + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.EnglishName = MyEntity.EnglishName;
					temp.LocalName = MyEntity.LocalName;
					temp.Days = MyEntity.Days;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}