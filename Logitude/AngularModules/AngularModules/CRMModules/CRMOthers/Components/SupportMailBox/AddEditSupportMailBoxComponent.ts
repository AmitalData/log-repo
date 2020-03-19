import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SupportMailboxPM } from '../../../../CRM/EntityPMs/SupportMailboxPM';
import { SupportMailboxPMService } from '../../../../CRM/Services/StandardPMs/SupportMailboxPMService';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

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
    private DefaultMailbox: string;
    private myService: SupportMailboxPMService;
    public ValidationErrorsList: string[] = [];
    constructor() {
        super();
        this.myService = new SupportMailboxPMService();        
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['Mailbox'];
        this.IsNewEntity = args['IsNew'];
        this.DefaultMailbox = args['DefaultMailbox'];

        this.SetUIProperties();
        this.Clone();
    }

    public IsDefaultInfoIconVisible: boolean = false;
    SetUIProperties() {
        var isDefaultEnabled: boolean = true;
        var inactiveEnabled: boolean = true;

        if (this.IsNewEntity) {
            
        }

        else {
            if (this.IsDefault) {
                isDefaultEnabled = false;
                inactiveEnabled = false;
                this.IsDefaultInfoIconVisible = true;
            }

            if (this.Inactive) {
                isDefaultEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("IsDefault", this.ObjectTableName, isDefaultEnabled);
        this.UIProperties.SetEnabled("Inactive", this.ObjectTableName, inactiveEnabled);
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

            this.UIProperties.SetEnabled("Inactive", this.ObjectTableName, !value);
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
        
        this.ValidationErrorsList = errors;
        
        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.IsDefault && !AppTool.IsNullOrEmpty(this.DefaultMailbox)) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("Please notice that the default mailbox already exists (" + this.DefaultMailbox + ") will be changed, ok?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.Save();
                    }
                });
            }

            else {
                this.Save();
            }                       
        }        
    }

    private Save() {
        if (this.IsNewEntity) {
            this.Insert();
        }
        else {
            this.Update();
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
