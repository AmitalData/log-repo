using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagementTests.Models
{
    public class TMProjectPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public object Description { get; set; }
        public string CustomerId { get; set; }
        public string SearchFields { get; set; }
        public string OwnerId { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string OwnerName { get; set; }
        public string CustomerName { get; set; }
        public bool IsInnerProject { get; set; }
        public bool Inactive { get; set; }
        public string BudgetId { get; set; }
        public string CategoryId { get; set; }
        public bool IsProrated { get; set; }
        public string ExternalProjectNumber { get; set; }
        public string CategoryName { get; set; }
        public bool ExcludeFromProrating { get; set; }
        public string DayOffTypeCode { get; set; }
        public bool BlockedForDataEntry { get; set; }
        public string BudgetName { get; set; }
        public string DayOffTypeName { get; set; }
        public int ChangeSetOp { get; set; }
    }
}
