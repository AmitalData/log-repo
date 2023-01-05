import { RequestParamsBase } from './RequestParamsBase';

export class MessageRestoreRequestParams extends RequestParamsBase {

    public CorrelationID: string;
    public FromDate? : Date;
    public ToDate?  : Date;
    
    public InterfaceManagementsCode: string;
    public InterfaceManagementsCodeValue: string;
    
}


export class MessageWaitingRequestParams extends RequestParamsBase {

    public CorrelationID: string;
    public FromDate? : Date;
    public ToDate?  : Date;
    
    public ServiceNameCode: string;
    public ServiceName: string;
    
}
