using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityQueries.Charts
{
    class TopFiveSalesmanQuery
    {
        int tenant;
        IQueryable<Quote> dataSourceQuery;
        QuoteDashboardArguments args;       
        List<string> keys;
        List<string> labels;
        Dictionary<string, string> salesmanNames;
        List<ChartingDataClass> data;
        private DateTime todayDate;
        public TopFiveSalesmanQuery(IQueryable<Quote> iQueryable, QuoteDashboardArguments args, int tenant)
        {
            this.args = args;
            this.tenant = tenant;
            this.data = new List<ChartingDataClass>();
            this.todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            this.dataSourceQuery = (from d in iQueryable
                                    where d.QuoteTypeCode == "A"
                                    && d.EstimateProfit != null
                                    && d.EstimateProfit != 0
                                    select d);
        }

        public List<ChartingDataClass> FilterToFiveSalesmanByProfit()
        {
            GetChartKeys();
            GetChartLabels();

            if (keys.Count > 0)
            {
                this.dataSourceQuery = this.dataSourceQuery.Where(d => keys.Contains(d.SalesmanUserId));
                
                GetChartDataItems();
                FillMissingData();
            }

            return data;
        }

        private void GetChartKeys()
        {
            var topFiveSalesman = (from d in dataSourceQuery
                                   group d by d.SalesmanUserId into g
                                   select new
                                   {
                                       Id = g.Key,
                                       Profit = g.Sum(s => s.EstimatedProfitInLocal),
                                   }).OrderByDescending(o => o.Profit).Take(5).ToList();

            keys = topFiveSalesman.Select(s => s.Id).ToList();

            if (keys.Count > 0)
            {
                UserQuery userQuery = new UserQuery();
                salesmanNames = userQuery.GetUsersListFromIdList(keys, tenant);
            }
        }
        private void GetChartLabels()
        {
            labels = new List<string>();

            switch (args.DatesCode)
            {
                case "0":
                    {
                        labels = DateLabelBuilder.GetDays(todayDate, todayDate);
                        break;
                    }

                case "-1":
                    {
                        labels = DateLabelBuilder.GetDays(todayDate.AddDays(-1), todayDate.AddDays(-1));
                        break;
                    }

                case "-7":
                    {
                        labels = DateLabelBuilder.GetDays(todayDate.AddDays(-6), todayDate);
                        break;
                    }

                case "-30":
                    {
                        labels = DateLabelBuilder.GetWeeks(todayDate.AddMonths(-1), todayDate);
                        break;
                    }

                case "-90":
                    {
                        labels = DateLabelBuilder.GetMonths(todayDate.AddMonths(-3), todayDate);
                        break;
                    }

                case "-365":
                    {
                        labels = DateLabelBuilder.GetQuarters(todayDate.AddYears(-1), todayDate);
                        break;
                    }

                case "-2":
                    {
                        if (args.FromDate != null && args.ToDate != null)
                        {
                            DateTime date1 = args.FromDate.Value;
                            DateTime date2 = args.ToDate.Value;

                            double days = ((TimeSpan)(args.ToDate - args.FromDate)).TotalDays;

                            if (days <= 7)
                            {
                                labels = DateLabelBuilder.GetDays(date1, date2);
                            }

                            else if (days <= 30)
                            {
                                labels = DateLabelBuilder.GetWeeks(date1, date2);
                            }

                            else if (days <= 90)
                            {
                                labels = DateLabelBuilder.GetMonths(date1, date2);
                            }

                            else if (days <= 365)
                            {
                                labels = DateLabelBuilder.GetQuarters(date1, date2);
                            }

                            else
                            {
                                labels = DateLabelBuilder.GetYears(date1, date2);
                            }
                        }

                        



                        //double period = Math.Ceiling(differnceDays / 4);
                        //DateTime fromDate = (DateTime)args.FromDate;
                        //DateTime toDate = fromDate.AddDays(period);
                        //for (int i = 0; i < 4; i++)
                        //{
                        //    if (differnceDays > 365)
                        //    {
                        //        output.Add(fromDate.ToString("MMM yy") + " - " + toDate.ToString("MMM yy") + " ");
                        //    }
                        //    else
                        //    {
                        //        output.Add(fromDate.ToString("dd MMM") + " - " + toDate.ToString("dd MMM") + " ");
                        //    }

                        //    fromDate = toDate.AddDays(1);
                        //    toDate = fromDate.AddDays(period);
                        //    if (toDate > args.ToDate)
                        //    {
                        //        toDate = (DateTime)args.ToDate;
                        //    }
                        //}

                        //labels = output;

                        break;
                    }
            }            
        }

        private void FillMissingData()
        {
            int index = 0;
            foreach (string key in keys)
            {
                foreach (string label in labels)
                {
                    if (!data.Where(d => d.Key == key && d.Label == label).Any())
                    {
                        string salesmanName;
                        salesmanNames.TryGetValue(key, out salesmanName);
                        data.Insert(index, new ChartingDataClass()
                        {
                            Key = key,
                            Label = label,
                            SalesmanUserName = salesmanName,
                            Value = 0
                        });
                    }
                    index++;
                }
            }

            foreach (ChartingDataClass item in data)
            {
                if (item.Value == null)
                {
                    item.Value = 0;
                }
                else
                {
                    item.Value = MethodHelper.Round(item.Value, 2);
                }
            }
        }






        private void GetChartDataItems()
        {
            string datesCode = args.DatesCode;
            switch (datesCode)
            {
                case "0":
                case "-1":
                case "-7":
                    {
                        data = GetChartItemsByDay(dataSourceQuery);
                        break;
                    }
                case "-30":
                    {
                        data = GetChartItemsByWeek(dataSourceQuery);
                        break;
                    }
                case "-90":
                    {
                        data = GetChartItemsByMonth(dataSourceQuery);
                        break;
                    }
                case "-365":
                    {
                        data = GetChartItemsByQuarter(dataSourceQuery);
                        break;
                    }
                case "-2":
                    {
                        data = GetChartItemsInCustom(dataSourceQuery);
                        break;
                    }
                default:
                    {
                        data = new List<ChartingDataClass>();
                        break;
                    }
            }
        }

        private List<ChartingDataClass> GetChartItemsInCustom(IQueryable<Quote> dataSourceQuery)
        {
            double differnceDays = ((TimeSpan)(args.ToDate - args.FromDate)).TotalDays;

            if (differnceDays < 0)
                throw new ApplicationException("To date must be smaller than from date");

            double period = Math.Ceiling(differnceDays / 4);
            DateTime fromDate = (DateTime)args.FromDate;
            DateTime toDate = fromDate.AddDays(period);
            List<ChartingDataClass> output = new List<ChartingDataClass>();
            List<ChartingDataClass> items = new List<ChartingDataClass>();
            IQueryable<Quote> temp;
            for (int i = 0; i < 4; i++)
            {
                temp = FilterPeriod(dataSourceQuery, fromDate, toDate);
                items  = (from d in temp.Include("SalesmanUser").Include("SalesmanUser.Contact")
                          group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName } into g
                               select new ChartingDataClass
                               {
                                   Key = g.Key.SalesmanUserId,
                                   SalesmanUserName = g.Key.EnglishName,
                                   Value = args.SelectedCurrency == "1" ? g.Sum(s => s.EstimatedProfitInLocal) : g.Sum(s => s.EstimatedProfitInProfit),
                               }).OrderByDescending(s => s.Key).ToList();

                foreach (ChartingDataClass item in items) 
                {
                    if (differnceDays > 365)
                    {
                        item.Label = fromDate.ToString("MMM yy") + " - " + toDate.ToString("MMM yy") + " ";
                    }
                    else
                    {
                        item.Label = fromDate.ToString("dd MMM") + " - " + toDate.ToString("dd MMM") + " ";
                    }
                }
                fromDate = toDate.AddDays(1);
                toDate = fromDate.AddDays(period);
                if (toDate > args.ToDate)
                {
                    toDate = (DateTime)args.ToDate;
                }
                output = output.Concat(items).OrderByDescending(x => x.Key).ToList();
            }
            return output;
        }
      
        private List<ChartingDataClass> GetChartItemsByQuarter(IQueryable<Quote> dataSourceQuery)
        {
            List<ChartingDataClass> output = GetChartItemsByMonth(dataSourceQuery);
            Dictionary<string, List<ChartingDataClass>> itemsGroupedBySalesman = GroupBySalesman(output);
            List<ChartingDataClass> result = new List<ChartingDataClass>();
            foreach (KeyValuePair<string, List<ChartingDataClass>> entry in itemsGroupedBySalesman)
            {
                result = result.Concat(GroupByQuarter(entry.Value, entry.Key)).ToList();
            }

            return result;
        }

        private List<ChartingDataClass> GroupByQuarter(List<ChartingDataClass> list, string SalemanId)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            Dictionary<int, List<ChartingDataClass>> map = new Dictionary<int, List<ChartingDataClass>>();
            int currentMonth = todayDate.Month;
            int index = 11;
            int quarterNumber = 4;
            ChartingDataClass item;
            List<ChartingDataClass> referenceList;
            int month;
            for (int i = 0; i < 12; i++)
            {
                month = currentMonth - i;
                if (month <= 0)
                {
                    month += 12;
                }
                item = list.Where(s => s.Label.Equals(month.ToString())).FirstOrDefault();
                if (item != null)
                {
                    if (index >= 0)
                    {
                        if (!map.TryGetValue(quarterNumber, out referenceList))
                        {
                            map.Add(quarterNumber, new List<ChartingDataClass>());
                        }
                        map.TryGetValue(quarterNumber, out referenceList);
                        referenceList.Add(item);
                    }
                }
                if (index % 3 == 0)
                {
                    quarterNumber--;
                }
                index--;
            }

            List<ChartingDataClass> output = new List<ChartingDataClass>();
            DateTime date1;
            DateTime date2;
            string salesmanName;
            salesmanNames.TryGetValue(SalemanId, out salesmanName);
            foreach (KeyValuePair<int, List<ChartingDataClass>> entry in map)
            {
                int differenceMonths = ((entry.Key - 1) * 3) - 11;
                date1 = todayDate.AddMonths(differenceMonths);
                date2 = date1.AddMonths(2);

                output.Add(new ChartingDataClass()
                {
                    Key = SalemanId,
                    SalesmanUserName = salesmanName,
                    Label = date1.ToString("MMM") + " - " + date2.ToString("MMM"),
                    Value = entry.Value.Sum(s => s.Value)
                });
            }
            output.Reverse();
            return output;
        }

        private Dictionary<string, List<ChartingDataClass>> GroupBySalesman(List<ChartingDataClass> list)
        {
            List<ChartingDataClass> refrenceList;
            Dictionary<string, List<ChartingDataClass>> output = new Dictionary<string, List<ChartingDataClass>>();
            foreach (ChartingDataClass item in list)
            {
                if (!output.TryGetValue(item.Key, out refrenceList))
                {
                    output.Add(item.Key, new List<ChartingDataClass>());
                }
                output.TryGetValue(item.Key, out refrenceList);
                refrenceList.Add(item);
            }
            return output;
        }

        private List<ChartingDataClass> GetChartItemsByWeek(IQueryable<Quote> dataSourceQuery)
        {
            List<ChartingDataClass> output = GetChartItemsByDay(dataSourceQuery);
            Dictionary<string, List<ChartingDataClass>> itemsGroupedBySalesman = GroupBySalesman(output);
            List<ChartingDataClass> result = new List<ChartingDataClass>();

            foreach (KeyValuePair<string, List<ChartingDataClass>> entry in itemsGroupedBySalesman)
            {
                result = result.Concat(GroupByWeek(entry.Value, entry.Key)).ToList();
            }

            return result;
        }

        private IEnumerable<ChartingDataClass> GroupByWeek(List<ChartingDataClass> list, string salemanId)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime fromDate = todayDate.AddMonths(-1).AddDays(1);
            DateTime tempDate;
            DateTime toDate = fromDate.AddDays(7);
            List<string> days = new List<string>();
            List<ChartingDataClass> items = new List<ChartingDataClass>();
            List<ChartingDataClass> output = new List<ChartingDataClass>();
            int i = 1;
            while (fromDate < todayDate)
            {
                if (toDate > todayDate)
                {
                    toDate = todayDate;
                }
                tempDate = fromDate;
                while (fromDate <= toDate)
                {
                    days.Add(fromDate.Day.ToString());
                    fromDate = fromDate.AddDays(1);
                }

                items = list.Where(x => days.Contains(x.Label)).ToList();
                string salesmanName;
                salesmanNames.TryGetValue(salemanId, out salesmanName);
                output.Add(new ChartingDataClass()
                {
                    Key = salemanId,
                    SalesmanUserName = salesmanName,
                    Label = tempDate.Day.ToString() + "/" + tempDate.Month.ToString() + " - " + toDate.Day.ToString() + "/" + toDate.Month.ToString(),
                    Value = items.Sum(x => x.Value)
                });
                i++;
                fromDate = toDate.AddDays(1);
                toDate = toDate.AddDays(7);
                days.Clear();
            }
            return output;
        }

        private List<ChartingDataClass> GetChartItemsByMonth(IQueryable<Quote> dataSourceQuery)
        {
            List<ChartingDataClass> output = (from d in dataSourceQuery.Include("SalesmanUser").Include("SalesmanUser.Contact")
                                              group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName, d.OpenDate.Year, d.OpenDate.Month } into g
                                              select new ChartingDataClass
                                              {
                                                  Key = g.Key.SalesmanUserId,
                                                  SalesmanUserName = g.Key.EnglishName,
                                                  Value = args.SelectedCurrency == "1" ? g.Sum(s => s.EstimatedProfitInLocal) : g.Sum(s => s.EstimatedProfitInProfit),
                                                  Label = (g.Key.Month).ToString()
                                              }).OrderByDescending(s => s.Key).ToList();
            output = ConvertLabel(output);

            return output;
        }

        private List<ChartingDataClass> GetChartItemsByDay(IQueryable<Quote> dataSourceQuery)
        {
            List<ChartingDataClass> output = (from d in dataSourceQuery.Include("SalesmanUser").Include("SalesmanUser.Contact")
                                              group d by new { d.SalesmanUserId, d.SalesmanUser.Contact.EnglishName, d.OpenDate.Year, d.OpenDate.Month, d.OpenDate.Day } into g
                                              select new ChartingDataClass
                                              {
                                                  Key = g.Key.SalesmanUserId,
                                                  SalesmanUserName = g.Key.EnglishName,
                                                  Value = args.SelectedCurrency == "1" ? g.Sum(s => s.EstimatedProfitInLocal) : g.Sum(s => s.EstimatedProfitInProfit),
                                                  Label = g.Key.Day.ToString(),
                                              }).OrderByDescending(s => s.Key).ToList();
            output = ConvertLabel(output);

            return output;
        }

        private List<ChartingDataClass> ConvertLabel(List<ChartingDataClass> list)
        {
            DateTime today = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            switch (args.DatesCode)
            {
                case "0":
                    {
                        return SetDay(list, today.DayOfWeek.ToString());
                    }
                case "-1":
                    {

                        return SetDay(list, today.AddDays(-1).DayOfWeek.ToString());
                    }
                case "-7":
                    {
                        return SetWeekDays(list, today);
                    }
                case "-30":
                    {
                        return list;
                    }
                case "-90":
                    {
                        return SetMonths(list, today);
                    }
            }
            return list;
        }

        private List<ChartingDataClass> SetMonths(List<ChartingDataClass> list, DateTime today)
        {
            int differenceMonths;
            foreach (ChartingDataClass item in list)
            {
                differenceMonths = Int32.Parse(item.Label) - today.Month;
                item.Label = today.AddMonths(differenceMonths).ToString("MMM");
            }
            return list;
        }

        private List<ChartingDataClass> SetWeekDays(List<ChartingDataClass> list, DateTime today)
        {
            int differenceDays;
            foreach (ChartingDataClass item in list)
            {
                differenceDays = Int32.Parse(item.Label) - today.Day;
                item.Label = today.AddDays(differenceDays).DayOfWeek.ToString();
            }
            return list;
        }

        private List<ChartingDataClass> SetDay(List<ChartingDataClass> list, string day)
        {
            foreach (ChartingDataClass item in list)
            {
                item.Label = day;
            }
            return list;
        }
        private IQueryable<Quote> FilterPeriod(IQueryable<Quote> dataSourceQuery, DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate != null && toDate != null)
            {
                dataSourceQuery = dataSourceQuery.Where(d => DbFunctions.TruncateTime(d.OpenDate) >= fromDate &&
                                                             DbFunctions.TruncateTime(d.OpenDate) <= toDate);
            }
            return dataSourceQuery;
        }
    }

    public class DateLabelBuilder
    {
        public static List<string> GetDays(DateTime date1, DateTime date2)
        {
            List<string> output = new List<string>();

            while (date1 <= date2)
            {
                output.Add(date1.ToString("ddd"));

                date1 = date1.AddDays(1);
            }

            return output;
        }

        public static List<string> GetWeeks(DateTime date1, DateTime date2)
        {
            List<string> output = new List<string>();

            while (date1 <= date2)
            {
                DateTime stepDate = date1.AddDays(7);

                if (stepDate > date2)
                {
                    stepDate = date2;
                }

                output.Add(date1.Day.ToString() + "/" + date1.Month.ToString() + " - " + stepDate.Day.ToString() + "/" + stepDate.Month.ToString());

                date1 = stepDate.AddDays(1);
            }

            return output;
        }

        public static List<string> GetMonths(DateTime date1, DateTime date2)
        {
            List<string> output = new List<string>();

            while (date1 <= date2)
            {
                DateTime stepDate = date1.AddMonths(1);

                if (stepDate > date2)
                {
                    stepDate = date2;
                }

                output.Add(date1.Day.ToString() + "/" + date1.Month.ToString() + " - " + stepDate.Day.ToString() + "/" + stepDate.Month.ToString());

                date1 = stepDate.AddDays(1);
            }

            return output;
        }

        public static List<string> GetQuarters(DateTime date1, DateTime date2)
        {
            List<string> output = new List<string>();

            while (date1 <= date2)
            {
                DateTime stepDate = date1.AddMonths(3);

                if (stepDate > date2)
                {
                    stepDate = date2;
                }

                output.Add(date1.Day.ToString() + "/" + date1.Month.ToString() + " - " + stepDate.Day.ToString() + "/" + stepDate.Month.ToString());

                date1 = stepDate.AddDays(1);
            }

            return output;
        }

        public static List<string> GetYears(DateTime date1, DateTime date2)
        {
            List<string> output = new List<string>();

            while (date1 <= date2)
            {
                DateTime stepDate = date1.AddYears(1);

                if (stepDate > date2)
                {
                    stepDate = date2;
                }

                output.Add(date1.Day.ToString() + "/" + date1.Month.ToString() + date1.Year.ToString() + " - " + stepDate.Day.ToString() + "/" + stepDate.Month.ToString() + stepDate.Year.ToString());

                date1 = stepDate.AddDays(1);
            }

            return output;
        }
    }
}

