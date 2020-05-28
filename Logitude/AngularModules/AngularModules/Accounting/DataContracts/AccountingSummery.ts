export class GLAccountSummary {
    Id : number;
    ActiveGLAccountCount : any;
    InactiveGLAccountCount : any;
    AllGLAccountCount : any;
    OpenFilesCount: any;
    OpenMastersCount: any;
    ClosedFilesGLAccountCount : any;
    AllFilesCount : any;
    AllJobsCount : any;
    
    // Customers
    ActiveCustomersCount : any;
    InactiveCustomersCount : any;
    CollectorsCount : any;
    DebitorsCount: any;
    AllCustomersCount: any;

    // Vendors
    ActiveVendorsCount : any;
    InactiveVendorsCount : any;
    //CollectorsCount : number;
    //DebitorsCount : number;
    AllVendorsCount: any;
}

export class JournalSummary {
    Id : number;
    AllJournalsCount : any;
    ApprovedJournalsCount : any;
    WaitingJournalsCount : any;
    VoidedJournalsCount : any;
    DraftJournalsCount : any;
}

export class BankAccountSummary {
    Id: number;
    AllBankAccountsCount: any;
}
export class PaymentChequeSummary {
    Id: number;
    AllPaymentChequesCount: any;
}
export class BankDepositSummary {
    Id: number;
    TodaysDepositCount: any;

}

export class CashBookSummary {
    Id: number;
    CashCashbookCount: any;
    ChequeCashbookCount: any;
    AllCashbookCount: any;
}
