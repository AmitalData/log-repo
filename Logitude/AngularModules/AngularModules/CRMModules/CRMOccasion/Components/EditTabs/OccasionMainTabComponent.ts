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
        this.EntityPM.OccasionInvitees.forEach(item => {
            this.OccasionLinesList.push(new OccasionLineClass(item, this));
        });
    }

    AddContactsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.Title = "Add Contact";
        logWindow.WindowArgs = this.EntityPM;        
        logWindow.Show("./CRMModules/CRMOccasion/Components/AddEdit/AddEditOccasionContactComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {

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

export class OccasionLineClass {

    public entityPM: OccasionInviteePM;

    constructor(entityPM: OccasionInviteePM, public father: OccasionMainTabComponent) {

    }
}
