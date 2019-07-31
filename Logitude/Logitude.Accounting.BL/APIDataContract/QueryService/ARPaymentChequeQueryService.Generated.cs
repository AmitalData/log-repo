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
   public partial class ARPaymentChequeQueryService
   {
   
		IAccountingContext  context;
		//ARPaymentChequeService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.ARPaymentChequeQueryService query; 

        public ARPaymentChequeQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new ARPaymentChequeService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.ARPaymentChequeQueryService(tenant);
        }

		
		public ARPaymentCheque GetARPaymentChequeById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ARPaymentCheque with Id " + Id + " doesn't exist");

				return ARPaymentChequeDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public ARPaymentCheque ARPaymentChequeDataMapping(ARPaymentChequePM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ARPaymentCheque(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.ChequeNumber = MyEntityPM.ChequeNumber;
				   temp.ValueDate = MyEntityPM.ValueDate;
				   temp.LocalAmount = MyEntityPM.LocalAmount;
				   temp.ForeignAmount = MyEntityPM.ForeignAmount;
				   temp.BankName = MyEntityPM.BankName;
				   temp.BankBranch = MyEntityPM.BankBranch;
				   temp.BankAccount = MyEntityPM.BankAccount;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ARPaymentChequePM ARPaymentChequeDataMappingAndValidatin(ARPaymentCheque MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new ARPaymentChequePM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("ARPaymentCheque with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					temp.ChequeNumber = MyEntity.ChequeNumber;
					temp.ValueDate = MyEntity.ValueDate;
					temp.LocalAmount = MyEntity.LocalAmount;
					temp.ForeignAmount = MyEntity.ForeignAmount;
					temp.BankName = MyEntity.BankName;
					temp.BankBranch = MyEntity.BankBranch;
					temp.BankAccount = MyEntity.BankAccount;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}