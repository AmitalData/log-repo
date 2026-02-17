import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ActivityPM} from '../../../../CRM/EntityPMs/ActivityPM';
import {ActivityInputTemplate} from './ActivityInputTemplate';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ActivityInputArgs} from '../../../../CRM/Args'
import {ActivityPMService} from '../../../../CRM/Services/StandardPMs/ActivityPMService'; 
import {ActivityPMInitService} from '../../../../CRM/EntityPMInitServices/ActivityPMInitService'; 
import {AppTool} from '../../../../Infrastructure/Tools'; 

@Component({
    moduleId: module.id,
    templateUrl: './NewActivityComponent.html',
})

export class NewActivityComponent {
    public EntityPM: ActivityPM;
    public ValidationErrorsList: string[] = [];

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.RunComponent();
    }

    public TypeCode = "";
    SetWindowArgs(args: ActivityInputArgs) {
        if (args != null) {
            this.TypeCode = args.TypeCode;
            if (args.Activity != null) {
                this.EntityPM = args.Activity;
            }
            else {
                this.EntityPM = new ActivityPM();
                ActivityPMInitService.InitValues(this.EntityPM, true);
                this.EntityPM.ActivityTypeCode = this.TypeCode;
            }
        }
        else {
            this.EntityPM = new ActivityPM();
            ActivityPMInitService.InitValues(this.EntityPM, true);
        }

        if (!AppTool.IsNullOrEmpty( args.CustomerId)) {
            this.EntityPM.CustomerId = args.CustomerId;
        }
        if (!AppTool.IsNullOrEmpty(args.QuoteId)) {
            this.EntityPM.QuoteId = args.QuoteId;
        }
        if (!AppTool.IsNullOrEmpty(args.OpportunityId)) {
            this.EntityPM.OpportunityId = args.OpportunityId;
        }
        if (!AppTool.IsNullOrEmpty(args.TicketId)) {
            this.EntityPM.TicketId = args.TicketId;
        }
        if (!AppTool.IsNullOrEmpty(args.Subject)) {
            this.EntityPM.Subject = args.Subject;
        }
        if (!AppTool.IsNullOrEmpty(args.DueDate)) {
            this.EntityPM.DueDate = args.DueDate;
        }
        if (!AppTool.IsNullOrEmpty(args.CallWithId)) {
            this.EntityPM.CallWithId = args.CallWithId;
        }
        
        this.IsAddCustomerAllowed = args.IsAddCustomerAllowed;
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private IsAddCustomerAllowed = true;
    private IsEnabled = true;
    private ActivityInputTemplate: ActivityInputTemplate;
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load("./CRMModules/CRMActivity/Components/NewEntity/ActivityInputTemplate", this.viewContainerRef)
            .then(cmpRef => {
                this.ActivityInputTemplate = cmpRef.instance;
                var args = new ActivityInputArgs();
                args.Activity = this.EntityPM;
                args.TypeCode = this.TypeCode;
                args.IsEnabled = true;
                args.IsEditMode = false;
                args.IsAddCustomerAllowed = this.IsAddCustomerAllowed;
                this.ActivityInputTemplate.InitTemplate(args);
            });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.Ok();
    }
    SaveAsCompletedClicked() {
        this.EntityPM.IsMarkedCompleted = true;
        this.Ok();
    }
    Ok() {
        this.ValidationErrorsList = this.ActivityInputTemplate.Validate();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service = new ActivityPMService();
            service.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit('ok');
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}
