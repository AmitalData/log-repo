import {ShipmentArchiveFilter} from '../../../../Controls/ShipmentArchiveFilter';
import {TransportsFilter} from '../../../../Controls/TransportsFilter';
import {Component, Output, EventEmitter, OnInit, AfterViewInit} from '@angular/core';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SearchTextBox} from '../../../../Controls/SearchTextBox';
import {IconButton} from '../../../../Controls/IconButton';
import {LogGridComponent} from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import {Http, Response} from '@angular/http';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogBoxDocumentsComponent} from './LogBoxDocumentsComponent';
import {ShipmentDomainService, ImporterQueriesDataCounts} from '../../../../Shipment/Services/ShipmentDomainService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {PortExtendedPMService} from '../../../../Common/Services/ExtendedPMs/PortExtendedPMService';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {EntityStatusExtendedListService} from '../../../../Infrastructure/Services/ExtendedLists/EntityStatusExtendedListService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    moduleId: module.id,
    templateUrl: './MultiArchiveShipmentsComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class MultiArchiveShipmentsComponent extends BaseComponent implements OnInit, AfterViewInit {
    private myShipmentDomainService: ShipmentDomainService;
    public AgentShortName: string = "";
    public IsPrivateLabel: boolean = false;
    private messageWindow: MessageWindow = new MessageWindow();
    //EntityPm: ShipmentPM = new ShipmentPM();
    DataContext: MultiArchiveShipmentsComponent = this;
    public _PortExtendedPMService: PortExtendedPMService;
    public _ShipmentPMService: ShipmentPMService;
    public _EntityStatusExtendedListService: EntityStatusExtendedListService;
    SelectedRecords: any[] = [];
    SelectedItemsCountText: string = null;// + this.dataCount.toString() + " פריטים מתוך " + this.dataCount.toString();
    SelectedRecordsCount: number = 0;
    AllRecordsCount: number = 0;
    public TransportationTypes = [new TransportationTypes("Ocean Haifa", "O", "HFA", "IL"), new TransportationTypes("Ocean Ashdod", "O", "ASH", "IL")];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityListService: EntityListService) {
        super();

        this.myShipmentDomainService = new ShipmentDomainService();
        //if (SessionLocator.PrivateLableSettings) {
            //this.ValidationErrorsList = [];
            //this.AgentShortName = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            //this.IsPrivateLabel = true;
            this._PortExtendedPMService = new PortExtendedPMService();
            this._ShipmentPMService = new ShipmentPMService();
            this._EntityStatusExtendedListService = new EntityStatusExtendedListService();
        //}
    }
    ngOnInit() {
        this.LoadImporterShipments();
    }
    ngAfterViewInit() {

    }
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() ShipmentSelectedEvent = new EventEmitter();
    @Output() OnImporterShipmentsFilterChanged = new EventEmitter();
    @Output() CustomColumnsReady = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    public columns: any[] = null;
    public items: any[] = [];
    public SelectedFilter: string = "My Shipments";
    public RecentImg: string = "./Images/LogBox/Recent.png";
    SearchFilter: string = "";
    SourceEntity: any;
    private mySelectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.mySelectedTransportFilter; }
    set SelectedTransportFilter(newValue: string) {
        if (this.mySelectedTransportFilter != newValue) {
            this.mySelectedTransportFilter = newValue;
            //if (this.filterAgrs == null) {
            //    this.filterAgrs = new ApiQueryFilters();
            //}
            //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'TransportModeId').length > 0) {
            //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'TransportModeId');
            //}
            //this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
            //this.LoadQueriesCounts();
            //this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: this.SelectedTransportFilter == "All" ? true : false });
            this.LoadImporterShipments();
        }
    }
    private mySelectedArchiveFilter: string = "O";
    get SelectedArchiveFilter() { return this.mySelectedArchiveFilter; }
    set SelectedArchiveFilter(newValue: string) {
        if (this.mySelectedArchiveFilter != newValue) {
            this.mySelectedArchiveFilter = newValue;
            //if (this.filterAgrs == null) {
            //    this.filterAgrs = new ApiQueryFilters();
            //}
            //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsOperationalClosed').length > 0) {
            //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsOperationalClosed');
            //}
            //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedTransportFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
            //this.LoadQueriesCounts();
            //this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: this.SelectedTransportFilter == "All" ? true : false });
            this.LoadImporterShipments();
        }
    }
    public AgentShipmentsCount: string;
    public MyShipmentsCount: string;
    public RequestedCount: string;
    public AllShipmentsCount: string;
    public searchFields: string;
    public HasSharedDocs: boolean = true;
    @Output() SearchFieldchangeevent = new EventEmitter();
    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingCol: "StatusDate",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            //if (!this.SelectedFilter) {
            //    this.SelectedFilter = tempo.rowData;
            //    this.ShipmentSelectedEvent.emit(this.SelectedRow);
            //}
            return tempo;
        },
    };
    private GetSourceEntityEvent: any = null;
    SetWindowArgs(args: any) {
        this.SourceEntity = {};//args.SourceEntity;
        this.HasSharedDocs = args.HasSharedDocs;
        if (this.SourceEntity) {
            if (this.SourceEntity.TransportModeId == "O") {
                this.TransportationTypes = [new TransportationTypes("Ashdod", "O", "ASH", "IL"), new TransportationTypes("Haifa", "O", "HFA", "IL"), new TransportationTypes("Eilat", "O", "ETH", "IL")];
            }
            else if (this.SourceEntity.TransportModeId == "I") {
                this.TransportationTypes = [new TransportationTypes("Nitzana", "I", "NZN", "IL"), new TransportationTypes("Arava", "I", "ARV", "IL"), new TransportationTypes("Alenbi", "I", "ALN", "IL"), new TransportationTypes("Jordan", "I", "JOR", "IL")];
            }
            else if (this.SourceEntity.TransportModeId == "A") {
                this.TransportationTypes = [new TransportationTypes("Tel-Aviv", "A", "TLV", "IL")];
            }

        }
        //this.CurrentSession.SessionEvent.subscribe(($event: any) => {
        //    if ($event.Name == "GetSourceEntity") {
        //        this.GetSourceEntity($event.EntityId);
        //    }
        //}); 
    }
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: "MyCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            IsCheckBox: true

        });
        this.columns.push({
            FieldName: "ForwarderShipmentNumber",
            DataTypeCode: 'String',
            Display: 'Shipment #',
            Styles: { width: '220px' },
            HtmlListComponentName: 'ReferenceNumberCellDisplayListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/ReferenceNumberCellDisplayListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ShipperName',
            DataTypeCode: 'String',
            Display: 'Supplier',
            Styles: { width: '175px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StatusName',
            DataTypeCode: 'String',
            Display: 'Status',
            Styles: { width: '120px' },
            HtmlListComponentName: 'StatusCellDisplayListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/StatusCellDisplayListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StatusDate',
            DataTypeCode: 'String',
            Display: 'Status Date',
            Styles: { width: '125px' },
            HtmlListComponentName: 'DateCellDisplayListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/DateCellDisplayListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CustomerReference1',
            DataTypeCode: 'String',
            Display: 'Order #',
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });

        this.CustomColumnsReady.emit(this.columns);
    }
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        //if (filters == null) {
        filters = new ApiQueryFilters();
        filters.SortBy = "StatusDate";
        filters.SortDirection = "Descending";
        //}
        if (!AppTool.IsNullOrEmpty(searchfields)) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, true, false, "String");
            //this.searchFields = searchfields;
        }
        else {
            filters.Filter1Value = "";
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
            //this.searchFields = undefined;
        }
        if (this.filterAgrs != null) {
            if (this.filterAgrs.AdditionalFilters.length > 0) {
                this.filterAgrs.AdditionalFilters.forEach((filter, key) => {
                    if (filters.AdditionalFilters.filter(a => a.FieldName == filter.FieldName).length > 0) {
                        filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != filter.FieldName);
                    }
                    filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, null, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType);
                });
            }
            else {
                var x = filters.AdditionalFilters.filter(a => a.FieldName == "ForwarderShipmentsFilter");
                if (x.length == 0) {
                    filters.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
                }

            }
        }
        else {
            var x = filters.AdditionalFilters.filter(a => a.FieldName == "ForwarderShipmentsFilter");
            if (x.length == 0) {
                filters.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
            }
        }


        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        if (sortingCol) {
            filters.SortBy = sortingCol;
        }
        if (sortingDir) {
            filters.SortDirection = sortingDir;
        }
        filters.Tenant = SessionLocator.Tenant;




        return this._entityListService.getByFilters("Shipment", filters);
    }


    SelectedRow: any;
    onRowSelected(CurrentRow) {
        this.SelectedRow = CurrentRow.rowData;
    }

    private LoadImporterShipments(FilterByOrderNumber: boolean = false) {
        this.IsAllRecordSelected = false;
        this.BuildColumns();
        var last7Date = DateTool.AddDays((new Date()), -7);
        last7Date.setUTCHours(0, 0, 0, 0);
        var last30Date = DateTool.AddDays((new Date()), -30);
        last30Date.setUTCHours(0, 0, 0, 0);
        var last45Date = DateTool.AddDays((new Date()), -45);
        last45Date.setUTCHours(0, 0, 0, 0);
        this.filterAgrs = new ApiQueryFilters();
        //if (FilterByOrderNumber == true) {
        //    if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'CustomerReference1').length > 0) {
        //        this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'CustomerReference1');
        //    }
        //    this.filterAgrs.addAdditionalFilter("CustomerReference1", this.CustomerReference1, null, null, "Equals", false, false, false, "String");
        //}
        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsCancelled').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsCancelled');
        }
        this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");

        if (this.SelectedTransportFilter != "All") {
            this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
        }
        else {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'TransportModeId').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'TransportModeId');
            }
        }

        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsOperationalClosed').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsOperationalClosed');
        }
        this.filterAgrs.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, true, false, "string");


        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ForwarderShipmentsFilter').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ForwarderShipmentsFilter');
        }
        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'NotForwarderShipmentsFilter').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'NotForwarderShipmentsFilter');
        }
        this.filterAgrs.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
        var FilterDate = new Date();
        if (this.SelectedValue == "7d") {
            FilterDate = last7Date;
        }
        else if (this.SelectedValue == "30d") {
            FilterDate = last30Date;
        }
        else if (this.SelectedValue == "45d") {
            FilterDate = last45Date;
        }
        else {
            FilterDate = null;
        }
        if (this.mySelectedFilter.Code == "CLD" && FilterDate != null) {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'CustomsClearanceDate').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'CustomsClearanceDate');
            }
            this.filterAgrs.addAdditionalFilter("CustomsClearanceDate", FilterDate, null, null, "LessThanOrEqual", false, true, false, "Date");

        }
        else if (FilterDate == null) {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'CustomsClearanceDate').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'CustomsClearanceDate');
            }
            this.filterAgrs.addAdditionalFilter("CustomsClearanceDate", FilterDate, null, null, "LessThanOrEqual", false, true, false, "Date", true);
        }

        if (this.mySelectedFilter.Code == "CRD" && FilterDate != null) {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'CreateDateTime').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'CreateDateTime');
            }
            this.filterAgrs.addAdditionalFilter("CreateDateTime", FilterDate, null, null, "LessThanOrEqual", false, true, false, "Date");
        }
        else if (FilterDate == null) {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'CreateDateTime').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'CreateDateTime');
            }
            this.filterAgrs.addAdditionalFilter("CreateDateTime", FilterDate, null, null, "LessThanOrEqual", false, true, false, "Date", true);
        }

        this.filterAgrs.SortBy = "StatusDate";
        this.filterAgrs.SortDirection = "Descending";
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    GridAfterViewInitCompleted($event) {
        this.LoadImporterShipments();
    }

    RefreshBtnClick() {
        this.LoadImporterShipments();
    }

    onSearchTextChangeEvent(event) {
        this.searchFields = event;
        this.SearchFieldchangeevent.emit(this.searchFields);
        this.SelectedRow = null;
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    GetSourceEntity(Id) {
        this.CurrentSession.FireEvent({ Name: 'SourceEntity', Entity: this.SourceEntity, EntityId: Id });
    }

    private isAllRecordSelected: boolean;
    public get IsAllRecordSelected() { return this.isAllRecordSelected };
    public set IsAllRecordSelected(value: boolean) {
        this.isAllRecordSelected = value;
        if (value == true) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("loading ..");
            this._ShipmentPMService.GetTop100ShipmentIds(this.filterAgrs).subscribe(myResult => {
                if (!myResult.HasError) {
                    this.SelectedRecordsCount = myResult.Result.length;
                    this.SelectedRecords = myResult.Result;
                    this.SelectedItemsCountText = this.SelectedRecordsCount + " of " + this.AllRecordsCount + " shipments selected";
                    
                    
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                }
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            });
           
        }
        else {
            this.SelectedItemsCountText = "0 of " + this.AllRecordsCount + " shipments selected";
            this.SelectedRecords = [];
            this.SelectedRecordsCount = 0;
        }

    }
    onCheckBoxChecked(event) {
        var temp = this.SelectedRecords.filter(a => a == event.rowData.Id);
        if (event.IsChecked) {
            if (temp.length == 0) {
                this.SelectedRecords.push(event.rowData.Id);
                this.SelectedRecordsCount++;
            }
        }
        else {
            if (temp.length > 0) {
                this.SelectedRecords = this.SelectedRecords.filter(a => a != event.rowData.Id);
                this.SelectedRecordsCount--;
            }
        }
        this.SelectedItemsCountText = this.SelectedRecordsCount + " of " + this.AllRecordsCount + " shipments selected";
    }

    onCountReady(count) {
        this.AllRecordsCount = count;
        this.SelectedItemsCountText = "0 of " + count + " shipments selected";//this.SelectedItemsCountText + " * " + this.DataSource.rowCount;
    }

    //private selectedTransportationTypes: TransportationTypes;
    //public get SelectedTransportationTypes() {
    //    return this.selectedTransportationTypes;
    //}
    //public set SelectedTransportationTypes(newValue: TransportationTypes) {
    //    this.selectedTransportationTypes = newValue;
    //    //this.TransportModeId = newValue.TransporationType;
    //    //this.ToPortId = newValue.ToPortCode;

    //}

    //onTransportationTypeChange($event) {
    //    this.SelectedTransportationTypes = $event;
    //}

    ValidationErrorsList: any[];


    SaveData() {

        if (this.SelectedRecords.length > 0 || this.IsAllRecordSelected == true) {
            this.StartBusyIndicator("Archiving ...");
            //if (this.IsAllRecordSelected == true) {
            //    this._ShipmentPMService.ArchiveAllShipments(this.filterAgrs).subscribe(myResult => {
            //        if (!myResult.HasError) {
            //            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            //            this.CurrentSession.SessionEvent.emit({ Name: "CustomReloadShipments" });
            //            this.CurrentSession.CurrentWindow.Close("");
            //        }
            //        else {
            //            this.ValidationErrorsList = myResult.ErrorsArray;
            //        }
            //    });
            //}
            //else {
                this._ShipmentPMService.ArchiveShipments(this.SelectedRecords).subscribe(myResult => {
                    if (!myResult.HasError) {
                        this.StopBusyIndicator();
                        this.LoadImporterShipments();
                        this.CurrentSession.SessionEvent.emit({ Name: "CustomReloadShipments" });
                        //this.CurrentSession.CloseCurrentWindow();//.CurrentWindow.Close("");
                    }
                    else {
                        this.ValidationErrorsList = myResult.ErrorsArray;
                    }
                });
            //}
        }

    }

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public StartBusyIndicator(myText: string) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    }

    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }
    public SelectedValue: string = "All";

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.LoadImporterShipments();
        }
    }
    itemMouseOver(itemValue: string) {

    }
    itemMouseLeave(itemValue: string) {
    }

    private myFilters: FilterValue[] = [new FilterValue("CLD", "Days Past Clearance Date"), new FilterValue("CRD", "Days Past Create Date")];
    public get MyFilters() { return this.myFilters; }
    public set MyFilters(newValue: FilterValue[]) {
        this.myFilters = newValue;
    }

    private myselectedFilter: FilterValue = this.myFilters[0];// = ["Days Past Clearance Date", "Days Past Create Date"];
    public get mySelectedFilter() { return this.myselectedFilter; }
    public set mySelectedFilter(newValue: FilterValue) {
        this.myselectedFilter = newValue;
    }

    mySelectedFilterValueChanged(event) {
        this.mySelectedFilter = event;
        this.LoadImporterShipments();
    }
}

export class TransportationTypes {
    constructor(private name: string, private transporationType: string, private toPortCode: string, CountryCode: string) {
        this.Name = name;
        this.TransporationType = transporationType;
        this.ToPortCode = toPortCode;
        this.CountryCode = CountryCode;
    }
    Name: string;
    TransporationType: string;
    ToPortCode: string;
    CountryCode: string;
}

export class FilterValue {

    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }

    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }

}
