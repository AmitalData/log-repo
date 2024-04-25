import { Component, OnInit, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { DateTool } from 'Infrastructure/Tools';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { QueueMessageMoreDetailsList } from 'Infrastructure/EntityLists/QueueMessageMoreDetailsList';
import { QueueMessagesMoreDetailsExtendedListService } from 'Accounting/Services/ExtendedLists/QueueMessagesMoreDetailsExtendedListService';
import { buildColumns } from './dynamic-columns-builder';


@Component({

    templateUrl: './QueueMessageMoreDetails.html',
})

export class QueueMessageMoreDetails extends BaseComponent implements OnInit {
    private _entityListService: QueueMessagesMoreDetailsExtendedListService;//simon
    public _CargoTrackingIncrementalArgs: CargoTrackingIncrementalArgs;
    public ValidationErrorsList: string[] = [];
    public DataContext: any = this;
    public ObjectTableName: string = "CargoTrackingIncrementalStat";
    public Main_Filter: ApiQueryFilters;
    @Output() onQueryChangeEvent = new EventEmitter();
    constructor() {
        super();
        this._CargoTrackingIncrementalArgs = new CargoTrackingIncrementalArgs();
        this._entityListService = new QueueMessagesMoreDetailsExtendedListService();
    }

    public GridHeaderText: string = "Incremental Records Details";

    ngOnInit() {
        this.InitializeDate();
        this.BuildColumns();
    }
    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.ValidateDate("FromDate");

        }
    }
    private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.ValidateDate("ToDate");

        }
    }
    public DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.Main_Filter = this.ImplementFilter(take, skip);
        return this._entityListService.getByFiltersForVirtualization(this.Main_Filter);
    }

    private ImplementFilter(take, skip): ApiQueryFilters {
        var filters = new ApiQueryFilters();
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.addAdditionalFilter("Start_End_Date", this.FromDate, this.ToDate, null, "Between", true, false, false, "DateTime");
        return filters;
    }
    public columns: any[] = null;

    BuildColumns() {
        this.columns = [
            {
                FieldName: 'Id',
                DataTypeCode: 'string',
                Display: 'Id',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'QueueDefinitionCode',
                DataTypeCode: 'string',
                Display: "Queue Definition Code",
                Styles: { width: '160px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'CreateDateTime',
                DataTypeCode: 'DateTime',
                Display: 'Create Date Time',
                Styles: { width: '160px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'Status',
                DataTypeCode: 'Number',
                Display: 'Status',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'MessageBody',
                DataTypeCode: 'string',
                Display: 'Message Body',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'NextRunDateTime',
                DataTypeCode: 'DateTime',
                Display: 'Next Run Date Time',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'ProcessingDateTime',
                DataTypeCode: 'DateTime',
                Display: 'Processing Date Time',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'CompleteDateTime',
                DataTypeCode: 'DateTime',
                Display: 'Complete Date Time',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'RetryNumber',
                DataTypeCode: 'Number',
                Display: 'Retry Number',
                Styles: { width: '130px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'Field1',
                DataTypeCode: 'string',
                Display: 'Field 1',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'Field2',
                DataTypeCode: 'string',
                Display: 'Field 2',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'Field3',
                DataTypeCode: 'string',
                Display: 'Field 3',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'StatusName',
                DataTypeCode: 'string',
                Display: 'Status Name',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            }
        ];
    }


    // BuildColumns() {
    //     this.columns = buildColumns(QueueMessageMoreDetailsList);
    // } TODO: find generic solution for this




    ReloadScreen() {

        this.onQueryChangeEvent.emit({ Filters: this.Main_Filter }); // refresh grid
    }
    ValidateDate(fieldName: any) {
        var date1 = DateTool.GetDateFromDate(this.FromDate, true);
        var date2 = DateTool.GetDateFromDate(this.ToDate, true);
        var diffDays = 0;
        if (this.FromDate && this.ToDate) {
            diffDays = date2.getDate() - date1.getDate();
        }
        if (DateTool.GetDateFromDate(this.FromDate, true) > DateTool.GetDateFromDate(this.ToDate, true)) {
            this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, "''To date'' field must be greater than or equal to ''From date'' field");
            this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, "''From date'' field must be less than or equal to ''To date'' field");
        }
        else if (diffDays > 7) {
            this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, "The date range should be less than or equal to 7 days.");
            this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, "The date range should be less than or equal to 7 days.");
        }
        else {

            this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, null);
            this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, null);
            this.ReloadScreen();
        }


    }
    InitializeDate() {

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var Day = new Date().getDate();
        this.ToDate = this.SetDate(Year, month, Day);
        this.FromDate = this.SetDate(Year, month, Day);
        this.FromDate.setUTCDate(this.ToDate.getDate() - 7);
    }

    SetDate(year: number, month: number, day: number) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        date.setUTCMilliseconds(0);

        return date;
    }
    public FromDateText: string = "Date From: ";
    public ToDateText: string = "To: ";
    public WaitingSinceText: string = " waiting since ";

    RefreshButtonClicked() {
        this.ReloadScreen();
    }
}

export class CargoTrackingIncrementalArgs {
    public RecordsWating: number;
    public IncrementalLastRun: Date;
    public OldestUpdateStillWaiting: string;
    public TotalUpdatedLast10Minutes: number;
    public DateOfOldestUpdateStillWaiting: Date;
}




