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
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {PortExtendedPMService} from '../../../../Common/Services/ExtendedPMs/PortExtendedPMService';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {EntityStatusExtendedListService} from '../../../../Infrastructure/Services/ExtendedLists/EntityStatusExtendedListService';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ShipmentPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {PackageTypeListService} from '../../../../Common/Services/StandardLists/PackageTypeListService';

@Component({
    moduleId: module.id,
    templateUrl: './ForwarderShipmentsComponent.html',
    //providers: [Http, ServiceArgs, EntityListService]
})

export class ForwarderShipmentsComponent extends BaseComponent implements OnInit, AfterViewInit {
    private myShipmentDomainService: ShipmentDomainService;
    public AgentShortName: string = "";
    public IsPrivateLabel: boolean = false;
    private messageWindow: MessageWindow = new MessageWindow();
    //EntityPm: ShipmentPM = new ShipmentPM();
    DataContext: ForwarderShipmentsComponent = this;
    public _PortExtendedPMService: PortExtendedPMService;
    public _PackageTypeListService: PackageTypeListService;
    public _ShipmentPMService: ShipmentPMService;
    public _EntityStatusExtendedListService: EntityStatusExtendedListService;
    public TransportationTypes = [new TransportationTypes("Ocean Haifa", "O", "HFA", "IL"), new TransportationTypes("Ocean Ashdod", "O", "ASH", "IL")];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityListService: EntityListService) {
        super();
        this.myShipmentDomainService = new ShipmentDomainService();
        this._PackageTypeListService = new PackageTypeListService();
        if (SessionLocator.PrivateLableSettings) {
            this.ValidationErrorsList = [];
            this.AgentShortName = SessionLocator.PrivateLableSettings.PrivateLabelShortName;
            this.IsPrivateLabel = true;
            this._PortExtendedPMService = new PortExtendedPMService();
            this._ShipmentPMService = new ShipmentPMService();
            this._EntityStatusExtendedListService = new EntityStatusExtendedListService();
        }
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
        this.SourceEntity = args.SourceEntity;
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
            //if (this.SourceEntity.MainCarriageToPortCode == "ASH") {
            //    this.SelectedTransportationTypes = new TransportationTypes("Ocean Ashdod", "O", "ASH", "IL");
            //}
            //else {
            //    this.SelectedTransportationTypes = new TransportationTypes("Ocean Haifa", "O", "HFA", "IL");
            //}
            if (this.SourceEntity.MainCarriageToPortCode == "ASH") {
                this.SelectedTransportationTypes = new TransportationTypes("Ocean Ashdod", "O", "ASH", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "HFA") {
                this.SelectedTransportationTypes = new TransportationTypes("Ocean Haifa", "O", "HFA", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "ETH") {
                this.SelectedTransportationTypes = new TransportationTypes("Ocean Eilat", "O", "ETH", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "NZN") {
                this.SelectedTransportationTypes = new TransportationTypes("Inland Nitzana", "I", "NZN", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "ARV") {
                this.SelectedTransportationTypes = new TransportationTypes("Inland Arava", "I", "ARV", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "ALN") {
                this.SelectedTransportationTypes = new TransportationTypes("Inland Alenbi", "I", "ALN", "IL");
            }
            else if (this.SourceEntity.MainCarriageToPortCode == "JOR") {
                this.SelectedTransportationTypes = new TransportationTypes("Inland Jordan", "I", "JOR", "IL");
            }
            else {
                this.SelectedTransportationTypes = new TransportationTypes("Air Tel-Aviv", "A", "TLV", "IL");
            }
        }
        this._PackageTypeListService.getAllFromCache().subscribe(myResult => {
            if (!myResult.HasError) {
                this.UnAssignedPackageTypeId = myResult.Result.filter(a => a.Tenant == SessionLocator.Tenant && a.Code == '---')[0].Id;
            }
            else {
                this.ValidationErrorsList = myResult.ErrorsArray;
            }
        });
        //this.CurrentSession.SessionEvent.subscribe(($event: any) => {
        //    if ($event.Name == "GetSourceEntity") {
        //        this.GetSourceEntity($event.EntityId);
        //    }
        //}); 
    }
    BuildColumns() {
        this.columns = [];
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
        this.columns.push({
            FieldName: this.SourceEntity.Id,
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '260px' },
            HtmlListComponentName: 'ActionButtonsListTemplate',
            HtmlListComponentUrl: './Shipment/Components/ListTemplates/ConnectButtonsListTemplate',
            IsCustomTemplate: true,
            EnableHoverVisibility: true,
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
                //ForwarderPartnerId
                //var x = filters.AdditionalFilters.filter(a => a.FieldName == "ForwarderPartnerId");
                //if (x.length == 0) {
                //    filters.addAdditionalFilter("ForwarderPartnerId", this.SourceEntity.ForwarderPartnerId, null, null, "NotEqual", true, true, false, "String");
                //}
            }
        }
        else {
            var x = filters.AdditionalFilters.filter(a => a.FieldName == "ForwarderShipmentsFilter");
            if (x.length == 0) {
                filters.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
            }
            //var x = filters.AdditionalFilters.filter(a => a.FieldName == "ForwarderPartnerId");
            //if (x.length == 0) {
            //    filters.addAdditionalFilter("ForwarderPartnerId", this.SourceEntity.ForwarderPartnerId, null, null, "NotEqual", true, true, false, "String");
            //}
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
        this.BuildColumns();

        this.filterAgrs = new ApiQueryFilters();
        if (FilterByOrderNumber == true) {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'CustomerReference1').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'CustomerReference1');
            }
            this.filterAgrs.addAdditionalFilter("CustomerReference1", this.CustomerReference1, null, null, "Equals", false, false, false, "String");
        }
        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'IsCancelled').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'IsCancelled');
        }
        this.filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        //if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ForwarderPartnerId').length > 0) {
        //    this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ForwarderPartnerId');
        //}
        //this.filterAgrs.addAdditionalFilter("ForwarderPartnerId", this.SourceEntity.ForwarderPartnerId, null, null, "Equals", false, false, false, "String");
       
        if (this.SelectedTransportFilter != "All") {
            this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string", this.SelectedTransportFilter == "All" ? true : false);
        }
        else {
            if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'TransportModeId').length > 0) {
                this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'TransportModeId');
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

        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'ForwarderShipmentsFilter').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'ForwarderShipmentsFilter');
        }
        if (this.filterAgrs.AdditionalFilters.filter(a => a.FieldName == 'NotForwarderShipmentsFilter').length > 0) {
            this.filterAgrs.AdditionalFilters = this.filterAgrs.AdditionalFilters.filter(a => a.FieldName != 'NotForwarderShipmentsFilter');
        }
        this.filterAgrs.addAdditionalFilter("ForwarderShipmentsFilter", "null", null, null, "NotEqual", true, true, false, "String");
        //this.filterAgrs.addAdditionalFilter("ForwarderShipmentNumber", "null", null, null, "NotEqual", false, true, false, "String");
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

    private selectedTransportationTypes: TransportationTypes;
    public get SelectedTransportationTypes() {
        return this.selectedTransportationTypes;
    }
    public set SelectedTransportationTypes(newValue: TransportationTypes) {
        this.selectedTransportationTypes = newValue;
        this.TransportModeId = newValue.TransporationType;
        this.ToPortId = newValue.ToPortCode;

    }

    onTransportationTypeChange($event) {
        this.SelectedTransportationTypes = $event;
    }

    public get ShipperName() { return this.SourceEntity.ShipperName }
    public set ShipperName(newValue: string) { this.SourceEntity.ShipperName = newValue; }

    public get CustomerReference1() { return this.SourceEntity.CustomerReference1 }
    public set CustomerReference1(newValue: string) { this.SourceEntity.CustomerReference1 = newValue; }

    public get CustomerId() { return this.SourceEntity.CustomerId }
    public set CustomerId(newValue: string) { this.SourceEntity.CustomerId = newValue; }

    public get StatusDate() { return this.SourceEntity.StatusDate }
    public set StatusDate(newValue: any) { this.SourceEntity.StatusDate = newValue; }

    public get StatusId() { return this.SourceEntity.StatusId }
    public set StatusId(newValue: string) { this.SourceEntity.StatusId = newValue; }

    public get ForwarderPartnerId() { return this.SourceEntity.ForwarderPartnerId }
    public set ForwarderPartnerId(newValue: string) { this.SourceEntity.ForwarderPartnerId = newValue; }

    public get DepartmentId() { return this.SourceEntity.DepartmentId }
    public set DepartmentId(newValue: string) { this.SourceEntity.DepartmentId = newValue; }

    public get BranchId() { return this.SourceEntity.BranchId }
    public set BranchId(newValue: string) { this.SourceEntity.BranchId = newValue; }

    public get ShipmentLevelCode() { return this.SourceEntity.ShipmentLevelCode }
    public set ShipmentLevelCode(newValue: string) { this.SourceEntity.ShipmentLevelCode = newValue; }

    public get DirectionId() { return this.SourceEntity.DirectionId }
    public set DirectionId(newValue: string) { this.SourceEntity.DirectionId = newValue; }

    public get TransportModeId() { return this.SourceEntity.TransportModeId }
    public set TransportModeId(newValue: string) { this.SourceEntity.TransportModeId = newValue; }

    public get ToPortId() { return this.SourceEntity.ToPortId }
    public set ToPortId(newValue: string) { this.SourceEntity.ToPortId = newValue; }

    public get OtherPrepaidCollectId() { return this.SourceEntity.OtherPrepaidCollectId }
    public set OtherPrepaidCollectId(newValue: string) { this.SourceEntity.OtherPrepaidCollectId = newValue; }

    public get FreightPrepaidCollectId() { return this.SourceEntity.FreightPrepaidCollectId }
    public set FreightPrepaidCollectId(newValue: string) { this.SourceEntity.FreightPrepaidCollectId = newValue; }

    public get FromPortId() { return this.SourceEntity.FromPortId }
    public set FromPortId(newValue: string) { this.SourceEntity.FromPortId = newValue; }

    public get Notes() { return this.SourceEntity.Notes }
    public set Notes(newValue: string) { this.SourceEntity.Notes = newValue; }

    public get Master() { return this.SourceEntity.Master }
    public set Master(newValue: string) { this.SourceEntity.Master = newValue; }

    public get House() { return this.SourceEntity.House }
    public set House(newValue: string) { this.SourceEntity.House = newValue; }

    private containerNumber: string;
    public get ContainerNumber() {
        if (this.SourceEntity.ShipmentPackages && this.SourceEntity.ShipmentPackages.length > 0) {
            this.containerNumber = this.SourceEntity.ShipmentPackages[0].ContainerNumber;
        }
        return this.containerNumber;
    }
    public set ContainerNumber(newValue: string) {
        if (this.SourceEntity && this.SourceEntity.ShipmentPackages && this.SourceEntity.ShipmentPackages.length == 0) {
            this.SourceEntity.ShipmentPackages = [];
            var MyPackage = new ShipmentPackagePM(this.SourceEntity);
            MyPackage.ContainerNumber = newValue;
            MyPackage.Weight = this.SourceEntity.GrossWeight;
            MyPackage.PackageTypeId = this.UnAssignedPackageTypeId;
            MyPackage.Quantity = this.SourceEntity.PackagesQuantity;
            this.SourceEntity.ShipmentPackages.push(MyPackage);
        }
        else if (this.SourceEntity && this.SourceEntity.ShipmentPackages.length > 0) {
            this.SourceEntity.ShipmentPackages[0].ContainerNumber = newValue;
            this.SourceEntity.ShipmentPackages[0].Weight = this.SourceEntity.GrossWeight;
            this.SourceEntity.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
            this.SourceEntity.ShipmentPackages[0].Quantity = this.SourceEntity.PackagesQuantity;
        }
        //this.ValidateContainerNumber(newValue);
    }


    WarningErrorsList: any[];
    ValidateContainerNumber(input: string) {
        this.ValidationErrorsList = [];
        this.WarningErrorsList = [];
        var error = FormatTool.ValidateContainerNumber(input);

        if (!AppTool.IsNullOrEmpty(error)) {
            this.WarningErrorsList.push(error);
        }
    }
    UnAssignedPackageTypeId: string = '';
    ValidationErrorsList: any[];
    IsCreateButtonClicked: boolean = false;
    CreateButtonClicked() {
        if (this.IsCreateButtonClicked == true) {
            return;
        }
        this.IsCreateButtonClicked = true;
        this.ValidationErrorsList = []; 
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (!this.SelectedTransportationTypes) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "TransportationTypes"));
        }

        if (AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "OrderNumber"));
        } 
        if (this.ValidationErrorsList.length == 0) {
            this._ShipmentPMService.GetSingleByCustomerReference1(this.CustomerReference1).subscribe(myResult => {
                if (myResult.Result) { 
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Title = "Warning !";
                    confirmWindow.Width = 300;
                    confirmWindow.Height = 150;
                    confirmWindow.YesButtonText = "Continue";
                    confirmWindow.NoButtonText = "Cancel";
                    confirmWindow.Show("Shipment (" + myResult.Result.ForwarderShipmentNumber + ") with the same order number is alreay exist in the query");
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.ContinueCreateShipmentProcess(); 
                        } 
                        else {
                            this.IsCreateButtonClicked = false;
                            this.LoadImporterShipments(true);
                        }
                    });
                    //this.messageWindow.Width = 300;
                    //this.messageWindow.Height = 150;
                    //this.messageWindow.Title = "";
                    //this.messageWindow.Message = "Shipment (" + myResult.Result.ForwarderShipmentNumber + ") with the same order number is alreay exist in the query";
                    //this.messageWindow.Show(this.messageWindow.Message);
                    //this.messageWindow.WindowClosed.subscribe(($event) => {
                       
                    //});
                }
                else {
                    this.ContinueCreateShipmentProcess(); 
                }
            }); 
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }

    }

    ContinueCreateShipmentProcess() {
        this._PortExtendedPMService.getSinglePort(this.SelectedTransportationTypes.ToPortCode, this.SelectedTransportationTypes.CountryCode, SessionLocator.Tenant).subscribe(myResult => {
            if (myResult.Result) {
                this.ToPortId = myResult.Result.Id;
                if (AppTool.IsNullOrEmpty(this.SourceEntity.FromPortId)) {
                    this._PortExtendedPMService.getSinglePort("---", "IL", SessionLocator.Tenant).subscribe(Result => {
                        this.FromPortId = Result.Result.Id;
                        this.SaveData();
                    });
                }
                else {
                    this.SaveData();
                }
            }
            else {
                this.SaveData();
            }
        });
    }

    SaveData() {
        this.ValidationErrorsList = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (!this.SelectedTransportationTypes) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "TransportationTypes"));
        }

        if (AppTool.IsNullOrEmpty(this.CustomerReference1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "OrderNumber"));
        }
        if (!AppTool.IsNullOrEmpty(this.ContainerNumber) && (AppTool.IsNullOrEmpty(this.SourceEntity.PackagesQuantity) || AppTool.IsNullOrEmpty(this.SourceEntity.GrossWeight))) {
            this.ValidationErrorsList.push("Weight and Quantity are required");
        }
        //else {
        //    if (this.SourceEntity.ShipmentPackages.length > 0) {
        //        this.SourceEntity.ShipmentPackages[0].ContainerNumber = this.ContainerNumber;
        //        this.SourceEntity.ShipmentPackages[0].Weight = this.SourceEntity.GrossWeight;
        //        this.SourceEntity.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
        //        this.SourceEntity.ShipmentPackages[0].Quantity = this.SourceEntity.PackagesQuantity;
        //    }
        //}
        //if (AppTool.IsNullOrEmpty(this.ForwarderPartnerId)) {
        //    this.ValidationErrorsList.push(msg.replace("%FieldName", "ForwarderPartnerId"));
        //}

        //if (AppTool.IsNullOrEmpty(this.FromPortId)) {
        //    this.ValidationErrorsList.push(msg.replace("%FieldName", "Gatway"));
        //}

        //if (AppTool.IsNullOrEmpty(this.ToPortId)) {
        //    this.ValidationErrorsList.push(msg.replace("%FieldName", "Destination"));
        //}
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Shipment Creation in Progress ...");
            this.SourceEntity.IsImporterShipment = true;
            this.SourceEntity.MainCarriageFromPortId = this.SourceEntity.FromPortId;
            this.SourceEntity.MainCarriageToPortId = this.SourceEntity.ToPortId;
            this.SourceEntity.MainCarriageToPortId = this.SourceEntity.ToPortId; 
            if (!AppTool.IsNullOrEmpty(this.ContainerNumber)) {
                if (this.SourceEntity.ShipmentPackages.length > 0) {
                    this.SourceEntity.ShipmentPackages[0].ContainerNumber = this.ContainerNumber;
                    this.SourceEntity.ShipmentPackages[0].Weight = this.SourceEntity.GrossWeight;
                    this.SourceEntity.ShipmentPackages[0].PackageTypeId = this.UnAssignedPackageTypeId;
                    this.SourceEntity.ShipmentPackages[0].Quantity = this.SourceEntity.PackagesQuantity;
                }
            }
           
            this._EntityStatusExtendedListService.getSingle("INPS").subscribe(Status => {
                this.SourceEntity.StatusId = Status.Result.Id;
                this._ShipmentPMService.update(this.SourceEntity).subscribe(myResult => {
                    if (!myResult.HasError) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CurrentSession.SessionEvent.emit({ Name: "ReloadShipments" });
                        this.CurrentSession.CurrentWindow.Close("");
                    }
                    else {
                        this.ValidationErrorsList = myResult.ErrorsArray;
                    }
                    this.IsCreateButtonClicked = false;
                });
            });
           
            
        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
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
