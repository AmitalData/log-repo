import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DateTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SupportMailboxPM } from '../../../../CRM/EntityPMs/SupportMailboxPM';
import { CRMDomainService } from '../../../../CRM/Services/CRMDomainService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    moduleId: module.id,
    templateUrl: './SupportMailBoxComponent.html',
})

export class SupportMailBoxComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public MailBoxesList: SupportMailboxPM[] = [];
    private domainService: CRMDomainService;
    constructor() {
        this.domainService = new CRMDomainService();
        this.LoadMailBoxes();
    }

    private LoadMailBoxes() {
        this.MailBoxesList = [];
        this.CurrentSession.StartBusyIndicatorLoading();

        this.domainService.GetSupportMailboxsByTenant().subscribe((myResponse: ServiceResponse) => {
            var pmResponse: ServiceResponse = myResponse;
            if (!pmResponse.HasError) {
                this.MailBoxesList = pmResponse.Result;              
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    AddMailBox() {
        var mailbox: SupportMailboxPM = new SupportMailboxPM();
        mailbox.Tenant = SessionLocator.Tenant;
        mailbox.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        mailbox.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        mailbox.CreatedByUserId = SessionLocator.LoggedUserId;
        mailbox.UpdatedByUserId = SessionLocator.LoggedUserId;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 300;
        logWindow.Title = "Add Support Mailbox";
        logWindow.WindowArgs = { Mailbox: mailbox, IsNew: true, DefaultMailboxsCount: this.MailBoxesList.filter(d => d.IsDefault).length };
        logWindow.Show('./CRMModules/CRMOthers/Components/SupportMailBox/AddEditSupportMailBoxComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "ok") {
                this.LoadMailBoxes();
            }
        });
    }

    EditMailBox(mailbox: SupportMailboxPM) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 300;
        logWindow.Title = "Edit Support Mailbox";
        logWindow.WindowArgs = { Mailbox: mailbox, IsNew: false, DefaultMailboxsCount: this.MailBoxesList.filter(d => d.IsDefault && d.Id != mailbox.Id).length };
        logWindow.Show('./CRMModules/CRMOthers/Components/SupportMailBox/AddEditSupportMailBoxComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "ok") {
                this.LoadMailBoxes();
            }
        });
    }

    DeleteMailBox(mailbox: SupportMailboxPM) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Support Mailbox?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicatorSaving();

                this.domainService.DeleteMailBox(mailbox.Id).subscribe((myResponse: ServiceResponse) => {
                    var pmResponse: ServiceResponse = myResponse;
                    if (!pmResponse.HasError) {
                        this.LoadMailBoxes();
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
