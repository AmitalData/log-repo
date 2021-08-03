export class AutomationSendInterface {

    constructor() {
        this.FTPDetails = new FTPAutomationDetails();
        this.WebHookDetails = new WebHookAutomationDetails();
    }


    public InterfaceName: string;
    public SendVia: string;
    public Format: string;
    public ComputingPartnerId: string;
    public FTBFolderId: string;
    public FTPDetails: FTPAutomationDetails;
    public WebHookDetails: WebHookAutomationDetails;
    public IsChanged: boolean;

}



export class FTPAutomationDetails {
    public Host: string;
    public Folder: string;
    public UserName: string;
    public Password: string;


}

export class WebHookAutomationDetails {
    public URL: string;
}
