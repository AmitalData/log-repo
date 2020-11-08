declare var window: any;
declare var System: any;
import {Directive, ElementRef, Input, Output, Component, OnInit, OnChanges, Injector, EventEmitter, AfterViewInit, OnDestroy} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {EntityListService} from '../../Services/EntityListService';
import {ServiceArgs} from '../../DataContracts/ServiceArgs';
import {ApiQueryFilters, FilterItem} from '../../DataContracts/ApiQueryFilters';
import {ControlsIdCounter} from '../../Utilities/ControlsIdCounter';
import {EntityResourceService} from '../../Services/EntityResourceService';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {FormControl} from '@angular/forms';
import {CustomEntityArgs} from './DWLogSearchWindowComponent';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../Utilities/InfraSettings';
import {EntityPMService} from '../../Services/EntityPMService';
import {PartnerTypeList} from '../../../Common/EntityLists/PartnerTypeList';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';
import { DWQueryBuilderService } from '../../Services/ExtendedPMs/DWQueryBuilderService';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
declare var logLoveReturnWhich, Selection: any;

@Component({
    selector: 'DWLov',
    
    templateUrl: './DWLovComponent.html',
    providers: [EntityListService, ServiceArgs, EntityResourceService],
    inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', 'DisplayMemberPath', 'SelectedValuePath',
        "AutoFocus", "IsFreeText", "AlwaysEnabled", "Operation","LOVAdditionalColumns"],
})

export class DWLovComponent implements OnInit, AfterViewInit, OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    public _DWQueryBuilderService: DWQueryBuilderService;
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
    public LOVAdditionalColumns: string = null;
    public ObjectTableName: string = null;
    public Operation: string;
    public AutoFocus: boolean;

    public SelectedValuePath: string;
    public DisplayMemberPath: string;
    public DataContext: any;
    public IsFreeText: boolean = false;
    public AlwaysEnabled: boolean = false;

    private dataContext: BaseComponent;

    ElementId: string;
    IsDropDownVisible: boolean;
    ctrl: FormControl;
    SearchTextValue: FormControl;


    isFirstTime: boolean = true;
    imgNgStyle: any;


    public IsOpen: boolean;
    public LayoutDirection = 'ltr'
    public LayoutDirectionClassName = 'RightCenter';
    public SelectedItem: any;
    @Input() SelectedItemObject: any;
    public ItemsSource: any[];
    public ItemsSourceCount: number = -1;
    public ItemsSourceStatic: any[];
    ClosedByBlur: boolean;
    DisplayValue: string;
    MouseInArea: boolean;
    private headerColumns: any[];
    private dataColumns: any[];
    DivLogLovId: string;
    ToolTipId: string;
    LogLOVControlClass: string;
    DisplayHeader: boolean;
  
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
            this.SearchTextNgModel = newValue;
            //this.ValueChanged.emit(this.SelectedValue);
        }
    }




    private  searchTextNgModel:string = "";
    public get SearchTextNgModel() {
        return this.searchTextNgModel;
    }
    public set SearchTextNgModel(newValue: any) {
        if (this.searchTextNgModel != newValue) {
            this.searchTextNgModel = newValue;
            
         
        }
    }


    displayTextNgModel: string;
    public get DisplayTextNgModel() {
        if (this.DataContext) {
            this.displayTextNgModel = this.DataContext.MultiSelectedDisplayName;
        }
        return this.displayTextNgModel;
    }
    public set DisplayTextNgModel(newValue: string) {
        if (this.displayTextNgModel != newValue) {
            this.displayTextNgModel = newValue;
        }
    }


    @Input() RunToggleMode: boolean;
    @Input() AutoCompleteSearchWindow: boolean;
    @Input() ForceShowAddLink: boolean;
    showToggleButton: boolean;
    showPopup: boolean = false;
    ObjectTable: ObjectTablePM;

    private PartnerTypes: Array<PartnerTypeList> = [];
    tabkeyDown: boolean = false;
    FocusOnMe: boolean = false;
    searchTextChanged: boolean = false;
    ErrorMessage: string;
    PropertyChangedSubscribtion: any;
    FocusOnSelect: boolean = true;
    @Input() DisplayFieldsFromList: string;
    private LovPartnerTypes: Array<PartnerTypeList> = [];
    constructor(public entityListService: EntityListService, private entityPMService: EntityPMService,
        private _entityResourceService: EntityResourceService) {
        this._DWQueryBuilderService = new DWQueryBuilderService();
        this.TenantPM = InfraSettings.TenantPM;
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.LayoutDirectionClassName = this.LayoutDirection == 'rtl' ? 'LeftCenter' : 'RightCenter';
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
    DropDownHeight: number = 204;

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

                            /*this.OldSearchInput = this.SearchTextNgModel;
                            this.IsDropDownVisible = true;
                            this.IsOpen = true; 
                            this._DWQueryBuilderService.GetDWDataForDimTabel(this.ObjectTableName, this.ObjectFieldName, this.SearchTextNgModel).subscribe((myResult:any) => {
                                if (!myResult.HasError) {
                                    this.ItemsSource = myResult.Result;
                                }

                            });
                            this.searchTextChanged = true;
                          */
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

    

    ngOnInit() { ////
        //this.entityListService.getAllFromCache(this.LookUpTableName, filters).then((res: any) => {
        //    res.subscribe((resp:any) => {

        //    });

        //});

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



        var objectFieldAvailable: boolean = true;
        this.ObjectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
        //(currentTable.AutoCompleteSearchWindow || this.AutoCompleteSearchWindow) && !this.RunToggleMode


        //*ngIf="ShowAddLink || ShowSearchButton"
        if (!this.ShowAddLink && !this.ShowSearchButton) {

            this.MyDropDownHeight = { 'height': '204px' }
        }

        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " LOV has no object field!");
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
            this.ToggleOpenDropDown();
        }
        else if ($event.keyCode != SHIFTKEY) {
            this.ToggleOpenDropDown(); 
        }




    }

    OnSelected(item: any) {
        this.isSelectedFromList = true;
        this.SelectedItem = item;
        this.SearchTextNgModel = item[this.ObjectFieldName];
        this.IsDropDownVisible = false;
        this.IsOpen =false;
        if (this.FocusOnSelect) {
            var elem = document.getElementById(this.ElementId);
            elem.focus();
        }
        this.FocusOnSelect = true;
        this.MouseInArea = false;
        this.ValueChanged.emit(item[this.ObjectFieldName]);

    }

    OnLogLovClicked() {
        this.ToggleOpenDropDown(); 

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
                if (this.ItemsSource.length > 0) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    lis[0].classList.add("highlighted");
                }

            }
            else {
                if (active[0].nextElementSibling) {
                    active[0].nextElementSibling.classList.add("highlighted");
                    active[0].classList.remove("highlighted");


                    active[0].scrollIntoView(false);
                }
            }
        }
        else {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {
                if (this.ItemsSource.length > 0) {
                    if (active[0].previousElementSibling) {
                        active[0].previousElementSibling.classList.add("highlighted");
                        active = document.getElementsByClassName("highlighted");
                        active[1].classList.remove("highlighted");
                        active[0].scrollIntoView(false);
                    }
                }
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
                var active = document.getElementsByClassName("highlighted");
                if (active[0]) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    for (var i = 0; i < lis.length; i++) {

                        if (lis[i].className.search("highlighted") > -1) {
                            this.FocusOnSelect = false;
                            lis[i].classList.remove("highlighted");
                            lis[i].click();
                        }
                    }

                }
                else {
                    var selected = document.getElementsByClassName("liItemSelected");
                    if (selected[0]) {
                        var input = document.getElementById(this.MyDataListId);
                        var lis = input.getElementsByTagName("li");
                        for (var i = 0; i < lis.length; i++) {

                            if (lis[i].className.search("liItemSelected") > -1) {
                                this.FocusOnSelect = false;
                                lis[i].classList.remove("liItemSelected");
                                lis[i].click();
                            }
                        }
                    }
                }

            }
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
            //this.OnBlurEvent.emit("");
        }

        if ($event.keyCode == ENTERKEY) {
            if (this.IsOpen) {
                var active = document.getElementsByClassName("highlighted");
                if (active[0]) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    for (var i = 0; i < lis.length; i++) {

                        if (lis[i].className.search("highlighted") > -1) {
                            lis[i].classList.remove("highlighted");
                            lis[i].click();
                        }
                    }

                }
                else {
                    var selected = document.getElementsByClassName("liItemSelected");
                    if (selected[0]) {
                        var input = document.getElementById(this.MyDataListId);
                        var lis = input.getElementsByTagName("li");
                        for (var i = 0; i < lis.length; i++) {

                            if (lis[i].className.search("liItemSelected") > -1) {
                                lis[i].classList.remove("liItemSelected");
                                lis[i].click();
                            }
                        }
                    }
                }
            }
        }

        if ($event.keyCode == DOWNKEY) {

            if (!this.IsOpen) {
                this.ToggleOpenDropDown(); 
            }
            else {
                var selected = document.getElementsByClassName("liItemSelected");
                if (selected[0]) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    for (var i = 0; i < lis.length; i++) {

                        if (lis[i].className.search("liItemSelected") > -1) {
                            lis[i].classList.remove("liItemSelected");
                            lis[i].classList.add("highlighted");
                        }
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
            
        }

        //if ((48 <= key && key <= 57) || (65 <= key && key <= 90) || key == 8 || key == 46 || key == 32) {
        //    this.OnDeleteValue(false);
        //}
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
    KeyPropertyPath: any;
    HighlightSelectedValue(items: any[]) {
        this.SelectedItemKey = null;
        if (this.SearchTextNgModel != null && this.SearchTextNgModel != undefined && this.SearchTextNgModel != "") {

            var oldItems = items;


            var item = items.filter(d => d[this.DisplayMemberPath] != null && d[this.DisplayMemberPath].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];

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
            else if (items.length > 0) {
                this.SelectedItemKey = items[0][this.KeyPropertyPath];
            }
        }
    }


    OnSearchInputBlur() {
        if (!this.MouseInArea) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
            clearTimeout(this.focusTimerToken);

            this.ShowErrorPopup = false;
            this.ShowMaintenanceBtn = false;
            this.IsPartnerMenuVisible = false;

            this.OnBlurEvent.emit({ Id: this.ElementId });
            this.LostFocus.emit(this.SelectedValue);
        }

        if (!this.SelectedItem && this.SearchTextNgModel != null && this.SearchTextNgModel != undefined) {

            this.SearchTextNgModel = null;
            this.OldSearchInput = this.SearchTextNgModel;
            //this.ValidateField();

        }

    }

    OnMouseOver() {
        //this.ShowToolTip = true;
        this.MouseInArea = true;
        if (!this.SelectedItem) {
            this.DeleteButtonNgStyle = { 'visibility': 'hidden' };
        } else {
            this.DeleteButtonNgStyle = null;
        }
    }

    OnMouseOut() {
        //this.ShowToolTip = false;
        this.MouseInArea = false;
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



    SearchButtonClicked() {


        //if (this.IsOpen)
        //    this.ToggleOpenDropDown();
        //this.showPopup = false;
        //this.ShowMaintenanceBtn = false;
        //this.IsPartnerMenuVisible = false;
        //this.ShowErrorPopup = false;
        ////var ObjectTable = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0];
        ////var ObjectTableId = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0].Id;
        var args = new CustomEntityArgs();

        args.ObjectTableName = this.ObjectTableName;
        args.DisplayFieldsFromList = this.ObjectFieldName;
        args.LOVAdditionalColumns = this.LOVAdditionalColumns;
        args.DataContext = this.DataContext;
        //args.IsAllDataVisible = this.IsAllDataVisible;
        //args.ShowInActive = this.ShowInActive;
        //args.PartnerTypes = this.PartnerTypes;

        //args.QueryFilterItems = this.QueryFilterItems;
        //args.HideAdd = this.HideAdd;
        //args.HideEdit = this.HideEdit;
        //args.IsAddDisabled = this.isAddDisabled;
        //args.IsEditDisabled = this.isEditDisabled;
        //args.DisplayFieldsFromList = this.DisplayFieldsFromList;
        //var tablename = TextCodeTranslator.TranslateTablePlural(this.GetObjectTableName(this.LookUpTableName));
        //if (tablename == "Cards")
        //    tablename = "Partners";

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 900;
        logitudeWindow.Height = 600;
        logitudeWindow.WindowArgs = args;


        logitudeWindow.Title = this.ObjectTableName + " Search";
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/DWLogSearchWindowComponent');

        logitudeWindow.WindowClosed.subscribe(($event) => {
            if ($event != "Cancel") {
                this.OnSearchWindowClosed($event);
            }
        });


    }

    OnSearchWindowClosed(args: any) {
        this.SearchTextNgModel = args;
        this.SelectedValue = args;
        this.SelectedItem = args;
        this.ValueChanged.emit(this.SelectedValue);

    }


    focusTimerToken: any;
    OnFocus() {

        this.showPopup = false;
        this.focusTimerToken = setTimeout(() => {
            //if (this.searchTextChanged || this.ForceShowValidation) {
            //    this.InputDivStyle = { 'border': '1px solid #ff0000' };
            //    this.ShowErrorPopup = true;
            //}
            if (!this.IsOpen) {
                var input = document.getElementById(this.ElementId);
                Selection(input);
                //input.select();
            }
        }, 1);

    }

    OnBlur() {
        //this.show = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        //if (this.uiProperty.ValidValue) {
        //    this.InputDivStyle = null;
        //}
    }

    OnToggleClicked() {
        /*this._DWQueryBuilderService.GetDWDataForDimTabel(this.ObjectTableName, this.ObjectFieldName, this.SearchTextNgModel ? this.SearchTextNgModel : "").subscribe((myResult:any) => {
            if (!myResult.HasError) {
                this.ItemsSource = myResult.Result;
                this.ToggleOpenDropDown();
            }

        });
        */
       
    }

    TogglePopup() {
        this.showPopup = !this.showPopup;
    }

    // Tool Button On Click
    OnMaintenanceClick() { 
        var input = document.getElementById(this.ElementId);
        input.focus();
        this.TogglePopup();
        this.IsOpen = false;
        this.IsDropDownVisible = false;
    }

    ColWidth(index: number) {

        return this.Widths[index];
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


    //****




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
                this.ShowMaintenanceBtn = true;

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

 

    OnSearchInputKeyUP($event) {
        var CTRL = 17;
        if ($event.keyCode == CTRL) {
            this.IsCTRLDown = false;
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

   
}

export class EntityArgs {
    public ObjectTableName: string = null;
    public ObjectTableId: string = null;
}
export class AddEntityArgs {
    public EntityPM: any;
    public ObjectTableName: string;
}
