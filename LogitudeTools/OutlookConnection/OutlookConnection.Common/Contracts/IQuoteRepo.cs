using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutlookConnection.Common.Contracts
{
    public interface IQuoteRepo
    {
        OutlookConnection.Common.QuoteWcfServiceReference.QuoteList[] GetQuoteList(OutlookConnection.Common.QuoteWcfServiceReference.QuoteApiFilters filters, int tenant, ref OutlookConnection.Common.QuoteWcfServiceReference.Response response);
    }
}
