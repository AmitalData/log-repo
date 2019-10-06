import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { OccasionPM } from '../../../../CRM/EntityPMs/OccasionPM';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

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
    public OccasionLinesList = [];

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM) {
            this.EntityId = this.EntityPM.Id;
        }

    }

    LoadLinesData() {


    }

    AddContacts() {


    }

    // Search
    private SearchText: string = "";
    SearchTextKeyUp(args: any) {
        this.SearchText = args;
        this.LoadLinesData();
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
