using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class TraceEventList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public string EventTypeId { get; set; }
        public DateTime? EventDateTime { get; set; }
        public DateTime LogDateTime { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
        public string EventTypeGroupCode { get; set; }
        public string EventTypeEnglishName { get; set; }
        public string ContactEnglishFirstName { get; set; }
        public bool Deleted { get; set; }
        public bool ShortView { get; set; }
        public string CustomerCareUserEmail { get; set; }
        public string Location { get; set; }
        public string PartnerName { get; set; }
    }
}