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
   public partial class VendorQueryService
   {
   
		ICommonDataContext  context;
		//CardService service; 
		
		CardQuery query; 

        public VendorQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new CardService(context, tenant); 
			query = new CardQuery(tenant);
        }

		
		public Vendor GetVendorById(string Id,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Card with Id " + Id + " doesn't exist");

				return VendorDataMapping(temp,Tenant,ComputingPartnerName);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Vendor GetVendorByCode(string Code,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePMByCode(Code,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Card with Code " + Code + " doesn't exist");

				return VendorDataMapping(temp,Tenant,ComputingPartnerName);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public Vendor VendorDataMapping(CardPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Vendor(); 
				   temp.Id = MyEntityPM.Id;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.VatNumber = MyEntityPM.VatNumber;
				   temp.CreateDate = MyEntityPM.CreateDate; 

			  
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
				    

			  
				   if(MyEntityPM.GLAccountId != null)
				   {
					   GLAccountQueryService GLAccountService2 = new GLAccountQueryService(Tenant);
					   					   temp.GLAccount = GLAccountService2.GLAccountCustomDataMapping(MyEntityPM.GLAccountId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.Code = MyEntityPM.Code;
				   ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant); 
				   temp.PartnerCode = helper.GetComputingPartnerCodeTranslation(MyEntityPM.Code,ComputingPartnerName,"Card");   

			  
				   if(MyEntityPM.BillingAddressId != null)
				   {
					   AddressQueryService AddressService3 = new AddressQueryService(Tenant);
					   					   temp.BillingAddress = AddressService3.GetAddressById(MyEntityPM.BillingAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public CardPM VendorDataMappingAndValidatin(Vendor MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
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
						temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
					} 
					if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
					{
                        if(string.IsNullOrEmpty(ComputingPartnerName))
                            throw new ApplicationException("ComputingPartnerCode is required");
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
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.EnglishName))
					{							//throw new ApplicationException("EnglishName Can't be update"); 
							temp.EnglishName = MyEntity.EnglishName;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.LocalName))
					{							//throw new ApplicationException("LocalName Can't be update"); 
							temp.LocalName = MyEntity.LocalName;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.VatNumber))
					{							//throw new ApplicationException("VatNumber Can't be update"); 
							temp.VatNumber = MyEntity.VatNumber;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.CreateDate != null)
					{							//throw new ApplicationException("CreateDate Can't be update"); 
							temp.CreateDate = MyEntity.CreateDate;

										}  

					
					PaymentTermQueryService PaymentTermPaymentTermService = new PaymentTermQueryService(Tenant);
					if(MyEntity.PaymentTerm != null)
					{
						var myPaymentTermPM = PaymentTermPaymentTermService.PaymentTermDataMappingAndValidatin(MyEntity.PaymentTerm,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myPaymentTermPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("PaymentTerm Can't be update"); 
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
							{								//throw new ApplicationException("MainAddress Can't be update"); 
								temp.MainAddressId = myMainAddressPM.Id;
						  
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
							{								//throw new ApplicationException("GLAccount Can't be update"); 
								temp.GLAccountId = myGLAccountPM.Id;
						  
							}  

							
						} 

					}
			
					
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Code))
						{
								//throw new ApplicationException("Code Can't be update"); 
								temp.Code = MyEntity.Code;
								
						
						}  

						
					}
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PartnerCode))
						{
								//throw new ApplicationException("PartnerCode Can't be update"); 
								temp.Code = MyEntity.PartnerCode;
								
						
						}  

						
					}
					AddressQueryService BillingAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.BillingAddress != null)
					{
						var myBillingAddressPM = BillingAddressAddressService.AddressDataMappingAndValidatin(MyEntity.BillingAddress,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myBillingAddressPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("BillingAddress Can't be update"); 
								temp.BillingAddressId = myBillingAddressPM.Id;
						  
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