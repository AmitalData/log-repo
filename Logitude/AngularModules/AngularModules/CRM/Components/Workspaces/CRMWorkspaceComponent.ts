import {Component, ViewChildren, QueryList} from '@angular/core';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'CRMComponent',
    moduleId: module.id,
    templateUrl: './CRMWorkspaceComponent.html',
    providers: [EntityResourceService],
})

export class CRMWorkspaceComponent {
    public IsOccasionVisible: boolean = false;
    public IsContactsVisible: boolean = false;

    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {

        this.RunComponent();

        if (FeatureLocator.HasFeaturePermession("Occasion", "Module")) {
            this.IsOccasionVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "CONTACTS")) {
            this.IsContactsVisible = true;
        }
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
                this.SelectedItem = "OVE";
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

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
            this.SelectionChanged();
        }
    }

    private Page_OVE: any = null;
    private Page_CUS: any = null;
    private Page_QUT: any = null;
    private Page_ACT: any = null;
    private Page_OPP: any = null;
    private Page_CON: any = null;
    private Page_DAS: any = null;
    private Page_OCC: any = null;
    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
                if (myLocation != null) {

                    switch (this.SelectedItem) {

                        case "OVE": {
                            if (this.Page_OVE == null) {
                                this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
                                    this._entityResourceService.getEntityResourceByTableName("Opportunity", 0).subscribe(response2 => {
                                        SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/OverviewWorkspaceComponent', myLocation.viewContainerRef)
                                            .then(cmpRef => {
                                                this.Page_OVE = cmpRef.instance;
                                                this.Page_OVE.InitComponent(this);
                                            });
                                    });
                                });
                            }

                            else {
                                this.Page_OVE.LoadAllScreenData();
                            }

                            break;
                        }

                        case "CUS": {
                            if (this.Page_CUS == null) {
                                this._entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(response => {
                                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/CustomerWorkspaceComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_CUS = cmpRef.instance;
                                        });
                                });
                            }

                            break;
                        }

                        case "QUT": {
                            if (this.Page_QUT == null) {
                                SessionLocator.DynamicLoader.Load('./Quote/Components/Workspaces/QuotesComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_QUT = cmpRef.instance;
                                    });
                            }

                            break;
                        }

                        case "ACT": {
                            if (this.Page_ACT == null) {
                                this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
                                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/ActivityWorkspaceComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_ACT = cmpRef.instance;
                                        });
                                });
                            }

                            else {
                                this.Page_ACT.LoadUpcomingEntities();
                            }
                            break;
                        }

                        case "OPP": {
                            if (this.Page_OPP == null) {
                                this._entityResourceService.getEntityResourceByTableName("Opportunity", 0).subscribe(response => {
                                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/OpportunityWorkspaceComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_OPP = cmpRef.instance;
                                        });
                                });
                            }

                            break;
                        }

                        case "CON": {
                            if (this.Page_CON == null) {
                                this._entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(response => {
                                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/ContactWorkspaceComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_CON = cmpRef.instance;
                                            this.Page_CON.InitComponent(this);
                                        });
                                });
                            }

                            break;
                        }

                        case "DAS": {
                            if (this.Page_DAS == null) {
                                SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/DashboardWorkspaceComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_DAS = cmpRef.instance;
                                        //this.Page_DAS.InitComponent();
                                    });
                            }

                            break;
                        }

                        case "OCC": {
                            if (this.Page_OCC == null) {
                                this._entityResourceService.getEntityResourceByTableName("Occasion", 0).subscribe(response => {
                                    SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/OccasionWorkspaceComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_OCC = cmpRef.instance;
                                        });
                                });
                            }

                            break;
                        }
                    }

                    this.CurrentSession.ChangeSessionHeader({ Text: TextCodeTranslator.Translate("General.MH.CRM") + "\\" + this.GetPageName() });
                }
            }
        }
    }

    GetPageName() {
        var myResult = "";

        switch (this.SelectedItem) {
            case "OVE": { myResult = "Overview"; break; }
            case "CUS": { myResult = TextCodeTranslator.Translate("General.MH.Customers");; break; }
            case "QUT": { myResult = TextCodeTranslator.Translate("General.MH.Quotes");; break; }
            case "ACT": { myResult = "Activities"; break; }
            case "OPP": { myResult = "Opportunities"; break; }
            case "CON": { myResult = TextCodeTranslator.Translate("General.MH.Contacts"); break; }
            case "DAS": { myResult = "Dashboard"; break; }
        }

        return myResult;
    }

    private mySelectedActivityFilter: string = "All";
    get SelectedActivityFilter() { return this.mySelectedActivityFilter; }
    set SelectedActivityFilter(value: string) {
        if (this.mySelectedActivityFilter != value) {
            this.mySelectedActivityFilter = value;
            this.Page_ACT.SelectedActivityFilter = value;
        }
    }

    // Upcoming
    private upcomingCount = 0;
    get UpcomingCount() { return this.upcomingCount; }
    set UpcomingCount(value: number) {
        this.upcomingCount = value;
    }

    private upcomingCountVisibility = false;
    get UpcomingCountVisibility() { return this.upcomingCountVisibility; }
    set UpcomingCountVisibility(value: boolean) {
        this.upcomingCountVisibility = value;
    }

}
