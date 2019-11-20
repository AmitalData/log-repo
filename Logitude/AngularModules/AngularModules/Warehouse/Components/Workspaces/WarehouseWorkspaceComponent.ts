

import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';

import {FormatTool, DateTool, AppTool} from '../../../Infrastructure/Tools';

import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ListComponentArgs, UserArgs} from '../../../Infrastructure/Args';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {WarehouseReleasePMExtendedService} from '../../Services/ExtendedPMs/WarehouseReleasePMExtendedService';
import {WarehouseEntryListExtendedService} from '../../Services/ExtendedLists/WarehouseEntryListExtendedService';
import {WarehouseReleaseListExtendedService} from '../../Services/ExtendedLists/WarehouseReleaseListExtendedService';



@Component({
    moduleId: module.id,
    selector: 'WarehouseWorkspaceComponent',
    templateUrl: './WarehouseWorkspaceComponent.html',

})

export class WarehouseWorkspaceComponent extends BaseComponent {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() ReloadWarehouseEntryQueries = new EventEmitter();

    @Output() ReloadWarehouseReleaseQueries = new EventEmitter();
    warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService;
    warehouseEntryListExtendedService: WarehouseEntryListExtendedService;
    warehouseReleaseListExtendedService: WarehouseReleaseListExtendedService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.SetReleasesQueriesVisibility();
        this.SetEntrysQueriesVisibility();
        this.SetNewReleaseVisibility();
        this.warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService();
        this.warehouseEntryListExtendedService = new WarehouseEntryListExtendedService();
        this.warehouseReleaseListExtendedService = new WarehouseReleaseListExtendedService();
        this._entityResourceService.getEntityResourceByTableName("WarehouseEntry").subscribe(response => {
        });
    }

    WarehouseEntryCreatedQueryCount: number = 0;
    WarehouseEntryEnteredQueryCount: number = 0;
    WarehouseEntryConnectedToShipmentsQueryCount: number = 0;
    WarehouseEntryNotConnectedToShipmentsQueryCount: number = 0;
    WarehouseEntryAllQueryCount: number = 0;
    IsNewReleaseVisible: boolean = false;

    WarehouseReleaseCreatedQueryCount: number = 0;
    WarehouseReleaseReleasedQueryCount: number = 0;
    WarehouseReleaseAllQueryCount: number = 0;
    WarehouseReleaseCanceledQueryCount: number = 0;

    public SearchText: string = "Search";

    InitComponent() {
        this.LoadAllData();

    }

    BuildCustomQueriesList() {
        this.ReloadWarehouseEntryQueries.emit();
        this.ReloadWarehouseReleaseQueries.emit();

    }

    LoadAllData() {
        this.LoadDataSummary();
        this.LoadRecentWarehouseEntries();
        this.LoadRecentWarehouseReleases();
        this.BuildCustomQueriesList();

    }

    public RecentWarehouseReleasesLists: any[] = [];
    public IsNoDataVisible_RecentWarehouseReleases: boolean = false;
    LoadRecentWarehouseReleases() {
        this.RecentWarehouseReleasesLists = [];
        this.IsNoDataVisible_RecentWarehouseReleases = false;

        this.warehouseReleaseListExtendedService.GetRecentWarehouseReleases().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    if (myResponse.Result) {
                        this.RecentWarehouseReleasesLists = myResponse.Result;

                        if (this.RecentWarehouseReleasesLists.length == 0) {
                            this.IsNoDataVisible_RecentWarehouseReleases = true;
                        }
                    }
                }
            }
        });
    }



    // Release Queries Features
    public WarehouseReleaseVisibility: boolean = false;
    public AllReleaseQueriesVisibility: boolean = false;
    public ReleasedQueriesVisibility: boolean = false;
    public CancledReleaseQueriesVisibility: boolean = false;
    public CreatedReleaseQueriesVisibility: boolean = false;
    private SetReleasesQueriesVisibility() {
        this.AllReleaseQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseRelease", "WarehouseRelease.Q.AllReleasesQuery") ? true : false;
        this.ReleasedQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseRelease", "WarehouseRelease.Q.ReleasedQuery") ? true : false;
        this.CancledReleaseQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseRelease", "WarehouseRelease.Q.CancelledReleasesQuery") ? true : false;
        this.CreatedReleaseQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseRelease", "WarehouseRelease.Q.CreatedReleasesQuery") ? true : false;
        this.WarehouseReleaseVisibility = this.AllReleaseQueriesVisibility || this.ReleasedQueriesVisibility || this.CancledReleaseQueriesVisibility || this.CreatedReleaseQueriesVisibility ? true : false;
    }

    SetNewReleaseVisibility() {

        if (FeatureLocator.HasFeaturePermession("WarehouseRelease", "ShowNewFullWarehouseRelease")) {
            this.IsNewReleaseVisible = true;
        }
    }

    CreatedEntryQueriesVisibility: boolean = false;
    EnteredEntryQueriesVisibility: boolean = false;
    ConnectedToShipmentsEntryQueriesVisibility: boolean = false;
    NotConnectedToShipmentsEntryQueriesVisibility: boolean = false;
    AllEntryQueriesVisibility: boolean = false;

    private SetEntrysQueriesVisibility() {
        this.CreatedEntryQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.CreatedEntriesQuery") ? true : false;
        this.EnteredEntryQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.EnteredEntriesQuery") ? true : false;
        this.ConnectedToShipmentsEntryQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.ConnectedEntriesQuery") ? true : false;
        this.NotConnectedToShipmentsEntryQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.NotConnectedEntriesQuery") ? true : false;
        this.AllEntryQueriesVisibility = FeatureLocator.HasFeaturePermession("WarehouseEntry", "WarehouseEntry.Q.AllEntriesQuery") ? true : false;
    }


    public RecentWarehouseEntriesLists: any[] = [];
    public IsNoDataVisible_RecentWarehouseEntries: boolean = false;
    LoadRecentWarehouseEntries() {
        this.RecentWarehouseEntriesLists = [];
        this.IsNoDataVisible_RecentWarehouseEntries = false;

        this.warehouseEntryListExtendedService.GetRecentWarehouseEntries().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.RecentWarehouseEntriesLists = myResponse.Result;

                    if (this.RecentWarehouseEntriesLists.length == 0) {
                        this.IsNoDataVisible_RecentWarehouseEntries = true;
                    }
                }
            }
        });
    }


    private mySelectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    set SelectedDirectionFilter(value: string) {
        if (this.mySelectedDirectionFilter != value) {
            this.mySelectedDirectionFilter = value;

            this.LoadAllData();

        }
    }

    private mySelectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.mySelectedTransportFilter; }
    set SelectedTransportFilter(value: string) {
        if (this.mySelectedTransportFilter != value) {
            this.mySelectedTransportFilter = value;

            this.LoadAllData();

        }
    }

    LoadDataSummary() {

        // this.CurrentSession.StartBusyIndicatorLoading();
        this.warehouseReleasePMExtendedService.GetCrossDockWorkspaceSummary(this.SelectedTransportFilter, this.SelectedDirectionFilter).subscribe(res => {
            var pmResponse: ServiceResponse = res;

            //  this.CurrentSession.StopBusyIndicator();

            if (!pmResponse.HasError) {
                var crossDockWorkspaceSummaryClass: any = pmResponse.Result;
                if (crossDockWorkspaceSummaryClass != null) {

                    this.WarehouseEntryCreatedQueryCount = crossDockWorkspaceSummaryClass.CreatedWarehouseEntriesCount;
                    this.WarehouseEntryEnteredQueryCount = crossDockWorkspaceSummaryClass.EnteredWarehouseEntriesCount;
                    this.WarehouseEntryConnectedToShipmentsQueryCount = crossDockWorkspaceSummaryClass.ConnectedToShipmentsWarehouseEntriesCount;
                    this.WarehouseEntryNotConnectedToShipmentsQueryCount = crossDockWorkspaceSummaryClass.NotConnectedToShipmentsWarehouseEntriesCount;
                    this.WarehouseEntryAllQueryCount = crossDockWorkspaceSummaryClass.AllWarehouseEntriesCount;

                    this.WarehouseReleaseCreatedQueryCount = crossDockWorkspaceSummaryClass.CreatedWarehouseReleasesCount;
                    this.WarehouseReleaseReleasedQueryCount = crossDockWorkspaceSummaryClass.WarehouseReleasedCount;
                    this.WarehouseReleaseAllQueryCount = crossDockWorkspaceSummaryClass.AllWarehouseReleasesCount;
                    this.WarehouseReleaseCanceledQueryCount = crossDockWorkspaceSummaryClass.CanceledReleasesCount;
                }
            }
        });
    }

    RefreshButtonClicked() {
        this.LoadAllData();
    }

    onWarehouseEntryQueriesBackComplete(event) {

    }
    onWarehouseReleaseQueriesBackComplete(event) {

    }


    NewEntryButtonclick() {
        var args: any = {};
        args.ShipmentLevelCode = "D";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "New Cross Dock Entry";
        logWindow.Show('./Warehouse/Components/NewEntity/NewFullWarehouseEntryComponent');

        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.LoadAllData();
            }
        });

    }



    NewReleaseButtonclick() {
        var args: any = {};
        args.ShipmentLevelCode = "D";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 940;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "New Cross Dock Release";
        logWindow.Show('./Warehouse/Components/NewEntity/NewFullWarehouseReleaseComponent');

        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.LoadAllData();
            }
        });

    }

    EditWarehouseEntry() {

    }

    private SetDirectionTransportFilter() {
        if (!AppTool.IsNullOrEmpty(this.SelectedDirectionFilter) && this.SelectedDirectionFilter != "All") {
            this.filterAgrs.addAdditionalFilter("DirectionId", this.SelectedDirectionFilter, null, null, "Equals", false, true, false, "string");
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedTransportFilter) && this.SelectedTransportFilter != "All") {
            this.filterAgrs.addAdditionalFilter("TransportModeId", this.SelectedTransportFilter, null, null, "Equals", false, true, false, "string");
        }
    }

    filterAgrs: ApiQueryFilters;
    ViewWarehouseEntryQuery(queryName: string) {

        this.filterAgrs = new ApiQueryFilters();

        var queryCode: string = "";
        var displayTitle: string = "";
        switch (queryName) {

            case "All":
                {
                    queryCode = "AllEntriesQuery";
                    displayTitle = "All Entries";

                    break;
                }
            case "Created":
                {
                    queryCode = "CreatedEntriesQuery";
                    displayTitle = "Created Entries";


                    break;
                }

            case "Entered":
                {
                    queryCode = "EnteredEntriesQuery";
                    displayTitle = "Entered Entries";

                    break;
                }

            case "ConnectedToShipments":
                {
                    queryCode = "ConnectedEntriesQuery";
                    displayTitle = "Connected To Shipments Entries";


                    break;
                }

            case "NotConnectedToShipments":
                {
                    queryCode = "NotConnectedEntriesQuery";
                    displayTitle = "Not Connected To Shipments Entries";

                    break;
                }

        }


        this.SetDirectionTransportFilter();
        var listArgs = new ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = "WarehouseEntry";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Cross Docks";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {

                    var filtersBar: any = null;
                    cmpRef.instance.FiltersBarLoaded.subscribe((myBar: any) => {
                        filtersBar = myBar;

                        if (filtersBar) {
                            if (filtersBar.SelectedValue != this.SelectedTransportFilter) {
                                filtersBar.SetTransport(this.SelectedTransportFilter);
                            }

                            if (filtersBar.SelectedDirection != this.SelectedDirectionFilter) {
                                filtersBar.SetDirection(this.SelectedDirectionFilter);
                            }
                        }
                    });

                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        if (filtersBar) {
                            if (filtersBar.SelectedValue != this.SelectedTransportFilter) {
                                this.mySelectedTransportFilter = filtersBar.SelectedValue;
                            }

                            if (filtersBar.SelectedDirection != this.SelectedDirectionFilter) {
                                this.mySelectedDirectionFilter = filtersBar.SelectedDirection;
                            }
                        }

                        this.LoadAllData();
                    });
                    listArgs.SelectedDirection = this.mySelectedDirectionFilter;
                    listArgs.SelectedTransportMode = this.mySelectedTransportFilter;
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });



    }
    ViewWarehouseReleaseQuery(queryName: string) {
        this.filterAgrs = new ApiQueryFilters();
        var queryCode: string = "";
        var displayTitle: string = "";
        switch (queryName) {
            case "All":
                {
                    queryCode = "AllReleasesQuery";
                    displayTitle = "All Releases";
                    break;
                }
            case "Created":
                {
                    queryCode = "CreatedReleasesQuery";
                    displayTitle = "Created Releases";
                    break;
                }

            case "Released":
                {
                    queryCode = "ReleasedQuery";
                    displayTitle = "Entered Releases";
                    break;
                }

            case "Canceled":
                {
                    queryCode = "CancelledReleasesQuery";
                    displayTitle = "Canceled Releases";
                    break;
                }
        }

        this.SetDirectionTransportFilter();
        var listArgs = new ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = "WarehouseRelease";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Cross Docks";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    var filtersBar: any = null;
                    cmpRef.instance.FiltersBarLoaded.subscribe((myBar: any) => {
                        filtersBar = myBar;

                        if (filtersBar) {
                            if (filtersBar.SelectedValue != this.SelectedTransportFilter) {
                                filtersBar.SetTransport(this.SelectedTransportFilter);
                            }

                            if (filtersBar.SelectedDirection != this.SelectedDirectionFilter) {
                                filtersBar.SetDirection(this.SelectedDirectionFilter);
                            }
                        }
                    });

                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        if (filtersBar) {
                            if (filtersBar.SelectedValue != this.SelectedTransportFilter) {
                                this.mySelectedTransportFilter = filtersBar.SelectedValue;
                            }

                            if (filtersBar.SelectedDirection != this.SelectedDirectionFilter) {
                                this.mySelectedDirectionFilter = filtersBar.SelectedDirection;
                            }
                        }

                        this.LoadAllData();
                    });
                    listArgs.SelectedDirection = this.mySelectedDirectionFilter;
                    listArgs.SelectedTransportMode = this.mySelectedTransportFilter;
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }

    EditWarehouseEntries(item: any) {

        var myBackButtonLabel = "Cross Docks";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseEntry", BackButtonLabel: myBackButtonLabel });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.LoadAllData();
                });
            });
    }
    EditWarehouseReleases(item: any) {

        var myBackButtonLabel = "Cross Docks";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseRelease", BackButtonLabel: myBackButtonLabel });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.LoadAllData();
                });
            });
    }
}

