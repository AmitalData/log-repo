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
        ChartData chartData;
        IQueryable<Quote> iQueryable;
        List<QuoteDataItem> data;
        QuoteDashboardArguments args;       
        public TopFiveSalesmanQuery(IQueryable<Quote> iQueryableQuotes, QuoteDashboardArguments args, int tenant)
        {
            this.args = args;
            this.tenant = tenant;
            this.chartData = new ChartData();

            iQueryable = (from d in iQueryableQuotes 
                               where d.QuoteTypeCode == "A"
                               && d.EstimateProfit != null
                               && d.EstimateProfit != 0
                               select d);

            BuildChartKeys();

            if (chartData.Keys.Count > 0)
            {
                this.data = (from d in iQueryable
                                  where chartData.Keys.Contains(d.SalesmanUserId)
                                  select new QuoteDataItem()
                                  {
                                      Id = d.Id,
                                      SalesmanId = d.SalesmanUserId,
                                      OpenDate = d.OpenDate,
                                      AmountInLocal = d.EstimatedProfitInLocal,
                                      AmountInProfit = d.EstimatedProfitInProfit
                                  }).ToList();

                BuildChartData();                
            }
        }

        public List<ChartingDataClass> GetChartData()
        {
            List<ChartingDataClass> output = new List<ChartingDataClass>();

            if (chartData.Keys.Count > 0)
            {
                foreach (ChartItem item in chartData.Items)
                {
                    output.Add(new ChartingDataClass()
                    {
                        Key = item.Id,
                        SalesmanUserName = item.Name,
                        Label = item.Label,
                        ProfitInLocal = item.AmountInLocal,
                        ProfitInProfit = item.AmountInProfit,
                    });
                }
            }

            return output;
        }

        private void BuildChartKeys()
        {
            var topSalesman = (from d in iQueryable
                               group d by d.SalesmanUserId into g
                               select new
                               {
                                   Id = g.Key,
                                   Profit = g.Sum(s => s.EstimateProfit),
                               }).OrderByDescending(o => o.Profit).Take(5).ToList();

            chartData.Keys = topSalesman.Select(s => s.Id).ToList();

            if (chartData.Keys.Count > 0)
            {
                UserQuery userQuery = new UserQuery();
                chartData.Users = userQuery.GetUsersListFromIdList(chartData.Keys, tenant);

                if (chartData.Users.Count == 0)
                {
                    foreach (string item in chartData.Keys)
                    {
                        chartData.Users.Add(item, item);
                    }
                }
            }
        }
        private void BuildChartData()
        {
            ChartDataBuilder builder = new ChartDataBuilder(chartData, data);

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            switch (args.DatesCode)
            {
                case "0":
                    {
                        builder.GetDaysData(todayDate, todayDate);
                        break;
                    }

                case "-1":
                    {
                        builder.GetDaysData(todayDate.AddDays(-1), todayDate.AddDays(-1));
                        break;
                    }

                case "-7":
                    {
                        builder.GetDaysData(todayDate.AddDays(-6), todayDate);
                        break;
                    }

                case "-30":
                    {
                        builder.GetWeeksData(todayDate.AddMonths(-1), todayDate);
                        break;
                    }

                case "-90":
                    {
                        builder.GetMonthsData(todayDate.AddMonths(-3), todayDate);
                        break;
                    }

                case "-365":
                    {
                        builder.GetQuartersData(todayDate.AddYears(-1), todayDate);
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
                                builder.GetDaysData(date1, date2);
                            }

                            else if (days <= 30)
                            {
                                builder.GetWeeksData(date1, date2);
                            }

                            else if (days <= 90)
                            {
                                builder.GetMonthsData(date1, date2);
                            }

                            else if (days <= 365)
                            {
                                builder.GetQuartersData(date1, date2);
                            }

                            else
                            {
                                builder.GetYearsData(date1, date2);
                            }
                        }

                        break;
                    }
            }            
        }
    }

    public class QuoteDataItem
    {
        public string Id { get; set; }
        public string SalesmanId { get; set; }
        public DateTime OpenDate { get; set; }
        public double? AmountInLocal { get; set; }
        public double? AmountInProfit { get; set; }
    }

    public class ChartData
    {
        public List<string> Keys { get; set; }
        public List<string> Labels { get; set; }
        public List<ChartItem> Items { get; set; }
        public Dictionary<string, string> Users { get; set; }
        public ChartData()
        {
            this.Keys = new List<string>();
            this.Labels = new List<string>();
            this.Items = new List<ChartItem>();
            this.Users = new Dictionary<string, string>();
        }
    }

    public class ChartItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public double AmountInLocal { get; set; }
        public double AmountInProfit { get; set; }
        public ChartItem()
        {
            AmountInLocal = 0;
            AmountInProfit = 0;
        }
    }

    public class ChartDataBuilder
    {
        private ChartData output;
        private List<QuoteDataItem> data;        
        public ChartDataBuilder(ChartData output, List<QuoteDataItem> data)
        {
            this.data = data;
            this.output = output;
        }

        public void GetDaysData(DateTime date1, DateTime date2)
        {
            while (date1 <= date2)
            {
                string label = date1.ToString("ddd");

                output.Labels.Add(label);

                BuildItems(date1, date1, label);

                date1 = date1.AddDays(1);
            }
        }

        public void GetWeeksData(DateTime date1, DateTime date2)
        {
            while (date1 <= date2)
            {
                DateTime stepDate = date1.AddDays(7);

                if (stepDate > date2)
                {
                    stepDate = date2;
                }

                string label = date1.Day.ToString() + "/" + date1.Month.ToString() + " - " + stepDate.Day.ToString() + "/" + stepDate.Month.ToString();

                output.Labels.Add(label);

                BuildItems(date1, stepDate, label);

                date1 = stepDate.AddDays(1);
            }
        }

        public void GetMonthsData(DateTime date1, DateTime date2)
        {
            while (date1 <= date2)
            {
                DateTime stepDate = date1.AddMonths(1);

                if (stepDate > date2)
                {
                    stepDate = date2;
                }

                string label = date1.Day.ToString() + "/" + date1.Month.ToString() + " - " + stepDate.Day.ToString() + "/" + stepDate.Month.ToString();

                output.Labels.Add(label);

                BuildItems(date1, stepDate, label);

                date1 = stepDate.AddDays(1);
            }
        }

        public void GetQuartersData(DateTime date1, DateTime date2)
        {
            while (date1 <= date2)
            {
                DateTime stepDate = date1.AddMonths(3);

                if (stepDate > date2)
                {
                    stepDate = date2;
                }

                string label = date1.Day.ToString() + "/" + date1.Month.ToString() + " - " + stepDate.Day.ToString() + "/" + stepDate.Month.ToString();

                output.Labels.Add(label);

                BuildItems(date1, stepDate, label);

                date1 = stepDate.AddDays(1);
            }
        }

        public void GetYearsData(DateTime date1, DateTime date2)
        {
            while (date1 <= date2)
            {
                DateTime stepDate = date1.AddYears(1);

                if (stepDate > date2)
                {
                    stepDate = date2;
                }

                string label = date1.Day.ToString() + "/" + date1.Month.ToString() + date1.Year.ToString() + " - " + stepDate.Day.ToString() + "/" + stepDate.Month.ToString() + stepDate.Year.ToString();

                output.Labels.Add(label);

                BuildItems(date1, stepDate, label);

                date1 = stepDate.AddDays(1);
            }
        }

        private void BuildItems(DateTime date1, DateTime date2, string label)
        {
            var items = (from d in data
                         where d.OpenDate.Date >= date1 && d.OpenDate.Date <= date2
                         group d by d.SalesmanId into g
                         select new
                         {
                             Key = g.Key,
                             AmountInLocal = g.Sum(s => s.AmountInLocal),
                             AmountInProfit = g.Sum(s => s.AmountInProfit),
                         }).ToList();

            foreach (var user in output.Users)
            {
                ChartItem itemChart = new ChartItem()
                {
                    Id = user.Key,
                    Name = user.Value,
                    Label = label,
                };

                var item = items.Where(d => d.Key == user.Key).FirstOrDefault();
                if(item != null)
                {
                    itemChart.AmountInLocal = item.AmountInLocal == null ? 0 : MethodHelper.Round(item.AmountInLocal, 2).Value;
                    itemChart.AmountInProfit = item.AmountInProfit == null ? 0 : MethodHelper.Round(item.AmountInProfit, 2).Value;
                }

                output.Items.Add(itemChart);
            }
        }
    }
}

