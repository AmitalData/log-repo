using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class SystemData
    {
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public byte[] Signature { get; set; }
        public string  Date { get; set; }
        public string LocalCurrencyId { get; set; }
        public string ContactId { get; set; }

        public string Company { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string IATA { get; set; }

        public string AddressId { get; set; }
        public string VatNumber { get; set; }
    }
}