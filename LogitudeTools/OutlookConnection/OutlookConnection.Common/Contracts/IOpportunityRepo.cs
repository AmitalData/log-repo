using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.OpportunityWcfServiceReference;

namespace OutlookConnection.Common.Contracts
{
    public interface IOpportunityRepo
    {
        OpportunityList[] GetOpportunityList(string email, string searchText, int tenant, int skip, int take, OutlookConnection.Common.OpportunityWcfServiceReference.OpportunityApiFilters filters, ref OutlookConnection.Common.OpportunityWcfServiceReference.Response response);

        OutlookConnection.Common.OpportunityWcfServiceReference.CustomerList GetCustomerListByOpportunityId(string opportunityId, int tenant, ref OutlookConnection.Common.OpportunityWcfServiceReference.Response response);

    }
}
