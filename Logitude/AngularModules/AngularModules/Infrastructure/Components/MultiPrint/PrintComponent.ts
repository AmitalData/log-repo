import { XmlParser } from '@angular/compiler';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { DocumentTypeCopyList } from '../../../Common/EntityLists/DocumentTypeCopyList';
import { DocumentTypeTemplateList } from '../../../Common/EntityLists/DocumentTypeTemplateList';
import { DocumentTypeTemplateListExtendedService } from '../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import { DocumentTypeCopyPMExtendedService } from '../../../Common/Services/ExtendedPMs/DocumentTypeCopyPMExtendedService';
import { ApiQueryFilters } from '../../DataContracts/ApiQueryFilters';
import { CodeNameClass } from '../../DataContracts/CodeNameClass';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { BatchTaskExecutionList } from '../../EntityLists/BatchTaskExecutionList';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { BatchPrintService, BatchPrintManagerArgs, PrintEntityKeys, PrintingResult, PrintingRow} from '../../Services/BatchPrintService';
import { EntityListService } from '../../Services/EntityListService';
import { BatchTaskExecutionListService } from '../../Services/StandardLists/BatchTaskExecutionListService';
import { AppTool } from '../../Tools';
import { SessionInfo } from '../../Utilities/SessionInfo';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { BaseComponent } from '../LogitudeComponents/BaseComponent';
declare var window: any;

@Component({
    selector: 'PrintComponent',
    templateUrl: 'PrintComponent.html',
})

export class PrintComponent extends BaseComponent implements OnInit {    
    private CurrentSession = SessionLocator.SelectedSession;
    ParentComponent: any;
    QueryCode: string;
    Filters: ApiQueryFilters;
    public columns: any[] = null;
    ObjectTableName: string;
    ObjectTableId: string;
    public ObjectTable: ObjectTablePM;
    Title: string;
    public items: any[] = [];
    SelectedRecords: any[] = [];
    SelectedRecordsCount: number = 0;
    AllRecords: any[] = [];
    AllRecordsCount: number = 0;
    SelectedItemsCountText: string = null;
    @Output() SearchFieldchangeevent = new EventEmitter();
    public DataContext = this;
    public DocumentTypeTemplateList: CodeNameClass[];
    public DocumentTypeCopiesList: CodeNameClass[];
    private documentTypeService: DocumentTypeTemplateListExtendedService;
    public ValidationErrorsList: string[];
    public DocumentTypeQueryFilters: ApiQueryFilters;
    private selectedEntitiesIds: string[];
    constructor(private _entityListService: EntityListService) {
        super();
        window.AllRecords = [];
        this.selectedEntitiesIds = [];
        this.documentTypeService = new DocumentTypeTemplateListExtendedService();
        this.BuildQueryFilters();
        this.Listen();
    }

    private BuildQueryFilters() {
        this.DocumentTypeQueryFilters = new ApiQueryFilters();
        this.DocumentTypeQueryFilters.addAdditionalFilter("TemplateFormatCode", "P", null, null, "Equals", false, false, false, "string");
    }

    ngOnInit() {
    }

    private ListenEvent: any = null;
    Listen() {
        this.ListenEvent = this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
            var temp = this.SelectedRecords.filter(a => a.Id == res.Id);
            this.ChangeItemCheck(res);
            if (res.IsChecked && temp.length == 0) {
                this.AddToSelectedRecords(res);
                return;
            }
            if (temp.length > 0) {
                this.RemoveFromSelectedRecords(res);
            }
        });
    }
    private ChangeItemCheck(res: any) {
        this.AllRecords.forEach(function (record) {
            if (record.Id == res.Id)
                record.IsChecked = res.IsChecked;
        });
    }
    private RemoveFromSelectedRecords(event: any) {
        this.SelectedRecords = this.SelectedRecords.filter(a => a.Id != event.Id);
        this.SelectedRecordsCount--;
        this.IsAllRecordSelectedChange(false, false);
        this.ChangeSelectedItemsCountText(this.SelectedRecordsCount);
    }

    private AddToSelectedRecords(event: any) {
        this.SelectedRecords.push(event);
        this.SelectedRecordsCount++;
        if (this.SelectedRecordsCount == this.AllRecords.length) {
            this.IsAllRecordSelectedChange(true, false);
        }
        this.ChangeSelectedItemsCountText(this.SelectedRecordsCount);
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.ListenEvent);
        this.ListenEvent = null
    }

    SetWindowArgs(windowArgs: any) {
        let args: any = windowArgs.args;
        this.ParentComponent = windowArgs.parentComponent;
        this.QueryCode = args.QueryCode;
        this.Filters = this.Clone(args.Filters);
        this.columns = this.Clone(args.Columns);
        this.ObjectTableName = args.ObjectTable.Name;
        this.ObjectTable = args.ObjectTable;
        this.ObjectTableId = args.ObjectTable.Id;
        this.Title = args.Title;

        this.InitGrid();
    }

    private InitGrid() {
        this.Filters.GetCount = true;
        var checkBoxColumn = {
            FieldName: "Checked",
            DataTypeCode: 'Boolean',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            HtmlListComponentName: 'MultiPrintCheckBoxComponent',
            HtmlListComponentUrl: './Infrastructure/Components/MultiPrint/MultiPrintCheckBoxComponent',
        };

        var printSuccess = {
            FieldName: 'PrintSuccess',
            DataTypeCode: 'Boolean',
            Display: 'Print Status',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'MultiPrintCheckTemplate',
            HtmlListComponentUrl: './Infrastructure/Components/MultiPrint/MultiPrintCheckTemplate',
        };
        this.columns.splice(0, 0, checkBoxColumn);
        this.columns.splice(1, 0, printSuccess);
    }

    OnPrintFinish(printingResult: PrintingResult) {
        var entities: PrintingRow[] = printingResult.NotValidRows;
        var results: PrintingRow[] = [];

        this.AllRecords.forEach((record) => {
            var entity = entities.find(item => item.EntityId == record.Id);
            record.PrintSuccess = entity ? !entity.Error : undefined;

            if (entity == null && this.selectedEntitiesIds.filter(d => d == record.Id)[0] != null) {
                record.PrintSuccess = true;
            }

            if (entity && entity.Error) {
                var row: PrintingRow = new PrintingRow();
                row.EntityId = entity.EntityId;
                row.EntityNumber = entity.EntityNumber;
                row.Error = entity.Error;
                results.push(row);
            }
        });

        this.ParentComponent.PrintingRows = results;
        this.ParentComponent.DocumentId = printingResult.DocumentId;
        this.ParentComponent.FileName = printingResult.FileName;
        this.ParentComponent.SecurityId = printingResult.SecurityId;
        this.RefreshList();
    }

    Clone(list: any): any {
        return JSON.parse(JSON.stringify(list));
    }

    private documentTypeId: string;
    get DocumentTypeId() { return this.documentTypeId; }
    set DocumentTypeId(value: string) {
        if (this.documentTypeId != value) {
            this.documentTypeId = value;

            this.SelectedDocumentTypeTemplate = null;
            this.SelectedDocumentTypeCopy = null;
            this.LoadTemplates();
            this.LoadCopies();
        }
    }    
    LoadTemplates() {        
        this.documentTypeService.getDocumentTypeTemplateListsForDocumentType(this.DocumentTypeId, SessionLocator.Tenant).subscribe((res: any) => {
            this.DocumentTypeTemplateList = new Array<CodeNameClass>();

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult: DocumentTypeTemplateList[] = pmResponse.Result;
                if (myResult) {
                    myResult.filter(d => d.InActive == false).forEach((item) => {
                        if (item.TemplateType == "P") {
                            this.DocumentTypeTemplateList.push(new CodeNameClass(item.Id, item.Name));
                        }
                    });
                }
            }
        });
    }
    LoadCopies() {
        this.documentTypeService.GetDocumentTypeCopiesForDocumentType(this.DocumentTypeId).subscribe((res: any) => {
            this.DocumentTypeCopiesList = new Array<CodeNameClass>();

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult: DocumentTypeCopyList[] = pmResponse.Result;
                if (myResult) {
                    myResult.filter(d => d.InActive == false).forEach((item) => {
                        this.DocumentTypeCopiesList.push(new CodeNameClass(item.Id, item.Name));
                    });
                }
            }
        });
    }

    private selectedDocumentTypeTemplate: CodeNameClass;
    get SelectedDocumentTypeTemplate() { return this.selectedDocumentTypeTemplate; }
    set SelectedDocumentTypeTemplate(value: CodeNameClass) {
        if (this.selectedDocumentTypeTemplate != value) {
            this.selectedDocumentTypeTemplate = value;
        }
    }

    private selectedDocumentTypeCopy: CodeNameClass;
    get SelectedDocumentTypeCopy() { return this.selectedDocumentTypeCopy; }
    set SelectedDocumentTypeCopy(value: CodeNameClass) {
        if (this.selectedDocumentTypeCopy != value) {
            this.selectedDocumentTypeCopy = value;
        }
    }

    dataSource = {
        pageSize: 100,
        rowCount: null,
        sortingCol: "",
        sortingDir: "",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.Filters.PageIndex = 0;
        filters = this.Filters;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.PageSize = 100;
        return this._entityListService.getByFilters(this.ObjectTableName, filters, null);
    }

    SelectedRow: any;
    onRowSelected(CurrentRow) {
        this.SelectedRow = CurrentRow.rowData;
    }

    GridAfterViewInitCompleted($event) {
        this.LoadList();
    }

    @Output() MenuHeaderchangeevent = new EventEmitter();
    LoadList() {
        this.Filters.SortBy = this.Filters.SortBy;
        this.Filters.SortDirection = this.Filters.SortDirection;
        this.MenuHeaderchangeevent.emit({ Filters: this.Filters, IgnoreFilter: false });
    }

    FirstTime: boolean = true;
    onCountReady(count) {
        if (!this.FirstTime) {
            return;
        }
        this.SelectedRecordsCount = count;
        this.AllRecordsCount = count;
    }
    AllRecordsReady(result) {
        if (!this.FirstTime) {
            return;
        }
        this.FirstTime = false;
        this.AllRecords = result;
        this.SelectedRecords = result;
        window.AllRecords = result;
        this.IsAllRecordSelectedChange(true, true, false);
    }

    public IsAllRecordSelected: boolean;
    public IsAllRecordSelectedChange(isChecked, applyForAll, refreshList = true) {
        this.IsAllRecordSelected = isChecked;
        if (applyForAll) {
            this.ChangeAllRecordsSelection(isChecked, refreshList);
        }
    }

    private ChangeAllRecordsSelection(isChecked: boolean, refreshList: boolean) {
        isChecked == true ? this.CheckAllRecords() : this.UnCheckAllRecords();
        if (refreshList) {
            this.RefreshList();
        }
    }
    RefreshList() {
        window.AllRecords = this.AllRecords;
        this.SearchFieldchangeevent.emit("");
    }
    private CheckAllRecords() {
        this.AllRecords.forEach(element => {
            element.IsChecked = true;
        });

        this.ChangeSelectedItemsCountText(this.AllRecordsCount);
        this.SelectedRecordsCount = this.AllRecords.length;
        this.SelectedRecords = this.AllRecords;
    }

    private UnCheckAllRecords() {
        this.AllRecords.forEach(element => {
            element.IsChecked = false;
        });

        this.ChangeSelectedItemsCountText(0);
        this.SelectedRecords = [];
        this.SelectedRecordsCount = 0;
    }
    private ChangeSelectedItemsCountText(selectedCount) {
        this.SelectedItemsCountText = selectedCount + " of " + this.AllRecordsCount;
    }

    NextClicked() {
        this.ParentComponent.NextButtonClicked();
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public IsMultiEntityPrintedSuccessfully: boolean = false;
    
    PrintClick() {
        if (!this.IsPrintValid()) {
            return;
        }
        
        var args: BatchPrintManagerArgs = new BatchPrintManagerArgs();
        args.DocumentTypeId = this.DocumentTypeId;
        args.TemplateId = this.SelectedDocumentTypeTemplate.Code;
        args.CopyId = this.SelectedDocumentTypeCopy.Code;
        args.ObjectTableId = this.ObjectTableId;
        args.Tenant = SessionInfo.LoggedUserTenant;
        args.EntityIds = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        //this.CurrentSession.StartBusyIndicator("Start Printing... ");
        this.SelectedRecords.forEach((item) => {
            var key: PrintEntityKeys = new PrintEntityKeys();
            if (this.ObjectTableName == "Shipment") {
                key.EntityId = item.Id;
                key.ObjectTableId = this.ObjectTableId;
            }
            else {
                key.ChildEntityId = item.Id;

                if (item.IsConsolidationInvoice || item.IsGeneralInvoice) {
                    key.EntityId = item.Id;
                    key.ObjectTableId = this.ObjectTableId;
                }
                else {
                    key.EntityId = item.MainEntityId;
                    key.ObjectTableId = window.ObjectTables.filter(d => d.Name == "Shipment")[0]?.Id;                    
                }
            }
            key.EntityNumber = this.ObjectTableName == "Shipment" ? item.ShipmentNumber : item.InvoiceNumber;
            args.EntityIds.push(key);
            this.selectedEntitiesIds.push(item.Id);
        });
        
        var service: BatchPrintService = new BatchPrintService();
        service.Print(args).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var batchTaskExecutionId: string = myResponse.Result;               
                this.StopTimer();

                this.timer = setInterval(() => {
                    this.CheckBatchTaskExecution(batchTaskExecutionId);
                }, this.timerInterval);
            }

           // this.CurrentSession.StopBusyIndicator();
        });
    }
    IsPrintValid(): boolean {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.DocumentTypeId)) {
            this.ValidationErrorsList.push("Please select document type");
        }

        if (this.SelectedDocumentTypeTemplate == null) {
            this.ValidationErrorsList.push("Please select document template");
        }

        if (this.SelectedDocumentTypeCopy == null) {
            this.ValidationErrorsList.push("Please select document copy");
        }

        if (!this.SelectedRecordsCount || this.SelectedRecordsCount === 0) {
            this.ValidationErrorsList.push("Please select at least one item to print");
        }

        if (this.ValidationErrorsList.length != 0) {
            return false;
        }

        return true;
    }

    timer: any;
    timerInterval: number = 1000;
    StopTimer() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    }

    CheckBatchTaskExecution(BatchTaskExecutionId: string) {
        var iBatchService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
        iBatchService.getSingle(BatchTaskExecutionId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BatchTaskExecutionList = myResponse.Result;

                if (list.StatusCode == "D") {
                    this.StopTimer();
                    this.IsMultiEntityPrintedSuccessfully = true;
                    this.CurrentSession.StopBusyIndicator();

                    var printingResult: PrintingResult = JSON.parse(list.PrametersXml);
                    this.OnPrintFinish(printingResult);
                }

                else if (list.StatusCode == "F") {
                    this.StopTimer();
                    this.CurrentSession.StopBusyIndicator();

                    var errors: string[] = [];
                    errors.push(list.ErrorLog);
                    this.ValidationErrorsList = errors;
                }

                else {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.StartBusyIndicator("Printing... " + list.ProgressPercentage + "/" + this.SelectedRecordsCount);
                }
            }

            else {
                this.StopTimer();
                this.CurrentSession.StopBusyIndicator();
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
}
