import {Component, ViewChildren, QueryList} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'TimeSheetWorkspaceComponent',
    moduleId: module.id,
    templateUrl: './TimeSheetWorkspaceComponent.html',
    providers: [EntityResourceService],

})

export class TimeSheetWorkspaceComponent {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService) {
    }

    InitComponent() {
        this.RunComponent();
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
                this.SetSelectedItem();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private SetSelectedItem() {
        this.SelectedTabCode = "Daily";
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }
    private PageChild_Daily: any = null;
    public PageChild_Weekly: any = null;
    public PageChild_Monthly: any = null;
    public PageChild_ClockTime: any = null;
    public PageChild_Vacations: any = null;
    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedTabCode != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
                if (myLocation != null) {

                    switch (this.SelectedTabCode) {
                        case "Daily": {
                            if (this.PageChild_Daily == null) {
                                this._entityResourceService.getEntityResourceByTableName("TMEmployeeTime", 0).subscribe(response => {
                                    this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(response => {
                                SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/DailyTimeSheetComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_Daily = cmpRef.instance;
                                        this.PageChild_Daily.InitTab(this);
                                            });
                                    });
                                });
                            }
                            else {
                                this.PageChild_Daily.RefreshTab();
                            }

                            break;
                        }

                        case "Weekly": {
                            if (this.PageChild_Weekly == null) {
                                this._entityResourceService.getEntityResourceByTableName("TMEmployeeTime", 0).subscribe(response => {
                                    this._entityResourceService.getEntityResourceByTableName("TMProject", 0).subscribe(response => {
                                SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/WeeklyTimeSheetComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_Weekly = cmpRef.instance;
                                        this.PageChild_Weekly.InitTab(this);
                                        });
                                    });
                                });
                            }

                            else {
                                this.PageChild_Weekly.RefreshTab();
                            }

                            break;
                        }

                        case 'Monthly': {
                            if (this.PageChild_Monthly == null) {
                                SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/MonthlyTimeSheetComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_Monthly = cmpRef.instance;
                                        this.PageChild_Monthly.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_Monthly.RefreshTab();
                            }

                            break;
                        }


                        case 'ClockTime': {
                            if (this.PageChild_ClockTime == null) {
                                SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/ClockTimeComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_ClockTime = cmpRef.instance;
                                        this.PageChild_ClockTime.InitTab(this);
                                    });
                            }

                            else {
                                this.PageChild_ClockTime.RefreshTab();
                            }

                            break;
                        }

                        case "Vacations": {

                            if (this.PageChild_Vacations == null) {
                                SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/VacationsComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_Vacations = cmpRef.instance;
                                        this.PageChild_Vacations.InitTab();
                                    });
                            }

                            else {
                                this.PageChild_Vacations.LoadAllScreenData();
                            }

                            break;
                        }
                    }
                }
            }
        }
    }
}
