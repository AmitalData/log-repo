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
				   
				if(MyEntityPM.Contacts != null && MyEntityPM.Contacts.Count > 0)
				{
					 ContactQueryService ContactService2 = new ContactQueryService(Tenant);
					 temp.Contacts = ContactService2.ContactCustomDataMapping(MyEntityPM,MyEntityPM.Contacts,Tenant);
				}

							 			  
				   if(MyEntityPM.BillingAddressId != null)
				   {
					   AddressQueryService AddressService2 = new AddressQueryService(Tenant);
					   					   temp.BillingAddress = AddressService2.GetAddressById(MyEntityPM.BillingAddressId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.GLAccountId != null)
				   {
					   GLAccountQueryService GLAccountService3 = new GLAccountQueryService(Tenant);
					   					   temp.GLAccount = GLAccountService3.GLAccountCustomDataMapping(MyEntityPM.GLAccountId,Tenant); 
			       
					   				   }
				   
				   temp.Code = MyEntityPM.Code;
				   temp.PartnerCode = MyEntityPM.PartnerCode;					
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
										   
					if(temp == null)
					{   
					    throw new ApplicationException("Card with Id " + MyEntity.Id + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Card with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//}
					}
					temp.EnglishName = MyEntity.EnglishName;
					temp.LocalName = MyEntity.LocalName;
					temp.VatNumber = MyEntity.VatNumber;
					PaymentTermQueryService PaymentTermPaymentTermService = new PaymentTermQueryService(Tenant);
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
			
					 

					if(MyEntity.Contacts != null && MyEntity.Contacts.Count > 0)
					{
						ContactQueryService ContactService4 = new ContactQueryService(Tenant);
						temp.Contacts = ContactService4.ContactCustomDataMappingAndValidatin(MyEntity,MyEntity.Contacts,Tenant,ComputingPartnerName);
					}

								 
					AddressQueryService BillingAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.BillingAddress != null)
					{
						var myBillingAddressPM = BillingAddressAddressService.AddressDataMappingAndValidatin(MyEntity.BillingAddress,Tenant,ComputingPartnerName);
												if(myBillingAddressPM != null)
						{
							temp.BillingAddressId = myBillingAddressPM.Id;
						}
						 
					}
			
					
					GLAccountQueryService GLAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(MyEntity.GLAccount != null)
					{
						var myGLAccountPM = GLAccountGLAccountService.GLAccountCustomDataMappingAndValidatin(MyEntity.GLAccount,Tenant);
												if(myGLAccountPM != null)
						{
							temp.GLAccountId = myGLAccountPM.Id;
						}
						 
					}
			
					
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						temp.Code = MyEntity.Code;
					}
					temp.PartnerCode = MyEntity.PartnerCode;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}