
import { RequestParamsBase } from './RequestParamsBase';

export class CustomsBookInRequestParams extends RequestParamsBase {

    public fromDate?: Date;
    public fromDateSpecified: boolean;
    public isGetHistoricalData: boolean;
    public toDate?: Date;
    public toDateSpecified: boolean;
    

}
