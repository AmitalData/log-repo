
import {CommunicationLogPM} from '../../../../../../Common/EntityPMs/CommunicationLogPM';

export class CommunicationLogPMViewModel {


    public Id: string;
    public Tenant: number;
    public To: string;
    public CC: string;
    public Subject: string;
    public CommunicationStatusTypeName: string;
    public CreateDate: Date;
    public DoneDate: Date;
    CurrentEntityPm: CommunicationLogPM;
    public DivSelectBackgroud: string;
    public CommunicationStatusTypeCode: string;
    public EmailDeliveryError: string;

    constructor(communicationLogPM: CommunicationLogPM) {
        this.Id = communicationLogPM.Id;
        this.Tenant = communicationLogPM.Tenant;
        this.To = communicationLogPM.To;
        this.CC = communicationLogPM.CC;
        this.Subject = communicationLogPM.Subject;
        this.CommunicationStatusTypeName = communicationLogPM.CommunicationStatusTypeName;
        this.CreateDate = communicationLogPM.CreateDate;
        this.DoneDate = communicationLogPM.DoneDate;
        this.CommunicationStatusTypeCode = communicationLogPM.CommunicationStatusTypeCode;
        this.EmailDeliveryError = communicationLogPM.EmailDeliveryError;
        this.CurrentEntityPm = communicationLogPM;
    }
}