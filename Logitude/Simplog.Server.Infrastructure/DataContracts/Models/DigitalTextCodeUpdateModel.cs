using System.Collections.Generic;

namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class DigitalTextCodeUpdateModel
    {
        public string ObjectTableId { get; set; }
        public string CardId { get; set; }
        public int Tenant { get; set; }
        public List<DigitalTextCodeObject> Lables { get; set; }
    }
}