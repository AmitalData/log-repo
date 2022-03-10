import { RequestParamsBase } from './RequestParamsBase';

export class LogisticActionRequestRequestParams extends RequestParamsBase {
    public ExporterIdentifierType: string;
    public ExporterNumber: string;
    public PassportCountry: string;
    public PassportNumber: string;
    public RequestType: string;
    public RequestReason: string;
    public DeliverySiteID: string;
    public CargoIdentifierType: string;
    public CargoIdentifierKey1: string;
    public CargoIdentifierKey2: string;
    public CargoIdentifierKey3: string;
    public PackagingTypeCode: string;
    public Quantity: number;
    public LogisticActionRequestId: string;
    public CustomsFile: string;
}