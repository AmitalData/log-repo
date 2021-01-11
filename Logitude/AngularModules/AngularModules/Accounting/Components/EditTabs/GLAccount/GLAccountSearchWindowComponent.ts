import {Component, Output, EventEmitter, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
declare var window: any;
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';


@Component({
    
    templateUrl: './GLAccountSearchWindowComponent.html',

})


export class GLAccountSearchWindowComponent extends BaseComponent implements OnInit

{
    ObjectTableName: string = "GLAccount";
    ObjectTableId: string = window.ObjectTables.filter(f => f.Name === this.ObjectTableName)[0].Id;
    DataContext: any = this;
    @Output() onQueryChangeEvent = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    entityListService: EntityListService = new EntityListService();
    @Output() MenuHeaderchangeevent = new EventEmitter();
    EntityPM: GLAccountPM;
    gLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    SelectedRow: any;
    ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
       
    }
    ngOnInit() {
        this.BuildColumns();
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        //this.MenuHeaderchangeevent.emit({ Filters: new ApiQueryFilters() , IgnoreFilter: false });

    }
    SetWindowArgs(args) {
        this.EntityPM = args.GLAccount;
    }
    DataSource = {
        pageSize: 30,
        rowCount: null,
       
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };
    searchFields: string = null;
    TextChanged(searchtext) {
        this.searchFields = searchtext;
        if (!AppTool.IsNullOrEmpty(searchtext)){
            this.searchFields = this.searchFields.trim();
            if (this.searchFields != null && this.searchFields != undefined)
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });

        }
        else {
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }

    }
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        var filters = new ApiQueryFilters;

        filters.GetAll = true;
        filters.GetCount = true;
        if (!AppTool.IsNullOrEmpty(this.searchFields)) {
            filters.addAdditionalFilter("SearchFields", this.searchFields, null, null, "Contains", false, false, false, "string");
         }
          filters.addAdditionalFilter("ParentAccountId", "22", null, null, "IsNull", false, false, false, "string");
        filters.addAdditionalFilter("Inactive", false, null, null, "Equals", false, false, false, "boolean");
       // if (this.EntityPM.ParentAccountId != null) {
            filters.addAdditionalFilter("IsParent","11", null, null, "Equals", true, false, false, "string");
       // }
        filters.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "Exclude", false, false, false, "string");
        filters.addAdditionalFilter("AccountTypeCode", this.EntityPM.AccountTypeCode, null, null, "Equals", false, false, false, "string");
       filters.addAdditionalFilter("ChartOfAccountsId", this.EntityPM.ChartOfAccountsId, null, null, "Equals", false, false, false, "string");

       
       return this.entityListService.getByFilters("GLAccount", filters);
    }
    OnRowSelected($event) {
        this.ValidationErrorsList = [];
            if ($event != null) {
                var entityList = $event.rowData;
                //console.log("row clicked : ", entityList, selectedEntityId);
               // var args = entityList.DisplayNumber + ',' + entityList.Id;
                this.gLAccountExtendedListService.SetParentAccountId(entityList.Id, this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {

                    if (myResponse) {
                        if (!myResponse.HasError) {
                            this.CurrentSession.CloseCurrentWindowEmit(entityList);
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        }
                        else
                        {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                     
                    }

                });

              
            }
        
        
    }
    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'DisplayNumber',
            DataTypeCode: 'string',
            Display: TextCodeTranslator.Translate('GLAccount.F.DisplayNumber'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
        
        });
        this.columns.push({
            FieldName: 'EnglishName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('GLAccount.F.EnglishName'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,

        });
        this.columns.push({
            FieldName: 'LocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('GLAccount.F.LocalName'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,

        });
        this.columns.push({
            FieldName: 'CurrencyCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('GLAccount.F.CurrencyCode'),
            IsCustomTemplate: true,
            Styles: { width: '80px' },
           
        });
        this.columns.push({
            FieldName: 'RevenueExpenseName',
            DataTypeCode: 'string',
            Display: TextCodeTranslator.Translate('GLAccount.F.RevenueExpenseName'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
        });

        this.columns.push({
            FieldName: 'ChartOfAccountsName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('GLAccount.F.ChartOfAccountsName'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
           
        });



    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
