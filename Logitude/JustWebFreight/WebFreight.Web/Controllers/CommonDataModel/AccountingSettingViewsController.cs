
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.Helpers;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.Repositories;
namespace WebFreight.Web.Controllers.CommonDataModel.Generated.ListControllers
{ 

    
    public partial class AccountingSettingViewsController : ApiController
    {


        //public HttpResponseMessage GetSingle(int id)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        //        AccountingSettingQuery accountingSettingQuery = new AccountingSettingQuery(authToken.Tenant);
        //        var accountingSettingList = accountingSettingQuery.GetSinglePM(id);

        //        return Request.CreateResponse(HttpStatusCode.OK, accountingSettingList);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}

         
        //public HttpResponseMessage GetAll()
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


        //        ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
        //        AccountingSettingRepository  accountingSystemRepository = new AccountingSettingRepository(MyContext);
        //        IQueryable<AccountingSetting> entityPocos = accountingSystemRepository.GetAccountingSettings();

        //        AccountingSettingQuery accountingSystemQuery = new AccountingSettingQuery(accountingSystemRepository);
        //        IQueryable<AccountingSettingList> entityLists = accountingSystemQuery.GetIQueryableEntityList(entityPocos);
        //        entityLists = entityLists.OrderBy(d => d.Name);
				
        //        return Request.CreateResponse(HttpStatusCode.OK, entityLists);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}
        
        //[HttpGet]
        //public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        //{
        //    try
        //    {
        //         string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //        int tenant = authToken.Tenant;
        //        if(filters.Tenant != null)
        //            tenant = tenant;
				
        //        QueryOperations queryOperations = new QueryOperations()
        //        {
        //            ObjectTableName = "AccountingSetting",
        //            PageIndex = filters.PageIndex,
        //            PageSize = filters.PageSize,
        //            QuerySection = "AccountingSettings",
        //            SortByColumnName = filters.SortBy,
        //            SortDirectin = filters.SortDirection,
        //            GetAll = filters.GetAll, 
        //        };

        //        List<ObjectField> AccountingSettingObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AccountingSetting",tenant);
        //        List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
        //        for (int i = 1; i <= 10; i++)
        //        {
        //            object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
        //            object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
        //            object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
        //            object filterValue2 = null;

        //            if (filterNameProp != null)
        //            {
        //                string filterName = filterNameProp.ToString();
        //                string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
        //                //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
        //                //{
        //                   //string[] values = filterValue1.ToString().Split(',');
        //                    //if (values.Count() > 1)
        //                    //{
        //                        //filterValue1 = values[0];
        //                        //filterValue2 = values[1];
        //                    //}
        //                //}
        //                //ToDo: Get object field by name and set the remained filter properties
        //                ObjectField field = AccountingSettingObjectFields.FirstOrDefault(f => f.FieldName == filterName);
        //                if (field != null)
        //                {
        //                    string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
        //                    object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

        //                    string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
        //                    object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

        //                    queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
        //                }
        //                else
        //                    queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
        //            }



        //        }

        //      if (!string.IsNullOrEmpty(filters.AdditionalFilters))
        //        {
        //            JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
        //            var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

        //            foreach (QueryFilterItem filter in filters_list)
        //            {
        //                ObjectField field = AccountingSettingObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
        //                if (field != null)
        //                {


        //                    string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
        //                    object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

        //                    string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
        //                    object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

        //                    queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
        //                }
        //                else
        //                {
        //                    queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
        //                }
        //            }
        //        }



        //        GenericFilter genericFilter = new GenericFilter();
        //        GenericSort sortClass = new GenericSort();

        //        ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
        //        AccountingSettingRepository  accountingSystemRepository = new AccountingSettingRepository(MyContext);
        //        IQueryable<AccountingSetting> entityPocos = accountingSystemRepository.GetAccountingSettings();

        //        AccountingSettingQuery accountingSystemQuery = new AccountingSettingQuery(accountingSystemRepository);
                
        //        QueryOperations nonListQueryOperation = new QueryOperations();
        //        nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //        QueryOperations listQueryOperation = new QueryOperations();
        //        listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
				
        //        entityPocos = genericFilter.GetFilteredQuery<AccountingSetting>(nonListQueryOperation, entityPocos);
        //        int skippedEntities = queryOperations.PageIndex;
        //        IQueryable<AccountingSettingList> entityLists = accountingSystemQuery.GetIQueryableEntityList(entityPocos);

        //        entityLists = genericFilter.GetFilteredQuery<AccountingSettingList>(listQueryOperation, entityLists);

		 
        //        if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
        //         {
        //           PropertyInfo propInfo = typeof(AccountingSettingList).GetProperty(queryOperations.SortByColumnName);
                   

        //           ObjectField objectField = (from a in AccountingSettingObjectFields
        //                                   where a.FieldName == queryOperations.SortByColumnName
        //                                   select a).FirstOrDefault();

        //           if (objectField != null)
        //           {
        //            if (objectField.IsCustom)
        //            {
        //                entityLists = sortClass.GetSorterQuery<AccountingSettingList, string>(queryOperations, entityLists);
        //            }
        //            else
        //            {
        //             switch (objectField.DataTypeCode.ToLower())
        //             {
        //                 case "ntext":
        //                case "text":
        //                    {
        //                        entityLists = sortClass.GetSorterQuery<AccountingSettingList, string>(queryOperations, entityLists);
        //                        break;
        //                    }
        //                case "sigdouble":
        //                case "double":
        //                    {
        //                        entityLists = sortClass.GetSorterQuery<AccountingSettingList, double>(queryOperations, entityLists);
        //                        break;
        //                    }
        //                case "date":
        //                case "datetime":
        //                    {
        //                        entityLists = sortClass.GetSorterQuery<AccountingSettingList, DateTime>(queryOperations, entityLists);
        //                        break;
        //                    }
        //                case "unsinteger":
        //                case "integer":
        //                    {
        //                        entityLists = sortClass.GetSorterQuery<AccountingSettingList, int>(queryOperations, entityLists);
        //                        break;
        //                    }
        //                case "boolean":
        //                    {
        //                        entityLists = sortClass.GetSorterQuery<AccountingSettingList, bool>(queryOperations, entityLists);
        //                        break;
        //                    }
        //                case "unsdecimal":
        //                case "decimal":
        //                    {
        //                        entityLists = sortClass.GetSorterQuery<AccountingSettingList, decimal>(queryOperations, entityLists);
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        entityLists = entityLists.OrderBy(d => d.Name);
        //                        break;
        //                    }
        //            }
        //         }
        //        }
        //    }
        //    else
        //    {
        //        entityLists = entityLists.OrderBy(d => d.Name);
        //    }

        //    ServiceResponse response = new ServiceResponse();
			
        //    if (filters.GetCount)
        //      {
        //            response.Count = entityLists.Count();
        //      }
        //        if(!queryOperations.GetAll)
        //         {

        //          entityLists = entityLists.Skip(skippedEntities);
        //          entityLists = entityLists.Take(queryOperations.PageSize);

        //        }

        //       response.Result = entityLists;
        //       HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

               
        //        return reponseMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}

		 
		
      
    }
}
	 