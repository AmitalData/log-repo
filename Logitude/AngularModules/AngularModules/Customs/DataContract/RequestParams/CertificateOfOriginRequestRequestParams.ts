import {GenericRequestParams} from './GenericRequestParams';
import { RequestParamsBase } from './RequestParamsBase';

export class CertificateOfOriginRequestRequestParams extends GenericRequestParams {
   
    public CertificateOfOriginId: string;
    public DeclarationId: string;
    public CustomFileNo: string;
    public RequestReasonCode: number;
}
