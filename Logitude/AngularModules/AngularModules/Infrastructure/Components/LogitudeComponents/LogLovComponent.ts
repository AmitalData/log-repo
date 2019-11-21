declare var window: any;
declare var System: any;
import {Directive, ElementRef, Renderer, Input, Output, Component, OnInit, OnChanges, Injector,  EventEmitter} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {UIProperty, UIProperties} from './UIProperties';
import {EntityListService} from '../../Services/EntityListService';
import {ServiceArgs} from '../../DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../DataContracts/ApiQueryFilters';
import {ControlsIdCounter} from '../../Utilities/ControlsIdCounter';
import {EntityResourceService} from '../../Services/EntityResourceService';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {AppTool} from '../../Tools';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {FocusMeDirective} from '../../Utilities/FocusMeDirective';
import {FixedPositionDirective} from '../../Utilities/FixedPositionDirective';
import {ServiceResponse} from '../../DataContracts/ServiceResponse';
import {FieldValidator} from '../../Validators/FieldValidator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { CustomEntityArgs } from './LogSearchWindowComponent';

@Component({
    moduleId: module.id,

    selector: 'LogLov_Old',
    templateUrl: './LogLovComponent.html',
    providers: [EntityListService, ServiceArgs, EntityResourceService],
    inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', 'LookUpTableName', 'DisplayMemberPath', 'SelectedValuePath',
        'PlaceHolder', 'DependencyFilter1Value', 'DependencyFilter2Value', "HideColumns", "HideLastColumn", "DependencyFilter1IsList",
        "DependencyFilter2IsList", "DependencyFilter1IsListExact", "DependencyFilter2IsListExact", "AutoFocus", "IsTenantZeroSearch", "ShowInActive"],        
})

export class LogLovComponent implements OnInit {
    public ShowHelp: boolean = false;
    public ObjectField: ObjectFieldPM;
    public ObjectFieldName: string = null;
    public ObjectFieldHelp: string = null;
    public ObjectTableName: string = null;
    public LookUpTableName: string = null;
    public DependencyFilter1Value: Object;
    public DependencyFilter2Value: Object;
    public DependencyFilter1IsList: boolean;
    public DependencyFilter2IsList: boolean;
    public DependencyFilter1IsListExact: any;
    public DependencyFilter2IsListExact: any;
    public AutoFocus: boolean;
    public PlaceHolder: string;
    public HideColumns: boolean = false;
    public HideLastColumn: boolean = false;
    public SelectedValuePath: string;
    public DisplayMemberPath: string;
    public DataContext: any;
    public DataList: any[];
    
    private dataContext: BaseComponent;
    private uiProperty: UIProperty;
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
    isFirstTime: boolean;
    imgNgStyle: any;
    private isDisabled: boolean;
    public get IsDisabled() {
        return this.isDisabled;
    }
    public set IsDisabled(newValue: boolean) {
        this.isDisabled = newValue;
        if (this.isDisabled) {
            this.imgNgStyle = { 'opacity': .5, 'pointer-events': 'none'};
        }
        else {
            this.imgNgStyle = { 'opacity': 1, 'pointer-events': 'all' };
        }
    }
    DropStyle: any;
    public ContainerClass: string;
    public LiClass: string;
    public IsOpen: boolean;
    public SelectedItem: any;
    @Input() SelectedItemObject: any;
    public ItemsSource: any[];
    public ItemsSourceStatic: any[];
    ClosedByBlur: boolean;
    DisplayValue: string;
    MouseInArea: boolean;
    private headerColumns: any[];
    private dataColumns: any[];
    DivLogLovId: string;
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
    public get SelectedValue() {
        return this.selectedValue;
    }
    @Input() public set SelectedValue(newValue: any) {
        if (this.selectedValue != newValue) {
            this.selectedValue = newValue;
           

            if (!this.isSelectedFromList) {//&& !this.isFirstTime) {

                if (this.uiProperty != null){
                    this.uiProperty.UIPropertyChanged.emit("valuechanges");
            }
                this.ValueChanged.emit(this.selectedValue);

                //if (!this.isSelectedFromList && !this.isFirstTime) {
                    var lookup = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];

                    //this.DataContext[this.ObjectFieldName] = this.selectedValue;
                    this.GetSingle(lookup);
                //}
                this.isFirstTime = false;
                this.isSelectedFromList = false;
            }
            this.isFirstTime = false;
            this.isSelectedFromList = false;
        }
    }
    //@Input() SelectedValue: any;
    constructor(public entityListService: EntityListService, private _entityResourceService: EntityResourceService) {
         this.show = false;
        this.isFirstTime = true;
        this.Detach = true;

    }
    DropPopUpStyle: any;
    isSelectedFromList: boolean;
    MyDropDownHeight: any;
    IsAllDataVisible: boolean;
    public ZeroItemsSource: any[];
    AllDataHeaderTitle: string;
    MyDataHeaderTitle: string;
    DropdownId: string;
    LovDropDownStyle: any;
    ShowInActive: boolean;
    DeleteButtonNgStyle: any;
    ShowErrorPopup: boolean;
    ErrorPopUpId: string;
    ShowSearchButton: boolean;
    ngAfterViewChecked() {
        
        }
    ngOnInit() {
        this.InitializeControl();
    }

    InitializeControl() {
          this.LogLOVControlClass = "LogLOVControl";
        this.counterId = ControlsIdCounter.GetNextIdCounter();
        this.DivLogLovId = 'LogLov - ' + this.ObjectFieldName + '-' + this.counterId.toString();
        this.ElementId = 'Search - ' + this.ObjectFieldName + '-' + this.counterId.toString();
        this.DropdownId = 'LogLovDropDown-' + this.ObjectFieldName + '-' + this.counterId.toString();
        this.ErrorPopUpId = 'loglovererrorpop_' + this.counterId;
      
      
        var objectFieldAvailable: boolean = true;
        var lookup = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
        var table = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];

        if (lookup.AutoCompleteSearchWindow) {
            this.ShowSearchButton = true;
        }
        else {
            this.ShowSearchButton = false;
        }

        this.LookUp1 = lookup.LookUp1;
        this.LookUp2 = lookup.LookUp2;
        this.headerColumns = [];
        this.dataColumns = [];

        if (this.LookUpTableName == 'Port' || this.LookUpTableName == 'Carrier') {
            this.IsAllDataVisible = true;
            this.MyDropDownHeight = { 'height': '100px' }
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

        if (!this.DisplayMemberPath) {
            if (lookup.LookUp2) {
                this.DisplayMemberPath = lookup.LookUp2;
            }
            else {
                this.DisplayMemberPath = lookup.LookUp1;
            }
        }

        if (!this.SelectedValuePath) {
            this.SelectedValuePath = lookup.KeyPropertyPath;
        }

        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        this.IsDisabled = !this.uiProperty.IsEnabled;
       
        //this.ctrl = new FormControl(this.DataContext[this.ObjectFieldName]);
        //this.LogitudeForm.addControl(this.ObjectFieldName, this.ctrl);

        this._entityResourceService.getEntityResourceByTableName(this.LookUpTableName, 0).subscribe(res => {

            var lookupFields: any[] = window.ObjectFields.filter(d => d.DisplayOnLookUp && d.ObjectTableId == lookup.Id);
            var dropdownWidth = lookupFields.length * 120 + 20;
            this.DropPopUpStyle = { width: dropdownWidth.toString() + 'px' };
            var additionalCols = lookupFields.filter(d => d.DisplayInLookUpIndex > 1);
            if (lookupFields.length == 1) {
                this.DisplayHeader = false;
            }
            else if (!this.IsAllDataVisible) {
                this.DisplayHeader = true;
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
                            width = 95;
                        }
                        if (additionalCols.length > 1) {
                            width = (350 - 170) / additionalCols.length;
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


            if (table) {
                this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
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
                    if (this.ObjectField.DependencyFilter1Value) {
                        if (this.ObjectField.DependencyFilter1Type == "Constant") {
                            this.DependencyFilter1Value = this.ObjectField.DependencyFilter1Value;
                            this.DependencyFilter1IsList = this.ObjectField.DependencyFilter1IsList;
                        }
                        else {
                            this.DependencyFilter1Value = this.DataContext[this.ObjectField.DependencyFilter1Value];
                        }
                    }

                    if (this.ObjectField.DependencyFilter2Value) {
                        if (this.ObjectField.DependencyFilter2Type == "Constant") {
                            this.DependencyFilter2Value = this.ObjectField.DependencyFilter2Value;
                            this.DependencyFilter2IsList = this.ObjectField.DependencyFilter2IsList;
                        }
                        else {
                            this.DependencyFilter2Value = this.DataContext[this.ObjectField.DependencyFilter2Value];
                        }
                    }
                }
              
            }


            //


            //if (table) {
            //    var field = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
            //    if (!field) {
            //        objectFieldAvailable = false;
            //    }
            //}
            //else {
            //    objectFieldAvailable = false;
            //}

      
            if (this.DataContext[this.ObjectFieldName]) {

                this.GetSingle(lookup);

            }


            //this.ctrl.valueChanges.subscribe(res=> {
            //    this.uiProperty.UIPropertyChanged.emit("valuechanges");
            //    this.ValueChanged.emit(res);

            //    if (!this.isSelectedFromList && !this.isFirstTime) {
            //        this.DataContext[this.ObjectFieldName] = res;
            //        this.GetSingle(lookup);
            //    }
            //    this.isFirstTime = false;
            //    this.isSelectedFromList = false;
            //});

            this.SearchTextValue = new FormControl();
            this.SearchTextValue.valueChanges
                .debounceTime(400)
                .distinctUntilChanged()
                .subscribe((search): any => {
                    if (search != undefined) {
                        this.Populate(search);
                    }
                });

            this.uiProperty.UIPropertyChanged.subscribe(value=> {
                if (value != "valuechanges") {

                    var uiProperty: UIProperty = value as UIProperty;

                    this.IsDisabled = !this.uiProperty.IsEnabled;

                    if (objectFieldAvailable) {
                        //this.SetControlPropertiesAndValidations(uiProperty, this.ctrl)
                    }
                }
                this.DetectChanges();
            });
            if (objectFieldAvailable) {
                //this.SetControlPropertiesAndValidations(this.uiProperty, this.ctrl);
            }

            
        });

   
        

    }

    GetSingle(lookup: any) {
        
        if (this.DataContext[this.ObjectFieldName]) {
            var apiFilters: ApiQueryFilters = new ApiQueryFilters();
            if (lookup.CacheOnClient) {
                this.entityListService.getSingleFromCache(this.DataContext[this.ObjectFieldName], this.LookUpTableName, apiFilters).then((res:any) => {
                    res.subscribe(myResponse => {
                        if (myResponse != null) {

                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse) {
                                list = myResponse.Result;
                            }

                            this.DisplayValue = list[this.DisplayMemberPath];
                            this.SelectedItem = list;
                            this.SelectedItemObject = this.SelectedItem;
                            this.SelectedItemChanged.emit(this.SelectedItem);
                            this.DetectChanges();

                        }
                    })
                });
            }
            else {
                this.entityListService.getSingle(this.DataContext[this.ObjectFieldName], this.LookUpTableName).then((res:any) => {
                    res.subscribe(myResponse => {
                        if (myResponse != null) {

                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse) {
                                list = myResponse.Result;
                            }

                            this.DisplayValue = list[this.DisplayMemberPath];
                            this.SelectedItem = list;
                            this.SelectedItemObject = this.SelectedItem;
                            this.SelectedItemChanged.emit(this.SelectedItem);
                            this.DetectChanges();
                        }
                    })
                });
            }
        }
        else {
            this.DisplayValue = null;
            this.SelectedItem = null;
            this.SelectedItemObject = null;
            this.SelectedItemChanged.emit(this.SelectedItem);
            this.DetectChanges();
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

    SetControlPropertiesAndValidations(uiProperty: UIProperty, ctrl: FormControl) {

        this.uiProperty.IsRequired = uiProperty.IsRequired;
        var table = window.ObjectTables.filter(d => d.Name === uiProperty.ObjectTableName)[0];
        var field = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === uiProperty.FieldName)[0];
        if (field) {
            var minlength = field.MinLength;
            var maxlenght = field.MaxLength;
            var hasminmax: boolean;
            hasminmax = false;

            if (field.DataTypeCode.toLowerCase() == "text" || field.DataTypeCode.toLowerCase() == "ntext") {
                if (maxlenght != 0) {
                    hasminmax = true;
                }
            }
        }
        if (this.uiProperty.IsRequired) {

            if (this.DataContext[this.ObjectFieldName] == null || this.DataContext[this.ObjectFieldName] == "") {
                this.ctrl.setErrors({ "required": true });
            }

            if (hasminmax) {
                this.ctrl.validator = Validators.compose([Validators.required, Validators.minLength(minlength), Validators.maxLength(maxlenght)]);
            }
            else {
                this.ctrl.validator = Validators.required;
            }
        }
        else if (!this.uiProperty.ValidValue) {
            this.ctrl.setErrors({ "error": this.uiProperty.ValidationError });
        }
        else {
            this.ctrl.setErrors(null);

            if (hasminmax) {
                this.ctrl.validator = Validators.compose([Validators.minLength(minlength), Validators.maxLength(maxlenght)]);
            }
            else {
                this.ctrl.validator = null;
            }
        }

    }

    OnLogLovFocus() {
        this.ContainerClass = 'chosen-container chosen-container-single chosen-container-active';
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
        this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
        this.SelectedItemObject = this.SelectedItem;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DetectChanges();
        this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
        this.ValidateField();
        this.ToggleOpenDropDown();
        this.ValueChanged.emit(this.DataContext[this.ObjectFieldName]);
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
            this.SearchTextNgModel = "";
        }
        this.DetectChanges();
    }

    OnLogLovBlur() {
        this.ContainerClass = 'chosen-container chosen-container-single';
    }

    OnSearchIputKeyDown($event) {
        var TABKEY = 9;
        var ENTERKEY = 13;
        var DOWNKEY = 40;
        var UPKEY = 38;
        var ESC = 27;
        if ($event.keyCode == TABKEY) {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {
                var input = document.getElementById(this.ElementId);
                var lis = input.parentElement.parentElement.parentElement.getElementsByTagName("li");
                for (var i = 0; i < lis.length; i++) {

                    if (lis[i].className.search("highlighted") > -1) {
                        lis[i].classList.remove("highlighted");
                        lis[i].click();
                    }
                }

            }

            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }

        if ($event.keyCode == ENTERKEY) {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {
                var input = document.getElementById(this.ElementId);
                var lis = input.parentElement.parentElement.parentElement.getElementsByTagName("li");
                for (var i = 0; i < lis.length; i++) {

                    if (lis[i].className.search("highlighted") > -1) {
                        lis[i].classList.remove("highlighted");
                        lis[i].click();
                    }
                }

            }
        }

        if ($event.keyCode == DOWNKEY) {

            var active = document.getElementsByClassName("highlighted");
            if (!active[0]) {
                var input = document.getElementById(this.ElementId);
                var lis = input.parentElement.parentElement.parentElement.getElementsByTagName("li");
                lis[0].classList.add("highlighted");

            }
            else {
                active[0].nextElementSibling.classList.add("highlighted");
                active[0].classList.remove("highlighted");
                active[0].scrollIntoView(false);
            }

        }

        if ($event.keyCode == UPKEY) {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {

                active[0].previousElementSibling.classList.add("highlighted");
                active = document.getElementsByClassName("highlighted");
                active[1].classList.remove("highlighted");
                active[0].scrollIntoView(false);
            }
        }
        if ($event.keyCode == ESC) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
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

    Populate(searchText: string) {
        var filters = new ApiQueryFilters();
        filters.SortDirection = "Ascending";
        if (this.IsTenantZeroSearch) {
            filters.Tenant = 0;
        }
        if (searchText) {
            filters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null);
            //filters.Filter1Name = "SearchFields";
            //filters.Filter1Operator = "Contains";
            //filters.Filter1Value = searchText;
        }
        this.SetDependencyProperties(filters);
        filters.PageIndex = 0;
        filters.PageSize = 50;
        var lookup = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
        if (lookup.SortingByObjectField) {
            filters.SortBy = lookup.SortingByObjectField;
        }
        var inactiveField = window.ObjectFields.filter(d=> d.FieldName.toLowerCase() === "inactive" && d.ObjectTableId === lookup.Id)[0];
        if (inactiveField && !this.ShowInActive) {
            filters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null);

            //filters.Filter4Name = inactiveField.FieldName;
            //filters.Filter4Operator = "Equals";
            //filters.Filter4Value = false;
        }
        if (!lookup.CacheOnClient || this.IsTenantZeroSearch) {
            this.entityListService.getByFilters(this.LookUpTableName, filters).then((res:any)=> {
                res.subscribe(resp=> {
                    if (resp.Result) {
                        this.ItemsSource = resp.Result;
                    }
                    else {
                        this.ItemsSource = resp;

                    }
                    this.ItemsSourceStatic = this.ItemsSource;
                    this.DetectChanges();
                    //if (searchText != null) {
                    //this.ItemsSource = this.ItemsSource.filter(d=> d[this.DisplayMemberPath].toLowerCase().startsWith(searchText.toLowerCase()));
                    //}
                })
            });
        }
        else {
            this.entityListService.getAllFromCache(this.LookUpTableName, filters).then((res:any)=> {
                res.subscribe(resp=> {
                    if (resp.Result) {
                        this.ItemsSource = resp.Result;
                    }
                    else {
                        this.ItemsSource = resp;

                    }
                    this.ItemsSourceStatic = this.ItemsSource;
                    this.DetectChanges();
                    //if (searchText != null) {
                    //    this.ItemsSource = this.ItemsSource.filter(d=> d[this.DisplayMemberPath].toLowerCase().startsWith(searchText.toLowerCase()));
                    //}
                })
            });
        }

        if (this.IsAllDataVisible) {
            var tenantZeroFilters = new ApiQueryFilters();
            tenantZeroFilters.SortDirection = "Ascending";
            if (lookup.SortingByObjectField) {
                tenantZeroFilters.SortBy = lookup.SortingByObjectField;
            }

            tenantZeroFilters.Tenant = 0;
            if (searchText) {
                tenantZeroFilters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null);

                //tenantZeroFilters.Filter1Name = "SearchFields";
                //tenantZeroFilters.Filter1Operator = "Contains";
                //tenantZeroFilters.Filter1Value = searchText;
            }
            if (inactiveField && !this.ShowInActive) {
                tenantZeroFilters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null);

                //tenantZeroFilters.Filter4Name = inactiveField.FieldName;
                //tenantZeroFilters.Filter4Operator = "Equals";
                //tenantZeroFilters.Filter4Value = false;
            }
            this.SetDependencyProperties(tenantZeroFilters);
            tenantZeroFilters.PageIndex = 0;
            tenantZeroFilters.PageSize = 50;
            this.entityListService.getByFilters(this.LookUpTableName, tenantZeroFilters).then((res:any)=> {
                res.subscribe(resp=> {
                    if (resp.Result) {
                        this.ZeroItemsSource = resp.Result;
                    }
                    else {
                        this.ZeroItemsSource = resp;

                    }
                    this.DetectChanges();
                })
            });
        }

    }

    OnSearchInputBlur() {
        if (!this.MouseInArea) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }

    }

    OnMouseOver() {
        this.MouseInArea = true;
        if (!this.SelectedItem) {
            this.DeleteButtonNgStyle = { 'visibility': 'hidden' };
        } else {
            this.DeleteButtonNgStyle = null;
        }
        this.DetectChanges();
    }

    OnMouseOut() {
        this.MouseInArea = false;
        this.DetectChanges();
    }

    SetDependencyProperties(apiQueryFilters: ApiQueryFilters) {
        
        var table = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
        if (table.DependencyFilter1 != null && table.DependencyFilter1 != undefined) {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {
                var dependencyField: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === table.DependencyFilter1)[0];
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
                        dependencyField.DataTypeCode);
                    //apiQueryFilters.Filter2Name = dependencyField.FieldName;
                    //apiQueryFilters.Filter2Operator = fieldOperator;
                    //apiQueryFilters.Filter2Value = this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value);

                }
            }
        }

        if (table.DependencyFilter2 != null && table.DependencyFilter2 != undefined) {
            if (this.DependencyFilter2Value != null && this.DependencyFilter2Value != undefined) {
                var dependencyField2: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === table.DependencyFilter2)[0];
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
                        dependencyField2.DataTypeCode);

                    //apiQueryFilters.Filter3Name = dependencyField2.FieldName;
                    //apiQueryFilters.Filter3Operator = fieldOperator;
                    //apiQueryFilters.Filter3Value = this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value);

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
                    date = value as Date;
                    return date;
                }
            case "boolean":
                {
                    var bb: boolean;
                    bb = Boolean(value);
                    return bb;

                }
            case "integer":
            case "double":
            case "decimal":
                {
                    return value as number;
                }
            default:
                {
                    return value;
                }
        }
    }

    FocusMe() {
        var input = document.getElementById(this.ElementId);
        var logLovCtrl = input.parentElement.parentElement.parentElement.getElementsByTagName("a");
        logLovCtrl[0].focus();
    }
    
    OnAllDataSelect(item: any) {
       
        this.entityListService.getEntityCopyToCurrentTenant(item.Id, this.LookUpTableName).then((res:any) => {
            res.subscribe(myResponse => {
                if (myResponse != null) {

                    var list = myResponse;
                    if (myResponse instanceof ServiceResponse) {
                        list = myResponse.Result;
                    }

                    this.isSelectedFromList = true;
                    this.SelectedItem = list;
                    this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
                    this.SelectedItemObject = this.SelectedItem;
                    this.SelectedItemChanged.emit(this.SelectedItem);
                    this.DetectChanges();
                    this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
                    this.ToggleOpenDropDown();
                }
            })
        });

    }
    
    OnDropDownMouseOver() {
        this.MouseInArea = true;
        var input = document.getElementById(this.ElementId);
        input.focus();
    }

    OnDropDownMouseOut() {
        this.MouseInArea = false;
    }

    OnDeleteValue() {
      
        this.SelectedItem = null;
        this.DataContext[this.ObjectFieldName] = null;
        this.SelectedItemObject = null;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DetectChanges();
        this.DisplayValue = null;
        this.ValidateField();
        this.ValueChanged.emit(null);
    }

    SetValidity(validValue: boolean, errorMessage) {

        this.uiProperty.ValidValue = validValue;
        this.uiProperty.ValidationError = errorMessage;
        if (!validValue) {
            this.InputDivStyle = { 'border': '1px solid #ff0000' };
            if (this.show) {
                this.ShowErrorPopup = true;
            }
        }

        else {
            this.ShowErrorPopup = false;
            if (this.show) {
                this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
            }
            else {
                this.InputDivStyle = null;
            }
        }
    }

    SearchButtonClicked() {

        var ObjectTable = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0];
        var ObjectTableId = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0].Id;
        var args = new CustomEntityArgs();
        args.ObjectTableId = ObjectTableId;
        args.ObjectTableName = this.LookUpTableName;
        args.IsTenantZeroSearch = this.IsAllDataVisible;
        args.DependencyFilter1Value = this.DependencyFilter1Value;
        args.DependencyFilter1IsList = this.DependencyFilter1IsList;
        args.DependencyFilter1IsListExact = this.DependencyFilter1IsListExact;
        args.DependencyFilter2Value = this.DependencyFilter2Value;
        args.DependencyFilter2IsList = this.DependencyFilter2IsList;
        args.DependencyFilter2IsListExact = this.DependencyFilter2IsListExact;


        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural(this.LookUpTableName) + " Search";
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/LogSearchWindowComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnSearchWindowClosed($event));

    }

    OnSearchWindowClosed(args: any) {
        if (args && args != 'event') {  // No value returned
            var item = args.SelectedItem;
            this.isSelectedFromList = true;
            this.SelectedItem = item;
            this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
            this.SelectedItemObject = this.SelectedItem;
            this.SelectedItemChanged.emit(this.SelectedItem);
            this.DetectChanges();
            this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
            this.ValidateField();
            //this.ToggleOpenDropDown();
        }
    }

    ValidateField() {

        var errors = null;
        var table = window.ObjectTables.filter(d => d.Name === this.uiProperty.ObjectTableName)[0];
        if (table) {
            var field: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.uiProperty.FieldName)[0];
            var fieldValidator: FieldValidator = new FieldValidator();
            errors = fieldValidator.Validate(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        }
        if (errors) {
            if (errors.length > 0) {
                this.SetValidity(false, errors[0]);
            }
            else if (this.uiProperty.IsValidManually == false) {
                this.SetValidity(false, this.uiProperty.ManualValidationError);
            }
            else {
                this.SetValidity(true, null);
            }

        }
        else if (this.uiProperty.IsValidManually == false) {
            this.SetValidity(false, this.uiProperty.ManualValidationError);
        }
        else {
            this.SetValidity(true, null);
        }
        this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
    }

    OnFocus() {
        this.Detach = false;
        this.DetectChanges();
        this.show = true;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
            this.ShowErrorPopup = false;
        }
        else {
            this.InputDivStyle = { 'border': '1px solid #ff0000' };
            this.ShowErrorPopup = true;
        }
    }

    OnBlur() {
        this.Detach = true;
        this.DetectChanges();
        this.show = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
    }
    Detach: boolean;
    DetectChanges() {
        return;

    }
   
}
