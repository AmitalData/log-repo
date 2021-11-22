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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;

 namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{ 
   public partial class PaymentTermQueryService
   {
   
		ICommonDataContext  context;
		//PaymentTermService service; 
		
		PaymentTermQuery query; 

        public PaymentTermQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new PaymentTermService(context, tenant); 
			query = new PaymentTermQuery(tenant);
        }

		
		public PaymentTerm GetPaymentTermById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("PaymentTerm with Id " + Id + " doesn't exist");

				return PaymentTermDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public PaymentTerm GetPaymentTermByExternalId(string ExternalId,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePMByExternalId(ExternalId, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("PaymentTerm with ExternalId " + ExternalId + " doesn't exist");

				return PaymentTermDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public PaymentTerm GetPaymentTermByCode(string Code,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePMByCode(Code, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("PaymentTerm with Code " + Code + " doesn't exist");

				return PaymentTermDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public PaymentTerm PaymentTermDataMapping(PaymentTermPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new PaymentTerm(); 
				   temp.Id = MyEntityPM.Id;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.Days = MyEntityPM.Days;
				   temp.ExternalId = MyEntityPM.ExternalId;
				   temp.Code = MyEntityPM.Code;
				   ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant); 
				   temp.PartnerCode = helper.GetComputingPartnerCodeTranslation(MyEntityPM.Code,ComputingPartnerName,"PaymentTerm");  					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public PaymentTermPM PaymentTermDataMappingAndValidatin(PaymentTerm MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new PaymentTermPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.ExternalId))
					{
						temp = query.GetSinglePMByExternalId(MyEntity.ExternalId, Tenant);
					} 
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
					} 
					if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
					{
                        if(string.IsNullOrEmpty(ComputingPartnerName))
                            throw new ApplicationException("ComputingPartnerCode is required");
						ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
						var MyCode = helper.GetLogitudeCodeTranslation(MyEntity.PartnerCode,ComputingPartnerName,"PaymentTerm");
					    if(string.IsNullOrEmpty(MyCode))
						{
						  throw new ApplicationException("PaymentTerm with Partner Code " + MyEntity.PartnerCode + " doesn't match any record");
						}
						temp = query.GetSinglePMByCode(MyCode, Tenant);
						
						
					}
					
					   					   
					if(temp == null)
					{   
					    throw new ApplicationException("PaymentTerm with Code " + MyEntity.Code + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("PaymentTerm with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.EnglishName))
					{							//throw new ApplicationException("EnglishName Can't be update"); 
							temp.EnglishName = MyEntity.EnglishName;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.LocalName))
					{							//throw new ApplicationException("LocalName Can't be update"); 
							temp.LocalName = MyEntity.LocalName;

										}  

					
                    
					if(!IsUpdate)// && MyEntity.Days != null)
					{							//throw new ApplicationException("Days Can't be update"); 
							temp.Days = MyEntity.Days;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.ExternalId))
					{							//throw new ApplicationException("ExternalId Can't be update"); 
							temp.ExternalId = MyEntity.ExternalId;

										}  

					
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Code))
						{
								//throw new ApplicationException("Code Can't be update"); 
								temp.Code = MyEntity.Code;
								
						
						}  

						
					}
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PartnerCode))
						{
								//throw new ApplicationException("PartnerCode Can't be update"); 
								temp.Code = MyEntity.PartnerCode;
								
						
						}  

						
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