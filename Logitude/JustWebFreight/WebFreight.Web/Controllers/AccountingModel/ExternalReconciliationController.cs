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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;

using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data.Repositories;
using System.Transactions;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Simplog.Data.Helpers;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated 
{
    public partial class ExternalReconciliationController : ApiController
    {
        public ExternalReconciliationController()
        {

        }

        [HttpGet]
        public HttpResponseMessage GetExternalAutomaticReconcilationsByFilter(
            bool amountReconcile,
            bool referenceReconcile,
            bool refDateReconcile,
			bool accoutingDateReconcile,
			string objectTableId,
            string entityId,
            string glAccountId,
            [FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;


                #region Trans filters


                QueryOperations queryOperationsTrans = new QueryOperations()
                {
                    ObjectTableName = "LedgerTransaction",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "LedgerTransaction",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
                List<PropertyInfo> filterPropertiesTrans = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterPropertiesTrans.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterPropertiesTrans.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterPropertiesTrans.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                        ObjectField field = LedgerTransactionObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperationsTrans.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                            queryOperationsTrans.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

                DateTime today = GetCurrentDateStart(tenant);

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = LedgerTransactionObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {

                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            
                            object value1;
                            if (valuestring1 == "#today")
                                value1 = today;
                            else
                                value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperationsTrans.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperationsTrans.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }
                #endregion


                #region Bank lines filters

                

                QueryOperations queryOperationsBankLine = new QueryOperations()
                {
                    ObjectTableName = "ReconcileExternalPageLine",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "ReconcileExternalPageLine",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> ReconcileExternalPageLineObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ReconcileExternalPageLine", tenant);
                List<PropertyInfo> filterPropertiesBankLine = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterPropertiesBankLine.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterPropertiesBankLine.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterPropertiesBankLine.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();


                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                        ObjectField field = ReconcileExternalPageLineObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperationsBankLine.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                            queryOperationsBankLine.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        // replace LedgerTransaction fields names with bank page line fields name
                        if (filter.FieldName != null)
                        {
                            filter.FieldName = filter.FieldName.Replace("ForeignAmount", "Amount");
                            filter.FieldName = filter.FieldName.Replace("DocumentDate", "ReferenceDate");
                        }

                        if (filter.FieldName == "IsExternalReconcile"
                            || filter.FieldName == "DueDate"
                            || filter.FieldName == "DUMMY_TransferAccountId"
                            || filter.FieldName == "InReconcileProgress"
                            || filter.FieldName == "InProgressExternalReconcile")
                            continue;

                        //

                        ObjectField field = ReconcileExternalPageLineObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperationsBankLine.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperationsBankLine.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }
                #endregion

                string transferGlAccountId = GetAndRemoveFilter(queryOperationsTrans, "DUMMY_TransferAccountId");

                var args = new AutoExternalReconcileArgs()
                {
                    AmountReconcile = amountReconcile,
                    ReferenceReconcile = referenceReconcile,
                    RefDateReconcile = refDateReconcile,
					AccoutingDateReconcile = accoutingDateReconcile,
					ObjectTableId = objectTableId,
                    EntityId = entityId,
                    GLAccountId = glAccountId,
                    TransferGLAccountId = transferGlAccountId,
                    TransactionQueryOperations = queryOperationsTrans,
                    BankPageLineQueryOperations = queryOperationsBankLine
                };

                var automaticExternalReconcileService = new AutomaticExternalReconcileService(tenant);
                MatchedReconciliationLines matchedLines = automaticExternalReconcileService.GetMatchedLines(args);

                ServiceResponse response = new ServiceResponse();
                response.Result = matchedLines;
                response.Count = matchedLines.Count;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        private DateTime GetCurrentDate(int tenant)
        {
            DateTime _today = TenantServerConfigration.GetCurrentDateTime(tenant);
            _today = new DateTime(_today.Year, _today.Month, _today.Day, 11, 59, 59);
            return _today;
        }
        private DateTime GetCurrentDateStart(int tenant)
        {
            DateTime _today = TenantServerConfigration.GetCurrentDateTime(tenant);
            _today = new DateTime(_today.Year, _today.Month, _today.Day, 0, 0, 0,0);
            return _today;
        }
        private static string GetAndRemoveFilter(QueryOperations queryOperations, string fieldName)
        {
            QueryFilterItem filterItem = queryOperations.QueryFilterItems.Find(d => d.FieldName == fieldName);
            string TransferGlAccountId = filterItem?.FieldValue.ToString();
            queryOperations.QueryFilterItems.Remove(filterItem);
            return TransferGlAccountId;
        }


        public HttpResponseMessage GetGenerateTestRecordsForExternalReco(string glAccountId, string bankAccountId, string type)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                AutomaticExternalReconcileService automaticExternalReconcileService = new  AutomaticExternalReconcileService(authToken.Tenant);
                automaticExternalReconcileService.GenerateTestRecordsForExternalReco(glAccountId, bankAccountId, type, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }

}