declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef} from '@angular/core';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';


import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { InterfaceManagementList } from '../../../Customs/EntityLists/InterfaceManagementList';
import { SystemTableRequestParams } from '../../../Customs/DataContract/RequestParams/SystemTableRequestParams';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomMessageProgressComponent, CustomMessageProgressHelper } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

@Component({
    
    templateUrl: './InterfaceManagementComponent.html',
})


export class InterfaceManagementComponent implements OnInit {
    public DataContext: InterfaceManagementComponent = this;
    public ObjectTableName: string = "Customs.InterfaceManagement";
    InterfaceManagement
    public columns: any[] = null;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public ComponentRef: ComponentRef<InterfaceManagementComponent>;

    BackBtnTitle: string;
    IsShowTipArea: boolean;

    _stratSearch: boolean = true;
    @Output()
    MenuHeaderchangeevent = new EventEmitter();
    @Output()
    onQueryChangeEvent = new EventEmitter();

    private _entityListService: EntityListService;
    constructor() {
        this._entityListService = new EntityListService();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            
        });
    }
    _IsLoaded: boolean = false;
    ngOnInit() {
        
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
        
                this._IsLoaded = true;
        


            this.BuildColumns();
            this.RefreshBtnClick()

        });
    }
    BackButtonClicked() {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
        }


    }
    

    BuildColumns() {
        this.columns = [];
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
          HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',

            FieldName: 'Code',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.InterfaceManagement.F.Code"),
            Styles: { width: '75px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',

            FieldName: 'Description',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.InterfaceManagement.F.Description"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',

            FieldName: 'InOut',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.InterfaceManagement.F.InOut"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
        });
        this.columns.push({

            FieldName: 'SendOptionsCode',
            DataTypeCode: 'Date',//'Number',
            Display: TextCodeTranslator.Translate("Customs.InterfaceManagement.F.DefaultSendOptionsCode"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',

        });

        this.columns.push({

            FieldName: 'DefaultPriority',
            DataTypeCode: 'Date',//'Number',
            Display: TextCodeTranslator.Translate("Customs.InterfaceManagement.F.DefaultPriority"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',

        });
        
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
            FieldName: 'HasDefinition',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.InterfaceManagement.F.HasDefinition"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
            FieldName: 'SignatureTypeName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.InterfaceManagement.F.SignatureTypeName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        let textInterfaceTypeName = TextCodeTranslator.Translate("Customs.InterfaceManagement.F.InterfaceTypeName");
        if (AppTool.IsNullOrEmpty(textInterfaceTypeName)) {
            textInterfaceTypeName = "InterfaceTypeName";
        }

        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
            FieldName: 'InterfaceTypeName',
            DataTypeCode: 'String',//'Number',
            Display: textInterfaceTypeName,
            Styles: { width: '120px' },
            IsCustomTemplate: true
        });

 
    }
    DataSource = {

        pageSize: 10,
        rowCount: null,
        //SortData("RequestCreateDate", "Descending", false, false);
        sortingCol: "",// "Id",
        sortingDir: "",//"Descending",
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

        filters.SortBy = "Code";
        filters.SortDirection = "Ascending";//"Descending";

        //filters.SortBy = "CustomsName";//"Id";
        //filters.SortDirection = "Descending";//"Descending";


        
        if (!AppTool.IsNullOrEmpty(this._SearchText)) {

            filters.addAdditionalFilter("SearchFields", this._SearchText, null, null, "Contains", false, false, false, "string");
        }
        /// filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");

        var myout = this._entityListService
            //.getExtendedByFilters("Customs.InterfaceManagement", filters);
            .getByFilters("Customs.InterfaceManagement", filters);
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

    ItemClicked(item: InterfaceManagementList) {



    }
    onRowSelected(selected) {
        let item: InterfaceManagementList = selected.rowData;
        let window = new LogitudeWindow();
        

        let windowTitle = TextCodeTranslator.Translate("Customs.General.O.EditInterfaceManagement");
        let logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { SelectedItem: item };
        logWindow.Width = 750;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        //logWindow.Show('./Customs/Components/Maintenance/AddEditInterfaceManagementComponent');
        logWindow.Show('./CustomsModules/CustomsMaintenance/Components/AddEditInterfaceManagementComponent');

    }
    RefreshBtnClick() {
        this._stratSearch = true;
        //this.CurrentSession.StartBusyIndicator("");
        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);
    }


}
