import {GenericRequestParams} from './GenericRequestParams';

export class SendALLDelayFormParams extends GenericRequestParams {

    public CourierMasterId: string;
    public HAWB: string;
    public CourierDeclarationStatusCode: string;
    public Declarations: string[];

}
