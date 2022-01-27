import { Component, ViewChildren, QueryList, OnDestroy } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TariffPM } from '../../../EntityPMs/TariffPM';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM'
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    
    templateUrl: './TariffTabsContentComponent.html',
})

export class TariffTabsContentComponent implements OnDestroy {
    
    private allItems: any[];
    pager: any = {};
    private AllTabs: TariffDetailsTab[] = [];
    public RightArrowDimmed: boolean = false;
    public LeftArrowDimmed: boolean = false;
    private pageService: PagerService;       
    public EntityPM: TariffPM;
    public Tabs: TariffDetailsTab[] = [];   
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private EditTabTariffType = "VR";
    constructor(public entityArgs: EntityArgs) {
        this.Listen();
        this.pageService = new PagerService();
    }

    setPage(page: number, IsNext: boolean) {
        if (this.SelectedTabItem != null) {           
            this.SetUIPropereties();
            var NewTab: TariffDetailsTab;
            if (IsNext) {
                if (this.SelectedTabItem.Index == this.pager.endIndex && this.pager.currentPage != this.pager.endPage) {
                    this.pager = this.pageService.getPager(this.AllTabs.length, page);
                    this.Tabs = this.AllTabs.slice(this.pager.startIndex, this.pager.endIndex + 1);
                    this.Tabs.forEach(item => { item.IsTabLoaded = false; });
                }

                NewTab = this.Tabs.filter(p => p.Index == this.SelectedTabItem.Index + 1)[0];
                if (NewTab) {
                    this.SelectionChanged(NewTab);
                }
            }

            else {        
                if (this.SelectedTabItem.Index == this.pager.startIndex && this.pager.currentPage!=1) {
                    this.pager = this.pageService.getPager(this.AllTabs.length, page);
                    this.Tabs = this.AllTabs.slice(this.pager.startIndex, this.pager.endIndex + 1);
                    this.Tabs.forEach(item => { item.IsTabLoaded = false; });
                }

                NewTab = this.Tabs.filter(p => p.Index == this.SelectedTabItem.Index + -1)[0];
                if (NewTab) {
                    this.SelectionChanged(NewTab,false);
                }
            }
        }

        else {
            this.pager = this.pageService.getPager(this.AllTabs.length, page);
            this.Tabs = this.AllTabs.slice(this.pager.startIndex, this.pager.endIndex + 1);
        }      
    }

    private SetUIPropereties() {
        if (this.SelectedTabItem) {
            if (this.SelectedTabItem.Index == this.pager.endIndex) {
                if (this.pager.currentPage == this.pager.endPage) {
                    this.RightArrowDimmed = true;
                }
                else {
                    this.RightArrowDimmed = false;
                }
            }
            else {
                this.RightArrowDimmed = false;
            }

            if (this.SelectedTabItem.Index == this.pager.startIndex && this.pager.currentPage == 1) {
                this.LeftArrowDimmed = true;
            }
            else {
                this.LeftArrowDimmed = false;
            }
        }
    }

    private CurrentSessionSelectedEvent: any = null;
    private CurrentSessionSaveEvent: any = null;

    Listen() {
        this.CurrentSessionSelectedEvent = this.entityArgs.EditComponent.CurrentSession.SessionSeleced.subscribe((isSessionSeleced: boolean) => {
            if (isSessionSeleced) {
                if (!this.AllLocations) {
                   
                    if (this.timerToken) {
                        clearTimeout(this.timerToken);
                    }

                    this.Retries = 0;

                    this.RunComponent();
                }
            }
        });


        this.CurrentSessionSaveEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((result:any) => {
            this.ComputeDraftHeader();

        });
    }

    ComputeDraftHeader(): any {
        var datePipe: DatePipe = new DatePipe("en-US");
        var from: string = "";
        var to: string = "";
        var header: string = "";

        var draftVersion: TariffVersionPM = this.EntityPM.TariffVersions.filter(d => d.IsDraft)[0];
        if (draftVersion != null) {
            if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
                header = "Version " + draftVersion.Version;
            }

            else {
                from = datePipe.transform(draftVersion.StartDate, 'dd/MMM/yy');
                to = datePipe.transform(draftVersion.ExpirationDate == null ? draftVersion.InitialEnddate : draftVersion.ExpirationDate, 'dd/MMM/yy');

                if (!from) {
                    from = "";
                }

                if (!to) {
                    to = "";
                }

                if (!AppTool.IsNullOrEmpty(from) && !AppTool.IsNullOrEmpty(to)) {
                    header = from + " - " + to;
                }

                else {
                    header = from + to;
                }

                var draftTab: TariffDetailsTab = this.Tabs.filter(d => d.IsDraft)[0];
                if (draftTab != null) {
                    draftTab.Header = header;
                }
            }
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.CurrentSessionSelectedEvent);
        AppTool.KillEventEmitter(this.CurrentSessionSaveEvent);        
    }

    Run(args: any) {
        this.EntityPM = args['EntityPM'];
        if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC") {
            this.EditTabTariffType = "SVR";
        }

        if (this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
            this.EditTabTariffType = "CCVR";
        }

        else if (this.EntityPM.TypeCode == "OLC") {
            this.EditTabTariffType = "VR";
        }

        else if (this.EntityPM.TypeCode == "OFC") {
            this.EditTabTariffType = "CVR";
        }

        else if (this.EntityPM.TypeCode == "OFS") {
            this.EditTabTariffType = "OVR";
        }

        else if (this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
            this.EditTabTariffType = "CHVR";
        }

        this.BuildTabs();
        this.RunComponent();        
    }
    
    BuildTabs() {
        this.Tabs = [];
        this.AllTabs = [];
        var datePipe: DatePipe = new DatePipe("en-US");
        var from: string = "";
        var to: string = "";
        var header: string = "";
        var index: number = 0;

        var todayDate = DateTool.GetCurrentDateAsUtc();

        var draftVersion: TariffVersionPM = this.EntityPM.TariffVersions.filter(d => d.IsDraft)[0];
        if (draftVersion != null) {
            if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
                header = "Version " + draftVersion.Version;
            }

            else {
                from = datePipe.transform(draftVersion.StartDate, 'dd/MMM/yy');
                to = datePipe.transform(draftVersion.ExpirationDate == null ? draftVersion.InitialEnddate : draftVersion.ExpirationDate, 'dd/MMM/yy');

                if (!from) {
                    from = "";
                }

                if (!to) {
                    to = "";
                }

                if (!AppTool.IsNullOrEmpty(from) && !AppTool.IsNullOrEmpty(to)) {
                    header = from + " - " + to;
                }

                else {
                    header = from + to;
                }
            }

            this.AllTabs.push(new TariffDetailsTab(index, this.EditTabTariffType, header, draftVersion));
            index++;
        }
                
        this.EntityPM.ActiveVersions.sort((a, b) => { return (a.Version === b.Version) ? 0 : (a.Version > b.Version) ? -1 : 1 }).forEach(item => {
            if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
                header = "Version " + item.Version;
            }

            else {
                from = datePipe.transform(item.StartDate, 'dd/MMM/yy');
                to = datePipe.transform(item.ExpirationDate, 'dd/MMM/yy');

                if (!from) {
                    from = "";
                }

                if (!to) {
                    to = "";
                }

                if (!AppTool.IsNullOrEmpty(from) && !AppTool.IsNullOrEmpty(to)) {
                    header = from + " - " + to;
                }

                else {
                    header = from + to;
                }
            }

            this.AllTabs.push(new TariffDetailsTab(index, this.EditTabTariffType, header, item));
            index++;
        });

        this.AllTabs.push(new TariffDetailsTab(index , "GN", "General"));
        this.AllTabs.push(new TariffDetailsTab(index + 1, "VH", "Version History"));
        this.AllTabs.push(new TariffDetailsTab(index + 2, "EV", "Events"));
        this.setPage(1, false);
    }

    private Retries: number = 0;
    private timerToken: any;
    private lineIdFromPriceCheck: string;
    private chargeableWeightInKG: number;
    RunComponent(IsNext: boolean = true) {
        var index = 0;
        if (!IsNext) {
            index = this.Tabs.length - 1;
        }
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer(IsNext);
            }

            else {
                if (!AppTool.IsNullOrEmpty(this.entityArgs.EditComponentArgument)) {

                    var versionId = this.entityArgs.EditComponentArgument['VersionId'];
                    this.lineIdFromPriceCheck = this.entityArgs.EditComponentArgument['LineId'];
                    this.chargeableWeightInKG = this.entityArgs.EditComponentArgument['ChargeableWeightInKG'];
                                        
                    var SelectedTab: TariffDetailsTab = this.Tabs.filter(p => p.VersionPM != null ? (p.VersionPM.Version == + versionId) : 0)[0];

                    if (SelectedTab) {
                        this.SelectionChanged(SelectedTab);
                    }

                    else {
                        SelectedTab = this.Tabs.filter(p => p.Code == "VH")[0];
                        if (SelectedTab == undefined) {
                            this.pager.startIndex = this.pager.endIndex + 1;
                            if (this.pager.endIndex + 4 >= this.AllTabs.length - 1)
                                this.pager.endIndex = this.AllTabs.length - 1;
                            else
                                this.pager.endIndex  = this.pager.endIndex + 4;
                            this.Tabs = this.AllTabs.slice(this.pager.startIndex, this.pager.endIndex + 1);
                            this.RunComponent(false);
                        }
                        this.SelectionChanged(SelectedTab);
                    }

                    this.entityArgs.EditComponent.PreSelectedTabCode = null;
                }

                else {
                    this.SelectionChanged(this.Tabs[index]);
                }
            }
        }

        else {
            this.RunComponentTimer(IsNext);
        }
    }
    RunComponentTimer(IsNext: boolean = true) {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 10) {
            this.timerToken = setTimeout(() => this.RunComponent(IsNext), 1);
        }
    }

    public SelectedTabItem: TariffDetailsTab;
    SelectionChanged(clickdTab: TariffDetailsTab, IsNext: boolean = true) {
        if (clickdTab != null) {
            if (this.SelectedTabItem != clickdTab) {
                this.SelectedTabItem = clickdTab;

                this.SetUIPropereties();

                this.Tabs.forEach((item) => {
                    item.IsSelected = false;
                });

                this.SelectedTabItem.IsSelected = true;
            }

            if (this.SelectedTabItem.IsTabLoaded) {

            }

            else {
                let locs = this.AllLocations.toArray().filter(f => f.Code == 'TariffTabLocation');
               
                let location: LocationDirective = locs.filter(f => f.Index == this.SelectedTabItem.Index)[0];
                if (location == null) {
                    this.Retries = 0;
                    this.RunComponentTimer(IsNext);
                }

                if (location) {

                    if (this.SelectedTabItem.IsTabLoaded) {

                    }

                    else if (this.SelectedTabItem.ComponentPath) {
                        SessionLocator.DynamicLoader.Load(this.SelectedTabItem.ComponentPath, location.viewContainerRef).then(cmpRef => {
                            this.SelectedTabItem.IsTabLoaded = true;
                            if (this.Tabs.filter(p => p.Index == this.SelectedTabItem.Index)[0]) {
                                this.Tabs.filter(p => p.Index == this.SelectedTabItem.Index)[0].IsTabLoaded = true;
                            }
                            if (this.SelectedTabItem.VersionPM) {
                                cmpRef.instance.Intialize({
                                    CurrentVersion: this.SelectedTabItem.VersionPM,
                                    SelectedVersionNumber: this.SelectedTabItem.VersionPM.Version,
                                    LineIdFromPriceCheck: this.lineIdFromPriceCheck,
                                    ChargeableWeightInKG: this.chargeableWeightInKG
                                });
                            }
                            if (this.SelectedTabItem.Code == "VH") {
                                cmpRef.instance.Intialize({
                                    LineIdFromPriceCheck: this.lineIdFromPriceCheck,
                                    ChargeableWeightInKG: this.chargeableWeightInKG
                                });
                            }
                        });
                    }
                }
            }
        }
    }
}

class TariffDetailsTab {
    public Code: string;
    public Index: number;
    public Header: string = null;
    public ComponentPath: string;
    public IsSelected: boolean = false;
    public IsTabLoaded: boolean = false;
    public IsDraft: boolean = false;
    public VersionPM: TariffVersionPM;
    constructor(index: number, code: string, header: string, version: TariffVersionPM = null) {
        this.Index = index;
        this.Code = code;
        this.Header = header;

        switch (this.Code) {
            case "VR": {
                this.IsDraft = version.IsDraft;
                this.VersionPM = version;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/VersionTabComponent";
                break;
            }

            case "SVR": {
                this.IsDraft = version.IsDraft;
                this.VersionPM = version;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/SurchargeVersionTabComponent";
                break;
            }

            case "CVR": {
                this.IsDraft = version.IsDraft;
                this.VersionPM = version;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/OceanFCLVersionTabComponent";
                break;
            }

            case "OVR":
                {
                    this.IsDraft = version.IsDraft;
                    this.VersionPM = version;
                    this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/OceanFCLSurchargeVersionTabComponent";
                    break;
                }

            case "GN": {
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/TariffGeneralTabComponent";
                break;
            }

            case "VH": {
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/VersionHistoryTabComponent";
                break;
            }

            case "EV": {
                this.ComponentPath = "./Common/Components/Events/EventsTabComponent";
                break;
            }

            case "CCVR": {
                this.IsDraft = version.IsDraft;
                this.VersionPM = version;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/CustomChargesVersionTabComponent";
                break;
            }
        }
    }
}

export class PagerService {
    getPager(totalItems: number, currentPage: number = 1, pageSize: number = 4) {
        let totalPages = Math.ceil(totalItems / pageSize);
        if (currentPage < 1) {
            currentPage = 1;
        } else if (currentPage > totalPages) {
            currentPage = totalPages;
        }
        let startPage: number, endPage: number;
        if (totalPages <= 10) {
            startPage = 1;
            endPage = totalPages;
        } else {
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }
        let startIndex = (currentPage - 1) * pageSize;
        let endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);
        let pages = Array.from(Array((endPage + 1) - startPage).keys()).map(i => startPage + i);
        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages
        };
    }
}
