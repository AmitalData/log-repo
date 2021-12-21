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
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.QuoteModel;

 namespace Logitude.BL.QuoteModel.APIDataContract.ApiV1
{ 
   public partial class QuotePriceStepsQueryService
   {
   
		QuotePriceStepsQuery query; 

        public QuotePriceStepsQueryService(int tenant)
        {
		
			query = new QuotePriceStepsQuery(tenant);
        }

		
		public List<QuotePriceSteps> QuotePriceStepsDataMapping(List<QuotePriceStepsPM> MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				var MyList = new List<QuotePriceSteps>();
				foreach (var item in MyEntityPM)
				{
				   
				   var temp = new QuotePriceSteps(); 
				   temp.Id = item.Id;
				   temp.PriceBreakStep = item.Step;
				   temp.CostUnitPrice = item.CostUnitPrice;
				   temp.SaleUnitPrice = item.SaleUnitPrice;
				   temp.MeasurementUnit = item.MeasurementUnit;					
					MyList.Add(temp);
				}
					
				   return MyList;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public List<QuotePriceStepsPM> QuotePriceStepsDataMappingAndValidatin(List<QuotePriceSteps> MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   
				var MyList = new List<QuotePriceStepsPM>();
				foreach (var item in MyEntity)
				{
					   					var temp = new QuotePriceStepsPM();								  
					if (!string.IsNullOrEmpty(item.Id))
					{
						temp = query.GetSinglePM(item.Id, Tenant);
					} 
										   
					if(temp == null)
					{   
					    throw new ApplicationException("QuotePriceSteps with Id " + item.Id + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(item.Id))
					    {
					        throw new ApplicationException("QuotePriceSteps with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = item.Id;

						//} 

						
					}
                    
					if(!IsUpdate)
					{							
						temp.Step = item.PriceBreakStep;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.CostUnitPrice = item.CostUnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.SaleUnitPrice = item.SaleUnitPrice;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.MeasurementUnit = item.MeasurementUnit;

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