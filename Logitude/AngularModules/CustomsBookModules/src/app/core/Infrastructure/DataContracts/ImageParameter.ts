
export class ImageParameter {




    public Key: string;
    public FileName: string;
    public EntityId: string;
    ContactId: string;

    public FileSize: number;
    public SentSize: number;
    public IsFirstTry: boolean;
    public BlocksNumber: number;
    PartsNumber: number;
    SendPartNumber: number;
    public UploadType: string;
    
    public BufferNumber: number;
    public Tenant: number;
    public  FileData: any;
    Extension: string;
    public buffer: any;
    public BlockIdsList: string[];
    public IsLoadEnd: boolean = false;
    public UploadMode: string ;
    public Width: number;
    public Height: number;
    public Base64String: string;
    public EncodedFileName: string;

    public Buffersize: number;
    public Position: number;
    public Result: string;
    public KeepOriginalSize:boolean;

    constructor() {
     
    }
}