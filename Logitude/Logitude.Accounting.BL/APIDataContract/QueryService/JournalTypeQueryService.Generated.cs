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
   public partial class JournalTypeQueryService
   {
   
		Logitude.Accounting.BL.EntityQueryServices.JournalTypeQueryService query; 

        public JournalTypeQueryService(int tenant)
        {
		
			query = new Logitude.Accounting.BL.EntityQueryServices.JournalTypeQueryService(tenant);
        }

		
		public JournalType GetJournalTypeByJournalTypeID(string JournalTypeID,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(JournalTypeID,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("JournalType with JournalTypeID " + JournalTypeID + " doesn't exist");

				return JournalTypeDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public JournalType JournalTypeDataMapping(JournalTypePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new JournalType(); 
				   temp.Code = MyEntityPM.JournalTypeID;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public JournalTypePM JournalTypeDataMappingAndValidatin(JournalType MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new JournalTypePM();
										if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePM(MyEntity.Code);
					} 					   
					if(temp == null)
					{
					    throw new ApplicationException("JournalType with Code " + MyEntity.Code + " doesn't exist");
						
					} 
					temp.JournalTypeID = MyEntity.Code;
					temp.EnglishName = MyEntity.EnglishName;
					temp.LocalName = MyEntity.LocalName;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}