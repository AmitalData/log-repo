using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
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
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalInvoiceController : ApiController
    {
        [HttpGet]
        [Route("DigitalInvoice/GetDigitalShipmentARInvoicesCharges")]
        public IHttpActionResult GetDigitalShipmentARInvoicesCharges(string shipmentId, string cardId)
        {
            var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
            var tenant = authToken.Tenant;
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            //SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);
            var rep = new ShipmentRepository(tenant);
            var shipment = rep.GetSingleShipment(shipmentId, tenant);
           // CheckSharedContactAuthenticationForShipment(shipment.AgentId, shipment.CustomerId, tenant);
            var resultClass = new ShipmentARInvoiceMoneyPM() { Id = "1-1" };
            var arInvoices = new List<ShipmentARInvoicePM>();
            var aRCharges = new List<ARInvoiceChargePM>();
            var arInvoiceReps = new ARInvoiceRepository(tenant);
            var aRInvoiceLineQuery = new ARInvoiceLineQuery(tenant);
            var invoices = arInvoiceReps.GetInvoicesByShipmentIdAndBillToId(shipmentId, cardId, tenant);
            var lines = new List<ARInvoiceLinePM>();
            var currencyRepository = new CurrencyRepository(tenant);
            var aRInvoiceStatusRepository = new ARInvoiceStatusRepository(tenant);
            var documentOutQuery = new DocumentOutQuery(tenant);
            var documentTypeQuery = new DocumentTypeQuery(tenant);

            foreach (var item in invoices.Where(d => d.IsPrinted))
            {
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
                    StatusName = item.Status?.Name,
                };

                entity.ReportUrl = GetDocumntURL(item, documentOutQuery, documentTypeQuery);

                var currency = currencyRepository.GetSingleCurrency(item.InvoiceCurrencyId, tenant);
                if (currency != null)
                {
                    entity.InvoiceCurrencyCode = currency.Code;
                }

                var localCurrency = currencyRepository.GetSingleCurrency(item.LocalCurrencyId, tenant);
                entity.InvoiceLocalCurrencyCode = localCurrency?.Code;

                entity.StatusName = aRInvoiceStatusRepository.GetSingleARInvoiceStatus(item.StatusCode)?.Name;

                if (entity.Id == entity.InvoiceNumber)
                {
                    entity.InvoiceNumber = item.DraftNumber + " (Draft)";
                }

                arInvoices.Add(entity);
            }

            var myId = 0;
            aRCharges = (from d in lines
                         group d by new
                         {
                             d.Description,
                             d.InvoiceCurrencyCode,
                             d.InvoiceLocalCurrencyCode
                         } into g

                         select new ARInvoiceChargePM()
                         {
                             Id = ++myId,
                             Description = g.Key.Description,
                             InvoiceCurrencyCode = g.Key.InvoiceCurrencyCode,
                             LocalCurrencyCode = g.Key.InvoiceLocalCurrencyCode,
                             AmountInInvoiceCurrency = g.Sum(s => s.InvoiceCurrencyAmount),
                             AmountInLocalCurrency = g.Sum(s => s.LocalCurrencyAmount)
                         }).ToList();

            resultClass.ARInvoices = arInvoices;
            resultClass.ARCharges = aRCharges;
            resultClass.IsShowAmountLocalCurrencyColumnInSharedLogistics = GetIsShowAmountLocalCurrencyColumnInSharedLogistics(tenant);
            return Ok(resultClass);
        }

        [HttpGet]
        [Route("DigitalInvoice/GetFilteredDigitalARPayments")]
        public IHttpActionResult GetFilteredDigitalARPayments(string arInvoiceId)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            var tenant = authToken.Tenant;
            SecurityUtility.AuthenticationOnTenant(tenant);

            var result = new List<ARPaymentList>();
            var statusRepository = new ARPaymentStatusRepository(tenant);
            var currencyRepository = new CurrencyRepository(tenant);
            var aRPaymentRepository = new ARPaymentRepository(tenant);
            var aRInvoicePaymentQuery = new ARInvoicePaymentQuery(tenant);
            var methodRepository = new AccountingPaymentMethodRepository(tenant);
            var invoicePayments = aRInvoicePaymentQuery.GetARInvoicePaymentPMsForInvoice(arInvoiceId, tenant);

            foreach (var item in invoicePayments)
            {
                var payment = aRPaymentRepository.GetSingleARPayment(item.ARPaymentId, tenant);

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

                    var currency = currencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, tenant);
                    if (currency != null)
                    {
                        list.PaymentCurrencyCode = currency.Code;
                    }

                    var status = statusRepository.GetSingleARPaymentStatus(payment.StatusCode);
                    if (status != null)
                    {
                        list.StatusName = status.Name;
                    }

                    var method = methodRepository.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant);
                    if (method != null)
                    {
                        list.PaymentMethodName = method.Name;
                    }

                    result.Add(list);
                }
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("DigitalInvoice/GetSingle")]
        public IHttpActionResult GetSingle(string id, string cardId)
        {
            var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

            var entityQuery = new ARInvoiceQuery(authToken.Tenant);

            var entityPM = entityQuery.GetSinglePM(id, authToken.Tenant);

            string documentTypeCode = GetDocumentTypeCodeByInvoiceType(entityPM.ARInvoiceTypeCode);
            var documentOutQuery = new DocumentOutQuery(authToken.Tenant);
            var query = new DocumentTypeQuery(authToken.Tenant);
            var docType = query.GetSinglePMByCodeAndTenant(documentTypeCode, authToken.Tenant);

            var docsOutData = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityPM.MainEntityId, entityPM.Id, docType.Id, authToken.Tenant);
            
            if (docsOutData != null)
            {
                var docId = docsOutData.Id;

                if (docsOutData.DocumentOutCopies.Count() > 0)
                {
                    docId = docsOutData.DocumentOutCopies.FirstOrDefault().DocumentId;
                }

                string url = "../WebPages/SharedDownloadPage.aspx?id=" + authToken.Tenant + ":" + docId + ":invc:" + entityPM.Id;
                entityPM.ReportUrl = url;
            }

            entityPM.IsShowAmountLocalCurrencyColumnInSharedLogistics = GetIsShowAmountLocalCurrencyColumnInSharedLogistics(authToken.Tenant);

            CheckSharedContactAuthenticationForInvoice(entityPM.BillToId, authToken.Tenant);

            return Ok(entityPM);
        }

        [HttpPost]
        [Route("DigitalInvoice/GetByFilters")]
        public IHttpActionResult GetByFilters(GeneralFilters newFilters)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, newFilters.CardId);
                var myTenantRepository = new TenantRepository(authToken.Tenant);
                var myTenant = myTenantRepository.GetSingleTenant(authToken.Tenant);

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

                if (!string.IsNullOrWhiteSpace(newFilters.CardId))
                {
                    queryOperations.SetFilter("BillToId", newFilters.CardId, false, "InList", null, false);
                }

                var ARInvoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARInvoice", authToken.Tenant);

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
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
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

                var customfilters = new ARInvoiceCustomFilter(authToken.Tenant);
                entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);
                entityPocos = ARInvoiceAPiHelper.ApplyFilters(entityPocos, authToken.Tenant);
                entityPocos = genericFilter.GetFilteredQuery(nonListQueryOperation, entityPocos);

                var entityLists = aRInvoiceQuery.GetIQueryableEntityList(entityPocos);
                entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

                if (!string.IsNullOrWhiteSpace(queryOperations.SortByColumnName) && !string.IsNullOrWhiteSpace(queryOperations.SortDirectin))
                {
                    ObjectField objectField = ARInvoiceObjectFields.FirstOrDefault( a => a.FieldName == queryOperations.SortByColumnName);

                    if (objectField != null)
                    {
                        var sortClass = new GenericSort();

                        if (!objectField.IsCustom)
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "text":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ARInvoiceList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ARInvoiceList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ARInvoiceList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "lookup":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ARInvoiceList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                default:
                                    {
                                        entityLists = entityLists.OrderByDescending(d => d.InvoiceDate);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            entityLists = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, entityLists);
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderByDescending(d => d.InvoiceDate);
                }

                var res = entityLists.GetPaged(queryOperations.PageIndex, queryOperations.PageSize);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        #region private 

        private void CheckAuthentication(string token, int tenant)
        {
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
        }

        private string GetDocumentTypeCodeByInvoiceType(string aRInvoiceTypeCode)
        {
            string code = "999S";

            if (aRInvoiceTypeCode == "CI")
            {
                code = "999CI";
            }

            return code;
        }
        
        private bool CheckSharedContactAuthenticationForInvoice(string partnerId, int tenant)
        {
            if (tenant != 0)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                bool exists = false;
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    string email = HttpContext.Current.User.Identity.Name;

                    ContactRepository contactrep = new ContactRepository(commonDataContext);
                    Contact contact = contactrep.GetSingleContactByEmail(email, tenant);

                    if (contact != null)
                    {
                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == contact.Id && d.CardId == partnerId).FirstOrDefault();
                        if (cardContact != null)
                        {
                            exists = true;
                        }
                    }
                }
                
                if (!exists)
                {
                    throw new AutenticationException("Sorry! you are not authorized to read data!");
                }

                return exists;
            }

            return true;
        }

        private bool CheckSharedContactAuthenticationForShipment(string agentId, string customerId, int tenant)
        {
            if (tenant != 0)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                bool exists = false;
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                    string email = HttpContext.Current.User.Identity.Name;

                    ContactRepository contactrep = new ContactRepository(commonDataContext);
                    Contact contact = contactrep.GetSingleContactByEmail(email, tenant);

                    if (contact != null)
                    {
                        CardContact cardContact = commonDataContext.CardContacts.Where(d => d.ContactId == contact.Id && (d.CardId == customerId || d.CardId == agentId)).FirstOrDefault();
                        if (cardContact != null)
                        {
                            exists = true;

                        }
                    }
                }

                if (!exists)
                {
                    throw new AutenticationException("Sorry! you are not authorized to read data!");
                }

                return exists;
            }

            return true;
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
        
        private string GetDocumntURL(ARInvoice item, DocumentOutQuery documentOutQuery, DocumentTypeQuery documentTypeQuery)
        {
            string reportUrl = null;
            var tenant = item.Tenant;
            var documentTypeCode = this.GetDocumentTypeCodeByInvoiceType(item.ARInvoiceTypeCode);
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

            string url = "../WebPages/SharedDownloadPage.aspx?id=" + tenant + ":" + docId + ":invc:" + item.Id;
            reportUrl = url;

            return reportUrl;
        }

        #endregion private
    }
}