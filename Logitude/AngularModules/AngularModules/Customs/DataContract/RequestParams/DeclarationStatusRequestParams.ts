import {GenericRequestParams} from './GenericRequestParams';

export class DeclarationStatusRequestParams extends GenericRequestParams {

    public CustomFileNo: string;
    public DeclarationNumber: string;

    public CargoTypeCode: string;
    public ManifestNumber: string;
    public SecondCargoID: string;
    public ThirdCargoID: string;

    public OldReshimonNumber: string;

    public DeclarationRadio: boolean;
    public CargoRadio: boolean;
    public OldReshimonRadio: boolean;

    public RequestOrigin: string;
    public DeclarationList: string;
    public CourierMaster: string;
}
