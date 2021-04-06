using Logitude.Accounting.BL.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
  public partial class GLAccountChequeDetails
    {
        public List<Cheque> GLaccountCheques { get; set; }
        public List<LedgerTransaction> ExternalTransactions { get; set; }

    }
}
