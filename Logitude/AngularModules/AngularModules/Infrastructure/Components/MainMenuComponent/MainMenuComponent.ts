declare var window: any;
import {Component, ViewChildren, QueryList, ViewChild, ViewContainerRef, Output, EventEmitter} from '@angular/core'
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {LocationDirective} from '../../Utilities/LocationDirective';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {FeatureLocator} from '../../Utilities/FeatureLocator';
import {LastFilterClass} from '../../Utilities/LastFilterClass';
import {AppTool} from '../../Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs} from '../../../Infrastructure/Args';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ObjectTablePM} from '../../../Infrastructure/EntityPMs/ObjectTablePM';
import {QueryPM} from '../../../Infrastructure/EntityPMs/QueryPM';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';
import {ServiceLocator} from '../../Locators/ServiceLocator';
import { retry } from 'rxjs/operators';

@Component({
    moduleId: module.id,
    templateUrl: './MainMenuComponent.html',
})

export class MainMenuComponent {
    public SelectedMenu: MainMenuItem;
    public MainMenuItems: Array<MainMenuItem>;
    public MainMenuWidth: number = 145;
    private MainMenuWidthCollapsed: number = 45;
    private MainMenuWidthOpened: number = 145;

    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild("MainMenuContainer", { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    LayoutDirection: string = 'ltr';
    @Output() SelectionChanging: EventEmitter<any> = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.MainMenuItems = new Array<MainMenuItem>();
        this.MainMenuItems = this.GetMainMenuItemsFromWindow();
        // Layout Direction
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        var defaultStatus: string = LastFilterClass.GetFilterValue("Simplog.Infrastructure.Views.MenuView", "Sidebar");
        if (!AppTool.IsNullOrEmpty(defaultStatus)) {
            this.IsMainSidebarCollapsed = defaultStatus == "true" ? true : false;
        } else {
            this.IsMainSidebarCollapsed = SessionLocator.IsMainSidebarCollapsed;
        }
        this.MainMenuWidth = this.IsMainSidebarCollapsed == true ? this.MainMenuWidthCollapsed : this.MainMenuWidthOpened;
    }

    private GetMainMenuItemsFromWindow() {

        var myResult: MainMenuItem[] = [];

        window.MenusTables.filter(f => f.MenuTypeCode.toUpperCase() == "MAIN").forEach((item) => {

       
            var isAddingItem = false;

            if (item.FeatureId == null) {
                isAddingItem = true;
            }

            else {
                if (FeatureLocator.IsFeatureGranted(item.FeatureId)) {
                    isAddingItem = true;
                }
            }
         
            if (isAddingItem) {
                var menuItem: MainMenuItem = new MainMenuItem(item.TextCode, AppTool.GetMainMenuIconCode(item.TextCode));
                menuItem.IndexOfOrder = item.IndexOfOrder;
                menuItem.ObjectTableId = item.ObjectTableId;
                menuItem.HtmlView = item.HtmlView;
                menuItem.ObjectTableName = item.ObjectTableName;
                myResult.push(menuItem);
            }
        });

        myResult = myResult.sort((a, b) => { return a.IndexOfOrder - b.IndexOfOrder });
        return myResult;
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }

            else {
             

                this.isLoaderReady = true;

                let locs = this.AllLocations.toArray().filter(f => f.Code == 'MainMenuContainer');
                let myLocation: LocationDirective = locs[0];
                this.CurrentSession.SessionMenuLocation = myLocation;

                this.InitSelectedMenu();
            }
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

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    InitSelectedMenu() {

      
        
        var mySelectedMenu = this.MainMenuItems[0];

        var selectedMenuTextCode: string = null;

        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams != null) {
                switch (SessionLocator.ExternalParams.Menu) {
                    case "Tickets": {
                        selectedMenuTextCode = "General.MH.Ticket";
                        break;
                    }
                    case "LogBox": {
                        selectedMenuTextCode = "General.MH.Importers";
                        break;
                    }
                    case "DAPP": {
                        selectedMenuTextCode = "General.MH.Importers";
                        break;
                    }
                    case "protractor": {
                        selectedMenuTextCode = "General.MH.Maintenance";
                        break;
                    }
                }
            }
        }

        else {
            if (SessionLocator.LoggedUserPM.DisplayGettingStarted && this.CurrentSession.SessionIndex == 0) {
                selectedMenuTextCode = "General.MH.GettingStarted";
            }

            else {
                selectedMenuTextCode = LastFilterClass.GetFilterValue("Simplog.Infrastructure.Views.MenuView", "MainMenu");
            }
        }

        if (!AppTool.IsNullOrEmpty(selectedMenuTextCode)) {
            var mySelectedItem: MainMenuItem = this.MainMenuItems.filter(m => m.TextCode == selectedMenuTextCode)[0];
            if (mySelectedItem != null) {
                mySelectedMenu = mySelectedItem;
            }
        }

        this.SelectionChanged(mySelectedMenu);
    }

    private isChangingSelected: boolean = false;
    private ClickedMenuItem: MainMenuItem = null;
    public BlockScreenLoad() {
        if (!AppTool.IsNullOrEmpty(SessionLocator.BlockType)) {
            this.CurrentSession.DestroyMenuReferences();
            this.CurrentSession.DestroyListComponentReferences();
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/LoginComponent/BlockScreenComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                });

        }
    }
    SelectionChanged(item: MainMenuItem) {
        if (this.ClickedMenuItem != item) {

            this.ClickedMenuItem = item;
            // Code
            if (!AppTool.IsNullOrEmpty(SessionLocator.BlockType)) {

                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/LoginComponent/BlockScreenComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                    });

            }
            else {


                this.SelectionChanging.emit(true);

                var isSubscribed: boolean = false;
                if (this.SelectionChanging) {
                    if (this.SelectionChanging.observers) {
                        if (this.SelectionChanging.observers.length > 0) {
                            isSubscribed = true;
                        }
                    }
                }

                if (isSubscribed == false) {
                    this.ChangeMenu();
                }
            }
        }
        else {
            this.SelectionChanging.emit(true);
        }
    }

    ChangeMenu() {
        if (!this.isChangingSelected) {

            this.isChangingSelected = true;

            if (this.SelectedMenu != this.ClickedMenuItem) {
                this.SelectedMenu = this.ClickedMenuItem;
                this.ChangeScreen();
            }

            else {
                this.isChangingSelected = false;
            }
        }
    }

    public ShowFollowUps: boolean = false;
    public FollowUpsTableId: string = null;
    public OldObjectTable: string = null;
    public pointerEvents: string = 'all';
    // count: number = 0;
    ChangeScreen() {
        if (this.isLoaderReady) {

            this.ShowFollowUps = false;
            var myComponentPath: string = null;
            var isListComponent: boolean = false;

            //if (this.count % 2 == 0) {
                this.CurrentSession.DestroyMenuReferences();
                this.CurrentSession.DestroyListComponentReferences();
               // this.count++;
           // }

            if (!SessionLocator.IsExternalParams) {
                var defaultFilterCode: string = LastFilterClass.GetFilterValue("Simplog.Infrastructure.Views.MenuView", "MainMenu");
                if (defaultFilterCode != this.SelectedMenu.TextCode) {
                    LastFilterClass.UpdateFilter("Simplog.Infrastructure.Views.MenuView", "MainMenu", this.SelectedMenu.TextCode);
                }
            }

            if (this.SelectedMenu != null) {
                
                switch (this.SelectedMenu.TextCode) {
                    case "General.MH.Operations": {
                        ServiceLocator.SendTotangoUserActivity("Operations", "Main View");
                        myComponentPath = "./Shipment/Components/Workspaces/OperationsComponent";

                        if (FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups")) {
                            this.FollowUpsTableId = this.SelectedMenu.ObjectTableId;
                            this.ShowFollowUps = true;
                        }
                        break;
                    }

                    case "General.MH.ContainersFU": {
                        myComponentPath = "./Shipment/Components/Workspaces/ContainersFUsComponent";
                        break;
                    }

                    case "General.MH.TimeManagement": {
                        myComponentPath = "./TimeManagement/Components/Workspaces/TimeManagementWorkspaceComponent"; 
                        break;
                    }

                    case "General.MH.FilingInbox": {
                        myComponentPath = "./CommonModules/CommonFilingInbox/Components/FilingInboxWorkspaceComponent";
                        break;
                    }

                    case "General.MH.Quotes": {
                        ServiceLocator.SendTotangoUserActivity("Quote", "List View");
                        myComponentPath = "./Quote/Components/Workspaces/QuotesComponent";

                        if (FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups")) {
                            this.FollowUpsTableId = this.SelectedMenu.ObjectTableId;
                            this.ShowFollowUps = true;
                        }

                        break;
                    }
                    case "General.MH.Dashboard": {
                        ServiceLocator.SendTotangoUserActivity("Dashboard", "Main View");
                        myComponentPath = "./Dashboard/Components/Workspace/DashboardComponent";
                        break;
                    }

                    case "General.MH.AirlineDashboard": {
                        ServiceLocator.SendTotangoUserActivity("Dashboard", "Main View");
                        myComponentPath = "./Dashboard/Components/Workspace/AirLineDashboardComponent";
                        break;
                    }
                    case "General.MH.CRM": {
                        ServiceLocator.SendTotangoUserActivity("CRM", "Main View");
                        myComponentPath = "./CRM/Components/Workspaces/CRMWorkspaceComponent";
                        break;
                    }
                    case "General.MH.Ticket": {
                        ServiceLocator.SendTotangoUserActivity("Ticket", "List View");
                        myComponentPath = "./CRM/Components/Workspaces/TicketsWorkspaceComponent";
                        break;
                    }
                    case "General.MH.Importers": {
                        ServiceLocator.SendTotangoUserActivity("Importers", "Main View");
                        myComponentPath = "./ShipmentModules/ShipmentLogBox/Components/Logbox/LogBoxMainComponent";
                        break;
                    }
                    case "General.MH.Accounting": {
                        ServiceLocator.SendTotangoUserActivity("Accounting", "Main View");
                        myComponentPath = "./Invoice/Components/Workspaces/InvoiceComponent";
                        break;
                    }
                    case "General.MH.Reports": {
                        ServiceLocator.SendTotangoUserActivity("Reports", "Main View");
                        //myComponentPath = "./Report/Components/Workspaces/ReportComponent";
                        myComponentPath = "./Report/Components/Workspaces/MainReportsWorkspace";
                        break;
                    }
                    case "General.MH.Maintenance": {
                        ServiceLocator.SendTotangoUserActivity("Maintenance", "Main View");
                        myComponentPath = "./Infrastructure/Components/Maintenance/MaintenanceComponent";
                        break;
                    }
                    case "General.MH.GettingStarted": {
                        ServiceLocator.SendTotangoUserActivity("GettingStarted", "Main View");
                        myComponentPath = "./InfrastructureModules/InfrastructureGettingStarted/Components/Workspaces/GettingStartedComponent";
                        break;
                    }
                    case "General.MH.FullAccounting": {
                        ServiceLocator.SendTotangoUserActivity("FullAccounting", "Main View");
                        myComponentPath = "./Accounting/Components/Workspaces/AccountingWorkspaceComponent";
                        break;
                    }
                    case "General.MH.SharedLogistics": {
                        ServiceLocator.SendTotangoUserActivity("SharedLogistics", "Main View");
                        myComponentPath = "./SharedLogistics/Components/SharedLogisticMainMenuComponent";
                        break;
                    }

                    case "General.MH.Shipments": {
                        ServiceLocator.SendTotangoUserActivity("SharedLogistics", "Main View");
                        myComponentPath = "./SharedLogistics/Components/Workspaces/SharedShipmentsWorkspaceComponent";
                        break;
                    }

                    case "General.MH.Invoices": {
                        ServiceLocator.SendTotangoUserActivity("SharedLogistics", "Main View");
                        myComponentPath = "./SharedLogistics/Components/Workspaces/SharedInvoicesWorkspaceComponent";
                        break;
                    }

                    case "General.MH.ActivationWizard": {
                        ServiceLocator.SendTotangoUserActivity("ActivationWizard", "Main View");
                        myComponentPath = "./InfrastructureModules/InfrastructureOthers/Components/ActivationWizard/ActivationWizardComponent";
                        break;
                    }                    
                    case "General.MH.Customers": {
                        ServiceLocator.SendTotangoUserActivity("Customers", "List View");
                        var listArgs = new ListComponentArgs();
                        listArgs.Perspective = "customers";
                        listArgs.QueryCode = "Customers";
                        listArgs.ObjectTableName = "Customer";
                        listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(response => {
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(cmpRef => {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run(listArgs);
                                    this.CurrentSession.AddMenuReference(cmpRef);
                                    this.ChangeSessionHeader(this.SelectedMenu);
                                    this.isChangingSelected = false;
                                    //this.pointerEvents = 'all';

                                });
                        });

                        break;
                    }
                    case "General.MH.Contacts": {
                        ServiceLocator.SendTotangoUserActivity("Contacts", "List View");
                        var listArgs = new ListComponentArgs();
                        listArgs.QueryCode = "Contacts";
                        listArgs.ObjectTableName = "Contact";
                        listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(response => {
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(cmpRef => {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run(listArgs);
                                    this.CurrentSession.AddMenuReference(cmpRef);
                                    this.ChangeSessionHeader(this.SelectedMenu);
                                    this.isChangingSelected = false;
                                   // this.pointerEvents = 'all';
                                });
                        });
                        break;
                    }
                    case "General.MH.CustomsCollateral": {

                        var listArgs = new ListComponentArgs();
                        listArgs.QueryCode = "OpenCollaterals";
                        listArgs.ObjectTableName = "Customs.CustomsCollateral";
                      //  listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                       listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral", 0).subscribe(response => {
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(cmpRef => {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run(listArgs);
                                    this.CurrentSession.AddMenuReference(cmpRef);
                                    this.ChangeSessionHeader(this.SelectedMenu);
                                    this.isChangingSelected = false;
                                    //this.pointerEvents = 'all';
                                });
                        });
                        break;
                    }
                        

                    case "General.MH.Social": { // Abed Code
                        ServiceLocator.SendTotangoUserActivity("Social", "Main View");
                        SessionLocator.DynamicLoader.Load('./Social/Components/SocialMainComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                            .then(cmpRef => {

                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.InitializeSocialMainComponent(null);
                                this.CurrentSession.AddMenuReference(cmpRef);
                                this.ChangeSessionHeader(this.SelectedMenu);
                                this.isChangingSelected = false;
                                //this.pointerEvents = 'all';
                            });

                        break;
                    }

                    case "General.MH.PaymentOrders": {
                        
                        var listArgs = new ListComponentArgs();
                        listArgs.ObjectTableName = "Customs.PaymentOrder";
                        listArgs.NewButtonLabel = TextCodeTranslator.Translate("Customs.General.O.NewPaymentOrder");
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder", 0).subscribe(response => {
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(cmpRef => {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run(listArgs);
                                    this.CurrentSession.AddMenuReference(cmpRef);
                                    this.ChangeSessionHeader(this.SelectedMenu);
                                    this.isChangingSelected = false;
                                    //this.pointerEvents = 'all';
                                });
                        });
                        break;
                    }

                    case "General.MH.AirlineDashboard": {
                        ServiceLocator.SendTotangoUserActivity("Dashboard", "Main View");
                        myComponentPath = "./Dashboard/Components/Workspace/AirLineDashboardComponent";
                        break;
                    }



                    case "General.MH.CrossDocks": { // Abed Code
                        ServiceLocator.SendTotangoUserActivity("CrossDocks", "Main View");
                        SessionLocator.DynamicLoader.Load('./Warehouse/Components/Workspaces/WarehouseWorkspaceComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                            .then(cmpRef => {

                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.InitComponent();
                                this.CurrentSession.AddMenuReference(cmpRef);
                                this.ChangeSessionHeader(this.SelectedMenu);
                                this.isChangingSelected = false;
                                //this.pointerEvents = 'all';
                            });

                        break;
                    }

                    case "General.MH.DeclarationCargoSplits": {

                        var listArgs = new ListComponentArgs();
                        listArgs.QueryCode = "OpenCargoSplits";
                        listArgs.ObjectTableName = "Customs.DeclarationCargoSplit";
                        //  listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit", 0).subscribe(response => {
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(cmpRef => {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run(listArgs);
                                    this.CurrentSession.AddMenuReference(cmpRef);
                                    this.ChangeSessionHeader(this.SelectedMenu);
                                    this.isChangingSelected = false;
                                    //this.pointerEvents = 'all';
                                });
                        });
                        break;
                    }
                        
                    case "General.MH.Tasks": {
                        ServiceLocator.SendTotangoUserActivity("Tasks", "Main View");
                        myComponentPath = "./InfrastructureModules/InfrastructureBusinessProcess/Components/Workspaces/TasksWorkspaceComponent";
                        break;
                    }

                    case "General.MH.Depositions": {
                        ServiceLocator.SendTotangoUserActivity("Customs Shipper", "List View");
                        var listArgs = new ListComponentArgs();
                        listArgs.QueryCode = "AllDepositionsQuery";
                        listArgs.ObjectTableName = "CustomsShipper";
                        listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("CustomsShipper", 0).subscribe(response => {
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(cmpRef => {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run(listArgs);
                                    this.CurrentSession.AddMenuReference(cmpRef);
                                    this.ChangeSessionHeader(this.SelectedMenu);
                                    this.isChangingSelected = false;
                                    // this.pointerEvents = 'all';
                                });
                        });
                        break;
                    }



                    default: {
                        if (this.SelectedMenu.ObjectTableName) {
                            ServiceLocator.SendTotangoUserActivity(this.SelectedMenu.ObjectTableName, "List View");
                        }

                        if (this.SelectedMenu.HtmlView == null || this.SelectedMenu.HtmlView == undefined || this.SelectedMenu.HtmlView.indexOf('/ListComponent/ListComponent') > -1) {

                            if (this.SelectedMenu.ObjectTableId != null && this.SelectedMenu.ObjectTableId != undefined) {
                                isListComponent = true;

                                var listArgs = new ListComponentArgs();
                                var objectTable: ObjectTablePM = window.ObjectTables.filter(x => x.Id === this.SelectedMenu.ObjectTableId)[0];

                                if (objectTable != null && objectTable != undefined) {
                                    listArgs.ObjectTableName = objectTable.Name;
                                    //listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                                    listArgs.HideBackButton = true;
                                    if (AppTool.IsNullOrEmpty(listArgs.NewButtonLabel))
                                    {
                                        var tempText = TextCodeTranslator.Translate(listArgs.ObjectTableName + ".NewButton");
                                        if (!AppTool.IsNullOrEmpty(tempText)) {
                                            listArgs.NewButtonLabel = tempText;
                                        }
                                    }

                                    this._entityResourceService.getEntityResourceByTableName(objectTable.Name, 0).subscribe(response => {
                                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                            .then(cmpRef => {
                                                cmpRef.instance.ComponentRef = cmpRef;
                                                cmpRef.instance.Run(listArgs);
                                                this.CurrentSession.AddMenuReference(cmpRef);
                                                this.ChangeSessionHeader(this.SelectedMenu);
                                                this.isChangingSelected = false;
                                                //this.pointerEvents = 'all';
                                            });
                                    });
                                }
                            }
                            else {
                                this.isChangingSelected = false;
                            }
                        }

                        else {
                            myComponentPath = this.SelectedMenu.HtmlView;
                        }
                    }
                }
            }

            if (myComponentPath != null) {
                SessionLocator.DynamicLoader.Load(myComponentPath, this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        if (this.SelectedMenu.TextCode == 'General.MH.CustomsRequestsSheets') {
                            if (cmpRef.entityArgs) {
                                cmpRef.entityArgs.ObjectTableName = null;
                                cmpRef.entityArgs.EntityPM = null;
                            }
                        }
                        this.CurrentSession.DestroyMenuReferences();
                        this.CurrentSession.DestroyListComponentReferences();
                        this.CurrentSession.AddMenuReference(cmpRef);
                        this.ChangeSessionHeader(this.SelectedMenu);
                        this.isChangingSelected = false;
                        //this.pointerEvents = 'all';
                    });
            }

            else if (!isListComponent) {
                this.isChangingSelected = false;
                //this.pointerEvents = 'all';
            }
        }

        else {
            this.isChangingSelected = false;
           // this.pointerEvents = 'all';

        }
    }
    ChangeSessionHeader(menu: MainMenuItem) {
        this.CurrentSession.ChangeSessionHeader({ MenuTextCode: menu.TextCode });
    }

    private isMainSidebarCollapsed: boolean = false;
    public get IsMainSidebarCollapsed() { return this.isMainSidebarCollapsed; }
    public set IsMainSidebarCollapsed(value: boolean) {
        if (this.isMainSidebarCollapsed != value) {
            this.isMainSidebarCollapsed = value;
            SessionLocator.IsMainSidebarCollapsed = value;
            LastFilterClass.UpdateFilter("Simplog.Infrastructure.Views.MenuView", "Sidebar", value+"");
            this.MainMenuWidth = value == true ? this.MainMenuWidthCollapsed : this.MainMenuWidthOpened;
        }
    }

}

export class MainMenuItem {
    public TextCode: string;
    public HtmlView: string;
    public IndexOfOrder: number;
    public ObjectTableId: string;
    public ObjectTableName: string;
    public IconCode: string;
    public IconSource: string;
    public IconSelectedSource: string;
    constructor(textCode: string, myIcon: string) {
        this.TextCode = textCode;
        this.IconCode = myIcon;
        this.IconSource = "./Images/Menu/" + myIcon + ".png";
        this.IconSelectedSource = "./Images/Menu/" + myIcon + ".Selected.png";
    }
}
