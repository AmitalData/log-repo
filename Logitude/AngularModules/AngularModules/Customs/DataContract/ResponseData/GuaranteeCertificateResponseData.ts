import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class GuaranteeCertificateResponseData extends INF_MSG_GenericResponseData {

    public ResponseStatusXML: string;
    public DeclarationID: string;
    public GeneralDetailsData: GeneralDetails;
    public AllocationList: Array<Allocation>;
    public RequestList: Array<Request>;

}

export class GeneralDetails {

    public certificateAvailableAmount: string;
    public certificateID: number;
    public guaranteeAmount: string;
    public GuaranteeCertificateStatus: number;
    public GuaranteeCertificateStatusName: string;
    public guaranteedId: number;
    public guaranteedName: string;
    public guaranteeExternalCertificateNumebr: string;
    public guaranteeType: number;
    public guaranteeTypeName: string;
    public guaranteeValidityDate: string;
    public totalCertificateAllocation: string;
}

export class Allocation {

    public certificateAllocationAmount: number;
    public displayFileNumber: string;
    public fileNumber: string;
    public fileType: number;
    public fileTypeName: string;
    public Numeral: number;
    public updateDate: Date;
    public validity: Date;
}

export class Request {

    public createTime: Date;
    public displayFileNumber: string;
    public fileNumber: string;
    public guaranteeStatus: number;
    public guaranteeStatusName: string;
    public guarenteeRequestNumber: number;
    public Numeral: number;
    public NumeralSpecified: boolean;
    public requestDescription: string;
    public requestedExecutionValue: number;
    public requestedExecutionValueSpecified: boolean;
    public requestedValidityDate: Date;
    public requestedValidityDateSpecified: boolean;
}