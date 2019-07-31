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
   public partial class JournalActionTypeQueryService
   {
   
		IAccountingContext  context;
		//JournalActionTypeService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.JournalActionTypeQueryService query; 

        public JournalActionTypeQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new JournalActionTypeService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.JournalActionTypeQueryService(tenant);
        }

		
		public JournalActionType GetJournalActionTypeById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("JournalActionType with Id " + Id + " doesn't exist");

				return JournalActionTypeDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public JournalActionType JournalActionTypeDataMapping(JournalActionTypePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new JournalActionType(); 
				   temp.Id = MyEntityPM.Id;
				   temp.LogitudeCode = MyEntityPM.Code;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.EnglishName = MyEntityPM.EnglishName;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public JournalActionTypePM JournalActionTypeDataMappingAndValidatin(JournalActionType MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new JournalActionTypePM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("JournalActionType with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					if(string.IsNullOrEmpty(temp.Code))
					{
						temp.Code = MyEntity.LogitudeCode;
					}
					temp.Tenant = MyEntity.Tenant;
					temp.LocalName = MyEntity.LocalName;
					temp.EnglishName = MyEntity.EnglishName;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}