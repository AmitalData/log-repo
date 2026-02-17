export class GLAccountSummary {
    Id : number;
    ActiveGLAccountCount : number;
    InactiveGLAccountCount : number;
    AllGLAccountCount : number;
    OpenFilesCount: number;
    OpenMastersCount: number;
    ClosedFilesGLAccountCount : number;
    AllFilesCount : number;
    AllJobsCount : number;
    
    // Customers
    ActiveCustomersCount : number;
    InactiveCustomersCount : number;
    CollectorsCount : number;
    DebitorsCount: number;
    AllCustomersCount: number;

    // Vendors
    ActiveVendorsCount : number;
    InactiveVendorsCount : number;
    //CollectorsCount : number;
    //DebitorsCount : number;
    AllVendorsCount: number;
}

export class JournalSummary {
    Id : number;
    AllJournalsCount : number;
    ApprovedJournalsCount : number;
    WaitingJournalsCount : number;
    VoidedJournalsCount : number;
    DraftJournalsCount : number;
}

export class BankAccountSummary {
    Id: number;
    AllBankAccountsCount: number;
}
export class PaymentChequeSummary {
    Id: number;
    AllPaymenChequesCount: number;
}
export class BankDepositSummary {
    Id: number;
    TodaysDepositCount: number;

}

export class CashBookSummary {
    Id: number;
    CashCashbookCount: number;
    ChequeCashbookCount: number;
    AllCashbookCount: number;
}
