using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InvoiceModel;

namespace Logitude.BL.InvoiceModel.CustomFilters
{
    public class APInvoiceCustomFilter
    {
        public int Tenant { get; set; }
        public APInvoiceCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<APInvoice> GetFilteredQuery(QueryOperations operations, IQueryable<APInvoice> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "APPaymentInvoicesSearch")
                    {
                        string searchText = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(searchText))
                        {
                            queryableData =
                                (from r in queryableData
                                 where
                                 !string.IsNullOrEmpty(r.InvoiceNumber) && r.InvoiceNumber.ToUpper().StartsWith(searchText.ToUpper())
                                 ||
                                 r.VendorCard != null && r.VendorCard.EnglishName.ToUpper().Contains(searchText.ToUpper())
                                 select r);
                        }
                    }

                    else if (item.FieldName == "APPaymentInvoicesConnected")
                    {
                        string iPaymentId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(iPaymentId))
                        {
                            IInvoiceContext iContext = InvoiceContext.GetContext(Tenant);
                            List<string> ids = (from d in iContext.APInvoicePayments where d.Tenant == Tenant && d.APPaymentId == iPaymentId select d.APInvoiceId).ToList();
                            queryableData = (from d in queryableData where ids.Contains(d.Id) select d);
                        }
                    }

                    else if (item.FieldName == "Reference")
                    {
                        string value = item.FieldValue as string;
                        APInvoiceEntityRepository apInvoiceEntityrep = new APInvoiceEntityRepository(Tenant);
                        IQueryable<APInvoiceEntity> apInvoiceEntities = apInvoiceEntityrep.GetAPInvoiceEntitiesByTenant(Tenant);
                        apInvoiceEntities = apInvoiceEntities.Where(e => e.EntityReference.StartsWith(value));
                        queryableData = queryableData.Where(d => d.InvoiceNumber.StartsWith(value) || apInvoiceEntities.Where(e => e.APInvoiceId == d.Id && e.EntityReference.StartsWith(value)).Any());
                        //queryableData = queryableData.Where(d => d.APInvoiceEntities.Where(e => e.EntityReference.StartsWith(value)).Any() || d.InvoiceNumber.StartsWith(value));
                    }

                    else if (item.FieldName == "SearchReadyInvoices")
                    {
                        string searchText = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(searchText))
                        {
                            queryableData =
                                (from r in queryableData
                                 where
                                 !string.IsNullOrEmpty(r.InvoiceNumber) && r.InvoiceNumber.ToUpper().StartsWith(searchText.ToUpper())
                                 ||
                                 r.VendorCard != null && r.VendorCard.EnglishName.ToUpper().Contains(searchText.ToUpper())
                                 select r);
                        }
                    }

                    else if (item.FieldName == "UnpaidInvoices")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD" && d.IsClosed == false);
                    }

                    else if (item.FieldName == "NotReadyInvoices")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD" && d.TransferStatusCode == "NR");
                    }

                    else if (item.FieldName == "ErrorInTransferInvoices")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD" && d.TransferStatusCode == "ET");
                    }

                    else if (item.FieldName == "MarkedAsBlockedForTransfer")
                    {
                        queryableData = queryableData.Where(d => d.StatusCode != "WA" && d.StatusCode != "VD" && d.TransferStatusCode == "BL");
                    }

                    else if (item.FieldName == "TransferedInvoices")
                    {
                        queryableData = queryableData.Where(d => d.TransferStatusCode == "TR");
                    }

                    else if (item.FieldName == "DraftGeneralAPInvoices")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false  && d.IsGeneralInvoice == true && d.StatusCode == "DR");
                    }

                    else if (item.FieldName == "ApprovalGeneralAPInvoices")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.IsGeneralInvoice == true && d.ApprovedDate != null);
                    }

                }
            }

            return queryableData;
        }
    }
}
