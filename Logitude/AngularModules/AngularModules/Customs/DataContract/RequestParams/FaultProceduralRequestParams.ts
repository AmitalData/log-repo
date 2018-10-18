import { RequestParamsBase } from './RequestParamsBase';

export class FaultProceduralRequestParams extends RequestParamsBase {

    public Client: string;
    public ImporterExternalID: string;
    public CustomsFile: string;
    public DeclarationNumber: string;
    public ProceduralFaultCode: string;
    public StartDate?: Date;
    public EndDate?: Date;
    public AgentExternalID: string;

    public ImporterId: string;
    public ImporterCode: string;
    public ImporterName: string;
    public PredefinedValue: string;
    public CustomerId: string;

}