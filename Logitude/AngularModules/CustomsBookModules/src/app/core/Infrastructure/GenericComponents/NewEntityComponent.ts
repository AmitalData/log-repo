declare var window: any;
import {Component, AfterViewInit, ViewChild, ViewContainerRef, ChangeDetectorRef, QueryList, ViewChildren} from '@angular/core';
import {EntityArgs} from '../DataContracts/EntityArgs';
import {SessionLocator} from '../Utilities/SessionLocator';
import {EntityPMService} from '../Services/EntityPMService';
import {EntityPMServiceResponse} from '../DataContracts/EntityPMServiceResponse';
import {FeatureLocator} from '../Utilities/FeatureLocator';
import {AppTool} from '../Tools';
import {EntityResourceService} from '../Services/EntityResourceService';
import {LocationDirective} from '../Utilities/LocationDirective';
import { CachedDataManager } from '../Utilities/CachedDataManager';
import {CommonDomainService} from '../../Common/Services/CommonDomainService';
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {ObjectsLocator} from '../Locators/ObjectsLocator';
import { InterestBasesPeriodPM } from '../../Accounting/EntityPMs/InterestBasesPeriodPM';
import { InterestBasesTypePMService } from '../../Accounting/Services/StandardPMs/InterestBasesTypePMService';

@Component({
    
    templateUrl: './NewEntityComponent.html',
    providers: [EntityArgs]
})

export class NewEntityComponent {
    private ScreenCode: string;
    ObjectTable: any;
    EntityPM: any;
    ObjectTableName: string;
    public ValidationErrorsList: string[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    LayoutDirection: string = 'ltr';
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs,private entityPMService: EntityPMService, private CD: ChangeDetectorRef, private _entityResourceService: EntityResourceService) {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    }

    SetWindowArgs(Args: any) {
        this.ScreenCode = Args.ObjectTableName + ".GeneralTabScreen";
        this.EntityPM = Args.EntityPM;
        this.ObjectTableName = Args.ObjectTableName;
        this.ObjectTableId = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0].Id;
        this.ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = this.ObjectTableName;
        this.entityArgs.IsNewEntity = true;

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response:any) => {
            this.InitEntityPM();
            this.BuildEditTabs();
            this.RunComponent();
        });
    }

    private isViewInited = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isViewInited = true;
                this.InitializeComponent();
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

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    InitializeComponent() {        
        this.SetSelectedTab();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
      
        this.SaveEntityChanges();
    }
    private SaveEntityChanges() {

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            this.entityPMService.insert(this.ObjectTableName, this.EntityPM).then((res: any) => {
                res.subscribe((response:any) => {
                 
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();

                    var mm: EntityPMServiceResponse = response;
                    if (!mm.HasError) {

                        this.ValidationErrorsList = [];

                        if (this.ObjectTable.CacheOnClient === true) {
                            CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                        }

                        if (this.ObjectTableName == 'VatType') {

                            var myCommonDomain = new CommonDomainService();
                            myCommonDomain.GetAllVatTypesGroups().subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    SessionLocator.AllVatTypesGroups = myResponse.Result;
                                }

                                this.CurrentSession.CloseCurrentWindowEmit(mm.Result["Id"]);
                            });
                        }

                        else {
                            this.CurrentSession.CloseCurrentWindowEmit(mm.Result["Id"]);
                        }
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                    }

                }, error => {
                    //var allErrors: string[] = [];
                    //allErrors.push(error['message']);
                    //this.ValidationErrorsList = allErrors;

                    console.log("Error===========>", error);
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                });
            });
       // }
    }
     
    private InitEntityPM() {

        switch (this.ObjectTableName) {
            case "Incoterm":
            case "State":
            case "CountryCity":
            case "EventType":
                {
                    this.EntityPM.AddedManually = true;
                    break;
                }

            case "VatType": {
                this.EntityPM.AddedManually = true;
                this.EntityPM.InActive = false;
                this.EntityPM.IsMultiPercentage = false;
                break;
            }
        }
    }
    public SelectedTab: TabItem;
    public TabsItemsSource: TabItem[] = [];
    public ObjectTableId: string;

    private BuildEditTabs() {
        var myTabsSorted: any[] = [];
        var allTabs: any[] = [];
        this.TabsItemsSource = [];

        allTabs = window.ObjectTableTabs.filter(d => d.ObjectTableId === this.ObjectTableId);
        //allTabs = this.FilterTabs(allTabs);
        allTabs = allTabs.sort((a, b) => { return a.IndexOrder - b.IndexOrder });

        for (var i = 0; i < allTabs.length; i++) {
            var tab = allTabs[i];

            if (tab.ControlPath.indexOf("EventsControl") == -1 && tab.HtmlComponentName != "ReportTemplateComponent") {
                if (FeatureLocator.IsFeatureGrantedByUniqeCode(tab.FeatureUniqeCode)) {
                    myTabsSorted.push(tab);
                }
            }
        }

        if (this.ObjectTableName == 'VatType') {
            myTabsSorted = myTabsSorted.filter(f => f.Code != 'VTPC');
        }

        myTabsSorted.forEach(item => {
            var itemTab: TabItem = new TabItem(item);
            this.TabsItemsSource.push(itemTab);
        });

        this.CD.detectChanges();
    }

    SetSelectedTab() {
        if (this.TabsItemsSource != null) {
            var selected: any = null;

            if (selected == null) {
                selected = this.TabsItemsSource[0];
            }

            this.SelectionChanged(selected);
        }
    }
    private LoadedTabsList: LoadedTabItem[] = [];
    SelectionChanged(mySelectedTab: TabItem) {

        if (this.SelectedTab != mySelectedTab) {
            this.SelectedTab = mySelectedTab;
            this.CD.detectChanges();
            if (this.LoadedTabsList == null) {
                this.LoadedTabsList = [];
            }

            if (this.LoadedTabsList.filter(d => d.Code == mySelectedTab.Code)[0] == null) {
                var myLoadedTabItem: LoadedTabItem = new LoadedTabItem(mySelectedTab.Code);
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
                            myComponentPath = "./Infrastructure/GenericComponents/GeneratedComponent";
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

                    case "Simplog.FreightLib.Views.Areas": {
                        myComponentName = "AreasTabComponent";
                        myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/AreasTabComponent";
                        break;
                    }

                    case "Simplog.FreightLib.Views.TariffTranslations": {
                        myComponentName = "TariffTranslationsTabComponent";
                        myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/TariffTranslations/TariffTranslationsTabComponent";
                        break;
                    }

                    // Ayman: no need for this the HtmlComponentUrl is enough (also for the apove cases)
                    //case "Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.PaymentTermAccountingTabControl": {
                    //    myComponentName = "PaymentTermAccountingTabComponent";
                    //    myComponentPath = "./Common/Components/Partners/EditTabs/PartnersAccountingTab/PaymentTermAccountingTabComponent";
                    //    break;
                    //}

                    default: {

                        if (mySelectedTab.EntityPM.HtmlComponentUrl != null) {
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
        }
    }

    private LoadTabComponent(loadedItem: LoadedTabItem) {
        if (loadedItem != null) {
            if (loadedItem.ComponentPath != null) {
                if (!loadedItem.IsLoaded) {

                    let locs = this.AllLocations.toArray().filter(f => f.Code == 'NewTabLocation');
                    let myLocation: LocationDirective = locs.filter(f => f.ItemCode == loadedItem.Code)[0];

                    if (myLocation != null) {
                        SessionLocator.DynamicLoader.Load(loadedItem.ComponentPath, myLocation.viewContainerRef)
                            .then(cmpRef => {
                                loadedItem.IsLoaded = true;

                                if (this.SelectedTab.EntityPM.ControlPath == "Simplog.Infrastructure.GeneralControls.GeneralTabControl") {
                                    if (AppTool.IsNullOrEmpty(this.SelectedTab.EntityPM.HtmlComponentUrl)) {
                                        cmpRef.instance.Run(this.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode, true, true);
                                    }
                                }
                            });
                    }
                }
            }
        }      
    }
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
    constructor(myCode: string) {
        this.Code = myCode;
    }
}
