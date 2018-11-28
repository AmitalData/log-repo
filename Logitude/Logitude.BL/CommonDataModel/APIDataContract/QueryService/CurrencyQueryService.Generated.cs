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
   public partial class CurrencyQueryService
   {
   
		ICommonDataContext  context;
		//CurrencyService service; 
		
		CurrencyQuery query; 

        public CurrencyQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new CurrencyService(context, tenant); 
			query = new CurrencyQuery(tenant);
        }

		
		public Currency GetCurrencyById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Currency with Id " + Id + " doesn't exist");

				return CurrencyDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Currency GetCurrencyByCode(string Code,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePMByCode(Code,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Currency with Code " + Code + " doesn't exist");

				return CurrencyDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Currency CurrencyDataMapping(CurrencyPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Currency(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Code = MyEntityPM.Code;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public CurrencyPM CurrencyDataMappingAndValidatin(Currency MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new CurrencyPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
					} 					   
					if(temp == null)
					{
					    throw new ApplicationException("Currency with Code " + MyEntity.Code + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					if(string.IsNullOrEmpty(temp.Code))
					{
						temp.Code = MyEntity.Code;
					}
					temp.EnglishName = MyEntity.EnglishName;
					temp.LocalName = MyEntity.LocalName;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}