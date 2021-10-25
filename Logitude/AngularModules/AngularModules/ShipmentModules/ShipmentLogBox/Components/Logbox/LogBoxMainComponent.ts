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
import { ShipmentDomainService, ImporterQueriesDataCounts, ShipmentsQueriesCountsArgs } from '../../../../Shipment/Services/ShipmentDomainService';
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
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService'; 
import { SystemEnvironmentService } from '../../../../Infrastructure/Utilities/SystemEnvironmentService';
import { CustomerTenantAccessRequestExtendedPMService } from '../../../../Common/Services/ExtendedPMs/CustomerTenantAccessRequestExtendedPMService';

@Component({
    templateUrl: './LogBoxMainComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class LogBoxMainComponent implements OnInit, AfterViewInit {
    private myShipmentDomainService: ShipmentDomainService;
    private myUserPMService: UserExtendedPMService;
    public LogoURL: string = ""
    public MainColor: string = "#1B90CB";
    public SecondaryColor: string = "transparent";  
    public IsDSV: boolean = false;
    public preventSelect: boolean = false;
    public DontShowLogboxToolTip: boolean = false;
    public _ShipmentAdditionalCloudDataService: ShipmentAdditionalCloudDataService;
    public _ShipmentPMService: ShipmentPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public ToggleIsExportShipments: boolean = false;
    public _UserLastSettingsPMService: UserLastSettingsPMService;
    public _UserLastSettingsExtendedPMService: UserLastSettingsExtendedPMService;
    RefTemplateWidth: string = '220px';
    public IsLongAgentName: boolean = false;
    public AgentLabelClass = {
        "ShortName": true,
        "LongName": false
    }
    private entityResourceService: EntityResourceService;
    public HasExportShipmentToggle: boolean = false;

    public IsPrivateLabelExportActivated: boolean = false;
    public IsPrivateLabelImportActivated: boolean = false;

    public IsExportActivated: boolean = true;
    public IsImportActivated: boolean = true;
    public customerTenantAccessRequestExtendedPMService: CustomerTenantAccessRequestExtendedPMService;

    public isLogbox: boolean = SystemEnvironmentService.IsLogBox();

    constructor(private _entityListService: EntityListService) {
        this.InitializeServices();
        this.LoadEntityResource("Shipment");
        var FeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LEX")[0];
        if (FeatureToggle) {
            this.ToggleIsExportShipments = true;
        } 
    }
     
    private InitializeServices() {
        this.myShipmentDomainService = new ShipmentDomainService();
        this.myUserPMService = new UserExtendedPMService();
        this._ShipmentPMService = new ShipmentPMService();
        this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService();
        this._UserLastSettingsPMService = new UserLastSettingsPMService();
        this._UserLastSettingsExtendedPMService = new UserLastSettingsExtendedPMService();
        this.entityResourceService = new EntityResourceService();
        this.customerTenantAccessRequestExtendedPMService = new CustomerTenantAccessRequestExtendedPMService();
    }
  

    ngOnInit() {
        this.DontShowLogboxToolTip = SessionLocator.LoggedUserPM.ShowLogBoxToolTip;
        this.handlePrivateLable();
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
        this.handleExternalParams();
    }

    private handlePrivateLable() {
        if (SessionLocator.PrivateLableSettings) {
            this.isPrivateLabel = true;
            this.IsDSV = SessionLocator.PrivateLableSettings.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1;
            let AgentName = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            this.LogoURL = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.MainLogo;
            this.MainColor = SessionLocator.PrivateLableSettings.MainColor;
            this.SecondaryColor = SessionLocator.PrivateLableSettings.SecondaryColor;
            this.SelectedFilter = this.AgentShipmentsLabel;
            this.setAgentLabelClass(AgentName);
            this.AgentShipmentsLabel = this.getAgentShipmentsLabel(AgentName);
            this.SelectedFilter = this.AgentShipmentsLabel;
            this.RequestedDocsLable = "Action Required";
            this.RefTemplateWidth = '150px';
            this.IsPrivateLabelExportActivated = SessionLocator.PrivateLableSettings.IsExportActivated;
            this.IsPrivateLabelImportActivated = SessionLocator.PrivateLableSettings.IsImportActivated;
            this.GetCustomerTenantAccessRequests(SessionLocator.PrivateLableSettings.HybridPartnerId);

        }
        else {
            this.RefTemplateWidth = this.ToggleIsExportShipments ? '250px' : '220px';
        }
    }
    GetCustomerTenantAccessRequests(hybridPartnerId: any) {

        var tenant = SessionLocator.Tenant;
        this.customerTenantAccessRequestExtendedPMService.getByForwarderId(tenant,hybridPartnerId).subscribe((res: any) => {
            if (!res.HasError) {

                this.IsImportActivated = res.Result.IsCustoms;
                this.IsExportActivated = res.Result.IsExport; 
               
            }
            else {
                //this.ValidationErrorsList = res.ErrorsArray;
                
            }
        });
    }

    private getAgentShipmentsLabel(agentName: string) {
        let agentShipmentsLabel = "";
        if (agentName.length < 10 && this.AgentLabelClass.LongName) {
            agentShipmentsLabel = agentName + '\n' + " Shipments";
        } else {

            agentShipmentsLabel = agentName + " Shipments";
        }
        return agentShipmentsLabel;
    }

    private setAgentLabelClass(agentName: string) {
        if (agentName.length > 7) {
            this.AgentLabelClass.ShortName = false;
            this.AgentLabelClass.LongName = true;
        }
    }

    private handleExternalParams() {
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
                        this._ShipmentPMService.getSingleByForwarderShipmentNumber(me.ForwarderShipmentNumber).subscribe((myResult: any) => {
                            if (!myResult.HasError) {
                                this._ShipmentAdditionalCloudDataService.get(myResult.Result.Id).subscribe((AdditionalResult: any) => {
                                    //this.CurrentSession.StopBusyIndicator();
                                    var newWindow = new LogitudeWindow();
                                    newWindow.Width = 665;
                                    newWindow.Height = 700;
                                    newWindow.RTL = true;
                                    //newWindow.CustomTitleIcon = "data:image/JPEG;base64," + SessionLocator.PrivateLableSettings.SmallLogo;
                                    newWindow.Title = "םישור היבוםן להגשת הצהרת יבום למכס";
                                    var windowArgs: any = {};
                                    //windowArgs.IsNew = false;
                                    windowArgs.EntityPm = myResult.Result;
                                    windowArgs.AdditionalData = AdditionalResult.Result;
                                    newWindow.WindowArgs = windowArgs; 
                                    //newWindow.Add(control); 
                                    let privateLabelApprovePaymentComponentPath = this.GetPrivateLabelApprovePaymentComponentPath();
                                    newWindow.Show(privateLabelApprovePaymentComponentPath);
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
    }

    GetPrivateLabelApprovePaymentComponentPath() {
        let isDSV = SessionLocator?.PrivateLableSettings?.PrivateLabelDomain?.toLowerCase()?.indexOf("dsv") > -1;
        let privateLabelApprovePaymentComponentPath = './ShipmentModules/ShipmentLogBox/Components/Logbox/' + (isDSV ? 'DSVApprovePaymentComponent' : 'PrivateLabelApprovePaymentComponent');
        return privateLabelApprovePaymentComponentPath;
    }

    ngAfterViewInit() {

    }

    ArchiveDone(Ship) {
        var element = document.getElementById('row' + this.SelectedRowIndex);
        if (element) {
            element.style.backgroundColor = Ship.Action == "A" ? "#EAEAEB" : "#DFECF7";
        }
        var elem = document.getElementById(Ship.Ship.Id);
        if (elem) {
            elem.style.visibility = Ship.Action == "A" ? "visible" : "hidden";
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
            this.LoadImporterShipments();
            console.log("4");
            this.SaveUserLastSettings("SelectedArchiveFilter", this.mySelectedArchiveFilter);
            ServiceLocator.SendTotangoUserActivity("LogBox", "Open/Close filter changed");

        }
    }

    SaveUserLastSettings(FilterName: string, FilterValue: string) {
        this._UserLastSettingsExtendedPMService.getsingleByUserIdFilterName(SessionLocator.LoggedUserId, FilterName).subscribe((Result:any) => {
            if (!Result.HasError && Result.Result == null) {
                this.upsertUserLastSettings(FilterName, FilterValue);
            }
            else if (!Result.HasError && Result.Result != null) {
                this.upsertUserLastSettings(FilterName, FilterValue, Result);
            }
        });
    }

    private upsertUserLastSettings(FilterName: string, FilterValue: string, Result: any = null, ) {
        let myFilterSettings = new UserLastSettingsPM();
        if (Result != null) myFilterSettings = Result.Result;
        myFilterSettings.FilterName = FilterName;
        myFilterSettings.FilterValue = FilterValue;
        myFilterSettings.ControlNameSpace = "ShipmentModules.ShipmentLogBox.LogBoxMainComponent";
        myFilterSettings.Tenant = SessionLocator.Tenant;
        myFilterSettings.UserId = SessionLocator.LoggedUserId;
        if (Result != null) {
            this._UserLastSettingsPMService.update(myFilterSettings).subscribe((myResult: any) => {
            });
        }
        else {
            this._UserLastSettingsPMService.insert(myFilterSettings).subscribe((myResult: any) => {
            });
        }
    }

    setUserLastSettings() {
        const nameSpace = "ShipmentModules.ShipmentLogBox.LogBoxMainComponent";
        this._UserLastSettingsExtendedPMService.getallByUserIdNameSpace(SessionLocator.LoggedUserId, nameSpace).subscribe((Result:any) => {
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
        const shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs  = {
            Tenant: SessionLocator.Tenant,
            TransportModeId: this.SelectedTransportFilter == "All" ? "" : this.SelectedTransportFilter,
            DirectionId: this.SelectedDirectionFilter == "All" ? "" : this.SelectedDirectionFilter,
            SearchFilter: this.SearchFilter == null ? "" : this.SearchFilter,
            ServiceContextUser: SessionLocator.LoggedUserId,
            TypeCode: this.SelectedArchiveFilter == "All" ? "" : this.SelectedArchiveFilter,
            ForwarderPartnerId: this.isPrivateLabel && !this.IsDSV ? SessionLocator.PrivateLableSettings.HybridPartnerId : '',
            DirectionOperator : 'Equal',
        };
        if (this.isLogbox && !shipmentsQueriesCountsArgs.DirectionId) this.SetDirectionFilter(shipmentsQueriesCountsArgs);

        this.SetPrivateLabelShipmentsFilter(shipmentsQueriesCountsArgs);


       
        this.myShipmentDomainService.GetShipmentsQueriesCounts(shipmentsQueriesCountsArgs).subscribe((myResult: ImporterQueriesDataCounts) => {
            if (myResult != null) {
                this.setAllShipmentsQueriesCounts(myResult);
            }
        });
    }

    HoverTemplateIndex: number = 6;

    DataSource = {
        pageSize: 20,
        rowCount: null,
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    private SetPrivateLabelShipmentsFilter(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) { 
        this.SetPrivateLabelExportFilter(shipmentsQueriesCountsArgs);
        this.SetPrivateLabelImportFilter(shipmentsQueriesCountsArgs);
    }

    private SetPrivateLabelImportFilter(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) {
        if (this.IsExecludedImport(shipmentsQueriesCountsArgs))
            this.SetExcludeImportFilter(shipmentsQueriesCountsArgs);
    }

    private IsExecludedImport(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) {
        return this.isPrivateLabel && !shipmentsQueriesCountsArgs.DirectionId && (!this.IsImportActivated || !this.IsPrivateLabelImportActivated);
    }

    private SetPrivateLabelExportFilter(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) {
        if (this.IsExecludedExport(shipmentsQueriesCountsArgs))
            this.SetExcludeExportFilter(shipmentsQueriesCountsArgs);
    }
 

    private IsExecludedExport(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) {
        return this.isPrivateLabel && !shipmentsQueriesCountsArgs.DirectionId && (!this.IsExportActivated || !this.IsPrivateLabelExportActivated);
    }

    private SetDirectionFilter(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) { 
       shipmentsQueriesCountsArgs.DirectionId = 'E';
       shipmentsQueriesCountsArgs.DirectionOperator = 'NotEqual'; 
    }

    private SetExcludeExportFilter(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) {
        shipmentsQueriesCountsArgs.DirectionId = 'E';
       shipmentsQueriesCountsArgs.DirectionOperator = 'NotEqual';
    }

    private SetExcludeImportFilter(shipmentsQueriesCountsArgs: ShipmentsQueriesCountsArgs) {
        shipmentsQueriesCountsArgs.DirectionId = 'I';
        shipmentsQueriesCountsArgs.DirectionOperator = 'NotEqual';
    }



     
    private setAllShipmentsQueriesCounts(myResult: ImporterQueriesDataCounts) {
        this.AgentShipmentsCount = myResult.AgentShipmentsCount > 1000 ? "1000+" : myResult.AgentShipmentsCount.toString();
        this.MyShipmentsCount = myResult.ImporterShipmentsCount > 1000 ? "1000+" : myResult.ImporterShipmentsCount.toString();
        this.AllShipmentsCount = myResult.AllShipmentsCount > 1000 ? "1000+" : myResult.AllShipmentsCount.toString();
        if (this.isPrivateLabel)
            this.RequestedCount = myResult.RequiredActionsCount > 1000 ? "1000+" : myResult.RequiredActionsCount.toString();
        else
            this.RequestedCount = myResult.RequestedDocsCount > 1000 ? "1000+" : myResult.RequestedDocsCount.toString();
    }

    GetQueryColumn(fieldName: string, dataTypeCode: string, displayText:string) {
        let queryColum: QueryColumnPM = new QueryColumnPM();
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

        this.QueryColumns.push(
            this.GetQueryColumn("PartnerName", 'Text', 'Agent Name')
        );

        this.QueryColumns.push(
            this.GetQueryColumn(("ShipmentNumber_" + this.SelectedFilter.replace(" ", "")), 'Text', 'Shipment #')
        );

        this.columns.push({
            FieldName: 'ShipperName',
            DataTypeCode: 'String',
            Display: this.HasExportShipmentToggle ? 'Supplier / Consignee' : 'Supplier' ,
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "ShipperName"
        });

        this.QueryColumns.push(
            this.GetQueryColumn("ShipperName", 'Text', this.HasExportShipmentToggle ? 'Supplier / Consignee' : 'Supplier')
        );

        if (this.HasExportShipmentToggle) { 
            this.DisplayAgentColumn();
        }


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
            this.HoverTemplateIndex = this.HasExportShipmentToggle ? 6 : 5;

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

            if (this.showComputedStatusDateField()) {
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
                this.HoverTemplateIndex = this.HasExportShipmentToggle ? 6 : 5;
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

        this.QueryColumns.push(
            this.GetQueryColumn("CustomerReference", 'Text', 'Reference #')
        );

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

        this.QueryColumns.push(
            this.GetQueryColumn("IsOperationalClosed", 'Boolean', 'Archived')
        );

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

    private DisplayAgentColumn() {
        this.columns.push({
            FieldName: 'Agent',
            DataTypeCode: 'String',
            Display: 'Agent',
            Styles: { width: '150px' }, 
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: "Agent"
        });
        this.QueryColumns.push(this.GetQueryColumn("Agent", 'Text', 'Agent'));
        this.HoverTemplateIndex = this.HoverTemplateIndex + 1;
    }
 
 

    private showComputedStatusDateField() {
        return this.isPrivateLabel == false || (this.isPrivateLabel == true && (this.SelectedFilter != "My Shipments" && this.SelectedFilter != "Action Required"));
    }

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        filters = this.getApiQueryFiltersWithAdditionalFilters(searchfields);
        filters.SortBy = "ComputedStatusDate";
        filters.SortDirection = "Descending";
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

    private getApiQueryFiltersWithAdditionalFilters(searchfields: string) {
        let filters = new ApiQueryFilters();
        if (this.isPrivateLabel && !this.IsDSV) {
            filters.addAdditionalFilter("ForwarderPartnerId", SessionLocator.PrivateLableSettings.HybridPartnerId, null, null, "Equal", true, true, false, "String");
        }
        if (!AppTool.IsNullOrEmpty(searchfields)) {
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
            filters.addAdditionalFilter("SearchFields", searchfields, null, null, "Contains", false, true, false, "String");
        }
        else {
            filters.Filter1Value = "";
            filters.AdditionalFilters = filters.AdditionalFilters.filter(a => a.FieldName != "SearchFields");
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

        return filters;
    }

    GetExportToExcelArgs() {
        this.filterAgrs = this.GetApiQueryFilters();
        this.filterAgrs.Tenant = SessionLocator.Tenant;
        let logboxShipmentExportExcelArgs: LogboxShipmentExportExcelArgs = new LogboxShipmentExportExcelArgs();
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
        this.RecentImg = Selected == "Recent" ? "./Images/LogBox/RecentW.png" : "./Images/LogBox/Recent.png";
        const isRecentSelected = Selected == "Recent";
        const isRequestedSelected = Selected == "Recent" ? false : Selected == this.RequestedDocsLable;
        this.OnImporterShipmentsFilterChanged.emit({ IsRecentSelected: isRecentSelected, IsRequestedSelected: isRequestedSelected});
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
        this.filterAgrs = this.GetApiQueryFilters();
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    GetApiQueryFilters() {
        this.filterAgrs = new ApiQueryFilters();
        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ImportersFilter').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ImportersFilter');
        }
        if (this.isPrivateLabel && !this.IsDSV) {
            this.filterAgrs.addAdditionalFilter("ForwarderPartnerId", SessionLocator.PrivateLableSettings.HybridPartnerId, null, null, "Equal", true, true, false, "String");
        }
        if (!AppTool.IsNullOrEmpty(this.SearchFilter)) {
            this.filterAgrs.addAdditionalFilter("ImportersFilter", this.SearchFilter, null, null, "Contains", true, false, false, "String");
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
         
        this.FilterLogboxShipments();
        this.FilterPrivateLabelShipments();

        return this.filterAgrs;
    }

    private FilterPrivateLabelShipments() {
        if (this.isPrivateLabel) {
            this.AddPrivateLabelAdditonalFilter();
        }
    }


    private AddPrivateLabelAdditonalFilter() {
        this.ExecludeImportShipments();
        this.ExecludeExportShipment();
    }

    private ExecludeExportShipment() {
        if (!this.IsPrivateLabelImportActivated || !this.IsImportActivated) {
            this.filterAgrs.addAdditionalFilter("DirectionId", "I", null, null, "NotEqual", true, true, false, "String");
        }
    }

    private ExecludeImportShipments() {
        if (!this.IsPrivateLabelExportActivated || !this.IsExportActivated) {
            this.filterAgrs.addAdditionalFilter("DirectionId", "E", null, null, "NotEqual", true, true, false, "String");
        }
    }

    private FilterLogboxShipments() {
        if (this.isLogbox) {
            this.filterAgrs.addAdditionalFilter("DirectionId", "E", null, null, "NotEqual", true, true, false, "String");
        }
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
        let windowArgs: any = {};
        windowArgs.ExportExcelArgs = this.GetExportToExcelArgs();
        windowArgs.tenant = SessionLocator.Tenant;
        windowArgs.ObjectTableName = "Shipment";
        windowArgs.QueryName = this.SelectedFilter;
        windowArgs.QueryType = "LogBox";
        let logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");//"Exporting View Data List To Excel File";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
    }

    AddNewEntity() {
        let NewShip = new ShipmentPM();
        NewShip.Tenant = SessionLocator.Tenant;
        let windowArgs: any = {};
        windowArgs.IsNew = true;
        let newWindow = new LogitudeWindow();
        let newWindowComponentPath = './ShipmentModules/ShipmentLogBox/Components/Logbox/';
        newWindow.WindowArgs = windowArgs;
        newWindow.Title = "Create New Shipment"; 
        windowArgs.IsImportActivated = this.IsImportActivated;
        windowArgs.IsExportActivate = this.IsExportActivated

        newWindowComponentPath = this.GetWindowComponentPath(newWindowComponentPath, newWindow); 
        newWindow.Show(newWindowComponentPath);
        newWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "MyShipmentAdded") {
                this.SelectedFilter = "My Shipments";
                this.LoadImporterShipments();
            }
        });
    }

    private GetWindowComponentPath(newWindowComponentPath: string, newWindow: LogitudeWindow) {

        if (this.hasExportShipmentOption()) {
 
            newWindowComponentPath = this.LoadNewAddShipmentComponent(newWindow, newWindowComponentPath);
        } else {
            newWindowComponentPath = this.LoadAddEditComponent(newWindow, newWindowComponentPath);
        }
        return newWindowComponentPath;
    }

    private hasExportShipmentOption() {
        return !this.isLogbox && (this.IsExportActivated && this.IsPrivateLabelExportActivated);
    }

    private LoadAddEditComponent(newWindow: LogitudeWindow, newWindowComponentPath: string) {
        newWindow.Width = 600;
        newWindow.Height = this.isPrivateLabel ? (this.IsDSV ? 376 : 420) : 350;
        newWindowComponentPath += this.isPrivateLabel ? 'AddEditPrivateLabelShipmentComponent' : 'AddEditImporterShipmentComponent';
        return newWindowComponentPath;
    }

    LoadEntityResource(objectTableName: string) {

        this.entityResourceService.getEntityResourceByTableName(objectTableName).subscribe((response: any) => {

        });
    }

    private LoadNewAddShipmentComponent(newWindow: LogitudeWindow, newWindowComponentPath: string) {
        newWindow.Width = this.isPrivateLabel ? 960 : 600;
        newWindow.Height = this.isPrivateLabel ? 600 : 350;
        newWindowComponentPath += this.isPrivateLabel ? 'AddPrivateLabelShipmentComponent' : 'AddEditImporterShipmentComponent';
        return newWindowComponentPath;
    }

    onSearchTextChangeEvent(event) {
        var temp = null;
        if (event) {
            temp = event.replace(/\s+$/, '');
        }
        if (this.searchFields != temp) {
            this.searchFields = temp;
            this.SearchFilter = temp;
            this.LoadImporterShipments();
            ServiceLocator.SendTotangoUserActivity("LogBox", "SearchFields filter changed");
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
        let newWindow = new LogitudeWindow();
        newWindow.Width = 600;
        newWindow.Height = 350;
        newWindow.Title = "Define document types for digital sign";
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
        let myPM = SessionLocator.LoggedUserPM;
        this.DontShowLogboxToolTip = true;
        this.myUserPMService.update(myPM).subscribe((myResult:any) => {

        });
    }

    MultiArchiveClicked() {
        let newWindow = new LogitudeWindow();
        newWindow.Width = 1050;
        newWindow.Height = 700;
        newWindow.Title = "Multi Archive Open Shipments";

        let windowArgs: any = {};
        //windowArgs.SourceEntity = myResult.Result;//this.rowData;
        //windowArgs.HasSharedDocs = this.HasSharedDocs;
        newWindow.WindowArgs = windowArgs;
        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/MultiArchiveShipmentsComponent');
        newWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.PseventRowSelectEvent.emit("AllowLogBoxSelect");
        });
    }
}
