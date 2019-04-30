import { Component, OnInit, OnDestroy, ViewChildren, QueryList } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';

@Component({
    moduleId: module.id,
    templateUrl: './TariffDetailsTabComponent.html',
})

export class TariffDetailsTabComponent implements OnInit, OnDestroy {
    public EntityPM: TariffPM;
    public Tabs: TariffDetailsTab[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Tabs = [];
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
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

    BuildTabs() {
        this.Tabs = [];
        this.Tabs.push(new TariffDetailsTab(0, "GN"));
        this.Tabs.push(new TariffDetailsTab(1, "VH"));
        this.Tabs.push(new TariffDetailsTab(2, "EV"));
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
    constructor(index: number, code: string) {
        this.Index = index;
        this.Code = code;

        switch (this.Code) {
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
        }
    }
}
