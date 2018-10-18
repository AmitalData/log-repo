import {GenericRequestParams} from './GenericRequestParams';

export class PrintRequestRequestParams extends GenericRequestParams {

    public IsSearchByDeclarationRadio: boolean;
    public DeclarationNumber: Array<string>;

    public IsSearchByCargoRadio: boolean;
    public CargoTypeCode: string;
    public ManifestNumber: string;
    public SecondCargoID: string;
    public ThirdCargoID: string;

    public CustomFileNo: string;
}