import {  Component, EventEmitter, OnDestroy, OnInit } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { ObservableCollection } from "Infrastructure/Utilities/ObservableCollection";
import { ShipmentPackagePM } from "Shipment/EntityPMs/ShipmentPackagePM";

@Component({    
    templateUrl: './ShipmentPackagesTabComponent.html',
})

export class ShipmentPackagesTabComponent extends BaseComponent implements OnInit,OnDestroy {
    public IsReadOnly: boolean = false;
    public ItemsSource: ObservableCollection;
    public IsDisplayOnly: boolean = false;
    SelectedRow: ShipmentPackageItemLine;
    ChangeScrollPosition: EventEmitter<any> = new EventEmitter();
    public ObjectTableName = "ShipmentPackages";

    
    constructor(public entityArgs: EntityArgs) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.EntityPM = this.entityArgs.EntityPM;
    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.Add();
        }
    }


    ngOnInit() {
        this.BuildItemsList();
    }
    ngOnDestroy(): void {
        if(this.ItemsSource){
            this.ItemsSource.Clear();
        }
    }

    Add(){
        if (this.IsDisplayOnly)
            return;
    }
    OnSelectedItemChanged(selectedRow: ShipmentPackageItemLine) {
        this.SelectedRow = selectedRow;

    }

    OnRowEnded($event){
        if(($event) == this.ItemsSource.Length){
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
                this.ItemsSource.Insert(new ShipmentPackageItemLine(item, this, index+1));
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
    public index:number;
    constructor(EntityPM: ShipmentPackagePM, parent: ShipmentPackagesTabComponent,index:number){
        super();
        this.entityPM = EntityPM;
        this.Parent = parent;
        this.index = index; 
    }


    public get ContainerNumber() { return this.entityPM.ContainerNumber; }
    public set ContainerNumber(value:string) { this.entityPM.ContainerNumber = value; }

    public get PackageTypeId() { return this.entityPM.PackageTypeId; }
    public set PackageTypeId(value:string) { this.entityPM.PackageTypeId = value; }

    public get ShipperSeal() { return this.entityPM.ShipperSeal; }
    public set ShipperSeal(value:string) { this.entityPM.ShipperSeal = value; }

    public get Weight() { return this.entityPM.Weight; }
    public set Weight(value:number) { this.entityPM.Weight = value; }
}
