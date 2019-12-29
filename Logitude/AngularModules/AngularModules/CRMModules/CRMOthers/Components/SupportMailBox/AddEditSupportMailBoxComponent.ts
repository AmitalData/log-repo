import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SupportMailboxPM } from '../../../../CRM/EntityPMs/SupportMailboxPM';
import { SupportMailboxPMService } from '../../../../CRM/Services/StandardPMs/SupportMailboxPMService';
import { Validator } from '../../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditSupportMailBoxComponent.html',
})

export class AddEditSupportMailBoxComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: AddEditSupportMailBoxComponent = this;
    public ObjectTableName: string = "SupportMailbox";
    public EntityPM: SupportMailboxPM;
    public IsNewEntity: boolean = false;
    private DefaultMailboxsCount: number;
    private myService: SupportMailboxPMService;
    public ValidationErrorsList: string[] = [];
    constructor() {
        super();
        this.myService = new SupportMailboxPMService();        
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['Mailbox'];
        this.IsNewEntity = args['IsNew'];
        this.DefaultMailboxsCount = args['DefaultMailboxsCount'];

        this.SetUIProperties();
        this.Clone();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("IsDefault", this.ObjectTableName, !this.Inactive);
        this.UIProperties.SetEnabled("Inactive", this.ObjectTableName, !this.IsDefault);
    }

    get Mailbox() { return this.EntityPM.Mailbox; }
    set Mailbox(value: string) {
        if (this.EntityPM.Mailbox != value) {
            this.EntityPM.Mailbox = value;
        }
    }

    get IsDefault() { return this.EntityPM.IsDefault; }
    set IsDefault(value: boolean) {
        if (this.EntityPM.IsDefault != value) {
            this.EntityPM.IsDefault = value;

            this.SetUIProperties();
        }
    }

    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;

            this.SetUIProperties();
        }
    }
    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.EntityPM.IsDefault) {
            if (this.DefaultMailboxsCount > 0) {
                errors.push("Only one Mailbox can be marked as Default");
            }
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNewEntity) {
                this.Insert();
            }
            else {
                this.Update();
            }
        }        
    }

    private Insert() {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
    private Update() {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Mailbox');
        this.myCloner.AddField('IsDefault');
        this.myCloner.AddField('Inactive');
       
        this.myCloner.AddEntity(this.EntityPM);       
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
