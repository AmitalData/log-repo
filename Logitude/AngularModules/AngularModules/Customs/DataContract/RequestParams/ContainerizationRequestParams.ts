import { RequestParamsBase } from './RequestParamsBase';

export class ContainerizationRequestParams extends RequestParamsBase {
    public ContainerizationList: Array<ContainerizationDetails>;
}

export class ContainerizationDetails {
    public DeclarationList: Array<string>;
    public CargoTypeCode: string;
    public ManifestNumber: string;
    public SecondCargoId: string;
    public ThirdCargoId: string;
}

