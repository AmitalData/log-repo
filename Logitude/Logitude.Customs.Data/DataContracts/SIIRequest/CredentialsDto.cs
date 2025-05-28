using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts.SIIRequest
{
    public class CredentialsDto
    {
        public string userId { get; set; }
        public string customerUniqueCode { get; set; }
        public string hashPassword { get; set; }
    }
}
