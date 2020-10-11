import { RequestParamsBase } from './RequestParamsBase';

export class CargoSealsRequestParams extends RequestParamsBase {

    public CargoSealIdentifierId: string;
    public UpdateDate: Date;
    public ContainerNumber: string;
    public CargoRowNumber: string;
    public ImporterNumber: string;
    public CargoIdentifierTypeCode: string;
    public CargoIdentifierKey1: string;
    public CargoIdentifierKey2: string;
    public CargoIdentifierKey3: string;
    public DeclarationNumber: string;
    public DeclarationID: string;
    public CustomFileNo: string;
    public CargoSealList: Array<CargoSealDetails>;
}

export class CargoSealDetails {

    public SealNumber: string;
    public SealTypeCode: string;
    public SealTypeName: string;
    public SealCompletenessStateCode: string;
    public SealCompletenessStateName: string;
    public UpdateReasonCode: string;
    public UpdateReasonName: string;
    public UpdateTypeCode: string;
    public UpdateTypeName: string;
    public Remarks: string;
}
