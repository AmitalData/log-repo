import {Component, ViewChildren, QueryList, OnInit} from '@angular/core';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
@Component({
    moduleId: module.id,
    selector: 'OperationsComponent',
    templateUrl: './OperationsComponent.html',
    providers: [EntityResourceService],
})

export class OperationsComponent implements OnInit {
    public IsMenuVisible: boolean = false;
    public IsBookingItemVisible: boolean = false;
    public IsSharedManifestItemVisible: boolean = false;
    public IsContainersFUItemVisible: boolean = false;
    public IsResourcesReady: boolean = false;
    public IsAMANACItemVisible: boolean = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService) {

    }

    ngOnInit() {
        var listservice: EntityListService = new EntityListService();
        var loadPr = listservice.getMock("Port");
        loadPr.then((res: any) => {
            res.subscribe(resp => {
                this._entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(res1 => {
                    this._entityResourceService.getEntityResourceByTableName("Master", 0).subscribe(res2 => {
                        this.IsResourcesReady = true;

                        if (FeatureLocator.HasFeaturePermession("Booking", "Booking.Menu")) {
                            this.IsBookingItemVisible = true;
                            this.IsMenuVisible = true;
                        }

                        if (FeatureLocator.HasFeaturePermession("Shipment", "AgentSharedManifest")) {
                            this.IsSharedManifestItemVisible = true;
                            this.IsMenuVisible = true;
                        }

                        if (FeatureLocator.HasFeaturePermession("Shipment", "Area.ContainersFU")) {
                            this.IsContainersFUItemVisible = true;
                            this.IsMenuVisible = true;
                        }

                        if (ObjectsLocator.CustomsInterfaceSettingPM.LocalCustomsInterfaceCode == "AMC" ) {
                            this.IsAMANACItemVisible = true;
                            this.IsMenuVisible = true;
                        }

                        this.RunComponent();
                    });
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
        if (FeatureLocator.HasFeaturePermession("Booking", "Booking.Menu")) {
            this.SelectedItem = "BOOK";
        }

        else {
            this.SelectedItem = "SHIP";
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

    private Page_SHMA: any = null;
    private Page_BOOK: any = null;
    private Page_SHIP: any = null;
    private Page_CNFU: any = null;
    private Page_AMANAC: any = null;

    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
                if (myLocation != null) {

                    switch (this.SelectedItem) {

                        case "BOOK": {
                            if (this.Page_BOOK == null) {
                                this._entityResourceService.getEntityResourceByTableName("Booking", 0).subscribe(response => {
                                    SessionLocator.DynamicLoader.Load('./Booking/Components/Workspaces/BookingsComponent', myLocation.viewContainerRef)
                                        .then(cmpRef => {
                                            this.Page_BOOK = cmpRef.instance;
                                            this.Page_BOOK.InitComponent();
                                        });
                                });
                            }

                            break;
                        }

                        case "SHIP": {
                            if (this.Page_SHIP == null) {

                                SessionLocator.DynamicLoader.Load('./Shipment/Components/Workspaces/ShipmentsComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_SHIP = cmpRef.instance;
                                        this.Page_SHIP.InitComponent();
                                    });
                            }

                            break;
                        }

                        case "SHMA": {
                            if (this.Page_SHMA == null) {
                                this._entityResourceService.getEntityResourceByTableName("AgentSharedManifest", 0).subscribe(response => {
                            
                                    SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestsWorkSpaces', myLocation.viewContainerRef)
                                            .then(cmpRef => {
                                                this.Page_SHMA = cmpRef.instance;
                                                this.Page_SHMA.InitComponent();
                                            });
                                 
                                });
                            }

                            break;
                        }

                        case "CNFU": {

                            SessionLocator.DynamicLoader.Load('./Shipment/Components/Workspaces/ContainersFUsComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.Page_CNFU = cmpRef.instance;
                                });

                            break;
                        }


                        case "AMANAC": {

                            SessionLocator.DynamicLoader.Load('./Shipment/Components/Workspaces/AMANACComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.Page_AMANAC = cmpRef.instance;
                                });

                            break;
                        }
                    }
                }
            }
        }
    }
}
