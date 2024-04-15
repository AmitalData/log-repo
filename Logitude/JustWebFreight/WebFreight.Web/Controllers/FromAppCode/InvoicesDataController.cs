using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel;
using Logitude.BL.InvoiceModel.CustomFilters;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;

namespace WebFreight.Web.App_Code
{
    public class InvoicesDataController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public ShipmentARInvoiceMoneyPM GetShipmentARInvoicesCharges(string shipmentId, string cardId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentRepository rep = new ShipmentRepository(tenant);
            Shipment shipment=  rep.GetSingleShipment(shipmentId, tenant);

            CheckSharedContactAuthenticationForShipment(shipment.AgentId, shipment.CustomerId, tenant);



            ShipmentARInvoiceMoneyPM resultClass = new ShipmentARInvoiceMoneyPM() { Id = "1-1" };
            List<ShipmentARInvoicePM> arInvoices = new List<ShipmentARInvoicePM>();
            List<ARInvoiceChargePM> aRCharges = new List<ARInvoiceChargePM>();

            ARInvoiceRepository arInvoiceReps = new ARInvoiceRepository(tenant);
            //ARInvoiceLineRepository arInvoiceLineReps = new ARInvoiceLineRepository(tenant);
            ARInvoiceLineQuery aRInvoiceLineQuery = new ARInvoiceLineQuery(tenant);

            List<ARInvoice> invoices = arInvoiceReps.GetInvoicesByShipmentIdAndBillToId(shipmentId, cardId, tenant);

            List<ARInvoiceLinePM> lines = new List<ARInvoiceLinePM>();

            foreach (ARInvoice item in invoices.Where(d=>d.IsPrinted))
            {
                List<ARInvoiceLinePM> itemLines = aRInvoiceLineQuery.GetInvoiceLinePMsByInvoiceId(item.Id, tenant).ToList();
                lines.AddRange(itemLines);

                Currency invoicecurrency = CurrencyRepository.GetSingleCurrency(item.InvoiceCurrencyId, item.Tenant, true);
                ShipmentARInvoicePM entity = new ShipmentARInvoicePM()
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
                };

                CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
                Currency currency = currencyRepository.GetSingleCurrency(item.InvoiceCurrencyId, tenant);
                if (currency != null)
                {
                    entity.InvoiceCurrencyCode = currency.Code;
                }

                if (entity.Id == entity.InvoiceNumber)
                {
                    entity.InvoiceNumber = item.DraftNumber + " (Draft)";
                }

                arInvoices.Add(entity);
            }

            int myId = 0;


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
            return resultClass;
        }

        public List<ARInvoiceList> PostFilteredARInvoices(int tenant, InvoiceFilters filters)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            SecurityUtility.CheckSharedContactAuthentication(tenant, filters.PartnerId);
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

            QueryOperations queryOperations = new QueryOperations();
            queryOperations.SetFilter("IsPrinted", true, false, "Equals", null, false);
            queryOperations.SetFilter("BillToId", filters.PartnerId, false, "Equals", null, false);
            queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, false);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            InvoiceCustomFilter customfilters = new InvoiceCustomFilter(tenant);

            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);
            IQueryable<ARInvoice> invoices = aRInvoiceRepository.GetIQueryableInvoices(tenant);

            invoices = customfilters.GetFilteredQuery(queryOperations, invoices); 

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            invoices = filter.GetFilteredQuery<ARInvoice>(nonListQueryOperation, invoices);
            int skippedShipments = queryOperations.PageIndex; 

            var query2 = from entity in invoices
                         select new ARInvoiceList()
                         {
                             IsClosed = entity.IsClosed,
                             BillToAddressId = entity.BillToAddressId,
                             BillToId = entity.BillToId,
                             VatNumber = entity.VatNumber,
                             CancelledByARInvoiceId = entity.CancelledByARInvoiceId,
                             DueDate = entity.DueDate,
                             AmountInInvoiceCurrency = entity.AmountInInvoiceCurrency,
                             AmountInLocalCurrency = entity.AmountInLocalCurrency,
                             Id = entity.Id,
                             InternalNotes = entity.InternalNotes,
                             InvoiceCurrencyId = entity.InvoiceCurrencyId,
                             InvoiceDate = entity.InvoiceDate,
                             InvoiceNumber = entity.InvoiceNumber,
                             StatusCode = entity.StatusCode,
                             InvoiceCurrencyExchangeRate = entity.InvoiceCurrencyExchangeRate,
                             ARInvoiceTypeCode = entity.ARInvoiceTypeCode,
                             IsAutoCredit = entity.IsAutoCredit,
                             IsCancelled = entity.IsCancelled,
                             IssuedByUserId = entity.IssuedByUserId,
                             LocalCurrencyId = entity.LocalCurrencyId,
                             PrintNotes = entity.PrintNotes,
                             PrintByUserId = entity.PrintByUserId,
                             PrintDate = entity.PrintDate,
                             SubTotalInInvoiceCurrency = entity.SubTotalInInvoiceCurrency,
                             SubTotalInLocalCurrency = entity.SubTotalInLocalCurrency,
                             Tenant = entity.Tenant,
                             BillToName = entity.BillTo.EnglishName,
                             BillToPartnerName = entity.BillTo.PartnerType.Name,
                             BillToPartnerId = entity.BillTo.PartnerTypeId,
                             CreateDate = entity.CreateDate,
                             CreatedByUserName = entity.CreatedByUser.Contact.EnglishName,
                             InvoiceCurrencyCode = entity.InvoiceCurrency.Code,
                             StatusName = entity.Status.Name,
                             ARInvoiceTypeName = entity.ARInvoiceType.Name,
                             CreatedByUserId = entity.CreatedByUserId,
                             IssuedByUserName = entity.IssuedByUser != null ? entity.IssuedByUser.Contact.EnglishName : null,
                             LocalCurrencyCode = entity.LocalCurrency.Code,
                             SearchFields = entity.SearchFields,
                             PrintByUserName = entity.PrintByUser != null ? entity.PrintByUser.Contact.EnglishName : null,
                             MainEntityReference = entity.MainEntityReference,
                             Sent = entity.Sent,
                             PaymentTermId = entity.PaymentTermId,
                             PaymentTermName = entity.PaymentTerm != null ? entity.PaymentTerm.EnglishName : null,
                             IsInvoiceNumberManuallySet = entity.IsInvoiceNumberManuallySet,
                             AmountDue = entity.AmountDue,
                             ExpectedPaymentDate = entity.ExpectedPaymentDate,
                             DraftNumber = entity.DraftNumber,
                             IsPrinted = entity.IsPrinted,
                             HasDoc = entity.DocumentFilingId != null ? true : false,
                             ProfitCurrencyCode = entity.ProfitCurrency != null ? entity.ProfitCurrency.Code : null,
                             AmountDueInLocalCurrency = entity.AmountDueInLocalCurrency,
                             AmountDueInProfitCurrency = entity.AmountDueInProfitCurrency,
                             Field1 = entity.Field1,
                             Field2 = entity.Field2,
                             Field3 = entity.Field3,
                             Field4 = entity.Field4,
                             Field5 = entity.Field5,
                             Field6 = entity.Field6,
                             Field7 = entity.Field7,
                             Field8 = entity.Field8,
                             Field9 = entity.Field9,
                             Field10 = entity.Field10,
                             TransferStatusCode = entity.TransferStatusCode,
                             TransferStatusName = entity.TransferStatus == null ? "" : entity.TransferStatus.Name,
                             ReadyForTransfer = entity.TransferStatusCode == "RD",
                             DebitAccount = entity.DebitAccount,
                             TransferError = entity.TransferError,
                             AmountInProfitCurrency = entity.AmountInProfitCurrency,
                             AccountingExternalCode = entity.AccountingExternalCode,
                         };

            query2 = filter.GetFilteredQuery<ARInvoiceList>(listQueryOperation, query2);

            query2 = query2.Where(d => d.StatusCode != "VD");

            if (filters.FilterName == "UnpaidInvoices")
            {
                query2 = query2.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && !d.IsAutoCredit && !d.IsCancelled && d.IsClosed == false);
            }
             

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARInvoiceList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> invoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARInvoice", tenant).ToList();

                ObjectField objectField = (from a in invoiceObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ARInvoiceList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.InvoiceDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.InvoiceDate);
            }

            query2 = query2.Skip(0);
            query2 = query2.Take(filters.PageSize);

            List<ARInvoiceList> listQuery = query2.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("ARInvoice", tenant, listQuery.Cast<object>().ToList());

            return listQuery;
        }

        public List<ARPaymentList> GetFilteredARPayments(string arInvoiceId,int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<ARPaymentList> result = new List<ARPaymentList>();

            ARPaymentStatusRepository statusRepository = new ARPaymentStatusRepository(tenant);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(tenant);
            ARInvoicePaymentQuery aRInvoicePaymentQuery = new ARInvoicePaymentQuery(tenant);
            AccountingPaymentMethodRepository methodRepository = new AccountingPaymentMethodRepository(tenant);

            List<ARInvoicePaymentPM> invoicePayments = aRInvoicePaymentQuery.GetARInvoicePaymentPMsForInvoice(arInvoiceId, tenant);

            foreach (ARInvoicePaymentPM item in invoicePayments)
            {
                ARPayment payment = aRPaymentRepository.GetSingleARPayment(item.ARPaymentId, tenant);

                if (payment != null)
                {
                    ARPaymentList list = new ARPaymentList()
                    {
                        Id = payment.Id,
                        Tenant = payment.Tenant,
                        PaymentNo = payment.PaymentNo,
                        AmountInPaymentCurrency = payment.AmountInPaymentCurrency,
                        OpenAmount = payment.OpenAmount,
                        ChequeOrPaymentRef  = payment.ChequeOrPaymentRef,
                        CreateDate = payment.CreateDate,
                    };

                    Currency currency = currencyRepository.GetSingleCurrency(payment.PaymentCurrencyId,tenant);
                    if (currency != null)
                    {
                        list.PaymentCurrencyCode = currency.Code;
                    }

                    ARPaymentStatus status = statusRepository.GetSingleARPaymentStatus(payment.StatusCode);
                    if (status != null)
                    {
                        list.StatusName = status.Name;
                    }

                    AccountingPaymentMethod method = methodRepository.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant);
                    if (method != null)
                    {
                        list.AccountingPaymentMethodName = method.Name;
                    }

                    result.Add(list);
                }
            }

            return result;
        }

        public ARInvoicePM GetSingleARInvoicePM(string invoiceId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceQuery entityQuery = new ARInvoiceQuery(tenant);
            ARInvoicePM entityPM = entityQuery.GetSinglePM(invoiceId, tenant);

            string documentTypeCode = this.GetDocumentTypeCodeByInvoiceType(entityPM.ARInvoiceTypeCode);
            DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
            DocumentTypeQuery query = new DocumentTypeQuery(tenant);
            DocumentTypePM docType = query.GetSinglePMByCodeAndTenant(documentTypeCode, tenant);

            string docId = "";
            DocumentOutPM docsOutData = documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityPM.MainEntityId, entityPM.Id, docType.Id, tenant);
            if (docsOutData != null)
            {
                docId = docsOutData.Id;
                if (docsOutData.DocumentOutCopies.Count() > 0)
                {
                    docId = docsOutData.DocumentOutCopies.FirstOrDefault().DocumentId;
                }
            }            
            
            string documentName = tenant + "_" + docId;
            string url = "../WebPages/SharedDownloadPage.aspx?id=" + tenant + ":" + docId + ":invc:" + entityPM.Id;
            entityPM.ReportUrl = url;
            entityPM.IsShowAmountLocalCurrencyColumnInSharedLogistics = GetIsShowAmountLocalCurrencyColumnInSharedLogistics(tenant);

            CheckSharedContactAuthenticationForInvoice(entityPM.BillToId, tenant);
          
            return entityPM;
        }
        private string GetDocumentTypeCodeByInvoiceType(string aRInvoiceTypeCode)
        {
            string code = "";

            if(aRInvoiceTypeCode == "CI")
            {
                code = "999CI";
            }

            else
            {
                code = "999S";
            }

            return code;
        }

        private  bool GetIsShowAmountLocalCurrencyColumnInSharedLogistics(int tenant)
        {
            bool isShowAmountLocalCurrencyColumnInSharedLogistics = false;
            SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            SharedLogisticsSetting sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
            if (sharedLogisticsSetting != null)
            {
                isShowAmountLocalCurrencyColumnInSharedLogistics = sharedLogisticsSetting.IsShowAmountLocalCurrency;
            }

            return isShowAmountLocalCurrencyColumnInSharedLogistics;
        }

        public  bool CheckSharedContactAuthenticationForInvoice(string partnerId, int tenant)
        {
            if (tenant != 0)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                bool exists = false;
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {//using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //}
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

        public bool CheckSharedContactAuthenticationForShipment(string agentId, string customerId, int tenant)
        {
            if (tenant != 0)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                bool exists = false;
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {//using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //}
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
    }
}