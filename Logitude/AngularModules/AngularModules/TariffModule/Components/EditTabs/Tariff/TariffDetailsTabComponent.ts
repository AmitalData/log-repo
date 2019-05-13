import { Component, OnInit, OnDestroy, ViewChildren, QueryList } from '@angular/core';
import { DatePipe } from '@angular/common';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './TariffDetailsTabComponent.html',
})

export class TariffDetailsTabComponent implements OnInit, OnDestroy {
    public EntityPM: TariffPM;
    public Tabs: TariffDetailsTab[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private EditTabTariffType = "VR";

    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        this.EntityPM = entityArgs.EntityPM;
        this.Tabs = [];
        this.Listen();
      
        if (this.EntityPM.TypeCode == "ASC") {
            this.EditTabTariffType = "SVR";
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private SessionEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CurrentSession.FireEvent("LoadEventTabData");
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CurrentSession.FireEvent("LoadEventTabData");

                    this.BuildTabs();
                    this.RunComponent();
                }
            });

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "NewVersionAdded" || s == "VersionApproved") {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildTabs();
                    this.RunComponent();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.SessionEvent);
    }
    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
            this.BuildTabs();
            this.RunComponent();
        });
    }

    BuildTabs() {
        this.Tabs = [];

        var datePipe: DatePipe = new DatePipe("en-US");
        var from: string = "";
        var to: string = "";
        var header: string = "Version";
        var index: number = 0;
        
        this.EntityPM.TariffVersions.sort((a, b) => { return (a.Version === b.Version) ? 0 : (a.Version > b.Version) ? -1 : 1 }).forEach(item => {
            from = datePipe.transform(item.StartDate, 'dd/MM/yyyy');
            to = datePipe.transform(item.ExpirationDate, 'dd/MM/yyyy');

            header = "Version " + item.Version + " (" + from + " - " + to + ")";
           
            this.Tabs.push(new TariffDetailsTab(index, this.EditTabTariffType, header, item));
            index++;
        });
        
        this.Tabs.push(new TariffDetailsTab(index + 1, "GN", "General"));
        this.Tabs.push(new TariffDetailsTab(index + 2, "VH", "Version History"));
        this.Tabs.push(new TariffDetailsTab(index + 3, "EV", "Events"));
    }

    private Retries: number = 0;
    private timerToken: any;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {                
                this.SelectionChanged(this.Tabs[0]);
            }
        }

        else {
            this.RunComponentTimer();
        }
    }
    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    
    public SelectedTabItem: TariffDetailsTab;
    SelectionChanged(clickdTab: TariffDetailsTab) {
        if (clickdTab != null) {
            if (this.SelectedTabItem != clickdTab) {
                this.SelectedTabItem = clickdTab;

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

                if (location) {
                    location.viewContainerRef.clear();

                    if (this.SelectedTabItem.IsTabLoaded) {

                    }

                    else if (this.SelectedTabItem.ComponentPath) {
                        SessionLocator.DynamicLoader.Load(this.SelectedTabItem.ComponentPath, location.viewContainerRef).then(cmpRef => {
                            this.SelectedTabItem.IsTabLoaded = true;
       
                            if (this.SelectedTabItem.Code == this.EditTabTariffType) {
                                cmpRef.instance.Intialize({ CurrentVersion: this.SelectedTabItem.SelectedVersion, });
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
    public SelectedVersion: TariffVersionPM;
    constructor(index: number, code: string, header: string, version: TariffVersionPM = null) {
        this.Index = index;
        this.Code = code;
        this.Header = header;

        switch (this.Code) {
            case "VR": {
                this.IsDraft = version.IsDraft;
                this.SelectedVersion = version;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/VersionTabComponent";
                break;
            }
            case "SVR": {
                this.IsDraft = version.IsDraft;
                this.SelectedVersion = version;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/SurchargeVersionTabComponent";
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
        }
    }
}
