export class AgingReportParameters {
    Tenant: number;
    AgingForDate: Date;
    NumberOfmonthsbackwards: number;
    VendorCustomerId: string;
    Category1Id: string;
    Category2Id: string;
    Category3Id: string;
    Category4Id: string;
    Category5Id: string;
    CollectorId: string;
    SalesmanId: string;
    IsCustomer: boolean;
    GroupByDate: string;
    ForceUseMonthMethod: boolean = false;
}

