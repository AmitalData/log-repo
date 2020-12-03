export class AutomationSendInterface {

    constructor() {
        this.FTPDetails = new FTPAutomationDetails();
    }


    public InterfaceName: string;
    public SendVia: string;
    public Format: string;
    public ComputingPartnerId: string;
    public FTBFolderId: string;
    public FTPDetails: FTPAutomationDetails;
    public IsChanged: boolean;

}



export class FTPAutomationDetails {
    public Host: string;
    public Folder: string;
    public UserName: string;
    public Password: string;


}
