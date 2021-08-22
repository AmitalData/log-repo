declare var window: any;
import { Component, Output ,EventEmitter, OnInit, ComponentRef} from '@angular/core';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';


import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { CustomsClosedTableList } from '../../../Customs/EntityLists/CustomsClosedTableList';
import { SystemTableRequestParams } from '../../../Customs/DataContract/RequestParams/SystemTableRequestParams';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomMessageProgressComponent, CustomMessageProgressHelper } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

@Component({
    
    templateUrl: './CustomsClosedTablesComponent.html',
})


export class CustomsClosedTablesComponent implements OnInit {
    public DataContext: CustomsClosedTablesComponent = this;
    public ObjectTableName: string = "Customs.CustomsClosedTable";
    public columns: any[] = null;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public ComponentRef: ComponentRef<CustomsClosedTablesComponent>;

    BackBtnTitle: string;
    IsShowTipArea: boolean;

    _stratSearch: boolean = true;
    @Output()
    MenuHeaderchangeevent = new EventEmitter();
    @Output()
    onQueryChangeEvent = new EventEmitter();

    private _entityListService: EntityListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this._entityListService = new EntityListService();

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            //try {
            this._entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe((response:any) => {
                //this._IsLoaded = true;
            });
            //} catch (err) {
            //    console.warn(err);
            //}
        });
    }
    _IsLoaded: boolean = false;
    ngOnInit() {
        //this._entityListService = new EntityListService();

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            //try {
            this._entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe((response:any) => {
                this._IsLoaded = true;
            });
            //} catch (err) {
            //  console.warn(err);
            //}



            this.BuildColumns();
            this.RefreshBtnClick()

        });
    }
    BackButtonClicked() {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
        }


    }
    private _Existed: boolean = true;
    public get Existed() { return this._Existed; }
    public set Existed(val: boolean) {
        if (this._Existed == val) return;
        this._Existed = val;
        this.RefreshBtnClick();
    }
    

    BuildColumns() {
        this.columns = [];
        this.columns.push({
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
          HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',

            FieldName: 'Id',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsClosedTable.O.Id"),
            Styles: { width: '75px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',

            FieldName: 'CustomsName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsClosedTable.F.CustomsName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',

            FieldName: 'CustomsLocalName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsClosedTable.F.CustomsLocalName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            
            FieldName: 'LastUpdateDate',
            DataTypeCode: 'Date',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsClosedTable.F.LastUpdateDate"),
            Styles: { width: '145px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',

        });
        this.columns.push({
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
            FieldName: 'StatusName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsClosedTable.F.StatusName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TableUpdateButton',
            DataTypeCode: 'String',//'Number',
            Display: "",
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
            

        });
        this.columns.push({
            FieldName: 'ShowDetailsButton',
            DataTypeCode: 'String',//'Number',
            Display: "",
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
            

        });
        
    }
    DataSource = {

        pageSize: 10,
        rowCount: null,
        //SortData("RequestCreateDate", "Descending", false, false);
        sortingCol: "" ,// "Id",
        sortingDir: "" ,//"Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };
    filterAgrs: ApiQueryFilters;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {



        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;

        filters.SortBy = "Id";
        filters.SortDirection = "Descending";

        filters.SortBy = "CustomsName";//"Id";
        filters.SortDirection = "Descending";//"Descending";

        
        if (this._Existed) {
            filters.addAdditionalFilter("Existed", this._Existed, null, null, "Equals", false, false, false, "boolean");
        }
        if (!AppTool.IsNullOrEmpty(this._SearchText)) {
            
            filters.addAdditionalFilter("SearchFields", this._SearchText, null, null, "Contains", false, false, false, "string");
        }
       /// filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");

        var myout = this._entityListService
            .getExtendedByFilters("Customs.CustomsClosedTable", filters);
        myout.then(res => {
            this._stratSearch = false;
            //this.CurrentSession.StopBusyIndicator();
        });

        return myout;

    }
   
    _SearchText: string;
    onSearchTextChangeEvent(text: string) {
        this._SearchText = text;
        this.RefreshBtnClick();
    }

    ItemClicked(item: CustomsClosedTableList) {



    }
    RefreshBtnClick() {
        this._stratSearch = true;
        //this.CurrentSession.StartBusyIndicator("");
        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);
    }
    UpdateAll() {
        let systemTableRequestParams = new SystemTableRequestParams();
        //systemTableRequestParams.TableId = this._CustomsClosedTable.Id;
        systemTableRequestParams.Tenant = SessionLocator.Tenant;
        //systemTableRequestParamsystemTableRequestParams.s.RequestVIA == SendRequestVIA.WebServiceBatch;//all the time 
        systemTableRequestParams.UpdateAllTables = true;

        var myCustomMessageProgressHelper = new CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(systemTableRequestParams.PBId, 5, true);
        
        
        
        //CustomMessageProgressComponent
        //    .ShowProgressBar(this.CurrentSession,systemTableRequestParams.PBId, "שליחת שאילתא להודעות בוקר", true)
        //    .then((res) => {
        //        //this.ResponseData = res;
        //        //this.OnMassageDisplayMethod();
        //    }
        //    ).catch((err) => {
        //        //this.ValidationErrorsList.push(err);
        //    });
        let myIIGGeneralMessagesService = new IIGGeneralMessagesService();
        myIIGGeneralMessagesService.PostUpdateClosedTables(systemTableRequestParams).subscribe(
            res => {
                myCustomMessageProgressHelper.MessageArrived = true;        
                this.CurrentSession.StopBusyIndicator();
            }
        )

    }

}
