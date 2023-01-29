import { RequestParamsBase } from './RequestParamsBase';

export class CustomsItemDetailsQueryRequestParams extends RequestParamsBase {

    public Classification?: string;
    public CustomsBookType?: number;
    public ValidToDate: Date;

}
