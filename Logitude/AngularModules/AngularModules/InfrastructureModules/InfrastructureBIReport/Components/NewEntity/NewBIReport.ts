import { Component, Output, EventEmitter } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportPM } from '../../../../Infrastructure/EntityPMs/BIReportPM';
import { BIReportPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BIReportExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/BIReportExtendedPMService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { BIReportExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/BIReportExtendedListService';
import { BIReportList } from '../../../../Infrastructure/EntityLists/BIReportList';

@Component({
    moduleId: module.id,
    templateUrl: './NewBIReport.html',
})

export class NewBIReport extends BaseComponent {
    public EntityPM: BIReportPM;
    public ValidationErrorsList: string[] = [];
    private myService: BIReportPMService;
    public DataContext: NewBIReport = this;
    public ObjectTableName: string = "BIReport";
    public IsNewQuery = true;
    public bIReportExtendedPMService : BIReportExtendedPMService;
    public bIReportExtendedListService : BIReportExtendedListService;
    private CurrentSession = SessionLocator.SelectedSession;
    private OriginalName: string = "";
    private IsCopy: boolean = false;
    private ComponentRef;
    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() searchFieldChangeEvent = new EventEmitter();
    constructor() {
        super();
        this.EntityPM = new BIReportPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.bIReportExtendedPMService = new BIReportExtendedPMService();
        this.bIReportExtendedListService = new BIReportExtendedListService();
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.LastRunDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.LastRunByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.TypeCode = "EXL";
        this.myService = new BIReportPMService();
        this.SetUIProperties();
        this.BuildColumns();
    }


    SetWindowArgs(args: any) {
        if (args.Name != null) {
            this.EntityPM.DWQueryId = args.DWQueryId;
            this.EntityPM.BIReportFolderId = args.BIReportFolderId;
            this.EntityPM.Name = args.Name + "/Copy";
            this.OriginalName = args.Name;
            this.EntityPM.Description = args.Description;
            this.IsCopy = true;
            //this.ComponentRef = args.ComponentRef;
            //this.BackCompleted = args.BackCompleted;
        }
        else {
            this.DWQueryId = args.DWQueryId;
            this.EntityPM.BIReportFolderId = args.FolderId;
        }
        this.SetUIProperties();
    }
    public items: any[] = [];
    SetUIProperties() {
        this.UIProperties.SetEnabled("TypeCode", this.ObjectTableName, false);
        this.UIProperties.SetRequired("DWQueryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DWQueryId));

        if (!AppTool.IsNullOrEmpty(this.DWQueryId)) {
            this.IsNewQuery = false;
        }

        else {
            this.IsNewQuery = true;
        }
    }
    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingCol: "CreateDate",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    }

     

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        filters = new ApiQueryFilters();
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = this.BIReportTenant;
        
        return this.bIReportExtendedListService.GetTenantReports(filters, this.BIReportTenant);
    }

    columns: any;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: "Name",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            Styles: { width: '150px' },

        });
        this.columns.push({
            FieldName: "Create Date",
            DataTypeCode: 'DateTime',
            IsCustomTemplate: true,
            Display: 'Create Date',
            Styles: { width: '150px' },
            //HtmlListComponentName: 'CreateDateComponent',
            //HtmlListComponentUrl: './Infrastructure/Components/Templates/CreateDateComponent',

        });
        this.columns.push({
            FieldName: "Update Date",
            DataTypeCode: 'DateTime',
            IsCustomTemplate: true,
            Display: 'Update Date',
            Styles: { width: '150px' },
            //HtmlListComponentName: 'UpdateDateComponent',
            //HtmlListComponentUrl: './Infrastructure/Components/Templates/UpdateDateComponent',

        });
    }

    // Tenant Search
    OnSearchTextChangeEvent(searchText) {
        this.BIReportTenant = searchText;
        this.searchFieldChangeEvent.emit(searchText);
    }

    onRowSelected(selected) {
        let item: BIReportList = selected.rowData;
        this.EntityPM.DWQueryId = item.DWQueryId;
        //this.EntityPM.BIReportFolderId = args.BIReportFolderId;
        this.EntityPM.Name = item.Name;
        this.EntityPM.Description = item.Description;
        this.IsCopy = true;
        //
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get BIReportTenant() { return this.EntityPM.Tenant; }
    set BIReportTenant(newValue: number) {
        if (this.EntityPM.Tenant != newValue) {
            this.EntityPM.Tenant = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get BIReportFolderId() { return this.EntityPM.BIReportFolderId; }
    set BIReportFolderId(newValue: string) {
        if (this.EntityPM.BIReportFolderId != newValue) {
            this.EntityPM.BIReportFolderId = newValue;
        }
    }

    get DWQueryId() { return this.EntityPM.DWQueryId; }
    set DWQueryId(newValue: string) {
        if (this.EntityPM.DWQueryId != newValue) {
            this.EntityPM.DWQueryId = newValue;
        }
    }

    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(newValue: string) {
        if (this.EntityPM.TypeCode != newValue) {
            this.EntityPM.TypeCode = newValue;
        }
    }

    ShowQueryBuilderClicked() {
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.DWQueryId = this.DWQueryId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1200;
        logWindow.Height = 820;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (s != null) {
                    this.EntityPM.DWQueryId = s.QID;
                    this.SetUIProperties();
                }
            });
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name Field is Required");
            
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.BIReportFolderId)) {
            this.ValidationErrorsList.push("Folder Field is Required");
             
        }
        if(this.ValidationErrorsList.length != 0) {
            return;
        }
            
        
        this.bIReportExtendedPMService.DoesReportExist(this.EntityPM.Name, this.EntityPM.BIReportFolderId).subscribe(response => {
            if (response.Result == true) {
                this.ValidationErrorsList.push("Please use other name for your report so it is different from others");
            }
            else {
                this.CurrentSession.CloseCurrentWindow();

                var logWindow = new LogitudeWindow();
                var windowArgs: any = {};
                windowArgs.DWQueryId = this.DWQueryId;
                windowArgs.IsCopy = this.IsCopy;
                windowArgs.IsCopyFromTenant = this.BIReportTenant;
                windowArgs.ComponentRef = this.ComponentRef;
                windowArgs.BackCompleted = this.BackCompleted;
                windowArgs.IsBIReportWorkspace = true;
                logWindow.WindowArgs = windowArgs;
                logWindow.Width = 1200;
                logWindow.Height = 820;
                logWindow.Title = "Query Builder";
                logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
                logWindow.ComponentLoaded.subscribe(s => {
                    logWindow.WindowClosed.subscribe(d => {
                        if (s != null) {
                            if (d != "cancel") {
                                this.EntityPM.DWQueryId = s.QID;

                                if (this.ValidationErrorsList.length == 0) {
                                    this.CurrentSession.StartBusyIndicatorSaving();
                                    this.EntityPM.Tenant = SessionLocator.Tenant;
                                    this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                                        this.CurrentSession.StopBusyIndicator();

                                        if (myResponse.HasError) {
                                            this.ValidationErrorsList = myResponse.ErrorsArray;
                                        }

                                        else {
                                            this.CurrentSession.CloseCurrentWindow();


                                            SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                                                .then(cmpRef => {
                                                    cmpRef.instance.ComponentRef = cmpRef;
                                                    cmpRef.instance.Run({
                                                        DWQueryId: s.QID,
                                                        ObjectTableName: 'BIReport',
                                                        EntityId: this.EntityPM.Id,
                                                        BackButtonLable: this.IsCopy ? "BI Report :" + this.OriginalName : "BI Reports",
                                                    });
                                                });

                                        }
                                    });
                                }
                            }
                        }
                    });
                });
            }});
         

       
   
    }
}
