using System.Collections.Generic;

namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class DigitalTextCodeUpdateModel
    {
        public string ObjectTableId { get; set; }
        public string CardId { get; set; }
        public int Tenant { get; set; }
        public string ProfileId { get; set; }
        public string ProfileCode { get; set; }
        public string LanguageCode { get; set; }
        public List<DigitalTextCodeUpdateObject> Lables { get; set; }
    }
}