import { RequestParamsBase } from './RequestParamsBase';

export class CH_NG_191_MSG2_ChangingTimeRequestParams extends RequestParamsBase {


    public Id: number;
    public DateSearchFrom?: Date;
    public DateSearchTo?: Date;
    public RequestType: string;
    public DateSearchFromSpecified: boolean;
    public DateSearchToSpecified: boolean;
    public QueueDateSpecified: boolean;
    public QueueDate?: Date;
    public PhysicalCheckId: string;
    public BringQueueForwardIndicator: boolean;
    public CheckTypeCode: string;

}