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
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {UserExtendedPMService} from '../../../../Common/Services/ExtendedPMs/UserExtendedPMService'; 
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {ShipmentAdditionalCloudDataService} from '../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService';

@Component({
    moduleId: module.id,
    templateUrl: './LogBoxMainComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class LogBoxMainComponent implements OnInit, AfterViewInit {
    private myShipmentDomainService: ShipmentDomainService;
    private myUserPMService: UserExtendedPMService;
    public LogoURL: string = ""
    public preventSelect: boolean = false;
    public DontShowLogboxToolTip: boolean = false;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService;
    public _ShipmentPMService: ShipmentPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public ToggleIsExportShipments: boolean = true;
    constructor(private _entityListService: EntityListService) {
        this.myShipmentDomainService = new ShipmentDomainService();
        this.myUserPMService = new UserExtendedPMService();
        this._ShipmentPMService = new ShipmentPMService();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();
        var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LEX" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (FeatureToggle) {
            this.ToggleIsExportShipments = true;
        }
    }
    ngOnInit() {
        this.DontShowLogboxToolTip = SessionLocator.LoggedUserPM.ShowLogBoxToolTip;
        if (SessionLocator.PrivateLableSettings) {
            this.isPrivateLabel = true;
            this.AgentShipmentsLabel = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Shipments";
            this.LogoURL = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.MainLogo;
            this.SelectedFilter = this.AgentShipmentsLabel;
            this.RequestedDocsLable = "Action Required";
        }
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "ReloadShipments") {
                this.SelectedFilter = "My Shipments";
                this.LoadImporterShipments();
            }
            if ($event.Name == "CustomReloadShipments") {
                this.LoadImporterShipments();
            }
            if ($event.Name == "ReloadPublicShipments") {
                //this.LoadImporterShipments();
                //var element = document.getElementById('row' + this.SelectedRowIndex);
                //if (element) {
                //    element.style.backgroundColor = "#EAEAEB";
                //}
            }

        });
        this.CurrentSession.SubscriptionAdd(
            this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
                if (res == "PreventLogBoxSelect") {
                    this.preventSelect = true;
                }
                else if (res == "AllowLogBoxSelect") {
                    this.preventSelect = false;
                }
            })
        );

        if (SessionLocator.IsExternalParams) {
            if (SessionLocator.ExternalParams) {
                if (SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "logbox") {

                    SessionLocator.ExternalParams.Args.forEach(arg => {
                        if (arg.FieldName == 'SearchField') {
                            this.searchFields = arg.FieldValue;
                            this.SearchFilter = arg.FieldValue;
                        }
                    });
                    this.SelectedFilter = "All Shipments";

                    SessionLocator.ClearExternalParams();

                }
                else if (SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "dapp") {



                    let me: any = SessionLocator.ExternalParams;
                    if (me.ForwarderShipmentNumber) {
                        this.SearchFilter = me.ForwarderShipmentNumber;
                        this._ShipmentPMService.getSingleByForwarderShipmentNumber(me.ForwarderShipmentNumber).subscribe(myResult => {
                            if (!myResult.HasError) {
                                this._ShipmentAdditionalCloudDataService.get(myResult.Result.Id).subscribe(AdditionalResult => {
                                    //this.CurrentSession.StopBusyIndicator();
                                    var newWindow = new LogitudeWindow();
                                    newWindow.Width = 665;
                                    newWindow.Height = 700;
                                    newWindow.RTL = true;
                                    //newWindow.CustomTitleIcon = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.SmallLogo;
                                    newWindow.Title = "אישור היבואן להגשת הצהרת יבוא למכס";
                                    var windowArgs: any = {};
                                    //windowArgs.IsNew = false;
                                    windowArgs.EntityPm = myResult.Result
                                    windowArgs.AdditionalData = AdditionalResult.Result
                                    newWindow.WindowArgs = windowArgs;
                                    //newWindow.Add(control); 
                                    newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelApprovePaymentComponent');
                                    newWindow.WindowClosed.subscribe(($event: any) => {
                                        this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
                                        //if ($event == "MyShipmentAdded") {
                                        //    this.CurrentSession.FireEvent({ Name: 'ReloadShipments' });
                                        //}
                                    });
                                });

                            }
                        });
                    }
                    //this.SelectedFilter = this.RequestedDocsLable;
                    this.SelectedFilter = "All Shipments";
                    SessionLocator.ClearExternalParams();

                }
            }
        }
        this.LoadImporterShipments();

    }
    ngAfterViewInit() {

    }
    ArchiveDone(Ship) {
        //this.LoadImporterShipments();
        if (Ship.Action == "A") {
            var element = document.getElementById('row' + this.SelectedRowIndex);
            if (element) {
                element.style.backgroundColor = "#EAEAEB";
            }
            var elem = document.getElementById(Ship.Ship.Id);
            if (elem) {
                elem.style.visibility = "visible";
            }
        }
        else {
            var element = document.getElementById('row' + this.SelectedRowIndex);
            if (element) {
                element.style.backgroundColor = "#DFECF7";
            }
            var elem = document.getElementById(Ship.Ship.Id);
            if (elem) {
                elem.style.visibility = "hidden";
            }
        }
    }
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() ShipmentSelectedEvent = new EventEmitter();
    @Output() OnImporterShipmentsFilterChanged = new EventEmitter();
    @Output() CustomColumnsReady = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    public columns: any[] = null;
    public items: any[] = [];
    public SelectedFilter: string = "Agent Shipments";
    public RecentImg: string = "./Images/LogBox/Recent.png";
    SearchFilter: string = "";
    private mySelectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.mySelectedTransportFilter; }
    set SelectedTransportFilter(newValue: string) {
        if (this.mySelectedTransportFilter != newValue) {
            this.mySelectedTransportFilter = newValue;
         
            this.LoadImporterShipments();
            ServiceLocator.SendTotangoUserActivity("LogBox", "Transportation type filter changed");
        }
    }
    private mySelectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    set SelectedDirectionFilter(newValue: string) {
        if (this.mySelectedDirectionFilter != newValue) {
            this.mySelectedDirectionFilter = newValue;

            this.LoadImporterShipments();
            ServiceLocator.SendTotangoUserActivity("LogBox", "Direction filter changed");
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
            ServiceLocator.SendTotangoUserActivity("LogBox", "Open/Close filter changed");
        }
    }
    public AgentShipmentsCount: string;
    public MyShipmentsCount: string;
    public RequestedCount: string;
    public AllShipmentsCount: string;
    public searchFields: string;
    public AgentShipmentsLabel: string = "Agent Shipments";
    public RequestedDocsLable: string = "Action Required";
    public isPrivateLabel: boolean = false;
    @Output() SearchFieldchangeevent = new EventEmitter();
    LoadQueriesCounts() {
        this.myShipmentDomainService.GetShipmentsQueriesCounts(SessionLocator.Tenant, this.SelectedTransportFilter == "All" ? "" : this.SelectedTransportFilter, this.SelectedDirectionFilter == "All" ? "" : this.SelectedDirectionFilter, this.SearchFilter == null ? "" : this.SearchFilter, SessionLocator.LoggedUserId, this.SelectedArchiveFilter == "All" ? "" : this.SelectedArchiveFilter).subscribe((myResult: ImporterQueriesDataCounts) => {
            if (myResult != null) {
                this.AgentShipmentsCount = myResult.AgentShipmentsCount > 1000 ? "1000+" : myResult.AgentShipmentsCount.toString();
                this.MyShipmentsCount = myResult.ImporterShipmentsCount > 1000 ? "1000+" : myResult.ImporterShipmentsCount.toString();
                if (this.isPrivateLabel) {
                    this.RequestedCount = myResult.RequiredActionsCount > 1000 ? "1000+" : myResult.RequiredActionsCount.toString();
                }
                else {
                    this.RequestedCount = myResult.RequestedDocsCount > 1000 ? "1000+" : myResult.RequestedDocsCount.toString();
                }
                this.AllShipmentsCount = myResult.AllShipmentsCount > 1000 ? "1000+" : myResult.AllShipmentsCount.toString();
            }
        });
    }
    HoverTemplateIndex: number = 6;
    DataSource = {
        pageSize: 20,
        rowCount: null,
        //sortingCol: "StatusDate",
        //sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            //if (!this.SelectedFilter) {
            //    this.SelectedFilter = tempo.rowData;
            //    this.ShipmentSelectedEvent.emit(this.SelectedRow);
            //}
            return tempo;
        },
    };
    BuildColumns() {
        this.HoverTemplateIndex = 6;
        this.columns = [];
        this.columns.push({
            FieldName: this.SelectedFilter,//"ShipmentNumber",
            DataTypeCode: 'String',
            Display: 'Shipment #',
            Styles: { width: !SessionLocator.PrivateLableSettings ? '220px' : '120px' },
            HtmlListComponentName: 'ReferenceNumberCellDisplayListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/ReferenceNumberCellDisplayListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "ShipmentNumber"
        });
        this.columns.push({
            FieldName: 'ShipperName',
            DataTypeCode: 'String',
            Display: 'Supplier',
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "ShipperName"
        });
        if (this.SelectedFilter == "Action Required") {
            this.columns.push({
                FieldName: 'Task',
                DataTypeCode: 'String',
                Display: 'Task',
                Styles: { width: '220px' },
                HtmlListComponentName: 'TaskCellDisplayListTemplate',
                HtmlListComponentUrl: './Shipment/Components/ListTemplates/TaskCellDisplayListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: false,
                SortByName: "Task"
            });
            this.HoverTemplateIndex = 5;
        }
        else {
            this.columns.push({
                FieldName: 'StatusName',
                DataTypeCode: 'String',
                Display: 'Status',
                Styles: { width: '120px' },
                HtmlListComponentName: 'StatusCellDisplayListTemplate',
                HtmlListComponentUrl: './Shipment/Components/ListTemplates/StatusCellDisplayListTemplate',
                IsCustomTemplate: true,
                ServerSideSortable: true,
                SortByName: "StatusName"
            });
            if (this.isPrivateLabel == false || (this.isPrivateLabel == true && (this.SelectedFilter != "My Shipments" && this.SelectedFilter != "Action Required"))) {
                this.columns.push({
                    FieldName: 'StatusDate',
                    DataTypeCode: 'String',
                    Display: 'Status Date',
                    Styles: { width: '125px' },
                    HtmlListComponentName: 'DateCellDisplayListTemplate',
                    HtmlListComponentUrl: './Shipment/Components/ListTemplates/DateCellDisplayListTemplate',
                    IsCustomTemplate: true,
                    ServerSideSortable: true,
                    SortByName: "StatusDate"
                });
            }
            else {
                this.HoverTemplateIndex = 5;
            }
        }
        
        //this.columns.push({
        //    FieldName: 'RequestedDocumentsCount',
        //    DataTypeCode: 'String',
        //    Display: 'Requested Docs',
        //    Styles: { width: '108px' },
        //    HtmlListComponentName: 'RequestedDocumentsCountListTemplate',
        //    HtmlListComponentUrl: './Shipment/Components/ListTemplates/RequestedDocumentsCountListTemplate',
        //    IsCustomTemplate: true,
        //    ServerSideSortable: true
        //});
        this.columns.push({
            FieldName: 'CustomerReference2',
            DataTypeCode: 'String',
            Display: 'Reference #',
            Styles: { width: '108px' },
            HtmlListComponentName: 'CustomReferenceListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/CustomReferenceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "CustomerReference2"
        });
        this.columns.push({
            FieldName: 'IsOperationalClosed',
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '73px' },
            HtmlListComponentName: 'ArchiveListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/ArchiveListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "IsOperationalClosed"
        });
        if (this.SelectedFilter == "My Shipments") {
            this.columns.push({
                FieldName: 'ActionButtonsListTemplate',//'MyShipments',
                DataTypeCode: 'String',
                Display: '',
                Styles: { width: SessionLocator.PrivateLableSettings ? '270px' : '200px' },
                HtmlListComponentName: 'ActionButtonsListTemplate',
                HtmlListComponentUrl: './Shipment/Components/ListTemplates/ActionButtonsListTemplate',
                IsCustomTemplate: true,
                EnableHoverVisibility: true,
                ServerSideSortable: false
            });
        }
        else if (this.RequestedDocsLable == "Action Required" && this.SelectedFilter == this.RequestedDocsLable && this.isPrivateLabel) {
            this.columns.push({
                FieldName: 'ActionRequired',
                DataTypeCode: 'String',
                Display: '',
                Styles: { width: '346px' },
                HtmlListComponentName: 'ActionButtonsListTemplate',
                HtmlListComponentUrl: './Shipment/Components/ListTemplates/ApprovePaymentButtonListTemplate',
                IsCustomTemplate: true,
                EnableHoverVisibility: true,
                ServerSideSortable: false
            });
        }
        else {
            if ((this.SelectedFilter != "Recent" && this.isPrivateLabel) || !this.isPrivateLabel && (this.RequestedDocsLable == "Action Required" && this.SelectedFilter != this.RequestedDocsLable)) {
                this.columns.push({
                    FieldName: 'EditShipmentButtonListTemplate' + this.SelectedFilter,//this.SelectedFilter,//'EditShipmentButtonListTemplate',
                    DataTypeCode: 'String',
                    Display: '',
                    Styles: { width: '100px' },
                    HtmlListComponentName: 'ActionButtonsListTemplate',
                    HtmlListComponentUrl: './Shipment/Components/ListTemplates/EditShipmentButtonListTemplate',
                    IsCustomTemplate: true,
                    EnableHoverVisibility: true,
                    ServerSideSortable: false
                });
            }
            else if (this.RequestedDocsLable == "Action Required" && this.SelectedFilter == this.RequestedDocsLable) {
                this.columns.push({
                    FieldName: 'RemoveTasksButtonListTemplate',
                    DataTypeCode: 'String',
                    Display: '',
                    Styles: { width: '200px' },
                    HtmlListComponentName: 'RemoveTasksButtonListTemplate',
                    HtmlListComponentUrl: './Shipment/Components/ListTemplates/RemoveTasksButtonListTemplate',
                    IsCustomTemplate: true,
                    EnableHoverVisibility: true,
                    ServerSideSortable: false
                });
            }
        }
       
        this.columns.push({
            FieldName: this.SearchFilter ? this.SearchFilter : '',
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '40px' },
            HtmlListComponentName: 'DocumentSearchResultListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/DocumentSearchResultListTemplate',
            IsCustomTemplate: true,
            EnableHoverVisibility: true,
            ServerSideSortable: false
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

    MenuFiltersClicked(Selected) {
        this.SelectedFilter = Selected;
        this.SelectedRow = null;
        if (Selected == "Recent") {
            this.RecentImg = "./Images/LogBox/RecentW.png";
            this.OnImporterShipmentsFilterChanged.emit({ IsRecentSelected: true, IsRequestedSelected: false });
        }
        else if (Selected == this.RequestedDocsLable) {
            this.RecentImg = "./Images/LogBox/Recent.png";
            this.OnImporterShipmentsFilterChanged.emit({ IsRecentSelected: false, IsRequestedSelected: true });
        }
        else {
            this.RecentImg = "./Images/LogBox/Recent.png";
            this.OnImporterShipmentsFilterChanged.emit({ IsRecentSelected: false, IsRequestedSelected: false });
        }
        this.LoadImporterShipments();
    }
    SelectedRow: any;
    SelectedRowIndex: any;
    onRowSelected(CurrentRow) {
        if (this.preventSelect == false) {
            this.SelectedRow = CurrentRow.rowData;
            this.SelectedRowIndex = CurrentRow.rowIndex;
            this.ShipmentSelectedEvent.emit(this.SelectedRow);
        }
        else {
            this.preventSelect = true;
        }
    }

    private LoadImporterShipments() {
        this.SelectedRow = null;
        this.ShipmentSelectedEvent.emit(this.SelectedRow);
        this.BuildColumns();
        this.LoadQueriesCounts();
        //if (this.filterAgrs == null) {
        this.filterAgrs = new ApiQueryFilters();
        //}
        //if (!string.IsNullOrEmpty(SearchFilter)) {
        //    searchValue = String.IsNullOrEmpty(SearchFilter.Trim()) ? null : SearchFilter.Trim();
        //}
        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ImportersFilter').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ImportersFilter');
        }
        if (!AppTool.IsNullOrEmpty(this.SearchFilter)) {
            this.filterAgrs.addAdditionalFilter("ImportersFilter", this.SearchFilter, null, null, "Contains", true, false, false, "String");
        }

        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsCancelled').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsCancelled');
        }
        this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");

        //}

        //this.filterAgrs.addAdditionalFilter("IsMissingDocument", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("ForwarderShipmentNumber", false, null, null, "Equals", false, false, false, "Boolean");
        //this.filterAgrs.addAdditionalFilter("IsRequestedDocuments", false, null, null, "Equals", false, false, false, "Boolean");
        if (this.SelectedTransportFilter != "All") {
            this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
        }
        else {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'TransportModeId').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'TransportModeId');
            }
        }
        if (this.SelectedDirectionFilter != "All") {
            this.filterAgrs.addAdditionalFilter("DirectionId", this.SelectedDirectionFilter, null, null, "Equals", false, true, false, "string", this.SelectedDirectionFilter == "All" ? true : false);
        }
        else {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'DirectionId').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'DirectionId');
            }
        }
        if (this.SelectedArchiveFilter != "All") {
            this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedArchiveFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", this.SelectedArchiveFilter == "All" ? true : false);
        }
        else {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsOperationalClosed').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsOperationalClosed');
            }
        }

        //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", this.SelectedTransportFilter == "O" ? false : true, null, null, "Equals", false, true, false, "string", true);

        //this.filterAgrs.addAdditionalFilter("ForwarderShipmentNumber", "null", null, null, "Equals", false, true, false, "string", true);

        //this.filterAgrs.addAdditionalFilter("IsRequestedDocuments", true, null, null, "Equals", false, true, false, "string", true);

        if (!AppTool.IsNullOrEmpty(this.SelectedFilter)) {
            if (this.SelectedFilter == this.AgentShipmentsLabel) {
                if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ForwarderShipmentsFilter').length > 0) {
                    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ForwarderShipmentsFilter');
                }
                if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'NotForwarderShipmentsFilter').length > 0) {
                    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'NotForwarderShipmentsFilter');
                }
                this.filterAgrs.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
                //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsOperationalClosed').length > 0) {
                //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsOperationalClosed');
                //}
                //this.filterAgrs.addAdditionalFilter("IsOperationalClosed", false, null, null, "Equals", false, true, false, "Boolean");
                this.filterAgrs.SortBy = "StatusDate";
                this.filterAgrs.SortDirection = "Descending";

            }
            else if (this.SelectedFilter == "Recent") {
                this.filterAgrs.SortBy = "LastDocumentDateTime";
                this.filterAgrs.SortDirection = "Descending";
            }
            else if (this.SelectedFilter == this.RequestedDocsLable) {
                if (this.isPrivateLabel == true) {

                    if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'PrivateLabelActionRequired').length > 0) {
                        this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'PrivateLabelActionRequired');
                    }
                    this.filterAgrs.addAdditionalFilter("PrivateLabelActionRequired", true, null, null, "Equals", true, true, false, "Boolean");
                    //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsImporterApprovalRequried').length > 0) {
                    //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsImporterApprovalRequried');
                    //}
                    //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ApprovedByUserName').length > 0) {
                    //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ApprovedByUserName');
                    //}
                    //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsRequestedDocuments').length > 0) {
                    //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsRequestedDocuments');
                    //}
                    //this.filterAgrs.addAdditionalFilter("IsRequestedDocuments", true, null, null, "Equals", false, true, false, "Boolean");
                    //this.filterAgrs.addAdditionalFilter("IsImporterApprovalRequried", true, null, null, "Equals", false, true, false, "Boolean");
                    //this.filterAgrs.addAdditionalFilter("ApprovedByUserName", "null", null, null, "Equals", false, true, false, "String");
                    this.filterAgrs.SortBy = "MainCarriageExpectedOrActual";
                    this.filterAgrs.SortDirection = "Descending";
                }
                else {
                    if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'NotPrivateLabelActionRequired').length > 0) {
                        this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'NotPrivateLabelActionRequired');
                    }
                    this.filterAgrs.addAdditionalFilter("NotPrivateLabelActionRequired", true, null, null, "Equals", true, true, false, "Boolean");
                    this.filterAgrs.SortBy = "MainCarriageExpectedOrActual";
                    this.filterAgrs.SortDirection = "Descending";
                }
                //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsDigitalSignRequired').length > 0) {
                //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsDigitalSignRequired');
                //}
                //this.filterAgrs.addAdditionalFilter("IsDigitalSignRequired", true, null, null, "Equals", false, true, false, "Boolean");
                //this.filterAgrs.SortBy = "MainCarriageExpectedOrActual";
                //this.filterAgrs.SortDirection = "Descending"; 
            }
            else if (this.SelectedFilter == "All Shipments") {
                this.filterAgrs.SortBy = "StatusDate";
                this.filterAgrs.SortDirection = "Descending";
            }
            else if (this.SelectedFilter == "My Shipments") {
                if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'NotForwarderShipmentsFilter').length > 0) {
                    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'NotForwarderShipmentsFilter');
                }
                if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ForwarderShipmentsFilter').length > 0) {
                    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ForwarderShipmentsFilter');
                }
                this.filterAgrs.addAdditionalFilter("NotForwarderShipmentsFilter", "null", null, null, "Equals", true, true, false, "String");
                this.filterAgrs.SortBy = "StatusDate";
                this.filterAgrs.SortDirection = "Descending";

            }
        }
        else {
            this.filterAgrs.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
            this.filterAgrs.SortBy = "StatusDate";
            this.filterAgrs.SortDirection = "Descending";

        }
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }
    GridAfterViewInitCompleted($event) {
        this.LoadImporterShipments();
    }
    RefreshBtnClick() {
        this.LoadImporterShipments();
    }

    AddNewEntity() {
        var NewShip = new ShipmentPM();
        NewShip.Tenant = SessionLocator.Tenant;
        //AddEditImporterShipmentViewModel viewModel = new AddEditImporterShipmentViewModel(NewShip, shipmentsContext, this);
        //AddEditImporterShipment control = new AddEditImporterShipment() { DataContext = viewModel };

        var newWindow = new LogitudeWindow();
        newWindow.Width = 600;
        newWindow.Height = 350;
        newWindow.Title = "Create New Shipment";
        var windowArgs: any = {};
        windowArgs.IsNew = true;
        newWindow.WindowArgs = windowArgs;
        //newWindow.Add(control);
        if (SessionLocator.PrivateLableSettings) {
            newWindow.Height = 376;
            newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditPrivateLabelShipmentComponent');
        }
        else {
            newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterShipmentComponent');
        }
        newWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "MyShipmentAdded") {
                this.SelectedFilter = "My Shipments";
                this.LoadImporterShipments();
            }
        });
    }

    onSearchTextChangeEvent(event) {
        var temp = null;
        if (event) {
            temp = event.replace(/\s+$/, '');
        }
        this.searchFields = temp;
        this.SearchFilter = temp;
        this.LoadImporterShipments();
        ServiceLocator.SendTotangoUserActivity("LogBox", "SearchFields filter changed");
        //this.SearchFieldchangeevent.emit(this.searchFields);
        //this.SelectedRow = null;
    }

    OnFirstRowSelected(event) {
        this.SelectedRow = event.SelectedRow;
        this.SelectedRowIndex = event.index;
        this.ShipmentSelectedEvent.emit(this.SelectedRow);
    }

    LearnMoreToolTipArea() {
        var newWindow = new LogitudeWindow();
        newWindow.Width = 600;
        newWindow.Height = 350;
        newWindow.Title = "Define document types for digital sign";
        //var windowArgs: any = {};
        //windowArgs.IsNew = true;
        //newWindow.WindowArgs = windowArgs;
        //newWindow.Add(control);

        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/DigitalSignDocTypeComponent');

        newWindow.WindowClosed.subscribe(($event: any) => {
            //if ($event == "MyShipmentAdded") {
            //    this.SelectedFilter = "My Shipments";
            //    this.LoadImporterShipments();
            //}
        });
    }

    CloseToolTipArea() {
        SessionLocator.LoggedUserPM.ShowLogBoxToolTip = true;
        var myPM = SessionLocator.LoggedUserPM;
        this.DontShowLogboxToolTip = true;
        this.myUserPMService.update(myPM).subscribe(myResult => {

        });
    }

    MultiArchiveClicked() {
        var newWindow = new LogitudeWindow();
        newWindow.Width = 1050;
        newWindow.Height = 700;

        newWindow.Title = "Multi Archive Open Shipments"; 

        var windowArgs: any = {};
        //windowArgs.SourceEntity = myResult.Result;//this.rowData;
        //windowArgs.HasSharedDocs = this.HasSharedDocs;
        newWindow.WindowArgs = windowArgs;
        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/MultiArchiveShipmentsComponent');
        newWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
        });
    }
}
