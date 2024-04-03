import { OnUpdateDocumentTypeAttachment } from '../Components/Maintenance/Automation/DocumentAttachmentsComponent';

export class AutomationOnUpdateDocument {

    constructor() {
        this.FTPDetails = new FTPAutomationDetails();
    }

    public SendVia: string;
    public FTPDetails: FTPAutomationDetails;
    public ComputingPartnerId: string;
    public DocumentTypeLists: OnUpdateDocumentTypeAttachment[];
    public IsChanged: boolean;
}

export class FTPAutomationDetails {
    public Host: string;
    public Folder: string;
    public UserName: string;
    public Password: string;
}
