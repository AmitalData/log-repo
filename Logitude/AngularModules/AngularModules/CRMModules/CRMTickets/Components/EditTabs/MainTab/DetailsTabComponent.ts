import {Component, OnInit, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';
import {TicketMainTabComponent} from '../MainTab/TicketMainTabComponent';
import {TextCodeTranslationPipe} from '../../../../../Controls/Pipes/TextCodeTranslationPipe';
import {UserList} from '../../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../../Common/Services/StandardLists/UserListService';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {CardListService} from '../../../../../Common/Services/StandardLists/CardListService';
import {TicketClassificationList} from '../../../../../CRM/EntityLists/TicketClassificationList';
import {TicketClassificationListService} from '../../../../../CRM/Services/StandardLists/TicketClassificationListService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {CRMDomainService} from '../../../../../CRM/Services/CRMDomainService';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ShipmentList} from '../../../../../Shipment/EntityLists/ShipmentList';
import {CRMTool} from '../../../../../CRM/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {TicketSeverityList} from '../../../../../CRM/EntityLists/TicketSeverityList';
import {TicketSeverityListService} from '../../../../../CRM/Services/StandardLists/TicketSeverityListService';
import {ContactList} from '../../../../../Common/EntityLists/ContactList';
import {ContactListService} from '../../../../../Common/Services/StandardLists/ContactListService';
import {ContactInputTemplateArgs} from '../../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator'; 
import {ContactItemClass} from '../../../../../CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent';
import {ContactPMService} from '../../../../../Common/Services/StandardPMs/ContactPMService';
import {CachedDataManager} from '../../../../../Infrastructure/Utilities/CachedDataManager';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
declare var window: any;

@Component({
    selector: 'DetailsTabComponent',
    moduleId: module.id,
    templateUrl: './DetailsTabComponent.html',
})

export class DetailsTabComponent extends BaseComponent implements AfterViewInit {

    public EntityPM: TicketPM;
    public Trigger: TicketMainTabComponent;
    public ObjectTableName: string;
    public DataContext: DetailsTabComponent = this;
    public Filters: ApiQueryFilters = null;
    public QuickSearchItems: ShipmentList[] = [];
    public IsFromOutSide = false;
    public IsShowConnectContact = false;

    public EntityList: EntityClass[] = [];
    public EntityNumberTitle = "Shipment Number";
    _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        //this.Listen();
        this.InitializeServices();
        
    }
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
    }

    ClearFilterData() {
        this.ShipmentId = null;
        this.ShipmentNumber = null;
        this.QuoteId = null;
        this.QuoteNumber = null;
    }

    ContactListService: ContactListService;
    TicketSeverityListService: TicketSeverityListService;
    TicketClassificationListService: TicketClassificationListService;
    InitializeServices() {
        this.ContactListService = new ContactListService();
        this.TicketSeverityListService = new TicketSeverityListService();
        this.TicketClassificationListService = new TicketClassificationListService();
    }
    ngAfterViewInit() {
        //if (this.Trigger.EntityPM != null && this.Trigger.EntityPM.TicketCorrespondence != null && this.Trigger.EntityPM.TicketCorrespondence[0].Direction == "O") {
        if (this.Trigger.EntityPM.Source == "MAL") {
            this.IsFromOutSide = true;
        }
        else {
            this.IsFromOutSide = false;
        }
    }
    InitTab(trigger: TicketMainTabComponent) {
        this.Trigger = trigger;
        this.EntityPM = this.Trigger.EntityPM;
        var objectTable = window.ObjectTables.filter(d => d.Id === this.Trigger.EntityPM.EntityType)[0];
        if (objectTable != null) {
            var objectTableName = window.ObjectTables.filter(d => d.Id === this.Trigger.EntityPM.EntityType)[0].Name;
            if (objectTableName == "Quote") {
                this.EntityNumberTitle = "Quote Number";
            }
        }
        
        this.Filters = new ApiQueryFilters();
        this.Filters.PageIndex = 0;
        this.Filters.PageSize = 10;
        if (!AppTool.IsNullOrEmpty(this.CompanyId)) {
            this.Filters.addAdditionalFilter("CustomerId", this.CompanyId, null, null, "Equals", false, false, false, "string");
        }
        this.ObjectTableName = this.Trigger.ObjectTableName;
        this.SetUIProperties();
        this.getGeneralClassification();
    }
    RefreshTab(trigger: TicketMainTabComponent) {
        this.EntityPM = this.Trigger.EntityPM;
    }
    SetUIProperties() {
        var descriptionIsRequired: boolean = AppTool.IsNullOrEmpty(this.TicketDescription) ? true : false;
        this.UIProperties.SetRequired("TicketDescription", this.ObjectTableName, descriptionIsRequired);

        if (this.InternalMode) {
            this.ContactIdCustomFilter = "ContactIdCustomFilter";
            //this.CustomerContactId = this.ContactId;
            this.CompanyIdCustomFilter = null;
        }
        else {
            this.ContactIdCustomFilter = null;
            this.CompanyIdCustomFilter = this.CompanyId;
        }
        this.UIProperties.SetVisibility("CustomerContactId", this.ObjectTableName, this.InternalMode);
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.MainClassificationId));
        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OwnerId));
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CompanyId));

        this.GetEntityLinkNumberVisibility();
        this.SetUIProperties_EntityClosed();
    }
    public IsTicketEditEnabled = false;
    private SetUIProperties_EntityClosed() {
        this.IsTicketEditEnabled = CRMTool.IsTicketEditEnabled(this.Trigger.EntityPM);
        this.UIProperties.SetEnabled("ShipmentNumber", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("ShipmentNumber", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("CompanyId", this.ObjectTableName, this.IsTicketEditEnabled);
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
        this.UIProperties.SetEnabled("TicketDescription", this.ObjectTableName, this.IsTicketEditEnabled);
        this.UIProperties.SetEnabled("ClosureDescription", this.ObjectTableName, this.IsTicketEditEnabled);
    }

    // Properties 
    get CompanyId() { return this.Trigger.EntityPM.CompanyId; }
    set CompanyId(newValue: string) {
        if (this.Trigger.EntityPM.CompanyId != newValue) {
            this.Trigger.EntityPM.CompanyId = newValue;
            this.SetUIProperties();
            this.UpdateCompanyContact();
            this.Filters = new ApiQueryFilters();
            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.Filters.addAdditionalFilter("CustomerId", newValue, null, null, "Equals", false, false, false, "string");
            }
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

    get ShipmentId() { return this.Trigger.EntityPM.ShipmentId; }
    set ShipmentId(newValue: string) {
        if (this.Trigger.EntityPM.ShipmentId != newValue) {
            this.Trigger.EntityPM.ShipmentId = newValue;
        }
    }

    get ShipmentNumber() { return this.Trigger.EntityPM.ShipmentNumber; }
    set ShipmentNumber(newValue: string) {
        this.Trigger.EntityPM.ShipmentNumber = newValue;
        this.GetEntityLinkNumberVisibility();
    }

    get QuoteId() { return this.Trigger.EntityPM.QuoteId; }
    set QuoteId(newValue: string) {
        this.Trigger.EntityPM.QuoteId = newValue;
    }

    get QuoteNumber() { return this.Trigger.EntityPM.QuoteNumber; }
    set QuoteNumber(newValue: string) {
        if (this.Trigger.EntityPM.QuoteNumber != newValue) {
            this.Trigger.EntityPM.QuoteNumber = newValue;
            this.GetEntityLinkNumberVisibility();
        }
    }

    get ContactId() { return this.Trigger.EntityPM.ContactId; }
    set ContactId(newValue: string) {
        //if (this.Trigger.EntityPM.ContactId != newValue) {
            this.Trigger.EntityPM.ContactId = newValue;
            this.ContactListService.getSingle(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: ContactList = resp.Result;
                    if (result != null) {
                        this.Trigger.EntityPM.ContactName = result.EnglishName;
                        this.Trigger.EntityPM.ContactEmail = result.Email;
                        this.Trigger.EntityPM.ContactPhone = result.BusinessPhone;
                    }
                }
            });
        //}
    }

    get Subject() { return this.Trigger.EntityPM.Subject; }
    set Subject(newValue: string) {
        if (this.Trigger.EntityPM.Subject != newValue) {
            this.Trigger.EntityPM.Subject = newValue;
        }
    }

    get TicketDescription() { return this.Trigger.EntityPM.TicketDescription; }
    set TicketDescription(newValue: string) {
        if (this.Trigger.EntityPM.TicketDescription != newValue) {
            this.Trigger.EntityPM.TicketDescription = newValue;
            this.SetUIProperties();
        }
    }

    get ClosureDescription() { return this.Trigger.EntityPM.ClosureDescription; }
    set ClosureDescription(newValue: string) {
        if (this.Trigger.EntityPM.ClosureDescription != newValue) {
            this.Trigger.EntityPM.ClosureDescription = newValue;
        }
    }

    get MainClassificationId() { return this.Trigger.EntityPM.MainClassificationId; }
    set MainClassificationId(newValue: string) {
        if (this.Trigger.EntityPM.MainClassificationId != newValue) {
            this.Trigger.EntityPM.MainClassificationId = newValue;
            this.getGeneralClassification();
            this.SecondaryClassificationId = null;
            this.SetUIProperties();
            this.setDeafaultsValues();

            this.TicketClassificationListService.getSingleFromCache(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: TicketClassificationList = resp.Result;
                    if (result != null) {
                        this.Trigger.EntityPM.MainClassificationName = result.Name;
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

    get SecondaryClassificationId() { return this.Trigger.EntityPM.SecondaryClassificationId; }
    set SecondaryClassificationId(newValue: string) {
        if (this.Trigger.EntityPM.SecondaryClassificationId != newValue) {
            this.Trigger.EntityPM.SecondaryClassificationId = newValue;
            this.setDeafaultsValues();

            this.TicketClassificationListService.getSingleFromCache(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: TicketClassificationList = resp.Result;
                    if (result != null) {
                        this.Trigger.EntityPM.SecondaryClassificationName = result.Name;
                    }
                }
            });
        }
    }

    public FirstClassificationId: string = "";
    private getGeneralClassification() {
        this.FirstClassificationId = "";
        var filters = new ApiQueryFilters();
        this.TicketClassificationListService.getAllFromCache(filters).subscribe((resp: any) => {
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

    get SeverityId() { return this.Trigger.EntityPM.SeverityId; }
    set SeverityId(newValue: string) {
        if (this.Trigger.EntityPM.SeverityId != newValue) {
            this.Trigger.EntityPM.SeverityId = newValue;
            this.TicketSeverityListService.getSingleFromCache(newValue).subscribe(result => {
                var severity: TicketSeverityList = result.Result;
                if (severity != null)
                    this.Trigger.EntityPM.SeverityName = severity.Name;
                else
                    this.Trigger.EntityPM.SeverityName = null;
            });
        }
    }

    get EmployeeGroupId() { return this.Trigger.EntityPM.EmployeeGroupId; }
    set EmployeeGroupId(newValue: string) {
        if (this.Trigger.EntityPM.EmployeeGroupId != newValue) {
            this.Trigger.EntityPM.EmployeeGroupId = newValue;
            this.CheckOwnerEmployeeGroup();
        }
    }

    CheckOwnerEmployeeGroup() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetOwnerEmployeeGroup(this.OwnerId, this.EmployeeGroupId).subscribe((myResult: any) => {
            this.OwnerId = myResult;
        });
    }

    get OwnerId() { return this.Trigger.EntityPM.OwnerId; }
    set OwnerId(newValue: string) {
        this.Trigger.EntityPM.OwnerId = newValue;
        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OwnerId));
        if (newValue == null) {
            this.Trigger.EntityPM.BusinessUnitId = null;
            this.Trigger.EntityPM.OwnerName = null;
        }

        else {
            var myService: UserListService = new UserListService();
            myService.getSingleFromCache(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: ServiceResponse = resp;
                    var list: UserList = result.Result;
                    if (list != null) {
                        this.Trigger.EntityPM.BusinessUnitId = list.BusinessUnitId;
                        this.Trigger.EntityPM.OwnerName = list.EnglishName;
                    }
                }
            });
        }
    }

    get CreateDate() { return this.Trigger.EntityPM.CreateDate; }
    set CreateDate(newValue: Date) {
        if (this.Trigger.EntityPM.CreateDate != newValue) {
            this.Trigger.EntityPM.CreateDate = newValue;
        }
    }

    get CustomerContactId() { return this.Trigger.EntityPM.CustomerContactId; }
    set CustomerContactId(value: string) {
        if (this.Trigger.EntityPM.CustomerContactId != value) {
            this.Trigger.EntityPM.CustomerContactId = value;
        }
    }

    get IsEditContactEnabled() {
        return AppTool.IsNullOrEmpty(this.EntityPM.ContactId) ? false : true;
    }

    get InternalMode() { return this.Trigger.EntityPM.InternalMode; }
    set InternalMode(value: boolean) {
        if (this.Trigger.EntityPM.InternalMode != value) {
            this.Trigger.EntityPM.InternalMode = value;
            this.SetCustomerContactValue();
            this.SetUIProperties();
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

    private entityObjectTableName: string;
    get EntityObjectTableName() { return this.entityObjectTableName; }
    set EntityObjectTableName(value: string) {
        if (this.entityObjectTableName != value) {
            this.entityObjectTableName = value;
            if (value == "Shipment") {
                this.EntityNumberTitle = "Shipment Number";
            }
            else {

                this.EntityNumberTitle = "Quote Number";
            }
            this.GetEntityLinkNumberVisibility();
        }
    }


    get EntityType() { return this.Trigger.EntityPM.EntityType; }
    set EntityType(value: string) {
        if (this.Trigger.EntityPM.EntityType != value) {
            this.Trigger.EntityPM.EntityType = value;
            this.ShipmentNumber = null;
            this.QuoteNumber = null;
            this.GetEntityLinkNumberVisibility();
        }
    }

    public EntityLinkNumberVisibility = false;
    GetEntityLinkNumberVisibility() {
        var myResult = false;
        if ((this.EntityObjectTableName == "Shipment" && !AppTool.IsNullOrEmpty(this.ShipmentNumber)) || (this.EntityObjectTableName == "Quote" && !AppTool.IsNullOrEmpty(this.QuoteNumber))) {
            myResult = true;
        }
        this.EntityLinkNumberVisibility = myResult;
    }

    ChooseShipment() {
        if (this.IsTicketEditEnabled) {

            var logWindow = new LogitudeWindow();
            logWindow.Width = 800;
            logWindow.Height = 570;
            logWindow.Title = "Shipments Search";
            logWindow.WindowArgs = this.Trigger.EntityPM;
            logWindow.Show('./CRMModules/CRMTickets/Components/NewEntity/ChooseShipmentComponent');
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    var shipmentList = s.SelectedShipment;
                    if (shipmentList != null) {
                        this.ShipmentId = shipmentList.Id;
                        this.ShipmentNumber = shipmentList.ShipmentNumber;
                        this.CompanyId = shipmentList.CustomerId;
                    }
                });
            });
        }
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
            logWindow.Show('./CommonModules/CommonFilingInbox/Components/ChooseEntityComponent');
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
                            this.ViewQuoteClicked();

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
            // Refresh user table
            CachedDataManager.RefreshTableData("User", true);
            this.ContactId = arg;
        }
    }
}

export class EntityClass {
    public Code: string;
    public Name: string;
}
