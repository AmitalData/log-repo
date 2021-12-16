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

		
		public Customer GetCustomerById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Card with Id " + Id + " doesn't exist");

				return CustomerDataMapping(temp,Tenant,ComputingPartnerName);
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
					   					   temp.PaymentTerm = PaymentTermService0.GetPaymentTermById(MyEntityPM.PaymentTermId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.MainAddressId != null)
				   {
					   AddressQueryService AddressService1 = new AddressQueryService(Tenant);
					   					   temp.MainAddress = AddressService1.GetAddressById(MyEntityPM.MainAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				if(MyEntityPM.Contacts != null && MyEntityPM.Contacts.Count > 0)
				{
					 ContactQueryService ContactService2 = new ContactQueryService(Tenant);
					 temp.Contacts = ContactService2.ContactCustomDataMapping(MyEntityPM,MyEntityPM.Contacts,Tenant,ComputingPartnerName);
				}

							  

			  
				   if(MyEntityPM.BillingAddressId != null)
				   {
					   AddressQueryService AddressService2 = new AddressQueryService(Tenant);
					   					   temp.BillingAddress = AddressService2.GetAddressById(MyEntityPM.BillingAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.GLAccountId != null)
				   {
					   GLAccountQueryService GLAccountService3 = new GLAccountQueryService(Tenant);
					   					   temp.GLAccount = GLAccountService3.GLAccountCustomDataMapping(MyEntityPM.GLAccountId,Tenant,ComputingPartnerName); 
			       
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

		public CardPM CustomerDataMappingAndValidatin(Customer MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
                    
					if(!IsUpdate)
					{							
						temp.EnglishName = MyEntity.EnglishName;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.LocalName = MyEntity.LocalName;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.VatNumber = MyEntity.VatNumber;

										}  

					
					PaymentTermQueryService PaymentTermPaymentTermService = new PaymentTermQueryService(Tenant);
					if(MyEntity.PaymentTerm != null)
					{
						var myPaymentTermPM = PaymentTermPaymentTermService.PaymentTermDataMappingAndValidatin(MyEntity.PaymentTerm,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPaymentTermPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PaymentTermId = myPaymentTermPM.Id;
						  
							}  

							
						} 

					}
			
					
					AddressQueryService MainAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.MainAddress != null)
					{
						var myMainAddressPM = MainAddressAddressService.AddressDataMappingAndValidatin(MyEntity.MainAddress,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainAddressPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.MainAddressId = myMainAddressPM.Id;
						  
							}  

							
						} 

					}
			
					 

					if(MyEntity.Contacts != null && MyEntity.Contacts.Count > 0)
					{
						ContactQueryService ContactService4 = new ContactQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.Contacts = ContactService4.ContactCustomDataMappingAndValidatin(MyEntity,MyEntity.Contacts,Tenant,ComputingPartnerName,IsUpdate);

					 
						}  

						
					}

								 
					AddressQueryService BillingAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.BillingAddress != null)
					{
						var myBillingAddressPM = BillingAddressAddressService.AddressDataMappingAndValidatin(MyEntity.BillingAddress,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myBillingAddressPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.BillingAddressId = myBillingAddressPM.Id;
						  
							}  

							
						} 

					}
			
					
					GLAccountQueryService GLAccountGLAccountService = new GLAccountQueryService(Tenant);
					if(MyEntity.GLAccount != null)
					{
						var myGLAccountPM = GLAccountGLAccountService.GLAccountCustomDataMappingAndValidatin(MyEntity.GLAccount,Tenant);
						
						if(myGLAccountPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.GLAccountId = myGLAccountPM.Id;
						  
							}  

							
						} 

					}
			
					
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Code))
						{								
							temp.Code = MyEntity.Code;
								
						
						}  

						
					}
                    
					if(!IsUpdate)
					{							
						temp.PartnerCode = MyEntity.PartnerCode;

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