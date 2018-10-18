using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerCompetitorList
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string CompetitorId { get; set; }

        public int Tenant { get; set; }

        public string CustomerName { get; set; }
        public string CompetitorName { get; set; }
    }
}