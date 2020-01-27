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

        public BluesnapPaymentsReportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            this.FilterByDates(iQueryOperations);
            this.FilterByShowAllRecurringTenants(iQueryOperations);
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
            ICommonDataContext iContext = CommonDataContext.GetContext(tenant);
            IQueryable<TenantManagement> iQueryable_Tenantmanagements = globalObjectContext.TenantManagements;
            iQueryable_BluesnapTransactions = globalObjectContext.BluesnapTransactions;
            iQueryable_BluesnapTransactions = iQueryable_BluesnapTransactions.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate) && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));

            if (this.showAllRecurringTenants == true)
            {
                iQueryable_Tenantmanagements = iQueryable_Tenantmanagements.Where(a => a.IsRecurring == true && a.RecurringPeriodCode == "MO");
            }

            this.iQueryable_JoinTenantBluesnapTransaction = (from tenantmanagements in iQueryable_Tenantmanagements
                                                             select new TenantJoinBluesnapTransactionList()
                                                             {
                                                                 Tenant = tenantmanagements.Id,
                                                                 TenantName = tenantmanagements.Name,
                                                                 ShopperId = tenantmanagements.BluesnapAccount,
                                                                 AmountToPay = tenantmanagements.TotalPrice,
                                                                 Transactions = (from a in iQueryable_BluesnapTransactions
                                                                                 join documents in iContext.Documents
                                                                                 on new { Id = a.DocumentId }
                                                                                 equals new { Id = documents.Id }
                                                                                 where a.Tenant == tenantmanagements.Id
                                                                                 select new BluesnapTransactionItem()
                                                                                 {
                                                                                     FileInfo = new BlobFileInfo()
                                                                                     {
                                                                                         Tenant = tenant,
                                                                                         FileName = documents == null ? null : documents.Id,
                                                                                         FolderName = documents == null ? null : documents.Folder,
                                                                                         Extension = documents == null ? null : documents.Extension,
                                                                                         FileSize = documents == null ? null : documents.FileSize,
                                                                                     },
                                                                                 }).ToList(),
                                                             });
        }

        private void BuildReportData()
        {
            var tenantTransactions = new List<BlusnapTransactionsList>();
            foreach (var item in iQueryable_JoinTenantBluesnapTransaction)
            {
                var itemRecord = new BlusnapTransactionsList();
                itemRecord.Tenant = item.Tenant;
                itemRecord.TenantName = item.TenantName;
                itemRecord.ShopperId = item.ShopperId;
                itemRecord.AmountToPay = item.AmountToPay;
                itemRecord.TransactionCount = item.Transactions != null ? item.Transactions.Count() : 0;
                double? TotalPayments = 0;
                foreach (var transaction in item.Transactions)
                {
                    var queryParameters = DeserializeDocumentBody(transaction.FileInfo);
                    if (queryParameters.Count > 0)
                    {
                        itemRecord.ContractCount = int.Parse(queryParameters["promoteContractsNum"]);
                        TotalPayments += Double.Parse(queryParameters["invoiceAmountUSD"]);
                    }
                   
                }
                itemRecord.TotalPayments = TotalPayments;
                itemRecord.PaymentDifference = itemRecord.TotalPayments - itemRecord.AmountToPay;
                itemRecord.Notes = itemRecord.PaymentDifference != 0 ? "payment missing " : "";
                tenantTransactions.Add(itemRecord);
            }

            this.iDataProvider.BlusnapTransactionsList = tenantTransactions;
        }

        private Dictionary<string, string> DeserializeDocumentBody(BlobFileInfo fileInfo)
        {
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] fileData = storageservice.Read(fileInfo);
            string Stringdetails = Encoding.UTF8.GetString(fileData);

            Dictionary<string, string> queryParameters = new Dictionary<string, string>();
            string[] querySegments = Stringdetails.Split('&');
            foreach (string segment in querySegments)
            {
                string[] parts = segment.Split('=');
                if (parts.Length > 0)
                {
                    string key = parts[0].Trim(new char[] { '?', ' ' });
                    string val = parts[1].Trim();

                    queryParameters.Add(WebUtility.UrlDecode(key), WebUtility.UrlDecode(val));
                }
            }
            return queryParameters;
        }
    }

    public class TenantJoinBluesnapTransactionList
    {
        public int Tenant { get; set; }
        public string TenantName { get; set; }
        public string ShopperId { get; set; }
        public double? AmountToPay { get; set; }
        public List<BluesnapTransactionItem> Transactions { get; set; }
    }
    public class BluesnapTransactionItem
    {
        public BlobFileInfo FileInfo { get; set; }
    }
}