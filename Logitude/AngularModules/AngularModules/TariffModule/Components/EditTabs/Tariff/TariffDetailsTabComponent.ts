import { Component, OnInit, OnDestroy, ViewChildren, QueryList } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { DateTimePipe } from '../../../../Controls/Pipes/DateTimePipe';
@Component({
    moduleId: module.id,
    templateUrl: './TariffDetailsTabComponent.html',
})

export class TariffDetailsTabComponent implements OnInit, OnDestroy {
    public EntityPM: TariffPM;
    public Tabs: TariffDetailsTab[] = [];
    private CurrentSession = SessionLocator.SelectedSession;    
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Tabs = [];
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    public ExistanceDraft: boolean = false;

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
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
    ngOnInit() {
        this.BuildTabs();
        this.RunComponent();        
    }

    NewDraftClicked() {



    }

    BuildTabs() {
        this.Tabs = [];
        var DatePipe = new DateTimePipe();
        var Draft = this.EntityPM.TariffVersions.filter(p => p.IsDraft == true)[0];
        if (Draft != null) {
            this.Tabs.push(new TariffDetailsTab(0, "VR", Draft.Version+""));
            this.ExistanceDraft = true;
        }
        else {
            this.ExistanceDraft = false;
        }
        var Index = 1;
        this.EntityPM.TariffVersions.filter(p => p.IsDraft == false).forEach(item => {
            this.Tabs.push(new TariffDetailsTab(Index, "Version " + item.Version + " (" + DatePipe.transform(item.StartDate, "D") + "-" + DatePipe.transform(item.ExpirationDate, "D") + ")", item.Version+""));
            Index++;
        });
        this.Tabs.push(new TariffDetailsTab(++Index, "GN"));
        this.Tabs.push(new TariffDetailsTab(++Index, "VH"));
        this.Tabs.push(new TariffDetailsTab(++Index, "EV"));
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
                    if (this.SelectedTabItem.IsTabLoaded) {

                    }

                    else if (this.SelectedTabItem.ComponentPath) {
                        SessionLocator.DynamicLoader.Load(this.SelectedTabItem.ComponentPath, location.viewContainerRef).then(cmpRef => {
                            this.SelectedTabItem.IsTabLoaded = true;
                            cmpRef.instance.SetWindowArgs(clickdTab.Version);
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
    public Version: string = null;
    constructor(index: number, code: string, version: string = null) {
        this.Index = index;
        this.Code = code;
        this.Version = version;
        switch (this.Code) {
            case "VR": {
                this.Header = "Draft";
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/VersionTabComponent";
                break;
            }

            case "GN": {
                this.Header = "General";
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/TariffGeneralTabComponent";
                break;
            }

            case "VH": {
                this.Header = "Version History";
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/VersionHistoryTabComponent";
                break;
            }

            case "EV": {
                this.Header = "Events";
                this.ComponentPath = "./Common/Components/Events/EventsTabComponent";
                break;                    
            }

            default: {
                this.Header = this.Code;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/VersionTabComponent";
                break;
            }
        }
    }
}
