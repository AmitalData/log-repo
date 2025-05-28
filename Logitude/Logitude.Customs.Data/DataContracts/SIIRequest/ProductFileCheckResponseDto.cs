using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts.SIIRequest
{
    public class ProductFileCheckResponseDto
    {
        public List<object> productFiles { get; set; } 
        public int responseCode { get; set; }
    }
}
