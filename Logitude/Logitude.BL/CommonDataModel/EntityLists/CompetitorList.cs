using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CompetitorList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
        public string Opportunity { get; set; }
        public string Threat { get; set; }
        public string AddressId { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
    }
}