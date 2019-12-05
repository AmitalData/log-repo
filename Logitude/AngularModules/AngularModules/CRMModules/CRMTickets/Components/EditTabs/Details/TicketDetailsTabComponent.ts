import {Component, OnInit, AfterViewInit, ViewChild, ViewContainerRef} from '@angular/core';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslationPipe} from '../../../../../Controls/Pipes/TextCodeTranslationPipe';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {CRMDomainService} from '../../../../../CRM/Services/CRMDomainService';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ShipmentList} from '../../../../../Shipment/EntityLists/ShipmentList';
import {ContactInputTemplateArgs} from '../../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import {UserList} from '../../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../../Common/Services/StandardLists/UserListService';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {CardListService} from '../../../../../Common/Services/StandardLists/CardListService';
import {TicketClassificationList} from '../../../../../CRM/EntityLists/TicketClassificationList';
import {TicketClassificationListService} from '../../../../../CRM/Services/StandardLists/TicketClassificationListService';
import {CRMTool} from '../../../../../CRM/Tools';
import {TicketSeverityList} from '../../../../../CRM/EntityLists/TicketSeverityList';
import {TicketSeverityListService} from '../../../../../CRM/Services/StandardLists/TicketSeverityListService';
import {ContactList} from '../../../../../Common/EntityLists/ContactList';
import {ContactListService} from '../../../../../Common/Services/StandardLists/ContactListService';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ContactItemClass} from '../../../../../CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent';
import {ContactPMService} from '../../../../../Common/Services/StandardPMs/ContactPMService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
declare var window: any;

@Component({
    selector: 'DetailsTabComponent',
    moduleId: module.id,
    templateUrl: './TicketDetailsTabComponent.html',
})

export class TicketDetailsTabComponent extends BaseComponent implements AfterViewInit {
    public EntityPM: TicketPM;
    public LabelColumnWidth: number = 153;
    public ControlColumnWidth: number = 180;
    public DataContext: TicketDetailsTabComponent = this;
    public ObjectTableName: string = "Ticket";
    public IsFromOutSide = false;
    public IsShowConnectContact = false; 
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    _entityResourceService: EntityResourceService = new EntityResourceService();
    public EntityList: EntityClass[] = [];
    public EntityNumberTitle = "Shipment Number";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        var objectTable = window.ObjectTables.filter(d => d.Id === this.EntityPM.EntityType)[0];
        if (objectTable != null) {
            var objectTableName = window.ObjectTables.filter(d => d.Id === this.EntityPM.EntityType)[0].Name;
            if (objectTableName == "Quote") {
                this.EntityNumberTitle = "Quote Number";
            }
        }
        this.CreateEntities();
        this.SetUIProperties();
        this.getGeneralClassification();
        this.Listen();
        this.InitializeServices();
        this.RunComponent();
    }
    ngAfterViewInit() {
        //if (this.EntityPM != null && this.EntityPM.TicketCorrespondence != null && this.EntityPM.TicketCorrespondence[0].Direction == "O") {
        if (this.EntityPM.Source == "MAL") {
            this.IsFromOutSide = true;
        }

        else {
            this.IsFromOutSide = false;
        }
    }

    private TabSelectedEvent: any = null;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties_EntityClosed();
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties_EntityClosed();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "TIGE") {
                    this.GetEntityLinkNumberVisibility();
                    this.CreateEntities();
                    this.UpdateEntityDetails();
                    this.SetUIProperties();
                }
            });
        }
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
    private GeneratedComponent: any;

    private selectedFilter: EntityClass = null;
    get SelectedFilter() {
        return this.selectedFilter;
    }
    set SelectedFilter(value: EntityClass) {
        if (this.selectedFilter != value) {
            this.selectedFilter = value;
            this.UpdateEntityDetails();
            this.ClearFilterData();
        }
    }
    UpdateEntityDetails() {
        if (this.SelectedFilter != null && this.SelectedFilter.Code == "1") {
            this.EntityObjectTableName = "Shipment";
            this.EntityNumberTitle = "Shipment Number";
        }
        else {
            this.EntityObjectTableName = "Quote";
            this.EntityNumberTitle = "Quote Number";
        }

    }
    ClearFilterData() {
        this.ShipmentId = null;
        this.ShipmentNumber = null;
        this.QuoteId = null;
        this.QuoteNumber = null;
    }
    CreateEntities() {
        this.EntityList = [];
        var s_entity = new EntityClass();
        s_entity.Code = "1";
        s_entity.Name = "Shipment";
        this.EntityList.push(s_entity);

        var q_entity = new EntityClass();
        q_entity.Code = "2";
        q_entity.Name = "Quote";
        this.EntityList.push(q_entity);

        if (!AppTool.IsNullOrEmpty(this.QuoteId)) {
            this.EntityNumberTitle = "Quote Number";
            this.EntityObjectTableName = "Quote";
            this.selectedFilter = q_entity;
        }

        if (!AppTool.IsNullOrEmpty(this.ShipmentId)) {
            this.EntityNumberTitle = "Shipment Number";
            this.EntityObjectTableName = "Shipment";
            this.selectedFilter = s_entity;
        }

        if (AppTool.IsNullOrEmpty(this.QuoteId) && AppTool.IsNullOrEmpty(this.QuoteId)) {
            this.EntityNumberTitle = "Shipment Number";
            this.EntityObjectTableName = "Shipment";
            this.selectedFilter = s_entity;
        }
    }

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
                this.GeneratedComponent = cmpRef.instance;
                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, "Ticket.AdditionalFields");
            });
    }
    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.IsTicketEditEnabled);
        }
    }

    ContactListService: ContactListService;
    TicketSeverityListService: TicketSeverityListService;
    TicketClassificationListService: TicketClassificationListService;
    InitializeServices() {
        this.ContactListService = new ContactListService();
        this.TicketSeverityListService = new TicketSeverityListService();
        this.TicketClassificationListService = new TicketClassificationListService();

    }

    private SetUIProperties() {
        if (this.InternalMode) {
            this.ContactIdCustomFilter = "ContactIdCustomFilter";
            this.CompanyIdCustomFilter = null;
            //this.CustomerContactId = this.ContactId;
            this.UIProperties.SetEnabled("CustomerContactId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.CompanyId));
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, true);
        }
        else {
            this.CompanyIdCustomFilter = this.CompanyId;
            this.ContactIdCustomFilter = null;
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.CompanyId));
        }
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.MainClassificationId));
        if (!AppTool.IsNullOrEmpty(this.ShipmentNumber) || !AppTool.IsNullOrEmpty(this.QuoteNumber)) {
            this.EntityLinkNumberVisibility = true;
        }
        this.UIProperties.SetVisibility("CustomerContactId", this.ObjectTableName, this.InternalMode);
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CompanyId));

        this.SetUIProperties_EntityClosed();
    }
    private SetUIProperties_EntityClosed() {
        this.IsTicketEditEnabled = CRMTool.IsTicketEditEnabled(this.EntityPM);

        if (!this.IsTicketEditEnabled) {
            this.AddContactEnabled = false;
        }

        this.UIProperties.SetEnabled("ShipmentNumber", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("EntityType", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CompanyId", this.ObjectTableName, this. IsTicketEditEnabled);
        this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CustomerContactId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("Subject", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("SeverityId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CreateDate", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("MainClassificationId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("EmployeeGroupId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("OwnerId", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("InternalMode", this.ObjectTableName, this.IsTicketEditEnabled);


    }
    private isTicketEditEnabled;
    get IsTicketEditEnabled() { return this.isTicketEditEnabled; }
    set IsTicketEditEnabled(value: boolean) {
        if (this.isTicketEditEnabled != value) {
            this.isTicketEditEnabled = value;
        }
    }
        
    // Properties 
    get CompanyId() { return this.EntityPM.CompanyId; }
    set CompanyId(newValue: string) {
        if (this.EntityPM.CompanyId != newValue) {
            this.EntityPM.CompanyId = newValue;
            this.GetEntityLinkNumberVisibility();
            this.SetUIProperties();
            this.UpdateCompanyContact();
        }
    }
    UpdateCompanyContact() {
        if (this.IsFromOutSide) {
            if (!AppTool.IsNullOrEmpty(this.CompanyId) && !AppTool.IsNullOrEmpty(this.ContactId)) {
                // Check if connected or not 
                var myDomainService: CRMDomainService = new CRMDomainService();
                myDomainService.GetContactCards(this.CompanyId, this.ContactId).subscribe((myResult: any) => {
                    if (myResult == null) {
                        this.IsShowConnectContact = true;
                    }
                    else {
                        this.IsShowConnectContact = false;
                    }
                });
            }
        }
        else {
            if (this.InternalMode) {
                this.CustomerContactId = null;
            }
            else {
                this.ContactId = null;
            }
            if (!AppTool.IsNullOrEmpty(this.CompanyId)) {
                var myService: CardListService = new CardListService();
                myService.getSingle(this.CompanyId).subscribe((myResult: ServiceResponse) => {
                    if (!myResult.HasError) {
                        var list = myResult.Result;
                        if (list != null) {
                            if (this.InternalMode) {
                                this.CustomerContactId = list.PrimaryContactId;
                            }
                            else {
                                this.ContactId = list.PrimaryContactId;
                            }
                        }
                    }
                });
            }
        }
    }

    get ContactId() { return this.EntityPM.ContactId; }
    set ContactId(newValue: string) {
        if (this.EntityPM.ContactId != newValue) {
            this.EntityPM.ContactId = newValue;
            this.ContactListService.getSingle(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: ContactList = resp.Result;
                    if (result != null) {
                        this.EntityPM.ContactName = result.EnglishName;
                        this.EntityPM.ContactPhone = result.BusinessPhone;
                    }
                }
            });
        }
    }

    private  addContactEnabled;
    get AddContactEnabled()
    {
        this.addContactEnabled = false;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CompanyId) && this.IsTicketEditEnabled) {
            this.addContactEnabled = true;
        }

        return this.addContactEnabled;
    }
    set AddContactEnabled(value: boolean)
    {
        if (this.addContactEnabled != value) {
            this.addContactEnabled = value;
        }
    }

    get Subject() { return this.EntityPM.Subject; }
    set Subject(newValue: string) {
        if (this.EntityPM.Subject != newValue) {
            this.EntityPM.Subject = newValue;
        }
    }

    get MainClassificationId() { return this.EntityPM.MainClassificationId; }
    set MainClassificationId(newValue: string) {
        if (this.EntityPM.MainClassificationId != newValue) {
            this.EntityPM.MainClassificationId = newValue;
            this.getGeneralClassification();
            this.SecondaryClassificationId = null;
            this.SetUIProperties();
            this.setDeafaultsValues();

            this.TicketClassificationListService.getSingleFromCache(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: TicketClassificationList = resp.Result;
                    if (result != null) {
                        this.EntityPM.MainClassificationName = result.Name;
                    }
                }
            });
        }
    }

    setDeafaultsValues() {
        if (this.SecondaryClassificationId != null) {
            this.TicketClassificationListService.getSingleFromCache(this.SecondaryClassificationId).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result = resp.Result;
                    if (result != null) {
                        this.SeverityId = result.DefaultSeverityId;
                        this.EmployeeGroupId = result.EmployeeGroupId;
                    }
                }
            });
        }

        else {
            if (this.MainClassificationId != null) {
                this.TicketClassificationListService.getSingleFromCache(this.MainClassificationId).subscribe((resp: ServiceResponse) => {
                    if (!resp.HasError) {
                        var result = resp.Result;
                        if (result != null) {
                            this.SeverityId = result.DefaultSeverityId;
                            this.EmployeeGroupId = result.EmployeeGroupId;
                        }
                    }
                });
            }

            else {
                this.SeverityId = null;
                this.EmployeeGroupId = null;
            }
        }
    }

    get SecondaryClassificationId() { return this.EntityPM.SecondaryClassificationId; }
    set SecondaryClassificationId(newValue: string) {
        if (this.EntityPM.SecondaryClassificationId != newValue) {
            this.EntityPM.SecondaryClassificationId = newValue;
            this.setDeafaultsValues();

            this.TicketClassificationListService.getSingleFromCache(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: TicketClassificationList = resp.Result;
                    if (result != null) {
                        this.EntityPM.SecondaryClassificationName = result.Name;
                    }
                }
            });
        }
    }

    public FirstClassificationId: string = "";
    private getGeneralClassification() {
        this.FirstClassificationId = "";
        var myService: TicketClassificationListService = new TicketClassificationListService();
        var filters = new ApiQueryFilters();
        myService.getAllFromCache(filters).subscribe((resp: any) => {
            if (!resp.HasError) {
                var result = resp.Result;
                var myClassification = result.filter(d => d.Name == "General" && d.Tenant == SessionLocator.TenantPM.Id)[0];
                if (myClassification != null) {
                    var filter: string = "!F";
                    this.FirstClassificationId = myClassification.Id.concat(filter);
                }
            }
        });
    }

    get SecondClassificationId() {
        var myGeneralId = "";
        if (this.MainClassificationId != null) {
            var filter: string = "!S";
            myGeneralId = this.MainClassificationId.concat(filter);
        }
        return myGeneralId;
    }

    get SeverityId() { return this.EntityPM.SeverityId; }
    set SeverityId(newValue: string) {
        if (this.EntityPM.SeverityId != newValue) {
            this.EntityPM.SeverityId = newValue;
            this.TicketSeverityListService.getSingleFromCache(newValue).subscribe(result => {
                var severity: TicketSeverityList = result.Result;
                if (severity != null)
                    this.EntityPM.SeverityName = severity.Name;
                else
                    this.EntityPM.SeverityName = null;
            });
        }
    }

    get EmployeeGroupId() { return this.EntityPM.EmployeeGroupId; }
    set EmployeeGroupId(newValue: string) {
        if (this.EntityPM.EmployeeGroupId != newValue) {
            this.EntityPM.EmployeeGroupId = newValue;
            this.CheckOwnerEmployeeGroup();
        }
    }

    CheckOwnerEmployeeGroup() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetOwnerEmployeeGroup(this.OwnerId, this.EmployeeGroupId).subscribe((myResult: any) => {
            this.OwnerId = myResult;
        });
    }

    get OwnerId() { return this.EntityPM.OwnerId; }
    set OwnerId(newValue: string) {
        this.EntityPM.OwnerId = newValue;
        if (newValue == null) {
            this.EntityPM.BusinessUnitId = null;
            this.EntityPM.OwnerName = null;
        }
        else {
            var myService: UserListService = new UserListService();
            myService.getSingleFromCache(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: ServiceResponse = resp;
                    var list: UserList = result.Result;
                    if (list != null) {
                        this.EntityPM.BusinessUnitId = list.BusinessUnitId;
                        this.EntityPM.OwnerName = list.EnglishName;
                    }
                }
            });
        }
    }

    get EntityType() { return this.EntityPM.EntityType; }
    set EntityType(newValue: string) {
        if (this.EntityPM.EntityType != newValue) {
            this.EntityPM.EntityType = newValue;
            this.GetEntityLinkNumberVisibility();
        }
    }

    get ShipmentId() { return this.EntityPM.ShipmentId; }
    set ShipmentId(newValue: string) {
        if (this.EntityPM.ShipmentId != newValue) {
            this.EntityPM.ShipmentId = newValue;
        }
    }

    get ShipmentNumber() { return this.EntityPM.ShipmentNumber; }
    set ShipmentNumber(newValue: string) {
        this.EntityPM.ShipmentNumber = newValue;
        this.GetEntityLinkNumberVisibility();
    }

    get QuoteId() { return this.EntityPM.QuoteId; }
    set QuoteId(newValue: string) {
        this.EntityPM.QuoteId = newValue;
    }

    get QuoteNumber() { return this.EntityPM.QuoteNumber; }
    set QuoteNumber(newValue: string) {
        if (this.EntityPM.QuoteNumber != newValue) {
            this.EntityPM.QuoteNumber = newValue;
            this.GetEntityLinkNumberVisibility();
        }
    }

    get CreateDate() { return this.EntityPM.CreateDate; }
    set CreateDate(newValue: Date) {
        if (this.EntityPM.CreateDate != newValue) {
            this.EntityPM.CreateDate = newValue;
        }
    }

    get CustomerContactId() { return this.EntityPM.CustomerContactId; }
    set CustomerContactId(value: string) {
        if (this.EntityPM.CustomerContactId != value) {
            this.EntityPM.CustomerContactId = value;
        }
    }

    get InternalMode() { return this.EntityPM.InternalMode; }
    set InternalMode(value: boolean) {
        if (this.EntityPM.InternalMode != value) {
            this.EntityPM.InternalMode = value;
            this.SetCustomerContactValue();
            this.SetUIProperties();
        }
    }

    get InternalUsers() { return this.EntityPM.InternalUsers; }
    set InternalUsers(value: string) {
        if (this.EntityPM.InternalUsers != value) {
            this.EntityPM.InternalUsers = value;
        }
    }

    get CCs() { return this.EntityPM.CCs; }
    set CCs(value: string) {
        if (this.EntityPM.CCs != value) {
            this.EntityPM.CCs = value;
        }
    }

    public ContactIdCustomFilter: string;
    public CompanyIdCustomFilter: string;
    private SetCustomerContactValue() {
        this.UIProperties.SetVisibility("CustomerContactId", this.ObjectTableName, this.InternalMode);
        if (this.InternalMode) {
            this.ContactIdCustomFilter = "ContactIdCustomFilter";
            this.CustomerContactId = this.ContactId;
            if (!this.IsFromOutSide) {
                this.ContactId = null;
            }
            this.CompanyIdCustomFilter = null;
        }
        else {
            if (!this.IsFromOutSide) {
                this.ContactId = this.CustomerContactId;
            }
            this.CustomerContactId = null;
            this.CompanyIdCustomFilter = this.CompanyId;
            this.ContactIdCustomFilter = null;
        }
        this.SetUIProperties();
    }

    get IsEditContactEnabled() {
        return AppTool.IsNullOrEmpty(this.EntityPM.ContactId) ? false : true;
    }

    public EntityLinkNumberVisibility = false;
    GetEntityLinkNumberVisibility() {
        var myResult = false;
        if ((this.EntityObjectTableName == "Shipment" && !AppTool.IsNullOrEmpty(this.ShipmentNumber)) || (this.EntityObjectTableName == "Quote" && !AppTool.IsNullOrEmpty(this.QuoteNumber))) {
            myResult = true;
        }
        this.EntityLinkNumberVisibility = myResult;
    }

    QuickSearchTextChanged(entity: any) {
        if (this.EntityObjectTableName == "Shipment") {
            this.ShipmentNumber = entity.ShipmentNumber;
            this.ShipmentId = entity.Id;
            this.CompanyId = entity.CustomerId;
        }
        else {
            this.QuoteNumber = entity.QuoteNumber;
            this.QuoteId = entity.Id;
            this.CompanyId = entity.CustomerId;
        }
    }
    private entityObjectTableName: string;
    get EntityObjectTableName() { return this.entityObjectTableName; }
    set EntityObjectTableName(value: string) {
        if (this.entityObjectTableName != value) {
            this.entityObjectTableName = value;
            if (this.EntityObjectTableName == "Shipment") {
                this.EntityNumberTitle = "Shipment Number";
            }
            else {

                this.EntityNumberTitle = "Quote Number";
            }
            this.GetEntityLinkNumberVisibility();
        }
    }
    ChooseEntity() {
        if (this.IsTicketEditEnabled) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 800;
            logWindow.Height = 570;
            var args: any = {};
            args.IsFromTicket = true;
            if (this.EntityObjectTableName == "Shipment") {
                logWindow.Title = "Shipments Search";
            }
            else {
                logWindow.Title = "Quotes Search";
            }
            args.EntityObjectTableName = this.EntityObjectTableName;
            logWindow.WindowArgs = args;
            logWindow.Show('./CommonModules/CommonFilingInbox/FilingInbox/ChooseEntityComponent');
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    var entityList = null;
                    if (s.EntityObjectTableName == "Shipment") {
                        entityList = s.SelectedShipment;
                    }
                    else {
                        entityList = s.SelectedQuote;
                    }
                    if (entityList != null) {
                        if (s.EntityObjectTableName == "Shipment") {
                            this.ShipmentNumber = entityList.ShipmentNumber;
                            this.ShipmentId = entityList.Id;
                            this.CompanyId = entityList.CustomerId;
                        }
                        else {
                            this.QuoteNumber = entityList.QuoteNumber;
                            this.QuoteId = entityList.Id;
                            this.CompanyId = entityList.CustomerId;
                        }

                    }
                });
            });
        }
    }
    AddButtonClicked() {
        var path = './Quote/ComponentsNewEntity/NewQuoteComponent';
        var windowTitle = "New Quote";
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = windowTitle;
            logWindow.Show(path);
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    if (s && d == "OK") {
                        var quote = s.EntityPM;
                        if (quote != null) {
                            this.QuoteNumber = quote.QuoteNumber;
                            this.QuoteId = quote.Id;
                            this.CompanyId = quote.CustomerId;
                        }
                    }
                });
            });
        });
    }
    ViewShipmentClicked() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.ShipmentId, ObjectTableName: 'Shipment', BackButtonLabel: "Tickets" });
            });
    }
    ViewQuoteClicked() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.QuoteId, ObjectTableName: 'Quote', BackButtonLabel: "Tickets" });
            });
    }
    DeleteEntity() {
        if (this.IsTicketEditEnabled) {
            this.ShipmentId = null;
            this.QuoteId = null;
            this.ShipmentNumber = null;
            this.QuoteNumber = null;
            this.EntityLinkNumberVisibility = false;
        }
    }

    AddContact() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Contact";
        var args = new ContactInputTemplateArgs();
        args.CustomerId = this.CompanyId;
        args.CardDependencyProperty1 = "AG,AL,CC,CG,CO,CS,FL,OT,SG,SL,TR,VD,WH";
        args.CustomerLable = "Company";
        args.CardDependencyProperty1IsList = true;
        args.ComponentName = "Ticket";
        logWindow.WindowArgs = args;
        logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewContactComponent');
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewTicketWindowClosed($event));
    }
    OnNewTicketWindowClosed(arg: any) {
        if (arg != 'cancel') {
            if (this.InternalMode) {
                this.EntityPM.CustomerContactId = arg;
            }
            else {
                this.EntityPM.ContactId = arg;
            }
        }
    }
    ConectContactClicked() {
        this.CurrentSession.StartBusyIndicator("Connecting");
        var myDomainService: CRMDomainService = new CRMDomainService();
        myDomainService.GetConnectContactCards(this.CompanyId, this.ContactId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                this.IsShowConnectContact = false;
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }
    AddCompanyClicked() {
        var str = TextCodeTranslator.Translate("General.O.NewEntity");
        str = "New Potential Customer";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 660;
        logWindow.Height = 570;
        logWindow.Title = str;
        var args: any = {};
        args.ShowContactPart = true;
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d) {
                    var customer = s.EntityPM;
                    if (customer != null) {
                        this.CompanyId = customer.Id;
                    }
                }
            });
        });
    }
    EditContactClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "Update Contact";
        var myService: ContactPMService = new ContactPMService();
        myService.get(this.ContactId).subscribe((myResult: ServiceResponse) => {
            if (!myResult.HasError) {
                var pm = myResult.Result;
                var itemComponent = new ContactItemClass(pm, null, false);
                logWindow.DataContext = itemComponent;
                var args: any = {};
                logWindow.WindowArgs = args;
                logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditContactComponent');
                logWindow.WindowClosed.subscribe(($event: any) => this.OnEditContactWindowClosed($event));
            }
        });
    }
    OnEditContactWindowClosed(arg: any) {
        if (arg != 'cancel') {
            if (this.InternalMode) {
                this.CustomerContactId = arg;
            }
            else {
                this.ContactId = arg;
            }
        }
    }
}

export class EntityClass {
    public Code: string;
    public Name: string;
}
