import { Component, ViewChildren, QueryList, OnInit } from '@angular/core';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    selector: 'MainReportsWorkspace',
    templateUrl: './MainReportsWorkspace.html',
    providers: [EntityResourceService],
})

export class MainReportsWorkspace implements OnInit {
    public IsMenuVisible: boolean = false;
    public IsBIItemVisible: boolean = false;
    public IsReportItemVisible: boolean = false;
    public IsResourcesReady: boolean = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService) {

    }
    ngOnInit() {
      
        this._entityResourceService.getEntityResourceByTableName("BIReportFolder", 0).subscribe(response => {
            this._entityResourceService.getEntityResourceByTableName("BIReport", 0).subscribe(response => {
                this._entityResourceService.getEntityResourceByTableName("Report", 0).subscribe(response => {
                this.IsResourcesReady = true;

                if (FeatureLocator.HasFeaturePermession("BIReport", "BIReport.Menu")) {
                    this.IsBIItemVisible = true;
                    this.IsMenuVisible = true;
                }

                    if (FeatureLocator.HasFeaturePermession("Report", "READ")) {
                    this.IsReportItemVisible = true;
                }


                this.RunComponent();
                });
            });
        });
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
        if (FeatureLocator.HasFeaturePermession("Report", "READ")) {
            this.SelectedItem = "Report";
        }
        else {
            this.SelectedItem = "BI";
        }
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
            this.SelectionChanged();
        }
    }


    private Page_BI: any = null;
    private Page_Report: any = null;

    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
                if (myLocation != null) {

                    switch (this.SelectedItem) {

                        case "Report": {
                            if (this.Page_Report == null) {
                                this._entityResourceService.getEntityResourceByTableName("Booking", 0).subscribe(response => {
                                    SessionLocator.DynamicLoader.Load('./Report/Components/Workspaces/ReportComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_Report = cmpRef.instance;
                                            this.Page_Report.InitComponent();
                                        });
                                });
                            }
                            break;
                        }

                        case "BI": {
                            if (this.Page_BI == null) {
                                SessionLocator.DynamicLoader.Load('./Report/Components/Workspaces/BIFolderReportComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_BI = cmpRef.instance;
                                        this.Page_BI.InitComponent();
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
