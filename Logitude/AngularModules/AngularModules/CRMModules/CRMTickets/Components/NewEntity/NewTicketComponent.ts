import {Component, OnInit, ViewChild, Output, EventEmitter, ViewContainerRef} from '@angular/core';
import {TicketPM} from '../../../../CRM/EntityPMs/TicketPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TicketPMService} from '../../../../CRM/Services/StandardPMs/TicketPMService';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CRMDomainService} from '../../../../CRM/Services/CRMDomainService';
import {ShipmentList} from '../../../../Shipment/EntityLists/ShipmentList';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {TicketClassificationList} from '../../../../CRM/EntityLists/TicketClassificationList';
import {TicketClassificationListService} from '../../../../CRM/Services/StandardLists/TicketClassificationListService';
import {TicketStageList} from '../../../../CRM/EntityLists/TicketStageList';
import {TicketStageListService} from '../../../../CRM/Services/StandardLists/TicketStageListService';
import {TicketSourceList} from '../../../../CRM/EntityLists/TicketSourceList';
import {TicketSourceListService} from '../../../../CRM/Services/StandardLists/TicketSourceListService';
import {TicketCreatedByTypeList} from '../../../../CRM/EntityLists/TicketCreatedByTypeList';
import {TicketCreatedByTypeListService} from '../../../../CRM/Services/StandardLists/TicketCreatedByTypeListService';
import {ContactInputTemplateArgs} from '../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {TicketPMInitService} from '../../../../CRM/EntityPMInitServices/TicketPMInitService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {NewTicketArgs} from '../../../../CRM/Args';
declare var window: any;


@Component({
    selector: 'NewTicketComponent',
    moduleId: module.id,
    templateUrl: './NewTicketComponent.html',
})

export class NewTicketComponent extends BaseComponent implements OnInit {
    @Output() OnCloseWindow = new EventEmitter();
    public ObjectTableName: string = "Ticket";
    public TenantPM: TenantPM;
    public DataContext: NewTicketComponent = this;
    public EntityPM: TicketPM = new TicketPM();;
    private ScreenCode: string = "Ticket.AdditionalFields";
    public imgNgStyle: any = null;
    public IsVisible = false;
    public QuickSearchItems: ShipmentList[] = [];
    public Filters: ApiQueryFilters = null;
    public EntityList: EntityClass[] = [];
    public EntityNumberTitle = "Shipment Number";

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.Filters = new ApiQueryFilters();
        this.Filters.PageIndex = 0;
        this.Filters.PageSize = 10;
        if (!AppTool.IsNullOrEmpty(this.CompanyId)) {
            this.Filters.addAdditionalFilter("CustomerId", this.CompanyId, null, null, "Equals", false, false, false, "string");
        }
        // this.RunComponent();
        this.getGeneralClassification();
    }

    ngOnInit() {
        //this.SetFieldsEnabled();
        this.CreateTicket();
        this.SetUIProperties();
        this.SetUIRequiredProperties();
        this.SetCustomerContactValue();
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
        this.SetUIProperties();
        this.SetUIRequiredProperties();
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, this.ScreenCode);
            });
    }

    public WindowArgs: NewTicketArgs;
    SetWindowArgs(args: NewTicketArgs) {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            this.WindowArgs = args;
        });
    }

    private isDisabled: boolean = false;
    public get IsShipmentIdDisabled() {
        return this.isDisabled;
    }

    public set IsShipmentIdDisabled(value: boolean) {
        this.isDisabled = value;
    }

    CreateTicket() {
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new TicketPM();
        if (SessionLocator.TenantPM.IsInternalTicketByDefault == true) {
            this.EntityPM.InternalMode = true;
        }
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByContactId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.IsCancelled = false;
        this.EntityPM.IsClosed = false;
       this.EntityPM.EntityType = window.ObjectTables.filter(d => d.Name === "Shipment")[0].Id;

        if (this.WindowArgs != null) {
            this.CompanyId = this.WindowArgs.CompanyId;
        }
        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams) {
                if (SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "tickets") {
                    if (SessionLocator.ExternalParams.Action.toLocaleLowerCase() == "new") {
                        var cardVisible = false, shipmentVisible = false;
                        SessionLocator.ExternalParams.Args.forEach(arg => {

                            this.EntityPM[arg.FieldName] = arg.FieldValue;
                            if (arg.FieldName.toLocaleLowerCase() == "companycode") {
                                if (arg.FieldValue) {
                                    this.GetCardForCode(arg.FieldValue);
                                    cardVisible = true;
                                }
                                else {
                                    //cardVisible = true;
                                    this.isCardFinished = true;
                                }
                            }

                            if (arg.FieldName.toLocaleLowerCase() == "shipmentid") {
                                if (!AppTool.IsNullOrEmpty(arg.FieldValue)) {
                                    this.GetShipmentById(arg.FieldValue);
                                    shipmentVisible = true;
                                }
                                else {
                                    //shipmentVisible = true;
                                    this.isShipmentFinished = true;
                                }
                            }

                            this.UIProperties.SetEnabled(arg.FieldName, this.ObjectTableName, false);

                        });

                        if (!cardVisible || !shipmentVisible) {
                            this.IsVisible = true;
                        }

                        SessionLocator.ClearExternalParams();
                    }
                }
            }
        }
        else {
            this.IsVisible = true;
        }

        TicketPMInitService.InitValues(this.EntityPM, true);
    }

    isCardFinished = false;
    GetCardForCode(code: string) {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        var myService: CardListService = new CardListService();

        if (!AppTool.IsNullOrEmpty(code)) {
            filters.addAdditionalFilter("Code", code, null, null, "Equals", false, false, false, "string");
        }
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var cards = myResponse.Result;
                if (cards != null && cards.length > 0) {
                    var card = cards[0];
                    this.CompanyId = card.Id;
                    this.UIProperties.SetEnabled("CompanyId", this.ObjectTableName, false);
                }

                this.isCardFinished = true;
            }
            else {
                this.isCardFinished = true;
            }
            this.SetIsVisible();
        });
    }

    isShipmentFinished = false;
    GetShipmentById(id: string) {
        this.ShipmentNumber = null;
        this.ShipmentId = null;
        //var service = new ShipmentDomainService();
        var service = new ShipmentPMService();
        service.get(id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var shipment = myResponse.Result;
                if (shipment != null) {
                    this.IsShipmentIdDisabled = true;
                    this.ShipmentNumber = shipment.ShipmentNumber;
                    this.ShipmentId = shipment.Id;
                }
                this.isShipmentFinished = true;
            }
            else {
                this.isShipmentFinished = true;
            }
            this.SetIsVisible();
        });
    }

    SetIsVisible() {
        if (this.isCardFinished && this.isShipmentFinished) {
            this.IsVisible = true;
        }
    }

    SetUIProperties() {
        var descriptionIsRequired: boolean = AppTool.IsNullOrEmpty(this.TicketDescription) ? true : false;
        this.UIProperties.SetRequired("TicketDescription", this.ObjectTableName, descriptionIsRequired);
        if (this.InternalMode) {
            this.UIProperties.SetEnabled("CustomerContactId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.CompanyId));
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.CompanyId));
        }
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.MainClassificationId));
    }

    SetUIRequiredProperties() {
        this.UIProperties.SetRequired("EmployeeGroupId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.EmployeeGroupId));
        this.UIProperties.SetRequired("OwnerId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OwnerId));
        this.UIProperties.SetRequired("CompanyId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CompanyId));
        this.UIProperties.SetRequired("ShipmentNumber", this.ObjectTableName, AppTool.IsNullOrEmpty(this.CompanyId));
    }

    SetFieldsEnabled() {
        this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.CompanyId));
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.MainClassificationId));
    }

    // Properties 
    get IsAddContactEnabled() {
        return AppTool.IsNullOrEmpty(this.EntityPM.CompanyId) ? false : true;
    }

    public SupportNotes: string = null;

    get SupportNotesVisibility() {
        var myResult = false;
        if (!AppTool.IsNullOrEmpty(this.SupportNotes)) {
            myResult = true;
        }
        return myResult;
    }

    get CompanyId() { return this.EntityPM.CompanyId; }
    set CompanyId(newValue: string) {
        if (this.EntityPM.CompanyId != newValue) {
            this.EntityPM.CompanyId = newValue;
            this.GetCompanyCardData();
            this.SetUIRequiredProperties();
            this.UpdateCompanyContact();
            this.SetUIProperties();
            this.SetCustomerContactValue();
            this.Filters = new ApiQueryFilters();

            if (!AppTool.IsNullOrEmpty(newValue)) {
                this.Filters.addAdditionalFilter("CustomerId", newValue, null, null, "Equals", false, false, false, "string");
            }
        }
    }

    GetCompanyCardData() {
        var card: CardList = null;
        var myService: CardListService = new CardListService();
        myService.getSingle(this.CompanyId).subscribe((myResult: ServiceResponse) => {
            card = myResult.Result;
            if (card != null) {
                this.SupportNotes = card.SupportNotes;
            } else {
                this.SupportNotes = null;
            }
        });
    }

    UpdateCompanyContact() {
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

    get ShipmentId() { return this.EntityPM.ShipmentId; }
    set ShipmentId(newValue: string) {
        //if (this.EntityPM.ShipmentId != newValue) {
        this.EntityPM.ShipmentId = newValue;
        // }
    }

    get ShipmentNumber() { return this.EntityPM.ShipmentNumber; }
    set ShipmentNumber(newValue: string) {
        if (this.EntityPM.ShipmentNumber != newValue) {
            this.EntityPM.ShipmentNumber = newValue;
        }
    }

    get EntityType() { return this.EntityPM.EntityType; }
    set EntityType(newValue: string) {
        if (this.EntityPM.EntityType != newValue) {
            this.EntityPM.EntityType = newValue;
        }
    }

    get QuoteId() { return this.EntityPM.QuoteId; }
    set QuoteId(newValue: string) {
        this.EntityPM.QuoteId = newValue;
    }

    get QuoteNumber() { return this.EntityPM.QuoteNumber; }
    set QuoteNumber(newValue: string) {
        if (this.EntityPM.QuoteNumber != newValue) {
            this.EntityPM.QuoteNumber = newValue;
        }
    }

    get ContactId() { return this.EntityPM.ContactId; }
    set ContactId(newValue: string) {
        if (this.EntityPM.ContactId != newValue) {
            this.EntityPM.ContactId = newValue;
        }
    }

    get Subject() { return this.EntityPM.Subject; }
    set Subject(newValue: string) {
        if (this.EntityPM.Subject != newValue) {
            this.EntityPM.Subject = newValue;
        }
    }

    get TicketDescription() { return this.EntityPM.TicketDescription; }
    set TicketDescription(newValue: string) {
        if (this.EntityPM.TicketDescription != newValue) {
            this.EntityPM.TicketDescription = newValue;
            this.SetUIProperties();
        }
    }

    get ClosureDescription() { return this.EntityPM.ClosureDescription; }
    set ClosureDescription(newValue: string) {
        if (this.EntityPM.ClosureDescription != newValue) {
            this.EntityPM.ClosureDescription = newValue;
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
        }
    }

    setDeafaultsValues() {
        if (this.SecondaryClassificationId != null) {
            var myService: TicketClassificationListService = new TicketClassificationListService();
            myService.getSingleFromCache(this.SecondaryClassificationId).subscribe((resp: any) => {
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

                var myService: TicketClassificationListService = new TicketClassificationListService();
                myService.getSingleFromCache(this.MainClassificationId).subscribe((resp: ServiceResponse) => {
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
        }
    }

    get TicketTypeId() { return this.EntityPM.TicketTypeId; }
    set TicketTypeId(newValue: string) {
        if (this.EntityPM.TicketTypeId != newValue) {
            this.EntityPM.TicketTypeId = newValue;
        }
    }

    get EmployeeGroupId() { return this.EntityPM.EmployeeGroupId; }
    set EmployeeGroupId(newValue: string) {
        if (this.EntityPM.EmployeeGroupId != newValue) {
            this.EntityPM.EmployeeGroupId = newValue;
            this.CheckOwnerEmployeeGroup();
            this.SetUIRequiredProperties();
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
        this.SetUIRequiredProperties();

        if (newValue == null) {
            this.EntityPM.BusinessUnitId = null;
        }

        else {
            var myService: UserListService = new UserListService();
            myService.getSingleFromCache(newValue).subscribe((resp: any) => {
                if (!resp.HasError) {
                    var result: ServiceResponse = resp;
                    var list: UserList = result.Result;
                    if (list != null) {
                        this.EntityPM.BusinessUnitId = list.BusinessUnitId;
                    }
                }
            });
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

    public ContactIdCustomFilter: string;
    public CompanyIdCustomFilter: string;
    private SetCustomerContactValue() {
        this.UIProperties.SetVisibility("CustomerContactId", this.ObjectTableName, this.InternalMode);
        if (this.InternalMode) {
            this.ContactIdCustomFilter = "ContactIdCustomFilter";
            this.CustomerContactId = this.ContactId;
            this.ContactId = null;
            this.CompanyIdCustomFilter = null;
        }
        else {
            this.ContactId = this.CustomerContactId;
            this.CustomerContactId = null;
            this.CompanyIdCustomFilter = this.CompanyId;
            this.ContactIdCustomFilter = null;
        }
        this.SetUIProperties();
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.OwnerId)) {
            errors.push("Owner is required");
        }

        if (AppTool.IsNullOrEmpty(this.EmployeeGroupId)) {
            errors.push("Employee Group field is required");
        }

        if (AppTool.IsNullOrEmpty(this.CompanyId)) {
            errors.push("Company field is required");
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreatingTicket();
        }
    }
    SubmitCreatingTicket() {
        this.CurrentSession.StartBusyIndicator("Creating...");

        var myService: CRMDomainService = new CRMDomainService();
        myService.InserNewTicket(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    if (this.WindowArgs != null) {
                        // Ticket Number
                        this.WindowArgs.TicketNumber = this.EntityPM.TicketNumber;
                    }

                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }

                else {
                    this.ValidationErrorsList = myRespone.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
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
    ChooseShipment() {
        if (!this.IsShipmentIdDisabled) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 800;
            logWindow.Height = 570;
            logWindow.Title = "Shipments Search";
            logWindow.WindowArgs = this.EntityPM;
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
        }
    }
    ChooseEntity() {
        if (!this.IsShipmentIdDisabled && this.EntityType != null) {
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
                    if (s) {
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
}

export class EntityClass {
    public Code: string;
    public Name: string;
}
