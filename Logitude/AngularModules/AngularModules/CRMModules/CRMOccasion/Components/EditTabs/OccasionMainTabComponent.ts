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
import { CRMDomainService } from '../../../../CRM/Services/CRMDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';


@Component({
    selector: 'OccasionMainTabComponent',
    moduleId: module.id,
    templateUrl: './OccasionMainTabComponent.html',
})

export class OccasionMainTabComponent extends BaseComponent {

    public EntityPM: OccasionPM;
    public EntityId: string;
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
        this.SetUIProperties_EntityClosed();
        this.LoadOccasionLinesData();
        this.Listen();
       
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties_EntityClosed();
                    this.LoadOccasionLinesData();
                  
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties_EntityClosed();
                }
            });

            this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "OCMN") {
                    this.SetUIProperties_EntityClosed();
                    this.LoadOccasionLinesData();
                   
                }
            });
        }
    }

    IsOccasionEnabled = true; 
    private SetUIProperties_EntityClosed() {
        var isEnabled = this.EntityPM.OccasionStatusId == "CD" ? false : true;
        this.IsOccasionEnabled = isEnabled;
        this.UIProperties.SetEnabled("IsCheckedAllContacts", null, isEnabled); 
    }

    LoadOccasionLinesData() {
        this.OccasionLinesList = [];
        var result: OccasionLineClass[] = [];
        this.EntityPM.OccasionInvitees.forEach(item => {
            result.push(new OccasionLineClass(item, this));

        });

        if (this.OccasionLinesList != null && !AppTool.IsNullOrEmpty(this.SearchText)) {
            result = result.filter(d => d.SearchFields != null && d.SearchFields && d.SearchFields.toUpperCase().indexOf(this.SearchText.toUpperCase()) > -1);
        }

        if (!AppTool.IsNullOrEmpty(this.mySelectedParticipatedFilter) && this.mySelectedParticipatedFilter != "A") {
            if (this.mySelectedParticipatedFilter == "PA") {
                result = result.filter(a => a.Participated);
            }
            else {
                result = result.filter(a => !a.Participated);
            }
        }

        if (!AppTool.IsNullOrEmpty(this.mySelectedInvitedFilter) && this.mySelectedInvitedFilter != "A") {
            if (this.mySelectedInvitedFilter == "IN") {
                result = result.filter(a => a.Invited);
            }
            else {
                result = result.filter(a => !a.Invited);
            }
        }

        this.OccasionLinesList = result;
        this.LoadQueriesCounts();
    }

    AddContactsClicked() {
        if (this.IsOccasionEnabled) {
            var logWindow = new LogitudeWindow();
            logWindow.IsFillScreen = true;
            logWindow.Title = "Add Contact";
            logWindow.WindowArgs = this.EntityPM;
            logWindow.Show("./CRMModules/CRMOccasion/Components/AddEdit/AddEditOccasionContactComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.LoadOccasionLinesData();
            });

        }
    }

    DeleteOccasionInviteeClicked(item: OccasionLineClass) {
        if (this.IsOccasionEnabled) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Delete this invitee?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.EntityPM.RemoveOccasionInvitee(item.EntityPM);
                    this.LoadOccasionLinesData();
                }
            });
        }
    }

    public isCheckedAllContacts: boolean;
    get IsCheckedAllContacts() { return this.isCheckedAllContacts; }
    set IsCheckedAllContacts(value: boolean) {
        if (this.isCheckedAllContacts != value) {
            this.isCheckedAllContacts = value;
            this.OccasionLinesList.forEach(item => {
                item.IsChecked = value;
            });
        }
    }

    // Search
    private SearchText: string = "";
    SearchTextKeyUp(args: any) {
        this.SearchText = args;
        this.LoadOccasionLinesData();
    }

    // Filters
    private mySelectedInvitedFilter: string = "";
    get SelectedInvitedFilter() { return this.mySelectedInvitedFilter; }
    set SelectedInvitedFilter(newValue: string) {
        if (this.mySelectedInvitedFilter != newValue) {
            this.mySelectedInvitedFilter = newValue;
            this.LoadOccasionLinesData();
        }
    }

    private mySelectedParticipatedFilter: string = "";
    get SelectedParticipatedFilter() { return this.mySelectedParticipatedFilter; }
    set SelectedParticipatedFilter(newValue: string) {
        if (this.mySelectedParticipatedFilter != newValue) {
            this.mySelectedParticipatedFilter = newValue;
            this.LoadOccasionLinesData();
        }
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
                {
                    objectTableName = "Contact";
                    queryCode = "Contacts";
                    filterAgrs.addAdditionalFilter("Occasion_ContactsQuery", this.AllContacts_Ids, null, null, "Equals", true, false, false, "string");
                    break;
                }
            case "InvitedContacts":
                {
                    objectTableName = "Contact";
                    queryCode = "Contacts";
                    filterAgrs.addAdditionalFilter("Occasion_ContactsQuery", this.InviteesContacts_Ids, null, null, "Equals", true, false, false, "string");
                    break;
                }
            case "ParticipatedContacts":
                {
                    filterAgrs.addAdditionalFilter("Occasion_ContactsQuery", this.ParticipatedContacts_Ids, null, null, "Equals", true, false, false, "string");
                    objectTableName = "Contact";
                    queryCode = "Contacts";
                    break;
                }
            case "AllCustomers":
                {
                    filterAgrs.addAdditionalFilter("Occasion_CustomersQuery", this.AllContacts_Ids, null, null, "Equals", true, false, false, "string");
                    objectTableName = "Customer";
                    queryCode = "Customers";
                    break;
                }
            case "InvitedCustomers":
                {
                    filterAgrs.addAdditionalFilter("Occasion_CustomersQuery", this.InviteesContacts_Ids, null, null, "Equals", true, false, false, "string");
                    objectTableName = "Customer";
                    queryCode = "Customers";
                    break;
                }
            case "ParticipatedCustomers":
                {
                    filterAgrs.addAdditionalFilter("Occasion_CustomersQuery", this.ParticipatedContacts_Ids, null, null, "Equals", true, false, false, "string");
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

    private AllContacts_Ids: string = "";
    private InviteesContacts_Ids: string = "";
    private ParticipatedContacts_Ids: string = "";

    LoadQueriesCounts() {
        this.LoadAllContactsCount();
        this.LoadAllCustomersCount();

        this.NumberInvitedCustomers = this.EntityPM.InvitedCustomers;
        this.NumberOfParticipatedCustomers = this.EntityPM.ParticipatedCustomers;
    }

    LoadAllContactsCount() {
        this.AllContacts_Ids = "";
        this.InviteesContacts_Ids = "";
        this.ParticipatedContacts_Ids = "";
        this.NumberAllContacts = this.EntityPM.OccasionInvitees.length;
        this.NumberInvitedContacts = this.EntityPM.InvitedContacts;
        this.NumberOfParticipatedContacts = this.EntityPM.ParticipatedContacts;

        this.EntityPM.OccasionInvitees.forEach(item => {
            this.AllContacts_Ids = this.AllContacts_Ids + item.ContactId + ",";
        });
       
        this.EntityPM.OccasionInvitees.filter(a => a.Invited).forEach(item => {
            this.InviteesContacts_Ids = this.InviteesContacts_Ids + item.ContactId + ",";
        });

        this.EntityPM.OccasionInvitees.filter(a => a.Participated).forEach(item => {
            this.ParticipatedContacts_Ids = this.ParticipatedContacts_Ids + item.ContactId + ",";
        });
    }

    LoadAllCustomersCount() {
        var service = new CRMDomainService();
        var contactsIds = "";

        this.EntityPM.OccasionInvitees.forEach(item => {
            contactsIds = contactsIds + item.ContactId + ",";
        });

        service.GetCountOfOccasionAllCustomers(contactsIds).subscribe(myResult => {
            var mm: ServiceResponse = myResult;
            var list_AllCustomers = [];
            if (!mm.HasError) {
                this.NumberAllCustomers = myResult.Result;
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    ActionsButtonClicked(args) {
        if (this.IsOccasionEnabled) {
            switch (args) {
                case "MAI":
                    {
                        this.MarkInvitees_Action(true);
                        this.IsCheckedAllContacts = false;
                        break;
                    }
                case "MAP":
                    {
                        this.MarkParticipated_Action(true);
                        this.IsCheckedAllContacts = false;
                        break;
                    }
                case "MAUI":
                    {
                        this.MarkInvitees_Action(false);
                        this.IsCheckedAllContacts = false;
                        break;
                    }
                case "MAUP":
                    {
                        this.MarkParticipated_Action(false);
                        this.IsCheckedAllContacts = false;
                        break;
                    }
                case "D":
                    {
                        this.DeleteCheckedOccasionInvitee();
                        break;
                    }
            }
        }
      
    }

    MarkInvitees_Action(isInvited) {
        if (this.IsOccasionEnabled) {
            this.OccasionLinesList.filter(a => a.IsChecked).forEach(item => {
                item.Invited = isInvited;
            });
            this.LoadOccasionLinesData();
        }
    }

    MarkParticipated_Action(isParticipated) {
        if (this.IsOccasionEnabled) {
            this.OccasionLinesList.filter(a => a.IsChecked).forEach(item => {
                item.Participated = isParticipated;
            });
            this.LoadOccasionLinesData();
        }
    }

    DeleteCheckedOccasionInvitee() {
        if (this.IsOccasionEnabled) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Delete all checked invitees?");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.OccasionLinesList.filter(a => a.IsChecked).forEach(item => {
                        this.EntityPM.RemoveOccasionInvitee(item.EntityPM);
                    });
                    this.LoadOccasionLinesData();
                    this.IsCheckedAllContacts = false;
                }
            });
        }
    }
}

export class OccasionLineClass extends BaseComponent {

    public EntityPM: OccasionInviteePM;
    public DataContext: OccasionLineClass = this;
    public ObjectTableName = "OccasionInvitee";

    constructor(entityPM: OccasionInviteePM, public father: OccasionMainTabComponent) {
        super();
        this.EntityPM = entityPM;
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.father.IsOccasionEnabled);
        this.UIProperties.SetEnabled("IsChecked", null, this.father.IsOccasionEnabled);
    }

    private isChecked: boolean = false;
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

    get CustomerName() {
        return this.EntityPM.CustomerName;
    }
    set CustomerName(value: string) {
        if (this.EntityPM.CustomerName != value) {
            this.EntityPM.CustomerName = value;
        }
    }
 
    get SearchFields() {
        return this.EntityPM.SearchFields;
    }
    set SearchFields(value: string) {
        if (this.EntityPM.SearchFields != value) {
            this.EntityPM.SearchFields = value;
        }
    }

    get Participated() {
        return this.EntityPM.Participated;
    }
    set Participated(value: boolean) {
        if (this.EntityPM.Participated != value) {
            this.EntityPM.Participated = value;
        }
    }

    get Invited() {
        return this.EntityPM.Invited;
    }
    set Invited(value: boolean) {
        if (this.EntityPM.Invited != value) {
            this.EntityPM.Invited = value;
        }
    }

    InvitedClicked(isInvited: boolean) {
        if (this.father.IsOccasionEnabled) {
            this.Invited = isInvited;
        }
    }

    ParticipatedClicked(isParticipated: boolean) {
        if (this.father.IsOccasionEnabled) {
            this.Participated = isParticipated;
        }
    }
}
