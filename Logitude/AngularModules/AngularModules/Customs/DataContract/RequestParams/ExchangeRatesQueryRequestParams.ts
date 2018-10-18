import { RequestParamsBase } from './RequestParamsBase';

export class ExchangeRatesQueryRequestParams extends RequestParamsBase {

    public FromDate?: Date;
    public ToDate?: Date;
    public CurrencyTypeId: string;

}
