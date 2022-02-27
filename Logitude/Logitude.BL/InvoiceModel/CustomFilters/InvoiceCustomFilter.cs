using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using Simplog.Data.Helpers;
using System.Data.Entity.Core.Objects;
using Simplog.Data.InvoiceModel;
using Logitude.BL.ShipmentsModel.CustomFilters;

namespace Logitude.BL.InvoiceModel.CustomFilters
{
    public class InvoiceCustomFilter
    {
        public int Tenant { get; set; }
        public InvoiceCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<ARInvoice> GetFilteredQuery(QueryOperations operations, IQueryable<ARInvoice> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            bool skipConstituentFilter = false;
            bool showIsConstituentInvoice = true;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "DigitalQuickSearch")
                    {
                        queryableData = DigitalCustomFilter.ApplyDigitalQuickSearchFilter(item, queryableData);
                    }

                    if (item.FieldName == "OpenConstituentInvoices")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.IsCancelled == false && d.IsConstituentInvoice == true && string.IsNullOrEmpty(d.ConsolidationInvoiceId) && d.StatusCode != "VD");
                    }

                    else if (item.FieldName == "SearchReadyInvoices")
                    {
                        showIsConstituentInvoice = false;

                        string searchText = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(searchText))
                        {
                            queryableData =
                                (from r in queryableData
                                 where
                                 !string.IsNullOrEmpty(r.InvoiceNumber) && r.InvoiceNumber.ToUpper().Contains(searchText.ToUpper())
                                 ||
                                 r.BillTo != null && r.BillTo.EnglishName.ToUpper().Contains(searchText.ToUpper())
                                 select r);
                        }
                    }

                    else if (item.FieldName == "ARPaymentInvoicesSearch")
                    {
                        showIsConstituentInvoice = false;

                        string searchText = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(searchText))
                        {
                            queryableData =
                                (from r in queryableData
                                 where
                                 !string.IsNullOrEmpty(r.InvoiceNumber) && r.InvoiceNumber.ToUpper().StartsWith(searchText.ToUpper())
                                 ||
                                 r.BillTo != null && r.BillTo.EnglishName.ToUpper().Contains(searchText.ToUpper())
                                 select r);
                        }
                    }

                    else if (item.FieldName == "ARPaymentInvoicesConnected")
                    {
                        string iPaymentId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(iPaymentId))
                        {
                            IInvoiceContext iContext = InvoiceContext.GetContext(Tenant);
                            List<string> ids = (from d in iContext.ARInvoicePayments where d.Tenant == Tenant && d.ARPaymentId == iPaymentId select d.ARInvoiceId).ToList();
                            queryableData = (from d in queryableData where ids.Contains(d.Id) select d);
                        }
                    }

                    else if (item.FieldName == "UnpaidInvoices")
                    {
                        skipConstituentFilter = true;
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.IsCancelled == false && d.StatusCode != "LL");
                        queryableData = queryableData.Where(d => (d.StatusCode != "DR" && d.StatusCode != "VD" && d.IsAutoCredit == false) || (d.IsConstituentInvoice && !string.IsNullOrEmpty(d.ConsolidationInvoiceId)));
                    }

                    else if (item.FieldName == "NotReadyInvoices")
                    {
                        showIsConstituentInvoice = false;
                        queryableData = queryableData.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && d.TransferStatusCode == "NR" && d.StatusCode != "LL");
                    }

                    else if (item.FieldName == "ErrorInTransferInvoices")
                    {
                        showIsConstituentInvoice = false;
                        queryableData = queryableData.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && !d.IsCancelled && d.TransferStatusCode == "ET");
                    }

                    else if (item.FieldName == "MarkedAsBlockedForTransfer")
                    {
                        showIsConstituentInvoice = false;
                        queryableData = queryableData.Where(d => d.StatusCode != "DR" && d.StatusCode != "VD" && !d.IsCancelled && d.TransferStatusCode == "BL");
                    }

                    else if (item.FieldName == "TransferedInvoices")
                    {
                        showIsConstituentInvoice = false;
                        queryableData = queryableData.Where(d => d.TransferStatusCode == "TR");
                    }
                    else if (item.FieldName == "OpenInvoices")
                    {
                        showIsConstituentInvoice = false;
                        queryableData = queryableData.Where(d => d.AmountDue != 0);
                    }
                    else if (item.FieldName == "IsConsolidationInvoice")
                    {
                        if (item.FieldValue != null)
                        {
                            bool myFlag = (bool)item.FieldValue;
                            queryableData = queryableData.Where(d => d.IsConsolidationInvoice == myFlag);
                        }                        
                    }
                    
                    else if (item.FieldName == "DraftGeneralInvoices")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.IsCancelled == false  && d.StatusCode == "DR");
                    }

                    else if (item.FieldName == "ApprovalGeneralInvoices")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.IsCancelled == false  && (d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsAutoCredit == false));
                    }

                    else if (item.FieldName == "IsCustomsInvoice")
                    {
                        if (item.FieldValue != null)
                        {
                            bool myFlag = (bool)item.FieldValue;
                            if (myFlag)
                            {
                                queryableData = queryableData.Where(d => d.ARInvoiceTypeCode == "CI" || d.ARInvoiceTypeCode == "CC");
                            }
                        }
                    }

                    else if (item.FieldName == "DailySpotlightFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();

                            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(Tenant).Date;
                            DateTime date1 = todayDate;
                            DateTime date2 = todayDate;

                            switch (code)
                            {
                                case "AR_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "AR_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "AR_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                            }

                            queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= date2);
                        }
                    }
                }
            }

            if (!skipConstituentFilter)
            {
                if (!showIsConstituentInvoice)
                {
                    queryableData = queryableData.Where(d => d.IsConstituentInvoice == false);
                }
            }

            return queryableData;
        }
    }
}
