using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class OpportunityMonthlyConversionDataProvider : BaseDataProvider
    {
        [Key]
        public int Id { get; set; }
        public List<MonthItemClass> MonthlyDataList { get; set; }
    }

    public class MonthItemClass
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string StageName { get; set; }
        public decimal? OpportunitiesCount { get; set; }
        public double Percentage { get; set; }
        public string PercentageString { get; set; }
        public int RowIndex { get; set; }
    }
}