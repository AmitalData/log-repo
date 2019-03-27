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
namespace WebFreight.Web.Controllers.AccountingModel //AccountingPeriodViewsController.cs
{
    //[RoutePrefix("api/ReconciliationOp")]
    public partial class ReconciliationOpController : ApiController
    {
        public ReconciliationOpController()
        {

        }


       

        public HttpResponseMessage DeleteResetDraftOpenReconciliation(string gLAccountId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                LedgerTransactionRepository repoLedgerTransaction = new LedgerTransactionRepository(tenant);
                repoLedgerTransaction.ResetDraftOpenReconciliation(gLAccountId, tenant);

                var o = new { success = true };
                return Request.CreateResponse(HttpStatusCode.OK, o);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    


        //[Route("{obj:ReconciliationPM}/InsertReconciliation")]
        public HttpResponseMessage PostInsertReconciliation(ReconciliationPM entityPm)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Reconciliation", "NEW", authToken.Tenant);

                    CreateReconciliationService recoService = new CreateReconciliationService();
                    entityPm.Tenant = authToken.Tenant;
                    RecoCallback recoCallback = recoService.CreateReconciliation(entityPm);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, recoCallback);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



        public HttpResponseMessage DeleteReconciliation(string ReconciliationOperation, string reconciliationId, int tenant)
        {
            ReconciliationPM entitypm = null;
            try
            {

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Reconciliation", "NEW", authToken.Tenant);
                    var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    var service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);
                    ReconciliationOperation = ReconciliationOperation ?? string.Empty;
                    switch (ReconciliationOperation.ToLower())
                    {
                        case "cancell":
                            {
                                entitypm = service.CancellReconciliation(reconciliationId, tenant);
                            }
                            break;
                        case "purge":
                            {
                                throw new Exception("Reconciliation Operation purge is not implement yet ...");
                            }
                            break;
                        default:
                            throw new Exception("ReconciliationOperation unknown");
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



        public HttpResponseMessage PostFilteredReconciliation(string gLAccountId, int tenant, FilteredReconciliation myQueryOperations)
        {
            var agg = new OpenReconciliationAggregate();
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            var accountingContext = AccountingContext.GetContext(tenant);
            var qs = new LedgerTransactionListQueryService(accountingContext);
            GenericCallBack ReconciliationFilterCallBack = null;
            myQueryOperations = myQueryOperations ?? new FilteredReconciliation() { QueryFilterItems = new List<QueryFilterItem>() };
            if (myQueryOperations.CallBack == null )
            {

                agg.OpenReconciliationDraft= qs.OpenReconciliationDraft(gLAccountId, tenant);
                myQueryOperations.CallBack = qs.GetOpenReconciliationFilterCallBack(myQueryOperations, gLAccountId, 
tenant);
            }
            var openReconciliation = qs.GetOpenReconciliationFilterList(myQueryOperations, myQueryOperations.CallBack, gLAccountId, tenant);


            agg.CallBack = myQueryOperations.CallBack;
            agg.OpenReconciliation = openReconciliation;
            return Request.CreateResponse(HttpStatusCode.OK, agg);
        }

        


        public HttpResponseMessage PutDelsertDraftLedgerTransaction(List<LedgerTransactionList> OpenRecilationDrafts )
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //SecurityUtility.CheckContactFeature("Reconciliation", "NEW", authToken.Tenant);
                //var accountingContext = AccountingContext.GetContext(tenant);
                //LedgerTransactionListQueryService listService = new LedgerTransactionListQueryService(accountingContext);
                if (OpenRecilationDrafts == null || OpenRecilationDrafts.Count==0)
                {
                    throw new Exception("PutDelSertDraftLedgerTransaction expected a list !");
                }
                
                var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                var qs = new LedgerTransactionListQueryService(accountingContext);

                LedgerTransactionUpdateService us = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), OpenRecilationDrafts.First().Tenant);
                us.DelSertOpenRecilationDrafts(OpenRecilationDrafts);

                return Request.CreateResponse(HttpStatusCode.OK, new { Ok=true });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


         public HttpResponseMessage PutAutomaticReconcileByFilter(string gLAccountId, int tenant, FilteredReconciliation myFilteredReconciliation)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                

                var automaticReconcileService = new AutomaticReconcileService();
                automaticReconcileService.AutomaticReconcile(gLAccountId, tenant, myFilteredReconciliation);
                
                return Request.CreateResponse(HttpStatusCode.OK, automaticReconcileService.AllLedgerTransactionList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOpenReconciliationsByFilters(string gLAccountId, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;
               

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "LedgerTransaction",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "LedgerTransactions",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                #region filters
                List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
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

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

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
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }
                #endregion

                var accountingContext = AccountingContext.GetContext(tenant);
                var qs = new LedgerTransactionListQueryService(accountingContext);
                GenericCallBack ReconciliationFilterCallBack = null;

                var callback = qs.GetOpenReconciliationFilterCallBack(queryOperations, gLAccountId, tenant);

                var openReconciliation = qs.GetOpenReconciliationFilterList(queryOperations, callback, gLAccountId, tenant);

                openReconciliation = openReconciliation.OrderByDescending(d=>d.DocumentDate).ToList();

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = callback.TotalRecord;
                    response.Count = count;
                }

                response.Result = openReconciliation;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet]
        public HttpResponseMessage GetAutomaticReconcileByFilter(string method1, string method2, string method3, string gLAccountId, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                
                #region filters

                int tenant = authToken.Tenant;
               

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "LedgerTransaction",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "LedgerTransactions",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> LedgerTransactionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("LedgerTransaction", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
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

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

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
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }
                #endregion

                AutomaticReconcilePM.AutomaticReconcileEnum m1;
                AutomaticReconcilePM.AutomaticReconcileEnum m2;
                AutomaticReconcilePM.AutomaticReconcileEnum m3;
                Enum.TryParse(method1, out m1);
                Enum.TryParse(method2, out m2);
                Enum.TryParse(method3, out m3);

                FilteredReconciliation myFilteredReconciliation = new FilteredReconciliation()
                {
                    QueryFilterItems = queryOperations.QueryFilterItems,
                    QuerySection = queryOperations.QuerySection,
                    ObjectTableName = queryOperations.ObjectTableName,
                    PageIndex = queryOperations.PageIndex,
                    PageSize = queryOperations.PageSize,
                    SortByColumnName = queryOperations.SortByColumnName,
                    SortDirectin = queryOperations.SortDirectin,
                    GetAll = queryOperations.GetAll,
                    method1 = m1,
                    method2 = m2,
                    method3 = m3,
                    ClientChooseAutoMethod= true,
                };

                

                var automaticReconcileService = new AutomaticReconcileService();
                automaticReconcileService.AutomaticReconcile(gLAccountId, tenant, myFilteredReconciliation);

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = automaticReconcileService.AllLedgerTransactionList.Count;
                    response.Count = count;
                }

                response.Result = automaticReconcileService.AllLedgerTransactionList;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetDraftReconciliations(string gLAccountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;

                LedgerTransactionQueryService transactionQuery = new LedgerTransactionQueryService(tenant);
                List<LedgerTransactionPM>  draftTransactions = transactionQuery.DraftLedgerTransactionPMsByAccountId(gLAccountId, tenant);


                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, draftTransactions);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        
        public HttpResponseMessage PostCreateJournalReconcile(
            List<ReconciliationLinePM> ReconciliationLines,
            string TheAccountId,
            string AdjustAccountId,
            DateTime AccountDate,
            string Ref1,
            string Ref2,
            string Ref3,
            string Remarks
            
            )
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;

                var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                var createJournalReconcileService = new CreateJournalReconcileService();
                var pm =createJournalReconcileService
                    .Create(
                    accountingContext, authToken.Tenant
                    , ReconciliationLines, TheAccountId, AdjustAccountId,
                    AccountDate, Ref1, Ref2, Ref3,Remarks);

                    HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, pm);


                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }

 
   
    public class OpenReconciliationAggregate
    {
        public GenericCallBack CallBack { get; set; }

        public List<LedgerTransactionList> OpenReconciliation { get; set; }
        public List<LedgerTransactionList> OpenReconciliationDraft { get; set; }
    }
}