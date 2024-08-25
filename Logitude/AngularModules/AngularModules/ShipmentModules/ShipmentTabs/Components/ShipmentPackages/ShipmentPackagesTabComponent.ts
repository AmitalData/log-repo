import { Component, EventEmitter, OnDestroy, OnInit } from "@angular/core";
import { PackageTypeList } from "Common/EntityLists/PackageTypeList";
import { PackageTypeListService } from "Common/Services/StandardLists/PackageTypeListService";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { AppTool, FormatTool } from "Infrastructure/Tools";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { Validator } from "Infrastructure/Validators/Validator";
import { ShipmentPackagePM } from "Shipment/EntityPMs/ShipmentPackagePM";

@Component({
    templateUrl: './ShipmentPackagesTabComponent.html',
})

export class ShipmentPackagesTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public IsReadOnly: boolean = false;
    public ItemsSource: ObservableCollection;
    public IsDisplayOnly: boolean = false;
    public IsDisplayMessage: boolean = false;
    public DisplayOnlyMessage: string = "";
    SelectedRow: ShipmentPackageItemLine;
    ChangeScrollPosition: EventEmitter<any> = new EventEmitter();
    public ObjectTableName = "ShipmentPackages";


    constructor(public entityArgs: EntityArgs) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.EntityPM = this.entityArgs.EntityPM;
        this.DisplayOnlyCheck();

    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.Add();
        }
    }

    DisplayOnlyCheck() {
        if (this.EntityPM.ShipmentTypeId == "LCLD") {
            this.IsDisplayOnly = true;
            this.IsDisplayMessage = true;
            this.DisplayOnlyMessage = TextCodeTranslator.Translate("ShipmentPackage.O.DisplayOnlyMessage");
            return;
        } else if (this.EntityPM.ShipmentTypeId == "FCLD") {
            return;
        }


    }


    ngOnInit() {
        this.BuildItemsList();
    }
    ngOnDestroy(): void {
        if (this.ItemsSource) {
            this.ItemsSource.Clear();
        }
    }

    Add() {
        if (this.IsDisplayOnly)
            return;

        var itemPM = new ShipmentPackagePM(this.EntityPM);
        itemPM.Tenant = this.EntityPM.Tenant;
        itemPM.ShipmentId = this.EntityPM.ShipmentId;
        itemPM.Quantity = 1;
        this.EntityPM.IsCustomShipment = true;
        this.EntityPM.ShipmentPackages.push(itemPM);
        this.ItemsSource.Insert(new ShipmentPackageItemLine(itemPM, this, this.ItemsSource.Length + 1));
    }
    OnSelectedItemChanged(selectedRow: ShipmentPackageItemLine) {
        this.SelectedRow = selectedRow;

    }

    OnRowEnded($event) {
        if (($event) == this.ItemsSource.Length) {
            this.Add();
        }
    }

    BuildItemsList() {
        // Clear existing items if any
        if (this.ItemsSource != null && this.ItemsSource.Length > 0) {
            this.ItemsSource.Clear();
        }

        // Check if there are packages to process
        if (this.EntityPM?.ShipmentPackages != null && this.EntityPM.ShipmentPackages.length > 0) {
            // Loop through each package and create a new item line with index
            this.EntityPM.ShipmentPackages.forEach((item, index) => {
                // Insert the new item into the ObservableCollection with the correct index
                // Assuming the ShipmentPackageItemLine constructor can accept an index
                this.ItemsSource.Insert(new ShipmentPackageItemLine(item, this, index + 1));
            });
        }
    }

    onCellSelected($event, Item: ShipmentPackageItemLine) {
        if (this.SelectedRow != Item) {
            this.OnSelectedItemChanged(Item);
        }
    }


}

export class ShipmentPackageItemLine extends BaseComponent {
    public entityPM: ShipmentPackagePM = null;
    public DataContext = this;
    Parent: ShipmentPackagesTabComponent;
    public index: number;
    constructor(EntityPM: ShipmentPackagePM, parent: ShipmentPackagesTabComponent, index: number) {
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
        this.index = index;
        this.UIProperties.SetRequired("ContainerNumber", "ShipmentPackages");

    }

    ValidateContainerNumber(logCellTemplate: any, containerNumberTextBox: any) {
        var error = FormatTool.ValidateContainerNumber(this.ContainerNumber);
        if (!AppTool.IsNullOrEmpty(error)) {
            this.UIProperties.SetValidity("ContainerNumber", "ShipmentPackages", false, error);
            //SessionLocator.SustainFocusOnCell = true;
            SessionLocator.SelectedSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: containerNumberTextBox.InputId });
        } else {
            this.UIProperties.SetValidity("ContainerNumber", "ShipmentPackages", true, '');
           // SessionLocator.SustainFocusOnCell = false;
        }
    }

    ClassificationKeyUp(event, logCellTemplate: any, containerNumberTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.ValidateContainerNumber(logCellTemplate, containerNumberTextBox);
        }
    }


    public get ContainerNumber() { return this.entityPM.ContainerNumber; }
    public set ContainerNumber(value: string) {
        this.entityPM.ContainerNumber = value;
    }

    public get PackageTypeId() { return this.entityPM.PackageTypeId; }
    public set PackageTypeId(newValue: string) {
        if (this.entityPM.PackageTypeId != newValue) {
            this.entityPM.PackageTypeId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.entityPM.PackageTypeName = null;
            }

            else {

                var packageTypeService = new PackageTypeListService();
                packageTypeService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myPackageTypeList: PackageTypeList = myResponse.Result;
                        if (myPackageTypeList != null) {
                            this.PackageTypeName = myPackageTypeList.LocalName;
                            
                        }
                    }
                });
            }
        }
    }

    public get PackageTypeName() { return this.entityPM.PackageTypeName; }
    public set PackageTypeName(value: string) { this.entityPM.PackageTypeName = value; }



    public get ShipperSeal() { return this.entityPM.ShipperSeal; }
    public set ShipperSeal(value: string) { this.entityPM.ShipperSeal = value; }

    public get Weight() { return this.entityPM.Weight; }
    public set Weight(value: number) { this.entityPM.Weight = value; }
}
