declare var window: any;
declare var System: any;
import {Component, OnInit, OnDestroy, Input, Output, EventEmitter, AfterViewInit} from '@angular/core';
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
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { CardExtendedPMService } from 'Common/Services/ExtendedPMs/CardExtendedPMService';

const localLanguageCode = 'L';
const englishLanguageCode = 'E';
const AccountingPartnerTypeCode = "AC";
@Component({


    selector: 'LogSearchWindow',
    templateUrl: './LogSearchWindowComponent.html',
    providers: [ServiceArgs, EntityListService, EntityPMService],
    inputs: ['DependencyFilter1Value', 'DependencyFilter2Value', 'DependencyFilter3Value',
        "DependencyFilter1IsList", "DependencyFilter2IsList", "DependencyFilter3IsList",
        "DependencyFilter1IsListExact", "DependencyFilter2IsListExact", "DependencyFilter3IsListExact",
            "IsTenantZeroSearch", "ShowInActive"],
})

export class LogSearchWindowComponent extends BaseComponent implements OnInit, OnDestroy {

    @Output() SearchFieldchangeevent = new EventEmitter();
    @Output() ItemSelected = new EventEmitter();

    private _entityListService: EntityListService
    private entityPMService: EntityPMService
    public SearchText: string = "Search";
    public DataContext: LogSearchWindowComponent = this;
    public ObjectTableName: string;
    public ObjectField: any;
    public ObjectTableId: string;
    public columns: any[] = [];
    public columns1: any[] = [];
    public ObjectFields: any[] = [];
    public AddButtonVisibility: boolean = false;
    public TenantPM: TenantPM;
    public items: any[] = [];
    public Args: CustomEntityArgs = new CustomEntityArgs();
    public DependencyFilter1Value: Object;
    public DependencyFilter2Value: Object;
    public DependencyFilter3Value: Object;
    public DependencyFilter1IsList: boolean;
    public DependencyFilter2IsList: boolean;
    public DependencyFilter3IsList: boolean;
    public DependencyFilter1IsListExact: any;
    public DependencyFilter2IsListExact: any;
    public DependencyFilter3IsListExact: any;
    public IsTenantZeroSearch: boolean = null;
    public IsAllDataVisible: boolean = null;
    public IsMyDataVisible: boolean = true;
    public UseCompactSearch: boolean;
    public searchFields: string;
    public searchText: string;
    public ShowInActive: boolean = false;
    private PartnerTypes: Array<any> = [];
    ObjectTableNamePluralName: string = '';
    ObjectTable: ObjectTablePM;
    public ParentTableName: string;
    public QueryFilterItems: ApiQueryFilters;
    public SourceEntityPM: any;

    public IsDataReady = true;
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
    DisplayLocalFieldsFromList: string = null;
    PseventRowSelectEventSub: any;
    private CurrentSession = SessionLocator.SelectedSession;
    DontApplyVirtualization: boolean = false;
    ConstantPageSize: number = 100;
    UsingLogGridV2: boolean = false;
    SearchFieldName: string = null;
    LanguageFilterValue: string;
    @Input() ForceShowLanguageFilter: boolean = false;
    @Input() ForceShowLocalAndEnglishColumns: boolean = false;
    ObjectFieldCode: string;
    public get ShowLanguageFilter(): boolean
    {
        return  SessionLocator?.LoggedUserPM?.ShowLocalNameInLOV
            && SessionLocator?.TenantPM?.AccountingActivated;
    }


    constructor() {
        super();
        this._entityListService = new EntityListService;
        this.entityPMService = new EntityPMService;
        this.TenantPM = InfraSettings.TenantPM;
        //this.CurrentSession.SubscriptionAdd(
        var UsingV2FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LV2")[0];
        if (UsingV2FeatureToggle || SessionLocator.LoggedUserPM.Email == "ahmada@logitudeworld.com") { this.UsingLogGridV2 = true; }

        this.PseventRowSelectEventSub=  this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
                if (res == this.ObjectTableName) {
                    this.preventSelect = true;
                }
            })
        //);
    }

    ngOnInit() {
        this.ParentTableName = this.GetObjectTableName(this.ObjectTableName);
        this.BuildColumns();
    }

    ngOnDestroy() {
        this.PseventRowSelectEventSub.unsubscribe();
        //this.CurrentSession.PseventRowSelectEvent.unsubscribe(); // this line commented, it cause object unsubscribed error
    }

    SetWindowArgs(args: CustomEntityArgs) {
        this.ObjectTableName = args.ObjectTableName; // lookup table
        this.ObjectField = args.ObjectField;
        this.ObjectTableId = args.ObjectTableId;
        this.ObjectTableNamePluralName = TextCodeTranslator.TranslateTablePlural(this.ObjectTableName);
        this.ShowInActive = args.ShowInActive;
        this.PartnerTypes = args.PartnerTypes;
        this.Args = args;
        this.IsTenantZeroSearch = args.IsTenantZeroSearch;
        this.IsAllDataVisible = args.IsAllDataVisible;
        this.DependencyFilter1Value = args.DependencyFilter1Value;
        this.DependencyFilter2Value = args.DependencyFilter2Value;
        this.DependencyFilter3Value = args.DependencyFilter3Value;
        this.DependencyFilter1IsList = args.DependencyFilter1IsList;
        this.DependencyFilter2IsList = args.DependencyFilter2IsList;
        this.DependencyFilter3IsList = args.DependencyFilter3IsList;
        this.DependencyFilter1IsListExact = args.DependencyFilter1IsListExact;
        this.DependencyFilter2IsListExact = args.DependencyFilter2IsListExact;
        this.DependencyFilter3IsListExact = args.DependencyFilter3IsListExact;
        this.UseCompactSearch = args.UseCompactSearch;
        this.ObjectTable = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];
        this.QueryFilterItems = args.QueryFilterItems;
        this.HideAdd = args.HideAdd;
        this.IsAddDisabled = args.IsAddDisabled;
        this.IsEditDisabled = args.IsEditDisabled;
        this.SourceEntityPM = args.EntityPM;
        this.ObjectFieldCode = args.ObjectFieldCode;
        this.SearchFieldName = args.SearchFieldName;
        
        if (this.IsUseCardSearchMechanism()) {
            this.DontApplyVirtualization = true;
        }

        if (!this.IsAddDisabled) {
            this.IsAddBtnVisible = true;
            if (this.ObjectTableName == "Warehouse") {
                if (this.TenantPM.Id != 0 && this.TenantPM.CountryCode == "US") {
                    this.IsAddUSWarehouseVisible = true;
                }

                else if (FeatureLocator.HasFeaturePermession("Warehouse", "AddWarehouses")) {
                    this.IsAddUSWarehouseVisible = true;
                }
            }
        }

        this.DisplayFieldsFromList = args.DisplayFieldsFromList;
        this.DisplayLocalFieldsFromList = args.DisplayLocalFieldsFromList;
        this.LanguageFilterValue = args.LanguageFilterValue;
        this.ForceShowLanguageFilter = args.ForceShowLanguageFilterOnSearchWindow;
        this.ForceShowLocalAndEnglishColumns = args.ForceShowLocalAndEnglishColumns;

        if (this.IsTenantZeroSearch) {
            this.IsAllDataVisible = true;
            this.IsMyDataVisible = false;
        }

        this.LookUpTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];

        if (this.ObjectTableName == "Card" || this.ObjectTableName == "Carrier")
        {
            if (this.DependencyFilter1Value)
            {
                if (this.DependencyFilter1Value.toString().split(',').length > 1)
                {
                    //* there is more than one dep

                    //  show toggle button and filter list by dep fields
                    var types: string[] = this.DependencyFilter1Value.toString().split(',');
                    this.LovPartnerTypes = this.PartnerTypes.filter(d => types.indexOf(d.Id) > -1);
                    this.LovPartnerTypes = this.LovPartnerTypes.filter(d => d.Id != "CC" && d.Id != "CO" && d.Id != "FL" && d.Id != "OT" && d.Id != "PO" && d.Id != "PT");
                    if (!this.IsAddDisabled) {
                        this.IsAddToggleVisible = true;
                        this.IsAddBtnVisible = false;
                    }



                }
                else
                {
                    //* there is only one dep

                    //show single add button
                    if (!this.IsAddDisabled)
                        this.IsAddBtnVisible = true;
                }

            }
            else
            {
                // no dependency, show all types
                this.LovPartnerTypes = this.PartnerTypes;
                this.LovPartnerTypes = this.LovPartnerTypes.filter(d => d.Id != "CC" && d.Id != "CO" && d.Id != "FL" && d.Id != "OT" && d.Id != "PO" && d.Id != "PT");
                if (!this.IsAddDisabled) {
                    this.IsAddToggleVisible = true;
                    this.IsAddBtnVisible = false;
                }
                
                if(!this.TenantPM.AccountingActivated)
                    this.LovPartnerTypes = this.LovPartnerTypes.filter(d => d.Id != AccountingPartnerTypeCode);

            }

        }

        else
        {
            if (!this.IsAddDisabled) {
                this.IsAddBtnVisible = true;

                if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString()) && !SessionLocator.LoggedUserPM.IsCustomerCare) {
                    if (this.ObjectTableName == "ChargesType") {
                        this.IsAddBtnVisible = false;
                    }
                }
            }

            else {
                this.IsAddBtnVisible = false;
            }
        }
    }


    IsReady: boolean = false;
    onCountReady(event) {

        this.IsReady = true;
    }







    BuildColumns() {

        var objectTableId = this.ObjectTableId;
        var lookupFields: any[];
        if (this.DisplayFieldsFromList != null && this.DisplayFieldsFromList != undefined) {
            var fields: string[] = this.DisplayFieldsFromList.split(',');
            lookupFields = window.ObjectFields.filter(d => d.ObjectTableId == this.LookUpTable.Id && fields.lastIndexOf(d.FieldName) > -1);
        }
        else {
            lookupFields = window.ObjectFields.filter(d => d.DisplayInSearchWindowList && d.ObjectTableId == this.LookUpTable.Id);
        }

        if(this.ForceShowLocalAndEnglishColumns){
            this.AddLocalNameColumn(lookupFields);
        }

        this.ObjectFields = lookupFields.sort((a, b) => { return a.DisplayInSearchWindowListIndex - b.DisplayInSearchWindowListIndex });

        this.columns1 = [];
        this.columns = [];

        for (var i = 0; i < this.ObjectFields.length; i++) {
            this.columns1.push({
                FieldName: this.ObjectFields[i].FieldName,
                IsCustomTemplate: true,
                DataTypeCode: this.ObjectFields[i].DataTypeCode,
                Display: TextCodeTranslator.Translate(this.ObjectFields[i].ListTextCodeCode),
                Styles: { width: '100px' },
                HtmlListComponentName: this.ObjectFields[i].HtmlListComponentName,
                HtmlListComponentUrl: this.ObjectFields[i].HtmlListComponentUrl,
                ColumnHeaderTemplateName: this.ObjectFields[i].ColumnHeaderTemplateName,
                ServerSideSortable: true,
            });
            this.columns.push({
                FieldName: this.ObjectFields[i].FieldName,
                IsCustomTemplate: true,
                DataTypeCode: this.ObjectFields[i].DataTypeCode,
                Display: TextCodeTranslator.Translate(this.ObjectFields[i].ListTextCodeCode),
                Styles: { width: '100px' },
                HtmlListComponentName: this.ObjectFields[i].HtmlListComponentName,
                HtmlListComponentUrl: this.ObjectFields[i].HtmlListComponentUrl,
                ColumnHeaderTemplateName: this.ObjectFields[i].ColumnHeaderTemplateName,
                ServerSideSortable: true,
            });
        }

        ////Edit Buttons
        // my == columns1
        if (this.IsEditDisabled == false) {
            this.columns1.push({
                FieldName: 'EditBtn,' + this.ObjectTableName,
                DataTypeCode: 'String',
                IsCustomTemplate: true,
                Display: '',
                Styles: { width: '100px' },
                HtmlListComponentName: 'LogSearchWindowButtonsComponent',
                HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/LogSearchWindowButtonsComponent',
                ServerSideSortable: true,
            });
        }
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event == 'ok from LSWBC') {
                this.SearchFieldchangeevent.emit("");
            }
        });



    }

    private AddLocalNameColumn(lookupFields: any[])
    {
        let localNameColumn = window.ObjectFields.filter(d => d.FieldName == 'LocalName' && d.ObjectTableId == this.LookUpTable.Id);

        if(localNameColumn){
            let englishNameColumnIndex = lookupFields.findIndex(d => d.FieldName.includes('EnglishName'));
            AppendLocalNameBesideEnglishName(lookupFields, englishNameColumnIndex, localNameColumn);
        }
    }

    IsUseCardSearchMechanism() {
        var result: boolean = false;
        if (this.ObjectTableName == "Card") {

                result = true;

        }
        return result;

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
        filters = this.SetDependencyProperties(filters);
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = this.TenantPM.Id;


        if (filters.AdditionalFilters.filter(a => a.FieldName == this.GetSearchFieldName()).length > 0 || filters.AdditionalFilters.filter(a => a.FieldName == "CardSearchField").length > 0) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != this.GetSearchFieldName());
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "CardSearchField");

        }

        if (this.IsUseCardSearchMechanism()) {
            filters.DontApplyVirtualization = this.DontApplyVirtualization;
            filters.PageSize = this.ConstantPageSize;
        }




        if (searchfields) {//&& !this.UseCompactSearch
            if (this.IsUseCardSearchMechanism()) {
              filters.addAdditionalFilter("CardSearchField", searchfields, null, null, "Contains", false, false, false, null);
            } else filters.addAdditionalFilter(this.GetSearchFieldName(), searchfields, null, null, "Contains", false, true, false, "String");

        }






        var parentName = this.GetObjectTableName(this.ObjectTableName);
        var parenttable = window.ObjectTables.filter(d => d.Name === parentName)[0];
        var originalTable = this.ObjectTable;
        if (originalTable.Name == "Carrier") {
            originalTable = window.ObjectTables.filter(d => d.Name === "Card")[0];
        }
        var inactiveField = window.ObjectFields.filter(d => d.FieldName.toLowerCase() === "inactive" && (d.ObjectTableId === parenttable.Id || d.ObjectTableId == originalTable.Id))[0];
        if (inactiveField && !this.ShowInActive) {
            filters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null);
        }
        //var x = filters.AdditionalFilters.filter(a => a.FieldName == "InActive");
        //if (x.length == 0) {
        //    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, true, false, "Boolean");
        //}

        var rowsObjectTable = this.ObjectTableName;
        //if (this.UseCompactSearch) {
        //    filters.addAdditionalFilter("CompactSearchField", searchfields, null, null, "Contains", false, false, false, null);
        //    return this._entityListService.getByCompactFilters(rowsObjectTable, filters);
        //}
        //else {
        filters = this.FillTreeFilterDetails(filters);
        if (rowsObjectTable == "Card"){
                    
            return this.cardExtendedPMService.getByCompactFilters(rowsObjectTable, filters);

        }
        return this._entityListService.getByFilters(rowsObjectTable, filters);
        //}
    }


    FillTreeFilterDetails(filters) {
        let objectField = window.ObjectFields.filter(f => f.Id == this.ObjectField?.Id)[0];
        if (!objectField) objectField = window.ObjectFields.filter(f => f.FieldCode == this.ObjectFieldCode)[0];

        filters.TreeFilters = objectField ? objectField.DefaultAdditionalFilters : this.ObjectField?.DefaultAdditionalFilters;
        filters.ParentEntityId = this.GetParentEntityId();
        filters.ParentEntity = this.GetParentEntity();
        filters.ParentObjectTableName = this.ObjectField?.ObjectTableName;
        return filters;
    }

    GetParentEntityId() {
        let parentEntityId = SessionLocator?.SelectedSession?.CurrentEditComponent?.EntityId;
        return parentEntityId ? parentEntityId : null;
    }

    GetParentEntity() {
        let entityPM = SessionLocator?.SelectedSession?.CurrentEditComponent?.EntityPM;
        if (!entityPM) entityPM = this.SourceEntityPM;
        if (!entityPM) return null;

        let objectField = window.ObjectFields.filter(f => f.Id == this.ObjectField?.Id)[0];
        if (!objectField) objectField = window.ObjectFields.filter(f => f.FieldCode == this.ObjectFieldCode)[0];

        let objectFieldAdditionalTreeFilters = objectField ? objectField.DefaultAdditionalTreeFilters : this.ObjectField?.DefaultAdditionalTreeFilters;
        if (!objectFieldAdditionalTreeFilters) return null;

        var parentEntity = {};
        return this.RestoreFilters(objectFieldAdditionalTreeFilters, parentEntity, entityPM);
    }

    RestoreFilters(BaseFilter: any, parentEntity: any, entityPM: any) {
        BaseFilter.QueryFilterItems?.forEach((field) => {
            this.RestoreQueryFilterViewItem(field, parentEntity, entityPM);
        });

        return JSON.stringify(parentEntity);
    }

    private RestoreQueryFilterViewItem(field: any, parentEntity: any, entityPM: any) {
        if (!field) return;

        if (field.FieldName?.indexOf('.') > -1) {
            let fieldName = field.FieldName.split('.')[1];
            parentEntity[fieldName] = entityPM[fieldName];
        }

        if (field.Operator?.indexOf('Field') > -1 && field.FieldValue?.indexOf('.') > -1) {
            let fieldName = field.FieldValue.split('.')[1];
            parentEntity[fieldName] = entityPM[fieldName];
        }

        if (!field.QueryFilterItems) {
            return;
        }

        if (field.QueryFilterItems.length == 0) {
            return;
        }

        this.RestoreFilters(field, parentEntity, entityPM);
    }

    //#endregion

    //#region Tenent Zero Data Source

    public rowCount2: number; //tenent 0
    DataSource2 = {
        pageSize: 20,
        rowCount2: null,
        //sortingCol: "CreateDateTime",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows2(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };
    private cardExtendedPMService:CardExtendedPMService=new CardExtendedPMService()

    //tenent 0
    getRows2(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        //if (filters == null) {
        if (this.QueryFilterItems != null && this.QueryFilterItems != undefined) {
            filters = this.QueryFilterItems;
        }
        else {
            filters = new ApiQueryFilters();
        }
        //}
        filters = this.SetDependencyProperties(filters);
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = 0;
        if (filters.AdditionalFilters.filter(a => a.FieldName == this.GetSearchFieldName()).length > 0) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != this.GetSearchFieldName());
        }
        if (searchfields) {//&& !this.UseCompactSearch
            filters.addAdditionalFilter(this.GetSearchFieldName(), searchfields, null, null, "Contains", false, true, false, "String");
        }
        var parentName = this.GetObjectTableName(this.ObjectTableName);
        var parenttable = window.ObjectTables.filter(d => d.Name === parentName)[0];
        var inactiveField = window.ObjectFields.filter(d => d.FieldName?.toLowerCase() === "inactive" &&( d.ObjectTableId === parenttable.Id || d.ObjectTableId === this.ObjectTable.Id))[0];

        if (inactiveField && !this.ShowInActive) {
            filters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null);
        }
        //var x = filters.AdditionalFilters.filter(a => a.FieldName == "InActive");
        //if (x.length == 0) {
        //    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, true, false, "Boolean");
        //}

        var rowsObjectTable = this.ObjectTableName;

        //if (this.UseCompactSearch) {
          //  filters.addAdditionalFilter("CompactSearchField", searchfields, null, null, "Contains", false, false, false, null);
          //  return this._entityListService.getByCompactFilters(rowsObjectTable, filters);
       // }
        //else {
            filters = this.FillTreeFilterDetails(filters);
            if (rowsObjectTable == "Card"){
                    
                return this.cardExtendedPMService.getByCompactFilters(rowsObjectTable, filters);
    
            }
            return this._entityListService.getByFilters(rowsObjectTable, filters);
      //  }
    }

    //#endregion

    //#region Dependency
    SetDependencyProperties(apiQueryFilters: ApiQueryFilters) {


        var table = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
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

        if (table.DependencyFilter3 != null && table.DependencyFilter3 != undefined) {
            if (this.DependencyFilter3Value != null && this.DependencyFilter3Value != undefined) {
                var dependencyField: ObjectFieldPM = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === table.DependencyFilter3)[0];
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
                        dependencyField.DataTypeCode);
                }
            }
        }

        return apiQueryFilters;
    }
    private GetObjectTableName(parentObjectName: string) {
        var dep: string = this.DependencyFilter1Value != null ? this.DependencyFilter1Value.toString() : "";
        return this.GetObjectTableNameForDependency(dep, parentObjectName);
    }
    private GetObjectTableNameForDependency(dependency: string, parentObjectName: string) {
        var partnerType = this.PartnerTypes.filter(p => p.Id.toLowerCase() == dependency.toLowerCase())[0];
        if (partnerType != null && partnerType != undefined) {
            var name: string = partnerType.Name.replace(" ", "");
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

    //#endregion

    //#region Buttons + Handlers

    AddButtonClicked() {
        //if (this.isAddDisabled)
        //    return;

        this.NewEntityMethod(this.ObjectTableName);
    }
    NewEntityMethod(objectTableName) {

        objectTableName = this.GetObjectTableName(objectTableName);
        var originalTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name.toLowerCase() === objectTableName.toLocaleLowerCase())[0];

        if (!FeatureLocator.HasFeaturePermession(this.ObjectTableName, "NEW") && this.LookUpTable.EnableSecurity) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("You have no permission to add a new entity of this type.");
            return;
        }

        if (!FeatureLocator.HasFeaturePermession(this.ObjectTableName, "Module") && this.LookUpTable.EnableSecurity) {
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
                    var tablename = this.GetObjectTableName(this.ObjectTableName);
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

        //var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];


        var componentPath: string = newWizardComponentPath;//this.LookUpTable.NewWizardComponentPath;

        if (componentPath != null) {

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;

            switch (this.ObjectTableName) {
                case "Customs.Client": {
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
        this.entityPMService.getNewEntity(this.ObjectTableName).then(response => {

            var args = new AddEntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = this.ObjectTableName;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.Translate(this.ObjectTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewEntityWindowClosed($event));
            logWindow.Show(componentPath);
        });
    }
    private OnNewEntityWindowClosed($event: any) {
        console.log($event);
        if ($event && $event != "event") {
            this._entityListService.getSingle($event, this.ObjectTableName).then((res: any) => {
                res.subscribe(myResponse => {
                    if (myResponse != null) {

                        // Refresh
                        this.SearchFieldchangeevent.emit("");

                        if (this.LookUpTable.CacheOnClient) {
                            CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                        }                       
                        //var list = myResponse;
                        //if (myResponse instanceof ServiceResponse) {
                        //    list = myResponse.Result;
                        //}

                        //this.SearchTextNgModel = list[this.DisplayMemberPath];
                        //this.OldSearchInput = this.SearchTextNgModel;
                        //this.DisplayValue = list[this.DisplayMemberPath];
                        //this.SelectedItem = list;
                        //this.SelectedItemObject = this.SelectedItem;
                        //this.SelectedItemChanged.emit(this.SelectedItem);
                        //this.selectedValue = this.SelectedItem[this.SelectedValuePath];
                        //this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
                        //this.ValidateField();
                    }
                })
            });
        }
    }


    CloseButtonClicked() {
        //this.CurrentSession.CloseCurrentWindow();
        if(this.QueryFilterItems){
            this.QueryFilterItems.removeAdditionalFilterForNoneLookUpfilter("CardSearchField");
        }
        
        this.CurrentSession.CloseCurrentWindowEmit(null);
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
    onRowSelected($event) {
        if (this.preventSelect == false) {
            if ($event != null) {
                var entityList = $event.rowData;
                var selectedEntityId = $event.rowData[this.ObjectTable.KeyPropertyPath];
                //console.log("row clicked : ", entityList, selectedEntityId);
                this.Args.SelectedItem = entityList;
                var args = selectedEntityId + ',' + entityList.Tenant + ',' + this.LanguageFilterValue;
                // Close windoew with Args
                if(this.QueryFilterItems){
                    this.QueryFilterItems.removeAdditionalFilterForNoneLookUpfilter("CardSearchField");
                }
                this.CurrentSession.CloseCurrentWindowEmit(args);
            }
        }
        else {
            this.preventSelect = false;
        }
    }
    AddPartnerOfType(partnerType: any) {
        var objectTableName = this.GetObjectTableNameForDependency(partnerType.Id, this.ObjectTableName);
        var objectTable: ObjectTablePM = window.ObjectTables.filter(d => d.Name == objectTableName)[0];
        if (objectTable.IsNewWizard) {
            this.RunNewEntityWizard(objectTable.NewWizardComponentPath, objectTable.Name);
        }
        else {
            this.RunNewGenaricEntity();
        }
    }
    //#endregion

    AddUSWarehouseClicked() {
        var windowTitle = "Add " + this.ObjectTableName;
        var logWindow = new LogitudeWindow();
        var args: ImportEntityArgs = new ImportEntityArgs();
        args.ObjectTableId = this.ObjectTable.Id;
        args.ObjectTableName = this.ObjectTableName;
        logWindow.WindowArgs = args;
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');

        logWindow.WindowClosed.subscribe(($event: any) => {
            this.TextChanged(this.searchFields);
        });
    }

    @Output() columnsReadyEvent = new EventEmitter();
    SwitchLanguage(languageCode: string) {
        this.LanguageFilterValue = languageCode;
        this.BuildColumns();
        this.columnsReadyEvent.emit({Columns: this.columns1});

    }

    GetSearchFieldName() {
        if (this.SearchFieldName)
            return this.SearchFieldName;
        return "SearchFields";
    }
}

export class CustomEntityArgs {
    public ObjectTableName: string = null;
    public ObjectField: any = null;
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
    public DisplayLocalFieldsFromList: string = null;
    public LanguageFilterValue: string;
    public HideEdit: boolean;
    public ShowLanguageFilter: boolean;
    public ForceShowLanguageFilterOnSearchWindow: boolean;
    public ForceShowLocalAndEnglishColumns: boolean;
    public EntityPM: any = null;
    public ObjectFieldCode: string = null;
    public SearchFieldName: string = null;
}
export class AddEntityArgs {
    public EntityPM: any;
    public ObjectTableName: string;
}
function AppendLocalNameBesideEnglishName(lookupFields: any[], englishNameColumnIndex: number, localNameColumn: any)
{
    lookupFields.splice(englishNameColumnIndex, 0, localNameColumn[0]);
}

