declare var System: any;
declare var window: any;
import {AppTool, DateTool, ArrayTool} from '../../Infrastructure/Tools';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';

import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {WarehouseReleasePM} from '../../Warehouse/EntityPMs/WarehouseReleasePM';
import {WarehouseReleasePackagePM} from '../../Warehouse/EntityPMs/WarehouseReleasePackagePM';

import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';



import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {PackageTypeListService} from '../../Common/Services/StandardLists/PackageTypeListService';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {ObservableCollection} from '../../Infrastructure/Utilities/ObservableCollection';
import {WarehouseEntryPackageItem} from '../../Warehouse/Components/AddEditWarehouseEntryPackagesAndContainers';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {WarehouseEntryPackagePMExtendedService} from '../../Warehouse/Services/ExtendedPMs/WarehouseEntryPackagePMExtendedService';

@Component({
    moduleId: module.id,
    selector: 'WarehouseReleasePackagesDetailsComponent',
    templateUrl: './WarehouseReleasePackagesDetailsComponent.html',
    providers: [WarehouseEntryPackagePMExtendedService],
})

export class WarehouseReleasePackagesDetailsComponent extends BaseComponent implements OnInit {
    public AllPackageTypes: PackageTypeList[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    DataContext: any = this;
    ViewModelTrigger: any;
    IsLoadPage: boolean = false;
    IsLCLEntity: boolean = false;
    ShowPackageSummary: boolean;
    WarehouseReleasePackagesLists: WarehouseReleasePackagePM[] = [];
    warehouseReleasePM: WarehouseReleasePM;
    ObjectTableName: string = "WarehouseRelease";
    VolumetricWeightLabel: string;
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    VolumeColumnHeader: string;
    WeightColumnHeader: string;
    DimensionsColumnHeader: string;
    VolumetricWeightColumnHeader: string;
    PackageTypeColumnHeader: string;

    ShowAddPackageButton: boolean = false;
    SelectedWarehouseReleasePackage: WarehouseReleasePackagePM;
    private CurrentSession = SessionLocator.SelectedSession;
    IsEditMode: boolean = false;
    IsFromFullWarehouseReleaseComponent: boolean = false;
    constructor(private warehouseEntryPackagePMExtendedService: WarehouseEntryPackagePMExtendedService) {
        super();


    }

    ngOnInit() { }

    SetWindowArgs(args: any) {


        this.Start(args);
        //this.IsFromFullWarehouseEntryComponent = args.IsFromFullWarehouseEntryComponent;
        //this.IsEditMode = args.IsEditMode;
        //this.ShowPackageSummary = args.ShowPackageSummary;
        //this.ShowAddPackageButton = args.ShowAddPackageButton;
        //if (!this.IsEditMode) {
        //    this.ShowAddPackageButton = true;
        //}

        //this.myPackageTypeService.getAllFromCache().subscribe((resp: any) => {
        //    if (!resp.HasError) {
        //        this.AllPackageTypes = resp.Result;
        //    }


        //});


    }

    ShipmentPM: any;
    SetValue() {

        this.TransportModeId = this.warehouseReleasePM.TransportModeId;
        this.DirectionId = this.warehouseReleasePM.DirectionId;
        if (this.ShipmentPM) {
            this.FromPortId = this.ShipmentPM ? this.ShipmentPM.MainCarriageFromPortId ? this.ShipmentPM.MainCarriageFromPortId : this.ShipmentPM.FromPortId : "";
            this.ToPortId = this.ShipmentPM.ShipmentLevelCode == "H" ? this.ShipmentPM.MainCarriageFinalDestinationPortId : this.ShipmentPM.FinalDistenationPortId;
            if (!this.ToPortId) {
                this.ToPortId = this.ShipmentPM.ToPortId;
            }
        }
        this.ConnectedTo = this.warehouseReleasePM.ConnectedTo;
        this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseReleasePM.TransportModeId, this.warehouseReleasePM.ShipmentTypeId);
    }


    SetLabel() {

        this.VolumeLabel = "Volume (" + SessionLocator.TenantPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + SessionLocator.TenantPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + SessionLocator.TenantPM.DimensionsUnitCode + ")";
        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.warehouseReleasePM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.warehouseReleasePM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.warehouseReleasePM.ChargeableWeightUnitCode);
        this.PackageTypeColumnHeader = TextCodeTranslator.Translate("ShipmentPackage.F.PackageTypeId");
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.warehouseReleasePM.ChargeableWeightUnitCode + ")";
    }

    Start(args) {

        if (args.IsFromFullWarehouseReleaseComponent) {
            this.warehouseReleasePM = args.warehouseReleasePM;
        }
        else {
            this.warehouseReleasePM = args.WarehouseEntryPM;
        }

        this.ViewModelTrigger = args.ViewModelTrigger;
        this.IsEditMode = args.IsEditMode;
        this.IsFromFullWarehouseReleaseComponent = args.IsFromFullWarehouseReleaseComponent;
        this.ShipmentPM = args.ShipmentPM;
        this.ShowPackageSummary = args.ShowPackageSummary;

        if (this.warehouseReleasePM) {
            this.WarehouseReleasePackagesLists = this.warehouseReleasePM.WarehouseReleasePackages;
        }
        this.SetLabel();

        this.SetValue();


    }


    IsChoosePackageOpen: boolean = false;
    WarehouseId: string;
    IsPackageOpen: boolean = false;
    AllWarehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];

    ChoosePackage(packageType: string) {
        this.IsChoosePackageOpen = true;

        if (!this.IsPackageOpen) {
            this.IsPackageOpen = true;
            this.IsChoosePackageOpen = true;
            if (this.CustomerId != this.warehouseReleasePM.CustomerId || this.WarehouseId != this.warehouseReleasePM.WarehouseId) {

                var shipmentId = this.IsFromFullWarehouseReleaseComponent ? this.warehouseReleasePM.ShipmentId : null;
                this.AllWarehouseEntryPackagesLists = [];
                this.warehouseEntryPackagePMExtendedService.GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(shipmentId, this.warehouseReleasePM.CustomerId, this.warehouseReleasePM.WarehouseId, this.warehouseReleasePM.Tenant).subscribe((res: any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        this.AllWarehouseEntryPackagesLists = pmResponse.Result;
                        this.OpenChoosePackage(packageType);
                    }

                });

            } else {
                this.OpenChoosePackage(packageType);
            }
        }

    }


    DeletePackage(item: WarehouseReleasePackagePM) {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this package");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var index = this.WarehouseReleasePackagesLists.indexOf(item);
                if (index != -1) this.WarehouseReleasePackagesLists.splice(index, 1);

                var entry = this.AllWarehouseEntryPackagesLists.filter(d => d.Id == item.EntryPackageId)[0];
                if (entry) {
                    entry.ReleaseQTY = 0;
                    entry.IsSelected = false;
                }

                if (this.WarehouseReleasePackagesLists.length == 0) {
                    this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", true);
                    this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", true);
                }
                this.warehouseReleasePM.WarehouseReleasePackages = this.WarehouseReleasePackagesLists;

            }
        });



    }





    TransportModeId: string;
    DirectionId: string;
    CustomerId: string;
    FromPortId: string;
    ToPortId: string;
    ConnectedTo: string;


    OpenChoosePackage(packageType: string) {

        this.WarehouseId = this.warehouseReleasePM.WarehouseId;
        this.CustomerId = this.warehouseReleasePM.CustomerId;
        this.IsPackageOpen = false;
        var windowArgs: any = {};
        windowArgs.WarehouseReleasePM = this.warehouseReleasePM;

        windowArgs.WarehouseEntryPackagesLists = this.AllWarehouseEntryPackagesLists;
        windowArgs.ViewModelTrigger = this;
        windowArgs.PackageType = packageType;

        var logWindow = new LogitudeWindow();
        if (this.IsFromFullWarehouseReleaseComponent) {
            windowArgs.IsFromFullWarehouseReleaseComponent = this.IsFromFullWarehouseReleaseComponent;
        }
        logWindow.Width = 1150;
        logWindow.Height = 550;
        logWindow.Title = "Choose Packages";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/WarehouseReleaseChoosePackagesComponent");


        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {
                if (this.WarehouseReleasePackagesLists.length > 0 && this.IsChoosePackageOpen) {
                    this.IsChoosePackageOpen = false;
                    this.warehouseReleasePM.UIProperties.SetEnabled("CustomerId", "WarehouseRelease", false);
                    this.warehouseReleasePM.UIProperties.SetEnabled("WarehouseId", "WarehouseRelease", false);
                }

                this.warehouseReleasePM.WarehouseReleasePackages = this.WarehouseReleasePackagesLists;
                this.ViewModelTrigger.IsRefreshCustomer = !this.ViewModelTrigger.IsRefreshCustomer;
                this.ComputeAndFullTotalPackage(true);
            }
        });


    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    MeasurmentsButtonToolTip: string = "Measurement Settings";
    IsMeasurmentsHidden: boolean = true;
    MeasurmentsSettingsClicked() {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;

        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = "Hide Measurement Settings";
        }

        else {
            this.MeasurmentsButtonToolTip = "Measurement Settings";
        }
    }


    get VolumeUnitCode() {
        var volumeUnitCode: string = null;
        if (this.warehouseReleasePM) volumeUnitCode = this.warehouseReleasePM.VolumeUnitCode;
        return volumeUnitCode;

    }
    set VolumeUnitCode(newValue: string) {
        if (this.warehouseReleasePM.VolumeUnitCode != newValue) {
            this.warehouseReleasePM.VolumeUnitCode = newValue;

            this.warehouseReleasePM.DimensionsUnitCode = AppTool.GetDimentionsCodeFromVolumeCode(newValue);

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.SetUIProperties_DimensionsUnitCode();
            this.OnMeasurmentsSettingsChanged();
        }
    }



    get DimensionsUnitCode() {
        var dimensionsUnitCode: string = null;
        if (this.warehouseReleasePM) dimensionsUnitCode = this.warehouseReleasePM.DimensionsUnitCode;
        return dimensionsUnitCode;

    }

    set DimensionsUnitCode(newValue: string) {
        if (this.warehouseReleasePM.DimensionsUnitCode != newValue) {
            this.warehouseReleasePM.DimensionsUnitCode = newValue;

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }



    get GrossWeightUnitCode() {
        var grossWeightUnitCode: string = null;
        if (this.warehouseReleasePM) grossWeightUnitCode = this.warehouseReleasePM.GrossWeightUnitCode;
        return grossWeightUnitCode;

    }
    set GrossWeightUnitCode(newValue: string) {
        if (this.warehouseReleasePM.GrossWeightUnitCode != newValue) {
            this.warehouseReleasePM.GrossWeightUnitCode = newValue;

            this.OnMeasurmentsSettingsChanged();
            this.ComputeGrossWeigh_Kg_Ton();
        }
    }

    get ChargeableWeightUnitCode() { return this.warehouseReleasePM.ChargeableWeightUnitCode; }
    set ChargeableWeightUnitCode(newValue: string) {
        if (this.warehouseReleasePM.ChargeableWeightUnitCode != newValue) {
            this.warehouseReleasePM.ChargeableWeightUnitCode = newValue;
            this.ComputeDimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }



    get Ratio() { return this.warehouseReleasePM.Ratio; }
    set Ratio(newValue: number) {
        if (this.warehouseReleasePM.Ratio != newValue) {
            this.warehouseReleasePM.Ratio = newValue;
            this.OnWarehouseEntryRatioChanged(this.warehouseReleasePM);
        }
    }




    ComputeDimFactor() {
        //  this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    }

    OnMeasurmentsSettingsChanged() {
        this.SetAttachedLabels();
        this.RecalculateShipmentFields(this.warehouseReleasePM);
    }
    SetUIProperties_DimFactor() {
        var isDimFactorVisibile: boolean = false;

        if (!AppTool.IsNullOrEmpty(this.DimensionsUnitCode)) {
            if (this.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }

        //this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    }
    ChargeableWeightUnitCodeLabel: string;
    public DimensionsDependencyProperty1: string = null;
    public DimensionsDependencyProperty1IsList: boolean = false;
    private SetUIProperties_DimensionsUnitCode() {
        var isFieldEnabled: boolean = false;

        if (this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }

        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }

        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }

        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    }


    DimensionsUnitLable: string;
    ChargeableWeightLabel: string = null;
    SetAttachedLabels() {
        this.VolumeLabel = "Volume (" + this.warehouseReleasePM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + this.warehouseReleasePM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + this.warehouseReleasePM.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.warehouseReleasePM.ChargeableWeightUnitCode + ")";
        this.ChargeableWeightUnitCodeLabel = "Chargeable Weight (" + this.warehouseReleasePM.ChargeableWeightUnitCode + ")";
        this.DimensionsUnitLable = " (" + this.warehouseReleasePM.DimensionsUnitCode + ")";

        // this.WidthLabel = "Width (" + this.DimensionsUnitCode + ")";
        //this.HeightLabel = "Height (" + this.DimensionsUnitCode + ")";
        // this.LengthLabel = "Length (" + this.DimensionsUnitCode + ")";


    }


    private ComputeGrossWeigh_Kg_Ton() {

    }

    public RecalculateShipmentFields(warehouseReleasePM: WarehouseReleasePM) {

        if (warehouseReleasePM != null) {

            if (warehouseReleasePM.Ratio == null) {
                warehouseReleasePM.Ratio = AppTool.GetRatio(warehouseReleasePM.DirectionId, warehouseReleasePM.TransportModeId, warehouseReleasePM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
            }

            if (warehouseReleasePM.WarehouseReleasePackages.length == 0) {
                warehouseReleasePM.TotalPieces = null;
                warehouseReleasePM.TotalGrossWeight = null;
                warehouseReleasePM.TotalVolume = null;
                warehouseReleasePM.TotalVolumetricWeight = null;

            }

            else {

                var myQuantity: number = 0;
                var myVolume: number = 0;
                var myGrossWeight: number = 0;
                var myVolumetricWeight: number = 0;

                warehouseReleasePM.WarehouseReleasePackages.forEach((item) => {


                    item.Volume = AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.Weight, warehouseReleasePM.Ratio, warehouseReleasePM.DimensionsUnitCode, warehouseReleasePM.VolumeUnitCode, warehouseReleasePM.GrossWeightUnitCode);

                    item.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(item.Quantity, item.Width, item.Height, item.Length, item.Volume, item.Weight, warehouseReleasePM.Ratio, warehouseReleasePM.DimensionsUnitCode, warehouseReleasePM.VolumeUnitCode, warehouseReleasePM.GrossWeightUnitCode, warehouseReleasePM.ChargeableWeightUnitCode);

                    if (item.Quantity != null) {
                        myQuantity += item.Quantity;
                    }

                    if (item.Volume != null) {
                        myVolume += item.Volume;
                    }

                    if (item.VolumetricWeight != null) {
                        myVolumetricWeight += item.VolumetricWeight;
                    }

                    if (item.Weight != null) {
                        myGrossWeight += item.Weight;
                    }
                })

                warehouseReleasePM.TotalPieces = myQuantity;
                warehouseReleasePM.TotalVolume = AppTool.Round(myVolume, 3);
                warehouseReleasePM.TotalVolumetricWeight = AppTool.Round(myVolumetricWeight, 3);
            }
        }
    }


    public OnWarehouseEntryRatioChanged(warehouseReleasePM: WarehouseReleasePM) {
        if (warehouseReleasePM) {
            if (warehouseReleasePM.Ratio == null) {
                warehouseReleasePM.Ratio = AppTool.GetRatio(warehouseReleasePM.DirectionId, warehouseReleasePM.TransportModeId, warehouseReleasePM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
            }
            // ShipmentPackages
            if (warehouseReleasePM.WarehouseReleasePackages.length == 0) {

                warehouseReleasePM.TotalGrossWeight = null;
                warehouseReleasePM.TotalVolume = null;
                warehouseReleasePM.TotalVolumetricWeight = null;
            }

            else {
                warehouseReleasePM.WarehouseReleasePackages.forEach((item) => {
                    if (item.Volume) {
                        item.VolumetricWeight = AppTool.GetWeightFromVolume(warehouseReleasePM.VolumeUnitCode, warehouseReleasePM.ChargeableWeightUnitCode, item.Volume, warehouseReleasePM.Ratio);
                    }
                });
                warehouseReleasePM.TotalVolumetricWeight = AppTool.Round(ArrayTool.Sum(warehouseReleasePM.WarehouseReleasePackages, "VolumetricWeight"), 3);

            }

        }
    }


    get TotalVolume() {
        var totalVolume: number = 0;
        if (this.warehouseReleasePM && this.warehouseReleasePM.TotalVolume) totalVolume = this.warehouseReleasePM.TotalVolume;
        return totalVolume;

    }
    get TotalGrossWeight() {
        var totalGrossWeight: number = 0;
        if (this.warehouseReleasePM && this.warehouseReleasePM.TotalGrossWeight)
            totalGrossWeight = this.warehouseReleasePM.TotalGrossWeight;
        return totalGrossWeight;

    }
    get TotalPieces() {
        var totalPieces: number = 0;
        if (this.warehouseReleasePM && this.warehouseReleasePM.TotalPieces) totalPieces = this.warehouseReleasePM.TotalPieces;
        return totalPieces;

    }
    get TotalVolumetricWeight() {
        var iResult: number = 0;

        if (this.warehouseReleasePM && this.warehouseReleasePM.TotalVolumetricWeight) {
            iResult = this.warehouseReleasePM.TotalVolumetricWeight;
        }

        return iResult;
    }
    get QuantityLabel() {
        var quantityLabel: string = "";
        if (this.IsLCLEntity) {
            quantityLabel = "Number of Packages";
        } else quantityLabel = "Number of Containers";

        return quantityLabel;

    }



    ComputeAndFullTotalPackage(firstTime: boolean = false) {

        var totalPieces: number = 0;
        var totalVolume: number = 0;
        var totalGrossWeight: number = 0;
        var totalVolumetricWeight: number = 0;

        if (this.WarehouseReleasePackagesLists && this.WarehouseReleasePackagesLists.length > 0) {
            this.WarehouseReleasePackagesLists.forEach((item) => {
                if (item.Quantity) totalPieces += item.Quantity;
                if (item.Volume) totalVolume += item.Volume;
                if (item.Weight) totalGrossWeight += item.Weight;
                if (item.VolumetricWeight) totalVolumetricWeight += item.VolumetricWeight;
            });
        }


        this.warehouseReleasePM.WarehouseReleasePackages = this.WarehouseReleasePackagesLists;
        this.warehouseReleasePM.TotalPieces = totalPieces;
        this.warehouseReleasePM.TotalVolume = totalVolume;
        this.warehouseReleasePM.TotalGrossWeight = totalGrossWeight;
        this.warehouseReleasePM.TotalVolumetricWeight = totalVolumetricWeight;
        if (firstTime && this.warehouseReleasePM.IsDirty) this.warehouseReleasePM.IsDirty = false;

    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////








    CancelButtonClicked() {


        this.CloseButtonClicked();

    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {


        this.CurrentSession.CurrentWindow.Close("Refresh");
    }




}
