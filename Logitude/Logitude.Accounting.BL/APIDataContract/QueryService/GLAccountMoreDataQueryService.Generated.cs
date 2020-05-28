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
   public partial class GLAccountMoreDataQueryService
   {
   
		IAccountingContext  context;
		//GLAccountMoreDataService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.GLAccountMoreDataQueryService query; 

        public GLAccountMoreDataQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new GLAccountMoreDataService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.GLAccountMoreDataQueryService(tenant);
        }

		
		public GLAccountMoreData GetGLAccountMoreDataByAccountId(string AccountId,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePMByAccountId(AccountId,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("GLAccountMoreData with AccountId " + AccountId + " doesn't exist");

				return GLAccountMoreDataDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public GLAccountMoreData GLAccountMoreDataDataMapping(GLAccountMoreDataPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new GLAccountMoreData(); 
				   temp.AccountId = MyEntityPM.AccountId;
				   temp.BalanceInLocalCurrency = MyEntityPM.BalanceInLocalCurrency;
				   temp.LocalBalanceInDue = MyEntityPM.LocalBalanceInDue;
				   temp.NextDueDate = MyEntityPM.NextDueDate;
				   temp.TotalOpenChequesInLocalCur = MyEntityPM.TotalOpenChequesInLocalCur;
				   temp.TotFutureOpenChequesInLocalCur = MyEntityPM.TotFutureOpenChequesInLocalCur;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public GLAccountMoreDataPM GLAccountMoreDataDataMappingAndValidatin(GLAccountMoreData MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new GLAccountMoreDataPM();
										if (!string.IsNullOrEmpty(MyEntity.AccountId))
					{
						temp = query.GetSinglePMByAccountId(MyEntity.AccountId, Tenant);
					} 					   
					if(temp == null)
					{
					    throw new ApplicationException("GLAccountMoreData with AccountId " + MyEntity.AccountId + " doesn't exist");
						
					} 
					temp.AccountId = MyEntity.AccountId;
					temp.BalanceInLocalCurrency = MyEntity.BalanceInLocalCurrency;
					temp.LocalBalanceInDue = MyEntity.LocalBalanceInDue;
					temp.NextDueDate = MyEntity.NextDueDate;
					temp.TotalOpenChequesInLocalCur = MyEntity.TotalOpenChequesInLocalCur;
					temp.TotFutureOpenChequesInLocalCur = MyEntity.TotFutureOpenChequesInLocalCur;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}