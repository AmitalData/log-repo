using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InvoiceModel.CustomFilters;
using Logitude.Infrastructure.Data.Repsitories;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using WebFreight.Web.Controllers.InvoiceModel.ApiHelpers;
using Simplog.Data.InvoiceModel;
using Logitude.Extensions;
using WebFreight.Web.Controllers.DigitalPortal.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Logitude.SystemLogs;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalInvoiceController : ApiController
    {
        [HttpGet]
        [Route("DigitalInvoice/GetDigitalShipmentARInvoicesCharges")]
        public IHttpActionResult GetDigitalShipmentARInvoicesCharges(string shipmentId, string cardId)
        {
            int tenant = 0;
            string email = "";

            try
            {
                var digitalPortalAuthenticationHelper = new DigitalPortalAuthenticationHelper();
                var shipmentIdAndTenant = digitalPortalAuthenticationHelper.AuthenticateResponse(cardId, shipmentId, true);
                shipmentId = shipmentIdAndTenant.Item1;
                tenant = shipmentIdAndTenant.Item2;
                email = shipmentIdAndTenant.Item3;

                var rep = new ShipmentRepository(tenant);
                var shipment = rep.GetSingleShipment(shipmentId, tenant);
                var resultClass = new ShipmentARInvoiceMoneyPM() { Id = "1-1" };
                var arInvoices = new List<ShipmentARInvoicePM>();
                var arInvoiceReps = new ARInvoiceRepository(tenant);
                var aRInvoiceLineQuery = new ARInvoiceLineQuery(tenant);
                var invoices = arInvoiceReps.GetDigitalInvoicesByShipmentIdAndBillToId(shipmentId, cardId, tenant);
                var lines = new List<ARInvoiceLinePM>();
                var currencyRepository = new CurrencyRepository(tenant);
                var aRInvoiceStatusRepository = new ARInvoiceStatusRepository(tenant);
                var documentOutQuery = new DocumentOutQuery(tenant);
                var documentTypeQuery = new DocumentTypeQuery(tenant);
                var aRInvoiceTypeRepository = new ARInvoiceTypeRepository(tenant);
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

                foreach (var item in invoices.Where(d => d.IsPrinted))
                {
                    if(item.IsConstituentInvoice && string.IsNullOrEmpty(item.ConsolidationInvoiceId))
                    {
                        continue;
                    }

                    var itemLines = aRInvoiceLineQuery.GetInvoiceLinePMsByInvoiceId(item.Id, tenant).ToList();
                    lines.AddRange(itemLines);
                    var invoicecurrency = CurrencyRepository.GetSingleCurrency(item.InvoiceCurrencyId, item.Tenant, true);
                    var entity = new ShipmentARInvoicePM()
                    {
                        Id = item.Id,
                        ShipmentId = shipmentId,
                        InvoiceNumber = item.InvoiceNumber,
                        InvoiceCurrencyId = item.InvoiceCurrencyId,
                        DueDate = item.DueDate,
                        StatusCode = item.StatusCode,
                        InvoiceTypeCode = item.ARInvoiceTypeCode,
                        AmountInLocalCurrency = item.AmountInLocalCurrency,
                        AmountInProfitCurrency = item.AmountInProfitCurrency,
                        AmountInInvoiceCurrency = item.AmountInInvoiceCurrency,
                        AmountDue = item.AmountDue,
                        IsAutoCredit = item.IsAutoCredit,
                        IsCancelled = item.IsCancelled,
                        InvoiceDate = item.InvoiceDate,
                        StatusName = item.PaidStatus,
                        IsDigitalDueDateColorRed = (item.DueDate == null || item.PaidStatus == "Paid") ? false : (item.DueDate.Value < todayDate ? true : false),
                        ConsolidationInvoiceId = item.ConsolidationInvoiceId,
                        IsConstituentInvoice = item.IsConstituentInvoice,
                    };

                    if (!string.IsNullOrEmpty(entity.ConsolidationInvoiceId))
                    {
                        entity.ConsolidationInvoiceNumber = arInvoiceReps.GetInvoiceNumber(entity.ConsolidationInvoiceId, tenant);
                    }

                    entity.ReportUrl = GetDocumntURL(item, documentOutQuery, documentTypeQuery, cardId);
                    var currency = currencyRepository.GetSingleCurrency(item.InvoiceCurrencyId, tenant);
                    if (currency != null)
                    {
                        entity.InvoiceCurrencyCode = currency.Code;
                    }

                    var localCurrency = currencyRepository.GetSingleCurrency(item.LocalCurrencyId, tenant);
                    entity.InvoiceLocalCurrencyCode = localCurrency?.Code;
                    if (entity.Id == entity.InvoiceNumber)
                    {
                        entity.InvoiceNumber = item.DraftNumber + " (Draft)";
                    }

                    var invoiceType = aRInvoiceTypeRepository.GetSingleARInvoiceType(entity.InvoiceTypeCode);
                    entity.InvoiceTypeName = invoiceType?.Name;
                    arInvoices.Add(entity);
                }

                var myId = 0;
                resultClass.ARInvoices = arInvoices;
                resultClass.ARCharges = lines.GroupBy(d => new
                {
                    d.Description,
                    d.InvoiceCurrencyCode,
                    d.InvoiceLocalCurrencyCode
                })
                .Select(g => new ARInvoiceChargePM()
                {
                    Id = ++myId,
                    Description = g.Key.Description,
                    InvoiceCurrencyCode = g.Key.InvoiceCurrencyCode,
                    LocalCurrencyCode = g.Key.InvoiceLocalCurrencyCode,
                    AmountInInvoiceCurrency = g.Sum(s => s.InvoiceCurrencyAmount),
                    AmountInLocalCurrency = g.Sum(s => s.LocalCurrencyAmount)
                }).ToList();

                resultClass.IsShowAmountLocalCurrencyColumnInSharedLogistics = GetIsShowAmountLocalCurrencyColumnInSharedLogistics(tenant);
                return Ok(resultClass);
            }
            catch (AutenticationException ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpGet]
        [Route("DigitalInvoice/GetFilteredDigitalARPayments")]
        public IHttpActionResult GetFilteredDigitalARPayments(string arInvoiceId, string cardId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                var result = new List<ARPaymentList>();
                var statusRepository = new ARPaymentStatusRepository(authToken.Tenant);
                var currencyRepository = new CurrencyRepository(authToken.Tenant);
                var aRPaymentRepository = new ARPaymentRepository(authToken.Tenant);
                var aRInvoicePaymentQuery = new ARInvoicePaymentQuery(authToken.Tenant);
                var methodRepository = new AccountingPaymentMethodRepository(authToken.Tenant);
                var invoicePayments = aRInvoicePaymentQuery.GetARInvoicePaymentPMsForInvoice(arInvoiceId, authToken.Tenant);

                foreach (var item in invoicePayments)
                {
                    var payment = aRPaymentRepository.GetSingleARPayment(item.ARPaymentId, authToken.Tenant);

                    if (payment != null)
                    {
                        var list = new ARPaymentList()
                        {
                            Id = payment.Id,
                            Tenant = payment.Tenant,
                            PaymentNo = payment.PaymentNo,
                            AmountInPaymentCurrency = payment.AmountInPaymentCurrency,
                            OpenAmount = payment.OpenAmount,
                            ChequeOrPaymentRef = payment.ChequeOrPaymentRef,
                            CreateDate = payment.CreateDate,
                            PaidAmount = payment.AmountInPaymentCurrency - payment.OpenAmount
                        };

                        var currency = currencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, authToken.Tenant);
                        if (currency != null)
                        {
                            list.PaymentCurrencyCode = currency.Code;
                        }

                        var status = statusRepository.GetSingleARPaymentStatus(payment.StatusCode);
                        if (status != null)
                        {
                            list.StatusName = status.Name;
                        }

                        var method = methodRepository.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, authToken.Tenant);

                        if (method != null)
                        {
                            list.PaymentMethodName = method.Name;
                        }

                        result.Add(list);
                    }
                }

                return Ok(result);
            }
            catch (AutenticationException ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpGet]
        [Route("DigitalInvoice/GetSingle")]
        public IHttpActionResult GetSingle(string id, string cardId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                var entityQuery = new ARInvoiceQuery(authToken.Tenant);

                var entityPM = entityQuery.GetSinglePM(id, authToken.Tenant);

                string documentTypeCode = GetDocumentTypeCodeByInvoiceType(entityPM.ARInvoiceTypeCode, entityPM.IsConsolidationInvoice);
                var documentOutQuery = new DocumentOutQuery(authToken.Tenant);
                var query = new DocumentTypeQuery(authToken.Tenant);
                var docType = query.GetSinglePMByCodeAndTenant(documentTypeCode, authToken.Tenant);
                var mainEntityId = entityPM.MainEntityId;
                var childEntityId = entityPM.Id;
                if (entityPM.IsConsolidationInvoice)
                {
                    mainEntityId = entityPM.Id;
                    childEntityId = null;
                }

                var docsOutData = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(mainEntityId, childEntityId, docType.Id, authToken.Tenant);

                if (docsOutData != null)
                {
                    var docId = docsOutData.Id;

                    if (docsOutData.DocumentOutCopies.Count() > 0)
                    {
                        docId = docsOutData.DocumentOutCopies.FirstOrDefault().DocumentId;
                    }

                    string url = "../WebPages/SharedDownloadPage.aspx?id=" + authToken.Tenant + ":" + docId + ":invc:" + entityPM.Id + ":isFromDigital:true:cardId:" + cardId;
                    entityPM.ReportUrl = url;
                }

                entityPM.IsShowAmountLocalCurrencyColumnInSharedLogistics = GetIsShowAmountLocalCurrencyColumnInSharedLogistics(authToken.Tenant);
                return Ok(entityPM);
            }
            catch (AutenticationException ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPost]
        [Route("DigitalInvoice/GetByFilters")]
        public IHttpActionResult GetByFilters(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                newFilters.Tenant = authToken.Tenant;
                var aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                var entityLists = aRInvoiceQuery.GetByFilters(newFilters);
                var res = entityLists.GetPaged(newFilters.PageIndex, newFilters.PageSize);

                return Ok(res);
            }
            catch (AutenticationException ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPost]
        [Route("DigitalInvoice/GetByInvoicesCounters")]
        public IHttpActionResult GetByInvoicesCounters(GeneralFilters newFilters)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);

                var filters = new ApiQueryFilters()
                {
                    Filter1Value = newFilters.CardId,
                    Filter2Value = newFilters.CardType
                };

                var queryOperations = new QueryOperations()
                {
                    ObjectTableName = "ARInvoice",
                    PageIndex = newFilters.PageIndex,
                    PageSize = newFilters.PageSize,
                    QuerySection = "ARInvoices",
                    SortByColumnName = newFilters.SortBy,
                    SortDirectin = newFilters.SortDirection
                };

                queryOperations.SetFilter("IsPrinted", true, false, "Equals", null, false);
                queryOperations.SetFilter("IsConstituentInvoice", false, false, "Equals", null, false);

                var cardFilterValues = newFilters.CardId;
                if (!string.IsNullOrWhiteSpace(cardFilterValues))
                {
                    var cardBillToId = GetCardBillToId(newFilters.CardId, authToken.Tenant);
                    if (!string.IsNullOrWhiteSpace(cardBillToId))
                    {
                        cardFilterValues = cardFilterValues + "," + cardBillToId;
                        queryOperations.SetFilter("PartnerId", newFilters.CardId, false, "Equals", null, false);
                    }

                    queryOperations.SetFilter("BillToId", cardFilterValues, false, "InList", null, false);
                }

                var ARInvoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableNameWithNoIncludes("ARInvoice", authToken.Tenant);

                if (newFilters.AdditionalFilters.Any())
                {
                    foreach (var filter in newFilters.AdditionalFilters)
                    {
                        var field = ARInvoiceObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue?.ToString();
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
                            string valuestring2 = filter.FieldValue2?.ToString();
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                        }
                        else
                        {
                            var isParsedSuccess = bool.TryParse(filter.FieldValue, out bool parsedBool);

                            if (isParsedSuccess)
                            {
                                queryOperations.SetFilter(filter.FieldName, parsedBool, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);

                            }
                            else
                            {
                                queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                            }
                        }
                    }
                }

                ARInvoiceAPiHelper.AddFilters(queryOperations, authToken.Tenant);
                var genericFilter = new GenericFilter();
                var MyContext = InvoiceContext.GetContext(authToken.Tenant);

                var nonListQueryOperation = new QueryOperations
                {
                    QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList()
                };

                var listQueryOperation = new QueryOperations
                {
                    QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList()
                };

                var aRInvoiceRepository = new ARInvoiceRepository(MyContext);
                var aRInvoiceQuery = new ARInvoiceQuery(aRInvoiceRepository);

                var entityPocos = aRInvoiceRepository.GetARInvoices(authToken.Tenant);

                entityPocos = aRInvoiceRepository.FilterInvoicesStatusesForList(entityPocos);

                var customfilters = new ARInvoiceCustomFilter(authToken.Tenant);
                entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);
                entityPocos = ARInvoiceAPiHelper.ApplyFilters(entityPocos, authToken.Tenant);
                entityPocos = genericFilter.GetFilteredQuery(nonListQueryOperation, entityPocos);

                var entityLists = aRInvoiceQuery.GetIQueryableEntityList(entityPocos);
                entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

                var res = new InvoicesCounter
                {
                    MaxOpenAmount = entityLists.Max(a => a.AmountDue),
                    MinOpenAmount = entityLists.Min(a => a.AmountDue),
                    MaxTotalAmount = entityLists.Max(a => a.AmountInInvoiceCurrency),
                    MinTotalAmount = entityLists.Min(a => a.AmountInInvoiceCurrency),
                };


                return Ok(res);
            }
            catch (AutenticationException ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        private string GetCardBillToId(string cardId, int tenant)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            var cardBillToId = cardRepository.GetBillToCardById(cardId, tenant);
            return cardBillToId;
        }

        #region private 

        private string GetDocumentTypeCodeByInvoiceType(string aRInvoiceTypeCode, bool isConsolidationInvoice)
        {
            string code = "999S";

            if (isConsolidationInvoice)
            {
                code = "999C";
            }
            else if (aRInvoiceTypeCode.Equals("CI", StringComparison.InvariantCultureIgnoreCase))
            {
                code = "999CI";
            }

            return code;
        }

        private bool GetIsShowAmountLocalCurrencyColumnInSharedLogistics(int tenant)
        {
            bool isShowAmountLocalCurrencyColumnInSharedLogistics = false;

            var sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            var sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);

            if (sharedLogisticsSetting != null)
            {
                isShowAmountLocalCurrencyColumnInSharedLogistics = sharedLogisticsSetting.IsShowAmountLocalCurrency;
            }

            return isShowAmountLocalCurrencyColumnInSharedLogistics;
        }

        private string GetDocumntURL(ARInvoice item, DocumentOutQuery documentOutQuery, DocumentTypeQuery documentTypeQuery, string cardId)
        {
            string reportUrl = null;
            var tenant = item.Tenant;
            var documentTypeCode = this.GetDocumentTypeCodeByInvoiceType(item.ARInvoiceTypeCode, item.IsConsolidationInvoice);
            var docType = documentTypeQuery.GetSinglePMByCodeAndTenant(documentTypeCode, tenant);
            var docId = "";
            var docsOutData = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(item.MainEntityId, item.Id, docType.Id, tenant);
            if (docsOutData == null)
            {
                return null;
            }

            docId = docsOutData.Id;
            if (docsOutData.DocumentOutCopies.Count() > 0)
            {
                docId = docsOutData.DocumentOutCopies.FirstOrDefault().DocumentId;
            }

            string url = "../WebPages/SharedDownloadPage.aspx?id=" + tenant + ":" + docId + ":invc:" + item.Id+ ":isFromDigital:true:cardId:"+ cardId;
            reportUrl = url;

            return reportUrl;
        }

        #endregion private
    }
}