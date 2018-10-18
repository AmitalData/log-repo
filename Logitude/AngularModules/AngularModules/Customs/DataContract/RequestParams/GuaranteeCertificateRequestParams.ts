import {GenericRequestParams} from './GenericRequestParams';

export class GuaranteeCertificateRequestParams extends GenericRequestParams {
    public certificateID : number;
    public certificateIDSpecified: boolean;
    public guaranteeCertificateType : number;
    public guaranteeCertificateTypeSpecified: boolean;
    public guaranteeExternalCertificateNumber: string;
    public guarantorID : number;
    public guarantorIDSpecified: boolean;
}