declare var System: any;
declare var window: any;

import {Guid} from '../../Infrastructure/Utilities/Guid';
import {Component, OnInit, ChangeDetectorRef}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';
import {NewWarehouseReleaseComponent} from '../../Warehouse/Components/NewWarehouseReleaseComponent';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';
import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';

import {WarehouseReleaseList} from '../../Warehouse/EntityLists/WarehouseReleaseList';

import {AppTool, DateTool, FormatTool} from '../../Infrastructure/Tools';
import {WarehouseReleaseListExtendedService} from '../../Warehouse/Services/ExtendedLists/WarehouseReleaseListExtendedService';
import {WarehouseReleasePackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseReleasePackagePMExtendedService';

import {DeliveryPackagesTabComponent} from '../../ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesTabComponent';
import {FontTool} from '../../Infrastructure/Tools';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentDeliveryPM} from '../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {ShipmentPickUpDeliveryPackagePM} from '../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
@Component({
    moduleId: module.id,
    selector: 'CopyFromReleasesPackagesComponent',
    templateUrl: './CopyFromReleasesPackagesComponent.html',
    providers: [WarehouseReleaseListExtendedService, WarehouseReleasePackagePMExtendedService],
})

export class CopyFromReleasesPackagesComponent implements OnInit {
    IsNoReleasePackage: boolean = false;
    LabelPackageRleaseArea: string = "Pressing on Copy Packages button will copy the release packages to your Delivery Packages ";
    WarehouseReleaseLists: WarehouseReleaseClass[];
    ShipmentPM: ShipmentPM;
    ShipmentDeliveryPM: ShipmentDeliveryPM;
    FatherComponent: DeliveryPackagesTabComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _warehouseReleaseListExtendedService: WarehouseReleaseListExtendedService, public _warehouseReleasePackagePMExtendedService: WarehouseReleasePackagePMExtendedService) {
        this.WarehouseReleaseLists = [];
    }

    ngOnInit(

    ) {


    }


    SetWindowArgs(args: any) {

        this.ShipmentPM = args.ShipmentPM;
        this.ShipmentDeliveryPM = args.ShipmentDeliveryPM;
        this.FatherComponent = args.FatherComponent;
        
        this.LoadWarehouseReleasesPackages();
    }


    LoadWarehouseReleasesPackages() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.WarehouseReleaseLists = [];
        if (this.ShipmentPM != null) {
            this._warehouseReleaseListExtendedService.getWarehouseReleaseListsByShipmentId(this.ShipmentPM.Id, SessionLocator.Tenant).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
               // IsNoReleasePackage
                if (!pmResponse.HasError) {

                    if (pmResponse.Result && pmResponse.Result.length > 0) {
                        pmResponse.Result.forEach((item) => {
                            this.WarehouseReleaseLists.push(new WarehouseReleaseClass(item));

                        });
                    } else this.IsNoReleasePackage = true;
                
                }

               
            });
        }

 
    }



    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


  

    CopyPackagebuttonClick(item: WarehouseReleaseClass) {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.YesButtonText = "confirm";
        confirmWindow.Title = "Confirmation Message";
        confirmWindow.Show("Please confirm copying packages from " + (item.ReleaseNumberLabel + item.ReleaseNumberValue));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator("Copy release Package...");

                if (!item.IsLoad) {
                    this._warehouseReleasePackagePMExtendedService.GetWarehouseReleasePackagePMListsByWarehouseReleaseId(item.Id, SessionLocator.Tenant).subscribe(res => {
                        var pmResponse: ServiceResponse = res;
           
                        if (!pmResponse.HasError) {
                            item.WarehouseReleasePackage = pmResponse.Result;
                            item.IsLoad = true;
                            this.CopyReleasePackageToShipmentDeliveryPM(item);
                        } else this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    });

                } else {

                    this.CopyReleasePackageToShipmentDeliveryPM(item);
                }
            }
        });

    }



    CopyReleasePackageToShipmentDeliveryPM(item: WarehouseReleaseClass) {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();

        if (item != null) {

            if (item.WarehouseReleasePackage != null && item.WarehouseReleasePackage.length > 0) {
                item.WarehouseReleasePackage.forEach(item => {
                    var newPickUpPackPM = new ShipmentPickUpDeliveryPackagePM(this.ShipmentDeliveryPM);
                    newPickUpPackPM.Tenant = this.ShipmentDeliveryPM.Tenant;
                    newPickUpPackPM.ContainerNumber = item.ContainerNumber;
                    newPickUpPackPM.Description = item.Description;
                    newPickUpPackPM.PackageTypeId = item.PackageTypeId;
                    newPickUpPackPM.PackageTypeName = item.PackageTypeName;
                    newPickUpPackPM.Quantity = item.Quantity;
                    newPickUpPackPM.Volume = item.Volume;
                    newPickUpPackPM.Weight = item.Weight;
                    newPickUpPackPM.ShipperSeal = item.Seal;
                    newPickUpPackPM.Width = item.Width;
                    newPickUpPackPM.Height = item.Height;
                    newPickUpPackPM.Length = item.Length;
                    newPickUpPackPM.Harmonize = item.Harmonize;
                    newPickUpPackPM.ShipmentPickUpDeliveryId = this.ShipmentDeliveryPM.Id;
                    this.ShipmentDeliveryPM.AddPackage(newPickUpPackPM);
                    this.FatherComponent.BuildItemsSource();
                });

                this.CloseButtonClicked();
            }
        }

    }

    ViewEntity(item: WarehouseReleaseClass) {
        var myBackButtonLabel = "Warehouse Releases" ;

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: "WarehouseRelease", BackButtonLabel: myBackButtonLabel });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.LoadWarehouseReleasesPackages();
                  
                });
            });
    }

}


export class WarehouseReleaseClass   {

    IsLoad: boolean = false;
    ReleaseNumberValue: string = "";
    ReleaseNumberLabel: string = "";
    Id: string = "";
    WarehouseReleasePackage: WarehouseReleasePackagePM[] = [];
    ExpectedReleaseDate: Date;
    ActualReleaseDate: Date;
    ReleaseBy: string;
    StatusName: String = "";
    ReleaseDateBackgroudColor: string;
    ReleaseDate: Date;
    WarehouseReleaseDateType: string;
    constructor(warehouseReleaseLists: WarehouseReleaseList) {
        this.ReleaseNumberLabel = "Warehouse Release # ";

        this.ReleaseNumberValue = warehouseReleaseLists.ReleaseNumber;
        this.Id = warehouseReleaseLists.Id;
        this.ExpectedReleaseDate = warehouseReleaseLists.ExpectedReleaseDate;
        this.ActualReleaseDate = warehouseReleaseLists.ActualReleaseDate;
        this.ReleaseBy = warehouseReleaseLists.ReleaseBy;
        this.StatusName = warehouseReleaseLists.StatusName;

  
        this.ComputeReleaseDate(this);
     



    }

    ComputeReleaseDate(item: WarehouseReleaseClass) {
    
        if (item.ActualReleaseDate != null) {
            item.ReleaseDateBackgroudColor = FontTool.Green;
            item.WarehouseReleaseDateType = " (actual)";
            item.ReleaseDate = item.ActualReleaseDate;

        } else if (item.ExpectedReleaseDate != null) {
            item.ReleaseDateBackgroudColor = FontTool.Red;
            this.WarehouseReleaseDateType = " (expected)";
            item.ReleaseDate = item.ExpectedReleaseDate;
        }
        

       
    }


}




