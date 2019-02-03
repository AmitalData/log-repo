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
   public partial class AddressQueryService
   {
   
		ICommonDataContext  context;
		//AddressService service; 
		
		AddressQuery query; 

        public AddressQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new AddressService(context, tenant); 
			query = new AddressQuery(tenant);
        }

		
		public Address GetAddressById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Address with Id " + Id + " doesn't exist");

				return AddressDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Address AddressDataMapping(AddressPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Address(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Name = MyEntityPM.Name;
				   temp.Address1 = MyEntityPM.Address1;
				   temp.Address2 = MyEntityPM.Address2;			  
				   if(MyEntityPM.CountryId != null)
				   {
					   CountryQueryService CountryService0 = new CountryQueryService(Tenant);
					   					   temp.Country = CountryService0.GetCountryById(MyEntityPM.CountryId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.City != null)
				   {
					   CityQueryService CityService1 = new CityQueryService(Tenant);
					   					   temp.City = CityService1.GetCityById(MyEntityPM.City,Tenant); 
			       
					   				   }
				   
				   temp.ZipCode = MyEntityPM.ZipCode;
				   temp.PhoneNumber = MyEntityPM.PhoneNumber;
				   temp.FaxNumber = MyEntityPM.FaxNumber;			  
				   if(MyEntityPM.StateId != null)
				   {
					   StateQueryService StateService2 = new StateQueryService(Tenant);
					   					   temp.State = StateService2.GetStateById(MyEntityPM.StateId,Tenant); 
			       
					   				   }
				   
				   temp.ExternalId = MyEntityPM.ExternalId;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public AddressPM AddressDataMappingAndValidatin(Address MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new AddressPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("Address with Id " + MyEntity.Id + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Name = MyEntity.Name;
					temp.Address1 = MyEntity.Address1;
					temp.Address2 = MyEntity.Address2;					CountryQueryService CountryCountryService = new CountryQueryService(Tenant);
					if(MyEntity.Country != null)
					{
						var myCountryPM = CountryCountryService.CountryDataMappingAndValidatin(MyEntity.Country,Tenant,ComputingPartnerName);
												if(myCountryPM != null)
						{
							temp.CountryId = myCountryPM.Id;
						}
						 
					}
			
										CityQueryService CityCityService = new CityQueryService(Tenant);
					if(MyEntity.City != null)
					{
						var myCityPM = CityCityService.CityDataMappingAndValidatin(MyEntity.City,Tenant,ComputingPartnerName);
												if(myCityPM != null)
						{
							temp.City = myCityPM.Id;
						}
						 
					}
			
					
					temp.ZipCode = MyEntity.ZipCode;
					temp.PhoneNumber = MyEntity.PhoneNumber;
					temp.FaxNumber = MyEntity.FaxNumber;					StateQueryService StateStateService = new StateQueryService(Tenant);
					if(MyEntity.State != null)
					{
						var myStatePM = StateStateService.StateDataMappingAndValidatin(MyEntity.State,Tenant,ComputingPartnerName);
												if(myStatePM != null)
						{
							temp.StateId = myStatePM.Id;
						}
						 
					}
			
					
					temp.ExternalId = MyEntity.ExternalId;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}