//import {Component, OnInit}  from '@angular/core';
//import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
//import {NewWarehouseEntryComponent} from '../../../Warehouse/Components/NewWarehouseEntryComponent';
//import {WarehouseEntryPackagePM} from '../../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
//import {AppTool, FormatTool} from '../../../Infrastructure/Tools';
//export class WarehouseEntryPackageItem extends BaseComponent {
//    public EntityPM: WarehouseEntryPackagePM;
//    public ObjectTableName: string = "ShipmentPackage";
//    public IsNewEntity: boolean = false;
//    public IsContainer: boolean = false;
//    public Row: any;
//    constructor(entity: WarehouseEntryPackagePM, public fatherComponent: NewWarehouseEntryComponent, isNew: boolean = false) {
//        super();
//        this.EntityPM = entity;
//        this.IsNewEntity = isNew;
//    }
//    public IsEditingEnabled: boolean = false;
//    public IsVolumeEnabled: boolean = false;
//    public IsBuildButtonEnabled: boolean = false;
//    SetUIProperties_Package() {
//        if (!this.IsContainer) {
//            var isVolumeEnabled: boolean = false;
//            var isDimensionEnabled: boolean = false;
//            if (this.IsEditingEnabled) {
//                    isVolumeEnabled = true;
//                    isDimensionEnabled = true;
//                    if (this.Height != null || this.Width != null || this.Length != null) {
//                        isVolumeEnabled = false;
//                    }
//                    else if (this.Volume != null) {
//                        isDimensionEnabled = false;
//                    }
//            }
//            this.IsVolumeEnabled = isVolumeEnabled;
//            this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
//            this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
//            this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
//            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
//        }
//    }
//    // Dimensions
//    get Quantity() { return this.EntityPM.Quantity; }
//    set Quantity(newValue: number) {
//        if (this.EntityPM.Quantity != newValue) {
//            this.EntityPM.Quantity = AppTool.Round(newValue, 0);
//            this.ComputeVolume();
//        }
//    }
//    get Length() { return this.EntityPM.Length; }
//    set Length(newValue: number) {
//        if (this.EntityPM.Length != newValue) {
//            this.EntityPM.Length = AppTool.Round(newValue, 2);
//            this.ComputeVolume();
//        }
//    }
//    get Width() { return this.EntityPM.Width; }
//    set Width(newValue: number) {
//        if (this.EntityPM.Width != newValue) {
//            this.EntityPM.Width = AppTool.Round(newValue, 2);
//            this.ComputeVolume();
//        }
//    }
//    get Height() { return this.EntityPM.Height; }
//    set Height(newValue: number) {
//        if (this.EntityPM.Height != newValue) {
//            this.EntityPM.Height = AppTool.Round(newValue, 2);
//            this.ComputeVolume();
//        }
//    }
//    get Dimensions() {
//        var myDimensions: string;
//        if (this.Length == null && this.Width == null && this.Height == null) {
//            myDimensions = " - - ";
//        }
//        else {
//            var myLength: number = 0;
//            var myWidth: number = 0;
//            var myHeight: number = 0;
//            if (this.Length != null) {
//                myLength = this.Length;
//            }
//            if (this.Width != null) {
//                myWidth = this.Width;
//            }
//            if (this.Height != null) {
//                myHeight = this.Height;
//            }
//            myDimensions = myLength + "-" + myWidth + "-" + myHeight;
//        }
//        return myDimensions;
//    }
//    get Volume() { return this.EntityPM.Volume; }
//    set Volume(newValue: number) {
//        if (this.EntityPM.Volume != newValue) {
//            this.EntityPM.Volume = AppTool.Round(newValue, 3);
//            //this.ComputeVolumetricWeight();
//            //this.SetUIProperties();
//        }
//    }
//    get Weight() { return this.EntityPM.Weight; }
//    set Weight(newValue: number) {
//        var myValue: number = AppTool.Round(newValue, 3);
//        if (this.EntityPM.Weight != myValue) {
//            this.EntityPM.Weight = myValue
//        }
//    }
//    private ComputeVolume() {
//        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
//    }
//    get Harmonize() { return this.EntityPM.Harmonize; }
//    set Harmonize(newValue: string) {
//        if (this.EntityPM.Harmonize != newValue) {
//            this.EntityPM.Harmonize = newValue;
//        }
//    }
//} 
//# sourceMappingURL=WarehouseEntryPackageItem.js.map