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
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel;

 namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{ 
   public partial class APInvoiceTransferStatusQueryService
   {
   
		APInvoiceTransferStatusQuery query; 

        public APInvoiceTransferStatusQueryService(int tenant)
        {
		
			query = new APInvoiceTransferStatusQuery(tenant);
        }

		
		public APInvoiceTransferStatus GetAPInvoiceTransferStatusByCode(string Code,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Code, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("APInvoiceTransferStatus with Code " + Code + " doesn't exist");

				return APInvoiceTransferStatusDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public APInvoiceTransferStatus APInvoiceTransferStatusDataMapping(APInvoiceTransferStatusPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new APInvoiceTransferStatus(); 
				   temp.Code = MyEntityPM.Code;
				   temp.Name = MyEntityPM.Name;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public APInvoiceTransferStatusPM APInvoiceTransferStatusDataMappingAndValidatin(APInvoiceTransferStatus MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new APInvoiceTransferStatusPM();
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePM(MyEntity.Code);
					} 
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("APInvoiceTransferStatus with Code " + MyEntity.Code + " doesn't exist");
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