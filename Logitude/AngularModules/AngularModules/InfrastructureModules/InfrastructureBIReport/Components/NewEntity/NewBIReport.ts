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
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DWObjectTableExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/DWObjectTableExtendedListService';
import { BIReportFolderExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/BIReportFolderExtendedListService';

@Component({
    
    templateUrl: './NewBIReport.html',
})

export class NewBIReport extends BaseComponent {
    public EntityPM: BIReportPM;
    public ValidationErrorsList: string[] = [];
    private myService: BIReportPMService;
    public DataContext: NewBIReport = this;
    public ObjectTableName: string = "BIReport";
    public IsNewQuery = true;
    public BIReportExtendedPMService: BIReportExtendedPMService;
    public BIReportExtendedListService: BIReportExtendedListService;
    public DWObjectTableExtendedListService: DWObjectTableExtendedListService;
    private folderListService: BIReportFolderExtendedListService;
    private CurrentSession = SessionLocator.SelectedSession;
    private OriginalName: string = "";
    public IsCopy: boolean = false;
    public IsNewBIReport: boolean = true;
    public IsTenantZero: boolean = false;
    public IsOneRowSelected: boolean = false;
    public HasCopyFeature: boolean = false;
    public CopyFromTitle: string;
    public FactTables: string[] = [];
    public BIReportFolders: string[] = [];
    public SelectdFactTableName: string;
    public SelectdBIReportFolder: string;
    private ComponentRef;
    @Output() BackCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() TenantFieldChangeEvent = new EventEmitter();
    constructor() {
        super();
        this.HasCopyFeature = FeatureLocator.HasFeaturePermession("BIReport", "BIReportCopyFromLibrary");
        this.EntityPM = new BIReportPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.BIReportExtendedPMService = new BIReportExtendedPMService();
        this.BIReportExtendedListService = new BIReportExtendedListService();
        this.DWObjectTableExtendedListService = new DWObjectTableExtendedListService();
        this.folderListService = new BIReportFolderExtendedListService();
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.LastRunDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.TypeCode = "EXL";
        this.FillFactTableNamesList();
        this.FillBIReportFolderNamesList();
        this.myService = new BIReportPMService();
        this.SetUIProperties();
        this.CheckTenantZero();
    }


    SetWindowArgs(args: any) {
        if (args.Name != null) {
            this.EntityPM.DWQueryId = args.DWQueryId;
            this.EntityPM.BIReportFolderId = args.BIReportFolderId;
            this.EntityPM.Name = args.Name + "/Copy";
            this.OriginalName = args.Name;
            this.EntityPM.Description = args.Description;
            this.IsCopy = args.IsCopy;
            this.EntityPM.FactTableName = args.FactTableName;
            this.BIReportsTenant = SessionLocator.Tenant;
            //this.ComponentRef = args.ComponentRef;
            //this.BackCompleted = args.BackCompleted;
        }
        else if (args.IsCopyFromLibrary) {
            this.IsNewBIReport = false;
            this.EntityPM.BIReportFolderId = args.FolderId;
            this.BuildColumns();
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

    FillFactTableNamesList() {
        this.SelectdFactTableName = "";
        this.DWObjectTableExtendedListService.GetFactTablesNames().subscribe((response: ServiceResponse) => {
            var factTablesNames: string[] = response.Result;
            factTablesNames.forEach((factTable: string) => {
                switch (factTable) {
                    case "Fact_Shipments":
                        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "BIReport.Fact.Shipments"))
                            this.FactTables.push("Shipments");
                        break;

                    case "Fact_Charges":
                        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "BIReport.Fact.ShipmentCharges"))
                            this.FactTables.push("Shipment Charges");
                        break;
                }
            });
            if (AppTool.IsNullOrEmpty(this.EntityPM.FactTableName)) {
                this.FactTableSelectionChanged("");
            }
            else {
                this.FactTableSelectionChanged(this.EntityPM.FactTableName);
            }
        });
    }

    FactTableSelectionChanged(selectControl: any) {
        this.UIProperties.SetRequired("FactTableName", this.ObjectTableName, false);

        switch (selectControl) {
            case "Shipments":
            case "Fact_Shipments":
                this.FactTableName = "Fact_Shipments";
                this.SelectdFactTableName = "Shipments";
                break;

            case "Shipment Charges":
            case "Fact_Charges":
                this.FactTableName = "Fact_Charges";
                this.SelectdFactTableName = "Shipment Charges";
                break;
            default:
                this.FactTableName = "";
                this.UIProperties.SetRequired("FactTableName", this.ObjectTableName, true);
                break;
        }
    }

    FillBIReportFolderNamesList() {
        this.folderListService.GetPermittedFolders().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.BIReportFolders = myResponse.Result;
                var selectedBIReport: string = myResponse.Result.filter(bi => bi.Id == this.EntityPM.BIReportFolderId)[0];
                if (!AppTool.IsNullOrEmpty(selectedBIReport)) this.BIReportFolderSelectionChanged(selectedBIReport);

            }
        });
    }

    BIReportFolderSelectionChanged(selectControl: any) {
        if (selectControl) {
            this.SelectdBIReportFolder = selectControl;
            this.BIReportFolderId = selectControl.Id;
            this.UIProperties.SetRequired("BIReportFolderId", this.ObjectTableName, false);
        }
        else {
            this.SelectdBIReportFolder = "";
            this.BIReportFolderId = "";
            this.UIProperties.SetRequired("BIReportFolderId", this.ObjectTableName, true);
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

        if (this.BIReportsTenant != -1) {
            filters.Tenant = this.BIReportsTenant;
            return this.BIReportExtendedListService.GetReportsByTenantNumber(filters, this.BIReportsTenant);
        }
        else {
            filters.Tenant = SessionLocator.Tenant;
            //filters.Filter1Name = "GetAllReports";
            filters.Filter1Value = true;
            //Get all reports
            return this.BIReportExtendedListService.GetReportsByTenantNumber(filters, SessionLocator.Tenant);
        }
    }

    columns: any;
    BuildColumns() {
        var dateWidth = '130px';
        if (this.IsTenantZero) {
            dateWidth = '200px';
        }
        this.columns = [];
        this.columns.push({
            FieldName: "Name",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: TextCodeTranslator.Translate("BIReport.F.Name"),
            Styles: { width: '150px' },
        });
        this.columns.push({
            FieldName: "CreateDate",
            DataTypeCode: 'DateTime',
            IsCustomTemplate: true,
            Display: TextCodeTranslator.Translate("BIReport.F.CreateDate"),
            Styles: { width: dateWidth },
            HtmlListComponentName: 'BIReportListTemplate',
            HtmlListComponentUrl: './InfrastructureModules/InfrastructureBIReport/Components/ListTemplates/BIReportListTemplate',
        });
        this.columns.push({
            FieldName: "UpdateDate",
            DataTypeCode: 'DateTime',
            IsCustomTemplate: true,
            Display: TextCodeTranslator.Translate("BIReport.F.UpdateDate"),
            Styles: { width: dateWidth },
            HtmlListComponentName: 'BIReportListTemplate',
            HtmlListComponentUrl: './InfrastructureModules/InfrastructureBIReport/Components/ListTemplates/BIReportListTemplate',
        });
    }

    // Tenant Search
    OnTenantFieldChangeEvent(copyFromTenant) {
        if (copyFromTenant != null) {
            this.BIReportsTenant = copyFromTenant;
        }
        else {
            this.BIReportsTenant = -1; //This means all reports from all tenants
        }
        this.TenantFieldChangeEvent.emit(copyFromTenant);
    }

    SetNewBIReport(value: boolean) {
        this.IsNewBIReport = value;
    }

    CheckTenantZero() {
        if (SessionLocator.Tenant == 0) {
            this.IsTenantZero = true;
            this.CopyFromTitle = "Copy From All Tenants";
            this.BIReportsTenant = -1;
            this.BuildColumns();
        }
        else if (this.HasCopyFeature) {
            this.BIReportsTenant = 0;
            this.CopyFromTitle = "Copy From Library";
        }
    }

    onRowSelected(selected) {
        let item: BIReportList = selected.rowData;
        this.EntityPM.DWQueryId = item.DWQueryId;
        this.EntityPM.Name = item.Name;
        this.EntityPM.FactTableName = item.FactTableName;
        this.EntityPM.Description = item.Description;
        this.BIReportsTenant = item.Tenant;
        this.IsCopy = true;
        this.IsOneRowSelected = true;
        if (this.EntityPM.FactTableName)
          this.FactTableSelectionChanged(this.EntityPM.FactTableName);
    }

    omit_special_char(value: string) {
        this.ValidationErrorsList = [];
        let format = /[!@#$%^&*()_+\-=\[\]{};:"\\|,.<>\/?~]/;
        if (format.test(value)) {
            this.ValidationErrorsList.push("Name field can't contain the following special characters # &")
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
            //this.omit_special_char(newValue);
        }
    }

    get BIReportsTenant() { return this.EntityPM.Tenant; }
    set BIReportsTenant(newValue: number) {
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

    get FactTableName() { return this.EntityPM.FactTableName; }
    set FactTableName(newValue: string) {
        if (this.EntityPM.FactTableName != newValue) {
            this.EntityPM.FactTableName = newValue;
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
        windowArgs.FactTableName = this.FactTableName;
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
                    if (s != "cancel") {
                        this.myService.update(this.EntityPM);
                    }
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
        else {
            this.omit_special_char(this.EntityPM.Name);
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.FactTableName)) {
            this.ValidationErrorsList.push("Fact Table Field is Required");

        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.BIReportFolderId)) {
            this.ValidationErrorsList.push("Folder Field is Required");
             
        }

        if (!this.IsNewBIReport && !this.IsOneRowSelected) {
            this.ValidationErrorsList.push("Please select one report");
        }
        if(this.ValidationErrorsList.length != 0) {
            return;
        }
            

        this.BIReportExtendedPMService.DoesReportExist(this.EntityPM.Name, this.EntityPM.BIReportFolderId).subscribe((response: ServiceResponse) => {
            if (response.Result == true) {
                this.ValidationErrorsList.push("Please use other name for your report so it is different from others");
            }
            else {
                this.CurrentSession.CloseCurrentWindow();

                var logWindow = new LogitudeWindow();
                var windowArgs: any = {};
                windowArgs.DWQueryId = this.DWQueryId;
                windowArgs.IsCopy = this.IsCopy;
                windowArgs.BIReportsTenant = this.BIReportsTenant;
                windowArgs.ComponentRef = this.ComponentRef;
                windowArgs.BackCompleted = this.BackCompleted;
                windowArgs.FactTableName = this.FactTableName;
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

                                            this.myService.update(this.EntityPM);

                                            SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                                                .then(cmpRef => {
                                                    cmpRef.instance.ComponentRef = cmpRef;
                                                    cmpRef.instance.Run({
                                                        DWQueryId: s.QID,
                                                        Name: this.EntityPM.Name,
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
