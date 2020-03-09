import {Guid} from '../../../Infrastructure/Utilities/Guid';

export enum SendRequestVIA {
    Default,// from CustomsMessaging Library
    WebServiceInteractive,
    WebServiceBatch,
    DCABatch
}
export class RequestParamsBase {

    //public readonly _PBId: string;
    //public readonly _IsAngularClient: boolean;

    //private _PBId: string = Guid.newGuid();;
    //private _IsAngularClient: boolean = true;

    
    //get PBId() { return this._PBId; }
    //get IsAngularClient() { return this._IsAngularClient; }

    public PBId: string = Guid.newGuid();;
    public IsAngularClient: boolean = true;

    public LoggingEnabled: boolean;

    public LoggingEntityReference: string;
    public LoggingObjectTableId: string;
    public LoggingEntityId: string;
    public LoggingObjectTableId2: string;
    public LoggingEntityId2: string;

    public LoggingUserId: string;
    public IsFakeResponse: boolean;
    public Tenant: number;
    public RequestName: string;
    public ResponseName: string;

    public RequestVIAChangeDue: string;
    public RequestVIA: SendRequestVIA;

    public DCAFileName: string;
    public InterfaceTypeCode: string;
    public MainInterfaceCode: string;
    public CustomsRequestsSheetId: string;

    public TransmitionDateTime: Date;

    public SuppressSplitWR: boolean;

    

    public ForcePersonalSign: boolean;
    public TestCase: TestCase
}
export class TestCase {
    public Code: string;
    public Param1: string;
    public Param2: string;
    public string: string;
}

export class CustomSendOptionsArgs {
    public Option: string;
    public ForcePersonalSign: boolean;
    public RequestVIA: SendRequestVIA;
    public TestCase: boolean= false;
} 
