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

		
		public Address GetAddressById(string Id,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Address with Id " + Id + " doesn't exist");

				return AddressDataMapping(temp,Tenant,ComputingPartnerName);
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
					   					   temp.Country = CountryService0.GetCountryById(MyEntityPM.CountryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.City = MyEntityPM.City;
				   temp.ZipCode = MyEntityPM.ZipCode;
				   temp.PhoneNumber = MyEntityPM.PhoneNumber;
				   temp.FaxNumber = MyEntityPM.FaxNumber; 

			  
				   if(MyEntityPM.StateId != null)
				   {
					   StateQueryService StateService1 = new StateQueryService(Tenant);
					   					   temp.State = StateService1.GetStateById(MyEntityPM.StateId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ExternalId = MyEntityPM.ExternalId; 

			  
				   if(MyEntityPM.AddressTypeId != null)
				   {
					   AddressTypeQueryService AddressTypeService2 = new AddressTypeQueryService(Tenant);
					   					   temp.AddressType = AddressTypeService2.GetAddressTypeById(MyEntityPM.AddressTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public AddressPM AddressDataMappingAndValidatin(Address MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Address with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Name))
					{							//throw new ApplicationException("Name Can't be update"); 
							temp.Name = MyEntity.Name;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Address1))
					{							//throw new ApplicationException("Address1 Can't be update"); 
							temp.Address1 = MyEntity.Address1;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Address2))
					{							//throw new ApplicationException("Address2 Can't be update"); 
							temp.Address2 = MyEntity.Address2;

										}  

					
					CountryQueryService CountryCountryService = new CountryQueryService(Tenant);
					if(MyEntity.Country != null)
					{
						var myCountryPM = CountryCountryService.CountryDataMappingAndValidatin(MyEntity.Country,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCountryPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Country Can't be update"); 
								temp.CountryId = myCountryPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.City))
					{							//throw new ApplicationException("City Can't be update"); 
							temp.City = MyEntity.City;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ZipCode))
					{							//throw new ApplicationException("ZipCode Can't be update"); 
							temp.ZipCode = MyEntity.ZipCode;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PhoneNumber))
					{							//throw new ApplicationException("PhoneNumber Can't be update"); 
							temp.PhoneNumber = MyEntity.PhoneNumber;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.FaxNumber))
					{							//throw new ApplicationException("FaxNumber Can't be update"); 
							temp.FaxNumber = MyEntity.FaxNumber;

										}  

					
					StateQueryService StateStateService = new StateQueryService(Tenant);
					if(MyEntity.State != null)
					{
						var myStatePM = StateStateService.StateDataMappingAndValidatin(MyEntity.State,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("State Can't be update"); 
								temp.StateId = myStatePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ExternalId))
					{							//throw new ApplicationException("ExternalId Can't be update"); 
							temp.ExternalId = MyEntity.ExternalId;

										}  

					
					AddressTypeQueryService AddressTypeAddressTypeService = new AddressTypeQueryService(Tenant);
					if(MyEntity.AddressType != null)
					{
						var myAddressTypePM = AddressTypeAddressTypeService.AddressTypeDataMappingAndValidatin(MyEntity.AddressType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAddressTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("AddressType Can't be update"); 
								temp.AddressTypeId = myAddressTypePM.Id;
						  
							}  

							
						} 

					}
			
										   
					return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}