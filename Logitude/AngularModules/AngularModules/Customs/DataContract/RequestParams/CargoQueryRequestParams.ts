 import {GenericRequestParams} from './GenericRequestParams';

export class CargoQueryRequestParams extends GenericRequestParams {

    public CustomsFile: string;
    public DeclarationNumber: string;
    public CargoTypeCode: string;
    public ManifestNumber: string;
    public SecondCargoID: string;
    public ThirdCargoID: string;

    // added by Alaa
    public DeclarationId: string;
    public ConsignmentNumber?: number;
    public AutoSend: Boolean;

}