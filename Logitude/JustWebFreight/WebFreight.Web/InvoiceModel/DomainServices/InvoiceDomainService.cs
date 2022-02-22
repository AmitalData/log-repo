using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Helpers;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Logitude.BL.InvoiceModel.EntityLists;
using System.IO;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using System.Reflection;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InvoiceModel;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    // TODO: Create methods containing your application logic.
    //[RequiresAuthentication]
    [EnableClientAccess()]
    public partial class InvoiceDomainService : LogitudeDomainService
    {
        private IInvoiceContext objectContext;
        private ARInvoiceRepository aRInvoiceRepository;
        private APInvoiceRepository aPInvoiceRepository;
        private ARPaymentRepository aRPaymentRepository;
        private APPaymentRepository aPPaymentRepository;

        private AccountRepository accountRepository;
        private AccountTypeRepository accountTypeRepository;
        private AccountingPaymentMethodRepository paymentMethodRepository;
        private ARPaymentStatusRepository aRPaymentStatusRepository;
        private ARInvoiceLineRepository aRInvoiceLineRepository;
        private APInvoiceLineRepository aPInvoiceLineRepository;
        private ARInvoiceTotalVATRepository aRInvoiceTotalVATRepository;
        private APInvoiceTotalVATRepository aPInvoiceTotalVATRepository;
        private APInvoiceStatusRepository aPInvoiceStatusRepository;
        private APInvoiceTypeRepository aPInvoiceTypeRepository;
     
        private APPaymentStatusRepository aPPaymentStatusRepository;
        private CreditCardTypeRepository creditCardTypeRepository;
        private ExternalSystemsTablesCodeRepository externalSystemsTablesCodeRepository;
        private ExternalSystemsMissingTranslationRepository externalSystemsMissingTranslationRepository;
        private ExternalSystemsSyncStatusRepository externalSystemsSyncStatusRepository;
        private AccountingSystemsSettingRepository accountingSystemsSettingRepository;
        private AccountingSystemsSyncStatusRepository accountingSystemsSyncStatusRepository;
        private QuickbooksSyncRequestTicketRepository requestTicketRepository;

        private AccountQuery accountQuery;
        private AccountTypeQuery accountTypeQuery;
        private APInvoiceLineQuery apInvoiceLineQuery;
        private APInvoiceStatusQuery apInvoiceStatusQuery;
        private APInvoiceTypeQuery apInvoiceTypeQuery;
        private APPaymentStatusQuery apPaymentStatusQuery;
        private ARInvoiceStatusQuery arInvoiceStatusQuery;
        private ARInvoiceTotalVATQuery arInvoiceTotalVatQuery;
        private ARInvoiceTypeQuery arInvoiceTypeQuery;
        private AccountingPaymentMethodQuery paymentMethodQuery;
        private ARPaymentStatusQuery arPaymentStatusQuery;
      
        private APInvoiceQuery apInvoiceQuery;
        private ARInvoiceQuery arInvoiceQuery;
        private APPaymentQuery apPaymentQuery;
        private ARPaymentQuery arPaymentQuery;
        private ARInvoiceEntityQuery arInvoiceEntityQuery;
        private CreditCardTypeQuery creditCardTypeQuery;
        private ExternalSystemsTablesCodeQuery externalSystemsTablesCodeQuery;
        private ExternalSystemsMissingTranslationQuery externalSystemsMissingTranslationQuery;
        private ExternalSystemsSyncStatusQuery externalSystemsSyncStatusQuery;
        private AccountingSystemsSettingQuery accountingSystemsSettingQuery;
        private AccountingSystemsSyncStatusQuery accountingSystemsSyncStatusQuery;
        private QuickbooksSyncRequestTicketQuery requestTicketQuery;
        private SATPaymentMethodQuery SATPaymentMethodQuery;
        private SATPaymentMethodRepository SATPaymentMethodRepository;
        private BankAccountLiteRepository BankAccountLiteRepository;
        private BankAccountLiteQuery BankAccountLiteQuery;

        private SATTransferStatusRepository SATTransferStatusRepository;
        public AccountReceivablesSummary GetAccountingReceivablesSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            AccountReceivablesSummary result = new AccountReceivablesSummary();

            
            if (SecurityUtility.CheckTableContactFeature("ARInvoice", "READ", tenant))
            {
                aRInvoiceRepository = new ARInvoiceRepository(tenant);

                IQueryable<ARInvoice> iQueryable_Data = aRInvoiceRepository.GetIQueryableInvoices(tenant);
                IQueryable<ARInvoice> iQueryable_Data2 = iQueryable_Data;
                iQueryable_Data = iQueryable_Data.Where(d => d.IsClosed == false && d.IsCancelled == false && d.StatusCode != "LL");
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), iQueryable_Data, tenant);

                result.ARInvoicesDraftsCount = iQueryable_Data.Where(d => d.StatusCode == "DR").Count();
                result.ARInvoicesUnpaidCount = iQueryable_Data.Where(d => (d.StatusCode != "DR" && d.StatusCode != "VD" && d.IsAutoCredit == false) || (d.IsConstituentInvoice && !string.IsNullOrEmpty(d.ConsolidationInvoiceId))).Count();
                result.ARInvoicesOpenConstituentCount = iQueryable_Data.Where(d => d.IsConstituentInvoice && string.IsNullOrEmpty(d.ConsolidationInvoiceId) && d.StatusCode != "VD").Count();
                result.ARGeneralInvoiceDraftCount = iQueryable_Data.Where(d => d.IsGeneralInvoice && d.StatusCode == "DR").Count();
                result.ARInvoicesSATFailedCount = iQueryable_Data.Where(d => d.SATTransferStatusCode == "TE").Count();
                result.ARInvoicesFailedCount = iQueryable_Data2.Where(d => d.TransferStatusCode == "ET").Count();


            }

            if (SecurityUtility.CheckTableContactFeature("ARPayment", "READ", tenant))
            {
                aRPaymentRepository = new ARPaymentRepository(tenant);

                IQueryable<ARPayment> iQueryable_Data = aRPaymentRepository.GetARPayments(tenant);
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARPayment>(new QueryOperations(), iQueryable_Data, tenant);
                IQueryable<ARPayment> iQueryable_Data2 = iQueryable_Data;
                result.ARPaymentsDraftsCount = iQueryable_Data.Where(d => d.StatusCode == "DR").Count();
                result.ARPaymentsOpenedCount = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.IsClosed == false).Count();
                result.ARPaymentsSATFailedCount = iQueryable_Data.Where(d => d.SATTransferStatusCode == "TE").Count();
                result.ARPaymentFailedCount = iQueryable_Data2.Where(d => d.TransferStatusCode == "ET").Count();


            }

            return result;
        }

        private static bool CheckFullAccountingEnabled(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            var tenantPoco = tenantRepository.GetSingleTenant(tenant);
            var isFullAccounting = tenantPoco?.AccountingActivated == true;
            return isFullAccounting;
        }

        public AccountPayablesSummary GetAccountingPayablesSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            AccountPayablesSummary result = new AccountPayablesSummary();

            bool isFullAccounting = CheckFullAccountingEnabled(tenant);


            if (SecurityUtility.CheckTableContactFeature("APInvoice", "READ", tenant))
            {
                aPInvoiceRepository = new APInvoiceRepository(tenant);
                IQueryable<APInvoice> apDraftResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), aPInvoiceRepository.GetDraftsAPInvoices(tenant), tenant);
                result.APInvoicesDraftsCount = apDraftResult.Count();

                if (isFullAccounting == false)
                {
                    IQueryable<APInvoice> apUnpaidResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), aPInvoiceRepository.GetUnpaidAPInvoices(tenant), tenant);
                    result.APInvoicesUnpaidCount = apUnpaidResult.Count();


                    IQueryable<APInvoice> APErrorInTransfer = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), aPInvoiceRepository.GetErrorInTransferAPInvoices(tenant), tenant);
                    result.APInvoicesFailedCount = APErrorInTransfer.Count();
                }
            }

            if (SecurityUtility.CheckTableContactFeature("APPayment", "READ", tenant) && isFullAccounting == false)
            {
                aPPaymentRepository = new APPaymentRepository(tenant);

                IQueryable<APPayment> apPaymentDraftResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APPayment>(new QueryOperations(), aPPaymentRepository.GetDraftsAPPayments(tenant), tenant);
                result.APPaymentsDraftsCount = apPaymentDraftResult.Count();

                IQueryable<APPayment> apPaymentOpenedResult = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APPayment>(new QueryOperations(), aPPaymentRepository.GetOpenedAPPayments(tenant), tenant);

                result.APPaymentsOpenedCount = apPaymentOpenedResult.Count();


                IQueryable<APPayment> APPaymentErrorInTransfer = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APPayment>(new QueryOperations(), aPPaymentRepository.GetErrorInTransferAPPayments(tenant), tenant);

                result.APPaymentFailedCount = APPaymentErrorInTransfer.Count();
            }

            return result;
        }

        public AccountTransferSummary GetAccountingTransferSummary(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            AccountTransferSummary myResult = new AccountTransferSummary();

            // ARInvoice
            if (SecurityUtility.CheckTableContactFeature("ARInvoice", "READ", tenant))
            {
                aRInvoiceRepository = new ARInvoiceRepository(tenant);

                IQueryable<ARInvoice> iQueryable_Data = aRInvoiceRepository.GetIQueryableInvoices(tenant);
                
                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsConstituentInvoice == false);
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), iQueryable_Data, tenant);

                myResult.ARInvoicesNotReadyCount = iQueryable_Data.Where(d => d.TransferStatusCode == "NR").Count();
                myResult.ARInvoicesDontTransferCount = iQueryable_Data.Where(d => d.TransferStatusCode == "BL" && d.IsCancelled == false).Count();
                myResult.ARInvoicesErrorInTransferCount = iQueryable_Data.Where(d => d.TransferStatusCode == "ET" && d.IsCancelled == false).Count();
            }

            // APInvoice
            if (SecurityUtility.CheckTableContactFeature("APInvoice", "READ", tenant))
            {
                aPInvoiceRepository = new APInvoiceRepository(tenant);

                IQueryable<APInvoice> iQueryable_Data = aPInvoiceRepository.GetIQueryableInvoices(tenant);

                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD");
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), iQueryable_Data, tenant);

                myResult.APInvoicesNotReadyCount = iQueryable_Data.Where(d => d.TransferStatusCode == "NR").Count();
                myResult.APInvoicesDontTransferCount = iQueryable_Data.Where(d => d.TransferStatusCode == "BL").Count();
                myResult.APInvoicesErrorInTransferCount = iQueryable_Data.Where(d => d.TransferStatusCode == "ET").Count();
            }

            // ARPayment
            if (SecurityUtility.CheckTableContactFeature("ARPayment", "READ", tenant))
            {
                aRPaymentRepository = new ARPaymentRepository(tenant);

                IQueryable<ARPayment> iQueryable_Data = aRPaymentRepository.GetARPayments(tenant);

                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD");
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARPayment>(new QueryOperations(), iQueryable_Data, tenant);

                myResult.ARPaymentsNotReadyCount = iQueryable_Data.Where(d => d.TransferStatusCode == "NR").Count();
                myResult.ARPaymentsDontTransferCount = iQueryable_Data.Where(d => d.TransferStatusCode == "BL").Count();
                myResult.ARPaymentsErrorInTransferCount = iQueryable_Data.Where(d => d.TransferStatusCode == "ET").Count();
            }

            // APPayment
            if (SecurityUtility.CheckTableContactFeature("APPayment", "READ", tenant))
            {
                aPPaymentRepository = new APPaymentRepository(tenant);

                IQueryable<APPayment> iQueryable_Data = aPPaymentRepository.GetAPPayments(tenant);

                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD");
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APPayment>(new QueryOperations(), iQueryable_Data, tenant);

                myResult.APPaymentsNotReadyCount = iQueryable_Data.Where(d => d.TransferStatusCode == "NR").Count();
                myResult.APPaymentsDontTransferCount = iQueryable_Data.Where(d => d.TransferStatusCode == "BL").Count();
                myResult.APPaymentsErrorInTransferCount = iQueryable_Data.Where(d => d.TransferStatusCode == "ET").Count();
            }

            return myResult;
        }

        [Invoke]
        public List<string> GetOnStartDateARInvoiceIds(int tenant, DateTime? startDate)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<string> myResult = new List<string>();

            if (SecurityUtility.CheckTableContactFeature("ARInvoice", "READ", tenant))
            {
                aRInvoiceRepository = new ARInvoiceRepository(tenant);

                IQueryable<ARInvoice> iQueryable_Data = aRInvoiceRepository.GetIQueryableInvoices(tenant);

                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsConstituentInvoice == false);
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), iQueryable_Data, tenant);
                iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "RD" || d.TransferStatusCode == "NR");

                if (startDate != null)
                {
                    startDate = startDate.Value.Date;
                    iQueryable_Data = iQueryable_Data.Where(d => DbFunctions.TruncateTime(d.InvoiceDate) < startDate);
                }

                myResult = iQueryable_Data.Select(s => s.Id).ToList();
            }

            return myResult;
        }

        [Invoke]
        public List<string> GetOnStartDateAPInvoiceIds(int tenant, DateTime? startDate)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<string> myResult = new List<string>();

            if (SecurityUtility.CheckTableContactFeature("APInvoice", "READ", tenant))
            {
                aPInvoiceRepository = new APInvoiceRepository(tenant);

                IQueryable<APInvoice> iQueryable_Data = aPInvoiceRepository.GetIQueryableInvoices(tenant);

                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD");
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), iQueryable_Data, tenant);
                iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "RD" || d.TransferStatusCode == "NR");

                if (startDate != null)
                {
                    startDate = startDate.Value.Date;
                    iQueryable_Data = iQueryable_Data.Where(d => DbFunctions.TruncateTime(d.InvoiceDate) < startDate);
                }

                myResult = iQueryable_Data.Select(s => s.Id).ToList();
            }

            return myResult;
        }

        [Invoke]
        public List<string> GetNotReadyARInvoicesIds(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<string> myResult = new List<string>();

            if (SecurityUtility.CheckTableContactFeature("ARInvoice", "READ", tenant))
            {
                aRInvoiceRepository = new ARInvoiceRepository(tenant);

                IQueryable<ARInvoice> iQueryable_Data = aRInvoiceRepository.GetIQueryableInvoices(tenant);

                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsConstituentInvoice == false);
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), iQueryable_Data, tenant);
                iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "NR");

                myResult = iQueryable_Data.Select(s => s.Id).ToList();
            }

            return myResult;
        }

        [Invoke]
        public List<string> GetNotReadyAPInvoicesIds(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<string> myResult = new List<string>();

            if (SecurityUtility.CheckTableContactFeature("APInvoice", "READ", tenant))
            {
                aPInvoiceRepository = new APInvoiceRepository(tenant);

                IQueryable<APInvoice> iQueryable_Data = aPInvoiceRepository.GetIQueryableInvoices(tenant);

                iQueryable_Data = iQueryable_Data.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD");
                iQueryable_Data = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), iQueryable_Data, tenant);
                iQueryable_Data = iQueryable_Data.Where(d => d.TransferStatusCode == "NR");

                myResult = iQueryable_Data.Select(s => s.Id).ToList();
            }

            return myResult;
        }

        [Invoke]
        public void SetAccountingSettingStartDate(int tenant, string entityCode, DateTime? myStartDate)
        {
            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting entityPOCO = accountingSettingRepository.GetSingleAccountSetting(tenant);

            if (entityCode == "AR")
            {
                entityPOCO.ARInvoiceTransferStartDate = myStartDate;
                accountingSettingRepository.Update(entityPOCO);
                accountingSettingRepository.SubmitChanges();
            }

            else if (entityCode == "AP")
            {
                entityPOCO.APInvoiceTransferStartDate = myStartDate;
                accountingSettingRepository.Update(entityPOCO);
                accountingSettingRepository.SubmitChanges();
            }
        }

        [Invoke]
        public void BlockInvoices(List<string> ids, int tenant, string entityCode)
        {
            if (ids.Count > 0)
            {
                if (objectContext == null)
                {
                    objectContext = InvoiceContext.GetContext(tenant);
                }

                if (entityCode == "AR")
                {
                    aRInvoiceRepository = new ARInvoiceRepository(objectContext);
                    List<ARInvoice> invoices = aRInvoiceRepository.GetInvoicesListFromIdList(ids, tenant);

                    foreach (ARInvoice item in invoices)
                    {
                        item.TransferStatusCode = "BL";
                        aRInvoiceRepository.Update(item);
                    }

                    aRInvoiceRepository.SubmitChanges();
                }

                else
                {
                    aPInvoiceRepository = new APInvoiceRepository(objectContext);
                    List<APInvoice> invoices = aPInvoiceRepository.GetInvoicesListFromIdList(ids, tenant);

                    foreach (APInvoice item in invoices)
                    {
                        item.TransferStatusCode = "BL";
                        aPInvoiceRepository.Update(item);
                    }

                    aPInvoiceRepository.SubmitChanges();
                }
            }
        }

        [Invoke]
        public void Recalculate(List<string> ids, int tenant, string entityCode)
        {
            if (ids.Count > 0)
            {
                TransferHelper invoiceTransferHelper = new TransferHelper(tenant, entityCode);
                invoiceTransferHelper.Recalculate(ids);
            }
        }



        //========================================
        [Query(HasSideEffects = true)]
        public IQueryable<SATInterfaceList> GetSATInterfaceFilters(byte[] xmlFilters, int tenant)
        {
            SATInterfaceRepository sATInterfaceRepository = new SATInterfaceRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SATInterface> iQueryable = sATInterfaceRepository.GetSATInterfaces();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SATInterface>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new SATInterfaceList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<SATInterfaceList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SATInterfaceList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SATInterface", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SATInterfaceList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SATInterfaceList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SATInterfaceList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SATInterfaceList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SATInterfaceList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetSATInterfaceFiltersCount(byte[] xmlFilters, int tenant)
        {
            SATInterfaceRepository sATInterfaceRepository = new SATInterfaceRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SATInterface> iQueryable = sATInterfaceRepository.GetSATInterfaces();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SATInterface>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new SATInterfaceList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<SATInterfaceList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


        public IQueryable<SATTransferStatusList> GetSATTransferStatusFilters(byte[] xmlFilters, int tenant)
        {
            SATTransferStatusRepository SATTransferStatusRepository = new SATTransferStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SATTransferStatus> iQueryable = SATTransferStatusRepository.GetSATTransferStatus();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SATTransferStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new SATTransferStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<SATTransferStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SATTransferStatusList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SATTransferStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SATTransferStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SATTransferStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SATTransferStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SATTransferStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SATTransferStatusList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetSATTransferStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SATTransferStatusRepository SATTransferStatusRepository = new SATTransferStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SATTransferStatus> iQueryable = SATTransferStatusRepository.GetSATTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SATTransferStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new SATTransferStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<SATTransferStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<SATInvoiceStatusList> GetSATInvoiceStatusFilters(byte[] xmlFilters, int tenant)
        {
            SATInvoiceStatusRepository SATInvoiceStatusRepository = new SATInvoiceStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SATInvoiceStatus> iQueryable = SATInvoiceStatusRepository.GetSATInvoiceStatus();

            //PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SATInvoiceStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new SATInvoiceStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<SATInvoiceStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SATInvoiceStatusList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SATInvoiceStatus", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();


                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SATInvoiceStatusList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SATInvoiceStatusList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SATInvoiceStatusList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SATInvoiceStatusList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SATInvoiceStatusList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetSATInvoiceStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SATInvoiceStatusRepository SATInvoiceStatusRepository = new SATInvoiceStatusRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SATInvoiceStatus> iQueryable = SATInvoiceStatusRepository.GetSATInvoiceStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SATInvoiceStatus>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new SATInvoiceStatusList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,
                         };

            query2 = filter.GetFilteredQuery<SATInvoiceStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }


        //[Query(HasSideEffects = true)]
        //public IQueryable<SATPaymentMethodList> GetSATPaymentMethodFilters(byte[] xmlFilters, int tenant)
        //{
        //    SATPaymentMethodRepository SATPaymentMethodRepository = new SATPaymentMethodRepository(tenant);
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    MemoryStream memorystream = new MemoryStream(xmlFilters);
        //    XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
        //    QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
        //    GenericFilter filter = new GenericFilter();
        //    GenericSort sortClass = new GenericSort();

        //    IQueryable<SATPaymentMethod> iQueryable = SATPaymentMethodRepository.GetSATPaymentMethods();

        //    //PortCustomFilter customfilters = new PortCustomFilter(tenant);
        //    QueryOperations nonListQueryOperation = new QueryOperations();
        //    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //    QueryOperations listQueryOperation = new QueryOperations();
        //    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

        //    iQueryable = filter.GetFilteredQuery<SATPaymentMethod>(nonListQueryOperation, iQueryable);

        //    int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

        //    var query2 = from entity in iQueryable
        //                 select new SATPaymentMethodList()
        //                 {
        //                     Code = entity.Code,
        //                     Name = entity.Name,
        //                     LocalName = entity.LocalName,
        //                     SearchFields = entity.SearchFields,
        //                 };

        //    query2 = filter.GetFilteredQuery<SATPaymentMethodList>(listQueryOperation, query2);

        //    if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
        //    {
        //        PropertyInfo propInfo = typeof(SATPaymentMethodList).GetProperty(queryOperations.SortByColumnName);
        //        //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
        //        List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SATPaymentMethod", tenant).ToList();

        //        ObjectField objectField = (from a in shipmentObjectFields
        //                                   where a.FieldName == queryOperations.SortByColumnName
        //                                   select a).FirstOrDefault();


        //        if (objectField != null)
        //        {
        //            switch (objectField.DataTypeCode.ToLower())
        //            {
        //                case "text":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<SATPaymentMethodList, string>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "double":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<SATPaymentMethodList, double>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "datetime":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<SATPaymentMethodList, DateTime>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "integer":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<SATPaymentMethodList, int>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "boolean":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<SATPaymentMethodList, bool>(queryOperations, query2);
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        query2 = query2.OrderByDescending(d => d.Name);
        //                        break;
        //                    }
        //            }
        //        }
        //    }

        //    else
        //    {
        //        query2 = query2.OrderByDescending(d => d.Code);
        //    }

        //    query2 = query2.Skip(skippedPorts);
        //    query2 = query2.Take(queryOperations.PageSize);
        //    return query2;
        //}


        //public int GetSATPaymentMethodFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SATPaymentMethodRepository SATPaymentMethodRepository = new SATPaymentMethodRepository(tenant);
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    MemoryStream memorystream = new MemoryStream(xmlFilters);
        //    XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
        //    QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
        //    GenericFilter filter = new GenericFilter();
        //    GenericSort sortClass = new GenericSort();

        //    IQueryable<SATPaymentMethod> iQueryable = SATPaymentMethodRepository.GetSATPaymentMethods();

        //    QueryOperations nonListQueryOperation = new QueryOperations();
        //    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //    QueryOperations listQueryOperation = new QueryOperations();
        //    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

        //    iQueryable = filter.GetFilteredQuery<SATPaymentMethod>(nonListQueryOperation, iQueryable);

        //    var query2 = from entity in iQueryable
        //                 select new SATPaymentMethodList()
        //                 {
        //                     Code = entity.Code,
        //                     Name = entity.Name,
        //                     LocalName = entity.LocalName,
        //                     SearchFields = entity.SearchFields,
        //                 };

        //    query2 = filter.GetFilteredQuery<SATPaymentMethodList>(listQueryOperation, query2);
        //    int count = query2.Count();
        //    return count;
        //}




        //========================================



        protected override bool PersistChangeSet()
        {
            objectContext.SaveChanges();
            return base.PersistChangeSet();
        }

        protected override bool ExecuteChangeSet()
        {
            return base.ExecuteChangeSet();
        }
    }
}


