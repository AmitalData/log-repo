import { RequestParamsBase } from './RequestParamsBase';

export class CargoSealsRequestParams extends RequestParamsBase {

    public UpdateDate?: Date;
    public ContainerNumber: string;
    public CargoRowNumber: string;
    public ImporterNumber: string;
    public CargoIdentifierTypeCode: string;
    public CargoIdentifierKey1: string;
    public CargoIdentifierKey2: string;
    public CargoIdentifierKey3: string;
    public DeclarationNumber: string;
    public DeclarationID: string;
}
