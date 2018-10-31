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
   public partial class CountryQueryService
   {
   
		ICommonDataContext  context;
		//CountryService service; 
		
		CountryQuery query; 

        public CountryQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new CountryService(context, tenant); 
			query = new CountryQuery(tenant);
        }

		
		public Country GetCountryById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Country with Id " + Id + " doesn't exist");

				return CountryDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Country CountryDataMapping(CountryPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Country(); 
				   temp.Id = MyEntityPM.Id;
				   ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant); 
				   temp.Code = helper.GetComputingPartnerCodeTranslation(MyEntityPM.Code,ComputingPartnerName,"Country");  
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public CountryPM CountryDataMappingAndValidatin(Country MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new CountryPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
						var MyCode = helper.GetLogitudeCodeTranslation(MyEntity.Code,ComputingPartnerName,"Country");
					    if(string.IsNullOrEmpty(MyCode))
						{
						  throw new ApplicationException("Country with Partner Code " + MyEntity.Code + " doesn't match any record");
						}
						temp = query.GetSinglePMByCode(MyCode, Tenant);
						
						
					}
					
					   					   
					if(temp == null)
					{
					    throw new ApplicationException("Country with Id " + MyEntity.Id + " doesn't exist");
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