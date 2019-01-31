using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class SharedLogisticsSummary
    {
        [Key]
        public int Id { get; set; }

        public int TodayCustomersCount { get; set; }
        public int LastWeekCustomersCount { get; set; }
        public int LastMonthCustomersCount { get; set; }
        public int TodayAgentsCount { get; set; }
        public int LastWeekAgentsCount { get; set; }
        public int LastMonthAgentsCount { get; set; }
    }

    public class SharedLogisticsCardLog
    {
        [Key]
        public int Id { get; set; }

        public DateTime LogDateTime { get; set; }
        public string CardId { get; set; }
    }

    public class CardLogDetails
    {
        [Key]
        public int Id { get; set; }

        public string CardId { get; set; }
        public string CardName { get; set; }
        public int NumberOfActivities { get; set; }
        public string ContactId { get; set; }
        public string ContactName { get; set; }

        private List<CardLogActivityDetails> details;
        public List<CardLogActivityDetails> CardLogActivityDetails
        {
            get
            {
                if (details == null)
                {
                    details = new List<CardLogActivityDetails>();
                }

                return details;
            }

            set
            {
                if (value != null)
                {
                    details = value;
                }
            }
        }
    }

    public class CardLogActivityDetails
    {
        [Key]
        public int Id { get; set; }

        public string Module { get; set; }
        public string Activity { get; set; }
        public string CardId { get; set; }
        public string PartnerTypeId { get; set; }
        public string ContactId { get; set; }
        public DateTime? GMTLogDateTime { get; set; }
    }

    public class LastLoginPartners
    {
        [Key]
        public int Id { get; set; }

        public string CardId { get; set; }
        public string CardName { get; set; }
        public DateTime? LastAccess { get; set; }
        public string PartnerTypeName { get; set; }
        public string ContactId { get; set; }
        public string ContactName { get; set; }
        public string Via { get; set; }
    }
}