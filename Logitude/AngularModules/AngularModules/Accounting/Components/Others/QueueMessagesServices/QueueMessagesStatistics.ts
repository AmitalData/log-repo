import { EventEmitter, OnInit, Output, Component, ComponentRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QueueMessagesStatExtendedListService } from 'Accounting/Services/ExtendedLists/QueueMessagesStatExtendedListService';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { CustomsClosedTablesComponent } from 'CustomsModules/CustomsMaintenance/Components/CustomsClosedTablesComponent';
import { ListComponentArgs } from 'Infrastructure/Args';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DateTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './QueueMessagesStatistics.html',
})
export class QueueMessagesStatistics extends BaseComponent implements OnInit {
    private _entityListService: QueueMessagesStatExtendedListService;
    public ValidationErrorsList: string[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public ComponentRef: ComponentRef<CustomsClosedTablesComponent>;
    private CurrentSession = SessionLocator.SelectedSession;
    public CurrentTenant = SessionLocator.Tenant;
    public columns: any[] = null;
    formData: FormGroup;
    @Output() onQueryChangeEvent = new EventEmitter();
    public Main_Filter: ApiQueryFilters;
    public FromDateText: string = "Date From: ";
    public ToDateText: string = "To: ";
    public ToStatusText: string = "Status:  ";
    public ToTenantText: string = "Tenant: ";
    public ToRetryNumberText: string = "Retry Number: ";

    public WaitingSinceText: string = " waiting since ";
    public Parag1: string = "Incremental last run on: ";
    public Parag2: string = "Records wating for incremental update: ";
    public Parag3: string = "Oldest update still waiting: ";
    public Parag4: string = "Total records updated in last 10 minutes: ";
    public ObjectTableName: string = "QueueMessage";
    public DataContext: any = this;

    constructor(private fb: FormBuilder) {
        super();
        this._entityListService = new QueueMessagesStatExtendedListService();
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var Day = new Date().getDate();
        this.ToDate = this.SetDate(Year, month, Day);
        this.FromDate = this.SetDate(Year, month, Day);
        this.FromDate.setUTCDate(this.ToDate.getDate() - 7);
    }
    public GridHeaderText:string = "Queue Messages";

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
       if (this.Status != null)
           filters.addAdditionalFilter("Status", this.Status, null, null, "Equals", false, false, false, "string", false, true);
       if (this.Tenant != null)
           filters.addAdditionalFilter("Tenant", this.Tenant, null, null, "Equals", false, false, false, "string", false, true);
       if (this.RetryNumber != null)
           filters.addAdditionalFilter("RetryNumber", this.RetryNumber, null, null, "Equals", false, false, false, "string", false, true);
        return filters;
    }

    public MoreDetails() {      
        this._entityResourceService.getEntityResourceByTableName("ARInvoice", 0).subscribe((response: any) => {
            var listArgs = new ListComponentArgs();
            listArgs.DisplayTitle = "Queue Messages More Details";
            SessionLocator.DynamicLoader.Load('./Accounting/Components/Others/QueueMessagesServices/QueueMessageMoreDetails',
                this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });
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
                Styles: { width: '200px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'NextRunDateTime',
                DataTypeCode: 'DateTime',
                Display: 'Next Run Date Time',
                Styles: { width: '160px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'ProcessingDateTime',
                DataTypeCode: 'DateTime',
                Display: 'Processing Date Time',
                Styles: { width: '160px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'CompleteDateTime',
                DataTypeCode: 'DateTime',
                Display: 'Complete Date Time',
                Styles: { width: '160px' },
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
                FieldName: 'Tenant',
                DataTypeCode: 'Number',
                Display: 'Tenant',
                Styles: { width: '100px' },
                IsCustomTemplate: true
            },
            {
                FieldName: 'HashCode',
                DataTypeCode: 'string',
                Display: 'Hash Code',
                Styles: { width: '200px' },
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
            this.UIProperties.SetValidity("ToDate", "QueueMessage", false, "''To date'' field must be greater than or equal to ''From date'' field");
            this.UIProperties.SetValidity("FromDate", "QueueMessage", false, "''From date'' field must be less than or equal to ''To date'' field");
        }
        else if (diffDays > 7) {
            this.UIProperties.SetValidity("ToDate", "QueueMessage", false, "The date range should be less than or equal to 7 days.");
            this.UIProperties.SetValidity("FromDate", "QueueMessage", false, "The date range should be less than or equal to 7 days.");
        }
        else {

            this.UIProperties.SetValidity("ToDate", "QueueMessage", true, null);
            this.UIProperties.SetValidity("FromDate", "QueueMessage", true, null);
            this.ReloadScreen();
        }
    }


    private status: Number;
    public get Status() { return this.status; }
    public set Status(value: Number) {
        if (this.status != value) {
            this.status = value;
            this.ValidateDate("Status");

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

    private retryNumber: Number;
    public get RetryNumber() { return this.retryNumber; }
    public set RetryNumber(value: Number) {
        if (this.retryNumber != value) {
            this.retryNumber = value;
            this.ValidateDate("RetryNumber");

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
