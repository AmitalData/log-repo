import { ApiQueryFiltersAddParams } from './../../DataContracts/ApiQueryFiltersAddParams';
declare var window: any;
declare var System: any;
import { Directive, ElementRef, Input, Output, Component, OnInit, OnChanges, Injector, EventEmitter, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from './BaseComponent';
import { EntityListService } from '../../Services/EntityListService';
import { ServiceArgs } from '../../DataContracts/ServiceArgs';
import { ApiQueryFilters, FilterItem } from '../../DataContracts/ApiQueryFilters';
import { ControlsIdCounter } from '../../Utilities/ControlsIdCounter';
import { EntityResourceService } from '../../Services/EntityResourceService';
import { ObjectFieldPM } from '../../EntityPMs/ObjectFieldPM';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { AppTool } from '../../Tools';
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';
import { FocusMeDirective } from '../../Utilities/FocusMeDirective';
import { FixedPositionDirective } from '../../Utilities/FixedPositionDirective';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { FieldValidator } from '../../Validators/FieldValidator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CustomEntityArgs } from './LogSearchWindowComponent';
import { UIProperty, UIProperties, UIPropertyArgs } from './UIProperties';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { FeatureLocator } from '../../Utilities/FeatureLocator';
import { InfraSettings } from '../../Utilities/InfraSettings';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { NewEntityArgs } from '../../Args';
import { EntityPMService } from '../../Services/EntityPMService';
import { ImportEntityArgs } from '../../../Common/Components/Maintenance/TenantImportComponent';
import { CachedDataManager } from '../../Utilities/CachedDataManager';
declare var logLoveReturnWhich, Selection: any;
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';
import { PartnerTypeList } from '../../../Common/EntityLists/PartnerTypeList';
import { ObjectsLocator } from '../../Locators/ObjectsLocator';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
    selector: 'LogLov',
    
    templateUrl: './LogLovV2Component.html',
    providers: [EntityListService, ServiceArgs, EntityResourceService],
    inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', 'LookUpTableName', 'DisplayMemberPath', 'SelectedValuePath',
        'PlaceHolder', 'DependencyFilter1Value', 'DependencyFilter2Value', 'DependencyFilter3Value', "HideColumns", "HideLastColumn", "DependencyFilter1IsList",
        "DependencyFilter2IsList", "DependencyFilter3IsList", "DependencyFilter1IsListExact", "DependencyFilter2IsListExact", "DependencyFilter3IsListExact", "AutoFocus", "IsTenantZeroSearch", "ShowInActive", "FocusOnMe", "IsFreeText", "AlwaysEnabled", "IgnoreCustomFieldCheck", "IsDecendingSort"],
})

export class LogLovV2Component implements OnInit, AfterViewInit, OnDestroy {

    private forceFocus: any;
    @Input()
    public get ForceFocus() {
        return this.forceFocus;
    }
    public set ForceFocus(newValue: any) {
        if (newValue) {
            var element = document.getElementById(this.ElementId);
            if (element) {
                element.focus();
            }
        }
        this.forceFocus = false;
    }
    CopyValueSubs: any;
    public ForceShowValidation: boolean = false;
    public ShowHelp: boolean = false;
    public ObjectField: ObjectFieldPM;
    public ObjectFieldName: string = null;
    public ObjectFieldHelp: string = null;
    public ObjectTableName: string = null;
    public LookUpTableName: string = null;
    public DependencyFilter1Value: Object;
    public DependencyFilter2Value: Object;
    public DependencyFilter3Value: Object;
    public DependencyFilter1IsList: boolean;
    public DependencyFilter2IsList: boolean;
    public DependencyFilter3IsList: boolean;
    public DependencyFilter1IsListExact: any;
    public DependencyFilter2IsListExact: any;
    public DependencyFilter3IsListExact: any;
    public AutoFocus: boolean;
    public PlaceHolder: string;
    public HideColumns: boolean = false;
    public HideLastColumn: boolean = false;
    public SelectedValuePath: string;
    public DisplayMemberPath: string;
    public DisplayMemberPathManuallySet: boolean = false;
    public DataContext: any;
    public DataList: any[];
    public IsFreeText: boolean = false;
    public AlwaysEnabled: boolean = false;
    public IgnoreCustomFieldCheck: boolean = false;
    LayoutDirection: string = 'ltr';
    private dataContext: BaseComponent;
    uiProperty: UIProperty;
    private show: boolean;
    private LookUp1: string;
    private LookUp2: string;
    ElementId: string;
    IsDropDownVisible: boolean;
    ctrl: FormControl;
    SearchTextValue: FormControl;
    private _serviceType: any;
    @Input() LogitudeForm: FormGroup;
    public selectedV: any;
    isFirstTime: boolean = true;
    imgNgStyle: any;
    private isDisabled: boolean;
    public UseCompactSearch: boolean;
    public IsDecendingSort: boolean = false;

    public get IsVisible() {
        if (!this.uiProperty) {
            this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        }
        return this.uiProperty.IsVisible;
    }
    //public set IsVisibile(newValue: boolean) {
    //  this.isVisibile = newValue;

    //}
    public get IsDisabled() {
        return this.isDisabled;
    }
    public set IsDisabled(newValue: boolean) {
        this.isDisabled = newValue;
        if (this.isDisabled) {
            this.imgNgStyle = { 'opacity': .5, 'pointer-events': 'none' };
            this.InputDivStyle = { 'opacity': .5, /*'pointer-events': 'none'*/ };
        }
        else {
            this.imgNgStyle = { 'opacity': 1, 'pointer-events': 'all' };
            if (this.uiProperty != null) {
                if (this.uiProperty.ValidValue) {
                    this.InputDivStyle = { 'opacity': 1, /*'pointer-events': 'all' */ };
                }
                else {
                    if (this.searchTextChanged) {
                        this.InputDivStyle = { 'opacity': 1, /*'pointer-events': 'all',*/ 'border': '1px solid #ff0000' };
                    }
                    else {
                        this.InputDivStyle = { 'opacity': 1 };
                    }
                }
            }
        }
    }
    DropStyle: any;
    public ContainerClass: string;
    public LiClass: string;
    public IsOpen: boolean;
    public SelectedItem: any;
    @Input() SelectedItemObject: any;

    public ItemsSource: any[];
    public ItemsSourceCount: number = -1;
    public ItemsSourceStatic: any[];
    ClosedByBlur: boolean;
    DisplayValue: string;
    MouseInArea: boolean;
    headerColumns: any[];
    private dataColumns: any[];
    DivLogLovId: string;
    ToolTipId: string;
    LogLOVControlClass: string;
    DisplayHeader: boolean;
    SearchTextNgModel: string;
    counterId: number;
    public IsReady: boolean = false
    @Output() ValueChanged = new EventEmitter();
    IsTenantZeroSearch: boolean;
    InputDivStyle: any;
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();
    private selectedValue: any;
    @Input()
    public get SelectedValue() {
        return this.selectedValue;
    }
    public set SelectedValue(newValue: any) {
        if (this.selectedValue != newValue) {
            this.selectedValue = newValue;


            if (!this.isSelectedFromList || this.IsFreeText) {//&& !this.isFirstTime) {

                if (this.uiProperty != null) {
                    //this.uiProperty.UIPropertyChanged.emit("valuechanges"); // caused "a changed was made after it was checked in generatedComponent"
                }
                this.ValueChanged.emit(this.selectedValue);
                var lookup = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
                if (!newValue) {
                    this.ForceShowValidation = true;
                    this.deleteSearchText = true;
                }
                else {
                    this.ForceShowValidation = false;
                }
                this.GetSingle(lookup);
                this.isSelectedFromList = false;
            }

            this.isSelectedFromList = false;
        }
    }
    @Input() RunToggleMode: boolean;
    @Input() AutoCompleteSearchWindow: boolean;
    @Input() ForceShowAddLink: boolean;
    showToggleButton: boolean;
    showPopup: boolean = false;
    ObjectTable: ObjectTablePM;
    LookUpTable: ObjectTablePM;
    private PartnerTypes: Array<PartnerTypeList> = [];
    tabkeyDown: boolean = false;
    FocusOnMe: boolean = false;
    searchTextChanged: boolean = false;
    ErrorMessage: string;
    PropertyChangedSubscribtion: any;
    FocusOnSelect: boolean = true;
    @Input() DisplayFieldsFromList: string;
    public isRTL: boolean = false;
    LovPartnerTypes: Array<PartnerTypeList> = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityListService: EntityListService, private entityPMService: EntityPMService,
        private _entityResourceService: EntityResourceService, private CD: ChangeDetectorRef) {
        this.show = false;
        this.TenantPM = InfraSettings.TenantPM;
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.CurrentSession.SessionEvent.subscribe((res) => {
            if (res == "TariffStepsRefresh") {
                this.OnEditCompleted();
            }
        });
    }

    DropPopUpStyle: any;
    isSelectedFromList: boolean;
    MyDropDownHeight: any;
    IsAllDataVisible: boolean;
    public ZeroItemsSource: any[];
    public ZeroItemsSourceCount: number = -1;
    AllDataHeaderTitle: string;
    MyDataHeaderTitle: string;
    DropdownId: string;
    LovDropDownStyle: any;
    ShowInActive: boolean;
    DeleteButtonNgStyle: any;
    ShowErrorPopup: boolean = false;
    ErrorPopUpId: string;
    ShowSearchButton: boolean;
    MyDataListId: string;
    AllDataListId: string;
    ItemsNgStyles: any[];
    DropDownWidth: number = 300;
    DropDownHeight: number = 257;

    public isLoading: boolean = false;
    public isLoadingZero: boolean = false;
    Widths: number[];
    MinWidths: number[];
    public TenantPM: TenantPM;

    isAddDisabled: boolean = true;
    ShowAddLink: boolean = false;
    isEditDisabled: boolean = true;
    isDeleteDisabled: boolean = true;
    showOnlyDelete: boolean = false;
    isAddVisible: boolean = true;
    isEditVisible: boolean = true;
    @Input() IsPickList: boolean;
    @Input() HideMaintenanceIcon: boolean = false;
    @Input() HideAddLink: boolean = false;
    bufferData: any[];
    callCount: number;
    currentFilter: string;
    @Input() NoObjectField: boolean = false;
    @Input() NoValidation: boolean = false;
    SearchIconId: string;
    ShowMaintenanceBtn: boolean = false;
    @Output() OnBlurEvent: EventEmitter<any> = new EventEmitter();

    @Output() LostFocus: EventEmitter<any> = new EventEmitter();
    public LovMessage: string;
    public AllDataLovMessage: string;
    @Input() HideEdit: boolean = false;
    @Input() HideAdd: boolean = false;
    @Input() QueryFilterItems: ApiQueryFilters;
    OriginalQueryFilterItems: ApiQueryFilters;
    IsCTRLDown: boolean = false;
    OldSearchInput: string;
    AfterViewInitialized: boolean = false;
    @Input() ManipulateData: boolean = false;
    LovToolTip: string = "";
    IsAddTypesVisible: boolean = false;
    IsPartnerMenuVisible: boolean = false;
    PartnersPopupTop: string;
    PartnersPopupLeft: string;
    _KeyDownSubscribe: any;
    @Output() KeyDownEvent: EventEmitter<any> = new EventEmitter();

    ngAfterViewInit() {
        this.RunComponent();
        //this.InitializeAfterViewInit();
    }

    RunComponent() {
        var input = document.getElementById(this.ElementId);

        if (input) {
            this.InitializeAfterViewInit();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerTokenComponent: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerTokenComponent) {
            clearTimeout(this.timerTokenComponent);
        }

        if (this.Retries < 20) {
            this.timerTokenComponent = setTimeout(() => this.RunComponent(), 1);
        }
    }

    InitializeAfterViewInit() {

        var input = document.getElementById(this.ElementId);
        if (input) {
            this.AfterViewInitialized = true;
        }

        else {
            this.AfterViewInitialized = false;
            this.RunComponentTimer();
        }

        if (this.AfterViewInitialized) {
            this._KeyDownSubscribe =
                fromEvent(input, 'keydown').pipe(
                    debounceTime(400))
                    .subscribe(keyboardEvent => {
                        var TABKEY = 9;
                        var ENTERKEY = 13;
                        var DOWNKEY = 40;
                        var UPKEY = 38;
                        var ESC = 27;
                        var END = 35;
                        var HOME = 36;
                        var CTRL = 17;
                        var BACKSPACE = 8;

                        var which = logLoveReturnWhich(keyboardEvent);

                        if (which == TABKEY || which == ENTERKEY || which == DOWNKEY || which == UPKEY
                            || which == ESC || which == END || which == HOME || which == 220 || which == CTRL || this.IsCTRLDown) {
                            return;
                        }




                        if (this.SearchTextNgModel != undefined) {

                            this.OldSearchInput = this.SearchTextNgModel;
                            this.IsDropDownVisible = true;
                            this.IsOpen = true;

                            this.Populate(this.SearchTextNgModel);
                            this.searchTextChanged = true;
                        }
                        if (!this.SearchTextNgModel) {
                            this.OnDeleteValue();
                        }
                    });

            if (this.FocusOnMe) {
                var element = document.getElementById(this.ElementId);
                element.focus();
                this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id, OnBlurEvent: this.OnBlurEvent });
                this.timerToken = setTimeout(() => {
                    Selection(element);
                }, 1);
            }
        }
    }

    ngOnInit() {
         this.LookUpTable = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
        this._entityResourceService.getEntityResourceByTableName(this.LookUpTableName, 0).subscribe((res: any) => {
 
            if (this.LookUpTable.CacheOnClient) {
                var filters: ApiQueryFilters;
                filters = new ApiQueryFilters();
                //filters.PageSize = 50;
                this.entityListService.getAllFromCache(this.LookUpTableName, filters).then((res: any) => {
                    res.subscribe((resp:any) => {

                    });

                });
            }
            else {
 
                var filters: ApiQueryFilters;
                filters = new ApiQueryFilters();
                //filters.PageSize = 50;

                var loadPr = this.entityListService.getByFilters(this.LookUpTableName, filters);
                loadPr.then((res: any) => {
                    res.subscribe((resp:any) => {
                        console.log(resp);
                    });
                });
            }
        });
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        this.uiProperty.UIPropertyChanged.subscribe(value => {

            if (value instanceof UIPropertyArgs) {
                var uiPropertyArgs: UIPropertyArgs = value as UIPropertyArgs;
                var uiProperty: UIProperty = uiPropertyArgs.uiProperty as UIProperty;
                if (uiProperty.FieldName == this.ObjectFieldName && uiProperty.ObjectTableName == this.ObjectTableName) {
                    if (uiPropertyArgs.property == "IsEnabled") {
                        var isEnabled = uiPropertyArgs.newValue;
                        this.IsDisabled = !isEnabled;
                        this.uiProperty.IsEnabled = isEnabled;
                    }
                    else if (uiPropertyArgs.property == "IsRequired") {
                        if (this.searchTextChanged) {

                            this.ValidateField(false);
                        }
                    }
                    else if (uiPropertyArgs.property == "IsVisible") {
                        if (uiPropertyArgs.newValue == true && this.AfterViewInitialized == false) {
                            this.InitializeAfterViewInit();
                        }
                    }
                    else if (uiPropertyArgs.property == "IsValid") {
                        this.ValidateField(false);
                    }
                }
            }

        });
        this.InitializeControl();

    }

    ngOnDestroy() {

        if (this._KeyDownSubscribe) {
            this._KeyDownSubscribe.unsubscribe();
        }
        if (this.PropertyChangedSubscribtion != null && this.PropertyChangedSubscribtion != undefined) {
            this.PropertyChangedSubscribtion.unsubscribe();
        }
        if (this.CopyValueSubs) {
            this.CopyValueSubs.unsubscribe();
            this.CopyValueSubs = null;
        }
        //if (this.uiProperty != null && this.uiProperty != undefined) {
        //    if (this.uiProperty.UIPropertyChanged != null && this.uiProperty.UIPropertyChanged != undefined) {
        //        this.uiProperty.UIPropertyChanged.unsubscribe();
        //        this.uiProperty = null;
        //    }
        //}
    }
    CheckIfExists(IdCom: string) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    }

    SetControlIds(baseIdCombination: string) {

        this.DivLogLovId = 'LogLov_' + baseIdCombination;
        this.ElementId = baseIdCombination;
        this.DropdownId = 'LogLovDropDown-' + baseIdCombination;
        this.ErrorPopUpId = 'loglovererrorpop_' + baseIdCombination;
        this.MyDataListId = 'mydatalist_' + baseIdCombination;
        this.AllDataListId = 'alldatalist_' + baseIdCombination;
        this.SearchIconId = 'searchicon_' + baseIdCombination;
        this.ToolTipId = 'tooltip_' + baseIdCombination;
    }
    SetIsDisabledTimer: any;
    InitializeControl() {
         this.Widths = [];
        this.MinWidths = [];
        this.ItemsNgStyles = [];
        this.LogLOVControlClass = "LogLOVControl";
        this.counterId = null;
        var baseIdCombination = null;
        if (this.ObjectTableName) {
            baseIdCombination = this.ObjectTableName + "_" + this.ObjectFieldName;
        }
        else {
            baseIdCombination = this.ObjectFieldName;
        }
        if (this.CheckIfExists(baseIdCombination)) {
            this.counterId = ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        }

        if (this.counterId != null) {
            baseIdCombination = baseIdCombination + '_' + this.counterId.toString();
        }

        this.SetControlIds(baseIdCombination);

        if (this.FocusOnMe) {// it means it is inside a grid.
            this.CopyValueSubs = this.CurrentSession.CopyCellIntoMemory.subscribe((id) => {
                if (id == this.ElementId) {
                    //this.CurrentSession.CopiedCell = this.DataContext[this.ObjectFieldName];
                    this.DataContext[this.ObjectFieldName] = this.CurrentSession.CopiedCell;
                    this.GetSingle(this.LookUpTable);
                    this.CurrentSession.CopiedCell = null;
                }
            });

            //if (this.CurrentSession.CopiedCell) {
            //    this.DataContext[this.ObjectFieldName] = this.CurrentSession.CopiedCell;
            //    this.CurrentSession.CopiedCell = null;
            //}
        }

        var objectFieldAvailable: boolean = true;
        this.LookUpTable = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
        this.ObjectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
        //(currentTable.AutoCompleteSearchWindow || this.AutoCompleteSearchWindow) && !this.RunToggleMode
        if ((this.LookUpTable.AutoCompleteSearchWindow || this.AutoCompleteSearchWindow) && !this.RunToggleMode) {
            this.ShowSearchButton = true;
            this.showToggleButton = false;
        }
        else {
            this.showToggleButton = true;
            this.ShowSearchButton = false;

        }

        this.LookUp1 = this.LookUpTable.LookUp1;
        this.LookUp2 = this.LookUpTable.LookUp2;
        this.headerColumns = [];
        this.dataColumns = [];
        if (this.LookUpTableName == 'Card' || this.LookUpTableName == 'User' || this.LookUpTableName == 'ChargesType') {
            this.DropDownWidth = 400;
        }
        if (this.LookUpTableName == 'Carrier') {
            this.DropDownWidth = 350;
        }
        if (this.LookUpTableName == 'GLAccount') {
            this.DropDownWidth = 400;
        }

        if (this.LookUpTableName == 'Port' || this.LookUpTableName == 'Carrier' || this.LookUpTableName == 'Card') {
            this.UseCompactSearch = true;
        }
        if (this.LookUpTableName == 'Port' || this.LookUpTableName == 'Carrier') {
            this.IsAllDataVisible = true;
            this.DropDownHeight = 280;
            this.MyDropDownHeight = { 'height': '105px' }
            this.DisplayHeader = false;
            if (this.LookUpTableName == 'Port') {
                this.AllDataHeaderTitle = 'All Ports';
                this.MyDataHeaderTitle = 'My Ports';
            }
            else {
                this.AllDataHeaderTitle = 'All Carriers';
                this.MyDataHeaderTitle = 'My Carriers';
            }
        }
        else {

            this.IsAllDataVisible = false;
        }


        if (!this.PlaceHolder) {
            this.PlaceHolder = '';
        }



        if (!this.SelectedValuePath) {
            this.SelectedValuePath = this.LookUpTable.KeyPropertyPath;
        }
        this.KeyPropertyPath = this.LookUpTable.KeyPropertyPath;
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        //console.log(this.uiProperty);
        if (this.AlwaysEnabled == true) {
            this.IsDisabled = false;
        }
        else {
            this.IsDisabled = !this.uiProperty.IsEnabled;
            //console.log("Out " + this.LookUpTableName+ " " + this.uiProperty.IsEnabled)
        }
        if (this.SetIsDisabledTimer) {
            clearTimeout(this.SetIsDisabledTimer);
        }
        this.SetIsDisabledTimer = setInterval(() => this.SetIsDisabled(), 1);
        this._entityResourceService.getEntityResourceByTableName(this.LookUpTableName, 0).subscribe((res: any) => {
            this._entityResourceService.getEntityResourceByTableName("PartnerType", 0).subscribe((res3: any) => {

                var apiQueryFilter: ApiQueryFilters;
                if (!this.QueryFilterItems) {
                    apiQueryFilter = new ApiQueryFilters();

                }
                else {
                    apiQueryFilter = new ApiQueryFilters();
                    for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter: FilterItem = this.QueryFilterItems.AdditionalFilters[i];
                        apiQueryFilter.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator,
                            filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, this.LookUpTable.CacheOnClient);
                    }


                }
                // apiQueryFilter.GetAll = true; by mohammad.
                this.entityListService.getAllFromCache("PartnerType", apiQueryFilter).then((res3: any) => {
                    res3.subscribe(res4 => {
                        this.PartnerTypes = res4.Result;
                        var parentName = this.GetObjectTableName(this.LookUpTableName);
                        this._entityResourceService.getEntityResourceByTableName(parentName, 0).subscribe((res5: any) => {

                            if (this.LookUpTableName == "Card" || this.LookUpTableName == "Carrier") {
                                if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {

                                    //this._entityResourceService.getEntityResourceByTableName(parentName, 0).subscribe((otherResp: any) => { });
                                    if (this.DependencyFilter1Value.toString().split(',').length > 1) {
                                        var types: string[] = this.DependencyFilter1Value.toString().split(',');
                                        this.LovPartnerTypes = this.PartnerTypes.filter(d => types.indexOf(d.Id) > -1);
                                    }
                                }
                            }

                            // drow columns
                            var lang = 'E';
                            if (SessionInfo.LoggedUserPM.ShowLocalNameInLOV) {
                                lang = 'L';
                                this.ShowLanguageFilter = true;
                            }
                            this.LanguageFilterValue = lang;
                            this.DrawColumns();
                            if (!this.DisplayMemberPath) {
                                this.SetDisplayMemberPath();
                            }
                            else {
                                this.DisplayMemberPathManuallySet = true;
                            }


                            if (this.ObjectTable) {
                                this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTable.Id && d.FieldName === this.ObjectFieldName)[0];
                                if (!this.ObjectField) {
                                    objectFieldAvailable = false;


                                }
                                else {
                                    objectFieldAvailable = true;
                                    if (this.ObjectField.HelpTextCodeCode != null) {
                                        this.ObjectFieldHelp = TextCodeTranslator.Translate(this.ObjectField.HelpTextCodeCode);

                                        if (!AppTool.IsNullOrEmpty(this.ObjectFieldHelp)) {
                                            if (this.ObjectFieldHelp.length > 1) {
                                                if (!this.IsFreeText) this.ShowHelp = true;

                                            }
                                        }
                                    }

                                    if (this.DependencyFilter1Value == null || this.DependencyFilter1Value === undefined) {
                                        if (this.ObjectField.DependencyFilter1Value) {
                                            if (this.ObjectField.DependencyFilter1Type == "Constant") {
                                                this.DependencyFilter1Value = this.ObjectField.DependencyFilter1Value;
                                                this.DependencyFilter1IsList = this.ObjectField.DependencyFilter1IsList;
                                            }
                                            else {

                                                this.DependencyFilter1Value = this.DataContext[this.ObjectField.DependencyFilter1Value];
                                            }
                                        }
                                        else if (this.ObjectField.ControlField1) {
                                            this.DependencyFilter1Value = this.DataContext[this.ObjectField.ControlField1];
                                        }
                                    }
                                   
                                    if (this.DependencyFilter2Value == null || this.DependencyFilter2Value === undefined) {
                                        if (this.ObjectField.DependencyFilter2Value) {
                                            if (this.ObjectField.DependencyFilter2Type == "Constant") {
                                                this.DependencyFilter2Value = this.ObjectField.DependencyFilter2Value;
                                                this.DependencyFilter2IsList = this.ObjectField.DependencyFilter2IsList;
                                            }
                                            else {
                                                this.DependencyFilter2Value = this.DataContext[this.ObjectField.DependencyFilter2Value];
                                            }
                                        }
                                        else if (this.ObjectField.ControlField2) {
                                            this.DependencyFilter2Value = this.DataContext[this.ObjectField.ControlField2];
                                        }
                                    }

                                    if (this.DependencyFilter3Value == null || this.DependencyFilter3Value === undefined) {
                                        if (this.ObjectField.DependencyFilter3Value) {
                                            if (this.ObjectField.DependencyFilter3Type == "Constant") {
                                                this.DependencyFilter3Value = this.ObjectField.DependencyFilter3Value;
                                                this.DependencyFilter3IsList = this.ObjectField.DependencyFilter3IsList;
                                            }
                                            else {

                                                this.DependencyFilter3Value = this.DataContext[this.ObjectField.DependencyFilter3Value];
                                            }
                                        }
                                        else if (this.ObjectField.ControlField3) {
                                            this.DependencyFilter3Value = this.DataContext[this.ObjectField.ControlField3];
                                        }
                                    }

                                    if (this.ObjectField.ControlField1 || this.ObjectField.ControlField2 || this.ObjectField.ControlField3) {
                                        if (this.DataContext.PropertyChanged != null && this.DataContext.PropertyChanged != undefined) {
                                            this.PropertyChangedSubscribtion = this.DataContext.PropertyChanged.subscribe(args => {
                                                if (args.PropertyName == this.ObjectField.ControlField1) {
                                                    this.DependencyFilter1Value = this.DataContext[this.ObjectField.ControlField1];
                                                    this.OnDeleteValue();
                                                }

                                                if (args.PropertyName == this.ObjectField.ControlField2) {
                                                    this.DependencyFilter2Value = this.DataContext[this.ObjectField.ControlField2];
                                                    this.OnDeleteValue();
                                                }

                                                if (args.PropertyName == this.ObjectField.ControlField3) {
                                                    this.DependencyFilter3Value = this.DataContext[this.ObjectField.ControlField3];
                                                    this.OnDeleteValue();
                                                }
                                            });
                                        }
                                    }
                                }

                            }

                            var value = this.DataContext[this.ObjectFieldName];
                            if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                                var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    value = customFieldClass.Value;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                }

                            }

                            if (value) {

                                this.GetSingle(this.LookUpTable);

                            }


                            // this.uiProperty.UIPropertyChanged.subscribe(value => {

                            //     if (value instanceof UIPropertyArgs) {
                            //         var uiPropertyArgs: UIPropertyArgs = value as UIPropertyArgs;
                            //         var uiProperty: UIProperty = uiPropertyArgs.uiProperty as UIProperty;
                            //         if (uiProperty.FieldName == this.ObjectFieldName && uiProperty.ObjectTableName == this.ObjectTableName) {
                            //             if (uiPropertyArgs.property == "IsEnabled") {
                            //                 var isEnabled = uiPropertyArgs.newValue;
                            //                 this.IsDisabled = !isEnabled;
                            //                 this.uiProperty.IsEnabled = isEnabled;
                            //             }
                            //             else if (uiPropertyArgs.property == "IsRequired") {
                            //                 if (this.searchTextChanged) {

                            //                     this.ValidateField(false);
                            //                 }
                            //             }
                            //             else if (uiPropertyArgs.property == "IsVisible") {
                            //                 if (uiPropertyArgs.newValue == true && this.AfterViewInitialized == false) {
                            //                     this.InitializeAfterViewInit();
                            //                 }
                            //             }
                            //             else if (uiPropertyArgs.property == "IsValid") {
                            //                 this.ValidateField(false);
                            //             }
                            //         }
                            //     }

                            // });
                        });
                    });
                });
            });
        });
        if (this.LookUpTable.IsClosed)
            this.showOnlyDelete = true;
        if (this.LookUpTable.EnableAddFromLOV) {
            this.isAddDisabled = false;

            if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString()) && !SessionLocator.LoggedUserPM.IsCustomerCare && (this.LookUpTableName == "User" || this.LookUpTableName == "Contact")) {
                this.ShowAddLink = false

            }

            else {
                this.ShowAddLink = true;
            }
        }

        if (this.LookUpTable.EnableEditFromLOV)
            this.isEditDisabled = false;

        if (this.isAddDisabled && this.isEditDisabled)
            this.showOnlyDelete = true;

        if (this.LookUpTableName == "Card" || this.LookUpTableName == "Carrier") {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {

                if (this.DependencyFilter1Value.toString().split(',').length > 1) {
                    this.IsAddTypesVisible = true;
                }
            }
        }

        if (this.HideAddLink) {
            this.isAddDisabled = true;
            this.ShowAddLink = false;
        }

        if (!this.LookUpTable.EnableAddFromLOV) {
            if (this.ForceShowAddLink) {
                this.isAddDisabled = false;
                this.ShowAddLink = true;
            }
        }

        //*ngIf="ShowAddLink || ShowSearchButton"
        if (!this.ShowAddLink && !this.ShowSearchButton) {

            this.MyDropDownHeight = { 'height': '255px' }
        }

        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " LOV has no object field!");
        }
    }
    DrawColumns() {

          var lookupFields: any[];
        this.headerColumns = [];
        this.dataColumns = [];
        if (this.DisplayFieldsFromList != null && this.DisplayFieldsFromList != undefined) {
            var fields: string[] = this.DisplayFieldsFromList.split(',');
            var fields: string[] = this.DisplayFieldsFromList.split(',');
            lookupFields = window.ObjectFields.filter(d => d.ObjectTableId == this.LookUpTable.Id && fields.lastIndexOf(d.FieldName) > -1);
        }
        else {
            //if(!SessionInfo.LoggedUserPM.ShowLocalNameInLOV){
            if (this.LanguageFilterValue == 'E') {
                lookupFields = window.ObjectFields.filter(d => d.DisplayOnLookUp && d.ObjectTableId == this.LookUpTable.Id);
            }
            else {
                lookupFields = window.ObjectFields.filter(d => d.DisplayOnLookUpLocal && d.ObjectTableId == this.LookUpTable.Id);
                if (lookupFields.length == 0) {
                    console.warn("There is no Fields defined as display in lookup local");
                    this.ShowLanguageFilter = false;
                    lookupFields = window.ObjectFields.filter(d => d.DisplayOnLookUp && d.ObjectTableId == this.LookUpTable.Id);
                }
            }
        }
        if (!this.DisplayFieldsFromList) {
            lookupFields = lookupFields.sort((a, b) => { return a.DisplayInLookUpIndex - b.DisplayInLookUpIndex });
        }

        for (var i = 0; i < lookupFields.length; i++) {
            //this.Widths.push(this.DropDownWidth / lookupFields.length);
            this.MinWidths.push(TextCodeTranslator.Translate(lookupFields[i].ListTextCodeCode).length * 8 + 6);
        }
        //var dropdownWidth = lookupFields.length * 120 + 20;
        //this.DropPopUpStyle = { width: dropdownWidth.toString() + 'px' };
        var additionalCols = lookupFields.filter(d => d.DisplayInLookUpIndex > 1);
        if (lookupFields.length == 1) {
            this.DisplayHeader = false;
        }
        else if (!this.IsAllDataVisible) {
            this.DisplayHeader = true;
        }
        if (!this.ShowAddLink && !this.ShowSearchButton) {
            if (this.DisplayHeader) {
                this.DropDownHeight = 278;
            }
            else {
                this.DropDownHeight = 260;
            }
        }
        this.IsReady = true;
        for (var i = 0; i < lookupFields.length; i++) {
            var width: number = 100;
            if (lookupFields[i].DisplayInLookupColumnSize) {
                width = lookupFields[i].DisplayInLookupColumnSize;
            }
            else {
                if (lookupFields[i].DisplayInLookUpIndex == 0) {
                    if (!additionalCols || additionalCols.length == 0 || additionalCols.length == 1) {
                        width = 120;
                    }
                    else {
                        width = 70;
                    }
                }
                if (lookupFields[i].DisplayInLookUpIndex == 1) {
                    if (!additionalCols || additionalCols.length == 0 || additionalCols.length == 1) {
                        width = 120;
                    }
                    else {
                        width = 100;
                    }
                }

                if (lookupFields[i].DisplayInLookUpIndex > 1) {
                    if (additionalCols.length == 1) {
                        width = 85;
                    }
                    if (additionalCols.length > 1) {
                        width = (400 - 170) / additionalCols.length;
                    }
                }

            }

            this.headerColumns.push({
                Display: TextCodeTranslator.Translate(lookupFields[i].ListTextCodeCode),
                Width: width,
                Field: lookupFields[i].FieldName,
                Index: lookupFields[i].DisplayInLookUpIndex,
            });
            this.dataColumns.push({ Field: lookupFields[i].FieldName });
        }
    }

    SetDisplayMemberPath() {
        if (!this.DisplayMemberPathManuallySet) {
            /// this case we have to show the local display member path if available
            if (this.LanguageFilterValue != 'E') {
                if (this.LookUpTable.LovDisplayMemberPathLocal) {
                    this.DisplayMemberPath = this.LookUpTable.LovDisplayMemberPathLocal;
                }
                else if (this.LookUpTable.LovDisplayMemberPath) {
                    this.DisplayMemberPath = this.LookUpTable.LovDisplayMemberPath;
                }
                else if (this.LookUpTable.LookUp2) {
                    this.DisplayMemberPath = this.LookUpTable.LookUp2;
                }
                else {
                    this.DisplayMemberPath = this.LookUpTable.LookUp1;
                }
            }
            /// this case we have to show the display member path if available
            else {
                if (this.LookUpTable.LovDisplayMemberPath) {
                    this.DisplayMemberPath = this.LookUpTable.LovDisplayMemberPath;
                }
                else if (this.LookUpTable.LookUp2) {
                    this.DisplayMemberPath = this.LookUpTable.LookUp2;
                }
                else {
                    this.DisplayMemberPath = this.LookUpTable.LookUp1;
                }
            }
        }
    }

    SetIsDisabled() {
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        //console.log(this.uiProperty);
        if (this.AlwaysEnabled == true) {
            this.IsDisabled = false;
        }
        else {
            this.IsDisabled = !this.uiProperty.IsEnabled;
            //console.log("Out " + this.LookUpTableName+ " " + this.uiProperty.IsEnabled)
        }
        if (this.SetIsDisabledTimer) {
            clearTimeout(this.SetIsDisabledTimer);
        }
    }
    GetSingle(lookup: any) {

        var dataContextValue = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                dataContextValue = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }

        }

        if (dataContextValue || this.IsFreeText) {

            var value = dataContextValue;//this.DataContext[this.ObjectFieldName];
            if (this.IsFreeText) value = this.SelectedValue;

            var apiFilters: ApiQueryFilters;
            if (!this.QueryFilterItems) {
                apiFilters = new ApiQueryFilters();

            }
            else {
                apiFilters = new ApiQueryFilters();
                for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                    var filter: FilterItem = this.QueryFilterItems.AdditionalFilters[i];
                    apiFilters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator,
                        filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, this.LookUpTable.CacheOnClient);
                }

            }
            if (lookup.CacheOnClient) {
                this.entityListService.getSingleFromCache(value, this.LookUpTableName, apiFilters).then((res: any) => {
                    res.subscribe(myResponse => {
                        if (myResponse != null) {

                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse) {
                                list = myResponse.Result;
                            }

                            if (list) {
                                this.SearchTextNgModel = list[this.DisplayMemberPath];
                                this.OldSearchInput = this.SearchTextNgModel;
                                this.DisplayValue = list[this.DisplayMemberPath];
                                this.SelectedItem = list;
                                this.SetToolTipInfo();
                                this.SelectedItemObject = this.SelectedItem;
                                this.SelectedItemChanged.emit(this.SelectedItem);
                            }

                        }
                    })
                });
            }
            else {
                this.entityListService.getSingle(value, this.LookUpTableName).then((res: any) => {
                    res.subscribe(myResponse => {
                        if (myResponse != null) {

                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse) {
                                list = myResponse.Result;
                            }
                            if (list) {
                                this.SearchTextNgModel = list[this.DisplayMemberPath];
                                this.OldSearchInput = this.SearchTextNgModel;
                                this.DisplayValue = list[this.DisplayMemberPath];
                                this.SelectedItem = list;
                                this.SetToolTipInfo();
                                this.SelectedItemObject = this.SelectedItem;
                                this.SelectedItemChanged.emit(this.SelectedItem);
                            }
                        }
                    })
                });
            }
            this.ValidateField(true);
        }
        else {
            if (this.deleteSearchText != false) {
                this.SearchTextNgModel = null;
                this.OldSearchInput = this.SearchTextNgModel;
            }
            this.DisplayValue = null;
            this.SelectedItem = null;
            this.SetToolTipInfo();
            this.SelectedItemObject = null;
            this.ValidateField(true);
            this.SelectedItemChanged.emit(this.SelectedItem);
        }
    }

    OnWindowClick($event) {
        if (!this.IsOpen) {
            var shownDropDowns = document.getElementsByClassName("show");
            var input = document.getElementById(this.ElementId);
            var dropdown = input.parentElement.parentElement;
            for (var i = 0; i < shownDropDowns.length; i++) {
                if (shownDropDowns[i]) {
                    shownDropDowns[i].classList.add("hide");
                    shownDropDowns[i].classList.remove("show");
                }
            }
        }

    }

    OnLogLovFocus() {
    }

    OnLogLovKeyDown($event) {
        var TABKEY = 9;
        var SHIFTKEY = 16;
        var DELETEKEY = 46;
        if ($event.keyCode == TABKEY) {

            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }

        }
        else if ($event.keyCode == DELETEKEY) {
            this.OnDeleteValue();
            this.ToggleOpenDropDown();
        }
        else if ($event.keyCode != SHIFTKEY) {
            this.ToggleOpenDropDown();
            if (this.IsOpen) {
                this.Populate(null);
            }
        }




    }

    OnSelected(item: any) {
        this.isSelectedFromList = true;
        this.SelectedItem = item;
        var value = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];;
                this.DataContext[this.ObjectFieldName] = customFieldClass;
                value = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }

        }
        else {
            this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
            value = this.DataContext[this.ObjectFieldName];
        }
        this.SelectedItemObject = this.SelectedItem;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
        this.SearchTextNgModel = this.SelectedItem[this.DisplayMemberPath];
        this.SetToolTipInfo();
        this.OldSearchInput = this.SearchTextNgModel;
        this.ValueChanged.emit(value);
        this.ValidateField();
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
        this.show = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
        if (this.FocusOnSelect) {
            var elem = document.getElementById(this.ElementId);
            elem.focus();
        }
        this.FocusOnSelect = true;
        this.MouseInArea = false;
        //this.OnBlurEvent.emit({ Id : this.ElementId});

    }

    OnLogLovClicked() {
        this.ToggleOpenDropDown();
        if (this.IsOpen) {
            this.Populate(null);
        }

    }

    ToggleOpenDropDown() {
        this.IsDropDownVisible = !this.IsDropDownVisible;
        this.IsOpen = !this.IsOpen;
        if (this.IsOpen) {
            var input = document.getElementById(this.ElementId);
            input.focus();
            this.showPopup = false;
        }
    }

    NavigateListItems(isDown: boolean) {
 
        if (isDown) {
            var isSelected = false;
            var active = document.getElementsByClassName("highlighted");
            if (!active[0]) {
                if (this.ItemsSource && this.ItemsSource.length > 0) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    lis[0].classList.add("highlighted");
                }
                else if (this.IsAllDataVisible) {
                    if (this.ZeroItemsSource && this.ZeroItemsSource.length > 0) {
                        var input = document.getElementById(this.AllDataListId);
                        var lis = input.getElementsByTagName("li");
                        lis[0].classList.add("highlighted");
                    }
                }

            }
            else {
                if (active[0].nextElementSibling) {
                    active[0].nextElementSibling.classList.add("highlighted");
                    active[0].classList.remove("highlighted");
                    active[0].scrollIntoView(false);
                }
                else if (this.IsAllDataVisible) {
                    if (this.ZeroItemsSource && this.ZeroItemsSource.length > 0) {
                        var input = document.getElementById(this.AllDataListId);
                        active[0].classList.remove("highlighted");
                        var lis = input.getElementsByTagName("li");
                        lis[0].classList.add("highlighted");
                    }
                }
            }
        }
        else {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {
                // if (this.ItemsSource && this.ItemsSource.length > 0) {
                if (active[0].previousElementSibling) {
                    active[0].previousElementSibling.classList.add("highlighted");
                    active = document.getElementsByClassName("highlighted");
                    active[1].classList.remove("highlighted");
                    active[0].scrollIntoView(false);
                }
                else if (this.IsAllDataVisible) {
                    if (this.ItemsSource && this.ItemsSource.length > 0) {
                        var input = document.getElementById(this.MyDataListId);
                        active[0].classList.remove("highlighted");
                        var lis = input.getElementsByTagName("li");
                        lis[lis.length - 1].classList.add("highlighted");
                    }
                }
                // }
                // else if(this.IsAllDataVisible){
                //     if (this.ZeroItemsSource && this.ZeroItemsSource.length > 0) {
                //         var input = document.getElementById(this.AllDataListId);
                //         var lis = input.getElementsByTagName("li");
                //         lis[0].classList.add("highlighted");
                //     }
                // }
            }
        }
    }

    OnSearchIputKeyDown($event) {

        var TABKEY = 9;
        var ENTERKEY = 13;
        var DOWNKEY = 40;
        var UPKEY = 38;
        var ESC = 27;
        var CTRL = 17;
        var SHIFT = 16;
        if ($event.keyCode == CTRL) {
            this.IsCTRLDown = true;
        }

        if ($event.keyCode == 220) {
            return false;
        }
        if ($event.keyCode == TABKEY) {
            this.MouseInArea = false;
            this.tabkeyDown = true;

            if (this.IsOpen) {
                this.FocusOnSelect = false;
                this.SelectHighlightedItem();
                this.ToggleOpenDropDown();
            }
        }

        if ($event.keyCode == ENTERKEY) {
            if (this.IsOpen) {
                this.SelectHighlightedItem();
            }
            else {
                this.KeyDownEvent.emit(13)
            }
        }

        if ($event.keyCode == DOWNKEY) {

            if (!this.IsOpen) {
                this.ToggleOpenDropDown();
                if (this.IsOpen) {
                    this.Populate(null);
                }
            }
            else {
                var selected = document.getElementsByClassName("liItemSelected");
                if (selected[0]) {
                    if (this.ItemsSource && this.ItemsSource.length > 0) {
                        this.RemoveSelectedAddHighlighted(this.MyDataListId);
                    }
                    else if (this.ZeroItemsSource && this.ZeroItemsSource.length > 0) {
                        this.RemoveSelectedAddHighlighted(this.AllDataListId);

                    }
                }
                this.NavigateListItems(true);
            }
            return false;
        }

        if ($event.keyCode == UPKEY) {
            this.NavigateListItems(false);
            return false;
        }

        if ($event.keyCode == ESC) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }

        var key = $event.keyCode;
        if (key != 17 && key != 18 && key != 19 && key != 20
            && key != 37 && key != 38 && key != 39 && key != 40
            && key != 35 && key != 45 && key != 33 && key != 34
            && key != 145 && key != 13 && key != 27 && key != 9
            && key != 16 && key != 36 && key != 144
            && !(key >= 112 && key <= 123) && !this.IsCTRLDown) {
            this.OnDeleteValue(false);
        }

        //if ((48 <= key && key <= 57) || (65 <= key && key <= 90) || key == 8 || key == 46 || key == 32) {
        //    this.OnDeleteValue(false);
        //}
    }

    RemoveSelectedAddHighlighted(dropDownId: string) {
        var myDataDropDown = document.getElementById(dropDownId);
        var lis = myDataDropDown.getElementsByTagName("li");
        for (var i = 0; i < lis.length; i++) {

            if (lis[i].className.search("liItemSelected") > -1) {
                lis[i].classList.remove("liItemSelected");
                lis[i].classList.add("highlighted");
            }
        }
    }

    ClickOnHighlightedItemReturnStatus(dropDownListId: string) {
        var input = document.getElementById(dropDownListId);
        var lis = input.getElementsByTagName("li");
        var selectedItemFound: boolean = false;
        for (var i = 0; i < lis.length; i++) {

            if (lis[i].className.search("highlighted") > -1) {
                selectedItemFound = true;
                lis[i].classList.remove("highlighted");
                lis[i].click();
            }
        }
        return selectedItemFound;
    }

    ClickOnSelectedItemReturnStatus(dropDownListId: string) {
        var input = document.getElementById(dropDownListId);
        var lis = input.getElementsByTagName("li");
        var selectedItemFound: boolean = false;
        for (var i = 0; i < lis.length; i++) {

            if (lis[i].className.search("liItemSelected") > -1) {
                selectedItemFound = true;
                lis[i].classList.remove("liItemSelected");
                lis[i].click();
            }
        }
        return selectedItemFound;
    }

    SelectHighlightedItem() {
        var selectedItemFound: boolean = false;
        var active = document.getElementsByClassName("highlighted");
        if (active[0]) {
            selectedItemFound = this.ClickOnHighlightedItemReturnStatus(this.MyDataListId);
            if (!selectedItemFound) {
                selectedItemFound = this.ClickOnHighlightedItemReturnStatus(this.AllDataListId);
            }

        }
        else {
            var selected = document.getElementsByClassName("liItemSelected");
            if (selected[0]) {
                selectedItemFound = this.ClickOnSelectedItemReturnStatus(this.MyDataListId);
            }
            if (!selectedItemFound) {
                selectedItemFound = this.ClickOnSelectedItemReturnStatus(this.AllDataListId);
            }
        }
    }

    OnLiMouseOver($event) {
        var active = document.getElementsByClassName("highlighted");
        if (active[0]) {
            active[0].classList.remove("highlighted");
        }
        if ($event.target.tagName != "DIV") {
            $event.target.classList.add("highlighted");
        }
        else {
            $event.target.parentElement.parentElement.classList.add("highlighted");
        }
    }

    OnLiMouseLeave($event) {
        $event.target.classList.remove("highlighted");
    }
    SelectedItemKey: any;
    SelectedZeroItemKey: any;
    KeyPropertyPath: any;
    HighlightSelectedValue() {
        this.SelectedItemKey = null;
        //this.SelectedZeroItemKey=null;
        if (this.SearchTextNgModel != null && this.SearchTextNgModel != undefined && this.SearchTextNgModel != "") {

            if (this.ItemsSource && this.ItemsSource.length > 0) {
                var oldItems = this.ItemsSource;

                var item = this.ItemsSource.filter(d => d[this.LookUp1] != null && d[this.LookUp1].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                if (!item && this.LookUp2) {
                    var item = this.ItemsSource.filter(d => d[this.LookUp2] != null && d[this.LookUp2].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                }
                if (!item) {
                    var item = this.ItemsSource.filter(d => d[this.DisplayMemberPath] != null && d[this.DisplayMemberPath].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                }
                if (item) {
                    this.ItemsSource = [];
                    this.ItemsSource.push(item);
                    var index = oldItems.indexOf(item);
                    oldItems.splice(index, 1);
                    oldItems.forEach((itm) => {
                        this.ItemsSource.push(itm);
                    });

                    //var index = items.indexOf(item);
                    //var temp = items[0];
                    //items[0] = item;
                    //items[index] = temp;
                    this.SelectedItemKey = item[this.KeyPropertyPath];
                }
                else if (this.ItemsSource.length > 0) {
                    this.SelectedItemKey = this.ItemsSource[0][this.KeyPropertyPath];
                }
            }
            else if (this.ZeroItemsSource && this.ZeroItemsSource.length > 0) {
                if (this.ZeroItemsSource.length > 0) {
                    var oldItems = this.ZeroItemsSource;

                    var item = this.ZeroItemsSource.filter(d => d[this.LookUp1] != null && d[this.LookUp1].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                    if (!item && this.LookUp2) {
                        var item = this.ZeroItemsSource.filter(d => d[this.LookUp2] != null && d[this.LookUp2].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                    }
                    if (!item) {
                        var item = this.ZeroItemsSource.filter(d => d[this.DisplayMemberPath] != null && d[this.DisplayMemberPath].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                    }
                    if (item) {
                        this.ZeroItemsSource = [];
                        this.ZeroItemsSource.push(item);
                        var index = oldItems.indexOf(item);
                        oldItems.splice(index, 1);
                        oldItems.forEach((itm) => {
                            this.ZeroItemsSource.push(itm);
                        });

                        //var index = items.indexOf(item);
                        //var temp = items[0];
                        //items[0] = item;
                        //items[index] = temp;
                        this.SelectedItemKey = item[this.KeyPropertyPath];
                    }
                    else if (this.ZeroItemsSource.length > 0) {
                        this.SelectedItemKey = this.ZeroItemsSource[0][this.KeyPropertyPath];
                    }
                }
            }
        }
    }

    Populate(searchText: string, setFirstAsSelected: boolean = false) {

        this.LovMessage = null;

        if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {

            this.LovMessage = "Your package doesn't include this module..";
            return;
        }
        else if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "READ") && this.LookUpTable.EnableSecurity) {
            this.LovMessage = "You have no permission to view entities of this type.";
            return;
        }


        //reset counters
        this.bufferData = [];
        this.callCount = 0;
        this.ZeroItemsSourceCount = -1;
        this.ItemsSourceCount = -1;

        this.ItemsSource = [];
        this.ItemsSourceStatic = [];

        var filters: ApiQueryFilters;
        if (!this.QueryFilterItems) {
            filters = new ApiQueryFilters();

        }
        else {
            // filters = this.QueryFilterItems;
            filters = new ApiQueryFilters();
            for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                var filter: FilterItem = this.QueryFilterItems.AdditionalFilters[i];

                filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator,
                    filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, this.LookUpTable.CacheOnClient);
            }
        }
        if (this.IsDecendingSort == true) {
            filters.SortDirection = "Decending";
        }
        else {
            filters.SortDirection = "Ascending";

        }

        if (this.IsTenantZeroSearch) {
            filters.Tenant = 0;
        }

        this.SetDependencyProperties(filters);
        filters.PageIndex = 0;

        if (this.LookUpTable.SortingByObjectField) {
            filters.SortBy = this.LookUpTable.SortingByObjectField;
        }
        var parentName = this.GetObjectTableName(this.LookUpTableName);
        var parenttable = window.ObjectTables.filter(d => d.Name === parentName)[0];
        var originalTable = this.LookUpTable;
        if (originalTable.Name == "Carrier") {
            originalTable = window.ObjectTables.filter(d => d.Name === "Card")[0];
        }
        var inactiveField = window.ObjectFields.filter(d => d.FieldName.toLowerCase() === "inactive" && (d.ObjectTableId === parenttable.Id || d.ObjectTableId === originalTable.Id))[0];
        if (inactiveField && !this.ShowInActive) {
            filters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        }
        if (searchText) {
            if (searchText.indexOf('"') > -1) {
                searchText = searchText.replace(/"/g, "");
            }
        }
        if (!this.LookUpTable.CacheOnClient || this.IsTenantZeroSearch) {//calling data from server;
            this.CallDataFromServer(searchText, filters);
        }
        else {//calling data from cache;
            this.CallDataFromCache(searchText, filters, setFirstAsSelected);
        }

        if (this.IsAllDataVisible && !setFirstAsSelected) {
            var tenantZeroFilters: ApiQueryFilters;
            if (!this.QueryFilterItems) {
                tenantZeroFilters = new ApiQueryFilters();

            }
            else {

                tenantZeroFilters = new ApiQueryFilters();
                for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                    var filter: FilterItem = this.QueryFilterItems.AdditionalFilters[i];
                    tenantZeroFilters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator,
                        filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, false);
                }
            }

            if (this.IsDecendingSort == true) {
                tenantZeroFilters.SortDirection = "Decending";
            }
            else {
                tenantZeroFilters.SortDirection = "Ascending";

            }
            if (this.LookUpTable.SortingByObjectField) {
                tenantZeroFilters.SortBy = this.LookUpTable.SortingByObjectField;
            }

            tenantZeroFilters.Tenant = 0;
            if (searchText && !this.UseCompactSearch) {
                tenantZeroFilters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null, false, false);
            }

            if (inactiveField && !this.ShowInActive) {
                tenantZeroFilters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null, false, false);
            }

            this.SetDependencyProperties(tenantZeroFilters);

            tenantZeroFilters.PageIndex = 0;
            if (this.IsAllDataVisible) {
                tenantZeroFilters.PageSize = 5;

            } else {
                tenantZeroFilters.PageSize = 10;

            }

            //turn loading flag on
            this.isLoadingZero = true;

            var loadPromise = this.entityListService.getByFilters(this.LookUpTableName, tenantZeroFilters);
            if (this.UseCompactSearch) {

                let filterParams: ApiQueryFiltersAddParams =new ApiQueryFiltersAddParams();
                filterParams.FieldName="CompactSearchField"
                filterParams.FieldValue=searchText;
                filterParams.Operator="Contains";
                filterParams.IsCustom=false;
                filterParams.DisplayInList=false;
                filterParams.IsCustomField=false;
                filterParams.FieldDataType=null;
                filterParams.IsCacheOnClient=false;
                filterParams.IsLookUpFilter=true;
                //filters.addAdditionalFilter("CompactSearchField", searchText, null, null, "Contains", false, false, false, null);
                tenantZeroFilters.pushAdditionalFilter(filterParams);

                //tenantZeroFilters.addAdditionalFilter("CompactSearchField", searchText, null, null, "Contains", false, false, false, null, false, false);
                loadPromise = this.entityListService.getByCompactFilters(this.LookUpTableName, tenantZeroFilters);
            }

            loadPromise.then((res: any) => {
                res.subscribe((resp:any) => {
                    //turn loading flag off
                    this.isLoadingZero = false;
                    if (resp.Result) {
                        this.CalculateWidths(resp.Result);
                        this.ZeroItemsSourceCount = resp.Result.length;
                        this.ZeroItemsSource = resp.Result;
                        
                    }
                    else {
                        this.CalculateWidths(resp);
                        this.ZeroItemsSourceCount = resp.length;
                        this.ZeroItemsSource = resp;

                    }
                    this.ShowOrHideAllDataNoresultMessage();
                    this.HighlightSelectedValue();
                })
            });
        }

    }
    
    ShowOrHideAllDataNoresultMessage(){
        if (this.IsAllDataVisible) {
            if (this.ZeroItemsSourceCount == 0) {
                this.AllDataLovMessage=TextCodeTranslator.Translate("General.O.NoMoreResult");
            }
            else {
                this.AllDataLovMessage=null;
            }
        }
    }

    OnSearchInputBlur() {
        if (!this.MouseInArea) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
            clearTimeout(this.focusTimerToken);
            this.show = false;
            this.ShowErrorPopup = false;
            this.ShowMaintenanceBtn = false;
            this.IsPartnerMenuVisible = false;
            if (this.uiProperty.ValidValue) {
                this.timerToken = setTimeout(() => {
                    this.InputDivStyle = null;
                }, 300);
            }
            this.OnBlurEvent.emit({ Id: this.ElementId });
            this.LostFocus.emit(this.SelectedValue);
        }

        if (!this.SelectedItem && this.SearchTextNgModel != null && this.SearchTextNgModel != undefined) {
            if (this.LookUpTable.CacheOnClient && this.tabkeyDown == true && !this.IsOpen) {
                this.Populate(this.SearchTextNgModel, true);
                this.tabkeyDown = false;

            }
            else {
                this.SearchTextNgModel = null;
                this.OldSearchInput = this.SearchTextNgModel;
                this.ValidateField();
            }

        }
        // this.OnBlurEvent.emit({ Id: this.ElementId, Close: true });
    }

    OnMouseOver() {
        //this.ShowToolTip = true;
        this.MouseInArea = true;
        if (!this.SelectedItem || this.IsDisabled) {
            this.DeleteButtonNgStyle = { 'visibility': 'hidden' };
        } else {
            this.DeleteButtonNgStyle = null;
        }

        if (this.IsDisabled) {
            this.InputDivStyle = { 'border': '1px solid #AAAAAA', 'opacity': .5, /*'pointer-events': 'none'*/ };
        }
        else {

        }
    }

    OnMouseOut() {
        //this.ShowToolTip = false;
        this.MouseInArea = false;
    }

    SetDependencyProperties(apiQueryFilters: ApiQueryFilters) {

        //var table = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
        if (this.LookUpTable.DependencyFilter1 != null && this.LookUpTable.DependencyFilter1 != undefined) {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {
                var dependencyField: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === this.LookUpTable.Id && d.FieldName === this.LookUpTable.DependencyFilter1)[0];
                if (dependencyField != null) {
                    var fieldOperator: string = dependencyField.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter1IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter1IsListExact) {
                        fieldOperator = "InListExact";
                    }
                    //var depValue1: any = resolver.ResolveValue(dependencyField.DataTypeCode, DependencyProperty1 == null ? null : DependencyProperty1.ToString());
                    //apiQueryFilters.SetFilter(table.DependencyFilter1, depValue1, dependencyField.IsCustomFilter, fieldOperator, null, dependencyField.DisplayInList);
                    apiQueryFilters.addAdditionalFilter(dependencyField.FieldName, this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value),
                        null, null, fieldOperator, dependencyField.IsCustomFilter, dependencyField.DisplayInList, dependencyField.IsCustom,
                        dependencyField.DataTypeCode, false, this.LookUpTable.CacheOnClient);
                    //apiQueryFilters.Filter2Name = dependencyField.FieldName;
                    //apiQueryFilters.Filter2Operator = fieldOperator;
                    //apiQueryFilters.Filter2Value = this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value);

                }
            }
        }

        if (this.LookUpTable.DependencyFilter2 != null && this.LookUpTable.DependencyFilter2 != undefined) {
            if (this.DependencyFilter2Value != null && this.DependencyFilter2Value != undefined) {
                var dependencyField2: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === this.LookUpTable.Id && d.FieldName === this.LookUpTable.DependencyFilter2)[0];
                if (dependencyField2 != null) {
                    var fieldOperator: string = dependencyField2.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter2IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter2IsListExact) {
                        fieldOperator = "InListExact";
                    }
                    //var depValue1: any = resolver.ResolveValue(dependencyField.DataTypeCode, DependencyProperty1 == null ? null : DependencyProperty1.ToString());
                    //apiQueryFilters.SetFilter(table.DependencyFilter1, depValue1, dependencyField.IsCustomFilter, fieldOperator, null, dependencyField.DisplayInList);

                    apiQueryFilters.addAdditionalFilter(dependencyField2.FieldName, this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value),
                        null, null, fieldOperator, dependencyField2.IsCustomFilter, dependencyField2.DisplayInList, dependencyField2.IsCustom,
                        dependencyField2.DataTypeCode, false, this.LookUpTable.CacheOnClient);

                    //apiQueryFilters.Filter3Name = dependencyField2.FieldName;
                    //apiQueryFilters.Filter3Operator = fieldOperator;
                    //apiQueryFilters.Filter3Value = this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value);

                }
            }
        }

        if (this.LookUpTable.DependencyFilter3 != null && this.LookUpTable.DependencyFilter3 != undefined) {
            if (this.DependencyFilter3Value != null && this.DependencyFilter3Value != undefined) {
                var dependencyField: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === this.LookUpTable.Id && d.FieldName === this.LookUpTable.DependencyFilter3)[0];
                if (dependencyField != null) {
                    var fieldOperator: string = dependencyField.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter3IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter3IsListExact) {
                        fieldOperator = "InListExact";
                    }

                    apiQueryFilters.addAdditionalFilter(dependencyField.FieldName, this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter3Value),
                        null, null, fieldOperator, dependencyField.IsCustomFilter, dependencyField.DisplayInList, dependencyField.IsCustom,
                        dependencyField.DataTypeCode, false, this.LookUpTable.CacheOnClient);
                }
            }
        }
    }

    GetFieldValue(dataTypeCode: string, value: any) {
        if (value == null || value == undefined) {
            return null;
        }
        switch (dataTypeCode.toLowerCase()) {
            case "ntext":
            case "text":
                {
                    return value;
                }
            case "datetime":
                {
                    var date: Date;
                    date = new Date(value);
                    return date;
                }
            case "boolean":
                {
                    var bb: boolean;
                    if (typeof (value) == "string") {
                        if (value.toLowerCase() == 'false') {
                            bb = false;
                        }
                        else if (value.toLowerCase() == 'true') {
                            bb = true;
                        }
                    }
                    else {
                        bb = value;
                    }

                    return bb;
                }
            case "integer":
            case "double":
            case "decimal":
                {
                    var nn: number;
                    nn = Number(value);
                    return nn;
                }
            default:
                {
                    return value;
                }
        }
    }

    OnAllDataSelect(item: any) {

        this.CopySelectedItem(item.Id)


    }

    OnDropDownMouseOver() {
        this.MouseInArea = true;
        //this.CanClose = false;
        var input = document.getElementById(this.ElementId);
        input.focus();
    }

    OnDropDownMouseOut() {
        //this.CanClose = true;
        this.MouseInArea = false;
    }

    deleteSearchText: boolean;
    OnDeleteValue(deleteSearch: boolean = true) {
        this.showPopup = false;
        this.ShowMaintenanceBtn = false;
        this.IsPartnerMenuVisible = false;
        this.deleteSearchText = deleteSearch;
        this.selectedValue = null;
        this.SelectedItem = null;
        this.SetToolTipInfo();
        var value = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                customFieldClass.Value = null;
                this.DataContext[this.ObjectFieldName] = customFieldClass;
                value = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }

        }
        else {
            this.DataContext[this.ObjectFieldName] = null;
            value = this.DataContext[this.ObjectFieldName];
        }

        this.SelectedItemObject = null;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DisplayValue = null;
        this.ValueChanged.emit(value);
        if (deleteSearch) {
            this.SearchTextNgModel = null;
            this.OldSearchInput = this.SearchTextNgModel;
            this.ValidateField();

        }

    }

    SetValidity(validValue: boolean, errorMessage) {

        this.uiProperty.ValidValue = validValue;
        this.uiProperty.ValidationError = errorMessage;
        if (!validValue) {

            if (this.IsDisabled) {
                this.ShowErrorPopup = false;
                this.show = false;
                this.InputDivStyle = { 'border': '1px solid #AAAAAA', 'opacity': .5, /*'pointer-events': 'none'*/ };
            }
            else {
                if (this.searchTextChanged || this.ForceShowValidation) {
                    this.InputDivStyle = { 'border': '1px solid #ff0000', /*'pointer-events': 'all'*/ };
                    if (this.show) {
                        this.ShowErrorPopup = true;
                    }
                }
            }



        }

        else {
            if (this.IsDisabled) {
                this.InputDivStyle = { 'border': '1px solid #AAAAAA', 'opacity': .5, /*'pointer-events': 'none'*/ };
            }
            else {
                this.ShowErrorPopup = false;
                if (this.show) {
                    this.InputDivStyle = { 'border': '1px solid #3BB3E2', /*'pointer-events': 'all'*/ };
                }
                else {
                    this.InputDivStyle = null;
                }
            }
        }
    }
    SearchButtonClicked() {

        if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("Your package doesn't include this module..");
            return;
        }

        else if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "READ") && this.LookUpTable.EnableSecurity) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("You have no permission to view entities of this type.");
            return;
        }

        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }

        this.showPopup = false;
        this.ShowMaintenanceBtn = false;
        this.IsPartnerMenuVisible = false;
        this.ShowErrorPopup = false;
        //var ObjectTable = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0];
        //var ObjectTableId = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0].Id;
        var args = new CustomEntityArgs();
        args.ObjectTableId = this.LookUpTable.Id;
        args.ObjectTableName = this.LookUpTableName;
        args.IsTenantZeroSearch = this.IsTenantZeroSearch;
        args.IsAllDataVisible = this.IsAllDataVisible;
        args.ShowInActive = this.ShowInActive;
        args.PartnerTypes = this.PartnerTypes;
        args.DependencyFilter1Value = this.DependencyFilter1Value;
        args.DependencyFilter1IsList = this.DependencyFilter1IsList;
        args.DependencyFilter1IsListExact = this.DependencyFilter1IsListExact;
        args.DependencyFilter2Value = this.DependencyFilter2Value;
        args.DependencyFilter2IsList = this.DependencyFilter2IsList;
        args.DependencyFilter2IsListExact = this.DependencyFilter2IsListExact;
        args.DependencyFilter3Value = this.DependencyFilter3Value;
        args.DependencyFilter3IsList = this.DependencyFilter3IsList;
        args.DependencyFilter3IsListExact = this.DependencyFilter3IsListExact;
        args.UseCompactSearch = this.UseCompactSearch;
        args.QueryFilterItems = this.QueryFilterItems;
        args.HideAdd = this.HideAdd;
        args.HideEdit = this.HideEdit;
        args.IsAddDisabled = this.isAddDisabled;
        args.IsEditDisabled = this.isEditDisabled;
        args.DisplayFieldsFromList = this.DisplayFieldsFromList;
        var tablename = TextCodeTranslator.TranslateTablePlural(this.GetObjectTableName(this.LookUpTableName));

        if (tablename == "Cards") {
            tablename = "Partners";
        }

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;

        if (AppTool.IsMobileDetected()) {
            logitudeWindow.IsFullScreen = true;
        }

        logitudeWindow.WindowArgs = args;
        logitudeWindow.Title = tablename + " Search";
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/LogSearchWindowComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnSearchWindowClosed($event));

    }

    OnSearchWindowClosed(args: any) {
        if (args && args != 'event') {  // No value returned
            // Get Selected value
            var id = null;
            var tenant = null;
            if (args.indexOf(',')) {
                var argsarr = args.split(',');
                id = argsarr[0];
                tenant = argsarr[1];
            }
            else {
                id = args;
            }
            if (tenant == 0 && !this.IsTenantZeroSearch) {
                this.CopySelectedItem(id);
            }
            else {
                this.entityListService.getSingle(id, this.LookUpTableName).then((res: any) => {
                    res.subscribe(myResponse => {
                        if (myResponse != null) {

                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse) {
                                list = myResponse.Result;
                            }
                            this.isSelectedFromList = true;
                            this.SearchTextNgModel = list[this.DisplayMemberPath];
                            this.OldSearchInput = this.SearchTextNgModel;
                            this.DisplayValue = list[this.DisplayMemberPath];
                            this.SelectedItem = list;
                            this.SetToolTipInfo();
                            var value = this.DataContext[this.ObjectFieldName];
                            if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                                var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];;
                                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                                    value = customFieldClass.Value;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                }


                            }
                            else {
                                this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
                                value = this.DataContext[this.ObjectFieldName];
                            }
                            this.SelectedItemObject = this.SelectedItem;
                            this.SelectedItemChanged.emit(this.SelectedItem);
                            this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
                            this.ValueChanged.emit(value);
                            this.ValidateField();
                            this.IsDropDownVisible = false;
                            this.IsOpen = false;
                        }
                    })
                });
            }
            //var item = this.GetSingle(args);

            ////var item = args.SelectedItem;
            //this.isSelectedFromList = true;
            //this.SelectedItem = item;
            //this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
            //this.SelectedItemObject = this.SelectedItem;
            //this.SelectedItemChanged.emit(this.SelectedItem);
            //this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
            //this.SearchTextNgModel = this.SelectedItem[this.DisplayMemberPath];
            //this.ValidateField();
            ////this.ToggleOpenDropDown();
            //this.IsDropDownVisible = false;
            //this.IsOpen = false;
        }
    }

    ValidateField(emitPropertyChanged: boolean = true) {
        if (!this.NoValidation && this.uiProperty) {
            var errors = null;
            //var table = window.ObjectTables.filter(d => d.Name === this.uiProperty.ObjectTableName)[0];
            if (this.ObjectTable) {
                var field: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTable.Id && d.FieldName === this.uiProperty.FieldName)[0];
                var fieldValidator: FieldValidator = new FieldValidator();
                errors = fieldValidator.Validate(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            }
            if (this.uiProperty.IsValidManually == false) {
                this.SetValidity(false, this.uiProperty.ManualValidationError);
            }
            else if (errors) {
                if (errors.length > 0) {
                    this.SetValidity(false, errors[0]);
                }
                //else if (this.uiProperty.IsValidManually == false) {
                //    this.SetValidity(false, this.uiProperty.ManualValidationError);
                //}
                else {
                    this.SetValidity(true, null);
                }

            }
            //else if (this.uiProperty.IsValidManually == false) {
            //    this.SetValidity(false, this.uiProperty.ManualValidationError);
            //}
            else {
                this.SetValidity(true, null);
            }
            if (emitPropertyChanged) {
                this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
            }
        }
    }
    focusTimerToken: any;
    OnFocus() {
        this.show = true;
        this.showPopup = false;
        this.focusTimerToken = setTimeout(() => {
            if (this.uiProperty.ValidValue) {
                this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
                this.ShowErrorPopup = false;
            }
            else if (this.searchTextChanged || this.ForceShowValidation) {
                this.InputDivStyle = { 'border': '1px solid #ff0000' };
                this.ShowErrorPopup = true;
            }
            if (!this.IsOpen) {
                var input = document.getElementById(this.ElementId);
                Selection(input);
                //input.select();
            }
        }, 1);

    }

    OnBlur() {
        this.show = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
    }

    OnToggleClicked() {
        this.ToggleOpenDropDown();
        if (this.IsOpen) {
            this.Populate(null);
        }
    }

    TogglePopup() {
        this.showPopup = !this.showPopup;
    }

    // Tool Button On Click
    OnMaintenanceClick() {
        this.CheckEditAddEnable();
        var input = document.getElementById(this.ElementId);
        input.focus();
        this.TogglePopup();
        this.IsOpen = false;
        this.IsDropDownVisible = false;
    }

    ColWidth(index: number) {

        return this.Widths[index];
    }

    CalculateWidths(items: any[]) {
        if (items.length > 0) {
            this.Widths = [];
            //var lookup = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
            var lookupFields: any[];
            if (this.DisplayFieldsFromList != null && this.DisplayFieldsFromList != undefined) {
                var fields: string[] = this.DisplayFieldsFromList.split(',');
                lookupFields = window.ObjectFields.filter(d => d.ObjectTableId == this.LookUpTable.Id && fields.lastIndexOf(d.FieldName) > -1);
            }
            else {
                //if(!SessionInfo.LoggedUserPM.ShowLocalNameInLOV){
                if (this.LanguageFilterValue == 'E') {
                    lookupFields = window.ObjectFields.filter(d => d.DisplayOnLookUp && d.ObjectTableId == this.LookUpTable.Id);
                }
                else {
                    lookupFields = window.ObjectFields.filter(d => d.DisplayOnLookUpLocal && d.ObjectTableId == this.LookUpTable.Id);
                    if (lookupFields.length == 0) {
                        console.warn("There is no Fields defined as display in lookup local");
                        this.ShowLanguageFilter = false;
                        lookupFields = window.ObjectFields.filter(d => d.DisplayOnLookUp && d.ObjectTableId == this.LookUpTable.Id);
                    }
                }
            }
            if (!this.DisplayFieldsFromList) {
                lookupFields = lookupFields.sort((a, b) => { return a.DisplayInLookUpIndex - b.DisplayInLookUpIndex });
            }

            for (var i = 0; i < lookupFields.length; i++) {
                var words: string[] = [];
                for (var j = 0; j < items.length; j++) {
                    words.push(items[j][lookupFields[i].FieldName]);
                }
                var value = this.GetTheLongestWord(words);
                var maxLength = value.length;
                if (maxLength > 22) {
                    maxLength = 22;
                }
                var width = (maxLength * 7) + 8;
                if (i > 1 && width > 85) {
                    width = 85;
                }
                this.Widths.push(width);

            }
            var sum = 0;
            for (var i = 0; i < this.Widths.length; i++) {
                if (i != 1) {
                    sum += (this.Widths[i] > this.MinWidths[i] ? this.Widths[i] : this.MinWidths[i]);
                }
            }
            this.Widths[1] = this.DropDownWidth - sum;
        }
    }

    GetTheLongestWord(words: string[]) {
        var maxWord = '';
        for (var i = 0; i < words.length; i++) {
            if (words[i]) {
                if (words[i].length > maxWord.length) {
                    maxWord = words[i];
                }
            }
        }
        return maxWord;
    }

    // Edit Button Commands
    OnEditValue() {
        if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "UPDATE") && this.LookUpTable.EnableSecurity) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("You have no permission to edit an entity of this type.");
            return;
        }

        if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("Your package doesn't include this module..");
            return;
        }
        if (this.isEditDisabled)
            return;
        if (!this.IsPickList) {
            var currentEntity: any = this.SelectedItem;

            if (currentEntity != null && currentEntity != null) {
                //disableValidationPopups = true;
                ////validationPopup.IsOpen = false;
                ////warningPopup.IsOpen = false;
                //SetWarningPopupVisibility(false);
                //SetValidationPopupVisibility(false);

                var objectTableName: string = this.LookUpTableName;

                if (objectTableName == "Card" || objectTableName == "Carrier") {
                    objectTableName = this.GetObjectTableNameForDependency(this.SelectedItem["PartnerTypeId"], objectTableName);
                }
                //if (objectTableName == "Card" || objectTableName == "Carrier") {
                //    objectTableName = this.GetObjectTableName(objectTableName);

                //}

                if (!AppTool.IsNullOrEmpty(currentEntity.Id)) {
                    var logWindow = new LogitudeWindow();
                    logWindow.Title = TextCodeTranslator.TranslateTable("General.B.Edit") + " " + TextCodeTranslator.TranslateTable(objectTableName);
                    logWindow.ShowEditComponent(currentEntity.Id, objectTableName);
                    logWindow.WindowClosed.subscribe(($event: any) => {
                        //if ($event)
                        this.OnEditCompleted();
                    });
                }
                //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                //    .then(cmpRef => {
                //        cmpRef.instance.ComponentRef = cmpRef;
                //        cmpRef.instance.Run({ EntityId: currentEntity.Id, ObjectTableName: objectTableName });
                //        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                //            if ($event)
                //                this.OnEditCompleted();
                //        });
                //    });
            }
        }

        this.ShowMaintenanceBtn = false;
        this.IsPartnerMenuVisible = false;
        this.showPopup = false;
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }

    }
    OnEditCompleted() {
        var value = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom) {
            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];;
                value = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }

        }

        this.entityListService.getSingle(value, this.LookUpTableName).then((res: any) => {
            res.subscribe(myResponse => {
                if (myResponse != null) {

                    var list = myResponse;
                    if (myResponse instanceof ServiceResponse) {
                        list = myResponse.Result;
                    }
                    this.SearchTextNgModel = list[this.DisplayMemberPath];
                    this.OldSearchInput = this.SearchTextNgModel;
                    this.DisplayValue = list[this.DisplayMemberPath];
                    this.SelectedItem = list;
                    this.SetToolTipInfo();
                    this.SelectedItemObject = this.SelectedItem;
                    this.SelectedItemChanged.emit(this.SelectedItem);
                    var dateContextValue = this.DataContext[this.ObjectFieldName];
                    if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                        var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                        if (customFieldClass != null && customFieldClass != undefined) {
                            customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];;
                            this.DataContext[this.ObjectFieldName] = customFieldClass;
                            dateContextValue = customFieldClass.Value;
                        }
                        else {
                            console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                        }

                    }
                    else {
                        //this.DataContext[this.ObjectFieldName] = null; -------------bug 33534
                        this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
                        dateContextValue = this.DataContext[this.ObjectFieldName];
                    }

                    this.ValueChanged.emit(dateContextValue);
                    this.ValidateField();
                }
            })
        });
        this.showPopup = false;
    }

    // Add Button Commands
    OnAddValue() {
        if (this.isAddDisabled || this.IsAddTypesVisible)
            return;
        this.ShowMaintenanceBtn = false;
        this.IsPartnerMenuVisible = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
        if (!this.IsPickList) {
            this.NewEntityMethod(this.LookUpTableName);
        }

    }
    NewEntityMethod(objectTableName) {
        // var lookup: ObjectTablePM = window.ObjectTables.filter(d => d.Name === objectTableName)[0];

        //disableValidationPopups = true;
        //SetWarningPopupVisibility(false);
        //SetValidationPopupVisibility(false);
        objectTableName = this.GetObjectTableName(objectTableName);
        var originalTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name.toLowerCase() === objectTableName.toLocaleLowerCase())[0];

        if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "NEW") && this.LookUpTable.EnableSecurity) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("You have no permission to add a new entity of this type.");
            return;
        }

        if (!FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("Your package doesn't include this module..");
            return;
        }

        if (this.TenantPM.Id != 0) {
            if (objectTableName == "Port" || objectTableName == "Airline" || objectTableName == "Carrier" || objectTableName == "ShippingLine") {

                var windowTitle = "Add " + objectTableName;
                var logWindow = new LogitudeWindow();
                var args = new ImportEntityArgs();
                args.ObjectTableId = this.LookUpTable.Id;
                args.ObjectTableName = objectTableName;
                logWindow.WindowArgs = args;
                logWindow.Width = 1000;
                logWindow.Height = 600;
                logWindow.Title = windowTitle;
                logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');
                logWindow.WindowClosed.subscribe(($event: any) => {
                    var tablename = this.GetObjectTableName(this.LookUpTableName);
                    CachedDataManager.RefreshTableData(tablename, true);
                    this.OnNewEntityWindowClosed($event);

                });



                return;
            }
        }
        if (originalTable.IsNewWizard) {
            this.RunNewEntityWizard(originalTable.NewWizardComponentPath, originalTable.Name);
        }

        else {
            this.RunNewGenaricEntity();
        }


    }
    private RunNewEntityWizard(newWizardComponentPath: string, originalTableName: string) {

        //var ObjectTable = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0];


        var componentPath: string = newWizardComponentPath;//this.LookUpTable.NewWizardComponentPath;

        if (componentPath != null) {

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;

            switch (this.LookUpTableName) {
                case "Customs.Client": 
                case "Customs.CourierPendingReason":
                    {
                        
                    logWindow.Width = 800;
                    logWindow.Height = 600;
                    logWindow.IsShowCloseButton = true;
                    break;
                }

                case "Customs.Declaration":
                case "Customs.PaymentOrder":
                    {
                        logWindow.Width = 400;
                        logWindow.Height = 300;
                        logWindow.IsShowCloseButton = true;
                        break;
                    }

                case "Customs.CustomsVendor": {
                    logWindow.IsShowCloseButton = true;
                    break;
                }
                case "User": {
                    logWindow.Width = 965;
                    logWindow.Height = 600;
                    break;
                }
            }

            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(originalTableName));
            logWindow.Title = windowTitle;


            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            logWindow.Show(componentPath);
        }

        else {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 450;
            messageWindow.Height = 190;
            messageWindow.Show("Fill NewWizard Component Path and Name in ObjectTable !!");
        }
    }
    private RunNewGenaricEntity() {

        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity(this.LookUpTableName).then(response => {

            var args = new AddEntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = this.LookUpTableName;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.LookUpTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            logWindow.Show(componentPath);
        });
    }
    private OnNewEntityWindowClosed($event: any) {
        //console.log($event);
        if ($event && $event != "event") {
            this.entityListService.getSingle($event, this.LookUpTableName).then((res: any) => {
                res.subscribe(myResponse => {
                    if (myResponse != null) {

                        if (this.LookUpTable.CacheOnClient) {
                            CachedDataManager.RefreshTableData(this.LookUpTableName, true);
                        }
                        var list = myResponse;
                        if (myResponse instanceof ServiceResponse) {
                            list = myResponse.Result;
                        }

                        this.SearchTextNgModel = list[this.DisplayMemberPath];
                        this.OldSearchInput = this.SearchTextNgModel;
                        this.DisplayValue = list[this.DisplayMemberPath];
                        this.SelectedItem = list;
                        this.SetToolTipInfo();
                        this.SelectedItemObject = this.SelectedItem;
                        this.SelectedItemChanged.emit(this.SelectedItem);
                        this.selectedValue = this.SelectedItem[this.SelectedValuePath];
                        var value = this.DataContext[this.ObjectFieldName];
                        if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                            var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                            if (customFieldClass != null && customFieldClass != undefined) {
                                customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];;
                                this.DataContext[this.ObjectFieldName] = customFieldClass;
                                value = customFieldClass.Value;
                            }
                            else {
                                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                            }

                        }
                        else {
                            this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
                            value = this.DataContext[this.ObjectFieldName];
                        }
                        this.ValidateField();
                    }
                })
            });
            this.showPopup = false;
        }
    }


    //****

    CheckEditAddEnable() {
        //var lookup = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];

        if (this.SelectedItem == null) {
            this.isEditDisabled = true;
            this.isDeleteDisabled = true;

        }
        else {
            this.isEditDisabled = false;
            this.isDeleteDisabled = false;

            if (ObjectsLocator.IsDemoTenant(this.TenantPM.Id.toString()) && !SessionLocator.LoggedUserPM.IsCustomerCare) {
                if (this.LookUpTableName == "ChargesType" || this.LookUpTableName == "User" || this.LookUpTableName == "Contact") {
                    this.isEditDisabled = true;
                }
            }
        }

        if (!this.IsPickList) {
            if ((this.LookUpTable.EnableAddFromLOV) && !this.HideAdd) {
                this.isAddDisabled = false;

                if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString()) && !SessionLocator.LoggedUserPM.IsCustomerCare && (this.LookUpTableName == "User" || this.LookUpTableName == "Contact")) {
                    this.ShowAddLink = false;

                }

                else {
                    this.ShowAddLink = true;
                }

                if (ObjectsLocator.IsDemoTenant(this.TenantPM.Id.toString()) && !SessionLocator.LoggedUserPM.IsCustomerCare) {
                    if (this.LookUpTableName == "ChargesType" || this.LookUpTableName == "User" || this.LookUpTableName == "Contact") {
                        this.isAddDisabled = true;
                        this.ShowAddLink = false;

                    }
                }
            }
            else {
                this.isAddVisible = false;
            }

            if (!this.LookUpTable.EnableEditFromLOV || this.HideEdit) {
                this.isEditVisible = false;
            }
        }

        if (this.LookUpTableName == "Card" || this.LookUpTableName == "Carrier") {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {
                if (this.DependencyFilter1Value.toString().split(',').length > 1) {
                    this.IsAddTypesVisible = true;

                }
            }
        }
    }

    private GetObjectTableName(parentObjectName: string) {
        var dep: string = this.DependencyFilter1Value != null ? this.DependencyFilter1Value.toString() : "";
        return this.GetObjectTableNameForDependency(dep, parentObjectName);
    }
    private GetObjectTableNameForDependency(dependency: string, parentObjectName: string) {
        var partnerType = this.PartnerTypes.filter(p => p.Id.toLowerCase() == dependency.toLowerCase())[0];
        if (partnerType != null && partnerType != undefined) {
            var name: string = partnerType.Name.replace(" ", "");

            if (name == "PotentialCustomer") {
                name = "Customer";
            }

            var table: ObjectTablePM = window.ObjectTables.filter(d => d.Name.toLowerCase() === name.toLocaleLowerCase())[0];
            if (table != null) {
                return table.Name;
            }
            else {
                return parentObjectName;
            }
        }
        else {
            return parentObjectName;
        }
    }

    private timerToken: any;
    onhover: boolean = false;
    OnMouseHover() {
        this.onhover = true;
        this.timerToken = setTimeout(() => {
            if (this.onhover) {
                //this.show = true;
                this.showPopup = false;

                if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString()) && !SessionLocator.LoggedUserPM.IsCustomerCare && (this.LookUpTableName == "User" || this.LookUpTableName == "Contact")) {
                    this.ShowMaintenanceBtn = false;
                }

                else {
                    this.ShowMaintenanceBtn = true;
                }
            }
        }, 700);
    }
    OnMouseLeave() {
        this.onhover = false;
        this.timerToken = setTimeout(() => {
            if (!this.showPopup && !this.MouseInArea) {
                this.ShowMaintenanceBtn = false;
                this.IsPartnerMenuVisible = false;
                this.showPopup = false;
            }
        }, 700);
    }

    CopySelectedItem(id: string) {
        this.entityListService.getEntityCopyToCurrentTenant(id, this.LookUpTableName).then((res: any) => {
            res.subscribe(myResponse => {
                if (myResponse != null) {

                    var list = myResponse;
                    if (myResponse instanceof ServiceResponse) {
                        list = myResponse.Result;
                    }

                    this.isSelectedFromList = true;
                    this.SelectedItem = list;
                    this.SetToolTipInfo();
                    var value = this.DataContext[this.ObjectFieldName];
                    if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                        var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                        if (customFieldClass != null && customFieldClass != undefined) {
                            customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];;
                            this.DataContext[this.ObjectFieldName] = customFieldClass;
                            value = customFieldClass.Value;
                        }
                        else {
                            console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                        }

                    }
                    else {
                        this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
                        value = this.DataContext[this.ObjectFieldName];
                    }
                    this.SelectedItemObject = this.SelectedItem;
                    this.SelectedItemChanged.emit(this.SelectedItem);
                    this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
                    this.SearchTextNgModel = this.SelectedItem[this.DisplayMemberPath];
                    this.OldSearchInput = this.SearchTextNgModel;
                    if (this.IsOpen) {
                        this.ToggleOpenDropDown();
                    }
                    this.ValidateField();
                    CachedDataManager.RefreshTableData(this.LookUpTableName, true);
                    // this.OnBlurEvent.emit("");
                }
            })
        });
    }

    CallDataFromCache(searchText: string, filters: ApiQueryFilters, setFirstAsSelected: boolean = false) {


        if (!this.LookUpTable.AutoCompleteSearchWindow) {
            filters.PageSize = 1000;
        }
        else {
            if (!this.IsAllDataVisible) {
                filters.PageSize = 10;
            }
            else {
                filters.PageSize = 5;
            }
        }

        if (this.UseCompactSearch) {
            filters.removeAdditionalFilter("CompactSearchField");
        }

        let filterParams: ApiQueryFiltersAddParams =new ApiQueryFiltersAddParams();
        filterParams.FieldValue=searchText;
        filterParams.Operator="StartsWith";
        filterParams.IsCustom=false;
        filterParams.DisplayInList=false;
        filterParams.IsCustomField=false;
        filterParams.FieldDataType=null;
        filterParams.IsCacheOnClient=this.LookUpTable.CacheOnClient;
        filterParams.IsLookUpFilter=true;
        if (searchText) {

            //if (this.QueryFilterItems && this.QueryFilterItems.AdditionalFilters.length > 0 && this.callCount == 0) {
            //    this.callCount = 1;

            //}

            if (this.currentFilter == null || this.currentFilter == undefined) {
                this.callCount = 1;
                var forceEnableAdd = false;
                if (this.QueryFilterItems && this.QueryFilterItems.AdditionalFilters.length > 0) {
                    for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter: FilterItem = this.QueryFilterItems.AdditionalFilters[i];
                        if (this.LookUp1 == filter.FieldName) {
                            forceEnableAdd = true;
                        }
                    }
                }
                
                filterParams.FieldName=this.LookUp1;
                filterParams.ForceEnableAdd=forceEnableAdd;
                
               // filters.addAdditionalFilter(this.LookUp1, searchText, null, null, "StartsWith", false, false, false, null, false, this.LookUpTable.CacheOnClient, forceEnableAdd);
                this.currentFilter = this.LookUp1;
            }
            else if (this.currentFilter == this.LookUp1 && this.LookUp2 != null && this.LookUp2 != undefined && this.LookUp1 != this.LookUp2) {
                this.callCount = 2;
                var forceEnableAdd = false;
                if (this.QueryFilterItems && this.QueryFilterItems.AdditionalFilters.length > 0) {
                    for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter: FilterItem = this.QueryFilterItems.AdditionalFilters[i];
                        if (this.LookUp2 == filter.FieldName) {
                            forceEnableAdd = true;
                        }
                    }
                }
                if (this.LookUpTable.DependencyFilter1 != this.LookUp1 
                    && this.LookUpTable.DependencyFilter2 != this.LookUp1 
                    && this.LookUpTable.DependencyFilter3 != this.LookUp1) {
                    filters.removeAdditionalFilter(this.LookUp1);
                }
                filterParams.FieldName=this.LookUp2;
                filterParams.ForceEnableAdd=forceEnableAdd;
                //filters.addAdditionalFilter(this.LookUp2, searchText, null, null, "StartsWith", false, false, false, null, false, this.LookUpTable.CacheOnClient, forceEnableAdd);
                this.currentFilter = this.LookUp2;
            }
            else {
                this.callCount = 3;
                var forceEnableAdd = false;
                if (this.QueryFilterItems && this.QueryFilterItems.AdditionalFilters.length > 0) {
                    for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter: FilterItem = this.QueryFilterItems.AdditionalFilters[i];
                        if ("SearchFields" == filter.FieldName) {
                            forceEnableAdd = true;
                        }
                    }
                }
                if (this.LookUpTable.DependencyFilter1 != this.LookUp1 && this.LookUpTable.DependencyFilter2 != this.LookUp1 && this.LookUpTable.DependencyFilter3 != this.LookUp1) {
                    filters.removeAdditionalFilter(this.LookUp1);
                }
                if (this.LookUpTable.DependencyFilter1 != this.LookUp2 && this.LookUpTable.DependencyFilter2 != this.LookUp2 && this.LookUpTable.DependencyFilter3 != this.LookUp2) {
                    filters.removeAdditionalFilter(this.LookUp2);
                }
                filterParams.FieldName="SearchFields";
                filterParams.ForceEnableAdd=forceEnableAdd;
                filterParams.Operator="Contains";
                
                //filters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null, false, this.LookUpTable.CacheOnClient, forceEnableAdd);
                this.currentFilter = null;
            }
            filters.pushAdditionalFilter(filterParams);
        }
        //turn loading flag on
        this.isLoading = true;
        this.entityListService.getAllFromCache(this.LookUpTableName, filters).then((res: any) => {
            res.subscribe((resp:any) => {
                if (resp.Result) {

                    for (var i = 0; i < resp.Result.length; i++) {
                        var item = this.bufferData.filter(d => d[this.LookUpTable.KeyPropertyPath] == resp.Result[i][this.LookUpTable.KeyPropertyPath])[0];
                        if (!item) {
                            this.bufferData.push(resp.Result[i]);
                        }
                    }
                    if (setFirstAsSelected) {
                        if (this.bufferData.length > 0) {
                            this.FocusOnSelect = false;
                            var item = this.bufferData.filter(d => d[this.LookUp1] != null && d[this.LookUp1].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                            if (!item && this.LookUp2) {
                                var item = this.bufferData.filter(d => d[this.LookUp2] != null && d[this.LookUp2].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                            }
                            if (!item) {
                                var item = this.bufferData.filter(d => d[this.DisplayMemberPath].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
                            }
                            if (item) {
                                this.OnSelected(item);
                            }
                            else {
                                this.SearchTextNgModel = null;
                                this.OldSearchInput = this.SearchTextNgModel;
                                this.ValidateField();
                            }
                            this.callCount = 0;
                            return;
                        }
                        else if (this.callCount == 3) {
                            this.SearchTextNgModel = null;
                            this.OldSearchInput = this.SearchTextNgModel;
                            this.ValidateField();
                            return;
                        }

                    }
                    if (this.bufferData.length < filters.PageSize && this.callCount < 3 && this.callCount > 0) {
                        filters.PageSize = filters.PageSize - this.bufferData.length;
                        this.CallDataFromCache(searchText, filters, setFirstAsSelected);
                        return;
                    }

                    this.callCount = 0;
                    this.CalculateWidths(this.bufferData);
                    if (this.ManipulateData) {
                        this.ApplyManipulateData(this.bufferData);

                    } else {
                        this.ItemsSource = this.bufferData;//resp.Result;
                        this.ItemsSourceCount = this.ItemsSource.length;//resp.Result.length;
                    }


                }
                else {

                    for (var i = 0; i < resp.length; i++) {
                        var item = this.bufferData.filter(d => d[this.LookUp1] == resp[i][this.LookUp1])[0];
                        if (!item) {
                            this.bufferData.push(resp[i]);
                        }
                    }
                    if (setFirstAsSelected) {
                        if (this.bufferData.length > 0) {
                            this.FocusOnSelect = false;
                            this.OnSelected(this.bufferData[0]);
                            this.callCount = 0;
                            return;
                        }
                        else if (this.callCount == 3) {
                            this.SearchTextNgModel = null;
                            this.OldSearchInput = this.SearchTextNgModel;
                            this.ValidateField();
                            return;
                        }

                    }
                    if (this.bufferData.length < filters.PageSize && this.callCount < 3 && this.callCount > 0) {
                        filters.PageSize = filters.PageSize - this.bufferData.length;
                        this.CallDataFromCache(searchText, filters, setFirstAsSelected);
                        return;
                    }

                    this.callCount = 0;
                    this.CalculateWidths(this.bufferData);
                    if (this.ManipulateData) {
                        this.ApplyManipulateData(this.bufferData);

                    } else {
                        this.ItemsSource = this.bufferData;//resp.Result;
                        this.ItemsSourceCount = this.ItemsSource.length;//resp.Result.length;
                    }

                }
                if (!this.ManipulateData) {
                    if (this.ItemsSourceCount == 0) {
                        //this.LovMessage = "No more results founds";
                        this.LovMessage = TextCodeTranslator.Translate("General.O.NoMoreResult");
                    }
                    else {
                        this.LovMessage = null;
                    }
                    
                    this.ItemsSourceStatic = this.ItemsSource;
                    this.HighlightSelectedValue();
                    this.isLoading = false;
                }
                this.CD.detectChanges();

            });
        });
    }

    CallDataFromServer(searchText: string, filters: ApiQueryFilters) {
        
        if (!this.LookUpTable.AutoCompleteSearchWindow) {
            filters.PageSize = 50;
        }
        else {
            if (!this.IsAllDataVisible) {
                filters.PageSize = 10;
            }
            else {
                filters.PageSize = 5;
            }
        }
        this.isLoading = true;


        var loadPromise = this.entityListService.getByFilters(this.LookUpTableName, filters);
        if (this.UseCompactSearch) {
            let filterParams: ApiQueryFiltersAddParams =new ApiQueryFiltersAddParams();
            filterParams.FieldName="CompactSearchField"
            filterParams.FieldValue=searchText;
            filterParams.Operator="Contains";
            filterParams.IsCustom=false;
            filterParams.DisplayInList=false;
            filterParams.IsCustomField=false;
            filterParams.FieldDataType=null;
            filterParams.IsCacheOnClient=false;
            filterParams.IsLookUpFilter=true;
            //filters.addAdditionalFilter("CompactSearchField", searchText, null, null, "Contains", false, false, false, null);
            filters.pushAdditionalFilter(filterParams);
            loadPromise = this.entityListService.getByCompactFilters(this.LookUpTableName, filters);
        }
        else if (searchText) {

            searchText = searchText.replace(/\\/g, "\\\\");

            let filterParams: ApiQueryFiltersAddParams =new ApiQueryFiltersAddParams();
            filterParams.FieldValue=searchText;
            filterParams.Operator="StartsWith";
            filterParams.IsCustom=false;
            filterParams.DisplayInList=false;
            filterParams.IsCustomField=false;
            filterParams.FieldDataType=null;
            filterParams.IsCacheOnClient=false;
            filterParams.IsLookUpFilter=true;

            if (this.currentFilter == null || this.currentFilter == undefined) {
                this.callCount = 1;
                filterParams.FieldName=this.LookUp1;
                
                //filters.addAdditionalFilter(this.LookUp1, searchText, null, null, "StartsWith", false, false, false, null);
                this.currentFilter = this.LookUp1;
            }
            else if (this.currentFilter == this.LookUp1 && this.LookUp2 != null && this.LookUp2 != undefined && this.LookUp1 != this.LookUp2) {
                this.callCount = 2;
                if (this.LookUpTable.DependencyFilter1 != this.LookUp1 && this.LookUpTable.DependencyFilter2 != this.LookUp1 && this.LookUpTable.DependencyFilter3 != this.LookUp1) {
                    filters.removeAdditionalFilter(this.LookUp1);
                }
                filterParams.FieldName=this.LookUp2;
                //filters.addAdditionalFilter(this.LookUp2, searchText, null, null, "StartsWith", false, false, false, null);
                this.currentFilter = this.LookUp2;
            }
            else {
                this.callCount = 3;
                if (this.LookUpTable.DependencyFilter1 != this.LookUp1 && this.LookUpTable.DependencyFilter2 != this.LookUp1 && this.LookUpTable.DependencyFilter3 != this.LookUp1) {
                    filters.removeAdditionalFilter(this.LookUp1);
                }
                if (this.LookUpTable.DependencyFilter1 != this.LookUp2 && this.LookUpTable.DependencyFilter2 != this.LookUp2 && this.LookUpTable.DependencyFilter3 != this.LookUp2) {
                    filters.removeAdditionalFilter(this.LookUp2);
                }
                filterParams.FieldName="SearchFields";
                filterParams.Operator="Contains";
                //filters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null);
                this.currentFilter = null;
            }
            filters.pushAdditionalFilter(filterParams);            


        }
 
        loadPromise.then((res: any) => {
            res.subscribe((resp:any) => {
                if (resp.Result) {

                    for (var i = 0; i < resp.Result.length; i++) {
                        var item = this.bufferData.filter(d => d[this.LookUpTable.KeyPropertyPath] == resp.Result[i][this.LookUpTable.KeyPropertyPath])[0];
                        if (!item) {
                            this.bufferData.push(resp.Result[i]);
                        }
                    }

                    if (this.bufferData.length < filters.PageSize && this.callCount < 3 && !this.UseCompactSearch && this.callCount > 0) {
                        this.CallDataFromServer(searchText, filters);
                        return;
                    }
                    this.callCount = 0;
                    this.CalculateWidths(this.bufferData);
                    if (this.ManipulateData) {
                        this.ApplyManipulateData(this.bufferData);

                    } else {
                        this.ItemsSource = this.bufferData;//resp.Result;
                        this.ItemsSourceCount = this.ItemsSource.length;//resp.Result.length;
                        this.headerColumns;

                    }

                }
                else {

                    for (var i = 0; i < resp.length; i++) {
                        this.bufferData.push(resp[i]);
                    }
                    if (this.bufferData.length < filters.PageSize) {
                        this.CallDataFromServer(searchText, filters);
                        return;
                    }
                    this.CalculateWidths(resp);
                    if (this.ManipulateData) {
                        this.ApplyManipulateData(this.bufferData);

                    } else {
                         this.ItemsSource = this.bufferData;//resp.Result;
                        this.ItemsSourceCount = this.ItemsSource.length;//resp.Result.length;
                        this.headerColumns;
                    }

                }
                if (!this.ManipulateData) {
                    if (this.ItemsSourceCount == 0) {
                        //this.LovMessage = "No more results founds";
                        this.LovMessage = TextCodeTranslator.Translate("General.O.NoMoreResult");
                    }
                    else {
                        this.LovMessage = null;
                    }
                    
                    this.ItemsSourceStatic = this.ItemsSource;
                    this.HighlightSelectedValue();

                    //turn loading flag off
                    this.isLoading = false;
                }
            })
        });
    }

    OnSearchInputKeyUP($event) {
        var CTRL = 17;
        if ($event.keyCode == CTRL) {
            this.IsCTRLDown = false;
        }
    }

    ApplyManipulateData(bufferData: any[]) {
 
        var objectTableName = this.LookUpTableName;
        if (this.LookUpTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = this.LookUpTable.ClientModuleName;
        var classname = objectTableName + "DataChangeService";
        var servicelink = './' + moduleName + '/Services/DataChange/' + classname;

        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                this.ItemsSource = service.ApplyDataChange(bufferData);
                this.ItemsSourceCount = this.ItemsSource.length;

                if (this.ItemsSourceCount == 0) {
                    //this.LovMessage = "No more results founds";
                    this.LovMessage = TextCodeTranslator.Translate("General.O.NoMoreResult");
                }
                else {
                    this.LovMessage = null;
                }
               
                this.ItemsSourceStatic = this.ItemsSource;
                this.HighlightSelectedValue();
                this.isLoading = false;
            });
        });
    }

    SetToolTipInfo() {
        if (this.SelectedItem != null && !AppTool.IsNullOrEmpty(this.SearchTextNgModel)) {
            var hasCodeField: boolean = this.headerColumns.filter(f => f.Field == "Code")[0];
            if (this.LookUp2 != null) {
                var nameText = this.SelectedItem[this.LookUp2];
                var codeText = this.SelectedItem[this.LookUp1];
                this.LovToolTip = codeText + "," + nameText;
            }
            else {
                if (this.LookUp1 != "Code" && hasCodeField) {
                    var actualCodeText = this.SelectedItem["Code"];
                    var codeText = this.SelectedItem[this.LookUp1];
                    this.LovToolTip = actualCodeText + "," + codeText;
                }
            }
        }
        else {
            this.LovToolTip = '';
        }
    }

    OnMouseOverAdd() {
        if (this.IsAddTypesVisible) {
            this.IsPartnerMenuVisible = true;
            var mtcPopup = document.getElementById('MTCPopup');
            var itemRect = mtcPopup.getBoundingClientRect();
            var top = itemRect.top;
            var left = itemRect.left;
            this.PartnersPopupTop = top + 'px';
            this.PartnersPopupLeft = left + 87 + 'px';
        }
    }

    OnMouseOverEdit() {
        if (this.IsAddTypesVisible) {
            this.IsPartnerMenuVisible = false;
        }
    }

    OnMouseOverDelete() {
        if (this.IsAddTypesVisible) {
            this.IsPartnerMenuVisible = false;
        }
    }

    OnMouseOutAdd() {
        //if (this.IsAddTypesVisible) {
        //    this.IsPartnerMenuVisible = false;
        //}
    }

    AddPartnerOfType(partnerType: PartnerTypeList) {
        this.IsPartnerMenuVisible = false;
        this.ShowMaintenanceBtn = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }

        var objectTableName = this.GetObjectTableNameForDependency(partnerType.Id, this.LookUpTableName);
        var objectTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == objectTableName)[0];
        if (objectTable.IsNewWizard) {
            this.RunNewEntityWizard(objectTable.NewWizardComponentPath, objectTable.Name);
        }
        else {
            this.RunNewGenaricEntity();
        }
    }
    LanguageFilterValue: string;
    ShowLanguageFilter: boolean = false;
    SwitchBetweenLocalAndEng(lang: string) {
        this.LanguageFilterValue = lang;
        this.DrawColumns();
        this.SetDisplayMemberPath();
    }
}

export class EntityArgs {
    public ObjectTableName: string = null;
    public ObjectTableId: string = null;
}
export class AddEntityArgs {
    public EntityPM: any;
    public ObjectTableName: string;
}
