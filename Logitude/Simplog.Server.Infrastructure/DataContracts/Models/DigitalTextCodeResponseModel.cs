using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class DigitalTextCodeResponseModel
    {
        public string ObjectTableId { get; set; }
        public string CardId { get; set; }
        public int Tenant { get; set; }
        public List<DigitalTextCodeUpdateObject> Lables { get; set; }
    }
}
