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
   public partial class CustomerQueryService
   {
   
		ICommonDataContext  context;
		//CardService service; 
		
		CardQuery query; 

        public CustomerQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new CardService(context, tenant); 
			query = new CardQuery(tenant);
        }

		
		public Customer GetCustomerById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Card with Id " + Id + " doesn't exist");

				return CustomerDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Customer CustomerDataMapping(CardPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Customer(); 
				   temp.Id = MyEntityPM.Id;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.VatNumber = MyEntityPM.VatNumber;			  
				   if(MyEntityPM.PaymentTermId != null)
				   {
					   PaymentTermQueryService PaymentTermService0 = new PaymentTermQueryService(Tenant);
					   					   temp.PaymentTerm = PaymentTermService0.GetPaymentTermById(MyEntityPM.PaymentTermId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.MainAddressId != null)
				   {
					   AddressQueryService AddressService1 = new AddressQueryService(Tenant);
					   					   temp.MainAddress = AddressService1.GetAddressById(MyEntityPM.MainAddressId,Tenant); 
			       
					   				   }
				   
				   ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant); 
				   temp.PartnerCode = helper.GetComputingPartnerCodeTranslation(MyEntityPM.Code,ComputingPartnerName,"Card");  
				if(MyEntityPM.Contacts != null && MyEntityPM.Contacts.Count > 0)
				{
					 ContactQueryService ContactService2 = new ContactQueryService(Tenant);
					 temp.Contacts = ContactService2.ContactCustomDataMapping(MyEntityPM,MyEntityPM.Contacts,Tenant);
				}

							 					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public CardPM CustomerDataMappingAndValidatin(Customer MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new CardPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
					{
						ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
						var MyCode = helper.GetLogitudeCodeTranslation(MyEntity.PartnerCode,ComputingPartnerName,"Card");
					    if(string.IsNullOrEmpty(MyCode))
						{
						  throw new ApplicationException("Card with Partner Code " + MyEntity.PartnerCode + " doesn't match any record");
						}
						temp = query.GetSinglePMByCode(MyCode, Tenant);
						
						
					}
					
					   					   
					if(temp == null)
					{
					    throw new ApplicationException("Card with Id " + MyEntity.Id + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.EnglishName = MyEntity.EnglishName;
					temp.LocalName = MyEntity.LocalName;
					temp.VatNumber = MyEntity.VatNumber;					PaymentTermQueryService PaymentTermPaymentTermService = new PaymentTermQueryService(Tenant);
					if(MyEntity.PaymentTerm != null)
					{
						var myPaymentTermPM = PaymentTermPaymentTermService.PaymentTermDataMappingAndValidatin(MyEntity.PaymentTerm,Tenant,ComputingPartnerName);
												if(myPaymentTermPM != null)
						{
							temp.PaymentTermId = myPaymentTermPM.Id;
						}
						 
					}
			
										AddressQueryService MainAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.MainAddress != null)
					{
						var myMainAddressPM = MainAddressAddressService.AddressDataMappingAndValidatin(MyEntity.MainAddress,Tenant,ComputingPartnerName);
												if(myMainAddressPM != null)
						{
							temp.MainAddressId = myMainAddressPM.Id;
						}
						 
					}
			
					
					if(string.IsNullOrEmpty(temp.Code))
					{
						temp.Code = MyEntity.PartnerCode;
					}
					if(MyEntity.Contacts != null && MyEntity.Contacts.Count > 0)
					{
						ContactQueryService ContactService2 = new ContactQueryService(Tenant);
						temp.Contacts = ContactService2.ContactCustomDataMappingAndValidatin(MyEntity,MyEntity.Contacts,Tenant,ComputingPartnerName);
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