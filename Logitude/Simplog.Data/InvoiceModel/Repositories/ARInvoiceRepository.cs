using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARInvoiceRepository: IRepository<ARInvoice>
    {
        IInvoiceContext invoiceContext;

        public ARInvoiceRepository(IInvoiceContext context)
        {
            invoiceContext = context;

        }

        public ARInvoiceRepository()
        {
            invoiceContext = new InvoiceContext();

        }

        public ARInvoiceRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public string GetAutoCreditByInvoiceNumber(string cancelledByArInvoiceId, int tenant)
        {
            string result = "";

            ARInvoice entity = (from r in invoiceContext.ARInvoices where r.CancelledByARInvoiceId == cancelledByArInvoiceId && r.Tenant == tenant select r).FirstOrDefault();
            if (entity != null)
            {
                if (string.IsNullOrEmpty(entity.InvoiceNumber))
                {
                    result = entity.DraftNumber;
                }

                else
                {
                    if (entity.InvoiceNumber == entity.Id)
                    {
                        result = entity.DraftNumber;
                    }

                    else
                    {
                        result = entity.InvoiceNumber;
                    }
                }
            }

            return result;
        }       
        
        public string GetAutoCreditByInvoiceId(string cancelledByArInvoiceId, int tenant)
        {
            string result = "";
            result = (from r in invoiceContext.ARInvoices where r.CancelledByARInvoiceId == cancelledByArInvoiceId && r.Tenant == tenant select r.Id).FirstOrDefault();

            return result;
        }

        public string GetAutoCreditedByInvoiceNumber(string creditedByARInvoiceId, int tenant)
        {
            string result = "";

            ARInvoice entity = (from r in invoiceContext.ARInvoices where r.CreditedByARInvoiceId == creditedByARInvoiceId && r.Tenant == tenant select r).FirstOrDefault();
            if (entity != null)
            {
                if (string.IsNullOrEmpty(entity.InvoiceNumber))
                {
                    result = entity.DraftNumber;
                }

                else
                {
                    if (entity.InvoiceNumber == entity.Id)
                    {
                        result = entity.DraftNumber;
                    }

                    else
                    {
                        result = entity.InvoiceNumber;
                    }
                }
            }

            return result;
        }

        public string GetAutoCreditedByInvoiceId(string creditedByARInvoiceId, int tenant)
        {
            string result = "";
            result = (from r in invoiceContext.ARInvoices where r.CreditedByARInvoiceId == creditedByARInvoiceId && r.Tenant == tenant select r.Id).FirstOrDefault();

            return result;
        }

        public string GetInvoiceNumber(string invoiceId, int tenant)
        {
            string myResult = invoiceId;

            ARInvoice entity = (from a in invoiceContext.ARInvoices
                                    where a.Id == invoiceId && a.Tenant == tenant
                                    select a).FirstOrDefault();

            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.InvoiceNumber))
                {
                    myResult = entity.InvoiceNumber;
                }

                else if (!string.IsNullOrEmpty(entity.DraftNumber))
                {
                    myResult = entity.DraftNumber;
                }
            }

            return myResult;
        }

        public IQueryable<ARInvoice> GetInvoices()
        {
            return context.ARInvoices;
        }

        public IQueryable<ARInvoice> GetARInvoiceById(int tenant,string id)
        {
            return context.ARInvoices.Where(i => i.Tenant == tenant && i.Id==id);
        }

        public ARInvoice GetARInvoiceByInvoiceNumber(int tenant, string invoiceNumber)
        {
            return (from a in context.ARInvoices where a.InvoiceNumber == invoiceNumber && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<ARInvoice> GetConnectedInvoices(int tenant, string consolidationInvoiceId)
        {
            IQueryable<ARInvoice> myResult = (from a in context.ARInvoices.Include("BillTo").Include("InvoiceCurrency")
                                              where a.IsConstituentInvoice == true
                                              && a.Tenant == tenant
                                              && a.ConsolidationInvoiceId == consolidationInvoiceId
                                              select a);

            return myResult;
        }

        public ARInvoice GetSingleInvoiceByCancelledById(string id, int tenant)
        {
            return (from a in context.ARInvoices where a.CancelledByARInvoiceId == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public ARInvoice GetSingleInvoice(string id)
        {
            return (from a in context.ARInvoices.Include("BillTo").Include("BillTo.PartnerType").Include("CreatedByUser.Contact").Include("InvoiceCurrency").Include("Status").Include("ARInvoiceType").Include("IssuedByUser.Contact").Include("PrintByUser.Contact").Include("PaymentTerm").Include("ProfitCurrency").Include("LocalCurrency").Include("TransferStatus").Include("ApprovedByUser.Contact").Include("SalesmanUser.Contact").Include("Confirmation")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public ARInvoice GetSingleARInvoice(string id, int tenant)
        {
            return context.ARInvoices
                          .Include("BillTo")
                          .Include("BillTo.PartnerType")
                          .Include("CreatedByUser.Contact")
                          .Include("InvoiceCurrency")
                          .Include("Status")
                          .Include("ARInvoiceType")
                          .Include("IssuedByUser.Contact")
                          .Include("PrintByUser.Contact")
                          .Include("PaymentTerm")
                          .Include("ProfitCurrency")
                          .Include("LocalCurrency")
                          .Include("TransferStatus")
                          .Include("ApprovedByUser.Contact")
                          .Include("SalesmanUser.Contact")
                          .Include("SATInvoiceStatus")
                          .Include("SATTransferStatus")
                          .Include("Branch")
                           .Include("ARInvoicesSignedStatus")
                            .Include("Confirmation")
                          .FirstOrDefault(a => a.Id == id 
                                               && a.Tenant == tenant);
        }

        public IQueryable<ARInvoice> GetARInvoices(int tenant)
        {
            return from a in context.ARInvoices where a.Tenant == tenant select a;
        }

        public IQueryable<ARInvoice> GetIQueryableInvoices(int tenant)
        {
            return from a in context.ARInvoices where a.Tenant == tenant select a;
        }

        public List<ARInvoice> GetInvoicesListFromIdList(List<string> ids, int tenant)
        {
            List<ARInvoice> invoices = new List<ARInvoice>();

            if (ids.Count > 0)
            {
                invoices = (from a in context.ARInvoices.Include("InvoiceCurrency")
                             where a.Tenant == tenant && ids.Contains(a.Id)
                             select a).ToList();
            }

            return invoices;
        }

        public List<ARInvoice> GetInvoicesByShipmentId(string shipmentId, int tenant)
        {
            List<ARInvoice> list = (from a in context.ARInvoiceEntities
                                    where a.Tenant == tenant
                                    && a.EntityId == shipmentId
                                    && (a.ObjectTable.Name == "Shipment" || a.ObjectTable.Name == "Master")
                                    select a.ARInvoice).OrderBy(a => a.Id).ToList();
            return list;
        }

        public List<ARInvoice> GetInvoicesByMainEntityId(string shipmentId, int tenant)
        {
            List<ARInvoice> list = (from a in context.ARInvoices
                                    where a.Tenant == tenant
                                    && a.MainEntityId == shipmentId
                                    select a).ToList();
            return list;
        }

        public List<ARInvoice> GetInvoicesByShipmentIdAndBillToId(string shipmentId, string cardId, int tenant)
        {
            IQueryable<ARInvoice> iQuery = (from a in context.ARInvoiceEntities
                                            where a.Tenant == tenant
                                            && a.EntityId == shipmentId
                                            && (a.ObjectTable.Name == "Shipment" || a.ObjectTable.Name == "Master")
                                            select a.ARInvoice);

            List<ARInvoice> list = iQuery.Where(d => d.BillToId == cardId).ToList();

            return list;
        }

        #region Digital Portal Methods

        public List<ARInvoice> GetDigitalInvoicesByShipmentIdAndBillToId(string shipmentId, string cardId, int tenant)
        {
            List<string> cards = new List<string>();
            if (!string.IsNullOrWhiteSpace(cardId))
            {
                cards = cardId?.Split(',').ToList();
            }

            IQueryable<ARInvoice> iQuery = (from a in context.ARInvoiceEntities
                                            where a.Tenant == tenant
                                            && a.EntityId == shipmentId
                                            && (a.ObjectTable.Name == "Shipment" || a.ObjectTable.Name == "Master")
                                            select a.ARInvoice);
           
            iQuery = FilterInvoicesStatuses(iQuery);
            var cardBillToId = GetCardBillToId(cardId, tenant);
            List<ARInvoice> list = iQuery.Where(d => cardId == null 
                                                     || !cards.Any()
                                                     || cards.Contains(d.BillToId)
                                                     || cardBillToId.Contains(d.BillToId)).ToList();

            return list;
        }


        public IQueryable<ARInvoice> GetDigitalInvoicesByShipmentId(string shipmentId, string cardId, int tenant)
        {
           var iQuery = (from a in context.ARInvoiceEntities
                                    where a.Tenant == tenant
                                    && a.EntityId == shipmentId
                                    && (a.ObjectTable.Name == "Shipment" || a.ObjectTable.Name == "Master")
                                    select a.ARInvoice);
             
            iQuery = FilterInvoicesStatuses(iQuery);
            var cardBillToId = GetCardBillToId(cardId, tenant);
            var list = iQuery.Where(a => cardId.Contains(a.BillToId)
                                         || cardBillToId.Contains(a.BillToId));

            return list;
        }

        private List<string> GetCardBillToId(string cardId, int tenant)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            var cardBillToId =  cardRepository.GetBillToCardById(cardId, tenant);

            if (cardBillToId == null)
            {
                return new List<string>();
            }

            return cardBillToId;
        }

        #endregion Digital Portal Methods

        public IQueryable<ARInvoice> GetUnpaidWithVoidandDraftARInvoices(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && !d.IsAutoCredit && !d.IsCancelled && d.IsClosed == false);
        }

        public IQueryable<ARInvoice> GetVoidARInvoices(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && d.StatusCode == "VD");
        }

        public IQueryable<ARInvoice> GetDraftsARInvoices(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && d.StatusCode == "DR" && d.IsCancelled == false && d.IsClosed == false);
        }

        public IQueryable<ARInvoice> GetAllARInvoicesWithoutVoidAndDraft(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" );
        }

        public IQueryable<ARInvoice> GetUnpaidARInvoices(int tenant)
        {
            return context.ARInvoices.Include("Status").Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && !d.IsAutoCredit && !d.IsCancelled && d.IsClosed == false);
        }

        public IQueryable<ARInvoice> GetNotReadyARInvoices(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && d.TransferStatusCode == "NR");
        }

        public IQueryable<ARInvoice> GetErrorInTransferARInvoices(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && !d.IsCancelled && d.IsClosed == false && d.TransferStatusCode == "ET");
        }

        public IQueryable<ARInvoice> GetBlockedTransferARInvoices(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && !d.IsCancelled && d.IsClosed == false && d.TransferStatusCode == "BL");
        }

        public IQueryable<ARInvoice> GetAccountingLedgerARInvoices(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && !d.IsConstituentInvoice );
        }

        public List<string> GetReadyForTransferORerrorInTransferARInvoices(int tenant)
        {
            List<string> Id = (from a in context.ARInvoices
                               where a.Tenant == tenant && a.TransferStatusCode == "RD" || a.TransferStatusCode == "ET"
                               select a.Id).ToList();
                          

            return Id;
        
        }

        public ARInvoice GetReadyForORerrorInTransferARInvoic(int tenant)
        {
            return context.ARInvoices.Where(d => d.Tenant == tenant && (d.TransferStatusCode == "RD" || d.TransferStatusCode == "ET") && d.TransferTries<5).OrderBy(d=>d.InvoiceDate).FirstOrDefault();
    

        }

        public ARInvoice GetInvoiceByInvoiceNumber(string invoiceNumber)
        {
            return context.ARInvoices.Where(d => d.InvoiceNumber == invoiceNumber).FirstOrDefault();


        }
 
        public IQueryable<AgingReportInvoiceDataView> GetAgingReportInvoiceDataView(int tenant, int index, string customerid)
        {
            IAgingReportInvoiceDataViewContext agingContext = AgingReportInvoiceDataViewContext.GetContext(tenant);

            IQueryable<AgingReportInvoiceDataView> agingReportInvoiceDataViews = null;
            if (string.IsNullOrEmpty(customerid))
            {
                agingReportInvoiceDataViews = from a in agingContext.AgingReportInvoiceDataViews
                                              where a.Tenant == tenant && a.StatusCode != "VD" && a.StatusCode != "PD" && a.StatusCode != "DR" && a.StatusCode != "LL"
                                              select a;
            }
            else
            {
                agingReportInvoiceDataViews = from a in agingContext.AgingReportInvoiceDataViews
                                              where a.Tenant == tenant && a.StatusCode != "VD" && a.StatusCode != "PD" && a.BillToId == customerid && a.StatusCode != "DR" && a.StatusCode != "LL"
                                              select a;
            }


            return agingReportInvoiceDataViews;

        }

        public List<AgingReportDashboardClass> GetAgingReportInvoiceData(int index, IQueryable<AgingReportInvoiceDataView> agingReportInvoiceDataViews)
        {
           

            List<AgingReportDashboardClass> finalResult = (from a in agingReportInvoiceDataViews
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
                currentitem = new AgingReportDashboardClass() { DateRange = "Current", Amount = 0, DueDate = DateTime.Now.Date, IndexOrder = 5, DateRangeLabel = "Current: 0", };
                datalist.Add(currentitem);
            }

            AgingReportDashboardClass firstItem = (from a in datalist
                                                   where a.DateRange == "1-30"
                                                   select a).FirstOrDefault();
            if (firstItem == null)
            {
                firstItem = new AgingReportDashboardClass() { DateRange = "1-30", Amount = 0, DueDate = DateTime.Now.Date.AddDays(15), IndexOrder = 4, DateRangeLabel = "1-30: 0", };
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

        public bool IsInvoiceNumberExists(string invoiceId, string invoiceNumber, int tenent)
        {
            return context.ARInvoices.Where(d => d.Id != invoiceId && d.InvoiceNumber == invoiceNumber && d.Tenant == tenent).Any();
        }

        public bool IsARInvoiceNumberExists(string invoiceNumber, int tenent)
        {
            return context.ARInvoices.Where(d => d.InvoiceNumber == invoiceNumber && d.Tenant == tenent).Any();
        }

        public ARInvoice GetFirstInvoice(int tenant)
        {
            return (from a in context.ARInvoices where a.Tenant == tenant select a).FirstOrDefault();
        }

        public double GetOpenARInvoicesForCustomer(int tenant, string customerid)
        {

            double? openarinvioces = (from a in context.ARInvoices
                                      where a.BillToId == customerid 
                                      && a.Tenant == tenant 
                                      && a.StatusCode != "DR" 
                                      && a.StatusCode != "PD" 
                                      && a.StatusCode != "VD" 
                                      && a.StatusCode != "LL"
                                      && a.IsClosed != true 
                                      && !a.IsAutoCredit 
                                      && !a.IsCancelled
                                      && !a.IsConstituentInvoice
                                      select a.AmountDueInLocalCurrency).Sum();

            //double? autoCredit = (from a in context.ARInvoices
            //                      where a.BillToId == customerid && a.Tenant == tenant && a.StatusCode != "DR" && a.StatusCode != "PD" && a.StatusCode != "VD" && a.IsClosed != true && !a.IsAutoCredit && a.ARInvoiceTypeCode == "CD" && !a.IsCancelled
            //                      select a.AmountDueInLocalCurrency).Sum();


            double? result = openarinvioces;// != null ? openarinvioces : 0;
            //if (autoCredit != null)
            //{
            //    result = openarinvioces - autoCredit;
            //}
            return result != null ? result.Value : 0;
        }
      
        public List<ARInvoice> GetARInvoicesByIds(int tenant, List<string> invoiceIds)
        {
           return (from a in invoiceContext.ARInvoices.Include("BillTo")
                                                 where a.Tenant == tenant && invoiceIds.Contains(a.Id)
                                                 select a).ToList();
        }
        public void Add(ARInvoice entity)
        {
            context.ARInvoices.Add(entity);
        }

        public void Remove(ARInvoice entity)
        {
            context.ARInvoices.Attach(entity);
            context.ARInvoices.Remove(entity);
        }

        public void Update(ARInvoice entity)
        {
            try
            {
                context.ARInvoices.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ARInvoice> All()
        {
            return context.ARInvoices.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARInvoice> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARInvoice GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<ARInvoice> GetInvoicesListFromIdListByDate(List<string> ids, DateTime taxReportMonth, int tenant)
        {
            List<ARInvoice> invoices = new List<ARInvoice>();

            if (ids.Count > 0)
            {
                invoices = (from a in context.ARInvoices 
                            where a.Tenant == tenant && ids.Contains(a.Id) && a.InvoiceDate <= taxReportMonth
                            select a).ToList();
            }

            return invoices;
        }
        public IQueryable<ARInvoice> GetUnpaidAndDraftARInvoices(int tenant)
        {
            return context.ARInvoices.Include("Status").Include("PaymentTerm").Where(d => d.Tenant == tenant && d.StatusCode != "VD" && d.StatusCode != "LL" && !d.IsAutoCredit && !d.IsCancelled && d.IsClosed == false);
        }

        public IQueryable<ARInvoice> FilterInvoicesStatuses(IQueryable<ARInvoice> invoices)
        {
            var blockedStatusCode = new List<string> 
            {
                "VD",
                "DR",
                "LL",
                "AR",
                "NT"
            };

            var filteredInvoices = invoices.Where(d => !blockedStatusCode.Contains(d.StatusCode));

            return filteredInvoices;
        }


        public IQueryable<ARInvoice> FilterInvoicesStatusesForList(IQueryable<ARInvoice> invoices)
        {
            var blockedStatusCode = new List<string>
            {
                "VD",
                "DR",
                "LL",
                "AR",
                "NT",
                "CN"
            };

            return invoices.Where(d => !blockedStatusCode.Contains(d.StatusCode));
        }

        public IQueryable<DigitalInvoicesCounterDataView> GetDigitalInvoicesCounterDataView(int tenant)
        {
            return context.DigitalInvoicesCounterDataView
                   .Where(a => a.Tenant == tenant)
                   .Select(a => a);
        }

        public IQueryable<ControlForInvoiceLinesDataView> GetControlForInvoiceLinesDataView(int tenant)
        {
            return context.ControlForInvoiceLinesDataView
                   .Where(a => a.Tenant == tenant)
                   .Select(a => a);
        }

    }
}
