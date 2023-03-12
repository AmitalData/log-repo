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
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
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
		
		public Customer GetCustomerByCode(string Code,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePMByCode(Code, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Card with Code " + Code + " doesn't exist");

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

			  
				   if(MyEntityPM.AccountManagerUserId != null)
				   {
					   UserQueryService UserService4 = new UserQueryService(Tenant);
					   					   temp.AccountManagerUser = UserService4.GetUserById(MyEntityPM.AccountManagerUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.SalesmanUserId != null)
				   {
					   UserQueryService UserService5 = new UserQueryService(Tenant);
					   					   temp.SalesmanUser = UserService5.GetUserById(MyEntityPM.SalesmanUserId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.CollectorId != null)
				   {
					   UserQueryService UserService6 = new UserQueryService(Tenant);
					   					   temp.Collector = UserService6.GetUserById(MyEntityPM.CollectorId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.TeamId != null)
				   {
					   TeamQueryService TeamService7 = new TeamQueryService(Tenant);
					   					   temp.Team = TeamService7.GetTeamById(MyEntityPM.TeamId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.IndustryId != null)
				   {
					   IndustryQueryService IndustryService8 = new IndustryQueryService(Tenant);
					   					   temp.Industry = IndustryService8.GetIndustryById(MyEntityPM.IndustryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.InvoiceCurrencyId != null)
				   {
					   CurrencyQueryService CurrencyService9 = new CurrencyQueryService(Tenant);
					   					   temp.InvoiceCurrency = CurrencyService9.GetCurrencyById(MyEntityPM.InvoiceCurrencyId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.VatTypeId != null)
				   {
					   VatTypeQueryService VatTypeService10 = new VatTypeQueryService(Tenant);
					   					   temp.VatType = VatTypeService10.GetVatTypeById(MyEntityPM.VatTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.LeadDescription = MyEntityPM.LeadDescription; 

			  
				   if(MyEntityPM.LeadSourceId != null)
				   {
					   LeadSourceQueryService LeadSourceService11 = new LeadSourceQueryService(Tenant);
					   					   temp.LeadSource = LeadSourceService11.GetLeadSourceById(MyEntityPM.LeadSourceId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.PickupDeliveryAddressId != null)
				   {
					   AddressQueryService AddressService12 = new AddressQueryService(Tenant);
					   					   temp.PickupDeliveryAddress = AddressService12.GetAddressById(MyEntityPM.PickupDeliveryAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.ReceivableExternalId = MyEntityPM.ReceivablesAccountingCard; 

				
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Customer");
				temp.CustomFields = customFieldService.CustomFieldCustomDataMapping(MyEntityPM, Tenant);
				 
				    

			  
				   if(MyEntityPM.CustomerSizeId != null)
				   {
					   CustomerSizeQueryService CustomerSizeService14 = new CustomerSizeQueryService(Tenant);
					   					   temp.CustomerSize = CustomerSizeService14.GetCustomerSizeById(MyEntityPM.CustomerSizeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.IsPotential = MyEntityPM.IsPotential; 

			  
				   if(MyEntityPM.PrimaryContactId != null)
				   {
					   ContactQueryService ContactService15 = new ContactQueryService(Tenant);
					   					   temp.PrimaryContact = ContactService15.GetContactById(MyEntityPM.PrimaryContactId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
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
					
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePMByCode(MyEntity.Code, Tenant  );
					} 
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("Card with Code " + MyEntity.Code + " doesn't exist");
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
						ContactQueryService ContactService16 = new ContactQueryService(Tenant);
						  
						if(!IsUpdate)
						{								
							temp.Contacts = ContactService16.ContactCustomDataMappingAndValidatin(MyEntity,MyEntity.Contacts,Tenant,ComputingPartnerName,IsUpdate);

					 
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

					
					UserQueryService AccountManagerUserUserService = new UserQueryService(Tenant);
					if(MyEntity.AccountManagerUser != null)
					{
						var myAccountManagerUserPM = AccountManagerUserUserService.UserDataMappingAndValidatin(MyEntity.AccountManagerUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myAccountManagerUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.AccountManagerUserId = myAccountManagerUserPM.Id;
						  
							}  

							
						} 

					}
			
					
					UserQueryService SalesmanUserUserService = new UserQueryService(Tenant);
					if(MyEntity.SalesmanUser != null)
					{
						var mySalesmanUserPM = SalesmanUserUserService.UserDataMappingAndValidatin(MyEntity.SalesmanUser,Tenant,ComputingPartnerName,IsUpdate);
						
						if(mySalesmanUserPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.SalesmanUserId = mySalesmanUserPM.Id;
						  
							}  

							
						} 

					}
			
					
					UserQueryService CollectorUserService = new UserQueryService(Tenant);
					if(MyEntity.Collector != null)
					{
						var myCollectorPM = CollectorUserService.UserDataMappingAndValidatin(MyEntity.Collector,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCollectorPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CollectorId = myCollectorPM.Id;
						  
							}  

							
						} 

					}
			
					
					TeamQueryService TeamTeamService = new TeamQueryService(Tenant);
					if(MyEntity.Team != null)
					{
						var myTeamPM = TeamTeamService.TeamDataMappingAndValidatin(MyEntity.Team,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myTeamPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.TeamId = myTeamPM.Id;
						  
							}  

							
						} 

					}
			
					
					IndustryQueryService IndustryIndustryService = new IndustryQueryService(Tenant);
					if(MyEntity.Industry != null)
					{
						var myIndustryPM = IndustryIndustryService.IndustryDataMappingAndValidatin(MyEntity.Industry,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myIndustryPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.IndustryId = myIndustryPM.Id;
						  
							}  

							
						} 

					}
			
					
					CurrencyQueryService InvoiceCurrencyCurrencyService = new CurrencyQueryService(Tenant);
					if(MyEntity.InvoiceCurrency != null)
					{
						var myInvoiceCurrencyPM = InvoiceCurrencyCurrencyService.CurrencyDataMappingAndValidatin(MyEntity.InvoiceCurrency,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myInvoiceCurrencyPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.InvoiceCurrencyId = myInvoiceCurrencyPM.Id;
						  
							}  

							
						} 

					}
			
					
					VatTypeQueryService VatTypeVatTypeService = new VatTypeQueryService(Tenant);
					if(MyEntity.VatType != null)
					{
						var myVatTypePM = VatTypeVatTypeService.VatTypeDataMappingAndValidatin(MyEntity.VatType,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myVatTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.VatTypeId = myVatTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.LeadDescription = MyEntity.LeadDescription;

										}  

					
					LeadSourceQueryService LeadSourceLeadSourceService = new LeadSourceQueryService(Tenant);
					if(MyEntity.LeadSource != null)
					{
						var myLeadSourcePM = LeadSourceLeadSourceService.LeadSourceDataMappingAndValidatin(MyEntity.LeadSource,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myLeadSourcePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.LeadSourceId = myLeadSourcePM.Id;
						  
							}  

							
						} 

					}
			
					
					AddressQueryService PickupDeliveryAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.PickupDeliveryAddress != null)
					{
						var myPickupDeliveryAddressPM = PickupDeliveryAddressAddressService.AddressDataMappingAndValidatin(MyEntity.PickupDeliveryAddress,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPickupDeliveryAddressPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PickupDeliveryAddressId = myPickupDeliveryAddressPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.ReceivablesAccountingCard = MyEntity.ReceivableExternalId;

										}  

					
				CustomFieldQueryService customFieldService = new CustomFieldQueryService(Tenant,"Customer");
				if (MyEntity.CustomFields != null)
				{
					 customFieldService.CustomFieldCustomDataMappingAndValidatin(MyEntity.CustomFields, temp, Tenant);
				}		
			
					
					CustomerSizeQueryService CustomerSizeCustomerSizeService = new CustomerSizeQueryService(Tenant);
					if(MyEntity.CustomerSize != null)
					{
						var myCustomerSizePM = CustomerSizeCustomerSizeService.CustomerSizeDataMappingAndValidatin(MyEntity.CustomerSize,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCustomerSizePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.CustomerSizeId = myCustomerSizePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.IsPotential = MyEntity.IsPotential;

										}  

					
					ContactQueryService PrimaryContactContactService = new ContactQueryService(Tenant);
					if(MyEntity.PrimaryContact != null)
					{
						var myPrimaryContactPM = PrimaryContactContactService.ContactDataMappingAndValidatin(MyEntity.PrimaryContact,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPrimaryContactPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.PrimaryContactId = myPrimaryContactPM.Id;
						  
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