import {GenericRequestParams} from './GenericRequestParams';

export class SendPayReadyLowRequestParams extends GenericRequestParams {

    public CourierMasterId: string;
    public HAWB: string;
    public InternalBankId: string;
    public Declarations: string[];
}
