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

    constructor() {
        super();
        this._entityListService = new EntityListService;
        this.entityPMService = new EntityPMService;
        this.TenantPM = InfraSettings.TenantPM;
        //SessionLocator.CurrentSession.SubscriptionAdd(
        this.PseventRowSelectEventSub=  SessionLocator.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
                if (res == this.ObjectTableName) {
                    this.preventSelect = true;
                }
            })
        //);
    }

    ngOnInit() {
        //this.ParentTableName = this.GetObjectTableName(this.ObjectTableName);
        this.BuildColumns();
        //this.ColumnsReady.emit("");
    }

    ngOnDestroy() {
        this.PseventRowSelectEventSub.unsubscribe();
        //SessionLocator.CurrentSession.PseventRowSelectEvent.unsubscribe(); // this line commented, it cause object unsubscribed error
    } 

    SetWindowArgs(args: CustomEntityArgs) {
        this.ObjectTableName = args.ObjectTableName; // lookup table
        this.ObjectFieldName = args.DisplayFieldsFromList;
        this.Args = args;
       
    }

    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'Name', 
            DataTypeCode: 'text',
            Display: this.ObjectFieldName.replace('[', '').replace(']',''),
            Styles: { width: '250px' },  
            IsCustomTemplate: true,
            HtmlListComponentName: 'DWLogSearchWindowFieldsComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/DWLogSearchWindowFieldsComponent',
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
                var selectedEntity = $event.rowData["Name"];
                
                SessionLocator.CurrentSession.CloseCurrentWindowEmit(selectedEntity);
            }
        }
        else {
            this.preventSelect = false;
        }
    }

    CloseButtonClicked() {
        //SessionLocator.CurrentSession.CloseCurrentWindow();
        SessionLocator.CurrentSession.CloseCurrentWindowEmit(null);

    }

}

export class CustomEntityArgs {
    public ObjectTableName: string = null;
    public ObjectTableId: string = null;
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
    
}
export class AddEntityArgs {
    public EntityPM: any;
    public ObjectTableName: string;
}