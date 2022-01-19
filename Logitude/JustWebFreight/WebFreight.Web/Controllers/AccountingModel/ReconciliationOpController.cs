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
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL.Reconcile;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.StorageService;

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
                SecurityUtility.AuthenticationOnTenant(tenant);


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
                    SecurityUtility.AuthenticationOnEntityTenant("ReconciliationPM", entityPm.Tenant, authToken.Tenant);
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
                    SecurityUtility.AuthenticationOnTenant(tenant);
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
            SecurityUtility.AuthenticationOnTenant(tenant);
            var accountingContext = AccountingContext.GetContext(tenant);
            var qs = new LedgerTransactionListQueryService(accountingContext);
            GenericCallBack ReconciliationFilterCallBack = null;
            myQueryOperations = myQueryOperations ?? new FilteredReconciliation() { QueryFilterItems = new List<QueryFilterItem>() };
            if (myQueryOperations.CallBack == null)
            {

                agg.OpenReconciliationDraft = qs.OpenReconciliationDraft(gLAccountId, tenant);
                myQueryOperations.CallBack = qs.GetReconciliationFilterCallBack(myQueryOperations, gLAccountId,
tenant);
            }
            var openReconciliation = qs.GetOpenReconciliationFilterList(myQueryOperations, myQueryOperations.CallBack, gLAccountId, tenant);


            agg.CallBack = myQueryOperations.CallBack;
            agg.OpenReconciliation = openReconciliation;
            return Request.CreateResponse(HttpStatusCode.OK, agg);
        }




        public HttpResponseMessage PutDraftReconciliationTransactions(List<LedgerTransactionPM> transactions)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();
                foreach (var item in transactions)
                {
                    SecurityUtility.AuthenticationOnEntityTenant("LedgerTransaction", item.Tenant, tenant);
                }
                BlockEmptyTransactions(transactions);

                UpdateDraftReconciliationTransactions(transactions, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, new { Ok = true });
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static void UpdateDraftReconciliationTransactions(List<LedgerTransactionPM> transactions, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionUpdateService query = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            query.DelSertOpenRecilationDrafts(transactions);
        }


        private static void BlockEmptyTransactions(List<LedgerTransactionPM> transactions)
        {
            if (transactions == null || transactions.Count == 0)
                throw new Exception("PutDelSertDraftLedgerTransaction expected a list !");
        }

        public HttpResponseMessage PutAutomaticReconcileByFilter(string gLAccountId, int tenant, FilteredReconciliation myFilteredReconciliation)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);


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
                int tenant = AuthinticateTenant();

                QueryOperations queryOperations = buildQueryOperationsForLedgerTransactions(filters, tenant);


                LedgerTransactionListQueryService transactionQuery = new LedgerTransactionListQueryService(AccountingContext.GetContext(tenant));
                transactionQuery.displayNotReconciledOnly = true;
                GenericCallBack callback = transactionQuery.GetReconciliationFilterCallBack(queryOperations, gLAccountId, tenant, true);


                List<LedgerTransactionList> openTransactions = transactionQuery.GetOpenReconciliationFilterList(queryOperations, callback, gLAccountId, tenant);

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = callback.TotalRecord;
                    response.Count = count;
                }

                response.Result = openTransactions;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

      
        [HttpGet]
        public HttpResponseMessage GetReconciliationsByFilter(string gLAccountId, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = AuthinticateTenant();

                QueryOperations queryOperations = buildQueryOperationsForLedgerTransactions(filters, tenant);


                string TransferGlAccountId = GetAndRemoveFilter(queryOperations, "DUMMY_TransferAccountId");


                LedgerTransactionListQueryService transactionQuery = new LedgerTransactionListQueryService(AccountingContext.GetContext(tenant));
                List<LedgerTransactionList> openReconciliation = transactionQuery.GetOpenLedgerTransactions(queryOperations, gLAccountId, TransferGlAccountId, tenant);
                int transactionsCount = transactionQuery.GetOpenLedgerTransactionsCount(queryOperations, gLAccountId, TransferGlAccountId, tenant);

                //GenericCallBack callback = transactionQuery.GetReconciliationFilterCallBack(queryOperations, gLAccountId, tenant, false);
                //GenericCallBack callback_transfer = transactionQuery.GetExternalReconciliationFilterCallBack(queryOperations, TransferGlAccountId, tenant);


                //List<LedgerTransactionList> openReconciliation = transactionQuery.GetReconciliationFilterList(queryOperations, callback, gLAccountId, tenant);
                //List<LedgerTransactionList> openReconciliation_transfer = transactionQuery.GetReconciliationFilterListForTransferGLAccount(queryOperations, callback_transfer, TransferGlAccountId, tenant);

                //openReconciliation = openReconciliation.Concat(openReconciliation_transfer).ToList();

                //openReconciliation = openReconciliation.OrderByDescending(d => d.DocumentDate).ToList();

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                    response.Count = transactionsCount;

                response.Result = openReconciliation;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static string GetFilter(ApiQueryFilters filters, string fieldName)
        {
            JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
            var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

            var filter = filters_list.Find(d => d.FieldName == fieldName);
            string value = filter?.FieldValue.ToString();
            return value;
        }
        private static string GetAndRemoveFilter(QueryOperations queryOperations, string fieldName)
        {
            QueryFilterItem filterItem = queryOperations.QueryFilterItems.Find(d => d.FieldName == fieldName);
            string TransferGlAccountId = filterItem?.FieldValue.ToString();
            queryOperations.QueryFilterItems.Remove(filterItem);
            return TransferGlAccountId;
        }


        private static int AuthinticateTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

            int tenant = authToken.Tenant;
            return tenant;
        }

        private static QueryOperations buildQueryOperationsForLedgerTransactions(ApiQueryFilters filters, int tenant)
        {
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

                        object value1;
                        if (valuestring1 == "#today")
                        {
                            var today = TenantServerConfigration.GetCurrentDateTime(tenant);
                            value1 = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0);
                        }
                        else
                        {
                            value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                        }



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
            return queryOperations;
        }

        [HttpGet]
        public HttpResponseMessage GetAutomaticReconcileByFilter(string method1, string method2, string method3, string gLAccountId, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();

                QueryOperations queryOperations = GetQueryOperationsFromFilter(filters, tenant);

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
                    ClientChooseAutoMethod = true,
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

        [HttpGet]
        public HttpResponseMessage GetFirst500LedgerForReconciliation(string gLAccountId, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                int tenant = GetAuthinticatedTenant();
                QueryOperations queryOperations = GetQueryOperationsFromFilter(filters, tenant);

                LedgerTransactionListQueryService transactionQuery = new LedgerTransactionListQueryService(AccountingContext.GetContext(tenant));

                GenericCallBack callback = transactionQuery.GetReconciliationFilterCallBack(queryOperations, gLAccountId, tenant, true);


                List<LedgerTransactionList> openTransactions = transactionQuery.GetOpenReconciliationFilterList(queryOperations, callback, gLAccountId, tenant);

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = callback.TotalRecord;
                    response.Count = count;
                }

                response.Result = openTransactions.Take(500);
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static int GetAuthinticatedTenant()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            int tenant = authToken.Tenant;
            return tenant;
        }

        private static QueryOperations GetQueryOperationsFromFilter(ApiQueryFilters filters, int tenant)
        {
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

            return queryOperations;
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
                List<LedgerTransactionPM> draftTransactions = transactionQuery.DraftLedgerTransactionPMsByAccountId(gLAccountId, tenant);


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
                var pm = createJournalReconcileService
                    .Create(
                    accountingContext, authToken.Tenant
                    , ReconciliationLines, TheAccountId, AdjustAccountId,
                    AccountDate, Ref1, Ref2, Ref3, Remarks);

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, pm);


                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetByNumber(string number)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;

                ReconciliationQueryService query = new ReconciliationQueryService(tenant);
                ReconciliationPM reco = query.GetByNumber(number, tenant);


                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, reco);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSingleWithoutLines(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Reconciliation", "READ", authToken.Tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                ReconciliationQueryService reconciliationQuery = new ReconciliationQueryService(MyContext);
                reconciliationQuery.InitializeSettings();
                ReconciliationPM reconciliationPM = reconciliationQuery.GetSingle(id, false, false);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, reconciliationPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostReconcileExcelData(ReconcileExcelDataArgs args)
        {
            SecurityUtility.AuthenticationOnTenant(args.Tenant);

            ExportToExcelArgs excelArgs = BuildExportToExcelArguments(args);
            var excelFile = new ExportToExcelHelper().ExportDataToExcel(excelArgs);
            BlobFileInfo fileInfo = SaveFileToStorage(args, excelFile);

            return Request.CreateResponse(HttpStatusCode.OK, fileInfo.FileName);
        }

        private static BlobFileInfo SaveFileToStorage(ReconcileExcelDataArgs args, byte[] excelFile)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = "DraftReconciliation-" + DateTime.Now.ToShortDateString(),
                FolderName = "others",
                Extension = "xls",
                Tenant = args.Tenant,
                FileSize = excelFile.Length,
            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(excelFile, fileInfo);
            return fileInfo;
        }

        private static ExportToExcelArgs BuildExportToExcelArguments(ReconcileExcelDataArgs args)
        {
            return new ExportToExcelArgs()
            {
                Data = args.Data.GetEnumerator(),
                QueryColumns = args.QueryColumns,
                Tenant = args.Tenant,
                QueryPM = new QueryPM()
                {
                    ObjectTableName = "Reconciliation",
                    DisplayText = "Draft Reconciliation",
                    Tenant = args.Tenant
                }
            };
        }
    }



    public class OpenReconciliationAggregate
    {
        public GenericCallBack CallBack { get; set; }

        public List<LedgerTransactionList> OpenReconciliation { get; set; }
        public List<LedgerTransactionList> OpenReconciliationDraft { get; set; }
    }


    public class ReconcileExcelDataArgs
    {
        public List<LedgerTransactionPM> Data { get; set; }
        public List<QueryColumnPM> QueryColumns { get; set; }
        public int Tenant { get; set; }
    }
}