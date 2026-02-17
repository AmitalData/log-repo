import {Component, OnInit}  from '@angular/core';
import {InboundEmailPM} from '../../../../Infrastructure/EntityPMs/InboundEmailPM';
import {InboundEmailLinePM} from '../../../../Infrastructure/EntityPMs/InboundEmailLinePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DateTool} from '../../../../Infrastructure/Tools'; 
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {InboundEmailWebService, InboundEmailResult} from '../../../../Infrastructure/Services/WebServices/InboundEmailWebService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'TicketDocsOutTabComponent',
    moduleId: module.id,
    templateUrl: './NewInboundEmailComponent.html',
})

export class NewInboundEmailComponent extends BaseComponent implements OnInit {

    public entityPM: InboundEmailPM = new InboundEmailPM();
    private linePM: InboundEmailLinePM = new InboundEmailLinePM(null);

    public ObjectTableName = "InboundEmailLine";
    public DataContext = this;
    public ValidationErrorsList = [];
    private myInboundEmailWebService: InboundEmailWebService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    ngOnInit() {
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.entityPM = new InboundEmailPM();
        this.entityPM.Tenant = SessionLocator.Tenant;
        this.entityPM.CreateDate = todayDate;
        this.entityPM.UpdateDate = todayDate;

        this.linePM = new InboundEmailLinePM(null);
        this.linePM.Tenant = SessionLocator.Tenant;
        this.linePM.InboundEmailId = this.entityPM.Id;
        this.linePM.CreateDate = todayDate;
        this.entityPM.AddInboundEmailLinePM(this.linePM);
    }

    get Recepient() { return this.linePM.Recepient; }
    set Recepient(value: string) {
        if (this.linePM.Recepient != value) {
            this.linePM.Recepient = value;
        }
    }

    get Subject() { return this.linePM.Subject; }
    set Subject(value: string) {
        if (this.linePM.Subject != value) {
            this.linePM.Subject = value;
        }
    }

    get Body() { return this.linePM.Body; }
    set Body(value: string) {
        if (this.linePM.Body != value) {
            this.linePM.Body = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        var toEmails = this.Recepient.replace(" ", "");
        var emailbody = this.Body;
        var emailSubject = this.Subject;

        if (AppTool.IsNullOrEmpty(toEmails)) {
            errors.push("Sending Email", "Please specify at least one recepient");
            return;
        }

        var isValidEmailsTo = this.CheckIsValidEmails(toEmails);
        if (!isValidEmailsTo) {
            errors.push("Sending Document", "Some of To e-mails are Invalid");
            return;
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            //this.Send();
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    CheckIsValidEmails(mailsList: string): boolean {
        var isOk = true;
        var mails = mailsList.split(';');
        mails.forEach(email => {
            if (!AppTool.IsNullOrEmpty(email)) {
                isOk = FormatTool.IsEmail(email);
            }
        });
        return isOk;
    }

    Send() {
        this.CurrentSession.StartBusyIndicator("Sending...");
        if (this.myInboundEmailWebService == null) {
            this.myInboundEmailWebService = new InboundEmailWebService();
        }
        this.myInboundEmailWebService.SendInboundEmailAsync(this.Recepient, this.entityPM.Tenant, this.Subject, this.Body, this.entityPM.Id).subscribe((myResult) => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {
                var mySendingResultClass = myResponse.Result;

                if (mySendingResultClass != null) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show("Email has been sent successfully");
                }

                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
