using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APInvoiceRepository: IRepository<APInvoice>
    {
        IInvoiceContext invoiceContext;
        public APInvoiceRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public APInvoiceRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public APInvoiceRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public APInvoice GetSingleAPInvoice(string id, int tenant)
        {
            return (from a in context.APInvoices.Include("Status").Include("LocalCurrency").Include("InvoiceCurrency").Include("ProfitCurrency").Include("VendorCard").Include("PaymentTerm").Include("TransferStatus").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact").Include("Branch")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<APInvoice> GetDraftsAPInvoices(int tenant)
        {
            return context.APInvoices.Where(d => d.Tenant == tenant && d.StatusCode == "WA");
        }

        public IQueryable<APInvoice> GetErrorInTransferAPInvoices(int tenant)
        {
            return context.APInvoices.Where(d => d.Tenant == tenant && d.TransferStatusCode == "ET");
        }

        public IQueryable<APInvoice> GetAPInvoicesWithWA(int tenant)
        {
            return context.APInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "VD");
        }

        public IQueryable<APInvoice> GetUnpaidAPInvoices(int tenant)
        {
            return context.APInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "WA" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsClosed == false);
        }

        public IQueryable<APInvoice> GetVoidAPInvoices(int tenant)
        {
            return context.APInvoices.Where(d => d.Tenant == tenant && d.StatusCode == "VD");
        }

        public IQueryable<APInvoice> GetAPInvoicesWithVoid(int tenant)
        {
            return context.APInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "WA");
        }

        public IQueryable<APInvoice> GetAccountingLedgerAPInvoices(int tenant)
        {
            return context.APInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "WA" && d.StatusCode != "VD" && d.StatusCode != "AC");
        }

        public APInvoice GetAPInvoiceByInvoiceNumber(int tenant, string invoiceNumber)
        {
            return (from a in context.APInvoices where a.InvoiceNumber == invoiceNumber && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<APInvoice> GetAllAPInvoicesWithoutVoidAndDraft(int tenant)
        {
            return context.APInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "WA" && d.StatusCode != "VD");
        }

        public IQueryable<APInvoice> GetAPInvoices(int tenant)
        {
            return (from a in context.APInvoices where a.Tenant == tenant select a);
        }

        public IQueryable<APInvoice> GetIQueryableInvoices(int tenant)
        {
            return (from a in context.APInvoices where a.Tenant == tenant select a);
        }

        public List<APInvoice> GetInvoicesListFromIdList(List<string> ids, int tenant)
        {
            List<APInvoice> invoices = new List<APInvoice>();

            if (ids.Count > 0)
            {
                invoices = (from a in context.APInvoices.Include("InvoiceCurrency")
                            where a.Tenant == tenant && ids.Contains(a.Id)
                            select a).ToList();
            }

            return invoices;
        }

        public bool IsInvoiceNumberExists(string invoiceNumber, int tenent)
        {
            return context.ARInvoices.Where(d => d.InvoiceNumber == invoiceNumber && d.Tenant == tenent).Any();
        }

        public IQueryable<APAgingReportDataView> GetAgingReportAPInvoiceDataView(int tenant, int index)
        {
            //APInvoiceAgingReportContext agingContext = new APInvoiceAgingReportContext();
            IApInvoiceAgingReportContext agingContext = APInvoiceAgingReportContext.GetContext(tenant);

            return (from a in agingContext.AgingReportInvoiceDataViews
                   where a.Tenant == tenant && a.StatusCode != "VD" && a.StatusCode != "PD"
                   select a);


        }

        public List<AgingReportDashboardClass> GetAgingReportAPInvoiceData(int tenant, int index, IQueryable<APAgingReportDataView> agingReportInvoiceDataViews)
        {
            //APInvoiceAgingReportContext agingContext = new APInvoiceAgingReportContext();
            IApInvoiceAgingReportContext agingContext = APInvoiceAgingReportContext.GetContext(tenant);
            //var datalist = (from a in agingContext.AgingReportInvoiceDataViews
            //                where a.Tenant == tenant && a.StatusCode != "PD" && a.StatusCode != "VD"
            //                group a by new
            //                {
            //                    a.DateRange,
            //                    AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
            //                    AmountDueInProfitCurrency=a.AmountDueInProfitCurrency,
            //                    a.IndexOrder,

            //                } into inv
            //                orderby inv.Key.DateRange
            //                select new
            //                {
            //                    DateRange = inv.Key.DateRange,
            //                    AmountDueInLocalCurrency = inv.Key.AmountDueInLocalCurrency,
            //                    AmountDueInProfitCurrency = inv.Key.AmountDueInProfitCurrency,
            //                    IndexOrder = inv.Key.IndexOrder,

            //                }
            //                                    ).ToList();

            List<AgingReportDashboardClass> finalResult = (from a in agingContext.AgingReportInvoiceDataViews
                                                           where a.Tenant == tenant && a.StatusCode != "VD" && a.StatusCode != "PD" && a.StatusCode != "DR"
                                                           group a by new
                                                           {
                                                               a.DateRange,
                                                               a.IndexOrder,

                                                           } into inv
                                                           orderby inv.Key.IndexOrder
                                                           select new AgingReportDashboardClass()
                                                           {
                                                               DateRange = inv.Key.DateRange,
                                                               Amount = index == 1 ? inv.Sum(d => d.AmountDueInLocalCurrency) : inv.Sum(d => d.AmountDueInProfitCurrency),
                                                               IndexOrder = inv.Key.IndexOrder,
                                                           }
                                                ).ToList();
            foreach (AgingReportDashboardClass c in finalResult)
            {
                c.DateRangeLabel = c.DateRange + ": " + c.Amount.ToString();
                c.AmountLabel = string.Format("{0:#,0.00}", c.Amount);
            }

            finalResult = FillEmptyDateRanges(finalResult);

            return finalResult;
           

        }

        public List<AgingReportDashboardClass> FillEmptyDateRanges(List<AgingReportDashboardClass> datalist)
        {
            AgingReportDashboardClass currentitem = (from a in datalist
                                                     where a.DateRange == "Current"
                                                     select a).FirstOrDefault();
            if (currentitem == null)
            {
                currentitem = new AgingReportDashboardClass() { DateRange = "Current", Amount = 0, DueDate = DateTime.Now.Date, IndexOrder = 5, DateRangeLabel = "Current: 0" };
                datalist.Add(currentitem);
            }

            AgingReportDashboardClass firstItem = (from a in datalist
                                                   where a.DateRange == "1-30"
                                                   select a).FirstOrDefault();
            if (firstItem == null)
            {
                firstItem = new AgingReportDashboardClass() { DateRange = "1-30", Amount = 0, DueDate = DateTime.Now.Date.AddDays(15), IndexOrder = 4, DateRangeLabel = "1-30: 0" };
                datalist.Add(firstItem);
            }

            AgingReportDashboardClass secondItem = (from a in datalist
                                                    where a.DateRange == "31-60"
                                                    select a).FirstOrDefault();
            if (secondItem == null)
            {

                secondItem = new AgingReportDashboardClass() { DateRange = "31-60", Amount = 0, DueDate = DateTime.Now.Date.AddDays(45), IndexOrder = 3, DateRangeLabel = "31-60: 0" };
                datalist.Add(secondItem);
            }

            AgingReportDashboardClass thirdItem = (from a in datalist
                                                   where a.DateRange == "61-90"
                                                   select a).FirstOrDefault();
            if (thirdItem == null)
            {
                thirdItem = new AgingReportDashboardClass() { DateRange = "61-90", Amount = 0, DueDate = DateTime.Now.Date.AddDays(75), IndexOrder = 2, DateRangeLabel = "61-90: 0" };
                datalist.Add(thirdItem);
            }

            AgingReportDashboardClass fourthItem = (from a in datalist
                                                    where a.DateRange == "+90"
                                                    select a).FirstOrDefault();
            if (fourthItem == null)
            {
                fourthItem = new AgingReportDashboardClass() { DateRange = "+90", Amount = 0, DueDate = DateTime.Now.Date.AddDays(95), IndexOrder = 1, DateRangeLabel = "+90: 0" };
                datalist.Add(fourthItem);
            }
            datalist = datalist.OrderBy(d => d.IndexOrder).ToList();
            return datalist;

        }
     
        public void Add(APInvoice entity)
        {
            context.APInvoices.Add(entity);
        }

        public void Remove(APInvoice entity)
        {
            context.APInvoices.Attach(entity);
            context.APInvoices.Remove(entity);
        }

        public void Update(APInvoice entity)
        {
            try
            {
                context.APInvoices.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<APInvoice> All()
        {
            return  context.APInvoices.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<APInvoice> GetInvoicesByShipmentId(string shipmentId, int tenant)
        {
            List<APInvoice> list = (from a in context.APInvoiceEntities
                                    where a.Tenant == tenant
                                    && a.EntityId == shipmentId
                                    && (a.ObjectTable.Name == "Shipment" || a.ObjectTable.Name == "Master")
                                    select a.APInvoice).ToList();
            return list;
        }

        public List<APInvoice> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public APInvoice GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
