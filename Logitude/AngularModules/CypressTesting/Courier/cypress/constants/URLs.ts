export class URLs {
    public static readonly Activity = "**/activities"
    public static readonly PutCompleteActivity = "**/PutCompleteActivity"
    public static readonly ActivityGetSingle = "**/activities/getsingle?**"
    public static readonly GetCompleteActivity = "**/CRMDomain/GetCompleteActivity?**"
    public static readonly GetTodayActivity = "**/activityviews/getbyfilters?**"

    public static readonly Opportunities = "**/opportunities"
    public static readonly OpportunitiesGetSingle = "**/opportunities/getsingle?**"
    public static readonly GetQuotesByOpportunityId = "**/QuoteDomain/GetQuotesByOpportunityId?**"

    public static GetFilterSearch(filterBy: string) {
        return '**/getbyfilters?**' + filterBy + '**'
    }

    public static readonly qaIndicatorUID = "**/shipment/getUserIdDetailsByShipmentSecurityKeyWithoutToken?key=d5e6d15f4cb24f12a8ac9c5e8c54a06d?**"
    public static readonly qaIndicatorPREQ = "**shipment/GetSingleBySecurityKeyWithoutToken?key=d5e6d15f4cb24f12a8ac9c5e8c54a06d**"
}


