import { BankDepositExtendedPMService } from './../../../Accounting/Services/ExtendedPMs/BankDepositExtendedPMService';
import { CashBookExtendedPMService } from './../../../Accounting/Services/ExtendedPMs/CashBookExtendedPMService';
import { ReconciliationExtendedPMService } from './../../../Accounting/Services/ExtendedPMs/ReconciliationExtendedPMService';
declare var window: any;
import { Component, Type, ComponentRef, ViewContainerRef, ViewChild, Output, EventEmitter, ViewChildren, QueryList, OnDestroy, ChangeDetectorRef,HostListener, AfterViewInit, ElementRef, OnChanges, SimpleChanges } from '@angular/core';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { ObjectFieldPM } from '../../EntityPMs/ObjectFieldPM';
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';
import { LocationDirective } from '../../Utilities/LocationDirective';
import { EntityArgs } from '../../DataContracts/EntityArgs';
import { ConfirmationMessageArgs } from '../../DataContracts/ConfirmationMessageArgs';
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
import { Subscription, TeardownLogic } from 'rxjs';//itzik
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { HeaderScreenDataResult } from '../../Interface/IHeaderScreenService';
 import { AmitalGatewayUtil } from 'Infrastructure/Utilities/AmitalGatewayUtil';
import { DeclarationEventManager } from 'Customs/Utilities/DeclarationEventManager';

import { CustomsSettingListService } from 'Customs/Services/StandardLists/CustomsSettingListService';
import { CustomsSettingPM } from 'Customs/EntityPMs/CustomsSettingPM';
import { CustomsSettingList } from 'Customs/EntityLists/CustomsSettingList';

import { CustomsClosedTablePM } from '../../../Customs/EntityPMs/CustomsClosedTablePM';
import { CustomsSettingExtendedListService } from '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';

 import { TableTabService } from 'Infrastructure/Services/ExtendedPMs/TableTabService';
import { ObjectTableTabPM } from 'Infrastructure/EntityPMs/ObjectTableTabPM';
import { CloneDeep } from 'Infrastructure/Helpers/LodashClone';
import { WorkFlowVersionPMService } from 'Workflow/Services/StandardPMs/WorkFlowVersionPMService';
//import { CloneEntityPM } from 'Infrastructure/Helpers/SafeCloneDeep';
import { GlobalDomainService } from '../../../Common/Services/GlobalDomainService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';


const InterestTransactionTabCode = 'GLIT';
const CustomerGLAccountTypeCode = "2";
 

@Component({
    templateUrl: './EditComponent.html',
    providers: [EntityArgs],
})

export class EditComponent implements OnDestroy, AfterViewInit {
    public HeaderId: string;
    public ComponentId: string;
    public EditComponentCellId: string;
    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() SaveStart: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() SaveCompleted: EventEmitter<any> = new EventEmitter<any>();
    @Output() TabSelected: EventEmitter<string> = new EventEmitter<string>();
    @Output() TabChanged: EventEmitter<string> = new EventEmitter<string>();
    @Output() SaveAndCloseCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() OnFirstTimeAfterSingleDataLoaded: EventEmitter<string> = new EventEmitter<string>();
    public ComponentRef: ComponentRef<EditComponent>;
    public EntityPM: any = null;
    public ClonedEntityPM: any = null;
    public EntityId: string = null;
    public JournalNumber: string = null;
   
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
    public IsTabsHidden: boolean = false;
    public IsFirstOpen: boolean = false;

    public IsEntityLoaded: boolean = false;
    public ComponentBackground: string = "white";
    public BackButtonLabel: string;
    public IsEditValid: boolean = true;
    public IsSaveBtnVisible: boolean = true;
    public IsSaveBtnDisable: boolean = false;
    public NeedRefresh: boolean = false;

    public EditComponentArgument: any = null;
    public InterestInvoiceARInvoiceType: string = "IT";

    EntityParentPM: any;
    ShowWindowsOverEditComponent: boolean = false;

    LayoutDirection: string = 'ltr';
    WorkEnvironment: string = 'logitude';
    public NavigationIds: string[];
    public CurrentNavigatedIndex: number;
    @ViewChild('Helper', { read: ViewContainerRef, static: false }) HelperViewContainerRef: ViewContainerRef;
    @ViewChild('ShortTitle', { read: ViewContainerRef, static: false }) ShortTitleViewContainerRef: ViewContainerRef;
    @ViewChild('MenuButtons', { read: ViewContainerRef, static: false }) MenuButtonsViewContainerRef: ViewContainerRef;
    @ViewChild('SplitComponentLocation', { read: ViewContainerRef, static: false }) SplitComponentViewContainerRef: ViewContainerRef;
    @ViewChild('WindowLocation', { read: ViewContainerRef, static: false }) WindowLocationViewContainerRef: ViewContainerRef;
    @ViewChild('TabControlBody', { read: ViewContainerRef, static: false }) TabControlBodyViewContainerRef: ViewContainerRef;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public CurrentSession = SessionLocator.SelectedSession;
    public IsReloadNeeded: boolean = false;
     public isEntityChange: boolean = false;
    private static _CustomsSettingList:CustomsSettingList=null;
    tabsService = new TableTabService();
    public IsDigitalAddsOn: boolean = false;

 
    constructor(private entityPMService: EntityPMService, private entityArgs: EntityArgs, private _entityResourceService: EntityResourceService, private _totangoService: TotangoService, private cd: ChangeDetectorRef) {
        this.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
        this.ComponentIndex = this.CurrentSession.GetNewEditComponentIndex();
        this.HeaderId = "HeaderScreen_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.ComponentId = "EditComponent_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.EditComponentCellId = "EditComponentCellId_" + this.CurrentSession.SessionIndex + "_" + this.ComponentIndex;
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.WorkEnvironment = ObjectsLocator.GlobalSetting == undefined ? "logitude" : ObjectsLocator.GlobalSetting.WorkEnvironment;
         this.FetchCustomsSetting();
        
    }

    
    
    ngAfterViewInit(): void {
        if (this.WorkEnvironment.toLowerCase() == "customs") {
            setTimeout(() => {
                const htmlElement = document?.querySelector('html');
                if (htmlElement.scrollTop > 0) {
                    htmlElement.scrollTop = 0;
                }
                const element1 = document?.querySelector('.scrollable-overflow-element');
                if (element1.scrollTop > 0) {
                    element1.scrollTop = 0;
                    ///this.findParentWithNonZeroScrollTop(element1);
                }
            }, 3000);
        }
        }
    

    findParentWithNonZeroScrollTop(element) {
        while (element) {
          if (element.scrollTop > 0) {
            return element;
          }
          element = element.parentElement;
        }
        return null;
      }
      

    OnSaveAndCloseHotKey(){
        if(!this.IsSaveBtnDisable){

            this.SaveChangesAndClose();
        }
    }

    OnSaveHotKeyPressed() {
        console.log("saving the edit component ");
        if (this.EntityPM && this.EntityPM.IsDirty) {
            this.SaveChanges();
        }
    }

    OnArrowLeftHotKeyPressed() {
        if (this.PreviousButtonDisabled == false
            && this.NextPreviousVisible == true) {
            console.log("Moving Previous ");
            this.Previous();
        }
    }

    OnEscHotKeyPressed() {
        console.log("Back from edit ");
        this.BackButtonClicked();
    }

    OnArrowRightHotKeyPressed() {
        if (this.NextButtonDisabled == false
            && this.NextPreviousVisible == true) {
            console.log("Moving Next ");
            this.Next();
        }
    }

    private QuerySection: string;
    private SelectedQueryCode: string;
    private EntityFields: any[] = null;
    private _CustomsClosedTablePM: CustomsClosedTablePM =null; 
    private _CustomsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    public async Run(args: any) {



        this.EntityId = args['EntityId'];
        this.EntityPM = args['EntityPM'];
        this.EntityParentPM = args['EntityParentPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.PreSelectedTabCode = args['SelectedTabCode'];
        this.QuerySection = !AppTool.IsNullOrEmpty(args['QuerySection']) ? args['QuerySection'] : null;
        this.SelectedQueryCode = !AppTool.IsNullOrEmpty(args['SelectedQueryCode']) ? args['SelectedQueryCode'] : null;
        this.BackButtonLabel = !AppTool.IsNullOrEmpty(args['BackButtonLabel']) ? args['BackButtonLabel'] : TextCodeTranslator.Translate("General.B.Back");  // "Back";
        this.ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        this.ObjectTableId = this.ObjectTable.Id;
        
        this.HasHelper = this.ObjectTable.HasHelper;
        this.HasShortTitle = this.ObjectTable.HasShortTitle;
        this.HasMenuButtons = this.ObjectTable.HasMenuButtons;
        this.IsTabsHidden = this.ObjectTable.IsTabsHidden;
        this.NavigationIds = args['NavigationIds'];
        this.EntityFields = args['EntityFields'];
 
        this.IsFirstOpen = args['IsFirstOpen'];
        this.JournalNumber = args['JournalNumber']
 
        if (this.ObjectTableName == "Customs.ExportStorge" || this.ObjectTableName == "QuoteOP"  ) {
            this.LayoutDirection = 'ltr'
        }
        else {
            this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        }

        if (this.NavigationIds) {
            this.NextPreviousVisible = true;
        }

        if (AppTool.IsNullOrEmpty(this.CurrentNavigatedIndex) && this.NavigationIds) {
            this.CurrentNavigatedIndex = 0;
            this.DeclarationNavigationMessage = (this.CurrentNavigatedIndex + 1).toString() + " מתוך " + this.NavigationIds.length.toString();
            //this.PreviousButtonDisabled = true;
            this.SetNextPreviousButtonsEnablity();
        }
        if (!AppTool.IsNullOrEmpty(this.ObjectTableId) && this.WorkEnvironment.toLowerCase() == "customs") {
            await new Promise<void>(resolve => {

            this._CustomsSettingExtendedListService.GetCustomsClosedTablePMByObjectTableId(this.ObjectTableId)
                .subscribe(
                    res => {
                        if (!AppTool.IsNullOrEmpty(res.Result)) {
                            this._CustomsClosedTablePM = res.Result;
                        }
                        resolve()
                    }

                 );
            })
        }
        await new Promise<void>(resolve => {
            this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response: any) => {

 
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
 


 
            this.IsSaveBtnVisible = this.ObjectTable.IsSaveButtonVisible || (this.ObjectTable.IsCustom && AppTool.IsNullOrEmpty(this.ObjectTable.ParentObjectTableId));
 
            var feature = FeatureLocator.Features.filter(d => d.Code == "SPLIT")[0];
  
            if (!AppTool.IsNullOrEmpty(feature)) { // granted
 
                if (!AppTool.IsNullOrEmpty(this.ObjectTable.SplitComponentPath)) {
 
                    this.IsSplitBtnVisible = true;
                         this.ShowWindowsOverEditComponent = true;
 
                     }
 
 
            }
 
            var isNewEntity = true;
 
            if (this.EntityId || (this.EntityId && this.EntityPM.Id))
 
                isNewEntity = false;
 
            if ((this.ObjectTableName == "ARPayment") && SessionLocator.TenantPM.AccountingActivated && isNewEntity) {
 
                this.IsSaveBtnVisible = false;
 
            }
                resolve();

 
            });
             
        });

    }

    private LoadEntityPM() {

        if (this.ObjectTableName == "Reconciliation") {
            var service = new ReconciliationExtendedPMService();
            service.GetSingleWithoutLines(this.EntityId).subscribe((response: ServiceResponse) => {
                
                console.log("[ReconciliationExtendedPMService.GetSingleWithoutLines] ", response);

                if (!response.HasError) {
                    var reconciliation = response.Result;
                    this.SetEntityPMAfterLoadIt(reconciliation);

                }
                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                    this.StopBusyIndicator();
                }
            });


        }
        else if (this.ObjectTableName == "CashBook") {
            let service = new CashBookExtendedPMService();
            service.GetSingleWithoutLines(this.EntityId).subscribe((response: ServiceResponse) => {
                console.log("[CashBookExtendedPMService.GetSingleWithoutLines] ", response);

                if (!response.HasError) {
                    var reconciliation = response.Result;
                    this.SetEntityPMAfterLoadIt(reconciliation);

                }
                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                    this.StopBusyIndicator();
                }
            });


        }
        else {
            this.entityPMService.getSingle(this.ObjectTableName, this.EntityId).then((response: any) => {
                response.subscribe((res) => {
                    var pmResponse: ServiceResponse = res;

                    if (!pmResponse.HasError) {
                        this.SetEntityPMAfterLoadIt(pmResponse.Result);
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

    }
    
    private SetEntityPMAfterLoadIt(result) {
        
        if (this.WorkEnvironment.toLowerCase() =="customs"  && !AppTool.IsNullOrEmpty(result["Tenant"])){
            const entityTenant:number =result["Tenant"];
            if (entityTenant!= SessionLocator.Tenant)//SessionLocator.LoggedUserPM.Tenant) 
            {
                if (this._CustomsClosedTablePM?.ObjectTableId == this.ObjectTableId){
                    console.warn(`_CustomsClosedTablePM={_CustomsClosedTablePM}`);
                }
                else   {
                    this.ValidationErrorsList=[];
                    this.ValidationErrorsList.push("You have no permission to view entities of this type. - Edit Component")
                    this.StopBusyIndicator();
                    throw new Error('You have no permission to view entities of this type. - Edit Component');
                }
            }
        }

        this.EntityPM = result;

        this.ClonedEntityPM = CloneDeep(this.EntityPM);

        if (this.EntityFields) {
            this.EntityFields.forEach(itemField => {
                this.EntityPM[itemField["FieldName"]] = itemField["FieldValue"];
            });
            this.EntityPM.IsDirty = false;
        }
        if (this.EntityPM) {
            this.entityArgs.EntityPM = this.EntityPM;
            this.entityArgs.ObjectTableName = this.ObjectTableName;
            this.entityArgs.EditComponent = this;
            this.SendActivityLog();
            this.BuildComponent();
            if (this.IsSplitComponentOpened) {
                this.LoadSplitComponent();
            }
        }
        else {
            this.StopBusyIndicator();
            this.ValidationErrorsList.push("Error displaying this " + TextCodeTranslator.Translate(this.ObjectTableName));
        }
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
            this.CheckDigtialPortalAddsOnPackage();
        }
    }

    CheckDigtialPortalAddsOnPackage() {
        if (this.ObjectTableName == "TenantManagement") {
            this.IsDigitalAddsOn = false;
            var globalDomainService = new GlobalDomainService();
            globalDomainService.CheckDigitalPortalAddsOn(this.EntityPM.Id).subscribe((result: any) => {
                var addOnPackage = result.Result;
                if (addOnPackage != null) {
                    this.IsDigitalAddsOn = true;
                }

                this.ContinueBuildComponent();

            });
        }
        else {
            this.ContinueBuildComponent();
        }
    }

    private ContinueBuildComponent() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response: any) => {
            this.GetControllerByTableName(this.ObjectTableName).then(EditComponentController => {
                //this.EditComponentController = EditComponentController as IEditComponentController;
                this.CurrentSession.AddEditComponent(this);
                this.EditComponentController = EditComponentController as IEditComponentController;
                this.EditComponentController.OnFirstTimeAfterSingleDataLoaded(this.EntityPM).then((isLock) => {
                    this.OnFirstTimeAfterSingleDataLoaded.emit(".EditComponentController.OnFirstTimeAfterSingleDataLoaded");
                    if (this.EditComponentController.ToCancell) {
                        this.Close();
                    }

                    else {

                        this._SubEditComponentDefaultController =
                            this.SaveCompleted.subscribe(isSaved => {
                                if (isSaved) {
                                    this.EditComponentController.HaveSaved = true;
                                    this._SubEditComponentDefaultController.unsubscribe();
                                    this._SubEditComponentDefaultController == null;
                                }
                            });
                            let sync: boolean = true;
                            if (sync) {
                                this.BuildEditTabs(() => {
                                    this.RunComponent();
                                    this.cd.detectChanges(); // to let HTML read split component location
                                    this.StopBusyIndicator();
                                });

                            } else {
 
                                 this.BuildEditTabs(null);
 
                                 this.RunComponent();
 
                                 this.StopBusyIndicator();
 
 
                    }
 

                         }
                    });
 
            });
        });
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
                this.SetNextPreviousButtonsEnablity();
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
                        cmpRef.instance.Run({ EntityPM: this.EntityPM, ObjectTable: this.ObjectTable, QuerySection: this.QuerySection });
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

        var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/MetaDataServices/HeaderScreenServices/" + this.GetObjectTableName() + "HeaderScreenService";
        SessionLocator.DynamicLoader.GetInstance(myComponentPath, true).then((headerScreenService: any) => {
            if (headerScreenService) {
                let result: HeaderScreenDataResult = headerScreenService.GetHeaderScreens({ ObjectTableId: this.ObjectTableId, EntityPM: this.EntityPM });
                this.GenerateHeaderScreen(result.HeaderScreen, result.ObjectFields);
            }
            else {
                this.BuildStandardHeaderScreen();
            }

        });

    }

    BuildStandardHeaderScreen() {

        var myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === this.ObjectTableId && (d.Code.indexOf("HeaderScreen") != -1 || d.IsHeaderScreen == true))[0];

        var myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTableId);

        if (this.ObjectTableName == "Shipment") {
            if (this.EntityPM.ShipmentLevelCode == "C") {
                this._entityResourceService.getEntityResourceByTableName("Master", 0).subscribe((response: any) => {


                    const masterObjectTable = window.ObjectTables.filter(x => x.Name === "Master")[0];
                    const shipmentObjectTable = window.ObjectTables.filter(x => x.Name === "Shipment")[0];

                    myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === masterObjectTable.Id && d.Code.indexOf("HeaderScreen") != -1 && d.QuerySection == this.QuerySection)[0];

                    const masterFields = window.ObjectFields.filter(d => d.ObjectTableId === masterObjectTable.Id);
                    const shipmentFields = window.ObjectFields.filter(d => d.ObjectTableId === shipmentObjectTable.Id);

                    myObjectFields = [...masterFields, ...shipmentFields];


                    this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
                });
            }

            else {
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        }

        else if (this.ObjectTableName == "ARInvoice") {
            var myObjectTable = window.ObjectTables.filter(x => x.Name === "ARInvoice")[0];
            var myObjectTableId = myObjectTable.Id;

            //get f. acc. Settings
            if (SessionLocator.TenantPM.AccountingActivated) {
                if (this.EntityPM.ARInvoiceTypeCode == "IT") {
                    myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code == "ARInvoice.InterestInvoiceHeaderScreen")[0];

                }
                else {
                    myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code == "ARInvoice.FullAccHeaderScreen")[0];
                }
                myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === myObjectTableId);
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }

            else {
                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code == "ARInvoice.HeaderScreen")[0];

                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        }
        else if (this.ObjectTableName == "ARPayment") {
            var myObjectTable = window.ObjectTables.filter(x => x.Name === "ARPayment")[0];
            var myObjectTableId = myObjectTable.Id;

            //get f. acc. Settings
            if (SessionLocator.TenantPM.AccountingActivated) {

                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code == "ARPayment.FullACCHeaderScreen")[0];
                myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === myObjectTableId);
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
            else {

                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code == "ARPayment.HeaderScreen")[0];

                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        }
        else if (this.ObjectTableName == "APPayment") {
            var myObjectTable = window.ObjectTables.filter(x => x.Name === "APPayment")[0];
            var myObjectTableId = myObjectTable.Id;

            //get f. acc. Settings
            if (SessionLocator.TenantPM.AccountingActivated) {

                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code == "APPayment.FullACCHeaderScreen")[0];
                myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === myObjectTableId);
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }

            else {
                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === myObjectTableId && d.Code == "APPayment.HeaderScreen")[0];
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        }

        else if (this.ObjectTableName == "APInvoice") {
            this.GenerateAPInvoiceHeader(myHeaderScreen, myObjectFields);
        }

        else if (this.ObjectTableName == "Tariff") {
            if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "IFT") {
                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === this.ObjectTableId && d.Code == "Tariff.SurchagesHeaderScreen")[0];
                myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTableId);
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }

            else if (this.EntityPM.TypeCode == "ECC" || this.EntityPM.TypeCode == "ICC") {
                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === this.ObjectTableId && d.Code == "Tariff.CustomsChargesHeaderScreen")[0];
                myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTableId);
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }

            else if (this.EntityPM.TypeCode == "ICS" || this.EntityPM.TypeCode == "ECS") {
                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === this.ObjectTableId && d.Code == "Tariff.SaleLocalChargesHeaderScreen")[0];
                myObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTableId);
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }

            else {
                myHeaderScreen = window.Screens.filter(d => d.ObjectTableId === this.ObjectTableId && d.Code == "Tariff.HeaderScreen")[0];
                this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
            }
        }

        else {
            this.GenerateHeaderScreen(myHeaderScreen, myObjectFields);
        }
    }

    private GenerateAPInvoiceHeader(headerScreen: any, objectFields: any) {


        if (SessionLocator.TenantPM.AccountingActivated) {
            var objectTable = window.ObjectTables.filter(x => x.Name === "APInvoice")[0];
            var objectTableId = objectTable.Id;

            headerScreen = window.Screens.filter(d => d.ObjectTableId === objectTableId && d.Code == "APInvoice.FullACCHeaderScreen")[0];
            objectFields = window.ObjectFields.filter(d => d.ObjectTableId === objectTableId);
            this.GenerateHeaderScreen(headerScreen, objectFields);
        }
        else {
            this.GenerateHeaderScreen(headerScreen, objectFields);
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


            this.GenerateHeaderScreen(HeaderScreen, ObjectFields);
        }

        else {
            this.FindHeaderRetries++;



            if (this.FindHeaderRetries < 3) {
                this.FindHeaderTimerToken = setTimeout(() => this.RunFindHeaderTimer(HeaderScreen, ObjectFields), 1);
            }
        }
    }

    private GenerateHeaderScreen(HeaderScreen: any, ObjectFields: ObjectFieldPM[]) {

        var element = document.getElementById(this.HeaderId);
        if (element == null) {
            this.RunFindHeaderTimer(HeaderScreen, ObjectFields);
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

                else if (HeaderScreen.NumberOfRows >= 3) {
                    this.HeaderScreenHeight = 75;
                    this.HeaderScreenRowHeight = 20;
                }

                var myScreenFields: any[] = window.ScreenFields.filter(d => d.ScreenCode === HeaderScreen.Code && d.Tenant == SessionLocator.Tenant);
                if (myScreenFields.length == 0) {
                    myScreenFields = window.ScreenFields.filter(d => d.ScreenCode === HeaderScreen.Code && d.Tenant == 0);
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

                    if (widthOfHeader == 0) {
                        var ApplicationSession = document.getElementById("ApplicationSession");
                        if (ApplicationSession) {
                            var appWidth = ApplicationSession.clientWidth;
                            widthOfHeader = appWidth - 42;
                        }
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
                            var myObjectField = ObjectFields.filter(d => d.FieldCode === myScreenField.ObjectFieldCode)[0];
                            if (myObjectField != null) {

                                myRow.Label = TextCodeTranslator.Translate(myObjectField.FullNameTextCodeCode);
                                myRow.ObjectField = myObjectField;

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
    private SingleDetailsTab: any = null;
    private BuildEditTabs(onCallBack: () => void ) {

        if (this.IsTabsHidden) {
            this.BuildSingleEditTab();
        }

        else {
            
            this.BuildTabsItemsSource(onCallBack);

        }
    }
    private BuildSingleEditTab() {
        var singleTab = window.ObjectTableTabs.filter(d => d.ObjectTableId === this.ObjectTableId && d.IndexOrder === 0)[0];
        if (singleTab) {
            if (FeatureLocator.IsFeatureGrantedByUniqeCode(singleTab.FeatureUniqeCode)) {
                if (!AppTool.IsNullOrEmpty(singleTab.HtmlComponentUrl)) {
                    this.SingleDetailsTab = singleTab;
                }
            }
        }
    }

    private CheckTabVisibility() {
        if (this.ObjectTableName != "EntityStatus" && this.ObjectTableName != "DeploymentPackage") return false;
        let isAllowedUser = SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor;
        if (this.ObjectTableName == "DeploymentPackage")
            return isAllowedUser;
        let entityStatusFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "EST")[0];
        return (isAllowedUser && entityStatusFeatureToggle);
    }

    FillTabsItemsSource(allTabs: any[]) {

        var myTabsSorted: any[] = [];

        allTabs = this.FilterTabs(allTabs);
        allTabs = allTabs.sort((a, b) => { return a.IndexOrder - b.IndexOrder });
        for (var i = 0; i < allTabs.length; i++) {

            var tab: ObjectTableTabPM = allTabs[i];
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

            }
 

            if (this.ObjectTableName == "PortTimeZone") {
                myTabsSorted.push(tab);
                 if (
                    AmitalGatewayUtil.Instance.AmitalBrowserInUse && 
                    (this.QuerySection === "Customs.ExportDeclaration" || this.BackButtonLabel.includes(TextCodeTranslator.Translate("Customs.Containerization.O.ExportFile"))) && 
                    tab.ControlPath.includes(".DeclarationDocsInControl"))
                    continue;                    
            }

            else {


                if (tab.Type == 'Custom' || this.CheckTabVisibility() || FeatureLocator.IsFeatureGrantedByUniqeCode(tab.FeatureUniqeCode)) {

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
                            case "GAIT":
                                {
                                    if (this.EntityPM.AccountTypeCode == "2")  // 2- Customer GLAccount
                                        myTabsSorted.push(tab);
                                    break;
                                }
                            case InterestTransactionTabCode:
                                {
                                    if (this.EntityPM.AccountTypeCode == CustomerGLAccountTypeCode)
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
        }

        myTabsSorted.forEach(item => {
            var itemTab: TabItem = new TabItem(item, this.EntityId, this);
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
    


    private BuildTabsItemsSource(onCallBack: () => void ) {

        this.TabsItemsSource = [];
        var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/MetaDataServices/TabsServices/" + this.GetObjectTableName() + "TabsService";
        SessionLocator.DynamicLoader.GetInstance(myComponentPath, true).then((tabsService: any) => {
            let allTabs = window.ObjectTableTabs.filter(d => d.ObjectTableId === this.ObjectTableId);
            if (tabsService) {
                allTabs = tabsService.GetTabs({ ObjectTableId: this.ObjectTableId, EntityPM: this.EntityPM });
            }

            this.FillTabsItemsSource(allTabs);
            if (this.TabControlBodyViewContainerRef)
                this.SetSelectedTab();


            onCallBack();
        });

    }


    private GetObjectTableName() {

        if (this.ObjectTable.Name.indexOf('Customs.') > -1) {
            return this.ObjectTable.Name.split('.')[1];
        }
        else return this.ObjectTable.Name;
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

                    if (this.EntityPM.ShipmentLevelCode == "C") {
                        var indexOfTab = allTabs.findIndex(t => t.Code == "SHPI");
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
                            if (!ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagInShipment) {
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


            case "ARPayment": {

                if (this.EntityPM.IsFullAccounting) {
                    let indexOfTab = allTabs.findIndex(t => t.Code == 'ARPD');
                    if (indexOfTab > -1)
                        allTabs.splice(indexOfTab, 1);

                }
                else {
                    let indexOfTab = allTabs.findIndex(t => t.Code == 'PYDF');
                    if (indexOfTab > -1)
                        allTabs.splice(indexOfTab, 1);
                }

                break;
            }

        }
        this.EditComponentController.FilterTabs(allTabs);



        return allTabs;
    }
    private RemoveItemByCode(items: any[], code: string) {
        let indexOfTab = items.findIndex(t => t.Code == code);
        if (indexOfTab > -1)
            items.splice(indexOfTab, 1);
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
        if (this.IsTabsHidden) {
            if (this.SingleDetailsTab) {
                SessionLocator.DynamicLoader.Load("./Infrastructure/Components/EditComponent/EditTabComponent", this.TabControlBodyViewContainerRef)
                    .then(cmpRef => {


                        this.entityArgs.PreSelectedTabCode = this.PreSelectedTabCode;
                        this.entityArgs.EditComponentArgument = this.EditComponentArgument;
                        cmpRef.instance.CurrentlySelected = true;
                        cmpRef.instance.Run(this.SingleDetailsTab.Code, this.SingleDetailsTab.HtmlComponentUrl);
                    });
            }
        }

        else {
            if (this.TabsItemsSource != null) {
                var selected: any = null;
                if (this.IsPendingApprovalDocumentQuery())
                    selected = this.TabsItemsSource.filter(d => d.Code == "SHDI")[0];

                if (this.ObjectTableName == "Container" && FeatureLocator.IsFeatureGrantedByUniqeCode("Container.Container.Tab.Routings")) {
                    selected = this.TabsItemsSource.filter(d => d.Code == "CORO")[0];
                }

                else {
                    if (this.PreSelectedTabCode != null) {
                        selected = this.TabsItemsSource.filter(d => d.Code == this.PreSelectedTabCode)[0];
                    }
                }

                if (selected == null) {
                    selected = this.TabsItemsSource[0];
                }

                this.SelectionChanged(selected);
            }
        }
    }

    IsPendingApprovalDocumentQuery(): boolean {
        if (!FeatureLocator.IsFeatureGrantedByUniqeCode("Shipment.DOCSIN"))
            return false;
        if (this.ObjectTableName != "Shipment")
            return false;
        if (this.SelectedQueryCode != "Pending Approval Documents")
            return false;
        return true;
    }

    SelectionChanged(mySelectedTab: TabItem) {
        if (this.SelectedTab != mySelectedTab) {
            this.SelectedTab = mySelectedTab;

            if (this.LoadedTabsList == null) {
                this.LoadedTabsList = [];
            }
            this.entityArgs.SelectedTabCode = this.SelectedTab ? this.SelectedTab.Code : "";

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
        var IsARInvoiceNeedsConfirmation = this.CheckIfFullAccountingARInvoiceNeedsConfirmation();
        var isNeedingConfirmation = this.NeedCloseConfirmation();
        const showBankTransferConfirmation = (this.EntityPM.IsDirty && this.ObjectTableName == "ARPayment" && this.EntityPM.ForceUsingBankTransferMethod && SessionLocator.TenantPM.AccountingActivated);

        const showAPPAymentConfirmation = (this.EntityPM.IsDirty && this.ObjectTableName == "APPayment" && this.EntityPM.ReconcileInternalTransIds && SessionLocator.TenantPM.AccountingActivated);

        if (showBankTransferConfirmation) {
            this.ShowBankTransferConfirmationMessage();
        }

        else if (IsARInvoiceNeedsConfirmation) {
            this.ShowConfirmationMessageForARInvoice();
        }

        else if (showAPPAymentConfirmation) {
            this.ShowAPPaymentConfirmationMessage();
        }

        else if (isNeedingConfirmation) {
            this.ShowConfirmationMessageForEntity();
        }

        else {
            this.Close();
        }
    }

    CheckIfFullAccountingARInvoiceNeedsConfirmation() {
        const StatusCode_AutoCreditARInvoice = "AC";
        if (this.ObjectTableName == "ARInvoice" && SessionLocator.TenantPM.AccountingActivated) {
            if (!this.EntityPM.IsDirty) {
                return false;
            }

            if (this.EntityPM.StatusCode == StatusCode_AutoCreditARInvoice) {
                return true;
            }
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

        if (this.ObjectTableName == "ARInvoice" && this.EntityPM.ARInvoiceTypeCode == this.InterestInvoiceARInvoiceType) {
            myResult = false;
        }

        if (this.ObjectTableName == "WorkFlow" && this.entityArgs?.EditComponentArgument?.HasChanges! == true) {
            myResult = true;
        }

        return myResult;
    }

    private ShowConfirmationMessageForARInvoice() {
        var yesAction = (): void => {
            this.Close();
        };

        this.ShowConfirmationMessage(new ConfirmationMessageArgs()
            .Builder
            .YesText(TextCodeTranslator.Translate('Accounting.General.B.OK'))
            .NoText(TextCodeTranslator.Translate('Accounting.General.B.Cancel'))
            .ShowCancelButton(false)
            .MessageText(TextCodeTranslator.Translate("ARInvoice.M.ConfirmNotAutoCreditedIfNotApproveInvoice"))
            .YesAction(yesAction)
            .build());
    }

    private ShowConfirmationMessageForEntity() {
        var yesAction = (): void => {
            this.SaveEntityChanges(true);
        };

        var noAction = (): void => {
            this.Close();
        };
        if(this.ObjectTableName == "Customs.Declaration"){
            this.ShowConfirmationMessage(new ConfirmationMessageArgs()
            .Builder
            .YesText(TextCodeTranslator.Translate('General.B.Save'))
            .NoText(TextCodeTranslator.Translate('General.B.DontSave'))
            .MessageText(TextCodeTranslator.Translate("Customs.Declaration.O.UnSavedDeclarations"))
            .ShowCancelButton(true)
            .YesAction(yesAction)
            .NoAction(noAction)
            .build());
        }
        else{
        this.ShowConfirmationMessage(new ConfirmationMessageArgs()
            .Builder
            .YesText(TextCodeTranslator.Translate('General.B.Save'))
            .NoText(TextCodeTranslator.Translate('General.B.DontSave'))
            .MessageText(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName)))
            .ShowCancelButton(true)
            .YesAction(yesAction)
            .NoAction(noAction)
            .build());
        }
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

    private ShowConfirmationMessage(ConfirmationMessageArgs: ConfirmationMessageArgs) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.ShowCancelButton = ConfirmationMessageArgs.ShowCancelButton;
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
        confirmWindow.YesButtonText = ConfirmationMessageArgs.YesText;
        confirmWindow.NoButtonText = ConfirmationMessageArgs.NoText;
        confirmWindow.Show(ConfirmationMessageArgs.MessageText);

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                ConfirmationMessageArgs.YesAction();
            }
            else if (confirmWindow.No) {
                ConfirmationMessageArgs.NoAction();
            }
        });
    }

    ShowBankTransferConfirmationMessage() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.ShowNoButton = false;
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Ok");
        confirmWindow.Show(TextCodeTranslator.Translate("ExternalReconciliation.O.BTUnsavedChanges"));

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes)
                this.Close();
        });
    }

    ShowAPPaymentConfirmationMessage() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.ShowNoButton = false;
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Ok");
        confirmWindow.Show(TextCodeTranslator.Translate("APPayment.M.UnsavedAPPaymentAlert"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes)
                this.Close();
        });
    }


    SaveChanges(busyIndicatorText: string = null) {

        if (this.EntityPM.IsDirty) {
            if (this.ObjectTableName == "Customs.Claim") {
                this.ValidateClaim();
            }
            if (this.IsEditValid) {
                this.SaveEntityChanges(false, busyIndicatorText);
            }
            else {
                this.FireSaveCompleted(false);
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

            this.SaveStart.emit(this.EntityPM)

            if ((this.ObjectTableName == "ARInvoice" || this.ObjectTableName == "APInvoice" || this.ObjectTableName == "ARPayment" || this.ObjectTableName == "APPayment"
                || this.ObjectTableName == "BankDeposit" || this.ObjectTableName == "UserDefinedReport" || this.ObjectTableName == "Journal" || this.ObjectTableName == "AccountingIntegrityCheck") && AppTool.IsNullOrEmpty(this.EntityPM.Id)) { // customs: notification defenetion, new declaration
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
                            
                            if(this.ObjectTableName == "ARInvoice" && myResponse.Result?.ConfirmationNumberStatus==5){
                                const messageWindow = new MessageWindow();
                                messageWindow.Title=TextCodeTranslator.Translate("ARInvoice.O.ConfirmationNumberFailedTitle");
                                messageWindow.RTL=true;
                                messageWindow.Height=250;
                                messageWindow.Width=420;
                                messageWindow.IsMessageMultiLine=true
                                messageWindow.LayoutDirection='rtl'
                                var text=TextCodeTranslator.Translate("ARInvoice.O.ConfirmationNumberFailedText")+'\n'+TextCodeTranslator.Translate("ARInvoice.O.ConfirmationNumberErrorDetails")+
                                 '\n'+ myResponse.Result?.APIResponseToConfirmation;
                                messageWindow.Show(text);
                            } 
                            
                            this.EntityPM = myResponse.Result;
                            this.EntityId = this.EntityPM.Id;
                            this.entityArgs.EntityPM = this.EntityPM;
                            this.isEntityChange = true;

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
                                this.SetNextPreviousButtonsEnablityAysnc();
                                //if (this.nextPreviousTimerToken) {
                                //    clearTimeout(this.nextPreviousTimerToken);
                                //}
                                //this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
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

            else if (this.ObjectTableName == "WorkFlow" && this.entityArgs?.EditComponentArgument?.HasChanges! == true) {
                this.entityPMService.update(this.ObjectTableName, this.EntityPM, this.ClonedEntityPM).then((res: any) => {
                    res.subscribe((myResponse: ServiceResponse) => {
                        if (myResponse.HasError) {
                            this.OnSavingFailed();
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                            this.FireSaveCompleted(false);
                        }
                        else {
                            this.EntityPM = myResponse.Result;
                            this.entityArgs.EntityPM = this.EntityPM;

                            this.SaveDraftVersion(isClosing);

                            this.SaveAndCloseCompleted.emit(true);


                            this.ClonedEntityPM = CloneDeep(this.EntityPM);
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

                this.entityPMService.update(this.ObjectTableName, this.EntityPM, this.ClonedEntityPM).then((res: any) => {
                    res.subscribe((myResponse: ServiceResponse) => {

                        this.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.OnSavingFailed();
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                            this.FireSaveCompleted(false);
                        }

                        else {
                            if (this.SelectedTab.Code == "DCCF" ) {
                                DeclarationEventManager.SavePendingAfterDeclarationSaved.emit(null);;
                            }
                            this.EntityPM = myResponse.Result;
                            this.entityArgs.EntityPM = this.EntityPM;
                            this.isEntityChange = true;
                            
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
                                    this.SetNextPreviousButtonsEnablityAysnc();
                                    //if (this.nextPreviousTimerToken) {
                                    //    clearTimeout(this.nextPreviousTimerToken);
                                    //}
                                    //this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
                                }
                                if (loadPreviousEntity) {
                                    this.CurrentNavigatedIndex = this.CurrentNavigatedIndex - 1;
                                    this.LoadNextPreviousEntity();
                                    this.SetNextPreviousButtonsEnablityAysnc();
                                    //if (this.nextPreviousTimerToken) {
                                    //    clearTimeout(this.nextPreviousTimerToken);
                                    //}
                                    //this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
                                }

                            }
                        }
 
                            this.ClonedEntityPM = CloneDeep(this.EntityPM);
                        });
 
                    }, error => {
                        this.OnSavingFailed();
                        this.StopBusyIndicator();
                        var myErrors: string[] = [];
                        myErrors.push(error.message);
                        this.ValidationErrorsList = myErrors;
                        this.FireSaveCompleted(false);
                    });
                
            
            }
        }

        else if (this.ObjectTableName == "WorkFlow" && this.entityArgs?.EditComponentArgument?.HasChanges! == true) {
            this.SaveDraftVersion(isClosing);
        }

        else {
            this.Close();
        }
    }

    public WorkFlowVersionPMService: WorkFlowVersionPMService = new WorkFlowVersionPMService();
    SaveDraftVersion(isClosing: boolean) {
        var CurrentDisplayedVersionId = this.entityArgs.EditComponentArgument?.CurrentDisplayedVersionId
        var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == CurrentDisplayedVersionId);

        this.StartBusyIndicator("Saving ...");

        this.WorkFlowVersionPMService.update(version).subscribe((serviceResponse: ServiceResponse) => { this.handleSaveDraftVersionResponse(serviceResponse, isClosing); });
    }
    handleSaveDraftVersionResponse(serviceResponse: ServiceResponse, isClosing: boolean) {
        if (!serviceResponse.HasError) {
            this.StopBusyIndicator();
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, HasChanges: false }
            this.entityArgs.SendMessage("RefreshWorkflowButtons");
            if (isClosing) {
                this.Close();
            }
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

            if (this.ObjectTableName == "Reconciliation") {
                let service = new ReconciliationExtendedPMService();
                service.GetSingleWithoutLines(this.EntityId).subscribe((response: ServiceResponse) => {
                    console.log("[GetSingleWithoutLines] ", response);

                    if (!response.HasError) {
                        var reconciliation = response.Result;

                        this.EntityPM = reconciliation;
                        this.entityArgs.EntityPM = this.EntityPM;

                        this.EditComponentController.OnReloadEntityPM().then((isLock) => {
                            this.StopBusyIndicator();
                            this.UpdateComponentMembers();
                            this.LoadCompleted.emit(true);
                        });

                    }
                    else {
                        this.StopBusyIndicator();
                        this.ValidationErrorsList = response.ErrorsArray;
                        this.LoadCompleted.emit(false);
                    }
                });


            }
            else if (this.ObjectTableName == "CashBook") {
                let service = new CashBookExtendedPMService();
                service.GetSingleWithoutLines(this.EntityId).subscribe((response: ServiceResponse) => {
                    console.log("[GetSingleWithoutLines] ", response);

                    if (!response.HasError) {
                        var cashbook = response.Result;

                        this.EntityPM = cashbook;
                        this.entityArgs.EntityPM = this.EntityPM;

                        this.EditComponentController.OnReloadEntityPM().then((isLock) => {
                            this.StopBusyIndicator();
                            this.UpdateComponentMembers();
                            this.LoadCompleted.emit(true);
                        });

                    }
                    else {
                        this.StopBusyIndicator();
                        this.ValidationErrorsList = response.ErrorsArray;
                        this.LoadCompleted.emit(false);
                    }
                });


            }
            else if (this.ObjectTableName == "BankDeposit") {
                let service = new BankDepositExtendedPMService();
                service.GetSingleWithoutLines(this.EntityId).subscribe((response: ServiceResponse) => {
                    console.log("[GetSingleWithoutLines] ", response);

                    if (!response.HasError) {
                        var bankdeposit = response.Result;

                        this.EntityPM = bankdeposit;
                        this.entityArgs.EntityPM = this.EntityPM;

                        this.EditComponentController.OnReloadEntityPM().then((isLock) => {
                            this.StopBusyIndicator();
                            this.UpdateComponentMembers();
                            this.LoadCompleted.emit(true);
                        });

                    }
                    else {
                        this.StopBusyIndicator();
                        this.ValidationErrorsList = response.ErrorsArray;
                        this.LoadCompleted.emit(false);
                    }
                });


            }
            else {
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
                                if (this.ObjectTableName == "Shipment") {
                                    this.CurrentSession.FireEvent("FollowupsChanged")
                                }
                         

                            });
                        }
                    });
                });
            }
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
        var notDefault = ["Declaration", "Vehicle","PhysicalCheck"];
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
        let entityId=this.EntityPM.Id
        this.FetchCustomsSetting();
        if ( EditComponent._CustomsSettingList?.CompanyType == "B") {
            entityId = "CourierD";//Task =172895
        }
        LastFilterClass.UpdateFilter("DeclarationEditControl", entityId, this.IsSplitComponentOpened ? "true" : "false");


    }

    LoadSplitComponent() {


        let locs = this.AllLocations.toArray();
        let myLocation: LocationDirective = locs.filter(f => f.Code == 'SplitComponentLocation')[0];

        console.log("Load SplitComponent @ ", myLocation);

        if (myLocation) {
            //this.myLocation.clear();
            var splitComponentPath = this.ObjectTable.SplitComponentPath;
            //var splitComponentPath = "./Customs/AngularModules/AngularModules/Customs/Components/Declaration/DeclarationSplitComponent";
            myLocation.viewContainerRef.clear();
            SessionLocator.DynamicLoader.Load(splitComponentPath, myLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.SetComponentArgs({ EntityPM: this.EntityPM });
                });
        }



    }
    
    async FetchCustomsSetting(){
        if (AppTool.IsNullOrEmpty(EditComponent._CustomsSettingList)){
            const dCustomsSettingListService: CustomsSettingListService = new CustomsSettingListService;
            const response = await dCustomsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString()).toPromise();
            EditComponent._CustomsSettingList = response.Result;
        }
    
    } 
    
 
    SetSplitComponentState() {
        if (this.IsSplitBtnVisible == false) {
            return;
        }





        let entityId = this.EntityPM.Id;
        this.FetchCustomsSetting();
        if ( EditComponent._CustomsSettingList?.CompanyType == "B") {
            entityId = "CourierD";//Task =172895
        }
        // state: opened / closed
        var defaultFilterCode: string = LastFilterClass.GetFilterValue("DeclarationEditControl", entityId);
        if (defaultFilterCode == "true") {
            if (!this.IsSplitComponentOpened)
                this.SplitButtonClicked(); // open split section
        }


    }

    //#endregion


    //navigation methods

    TextMoveToValue: number;
    MoveToTextBoxKeyUp(event) {
        //96542123
        if (!AppTool.IsNullOrEmpty(this.TextMoveToValue)) {

            let theSelectedIndex = this.TextMoveToValue;
            this.TextMoveToValue=null
            if (theSelectedIndex < 1) {
                theSelectedIndex = 1;
            }
            else if (theSelectedIndex > this.NavigationIds.length) {
                theSelectedIndex = this.NavigationIds.length;
            }

            this.CurrentNavigatedIndex = theSelectedIndex;
            this.Previous();

            
        }

    }

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

            this.SetNextPreviousButtonsEnablityAysnc();
            //if (this.nextPreviousTimerToken) {
            //    clearTimeout(this.nextPreviousTimerToken);
            //}
            //this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
        }
    }

    Previous() {

        if (this.EntityPM.IsDirty) {
            this.SaveEntityChanges(false, null, false, true);
        }
        else {
            this.CurrentNavigatedIndex = this.CurrentNavigatedIndex - 1;
            this.LoadNextPreviousEntity();

            this.SetNextPreviousButtonsEnablityAysnc();
            //if (this.nextPreviousTimerToken) {
            //    clearTimeout(this.nextPreviousTimerToken);
            //}
            //this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
        }
    }
    SetNextPreviousButtonsEnablityAysnc() {
        let move2Sync: Boolean = true;
        if (move2Sync) {
            return;
        }
        if (this.nextPreviousTimerToken) {
            clearTimeout(this.nextPreviousTimerToken);
        }
        this.nextPreviousTimerToken = setTimeout(() => this.SetNextPreviousButtonsEnablity(), 500);
    }

    nextPreviousTimerToken: any;
    LoadNextPreviousEntity() {

        var selectedTab = this.PreSelectedTabCode;
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
            this.EditComponentController.OnCloseEditControl(
                () => {
                    this.LoadNextPreviousEntity_AfterCloseEditControl(selectedTab);
                });
        } else {
            this.LoadNextPreviousEntity_AfterCloseEditControl(selectedTab);
        }

        //if (this.ComponentRef != null) {
        //  this.CurrentSession.RemoveEditComponent(this);
        //  this.ComponentRef.destroy();
        //  this.ComponentRef = null;
        //}



    }
    LoadNextPreviousEntity_AfterCloseEditControl(selectedTab:any) {
        this.CurrentSession.RemoveEditComponent(this);
 
        this.ngOnDestroy();
        var args: any = {};
        args.EntityId = this.NavigationIds[this.CurrentNavigatedIndex];
        args.ObjectTableName = this.ObjectTableName;
        args.BackButtonLabel = this.BackButtonLabel;
        args.NavigationIds = this.NavigationIds;
        args.SelectedTabCode = selectedTab;
        this.Run(args);

    }

    SetNextPreviousButtonsEnablity() {
        if (this.NavigationIds) {
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

    ValidateClaim() {
        this.IsEditValid = true;
        this.ValidationErrorsList = [];

        this.EntityPM.ClaimsRelatedEntities.forEach(entity => {
            if (AppTool.IsNullOrEmpty(entity.ClaimExplanation)) {
                this.IsEditValid = false;
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Claim.O.MissingClaimExplanation"));
            }
            if (AppTool.IsNullOrEmpty(entity.ClaimEntityNumber)) {
                this.IsEditValid = false;
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Claim.O.MissingClaimEntityNumber"));
            }
        });
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
export class TabItem {
    public Code: string;
    public EntityPM: any;
    public TextCode: string;
    public TextCodeId: string;
    public IsDisabled: boolean = false;
    private entityId: string;
    constructor(itemPM: any, entityId: any, public fatherComponent: EditComponent) {
        this.Code = itemPM.Code;
        this.EntityPM = itemPM;
        this.entityId = entityId;
        this.TextCode = this.GetTextCode(itemPM);
        this.TextCodeId = AppTool.Replace(this.TextCode, ".", "");
    }

    private GetTextCode(itemPM: any) {
        var textCode = itemPM.TabNameTextCodeCode;

        if (itemPM.TabNameTextCodeCode == "TenantManagement.TH.CargoTrackingBranding") {
            return this.CheckDigtialPortalAddsOnPackage(itemPM);
        }

        return textCode;
    }

    CheckDigtialPortalAddsOnPackage(itemPM: any): string {
        var textCode = itemPM.TabNameTextCodeCode;
        if (this.fatherComponent.IsDigitalAddsOn) {
            textCode = "TenantManagement.TH.LogitudeDigitalBranding";
        }

        return textCode;
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
    OnCloseEditControl(onCallBack?: () => void ) {
        if (!AppTool.IsNullOrEmpty(onCallBack)) {
            onCallBack();
        }
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
    FilterTabs(allTabs: any[]) {

    }

}
export interface IEditComponentController {
    OnFirstTimeAfterSingleDataLoaded(CurrentEntity): Promise<boolean>;
    OnReloadEntityPM(): Promise<any>;
    OnCloseEditControl(onCallBack?: () => void ): void;
    HaveSaved: boolean;
    InDisplayMode: boolean;
    ToCancell: boolean;
    InDisplayModeMessage: string;
    MustRefresh: boolean;
    MustRefreshMessage: string;
    IsInBatchRequest: boolean;
    ResetMustRefresh(): void;
    IsDisabled(itemTabCode: string): boolean;
    FilterTabs(allTabs: any[]);
}

