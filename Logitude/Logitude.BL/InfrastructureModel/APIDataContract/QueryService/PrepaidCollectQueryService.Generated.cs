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
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;

 namespace Logitude.BL.InfrastructureModel.APIDataContract.ApiV1
{ 
   public partial class PrepaidCollectQueryService
   {
   
		PrepaidCollectQuery query; 

        public PrepaidCollectQueryService(int tenant)
        {
		
			query = new PrepaidCollectQuery(tenant);
        }

		
		public PrepaidCollect GetPrepaidCollectById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("PrepaidCollect with Id " + Id + " doesn't exist");

				return PrepaidCollectDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public PrepaidCollect PrepaidCollectDataMapping(PrepaidCollectPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new PrepaidCollect(); 
				   temp.Code = MyEntityPM.Id;
				   temp.Name = MyEntityPM.Name;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public PrepaidCollectPM PrepaidCollectDataMappingAndValidatin(PrepaidCollect MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new PrepaidCollectPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePM(MyEntity.Code);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("PrepaidCollect with Code " + MyEntity.Code + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Code))
					    {
					        throw new ApplicationException("PrepaidCollect with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Code;

						//}
					}
					temp.Name = MyEntity.Name;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}