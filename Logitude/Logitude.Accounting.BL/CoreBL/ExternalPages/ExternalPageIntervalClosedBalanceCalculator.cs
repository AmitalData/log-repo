using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalPages
{
    public abstract class ExternalPageIntervalClosedBalanceCalculator
    {
        protected int tenant;
        protected DateTime date;
        protected List<ReconcileExternalPage> externalPages;

        public ExternalPageIntervalClosedBalanceCalculator(DateTime date, List<ReconcileExternalPage> externalPages, int tenant)
        {
            this.date = date;
            this.externalPages = externalPages;
            this.tenant = tenant;
        }
        public abstract decimal CalculateClosingBalance();
    }

    

}
