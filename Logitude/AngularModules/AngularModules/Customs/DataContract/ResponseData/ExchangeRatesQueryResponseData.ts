import {ResponseDataBase} from './ResponseDataBase';

export class ExchangeRatesQueryResponseData extends ResponseDataBase {
    public ExchangeRatesQueryList: Array<ExchangeRatesQueryResult>;
}

export class ExchangeRatesQueryResult {
    public StartDate: Date;
    public CurrencyTypeCode: string;
    public CurrencyTypeName: string;
    public CustomsCurrencyRate: Float32Array;
}