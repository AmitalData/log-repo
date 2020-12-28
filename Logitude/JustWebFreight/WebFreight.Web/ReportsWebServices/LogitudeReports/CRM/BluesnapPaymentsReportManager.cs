using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using System.Net;
using System.Data.Entity;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Bluesnap
{
    public class BluesnapPaymentsReportManager
    {
        private int tenant;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private bool showAllRecurringTenants = false;
        private BluesnapPaymentsDataProvider iDataProvider;
        private IQueryable<TenantJoinBluesnapTransactionList> iQueryable_JoinTenantBluesnapTransaction;
        private IQueryable<BluesnapTransaction> iQueryable_BluesnapTransactions;
        ICommonDataContext commonDataContext;
        IBlobService storageservice;
        DocumentRepository documentRepository;
        private List<BlusnapTransactionsList> otherTenantsTransactions;
        private List<CustomerCRMData> customers_CRM;
        public BluesnapPaymentsReportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            documentRepository = new DocumentRepository(tenant);
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            this.FilterByDates(iQueryOperations);
            this.FilterByShowAllRecurringTenants(iQueryOperations);
            commonDataContext = CommonDataContext.GetContext(tenant); 
        }

        private void FilterByDates(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_FromDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }
            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_ToDate.FieldValue;
                }
            }
        }

        private void FilterByShowAllRecurringTenants(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_ShowAllRecurringTenants = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShowAllRecurringTenants").FirstOrDefault();

            if (filterItem_ShowAllRecurringTenants != null)
            {
                if (filterItem_ShowAllRecurringTenants.FieldValue != null)
                {
                    showAllRecurringTenants = Convert.ToBoolean(filterItem_ShowAllRecurringTenants.FieldValue);
                }
            }
        }

        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BluesnapPaymentsDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, iDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private void LoadDataProvider()
        {
            this.iDataProvider = new BluesnapPaymentsDataProvider()
            {
                BlusnapTransactionsList = new List<BlusnapTransactionsList>(),
            };

            this.BuildReportHeader();
            this.BuildSourceData();
            this.BuildReportData();
        }

        private void BuildReportHeader()
        {
            iDataProvider.FromDate = this.fromDate;
            iDataProvider.ToDate = this.toDate;
            iDataProvider.ShowAllRecurringTenants = this.showAllRecurringTenants;
        }

        private void BuildSourceData()
        {
            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            IQueryable<TenantManagement> iQueryable_Tenantmanagements = (from a in globalObjectContext.TenantManagements.Include("GlobalTenant")
                                                                         where a.GlobalTenant.IsActive && a.IsRecurring == true && a.RecurringPeriodCode == "MO" && a.PaymentChannelCode == "PL"
                                                                         select a);
            iQueryable_BluesnapTransactions = globalObjectContext.BluesnapTransactions;
            iQueryable_BluesnapTransactions = iQueryable_BluesnapTransactions.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.TransactionDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate) && System.Data.Entity.DbFunctions.TruncateTime(d.TransactionDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
            List<string> tenantsIds = iQueryable_Tenantmanagements.ToList().Select(e => e.Id.ToString()).ToList();
            this.customers_CRM = (from a in commonDataContext.Cards.Include("Customer")
                             where a.Tenant == 341 && !string.IsNullOrEmpty(a.ReceivablesAccountingCard) && tenantsIds.Contains(a.ReceivablesAccountingCard)  
                             select new CustomerCRMData
                             {
                                 EnglishName = a.EnglishName,
                                 ReceivablesAccountingCard = a.ReceivablesAccountingCard
                             }).ToList();
                         
            this.iQueryable_JoinTenantBluesnapTransaction = (from tenantmanagements in iQueryable_Tenantmanagements
                                                             select new TenantJoinBluesnapTransactionList()
                                                             {
                                                                 Tenant = tenantmanagements.Id,
                                                                 TenantName = tenantmanagements.Name,
                                                                 AmountToPay = tenantmanagements.TotalPaymentamount,
                                                                 ShopperId = tenantmanagements.BluesnapAccount,
                                                                 IsParentTenant = tenantmanagements.IsParentTenant,
                                                                 ParentTenantId = tenantmanagements.ParentTenantId,
                                                                 NoPaymentForChildTenants = tenantmanagements.NoPaymentForChildTenants,
                                                                 Transactions = (from a in iQueryable_BluesnapTransactions
                                                                                 where a.Tenant == tenantmanagements.Id
                                                                                 select new BluesnapTransactionItem()
                                                                                 {
                                                                                     Tenant = a.Tenant,
                                                                                     DocumentId = a.DocumentId,
                                                                                     TransactionDate = a.TransactionDate,
                                                                                 }).ToList(),
                                                             });

        }

       

        private void BuildReportData()
        {
            this.iDataProvider.BlusnapTransactionsList = new List<BlusnapTransactionsList>();
            this.BuildOthersTenantData();
        }

        private void BuildOthersTenantData()
        {
            otherTenantsTransactions = new List<BlusnapTransactionsList>();
            var otherTenantTransactions_List = iQueryable_JoinTenantBluesnapTransaction.Where(a => a.Tenant != 0).ToList();

            foreach (var item in otherTenantTransactions_List)
            {
                bool isAddingTenant = true;
                if(item.ParentTenantId != null)
                {
                    var parentTenant = (from a in iQueryable_JoinTenantBluesnapTransaction where a.Tenant == item.ParentTenantId select a).FirstOrDefault();
                    if(parentTenant != null)
                    {
                        if (parentTenant.NoPaymentForChildTenants)
                        {
                            isAddingTenant = false;
                        }
                    }
                }

                if (isAddingTenant)
                {
                    AddToBlueSnapTransactionList(item);
                }
            }

            this.iDataProvider.BlusnapTransactionsList.AddRange(otherTenantsTransactions);
            this.BuildTenantZeroData();
        }

        private void BuildTenantZeroData()
        {
            List<BlusnapTransactionsList> tenantZeroTransactions = new List<BlusnapTransactionsList>();
            var tenantZeroTransactions_List = iQueryable_JoinTenantBluesnapTransaction.Where(a => a.Tenant == 0).ToList();
            SetShopperIdField(tenantZeroTransactions_List);

            var tenantZeroList = (from tenant in BluesnapTransactionItem_TenantZeroList
                                  group tenant by tenant.ShopperId into g
                                  select new
                                  {
                                      ShopperId = g.Key,
                                      Transactions = g.ToList(),
                                  }).ToList();

            foreach (var item in tenantZeroList)
            {
                var itemRecord = new BlusnapTransactionsList();
                itemRecord.Tenant = null;
                itemRecord.TenantName = "";
                itemRecord.ShopperId = item.ShopperId;
                itemRecord.TransactionCount = item.Transactions != null ? item.Transactions.Count() : 0;
                this.CalculateContractCountAndTotalPayments(itemRecord, item.Transactions);
                itemRecord.Notes = "unmatched transaction";
                tenantZeroTransactions.Add(itemRecord);
            }
            this.iDataProvider.BlusnapTransactionsList.AddRange(tenantZeroTransactions);
        }

        private List<BluesnapTransactionItem> BluesnapTransactionItem_TenantZeroList;
        private void SetShopperIdField(List<TenantJoinBluesnapTransactionList> list)
        {
            BluesnapTransactionItem_TenantZeroList = new List<BluesnapTransactionItem>();
            if (list != null)
            {
                var bluesnapTransaction = list.ToList();
                foreach (var item in bluesnapTransaction)
                {
                    if (item.ShopperId == null)
                    {
                        foreach (BluesnapTransactionItem transaction in item.Transactions)
                        {
                            var queryParameters = DeserializeDocumentBody(transaction.DocumentId, transaction.Tenant);
                            string shopperId = null;
                            if (queryParameters != null && queryParameters.Count > 0)
                            {
                                if (queryParameters.ContainsKey("accountId"))
                                {
                                    shopperId = queryParameters["accountId"];
                                }
                            }

                            BluesnapTransactionItem_TenantZeroList.Add(new BluesnapTransactionItem
                            {
                                ShopperId = shopperId,
                                DocumentId = transaction.DocumentId,
                                Tenant = transaction.Tenant, 
                                TransactionDate = transaction.TransactionDate,
                            });
                        }
                    }
                }
            }
        }

        private void AddToBlueSnapTransactionList(TenantJoinBluesnapTransactionList item)
        {
            var itemRecord = new BlusnapTransactionsList();
            itemRecord.Tenant = item.Tenant;
            itemRecord.TenantName = item.TenantName;

            itemRecord.ShopperId = item.ShopperId;
            itemRecord.AmountToPay = item.AmountToPay;
            itemRecord.TransactionCount = item.Transactions != null ? item.Transactions.Count() : 0;
            this.CalculateContractCountAndTotalPayments(itemRecord, item.Transactions);
            itemRecord.PaymentDifference = itemRecord.TotalPayments - itemRecord.AmountToPay;
            itemRecord.Notes = itemRecord.PaymentDifference != 0 ? "payment missing " : "";
            if (!this.showAllRecurringTenants && itemRecord.PaymentDifference != null && itemRecord.PaymentDifference != 0)
            {
                otherTenantsTransactions.Add(itemRecord);
            }
            if (this.showAllRecurringTenants)
            {
                otherTenantsTransactions.Add(itemRecord);
            }
            itemRecord.CRMCustomer = this.customers_CRM.Where(e => e.ReceivablesAccountingCard == item.Tenant.ToString()).Select(a=>a.EnglishName).FirstOrDefault();
        }

        private void CalculateContractCountAndTotalPayments(BlusnapTransactionsList itemRecord, List<BluesnapTransactionItem> transactions)
        {
            double? totalPayments = 0;
            List<string> Contracts = new List<string>();
            foreach (var transaction in transactions)
            {
                var queryParameters = DeserializeDocumentBody(transaction.DocumentId, transaction.Tenant);
                if (queryParameters != null && queryParameters.Count > 0)
                {
                    if (queryParameters.ContainsKey("contractId"))
                    {
                        Contracts.Add(queryParameters["contractId"]);
                    }

                    if (queryParameters.ContainsKey("invoiceAmountUSD"))
                    {
                        if(queryParameters["invoiceAmountUSD"] != null)
                        {
                            double result = 0;
                            Double.TryParse(queryParameters["invoiceAmountUSD"], out result);
                            totalPayments += result;
                        }
                    }

                    if (queryParameters.ContainsKey("taxAmountUSD"))
                    {
                        if (queryParameters["taxAmountUSD"] != null)
                        {
                            double result = 0;
                            Double.TryParse(queryParameters["taxAmountUSD"], out result);
                            totalPayments -= result;
                        }
                    }
                }
            }
            itemRecord.ContractCount = new HashSet<string>(Contracts).Count();
            itemRecord.TotalPayments = totalPayments;
        }

        private Dictionary<string, string> DeserializeDocumentBody(string documentId, int tenant)
        {
            Dictionary<string, string> queryParameters = null;
            Document document = documentRepository.GetSingleDocument(tenant, documentId);
            if (document != null)
            {
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = document.FileSize,
                };

                byte[] fileData = storageservice.Read(fileInfo);

                if (fileData != null)
                {
                    string Stringdetails = Encoding.UTF8.GetString(fileData);
                    queryParameters = new Dictionary<string, string>();
                    string[] querySegments = Stringdetails.Split('&');
                    foreach (string segment in querySegments)
                    {
                        string[] parts = segment.Split('=');
                        if (parts.Length > 0)
                        {
                            string key = parts[0].Trim(new char[] { '?', ' ' });
                            string val = parts[1].Trim();
                            if (!queryParameters.ContainsKey(key))
                            {
                                queryParameters.Add(WebUtility.UrlDecode(key), WebUtility.UrlDecode(val));
                            }
                        }
                    }
                }
            }
            return queryParameters;
        }
    }

    public class TenantJoinBluesnapTransactionList
    {
        public TenantJoinBluesnapTransactionList()
        {
            this.Transactions = new List<BluesnapTransactionItem>();
        }

        public int Tenant { get; set; }
        public string TenantName { get; set; }
        public string ShopperId { get; set; }
        public double? AmountToPay { get; set; }
        public bool IsParentTenant { get; set; }
        public int? ParentTenantId { get; set; }
        public bool NoPaymentForChildTenants { get; set; }
        public List<BluesnapTransactionItem> Transactions { get; set; }
    }

    public class BluesnapTransactionItem
    {
        public int Tenant { get; set; }
        public string DocumentId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string ShopperId { get; set; }
    }

    public class CustomerCRMData
    {
        public string EnglishName { get; set; }
        public string ReceivablesAccountingCard { get; set; }
    }
}