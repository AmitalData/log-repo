export class Urls {
    public static readonly Activity = "**/activities"
    public static readonly PutCompleteActivity = "**/PutCompleteActivity"
    public static readonly ActivityGetSingle = "**/activities/getsingle?**"
    public static readonly GetCompleteActivity = "**/CRMDomain/GetCompleteActivity?**"

    public static readonly Opportunities = "**/opportunities"
    public static readonly OpportunitiesGetSingle = "**/opportunities/getsingle?**"
    public static readonly GetQuotesByOpportunityId = "**/QuoteDomain/GetQuotesByOpportunityId?**"

    public static GetFilterSearch(filterBy: string) {
        return '**/getbyfilters?**' + filterBy + '**'
    }
}