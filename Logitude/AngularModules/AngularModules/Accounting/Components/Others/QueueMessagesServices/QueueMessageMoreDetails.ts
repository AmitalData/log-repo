import { EventEmitter, OnInit, Output, Component, ComponentRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { QueueMessagesMoreDetailsExtendedListService } from 'Accounting/Services/ExtendedLists/QueueMessagesMoreDetailsExtendedListService';
import { CustomsClosedTablesComponent } from 'CustomsModules/CustomsMaintenance/Components/CustomsClosedTablesComponent';
import { ListComponentArgs } from 'Infrastructure/Args';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DateTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './QueueMessageMoreDetails.html',
})
export class QueueMessageMoreDetails extends BaseComponent implements OnInit {
    private _entityListService: QueueMessagesMoreDetailsExtendedListService;
    public ValidationErrorsList: string[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public ComponentRef: ComponentRef<CustomsClosedTablesComponent>;
    private CurrentSession = SessionLocator.SelectedSession;
    public columns: any[] = null;
    formData: FormGroup;
    @Output() onQueryChangeEvent = new EventEmitter();
    public Main_Filter: ApiQueryFilters;
    public FromDateText: string = "Date From: ";
    public ToDateText: string = "To: ";
    public ToStatusText: string = "Status:  ";
    public ToTenantText: string = "Tenant: ";
    public BackBtnTitle:string ="";
    public WaitingSinceText: string = " waiting since ";
    public Parag1: string = "Incremental last run on: ";
    public Parag2: string = "Records wating for incremental update: ";
    public Parag3: string = "Oldest update still waiting: ";
    public Parag4: string = "Total records updated in last 10 minutes: ";
    public ObjectTableName: string = "QueueMessageMoreDetails";
    public DataContext: any = this;

    constructor(private fb: FormBuilder) {
        super();
        this._entityListService = new QueueMessagesMoreDetailsExtendedListService();
        // var month = new Date().getMonth();
        // var Year = new Date().getFullYear();
        // var Day = new Date().getDate();
        // this.ToDate = this.SetDate(Year, month, Day);
        // this.FromDate = this.SetDate(Year, month, Day);
        // this.FromDate.setUTCDate(this.ToDate.getDate() - 14);
    }
    public GridHeaderText: string = "Queue Messages More Details";

    ReloadScreen() {
        this.onQueryChangeEvent.emit({ Filters: this.Main_Filter });
    }

    ngOnInit() {
        this.BuildColumns();
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
    private ImplementFilter(take, skip): ApiQueryFilters {
        var filters = new ApiQueryFilters();
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.addAdditionalFilter("CreateDateTime", this.FromDate, this.ToDate, null, "Between", true, false, false, "DateTime");
        return filters;
    }

    BackButtonClicked() {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
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


    ValidateDate(fieldName: string) {
        var date1 = DateTool.GetDateFromDate(this.FromDate, true);
        var date2 = DateTool.GetDateFromDate(this.ToDate, true);
        var diffDays = 0;
        if (this.FromDate && this.ToDate) {
            diffDays = date2.getDate() - date1.getDate();
        }
        if (DateTool.GetDateFromDate(this.FromDate, true) > DateTool.GetDateFromDate(this.ToDate, true)) {
            this.UIProperties.SetValidity("ToDate", "QueueMessageMoreDetails", false, "''To date'' field must be greater than or equal to ''From date'' field");
            this.UIProperties.SetValidity("FromDate", "QueueMessageMoreDetails", false, "''From date'' field must be less than or equal to ''To date'' field");
        }
        else if (diffDays > 7) {
            this.UIProperties.SetValidity("ToDate", "QueueMessageMoreDetails", false, "The date range should be less than or equal to 7 days.");
            this.UIProperties.SetValidity("FromDate", "QueueMessageMoreDetails", false, "The date range should be less than or equal to 7 days.");
        }
        else {

            this.UIProperties.SetValidity("ToDate", "QueueMessageMoreDetails", true, null);
            this.UIProperties.SetValidity("FromDate", "QueueMessageMoreDetails", true, null);
            this.ReloadScreen();
        }
    }

    private tenant: Number;
    public get Tenant() { return this.tenant; }
    public set Tenant(value: Number) {
        if (this.tenant != value) {
            this.tenant = value;
            this.ValidateDate("Tenant");

        }
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
}
