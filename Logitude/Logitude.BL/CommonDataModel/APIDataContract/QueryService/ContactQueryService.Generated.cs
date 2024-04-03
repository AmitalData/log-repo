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
   public partial class ContactQueryService
   {
   
		ICommonDataContext  context;
		//ContactService service; 
		
		ContactQuery query; 

        public ContactQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new ContactService(context, tenant); 
			query = new ContactQuery(tenant);
        }

		
		public Contact GetContactById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Contact with Id " + Id + " doesn't exist");

				return ContactDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Contact GetContactByEmail(string Email,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePMByEmail(Email, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Contact with Email " + Email + " doesn't exist");

				return ContactDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Contact ContactDataMapping(ContactPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Contact(); 
				   temp.Id = MyEntityPM.Id;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.Code = MyEntityPM.ExternalId;
				   temp.Email = MyEntityPM.Email;
				   temp.Position = MyEntityPM.Position;
				   temp.BusinessPhone = MyEntityPM.BusinessPhone;
				   temp.Mobile = MyEntityPM.Mobile;
				   temp.IsPrimaryContact = MyEntityPM.SetAsPrimaryForCard;
				   temp.InActive = MyEntityPM.InActive;
				   temp.Notes = MyEntityPM.Notes;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ContactPM ContactDataMappingAndValidatin(Contact MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new ContactPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.Email))
					{
						temp = query.GetSinglePMByEmail(MyEntity.Email, Tenant  );
					} 
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("Contact with Email " + MyEntity.Email + " doesn't exist");
					} 
				 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Contact with provided key doesn't exist");
						
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
						temp.ExternalId = MyEntity.Code;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Email = MyEntity.Email;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Position = MyEntity.Position;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.BusinessPhone = MyEntity.BusinessPhone;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Mobile = MyEntity.Mobile;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SetAsPrimaryForCard = MyEntity.IsPrimaryContact;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.InActive = MyEntity.InActive;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.Notes = MyEntity.Notes;

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