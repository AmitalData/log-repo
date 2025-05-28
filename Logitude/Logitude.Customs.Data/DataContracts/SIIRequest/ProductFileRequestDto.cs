using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts.SIIRequest
{
    public class ProductFileRequestDto
    {
        public CredentialsDto credentials { get; set; }
        public string importerNumber { get; set; }
        public string modelCode { get; set; }
        public string originCountry { get; set; }
    }
}
