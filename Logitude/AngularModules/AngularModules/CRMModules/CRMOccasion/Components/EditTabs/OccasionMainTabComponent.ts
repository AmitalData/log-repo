import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { OccasionPM } from '../../../../CRM/EntityPMs/OccasionPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { OccasionInviteePM } from '../../../../CRM/EntityPMs/OccasionInviteePM'; 
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    selector: 'OccasionMainTabComponent',
    moduleId: module.id,
    templateUrl: './OccasionMainTabComponent.html',
})

export class OccasionMainTabComponent extends BaseComponent {

    public EntityPM: OccasionPM;
    public EntityId: string;
    public IsCheckedAllContacts: false;
    public DataContext: OccasionMainTabComponent = this;
    public ObjectTableName = "Occasion";
    public _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public OccasionLinesList: OccasionLineClass[] = [];

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.EntityId = this.EntityPM.Id;
        }
        this.LoadOccasionLinesData();
    }

    LoadOccasionLinesData() {
        this.OccasionLinesList = [];
        var result = [];
        this.EntityPM.OccasionInvitees.forEach(item => {
            result.push(new OccasionLineClass(item, this));
           
        });

        if (this.OccasionLinesList != null && !AppTool.IsNullOrEmpty(this.SearchText)) {
            result = result.filter(d => d.SearchFields != null && d.SearchFields && d.SearchFields.toUpperCase().indexOf(this.SearchText.toUpperCase()) > -1);
        }

        this.OccasionLinesList = result;
    }

    AddContactsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.Title = "Add Contact";
        logWindow.WindowArgs = this.EntityPM;        
        logWindow.Show("./CRMModules/CRMOccasion/Components/AddEdit/AddEditOccasionContactComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.LoadOccasionLinesData();
        });
    }

    DeleteOccasionInviteeClicked(item: OccasionLineClass ) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this invitee?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.RemoveOccasionInvitee(item.EntityPM);
                this.LoadOccasionLinesData();
            }
        });
    }

    // Search
    private SearchText: string = "";
    SearchTextKeyUp(args: any) {
        this.SearchText = args;
        this.LoadOccasionLinesData();
    }

    // Statistics
    public NumberAllContacts: number = 0;
    public NumberInvitedContacts: number = 0;
    public NumberOfParticipatedContacts: number = 0;
    public NumberAllCustomers: number = 0;
    public NumberInvitedCustomers: number = 0;
    public NumberOfParticipatedCustomers: number = 0;

    ViewAllData(arg) {
        var objectTableName = "";
        var queryCode = "";
        var filterAgrs = new ApiQueryFilters();
        var listArgs = new ListComponentArgs();

        switch (arg) {
            case "AllContacts":
            case "InvitedContacts":
            case "ParticipatedContacts":
                {
                    objectTableName = "Contact";
                    queryCode = "Contacts";
                    break;
                }
            case "AllContacts":
            case "InvitedContacts":
            case "ParticipatedContacts":
                {
                    objectTableName = "Customer";
                    queryCode = "Customers";
                    break;
                }
        }

        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.BackButtonTitle = "Occasions";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                });
        });
    }
}

export class OccasionLineClass extends BaseComponent {

    public EntityPM: OccasionInviteePM;
    public DataContext: OccasionLineClass = this;
    public ObjectTableName = "OccasionInvitee";

    constructor(entityPM: OccasionInviteePM, public father: OccasionMainTabComponent) {
        super();
        this.EntityPM = entityPM; 
    }

    private isChecked: boolean;
    get IsChecked() {
        return this.isChecked;
    }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
        }
    }

    get Notes() {
        return this.EntityPM.Notes;
    }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    get OccasionName() {
        return this.EntityPM.OccasionName;
    }
    set OccasionName(value: string) {
        if (this.EntityPM.OccasionName != value) {
            this.EntityPM.OccasionName = value;
        }
    }

    get ContactName() {
        return this.EntityPM.ContactName;
    }
    set ContactName(value: string) {
        if (this.EntityPM.ContactName != value) {
            this.EntityPM.ContactName = value;
        }
    }

    get ContactEmail() {
        return this.EntityPM.ContactEmail;
    }
    set ContactEmail(value: string) {
        if (this.EntityPM.ContactEmail != value) {
            this.EntityPM.ContactEmail = value;
        }
    }

    get ContactTel() {
        return this.EntityPM.ContactTel;
    }
    set ContactTel(value: string) {
        if (this.EntityPM.ContactTel != value) {
            this.EntityPM.ContactTel = value;
        }
    }

    get ContactMobile() {
        return this.EntityPM.ContactMobile;
    }

    set ContactMobile(value: string) {
        if (this.EntityPM.ContactMobile != value) {
            this.EntityPM.ContactMobile = value;
        }
    }

    get ContactPosition() {
        return this.EntityPM.ContactPosition;
    }
    set ContactPosition(value: string) {
        if (this.EntityPM.ContactPosition != value) {
            this.EntityPM.ContactPosition = value;
        }
    }

    public InviteeColor: string = "black";
    public ParticipatedColor: string = "black";

    MarkAsInvitedClicked() {

    }

    MarkAsParticipatedClicked() {

    }
}
