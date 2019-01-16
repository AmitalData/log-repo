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
   public partial class AutomaticReconcileMethodQueryService
   {
   
		IAccountingContext  context;
		//AutomaticReconcileMethodService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.AutomaticReconcileMethodQueryService query; 

        public AutomaticReconcileMethodQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new AutomaticReconcileMethodService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.AutomaticReconcileMethodQueryService(tenant);
        }

		
		public AutomaticReconcileMethod GetAutomaticReconcileMethodById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("AutomaticReconcileMethod with Id " + Id + " doesn't exist");

				return AutomaticReconcileMethodDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public AutomaticReconcileMethod AutomaticReconcileMethodDataMapping(AutomaticReconcileMethodPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new AutomaticReconcileMethod(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.LogitudeCode = MyEntityPM.Code;			  
				   if(MyEntityPM.AutomaticReconcile1 != null)
				   {
					   AutomaticReconcileQueryService AutomaticReconcileService0 = new AutomaticReconcileQueryService(Tenant);
					   					   temp.AutomaticReconcile1 = AutomaticReconcileService0.GetAutomaticReconcileByCode(MyEntityPM.AutomaticReconcile1,Tenant); 
			       
					   				   }
				   
				   temp.AutomaticReconcileName1 = MyEntityPM.AutomaticReconcileName1;
				   temp.AutomaticReconcileName2 = MyEntityPM.AutomaticReconcileName2;
				   temp.AutomaticReconcileName3 = MyEntityPM.AutomaticReconcileName3;
				   temp.Name = MyEntityPM.Name;
				   temp.LocalName = MyEntityPM.LocalName;			  
				   if(MyEntityPM.AutomaticReconcile2 != null)
				   {
					   AutomaticReconcileQueryService AutomaticReconcileService1 = new AutomaticReconcileQueryService(Tenant);
					   					   temp.AutomaticReconcile2 = AutomaticReconcileService1.GetAutomaticReconcileByCode(MyEntityPM.AutomaticReconcile2,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.AutomaticReconcile3 != null)
				   {
					   AutomaticReconcileQueryService AutomaticReconcileService2 = new AutomaticReconcileQueryService(Tenant);
					   					   temp.AutomaticReconcile3 = AutomaticReconcileService2.GetAutomaticReconcileByCode(MyEntityPM.AutomaticReconcile3,Tenant); 
			       
					   				   }
				   
				   temp.Inactive = MyEntityPM.Inactive;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public AutomaticReconcileMethodPM AutomaticReconcileMethodDataMappingAndValidatin(AutomaticReconcileMethod MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
					var temp = new AutomaticReconcileMethodPM();
												  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("AutomaticReconcileMethod with Id " + MyEntity.Id + " doesn't exist");
						
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
						temp.Id = MyEntity.Id;
					}
					temp.Tenant = MyEntity.Tenant;
					if(string.IsNullOrEmpty(temp.Code))
					{
						temp.Code = MyEntity.LogitudeCode;
					}
					AutomaticReconcileQueryService AutomaticReconcile1AutomaticReconcileService = new AutomaticReconcileQueryService(Tenant);
					if(MyEntity.AutomaticReconcile1 != null)
					{
						var myAutomaticReconcile1PM = AutomaticReconcile1AutomaticReconcileService.AutomaticReconcileDataMappingAndValidatin(MyEntity.AutomaticReconcile1,Tenant,ComputingPartnerName);
												if(myAutomaticReconcile1PM != null)
						{
							temp.AutomaticReconcile1 = myAutomaticReconcile1PM.Code;
						}
						 
					}
			
					
					temp.AutomaticReconcileName1 = MyEntity.AutomaticReconcileName1;
					temp.AutomaticReconcileName2 = MyEntity.AutomaticReconcileName2;
					temp.AutomaticReconcileName3 = MyEntity.AutomaticReconcileName3;
					temp.Name = MyEntity.Name;
					temp.LocalName = MyEntity.LocalName;
					AutomaticReconcileQueryService AutomaticReconcile2AutomaticReconcileService = new AutomaticReconcileQueryService(Tenant);
					if(MyEntity.AutomaticReconcile2 != null)
					{
						var myAutomaticReconcile2PM = AutomaticReconcile2AutomaticReconcileService.AutomaticReconcileDataMappingAndValidatin(MyEntity.AutomaticReconcile2,Tenant,ComputingPartnerName);
												if(myAutomaticReconcile2PM != null)
						{
							temp.AutomaticReconcile2 = myAutomaticReconcile2PM.Code;
						}
						 
					}
			
					
					AutomaticReconcileQueryService AutomaticReconcile3AutomaticReconcileService = new AutomaticReconcileQueryService(Tenant);
					if(MyEntity.AutomaticReconcile3 != null)
					{
						var myAutomaticReconcile3PM = AutomaticReconcile3AutomaticReconcileService.AutomaticReconcileDataMappingAndValidatin(MyEntity.AutomaticReconcile3,Tenant,ComputingPartnerName);
												if(myAutomaticReconcile3PM != null)
						{
							temp.AutomaticReconcile3 = myAutomaticReconcile3PM.Code;
						}
						 
					}
			
					
					temp.Inactive = MyEntity.Inactive;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}