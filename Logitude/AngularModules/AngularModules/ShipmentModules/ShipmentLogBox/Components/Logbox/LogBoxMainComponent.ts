import { ShipmentArchiveFilter } from '../../../../Controls/ShipmentArchiveFilter';
import { TransportsFilter } from '../../../../Controls/TransportsFilter';
import { Component, Output, EventEmitter, OnInit, AfterViewInit } from '@angular/core';
import { ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SearchTextBox } from '../../../../Controls/SearchTextBox';
import { IconButton } from '../../../../Controls/IconButton';
import { LogGridComponent } from '../../../../Infrastructure/Components/LogitudeComponents/LogGridComponent/LogGridComponent'
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentDomainService, ImporterQueriesDataCounts } from '../../../../Shipment/Services/ShipmentDomainService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ServiceLocator } from '../../../../Infrastructure/Locators/ServiceLocator';
import { UserExtendedPMService } from '../../../../Common/Services/ExtendedPMs/UserExtendedPMService';
import { ShipmentPMService } from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import { ShipmentAdditionalCloudDataService } from '../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService';
import { UserLastSettingsPM } from '../../../../Common/EntityPMs/UserLastSettingsPM';
import { UserLastSettingsPMService } from '../../../../Common/Services/StandardPMs/UserLastSettingsPMService';
import { UserLastSettingsExtendedPMService } from '../../../../Common/Services/ExtendedPMs/UserLastSettingsExtendedPMService';
import { QueryColumnPM} from '../../../../Infrastructure/EntityPMs/QueryColumnPM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogboxShipmentExportExcelArgs } from '../../../../Shipment/DataContract/LogboxShipmentExportExcelArgs';


@Component({
    
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
    public ToggleIsExportShipments: boolean = false;
    public _UserLastSettingsPMService: UserLastSettingsPMService;
    public _UserLastSettingsExtendedPMService: UserLastSettingsExtendedPMService;

    RefTemplateWidth: string = '220px';
    constructor(private _entityListService: EntityListService) {
        this.myShipmentDomainService = new ShipmentDomainService();
        this.myUserPMService = new UserExtendedPMService();
        this._ShipmentPMService = new ShipmentPMService();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();

        this._UserLastSettingsPMService = new UserLastSettingsPMService();
        this._UserLastSettingsExtendedPMService = new UserLastSettingsExtendedPMService();
        var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LEX" && d.TenantNumber == SessionLocator.Tenant)[0];
        if (FeatureToggle) {
            this.ToggleIsExportShipments = true;
        }
    }
    ngOnInit() {
        //!SessionLocator.PrivateLableSettings ? '250px' : '150px'
        this.DontShowLogboxToolTip = SessionLocator.LoggedUserPM.ShowLogBoxToolTip;
        if (SessionLocator.PrivateLableSettings) {
            this.isPrivateLabel = true;
            this.AgentShipmentsLabel = SessionLocator.PrivateLableSettings.PrivateLabelShortName + " Shipments";
            this.LogoURL = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.MainLogo;
            this.SelectedFilter = this.AgentShipmentsLabel;
            this.RequestedDocsLable = "Action Required";
            if (this.ToggleIsExportShipments) {
                this.RefTemplateWidth = '150px';
            }
            else {
                this.RefTemplateWidth = '120px';
            }
        }
        else {
            if (this.ToggleIsExportShipments) {
                this.RefTemplateWidth = '250px';
            }
            else {
                this.RefTemplateWidth = '220px';
            }
        }
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "ReloadShipments") {
                this.SelectedFilter = "My Shipments";
                this.LoadImporterShipments();
                console.log("5");
            }
            if ($event.Name == "CustomReloadShipments") {
                this.LoadImporterShipments();
                console.log("6");
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
                        this._ShipmentPMService.getSingleByForwarderShipmentNumber(me.ForwarderShipmentNumber).subscribe((myResult:any) => {
                            if (!myResult.HasError) {
                                this._ShipmentAdditionalCloudDataService.get(myResult.Result.Id).subscribe((AdditionalResult:any) => {
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
        //this.setUserLastSettings();
        

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
            console.log("2");
            this.SaveUserLastSettings("SelectedTransportFilter", this.mySelectedTransportFilter);
            ServiceLocator.SendTotangoUserActivity("LogBox", "Transportation type filter changed");
        }
    }
    private mySelectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    set SelectedDirectionFilter(newValue: string) {
        if (this.mySelectedDirectionFilter != newValue) {
            this.mySelectedDirectionFilter = newValue;

            this.LoadImporterShipments();
            console.log("3");
            this.SaveUserLastSettings("SelectedDirectionFilter", this.mySelectedDirectionFilter);
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
            console.log("4");
            this.SaveUserLastSettings("SelectedArchiveFilter", this.mySelectedArchiveFilter);
            ServiceLocator.SendTotangoUserActivity("LogBox", "Open/Close filter changed");
           
        }
    }

    SaveUserLastSettings(FilterName: string, FilterValue: string) {
       
        this._UserLastSettingsExtendedPMService.getsingleByUserIdFilterName(SessionLocator.LoggedUserId, FilterName).subscribe((Result:any) => {
            if (!Result.HasError && Result.Result == null) {
                var myFilterSettings = new UserLastSettingsPM();
                myFilterSettings.FilterName = FilterName;
                myFilterSettings.FilterValue = FilterValue;
                myFilterSettings.ControlNameSpace = "ShipmentModules.ShipmentLogBox.LogBoxMainComponent";
                myFilterSettings.Tenant = SessionLocator.Tenant;
                myFilterSettings.UserId = SessionLocator.LoggedUserId;
                this._UserLastSettingsPMService.insert(myFilterSettings).subscribe((myResult:any) => {
                });
            }
            else if (!Result.HasError && Result.Result != null) {
                var myUpdatedFilterSettings = Result.Result;
                myUpdatedFilterSettings.FilterName = FilterName;
                myUpdatedFilterSettings.FilterValue = FilterValue;
                myUpdatedFilterSettings.ControlNameSpace = "ShipmentModules.ShipmentLogBox.LogBoxMainComponent";
                myUpdatedFilterSettings.Tenant = SessionLocator.Tenant;
                myUpdatedFilterSettings.UserId = SessionLocator.LoggedUserId;
                this._UserLastSettingsPMService.update(myUpdatedFilterSettings).subscribe((myResult:any) => {
                });
            }
        });
        
    }

    setUserLastSettings() {

        this._UserLastSettingsExtendedPMService.getallByUserIdNameSpace(SessionLocator.LoggedUserId, "ShipmentModules.ShipmentLogBox.LogBoxMainComponent").subscribe((Result:any) => {
            if (!Result.HasError && Result.Result != null) {
                var myFiltersSettings = Result.Result;
           
                var myArchiveFilter = myFiltersSettings.filter(a => a.FilterName == 'SelectedArchiveFilter');
                var myDirectionFilter = myFiltersSettings.filter(a => a.FilterName == 'SelectedDirectionFilter');
                var myTransportFilter = myFiltersSettings.filter(a => a.FilterName == 'SelectedTransportFilter');

                if (myArchiveFilter.length > 0) {
                    this.mySelectedArchiveFilter = myArchiveFilter[0].FilterValue;
                }
                if (myDirectionFilter.length > 0) {
                    this.mySelectedDirectionFilter = myDirectionFilter[0].FilterValue;
                }
                if (myTransportFilter.length > 0) {
                    this.mySelectedTransportFilter = myTransportFilter[0].FilterValue;
                } 
            }
            this.LoadImporterShipments();
            //this.LoadImporterShipments();
        });

    }

    public AgentShipmentsCount: string;
    public MyShipmentsCount: string;
    public RequestedCount: string;
    public AllShipmentsCount: string;
    public searchFields: string = null;
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
        //sortingCol: "ComputedStatusDate",
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


    GetQueryColumn(fieldName: string, dataTypeCode: string, displayText:string) {
        var queryColum: QueryColumnPM = new QueryColumnPM();
        queryColum.ObjectFieldDataTypeCode = dataTypeCode;
        queryColum.ObjectFieldName = fieldName;
        queryColum.DisplayText = displayText;
        queryColum.ObjectFieldListLabelTextCodeCode = fieldName;
        return queryColum;
        

    }

    QueryColumns: QueryColumnPM[] = [];


    BuildColumns() {

        this.QueryColumns = [];
        this.HoverTemplateIndex = 6;
        this.columns = [];
        this.columns.push({
            FieldName: this.SelectedFilter,//"ShipmentNumber",
            DataTypeCode: 'String',
            Display: 'Shipment #',
            Styles: { width: this.RefTemplateWidth },
            HtmlListComponentName: 'ReferenceNumberCellDisplayListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/ReferenceNumberCellDisplayListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "ShipmentNumber"
        });

        this.QueryColumns.push(this.GetQueryColumn("PartnerName", 'Text', 'Agent Name'));
        this.QueryColumns.push(this.GetQueryColumn(("ShipmentNumber_" + this.SelectedFilter.replace(" ", "")), 'Text', 'Shipment #'));

        this.columns.push({
            FieldName: 'ShipperName',
            DataTypeCode: 'String',
            Display: 'Supplier',
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "ShipperName"
        });
        this.QueryColumns.push(this.GetQueryColumn("ShipperName", 'Text', 'Supplier'));



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


            this.QueryColumns.push(this.GetQueryColumn("Task", 'Text', 'Task'));


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
            this.QueryColumns.push(this.GetQueryColumn("StatusName", 'Text', 'Status'));




            if (this.isPrivateLabel == false || (this.isPrivateLabel == true && (this.SelectedFilter != "My Shipments" && this.SelectedFilter != "Action Required"))) {
                this.columns.push({
                    FieldName: 'ComputedStatusDate',
                    DataTypeCode: 'String',
                    Display: 'Status Date',
                    Styles: { width: '125px' },
                    HtmlListComponentName: 'DateCellDisplayListTemplate',
                    HtmlListComponentUrl: './Shipment/Components/ListTemplates/DateCellDisplayListTemplate',
                    IsCustomTemplate: true,
                    ServerSideSortable: true,
                    SortByName: "ComputedStatusDate"
                });
                this.QueryColumns.push(this.GetQueryColumn("ComputedStatusDate", 'DateTime', 'Status Date' ));

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

        this.QueryColumns.push(this.GetQueryColumn("CustomerReference", 'Text', 'Reference #'));


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

        this.QueryColumns.push(this.GetQueryColumn("IsOperationalClosed", 'Boolean', 'Operational Closed' ));


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
                    Styles: { width: '300px' },
                    HtmlListComponentName: 'RemoveTasksButtonListTemplate',
                    HtmlListComponentUrl: './Shipment/Components/ListTemplates/RemoveTasksButtonListTemplate',
                    IsCustomTemplate: true,
                    EnableHoverVisibility: true,
                    ServerSideSortable: false
                });
            }
        }
        if (this.RequestedDocsLable != "Action Required") {
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



        }
        this.CustomColumnsReady.emit(this.columns);
    }
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        //if (filters == null) {
        filters = new ApiQueryFilters();
        filters.SortBy = "ComputedStatusDate";
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

    GetExportToExcelArgs() {
        this.filterAgrs = this.GetApiQueryFilters();
        this.filterAgrs.Tenant = SessionLocator.Tenant;
        var logboxShipmentExportExcelArgs: LogboxShipmentExportExcelArgs = new LogboxShipmentExportExcelArgs();
        logboxShipmentExportExcelArgs.Tenant = SessionLocator.Tenant;
        logboxShipmentExportExcelArgs.AdditionalFilters = this.filterAgrs.AdditionalFilters;
        logboxShipmentExportExcelArgs.PageIndex = this.filterAgrs.PageIndex;
        logboxShipmentExportExcelArgs.PageSize = this.filterAgrs.PageSize;
        logboxShipmentExportExcelArgs.ObjectTableName = "Shipment";
        logboxShipmentExportExcelArgs.QueryColumns = this.QueryColumns;
        logboxShipmentExportExcelArgs.UserId = SessionLocator.LoggedUserId;
        logboxShipmentExportExcelArgs.SortBy = this.filterAgrs.SortBy;
        logboxShipmentExportExcelArgs.SortDirection = this.filterAgrs.SortDirection;
        logboxShipmentExportExcelArgs.QueryName = this.SelectedFilter;
        logboxShipmentExportExcelArgs.QuerySection = "Shipment";
        logboxShipmentExportExcelArgs.Filters = this.filterAgrs; 
        return logboxShipmentExportExcelArgs;
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
        console.log("7");
    }
    SelectedRow: any;
    SelectedRowIndex: any;
    RowSelectedTimerToken: any;
    onRowSelected(CurrentRow) {
        if (this.RowSelectedTimerToken) {
            clearTimeout(this.RowSelectedTimerToken);
        }
        this.RowSelectedTimerToken = setTimeout(() => this.TriggerShipmentSelectedEvent(CurrentRow), 500); 
    }

    TriggerShipmentSelectedEvent(CurrentRow) {
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
        console.log("LoadImporterShipments");
        
        this.SelectedRow = null;
        this.ShipmentSelectedEvent.emit(this.SelectedRow);
        this.BuildColumns();
        this.LoadQueriesCounts();
        //if (this.filterAgrs == null) {
        
        this.filterAgrs = this.GetApiQueryFilters();

        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }


    GetApiQueryFilters() {
        this.filterAgrs = new ApiQueryFilters();
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
                this.filterAgrs.SortBy = "ComputedStatusDate";
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
                this.filterAgrs.SortBy = "ComputedStatusDate";
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
                this.filterAgrs.SortBy = "ComputedStatusDate";
                this.filterAgrs.SortDirection = "Descending";

            }
        }
        else {
            this.filterAgrs.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
            this.filterAgrs.SortBy = "ComputedStatusDate";
            this.filterAgrs.SortDirection = "Descending";

        }

        return this.filterAgrs;

    }

    GridAfterViewInitCompleted($event) {
        this.setUserLastSettings();
        
        console.log("8");
    }
    timerToken: any;
    RefreshBtnClick() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        this.timerToken = setTimeout(() => this.LoadImporterShipments(), 500);
    }


    btnExcelCLicked() {
        var windowArgs: any = {};
        windowArgs.ExportExcelArgs = this.GetExportToExcelArgs();

        windowArgs.tenant = SessionLocator.Tenant;
        windowArgs.ObjectTableName = "Shipment";
        windowArgs.QueryName = this.SelectedFilter;
        windowArgs.QueryType = "LogBox";
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");//"Exporting View Data List To Excel File";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');


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
        if (this.searchFields != temp) {
            this.searchFields = temp;
            this.SearchFilter = temp;
            //if (this.searchFields != temp) {
            this.LoadImporterShipments();
            ServiceLocator.SendTotangoUserActivity("LogBox", "SearchFields filter changed");
            //}
            //this.SearchFieldchangeevent.emit(this.searchFields);
            //this.SelectedRow = null;
        }
    }

    FirstRowSelectedTimerToken: any;
    OnFirstRowSelected(event) {
        if (this.FirstRowSelectedTimerToken) {
            clearTimeout(this.FirstRowSelectedTimerToken);
        }
        this.FirstRowSelectedTimerToken = setTimeout(() => this.FirstShipmentSelectedEvent(event), 500);
    }

    FirstShipmentSelectedEvent(event) {
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
        this.myUserPMService.update(myPM).subscribe((myResult:any) => {

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

