using Logitude.BL.InfrastructureModel.EntityLists;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TruckerSettingList 
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SearchFields { get; set; }
        public string AddressId { get; set; }
        public string FromAddressCityId { get; set; }
        public string ToAddressCityId { get; set; }
        public string ShipmentType { get; set; }
        public string TruckerId { get; set; }
        public string Responsibility { get; set; }
        public string ToAddressCityName { get; set; }
        public string FromAddressCityName { get; set; }
        public string TruckerName { get; set; }
        public string ResponsibilityName { get; set; }
        public string ShipmentTypeName { get; set; }

       

    }
}