declare var window: any;
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { DigitalCustomizationService} from '../../../Infrastructure/Services/WebServices/DigitalCustomizationService';

@Component({

    selector: 'AddDigitalLogitudeFieldComponent',
    templateUrl: './AddDigitalLogitudeFieldComponent.html',
})

export class AddDigitalLogitudeFieldComponent extends BaseComponent implements OnInit{
    public SearchText: string = "Search";
    public DataContext: AddDigitalLogitudeFieldComponent = this;
    public ObjectTableId: string;
    public ObjectTableName: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public IsVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    @Output() SearchFieldchangeevent = new EventEmitter();
    public searchFields: string;
    public searchText: string;
    public TenantPM: TenantPM;
    public columns: any[] = [];
    public ObjectFields: any[] = [];
    digitalCustomizationService: DigitalCustomizationService;
    public items: any[] = [];

    constructor(private _entityListService: EntityListService) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.digitalCustomizationService = new DigitalCustomizationService();
    }

    SetWindowArgs(args) {
        this.ObjectTableId = args.ObjectTableId;
    }

    ngOnInit() {
        var ObjectTable = window.ObjectTables.filter(x => x.Id === this.ObjectTableId)[0];
        this.ObjectTableName = ObjectTable.Name;
        this._entityResourceService.getEntityResourceByTableName(ObjectTable.Name, 0).subscribe(response => {
            this.IsVisible = true;
            this.BuildColumns();
        });
    }

    public IsDataReady = false;

    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    public rowCount: number;

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = 0;
        filters.addAdditionalFilter("ObjectTableId", this.ObjectTableId, null, null, "Equals", false, false, false, "string");

        var servicelink = './Infrastructure/Services/WebServices/DigitalCustomizationService';
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(this.digitalCustomizationService.GetObjectFieldsByFilters(filters));
            });
        });
    }

    BuildColumns() {
        var objectTableId = this.ObjectTableId;
        this.ObjectFields = window.ObjectFields.filter(f =>  f.ObjectTableId == objectTableId);
        this.columns = [];

        this.columns.push({
            FieldName: 'FieldCode',
            DataTypeCode: 'String',
            Display: 'Default Name',
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'DataTypeCode',
            DataTypeCode: 'String',
            Display: 'Data Type',
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'IsCustom',
            DataTypeCode: 'boolean',
            Display: 'Is Custom',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'DigitalCheckBoxComponent',
            HtmlListComponentUrl: './SharedLogistics/Components/DigitalPortal/DigitalCheckBoxComponent',
        });
 
        this.columns.push({
            FieldName: this.ObjectTableName,
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '70px' },
            HtmlListComponentName: 'DigitalButtonComponent',
            HtmlListComponentUrl: './SharedLogistics/Components/DigitalPortal/DigitalButtonComponent',
        });
    }

    TextChanged(searchtext) {
        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
