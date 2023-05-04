import {Component, ViewChildren, QueryList} from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';

@Component({

    selector: 'SharedLogisticMainMenuComponent',
    templateUrl: './SharedLogisticMainMenuComponent.html',
    providers: [EntityResourceService],
})

export class SharedLogisticMainMenuComponent {
    public CustomerTenantAccessVisibility: boolean = false;
    public CargoTrackingAccessVisibility: boolean = false;
    public CtoolAccessVisibility: boolean = false;
    public IsDigitalPortalVisibile: boolean = false;
    public SharedLogisticAndMobileVisibility: boolean = false;
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
    private CheckFeatures() {
        this.CustomerTenantAccessVisibility = false;
        this.CargoTrackingAccessVisibility = false;
        this.CtoolAccessVisibility = false;
        this.IsDigitalPortalVisibile = false;
        this.SharedLogisticAndMobileVisibility = false;

        if (FeatureLocator.HasFeaturePermession("General", "SHLOGANDMOBILE")) {
            this.SharedLogisticAndMobileVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
            this.CustomerTenantAccessVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "SHLOGCARGOTRACKING")) {
            this.CargoTrackingAccessVisibility = true;
        }

        let collaborationToolFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "CTL")[0];
        if (collaborationToolFeatureToggle) {
            this.CtoolAccessVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "SHLOGDIGITALPORTAL")) {
            this.IsDigitalPortalVisibile = true;
        }
    }
    private SetSelectedItem() {
        this.CheckFeatures();
        if (this.SharedLogisticAndMobileVisibility) {
            this.SelectedItem = "SHLO";
        }
        else if (this.IsDigitalPortalVisibile) {
            this.SelectedItem = "DIGP";
        }
        else if (this.CustomerTenantAccessVisibility) {
            this.SelectedItem = "LOBO";
        }
        else if (this.CargoTrackingAccessVisibility) {
            this.SelectedItem = "CATR";
        }
        else if (this.CtoolAccessVisibility) {
            this.SelectedItem = "CTOOL";
        }
        return;
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
            this.SelectionChanged();
        }
    }

    private Page_BOOK: any = null;
    private Page_SHIP: any = null;
    private Page_CATR: any = null;
    private Page_CTOOL: any = null;
    private Page_DIGP: any = null;

    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
                if (myLocation != null) {

                    switch (this.SelectedItem) {
                        //SharedLogistics
                        case "SHLO": {
                            if (this.Page_SHIP == null) {
                                SessionLocator.DynamicLoader.Load('./SharedLogistics/Components/SharedLogisticsMainComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_SHIP = cmpRef.instance;
                                        //this.Page_SHIP.InitComponent();
                                    });
                            }

                            break;
                        }
                        //DIGP
                        case "DIGP": {
                            if (this.Page_DIGP == null) {
                                SessionLocator.DynamicLoader.Load('./SharedLogistics/Components/SharedLogisticsDigitalPortalComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_DIGP = cmpRef.instance;
                                        this.Page_DIGP.SetSharedTitleType("DigitalPortal");
                                        //this.Page_SHIP.InitComponent();
                                    });
                            }

                            break;
                        }
                        //LogBox
                        case "LOBO": {
                            if (this.Page_BOOK == null) {
                                SessionLocator.DynamicLoader.Load('./SharedLogistics/Components/CutsomerTenantAccessManagementComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_SHIP = cmpRef.instance;
                                        //this.Page_SHIP.InitComponent();
                                    });
                            }

                            break;
                        }

                        //CargoTracking
                        case "CATR": {
                            if (this.Page_CATR == null) {
                                SessionLocator.DynamicLoader.Load('./SharedLogistics/Components/SharedLogisticsMainComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_CATR = cmpRef.instance;
                                        this.Page_CATR.SetSharedTitleType("CargoTracking");
                                    });
                            }
                            break;
                        }

                        case "CTOOL": {
                            if (this.Page_CTOOL == null) {
                                SessionLocator.DynamicLoader.Load('./SharedLogistics/Components/SharedLogisticsMainComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_CTOOL = cmpRef.instance;
                                        this.Page_CTOOL.SetSharedTitleType("CTool");
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
