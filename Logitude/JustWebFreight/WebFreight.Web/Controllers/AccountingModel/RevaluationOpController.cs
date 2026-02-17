using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using System.Transactions;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Accounting.BL.Utils;

namespace WebFreight.Web.Controllers.AccountingModel //AccountingPeriodViewsController.cs
{
    //[RoutePrefix("api/RevaluationOp")]
    public partial class RevaluationOpController : ApiController
    {
        public RevaluationOpController()
        {

        }
        public HttpResponseMessage GetRunAllOpenRevaluations(int tenant)
        {
            try
            {


                string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //SecurityUtility.CheckContactFeature("Revaluation", "NEW", authToken.Tenant);

                RevaluationBatch revaluationBatch = new RevaluationBatch();
                revaluationBatch.RunAllOpenRevaluations(tenant);
                string responseText = revaluationBatch.ResponseText();
                HttpStatusCode StatusCode = revaluationBatch.StatusCode();
                var res1 = new { Success = true, Message = responseText};

                return Request.CreateResponse(StatusCode, res1);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }




        //public HttpResponseMessage DeleteResetDraftOpenRevaluation(string gLAccountId, int tenant)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


        //        LedgerTransactionRepository repoLedgerTransaction = new LedgerTransactionRepository(tenant);
        //        repoLedgerTransaction.ResetDraftOpenRevaluation(gLAccountId, tenant);

        //        var o = new { success = true };
        //        return Request.CreateResponse(HttpStatusCode.OK, o);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}




        //[Route("{obj:RevaluationPM}/InsertRevaluation")]
        public HttpResponseMessage PostInsertRevaluation(RevaluationPM entityPm)
        {
            try
            {


                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Revaluation", "NEW", authToken.Tenant);
                var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                var qs = new LedgerTransactionListQueryService(accountingContext);

                RevaluationUpdateService service = new RevaluationUpdateService(accountingContext, new Dictionary<string, IContext>(), entityPm.Tenant);
                if (entityPm.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                {
                    throw new Exception("Meanwhile Only Insert Enable ");
                }
                entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                //foreach (var item in entityPm.RevaluationLines)
                //{
                //    item.ChangeSetOp = ChangeSetOperation.Insert;
                //}
                entityPm.Tenant = authToken.Tenant;
                //entityPm.AccountId = gLAccountId;
                service.Update(entityPm, true);
                return Request.CreateResponse(HttpStatusCode.OK, entityPm);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        public HttpResponseMessage DeleteRevaluation(string RevaluationOperation, string revaluationId, int tenant)
        {
            RevaluationPM entitypm = null;
            try
            {

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Revaluation", "NEW", authToken.Tenant);
                    var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    var service = new RevaluationUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);
                    RevaluationOperation = RevaluationOperation ?? string.Empty;
                    switch (RevaluationOperation.ToLower())
                    {
                        case "cancell":
                            {
                                entitypm = service.CancelRevaluation(revaluationId, tenant);
                            }
                            break;
                        case "purge":
                            {
                                throw new Exception("Revaluation Operation purge is not implement yet ...");
                            }
                            break;
                        default:
                            throw new Exception("RevaluationOperation unknown");
                            break;
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entitypm);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



//        public HttpResponseMessage PostFilteredRevaluation(string gLAccountId, int tenant, FilteredRevaluation myQueryOperations)
//        {
//            var agg = new OpenRevaluationAggregate();
//            string token = HttpContext.Current.Request.Headers["Token"];
//            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//            var accountingContext = AccountingContext.GetContext(tenant);
//            var qs = new LedgerTransactionListQueryService(accountingContext);
//            GenericCallBack RevaluationFilterCallBack = null;
//            myQueryOperations = myQueryOperations ?? new FilteredRevaluation() { QueryFilterItems = new List<QueryFilterItem>() };
//            if (myQueryOperations.CallBack == null)
//            {

//                agg.OpenRevaluationDraft = qs.OpenRevaluationDraft(gLAccountId, tenant);
//                myQueryOperations.CallBack = qs.GetOpenRevaluationFilterCallBack(myQueryOperations, gLAccountId,
//tenant);
//            }
//            var openRevaluation = qs.GetOpenRevaluationFilterList(myQueryOperations, myQueryOperations.CallBack, gLAccountId, tenant);


//            agg.CallBack = myQueryOperations.CallBack;
//            agg.OpenRevaluation = openRevaluation;
//            return Request.CreateResponse(HttpStatusCode.OK, agg);
//        }





        //public HttpResponseMessage PutAutomaticReconcileByFilter(string gLAccountId, int tenant, FilteredRevaluation myFilteredRevaluation)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


        //        var automaticReconcileService = new AutomaticReconcileService();
        //        automaticReconcileService.AutomaticReconcile(gLAccountId, tenant, myFilteredRevaluation);

        //        return Request.CreateResponse(HttpStatusCode.OK, automaticReconcileService.AllLedgerTransactionList);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}




        //[HttpGet]
        //public HttpResponseMessage GetOpenRevaluationsByFilters(string gLAccountId, [FromUri] ApiQueryFilters filters)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        //        int tenant = authToken.Tenant;
      

        //        QueryOperations queryOperations = new QueryOperations()
        //        {
        //            ObjectTableName = "LedgerTransaction",
        //            PageIndex = filters.PageIndex,
        //            PageSize = filters.PageSize,
        //            QuerySection = "LedgerTransactions",
        //            SortByColumnName = filters.SortBy,
        //            SortDirectin = filters.SortDirection,
        //            GetAll = filters.GetAll,
        //        };

        //        #region filters
        //        List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
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
        //                ObjectField field = LedgerTransactionObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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

        //        if (!string.IsNullOrEmpty(filters.AdditionalFilters))
        //        {
        //            JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
        //            var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

        //            foreach (QueryFilterItem filter in filters_list)
        //            {
        //                ObjectField field = LedgerTransactionObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
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
        //        #endregion

        //        var accountingContext = AccountingContext.GetContext(tenant);
        //        var qs = new LedgerTransactionListQueryService(accountingContext);
        //        GenericCallBack RevaluationFilterCallBack = null;

        //        var callback = qs.GetOpenRevaluationFilterCallBack(queryOperations, gLAccountId, tenant);

        //        var openRevaluation= qs.GetOpenRevaluationFilterList(queryOperations, callback, gLAccountId, tenant);


        //        ServiceResponse response = new ServiceResponse();
        //        if (filters.GetCount)
        //        {
        //            int count = callback.TotalRecord;
        //            response.Count = count;
        //        }

        //        response.Result = openRevaluation;
        //        HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

        //        return reponseMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}

        //[HttpGet]
        //public HttpResponseMessage GetAutomaticReconcileByFilter(string method1, string method2, string method3, string gLAccountId, [FromUri] ApiQueryFilters filters)
        //{
        //    try
        //    {
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

        //        #region filters

        //        int tenant = authToken.Tenant;


        //        QueryOperations queryOperations = new QueryOperations()
        //        {
        //            ObjectTableName = "LedgerTransaction",
        //            PageIndex = filters.PageIndex,
        //            PageSize = filters.PageSize,
        //            QuerySection = "LedgerTransactions",
        //            SortByColumnName = filters.SortBy,
        //            SortDirectin = filters.SortDirection,
        //            GetAll = filters.GetAll,
        //        };

        //        List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
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
        //                ObjectField field = LedgerTransactionObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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

        //        if (!string.IsNullOrEmpty(filters.AdditionalFilters))
        //        {
        //            JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
        //            var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

        //            foreach (QueryFilterItem filter in filters_list)
        //            {
        //                ObjectField field = LedgerTransactionObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
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
        //        #endregion

        //        AutomaticReconcilePM.AutomaticReconcileEnum m1;
        //        AutomaticReconcilePM.AutomaticReconcileEnum m2;
        //        AutomaticReconcilePM.AutomaticReconcileEnum m3;
        //        Enum.TryParse(method1, out m1);
        //        Enum.TryParse(method2, out m2);
        //        Enum.TryParse(method3, out m3);

        //        FilteredRevaluation myFilteredRevaluation = new FilteredRevaluation()
        //        {
        //            QueryFilterItems = queryOperations.QueryFilterItems,
        //            QuerySection = queryOperations.QuerySection,
        //            ObjectTableName = queryOperations.ObjectTableName,
        //            PageIndex = queryOperations.PageIndex,
        //            PageSize = queryOperations.PageSize,
        //            SortByColumnName = queryOperations.SortByColumnName,
        //            SortDirectin = queryOperations.SortDirectin,
        //            GetAll = queryOperations.GetAll,
        //            method1 = m1,
        //            method2 = m2,
        //            method3 = m3,
        //        };



        //        var automaticReconcileService = new AutomaticReconcileService();
        //        automaticReconcileService.AutomaticReconcile(gLAccountId, tenant, myFilteredRevaluation);

        //        ServiceResponse response = new ServiceResponse();
        //        if (filters.GetCount)
        //        {
        //            int count = automaticReconcileService.AllLedgerTransactionList.Count;
        //            response.Count = count;
        //        }

        //        response.Result = automaticReconcileService.AllLedgerTransactionList;
        //        HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

        //        return reponseMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }

        //}

    }



    public class OpenRevaluationAggregate
    {
        public GenericCallBack CallBack { get; set; }

        public List<GLAccountList> OpenRevaluation { get; set; }
        public List<GLAccountList> OpenRevaluationDraft { get; set; }
    }
}