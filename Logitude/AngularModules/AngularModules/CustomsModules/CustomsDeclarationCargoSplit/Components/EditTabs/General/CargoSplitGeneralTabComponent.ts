declare var window: any;
import { Component, Input, AfterContentInit, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DateTool, AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationCargoSplitPM } from '../../../../../Customs/EntityPMs/DeclarationCargoSplitPM';
import { DecCargoSplitCargoIdentifierPM } from '../../../../../Customs/EntityPMs/DecCargoSplitCargoIdentifierPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { DeclarationList } from '../../../../../Customs/EntityLists/DeclarationList';
import { ConsignmentPM } from '../../../../../Customs/EntityPMs/ConsignmentPM';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { LogDatePickerComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/LogDatePickerComponent';
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { SendRequestVIA } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
//import {INF_MSG_GenericResponseData} from '../../DataContract/ResponseData/INF_MSG_GenericResponseData';//4
import { IIGGeneralMessagesService } from '../../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { EntityPMService } from '../../../../../Infrastructure/Services/EntityPMService';
import { DeclarationCargoSplitController } from '../../Controller/DeclarationCargoSplitController';
import { DeclarationCargoSplitPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationCargoSplitPMService';
import { DeclarationMessagesService } from '../../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { CargoSplitRequestParams } from '../../../../../Customs/DataContract/RequestParams/CargoSplitRequestParams';
import { INF_MSG_GenericResponseData } from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { DecCargoSplitConPM } from '../../../../../Customs/EntityPMs/DecCargoSplitConPM';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { CargoIdentifireTypePM } from '../../../../../Customs/EntityPMs/CargoIdentifireTypePM';
import { CargoIdentifireTypeListService } from '../../../../../Customs/Services/StandardLists/CargoIdentifireTypeListService';
//import {DecCargoSplitConComponent} from '../DecCargoSplitConComponent';

@Component({
    
    templateUrl: './CargoSplitGeneralTabComponent.html',
})

export class CargoSplitGeneralTabComponent
    extends BaseComponent
//implements AfterViewInit, AfterContentInit{
{
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public DataContext: CargoSplitGeneralTabComponent = this;
    //public EntityPM: DeclarationCargoSplitPM;
    entityPM: DeclarationCargoSplitPM;
    public get EntityPM() { return this.entityPM; }
    public set EntityPM(val: DeclarationCargoSplitPM) {
        this.entityPM = val;
        this.BuildTabs();
    }
    public ObjectTableName: string = "Customs.DeclarationCargoSplit";
    public TabsItemsSource: TabItem[] = [];
    public Tabs: LogTab[] = [];
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];
    private currentEditComponentId: string;
    public IsDisplayOnly: boolean = false;
    public DisplayOnlyMessage: string = "";
    public ImporterCode: string = "";
    public IsCustomsFileRetrieved: boolean = false;
    public CargoIdentifiersList: ObservableCollection;
    public ExportCargoTypeFilterItems: ApiQueryFilters;

    FIELD_IS_REQUIERD: string;
    RequestVIA: SendRequestVIA;
    //public ItemsList: ObservableCollection;
    public decCargoSplitCargoIdentifierModel: DecCargoSplitCargoIdentifierModel;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    IsDelete: boolean = false;
    SendButtonEnabled: boolean = true;
    OKButtonEnabled: boolean = true;
    TabIndex: number;

    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService();
    private declarationCargoSplitController: DeclarationCargoSplitController;
    private _entityListService: EntityListService;
    declarationCargoSplitPMService: DeclarationCargoSplitPMService = new DeclarationCargoSplitPMService();
    declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();

    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();

    _LastFetchDeclarationList: DeclarationList;
    _LastFetchConsignmentPMList: ConsignmentPM[];
    requestParams: CargoSplitRequestParams = new CargoSplitRequestParams();
    responseData: INF_MSG_GenericResponseData = new INF_MSG_GenericResponseData();

    public XrayItems: XRayAvailableItem[] = [];

    SelectedDateTime: Date;


    _InputParam: EntityArgs;
    //@Input()
    //set DeclarationCargoSplitParam(val: EntityArgs) {
    //    this.entityArgs = this._InputParam = val;
    //    this.Init();
    //}
    _EntityResourceFinished: boolean = false
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new DeclarationCargoSplitPM();
        //this.ItemsList = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        //this.entityArgs.ObjectTableName = "Customs.DeclarationCargoSplit";
        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.DecCargoSplitCargoIdentifier").subscribe((response:any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.DecCargoSplitCon").subscribe((response:any) => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.DecCargoSplitConsItem").subscribe((response:any) => {
                            this.EntityResourceService.getEntityResourceByTableName("Customs.DecCargoSplitConsPackDet").subscribe((response:any) => {
                                this.EntityResourceService.getEntityResourceByTableName("Customs.Client").subscribe((response:any) => {
                                //this.Init();
                                //this.EntityPM = this.entityArgs.EntityPM;
                                    //this.ObjectTableName = this.entityArgs.ObjectTableName;
                                    this._EntityResourceFinished = true;
                                    this.ExportCargoTypeFilterItems = new ApiQueryFilters();
                                    this.ExportCargoTypeFilterItems.addAdditionalFilter("IsForDeclarationExport", true, null, null, "Equal", false, false, false, "boolean", false, true);
                                    this.Listen();
                                    //this.BuildTabs();

                                });
                            });
                        });
                    });
                });
            });
        });
        this.declarationCargoSplitController = new DeclarationCargoSplitController(this.EntityPM);
        this.CargoIdentifiersList = new ObservableCollection([]);
        if (!AppTool.IsNullOrEmpty(this.EntityPM) && this.EntityPM.DecCargoSplitCargoIdentifiers != null && this.EntityPM.DecCargoSplitCargoIdentifiers.length > 0) {
            this.CargoIdentifiersList.InsertCollection(this.EntityPM.DecCargoSplitCargoIdentifiers);
        }
        this._entityListService = new EntityListService();
        if (this.IsDisplayOnly) {
            this.SetDisplayFields(this.ResponseStatusCode);
        }
    }


    IsExportDeclaration: boolean = false;
    GetFileData() {
        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) return;
        this.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe((myDeclarationResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (myDeclarationResponse.Result == null || (myDeclarationResponse.Result != null && AppTool.IsNullOrEmpty(myDeclarationResponse.Result.Id))) {
                    this.CustomFileNo = "";
                    this.EntityPM.DeclarationId = null;
                }
                this._LastFetchDeclarationList = myDeclarationResponse.Result;
                if (AppTool.IsNullOrEmpty(this._LastFetchDeclarationList)) {
                    this.NoConnectedConsignmentEnableField();
                } else {
                    if (this._LastFetchDeclarationList.Direction == "E") {
                        this.IsExportDeclaration = true;
                        this.AddItem();
                    }
                    this.CurrentSession.StartBusyIndicator("")
                    this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
                        .subscribe((myResponse: ServiceResponse) => {
                            this.CurrentSession.StopBusyIndicator();
                            this.FetchConsignment(myResponse, false);
                        });
                }
            });
    }

    Init() {

        this.SetDisplayFields(this.ResponseStatusCode);
        this.GetFileData();
        this.InitCargoIdentifiers();
        //if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) {
        //    if (this.EntityPM != null && this.entityArgs != null) this.entityArgs.EntityPM = this.EntityPM;
        //    return;
        //}
        //if (this.EntityPM == null)this.EntityPM = this.entityArgs.EntityPM;
        //this.ObjectTableName = this.entityArgs.ObjectTableName;

    }

    SetDisplayFields(ResponseStatusCode: string) {
        this.UIProperties.SetEnabled("RequestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ResponseStatusCode", this.ObjectTableName, false);
        //if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) {
        //    if (this.IsNewEntity == false) return;
        //    if (this.RequestDate == null) this.RequestDate = DateTool.GetDateByDay(+0);
        //    return;
        //}
        if (this.ResponseStatusCode != "5" && this.ResponseStatusCode != "" && this.ResponseStatusCode != null) {
            this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestRemarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RequestReason", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, false);
            this.OKButtonEnabled = false;
            if (this.ResponseStatusCode != "3") {
                this.UIProperties.SetEnabled("ActionTypeCode", this.ObjectTableName, false);
                this.SendButtonEnabled = false;
            }
            else {
                this.UIProperties.SetEnabled("ActionTypeCode", this.ObjectTableName, true);
                this.SendButtonEnabled = true;
            }
        }
        else {
            this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestRemarks", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestReason", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ActionTypeCode", this.ObjectTableName, true);
            this.SendButtonEnabled = true;
            this.OKButtonEnabled = true;
        }
        if (this.SendButtonEnabled != true || this.OKButtonEnabled != true) {
            this.IsDisplayOnly = true;
        }

    }
    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        
        if (args.EntityPM instanceof DeclarationCargoSplitPM) this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        if (this.IsDisplayOnly != true && args.IsDisplayOnly == true) {
            this.IsDisplayOnly = args.IsDisplayOnly;
        }

        console.log("EntityPM", this.EntityPM);

    }

    public SendButtonsVisibility: boolean = false;


    //ngAfterViewInit() {
    //    if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) {
    //        if (this.IsNewEntity == false) return;
    //        if (this.RequestDate == null) {
    //            this.RequestDate = DateTool.GetDateByDay(+0);
    //        }
    //        else {
    //            this.RequestDate = this.RequestDate;
    //        }
    //        return;
    //    }
    //    // viewChildren is set
    //   // this.SetByAvailableTimeChecked();
    //}

    //ngAfterContentInit()
    //{
    //    if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) {
    //        if (this.IsNewEntity == false) return;
    //        if (this.RequestDate == null) {
    //            this.RequestDate = DateTool.GetDateByDay(+0);
    //        }
    //        else {
    //            this.RequestDate = this.RequestDate;
    //        }
    //        return;
    //    }
    //    //this.RequestDate = DateTool.GetDateByDay(+0);
    //    //this.ToDate = DateTool.GetDateByDay(+7);
    //    //this.SetByAvailableTimeChecked()
    //    //alert(this.AllDates);
    //    //this.UIProperties.SetRequired("RequestDate", this.ObjectTableName, true);
    //    //this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
    //}

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        if (this.CurrentSession.CurrentEditComponent.EntityPM instanceof DeclarationCargoSplitPM)this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.RefreshEntity();

                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        if (this.CurrentSession.CurrentEditComponent.EntityPM instanceof DeclarationCargoSplitPM)this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildTabs();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.currentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEGC") {
                            //this.RefreshEntity();
                            this.DisplayOnlyCheck();
                        }
                    }
                })
            );
        }
    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetDisplayFields(this.ResponseStatusCode);
            return;
        }
    }

    SetNewWizardArgs(args: any) {
        this.IsNewEntity = args['IsNewEntity'];
        if (this.IsDisplayOnly != true && args.IsDisplayOnly == true) {
            this.IsDisplayOnly = args.IsDisplayOnly;
        }
        this.Init();
        this.BuildTabs();
        this.RequestDate = DateTool.GetDateByDay(+0);
    }

    SetWindowArgs(winArg: any) {
        if (winArg.CurrentEntity instanceof DeclarationCargoSplitPM)this.EntityPM = winArg.CurrentEntity;
        if (!AppTool.IsNullOrEmpty(winArg.CustomFileNo)) {
            this.IsNewEntity = true;
            this.CustomFileNo = winArg.CustomFileNo;
            this.RequestDate = DateTool.GetDateByDay(+0);
            //this.CustomFileNoTextChanged(winArg.CustomFileNo);
        }
        this.Init();
        if (AppTool.IsNullOrEmpty(this.ImporterCode)) {
            if (this._LastFetchConsignmentPMList != null && this._LastFetchDeclarationList != null) {
                this.ImporterCode = this._LastFetchDeclarationList.ImporterCode;
            }
            else if (this.IsCustomsFileRetrieved != true) {
                this.CustomFileNoTextChanged("ImporterOnly");
            }
        }

        this.BuildTabs();
        if (this.IsDisplayOnly != true && winArg.IsDisplayOnly == true) {
            this.IsDisplayOnly = winArg.IsDisplayOnly;
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.DecCargoSplitCargoIdentifiers)) {
            for (let conItem of this.EntityPM.DecCargoSplitCargoIdentifiers) {
                var item = new DecCargoSplitCargoIdentifierModel(conItem);
                this.decCargoSplitCargoIdentifierModel = item;
                //this.ItemsList.Insert(item);
            }
        }
        //this.EntityPM = winArg.declarationPM;
    }

    AddItem() {
        if (!this.IsDisplayOnly) {
            var counter: number = 0;
            if (this.EntityPM.DecCargoSplitCargoIdentifiers.length > 0) {

                //var items = this.EntityPM.DecCargoSplitCargoIdentifiers.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });
                //if (items.length == 0) counter = 0;
                //else {
                //    counter = items[this.EntityPM.DecCargoSplitCargoIdentifiers.length - 1].LineNumber;
                //}
                return;
            }

            counter += 1;

            var item: DecCargoSplitCargoIdentifierPM = new DecCargoSplitCargoIdentifierPM(this.EntityPM);

            item.DeclarationCargoSplitId = this.EntityPM.Id;
            item.Tenant = this.EntityPM.Tenant;
            item.LineNumber = counter;

            if (!this.EntityPM.DecCargoSplitCargoIdentifiers.includes(item)) {
                this.EntityPM.AddDecCargoSplitCargoIdentifier(item);
                var line = new DecCargoSplitCargoIdentifierModel(item);
                this.decCargoSplitCargoIdentifierModel = line;
                //this.ItemsList.Insert(line);
            }

        }

    }

    //OnRowEnded($event) {
    //    console.log("this.ItemsList.Length : " + this.ItemsList.Length);
    //    if (($event) == this.ItemsList.Length) {
    //        this.AddItem();
    //    }
    //}

    //public SelectedRow: any = null;
    //OnRowSelected(itemComponent: any) {
    //    this.SelectedRow = itemComponent;
    //    if (this.ItemsList.Length == 0) {
    //        this.AddItem();
    //    }
    //}

    //OnFocus() {
    //    if (this.ItemsList.Length == 0) {
    //        this.AddItem();
    //    }
    //}

    //DeleteButtonClicked(item: any) {

    //    let confirmWindow = new ConfirmWindow();
    //    confirmWindow.Width = 300;
    //    confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");

    //    confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeletePackage"));
    //    confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
    //    confirmWindow.WindowClosed.subscribe((event: any) => {
    //        if (confirmWindow.Yes) {
    //            this.DeleteSelected(item);
    //        }
    //        else if (confirmWindow.No) {

    //        }
    //    });
    //}

    //lastDeletedItem: DecCargoSplitCargoIdentifierPM;
    //DeleteSelected(item: any) {
    //    this.lastDeletedItem = item.EntityPM;

    //    this.ItemsList.Remove(item);
    //    this.EntityPM.RemoveDecCargoSplitCargoIdentifier(item.EntityPM);

    //}



    BuildTabs() {
        var tab;
        this.Tabs = [];
        
        if (this.EntityPM.DecCargoSplitCons != null && this.EntityPM.DecCargoSplitCons.length > 0) {

            var items: DecCargoSplitConPM[] = this.EntityPM.DecCargoSplitCons.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });
            this.TabIndex = 0;
            for (let item of items) {

                tab = new LogTab();
                tab.EntityPM = item;
                //tab.Code = item.LineNumber;
                //tab.Header = item.LineNumber;
                tab.Code = ++this.TabIndex;
                tab.Header = this.TabIndex;
                tab.Parent = this.EntityPM;
                tab.IsDisplayOnly = this.IsDisplayOnly;
                tab.ComponentPath = "./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/DecCargoSplitConComponent";
                this.Tabs.push(tab);
            }
        }
        else {
            this.AddTab(null);
        }

        this.SelectedTab = this.Tabs[0];
    }

    AddTab(event) {

        if (this.IsDisplayOnly) {
            return;
        }
        this.TabIndex = 0;

        if (this.Tabs.length > 0) {
            //var maxObj = this.EntityPM.DecCargoSplitCons.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current });
            var maxObj = this.Tabs.reduce(function (prev, current) { return (prev.Code > current.Code) ? prev : current });
            var code: number = parseInt(maxObj.Code);
            if (maxObj != null) {
                if (this.TabIndex <= code)
                    this.TabIndex = code;
            }
        }
        this.TabIndex=(ArrayTool.Max(this.Tabs, "Code") );

        var Tab: DecCargoSplitConPM = new DecCargoSplitConPM(this.EntityPM);
        Tab.DeclarationCargoSplitId = this.EntityPM.Id;
        Tab.Tenant = this.EntityPM.Tenant;
        //Tab.LineNumber = ++this.TabIndex;
        ++this.TabIndex;
        Tab.LineNumber = (ArrayTool.Max(this.EntityPM.DecCargoSplitCons, "LineNumber") + 1);
        this.EntityPM.AddDecCargoSplitCon(Tab);

        // new tab
        if (AppTool.IsNullOrEmpty(this.ImporterCode)) {
            if (this._LastFetchConsignmentPMList != null && this._LastFetchDeclarationList != null) {
                this.ImporterCode = this._LastFetchDeclarationList.ImporterCode;
            }
            else if (this.IsCustomsFileRetrieved != true) {
                this.CustomFileNoTextChanged("ImporterOnly");
            }
        }
        var tab = new LogTab();
        Tab.ImporterCode = this.ImporterCode;
        tab.EntityPM = Tab;
        //tab.Code = Tab.LineNumber.toString();
        //tab.Header = Tab.LineNumber.toString();
        tab.Code = this.TabIndex.toString();
        tab.Header = this.TabIndex.toString();
        tab.Parent = this.EntityPM;
        tab.ComponentPath = "./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/DecCargoSplitConComponent";
        this.Tabs.push(tab);

        // select the tab
        this.SelectedTab = tab;
    }

    DeleteTab(tab: LogTab) {
        if (!AppTool.IsNullOrEmpty(tab)) {

            var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeleteConsignment");
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 300;
            confirmWindow.Height = 150;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
            confirmWindow.Show(msg);
            var t = tab;
            confirmWindow.WindowClosed.subscribe((event: any) => {

                if (confirmWindow.Yes) { // YES
                    tab = t;
                    var index = this.Tabs.indexOf(tab);
                    if (index < 0) {
                        console.log("The tab was not found, could not delete it :( ", tab);
                        return;
                    }
                    this.EntityPM.RemoveDecCargoSplitCon(tab.EntityPM);
                    this.Tabs.splice(index, 1);


                    for (var i = 0; i < this.EntityPM.DecCargoSplitCons.length; i++) {
                        var Tab = this.EntityPM.DecCargoSplitCons[i];
                        //Tab.LineNumber = i + 1;

                    }
                    for (var i = 0; i < this.Tabs.length; i++) {
                        var DecCargoSplitConTab: DecCargoSplitConPM = this.Tabs[i].EntityPM;
                        //DecCargoSplitConTab.LineNumber = i + 1;
                        //this.Tabs[i].Code = DecCargoSplitConTab.LineNumber.toString();
                        //this.Tabs[i].Header = DecCargoSplitConTab.LineNumber.toString();
                        this.Tabs[i].Code = (i + 1).toString();
                        this.Tabs[i].Header = (i + 1).toString();
                    }

                    // select the last tab
                    var tab = this.Tabs[0];
                    this.SelectedTab = tab;
                }
            });

        }
    }

    SwitchTabsForExportORImport(direction) {
        for (var tab of this.Tabs) {
            var index = this.Tabs.indexOf(tab);
            if (index > -1) {
                //this.EntityPM.RemoveDecCargoSplitCon(tab.EntityPM);
                //this.Tabs.splice(index, 1)
                if (this.Tabs[index].ComponentReference != null) {
                    this.Tabs[index].ComponentReference.RefreshTabs(direction);
                }
            }
        }
        //this.BuildTabs;
        //this.RefreshEntity();

    }

    OnSelectedChanged(tab: LogTab) {
        if (!AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;

        }
    }

    CustomFileNoTextChanged(searchtext) {

        var errorMessage = "";
        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.IsCustomsFileRetrieved = false;
            this.EntityPM.DeclarationId = null;
            this._LastFetchDeclarationList = null;
            this._LastFetchConsignmentPMList = null;
            this.NoConnectedConsignmentEnableField();
        }
        else {
            this.IsCustomsFileRetrieved = true;
            this.CurrentSession.StartBusyIndicator("")
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe((myDeclarationResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();
                    if (myDeclarationResponse.Result == null || (myDeclarationResponse.Result != null && AppTool.IsNullOrEmpty(myDeclarationResponse.Result.Id))) {
                        this.CustomFileNo = "";
                        this.EntityPM.DeclarationId = null;
                        errorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
                        //this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(errorMessage);
                        this.MessageCustomsFileWindow(errorMessage);

                        //return;
                    }

                    this._LastFetchDeclarationList = myDeclarationResponse.Result;
                    if (AppTool.IsNullOrEmpty(this._LastFetchDeclarationList)) {
                        this.NoConnectedConsignmentEnableField();
                    } else {
                        this.CurrentSession.StartBusyIndicator("")
                        this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
                            .subscribe((myResponse: ServiceResponse) => {
                                this.CurrentSession.StopBusyIndicator();
                                if (this._LastFetchDeclarationList.Direction == "E") {
                                    this.IsExportDeclaration = true;
                                    this.AddItem();
                                } else {
                                    this.IsExportDeclaration = false;
                                }
                                this.SwitchTabsForExportORImport(this._LastFetchDeclarationList.Direction);

                                if (searchtext == "ImporterOnly") {
                                    this._LastFetchConsignmentPMList = myResponse.Result
                                    if (this._LastFetchConsignmentPMList != null && this._LastFetchDeclarationList != null) {
                                        this.ImporterCode = this._LastFetchDeclarationList.ImporterCode;
                                        if (this.SelectedTab != null) {
                                            this.SelectedTab.EntityPM.ImporterCode = this.ImporterCode;
                                            if (this.SelectedTab.ComponentReference != null) {
                                                this.SelectedTab.ComponentReference.DataContext.ImporterCode = this.ImporterCode;
                                            }
                                        }
                                    }
                                }
                                else {
                                    this.FetchConsignment(myResponse, false);
                                }

                            });
                    }

                });

        }
    }

    NoConnectedConsignmentEnableField() {
        //this.ImporterCode = "";
        //this.CargoTypeCode = "";
        //this.ManifestNumber = "";
        //this.SecondCargoID = "";
        //this.ThirdCargoID = "";
        this.ImporterCode = "";
        if (this._LastFetchDeclarationList == null) this.EntityPM.DeclarationId = null;

    }

    FetchConsignment(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this._LastFetchConsignmentPMList = myResponse.Result
        if (this._LastFetchConsignmentPMList != null && this._LastFetchDeclarationList != null) {
            var pm = this._LastFetchConsignmentPMList[0]
            this.ImporterCode = this._LastFetchDeclarationList.ImporterCode;
            if (this.SelectedTab != null && this.SelectedTab.ComponentReference != null) {
                this.SelectedTab.ComponentReference.DataContext.ImporterCode = this.ImporterCode;
                this.SelectedTab.EntityPM.ImporterCode = this.ImporterCode;
            }
            if (!AppTool.IsNullOrEmpty(this.ImporterCode)) {
                this.Tabs.forEach((tab) => {
                    if (AppTool.IsNullOrEmpty(tab.EntityPM.ImporterCode)) {
                        tab.EntityPM.ImporterCode = this.ImporterCode;
                        tab.ComponentReference.DataContext.ImporterCode = this.ImporterCode;
                    }
                });
            }

            this.UIProperties.SetRequired("ImporterCode", "Customs.DecCargoSplitCon", AppTool.IsNullOrEmpty(this.SelectedTab.EntityPM.ImporterCode));
            this.CargoTypeCode = pm.CargoTypeCode;
            this.ManifestNumber = pm.ManifestNumber;
            this.SecondCargoID = pm.SecondCargoID;
            this.ThirdCargoID = pm.ThirdCargoID;
            if (this._LastFetchDeclarationList != null) this.EntityPM.DeclarationId = this._LastFetchDeclarationList.Id;
        } else {

            this.NoConnectedConsignmentEnableField();
        }
    }

    MessageCustomsFileWindow(message: string) {
        var messageWindow = new MessageWindow();
        messageWindow.Width = 300;
        messageWindow.Height = 150;
        messageWindow.Show(message);
    }

    public get ErrorsList() { return this.ValidationErrorsList; }
    public set ErrorsList(val: string[]) {
        this.ValidationErrorsList = val;
    }

    //public get ActionTypeCode() { return this.EntityPM.ActionTypeCode; }
    get ActionTypeCode() { return this.EntityPM != null ? this.EntityPM.ActionTypeCode : null; }
    set ActionTypeCode(value: string) { this.EntityPM.ActionTypeCode = value; }
    //public get ActionTypeName() { return this.EntityPM.ActionTypeName; }
    get ActionTypeName() { return this.EntityPM != null ? this.EntityPM.ActionTypeName : null; }
    set ActionTypeName(value: string) { this.EntityPM.ActionTypeName = value; }
    //public get CargoTypeCode() { return this.EntityPM.CargoTypeCode; }
    get CargoTypeCode() { return this.EntityPM != null ? this.EntityPM.CargoTypeCode : null; }
    set CargoTypeCode(value: string) {
        this.EntityPM.CargoTypeCode = value;
    }
    //public get CargoTypeName() { return this.EntityPM.CargoTypeName; }
    get CargoTypeName() { return this.EntityPM != null ? this.EntityPM.CargoTypeName : null; }
    set CargoTypeName(value: string) { this.EntityPM.CargoTypeName = value; }
    //public get CustomFileNo() { return this.EntityPM.CustomFileNo; }
    get CustomFileNo() { return this.EntityPM != null ? this.EntityPM.CustomFileNo : null; }
    set CustomFileNo(value: string) { this.EntityPM.CustomFileNo = value; }
    //public get ManifestNumber() { return this.EntityPM.ManifestNumber; }
    get ManifestNumber() { return this.EntityPM != null ? this.EntityPM.ManifestNumber : null; }
    set ManifestNumber(value: string) { this.EntityPM.ManifestNumber = value; }

    //public get RequestDate() { return this.EntityPM.RequestDate; }
    get RequestDate() { return this.EntityPM != null ? this.EntityPM.RequestDate : null; }
    set RequestDate(value: Date) { this.EntityPM.RequestDate = value; }
    //public get RequestNumber() { return this.EntityPM.RequestNumber; }
    get RequestNumber() { return this.EntityPM != null ? this.EntityPM.RequestNumber : null; }
    set RequestNumber(value: string) { this.EntityPM.RequestNumber = value; }
    //public get RequestReason() { return this.EntityPM.RequestReason; }
    get RequestReason() { return this.EntityPM != null ? this.EntityPM.RequestReason : null; }
    set RequestReason(value: string) { this.EntityPM.RequestReason = value; }
    //public get RequestReasonName() { return this.EntityPM.RequestReasonName; }
    get RequestReasonName() { return this.EntityPM != null ? this.EntityPM.RequestReasonName : null; }
    set RequestReasonName(value: string) { this.EntityPM.RequestReasonName = value; }
    //public get ResponseStatusCode() { return this.EntityPM.ResponseStatusCode; }
    get ResponseStatusCode() { return this.EntityPM != null ? this.EntityPM.ResponseStatusCode : null; }
    set ResponseStatusCode(value: string) { this.EntityPM.ResponseStatusCode = value; }
    //public get ResponseStatusName() { return this.EntityPM.ResponseStatusName; }
    get ResponseStatusName() { return this.EntityPM != null ? this.EntityPM.ResponseStatusName : null; }
    set ResponseStatusName(value: string) { this.EntityPM.ResponseStatusName = value; }
    //public get SecondCargoID() { return this.EntityPM.SecondCargoID; }
    get SecondCargoID() { return this.EntityPM != null ? this.EntityPM.SecondCargoID : null; }
    set SecondCargoID(value: string) { this.EntityPM.SecondCargoID = value; }
    //public get ThirdCargoID() { return this.EntityPM.ThirdCargoID; }
    get ThirdCargoID() { return this.EntityPM != null ? this.EntityPM.ThirdCargoID : null; }
    set ThirdCargoID(value: string) { this.EntityPM.ThirdCargoID = value; }

    get RequestRemarks() { return this.EntityPM != null ? this.EntityPM.RequestRemarks : null; }
    set RequestRemarks(value: string) { this.EntityPM.RequestRemarks = value; }
    //get ImporterCode() { return this.SelectedTab != null ? this.SelectedTab.EntityPM.ImporterCode : null; }
    //set ImporterCode(value: string) {

    //    this.SelectedTab.EntityPM.ImporterCode = value;

    //this.UIProperties.SetRequired("ImporterCode", "Customs.Client", !AppTool.IsNullOrEmpty(value));

    //}



    //#region Send + Delete
    SendButtonClicked() {
        //this.SaveEntityChanges(null);
        //if (this.ValidationErrorsList.length == 0) return;
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs

        // validate DeclarationCargoSplit
        Validator.TryValidateObject(this.EntityPM, "Customs.DeclarationCargoSplit", errors);

        if (errors.length == 0) {
            errors = this.SendChecks();
        }

        if (errors.length == 0) {
            //           this.declarationCargoSplitPMService.update(this.EntityPM).subscribe(response => {
            //this.entityPMService.update(this.ObjectTableName, this.EntityPM).subscribe(response => {
            this.CurrentSession.StartBusyIndicator("");
            this.OnMassageDisplayMethod();
            var LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0].Id;
            this.requestParams = new CargoSplitRequestParams();
            this.requestParams.LoggingEnabled = true;
            this.requestParams.LoggingUserId = SessionLocator.LoggedUserId;
            this.requestParams.AppicationId = this.EntityPM.Id;
            this.requestParams.Tenant = this.EntityPM.Tenant;
            this.requestParams.RequestName = "Send Cargo Split Request";
            this.requestParams.ResponseName = "Send Cargo Split Response";
            this.requestParams.LoggingEntityId = this.EntityPM.Id;
            this.requestParams.RequestVIA = this.RequestVIA;
            this.requestParams.DeclarationCargoSplit = this.EntityPM.Id;


            LoggingObjectTableId = LoggingObjectTableId;

            CustomMessageProgressComponent
                .ShowProgressBar(this.CurrentSession,this.requestParams.PBId,
                    "שליחת בקשה לפיצול מטען", false)
                .then((res) => {
                    this.responseData = res;
                    this.OnMassageDisplayMethod();
                }
                ).catch((err) => {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(err);
                });


        this.declarationMessagesService.PostSendCargoSplit(this.requestParams)
          .subscribe((response: ServiceResponse) => {
            if (response) {
              if (!response.HasError) {
                if (response.Result.Succeeded) {
                  this.declarationCargoSplitPMService.get(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
                    if (response) {
                      if (!response.HasError) {
                          if (response.Result instanceof DeclarationCargoSplitPM)this.EntityPM = response.Result;
                        this.BuildTabs();
                      }
                    }

                                });

                            }
                        }
                    }
                });

            //           });

        }

        else {
            this.ValidationErrorsList = errors;
            this.FillValidationErrorList.emit(errors);
        }

    }

    OnMassageDisplayMethod() {
        if (this.requestParams == null) {
            this.requestParams = new CargoSplitRequestParams();
        }
        if (this.responseData == null) {
            this.responseData = new INF_MSG_GenericResponseData();
        }
    }

    OnSendCompleted() {
        if (this.IsDelete) {
            this.ApplyDeleteDeclarationCargoSplit();
        }
    }

    ApplyDeleteDeclarationCargoSplit() {
        this.IsDelete = false;

        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        var responseStatusCode = this.ResponseStatusCode;
        //this.ResponseStatusCode = null;
        //this.SaveEntityChanges(customSendOptionsArgs);
        //if (this.ValidationErrorsList.length != 0) {
        //   this.ResponseStatusCode = responseStatusCode;
        //}
        //else {
        //this.SendButtonClicked();
        this.SaveEntityChanges(customSendOptionsArgs);
        //}

    }

    OkButtonClicked() {
        //this.CurrentSession.CloseCurrentWindowEmit("Ok");
        this.SaveEntityChanges(null);
        return;
        //if (!this.IsDisplayOnly) {
        //    if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //        this.declarationCargoSplitPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
        //            var res = response.Result;
        //            if (response.HasError) {
        //                this.ValidationErrorsList = [];
        //                this.ValidationErrorsList = response.ErrorsArray;
        //            } else {
        //                this.CurrentSession.CloseCurrentWindow();
        //            }
        //        });
        //        this.RefreshEntity();
        //    }
        //    else {
        //        this.declarationCargoSplitPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
        //            var res = response.Result;
        //            if (response.HasError) {
        //                this.ValidationErrorsList = [];
        //                this.ValidationErrorsList = response.ErrorsArray;
        //            } else {
        //                this.CurrentSession.CloseCurrentWindow();
        //            }
        //        });
        //    }
        //}
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    RefreshEntity() {
        //if ()this.EntityPM
        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
        if (this.EntityPM.DeclarationId != null) {
            this.declarationCargoSplitController.CheckRequestsInProgress(this.EntityPM.DeclarationId).subscribe((response: ServiceResponse) => {
                if (response.Result != null) {
                    if (response.Result.IsDisplayOnly) {
                        this.SendButtonEnabled = false;
                    }
                }
            });
        }
        if (!AppTool.IsNullOrEmpty(this.EntityPM) && this.EntityPM.DecCargoSplitCargoIdentifiers != null && this.EntityPM.DecCargoSplitCargoIdentifiers.length > 0) {
            this.CargoIdentifiersList.InsertCollection(this.EntityPM.DecCargoSplitCargoIdentifiers);
        }
    }

    InitCargoIdentifiers() {

        if (!AppTool.IsNullOrEmpty(this.EntityPM) && this.EntityPM.DecCargoSplitCargoIdentifiers != null && this.EntityPM.DecCargoSplitCargoIdentifiers.length > 0) {
            this.CargoIdentifiersList.InsertCollection(this.EntityPM.DecCargoSplitCargoIdentifiers);
        }
    }

    private SaveEntityChanges(customSendOptionsArgs) {
        this.EntityPM.Tenant = SessionLocator.Tenant;
        var errors = [];
        this.FillValidationErrorList.emit(errors);
        this.ValidationErrorsList = [];

        Validator.TryValidateObject(this.EntityPM, "Customs.DeclarationCargoSplit", errors);

        if (this.EntityPM.DecCargoSplitCons == null || this.EntityPM.DecCargoSplitCons.length < 1) {
            errors.push(TextCodeTranslator.Translate("חובה להזין נתונים לפחות ליבוםן םחד"));
        } else {
            this.Tabs.forEach((consignment) => {
                Validator.TryValidateObject(consignment.EntityPM, "Customs.DecCargoSplitCon", errors);
                errors.forEach((item, index) => {
                    if (item.includes("DeclarationCargoSplitId")) errors.splice(index, 1);
                });
                if (consignment.EntityPM.DecCargoSplitConsItems != null && consignment.EntityPM.DecCargoSplitConsItems.length > 0) {
                    consignment.EntityPM.DecCargoSplitConsItems.forEach((item) => {
                        Validator.TryValidateObject(item, "Customs.DecCargoSplitConsItem", errors);
                        errors.forEach((item, index) => {
                            if (item.includes("DeclarationCargoSplitId")) errors.splice(index, 1);
                        });
                    });
                }
            });
            //this.EntityPM.DecCargoSplitCons.forEach((consignment) => {
            //    Validator.TryValidateObject(consignment, "Customs.DecCargoSplitCon", errors);
            //    if (consignment.DecCargoSplitConsItems != null && consignment.DecCargoSplitConsItems.length > 0) {

            //        consignment.DecCargoSplitConsItems.forEach((item) => {
            //            Validator.TryValidateObject(item, "Customs.DecCargoSplitConsItem", errors);
            //        });
            //    }
            //});
        }

        errors.forEach((item, index) => {
            if (item.includes("DeclarationCargoSplitId")) errors.splice(index, 1);
        });
        if (this.IsExportDeclaration) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.DecCargoSplitCargoIdentifiers[0].CargoTypeCode)) {
                errors.push("מזהה מטען מפוצל- חובה להזין סוג מזהה מטען");
            }
            if (AppTool.IsNullOrEmpty(this.EntityPM.DecCargoSplitCargoIdentifiers[0].CargoIdentifierKey1)) {
                errors.push("מזהה מטען מפוצל- חובה להזין מזהה מטען רםשון");
            }
            errors.push.apply(errors, this.decCargoSplitCargoIdentifierModel.CheckRequired());

        }

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            this.FillValidationErrorList.emit(errors);
        } else {
            if (this.ScreenChecks().length != 0) {
                this.ValidationErrorsList = this.ScreenChecks();
                this.FillValidationErrorList.emit(errors);
            }
        }
        //this.CancelButtonClicked();
        //return;
        /*
        if (this.EntityPM.DecCargoSplitCons == null || this.EntityPM.DecCargoSplitCons.length < 1) {
            this.ValidationErrorsList.push("חובה להזין נתונים לפחות ליבוםן םחד");
        }
        for (let consignment of this.EntityPM.DecCargoSplitCons) {
            if (AppTool.IsNullOrEmpty(consignment.ImporterCode)) {
                this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ImporterCode")));
                break;
            }
            else {

                if (consignment.ProcedureCurrentCode == null) {
                    this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ProcedureCurrentCode")));
                    break;
                }
            }
        }
        if (this.ScreenChecks().length != 0) {
            this.ValidationErrorsList = this.ScreenChecks();
        }
        */
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            //this.declarationCargoSplitPMService.update(this.EntityPM).then((res: any) => {
            //    res.subscribe((myResponse: ServiceResponse) => {
            this.declarationCargoSplitPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }

                    else {
                        if (myResponse.Result instanceof DeclarationCargoSplitPM)this.EntityPM = myResponse.Result;
                        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

                        var myErrors: string[] = [];
                        myErrors.push("this.EntityPM.Id is null");
                        this.ValidationErrorsList = myErrors;
                    }
                    //else if (this.ScreenChecks().length != 0){
                    //    this.ValidationErrorsList = this.ScreenChecks();
                    //}
                    else {
                        if (customSendOptionsArgs == null) {
                            this.CancelButtonClicked();
                        } else {
                            /*
                            CustomMessageProgressComponent
                                .ShowProgressBar(this.CurrentSession,"",
                                " ", true)
                                .then((res) => {
                                    console.log(res);
                                    //this.CancelButtonClicked();
                                }
                                ).catch((err) => {
                                    this.ValidationErrorsList.push(err);
                                    this.CancelButtonClicked();
                                });

                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService();
                            */
                            this.SendButtonClicked();
                        }

                    }
                }
                /*
            }, error => {
                this.CurrentSession.StopBusyIndicator();
                var myErrors: string[] = [];
                myErrors.push(error.message);
                this.ValidationErrorsList = myErrors;
                //this.SaveCompleted.emit(false);
            
            });
               */
            });
            //this.RefreshEntity();
        }
        else {
            //this.declarationCargoSplitPMService.insert(this.EntityPM).then((res: any) => {
            //res.subscribe((myResponse: ServiceResponse) => {
            this.declarationCargoSplitPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }

                else {
                    if (myResponse.Result instanceof DeclarationCargoSplitPM)this.EntityPM = myResponse.Result;
                    if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

                        var myErrors: string[] = [];
                        myErrors.push("this.EntityPM.Id is null");
                        this.ValidationErrorsList = myErrors;
                    }
                    //else if (this.ScreenChecks().length != 0) {
                    //    this.ValidationErrorsList = this.ScreenChecks();
                    //}
                    else {
                        if (customSendOptionsArgs == null) {
                            this.CancelButtonClicked();
                        } else {
                            /*
                            CustomMessageProgressComponent
                                .ShowProgressBar(this.CurrentSession,"",
                                " ", true)
                                .then((res) => {
                                    console.log(res);
                                    this.CancelButtonClicked();
                                }
                                ).catch((err) => {
                                    this.ValidationErrorsList.push(err);
                                    this.CancelButtonClicked();
                                });

                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService();
                            */
                            //myIIGGeneralMessagesService.PostDeclarationCargoSplitRequest("")
                            //                                        .subscribe((myServiceResponse: ServiceResponse) => {
                            //                                      });
                            this.SendButtonClicked();
                        }

                    }
                }
                /*
            }, error => {
                    this.CurrentSession.StopBusyIndicator();
                    var myErrors: string[] = [];
                    myErrors.push(error.message);
                    this.ValidationErrorsList = myErrors;
                    //this.SaveCompleted.emit(false);
                });
                */
            });
            //}
            //this.RefreshEntity();
        }

    }

    ScreenChecks() {
        var errors: string[] = [];
        if ((this.ActionTypeCode == "3" || this.ActionTypeCode == "4") && AppTool.IsNullOrEmpty(this.RequestNumber)) {
            errors.push("מס' בקשת פיצול חסר");
        }

        if (this.Tabs == null || this.Tabs.length < 1) {
            errors.push("חובה להזין נתונים לפחות ליבוםן םחד");
        }
        for (let tab of this.Tabs) {
            if (AppTool.IsNullOrEmpty(tab.EntityPM.ImporterCode)) {
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ImporterCode")));
                break;
            }
            else {

                if (tab.EntityPM.ProcedureCurrentCode == null) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ProcedureCurrentCode")));
                    break;
                }
            }
            for (let item of tab.EntityPM.DecCargoSplitConsItems) {
                if (AppTool.IsNullOrEmpty(item.ParentCargoConsinmentItem)) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.ParentCargoConsinmentItem")));
                    break;
                }
                else {

                    if (AppTool.IsNullOrEmpty(item.CargoDescription)) {
                        errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.CargoDescription")));
                        break;
                    }
                    else {

                        if (AppTool.IsNullOrEmpty(item.RequestReasonCode)) {
                            errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.RequestReasonCode")));
                            break;
                        }
                    }
                }
                for (let pack of item.DecCargoSplitConsPackDets) {

                }
            }
        }
        /*
        if (this.EntityPM.DecCargoSplitCons == null || this.EntityPM.DecCargoSplitCons.length < 1) {
            errors.push("חובה להזין נתונים לפחות ליבוםן םחד");
        }
        for (let consignment of this.EntityPM.DecCargoSplitCons) {
            if (AppTool.IsNullOrEmpty(consignment.ImporterCode)) {
                errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ImporterCode")));
                break;
            }
            else {

                if (consignment.ProcedureCurrentCode == null) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitCon.F.ProcedureCurrentCode")));
                    break;
                }
            }
            for (let item of consignment.DecCargoSplitConsItems) {
                if (AppTool.IsNullOrEmpty(item.ParentCargoConsinmentItem)) {
                    errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.ParentCargoConsinmentItem")));
                    break;
                }
                else {

                    if (AppTool.IsNullOrEmpty(item.CargoDescription)) {
                        errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.CargoDescription")));
                        break;
                    }
                    else {

                        if (AppTool.IsNullOrEmpty(item.RequestReasonCode)) {
                            errors.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.DecCargoSplitConsItem.F.RequestReasonCode")));
                            break;
                        }
                    }
                }
                if (item.DecCargoSplitConsPackDets == null || item.DecCargoSplitConsPackDets.length < 1) {
                    errors.push("קיימות םריזות ללם פירוט");
                    break;
                }
                for (let pack of item.DecCargoSplitConsPackDets) {

                }
            }
        }
        */
        return errors;
    }

    SendChecks() {
        var errors: string[] = [];

        for (let tab of this.Tabs) {
            if (tab.EntityPM.DecCargoSplitConsItems == null || tab.EntityPM.DecCargoSplitConsItems.length < 1) {
                errors.push("חובה להזין נתוני םריזות");
            }
            for (let item of tab.EntityPM.DecCargoSplitConsItems) {
                if (item.DecCargoSplitConsPackDets == null || item.DecCargoSplitConsPackDets.length < 1) {
                    errors.push("קיימות םריזות ללם פירוט");
                    break;
                }
                for (let pack of item.DecCargoSplitConsPackDets) {

                }
            }
        }


        //for (let consignment of this.EntityPM.DecCargoSplitCons) {
        //    if (consignment.DecCargoSplitConsItems == null || consignment.DecCargoSplitConsItems.length < 1) {
        //        errors.push("חובה להזין נתוני םריזות");
        //    }
        //    for (let item of consignment.DecCargoSplitConsItems) {
        //        if (item.DecCargoSplitConsPackDets == null || item.DecCargoSplitConsPackDets.length < 1) {
        //            errors.push("קיימות םריזות ללם פירוט");
        //            break;
        //        }
        //        for (let pack of item.DecCargoSplitConsPackDets) {

        //        }
        //    }
        //}
        return errors;
    }
}

export class XRayAvailableItem {

}

export class DecCargoSplitCargoIdentifierModel extends BaseComponent
     {
    public EntityPM: DecCargoSplitCargoIdentifierPM;

    constructor(line: DecCargoSplitCargoIdentifierPM) {
        super();
        this.EntityPM = line;
        if (this.CargoTypeCode != null) {
            this.ChangeCargoIdentifireType();
        }
    }

    setRequired() {
       
        this.UIProperties.SetRequired("CargoIdentifierKey1", "Customs.DecCargoSplitCargoIdentifier", true);
        if (this.CargoIdentifierKey1 != null) {
            this.UIProperties.SetRequired("CargoIdentifierKey1", "Customs.DecCargoSplitCargoIdentifier", false);
            }
        if (this.cargoIdentifireType != null) {
            if (this.cargoIdentifireType.IsKey2Mandatory) {
                this.UIProperties.SetRequired("CargoIdentifierKey2", "Customs.DecCargoSplitCargoIdentifier", true);
                if (this.CargoIdentifierKey2 != null) {
                    this.UIProperties.SetRequired("CargoIdentifierKey2", "Customs.DecCargoSplitCargoIdentifier", false);
                }
            } else {
                this.UIProperties.SetRequired("CargoIdentifierKey2", "Customs.DecCargoSplitCargoIdentifier", false);
            }
            if (this.cargoIdentifireType.IsKey3Mandatory) {
                this.UIProperties.SetRequired("CargoIdentifierKey3", "Customs.DecCargoSplitCargoIdentifier", true);
                if (this.CargoIdentifierKey3 != null) {
                    this.UIProperties.SetRequired("CargoIdentifierKey3", "Customs.DecCargoSplitCargoIdentifier", false);
                }
            } else {
                this.UIProperties.SetRequired("CargoIdentifierKey3", "Customs.DecCargoSplitCargoIdentifier", false);
            }
        }
            }

    CheckRequired() {
        var errors = [];
        if (this.cargoIdentifireType != null) {
            if (this.cargoIdentifireType.IsKey2Mandatory) {
                if (this.CargoIdentifierKey2 == null) {
                    errors.push("מזהה מטען מפוצל- חובה להזין מזהה מטען שני");
                }
            }
            if (this.cargoIdentifireType.IsKey3Mandatory) {
                if (this.CargoIdentifierKey3 == null) {
                    errors.push("מזהה מטען מפוצל- חובה להזין מזהה מטען שלישי");
                }
            }
        }
        return errors;
    }

    public SecondCargoIDPlaceholder: string = " ";
    public ManifestNumberPlaceholder: string = " ";
    public ThirdCargoIdPlaceholder: string = " ";

    ChangeCargoIdentifireType() {
        var service = new CargoIdentifireTypeListService();
        service.getSingleFromCache(this.CargoTypeCode).subscribe((response: any) => {
            if (response != null) {
                this.ManifestNumberPlaceholder = response.Result.CargoIdentifierKey1Name;
                this.SecondCargoIDPlaceholder = response.Result.CargoIdentifierKey2Name ?? '';
                this.ThirdCargoIdPlaceholder = response.Result.CargoIdentifierKey3Name ?? '';
                this.CargoIdentifireType = response.Result;
                this.setRequired();
            }
        });
    }

    //#region Properties
    public get CargoTypeCode() { return this.EntityPM.CargoTypeCode; }
    public set CargoTypeCode(newValue: string) { this.EntityPM.CargoTypeCode = newValue; }
     
    public get CargoIdentifierKey1() { return this.EntityPM.CargoIdentifierKey1; }
    public set CargoIdentifierKey1(newValue: string)
    {
        this.EntityPM.CargoIdentifierKey1 = newValue;
        this.setRequired();
    }

    public get CargoIdentifierKey2() { return this.EntityPM.CargoIdentifierKey2; }
    public set CargoIdentifierKey2(newValue: string) {
        this.EntityPM.CargoIdentifierKey2 = newValue;
        this.setRequired();
    }

    public get CargoIdentifierKey3() { return this.EntityPM.CargoIdentifierKey3; }
    public set CargoIdentifierKey3(newValue: string) {
        this.EntityPM.CargoIdentifierKey3 = newValue;
        this.setRequired();
    }

    cargoIdentifireType: CargoIdentifireTypePM;
    public get CargoIdentifireType() { return this.cargoIdentifireType; }
    public set CargoIdentifireType(newValue: CargoIdentifireTypePM) {
        this.cargoIdentifireType = newValue;
        if (newValue != null) {
            this.CargoIdentifireTypeName = newValue.LocalName;
        }
    }

    cargoIdentifireTypeName: string;
    public get CargoIdentifireTypeName() { return this.cargoIdentifireTypeName; }
    public set CargoIdentifireTypeName(newValue: string) { this.cargoIdentifireTypeName = newValue; }
}

class TabItem { constructor(public code: string, public textCode: string) { } }
