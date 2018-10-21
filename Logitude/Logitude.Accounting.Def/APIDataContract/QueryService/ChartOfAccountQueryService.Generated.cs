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
   public partial class ChartOfAccountQueryService
   {
   
		IAccountingContext  context;
		//ChartOfAccountService service; 
		
		Logitude.Accounting.BL.EntityQueryServices.ChartOfAccountQueryService query; 

        public ChartOfAccountQueryService(int tenant)
        {
				    context = AccountingContext.GetContext(tenant); 
			//service = new ChartOfAccountService(context, tenant); 
			query = new Logitude.Accounting.BL.EntityQueryServices.ChartOfAccountQueryService(tenant);
        }

		
		public ChartOfAccount GetChartOfAccountById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("ChartOfAccount with Id " + Id + " doesn't exist");

				return ChartOfAccountDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public ChartOfAccount ChartOfAccountDataMapping(ChartOfAccountPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new ChartOfAccount(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Tenant = MyEntityPM.Tenant;
				   temp.LogitudeCode = MyEntityPM.Code;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.EnglishName = MyEntityPM.EnglishName;			  
				   if(MyEntityPM.TypeCode != null)
				   {
					   ChartOfAccountsTypeQueryService ChartOfAccountsTypeService0 = new ChartOfAccountsTypeQueryService(Tenant);
					   					   temp.TypeCode = ChartOfAccountsTypeService0.GetChartOfAccountsTypeByCode(MyEntityPM.TypeCode,Tenant); 
			       
					   				   }
				   
				   temp.TypeName = MyEntityPM.TypeName;
				   temp.ParentName = MyEntityPM.ParentName;			  
				   if(MyEntityPM.ParentId != null)
				   {
					   ChartOfAccountQueryService ChartOfAccountService1 = new ChartOfAccountQueryService(Tenant);
					   					   temp.Parent = ChartOfAccountService1.GetChartOfAccountById(MyEntityPM.ParentId,Tenant); 
			       
					   				   }
				   
				   temp.Inactive = MyEntityPM.Inactive;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public ChartOfAccountPM ChartOfAccountDataMappingAndValidatin(ChartOfAccount MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new ChartOfAccountPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
										   
					if(temp == null)
					{
					    throw new ApplicationException("ChartOfAccount with Id " + MyEntity.Id + " doesn't exist");
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
					temp.LocalName = MyEntity.LocalName;
					temp.EnglishName = MyEntity.EnglishName;
					ChartOfAccountsTypeQueryService TypeCodeChartOfAccountsTypeService = new ChartOfAccountsTypeQueryService(Tenant);
					if(MyEntity.TypeCode != null)
					{
						var myTypeCodePM = TypeCodeChartOfAccountsTypeService.ChartOfAccountsTypeDataMappingAndValidatin(MyEntity.TypeCode,Tenant,ComputingPartnerName);
												if(myTypeCodePM != null)
						{
							temp.TypeCode = myTypeCodePM.Code;
						}
						 
					}
			
					
					temp.TypeName = MyEntity.TypeName;
					temp.ParentName = MyEntity.ParentName;
					ChartOfAccountQueryService ParentChartOfAccountService = new ChartOfAccountQueryService(Tenant);
					if(MyEntity.Parent != null)
					{
						var myParentPM = ParentChartOfAccountService.ChartOfAccountDataMappingAndValidatin(MyEntity.Parent,Tenant,ComputingPartnerName);
												if(myParentPM != null)
						{
							temp.ParentId = myParentPM.Id;
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