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
   public partial class AddressTypeQueryService
   {
   
		AddressTypeQuery query; 

        public AddressTypeQueryService(int tenant)
        {
		
			query = new AddressTypeQuery(tenant);
        }

		
		public AddressType GetAddressTypeById(string Id,int Tenant,string ComputingPartnerName = "")
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("AddressType with Id " + Id + " doesn't exist");

				return AddressTypeDataMapping(temp,Tenant,ComputingPartnerName);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public AddressType AddressTypeDataMapping(AddressTypePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new AddressType(); 
				   temp.Name = MyEntityPM.Name;
				   temp.Id = MyEntityPM.Id;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public AddressTypePM AddressTypeDataMappingAndValidatin(AddressType MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new AddressTypePM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("AddressType with Id " + MyEntity.Id + " doesn't exist");
					} 
					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Name))
					{							//throw new ApplicationException("Name Can't be update"); 
							temp.Name = MyEntity.Name;

										}  

					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("AddressType with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
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