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
   public partial class APInvoiceTotalVATQueryService
   {
   
		APInvoiceTotalVATQuery query; 

        public APInvoiceTotalVATQueryService(int tenant)
        {
		
			query = new APInvoiceTotalVATQuery(tenant);
        }

		
		public List<APInvoiceTotalVAT> APInvoiceTotalVATDataMapping(List<APInvoiceTotalVATPM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<APInvoiceTotalVAT>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new APInvoiceTotalVAT(); 
				   temp.Id = item.Id; 

			  
				   if(item.VatTypeId != null)
				   {
					   VatTypeQueryService VatTypeService0 = new VatTypeQueryService(Tenant);
					   					   temp.VatType = VatTypeService0.VatTypeCustomDataMapping(item.VatTypeId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.VatPercent = item.VatPercent;
				   temp.InvoiceCurrencyVATAmount = item.InvoiceCurrencyVATAmount;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<APInvoiceTotalVATPM> APInvoiceTotalVATDataMappingAndValidatin(List<APInvoiceTotalVAT> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<APInvoiceTotalVATPM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new APInvoiceTotalVATPM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("APInvoiceTotalVAT with Id " + item.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("APInvoiceTotalVAT with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
					}
					VatTypeQueryService VatTypeVatTypeService = new VatTypeQueryService(Tenant);
					if(item.VatType != null)
					{
						var myVatTypePM = VatTypeVatTypeService.VatTypeCustomDataMappingAndValidatin(item.VatType,Tenant);
						
						if(myVatTypePM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.VatTypeId = myVatTypePM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.VatPercent = item.VatPercent;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.InvoiceCurrencyVATAmount = item.InvoiceCurrencyVATAmount;

										}  

										   
						MyList.Add(temp);
					}
						
					return MyList;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}