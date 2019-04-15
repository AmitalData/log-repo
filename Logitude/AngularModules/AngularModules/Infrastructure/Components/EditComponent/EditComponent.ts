import { Settings } from './../../Settings';
declare var window: any;
import { Component, Type, ComponentRef, ViewContainerRef, ViewChild, Output, EventEmitter, ViewChildren, QueryList, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { ObjectFieldPM } from '../../EntityPMs/ObjectFieldPM';
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';
import { LocationDirective } from '../../Utilities/LocationDirective';
import { EntityArgs } from '../../DataContracts/EntityArgs';
import { AppTool } from '../../Tools';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { FeatureLocator } from '../../Utilities/FeatureLocator';
import { EntityPMService } from '../../Services/EntityPMService';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { EntityLastActivityService } from '../../Services/EntityLastActivityService';
import { EntityResourceService } from '../../Services/EntityResourceService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TotangoService } from '../../Services/WebServices/TotangoService';
import { CachedDataManager } from '../../Utilities/CachedDataManager';
import { LastFilterClass } from '../../Utilities/LastFilterClass';
import { EditTabComponent } from './EditTabComponent';
import { Subscription, TeardownLogic } from 'rxjs/Subscription';//itzik
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
@Component({
    moduleId: module.id,
    templateUrl: './EditComponent.html',
    providers: [EntityArgs],
})

export class EditComponent implements OnDestroy {
    public HeaderId: string;
    public ComponentId: string;
    public EditComponentCellId: string;
    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() SaveCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() TabSelected: EventEmitter<string> = new EventEmitter<string>();
    @Output() TabChanged: EventEmitter<string> = new EventEmitter<string>();
    @Output() SaveAndCloseCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() OnFirstTimeAfterSingleDataLoaded: EventEmitter<string> = new EventEmitter<string>();
    public ComponentRef: ComponentRef<EditComponent>;
    public EntityPM: any = null;
    public EntityId: string = null;
    public ObjectTable: ObjectTablePM;
    public ObjectTableId: string;
    public ObjectTableName: string;
    public ComponentIndex: number = null;
    public ValidationErrorsList: string[] = [];
    public IsInsideWindow: boolean = false;
    public PreSelectedTabCode: string = null;
    public HasHelper: boolean = false;
    public HasShortTitle: boolean = false;
    public HasMenuButtons: boolean = false;
    public IsEntityLoaded: boolean = false;
    public ComponentBackground: string = "white";
    public BackButtonLabel: string;
    public IsEditValid: boolean = true;
    public IsSaveBtnVisible: boolean = true;
    public IsSaveBtnDisable: boolean = false;
    EntityParentPM: any;
    ShowWindowsOverEditComponent: boolean = false;

    LayoutDirection: string = 'ltr';
    WorkEnvironment: string = 'logitude';
    public NavigationIds: string[];
    public CurrentNavigatedIndex: number;
    @ViewChild('Helper', { read: ViewContainerRef }) HelperViewContainerRef: ViewContainerRef;
    @ViewChild('ShortTitle', { read: ViewContainerRef }) ShortTitleViewContainerRef: ViewContainerRef;
    @ViewChild('MenuButtons', { read: ViewContainerRef }) MenuButtonsViewContainerRef: ViewContainerRef;
    @ViewChild('SplitComponentLocation', { read: ViewContainerRef }) SplitComponentViewContainerRef: ViewContainerRef;
    @ViewChild('WindowLocation', { read: ViewContainerRef }) WindowLocationViewContainerRef: ViewContainerRef;
    @ViewChild('TabControlBody', { read: ViewContainerRef }) TabControlBodyViewContainerRef: ViewContainerRef;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityPMService: EntityPMService, private entityArgs: EntityArgs, private _entityResourceService: EntityResourceService, private _totangoService: TotangoService, private cd: ChangeDetectorRef) {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
        this.ComponentIndex = this.CurrentSession.GetNewEditComponentIndex();
        this.HeaderId = "HeaderScreen_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.ComponentId = "EditComponent_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.EditComponentCellId = "EditComponentCellId_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.WorkEnvironment = ObjectsLocator.GlobalSetting == undefined ? "logitude" : ObjectsLocator.GlobalSetting.WorkEnvironment;
    }

    public Run(args: any) {
        this.EntityId = args['EntityId'];
        this.EntityPM = args['EntityPM'];
        this.EntityParentPM = args['EntityParentPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.PreSelectedTabCode = args['SelectedTabCode'];
        this.BackButtonLabel = !AppTool.IsNullOrEmpty(args['BackButtonLabel']) ? args['BackButtonLabel'] : TextCodeTranslator.Translate("General.B.Back");  // "Back";
        this.ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.HasHelper = this.ObjectTable.HasHelper;
        this.HasShortTitle = this.ObjectTable.HasShortTitle;
        this.HasMenuButtons = this.ObjectTable.HasMenuButtons;
        this.NavigationIds = args['NavigationIds'];
        if (this.NavigationIds) {
            this.NextPreviousVisible = true;
        }
        if (AppTool.IsNullOrEmpty(this.CurrentNavigatedIndex) && this.NavigationIds) {
            this.CurrentNavigatedIndex = 0;
            this.DeclarationNavigationMessage = (this.CurrentNavigatedIndex + 1).toString() + " מתוך " + this.NavigationIds.length.toString();
            //this.PreviousButtonDisabled = true;
            this.SetNextPreviousButtonsEnablity();
        }
        if (this.EntityPM != null) {
            this.entityArgs.EntityPM = this.EntityPM;
            this.entityArgs.ObjectTableName = this.ObjectTableName;
            this.entityArgs.EditComponent = this;
            this.BuildComponent();
        }

        else if (this.EntityId != null) {
            this.LoadEntityPM();
        }

        if (this.ObjectTableName != "Country" && this.ObjectTableName != "PackageType") {
            this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "View " + this.ObjectTableName);
        }

        if (this.ObjectTableName == "CommunicationLog") {
            this.IsSaveBtnDisable = true;
        }

        this.IsSaveBtnVisible = this.ObjectTable.IsSaveButtonVisible;



        // Split Component
        var feature = FeatureLocator.Features.filter(d => d.Code == "SPLIT")[0];
        if (!AppTool.IsNullOrEmpty(feature)) { // granted

            //if (this.ObjectTable.Name == "Customs.Declaration")
            //    this.IsSplitBtnVisible = true;

            if (!AppTool.IsNullOrEmpty(this.ObjectTable.SplitComponentPath)) {
                this.IsSplitBtnVisible = true;
                this.ShowWindowsOverEditComponent = true;
            }


        }

    }

    private LoadEntityPM() {

        this.entityPMService.getSingle(this.ObjectTableName, this.EntityId).then((response: any) => {
            response.subscribe((res) => {
                var pmResponse: ServiceResponse = res;

                if (!pmResponse.HasError) {
                    this.EntityPM = pmResponse.Result;

                    if (this.EntityPM) {
                        this.entityArgs.EntityPM = this.EntityPM;
                        this.entityArgs.ObjectTableName = this.ObjectTableName;
                        this.entityArgs.EditComponent = this;
                        this.SendActivityLog();
                        this.BuildComponent();
                    }

                    else {
                        this.StopBusyIndicator();
                        this.ValidationErrorsList.push("Error displaying this " + TextCodeTranslator.Translate(this.ObjectTableName));
                    }
                }

                else {
                    this.StopBusyIndicator();
                    this.ValidationErrorsList = pmResponse.ErrorsArray;
                    //console.error(pmResponse.ErrorsArray);
                }
            }, error => {
                this.StopBusyIndicator();
            });
        });
    }
    private SendActivityLog() {
        if (this.ObjectTableName == "Customer") {
            if (this.EntityPM['IsCustomer']) {
                var myService: EntityLastActivityService = new EntityLastActivityService();
                myService.AddActivityLog(this.EntityId, this.ObjectTableId, SessionLocator.LoggedUserId, 'V').subscribe();
            }
        }

        else {
            var myService: EntityLastActivityService = new EntityLastActivityService();
            myService.AddActivityLog(this.EntityId, this.ObjectTableId, SessionLocator.LoggedUserId, 'V').subscribe();
        }
    }
    private BuildComponent() {
        if (this.EntityPM) {
            this.IsEntityLoaded = true;

            this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
                this.GetControllerByTableName(this.ObjectTableName).then(EditComponentController => {
                    //this.EditComponentController = EditComponentController as IEditComponentController;
                    this.CurrentSession.AddEditComponent(this);
                    this.EditComponentController = EditComponentController as IEditComponentController;
                    this.EditComponentController.OnFirstTimeAfterSingleDataLoaded(this.EntityPM).then((isLock) => {
                        this.OnFirstTimeAfterSingleDataLoaded.emit(".EditComponentController.OnFirstTimeAfterSingleDataLoaded");
                        if (this.EditComponentController.ToCancell) {
                            this.Close();
                        } else {

                            this._SubEditComponentDefaultController =
                                this.SaveCompleted.subscribe(isSaved => {
                                    if (isSaved) {
                                        this.EditComponentController.HaveSaved = true;
                                        this._SubEditComponentDefaultController.unsubscribe();
                                        this._SubEditComponentDefaultController == null;
                                    }
                                });

                            // if (this.CurrentNavigatedIndex ==0) {
                            this.BuildEditTabs();
                            //}
                            this.RunComponent();
                            this.StopBusyIndicator();
                        }
                    });
                });
            });
        }
    }

    private isLoaderReady: boolean = false;
    RunComponent() {

        if (this.TabControlBodyViewContainerRef) {

            //if (this.AllLocations.length == 0) {
            //    this.RunComponentTimer();
            //}

            if (this.HasHelper && !this.HelperViewContainerRef) {
                this.RunComponentTimer();
            }

            else if (this.HasShortTitle && !this.ShortTitleViewContainerRef) {
                this.RunComponentTimer();
            }

            else if (this.HasMenuButtons && !this.MenuButtonsViewContainerRef) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
                this.BuildHelperControl();
                this.BuildMenuButtons();
                this.BuildShortTitle();
                this.BuildHeaderScreen();
                this.SetSelectedTab();
                this.SetSplitComponentState();
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

    private BuildHelperControl() {
        if (this.HasHelper) {
            if (this.HelperViewContainerRef) {
                this.HelperViewContainerRef.clear();

                var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Helpers/" + this.ObjectTable.Name + "HelperComponent";
                SessionLocator.DynamicLoader.Load(myComponentPath, this.HelperViewContainerRef)
                    .then(cmpRef => {
                    });
            }
        }
    }
    private BuildMenuButtons() {
        if (this.HasMenuButtons) {
            if (this.MenuButtonsViewContainerRef) {
                this.MenuButtonsViewContainerRef.clear();

                var myComponentPath = './Infrastructure/Components/LogitudeComponents/MenuButtonsComponent/MenuButtonsComponent';

                SessionLocator.DynamicLoader.Load(myComponentPath, this.MenuButtonsViewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.Run({ EntityPM: this.EntityPM, ObjectTable: this.ObjectTable });
                    });
            }
        }
    }
    private BuildShortTitle() {
        if (this.HasShortTitle) {
            if (this.ShortTitleViewContainerRef) {
                this.ShortTitleViewContainerRef.clear();

                if (this.ObjectTable.Name != null && this.ObjectTable.Name.includes("Customs")) {
                    var name: string[] = this.ObjectTable.Name.split('.');
                    var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/ShortTitles/" + name[1] + "ShortTitleComponent";

                }

                else {
                    var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/ShortTitles/" + this.ObjectTable.Name + "ShortTitleComponent";
                }

                SessionLocator.DynamicLoader.Load(myComponentPath, this.ShortTitleViewContainerRef);
            }
        }
    }

    public HeaderScreenHeight: number = 65;
    public HeaderScreenRowHeight: number = 25;
    public HeaderScreenColumns: HeaderScreenColumn[] = [];
    public SavedWidthOfHeader: number = 0;
    public BuildHeaderScreen() {

        var myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === this.ObjectTableId && d.Code.indexOf("HeaderScreen") != -1)[0];
        var myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTableId);

        if (this.ObjectTableName == "Shipment") {
            if (this.EntityPM.ShipmentLevelCode == "C") {
                this._entityResourceService.getEntityResourceByTableName("Master", 0).subscribe(response => {
                    var myObjectTable = window.ObjectTables.filter(x => x.Name === "Master")[0];
                    var myObjectTableId = myObjectTable.Id;

                    myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code.indexOf("HeaderScreen") != -1)[0];
                    myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === myObjectTableId);
                    this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
                });
            }

            else {
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        } else if (this.ObjectTableName == "ARInvoice") {

            //get f. acc. Settings
            if (SessionLocator.TenantPM.AccountingActivated) {
                var myObjectTable = window.ObjectTables.filter(x => x.Name === "ARInvoice")[0];
                var myObjectTableId = myObjectTable.Id;

                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code == "ARInvoice.FullAccHeaderScreen")[0];
                myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === myObjectTableId);
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
            else {
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        }

        //else if (this.ObjectTableName == "Customs.Declaration") {
        //    if (this.EntityPM.IsCourierDeclaration == true) {
        //        this._entityResourceService.getEntityResourceByTableName("Customs.CourierDeclaration", 0).subscribe(response => {
        //            var myObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.CourierDeclaration")[0];
        //            var myObjectTableId = myObjectTable.Id;

        //            myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code.indexOf("HeaderScreen") != -1)[0];
        //            myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === myObjectTableId);
        //            this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
        //        });
        //    }
        //    else {
        //        this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
        //    }
        //}

        else {
            this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
        }
    }


    private FindHeaderRetries: number = 0;
    private FindHeaderTimerToken: any;
    private RunFindHeaderTimer(HeaderScreen: any, ObjectFields: ObjectFieldPM[]) {

        if (this.FindHeaderTimerToken) {
            clearTimeout(this.FindHeaderTimerToken);
        }

        var element = document.getElementById(this.HeaderId);
        if (element) {


            this.GenerateHeaderScreen(HeaderScreen,ObjectFields);
        }

        else {
            this.FindHeaderRetries++;



            if (this.FindHeaderRetries < 3) {
                this.FindHeaderTimerToken = setTimeout(() => this.RunFindHeaderTimer(HeaderScreen,ObjectFields), 1);
            }
        }
    }

    private GenerateHeaderScreen(HeaderScreen: any, ObjectFields: ObjectFieldPM[]) {

        var element = document.getElementById(this.HeaderId);
        if (element == null) {
            this.RunFindHeaderTimer(HeaderScreen,ObjectFields);
        }

        else {
            this.HeaderScreenColumns = [];

            if (HeaderScreen != null) {

                if (HeaderScreen.NumberOfRows <= 1) {
                    this.HeaderScreenHeight = 40;
                    this.HeaderScreenRowHeight = 25;
                }

                else if (HeaderScreen.NumberOfRows == 2) {
                    this.HeaderScreenHeight = 65;
                    this.HeaderScreenRowHeight = 25;
                }

                else if (HeaderScreen.NumberOfRows == 3) {
                    this.HeaderScreenHeight = 75;
                    this.HeaderScreenRowHeight = 20;
                }

                var myScreenFields: any[] = window.ScreenFields.filter(d => d.ScreenId === HeaderScreen.Id && d.Tenant == SessionLocator.Tenant);
                if (myScreenFields.length == 0) {
                    myScreenFields = window.ScreenFields.filter(d => d.ScreenId === HeaderScreen.Id && d.Tenant == 0);
                }

                var widthOfColumn: number = 0;
                if (element != null) {
                    var widthOfHeader = element.clientWidth;

                    if (widthOfHeader == 0) {
                        widthOfHeader = this.SavedWidthOfHeader;
                    }

                    else {
                        this.SavedWidthOfHeader = widthOfHeader;
                    }

                    var widthOfSeparator = (HeaderScreen.NumberOfColumns - 1) * 20;
                    widthOfColumn = (widthOfHeader - widthOfSeparator) / HeaderScreen.NumberOfColumns;
                }

                for (var c = 0; c < HeaderScreen.NumberOfColumns; c++) {
                    var myColumn = new HeaderScreenColumn(false);
                    var myColumnLabelWidth = 0;

                    for (var r = 0; r < HeaderScreen.NumberOfRows; r++) {
                        var myRow = new HeaderScreenRow();

                        var myScreenField = myScreenFields.filter(f => f.Column == c && f.Row == r)[0];
                        if (myScreenField != null) {
                            var myObjectField = ObjectFields.filter(d => d.Id === myScreenField.ObjectFieldId)[0];
                            if (myObjectField != null) {

                                myRow.Label = TextCodeTranslator.Translate(myObjectField.FullNameTextCodeCode);
                                myRow.ObjectField = myObjectField;
                                //if ((this.ObjectTableName == "ARInvoice" || this.ObjectTableName == "ARPayment") && myObjectField.FieldName == "SATTransferStatusName" && SessionLocator.SATInterfaceSettings.SATInterfaceCode == "NONE") {
                                //    myRow.HideField = true;
                                //}

                                if (!AppTool.IsNullOrEmpty(myRow.Label)) {
                                    myRow.Label += ":";
                                }

                                var widthOfLabel = AppTool.GetTextWidth(myRow.Label);

                                if (widthOfLabel > myColumnLabelWidth) {
                                    myColumnLabelWidth = widthOfLabel;
                                }

                                if (myColumnLabelWidth > (widthOfColumn / 2)) {
                                    myColumnLabelWidth = widthOfColumn / 2;
                                }

                                myColumn.LabelWidth = (Math.ceil(myColumnLabelWidth) + 10) + "px";
                                myColumn.ValueMaxWidth = widthOfColumn - myColumnLabelWidth - 12;
                            }
                        }

                        myColumn.Rows.push(myRow);
                    }

                    this.HeaderScreenColumns.push(myColumn);

                    if (HeaderScreen.NumberOfColumns - c > 1) {
                        this.HeaderScreenColumns.push(new HeaderScreenColumn(true));
                    }
                }
            }
        }
    }

    // Tabs work
    public SelectedTab: TabItem;
    //public ObjectTableTabs: any[] = [];
    public TabsItemsSource: TabItem[] = [];
    private LoadedTabsList: LoadedTabItem[] = [];
    private BuildEditTabs() {
        var allTabs: any[] = [];
        var myTabsSorted: any[] = [];
        this.TabsItemsSource = [];

        allTabs = window.ObjectTableTabs.filter(d => d.ObjectTableId === this.ObjectTableId);
        allTabs = this.FilterTabs(allTabs);
        allTabs = allTabs.sort((a, b) => { return a.IndexOrder - b.IndexOrder });

        for (var i = 0; i < allTabs.length; i++) {
            var tab = allTabs[i];

            if (tab.ControlPath != null) {
                if (tab.ControlPath.indexOf("ExternalDocumentsControl") != -1) {
                    if (!FeatureLocator.HasFeaturePermession(this.ObjectTableName, "DOCSIN")) {
                        continue;
                    }
                }

                if (tab.ControlPath.indexOf("EventsControl") != -1) {
                    var eventsTabCode = tab.ObjectTableName + ".Tab.Events";
                    var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "EVENTS" || f.Code == eventsTabCode) && f.ObjectTableId == tab.ObjectTableId)[0];
                    if (eventsTabFeature = null) {
                        continue;
                    }
                }


                //if (tab.ControlPath.indexOf("WarehouseConnectionsTabComponent") != -1) {
                //    if (this.EntityPM && AppTool.IsNullOrEmpty(this.EntityPM.ShipmentId)) continue;
                //}
            }

            if (FeatureLocator.IsFeatureGranted(tab.FeatureId)) {

                if (this.ObjectTableName == "GLAccount") {

                    switch (tab.Code) {

                        case "GAAD":
                            {
                                if (this.EntityPM.AccountTypeCode == "2" || this.EntityPM.AccountTypeCode == "3")
                                    myTabsSorted.push(tab);
                                break;
                            }
                        case "GLTX":
                            {
                                if (this.EntityPM.AccountTypeCode == "3")
                                    myTabsSorted.push(tab);
                                break;
                            }
                        case "GAOV":
                            {
                                if (this.EntityPM.AccountTypeCode == "2")  // 2- Customer GLAccount
                                    myTabsSorted.push(tab);
                                break;
                            }
                        default:
                            {
                                myTabsSorted.push(tab);
                                break;
                            }
                    }
                }
                else
                    myTabsSorted.push(tab);
            }
        }

        //this.ObjectTableTabs = myTabsSorted;

        myTabsSorted.forEach(item => {

            var itemTab: TabItem = new TabItem(item);
            itemTab.IsDisabled = this.EditComponentController.IsDisabled(itemTab.Code)
            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                if (item.ControlPath.indexOf("Doc") > -1) {
                    switch (this.ObjectTableName) {
                        case "ARInvoice":
                        case "APInvoice":
                        case "ARPayment":
                        case "APPayment":
                            {
                                itemTab.IsDisabled = true;
                                break;
                            }
                    }
                }
            }

            this.TabsItemsSource.push(itemTab);
        });
    }
    private FilterTabs(allTabs: any[]) {
        switch (this.ObjectTableName) {

            case "Master":
            case "Shipment":
                {
                    // SHCO: Shipment Consolidations
                    if (this.EntityPM.ShipmentLevelCode == "H" || this.EntityPM.ShipmentLevelCode == "D") {
                        var indexOfTab = allTabs.findIndex(t => t.Code == "SHCO");
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }

                    // SHMS: Shipment Master
                    if (this.EntityPM.ShipmentLevelCode != "H") {
                        var indexOfTab = allTabs.findIndex(t => t.Code == "SHMS");
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }

                    // SHCF: Customs File
                    if (this.EntityPM.ShipmentLevelCode != "D" && this.EntityPM.ShipmentLevelCode != "H") {
                        var indexOfTab = allTabs.findIndex(t => t.Code == "SHCF");
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }

                    //Customs
                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        var indexOfTab = allTabs.findIndex(t => t.Code == "SHCT");
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }
                    else {
                        if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
                            if (!ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                                var indexOfTab = allTabs.findIndex(t => t.Code == "SHCT");
                                if (indexOfTab > -1) {
                                    allTabs.splice(indexOfTab, 1);
                                }
                            }
                        }
                    }

                    // SHFF: Freight Files
                    if (this.EntityPM.ShipmentLevelCode != "A") {
                        var indexOfTab = allTabs.findIndex(t => t.Code == "SHFF");
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }


                    // MHGC: Master General
                    // SHGC: Shipment General
                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        var indexOfTab = allTabs.findIndex(t => t.Code == "SHGC");
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }

                    else {
                        var indexOfTab = allTabs.findIndex(t => t.Code == "MHGC");
                        if (indexOfTab > -1) {
                            allTabs.splice(indexOfTab, 1);
                        }
                    }

                    break;
                }

            case "User": {
                if (SessionLocator.Tenant != 0) {
                    var indexOfTab = allTabs.findIndex(t => t.Code == "USDS");
                    if (indexOfTab > -1) {
                        allTabs.splice(indexOfTab, 1);
                    }
                }

                break;
            }

            case "Customs.PaymentOrder": {
                if (!this.EntityPM.HasDeficit) {
                    var indexOfTab = allTabs.findIndex(t => t.Code == "PODF");
                    if (indexOfTab > -1) {
                        allTabs.splice(indexOfTab, 1);
                    }
                }

                if (!this.EntityPM.HasDeposit) {
                    var indexOfTab = allTabs.findIndex(t => t.Code == "PODP");
                    if (indexOfTab > -1) {
                        allTabs.splice(indexOfTab, 1);
                    }
                }

                break;
            }

            case "Customs.Declaration": {
                //CustomsSettingList customsSetting = DataProvider.GetCachedList<CustomsSettingList>("Customs.CustomsSetting").FirstOrDefault();
                //if (customsSetting != null) {
                //    if (customsSetting.IsConnectedToUniFreight) {
                //        tabItem = objectTableTabs.Where(t => t.Code == "DCMF").FirstOrDefault();
                //        objectTableTabs.Remove(tabItem);
                //    }
                //}
                break;
            }
            case "ARPayment": {


                break;
            }
        }

        return allTabs;
    }
    private OnEntityCreated() {
        switch (this.ObjectTableName) {
            case "ARInvoice":
            case "APInvoice":
            case "ARPayment":
            case "APPayment":
                {
                    if (this.EntityPM) {
                        if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                            this.TabsItemsSource.forEach((item: TabItem) => {
                                if (item.EntityPM.ControlPath.indexOf("Doc") > -1) {
                                    item.IsDisabled = false;
                                }
                            });
                        }
                    }

                    break;
                }
        }
    }

    SetSelectedTab() {
        if (this.TabsItemsSource != null) {
            var selected: any = null;

            if (this.PreSelectedTabCode != null) {
                selected = this.TabsItemsSource.filter(d => d.Code == this.PreSelectedTabCode)[0];
            }

            if (selected == null) {
                selected = this.TabsItemsSource[0];
            }

            this.SelectionChanged(selected);
        }
    }

    SelectionChanged(mySelectedTab: TabItem) {

        if (this.SelectedTab != mySelectedTab) {
            this.SelectedTab = mySelectedTab;

            if (this.LoadedTabsList == null) {
                this.LoadedTabsList = [];
            }

            this.LoadedTabsList.forEach(item => {
                if (item.EditTabComponent) {
                    item.EditTabComponent.Selected = false;
                    item.EditTabComponent.CurrentlySelected = false;
                }
            });

            var myLoadedTabItem: LoadedTabItem = this.LoadedTabsList.filter(d => d.Code == mySelectedTab.Code)[0];

            if (myLoadedTabItem == null) {
                myLoadedTabItem = new LoadedTabItem(mySelectedTab.Code);
                this.LoadedTabsList.push(myLoadedTabItem);

                var myComponentName: string = null;
                var myComponentPath: string = null;

                switch (mySelectedTab.EntityPM.ControlPath) {

                    case "Simplog.Infrastructure.GeneralControls.GeneralTabControl": {
                        if (!AppTool.IsNullOrEmpty(mySelectedTab.EntityPM.HtmlComponentUrl)) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = AppTool.GetComponentName(myComponentPath);
                        }

                        else {
                            myComponentName = "GeneralTabComponent";
                            myComponentPath = "./Infrastructure/GenericComponents/GeneralTabComponent";
                        }

                        break;
                    }

                    case "Simplog.Infrastructure.GeneralControls.BillingTabControl": {
                        if (!AppTool.IsNullOrEmpty(mySelectedTab.EntityPM.HtmlComponentUrl)) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = AppTool.GetComponentName(myComponentPath);
                        }

                        else {
                            myComponentName = "BillingTabComponent";
                            myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/BillingTabComponent";
                        }
                        break;
                    }

                    case "Simplog.Infrastructure.Views.Events.EventsControl": {
                        myComponentName = "EventsTabComponent";
                        myComponentPath = "./Common/Components/Events/EventsTabComponent";
                        break;
                    }

                    case "Simplog.Infrastructure.Views.Communications.CommunicationsControl": {
                        myComponentName = "CommunicationsTabComponent";
                        myComponentPath = "./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent";
                        break;
                    }

                    case "Simplog.FreightLib.Views.PartnersTabs.PartnerContactsTab": {
                        myComponentName = "ContactsTabComponent";
                        myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent";
                        break;
                    }

                    case "Simplog.FreightLib.Views.PartnersTabs.PartnerAddressesTab": {
                        myComponentName = "AddressesTabComponent";
                        myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/AddressesTabComponent";
                        break;
                    }

                    default: {

                        if (!AppTool.IsNullOrEmpty(mySelectedTab.EntityPM.HtmlComponentUrl)) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = AppTool.GetComponentName(myComponentPath);
                        }

                        break;
                    }
                }

                if (myComponentPath != null) {
                    myLoadedTabItem.ComponentName = myComponentName;
                    myLoadedTabItem.ComponentPath = myComponentPath;
                    this.LoadTabComponent(myLoadedTabItem);
                }
            }

            else {
                this.TabSelected.emit(mySelectedTab.Code);

                if (myLoadedTabItem.EditTabComponent) {
                    myLoadedTabItem.EditTabComponent.Selected = true;
                    myLoadedTabItem.EditTabComponent.CurrentlySelected = true;
                }
            }

            if (this.SelectedTab.Code != "SHOV") {

                var myTab = window.ObjectTableTabs.filter(d => d.ObjectTableId === this.ObjectTableId && d.Code === this.SelectedTab.Code)[0];
                ServiceLocator.SendTotangoUserActivity(myTab.ObjectTableName, myTab.TabNameTextCodeDefaultText + " Tab View");
            }

            this.TabChanged.emit(mySelectedTab.Code);
        }
    }

    private LoadTabComponent(loadedItem: LoadedTabItem) {
        if (loadedItem != null) {
            if (loadedItem.ComponentPath != null) {
                if (!loadedItem.IsLoaded) {
                    SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditTabComponent", this.TabControlBodyViewContainerRef)
                        .then(cmpRef => {
                            loadedItem.IsLoaded = true;
                            loadedItem.EditTabComponent = cmpRef.instance;
                            if (this.SelectedTab.Code == loadedItem.Code) {
                                loadedItem.EditTabComponent.CurrentlySelected = true;
                            }
                            cmpRef.instance.Run(loadedItem.Code, loadedItem.ComponentPath);
                        });
                }
            }
        }
    }

    //private LoadTabComponent(loadedItem: LoadedTabItem) {
    //    if (loadedItem != null) {
    //        if (loadedItem.ComponentPath != null) {
    //            if (!loadedItem.IsLoaded) {

    //                let locs = this.AllLocations.toArray().filter(f => f.Code == 'EditTabLocation');
    //                let myLocation: LocationDirective = locs.filter(f => f.ItemCode == loadedItem.Code)[0];

    //                if (myLocation != null) {
    //                    SessionLocator.DynamicLoader.Load(loadedItem.ComponentPath, myLocation.viewContainerRef)
    //                        .then(cmpRef => {
    //                            loadedItem.IsLoaded = true;
    //                        });
    //                }
    //            }
    //        }
    //    }
    //}

    // Commands
    BackButtonClicked() {
        var isNeedingConfirmation = this.NeedCloseConfirmation();
        if (isNeedingConfirmation) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.DontSave");
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName)));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.SaveEntityChanges(true);
                }

                else if (confirmWindow.No) {
                    this.Close();
                }
            });
        }
        else {
            this.Close();
        }
    }

    public NeedCloseConfirmation() {
        var myResult = true;
        if (!this.EntityPM) {
            myResult = false;
        }

        else if (!this.EntityPM.IsDirty) {
            myResult = false;
        }
        else if (this.ObjectTableName == "TaxReport" || this.ObjectTableName == "BankDeposit") {
            myResult = false;
        }
        return myResult;
    }
    Close() {
        if (this.IsInsideWindow) {
            this.CurrentSession.CloseCurrentWindow();
        }

        if (this.EditComponentController) {
            this.EditComponentController.OnCloseEditControl();
        }

        this.DestroyEditControl();
        this.BackCompleted.emit(true);
    }

    SaveChanges(busyIndicatorText: string = null) {

        if (this.EntityPM.IsDirty) {
            if (this.IsEditValid) {
                this.SaveEntityChanges(false, busyIndicatorText);
            }
        }

        else {
            this.FireSaveCompleted(true);
        }
    }

    SaveChangesAndClose() {
        this.SaveEntityChanges(true);
    }

    private SaveEntityChanges(isClosing: boolean, busyIndicatorText: string = null, loadNextEntity: boolean = false, loadPreviousEntity: boolean = false) {
        if (this.EntityPM.IsDirty) {

            this.ValidationErrorsList = [];

            if (!AppTool.IsNullOrEmpty(busyIndicatorText)) {
                this.StartBusyIndicator(busyIndicatorText);
            }

            else {
                this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            }
            if ((this.ObjectTableName == "ARInvoice" || this.ObjectTableName == "APInvoice" || this.ObjectTableName == "ARPayment" || this.ObjectTableName == "APPayment"
                || this.ObjectTableName == "BankDeposit" || this.ObjectTableName == "Journal" || this.ObjectTableName == "AccountingIntegrityCheck") && AppTool.IsNullOrEmpty(this.EntityPM.Id)) { // customs: notification defenetion, new declaration
                this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);
                this.entityPMService.insert(this.ObjectTableName, this.EntityPM).then((res: any) => {
                    res.subscribe((myResponse: ServiceResponse) => {

                        this.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.OnSavingFailed();
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                            this.FireSaveCompleted(false);
                        }

                        else {
                            this.EntityPM = myResponse.Result;
                            this.EntityId = this.EntityPM.Id;
                            this.entityArgs.EntityPM = this.EntityPM;

                            if (this.ObjectTable.CacheOnClient) {
                                CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                            }

                            if (isClosing) {
                                this.SaveAndCloseCompleted.emit(true);
                                this.Close();
                            }

                            else {
                                this.OnEntityCreated();
                                this.UpdateComponentMembers();
                                this.FireSaveCompleted(true);
                                // this is for navigation
                                if (loadNextEntity) {
                                    this.CurrentNavigatedIndex = this.CurrentNavigatedIndex + 1;
                                    this.LoadNextPreviousEntity();

                                }
                                if (loadPreviousEntity) {
                                    this.CurrentNavigatedIndex = this.CurrentNavigatedIndex - 1;
                                    this.LoadNextPreviousEntity();
                                }

                                if (this.nextPreviousTimerToken) {
                                    clearTimeout(this.nextPreviousTimerToken);
                                }
                                this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
                            }

                            this.EditComponentController.HaveSaved = true;
                        }

                    }, error => {
                        this.OnSavingFailed();
                        this.StopBusyIndicator();
                        var myErrors: string[] = [];
                        myErrors.push(error.message);
                        this.ValidationErrorsList = myErrors;
                        this.FireSaveCompleted(false);
                    });
                });
            }

            else {
                this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "Edit " + this.ObjectTableName);

                this.entityPMService.update(this.ObjectTableName, this.EntityPM).then((res: any) => {
                    res.subscribe((myResponse: ServiceResponse) => {

                        this.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.OnSavingFailed();
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                            this.FireSaveCompleted(false);
                        }

                        else {
                            this.EntityPM = myResponse.Result;
                            this.entityArgs.EntityPM = this.EntityPM;

                            if (this.ObjectTable.CacheOnClient) {
                                CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                            }

                            if (isClosing) {
                                this.SaveAndCloseCompleted.emit(true);
                                this.Close();
                            }

                            else {
                                this.UpdateComponentMembers();
                                this.FireSaveCompleted(true);
                                // this is for navigation
                                if (loadNextEntity) {
                                    this.CurrentNavigatedIndex = this.CurrentNavigatedIndex + 1;
                                    this.LoadNextPreviousEntity();
                                    if (this.nextPreviousTimerToken) {
                                        clearTimeout(this.nextPreviousTimerToken);
                                    }
                                    this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
                                }
                                if (loadPreviousEntity) {
                                    this.CurrentNavigatedIndex = this.CurrentNavigatedIndex - 1;
                                    this.LoadNextPreviousEntity();
                                    if (this.nextPreviousTimerToken) {
                                        clearTimeout(this.nextPreviousTimerToken);
                                    }
                                    this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
                                }

                            }
                        }

                    }, error => {
                        this.OnSavingFailed();
                        this.StopBusyIndicator();
                        var myErrors: string[] = [];
                        myErrors.push(error.message);
                        this.ValidationErrorsList = myErrors;
                        this.FireSaveCompleted(false);
                    });
                });
            }
        }

        else {
            this.Close();
        }
    }

    private OnSavingFailed() {
        switch (this.ObjectTableName) {
            case "APInvoice": {
                var isDirty = this.EntityPM['IsDirty'];
                this.EntityPM['SetVoided'] = false;
                this.EntityPM['SetApproved'] = false;
                this.EntityPM['SetReTransfer'] = false;
                this.EntityPM['SetCancelApproval'] = false;
                this.EntityPM['IsDirty'] = isDirty
                break;
            }

            case "ARInvoice": {
                var isDirty = this.EntityPM['IsDirty'];
                this.EntityPM['SetVoided'] = false;
                this.EntityPM['SetAsSent'] = false;
                this.EntityPM['SetApproved'] = false;
                this.EntityPM['SetReTransfer'] = false;
                this.EntityPM['SetCancelDraft'] = false;
                this.EntityPM['IsDirty'] = isDirty
                break;
            }
        }
    }

    ReloadEntityPM() {
        if (this.EntityId) {
            this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));

            this.entityPMService.getSingle(this.ObjectTableName, this.EntityId).then((res: any) => {
                res.subscribe((myResponse: ServiceResponse) => {

                    //this.StopBusyIndicator();

                    if (myResponse.HasError) {
                        this.StopBusyIndicator();
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.LoadCompleted.emit(false);
                    }

                    else {
                        this.EntityPM = myResponse.Result;
                        this.entityArgs.EntityPM = this.EntityPM;

                        this.EditComponentController.OnReloadEntityPM().then((isLock) => {
                            this.StopBusyIndicator();
                            this.UpdateComponentMembers();
                            this.LoadCompleted.emit(true);
                        });
                    }
                });
            });
        }
    }

    private UpdateComponentMembers() {
        //this.BuildHelperControl();
        //this.BuildMenuButtons();
        this.BuildHeaderScreen();
    }

    private busyIndicatorText: string = null;
    public get BusyIndicatorText() { return this.busyIndicatorText; }
    public set BusyIndicatorText(value: string) {
        if (this.busyIndicatorText != value) {
            this.busyIndicatorText = value;
        }
    }

    private showBusyIndicator: boolean = false;
    public get ShowBusyIndicator() { return this.showBusyIndicator; }
    public set ShowBusyIndicator(value: boolean) {
        if (this.showBusyIndicator != value) {
            this.showBusyIndicator = value;
        }
    }

    public StartBusyIndicator(myText: string) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    }
    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
    EditComponentController: IEditComponentController;
    GetControllerByTableName(objectTableName: string) {
        var notDefault = ["Declaration", "Vehicle"];
        var table = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = table.ClientModuleName;
        var servicename = objectTableName + "EditComponentController";
        var servicelink = './' + moduleName + '/Controller/' + servicename;

        return new Promise((resolve) => {
            if (notDefault.indexOf(objectTableName) > -1) {
                SessionLocator.DynamicLoader.GetInstance(servicelink, true
                ).then((service: any) => {
                    resolve(service);
                    //}).catch((rejectReson) => {
                    //    var myEditComponentDefaultController = new EditComponentDefaultController()
                    //    resolve(myEditComponentDefaultController);
                });
            } else {
                var myEditComponentDefaultController = new EditComponentDefaultController()
                resolve(myEditComponentDefaultController);
            }
        });
    }

    public StartBusyIndicatorSaving() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
    }
    public StartBusyIndicatorLoading() {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
    }

    private FireSaveCompleted(isSaveSuccess: boolean) {
        this.SaveCompleted.emit(isSaveSuccess);

        //Abed Code
        if (this.ObjectTableName == "Shipment" && this.EntityPM.IsRefreshFollowUp) {
            this.EntityPM.IsRefreshFollowUp = false;
            this.CurrentSession.FireEvent("FollowupsChanged");
        }
    }

    private _Subscription: Subscription = new Subscription();//itzik///https://stackoverflow.com/a/42274637
    public SubscriptionAdd(teardown: TeardownLogic) {
        //    this.someService.change.subscribe(() => {
        //[...]
        //    })

        this._Subscription.add(teardown);
    }
    DestroyEditControl() {
        if (this.ComponentRef != null) {
            this.CurrentSession.RemoveEditComponent(this);
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    }
    public MenuButtonsHandlerREF: any;
    private _SubEditComponentDefaultController;
    ngOnDestroy() {
        console.log("EditComp:ngOnDestroy")

        this.LoadedTabsList.forEach(item => {
            if (item.EditTabComponent) {
                item.EditTabComponent = null;
            }
        });
        this.LoadedTabsList = null;

        this.CurrentSession.UnsubscribeStaticEvent();
        this._Subscription.unsubscribe();//itzik
        if (this._SubEditComponentDefaultController) {
            this._SubEditComponentDefaultController.unsubscribe()
            this._SubEditComponentDefaultController = null;
        }
        if (this.MenuButtonsHandlerREF && this.MenuButtonsHandlerREF.ngOnDestroy) {
            this.MenuButtonsHandlerREF.ngOnDestroy()
            this.MenuButtonsHandlerREF = null;
        }
    }

    //#region Split Component
    public IsSplitBtnVisible: boolean = false;
    IsSplitComponentOpened: boolean = false;

    token: any;
    SplitButtonClicked() {

        this.IsSplitComponentOpened = !this.IsSplitComponentOpened;

        this.cd.detectChanges(); // to let HTML read split component location

        if (this.IsSplitComponentOpened == true) {
            this.token = setTimeout(() => {
                this.LoadSplitComponent();
            }, 100);
        }

        //// show and hide component with slide animation
        //if (this.IsSplitComponentOpened == true) {
        //    this.SplitWidth = 0;
        //    this.token = setTimeout(() => {
        //        this.IsSplitComponentOpened = false;
        //    }, 500);
        //} else {
        //    this.SplitWidth = 870;
        //    this.IsSplitComponentOpened = true;
        //    this.cd.detectChanges();
        //    this.LoadSplitComponent();
        //}

        // update opened/closed state
        LastFilterClass.UpdateFilter("DeclarationEditControl", this.EntityPM.Id, this.IsSplitComponentOpened ? "true" : "false");


    }

    LoadSplitComponent() {


        let locs = this.AllLocations.toArray();
        let myLocation: LocationDirective = locs.filter(f => f.Code == 'SplitComponentLocation')[0];

        console.log("Load SplitComponent @ ", myLocation);

        if (myLocation) {
            //this.myLocation.clear();
            var splitComponentPath = this.ObjectTable.SplitComponentPath;
            //var splitComponentPath = "./Customs/AngularModules/AngularModules/Customs/Components/Declaration/DeclarationSplitComponent";

            SessionLocator.DynamicLoader.Load(splitComponentPath, myLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.SetComponentArgs({ EntityPM: this.EntityPM });
                });
        }



    }

    SetSplitComponentState() {
        if (this.IsSplitBtnVisible == false)
            return;

        // state: opened / closed
        var defaultFilterCode: string = LastFilterClass.GetFilterValue("DeclarationEditControl", this.EntityPM.Id);
        if (defaultFilterCode == "true") {
            this.SplitButtonClicked(); // open split section
        }

    }

    //#endregion


    //navigation methods

    public NextPreviousVisible: boolean = false;
    public PreviousButtonDisabled: boolean = false;
    public NextButtonDisabled: boolean = false;
    public DeclarationNavigationMessage: string = "";
    Next() {

        if (this.EntityPM.IsDirty) {
            this.SaveEntityChanges(false, null, true);
        }
        else {
            this.CurrentNavigatedIndex = this.CurrentNavigatedIndex + 1;
            this.LoadNextPreviousEntity();


            if (this.nextPreviousTimerToken) {
                clearTimeout(this.nextPreviousTimerToken);
            }
            this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
        }
    }

    Previous() {

        if (this.EntityPM.IsDirty) {
            this.SaveEntityChanges(false, null, false, true);
        }
        else {
            this.CurrentNavigatedIndex = this.CurrentNavigatedIndex - 1;
            this.LoadNextPreviousEntity();


            if (this.nextPreviousTimerToken) {
                clearTimeout(this.nextPreviousTimerToken);
            }
            this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
        }
    }

    nextPreviousTimerToken: any;
    LoadNextPreviousEntity() {


        this.NextButtonDisabled = true;
        this.PreviousButtonDisabled = true;
        this.cd.detectChanges();

        this.TabsItemsSource = [];
        this.LoadedTabsList.forEach((tab) => {
            tab.EditTabComponent.DestroyCurrentTab();
        });
        this.LoadedTabsList = [];

        this.TabControlBodyViewContainerRef.clear();
        if (this.EditComponentController) {
            this.EditComponentController.OnCloseEditControl();
        }

        //if (this.ComponentRef != null) {
        //  this.CurrentSession.RemoveEditComponent(this);
        //  this.ComponentRef.destroy();
        //  this.ComponentRef = null;
        //}

        this.CurrentSession.RemoveEditComponent(this);
        this.ngOnDestroy();



        var args: any = {};
        args.EntityId = this.NavigationIds[this.CurrentNavigatedIndex];
        args.ObjectTableName = this.ObjectTableName;
        args.BackButtonLabel = this.BackButtonLabel;
        args.NavigationIds = this.NavigationIds;
        this.Run(args);

    }

    SetNextPreviousButtonsEnablity() {
        if(this.NavigationIds)
        {
            if (this.CurrentNavigatedIndex == 0) {
                this.PreviousButtonDisabled = true;
            }
            else {
                this.PreviousButtonDisabled = false;
            }

            if (this.CurrentNavigatedIndex == this.NavigationIds.length - 1) {
                this.NextButtonDisabled = true;
            }
            else {
                this.NextButtonDisabled = false;
            }

            this.DeclarationNavigationMessage = (this.CurrentNavigatedIndex + 1).toString() + " מתוך " + this.NavigationIds.length.toString();
        }
    }

    public SetSelectedTabByCode(code: string) {
        if (this.TabsItemsSource != null) {
            var selected = this.TabsItemsSource.filter(d => d.Code == code)[0];

            if (selected != null) {
                this.SelectionChanged(selected);
            }
        }
    }

}

class HeaderScreenColumn {
    public IsSeparator: boolean = false;
    public Width: string = "auto";
    public Rows: HeaderScreenRow[] = [];
    public LabelWidth: string = "auto";
    public ValueMaxWidth: number = 0;
    constructor(isSeparator: boolean) {
        this.IsSeparator = isSeparator;
        if (isSeparator) {
            this.Width = "20px";
        }
    }
}
class HeaderScreenRow {
    public Label: string = null;
    public ObjectField: ObjectFieldPM;
    public HideField: boolean = false;
}
class TabItem {
    public Code: string;
    public EntityPM: any;
    public TextCode: string;
    public IsDisabled: boolean = false;
    constructor(itemPM: any) {
        this.Code = itemPM.Code;
        this.EntityPM = itemPM;
        this.TextCode = itemPM.TabNameTextCodeCode;
    }
}
class LoadedTabItem {
    public Code: string;
    public IsLoaded: boolean = false;
    public ComponentName: string;
    public ComponentPath: string;
    public EditTabComponent: EditTabComponent;
    constructor(myCode: string) {
        this.Code = myCode;
    }
}

export class EditComponentDefaultController implements IEditComponentController {
    OnFirstTimeAfterSingleDataLoaded(CurrentEntity): Promise<boolean> {
        return new Promise((resolve, reject) => {
            resolve(false);
        });
    }
    OnReloadEntityPM(): Promise<any> {
        return new Promise((resolve, reject) => {
            resolve();
        });
    }
    OnCloseEditControl() {

    }
    HaveSaved: boolean;
    InDisplayMode: boolean;
    ToCancell: boolean;
    InDisplayModeMessage: string;
    MustRefresh: boolean;
    MustRefreshMessage: string;
    ResetMustRefresh() { };
    IsInBatchRequest: boolean;
    IsDisabled(itemTabCode: string): boolean {
        return false;
    }
}
export interface IEditComponentController {
    OnFirstTimeAfterSingleDataLoaded(CurrentEntity): Promise<boolean>;
    OnReloadEntityPM(): Promise<any>;
    OnCloseEditControl(): void;
    HaveSaved: boolean;
    InDisplayMode: boolean;
    ToCancell: boolean;
    InDisplayModeMessage: string;
    MustRefresh: boolean;
    MustRefreshMessage: string;
    IsInBatchRequest: boolean;
    ResetMustRefresh(): void;
    IsDisabled(itemTabCode: string): boolean;
}

