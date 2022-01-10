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
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;

 namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{ 
   public partial class ChargesGroupQueryService
   {
   
		IWebFreightContext  context;
		//ChargesGroupService service; 
		
		ChargesGroupQuery query; 

        public ChargesGroupQueryService(int tenant)
        {
				    context = WebFreightContext.GetContext(tenant); 
			//service = new ChargesGroupService(context, tenant); 
			query = new ChargesGroupQuery(tenant);
        }

		
		public ChargesGroup GetChargesGroupById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ChargesGroup with Id " + Id + " doesn't exist");

				return ChargesGroupDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public ChargesGroup ChargesGroupDataMapping(ChargesGroupPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ChargesGroup(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Code = MyEntityPM.Code;
				   temp.Name = MyEntityPM.Name;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ChargesGroupPM ChargesGroupDataMappingAndValidatin(ChargesGroup MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new ChargesGroupPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("ChargesGroup with Id " + MyEntity.Id + " doesn't exist");
					} 
				 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("ChargesGroup with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
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
						temp.Name = MyEntity.Name;

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