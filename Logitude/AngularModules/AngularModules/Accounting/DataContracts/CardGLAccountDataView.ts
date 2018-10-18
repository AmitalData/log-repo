export class CardGLAccountDataView {
    // GLAccount table
    Id: string;
    Tenant: number;
    InternalNumber: string;
    AccountTypeCode: string;
    DisplayNumber: string;
    GLAccountLocalName: string;
    GLAccountEnglishName: string;
    SearchFields: string;
    IsMultiCurrency: boolean;
    CurrencyId: string;
    RevenueExpenseType: string;
    IsControlAccount: boolean;
    ChartOfAccountsId: string;
    Inactive: boolean;
    ChartOfAccountsTypeCode: string;
    ReconcileMethodCode: string;
    ControlAccountId: string;
    AutomaticReconcileId: string;
    PreviousEnglishName: string;
    PreviousEnglishNameChangeDate: Date;
    PreviousLocalName: string;
    PreviousLocalNameChangeDate: Date;
    PreviousNumber: string;
    PreviousNumberChangeDate: Date;
    PreviousChartOfAccountsId: string;
    PreviousChartOfAccountsChangeDate: Date;
    CustomerGLAccountId: string;
    BalanceInLocalCurrency: number;
    RevaluationEnabled: boolean;
    ParentAccountId: string;
    Category1Id: string;
    Category2Id: string;
    Category3Id: string;
    Category4Id: string;
    Category5Id: string;
    IsVATExempt: boolean;

    // glaccount list properties
    AccountTypeName: string;
    CurrencyName: string;
    RevenueExpenseName: string;
    ChartOfAccountsName: string;
    ChartOfAccountsTypeName: string;
    CurrencyCode: string;
    ReconcileMethodName: string;
    ControlAccountName: string;
    ControlAccountNumber: string;
    ActiveStatusName: string;
    AutomaticReconcileName: string;
    CustomerGLAccountName: string;
    CustomerGLAccountNumber: string;
    ParentAccountName: string;
    ParentAccountNumber: string;
    Category1Name: string;
    Category2Name: string;
    Category3Name: string;
    Category4Name: string;
    Category5Name: string;
    LastActivityDate: Date;
    LastActivityTypeName: string;
    LastActivityByUserName: string;

    // Card
    SalesmanUserId: string;
    CollectorId: string;
    CardEnglishName: string;
    CardLocalName: string;
    CardGLAccountId: string;
    VatTypeId: string;
    CountryId: string;
    CountryCode: string;
    CityName: string;
    CountryName: string;
    PaymentTermId: string;
    VatNumber: string;

    // Contacts (SalesMans)
    SalesManEnglishName: string;
    SalesManLocalName: string;

    // Contacts (Collectors)
    CollectorEnglishName: string;
    CollectorLocalName: string;
    
    // ChartOfAccounts
    ChartOfAccountsLocalName: string;
    ChartOfAccountsEnglishName: string;
    ChartOfAccountsCode: string;
}
