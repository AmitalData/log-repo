using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.GlobalModel.CustomFilters;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Security;

namespace WebFreight.Web.GlobalModel
{
    public partial class GlobalDomainService
    {
        private TenantManagementRepository tenantManagementRepository;
        private TenantManagementQuery tenantManagementQuery;

        [RequiresAuthentication]
        [Query(IsDefault = true)]
        public IQueryable<TenantManagement> GetTenants()
        {
            SecurityUtility.CheckContactFeature("TenantManagement", "READ", 0);
            return tenantManagementRepository.GetTenants();
        }

        public TenantManagementPM GetSingleTenantManagementPM(int id)
        {
            tenantManagementQuery = new TenantManagementQuery(id);
            return tenantManagementQuery.GetSinglePM(id);
        }

        public IQueryable<TenantManagementPM> GetTenantPMs(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TenantManagement", "READ", tenant);
            tenantManagementQuery = new TenantManagementQuery(tenant);
            return tenantManagementQuery.GetTenantManagementPMs();
        }

        public void UpdateTenantMngmntPM(TenantManagementPM entityPM)
        {
            if (objectContext == null)
            {
                objectContext = GlobalContext.GetContext();
            }

            #region Licenses
            List<TenantManagementLicensePM> licensesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.TenantManagementLicenses).Cast<TenantManagementLicensePM>().ToList();
            foreach (TenantManagementLicensePM itemPM in licensesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            #region Add-Ons
            List<TenantAddOnPM> addOnsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.AddOns).Cast<TenantAddOnPM>().ToList();
            foreach (TenantAddOnPM itemPM in addOnsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            TenantManagementService service = new TenantManagementService(objectContext);
            service.SetChangeSet(licensesChangeSet, addOnsChangeSet);
            service.Update(entityPM);
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TenantManagementList> GetTenantManagementFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TenantManagement", "READ", tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TenantManagement> iQueryable = tenantManagementRepository.GetTenants();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            TenantManagementCustomFilter customfilters = new TenantManagementCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = filter.GetFilteredQuery<TenantManagement>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from a in iQueryable
                         select new TenantManagementList()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             PackageCode = a.PackageCode,
                             PackageName = a.PackageName,
                             IsTrial = a.IsTrial,
                             TrialStartDate = a.TrialStartDate,
                             TrialEndDate = a.TrialEndDate,
                             FirstPaymentDate = a.FirstPaymentDate,
                             PaidUntilDate = a.PaidUntilDate,
                             NumberOfUsers = a.NumberOfUsers,
                             IsActive = a.GlobalTenant.IsActive,
                             GlobalDBId = a.GlobalTenant.GlobalDBId,
                             SearchFields = a.SearchFields,
                             FreeUsers = a.FreeUsers,
                             IsRecurring = a.IsRecurring,
                             RecurringPeriodCode = a.RecurringPeriodCode,
                             CreateDate = a.CreateDate,
                             UpdateDate = a.UpdateDate,
                             PaymentFailure = a.PaymentFailure,
                             SuspendDate = a.SuspendDate,
                             InternalNotes = a.InternalNotes,
                             BluesnapAccount = a.BluesnapAccount,
                             MainContract = a.MainContract,
                             TemporalPackageCode = a.TemporalPackageCode,
                             TemporalStartDate = a.TemporalStartDate,
                             TemporalEndDate = a.TemporalEndDate,
                             LicensePrice = a.LicensePrice,
                             PaymentChannelCode = a.PaymentChannelCode,
                             PaymentMethodCode = a.PaymentMethodCode,
                             Notes = a.Notes,
                             TTY = a.TTY,
                             APInvoiceLastDate = a.APInvoiceLastDate,
                             APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                             APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                             ARInvoiceLastDate = a.ARInvoiceLastDate,
                             ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                             ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                             CustomerLastDate = a.CustomerLastDate,
                             CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                             CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                             LastFHLSentDate = a.LastFHLSentDate,
                             LastFWBSentDate = a.LastFWBSentDate,
                             QuoteLastDate = a.QuoteLastDate,
                             QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                             QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                             ShipmentLastDate = a.ShipmentLastDate,
                             ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                             ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                             StatisticsUpdateDate = a.StatisticsUpdateDate,
                             CountryName = a.CountryName,
                             LastLoginDateTime = a.LastLoginDateTime,
                             PaymentCurrencyCode = a.PaymentCurrencyCode,
                             IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                             ManageLicencesPerUser = a.ManageLicencesPerUser,
                             DistributorCode = a.DistributorCode,
                             IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                             IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                             IsCargonautEnabled = a.IsCargonautEnabled,
                             LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                             LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                             IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                             BluesnapContractId = a.BluesnapContractId,
                             AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                             PIMA = a.PIMA,
                             LastFFRSentDate = a.LastFFRSentDate,
                             ActivityLastDate = a.ActivityLastDate,
                             ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                             ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                             OpportunityLastDate = a.OpportunityLastDate,
                             OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                             OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                             FSULastReceivedDate = a.FSULastReceivedDate,
                             FSALastReceivedDate = a.FSALastReceivedDate,
                             FSRLastSentDate = a.FSRLastSentDate,
                             BillingByLogitude = a.BillingByLogitude,
                             ResellerCommission = a.ResellerCommission,
                             TenantTypeCode = a.TenantTypeCode,
                             TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                             SupportActivated= a.SupportActivated,
                             SupportEmail=a.SupportEmail,
                             IsMultiPackage = a.IsMultiPackage,
                             MobileLastDate = a.MobileLastDate,
                             MobileTotalLastWeek = a.MobileTotalLastWeek,
                             MobileTotalLastMonth = a.MobileTotalLastMonth,
                             ShardLogisticLastDate = a.ShardLogisticLastDate,
                             ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                             ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                             RequestedAirlines = a.RequestedAirlines,
                             RegisteredAirlines = a.RegisteredAirlines,
                             PendingAirlines = a.PendingAirlines,
                             Technology = a.Technology,
                             PrivateLabelId = a.GlobalTenant != null ? a.GlobalTenant.PrivateLabelId : "",
                             SilverlightEndDate = a.SilverlightEndDate,
                         };

            query2 = filter.GetFilteredQuery<TenantManagementList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TenantManagementList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TenantManagement", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagementList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagementList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagementList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagementList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagementList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTenantManagementFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TenantManagement", "READ", tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TenantManagement> iQueryable = tenantManagementRepository.GetTenants();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            TenantManagementCustomFilter customfilters = new TenantManagementCustomFilter(tenant);
            iQueryable = customfilters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = filter.GetFilteredQuery<TenantManagement>(nonListQueryOperation, iQueryable);

            var query2 = from a in iQueryable
                         select new TenantManagementList()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             PackageCode = a.PackageCode,
                             PackageName = a.PackageName,
                             IsTrial = a.IsTrial,
                             TrialStartDate = a.TrialStartDate,
                             TrialEndDate = a.TrialEndDate,
                             FirstPaymentDate = a.FirstPaymentDate,
                             PaidUntilDate = a.PaidUntilDate,
                             NumberOfUsers = a.NumberOfUsers,
                             SearchFields = a.SearchFields,
                             IsActive = a.GlobalTenant.IsActive,
                             FreeUsers = a.FreeUsers,
                             IsRecurring = a.IsRecurring,
                             RecurringPeriodCode = a.RecurringPeriodCode,
                             CreateDate = a.CreateDate,
                             UpdateDate = a.UpdateDate,
                             PaymentFailure = a.PaymentFailure,
                             SuspendDate = a.SuspendDate,
                             InternalNotes = a.InternalNotes,
                             BluesnapAccount = a.BluesnapAccount,
                             MainContract = a.MainContract,
                             TemporalPackageCode = a.TemporalPackageCode,
                             TemporalStartDate = a.TemporalStartDate,
                             TemporalEndDate = a.TemporalEndDate,
                             LicensePrice = a.LicensePrice,
                             PaymentChannelCode = a.PaymentChannelCode,
                             PaymentMethodCode = a.PaymentMethodCode,
                             Notes = a.Notes,
                             TTY = a.TTY,
                             APInvoiceLastDate = a.APInvoiceLastDate,
                             APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                             APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                             ARInvoiceLastDate = a.ARInvoiceLastDate,
                             ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                             ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                             CustomerLastDate = a.CustomerLastDate,
                             CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                             CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                             LastFHLSentDate = a.LastFHLSentDate,
                             LastFWBSentDate = a.LastFWBSentDate,
                             QuoteLastDate = a.QuoteLastDate,
                             QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                             QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                             ShipmentLastDate = a.ShipmentLastDate,
                             ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                             ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                             StatisticsUpdateDate = a.StatisticsUpdateDate,
                             CountryName = a.CountryName,
                             LastLoginDateTime = a.LastLoginDateTime,
                             PaymentCurrencyCode = a.PaymentCurrencyCode,
                             IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                             ManageLicencesPerUser = a.ManageLicencesPerUser,
                             DistributorCode = a.DistributorCode,
                             IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                             IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                             IsCargonautEnabled = a.IsCargonautEnabled,
                             LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                             LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                             IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                             BluesnapContractId = a.BluesnapContractId,
                             AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                             PIMA = a.PIMA,
                             LastFFRSentDate = a.LastFFRSentDate,
                             ActivityLastDate = a.ActivityLastDate,
                             ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                             ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                             OpportunityLastDate = a.OpportunityLastDate,
                             OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                             OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                             FSULastReceivedDate = a.FSULastReceivedDate,
                             FSALastReceivedDate = a.FSALastReceivedDate,
                             FSRLastSentDate = a.FSRLastSentDate,
                             BillingByLogitude = a.BillingByLogitude,
                             ResellerCommission = a.ResellerCommission,
                             TenantTypeCode = a.TenantTypeCode,
                             TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                             SupportActivated = a.SupportActivated,
                             SupportEmail = a.SupportEmail,
                             IsMultiPackage = a.IsMultiPackage,
                             MobileLastDate = a.MobileLastDate,
                             MobileTotalLastWeek = a.MobileTotalLastWeek,
                             MobileTotalLastMonth = a.MobileTotalLastMonth,
                             ShardLogisticLastDate = a.ShardLogisticLastDate,
                             ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                             ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                             RequestedAirlines = a.RequestedAirlines,
                             RegisteredAirlines = a.RegisteredAirlines,
                             PendingAirlines = a.PendingAirlines,
                             Technology = a.Technology,
                             PrivateLabelId = a.GlobalTenant != null ? a.GlobalTenant.PrivateLabelId : "",
                             SilverlightEndDate = a.SilverlightEndDate,
                         };

            query2 = filter.GetFilteredQuery<TenantManagementList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<TenantManagementList> GetTenantLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TenantManagement", "READ", tenant);
            IQueryable<TenantManagement> tenants = tenantManagementRepository.GetTenants();

            var query2 = from a in tenants
                         select new TenantManagementList()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             PackageCode = a.PackageCode,
                             PackageName = a.PackageName,
                             IsTrial = a.IsTrial,
                             TrialStartDate = a.TrialStartDate,
                             TrialEndDate = a.TrialEndDate,
                             FirstPaymentDate = a.FirstPaymentDate,
                             PaidUntilDate = a.PaidUntilDate,
                             NumberOfUsers = a.NumberOfUsers,
                             SearchFields = a.SearchFields,
                             IsActive = a.GlobalTenant.IsActive,
                             FreeUsers = a.FreeUsers,
                             IsRecurring = a.IsRecurring,
                             RecurringPeriodCode = a.RecurringPeriodCode,
                             CreateDate = a.CreateDate,
                             UpdateDate = a.UpdateDate,
                             PaymentFailure = a.PaymentFailure,
                             SuspendDate = a.SuspendDate,
                             InternalNotes = a.InternalNotes,
                             BluesnapAccount = a.BluesnapAccount,
                             MainContract = a.MainContract,
                             TemporalPackageCode = a.TemporalPackageCode,
                             TemporalStartDate = a.TemporalStartDate,
                             TemporalEndDate = a.TemporalEndDate,
                             LicensePrice = a.LicensePrice,
                             PaymentChannelCode = a.PaymentChannelCode,
                             PaymentMethodCode = a.PaymentMethodCode,
                             Notes = a.Notes,
                             TTY = a.TTY,
                             APInvoiceLastDate = a.APInvoiceLastDate,
                             APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                             APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                             ARInvoiceLastDate = a.ARInvoiceLastDate,
                             ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                             ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                             CustomerLastDate = a.CustomerLastDate,
                             CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                             CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                             LastFHLSentDate = a.LastFHLSentDate,
                             LastFWBSentDate = a.LastFWBSentDate,
                             QuoteLastDate = a.QuoteLastDate,
                             QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                             QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                             ShipmentLastDate = a.ShipmentLastDate,
                             ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                             ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                             StatisticsUpdateDate = a.StatisticsUpdateDate,
                             CountryName = a.CountryName,
                             LastLoginDateTime = a.LastLoginDateTime,
                             PaymentCurrencyCode = a.PaymentCurrencyCode,
                             IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                             ManageLicencesPerUser = a.ManageLicencesPerUser,
                             DistributorCode = a.DistributorCode,
                             IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                             IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                             IsCargonautEnabled = a.IsCargonautEnabled,
                             LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                             LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                             IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                             BluesnapContractId = a.BluesnapContractId,
                             AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                             PIMA = a.PIMA,
                             LastFFRSentDate = a.LastFFRSentDate,
                             ActivityLastDate = a.ActivityLastDate,
                             ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                             ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                             OpportunityLastDate = a.OpportunityLastDate,
                             OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                             OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                             FSULastReceivedDate = a.FSULastReceivedDate,
                             FSALastReceivedDate = a.FSALastReceivedDate,
                             FSRLastSentDate = a.FSRLastSentDate,
                             BillingByLogitude = a.BillingByLogitude,
                             ResellerCommission = a.ResellerCommission,
                             TenantTypeCode = a.TenantTypeCode,
                             TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                             SupportActivated = a.SupportActivated,
                             SupportEmail = a.SupportEmail,
                             IsMultiPackage = a.IsMultiPackage,
                             MobileLastDate = a.MobileLastDate,
                             MobileTotalLastWeek = a.MobileTotalLastWeek,
                             MobileTotalLastMonth = a.MobileTotalLastMonth,
                             ShardLogisticLastDate = a.ShardLogisticLastDate,
                             ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                             ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                             RequestedAirlines = a.RequestedAirlines,
                             RegisteredAirlines = a.RegisteredAirlines,
                             PendingAirlines = a.PendingAirlines,
                             Technology = a.Technology,
                             PrivateLabelId = a.GlobalTenant != null ? a.GlobalTenant.PrivateLabelId : "",
                             SilverlightEndDate = a.SilverlightEndDate,
                         };

            return query2;
        }

        public TenantManagementList GetSingleTenantManagementList(int id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            tenantManagementRepository = new TenantManagementRepository();

            TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(id);

            if (tenantManagement != null)
            {
                TenantManagementList tenantManagementList = new TenantManagementList()
                {
                    Id = tenantManagement.Id,
                    Name = tenantManagement.Name,
                    PackageCode = tenantManagement.PackageCode,
                    PackageName = tenantManagement.PackageName,
                    IsTrial = tenantManagement.IsTrial,
                    TrialStartDate = tenantManagement.TrialStartDate,
                    TrialEndDate = tenantManagement.TrialEndDate,
                    FirstPaymentDate = tenantManagement.FirstPaymentDate,
                    PaidUntilDate = tenantManagement.PaidUntilDate,
                    NumberOfUsers = tenantManagement.NumberOfUsers,
                    IsActive = tenantManagement.GlobalTenant.IsActive,
                    GlobalDBId = tenantManagement.GlobalTenant.GlobalDBId,
                    SearchFields = tenantManagement.SearchFields,
                    FreeUsers = tenantManagement.FreeUsers,
                    IsRecurring = tenantManagement.IsRecurring,
                    RecurringPeriodCode = tenantManagement.RecurringPeriodCode,
                    CreateDate = tenantManagement.CreateDate,
                    UpdateDate = tenantManagement.UpdateDate,
                    PaymentFailure = tenantManagement.PaymentFailure,
                    SuspendDate = tenantManagement.SuspendDate,
                    InternalNotes = tenantManagement.InternalNotes,
                    BluesnapAccount = tenantManagement.BluesnapAccount,
                    MainContract = tenantManagement.MainContract,
                    TemporalPackageCode = tenantManagement.TemporalPackageCode,
                    TemporalStartDate = tenantManagement.TemporalStartDate,
                    TemporalEndDate = tenantManagement.TemporalEndDate,
                    LicensePrice = tenantManagement.LicensePrice,
                    PaymentChannelCode = tenantManagement.PaymentChannelCode,
                    PaymentMethodCode = tenantManagement.PaymentMethodCode,
                    Notes = tenantManagement.Notes,
                    TTY = tenantManagement.TTY,
                    APInvoiceLastDate = tenantManagement.APInvoiceLastDate,
                    APInvoiceTotalLastMonth = tenantManagement.APInvoiceTotalLastMonth,
                    APInvoiceTotalLastWeek = tenantManagement.APInvoiceTotalLastWeek,
                    ARInvoiceLastDate = tenantManagement.ARInvoiceLastDate,
                    ARInvoiceTotalLastMonth = tenantManagement.ARInvoiceTotalLastMonth,
                    ARInvoiceTotalLastWeek = tenantManagement.ARInvoiceTotalLastWeek,
                    CustomerLastDate = tenantManagement.CustomerLastDate,
                    CustomerTotalLastMonth = tenantManagement.CustomerTotalLastMonth,
                    CustomerTotalLastWeek = tenantManagement.CustomerTotalLastWeek,
                    LastFHLSentDate = tenantManagement.LastFHLSentDate,
                    LastFWBSentDate = tenantManagement.LastFWBSentDate,
                    QuoteLastDate = tenantManagement.QuoteLastDate,
                    QuoteTotalLastMonth = tenantManagement.QuoteTotalLastMonth,
                    QuoteTotalLastWeek = tenantManagement.QuoteTotalLastWeek,
                    ShipmentLastDate = tenantManagement.ShipmentLastDate,
                    ShipmentTotalLastMonth = tenantManagement.ShipmentTotalLastMonth,
                    ShipmentTotalLastWeek = tenantManagement.ShipmentTotalLastWeek,
                    StatisticsUpdateDate = tenantManagement.StatisticsUpdateDate,
                    CountryName = tenantManagement.CountryName,
                    LastLoginDateTime = tenantManagement.LastLoginDateTime,
                    PaymentCurrencyCode = tenantManagement.PaymentCurrencyCode,
                    IsAWBStockPrepaid = tenantManagement.IsAWBStockPrepaid,
                    ManageLicencesPerUser = tenantManagement.ManageLicencesPerUser,
                    DistributorCode = tenantManagement.DistributorCode,
                    IsDistributorSupportEnabled = tenantManagement.IsDistributorSupportEnabled,
                    IsSystemSupportEnabled = tenantManagement.IsSystemSupportEnabled,
                    IsCargonautEnabled = tenantManagement.IsCargonautEnabled,
                    LastFHLCargonautSentDate = tenantManagement.LastFHLCargonautSentDate,
                    LastFWBCargonautSentDate = tenantManagement.LastFWBCargonautSentDate,
                    IsDEXXConnectionEnabled = tenantManagement.IsDEXXConnectionEnabled,
                    BluesnapContractId = tenantManagement.BluesnapContractId,
                    AWBMessagesCCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode,
                    PIMA = tenantManagement.PIMA,
                    LastFFRSentDate = tenantManagement.LastFFRSentDate,
                    ActivityLastDate = tenantManagement.ActivityLastDate,
                    ActivityTotalLastWeek = tenantManagement.ActivityTotalLastWeek,
                    ActivityTotalLastMonth = tenantManagement.ActivityTotalLastMonth,
                    OpportunityLastDate = tenantManagement.OpportunityLastDate,
                    OpportunityTotalLastWeek = tenantManagement.OpportunityTotalLastWeek,
                    OpportunityTotalLastMonth = tenantManagement.OpportunityTotalLastMonth,
                    FSULastReceivedDate = tenantManagement.FSULastReceivedDate,
                    FSALastReceivedDate = tenantManagement.FSALastReceivedDate,
                    FSRLastSentDate = tenantManagement.FSRLastSentDate,
                    BillingByLogitude = tenantManagement.BillingByLogitude,
                    ResellerCommission = tenantManagement.ResellerCommission,
                    TenantTypeCode = tenantManagement.TenantTypeCode,
                    TenantConnectedToAirlineCode = tenantManagement.TenantConnectedToAirlineCode,
                    SupportActivated = tenantManagement.SupportActivated,
                    SupportEmail = tenantManagement.SupportEmail,
                    IsMultiPackage = tenantManagement.IsMultiPackage,
                    MobileLastDate = tenantManagement.MobileLastDate,
                    MobileTotalLastWeek = tenantManagement.MobileTotalLastWeek,
                    MobileTotalLastMonth = tenantManagement.MobileTotalLastMonth,
                    ShardLogisticLastDate = tenantManagement.ShardLogisticLastDate,
                    ShardLogisticTotalLastWeek = tenantManagement.ShardLogisticTotalLastWeek,
                    ShardLogisticTotalLastMonth = tenantManagement.ShardLogisticTotalLastMonth,
                    RequestedAirlines = tenantManagement.RequestedAirlines,
                    RegisteredAirlines = tenantManagement.RegisteredAirlines,
                    PendingAirlines = tenantManagement.PendingAirlines,
                    Technology = tenantManagement.Technology,
                    PrivateLabelId = tenantManagement.GlobalTenant != null ? tenantManagement.GlobalTenant.PrivateLabelId : "",
                    SilverlightEndDate = tenantManagement.SilverlightEndDate,
                };

                return tenantManagementList;
            }

            else
            {
                return null;
            }
        }

        public void UpdateTenantManagementList(TenantManagementList currentEntity)
        {

        }

        public TenantUserDataClass CheckTenantMangmnt(int tenantMngmntId, string loggedUserId)
        {
            TenantUserDataClass dataClass = new TenantUserDataClass();
            try
            {


                dataClass.Id = tenantMngmntId + loggedUserId;

                tenantManagementQuery = new TenantManagementQuery(tenantMngmntId);
                TenantManagementPM tenant = tenantManagementQuery.GetSinglePM(tenantMngmntId);

                dataClass.IsTrial = tenant.IsTrial;
                dataClass.IsRecurring = tenant.IsRecurring;
                dataClass.PaidUntilDate = tenant.PaidUntilDate;
                dataClass.PaymentFailure = tenant.PaymentFailure;

                if (tenant.PaymentFailure)
                {
                    if (tenant.SuspendDate.Value.Date < DateTime.Now.Date)
                    {
                        dataClass.DoBlocking = true;
                        dataClass.BlockType = "suspend";
                    }
                    else if (tenant.SuspendDate.Value.Date == DateTime.Now.Date)
                    {
                        dataClass.SuspendDaysLeft = 0;
                    }
                    else
                    {
                        dataClass.SuspendDaysLeft = tenantManagementQuery.ComputeDaysLeft(tenant.SuspendDate);
                    }
                }

                if (tenant.IsTrial)
                {
                    if (tenant.TrialEndDate.Value.Date < DateTime.Now.Date)
                    {
                        dataClass.DoBlocking = true;
                        dataClass.BlockType = "company";
                    }
                    else if (tenant.TrialEndDate.Value.Date == DateTime.Now.Date)
                    {
                        dataClass.TrailDaysLeft = 0;
                    }
                    else
                    {
                        dataClass.TrailDaysLeft = tenantManagementQuery.ComputeDaysLeft(tenant.TrialEndDate);
                    }
                }
                else if (tenant.PaidUntilDate != null)
                {
                    if (!tenant.IsRecurring)
                    {
                        if ((tenant.PaidUntilDate - DateTime.Now).Value.Days < 0)
                        {
                            dataClass.DoBlocking = true;
                            dataClass.BlockType = "company";
                        }
                        else
                        {
                            dataClass.PaidDaysLeft = tenantManagementQuery.ComputeDaysLeft(tenant.PaidUntilDate);
                        }
                    }
                }

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    ContactDomainService service = new ContactDomainService();
                    UserPM user = service.GetSingleUser(loggedUserId, tenantMngmntId);

                    if (user != null)
                    {
                        dataClass.ExpirationDate = user.ExpirationDate;


                        if (user.ExpirationDate != null)
                        {
                            if (user.ExpirationDate.Value.Date < DateTime.Now.Date)
                            {
                                dataClass.DoBlocking = true;
                                dataClass.BlockType = "user";
                            }
                            else if (user.ExpirationDate.Value.Date == DateTime.Now.Date)
                            {
                                dataClass.ExpirationDaysLeft = 0;
                            }
                            else
                            {
                                dataClass.ExpirationDaysLeft = tenantManagementQuery.ComputeDaysLeft(user.ExpirationDate);
                            }
                        }
                    }
                    scope.Complete();
                }
                return dataClass;

            }
            catch
            {
                return dataClass;
            }

        }

        public List<TenantManagementList> GetTenantManagementsForDistributor(string distributorCode)
        {
            List<int> activeTennats = globalTenantsRepository.GetActiveGlobalTenantsIds();


            IQueryable<TenantManagement> tenantManagements = tenantManagementRepository.GetTenants();
            List<TenantManagementList> tenantManagementList = (from a in tenantManagements
                                                               where activeTennats.Contains(a.Id) && a.DistributorCode == distributorCode
                                                               select new TenantManagementList()
                                                               {
                                                                   Id = a.Id,
                                                                   Name = a.Name,
                                                                   PackageCode = a.PackageCode,
                                                                   PackageName = a.PackageName,
                                                                   IsTrial = a.IsTrial,
                                                                   TrialStartDate = a.TrialStartDate,
                                                                   TrialEndDate = a.TrialEndDate,
                                                                   FirstPaymentDate = a.FirstPaymentDate,
                                                                   PaidUntilDate = a.PaidUntilDate,
                                                                   NumberOfUsers = a.NumberOfUsers,
                                                                   SearchFields = a.SearchFields,
                                                                   IsActive = a.GlobalTenant.IsActive,
                                                                   FreeUsers = a.FreeUsers,
                                                                   IsRecurring = a.IsRecurring,
                                                                   RecurringPeriodCode = a.RecurringPeriodCode,
                                                                   CreateDate = a.CreateDate,
                                                                   UpdateDate = a.UpdateDate,
                                                                   PaymentFailure = a.PaymentFailure,
                                                                   SuspendDate = a.SuspendDate,
                                                                   InternalNotes = a.InternalNotes,
                                                                   BluesnapAccount = a.BluesnapAccount,
                                                                   MainContract = a.MainContract,
                                                                   TemporalPackageCode = a.TemporalPackageCode,
                                                                   TemporalStartDate = a.TemporalStartDate,
                                                                   TemporalEndDate = a.TemporalEndDate,
                                                                   LicensePrice = a.LicensePrice,
                                                                   PaymentChannelCode = a.PaymentChannelCode,
                                                                   PaymentMethodCode = a.PaymentMethodCode,
                                                                   Notes = a.Notes,
                                                                   TTY = a.TTY,
                                                                   APInvoiceLastDate = a.APInvoiceLastDate,
                                                                   APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                                                                   APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                                                                   ARInvoiceLastDate = a.ARInvoiceLastDate,
                                                                   ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                                                                   ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                                                                   CustomerLastDate = a.CustomerLastDate,
                                                                   CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                                                                   CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                                                                   LastFHLSentDate = a.LastFHLSentDate,
                                                                   LastFWBSentDate = a.LastFWBSentDate,
                                                                   QuoteLastDate = a.QuoteLastDate,
                                                                   QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                                                                   QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                                                                   ShipmentLastDate = a.ShipmentLastDate,
                                                                   ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                                                                   ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                                                                   StatisticsUpdateDate = a.StatisticsUpdateDate,
                                                                   CountryName = a.CountryName,
                                                                   LastLoginDateTime = a.LastLoginDateTime,
                                                                   PaymentCurrencyCode = a.PaymentCurrencyCode,
                                                                   IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                                                                   ManageLicencesPerUser = a.ManageLicencesPerUser,
                                                                   DistributorCode = a.DistributorCode,
                                                                   IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                                                                   IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                                                                   IsCargonautEnabled = a.IsCargonautEnabled,
                                                                   LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                                                                   LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                                                                   IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                                                                   BluesnapContractId = a.BluesnapContractId,
                                                                   AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                                                                   PIMA = a.PIMA,
                                                                   LastFFRSentDate = a.LastFFRSentDate,
                                                                   ActivityLastDate = a.ActivityLastDate,
                                                                   ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                                                                   ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                                                                   OpportunityLastDate = a.OpportunityLastDate,
                                                                   OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                                                                   OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                                                                   FSULastReceivedDate = a.FSULastReceivedDate,
                                                                   FSALastReceivedDate = a.FSALastReceivedDate,
                                                                   FSRLastSentDate = a.FSRLastSentDate,
                                                                   BillingByLogitude = a.BillingByLogitude,
                                                                   ResellerCommission = a.ResellerCommission,
                                                                   TenantTypeCode = a.TenantTypeCode,
                                                                   TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                                                                   SupportEmail = a.SupportEmail,
                                                                   SupportActivated=a.SupportActivated,
                                                                   IsMultiPackage = a.IsMultiPackage,
                                                                   MobileLastDate = a.MobileLastDate,
                                                                   MobileTotalLastWeek = a.MobileTotalLastWeek,
                                                                   MobileTotalLastMonth = a.MobileTotalLastMonth,
                                                                   ShardLogisticLastDate = a.ShardLogisticLastDate,
                                                                   ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                                                                   ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                                                                   RequestedAirlines = a.RequestedAirlines,
                                                                   RegisteredAirlines = a.RegisteredAirlines,
                                                                   PendingAirlines = a.PendingAirlines,
                                                                   Technology = a.Technology,
                                                                   PrivateLabelId = a.GlobalTenant != null ? a.GlobalTenant.PrivateLabelId : "",
                                                                   SilverlightEndDate = a.SilverlightEndDate,
                                                               }).ToList();
            return tenantManagementList;

        }

        public IQueryable<TenantManagementList> GetActiveTenantLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TenantManagement", "READ", tenant);
            IQueryable<TenantManagement> tenants = tenantManagementRepository.GetTenants();

            var query2 = from a in tenants
                         where a.GlobalTenant.IsActive == true
                         select new TenantManagementList()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             PackageCode = a.PackageCode,
                             PackageName = a.PackageName,
                             IsTrial = a.IsTrial,
                             TrialStartDate = a.TrialStartDate,
                             TrialEndDate = a.TrialEndDate,
                             FirstPaymentDate = a.FirstPaymentDate,
                             PaidUntilDate = a.PaidUntilDate,
                             NumberOfUsers = a.NumberOfUsers,
                             SearchFields = a.SearchFields,
                             IsActive = a.GlobalTenant.IsActive,
                             FreeUsers = a.FreeUsers,
                             IsRecurring = a.IsRecurring,
                             RecurringPeriodCode = a.RecurringPeriodCode,
                             CreateDate = a.CreateDate,
                             UpdateDate = a.UpdateDate,
                             PaymentFailure = a.PaymentFailure,
                             SuspendDate = a.SuspendDate,
                             InternalNotes = a.InternalNotes,
                             BluesnapAccount = a.BluesnapAccount,
                             MainContract = a.MainContract,
                             TemporalPackageCode = a.TemporalPackageCode,
                             TemporalStartDate = a.TemporalStartDate,
                             TemporalEndDate = a.TemporalEndDate,
                             LicensePrice = a.LicensePrice,
                             PaymentChannelCode = a.PaymentChannelCode,
                             PaymentMethodCode = a.PaymentMethodCode,
                             Notes = a.Notes,
                             TTY = a.TTY,
                             APInvoiceLastDate = a.APInvoiceLastDate,
                             APInvoiceTotalLastMonth = a.APInvoiceTotalLastMonth,
                             APInvoiceTotalLastWeek = a.APInvoiceTotalLastWeek,
                             ARInvoiceLastDate = a.ARInvoiceLastDate,
                             ARInvoiceTotalLastMonth = a.ARInvoiceTotalLastMonth,
                             ARInvoiceTotalLastWeek = a.ARInvoiceTotalLastWeek,
                             CustomerLastDate = a.CustomerLastDate,
                             CustomerTotalLastMonth = a.CustomerTotalLastMonth,
                             CustomerTotalLastWeek = a.CustomerTotalLastWeek,
                             LastFHLSentDate = a.LastFHLSentDate,
                             LastFWBSentDate = a.LastFWBSentDate,
                             QuoteLastDate = a.QuoteLastDate,
                             QuoteTotalLastMonth = a.QuoteTotalLastMonth,
                             QuoteTotalLastWeek = a.QuoteTotalLastWeek,
                             ShipmentLastDate = a.ShipmentLastDate,
                             ShipmentTotalLastMonth = a.ShipmentTotalLastMonth,
                             ShipmentTotalLastWeek = a.ShipmentTotalLastWeek,
                             StatisticsUpdateDate = a.StatisticsUpdateDate,
                             CountryName = a.CountryName,
                             LastLoginDateTime = a.LastLoginDateTime,
                             PaymentCurrencyCode = a.PaymentCurrencyCode,
                             IsAWBStockPrepaid = a.IsAWBStockPrepaid,
                             ManageLicencesPerUser = a.ManageLicencesPerUser,
                             DistributorCode = a.DistributorCode,
                             IsDistributorSupportEnabled = a.IsDistributorSupportEnabled,
                             IsSystemSupportEnabled = a.IsSystemSupportEnabled,
                             IsCargonautEnabled = a.IsCargonautEnabled,
                             LastFHLCargonautSentDate = a.LastFHLCargonautSentDate,
                             LastFWBCargonautSentDate = a.LastFWBCargonautSentDate,
                             IsDEXXConnectionEnabled = a.IsDEXXConnectionEnabled,
                             BluesnapContractId = a.BluesnapContractId,
                             AWBMessagesCCSTypeCode = a.AWBMessagesCCSTypeCode,
                             PIMA = a.PIMA,
                             LastFFRSentDate = a.LastFFRSentDate,
                             ActivityLastDate = a.ActivityLastDate,
                             ActivityTotalLastWeek = a.ActivityTotalLastWeek,
                             ActivityTotalLastMonth = a.ActivityTotalLastMonth,
                             OpportunityLastDate = a.OpportunityLastDate,
                             OpportunityTotalLastWeek = a.OpportunityTotalLastWeek,
                             OpportunityTotalLastMonth = a.OpportunityTotalLastMonth,
                             FSULastReceivedDate = a.FSULastReceivedDate,
                             FSALastReceivedDate = a.FSALastReceivedDate,
                             FSRLastSentDate = a.FSRLastSentDate,
                             BillingByLogitude = a.BillingByLogitude,
                             ResellerCommission = a.ResellerCommission,
                             TenantTypeCode = a.TenantTypeCode,
                             TenantConnectedToAirlineCode = a.TenantConnectedToAirlineCode,
                             SupportEmail = a.SupportEmail,
                             SupportActivated = a.SupportActivated,
                             IsMultiPackage = a.IsMultiPackage,
                             MobileLastDate = a.MobileLastDate,
                             MobileTotalLastWeek = a.MobileTotalLastWeek,
                             MobileTotalLastMonth = a.MobileTotalLastMonth,
                             ShardLogisticLastDate = a.ShardLogisticLastDate,
                             ShardLogisticTotalLastWeek = a.ShardLogisticTotalLastWeek,
                             ShardLogisticTotalLastMonth = a.ShardLogisticTotalLastMonth,
                             RequestedAirlines = a.RequestedAirlines,
                             RegisteredAirlines = a.RegisteredAirlines,
                             PendingAirlines = a.PendingAirlines,
                             Technology = a.Technology,
                             PrivateLabelId = a.GlobalTenant != null ? a.GlobalTenant.PrivateLabelId : "",
                             SilverlightEndDate = a.SilverlightEndDate,
                         };

            return query2;
        }

        //public List<TenantManagementList> GetAWBMessagingStockTenantsList(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    IGlobalContext objectContext = GlobalContext.GetContext();

        //    List<TenantManagementList> myResult = (from d in objectContext.TenantManagements.Include("GlobalTenant")
        //                                           where
        //                                           (d.IsAWBStockPrepaid || d.IsINTTRAStockPrepaid)
        //                                           &&
        //                                           (d.GlobalTenant != null && d.GlobalTenant.IsActive)
        //                                           select new TenantManagementList()
        //                                           {
        //                                               Id = d.Id,
        //                                               Name = d.Name,
        //                                               PackageCode = d.PackageCode,
        //                                               IsAWBStockPrepaid = d.IsAWBStockPrepaid,
        //                                               IsINTTRAStockPrepaid = d.IsINTTRAStockPrepaid,
        //                                           }).ToList();

        //    return myResult;
        //}
        
        public TenantManagementPM GetSingleTenantManagementPMBySupportEmail(string supportEmail)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            tenantManagementRepository = new TenantManagementRepository();

            TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagementPMBySupportEmail(supportEmail);

            if (tenantManagement != null)
            {
                TenantManagementPM tenantManagementList = new TenantManagementPM()
                {
                    Id = tenantManagement.Id,
                    Name = tenantManagement.Name,
                    PackageCode = tenantManagement.PackageCode,
                    PackageName = tenantManagement.PackageName,
                    IsTrial = tenantManagement.IsTrial,
                    TrialStartDate = tenantManagement.TrialStartDate,
                    TrialEndDate = tenantManagement.TrialEndDate,
                    FirstPaymentDate = tenantManagement.FirstPaymentDate,
                    PaidUntilDate = tenantManagement.PaidUntilDate,
                    NumberOfUsers = tenantManagement.NumberOfUsers,
                    IsActive = tenantManagement.GlobalTenant.IsActive,
                    GlobalDBId = tenantManagement.GlobalTenant.GlobalDBId,
                    SearchFields = tenantManagement.SearchFields,
                    FreeUsers = tenantManagement.FreeUsers,
                    IsRecurring = tenantManagement.IsRecurring,
                    RecurringPeriodCode = tenantManagement.RecurringPeriodCode,
                    CreateDate = tenantManagement.CreateDate,
                    UpdateDate = tenantManagement.UpdateDate,
                    PaymentFailure = tenantManagement.PaymentFailure,
                    SuspendDate = tenantManagement.SuspendDate,
                    InternalNotes = tenantManagement.InternalNotes,
                    BluesnapAccount = tenantManagement.BluesnapAccount,
                    MainContract = tenantManagement.MainContract,
                    TemporalPackageCode = tenantManagement.TemporalPackageCode,
                    TemporalStartDate = tenantManagement.TemporalStartDate,
                    TemporalEndDate = tenantManagement.TemporalEndDate,
                    LicensePrice = tenantManagement.LicensePrice,
                    PaymentChannelCode = tenantManagement.PaymentChannelCode,
                    PaymentMethodCode = tenantManagement.PaymentMethodCode,
                    Notes = tenantManagement.Notes,
                    TTY = tenantManagement.TTY,
                    APInvoiceLastDate = tenantManagement.APInvoiceLastDate,
                    APInvoiceTotalLastMonth = tenantManagement.APInvoiceTotalLastMonth,
                    APInvoiceTotalLastWeek = tenantManagement.APInvoiceTotalLastWeek,
                    ARInvoiceLastDate = tenantManagement.ARInvoiceLastDate,
                    ARInvoiceTotalLastMonth = tenantManagement.ARInvoiceTotalLastMonth,
                    ARInvoiceTotalLastWeek = tenantManagement.ARInvoiceTotalLastWeek,
                    CustomerLastDate = tenantManagement.CustomerLastDate,
                    CustomerTotalLastMonth = tenantManagement.CustomerTotalLastMonth,
                    CustomerTotalLastWeek = tenantManagement.CustomerTotalLastWeek,
                    LastFHLSentDate = tenantManagement.LastFHLSentDate,
                    LastFWBSentDate = tenantManagement.LastFWBSentDate,
                    QuoteLastDate = tenantManagement.QuoteLastDate,
                    QuoteTotalLastMonth = tenantManagement.QuoteTotalLastMonth,
                    QuoteTotalLastWeek = tenantManagement.QuoteTotalLastWeek,
                    ShipmentLastDate = tenantManagement.ShipmentLastDate,
                    ShipmentTotalLastMonth = tenantManagement.ShipmentTotalLastMonth,
                    ShipmentTotalLastWeek = tenantManagement.ShipmentTotalLastWeek,
                    StatisticsUpdateDate = tenantManagement.StatisticsUpdateDate,
                    CountryName = tenantManagement.CountryName,
                    LastLoginDateTime = tenantManagement.LastLoginDateTime,
                    PaymentCurrencyCode = tenantManagement.PaymentCurrencyCode,
                    IsAWBStockPrepaid = tenantManagement.IsAWBStockPrepaid,
                    ManageLicencesPerUser = tenantManagement.ManageLicencesPerUser,
                    DistributorCode = tenantManagement.DistributorCode,
                    IsDistributorSupportEnabled = tenantManagement.IsDistributorSupportEnabled,
                    IsSystemSupportEnabled = tenantManagement.IsSystemSupportEnabled,
                    IsCargonautEnabled = tenantManagement.IsCargonautEnabled,
                    LastFHLCargonautSentDate = tenantManagement.LastFHLCargonautSentDate,
                    LastFWBCargonautSentDate = tenantManagement.LastFWBCargonautSentDate,
                    IsDEXXConnectionEnabled = tenantManagement.IsDEXXConnectionEnabled,
                    BluesnapContractId = tenantManagement.BluesnapContractId,
                    AWBMessagesCCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode,
                    PIMA = tenantManagement.PIMA,
                    LastFFRSentDate = tenantManagement.LastFFRSentDate,
                    ActivityLastDate = tenantManagement.ActivityLastDate,
                    ActivityTotalLastWeek = tenantManagement.ActivityTotalLastWeek,
                    ActivityTotalLastMonth = tenantManagement.ActivityTotalLastMonth,
                    OpportunityLastDate = tenantManagement.OpportunityLastDate,
                    OpportunityTotalLastWeek = tenantManagement.OpportunityTotalLastWeek,
                    OpportunityTotalLastMonth = tenantManagement.OpportunityTotalLastMonth,
                    FSULastReceivedDate = tenantManagement.FSULastReceivedDate,
                    FSALastReceivedDate = tenantManagement.FSALastReceivedDate,
                    FSRLastSentDate = tenantManagement.FSRLastSentDate,
                    BillingByLogitude = tenantManagement.BillingByLogitude,
                    ResellerCommission = tenantManagement.ResellerCommission,
                    TenantTypeCode = tenantManagement.TenantTypeCode,
                    TenantConnectedToAirlineCode = tenantManagement.TenantConnectedToAirlineCode,
                    SupportActivated = tenantManagement.SupportActivated,
                    SupportEmail = tenantManagement.SupportEmail,
                    IsMultiPackage = tenantManagement.IsMultiPackage,

                    MobileLastDate = tenantManagement.MobileLastDate,
                    MobileTotalLastWeek = tenantManagement.MobileTotalLastWeek,
                    MobileTotalLastMonth = tenantManagement.MobileTotalLastMonth,
                    ShardLogisticLastDate = tenantManagement.ShardLogisticLastDate,
                    ShardLogisticTotalLastWeek = tenantManagement.ShardLogisticTotalLastWeek,
                    ShardLogisticTotalLastMonth = tenantManagement.ShardLogisticTotalLastMonth,
                    RequestedAirlines = tenantManagement.RequestedAirlines,
                    RegisteredAirlines = tenantManagement.RegisteredAirlines,
                    PendingAirlines = tenantManagement.PendingAirlines,
                    Technology = tenantManagement.Technology,
                    PrivateLabelId = tenantManagement.GlobalTenant != null ? tenantManagement.GlobalTenant.PrivateLabelId : "",
                    SilverlightEndDate = tenantManagement.SilverlightEndDate,
                    LastEbookingSentDate = tenantManagement.LastEbookingSentDate,
                    LastSISentDate = tenantManagement.LastSISentDate,
                    NumberOfBookingSentLastWeek = tenantManagement.NumberOfBookingSentLastWeek,
                    NumberOfSISentLastWeek = tenantManagement.NumberOfSISentLastWeek,
                    LastContainerStatusReceived = tenantManagement.LastContainerStatusReceived,
                };

                return tenantManagementList;
            }

            else
            {
                return null;
            }
        }

        public TenantManagementPM GetAirlineTenantExistsForAirline(string airlineCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            tenantManagementRepository = new TenantManagementRepository();
            tenantManagementQuery = new TenantManagementQuery(tenantManagementRepository);

            TenantManagement entityPOCO = tenantManagementRepository.GetTenantManagementByConnectedArline(airlineCode);
            TenantManagementPM myResult = null;

            if (entityPOCO != null)
            {
                myResult = tenantManagementQuery.GetSinglePM(entityPOCO.Id);
            }

            return myResult;
        }
    }
}