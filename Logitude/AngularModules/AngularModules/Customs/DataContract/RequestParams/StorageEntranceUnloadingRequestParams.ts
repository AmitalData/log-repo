import { RequestParamsBase } from './RequestParamsBase';

export class StorageEntranceUnloadingRequestParams extends RequestParamsBase {

    public DeclarationId: string;
    public DeclarationNumber: string;
    public ConsignmentNumber: string;
    public ManifestNumber: string;
    public LineNumber: string;
    public EntryDate: Date;
    public Quantity: string;
    public GrossWeight: string;
    public PackageTypeCode: string;

}