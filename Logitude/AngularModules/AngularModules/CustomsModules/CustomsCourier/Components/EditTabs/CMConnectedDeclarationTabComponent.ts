import {Component, EventEmitter, Output} from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CourierMasterService} from '../../../../Customs/Services/Others/CourierMasterService';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CourierMasterPM} from '../../../../Customs/EntityPMs/CourierMasterPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { CourierMasterValidator } from '../../../../Customs/Validators/CourierMasterValidator';
import { CustomsRequestsSheetPM } from '../../../../Customs/EntityPMs/CustomsRequestsSheetPM';

@Component({
    moduleId: module.id,
    templateUrl: './CMConnectedDeclarationTabComponent.html',
})


export class CMConnectedDeclarationTabComponent extends BaseComponent {
    CourierMasterService: CourierMasterService = new CourierMasterService();
    ObjectTableName: string = "Customs.CourierMaster";
    DataContext: any = this;
    entityPM: CourierMasterPM;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() MenuHeaderchangeevent1 = new EventEmitter();
    IsVisibile: boolean;
    connectedListIds: ObservableCollection;
    notConnectedListIds: ObservableCollection;

    public CurrentEditComponentId: string;

    private _ConnectedSearch: string;
    private _NotConnectedSearch: string;

    _CourierMasterValidator: CourierMasterValidator = new CourierMasterValidator();
    public IsDisplayOnly: boolean = false;
    public DisplayOnlyMessage: string = "";

    private EntityResourceService: EntityResourceService;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityResourceService = new EntityResourceService();
        this.entityPM = entityArgs.EntityPM;
        this.connectedListIds = new ObservableCollection([]);
        this.notConnectedListIds = new ObservableCollection([]);
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(response => {
             
                this.IsVisibile = true;
                this.BuildColumns();
                this.BuildColumns1();
                this.LoadConnectedItems();
                this.DisplayOnlyCheck();
                this.Listen();
            });
        });
    }


    get ConnectedSearch() { return this._ConnectedSearch; }
    set ConnectedSearch(value: string) {
        if (this._ConnectedSearch != value) {
            this._ConnectedSearch = value;
        }
    }

    get NotConnectedSearch() { return this._NotConnectedSearch; }
    set NotConnectedSearch(value: string) {
        if (this._NotConnectedSearch != value) {
            this._NotConnectedSearch = value;
        }
    }

    private _TotalConnected: number = 0;
    get TotalConnected() { return this.DataSource != null ? this.DataSource.rowCount : 0; }
    set TotalConnected(value: number) {
        if (this._TotalConnected != value) {
            this._TotalConnected = value;
        }
    }

    private Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = SessionLocator.CurrentSession.CurrentEditComponent.ComponentId;
            SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    this.LoadConnectedDeclarationGrid();
                    this.LoadNotConnectedDeclarationGrid();
                }
            });

            SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    //this.BuildColumns();
                    this.LoadConnectedItems();
                }
            });

            SessionLocator.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                if (this.CurrentEditComponentId == SessionLocator.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "COCD") {

                    }
                }
            });
        }
    }
    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: "MyConnectedCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'CourierConnectedDeclarationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierConnectedDeclarationListTemplate',
        });

        this.columns.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.CustomFileNo"),
            Styles: { width: '80px' },

            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierConnectedDeclarationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierConnectedDeclarationListTemplate',

        });

        this.columns.push({
            FieldName: 'CourierHAWB',
            DataTypeCode: 'String',
            Display: "שטר מטען בלדר",
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'DeclarationNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.DeclarationNumber"),
            Styles: { width: '120px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'CustomerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.CustomerName"),
            Styles: { width: '250px' },
            IsCustomTemplate: true
        });



        this.columns.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.DeclarationStatusTypeCode"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });

    }

    public columns1: any[] = null;
    BuildColumns1() {
        this.columns1 = [];

        this.columns1.push({
            FieldName: "MyNotConnectedCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'CourierConnectedDeclarationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierConnectedDeclarationListTemplate',
        });

        this.columns1.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.CustomFileNo"),
            Styles: { width: '80px' },

            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierConnectedDeclarationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierConnectedDeclarationListTemplate',

        });

        this.columns1.push({
            FieldName: 'CourierHAWB',
            DataTypeCode: 'String',
            Display: "שטר מטען בלדר",
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });

        this.columns1.push({
            FieldName: 'DeclarationNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.DeclarationNumber"),
            Styles: { width: '120px' },
            IsCustomTemplate: true
        });

        this.columns1.push({
            FieldName: 'CustomerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.CustomerName"),
            Styles: { width: '250px' },
            IsCustomTemplate: true
        });

        this.columns1.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.Declaration.F.DeclarationStatusTypeCode"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });
       
    }


    DataSource = {

        pageSize: 10,
        rowCount: null,
       
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },

    };
    filterAgrs: ApiQueryFilters;

    LoadConnectedItems() {
       this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
       
    }

    DataSource1 = {

        pageSize: 10,
        rowCount: null,

        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows1(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;


        },

    };

    ViewInitCompleted($event) {
        this.LoadConnectedDeclarationGrid();
    }

    ConnectedSearchCompleted(searchText: any) {
        this.ConnectedSearch = searchText;
        this.LoadConnectedDeclarationGrid();
    }

    NotConnectedSearchCompleted(searchText: any) {
        this.NotConnectedSearch = searchText;
        this.LoadNotConnectedDeclarationGrid();
    }

    ViewInitCompleted1($event) {
        this.LoadNotConnectedDeclarationGrid();
    }

    LoadConnectedDeclarationGrid() {
        this.filterAgrs = new ApiQueryFilters();
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    LoadNotConnectedDeclarationGrid() {
        this.filterAgrs = new ApiQueryFilters();
        this.MenuHeaderchangeevent1.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
        if (!AppTool.IsNullOrEmpty(this.ConnectedSearch)) {
            filters.addAdditionalFilter("CourierSearchFields", this.ConnectedSearch, null, null, "Contains", false, false, false, "string");
        }
        return this.CourierMasterService.getPromiseByFilters(filters);

    }

    getRows1(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        if (!AppTool.IsNullOrEmpty(this.NotConnectedSearch)) {
            filters.addAdditionalFilter("CourierSearchFields", this.NotConnectedSearch, null, null, "Contains", false, false, false, "string");
        }

        return this.CourierMasterService.getPromiseByFilters1(filters);

    }

    onCheckBoxChecked($event) {
 
        if (!this.entityPM.NotConnectedDeclarations) {
            this.entityPM.NotConnectedDeclarations = "";
        }
        if (!$event.IsChecked) {
            if (!this.entityPM.NotConnectedDeclarations.includes($event.rowData.Id)) {
                this.notConnectedListIds.Collection.push($event.rowData.Id);
                this.entityPM.NotConnectedDeclarations = this.entityPM.NotConnectedDeclarations + $event.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.NotConnectedDeclarations.includes($event.rowData.Id)) {
     
                this.entityPM.NotConnectedDeclarations = this.entityPM.NotConnectedDeclarations.replace($event.rowData.Id+",", "");
            }
        }
      
    }
    onCheckBoxChecked1($event) {
        if (!this.entityPM.ConnectedDeclarations) {
            this.entityPM.ConnectedDeclarations = "";
        }
        if ($event.IsChecked) {
            if (!this.entityPM.ConnectedDeclarations.includes($event.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations + $event.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.ConnectedDeclarations.includes($event.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations.replace($event.rowData.Id+",", "");
            }
        }
    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = false;

        //Check if changing StorageSiteCode
        this._CourierMasterValidator.SetEntityPM(this.entityPM);
        this._CourierMasterValidator.CheckRequestInProgressForCourierMaster(this.entityPM.Tenant, "UCBCMSS", this.entityPM.Id).subscribe((response: any) => {
            var displayOnlyCheckResult = response.Result;
            if (displayOnlyCheckResult != null && displayOnlyCheckResult.length > 0) {
                let customsRequestsSheetPM: CustomsRequestsSheetPM = displayOnlyCheckResult.filter(r => r.InterfaceTypeCode == "UCBCMSS")[0];
                if (customsRequestsSheetPM != null) {
                    this.IsDisplayOnly = true;
                    this.DisplayOnlyMessage = "לתצוגה בלבד - קיימת בקשה לשינוי אתר איחסון ברקע ";
                }
            }
        });
    }

    RefreshEntity() {
        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.DisplayOnlyCheck();
    }

}
