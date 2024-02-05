using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.CommonDataModel;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using Logitude.BL.InvoiceModel.CustomFilters;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceQuery
    {
        public ARInvoiceRepository repository;
        private Tenant tenantPoco;
        private const string InterestReportInvoiceTypeCode = "IT";
        public ARInvoiceQuery()
        {
            repository = new ARInvoiceRepository();
        }

        public ARInvoiceQuery(int tenant)
        {
            repository = new ARInvoiceRepository(tenant);
        }

        public ARInvoiceQuery(ARInvoiceRepository arInvoiceRepository)
        {
            repository = arInvoiceRepository;
        }

        public ARInvoicePM GetSinglePM(string id, int tenant)
        {
            ARInvoicePM entityPM = null;

            ARInvoice entityPOCO = repository.context
                                             .ARInvoices
                                             .Include("Status")
                                             .Include("ARInvoiceType")
                                             .Include("ProfitCurrency")
                                             .Include("InvoiceCurrency")
                                             .Include("Status")
                                             .Include("LocalCurrency")
                                             .Include("BillTo")
                                             .Include("TransferStatus")
                                             .Include("ApprovedByUser")
                                             .Include("ApprovedByUser.Contact")
                                             .Include("SalesmanUser")
                                             .Include("SalesmanUser.Contact")
                                             .Include("SATInvoiceStatus")
                                             .Include("SATTransferStatus")
                                             .Include("Branch")
                                             .Include("ARInvoicesSignedStatus")
                                             .FirstOrDefault(a => a.Id == id && a.Tenant == tenant);

            if (entityPOCO != null)
            {
                entityPM = GetSingleMappedEntityPM(entityPOCO, true);
            }

            return entityPM;
        }

        public ARInvoicePM GetSinglePMForInterest(string id, int tenant)
        {
            ARInvoicePM entityPM =
               (from a in repository.context.ARInvoices
                where a.Id == id && a.Tenant == tenant
                select new ARInvoicePM()
                {
                    Id = a.Id,
                    InvoiceNumber = a.InvoiceNumber,
                    Tenant = a.Tenant,
                }).FirstOrDefault();
            if (entityPM != null)
            {
                entityPM = SetJournalFields(entityPM);

            }

            return entityPM;
        }

        private ARInvoicePM SetJournalFields(ARInvoicePM entityPM)
        {
            JournalRepository rep = new JournalRepository(entityPM.Tenant);
            JournalEntity journal = rep.GetJournalByARInvoiceEntity(entityPM.Id, entityPM.Tenant);
            if (journal != null)
            {
                entityPM.JournalId = journal.JournalId;
                entityPM.JournalNumber = journal.JournalNumber;
            }

            return entityPM;
        }


        public ARInvoice GetSingleARInvoice(string id, int tenant)
        {

            ARInvoice entityPOCO =
                (from a in repository.context.ARInvoices.Include("ProfitCurrency").Include("InvoiceCurrency").Include("Status").Include("LocalCurrency").Include("BillTo").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("SATInvoiceStatus").Include("SATTransferStatus").Include("Branch")
                 where a.Id == id && a.Tenant == tenant
                 select a).FirstOrDefault();


            return entityPOCO;
        }
        public bool CheckARInvoiceByExternalAccountingEnityId(string externalEntityId, int tenant)
        {

            return
                  (from a in repository.context.ARInvoices
                   where a.ExternalAccountingEntityId == externalEntityId && a.Tenant == tenant
                   select a).Any();



        }
        public ARInvoicePM GetSingleInvoiceByInvoiceNumber(string invoiceNumber, int tenant)
        {
            ARInvoicePM entityPM = null;

            ARInvoice entityPOCO =
                (from a in repository.context.ARInvoices.Include("ProfitCurrency").Include("InvoiceCurrency").Include("Status").Include("LocalCurrency").Include("BillTo").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("Branch")
                 where a.InvoiceNumber == invoiceNumber && a.Tenant == tenant
                 select a).FirstOrDefault();

            if (entityPOCO != null)
            {
                entityPM = this.GetSingleMappedEntityPM(entityPOCO, true);
            }

            return entityPM;
        }

        public string GetSingleInvoiceIdByInvoiceNumber(string invoiceNumber, int tenant)
        {
            ARInvoice entityPOCO =
                (from a in repository.context.ARInvoices
                 where a.InvoiceNumber == invoiceNumber && a.Tenant == tenant
                 select a).FirstOrDefault();
            if (entityPOCO == null)
            {
                return null;
            }
            else
            {
                return entityPOCO.Id;
            }
        }


        public string GetCheckInvoiceId(string invoiceId, int tenant)
        {
            ARInvoice entityPOCO =
                (from a in repository.context.ARInvoices
                 where a.Id == invoiceId && a.Tenant == tenant
                 select a).FirstOrDefault();
            if (entityPOCO == null)
            {
                return null;
            }
            else
            {
                return entityPOCO.Id;
            }
        }


        public ARInvoicePM GetReadyForTransferOrErrorInTransferInvoicePM(int tenant)
        {
            ARInvoicePM entityPM = null;
            ARInvoice entityPOCO = null;

            IQueryable<ARInvoice> iQueryable_Data =
                (from a in repository.context.ARInvoices.Include("ProfitCurrency").Include("InvoiceCurrency").Include("Status").Include("LocalCurrency").Include("BillTo").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("Branch")
                 where a.Tenant == tenant
                 && a.TransferTries < 5
                 && a.StatusCode != "DR"
                 && a.StatusCode != "VD"
                 select a);

            entityPOCO = (from d in iQueryable_Data where d.TransferStatusCode == "RD" select d).FirstOrDefault();

            if (entityPOCO == null)
            {
                entityPOCO = (from d in iQueryable_Data where d.TransferStatusCode == "ET" select d).FirstOrDefault();
            }

            if (entityPOCO != null)
            {
                entityPM = this.GetSingleMappedEntityPM(entityPOCO, true);
            }

            return entityPM;
        }

        public List<MoneyStatusClass> GetMoneyStatusForTenant(string type, int lastMonths, int lastDays, int tenant, int selectedIndex, int currencyindex)
        {
            int months = 0;
            DateTime lastDate;
            int days;

            days = lastDays + 1;
            lastDate = DateTime.Today.Date.AddDays(days);

            if (lastMonths != 0)
            {
                months = lastMonths + 1;
                lastDate = DateTime.Today.Date.AddMonths(months);
            }

            IQueryable<ARInvoice> invoices = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), repository.context.ARInvoices.Where(t => t.Tenant == tenant), tenant);
            IQueryable<ARPayment> payments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARPayment>(new QueryOperations(), repository.context.ARPayments.Where(t => t.Tenant == tenant), tenant);

            List<MoneyStatusClass> datalistInvoice = (from a in invoices
                                                      where a.StatusCode != "DR" && a.StatusCode != "VD" && a.StatusCode != "LL" && a.InvoiceDate >= lastDate && a.Tenant == tenant //&& !a.IsAutoCredit && !a.IsClosed && !a.IsCancelled
                                                     && !a.IsConstituentInvoice
                                                      group a by new
                                                      {
                                                          a.InvoiceDate.Value.Day,
                                                          a.InvoiceDate.Value.Month,
                                                          a.InvoiceDate.Value.Year,
                                                      } into inv
                                                      orderby inv.Key.Day, inv.Key.Month, inv.Key.Year
                                                      select new MoneyStatusClass()
                                                      {
                                                          day = inv.Key.Day,
                                                          month = inv.Key.Month,
                                                          year = inv.Key.Year,
                                                          TotalAmount = currencyindex == 1 ? inv.Sum(d => d.AmountInLocalCurrency) : inv.Sum(d => d.AmountInProfitCurrency),
                                                          DataType = "Invoices",
                                                      }
                                                 ).ToList();

            List<MoneyStatusClass> datalistPayment = (from a in payments
                                                      where a.RegisterDate >= lastDate && a.Tenant == tenant && a.StatusCode != "VD" && a.StatusCode != "DR" //&& !a.IsClosed
                                                      group a by new
                                                      {
                                                          a.RegisterDate.Value.Day,
                                                          a.RegisterDate.Value.Month,
                                                          a.RegisterDate.Value.Year,
                                                      } into inv
                                                      orderby inv.Key.Day, inv.Key.Month, inv.Key.Year
                                                      select new MoneyStatusClass()
                                                      {
                                                          day = inv.Key.Day,
                                                          month = inv.Key.Month,
                                                          year = inv.Key.Year,
                                                          TotalAmount = currencyindex == 1 ? inv.Sum(d => d.AmountInLocalCurrency) : inv.Sum(d => d.AmountInProfitCurrency),
                                                          DataType = "Payments",
                                                      }
                                                ).ToList();

            List<MoneyStatusClass> datalist = datalistInvoice.Concat(datalistPayment).ToList();

            List<MoneyStatusClass> datalist2 = null;
            switch (selectedIndex)
            {
                case 1:
                case 2:
                    {
                        #region by week
                        foreach (MoneyStatusClass m in datalist)
                        {
                            DateTime todayDate = new DateTime(m.year, m.month, m.day);

                            int day = Convert.ToInt32(todayDate.DayOfWeek);
                            DateTime startOfWeek = todayDate.AddDays((-1 * day));
                            DateTime endOfWeek = todayDate.AddDays((6 - day));

                            m.StartDate = startOfWeek;
                            m.EndDate = endOfWeek;
                            m.DateRange = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;
                        }

                        if (selectedIndex == 1)
                        {
                            datalist = FillEmptyDates(datalist, -1);
                        }
                        else
                        {
                            datalist = FillEmptyDates(datalist, -3);
                        }

                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         a.StartDate,
                                         a.EndDate,
                                         a.DateRange,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.StartDate, inv.Key.EndDate
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = inv.Key.DateRange,
                                         StartDate = inv.Key.StartDate,
                                         EndDate = inv.Key.EndDate,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        break;
                        #endregion
                    }
                case 0:
                    {
                        #region by 7 days
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         a.day,
                                         a.month,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.month, inv.Key.day
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = inv.Key.day.ToString() + "/" + inv.Key.month.ToString(),
                                         day = inv.Key.day,
                                         month = inv.Key.month,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 7)
                        {
                            datalist2 = FillEmptyDates(datalist2, -6);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.month, a.day
                                         select a).ToList();
                        }
                        break;

                        #endregion

                        #region by 6 months
                        //datalist2 = (from a in datalist
                        //             group a by new
                        //             {

                        //                 a.month,
                        //                 a.year,
                        //                 a.DataType,
                        //             } into inv
                        //             orderby inv.Key.year, inv.Key.month
                        //             select new MoneyStatusClass()
                        //             {
                        //                 DateRange = inv.Key.month.ToString() + "/" + inv.Key.year.ToString(),

                        //                 month = inv.Key.month,
                        //                 year = inv.Key.year,
                        //                 TotalAmount = inv.Sum(d => d.TotalAmount),
                        //                 DataType = inv.Key.DataType,
                        //             }).ToList();
                        //if (datalist2.Count < 6)
                        //{
                        //    datalist2 = FillEmptyDates(datalist2, -5);
                        //    datalist2 = (from a in datalist2
                        //                 orderby a.year, a.month
                        //                 select a).ToList();
                        //}
                        //break;
                        #endregion
                    }
                case 3:
                    {
                        #region by 12 months
                        datalist2 = (from a in datalist
                                     group a by new
                                     {

                                         a.month,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.month
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = inv.Key.month.ToString() + "/" + inv.Key.year.ToString(),

                                         month = inv.Key.month,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 12)
                        {
                            datalist2 = FillEmptyDates(datalist2, -12);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.month
                                         select a).ToList();
                        }
                        break;
                        #endregion
                    }
                case 5:
                    {
                        #region year by quarter
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         Quarter = ((a.month - 1) / 3) + 1,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.Quarter
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = "Q" + inv.Key.Quarter.ToString() + "." + inv.Key.year.ToString().Substring(2, 2),
                                         Quarter = inv.Key.Quarter,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 4)
                        {
                            datalist2 = getFilledListByQuarters(datalist2, -11);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.Quarter
                                         select a).ToList();
                        }

                        break;
                        #endregion
                    }
                case 4:
                    {
                        #region 3 years by querter
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         Quarter = ((a.month - 1) / 3) + 1,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.Quarter
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = "Q" + inv.Key.Quarter.ToString() + "." + inv.Key.year.ToString().Substring(2, 2),
                                         Quarter = inv.Key.Quarter,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 12)
                        {
                            datalist2 = getFilledListByQuarters(datalist2, -35);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.Quarter
                                         select a).ToList();
                        }
                        break;
                        #endregion
                    }
            }

            foreach (MoneyStatusClass m in datalist2)
            {
                m.TotalAmountLabel = string.Format((string)"{0:#,0.00}", (object)m.TotalAmount);
            }

            return datalist2;
        }

        public List<MoneyStatusClass> GetMoneyStatusForTenantCustom(string type, DateTime? ToDate, DateTime? FromDate, int tenant)
        {
            bool AddYearFlag = false;
            if (FromDate.Value.Year != ToDate.Value.Year)
                AddYearFlag = true;

            var currencyindex = 1;

            IQueryable<ARInvoice> invoices = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), repository.context.ARInvoices.Where(t => t.Tenant == tenant), tenant);
            IQueryable<ARPayment> payments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARPayment>(new QueryOperations(), repository.context.ARPayments.Where(t => t.Tenant == tenant), tenant);

            List<MoneyStatusClass> datalistInvoice = (from a in invoices
                                                      where a.StatusCode != "DR" && a.StatusCode != "VD" && a.StatusCode != "LL" && a.InvoiceDate >= FromDate && a.Tenant == tenant //&& !a.IsAutoCredit && !a.IsClosed && !a.IsCancelled
                                                      group a by new
                                                      {
                                                          a.InvoiceDate.Value.Day,
                                                          a.InvoiceDate.Value.Month,
                                                          a.InvoiceDate.Value.Year,
                                                          a.InvoiceDate,
                                                      } into inv
                                                      orderby inv.Key.Day, inv.Key.Month, inv.Key.Year
                                                      select new MoneyStatusClass()
                                                      {
                                                          day = inv.Key.Day,
                                                          month = inv.Key.Month,
                                                          year = inv.Key.Year,
                                                          TotalAmount = currencyindex == 1 ? inv.Sum(d => d.AmountInLocalCurrency) : inv.Sum(d => d.AmountInProfitCurrency),
                                                          DataType = "Invoices",
                                                          FullDate = inv.Key.InvoiceDate,

                                                      }
                                                 ).ToList();

            List<MoneyStatusClass> datalistPayment = (from a in payments
                                                      where a.RegisterDate >= FromDate && a.Tenant == tenant && a.StatusCode != "VD" && a.StatusCode != "DR" //&& !a.IsClosed
                                                      group a by new
                                                      {
                                                          a.RegisterDate.Value.Day,
                                                          a.RegisterDate.Value.Month,
                                                          a.RegisterDate.Value.Year,
                                                          a.RegisterDate
                                                      } into inv
                                                      orderby inv.Key.Day, inv.Key.Month, inv.Key.Year
                                                      select new MoneyStatusClass()
                                                      {
                                                          day = inv.Key.Day,
                                                          month = inv.Key.Month,
                                                          year = inv.Key.Year,
                                                          TotalAmount = currencyindex == 1 ? inv.Sum(d => d.AmountInLocalCurrency) : inv.Sum(d => d.AmountInProfitCurrency),
                                                          DataType = "Payments",
                                                          FullDate = inv.Key.RegisterDate,

                                                      }
                                                ).ToList();

            List<MoneyStatusClass> datalist = datalistInvoice.Concat(datalistPayment).ToList();

            List<MoneyStatusClass> datalist2 = null;

            int lastDays = (FromDate.Value - ToDate.Value).Days;
            lastDays = lastDays *= -1;
            int Perdio = lastDays / 6;

            var dates = new List<DateTime?>();
            Dictionary<string, MoneyStatusClass> Listt = new Dictionary<string, MoneyStatusClass>();

            var iteration = 0;
            for (DateTime? dt = FromDate; dt <= ToDate; dt = dt.Value.AddDays(Perdio))
            {
                dates.Add(dt);
                string DatePeriod = dt.Value.Day + "/" + dt.Value.Month + (AddYearFlag == true ? "/" + dt.Value.Year + "" : "");

                if (iteration != 0)
                {
                    var oldDate = dates[iteration - 1].Value.Day + "/" + dates[iteration - 1].Value.Month + (AddYearFlag == true ? "/" + dates[iteration - 1].Value.Year + "" : ""); ;

                    string newDate = oldDate + "-" + DatePeriod;
                    var Entity1 = new MoneyStatusClass();
                    Entity1.linePrimary = 0;
                    Entity1.TotalAmount = 0;
                    Entity1.FullDate = dt;
                    Entity1.DateRange = newDate;
                    Entity1.DataType = "Invoices";
                    Listt.Add(newDate + "I", Entity1);
                    var Entity2 = new MoneyStatusClass();
                    Entity2.linePrimary = 0;
                    Entity2.TotalAmount = 0;
                    Entity2.FullDate = dt;
                    Entity2.DateRange = newDate;
                    Entity2.DataType = "Payments";
                    Listt.Add(newDate + "P", Entity2);

                }

                iteration++;
            }
            dates.Sort();

            datalist2 = (from a in datalist
                         group a by new
                         {
                             a.FullDate,
                             a.DateRange,
                             a.DataType,
                         } into inv
                         orderby inv.Key.FullDate
                         select new MoneyStatusClass()
                         {
                             DateRange = inv.Key.DateRange,
                             FullDate = inv.Key.FullDate,
                             TotalAmount = inv.Sum(d => d.TotalAmount),
                             DataType = inv.Key.DataType,
                         }).ToList();

            foreach (MoneyStatusClass d in datalist2)
            {
                string DatePeriod = "";
                int count = dates.Count;

                if (count >= 1 ? d.FullDate <= dates[0] : false)
                {
                    DatePeriod = dates[0].Value.Day + "/" + dates[0].Value.Month + (AddYearFlag == true ? "/" + dates[0].Value.Year + "" : "") + "-" + dates[1].Value.Day + "/" + dates[1].Value.Month + (AddYearFlag == true ? "/" + dates[1].Value.Year + "" : "");
                    var NewDatePeriod = DatePeriod;
                    if (d.DataType == "Invoices")
                        NewDatePeriod = DatePeriod + "I";
                    else
                        NewDatePeriod = DatePeriod + "P";

                    if (!Listt.ContainsKey(NewDatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[NewDatePeriod].TotalAmount += d.TotalAmount;
                        Listt[NewDatePeriod].DataType = d.DataType;
                    }
                }

                else if (count >= 2 ? d.FullDate <= dates[1] : false)
                {
                    DatePeriod = dates[1].Value.Day + "/" + dates[1].Value.Month + (AddYearFlag == true ? "/" + dates[1].Value.Year + "" : "") + "-" + dates[2].Value.Day + "/" + dates[2].Value.Month + (AddYearFlag == true ? "/" + dates[2].Value.Year + "" : "");
                    var NewDatePeriod = DatePeriod;
                    if (d.DataType == "Invoices")
                        NewDatePeriod = DatePeriod + "I";
                    else
                        NewDatePeriod = DatePeriod + "P";

                    if (!Listt.ContainsKey(NewDatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[NewDatePeriod].TotalAmount += d.TotalAmount;
                        Listt[NewDatePeriod].DataType = d.DataType;
                    }
                }



                else if (count >= 3 ? d.FullDate <= dates[2] : false)
                {
                    DatePeriod = dates[2].Value.Day + "/" + dates[2].Value.Month + (AddYearFlag == true ? "/" + dates[2].Value.Year + "" : "") + "-" + dates[3].Value.Day + "/" + dates[3].Value.Month + (AddYearFlag == true ? "/" + dates[3].Value.Year + "" : "");
                    var NewDatePeriod = DatePeriod;
                    if (d.DataType == "Invoices")
                        NewDatePeriod = DatePeriod + "I";
                    else
                        NewDatePeriod = DatePeriod + "P";

                    if (!Listt.ContainsKey(NewDatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[NewDatePeriod].TotalAmount += d.TotalAmount;
                        Listt[NewDatePeriod].DataType = d.DataType;
                    }
                }


                else if (count >= 4 ? d.FullDate <= dates[3] : false)
                {
                    DatePeriod = dates[3].Value.Day + "/" + dates[3].Value.Month + (AddYearFlag == true ? "/" + dates[3].Value.Year + "" : "") + "-" + dates[4].Value.Day + "/" + dates[4].Value.Month + (AddYearFlag == true ? "/" + dates[4].Value.Year + "" : "");
                    var NewDatePeriod = DatePeriod;
                    if (d.DataType == "Invoices")
                        NewDatePeriod = DatePeriod + "I";
                    else
                        NewDatePeriod = DatePeriod + "P";

                    if (!Listt.ContainsKey(NewDatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[NewDatePeriod].TotalAmount += d.TotalAmount;
                        Listt[NewDatePeriod].DataType = d.DataType;
                    }
                }


                else if (count >= 5 ? d.FullDate <= dates[4] : false)
                {
                    DatePeriod = dates[4].Value.Day + "/" + dates[4].Value.Month + (AddYearFlag == true ? "/" + dates[4].Value.Year + "" : "") + "-" + dates[5].Value.Day + "/" + dates[5].Value.Month + (AddYearFlag == true ? "/" + dates[5].Value.Year + "" : "");
                    var NewDatePeriod = DatePeriod;
                    if (d.DataType == "Invoices")
                        NewDatePeriod = DatePeriod + "I";
                    else
                        NewDatePeriod = DatePeriod + "P";

                    if (!Listt.ContainsKey(NewDatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[NewDatePeriod].TotalAmount += d.TotalAmount;
                        Listt[NewDatePeriod].DataType = d.DataType;
                    }
                }


                else if (count >= 6 ? d.FullDate <= dates[5] : false)
                {
                    DatePeriod = dates[5].Value.Day + "/" + dates[5].Value.Month + (AddYearFlag == true ? "/" + dates[5].Value.Year + "" : "") + "-" + dates[6].Value.Day + "/" + dates[6].Value.Month + (AddYearFlag == true ? "/" + dates[6].Value.Year + "" : "");
                    var NewDatePeriod = DatePeriod;
                    if (d.DataType == "Invoices")
                        NewDatePeriod = DatePeriod + "I";
                    else
                        NewDatePeriod = DatePeriod + "P";

                    if (!Listt.ContainsKey(NewDatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[NewDatePeriod].TotalAmount += d.TotalAmount;
                        Listt[NewDatePeriod].DataType = d.DataType;
                    }
                }

                else if (count >= 7 ? d.FullDate <= dates[6] : false)
                {
                    DatePeriod = dates[6].Value.Day + "/" + dates[6].Value.Month + (AddYearFlag == true ? "/" + dates[6].Value.Year + "" : "") + "-" + dates[7].Value.Day + "/" + dates[7].Value.Month + (AddYearFlag == true ? "/" + dates[7].Value.Year + "" : "");
                    var NewDatePeriod = DatePeriod;
                    if (d.DataType == "Invoices")
                        NewDatePeriod = DatePeriod + "I";
                    else
                        NewDatePeriod = DatePeriod + "P";

                    if (!Listt.ContainsKey(NewDatePeriod))
                    {
                        Listt.Add(DatePeriod, d);
                    }
                    else
                    {
                        Listt[NewDatePeriod].TotalAmount += d.TotalAmount;
                        Listt[NewDatePeriod].DataType = d.DataType;
                    }
                }
            }
            var resulList = new List<MoneyStatusClass>();

            for (int i = 0; i < Listt.Count; i++)
            {
                Listt.Values.ElementAt(i).DateRange = Listt.Keys.ElementAt(i).Remove(Listt.Keys.ElementAt(i).Length - 1);
                resulList.Add(Listt.Values.ElementAt(i));
            }
            return resulList;

        }

        public List<MoneyStatusClass> FillEmptyDates(List<MoneyStatusClass> datalist, int months)
        {
            if (months == -1 || months == -3)
            {
                #region Fill the empty dates
                DateTime startDate = DateTime.Today.Date.AddMonths(months);

                while (startDate <= DateTime.Today.Date)
                {
                    DateTime date = startDate;
                    var monthShipments = from s in datalist
                                         where s.month == date.Month && s.year == date.Year && s.day == date.Day
                                         select s;
                    if (monthShipments.Count() == 0)
                    {
                        MoneyStatusClass newEntry = new MoneyStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "Invoices",
                            TotalAmount = 0,
                        };
                        MoneyStatusClass newEntry2 = new MoneyStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "Payments",
                            TotalAmount = 0,
                        };

                        int day = Convert.ToInt32(startDate.DayOfWeek);
                        DateTime startOfWeek = startDate.AddDays((-1 * day));
                        DateTime endOfWeek = startDate.AddDays((6 - day));

                        newEntry.StartDate = startOfWeek;
                        newEntry.EndDate = endOfWeek;
                        newEntry.DateRange = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;

                        newEntry2.StartDate = startOfWeek;
                        newEntry2.EndDate = endOfWeek;
                        newEntry2.DateRange = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;

                        datalist.Add(newEntry);
                        datalist.Add(newEntry2);
                    }

                    startDate = startDate.AddDays(1);
                }
                #endregion
            }
            else if (months == -6)
            {

                DateTime startDate = DateTime.Today.Date.AddDays(months);

                while (startDate <= DateTime.Today.Date)
                {
                    DateTime date = startDate;
                    var monthShipments = from s in datalist
                                         where s.month == date.Month && s.year == date.Year && s.day == date.Day
                                         select s;
                    if (monthShipments.Count() == 0)
                    {
                        MoneyStatusClass newEntry = new MoneyStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "Invoices",
                            DateRange = startDate.Day.ToString() + "/" + startDate.Month,
                            TotalAmount = 0,
                        };
                        MoneyStatusClass newEntry2 = new MoneyStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "Payments",
                            DateRange = startDate.Day.ToString() + "/" + startDate.Month,
                            TotalAmount = 0,
                        };

                        datalist.Add(newEntry);
                        datalist.Add(newEntry2);
                    }

                    startDate = startDate.AddDays(1);
                }
            }

            else
            {
                DateTime startDate = DateTime.Today.Date.AddMonths(months);

                while (startDate <= DateTime.Today.Date)
                {
                    DateTime date = startDate;
                    var monthShipments = from s in datalist
                                         where s.month == date.Month && s.year == date.Year && s.day == date.Day
                                         select s;
                    if (monthShipments.Count() == 0)
                    {
                        MoneyStatusClass newEntry = new MoneyStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "Invoices",
                            DateRange = startDate.Month.ToString() + "/" + startDate.Year,
                            TotalAmount = 0,
                        };
                        MoneyStatusClass newEntry2 = new MoneyStatusClass()
                        {
                            day = startDate.Day,
                            month = startDate.Month,
                            year = startDate.Year,
                            DataType = "Payments",
                            DateRange = startDate.Month.ToString() + "/" + startDate.Year,
                            TotalAmount = 0,
                        };

                        datalist.Add(newEntry);
                        datalist.Add(newEntry2);
                    }

                    startDate = startDate.AddMonths(1);
                }
            }
            return datalist;
        }

        public List<MoneyStatusClass> GetMoneyOutStatusForTenant(int lastMonths, int lastDays, int tenant, int selectedIndex, int currencyindex)
        {
            int months = 0;
            DateTime lastDate;
            int days;
            DateTime fiXedDateDays;

            days = lastDays + 1;
            lastDate = DateTime.Today.Date.AddDays(days);

            if (lastMonths != 0)
            {
                months = lastMonths + 1;
                lastDate = DateTime.Today.Date.AddMonths(months);
            }

            IQueryable<APInvoice> invoices = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoice>(new QueryOperations(), repository.context.APInvoices.Where(t => t.Tenant == tenant), tenant);
            IQueryable<APPayment> payments = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APPayment>(new QueryOperations(), repository.context.APPayments.Where(t => t.Tenant == tenant), tenant);

            List<MoneyStatusClass> datalistInvoice = (from a in invoices
                                                      where a.StatusCode != "LL" && a.StatusCode != "VD" && a.InvoiceDate >= lastDate && a.Tenant == tenant
                                                      group a by new
                                                      {
                                                          a.InvoiceDate.Value.Day,
                                                          a.InvoiceDate.Value.Month,
                                                          a.InvoiceDate.Value.Year,
                                                      } into inv
                                                      orderby inv.Key.Day, inv.Key.Month, inv.Key.Year
                                                      select new MoneyStatusClass()
                                                      {
                                                          day = inv.Key.Day,
                                                          month = inv.Key.Month,
                                                          year = inv.Key.Year,
                                                          TotalAmount = currencyindex == 1 ? inv.Sum(d => d.AmountInLocalCurrency) : inv.Sum(d => d.AmountInProfitCurrency),
                                                          DataType = "Invoices",
                                                      }
                                                 ).ToList();

            List<MoneyStatusClass> datalistPayment = (from a in payments
                                                      where a.RegisterDate >= lastDate && a.Tenant == tenant && a.StatusCode != "VD"
                                                      group a by new
                                                      {
                                                          a.RegisterDate.Value.Day,
                                                          a.RegisterDate.Value.Month,
                                                          a.RegisterDate.Value.Year,
                                                      } into inv
                                                      orderby inv.Key.Day, inv.Key.Month, inv.Key.Year
                                                      select new MoneyStatusClass()
                                                      {
                                                          day = inv.Key.Day,
                                                          month = inv.Key.Month,
                                                          year = inv.Key.Year,
                                                          TotalAmount = currencyindex == 1 ? inv.Sum(d => d.AmountInLocalCurrency) : inv.Sum(d => d.AmountInProfitCurrency),
                                                          DataType = "Payments",
                                                      }
                                                ).ToList();

            List<MoneyStatusClass> datalist = datalistInvoice.Concat(datalistPayment).ToList();

            List<MoneyStatusClass> datalist2 = null;
            switch (selectedIndex)
            {
                case 1:
                case 2:
                    {
                        #region by week
                        foreach (MoneyStatusClass m in datalist)
                        {
                            DateTime todayDate = new DateTime(m.year, m.month, m.day);

                            int day = Convert.ToInt32(todayDate.DayOfWeek);
                            DateTime startOfWeek = todayDate.AddDays((-1 * day));
                            DateTime endOfWeek = todayDate.AddDays((6 - day));

                            m.StartDate = startOfWeek;
                            m.EndDate = endOfWeek;
                            m.DateRange = startOfWeek.Day + "/" + startOfWeek.Month + "-" + endOfWeek.Day + "/" + endOfWeek.Month;
                        }

                        if (selectedIndex == 1)
                        {
                            datalist = FillEmptyDates(datalist, -1);
                        }
                        else
                        {
                            datalist = FillEmptyDates(datalist, -3);
                        }

                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         a.StartDate,
                                         a.EndDate,
                                         a.DateRange,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.StartDate, inv.Key.EndDate
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = inv.Key.DateRange,
                                         StartDate = inv.Key.StartDate,
                                         EndDate = inv.Key.EndDate,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        break;
                        #endregion
                    }
                case 0:
                    {
                        #region by 7 days
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         a.day,
                                         a.month,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.month, inv.Key.day
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = inv.Key.day.ToString() + "/" + inv.Key.month.ToString(),
                                         day = inv.Key.day,
                                         month = inv.Key.month,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 7)
                        {
                            datalist2 = FillEmptyDates(datalist2, -6);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.month, a.day
                                         select a).ToList();
                        }
                        break;

                        #endregion

                        #region by 6 months
                        //datalist2 = (from a in datalist
                        //             group a by new
                        //             {

                        //                 a.month,
                        //                 a.year,
                        //                 a.DataType,
                        //             } into inv
                        //             orderby inv.Key.year, inv.Key.month
                        //             select new MoneyStatusClass()
                        //             {
                        //                 DateRange = inv.Key.month.ToString() + "/" + inv.Key.year.ToString(),

                        //                 month = inv.Key.month,
                        //                 year = inv.Key.year,
                        //                 TotalAmount = inv.Sum(d => d.TotalAmount),
                        //                 DataType = inv.Key.DataType,
                        //             }).ToList();
                        //if (datalist2.Count < 6)
                        //{
                        //    datalist2 = FillEmptyDates(datalist2, -5);
                        //    datalist2 = (from a in datalist2
                        //                 orderby a.year, a.month
                        //                 select a).ToList();
                        //}
                        //break;
                        #endregion
                    }
                case 3:
                    {
                        #region by 12 months
                        datalist2 = (from a in datalist
                                     group a by new
                                     {

                                         a.month,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.month
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = inv.Key.month.ToString() + "/" + inv.Key.year.ToString(),

                                         month = inv.Key.month,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 12)
                        {
                            datalist2 = FillEmptyDates(datalist2, -12);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.month
                                         select a).ToList();
                        }
                        break;
                        #endregion
                    }
                case 5:
                    {
                        #region year by quarter
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         Quarter = ((a.month - 1) / 3) + 1,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.Quarter
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = "Q" + inv.Key.Quarter.ToString() + "." + inv.Key.year.ToString().Substring(2, 2),
                                         Quarter = inv.Key.Quarter,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 4)
                        {
                            datalist2 = getFilledListByQuarters(datalist2, -11);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.Quarter
                                         select a).ToList();
                        }

                        break;
                        #endregion
                    }
                case 4:
                    {
                        #region 3 years by querter
                        datalist2 = (from a in datalist
                                     group a by new
                                     {
                                         Quarter = ((a.month - 1) / 3) + 1,
                                         a.year,
                                         a.DataType,
                                     } into inv
                                     orderby inv.Key.year, inv.Key.Quarter
                                     select new MoneyStatusClass()
                                     {
                                         DateRange = "Q" + inv.Key.Quarter.ToString() + "." + inv.Key.year.ToString().Substring(2, 2),
                                         Quarter = inv.Key.Quarter,
                                         year = inv.Key.year,
                                         TotalAmount = inv.Sum(d => d.TotalAmount),
                                         DataType = inv.Key.DataType,
                                     }).ToList();
                        if (datalist2.Count < 12)
                        {
                            datalist2 = getFilledListByQuarters(datalist2, -35);
                            datalist2 = (from a in datalist2
                                         orderby a.year, a.Quarter
                                         select a).ToList();
                        }
                        break;
                        #endregion
                    }
            }

            foreach (MoneyStatusClass m in datalist2)
            {
                m.TotalAmountLabel = string.Format((string)"{0:#,0.00}", (object)m.TotalAmount);
            }

            return datalist2;
        }

        private List<MoneyStatusClass> getFilledListByQuarters(List<MoneyStatusClass> list, int months)
        {
            DateTime startDate = DateTime.Today.Date.AddMonths(months);
            int startYear = startDate.Year;
            int startQuarter = Convert.ToInt32(((startDate.Month - 1) / 3) + 1);
            int endQuarter = Convert.ToInt32(((DateTime.Today.Date.Month - 1) / 3) + 1);

            while (startYear <= DateTime.Today.Date.Year)
            {
                while (startQuarter != 0)
                {
                    int year = startYear;
                    int quarter = startQuarter;
                    var quarterShipments = from s in list
                                           where s.year == year
                                           && s.Quarter == quarter
                                           select s;
                    if (quarterShipments.Count() == 0)
                    {
                        MoneyStatusClass newEntry = new MoneyStatusClass()
                        {
                            year = startYear,
                            Quarter = startQuarter,
                            DateRange = "Q" + startQuarter.ToString() + "." + startYear.ToString().Substring(2, 2),
                            TotalAmount = 0,
                            DataType = "Invoices",
                        };
                        MoneyStatusClass newEntry2 = new MoneyStatusClass()
                        {
                            year = startYear,
                            Quarter = startQuarter,
                            DateRange = "Q" + startQuarter.ToString() + "." + startYear.ToString().Substring(2, 2),
                            TotalAmount = 0,
                            DataType = "Payments",
                        };

                        list.Add(newEntry);
                        list.Add(newEntry2);
                    }

                    startQuarter++;
                    if (startQuarter > 4)
                    {
                        startQuarter = 0;
                    }

                    else if (startYear == DateTime.Today.Date.Year && startQuarter > endQuarter)
                    {
                        startQuarter = 0;
                    }
                }

                startYear++;
                startQuarter++;
            }

            return list;
        }

        public List<DebtorsClass> GetDebtorExposure(int tenant, int currencyIndex)
        {
            IQueryable<ARInvoicePM> invoiceList = (from a in repository.context.ARInvoices
                                                   where a.Tenant == tenant && (a.StatusCode != "DR" && a.IsConstituentInvoice != true && a.StatusCode != "LL" && a.StatusCode != "VD" && a.IsClosed == false)
                                                   select new ARInvoicePM()
                                                   {
                                                       AmountDue = a.AmountDue,
                                                       AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                                                       AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                                                       BillToId = a.BillToId,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                                                       BranchId = a.BranchId,
                                                   }).AsQueryable();

            invoiceList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoicePM>(new QueryOperations(), invoiceList, tenant);



            List<DebtorsClass> datalist = (from a in invoiceList
                                           where a.Tenant == tenant
                                           group a by new
                                           {
                                               a.BillToId,
                                           } into gr
                                           orderby gr.Key.BillToId
                                           select new DebtorsClass()
                                           {
                                               Amount = currencyIndex == 1 ? gr.Sum(d => d.AmountDueInLocalCurrency) : gr.Sum(d => d.AmountDueInProfitCurrency),
                                               DebtorId = gr.Key.BillToId,

                                           }).OrderByDescending(d => d.Amount).Take(5).ToList();

            foreach (DebtorsClass invoice in datalist)
            {
                Card billto = CardRepository.GetSingleCard(invoice.DebtorId, tenant, true);
                if (billto != null)
                {
                    invoice.DebtorName = billto.EnglishName;
                }
            }

            return datalist;
        }

        public List<DebtorsClass> GetDebtorsExposureForGridControl(int tenant, int currencyIndex, bool isBranchRestricted)
        {
            if (FeatureToggleHelper.HasFeatureToggle("QPI", tenant))
                return GetDebtorsExposureForGridControl_NewStyle(tenant, currencyIndex, isBranchRestricted);
            else
                return GetDebtorsExposureForGridControl_OldStyle(tenant, currencyIndex);
        }
        private List<DebtorsClass> GetDebtorsExposureForGridControl_NewStyle(int tenant, int currencyIndex, bool isBranchRestricted)
        {
            var currentDate = DateTime.Now.AddMonths(-3);
            if (!repository.context.ARPayments.Any(x => x.CreateDate >= currentDate)) return new List<DebtorsClass>();

            List<string> allowedBranchesIds = new List<string>();
            if (isBranchRestricted) allowedBranchesIds = BranchPermitionsFilter.GetAllowedLoggedUserBranches(tenant);

            return (from a in repository.context.ARInvoices.Include("BillTo")
                    where a.Tenant == tenant
                    && a.StatusCode != "VD" && a.IsConstituentInvoice != true && a.StatusCode != "PD" && a.StatusCode != "DR" && a.StatusCode != "LL"
                    && !a.IsAutoCredit
                    && !a.IsCancelled
                    && !a.IsClosed
                    && (!isBranchRestricted || allowedBranchesIds.Contains(a.BranchId))
                    group a by new
                    {
                        a.BillTo.EnglishName,
                        a.BillToId,
                        a.BillTo.PartnerTypeId,
                    } into gr
                    orderby gr.Key.EnglishName
                    select new DebtorsClass()
                    {
                        Outstanding = currencyIndex == 1 ? gr.Sum(d => (d.AmountDueInLocalCurrency)) : gr.Sum(d => (d.AmountDueInProfitCurrency)),
                        DebtorName = gr.Key.EnglishName,
                        DebtorId = gr.Key.BillToId,
                        Overdue = currencyIndex == 1 ? gr.Where(d => d.DueDate <= DateTime.Today.Date).Sum(s => (s.AmountDueInLocalCurrency)) : gr.Where(d => d.DueDate <= DateTime.Today.Date).Sum(s => (s.AmountDueInProfitCurrency)),
                        DebtorType = gr.Key.PartnerTypeId,
                    }).OrderByDescending(d => d.Outstanding).Take(10).ToList();
        }
        private List<DebtorsClass> GetDebtorsExposureForGridControl_OldStyle(int tenant, int currencyIndex)
        {
            List<ARInvoicePM> invoiceList = (from a in repository.context.ARInvoices
                                             where a.Tenant == tenant && (a.StatusCode != "VD" && a.IsConstituentInvoice != true && a.StatusCode != "PD" && a.StatusCode != "DR" && a.StatusCode != "LL" && !a.IsAutoCredit && !a.IsCancelled && a.IsClosed == false)
                                             select new ARInvoicePM()
                                             {
                                                 AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                                                 AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                                                 BillToId = a.BillToId,
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                                                 DueDate = a.DueDate,
                                                 StatusCode = a.StatusCode,
                                                 ARInvoiceTypeCode = a.ARInvoiceTypeCode,
                                                 BranchId = a.BranchId,
                                             }).ToList();

            invoiceList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoicePM>(new QueryOperations(), invoiceList.AsQueryable<ARInvoicePM>(), tenant).ToList();

            foreach (ARInvoicePM invoice in invoiceList)
            {
                Card billto = CardRepository.GetSingleCard(invoice.BillToId, invoice.Tenant, true);
                invoice.BillToName = billto.EnglishName;
                invoice.BillToLocalName = billto.LocalName;
                invoice.BillToType = billto.PartnerTypeId;
            }

            List<DebtorsClass> datalist = (from a in invoiceList
                                           where a.Tenant == tenant
                                           group a by new
                                           {
                                               a.BillToName,
                                               a.BillToId,
                                               a.BillToType,
                                           } into gr
                                           orderby gr.Key.BillToName
                                           select new DebtorsClass()
                                           {
                                               Outstanding = currencyIndex == 1 ? gr.Sum(d => (d.AmountDueInLocalCurrency)) : gr.Sum(d => (d.AmountDueInProfitCurrency)),
                                               DebtorName = gr.Key.BillToName,
                                               DebtorId = gr.Key.BillToId,
                                               Overdue = currencyIndex == 1 ? gr.Where(d => d.DueDate <= DateTime.Today.Date).Sum(s => (s.AmountDueInLocalCurrency)) : gr.Where(d => d.DueDate <= DateTime.Today.Date).Sum(s => (s.AmountDueInProfitCurrency)),
                                               DebtorType = gr.Key.BillToType,
                                           }).ToList();

            datalist = datalist.OrderByDescending(d => d.Outstanding).Take(10).ToList();
            foreach (DebtorsClass debtor in datalist)
            {
                debtor.AmountLabel = string.Format((string)"{0:#,0.00}", (object)debtor.Amount);
            }
            return datalist;
        }

        public List<ARInvoiceTransferHistoryPM> GetARInvoiceTransferHistory(string entityId, int tenant)
        {
            AccountingTransferHeaderRepository transferHeaderRepository = new AccountingTransferHeaderRepository(tenant);
            AccountingTransferLineRepository transferLineRepository = new AccountingTransferLineRepository(tenant);
            List<AccountingTransferLine> transferLines = transferLineRepository.GetARInvoiceTransferLines(entityId, tenant).ToList();
            List<ARInvoiceTransferHistoryPM> TransferHistoryList = new List<ARInvoiceTransferHistoryPM>();

            foreach (AccountingTransferLine item in transferLines)
            {
                AccountingTransferHeader transferHeader = transferHeaderRepository.GetSingleEntity(item.AccountingTransferHeaderId, tenant);
                if (transferHeader != null)
                {
                    if (transferHeader.AccountingTransferTypeCode == "ARIN")
                    {
                        ARInvoiceTransferHistoryPM invoiceTransfer = new ARInvoiceTransferHistoryPM()
                        {
                            Id = transferHeader.Id,
                            ARInvoiceId = entityId,
                            FileName = transferHeader.FileName,
                            TransferDate = transferHeader.TransferDate,
                            TransferNumber = transferHeader.TransferNumber
                        };

                        TransferHistoryList.Add(invoiceTransfer);
                    }
                }
            }
            return TransferHistoryList;
        }

        public double GetInvoicesDueForCustomer(int tenant, string customerid)
        {
            DateTime nowdate = TenantServerConfigration.GetCurrentDateTime(tenant);
            IQueryable<ARInvoice> invoiceList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<ARInvoice>(new QueryOperations(), repository.context.ARInvoices.Where(t => t.Tenant == tenant), tenant);

            List<string> allowedStatuses = new List<string>() { "AD", "PP", "NT" };
            invoiceList = invoiceList.Where(a => a.BillToId == customerid && a.Tenant == tenant && allowedStatuses.Contains(a.StatusCode) && a.DueDate < nowdate && !a.IsAutoCredit && !a.IsClosed && !a.IsCancelled);

            double? invoicedue = (from a in invoiceList
                                  where a.ARInvoiceTypeCode != "CD"
                                  select a.AmountDueInLocalCurrency).Sum();

            double? autoCredit = (from a in invoiceList
                                  where a.ARInvoiceTypeCode == "CD"
                                  select a.AmountDueInLocalCurrency).Sum();

            double? result = invoicedue != null ? invoicedue : 0;
            if (autoCredit != null)
            {
                result = invoicedue - autoCredit;
            }
            return result != null ? result.Value : 0;
        }

        public int GetReadyForTransferInvoicesCount(int tenant)
        {
            int invoicesCount = (from a in repository.context.ARInvoices
                                 where a.Tenant == tenant && (a.TransferStatusCode == "RD" || a.TransferStatusCode == "ET") && a.TransferTries < 5 && a.StatusCode != "DR" && a.StatusCode != "VD"
                                 select a).Count();
            return invoicesCount;
        }

        public IQueryable<ARInvoiceList> GetUnpaidARInvoices(int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            var query = from a in repository.context.ARInvoices.Include("InvoiceCurrency").Include("BillTo").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("Branch")
                        where a.Tenant == tenant && a.StatusCode != "DR" && a.StatusCode != "VD" && a.StatusCode != "LL" && !a.IsAutoCredit && !a.IsCancelled && !a.IsClosed
                        select new ARInvoiceList()
                        {
                            AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                            AmountInLocalCurrency = a.AmountInLocalCurrency,
                            AmountInProfitCurrency = a.AmountInProfitCurrency,
                            AmountDue = a.AmountDue,
                            AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                            AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                            CreateDate = a.CreateDate,
                            CreatedByUserId = a.CreatedByUserId,
                            BillToAddressId = a.BillToAddressId,
                            BillToId = a.BillToId,
                            VatNumber = a.VatNumber,
                            DueDate = a.DueDate,
                            Id = a.Id,
                            InvoiceCurrencyId = a.InvoiceCurrencyId,
                            InvoiceDate = a.InvoiceDate,
                            InvoiceNumber = a.InvoiceNumber,
                            StatusCode = a.StatusCode,
                            InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                            ARInvoiceTypeCode = a.ARInvoiceTypeCode,
                            PrintDate = a.PrintDate,
                            Tenant = a.Tenant,
                            IsClosed = a.IsClosed,
                            HouseNumber = a.HouseNumber,
                            MasterNumber = a.MasterNumber,
                            UpdateDate = a.UpdateDate,
                            UpdatedByUserId = a.UpdatedByUserId,
                            BranchId = a.BranchId,
                            IsPrinted = a.IsPrinted,
                            HasDoc = a.DocumentFilingId != null ? true : false,
                            BillToName = a.BillTo == null ? "" : a.BillTo.EnglishName,
                            BillToCity = a.BillTo == null ? "" : a.BillTo.CityName,
                            BillToCountry = a.BillTo == null ? "" : a.BillTo.CountryName,
                            BillToCode = a.BillTo == null ? "" : a.BillTo.Code,
                            InvoiceCurrencyCode = a.InvoiceCurrency == null ? null : a.InvoiceCurrency.Code,
                            CustomerRef = a.CustomerRef,
                            IsConstituentInvoice = a.IsConstituentInvoice,
                            IsConsolidationInvoice = a.IsConsolidationInvoice,
                            ConsolidationInvoiceId = a.ConsolidationInvoiceId,
                            TransferTries = a.TransferTries,
                            TransferError = a.TransferError,
                            IsTransferStarted = a.IsTransferStarted,
                            TransferStatusCode = a.TransferStatusCode,
                            TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                            AccountingExternalCode = a.AccountingExternalCode,
                            ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                            PaymentTermExternalId = a.PaymentTermExternalId,
                            MainEntityId = a.MainEntityId,
                            MasterEntityId = a.MainEntityId,
                            MainEntityReference = a.MainEntityReference,
                            IsDueDateColorRed = (a.DueDate == null || a.StatusCode == "PD") ? false : (a.DueDate.Value < todayDate ? true : false),
                            IsExpectedPaymentDateColorRed = (a.ExpectedPaymentDate == null || a.StatusCode == "PD") ? false : (a.ExpectedPaymentDate.Value < todayDate ? true : false),
                            PrintByUserId = a.PrintByUserId,
                            ApprovedDate = a.ApprovedDate,
                            ApprovedByUserId = a.ApprovedByUserId,
                            ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                            OperationalDate = a.OperationalDate,
                            DateForInterest = a.DateForInterest,
                            SplitJournalByCurrency = a.SplitJournalByCurrency,
                            IsExternalEntity = a.IsExternalEntity,
                            IsGeneralInvoice = a.IsGeneralInvoice,
                            SATPaymentMethodCode = a.SATPaymentMethodCode,
                            SalesmanUserId = a.SalesmanUserId,
                            SalesmanUserName = a.SalesmanUser == null ? null : (a.SalesmanUser.Contact == null ? null : a.SalesmanUser.Contact.EnglishName),
                            IsCustomsChargesOnly = a.IsCustomsChargesOnly,
                            RelatedInvoice = a.RelatedInvoice,
                            MetodoPagoCode = a.MetodoPagoCode,
                            UsoCFDICode = a.UsoCFDICode,
                            RegimenFiscalCode = a.RegimenFiscalCode,
                            PeriodCode = a.PeriodCode,
                            SATTransferStatusCode = a.SATTransferStatusCode,
                            SATTransferStatusName = a.SATTransferStatus != null ? a.SATTransferStatus.Name : null,
                            SATInvoiceStatusCode = a.SATInvoiceStatusCode,
                            SATInvoiceStatusName = a.SATInvoiceStatus != null ? a.SATInvoiceStatus.Name : null,
                            TransmissionError = a.TransmissionError,
                            Intercompany = a.Intercompany,
                            BankAccountLiteId = a.BankAccountLiteId,
                            IsMultiCurrency = a.IsMultiCurrency,
                            TotalAmountForTaxReport = a.TotalAmountForTaxReport,
                            TotalVAT = a.TotalVAT,
                            TotaVatableAmountForTaxReport = a.TotaVatableAmountForTaxReport,
                            SATApprovalDate = a.SATApprovalDate,
                            IsFullAccounting = a.IsFullAccounting,
                            ARInvoiceStockId = a.ARInvoiceStockId,
                            BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                            CreatedByPartner = a.CreatedByPartner,
                            RegionalTaxId = a.RegionalTaxId,
                            RegionalTaxPercentage = a.RegionalTaxPercentage,
                            PaidDate = a.PaidDate,
                            PaidStatus = a.PaidStatus,
                            PartnerId = a.PartnerId,
                            GlobalTaxCalculation = a.GlobalTaxCalculation,
                            PaymentReferences = a.PaymentReferences,
                            SATCancelReasonCode = a.SATCancelReasonCode,
                            TotalAmountNotForTaxReport = (a.SubTotalInLocalCurrency ?? 0)
                                                         - (double)(a.TotalAmountForTaxReport ?? 0)
                        };

            return query;
        }

        public IQueryable<ARInvoiceList> GetIQueryableEntityList(IQueryable<ARInvoice> iQueryable/*, int tenant*/)
        {
            int tenant = 0;
            if (iQueryable != null && iQueryable.Count() > 0)
            {
                tenant = iQueryable.First().Tenant;
            }

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            // string[] invoiceStatusCodes = { "DR", "LL" };
            HashSet<string> invoiceStatusCodes = new HashSet<string>();
            invoiceStatusCodes.Add("DR");
            invoiceStatusCodes.Add("LL");

            var result = from entity in iQueryable.Include("BillTo").Include("BillTo.PartnerType").Include("CreatedByUser.Contact").Include("InvoiceCurrency").Include("Status").Include("ARInvoiceType").Include("IssuedByUser.Contact").Include("PrintByUser.Contact").Include("PaymentTerm").Include("ProfitCurrency").Include("LocalCurrency").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("CreditedByARInvoice").Include("SATInvoiceStatus").Include("SATTransferStatus").Include("Branch").Include("ARInvoicesSignedStatus")
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
                             InvoiceNumber = invoiceStatusCodes.Contains(entity.StatusCode) ? entity.DraftNumber : entity.InvoiceNumber,
                             DraftNumber = !string.IsNullOrEmpty(entity.DraftNumber) ? entity.DraftNumber : entity.Id,
                             StatusCode = entity.StatusCode,
                             StatusName = entity.Status == null ? "" : entity.Status.Name,
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
                             BillToCity = entity.BillTo == null ? "" : entity.BillTo.CityName,
                             BillToCountry = entity.BillTo == null ? "" : entity.BillTo.CountryName,
                             BillToCode = entity.BillTo.Code,
                             BillToPartnerName = entity.BillTo.PartnerType.Name,
                             BillToPartnerId = entity.BillTo.PartnerTypeId,
                             CreateDate = entity.CreateDate,
                             CreatedByUserName = entity.CreatedByUser.Contact.EnglishName,
                             InvoiceCurrencyCode = entity.InvoiceCurrency.Code,
                             ARInvoiceTypeName = entity.ARInvoiceType.Name,
                             CreatedByUserId = entity.CreatedByUserId,
                             IssuedByUserName = entity.IssuedByUser != null ? entity.IssuedByUser.Contact.EnglishName : null,
                             LocalCurrencyCode = entity.LocalCurrency.Code,
                             SearchFields = entity.SearchFields,
                             PrintByUserName = entity.PrintByUser != null ? entity.PrintByUser.Contact.EnglishName : null,
                             Sent = entity.Sent,
                             PaymentTermId = entity.PaymentTermId,
                             PaymentTermName = entity.PaymentTerm != null ? entity.PaymentTerm.EnglishName : null,
                             IsInvoiceNumberManuallySet = entity.IsInvoiceNumberManuallySet,
                             AmountDue = entity.AmountDue,
                             ExpectedPaymentDate = entity.ExpectedPaymentDate,
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
                             DebitAccount = entity.DebitAccount,
                             AmountInProfitCurrency = entity.AmountInProfitCurrency,
                             HouseNumber = entity.HouseNumber,
                             MasterNumber = entity.MasterNumber,
                             Description = entity.Description,
                             CustomerRef = entity.CustomerRef,
                             IsConstituentInvoice = entity.IsConstituentInvoice,
                             IsConsolidationInvoice = entity.IsConsolidationInvoice,
                             ConsolidationInvoiceId = entity.ConsolidationInvoiceId,
                             TransferTries = entity.TransferTries,
                             TransferError = entity.TransferError,
                             IsTransferStarted = entity.IsTransferStarted,
                             TransferStatusCode = entity.TransferStatusCode,
                             TransferStatusName = entity.TransferStatus == null ? "" : entity.TransferStatus.Name,
                             AccountingExternalCode = entity.AccountingExternalCode,
                             ReadyForTransfer = entity.TransferStatusCode == "RD" ? true : false,
                             PaymentTermExternalId = entity.PaymentTermExternalId,
                             MainEntityId = entity.MainEntityId,
                             MasterEntityId = entity.MainEntityId,
                             MainEntityReference = entity.MainEntityReference,
                          //   IsDueDateColorRed = (entity.DueDate == null || entity.StatusCode == "PD") ? false : (entity.DueDate.Value < todayDate ? true : false),
                           //  IsDigitalDueDateColorRed = (entity.DueDate == null || entity.PaidStatus == "Paid") ? false : (entity.DueDate.Value < todayDate ? true : false),
                            // IsExpectedPaymentDateColorRed = (entity.ExpectedPaymentDate == null || entity.StatusCode == "PD") ? false : (entity.ExpectedPaymentDate.Value < todayDate ? true : false),
                             UpdateDate = entity.UpdateDate,
                             UpdatedByUserId = entity.UpdatedByUserId,
                             ApprovedDate = entity.ApprovedDate,
                             ApprovedByUserId = entity.ApprovedByUserId,
                             ApprovedByUserName = entity.ApprovedByUser == null ? null : (entity.ApprovedByUser.Contact == null ? null : entity.ApprovedByUser.Contact.EnglishName),
                             OperationalDate = entity.OperationalDate,
                             DateForInterest = entity.DateForInterest,
                             SplitJournalByCurrency = entity.SplitJournalByCurrency,
                             IsExternalEntity = entity.IsExternalEntity,
                             IsGeneralInvoice = entity.IsGeneralInvoice,
                             SATPaymentMethodCode = entity.SATPaymentMethodCode,
                             SalesmanUserId = entity.SalesmanUserId,
                             SalesmanUserName = entity.SalesmanUser == null ? null : (entity.SalesmanUser.Contact == null ? null : entity.SalesmanUser.Contact.EnglishName),
                             IsCustomsChargesOnly = entity.IsCustomsChargesOnly,
                             RelatedInvoice = entity.RelatedInvoice,
                             CreditedByARInvoiceId = entity.CreditedByARInvoiceId,
                             CreditedByARInvoiceTypeCode = entity.CreditedByARInvoice == null ? null : entity.CreditedByARInvoice.ARInvoiceTypeCode,
                             MetodoPagoCode = entity.MetodoPagoCode,
                             UsoCFDICode = entity.UsoCFDICode,
                             RegimenFiscalCode = entity.RegimenFiscalCode,
                             PeriodCode = entity.PeriodCode,
                             SATTransferStatusCode = entity.SATTransferStatusCode,
                             SATTransferStatusName = entity.SATTransferStatus != null ? entity.SATTransferStatus.Name : null,
                             SATInvoiceStatusCode = entity.SATInvoiceStatusCode,
                             SATInvoiceStatusName = entity.SATInvoiceStatus != null ? entity.SATInvoiceStatus.Name : null,
                             TransmissionError = entity.TransmissionError,
                             Intercompany = entity.Intercompany,
                             BankAccountLiteId = entity.BankAccountLiteId,
                             IsMultiCurrency = entity.IsMultiCurrency,
                             TotalAmountForTaxReport = entity.TotalAmountForTaxReport,
                             TotalVAT = entity.TotalVAT,
                             TotaVatableAmountForTaxReport = entity.TotaVatableAmountForTaxReport,
                             SATApprovalDate = entity.SATApprovalDate,
                             IsFullAccounting = entity.IsFullAccounting,
                             IsInvoiceNumberFromStock = entity.IsInvoiceNumberFromStock,
                             BranchName = entity.Branch == null ? null : entity.Branch.EnglishName,
                             CreatedByPartner = entity.CreatedByPartner,
                             RegionalTaxId = entity.RegionalTaxId,
                             RegionalTaxPercentage = entity.RegionalTaxPercentage,
                             PaidDate = entity.PaidDate,
                             PaidStatus = entity.PaidStatus,
                             IsFromInterestBatchInvoice = entity.IsFromInterestBatchInvoice,
                             PartnerId = entity.PartnerId,
                             ShipmentsNumbers = entity.ShipmentsNumbers,
                             MasterNumbers = entity.MasterNumbers,
                             MasterShipmentNumbers = entity.MasterShipmentNumbers,
                             HouseNumbers = entity.HouseNumbers,
                             GlobalTaxCalculation = entity.GlobalTaxCalculation,
                             PaymentReferences = entity.PaymentReferences,
                             SATCancelReasonCode = entity.SATCancelReasonCode,
                             TotalExamptFortaxReport = entity.TotalExamptFortaxReport,
                             ConcurrencyGUID = entity.ConcurrencyGUID,
                             DocumentTemplateId = entity.DocumentTemplateId,
                             TotalAmountNotForTaxReport =
                                 (entity.SubTotalInLocalCurrency ?? 0)
                                 - (double)(entity.TotalAmountForTaxReport ?? 0),
                             IsSigned= entity.IsSigned,
                             IsSignedName= entity.ARInvoicesSignedStatus == null ? null: entity.ARInvoicesSignedStatus.LocalName,
                         };

            return result;
        }

        public IQueryable<ARInvoiceList> GetDigitalIQueryableEntityList(IQueryable<ARInvoice> iQueryable, int tenant = 0)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            string[] invoiceStatusCodes = { "DR", "LL" };

            var result = iQueryable.Include("BillTo")
                                    .Include("BillTo.PartnerType")
                                    .Include("CreatedByUser.Contact").Include("InvoiceCurrency")
                                    .Include("Status")
                                    .Include("ARInvoiceType")
                                    .Include("IssuedByUser.Contact")
                                    .Include("PrintByUser.Contact")
                                    .Include("PaymentTerm")
                                    .Include("ProfitCurrency")
                                    .Include("LocalCurrency")
                                    .Include("TransferStatus")
                                    .Include("ApprovedByUser")
                                    .Include("ApprovedByUser.Contact")
                                    .Include("SalesmanUser")
                                    .Include("SalesmanUser.Contact")
                                    .Include("CreditedByARInvoice")
                                    .Include("SATInvoiceStatus")
                                    .Include("SATTransferStatus")
                                    .Include("Branch")
                                    .Select(entity =>   new ARInvoiceList()
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
                                         InvoiceNumber = invoiceStatusCodes.Contains(entity.StatusCode) ? entity.DraftNumber : entity.InvoiceNumber,
                                         DraftNumber = !string.IsNullOrEmpty(entity.DraftNumber) ? entity.DraftNumber : entity.Id,
                                         StatusCode = entity.StatusCode,
                                         StatusName = entity.Status == null ? "" : entity.Status.Name,
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
                                         BillToCity = entity.BillTo == null ? "" : entity.BillTo.CityName,
                                         BillToCountry = entity.BillTo == null ? "" : entity.BillTo.CountryName,
                                         BillToCode = entity.BillTo.Code,
                                         BillToPartnerName = entity.BillTo.PartnerType.Name,
                                         BillToPartnerId = entity.BillTo.PartnerTypeId,
                                         CreateDate = entity.CreateDate,
                                         CreatedByUserName = entity.CreatedByUser.Contact.EnglishName,
                                         InvoiceCurrencyCode = entity.InvoiceCurrency.Code,
                                         ARInvoiceTypeName = entity.ARInvoiceType.Name,
                                         CreatedByUserId = entity.CreatedByUserId,
                                         IssuedByUserName = entity.IssuedByUser != null ? entity.IssuedByUser.Contact.EnglishName : null,
                                         LocalCurrencyCode = entity.LocalCurrency.Code,
                                         SearchFields = entity.SearchFields,
                                         PrintByUserName = entity.PrintByUser != null ? entity.PrintByUser.Contact.EnglishName : null,
                                         Sent = entity.Sent,
                                         PaymentTermId = entity.PaymentTermId,
                                         PaymentTermName = entity.PaymentTerm != null ? entity.PaymentTerm.EnglishName : null,
                                         IsInvoiceNumberManuallySet = entity.IsInvoiceNumberManuallySet,
                                         AmountDue = entity.AmountDue,
                                         ExpectedPaymentDate = entity.ExpectedPaymentDate,
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
                                         DebitAccount = entity.DebitAccount,
                                         AmountInProfitCurrency = entity.AmountInProfitCurrency,
                                         HouseNumber = entity.HouseNumber,
                                         MasterNumber = entity.MasterNumber,
                                         Description = entity.Description,
                                         CustomerRef = entity.CustomerRef,
                                         IsConstituentInvoice = entity.IsConstituentInvoice,
                                         IsConsolidationInvoice = entity.IsConsolidationInvoice,
                                         ConsolidationInvoiceId = entity.ConsolidationInvoiceId,
                                         TransferTries = entity.TransferTries,
                                         TransferError = entity.TransferError,
                                         IsTransferStarted = entity.IsTransferStarted,
                                         TransferStatusCode = entity.TransferStatusCode,
                                         TransferStatusName = entity.TransferStatus == null ? "" : entity.TransferStatus.Name,
                                         AccountingExternalCode = entity.AccountingExternalCode,
                                         ReadyForTransfer = entity.TransferStatusCode == "RD" ? true : false,
                                         PaymentTermExternalId = entity.PaymentTermExternalId,
                                         MainEntityId = entity.MainEntityId,
                                         MasterEntityId = entity.MainEntityId,
                                         MainEntityReference = entity.MainEntityReference,
                                         IsDueDateColorRed = (entity.DueDate == null || entity.StatusCode == "PD") ? false : (entity.DueDate.Value < todayDate ? true : false),
                                         IsDigitalDueDateColorRed = (entity.DueDate == null || entity.PaidStatus == "Paid") ? false : (entity.DueDate.Value < todayDate ? true : false),
                                         IsExpectedPaymentDateColorRed = (entity.ExpectedPaymentDate == null || entity.StatusCode == "PD") ? false : (entity.ExpectedPaymentDate.Value < todayDate ? true : false),
                                         UpdateDate = entity.UpdateDate,
                                         UpdatedByUserId = entity.UpdatedByUserId,
                                         ApprovedDate = entity.ApprovedDate,
                                         ApprovedByUserId = entity.ApprovedByUserId,
                                         ApprovedByUserName = entity.ApprovedByUser == null ? null : (entity.ApprovedByUser.Contact == null ? null : entity.ApprovedByUser.Contact.EnglishName),
                                         OperationalDate = entity.OperationalDate,
                                         DateForInterest = entity.DateForInterest,
                                         SplitJournalByCurrency = entity.SplitJournalByCurrency,
                                         IsExternalEntity = entity.IsExternalEntity,
                                         IsGeneralInvoice = entity.IsGeneralInvoice,
                                         SATPaymentMethodCode = entity.SATPaymentMethodCode,
                                         SalesmanUserId = entity.SalesmanUserId,
                                         SalesmanUserName = entity.SalesmanUser == null ? null : (entity.SalesmanUser.Contact == null ? null : entity.SalesmanUser.Contact.EnglishName),
                                         IsCustomsChargesOnly = entity.IsCustomsChargesOnly,
                                         RelatedInvoice = entity.RelatedInvoice,
                                         CreditedByARInvoiceId = entity.CreditedByARInvoiceId,
                                         CreditedByARInvoiceTypeCode = entity.CreditedByARInvoice == null ? null : entity.CreditedByARInvoice.ARInvoiceTypeCode,
                                         MetodoPagoCode = entity.MetodoPagoCode,
                                         UsoCFDICode = entity.UsoCFDICode,
                                         RegimenFiscalCode = entity.RegimenFiscalCode,
                                         PeriodCode = entity.PeriodCode,
                                         SATTransferStatusCode = entity.SATTransferStatusCode,
                                         SATTransferStatusName = entity.SATTransferStatus != null ? entity.SATTransferStatus.Name : null,
                                         SATInvoiceStatusCode = entity.SATInvoiceStatusCode,
                                         SATInvoiceStatusName = entity.SATInvoiceStatus != null ? entity.SATInvoiceStatus.Name : null,
                                         TransmissionError = entity.TransmissionError,
                                         Intercompany = entity.Intercompany,
                                         BankAccountLiteId = entity.BankAccountLiteId,
                                         IsMultiCurrency = entity.IsMultiCurrency,
                                         TotalAmountForTaxReport = entity.TotalAmountForTaxReport,
                                         TotalVAT = entity.TotalVAT,
                                         TotaVatableAmountForTaxReport = entity.TotaVatableAmountForTaxReport,
                                         SATApprovalDate = entity.SATApprovalDate,
                                         IsFullAccounting = entity.IsFullAccounting,
                                         IsInvoiceNumberFromStock = entity.IsInvoiceNumberFromStock,
                                         BranchName = entity.Branch == null ? null : entity.Branch.EnglishName,
                                         CreatedByPartner = entity.CreatedByPartner,
                                         RegionalTaxId = entity.RegionalTaxId,
                                         RegionalTaxPercentage = entity.RegionalTaxPercentage,
                                         PaidDate = entity.PaidDate,
                                         PaidStatus = entity.PaidStatus,
                                         IsFromInterestBatchInvoice = entity.IsFromInterestBatchInvoice,
                                         PartnerId = entity.PartnerId,
                                         ShipmentsNumbers = entity.ShipmentsNumbers,
                                         MasterNumbers = entity.MasterNumbers,
                                         MasterShipmentNumbers = entity.MasterShipmentNumbers,
                                         HouseNumbers = entity.HouseNumbers,
                                         GlobalTaxCalculation = entity.GlobalTaxCalculation,
                                         PaymentReferences = entity.PaymentReferences,
                                         SATCancelReasonCode = entity.SATCancelReasonCode,
                                         TotalExamptFortaxReport = entity.TotalExamptFortaxReport,
                                         DocumentTemplateId = entity.DocumentTemplateId,
                                         ConcurrencyGUID = entity.ConcurrencyGUID
                                    });
            return result;
        }

        public List<ARInvoicePM> GetInvoicesByCustomer(string customerId, int tenant)
        {
            List<ARInvoicePM> invoices = (from a in repository.context.ARInvoices.Include("BillTo").Include("InvoiceCurrency").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("Branch")
                                          where a.BillToId == customerId && a.Tenant == tenant
                                          select new ARInvoicePM()
                                          {
                                              ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                              ProfitCurrencyId = a.ProfitCurrencyId,
                                              SubTotalInInvoiceCurrency = a.SubTotalInInvoiceCurrency,
                                              SubTotalInLocalCurrency = a.SubTotalInLocalCurrency,
                                              AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                                              AmountInLocalCurrency = a.AmountInLocalCurrency,
                                              AmountInProfitCurrency = a.AmountInProfitCurrency,
                                              AmountDue = a.AmountDue,
                                              AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                                              AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                                              PaymentTermId = a.PaymentTermId,
                                              CreateDate = a.CreateDate,
                                              CreatedByUserId = a.CreatedByUserId,
                                              BillToAddressId = a.BillToAddressId,
                                              BillToId = a.BillToId,
                                              VatNumber = a.VatNumber,
                                              CancelledByARInvoiceId = a.CancelledByARInvoiceId,
                                              CreditedByARInvoiceId = a.CreditedByARInvoiceId,
                                              DueDate = a.DueDate,
                                              Id = a.Id,
                                              InternalNotes = a.InternalNotes,
                                              InvoiceCurrencyId = a.InvoiceCurrencyId,
                                              InvoiceDate = a.InvoiceDate,
                                              InvoiceNumber = a.InvoiceNumber,
                                              StatusCode = a.StatusCode,
                                              InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                                              ARInvoiceTypeCode = a.ARInvoiceTypeCode,
                                              IsAutoCredit = a.IsAutoCredit,
                                              IsCancelled = a.IsCancelled,
                                              IssuedByUserId = a.IssuedByUserId,
                                              LocalCurrencyId = a.LocalCurrencyId,
                                              PrintNotes = a.PrintNotes,
                                              PrintByUserId = a.PrintByUserId,
                                              PrintDate = a.PrintDate,
                                              Tenant = a.Tenant,
                                              PrepaidCollectId = a.PrepaidCollectId,
                                              DraftNumber = a.DraftNumber,
                                              IsInvoiceNumberManuallySet = a.IsInvoiceNumberManuallySet,
                                              Sent = a.Sent,
                                              ExchangeRateDate = a.ExchangeRateDate,
                                              MainEntityReference = a.MainEntityReference,
                                              MainEntityId = a.MainEntityId,
                                              IsClosed = a.IsClosed,
                                              HouseNumber = a.HouseNumber,
                                              MasterNumber = a.MasterNumber,
                                              Description = a.Description,
                                              ExpectedPaymentDate = a.ExpectedPaymentDate,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              BranchId = a.BranchId,
                                              IsPrinted = a.IsPrinted,
                                              HasDoc = a.DocumentFilingId != null ? true : false,
                                              DebitAccount = a.DebitAccount,
                                              BillToName = a.BillTo == null ? "" : a.BillTo.EnglishName,
                                              BillToCity = a.BillTo == null ? "" : a.BillTo.CityName,
                                              BillToCountry = a.BillTo == null ? "" : a.BillTo.CountryName,
                                              BillToCode = a.BillTo == null ? "" : a.BillTo.Code,
                                              BillToPartnerTypeId = a.BillTo == null ? "" : a.BillTo.PartnerTypeId,
                                              InvoiceCurrencyCode = a.InvoiceCurrency != null ? a.InvoiceCurrency.Code : null,
                                              CustomerRef = a.CustomerRef,
                                              IsConstituentInvoice = a.IsConstituentInvoice,
                                              IsConsolidationInvoice = a.IsConsolidationInvoice,
                                              ConsolidationInvoiceId = a.ConsolidationInvoiceId,
                                              TransferTries = a.TransferTries,
                                              TransferError = a.TransferError,
                                              IsTransferStarted = a.IsTransferStarted,
                                              TransferStatusCode = a.TransferStatusCode,
                                              TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                              AccountingExternalCode = a.AccountingExternalCode,
                                              ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                              PaymentTermExternalId = a.PaymentTermExternalId,
                                              ApprovedDate = a.ApprovedDate,
                                              ApprovedByUserId = a.ApprovedByUserId,
                                              ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                                              OperationalDate = a.OperationalDate,
                                              DateForInterest = a.DateForInterest,
                                              SplitJournalByCurrency = a.SplitJournalByCurrency,
                                              IsExternalEntity = a.IsExternalEntity,
                                              IsGeneralInvoice = a.IsGeneralInvoice,
                                              SATPaymentMethodCode = a.SATPaymentMethodCode,
                                              ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                              SalesmanUserId = a.SalesmanUserId,
                                              SalesmanUserName = a.SalesmanUser == null ? null : (a.SalesmanUser.Contact == null ? null : a.SalesmanUser.Contact.EnglishName),
                                              IsCustomsChargesOnly = a.IsCustomsChargesOnly,
                                              RelatedInvoice = a.RelatedInvoice,
                                              MetodoPagoCode = a.MetodoPagoCode,
                                              UsoCFDICode = a.UsoCFDICode,
                                              RegimenFiscalCode = a.RegimenFiscalCode,
                                              PeriodCode = a.PeriodCode,
                                              SATTransferStatusCode = a.SATTransferStatusCode,
                                              SATInvoiceStatusCode = a.SATInvoiceStatusCode,
                                              TransmissionError = a.TransmissionError,
                                              Intercompany = a.Intercompany,
                                              BankAccountLiteId = a.BankAccountLiteId,
                                              IsMultiCurrency = a.IsMultiCurrency,
                                              TotalAmountForTaxReport = a.TotalAmountForTaxReport,
                                              TotalVAT = a.TotalVAT,
                                              TotaVatableAmountForTaxReport = a.TotaVatableAmountForTaxReport,
                                              SATApprovalDate = a.SATApprovalDate,
                                              IsFullAccounting = a.IsFullAccounting,
                                              ARInvoiceStockId = a.ARInvoiceStockId,
                                              IsInvoiceNumberFromStock = a.IsInvoiceNumberFromStock,
                                              BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                              CreatedByPartner = a.CreatedByPartner,
                                              RegionalTaxId = a.RegionalTaxId,
                                              RegionalTaxPercentage = a.RegionalTaxPercentage,
                                              PaidDate = a.PaidDate,
                                              PaidStatus = a.PaidStatus,
                                              PartnerId = a.PartnerId,
                                              ShipmentsNumbers = a.ShipmentsNumbers,
                                              MasterNumbers = a.MasterNumbers,
                                              MasterShipmentNumbers = a.MasterShipmentNumbers,
                                              HouseNumbers = a.HouseNumbers,
                                              GlobalTaxCalculation = a.GlobalTaxCalculation,
                                              PaymentReferences = a.PaymentReferences,
                                              SATCancelReasonCode = a.SATCancelReasonCode,
                                              TotalEquation = a.TotalEquation,
                                              DocumentTemplateId = a.DocumentTemplateId,
                                              IsSigned=a.IsSigned,
                                          }).ToList();
            return invoices;
        }


        private ARInvoicePM GetSingleMappedEntityPM(ARInvoice entityPOCO, bool withComposition)
        {
            ARInvoicePM entityPM = null;
            ARInvoicePM securedEntityPM = null;

            if (entityPOCO != null)
            {
                int tenant = entityPOCO.Tenant;
                string entityId = entityPOCO.Id;
                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

                entityPM = new ARInvoicePM()
                {
                    StatusName = entityPOCO.Status?.Name,
                    ARInvoiceTypeName = entityPOCO.ARInvoiceType?.Name,
                    ProfitCurrencyExchangeRate = entityPOCO.ProfitCurrencyExchangeRate,
                    ProfitCurrencyId = entityPOCO.ProfitCurrencyId,
                    SubTotalInInvoiceCurrency = entityPOCO.SubTotalInInvoiceCurrency,
                    SubTotalInLocalCurrency = entityPOCO.SubTotalInLocalCurrency,
                    AmountInInvoiceCurrency = entityPOCO.AmountInInvoiceCurrency,
                    AmountInLocalCurrency = entityPOCO.AmountInLocalCurrency,
                    AmountInProfitCurrency = entityPOCO.AmountInProfitCurrency,
                    AmountDue = entityPOCO.AmountDue,
                    AmountDueInLocalCurrency = entityPOCO.AmountDueInLocalCurrency,
                    AmountDueInProfitCurrency = entityPOCO.AmountDueInProfitCurrency,
                    PaymentTermId = entityPOCO.PaymentTermId,
                    CreateDate = entityPOCO.CreateDate,
                    CreatedByUserId = entityPOCO.CreatedByUserId,
                    VatNumber = entityPOCO.VatNumber,
                    CancelledByARInvoiceId = entityPOCO.CancelledByARInvoiceId,
                    CreditedByARInvoiceId = entityPOCO.CreditedByARInvoiceId,
                    DueDate = entityPOCO.DueDate,
                    Id = entityPOCO.Id,
                    InternalNotes = entityPOCO.InternalNotes,
                    InvoiceCurrencyId = entityPOCO.InvoiceCurrencyId,
                    InvoiceDate = entityPOCO.InvoiceDate,
                    InvoiceNumber = entityPOCO.InvoiceNumber,
                    StatusCode = entityPOCO.StatusCode,
                    InvoiceCurrencyExchangeRate = entityPOCO.InvoiceCurrencyExchangeRate,
                    ARInvoiceTypeCode = entityPOCO.ARInvoiceTypeCode,
                    IsAutoCredit = entityPOCO.IsAutoCredit,
                    IsCancelled = entityPOCO.IsCancelled,
                    IssuedByUserId = entityPOCO.IssuedByUserId,
                    LocalCurrencyId = entityPOCO.LocalCurrencyId,
                    PrintNotes = entityPOCO.PrintNotes,
                    PrintByUserId = entityPOCO.PrintByUserId,
                    PrintDate = entityPOCO.PrintDate,
                    Tenant = entityPOCO.Tenant,
                    PrepaidCollectId = entityPOCO.PrepaidCollectId,
                    DraftNumber = entityPOCO.DraftNumber,
                    IsInvoiceNumberManuallySet = entityPOCO.IsInvoiceNumberManuallySet,
                    Sent = entityPOCO.Sent,
                    ExchangeRateDate = entityPOCO.ExchangeRateDate,
                    MainEntityReference = entityPOCO.MainEntityReference,
                    MainEntityId = entityPOCO.MainEntityId,
                    MasterEntityId = entityPOCO.MainEntityId,
                    IsClosed = entityPOCO.IsClosed,
                    HouseNumber = entityPOCO.HouseNumber,
                    MasterNumber = entityPOCO.MasterNumber,
                    Description = entityPOCO.Description,
                    ExpectedPaymentDate = entityPOCO.ExpectedPaymentDate,
                    UpdateDate = entityPOCO.UpdateDate,
                    UpdatedByUserId = entityPOCO.UpdatedByUserId,
                    BranchId = entityPOCO.BranchId,
                    IsPrinted = entityPOCO.IsPrinted,
                    HasDoc = entityPOCO.DocumentFilingId != null ? true : false,
                    DebitAccount = entityPOCO.DebitAccount,
                    BillToId = entityPOCO.BillToId,
                    BillToAddressId = entityPOCO.BillToAddressId,
                    CustomerRef = entityPOCO.CustomerRef,
                    IsConstituentInvoice = entityPOCO.IsConstituentInvoice,
                    IsConsolidationInvoice = entityPOCO.IsConsolidationInvoice,
                    ConsolidationInvoiceId = entityPOCO.ConsolidationInvoiceId,
                    TransferTries = entityPOCO.TransferTries,
                    TransferError = entityPOCO.TransferError,
                    IsTransferStarted = entityPOCO.IsTransferStarted,
                    TransferStatusCode = entityPOCO.TransferStatusCode,
                    TransferStatusName = entityPOCO.TransferStatus == null ? "" : entityPOCO.TransferStatus.Name,
                    AccountingExternalCode = entityPOCO.AccountingExternalCode,
                    ReadyForTransfer = entityPOCO.TransferStatusCode == "RD",
                    PaymentTermExternalId = entityPOCO.PaymentTermExternalId,
                    ApprovedDate = entityPOCO.ApprovedDate,
                    ApprovedByUserId = entityPOCO.ApprovedByUserId,
                    ApprovedByUserName = entityPOCO.ApprovedByUser?.Contact?.EnglishName,
                    OperationalDate = entityPOCO.OperationalDate,
                    DateForInterest = entityPOCO.DateForInterest,
                    SplitJournalByCurrency = entityPOCO.SplitJournalByCurrency,
                    IsExternalEntity = entityPOCO.IsExternalEntity,
                    IsGeneralInvoice = entityPOCO.IsGeneralInvoice,
                    ExternalAccountingEntityId = entityPOCO.ExternalAccountingEntityId,
                    SATPaymentMethodCode = entityPOCO.SATPaymentMethodCode,
                    SalesmanUserId = entityPOCO.SalesmanUserId,
                    SalesmanUserName = entityPOCO.SalesmanUser?.Contact?.EnglishName,
                    TransmissionError = entityPOCO.TransmissionError,
                    IsCustomsChargesOnly = entityPOCO.IsCustomsChargesOnly,
                    RelatedInvoice = entityPOCO.RelatedInvoice,
                    MetodoPagoCode = entityPOCO.MetodoPagoCode,
                    UsoCFDICode = entityPOCO.UsoCFDICode,
                    RegimenFiscalCode = entityPOCO.RegimenFiscalCode,
                    PeriodCode = entityPOCO.PeriodCode,
                    SATTransferStatusCode = entityPOCO.SATTransferStatusCode,
                    SATInvoiceStatusCode = entityPOCO.SATInvoiceStatusCode,
                    SATTransferStatusName = entityPOCO.SATTransferStatus?.Name,
                    SATInvoiceStatusName = entityPOCO.SATInvoiceStatus?.Name,
                    Intercompany = entityPOCO.Intercompany,
                    BankAccountLiteId = entityPOCO.BankAccountLiteId,
                    IsMultiCurrency = entityPOCO.IsMultiCurrency,
                    TotalAmountForTaxReport = entityPOCO.TotalAmountForTaxReport,
                    TotalVAT = entityPOCO.TotalVAT,
                    TotaVatableAmountForTaxReport = entityPOCO.TotaVatableAmountForTaxReport,
                    SATApprovalDate = entityPOCO.SATApprovalDate,
                    IsFullAccounting = entityPOCO.IsFullAccounting,
                    ARInvoiceStockId = entityPOCO.ARInvoiceStockId,
                    IsInvoiceNumberFromStock = entityPOCO.IsInvoiceNumberFromStock,
                    DocumentFilingId = entityPOCO.DocumentFilingId,
                    BranchName = entityPOCO.Branch?.EnglishName,
                    CreatedByPartner = entityPOCO.CreatedByPartner,
                    RegionalTaxId = entityPOCO.RegionalTaxId,
                    RegionalTaxPercentage = entityPOCO.RegionalTaxPercentage,
                    PaidDate = entityPOCO.PaidDate,
                    PaidStatus = entityPOCO.PaidStatus,
                    IsFromInterestBatchInvoice= entityPOCO.IsFromInterestBatchInvoice,
                    PartnerId = entityPOCO.PartnerId,
                    ShipmentsNumbers = entityPOCO.ShipmentsNumbers,
                    MasterNumbers = entityPOCO.MasterNumbers,
                    MasterShipmentNumbers = entityPOCO.MasterShipmentNumbers,
                    HouseNumbers = entityPOCO.HouseNumbers,
                    GlobalTaxCalculation = entityPOCO.GlobalTaxCalculation,
                    PaymentReferences = entityPOCO.PaymentReferences,
                    SATCancelReasonCode = entityPOCO.SATCancelReasonCode,
                    BillToGLAccountId = entityPOCO.BillToGLAccountId,
                    IsDigitalDueDateColorRed = (entityPOCO.DueDate == null || entityPOCO.PaidStatus == "Paid") ? false : (entityPOCO.DueDate.Value < todayDate ? true : false),
                    TotalEquation = entityPOCO.TotalEquation,
                    SATXML = entityPOCO.SATXML,
                    DocumentTemplateId = entityPOCO.DocumentTemplateId,
                    TransferStatusCode_Original = entityPOCO.TransferStatusCode,
                    IsTransferStarted_Original = entityPOCO.IsTransferStarted,
                    TransferError_Original = entityPOCO.TransferError,
                    IsSigned=entityPOCO.IsSigned,
                    ConfirmationNumber=entityPOCO.ConfirmationNumber,
                };

                entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
                entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();

                ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

                var myCardRepository = new CardRepository(myCommonContext);

                var myBillTo = myCardRepository.GetSingleCard(entityPOCO.BillToId, tenant);
                SetInterestReportFields(entityPM);

                if (myBillTo != null)
                {
                    entityPM.BillToName = myBillTo.EnglishName;
                    entityPM.BillToLocalName = myBillTo.LocalName;
                    entityPM.BillToCode = myBillTo.Code;
                    entityPM.BillToPartnerTypeId = myBillTo.PartnerTypeId;
                    entityPM.IsBillToAllowConsolidation = myBillTo.EnableConsolidationInvoices;
                    entityPM.BillToContactId = myBillTo.PrimaryContactId;

                    if (myBillTo.Customer != null)
                    {
                        if (myBillTo.Customer.AccountManagerUserId != null)
                        {
                            var myContactRepository = new ContactRepository(myCommonContext);

                            var myContact = myContactRepository.GetSingleContact(myBillTo.Customer.AccountManagerUserId, tenant);

                            if (myContact != null)
                            {
                                entityPM.BillToAccountManagerName = myContact.EnglishName;
                            }
                        }

                        entityPM.BillToIsCreditLimitEnabled = myBillTo.Customer.IsCreditLimitEnabled;
                        entityPM.BillToCreditLimitAmount = myBillTo.Customer.CreditLimitAmount;
                        entityPM.BillToCreditLimitOpenBalance = myBillTo.Customer.CreditLimitOpenBalance;
                        entityPM.BillToCreditLimitWarningPercentage = myBillTo.Customer.CreditLimitWarningPercentage;
                        entityPM.BillToBlockNewInvoiceCreation = myBillTo.Customer.BlockNewInvoiceCreation;
                        entityPM.BillToIsCustomer = myBillTo.Customer.IsCustomer;
                        entityPM.BillToCreditLimitActualAmount = this.GetCustomerCreditLimitActualAmount(entityPOCO.BillToId, tenant);

                        double? ActualBalance = 0;

                        if (entityPM.BillToCreditLimitOpenBalance != null)
                        {
                            ActualBalance += entityPM.BillToCreditLimitOpenBalance;
                        }

                        if (entityPM.BillToCreditLimitActualAmount != null)
                        {
                            ActualBalance += entityPM.BillToCreditLimitActualAmount;
                        }

                        entityPM.BillToCreditLimitActualBalance = ActualBalance;
                    }

                    else if (myBillTo.PartnerTypeId == "AG")
                    {
                        AgentRepository myRepository = new AgentRepository(myCommonContext);
                        Agent myAgent = myRepository.GetSingleAgent(tenant, entityPOCO.BillToId);

                        if (myAgent != null)
                        {
                            entityPM.BillToIsCreditLimitEnabled = myAgent.IsCreditLimitEnabled;
                            entityPM.BillToBlockNewInvoiceCreation = myAgent.BlockNewInvoiceCreation;
                        }
                    }
                }

                if (withComposition)
                {
                    var invoiceLineRepository = new ARInvoiceLineRepository(repository.context);
                    var invoiceEntityRepository = new ARInvoiceEntityRepository(repository.context);
                    var arInvoicePaymentRepository = new ARInvoicePaymentRepository(repository.context);
                    var myTotalVATRepository = new ARInvoiceTotalVATRepository(repository.context);
                    var arInvoiceLineQuery = new ARInvoiceLineQuery(invoiceLineRepository);
                    var arInvoiceEntityQuery = new ARInvoiceEntityQuery(invoiceEntityRepository);
                    var arInvoicePaymentQuery = new ARInvoicePaymentQuery(arInvoicePaymentRepository);
                    var myTotalVATQuery = new ARInvoiceTotalVATQuery(myTotalVATRepository);

                    entityPM.InvoiceLines = arInvoiceLineQuery.GetInvoiceLinePMsByInvoiceId(entityId, tenant);
                    if (IsAccountingActivated(tenant))
                        entityPM.InvoiceLines = arInvoiceLineQuery.GetGLAccountLocalNameAndDisplayNumber(entityPM.InvoiceLines, entityId, tenant);
                    entityPM.InvoiceEntities = arInvoiceEntityQuery.GetInvoiceEntityPMsForInvoice(entityId, tenant);
                    entityPM.InvoicePayments = arInvoicePaymentQuery.GetARInvoicePaymentPMsForInvoice(entityId, tenant);
                    entityPM.TotalVATs = myTotalVATQuery.GetTotalVATs(entityId, tenant).ToList();

                    if (entityPM.IsConsolidationInvoice)
                    {
                        entityPM.ConstituentInvoices = repository.context
                                                                 .ARInvoices
                                                                 .Where(d => d.Tenant == tenant
                                                                             && d.IsConstituentInvoice == true
                                                                             && d.ConsolidationInvoiceId == entityPM.Id)
                                                                 .Select(d => new ConstituentPM
                                                                 {
                                                                     Id = d.Id,
                                                                     Tenant = d.Tenant,
                                                                     ConsolidationInvoiceId = d.ConsolidationInvoiceId,
                                                                     InvoiceNumber = d.InvoiceNumber,
                                                                     CustomerRef = d.CustomerRef,
                                                                     MasterNumber = d.MasterNumber,
                                                                     HouseNumber = d.HouseNumber,
                                                                     MainEntityReference = d.MainEntityReference,
                                                                     AmountInInvoiceCurrency = d.AmountInInvoiceCurrency,
                                                                     SubTotalInInvoiceCurrency = d.SubTotalInInvoiceCurrency,
                                                                     ConcurrencyGUID = d.ConcurrencyGUID,
                                                                 }).ToList();

                        var ids = entityPM.ConstituentInvoices
                                          .Select(a => a.Id)
                                          .ToList();

                        var totalValts = myTotalVATQuery
                                         .GetTotalVATsByInvoicesIds(ids, tenant)
                                         .GroupBy(a => a.ARInvoiceId)
                                         .ToDictionary(a => a.Key,
                                                       x => x.Sum(a => a.InvoiceCurrencyVatableAmount));

                        foreach (var item in entityPM.ConstituentInvoices)
                        {
                            item.TotalVATs = totalValts.ContainsKey(item.Id)
                                             ? totalValts[item.Id]
                                             : 0.00;
                        }
                    }

                }

                var myPaymentTermRepository = new PaymentTermRepository(myCommonContext);
                var paymentTerm = myPaymentTermRepository.GetSinglePaymentTerm(entityPOCO.PaymentTermId, tenant);

                if (paymentTerm != null)
                {
                    entityPM.PaymentTermName = paymentTerm.EnglishName;
                }

                Currency currency = CurrencyRepository.GetSingleCurrency(entityPOCO.InvoiceCurrencyId, tenant, true);
                entityPM.InvoiceCurrencyCode = currency?.Code;

                Currency localCurrency = CurrencyRepository.GetSingleCurrency(entityPOCO.LocalCurrencyId, tenant, true);
                entityPM.LocalCurrencyCode = localCurrency?.Code;

                Currency profitCurrency = CurrencyRepository.GetSingleCurrency(entityPOCO.ProfitCurrencyId, tenant, true);
                entityPM.ProfitCurrencyCode = profitCurrency?.Code;

                ARInvoiceStatusRepository myARInvoiceStatusRepository = new ARInvoiceStatusRepository(repository.context);
                ARInvoiceStatus invoicestatus = myARInvoiceStatusRepository.GetSingleARInvoiceStatus(entityPOCO.StatusCode);
                entityPM.StatusName = invoicestatus?.Name;

                ARInvoiceTransferStatusRepository myARInvoiceTransferStatusRepository = new ARInvoiceTransferStatusRepository(repository.context);
                ARInvoiceTransferStatus transferStatus = myARInvoiceTransferStatusRepository.GetSingleARInvoiceTransferStatus(entityPOCO.TransferStatusCode);
                entityPM.TransferStatusName = transferStatus?.Name;

                var shipment = new ShipmentRepository(tenant).GetSingleForARInvoiceByIdAndTenant(entityPOCO.MainEntityId, tenant);
                entityPM.MainEntityStatus = shipment?.EntityStatus?.Name;
                entityPM.AgentReference1 = shipment?.AgentReference1;
                entityPM.AgentReference2 = shipment?.AgentReference2;

                if (entityPM.IsAutoCredit)
                {
                    entityPM.AutoCreditByARInvoiceId = repository.GetAutoCreditByInvoiceId(entityId, tenant);
                    entityPM.AutoCreditByARInvoiceNumber = repository.GetAutoCreditByInvoiceNumber(entityId, tenant);
                }

                if (entityPM.StatusCode == "AR")
                {
                    entityPM.AutoCreditedByARInvoiceId = repository.GetAutoCreditedByInvoiceId(entityId, tenant);
                    entityPM.AutoCreditedByARInvoiceNumber = repository.GetAutoCreditedByInvoiceNumber(entityId, tenant);
                }

                if (entityPM.IsConstituentInvoice)
                {
                    if (!string.IsNullOrEmpty(entityPM.ConsolidationInvoiceId))
                    {
                        ARInvoice myInvoice = repository.GetSingleInvoice(entityPM.ConsolidationInvoiceId);
                        if (myInvoice != null)
                        {
                            entityPM.ConsolidationInvoiceNumber = myInvoice.InvoiceNumber;
                        }
                    }
                }

                TenantRepository tenantRepository = new TenantRepository(myCommonContext);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
                if (tenantPOCO != null && tenantPOCO.AccountingActivated)
                {
                    var rep = new JournalRepository(tenant);

                    var journal = rep.GetJournalByAccountingEntityIdAndTypeCode(entityPM.Id, "2", tenant);

                    if (journal != null)
                    {
                        entityPM.JournalId = journal.JournalId;
                        entityPM.JournalNumber = journal.JournalNumber;
                    }
                }

                securedEntityPM = new ARInvoicePM();
                SecuredMapping.GetMappedPM(entityPM, securedEntityPM, "ARInvoice", tenant);

                if (securedEntityPM != null)
                {
                    securedEntityPM.Field1 = new CustomFieldClass("Field1", "ARInvoice", entityPOCO.Field1);
                    securedEntityPM.Field2 = new CustomFieldClass("Field2", "ARInvoice", entityPOCO.Field2);
                    securedEntityPM.Field3 = new CustomFieldClass("Field3", "ARInvoice", entityPOCO.Field3);
                    securedEntityPM.Field4 = new CustomFieldClass("Field4", "ARInvoice", entityPOCO.Field4);
                    securedEntityPM.Field5 = new CustomFieldClass("Field5", "ARInvoice", entityPOCO.Field5);
                    securedEntityPM.Field6 = new CustomFieldClass("Field6", "ARInvoice", entityPOCO.Field6);
                    securedEntityPM.Field7 = new CustomFieldClass("Field7", "ARInvoice", entityPOCO.Field7);
                    securedEntityPM.Field8 = new CustomFieldClass("Field8", "ARInvoice", entityPOCO.Field8);
                    securedEntityPM.Field9 = new CustomFieldClass("Field9", "ARInvoice", entityPOCO.Field9);
                    securedEntityPM.Field10 = new CustomFieldClass("Field10", "ARInvoice", entityPOCO.Field10);

                    securedEntityPM = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedEntityPM, tenant);

                    if (securedEntityPM == null)
                    {
                        throw new ApplicationException("This invoice is branch Restricted");
                    }
                }
            }

            return securedEntityPM;
        }

        private void SetInterestReportFields(ARInvoicePM invoice)
        {
            InterestReport interestReport = new InterestReport();
            if (invoice.ARInvoiceTypeCode == InterestReportInvoiceTypeCode && IsAccountingActivated(invoice.Tenant))
            {
                InterestReportRepository interestReportRepository = new InterestReportRepository(invoice.Tenant);
                interestReport = interestReportRepository.GetSingleByARInvoiceId(invoice.Id, invoice.Tenant);
            }
            if (interestReport != null)
            {
                invoice.InterestReportNumber = interestReport.ReportNumber;
                invoice.InterestReportId = interestReport.Id;
            }
        }
        private bool IsAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            tenantPoco = tenantRepository.GetSingleTenant(tenant);
            return tenantPoco.AccountingActivated;
        }

        public IQueryable<ARInvoice> GetAllInterestInvoices(DateTime fromDate, DateTime toDate, bool ShowPrintedInvoice, int tenant)
        {
            var result = (from a in repository.context.ARInvoices where a.Tenant == tenant && a.ARInvoiceTypeCode == "IT" && a.InvoiceDate >= fromDate && a.InvoiceDate <= toDate select a);
            if (!ShowPrintedInvoice)
            {
                result = result.Where(s => s.IsPrinted == false);
            }
            return result;
        }

        public List<string> GetInterestInvoiceNumbersByIds(List<string> ARInvoiceIds, int tenant)
        {
            var result = (from a in repository.context.ARInvoices where a.Tenant == tenant && a.ARInvoiceTypeCode == "IT" && ARInvoiceIds.Contains(a.Id) select a.InvoiceNumber).ToList();

            return result;
        }
        public IQueryable<ARInvoiceList> GetInvoiceListByTenant(int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            var result = from entity in repository.context.ARInvoices.Include("BillTo").Include("BillTo.PartnerType").Include("CreatedByUser.Contact").Include("InvoiceCurrency").Include("Status").Include("ARInvoiceType").Include("IssuedByUser.Contact").Include("PrintByUser.Contact").Include("PaymentTerm").Include("ProfitCurrency").Include("LocalCurrency").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("Branch")
                         where entity.Tenant == tenant && entity.StatusCode != "LL"
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
                             InvoiceNumber = entity.StatusCode != "DR" && entity.StatusCode != "LL" ? entity.InvoiceNumber : (!string.IsNullOrEmpty(entity.DraftNumber) ? entity.DraftNumber : entity.Id),
                             DraftNumber = !string.IsNullOrEmpty(entity.DraftNumber) ? entity.DraftNumber : entity.Id,
                             StatusCode = entity.StatusCode,
                             StatusName = entity.Status == null ? "" : entity.Status.Name,
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
                             BillToCity = entity.BillTo == null ? "" : entity.BillTo.CityName,
                             BillToCountry = entity.BillTo == null ? "" : entity.BillTo.CountryName,
                             BillToCode = entity.BillTo.Code,
                             BillToPartnerName = entity.BillTo.PartnerType.Name,
                             BillToPartnerId = entity.BillTo.PartnerTypeId,
                             CreateDate = entity.CreateDate,
                             CreatedByUserName = entity.CreatedByUser.Contact.EnglishName,
                             InvoiceCurrencyCode = entity.InvoiceCurrency.Code,
                             ARInvoiceTypeName = entity.ARInvoiceType.Name,
                             CreatedByUserId = entity.CreatedByUserId,
                             IssuedByUserName = entity.IssuedByUser != null ? entity.IssuedByUser.Contact.EnglishName : null,
                             LocalCurrencyCode = entity.LocalCurrency.Code,
                             SearchFields = entity.SearchFields,
                             PrintByUserName = entity.PrintByUser != null ? entity.PrintByUser.Contact.EnglishName : null,
                             Sent = entity.Sent,
                             PaymentTermId = entity.PaymentTermId,
                             PaymentTermName = entity.PaymentTerm != null ? entity.PaymentTerm.EnglishName : null,
                             IsInvoiceNumberManuallySet = entity.IsInvoiceNumberManuallySet,
                             AmountDue = entity.AmountDue,
                             ExpectedPaymentDate = entity.ExpectedPaymentDate,
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
                             DebitAccount = entity.DebitAccount,
                             AmountInProfitCurrency = entity.AmountInProfitCurrency,
                             HouseNumber = entity.HouseNumber,
                             MasterNumber = entity.MasterNumber,
                             Description = entity.Description,
                             CustomerRef = entity.CustomerRef,
                             IsConstituentInvoice = entity.IsConstituentInvoice,
                             IsConsolidationInvoice = entity.IsConsolidationInvoice,
                             ConsolidationInvoiceId = entity.ConsolidationInvoiceId,
                             TransferTries = entity.TransferTries,
                             TransferError = entity.TransferError,
                             IsTransferStarted = entity.IsTransferStarted,
                             TransferStatusCode = entity.TransferStatusCode,
                             TransferStatusName = entity.TransferStatus == null ? "" : entity.TransferStatus.Name,
                             AccountingExternalCode = entity.AccountingExternalCode,
                             ReadyForTransfer = entity.TransferStatusCode == "RD" ? true : false,
                             PaymentTermExternalId = entity.PaymentTermExternalId,
                             MainEntityId = entity.MainEntityId,
                             MasterEntityId = entity.MainEntityId,
                             MainEntityReference = entity.MainEntityReference,
                             IsDueDateColorRed = (entity.DueDate == null || entity.StatusCode == "PD") ? false : (entity.DueDate.Value < todayDate ? true : false),
                             IsExpectedPaymentDateColorRed = (entity.ExpectedPaymentDate == null || entity.StatusCode == "PD") ? false : (entity.ExpectedPaymentDate.Value < todayDate ? true : false),
                             UpdateDate = entity.UpdateDate,
                             UpdatedByUserId = entity.UpdatedByUserId,
                             BranchId = entity.BranchId,
                             ApprovedDate = entity.ApprovedDate,
                             ApprovedByUserId = entity.ApprovedByUserId,
                             ApprovedByUserName = entity.ApprovedByUser == null ? null : (entity.ApprovedByUser.Contact == null ? null : entity.ApprovedByUser.Contact.EnglishName),
                             OperationalDate = entity.OperationalDate,
                             DateForInterest = entity.DateForInterest,
                             SplitJournalByCurrency = entity.SplitJournalByCurrency,
                             IsExternalEntity = entity.IsExternalEntity,
                             IsGeneralInvoice = entity.IsGeneralInvoice,
                             SATPaymentMethodCode = entity.SATPaymentMethodCode,
                             SalesmanUserId = entity.SalesmanUserId,
                             SalesmanUserName = entity.SalesmanUser == null ? null : (entity.SalesmanUser.Contact == null ? null : entity.SalesmanUser.Contact.EnglishName),
                             IsCustomsChargesOnly = entity.IsCustomsChargesOnly,
                             MetodoPagoCode = entity.MetodoPagoCode,
                             UsoCFDICode = entity.UsoCFDICode,
                             RegimenFiscalCode = entity.RegimenFiscalCode,
                             PeriodCode = entity.PeriodCode,
                             SATTransferStatusCode = entity.SATTransferStatusCode,
                             SATTransferStatusName = entity.SATTransferStatus != null ? entity.SATTransferStatus.Name : null,
                             SATInvoiceStatusCode = entity.SATInvoiceStatusCode,
                             SATInvoiceStatusName = entity.SATInvoiceStatus != null ? entity.SATInvoiceStatus.Name : null,
                             TransmissionError = entity.TransmissionError,
                             Intercompany = entity.Intercompany,
                             BankAccountLiteId = entity.BankAccountLiteId,
                             IsMultiCurrency = entity.IsMultiCurrency,
                             TotalAmountForTaxReport = entity.TotalAmountForTaxReport,
                             TotalVAT = entity.TotalVAT,
                             TotaVatableAmountForTaxReport = entity.TotaVatableAmountForTaxReport,
                             SATApprovalDate = entity.SATApprovalDate,
                             IsFullAccounting = entity.IsFullAccounting,
                             ARInvoiceStockId = entity.ARInvoiceStockId,
                             IsInvoiceNumberFromStock = entity.IsInvoiceNumberFromStock,
                             BranchName = entity.Branch == null ? null : entity.Branch.EnglishName,
                             CreatedByPartner = entity.CreatedByPartner,
                             SATXML = entity.SATXML,
                             RegionalTaxId = entity.RegionalTaxId,
                             RegionalTaxPercentage = entity.RegionalTaxPercentage,
                             PaidDate = entity.PaidDate,
                             PaidStatus = entity.PaidStatus,
                             PartnerId = entity.PartnerId,
                             PartnerName = entity.Partner.EnglishName,
                             GlobalTaxCalculation = entity.GlobalTaxCalculation,
                             PaymentReferences = entity.PaymentReferences,
                             SATCancelReasonCode = entity.SATCancelReasonCode,
                             DocumentTemplateId = entity.DocumentTemplateId,
                             TotalAmountNotForTaxReport = (entity.SubTotalInLocalCurrency ?? 0)
                                                          - (double)(entity.TotalAmountForTaxReport ?? 0)
                         };

            return result;
        }

        public double? GetCustomerCreditLimitActualAmount(string myCustomerId, int tenant, string invoiceId = null)
        {
            double? myResult = 0;

            if (!string.IsNullOrEmpty(myCustomerId))
            {
                IInvoiceContext myContext = repository.context;

                DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

                double? sumOfAmountDue = 0;

                if (invoiceId == null)
                {
                    sumOfAmountDue = (from d in myContext.ARInvoices
                                      where d.Tenant == tenant
                                      && d.BillToId == myCustomerId
                                      && d.StatusCode != "VD"
                                      && d.StatusCode != "AR"
                                      && d.StatusCode != "LL"
                                      select d).Sum(s => s.AmountDueInLocalCurrency);
                }

                else
                {
                    sumOfAmountDue = (from d in myContext.ARInvoices
                                      where d.Tenant == tenant
                                      && d.BillToId == myCustomerId
                                      && d.StatusCode != "VD"
                                      && d.StatusCode != "AR"
                                      && d.StatusCode != "LL"
                                      && d.Id != invoiceId
                                      select d).Sum(s => s.AmountDueInLocalCurrency);
                }

                double? sumOfOpenAmount = (from d in myContext.ARPayments
                                           where d.Tenant == tenant
                                           && d.BillToId == myCustomerId
                                           && d.StatusCode != "VD"
                                           select d).Sum(s => s.OpenAmount * s.PaymentCurrencyExchangeRate);

                double? sumOfOpenAmountFuture = (from d in myContext.ARPayments
                                                 where d.Tenant == tenant
                                                 && d.BillToId == myCustomerId
                                                 && d.StatusCode != "VD"
                                                 && d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) > todayDate
                                                 select d).Sum(s => s.OpenAmount * s.PaymentCurrencyExchangeRate);

                if (sumOfAmountDue == null)
                {
                    sumOfAmountDue = 0;
                }

                if (sumOfOpenAmount == null)
                {
                    sumOfOpenAmount = 0;
                }

                if (sumOfOpenAmountFuture == null)
                {
                    sumOfOpenAmountFuture = 0;
                }

                myResult = sumOfAmountDue - sumOfOpenAmount + sumOfOpenAmountFuture;
            }

            return myResult;
        }

        public List<ARInvoicePM> GetARInvoicePMsByIdList(List<string> idList, int tenant)
        {
            List<ARInvoice> entityPOCOs =
                        (from a in repository.context.ARInvoices.Include("ProfitCurrency").Include("InvoiceCurrency").Include("Status").Include("LocalCurrency").Include("BillTo").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("SalesmanUser").Include("SalesmanUser.Contact").Include("SATInvoiceStatus").Include("SATTransferStatus")
                         where idList.Contains(a.Id) && a.Tenant == tenant
                         select a).ToList();

            List<ARInvoicePM> pms = entityPOCOs.Select(poco => GetSingleMappedEntityPM(poco, true)).ToList();
            return pms;
        }

        public string GetARinvoiceTypeCode(string id, int tenant)
        {
            ARInvoice invoice = (from a in repository.context.ARInvoices
                                 where a.Id == id && a.Tenant == tenant
                                 select a).FirstOrDefault();
            return invoice != null ? invoice.ARInvoiceTypeCode : null;
        }

        #region Digital Portal 

        public IQueryable<ARInvoice> GetByFiltersForDashBoard(GeneralFilters newFilters)
        {
            var tenant = newFilters.Tenant;
            var myTenantRepository = new TenantRepository(tenant);
            var myTenant = myTenantRepository.GetSingleTenant(tenant);

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
            queryOperations.SetFilter("IsConstituentInvoice", false, false, "Equals", null, false);

            var cardFilterValues = newFilters.CardId;
            if (!string.IsNullOrWhiteSpace(cardFilterValues))
            {
                var cardBillToIds = GetCardBillToId(newFilters.CardId, tenant);
                if (cardBillToIds.Any())
                {
                    cardFilterValues = cardFilterValues + "," + string.Join(",", cardBillToIds);
                    queryOperations.SetFilter("PartnerId", newFilters.CardId, false, "InList", null, false);
                }

                queryOperations.SetFilter("BillToId", cardFilterValues, false, "InList", null, false);
            }

            var ARInvoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARInvoice", tenant);

            if (newFilters.AdditionalFilters.Any())
            {
                foreach (var filter in newFilters.AdditionalFilters)
                {
                    var field = ARInvoiceObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                    if (field != null)
                    {
                        string valuestring1 = filter.FieldValue?.ToString();
                        object value1 = FieldValueResolver.GetFieldDataValue(field, valuestring1);
                        string valuestring2 = filter.FieldValue2?.ToString();
                        object value2 = FieldValueResolver.GetFieldDataValue(field, valuestring2);
                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }

            var genericFilter = new GenericFilter();
            var MyContext = InvoiceContext.GetContext(tenant);

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

            var entityPocos = aRInvoiceRepository.GetARInvoices(tenant);

            entityPocos = aRInvoiceRepository.FilterInvoicesStatusesForList(entityPocos);

            var customfilters = new ARInvoiceCustomFilter(tenant);
            entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);
            entityPocos = genericFilter.GetFilteredQuery(nonListQueryOperation, entityPocos);
            return entityPocos;
        }

        public IQueryable<ARInvoiceList> GetByFilters(GeneralFilters newFilters)
        {
            var tenant = newFilters.Tenant;
            var myTenantRepository = new TenantRepository(tenant);
            var myTenant = myTenantRepository.GetSingleTenant(tenant);

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
            queryOperations.SetFilter("IsConstituentInvoice", false, false, "Equals", null, false);

            var cardFilterValues = newFilters.CardId;
            if (!string.IsNullOrWhiteSpace(cardFilterValues))
            {
                var cardBillToIds = GetCardBillToId(newFilters.CardId, tenant);
                if (cardBillToIds.Any())
                {
                    cardFilterValues = cardFilterValues + "," + string.Join(",", cardBillToIds);
                    queryOperations.SetFilter("PartnerId", newFilters.CardId, false, "InList", null, false);
                }

                queryOperations.SetFilter("BillToId", cardFilterValues, false, "InList", null, false);
            }

            var ARInvoiceObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ARInvoice", tenant);

            if (newFilters.AdditionalFilters.Any())
            {
                foreach (var filter in newFilters.AdditionalFilters)
                {
                    var field = ARInvoiceObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);

                    if (field != null)
                    {
                        string valuestring1 = filter.FieldValue?.ToString();
                        object value1 = FieldValueResolver.GetFieldDataValue(field, valuestring1);
                        string valuestring2 = filter.FieldValue2?.ToString();
                        object value2 = FieldValueResolver.GetFieldDataValue(field, valuestring2);
                        queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);
                    }
                    else
                    {
                        queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                    }
                }
            }

            var genericFilter = new GenericFilter();
            var MyContext = InvoiceContext.GetContext(tenant);

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

            var entityPocos = aRInvoiceRepository.GetARInvoices(tenant);

            entityPocos = aRInvoiceRepository.FilterInvoicesStatusesForList(entityPocos);

            var customfilters = new ARInvoiceCustomFilter(tenant);
            entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos);
            entityPocos = genericFilter.GetFilteredQuery(nonListQueryOperation, entityPocos);

            var entityLists = aRInvoiceQuery.GetDigitalIQueryableEntityList(entityPocos, tenant);

            entityLists = genericFilter.GetFilteredQuery(listQueryOperation, entityLists);

            if (!string.IsNullOrWhiteSpace(queryOperations.SortByColumnName) && !string.IsNullOrWhiteSpace(queryOperations.SortDirectin))
            {
                ObjectField objectField = ARInvoiceObjectFields.FirstOrDefault(a => a.FieldName == queryOperations.SortByColumnName);

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

            return entityLists;
        }

        public List<string> GetCardBillToId(string cardId, int tenant)
        {
            CardRepository cardRepository = new CardRepository(tenant);
            var cardBillToIds = cardRepository.GetBillToCardById(cardId, tenant);
            return cardBillToIds;
        }

        #endregion Digital Portal 
    }
}
