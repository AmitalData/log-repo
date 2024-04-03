declare var System: any;
declare var window: any;
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';

import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {WarehouseReleasePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseReleasePMExtendedService';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../Controls/Windows/LogitudeWindow';
@Component({
    
    selector: 'ChoosePackagesFromWarehousePackageReleasesComponent',
    templateUrl: './ChoosePackagesFromWarehousePackageReleasesComponent.html',
})

export class ChoosePackagesFromWarehousePackageReleasesComponent extends BaseComponent implements OnInit {
  public SelectedWarehouseEntryPackage: any;

    private _entityResourceService: EntityResourceService = new EntityResourceService();

    ObjectTableName: string = "WarehouseRelease";
    WarehouseReleasePMLists: WarehouseReleasePM[] = [];
    WarehouseReleaseGroupLists: WarehouseReleaseGroup[] = [];
    AllWarehouseReleaseGroupLists: WarehouseReleaseGroup[] = [];


    warehouseReleasePMExtendedService: WarehouseReleasePMExtendedService;
    DataContext: any = this;
    private CurrentSession = SessionLocator.SelectedSession;
    ViewModelTrigger: any;
    IsLoadPage: boolean = false;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsUnitCode: string;
    VolumetricWeightLabel: string;
    IsContainerShipment: boolean = false;
    IsShowMessageNoResult: boolean = false;
    ShowNewWarehouseReleaseButton: boolean = false;

    constructor() {
        super();
        this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, true);
        this.warehouseReleasePMExtendedService = new WarehouseReleasePMExtendedService();
    }

    ngOnInit(

    ) {

    }

    Shipment: any;
    SetWindowArgs(args: any) {
        this._entityResourceService.getEntityResourceByTableName("WarehouseReleasePackage").subscribe((response:any) => {
            this.Initialize(args);
        });

    }

    Initialize(args) {
        this.ViewModelTrigger = args.ViewModelTrigger;
        this.Shipment = this.ViewModelTrigger ? this.ViewModelTrigger.EntityPM : null;
        this.ShowNewWarehouseReleaseButton = args.ShowNewWarehouseReleaseButton;
        this.IsContainerShipment = args.IsContainer;

        if (this.Shipment) {
            this.CustomerId = this.Shipment.CustomerId;
            this.WarehouseId = this.Shipment.WarehouseLegWarehouseId;
        }

        this.SetHeaderLable();


    }

    SetHeaderLable() {
        this.VolumeLabel = "Volume (" + SessionLocator.TenantPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + SessionLocator.TenantPM.GrossWeightUnitCode + ")";
        this.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;;
        this.VolumetricWeightLabel = "Volumetric Weight (" + SessionLocator.TenantPM.ChargeableWeightUnitCode + ")";


    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }



    HideWarehouseReleaseGroup(warehouseReleaseGroup: WarehouseReleaseGroup) {
        warehouseReleaseGroup.IsHide = !warehouseReleaseGroup.IsHide;

        if (warehouseReleaseGroup.IsHide) {
            warehouseReleaseGroup.WarehouseReleasePMLists = [];
        } else {

            var warehouseReleasePMLists = this.AllWarehouseReleaseGroupLists.filter(d => d.Title == warehouseReleaseGroup.Title)[0].WarehouseReleasePMLists;
            if (warehouseReleasePMLists) {
                warehouseReleasePMLists.forEach((item) => {
                    warehouseReleaseGroup.WarehouseReleasePMLists.push(item);
                });
            }

        }
    }


    GetWarehouseReleasePMLists(title: string) {
        var warehouseRelease: any = [];
        if (this.WarehouseReleasePMLists) {
            if (title == "Not Connected") {
                warehouseRelease = this.WarehouseReleasePMLists.filter(d => AppTool.IsNullOrEmpty(d.ShipmentId));

            } else if (title == "Connected to my Shipment") {
                warehouseRelease = this.WarehouseReleasePMLists.filter(d => !AppTool.IsNullOrEmpty(d.ShipmentId) && d.ShipmentId == this.Shipment.Id);
            }

            else if (title == "Connected to other Shipments") {
                warehouseRelease = this.WarehouseReleasePMLists.filter(d => !AppTool.IsNullOrEmpty(d.ShipmentId) && d.ShipmentId != this.Shipment.Id);
            }
        }
        return warehouseRelease;
    }

    LoadWarehouseReleasePackages() {
        this.WarehouseReleaseGroupLists = [];
        this.AllWarehouseReleaseGroupLists = [];
        this.IsShowMessageNoResult = false;
        if (this.CustomerId && this.WarehouseId) {

            this.CurrentSession.StartBusyIndicatorLoading();
            this.warehouseReleasePMExtendedService.GetWarehouseReleaseByCustomerIdAndwarehouseId(this.customerId, this.WarehouseId).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    if (myResponse.Result && myResponse.Result.length > 0) {
                        this.WarehouseReleasePMLists = myResponse.Result;


                        this.AllWarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.GetWarehouseReleasePMLists("Not Connected"), "Not Connected", this));
                        this.AllWarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.GetWarehouseReleasePMLists("Connected to my Shipment"), "Connected to my Shipment", this));
                        this.AllWarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.GetWarehouseReleasePMLists("Connected to other Shipments"), "Connected to other Shipments", this));

                        this.WarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.GetWarehouseReleasePMLists("Not Connected"), "Not Connected", this));
                        this.WarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.GetWarehouseReleasePMLists("Connected to my Shipment"), "Connected to my Shipment", this));

                        if (this.isShowConnectedToOtherShipments) {
                            this.WarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(this.GetWarehouseReleasePMLists("Connected to other Shipmentst"), "Connected to other Shipments", this));
                        } else {
                            if (this.WarehouseReleaseGroupLists.filter(d => d.ReleasePackageCount != 0 && d.Title != "Connected to other Shipments").length == 0) {
                                this.IsShowMessageNoResult = true;
                            }
                        }



                    } else this.IsShowMessageNoResult = true;
                }
                else if (myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }
                this.IsLoadPage = true;

            });
        } else this.IsLoadPage = true;
    }

    NewWarehouseReleaseButtonClicked() {
        if (!this.ShowNewWarehouseReleaseButton) return;
        var windowArgs: any = {};
        windowArgs.ShipmentPM = this.Shipment;
        windowArgs.ConnectedTo = "Shipment";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1030;
        logWindow.Height = 620;
        logWindow.Title = "New Cross Dock Release";
        logWindow.WindowArgs = windowArgs;
        var widnowPath: string = "./Warehouse/Components/NewWarehouseReleaseComponent";
        logWindow.Show(widnowPath);
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "Refresh") {
                this.LoadWarehouseReleasePackages();
            }
        });
    }

    SaveButtonClicked() {


        var warehouseReleasePackagePMLists = [];
        var warehouseReleasesIds: string = "";
        this.WarehouseReleaseGroupLists.forEach((warehouseReleaseGroup) => {
            if (warehouseReleaseGroup.WarehouseReleasePMLists) {
                warehouseReleaseGroup.WarehouseReleasePMLists.filter(d => d.IsUsed).forEach((warehouseReleasePM) => {
                    if (warehouseReleasePM.WarehouseReleasePackages) {
                        warehouseReleasePM.WarehouseReleasePackages.forEach((warehouseReleasePackagePM) => {
                            warehouseReleasePackagePMLists.push(warehouseReleasePackagePM);
                        });

                        warehouseReleasesIds += warehouseReleasePM.Id + ",";
                    }

                });
            }
        });

        if (warehouseReleasesIds && this.ViewModelTrigger.EntityPM) {
            warehouseReleasesIds += ")";
            warehouseReleasesIds = warehouseReleasesIds.replace(",)", "");
            this.ViewModelTrigger.EntityPM.WarehouseReleasesIds = warehouseReleasesIds;
        }

        this.ViewModelTrigger.GeneratePackagesFromWarehouseReleasesPackages(warehouseReleasePackagePMLists);
        this.CloseButtonClicked();

    }




    private customerId: string;
    get CustomerId() {
        return this.customerId;

    }
    set CustomerId(newValue: string) {
        if (this.customerId != newValue) {
            this.customerId = newValue;
            this.LoadWarehouseReleasePackages();
        }
    }


    private warehouseId: string;
    get WarehouseId() {
        return this.warehouseId;

    }
    set WarehouseId(newValue: string) {
        if (this.warehouseId != newValue) {
            this.warehouseId = newValue;
            this.LoadWarehouseReleasePackages();
        }
    }








    private isShowConnectedToOtherShipments: boolean;
    get IsShowConnectedToOtherShipments() {
        return this.isShowConnectedToOtherShipments;

    }
    set IsShowConnectedToOtherShipments(newValue: boolean) {
        if (this.isShowConnectedToOtherShipments != newValue) {
            this.isShowConnectedToOtherShipments = newValue;
            if (this.isShowConnectedToOtherShipments) {

                if (!this.WarehouseReleaseGroupLists.filter(d => d.Title == "Connected to other Shipments")[0]) {

                    var warehouseReleasePMLists = this.GetWarehouseReleasePMLists("Connected to other Shipments");
                    if (this.WarehouseReleasePMLists && this.WarehouseReleasePMLists.length > 0) {
                        this.IsShowMessageNoResult = false;
                        this.WarehouseReleaseGroupLists.push(new WarehouseReleaseGroup(warehouseReleasePMLists, "Connected to other Shipments", this));
                    }
                }
            } else {
                if (this.WarehouseReleaseGroupLists && this.WarehouseReleaseGroupLists.length > 0) {
                    this.IsShowMessageNoResult = false;
                    this.WarehouseReleaseGroupLists = this.WarehouseReleaseGroupLists.filter(d => d.Title != "Connected to other Shipments");
                }
            }

        }
    }






    ViewReleaseClicked(warehouseReleaseItem) {

        var myBackButtonLabel = "Choose Packages";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: warehouseReleaseItem.Id, ObjectTableName: "WarehouseRelease", BackButtonLabel: myBackButtonLabel });

                
                cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.LoadWarehouseReleasePackages();
                    }
                });
            });
    }

}

class WarehouseReleaseGroup {
    WarehouseReleasePMLists: WarehouseReleasePM[] = [];
    Title: string;
    IsHide: boolean = true;
    ReleasePackageCount: number = 0;
    constructor(warehouseReleasePMLists: WarehouseReleasePM[], title: string, viewModel: ChoosePackagesFromWarehousePackageReleasesComponent) {
        this.WarehouseReleasePMLists = warehouseReleasePMLists;
        this.Title = title;

        this.WarehouseReleasePMLists.forEach((item) => {
            if (item.WarehouseReleasePackages) {
                item.WarehouseReleasePackages.forEach((warehouseReleasePackage) => {
                    warehouseReleasePackage.ReleaseNumber = item.ReleaseNumber;
                });
            }
        });


        if (viewModel.IsContainerShipment) {
            this.WarehouseReleasePMLists.forEach((item) => {
                if (item.WarehouseReleasePackages && item.WarehouseReleasePackages.length > 0) {
                    item.WarehouseReleasePackages = item.WarehouseReleasePackages.filter(d => d.IsContainer);
                }
            });

            this.WarehouseReleasePMLists = this.WarehouseReleasePMLists.filter(d => d.WarehouseReleasePackages && d.WarehouseReleasePackages.length > 0);

        }

        if (this.WarehouseReleasePMLists && this.WarehouseReleasePMLists.length > 0) {
            this.IsHide = false;
            this.ReleasePackageCount = this.WarehouseReleasePMLists.length;
        }



    }


    private haveWarehouseReleasePackages: boolean;
    get HaveWarehouseReleasePackages() {
        return ((this.WarehouseReleasePMLists && this.WarehouseReleasePMLists.length > 0) || this.IsHide ? true : false);
    }

}

