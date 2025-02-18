import {Component, ViewChildren, QueryList, AfterViewInit} from '@angular/core';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
@Component({
    
    selector: 'OperationsComponent',
    templateUrl: './OperationsComponent.html',
    providers: [EntityResourceService],
})

export class OperationsComponent implements AfterViewInit {
    public IsMenuVisible: boolean = false;
    public IsBookingItemVisible: boolean = false;
    public IsSharedManifestItemVisible: boolean = false;
    public IsContainersFUItemVisible: boolean = false;
    public IsResourcesReady: boolean = false;
    public IsAMANACItemVisible: boolean = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService) {
        if (FeatureLocator.HasFeaturePermession("Booking", "Booking.Menu")) {
            this.selectedItem = "BOOK";
        }

        else {
            this.selectedItem = "SHIP";
        }
    }

    ngAfterViewInit() {
        var listservice: EntityListService = new EntityListService();
        var loadPr = listservice.getMock("Port");
        loadPr.then((res: any) => {
            res.subscribe((resp:any) => {
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

                        this.SelectionChanged();
                    });
                });
            });
        });
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
        if (this.SelectedItem != null) {

            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedItem)[0];
            if (myLocation != null) {

                switch (this.SelectedItem) {

                    case "BOOK": {
                        if (this.Page_BOOK == null) {
                            this._entityResourceService.getEntityResourceByTableName("Booking", 0).subscribe((response: any) => {
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
                            this._entityResourceService.getEntityResourceByTableName("AgentSharedManifest", 0).subscribe((response: any) => {

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
                        if (this.Page_CNFU == null) {
                            SessionLocator.DynamicLoader.Load('./Shipment/Components/Workspaces/ContainersFUsComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.Page_CNFU = cmpRef.instance;
                                });
                        }

                        break;
                    }


                    case "AMANAC": {
                        if (this.Page_AMANAC == null) {
                            SessionLocator.DynamicLoader.Load('./Shipment/Components/Workspaces/AMANACComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.Page_AMANAC = cmpRef.instance;
                                });
                        }

                        break;
                    }
                }
            }
        }
    }
}
