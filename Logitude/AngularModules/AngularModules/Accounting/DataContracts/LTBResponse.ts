export class LTBResponse {
    EndBalanceForeign: number;
    EndBalanceLocal: number;
    Have1CurrencyIdInPeriod: string;
    MaxCreateAt: Date;
    StartBalanceForeign: number;
    StartBalanceLocal: number;
    TotalRowCount: number;
    SuppressCumulativeDueMultiCurrencyInPeriod: boolean;
    StartBalanceForeignList: BalanceCurrency[];
    EndBalanceForeignList: BalanceCurrency[];
}

export class BalanceCurrency {
    CurrencyId: string;
    CurrencyCode: string;
    CurrencySign: string;
    BalanceForeign: string;
    BalanceLocal: string;
}
