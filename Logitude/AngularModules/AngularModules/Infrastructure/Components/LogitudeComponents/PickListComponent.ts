declare var window: any;
declare var logLoveReturnWhich, Selection: any;
import {Input, Output, Component, OnInit, EventEmitter, AfterViewInit, OnDestroy} from '@angular/core';
import {CustomFieldClass} from '../../DataContracts/CustomFieldClass';
import {BaseComponent} from './BaseComponent';
import {UIProperty, UIProperties, UIPropertyArgs} from './UIProperties';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {AppTool} from '../../Tools';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';
import {ApiQueryFilters, FilterItem} from '../../DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../Services/EntityResourceService';
import {EntityListService} from '../../Services/EntityListService';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/throttleTime';
import 'rxjs/add/observable/fromEvent';
import {FieldValidator} from '../../Validators/FieldValidator';
import {ControlsIdCounter} from '../../Utilities/ControlsIdCounter';

@Component({
    selector: 'PickList',
    moduleId: './Infrastructure/Components/LogitudeComponents/',
    templateUrl: 'PickListComponent.html',
})

export class PickListComponent implements OnInit, AfterViewInit, OnDestroy {
    
    @Input() ObjectFieldName: string;
    @Input() ObjectTableName: string;
    @Input() DataContext: any;
    @Input() HideColumns: boolean;
    @Input() HideLastColumn: boolean;
    @Input() NoValidation: boolean;    
    @Input() PlaceHolder: string;
    @Input() IsMultipleChoice: boolean = false;
    @Input() PickListCode: string;
    @Input() FromNewView: boolean = false;
    @Input() IsFreeText: boolean = false;
    @Input() IgnoreCustomFieldCheck: boolean = false;
    //-------------------------------------------------------

    @Output() OnBlurEvent: EventEmitter<any> = new EventEmitter();
    @Output() ValueChanged = new EventEmitter();
    @Output() SelectedItemChanged: EventEmitter<any> = new EventEmitter();
    @Output() LostFocus: EventEmitter<any> = new EventEmitter();

    //-------------------------------------------------------
    DropDownWidth: number = 300;
    DropDownHeight: number = 257;
    ObjectTable: ObjectTablePM;
    ObjectField: ObjectFieldPM;
    LookUpTable: ObjectTablePM;
    DisplayMemberPath: string;
    SelectedValuePath: string;
    QueryFilterItems: ApiQueryFilters;
    LookUpTableName: string;
    private uiProperty: UIProperty;
    public ItemsSource: any[];
    public ItemsSourceCount: number = -1;
    public ItemsSourceStatic: any[];
    LovMessage: string;
    isLoading: boolean;
    SearchTextNgModel: string;
    SelectedItemKey: any;
    KeyPropertyPath: any;
    imgNgStyle: any;
  InputDivStyle: any;

  public get IsVisible() {
    if (!this.uiProperty) {
      this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
    }
    return this.uiProperty.IsVisible;
  }

    private isDisabled: boolean;
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
                    this.InputDivStyle = { 'opacity': 1, /*'pointer-events': 'all',*/ 'border': '1px solid #ff0000' };
                }
            }
        }
    }
    OldSearchInput: string;
    ElementId: string;
    AfterViewInitialized: boolean = false;
    IsCTRLDown: boolean = false;
    IsDropDownVisible: boolean;
    public IsOpen: boolean;
    searchTextChanged: boolean = false;
    FocusOnMe: boolean = false;
    private timerToken: any;
    showPopup: boolean = false;
    deleteSearchText: boolean;
    public SelectedItem: any;
    DisplayValue: string;
    ShowErrorPopup: boolean = false;
    show: boolean = false;
    MouseInArea: boolean;
    DeleteButtonNgStyle: any;
    counterId: number;
    DivPickListId: string;
    DropdownId: string;
    ErrorPopUpId: string;
    MyDataListId: string;
    isDeleteDisabled: boolean;
    HideMaintenanceIcon: boolean;
    ShowMaintenanceBtn: boolean;
    isSelectedFromList: boolean;
    tabkeyDown: boolean = false;
    onhover: boolean = false;
    public ObjectFieldHelp: string = null;
    public ShowHelp: boolean = false;
    private selectedValue: any;
    @Input()
    public get SelectedValue() {
        return this.selectedValue;
    }
    public set SelectedValue(newValue: any) {
        if (this.selectedValue != newValue) {
            this.selectedValue = newValue;
            if (!this.LookUpTable || this.IsFreeText) {
                this.LookUpTable = new ObjectTablePM();
                this.LookUpTable.Name = 'CustomPickList';
                this.LookUpTableName = 'CustomPickList';
                this.LookUpTable.Tenant = SessionLocator.Tenant;
                this.LookUpTable.LookUp1 = "Value";
                this.LookUpTable.CacheOnClient = true;
            }


            if (this.LookUpTable) {
                if (!this.isSelectedFromList || this.IsFreeText) {

                    this.ValueChanged.emit(this.selectedValue);

                    this.GetSingle(this.LookUpTable);
                }

                this.isSelectedFromList = false;
            }
        }
    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService, public entityListService: EntityListService) {

    }

    ngOnInit() {

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
        this.ObjectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
        this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTable.Id && d.FieldName === this.ObjectFieldName)[0];
        this.LookUpTable = new ObjectTablePM();
        this.LookUpTable.Name = 'CustomPickList';
        this.LookUpTableName = 'CustomPickList';
        this.LookUpTable.Tenant = SessionLocator.Tenant;
        this.LookUpTable.LookUp1 = "Value";
        this.LookUpTable.CacheOnClient = true;
        this.DisplayMemberPath = 'Value';
        this.SelectedValuePath = 'Id';
        this.KeyPropertyPath = 'Id';
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        if (this.IgnoreCustomFieldCheck == true) {
            this.DataContext[this.ObjectFieldName] = this.DataContext["TextValue"];
        }
        this.IsDisabled = !this.uiProperty.IsEnabled;
        this._entityResourceService.getEntityResourceByTableName('CustomPickList', 0).subscribe((res: any) => {

            var objectFieldAvailable: boolean = true;
            if (!this.ObjectField) {
                objectFieldAvailable = false;
            }
            else {
                objectFieldAvailable = true;
                if (this.ObjectField.HelpTextCodeId != null) {
                    this.ObjectFieldHelp = TextCodeTranslator.Translate(this.ObjectField.HelpTextTextCodeCode);

                    if (!AppTool.IsNullOrEmpty(this.ObjectFieldHelp)) {
                        if (this.ObjectFieldHelp.length > 1) {
                            this.ShowHelp = true;
                        }
                    }
                }

            }


            if (this.DataContext[this.ObjectFieldName]) {
                this.GetSingle(this.LookUpTable);
            }
            else if (this.FromNewView) {
                var lookup = this.LookUpTable;
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
                    //apiFilters = this.QueryFilterItems;
                }
                if (lookup.CacheOnClient) {
                    this.entityListService.getSingleFromCache(this.DataContext['TextValue'], this.LookUpTableName, apiFilters).then((res: any) => {
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
                                    this.SelectedItemChanged.emit(this.SelectedItem);
                                }

                            }
                        })
                    });
                }
                else {
                    this.entityListService.getSingle(this.DataContext['TextValue'], this.LookUpTableName).then((res: any) => {
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

                                    this.SelectedItemChanged.emit(this.SelectedItem);
                                }
                            }
                        })
                    });
                }
                if (objectFieldAvailable == true) {
                    this.ValidateField(true);
                }
            }
            if (objectFieldAvailable == true) {
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
            }


        });


    }

    ngAfterViewInit() {
        this.InitializeAfterViewInit();
    }

    ngOnDestroy() {

    }

    InitializeAfterViewInit() {

        var input = document.getElementById(this.ElementId);
        if (input != null && input != undefined) {
            this.AfterViewInitialized = true;
        }
        Observable.fromEvent(input, 'keydown')
            .debounceTime(400)
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

    CheckIfExists(IdCom: string) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    }

    SetControlIds(baseIdCombination: string) {

        this.DivPickListId = 'picklist_' + baseIdCombination;
        this.ElementId = baseIdCombination;
        this.DropdownId = 'picklistdropdown-' + baseIdCombination;
        this.ErrorPopUpId = 'picklistererrorpop_' + baseIdCombination;
        this.MyDataListId = 'mydatapicklist_' + baseIdCombination;
    }

    Populate(searchText: string, setFirstAsSelected: boolean = false) {

        this.LovMessage = null;

        //reset counters
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

        filters.SortDirection = "Ascending";
        filters.PageIndex = 0;
        filters.SortBy = 'Value';
        if (this.ObjectField != null) {
            filters.addAdditionalFilter("Code", this.ObjectField.CustomPickListCode, null, null, "Equals", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        }
        else {
            filters.addAdditionalFilter("Code", this.PickListCode, null, null, "Equals", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        }
        filters.addAdditionalFilter("IsMultipleChoice", this.IsMultipleChoice, null, null, "Equals", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        this.CallDataFromCache(searchText, filters, setFirstAsSelected);

    }

    CallDataFromCache(searchText: string, filters: ApiQueryFilters, setFirstAsSelected: boolean = false) {


        if (!this.LookUpTable.AutoCompleteSearchWindow) {
            filters.PageSize = 1000;
        }
        else {
            filters.PageSize = 10;
        }
        if (searchText) {
            filters.addAdditionalFilter('Value', searchText, null, null, "StartsWith", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        }

        //turn loading flag on
        this.isLoading = true;
        this.entityListService.getByFilters(this.LookUpTableName, filters).then((res: any) => {
            res.subscribe(resp => {
                if (resp.Result) {

                    this.ItemsSource = resp.Result;
                    this.ItemsSourceCount = resp.Result.length;
                }
                else {
                    this.ItemsSource = resp;
                    this.ItemsSourceCount = resp.length;
                }
                if (this.ItemsSourceCount == 0) {
                    this.LovMessage = "No more results founds";
                }
                else {
                    this.LovMessage = null;
                }
                this.ItemsSourceStatic = this.ItemsSource;
                this.HighlightSelectedValue(this.ItemsSource);
                this.isLoading = false;
            });
        });
    }

    HighlightSelectedValue(items: any[]) {
        this.SelectedItemKey = null;
        if (this.SearchTextNgModel != null && this.SearchTextNgModel != undefined && this.SearchTextNgModel != "") {

            var item = items.filter(d => d[this.DisplayMemberPath].toLowerCase() === this.SearchTextNgModel.toLowerCase())[0];
            if (item) {
                var index = items.indexOf(item);
                var temp = items[0];
                items[0] = item;
                items[index] = temp;
                this.SelectedItemKey = item[this.KeyPropertyPath];
            }
            else if (items.length > 0) {
                this.SelectedItemKey = items[0][this.KeyPropertyPath];
            }
        }
    }

    GetSingle(lookup: any) {

        if (this.DataContext[this.ObjectFieldName] || this.IsFreeText) {

            var value = this.IsFreeText ? this.selectedValue : this.DataContext[this.ObjectFieldName];
            if (this.ObjectField) {
                if (this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                    var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];

                    if (customFieldClass != null && customFieldClass != undefined) {
                        value = customFieldClass.Value;
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                    }

                }
            }

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
                //apiFilters = this.QueryFilterItems;
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
            this.SelectedItemChanged.emit(this.SelectedItem);
        }
    }

    OnDeleteValue(deleteSearch: boolean = true) {
        this.showPopup = false;
        this.deleteSearchText = deleteSearch;
        this.SelectedItem = null;

        if (this.ObjectField) {
            if (this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    customFieldClass.Value = null;
                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                }
                else {
                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                }


            }
            else {
                this.DataContext[this.ObjectFieldName] = null;
            }
        }
        else {
            this.DataContext[this.ObjectFieldName] = null;
        }
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DisplayValue = null;
        this.ValueChanged.emit(this.DataContext[this.ObjectFieldName]);
        if (deleteSearch) {
            this.SearchTextNgModel = null;
            this.OldSearchInput = this.SearchTextNgModel;
            this.ValidateField();

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
                this.InputDivStyle = { 'border': '1px solid #ff0000', /*'pointer-events': 'all'*/ };
                if (this.show) {
                    this.ShowErrorPopup = true;
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

    OnMouseOver() {
        this.MouseInArea = true;
        if (!this.SelectedItem || this.IsDisabled) {
            this.DeleteButtonNgStyle = { 'visibility': 'hidden' };
        } else {
            this.DeleteButtonNgStyle = null;
        }
        if (this.IsDisabled) {
            this.InputDivStyle = { 'border': '1px solid #AAAAAA', 'opacity': .5, /*'pointer-events': 'none'*/ };
        }
    }

    OnMouseOut() {
        this.MouseInArea = false;
    }


    OnMouseHover() {
        this.onhover = true;
        this.timerToken = setTimeout(() => {
            if (this.onhover) {
                //this.show = true;
                this.ShowMaintenanceBtn = true;
                this.showPopup = false;
            }
        }, 700);
    }

    OnMouseLeave() {
        this.onhover = false;
        this.timerToken = setTimeout(() => {
            if (!this.showPopup && !this.MouseInArea) {
                this.ShowMaintenanceBtn = false;
                this.showPopup = false;
            }
        }, 700);
    }

    OnToggleClicked() {
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

    OnMaintenanceClick() {
        this.CheckEditAddEnable();
        var input = document.getElementById(this.ElementId);
        input.focus();
        this.TogglePopup();
        this.IsOpen = false;
        this.IsDropDownVisible = false;
    }

    CheckEditAddEnable() {
        if (this.SelectedItem == null) {
            this.isDeleteDisabled = true;

        }
        else {
            this.isDeleteDisabled = false;
        }
    }

    TogglePopup() {
        this.showPopup = !this.showPopup;
    }

    OnSearchInputBlur() {
        if (!this.MouseInArea) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
            this.show = false;
            this.ShowErrorPopup = false;
            this.ShowMaintenanceBtn = false;
            if (this.uiProperty.ValidValue) {
                this.InputDivStyle = null;
            }
            this.OnBlurEvent.emit({ Id: this.ElementId });
            if (this.ObjectField) {
                if (this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                    var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null && customFieldClass != undefined) {
                        this.LostFocus.emit(customFieldClass.Value);
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                    }

                }
                else {
                    this.LostFocus.emit(this.DataContext[this.ObjectFieldName]);
                }
            }
            else {
                this.LostFocus.emit(this.DataContext[this.ObjectFieldName]);
            }

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
    }

    OnSelected(item: any) {
        this.isSelectedFromList = true;
        this.SelectedItem = item;

        if (this.ObjectField) {
            if (this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                var customFieldClass: CustomFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];
                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                }
                else {
                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                }


            }
            else {
                this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
            }
        }
        else {
            this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
        }
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
        this.SearchTextNgModel = this.SelectedItem[this.DisplayMemberPath];
        this.OldSearchInput = this.SearchTextNgModel;
        this.ValueChanged.emit(this.DataContext[this.ObjectFieldName]);
        this.ValidateField();
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
        this.show = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
        var elem = document.getElementById(this.ElementId);
        elem.focus();
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

    OnFocus() {
        this.show = true;
        this.showPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
            this.ShowErrorPopup = false;
        }
        else {
            this.InputDivStyle = { 'border': '1px solid #ff0000' };
            this.ShowErrorPopup = true;
        }
        if (!this.IsOpen) {
            var input = document.getElementById(this.ElementId);
            Selection(input);
            //input.select();
        }

    }

    OnBlur() {
        this.show = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
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
                if (this.IsOpen) {
                    this.Populate(null);
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
            this.OnDeleteValue(false);
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

    OnSearchInputKeyUP($event) {
        var CTRL = 17;
        if ($event.keyCode == CTRL) {
            this.IsCTRLDown = false;
        }
    }
}
