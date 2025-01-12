using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class EntityCasualData
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CasualTypeCode { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string SearchFields { get; set; }

    }
}
