import {Component, ViewChildren, QueryList} from '@angular/core';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'TimeManagementWorkspaceComponent',
    moduleId: module.id,
    templateUrl: './TimeManagementWorkspaceComponent.html',
    providers: [EntityResourceService],
})

export class TimeManagementWorkspaceComponent {

    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService) {
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

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private SetSelectedItem() {
        this.SelectedItem = "TIMESHEET";
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
            this.SelectionChanged();
        }
    }

    private Page_TIMESHEET: any = null;
    private Page_REPORTS: any = null;
    private Page_SETTINGS: any = null;
    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
                if (myLocation != null) {

                    switch (this.SelectedItem) {

                        case "TIMESHEET": {
                            if (this.Page_TIMESHEET == null) {
                                SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/TimeSheet/TimeSheetWorkspaceComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_TIMESHEET = cmpRef.instance;
                                        this.Page_TIMESHEET.InitComponent();
                                    });
                            }

                            break;
                        }

                        case "SETTINGS": {
                            if (this.Page_SETTINGS == null) {
                                SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/SettingsWorkspaceComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_SETTINGS = cmpRef.instance;

                                    });
                            }
                            break;
                        }

                        case "REPORTS": {
                            if (this.Page_REPORTS == null) {
                                SessionLocator.DynamicLoader.Load('./TimeManagement/Components/Workspaces/ReportsWorkspaceComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_REPORTS = cmpRef.instance;
                                        this.Page_REPORTS.InitComponent();
                                    });
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
