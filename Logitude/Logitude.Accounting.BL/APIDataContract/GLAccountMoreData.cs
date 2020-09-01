using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
   public partial class GLAccountMoreData
    {
        public List<GLAccountTotalsByCurrency> GLAccountTotalsByCurrencies { get; set; }
    }
}
