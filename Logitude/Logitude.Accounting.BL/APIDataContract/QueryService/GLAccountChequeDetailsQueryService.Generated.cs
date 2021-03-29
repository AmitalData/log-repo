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
   public partial class GLAccountChequeDetailsQueryService
   {
   
		IAccountingContext  context;
		//GLAccountService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService query; 

        public GLAccountChequeDetailsQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new GLAccountService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.GLAccountQueryService(tenant);
        }

		
		public GLAccountChequeDetails GetGLAccountChequeDetailsById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("GLAccount with Id " + Id + " doesn't exist");

				return GLAccountChequeDetailsDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public GLAccountChequeDetails GLAccountChequeDetailsDataMapping(GLAccountPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new GLAccountChequeDetails(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.TotalOpenChequesInLocalCur = MyEntityPM.TotalOpenChequesInLocalCur;
				   temp.TotFutureOpenChequesInLocalCur = MyEntityPM.TotFutureOpenChequesInLocalCur;
				   temp.DisplayNumber = MyEntityPM.DisplayNumber;
				   temp.LocalName = MyEntityPM.LocalName;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public GLAccountPM GLAccountChequeDetailsDataMappingAndValidatin(GLAccountChequeDetails MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new GLAccountPM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("GLAccount with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					temp.TotalOpenChequesInLocalCur = MyEntity.TotalOpenChequesInLocalCur;
					temp.TotFutureOpenChequesInLocalCur = MyEntity.TotFutureOpenChequesInLocalCur;
					temp.DisplayNumber = MyEntity.DisplayNumber;
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
