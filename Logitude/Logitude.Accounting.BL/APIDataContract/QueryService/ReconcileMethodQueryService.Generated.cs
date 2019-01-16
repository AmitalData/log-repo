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
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;

 namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{ 
   public partial class ReconcileMethodQueryService
   {
   
		Logitude.Accounting.BL.EntityQueryServices.ReconcileMethodQueryService query; 

        public ReconcileMethodQueryService(int tenant)
        {
		
			query = new Logitude.Accounting.BL.EntityQueryServices.ReconcileMethodQueryService(tenant);
        }

		
		public ReconcileMethod GetReconcileMethodByCode(string Code,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Code,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ReconcileMethod with Code " + Code + " doesn't exist");

				return ReconcileMethodDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public ReconcileMethod ReconcileMethodDataMapping(ReconcileMethodPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ReconcileMethod(); 
				   temp.Code = MyEntityPM.Code;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.Inactive = MyEntityPM.Inactive;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ReconcileMethodPM ReconcileMethodDataMappingAndValidatin(ReconcileMethod MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new ReconcileMethodPM();
										if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePM(MyEntity.Code);
					} 					   
					if(temp == null)
					{
					    throw new ApplicationException("ReconcileMethod with Code " + MyEntity.Code + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Code))
					{
						temp.Code = MyEntity.Code;
					}
					temp.EnglishName = MyEntity.EnglishName;
					temp.LocalName = MyEntity.LocalName;
					temp.Inactive = MyEntity.Inactive;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}