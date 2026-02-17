declare var window: any;
declare var System: any;
import {Component, OnInit, OnDestroy, Input, Output, EventEmitter, AfterViewInit} from '@angular/core';
//import {NgForm, NgStyle, NgFormControl, CORE_DIRECTIVES, FORM_DIRECTIVES,  FormBuilder, ControlGroup, Validators, Control} from '@angular/common';
//import {Http, HTTP_PROVIDERS, Response} from '@angular/http';
import {Http, Response} from '@angular/http';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LogGridComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {IconButton} from '../../../Controls/IconButton';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {PartnersDomainService} from '../../../Common/Services/PartnersDomainService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {SearchTextBox} from '../../../Controls/SearchTextBox';
import {ObjectFieldPM} from '../../EntityPMs/ObjectFieldPM';
import {ObjectTablePM} from '../../EntityPMs/ObjectTablePM';
import {FeatureLocator} from '../../Utilities/FeatureLocator';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import { NewEntityArgs} from '../../Args';
import {ImportEntityArgs} from '../../../Common/Components/Maintenance/TenantImportComponent';
import {CachedDataManager} from '../../Utilities/CachedDataManager';
import {MultiSelectedValue, ValueDetails} from '../../../CommonModules/CommonOthers/Components/DWQueryBuilder/DWQueryBuilderComponent';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {ComponentArgs} from '../../../Infrastructure/DataContracts/ComponentArgs';
import {ParameterComponentArgs} from '../../../Infrastructure/DataContracts/ParameterComponentArgs';
import { filter } from 'rxjs/operators';
;

@Component({
    moduleId: module.id,

    selector: 'DWLogSearchWindow',
    templateUrl: './DWLogSearchWindowComponent.html',
    providers: [Http, ServiceArgs, EntityListService, EntityPMService],
})

export class DWLogSearchWindowComponent extends BaseComponent implements OnInit, OnDestroy {

    @Output() SearchFieldchangeevent = new EventEmitter();
    @Output() ItemSelected = new EventEmitter();
   
    private _entityListService: EntityListService
    private entityPMService: EntityPMService
    public SearchText: string = "Search";
    public DataContext: DWLogSearchWindowComponent = this;
    public ObjectTableName: string;
    public ObjectFieldName: string;
    public LOVAdditionalColumns: string;
    public ObjectTableId: string;
    public columns: any[] = [];
    
    public ObjectFields: any[] = [];
    public AddButtonVisibility: boolean = false;
    public TenantPM: TenantPM;
    public items: any[] = [];
    public Args: CustomEntityArgs = new CustomEntityArgs();
   
    public searchFields: string;
    public searchText: string;
    public ShowInActive: boolean = false;
    private PartnerTypes: Array<any> = [];
    ObjectTableNamePluralName: string = '';
    ObjectTable: ObjectTablePM;
    public ParentTableName: string;
    public QueryFilterItems: ApiQueryFilters;

   
    HideAdd: boolean;
    IsAddDisabled: boolean = true;
    IsEditDisabled: boolean = true;
    preventSelect: boolean = false;
    //ObjectTable: ObjectTablePM;
    LookUpTable: ObjectTablePM;
    LovPartnerTypes: Array<any> = [];
    IsAddToggleVisible: boolean = false;
    IsAddBtnVisible: boolean = true;
    IsAddUSWarehouseVisible: boolean = false;
    DisplayFieldsFromList: string = null;
    PseventRowSelectEventSub: any;

    SecondListHeaderItems: string[] = [];
    SecondListValueItems: MultiSelectedValue[] = [];
    MultiSelectedValueLists: MultiSelectedValue[] = [];
    private ViewModel: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this._entityListService = new EntityListService;
        this.entityPMService = new EntityPMService;
        this.TenantPM = InfraSettings.TenantPM;




        this.PseventRowSelectEventSub=  this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
                if (res == this.ObjectTableName) {
                    this.preventSelect = true;
                }
        })


        this.CurrentSession.SessionEvent.subscribe((res) => {

            if (res && res.ComponentName == "DWLogSearchAddFieldsComponent" && res.IsFirstRequest && res.Item) {
                var item = res.Item;
                res.IsFirstRequest = false;
                var newItem = new MultiSelectedValue();
              
                var key = "";
                var i = 0;

                if (!AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
                    if (ComponentArgs && ComponentArgs.ComponentLists) {
                        var sessionkey: string = this.CurrentSession.Sessionkey + "DWLogSearchWindow";
                        var Component = ComponentArgs.ComponentLists.filter(d => d.key == sessionkey)[0];
                        if (Component) {
                            var myComponent = Component.Component;
                            if (myComponent) {
                                myComponent.SecondListHeaderItems.forEach((field) => {
                                    if (i != 0) key = i.toString();
                                    var valueDetails: ValueDetails = new ValueDetails();
                                    valueDetails.Header = field;
                                    valueDetails.Row = item["Field" + key];
                                    newItem["Value" + key] = valueDetails;
                                    i += 1;
                                });

                                myComponent.SecondListValueItems.push(newItem);
                            }
                        }
                    }
                }

            }


        });

   
    }

   

    ngOnInit() {
        //this.ParentTableName = this.GetObjectTableName(this.ObjectTableName);
        this.BuildColumns();
        //this.ColumnsReady.emit("");
    }

    ngOnDestroy() {
        this.PseventRowSelectEventSub.unsubscribe();
        //this.CurrentSession.PseventRowSelectEvent.unsubscribe(); // this line commented, it cause object unsubscribed error
    } 

    SetWindowArgs(args: CustomEntityArgs) {
        if (AppTool.IsNullOrEmpty(this.CurrentSession.Sessionkey)) {
            this.CurrentSession.Sessionkey = Guid.newGuid();
        }


        ComponentArgs.AddComponent(new ParameterComponentArgs(this.CurrentSession.Sessionkey + "DWLogSearchWindow", this));

        this.ObjectTableName = args.ObjectTableName; // lookup table
        this.ObjectFieldName = args.DisplayFieldsFromList;
        this.LOVAdditionalColumns = this.BuildAdditionalColumns(args.LOVAdditionalColumns);
        this.ViewModel = args.DataContext; 

        if (this.ViewModel) {
            this.MultiSelectedValueLists = this.ViewModel.MultiSelectedValueLists;
        }

        if (!this.MultiSelectedValueLists) {
            this.MultiSelectedValueLists = [];
        }

        this.SecondListHeaderItems = [];
        this.SecondListValueItems = [];

        this.Args = args;

        
        this.BuildSecondListHeader();
        this.BuildSecondListValues();

    }


    BuildAdditionalColumns(columns:string) {
        var result = "";
        if (columns) {
            var headerLists: string[]= [];
            var additionalColumns = columns.split(',');
            if (additionalColumns.length > 0) {
                additionalColumns.forEach((field) => {
                    if (field != this.ObjectFieldName) {
                        if (!headerLists.filter(d => d == field)[0]) {
                            headerLists.push(field);
                        }
                    }
                });

                headerLists.forEach((field) => {
                    result += field + ",";
                });

                result += "@";
                result = result.replace(",@", "").replace("@","");

            }
        }

        return result;

    }

    BuildColumns() {
        this.columns = [];
        var AdditionalColumns = [];
        if (this.LOVAdditionalColumns) {
            AdditionalColumns = this.LOVAdditionalColumns.split(',');
        }
       
        this.columns.push({
            FieldName: 'Field', 
            DataTypeCode: 'text',
            Display: this.ObjectFieldName.replace('[', '').replace(']',''),
            Styles: { width: '120px' },  
            IsCustomTemplate: true,
            HtmlListComponentName: 'DWLogSearchWindowFieldsComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/DWLogSearchWindowFieldsComponent',
        });
        if (AdditionalColumns.length > 0) {
            var index = 1;
            AdditionalColumns.forEach((field) => {
                this.columns.push({
                    FieldName: 'Field' + index,
                    DataTypeCode: 'text',
                    Display: field.replace('[', '').replace(']', ''),
                    Styles: { width: '120px' },
                    IsCustomTemplate: true,
                    HtmlListComponentName: 'DWLogSearchWindowFieldsComponent',
                    HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/DWLogSearchWindowFieldsComponent',
                });
                index++;
            });
        }

        this.columns.push({
            FieldName: "Add",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '40px' },
            HtmlListComponentName: 'DWLogSearchAddFieldsComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/DWLogSearchAddFieldsComponent',
        });

   
    }
    
    TextChanged(searchtext) {
        if (searchtext != null && searchtext != undefined) {
            this.searchFields = searchtext;
            this.searchFields = this.searchFields.trim();
            if (this.searchFields != null && this.searchFields != undefined)
                this.SearchFieldchangeevent.emit(this.searchFields);
        }
        else {
            this.SearchFieldchangeevent.emit("");
        }


    }

    //#region My Data
    

    public rowCount: number;
    DataSource = {
        pageSize: 20,
        rowCount: null,
        //sortingCol: "CreateDateTime",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
       
        //if (filters == null) {
       
        if (this.QueryFilterItems != null && this.QueryFilterItems != undefined) {
            filters = this.QueryFilterItems;
        }
        else {
            filters = new ApiQueryFilters();
        }
        //}

        filters.Filter1Name = this.ObjectTableName;
        filters.Filter2Name = this.ObjectFieldName;
        filters.Filter3Name = this.LOVAdditionalColumns;
        if (this.LOVAdditionalColumns) {
            filters.Filter3Name = this.LOVAdditionalColumns;
        }
        //else {
        //    filters.Filter3Name = null;
        //}
        if (searchfields) {
            filters.Filter2Value = searchfields;
        }
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.ObjectTableName = this.ObjectTableName; 
        //if (filters.AdditionalFilters.filter(a => a.FieldName == "SearchFields").length > 0) {
        //    filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
        //}
        //if (searchfields) {
        //    filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, true, false, "String");
        //}
                
        return this._entityListService.getDWDimByFilters(this.ObjectTableName, filters);
       
    }

    onRowSelected($event) {
        if (this.preventSelect == false) {
            if ($event != null) {
                var entityList = $event.rowData;
                var selectedEntity = $event.rowData["Field"];
                
               // this.CurrentSession.CloseCurrentWindowEmit(selectedEntity);
            }
        }
        else {
            this.preventSelect = false;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    OkButtonClicked() {

        var textValue = this.GetTextValue(this.SecondListValueItems);
    
        if (this.ViewModel) {
            this.ViewModel.MultiSelectedValueLists = this.SecondListValueItems;
        }
        this.CurrentSession.CloseCurrentWindowEmit(textValue);

    }

    
    BuildSecondListHeader() {

        var test: MultiSelectedValue[] = [];

        if (this.ObjectFieldName) {
            this.SecondListHeaderItems.push(this.ObjectFieldName.replace('[', '').replace(']', ''));
        }

        var additionalColumns = [];
        if (this.LOVAdditionalColumns) {
            additionalColumns = this.LOVAdditionalColumns.split(',');
            if (additionalColumns.length > 0) {
                additionalColumns.forEach((field) => {
                    this.SecondListHeaderItems.push(field.replace('[', '').replace(']', ''));
                });
            }

        }
        

        if (this.MultiSelectedValueLists && this.MultiSelectedValueLists.length >0) {
            var items = this.MultiSelectedValueLists[0];
            var i = "";
            var j = 0;
            while (items["Value" + i]) {
                var columnName = items["Value" + i].Header;
                if (!this.SecondListHeaderItems.filter(d => d == columnName)[0]) {
                    this.SecondListHeaderItems.push(columnName);
                }

                j += 1;
                i = j.toString();

            }

           
        }



    }

    BuildSecondListValues() {
        this.SecondListValueItems = [];

        this.MultiSelectedValueLists.forEach((item) => {
            var multiSelectedValue: MultiSelectedValue = new MultiSelectedValue();
            var i = "";
            var j = 0;
            this.SecondListHeaderItems.forEach((header) => {
                var valueDetails: ValueDetails = new ValueDetails();
                valueDetails.Header = header;
                valueDetails.Row = this.ResolveValue(item, header);
                multiSelectedValue["Value" + i] = valueDetails;

                j += 1;
                i = j.toString();
            });
            this.SecondListValueItems.push(multiSelectedValue);

        });
    }

    ResolveValue(Values: MultiSelectedValue, header: string) {
        var i = "";
        var j = 0;
        var result = "";
        while (Values["Value" + i]) {
            if (Values["Value" + i].Header == header) {
                result = Values["Value" + i].Row;
                return result;
            }

            j += 1;
            i = j.toString();

        }

        return result;
    }

    GetTextValue(multiSelectedValueLists: MultiSelectedValue[] ) {
        var textValue = "";
        if (multiSelectedValueLists) {
            multiSelectedValueLists.forEach((field) => {
                if (field["Value"]) {
                    if (textValue) textValue += ";;";
                    var rowValues: any = field["Value"];

                    if (rowValues) textValue += rowValues.Row;

                }

            });
        }

        textValue += "@@";
        textValue = textValue.replace(";@@","");
        textValue = textValue.replace("@@", "");

        return textValue;
    }

    RemoveItemFromSecondList(item: any) {
        if (item != null) {
            var index = this.SecondListValueItems.indexOf(item);
            if (index > -1) {
                this.SecondListValueItems.splice(index, 1);

            }
        }
    }

}

export class CustomEntityArgs {
    public ObjectTableName: string = null;
    public ObjectTableId: string = null;
    public LOVAdditionalColumns: string = null;
    public SelectedItem: any = null;
    public ShowInActive: boolean = false;
    public IsTenantZeroSearch: boolean = null;
    public IsAllDataVisible: boolean = null;
    public DependencyFilter1Value: Object;
    public DependencyFilter2Value: Object;
    public DependencyFilter3Value: Object;
    public DependencyFilter1IsList: boolean;
    public DependencyFilter2IsList: boolean;
    public DependencyFilter3IsList: boolean;
    public DependencyFilter1IsListExact: any;
    public DependencyFilter2IsListExact: any;
    public DependencyFilter3IsListExact: any;
    public PartnerTypes: Array<any> = [];
    public UseCompactSearch: boolean;
    public QueryFilterItems: ApiQueryFilters;
    public HideAdd: boolean;
    public IsAddDisabled: boolean = true;
    public IsEditDisabled: boolean = true;
    public DisplayFieldsFromList: string = null;
    public HideEdit: boolean;
    public DataContext: any;
}
export class AddEntityArgs {
    public EntityPM: any;
    public ObjectTableName: string;
}
