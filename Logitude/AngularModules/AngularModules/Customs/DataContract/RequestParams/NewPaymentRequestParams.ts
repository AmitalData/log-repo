import {GenericRequestParams} from './GenericRequestParams';

export class NewPaymentRequestParams extends GenericRequestParams {

    public PaymentNumber: string;
    public ExternalId: string;

    public FirstEntityID: string;
    public SecondEntityID: string;
    public ThirdEntityID: string;
    public RequestParamsVersion: number;

}