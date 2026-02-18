import {Component, EventEmitter, Output} from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CourierMasterPM} from '../../../../Customs/EntityPMs/CourierMasterPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { CourierMasterValidator } from '../../../../Customs/Validators/CourierMasterValidator';
import { CustomsRequestsSheetPM } from '../../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

@Component({
    
    templateUrl: './CMConnectedDeclarationTabComponent.html',
    providers: [CourierMasterService]
})


export class CMConnectedDeclarationTabComponent extends BaseComponent {
  public SelectedRow2: any;

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
    status: string;

    constructor(public entityArgs: EntityArgs, public CourierMasterService: CourierMasterService) {
        super();
        this.EntityResourceService = new EntityResourceService();
        this.entityPM = entityArgs.EntityPM;
        this.connectedListIds = new ObservableCollection([]);
        this.notConnectedListIds = new ObservableCollection([]);
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe((response: any) => {

                    this.IsVisibile = true;
                    this.OnAllConnectedChecked(true);
                    this.BuildColumns();
                    this.BuildColumns1();
                    this.LoadConnectedItems();
                    this.DisplayOnlyCheck();
                    this.Listen();
                });
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


    private _TotalDisconnected: number = 0;
    get TotalDisconnected() { return this.DataSource1 != null ? this.DataSource1.rowCount : 0; }
    set TotalDisconnected(value: number) {
        if (this._TotalDisconnected != value) {
            this._TotalDisconnected = value;
        }
    }
    private Listen() {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = SessionLocator.SelectedSession.CurrentEditComponent.ComponentId;
            SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                
                if (isSaveSuccess) {
                    this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    this.CourierMasterService.isNotDirty = true;
                    if (this.CourierMasterService.disconnectedSelectAll) {
                        //  this.CourierMasterService.connectedSelectAll = true;
                        this.CourierMasterService.disconnectedSelectAll = false;
                    }
                    
                    else if (this.CourierMasterService.connectedSelectAll) {
                        this.CourierMasterService.disconnectedSelectAll = false;
                        //     this.CourierMasterService.connectedSelectAll = false;
                    }
                    this.CourierMasterService.connectedSelectAll = true;
                    
                    this.LoadConnectedDeclarationGrid();
                    this.LoadNotConnectedDeclarationGrid();
                }
            });
            
            SessionLocator.SelectedSession.CurrentEditComponent.SaveStart.subscribe((entityPM: any) => {
                this.sendConnectDeclaration();
            });
            
            SessionLocator.SelectedSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    //this.BuildColumns();
                    this.LoadConnectedItems();
                }
            });

            SessionLocator.SelectedSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                if (this.CurrentEditComponentId == SessionLocator.SelectedSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "COCD") {

                    }
                }
            });
        }
    }

    private async sendConnectDeclaration() {
        if (
                this.CourierMasterService.connectedSelectAll
                && !this.CourierMasterService.disconnectedSelectAll
                && this.entityPM.ConnectedDeclarations?.split(',')?.length < 100 
                && this.entityPM.NotConnectedDeclarations?.split(',')?.length < 100
            )
            return;
            
        SessionLocator.SelectedSession.StartBusyIndicator('פותח מסר קישור הצהרות')
        await this.CourierMasterService.sendConnectDeclaration(this.entityPM.Id, this.entityPM.Tenant, this.entityPM.HAWB, this.CourierMasterService.connectedSelectAll, this.CourierMasterService.disconnectedSelectAll, this.CourierMasterService.connectedItems.Collection, this.CourierMasterService.disconnectedItems.Collection)
            .then(() => { 
                CustomMessageProgressComponent.ShowCustomMessageProgressComponent('הצהרות מקושרות', 'עדכון כל ההצהרות נשלח בתהליך ברקע', () => { })
                SessionLocator.SelectedSession.StopBusyIndicator();
            })
            .catch(() => {
                CustomMessageProgressComponent.ShowCustomMessageProgressComponent('הצהרות מקושרות', 'עדכון כל ההצהרות נכשל', () => { })
                SessionLocator.SelectedSession.StopBusyIndicator();
            });
    }

    OnAllConnectedChecked(isFirst: boolean) {
        this.CourierMasterService.connectedSelectAll = true;
        this.CourierMasterService.connectedItems.Clear();
        this.entityPM.NotConnectedDeclarations = "";
        this.CourierMasterService.isNotDirty = !!isFirst;
        
        this.LoadConnectedItems();
    }
    
    OnAllConnectedUnchecked() {
        this.CourierMasterService.connectedSelectAll = false;
        this.CourierMasterService.connectedItems.Clear();
        this.entityPM.NotConnectedDeclarations = "ALL";
        this.CourierMasterService.isNotDirty = false;

        this.LoadConnectedItems();
    }

    OnAllDiconnectedChecked() {
        this.CourierMasterService.disconnectedSelectAll = true;
        this.CourierMasterService.disconnectedItems.Clear();
        this.entityPM.ConnectedDeclarations = "ALL";
        
        this.LoadNotConnectedDeclarationGrid();
        this.CourierMasterService.isNotDirty = false;
    }
    
    OnAllDiconnectedUnchecked() {
        this.CourierMasterService.disconnectedSelectAll = false;
        this.CourierMasterService.disconnectedItems.Clear();
        this.entityPM.ConnectedDeclarations = "";
        this.CourierMasterService.isNotDirty = false;

        this.LoadNotConnectedDeclarationGrid();
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
    test: any;

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
        this._CourierMasterValidator.CheckRequestInProgressForCourierMaster(this.entityPM.Tenant, "UCADPE", this.entityPM.Id).subscribe((response: any) => {
            var displayOnlyCheckResult = response.Result;
            if (displayOnlyCheckResult != null && displayOnlyCheckResult.length > 0) {
                let customsRequestsSheetPM: CustomsRequestsSheetPM = displayOnlyCheckResult.filter(r => r.InterfaceTypeCode == "UCADPE")[0];
                if (customsRequestsSheetPM != null) {
                    this.IsDisplayOnly = true;
                    this.DisplayOnlyMessage = "לתצוגה בלבד - קיימת בקשה לעדכון פנדינג ברקע ";
                }
            }
        });
    }

    RefreshEntity() {
        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
        this.DisplayOnlyCheck();
    }

}
