import { RequestParamsBase } from './RequestParamsBase';

export class CreditQueryRequestParams extends RequestParamsBase {

    public AgentID: string;
    public AgentExternalId: string;
    public ExtertnalID: string;
    public DateFrom: Date;
    public DateTo: Date;
}