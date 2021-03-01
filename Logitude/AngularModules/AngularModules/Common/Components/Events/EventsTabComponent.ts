declare var window: any;
import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TraceEventPM} from '../../../Infrastructure/EntityPMs/TraceEventPM';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {DateTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './EventsTabComponent.html',
})

export class EventsTabComponent implements OnDestroy {
    public EventsCount: number = 0;
    public ItemsSource: EventItemClass[];
    public EntityId: string;
    public ObjectTableId: string; 
    public ObjectTableName: string;
    public IsCustomerCare: boolean = false;
    public IsHybrid: boolean = false;
    private myDomainService: WebFreightDomainService = null;
    private AllTraceEvents: TraceEventPM[];
    public IsVisibile: boolean = false;
    public TabHeaderTextCode: string;
    LayoutDirection: string = 'ltr';
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.TabHeaderTextCode = entityArgs.ObjectTableName + ".TH.Events";

        this._entityResourceService.getEntityResourceByTableName("TraceEvent", 0).subscribe((response:any) => {
            this.IsVisibile = true;
            this.myDomainService = new WebFreightDomainService();
            this.ItemsSource = [];
            this.AllTraceEvents = [];
            this.InitTab();            
        });

        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    }

    InitTab() {
        if (this.entityArgs.ObjectTableName == "HelpResource") {
            this.EntityId = this.entityArgs.EntityPM.Code;
        }

        else {
            this.EntityId = this.entityArgs.EntityPM.Id;
        }

        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.ObjectTableId = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0].Id;
        this.IsCustomerCare = SessionLocator.LoggedUserPM.IsCustomerCare;
        this.IsHybrid = SessionLocator.TenantPM.IsHybrid;
        this.BuildPagesMenu();
        this.SetUIProperties();
        this.Listen();
        this.LoadData();
    }

    IsRefreshFollowUp: boolean = false;
    IsOpenAddEditEventTypeComponent: boolean = false;
    public IsAddButtonEnabled: boolean = false;
    SetUIProperties() {
        var isEnabled = false

        if (this.EntityId) {
            if (!this.IsHybrid) {
                isEnabled = true;
            }
        }

        this.IsAddButtonEnabled = isEnabled;
    }
    private ReLoadEntityCompletedEvent: any = null;

    private SessionEvent: any = null;
    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    Listen() {

        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "LoadEventTabData") {
                this.LoadData();
            }
        });

        if (this.entityArgs.EditComponent) {

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                var textcode = this.CurrentSession.CurrentEditComponent.SelectedTab.TextCode;
                if (textcode && textcode.includes("TH.Event")) {
                    this.LoadData();
                }
            });

            this.ReLoadEntityCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (this.IsRefreshFollowUp) {
                    this.CurrentSession.FireEvent("FollowupsChanged");
                    this.IsRefreshFollowUp = false;
                }

            });

                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        if (!this.EntityId) {
                            this.EntityId = this.entityArgs.EditComponent.EntityPM.Id;
                            this.SetUIProperties();
                        }

                        if (this.IsOpenAddEditEventTypeComponent) {
                            if (this.SelectedEventTypeClass) {
                                this.ShowAddEditEventWindow(this.SelectedEventTypeClass, this.SelectedEventTypeClass.Title);
                            }
                            this.IsOpenAddEditEventTypeComponent = false;
                            this.SelectedEventTypeClass = null;

                        }
                    }
                });
            
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.ReLoadEntityCompletedEvent);

        
    }

    public PagesMenu: PageMenu[];
    public SelectedMenu: PageMenu;
    public IsPagesMenuVisible: boolean = false;
    private BuildPagesMenu() {
        this.PagesMenu = [];
        this.PagesMenu.push(new PageMenu("ALL"));

        if (this.ObjectTableName == "Shipment") {
            this.PagesMenu.push(new PageMenu("LEG"));
            this.PagesMenu.push(new PageMenu("OPE"));
            this.PagesMenu.push(new PageMenu("LOG"));
            this.PagesMenu.push(new PageMenu("CUS"));
            //this.PagesMenu.push(new PageMenu("DOC"));

            this.IsPagesMenuVisible = true;
        }

        this.OnPagesMenuSelecting(this.PagesMenu[0]);
    }
    OnPagesMenuSelecting(item: PageMenu) {
        this.SelectedMenu = item;
        this.BuildItemsSource();
    }

    private includeDeleted: boolean = false;
    get IncludeDeleted() { return this.includeDeleted; }
    set IncludeDeleted(newValue: boolean) {
        if (this.includeDeleted != newValue) {
            this.includeDeleted = newValue;
            this.BuildItemsSource();
        }
    }

    RefreshButtonClicked() {
        this.LoadData();
    }

    public LoadData() {
        this.ItemsSource = [];

        this.myDomainService.GetTraceEventsForEntity(this.ObjectTableId, this.EntityId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.AllTraceEvents = myResponse.Result;
                    this.BuildItemsSource();
                }
            }
        });
    }

    private SearchText: string = null;
    SearchMethod(text: string) {
        if (AppTool.IsNullOrEmpty(text)) {
            this.SearchText = null;
        }

        else {
            this.SearchText = text;
        }

        this.BuildItemsSource();
    }

    private BuildItemsSource() {
        this.ItemsSource = [];

        if (this.SelectedMenu != null) {
            if (this.AllTraceEvents != null) {

                var items: TraceEventPM[] = [];

                this.AllTraceEvents.forEach(item => {
                    if (!AppTool.IsNullOrEmpty(this.SearchText)) {

                        var searchText = this.SearchText.toLowerCase();

                        if (item.EventTypeCode != null && item.EventTypeCode.toLowerCase().indexOf(searchText) != -1) {
                            items.push(item);
                        }

                        else if (item.EventTypeEnglishName != null && item.EventTypeEnglishName.toLowerCase().indexOf(searchText) != -1) {
                            items.push(item);
                        }

                        else if (item.ContactEnglishFirstName != null && item.ContactEnglishFirstName.toLowerCase().indexOf(searchText) != -1) {
                            items.push(item);
                        }

                        else if (item.Notes != null && item.Notes.toLowerCase().indexOf(searchText) != -1) {
                            items.push(item);
                        }
                    }

                    else {
                        items.push(item);
                    }
                });

                if (!this.IncludeDeleted) {
                    items = items.filter(f => f.Deleted == false);
                }

                switch (this.SelectedMenu.Code) {
                    case "ALL": {
                        break;
                    }

                    case "CUS": {
                        items = items.filter(f => f.IsCustomerView);
                        break;
                    }

                    default: {
                        items = items.filter(f => f.EventTypeCategoryCode != null);
                        items = items.filter(f => f.EventTypeCategoryCode.toUpperCase() == this.SelectedMenu.Code.toUpperCase());
                        break;
                    }
                }

                this.ItemsSource = [];
                this.EventsCount = items.length;

                items.forEach(item => {
                    this.ItemsSource.push(new EventItemClass(item, this));
                });
            }
        }
    }

    AddButtonClicked() {
        var traceEventPM = new TraceEventPM();
        traceEventPM.IsManualEntry = true;
        traceEventPM.LogDateTime = DateTool.GetCurrentDateTimeAsUtc();
        traceEventPM.EventDateTime = DateTool.GetCurrentDateAsUtc();
        traceEventPM.Tenant = SessionLocator.Tenant;
        traceEventPM.UserId = SessionLocator.LoggedUserId;
        traceEventPM.ObjectTableId = this.ObjectTableId;
        traceEventPM.EntityId = this.EntityId;
        traceEventPM.IsAddedManually = true;

        var itemClass = new EventItemClass(traceEventPM, this);
        itemClass.IsNewEntity = true;

        this.RunAddEditWindow(itemClass, TextCodeTranslator.Translate("TraceEvent.O.AddEvent"));
    }

    EditItemClicked(itemClass: EventItemClass) {
        this.RunAddEditWindow(itemClass, "Edit Event");
    }
    DeleteItemClicked(itemClass: EventItemClass) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Remove this event?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.myDomainService.DeleteTraceEvent(this.EntityId, this.ObjectTableId, itemClass.EntityPM.Id, true).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            this.LoadData();
                        }
                    }
                });
            }
        });
    }
    SelectedEventTypeClass: EventItemClass;
    RunAddEditWindow(item: EventItemClass, title: string) {

        if (this.ObjectTableName == "Shipment" && this.CurrentSession.CurrentEditComponent && this.entityArgs && this.entityArgs.EntityPM) {
            item.Title = title;
            this.SelectedEventTypeClass = item;
            this.IsOpenAddEditEventTypeComponent = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        else {
            this.ShowAddEditEventWindow(item, title);
        }

    }




    ShowAddEditEventWindow(item: EventItemClass, title: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 400;
        logWindow.WindowArgs = item
        logWindow.Title = title;
        logWindow.Show('./Common/Components/Events/AddEditEventComponent');
        logWindow.WindowClosed.subscribe((event: any) => {
            if (this.ObjectTableName == "Shipment" && this.CurrentSession.CurrentEditComponent && this.entityArgs && this.entityArgs.EntityPM && event != "Cancel") {
                this.IsRefreshFollowUp = true;
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

            }
            if (this.ObjectTableName == "Quote" && this.CurrentSession.CurrentEditComponent && this.entityArgs && this.entityArgs.EntityPM && event != "Cancel") {
                this.IsRefreshFollowUp = true;
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

            }
        });

    }
}

export class EventItemClass extends BaseComponent {
    public IsNewEntity: boolean = false;
    public EntityPM: TraceEventPM;
    public ObjectTableName = "TraceEvent";
    Title: string;
    constructor(item: TraceEventPM, public father: EventsTabComponent) {
        super();
        this.EntityPM = item;
        this.SetBackground();
        this.SetUIProperties();
    }

    showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;

    public Background: string = "rgba(235, 235, 235, 0.5)";
    SetBackground() {
        var myResult = "rgba(235, 235, 235, 0.5)";

        if (this.EntityPM.Deleted && this.EntityPM.IsAddedManually) {
            myResult = "rgba(247, 227, 227, 1)";
        }

        else if (this.EntityPM.Deleted) {
            myResult = "rgba(247, 227, 227, 1)";
        }

        else if (this.EntityPM.IsAddedManually) {
            myResult = "rgba(0, 125, 0, 0.098)";
        }

        this.Background = myResult;
    }

    public IsEditingDisabled: boolean = true;
    public IsDeletingDisabled: boolean = true;
    SetUIProperties() {
        var isEditingDisabled = true;
        var isDeletingDisabled = true;

        if (!this.Deleted) {
            if (this.IsAddedManually) {
                isEditingDisabled = false;
                isDeletingDisabled = false;
            }

            if (this.IsCustomerView) {
                isEditingDisabled = false;
            }
        }

        this.IsEditingDisabled = isEditingDisabled;
        this.IsDeletingDisabled = isDeletingDisabled;

        this.UIProperties.SetEnabled("EventDateTime", this.ObjectTableName, this.IsManualEntry);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, this.IsManualEntry);
    }

    get Name() {
        return this.showLocal ? this.EntityPM.EventTypeLocalName : this.EntityPM.EventTypeEnglishName;
    }
    get Location() { return this.EntityPM.Location; }
    get LogDateTime() { return this.EntityPM.LogDateTime; }

    get User() {
        var userName = this.EntityPM.ContactEnglishFirstName;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.PartnerName)) {
            userName = this.EntityPM.PartnerName;
        }
        return userName;
    }

    
    get LableUser() {
        var lableUser = "User";
        if (!AppTool.IsNullOrEmpty(this.EntityPM.PartnerName)) {
            lableUser = "Partner";
        }
        return lableUser;
    }

    get CustomerCare() { return this.EntityPM.CustomerCareUserEmail; }
    get Deleted() { return this.EntityPM.Deleted; }
    get IsManualEntry() { return this.EntityPM.IsManualEntry; }
    get IsAddedManually() { return this.EntityPM.IsAddedManually; }
    get IsCustomerView() { return this.EntityPM.IsCustomerView; }
    get ObjectTableId() { return this.EntityPM.ObjectTableId; }

    get EventTypeId() { return this.EntityPM.EventTypeId; }
    set EventTypeId(newValue: string) {
        if (this.EntityPM.EventTypeId != newValue) {
            this.EntityPM.EventTypeId = newValue;
        }
    }

    get EventDateTime() { return this.EntityPM.EventDateTime; }
    set EventDateTime(newValue: Date) {
        if (this.EntityPM.EventDateTime != newValue) {
            this.EntityPM.EventDateTime = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }
}
class PageMenu {
    Code: string;
    Name: string;
    constructor(myCode: string) {
        this.Code = myCode;

        switch (myCode) {
            case "ALL": {
                this.Name = TextCodeTranslator.Translate("TraceEvent.O.All");
                break;
            }

            case "LEG": {
                this.Name = TextCodeTranslator.Translate("TraceEvent.O.Routings");
                break;
            }

            case "OPE": {
                this.Name = TextCodeTranslator.Translate("TraceEvent.O.Operations");
                break;
            }

            case "LOG": {
                this.Name = TextCodeTranslator.Translate("TraceEvent.O.Logs");
                break;
            }

            case "CUS": {
                this.Name = "Shared with Customer";
                break;
            }

            case "DOC": {
                this.Name = "Documents";
                break;
            }
        }
    }
}
