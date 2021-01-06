export class AutomationSendDocument {

    constructor() {
        this.FTPDetails = new FTPAutomationDetails();
    }

    public SendVia: string;
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
