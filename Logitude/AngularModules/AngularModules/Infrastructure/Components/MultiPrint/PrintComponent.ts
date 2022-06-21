import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { DocumentTypeTemplateList } from '../../../Common/EntityLists/DocumentTypeTemplateList';
import { DocumentTypeTemplateListExtendedService } from '../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import { ApiQueryFilters } from '../../DataContracts/ApiQueryFilters';
import { CodeNameClass } from '../../DataContracts/CodeNameClass';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { EntityListService } from '../../Services/EntityListService';
import { AppTool } from '../../Tools';
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
    public SelectedDocumentTypeTemplate: CodeNameClass;
    public DocumentTypeTemplateList: CodeNameClass[];

    constructor(private _entityListService: EntityListService) {
        super();
        window.AllRecords = [];
        this.Listen();
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
        //this.FillObjectField();
    }

    private InitGrid() {
        this.Filters.GetCount = true;
        var checkBoxColumn = {
            FieldName: "Checked",
            DataTypeCode: 'Boolean',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            HtmlListComponentName: 'MultiUpdateCheckBoxComponent',
            HtmlListComponentUrl: './Infrastructure/Components/MultiUpdateComponent/MultiUpdateCheckBoxComponent',
        };

        var updateSuccess = {
            FieldName: 'UpdateSuccess',
            DataTypeCode: 'Boolean',
            Display: 'Update Status',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'MultiUpdateCheckTemplate',
            HtmlListComponentUrl: './Infrastructure/Components/MultiUpdateComponent/MultiUpdateCheckTemplate',
        };
        this.columns.splice(0, 0, checkBoxColumn);
        this.columns.splice(1, 0, updateSuccess);
    }

    OnUpdateFinish(entities) {
        //this.AllRecords.forEach(function (record) {
        //    var entity = entities.find(item => item.EntityId == record.Id);
        //    record.UpdateSuccess = entity ? !entity.HasException : undefined;
        //});

        //this.RefreshList();
    }

    Clone(list: any): any {
        return JSON.parse(JSON.stringify(list));
    }

    private documentTypeId: string;
    get DocumentTypeId() { return this.documentTypeId; }
    set DocumentTypeId(value: string) {
        if (this.documentTypeId != value) {
            this.documentTypeId = value;

            this.LoadTemplates();
            this.LoadCopies();
        }
    }
    LoadCopies() {
        throw new Error('Method not implemented.');
    }
    LoadTemplates() {
        var service: DocumentTypeTemplateListExtendedService = new DocumentTypeTemplateListExtendedService();
        service.getDocumentTypeTemplateListsForDocumentType(this.DocumentTypeId, SessionLocator.Tenant).subscribe((res: any) => {
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

    private documentTypeTemplateId: string;
    get DocumentTypeTemplateId() { return this.documentTypeTemplateId; }
    set DocumentTypeTemplateId(value: string) {
        if (this.documentTypeTemplateId != value) {
            this.documentTypeTemplateId = value;
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
        this.SelectedItemsCountText = selectedCount + " of " + this.AllRecordsCount + " " + this.ObjectTable.DBTableName + " selected";
    }
}
