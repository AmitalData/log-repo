

import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';

export class ReportFliter {
    CurrentCurrencyCodeType: string;
    DateType: string;
    IncludeOperationalyClosed: boolean;
    ReportCode: string;
    FilterControlName: string
    ReportDocumentId: string;
    CustomerId: string
    QuoteCustomerTypeCode: string;
    FieldDataType: string;
    Tenant: number;
    QueryFilterItemLists: QueryFilterItem[];
    NumberOfPage: number;
    ProcessType: string;
    ReportKey: string;
    ReportName: string;
    InvoiceType: string;
    DefaultTemplateId: string;
    DefaultTemplateVsersion: number;
    ReportsRunUsingWR: boolean = false;
    UserId: string;
    ReportId: string;
    NumberOfRequests: number;
    Level: string;
    DisablePreview: boolean;
    NotDisplayInMenu : boolean;

    constructor() {

    }
}


