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
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel;

 namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{ 
   public partial class ARPaymentChequeStatusReplicaQueryService
   {
   
		ARPaymentChequeStatusReplicaQuery query; 

        public ARPaymentChequeStatusReplicaQueryService(int tenant)
        {
		
			query = new ARPaymentChequeStatusReplicaQuery(tenant);
        }

		
		public ARPaymentChequeStatusReplica GetARPaymentChequeStatusReplicaByCode(string Code,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Code,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ARPaymentChequeStatusReplica with Code " + Code + " doesn't exist");

				return ARPaymentChequeStatusReplicaDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public ARPaymentChequeStatusReplica ARPaymentChequeStatusReplicaDataMapping(ARPaymentChequeStatusReplicaPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ARPaymentChequeStatusReplica(); 
				   temp.Code = MyEntityPM.Code;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.EnglishName = MyEntityPM.EnglishName;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ARPaymentChequeStatusReplicaPM ARPaymentChequeStatusReplicaDataMappingAndValidatin(ARPaymentChequeStatusReplica MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new ARPaymentChequeStatusReplicaPM();
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePM(MyEntity.Code);
					} 					   
					if(temp == null)
					{
					    throw new ApplicationException("ARPaymentChequeStatusReplica with Code " + MyEntity.Code + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Code))
					{
						temp.Code = MyEntity.Code;
					}
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