import {Component, ViewChild, ViewContainerRef, OnInit} from '@angular/core';
import {ActivityPM} from '../../../../CRM/EntityPMs/ActivityPM';
import {ActivityPMService} from '../../../../CRM/Services/StandardPMs/ActivityPMService';
import {ActivityValidator} from '../../../../CRM/Validators/ActivityValidator';
import {ActivityInputArgs} from'../../../../CRM/Args';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../Infrastructure/Tools';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'NewTaskComponent',
    moduleId: module.id,
    templateUrl: './NewTaskComponent.html',
})

export class NewTaskComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Activity";
    public DataContext: NewTaskComponent = this;
    public EntityPM: ActivityPM;    
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private myActivityPMService: ActivityPMService;
    private entityResourceService: EntityResourceService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.myActivityPMService = new ActivityPMService();
        this.entityResourceService = new EntityResourceService();
        this.EntityPM = this.myActivityPMService.GetNewEntityPM();        
    }

    ngOnInit() {
        this.RunComponent();
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

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, "Activity.AdditionalFields");
            });
    }
    
    public IsAddCustomerVisible: boolean = false;
    SetWindowArgs(args: ActivityInputArgs) {
        if (args != null) {
            this.EntityPM.ActivityTypeCode = args.TypeCode;
            this.IsAddCustomerVisible = args.IsAddCustomerAllowed;

            this.SetUIProperties();
        }
    }

    private SetUIProperties() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
            this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
        }        
    }

    //Properties
    get Subject() { return this.EntityPM.Subject; }
    set Subject(newValue: string) {
        if (this.EntityPM.Subject != newValue) {
            this.EntityPM.Subject = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get OwnerId() { return this.EntityPM.OwnerId; }
    set OwnerId(newValue: string) {
        if (this.EntityPM.OwnerId != newValue) {
            this.EntityPM.OwnerId = newValue;            
        }
    }

    private owner: UserList = null;
    get Owner() { return this.owner; }
    set Owner(newValue: UserList) {
        if (this.owner != newValue) {
            this.owner = newValue;
            this.OnOwnerChanged(newValue);
        }
    }

    get StartDateTime() { return this.EntityPM.StartDateTime; }
    set StartDateTime(newValue: Date) {
        if (this.EntityPM.StartDateTime != newValue) {
            this.EntityPM.StartDateTime = newValue;

            this.EntityPM.EndDateTime = newValue;
        }
    }

    get PriorityCode() { return this.EntityPM.PriorityCode; }
    set PriorityCode(newValue: string) {
        if (this.EntityPM.PriorityCode != newValue) {
            this.EntityPM.PriorityCode = newValue;
        }
    }

    get DueDate() { return this.EntityPM.DueDate; }
    set DueDate(newValue: Date) {
        if (this.EntityPM.DueDate != newValue) {
            this.EntityPM.DueDate = newValue;
        }
    }

    get CustomerId() { return this.EntityPM.CustomerId; }
    set CustomerId(newValue: string) {
        if (this.EntityPM.CustomerId != newValue) {
            this.EntityPM.CustomerId = newValue;
        }
    }
    
    private OnOwnerChanged(list: UserList) {
        if (list == null) {
            this.EntityPM.BusinessUnitId = null;
        }

        else {
            this.EntityPM.BusinessUnitId = list.BusinessUnitId;
        }
    }

    AddCustomerClicked() {
        this.entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(response1 => {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "New Potential Customer";
                logWindow.Width = 990;
                logWindow.Height = 600;
                logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");

                logWindow.ComponentLoaded.subscribe(comp => {
                    logWindow.WindowClosed.subscribe(s => {
                        if (s) {
                            this.CustomerId = comp.EntityPM.Id;
                        }
                    });
                });
            });
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();

        if (AppTool.IsNullOrEmpty(this.EntityPM.BusinessUnitId)) {
            this.EntityPM.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
        }
        
        var activityValidator: ActivityValidator = new ActivityValidator();
        this.ValidationErrorsList = activityValidator.Validate(this.EntityPM);

        if (this.ValidationErrorsList.length == 0) {
            this.myActivityPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.CurrentSession.CloseCurrentWindowEmit('OK');
                }
            });
        }

        else {
            this.CurrentSession.StopBusyIndicator();
        } 
    }
}
