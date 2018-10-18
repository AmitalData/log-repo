import {INF_MSG_GenericResponseData} from './INF_MSG_GenericResponseData';

export class ImporterDeclarationResponseData extends INF_MSG_GenericResponseData {

    public PeriodDeclarationList: Array<PeriodDeclarationResult>;
    public LoiDeclarationList: Array<LoiDeclarationResult>;
    public SecurityDeclarationList: Array<SecurityDeclarationResult>;

}

export class PeriodDeclarationResult {

    public PeriodDeclarationID: string;
    public VendorID: string;
    public VendorName: string;
    public CreateDate: string;
    public ValidityFrom: string;
    public ExpirationDate: string;
    public Status: string;
    public StatusName: string;
    public DocumentID: string;
}

export class LoiDeclarationResult {

    public DeclarationID: string;
    public LoiDeclarationID: string;
    public VendorID: string;
    public VendorName: string;
    public CreateDate: string;
    public DocumentID: string;
}

export class SecurityDeclarationResult {

    public SecurityDeclarationType: string;
    public SecurityDeclarationName: string;
    public SecurityImporterDeclarationId: string;
    public DeclarationDate: string;
    public ExpirationDate: string;
    public Status: string;
    public StatusName: string;
    public DocumentID: string;
}

