import { RequestParamsBase } from './RequestParamsBase';

export class CustomItemLegalDemandsQueryRequestParams extends RequestParamsBase {

    public ValidToDate?: Date;
    public ClassificationCode: string;
    public CustomsBookType: string;

}
