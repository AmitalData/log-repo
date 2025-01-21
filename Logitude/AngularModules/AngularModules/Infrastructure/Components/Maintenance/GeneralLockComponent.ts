import { Component, Output, EventEmitter, OnInit, ComponentRef} from '@angular/core';
import { EntityArgs } from '../../DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../Tools';
import { FeatureLocator } from '../../Utilities/FeatureLocator';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { LogTab } from '../LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';
import { CustomsDocumentsDefinitionPM } from '../../../Customs/EntityPMs/CustomsDocumentsDefinitionPM';
import { BaseComponent } from '../LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../Utilities/ObservableCollection';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { CustomsDocumentsDefinitionListService } from '../../../Customs/Services/StandardLists/CustomsDocumentsDefinitionListService';
import { CustomsDocumentsDefinitionPMService } from '../../../Customs/Services/StandardPMs/CustomsDocumentsDefinitionPMService';
import { CustomsDocumentsDefinitionExtendedService } from '../../../Customs/Services/ExtendedPMs/CustomsDocumentsDefinitionExtendedService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import {ApiQueryFilters} from '../../DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../Services/EntityResourceService';
import { GeneralLockService } from 'Infrastructure/Services/ExtendedLists/GeneralLockService';
import { GeneralLockPM } from 'Infrastructure/EntityPMs/GeneralLockPM';
import { GeneralLockPMService } from 'Infrastructure/Services/StandardPMs/GeneralLockPMService';
import { EntityListService } from 'Infrastructure/Services/EntityListService';


@Component({
    
    templateUrl: './GeneralLockComponent.html',
})

export class GeneralLockComponent extends BaseComponent implements OnInit {
    public DataContext: GeneralLockComponent = this;
    public ObjectTableName: string = "GeneralLock";
    public columns: any[] = null;
    public generalLockPM: GeneralLockPM = new GeneralLockPM();
    public ToDateText: string = TextCodeTranslator.Translate("Accounting.General.O.Between");

    private _entityResourceService: EntityResourceService = new EntityResourceService();


    @Output()
    MenuHeaderchangeevent = new EventEmitter();
    @Output()
    onQueryChangeEvent = new EventEmitter();

    private _entityListService: EntityListService =  new EntityListService();;
    private CurrentSession = SessionLocator.SelectedSession;
    CurrentQueryFilters: ApiQueryFilters;
    private SessionEvent: any = null;


    constructor() {
        super();
    }
    _IsLoaded: boolean = false;
    ngOnInit() {
        
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
        
            
            this.BuildColumns();
            this.RefreshBtnClick()
            this._IsLoaded = true;

        });
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(res => {
            if (res == "IsGeneralLockChanged") {
                this.RefreshBtnClick()

            }
        });
    
    }
    
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'CreatedAt',
            DataTypeCode: 'Date',
            Display: TextCodeTranslator.Translate("GeneralLock.F.CreatedAt"),
            Styles: { width: '150px' },
            HtmlListComponentName: 'GeneralLockListTemplate',
            HtmlListComponentUrl: './Infrastructure/Components/Templates/GeneralLockListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CreatedAt'
        });

        this.columns.push({
            FieldName: 'UserName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("GeneralLock.F.UserName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'UserName'
        });
        this.columns.push({
            FieldName: 'EntityId1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("GeneralLock.F.EntityId1"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'EntityId1'
        });
        this.columns.push({
            FieldName: 'ObjectTableName1',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("GeneralLock.F.ObjectTableName1"),
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ObjectTableName1'
        });
        this.columns.push({
            FieldName: 'EntityId2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("GeneralLock.F.EntityId2"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'EntityId2'
        });
        this.columns.push({
            FieldName: 'ObjectTableName2',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("GeneralLock.F.ObjectTableName2"),
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ObjectTableName2'
        });
        this.columns.push({
            FieldName: 'Delete',
            DataTypeCode: 'String',
            Styles: { width: '30px' },
            HtmlListComponentName: 'GeneralLockListTemplate',
            HtmlListComponentUrl: './Infrastructure/Components/Templates/GeneralLockListTemplate',
            IsCustomTemplate: true,
        });
    }

    
    DataSource = {

        pageSize: 10,
        rowCount: null,
        sortingCol: "CreatedAt",// "Id",
        sortingDir: "Descending",//"Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };
    filterAgrs: ApiQueryFilters;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

            filters = new ApiQueryFilters();

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        

        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        if (!AppTool.IsNullOrEmpty(this.UserId)) {

            filters.addAdditionalFilter("UserId", this.UserId, null, null, "Equals", false, false, false, "string");
        }
        if (!AppTool.IsNullOrEmpty(this.EntityId1)) {

            filters.addAdditionalFilter("EntityId1", this.EntityId1, null, null, "Equals", false, false, false, "string");
        }
        if (!AppTool.IsNullOrEmpty(this.ObjectTableId1)) {

            filters.addAdditionalFilter("ObjectTableId1", this.ObjectTableId1, null, null, "Equals", false, false, false, "LookUp");
        }
        if (!AppTool.IsNullOrEmpty(this.EntityId2)) {

            filters.addAdditionalFilter("EntityId2", this.EntityId2, null, null, "Equals", false, false, false, "string");
        }
        if (!AppTool.IsNullOrEmpty(this.ObjectTableId2)) {

            filters.addAdditionalFilter("ObjectTableId2", this.ObjectTableId2, null, null, "Equals", false, false, false, "LookUp");
        }
        if (!AppTool.IsNullOrEmpty(this.FromDate) && !AppTool.IsNullOrEmpty(this.ToDate)) {
            filters.addAdditionalFilter("CreatedAt", this.FromDate, this.ToDate, null, "Between", true, false, false, "DateTime");
        }

        
        this.CurrentSession.StartBusyIndicator("");

        var myout = this._entityListService
            .getByFilters("GeneralLock", filters);
        myout.then(res => {
            this.CurrentSession.StopBusyIndicator();

        });

        return myout;

    }

    public get UserId() { return this.generalLockPM.UserId; }
    public set UserId(newValue: string) { this.generalLockPM.UserId = newValue; }

    public get EntityId1() { return this.generalLockPM.EntityId1; }
    public set EntityId1(newValue: string) { this.generalLockPM.EntityId1 = newValue; }

    public get ObjectTableId1() { return this.generalLockPM.ObjectTableId1; }
    public set ObjectTableId1(newValue: string) { this.generalLockPM.ObjectTableId1 = newValue; }

    public get EntityId2() { return this.generalLockPM.EntityId2; }
    public set EntityId2(newValue: string) { this.generalLockPM.EntityId2 = newValue; }

    public get ObjectTableId2() { return this.generalLockPM.ObjectTableId2; }
    public set ObjectTableId2(newValue: string) { this.generalLockPM.ObjectTableId2 = newValue; } 

    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
        }
    }
    private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
        }
    }
    
   
  
  
    RefreshBtnClick() {
         setTimeout(() => {
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });

         }, 10);
    }

    ClearFilter(){
        this.UserId = null;
        this.EntityId1 = null;
        this.ObjectTableId1 = null;
        this.EntityId2 = null;
        this.ObjectTableId2 = null;
        this.FromDate = null;
        this.ToDate = null;
        this.RefreshBtnClick();
    }

   
}
