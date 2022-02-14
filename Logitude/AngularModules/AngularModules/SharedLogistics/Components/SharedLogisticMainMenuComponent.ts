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
     
       
            this.CustomerTenantAccessVisibility = false;
            this.CargoTrackingAccessVisibility = false;

            this.SelectedItem = "SHLO";
      
        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
            this.CustomerTenantAccessVisibility = true;
           
        }

        if (FeatureLocator.HasFeaturePermession("General", "SHLOGCARGOTRACKING")) {
            this.CargoTrackingAccessVisibility = true;
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

    private Page_BOOK: any = null;
    private Page_SHIP: any = null;
    private Page_CATR: any = null;
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
                    }
                }
            }
        }
    }
}
