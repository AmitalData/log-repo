declare var System: any;
declare var window: any;
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';

import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {WarehouseEntryPM} from '../../Warehouse/EntityPMs/WarehouseEntryPM';
import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
import {NewWarehouseEntryComponent} from '../../Warehouse/Components/NewWarehouseEntryComponent';
import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {PackageTypeListService} from '../../Common/Services/StandardLists/PackageTypeListService';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
@Component({
    moduleId: module.id,
    selector: 'WarehouseEntryPackagesDetailsComponent',
    templateUrl: './WarehouseEntryPackagesDetailsComponent.html',

})

export class WarehouseEntryPackagesDetailsComponent extends BaseComponent implements OnInit {
    public AllPackageTypes: PackageTypeList[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    DataContext: any = this;
    ViewModelTrigger: any;
    IsLoadPage: boolean = false;
    IsLCLEntity: boolean = false;
    ShowPackageSummary: boolean;
    WarehouseEntryPackagesLists: WarehouseEntryPackagePM[] = [];
    SelectedWarehouseEntryPackage: WarehouseEntryPackagePM;
    warehouseEntryPM: WarehouseEntryPM;
    ObjectTableName: string = "WarehouseEntryPM";
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;
    VolumetricWeightLabel: string;


    IsEditMode: boolean = false;
    IsFromFullWarehouseEntryComponent: boolean = false;

    myPackageTypeService: PackageTypeListService;
    savedItems: WarehouseEntryPackagePM[] = [];

    ShowAddPackageButton: boolean = false;


    constructor() {
        super();
        this.myPackageTypeService = new PackageTypeListService();
    }

    ngOnInit(

    ) {


    }

    SetWindowArgs(args: any) {
      


        this.IsFromFullWarehouseEntryComponent = args.IsFromFullWarehouseEntryComponent;
        this.IsEditMode = args.IsEditMode;
        this.ShowPackageSummary = args.ShowPackageSummary;
        this.ShowAddPackageButton = args.ShowAddPackageButton;
        if (!this.IsEditMode) {
            this.ShowAddPackageButton = true;
        }
        
        this.myPackageTypeService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                this.AllPackageTypes = resp.Result;
            }

            this.Start(args);
        });
     

    }


    // Measurments
    MeasurmentsButtonToolTip: string ="Measurement Settings";
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
        if (this.warehouseEntryPM) volumeUnitCode = this.warehouseEntryPM.VolumeUnitCode;
        return volumeUnitCode;

    }
    set VolumeUnitCode(newValue: string) {
        if (this.warehouseEntryPM.VolumeUnitCode != newValue) {
            this.warehouseEntryPM.VolumeUnitCode = newValue;

            this.warehouseEntryPM.DimensionsUnitCode = AppTool.GetDimentionsCodeFromVolumeCode(newValue);

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.SetUIProperties_DimensionsUnitCode();
            this.OnMeasurmentsSettingsChanged();
        }
    }



    get DimensionsUnitCode() {
        var dimensionsUnitCode: string = null;
        if (this.warehouseEntryPM) dimensionsUnitCode = this.warehouseEntryPM.DimensionsUnitCode;
        return dimensionsUnitCode;

    }

    set DimensionsUnitCode(newValue: string) {
        if (this.warehouseEntryPM.DimensionsUnitCode != newValue) {
            this.warehouseEntryPM.DimensionsUnitCode = newValue;

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }



    get GrossWeightUnitCode() {
        var grossWeightUnitCode: string = null;
        if (this.warehouseEntryPM) grossWeightUnitCode = this.warehouseEntryPM.GrossWeightUnitCode;
        return grossWeightUnitCode;

    }
    set GrossWeightUnitCode(newValue: string) {
        if (this.warehouseEntryPM.GrossWeightUnitCode != newValue) {
            this.warehouseEntryPM.GrossWeightUnitCode = newValue;

            this.OnMeasurmentsSettingsChanged();
            this.ComputeGrossWeigh_Kg_Ton();
        }
    }

    //get ChargeableWeightUnitCode() { return this.warehouseEntryPM.ChargeableWeightUnitCode; }
    //set ChargeableWeightUnitCode(newValue: string) {
    //    if (this.warehouseEntryPM.ChargeableWeightUnitCode != newValue) {
    //        this.warehouseEntryPM.ChargeableWeightUnitCode = newValue;

    //        this.ComputeDimFactor();
    //        this.OnMeasurmentsSettingsChanged();
    //    }
    //}

    //get Ratio() { return this.warehouseEntryPM.Ratio; }
    //set Ratio(newValue: number) {
    //    if (this.warehouseEntryPM.Ratio != newValue) {
    //        this.warehouseEntryPM.Ratio = newValue;

    //        this.ComputeDimFactor();
    //        ShipmentTool.OnShipmentRatioChanged(this.EntityPM);
    //    }
    //}


    ComputeDimFactor() {
      //  this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    }

    OnMeasurmentsSettingsChanged() {
        this.SetAttachedLabels();
        this.RecalculateShipmentFields(this.warehouseEntryPM);
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

    ChargeableWeightLabel: string = null;
    SetAttachedLabels() {
        this.VolumeLabel = "Volume (" + this.warehouseEntryPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + this.warehouseEntryPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + this.warehouseEntryPM.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.warehouseEntryPM.ChargeableWeightUnitCode + ")";

    }


    private ComputeGrossWeigh_Kg_Ton() {
        //var weigh_Kg: number = null;
        //var weigh_Ton: number = null;

        //if (this.GrossWeight != null) {
        //    var factorOfConvert: number = 1;

        //    if (!AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
        //        switch (this.GrossWeightUnitCode.toUpperCase()) {
        //            case "KG": { factorOfConvert = 1; break; }
        //            case "LB": { factorOfConvert = 0.45359237; break; }
        //            case "MT": { factorOfConvert = 1000; break; }
        //        }
        //    }

        //    weigh_Kg = this.GrossWeight * factorOfConvert;
        //}

        //if (weigh_Kg != null) {
        //    weigh_Kg = AppTool.Round(weigh_Kg, 3);

        //    weigh_Ton = weigh_Kg / 1000;
        //}

        //if (weigh_Ton != null) {
        //    weigh_Ton = AppTool.Round(weigh_Ton, 3);
        //}

        //this.EntityPM.GrossWeightInKG = weigh_Kg;
        //this.EntityPM.GrossWeightPerTon = weigh_Ton;
    }

    public  RecalculateShipmentFields(warehouseEntryPM: WarehouseEntryPM) {
      
        if (warehouseEntryPM != null) {

            //if (warehouseEntryPM.Ratio == null) {
            //    warehouseEntryPM.Ratio = AppTool.GetRatio(warehouseEntryPM.DirectionId, warehouseEntryPM.TransportModeId, warehouseEntryPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
            //}

            if (warehouseEntryPM.WarehouseEntryPackages.length == 0) {
                warehouseEntryPM.TotalPieces = null;
                warehouseEntryPM.TotalGrossWeight = null;
                warehouseEntryPM.TotalVolume = null;
                //warehouseEntryPM.VolumetricWeight = null;
               // warehouseEntryPM.ChargeableWeight = null;
               // warehouseEntryPM.AWBCommodityItemNumber = null;
               // warehouseEntryPM.GrossWeightEdited = false;
               // warehouseEntryPM.ChargeableWeightEdited = false;
            }

            else {

                var myQuantity: number = 0;
                var myVolume: number = 0;
                var myGrossWeight: number = 0;
                var myVolumetricWeight: number = 0;

                warehouseEntryPM.WarehouseEntryPackages.forEach((item) => {

                    //item.Volume = AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.Weight, warehouseEntryPM.Ratio, warehouseEntryPM.DimensionsUnitCode, warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.GrossWeightUnitCode);

                    item.Volume = AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.Weight, null, warehouseEntryPM.DimensionsUnitCode, warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.GrossWeightUnitCode);



                        //item.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(item.Quantity, item.Width, item.Height, item.Length, item.Volume, item.Weight, warehouseEntryPM.Ratio, warehouseEntryPM.DimensionsUnitCode, warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.GrossWeightUnitCode, warehouseEntryPM.ChargeableWeightUnitCode);



                    if (item.Quantity != null) {
                        myQuantity += item.Quantity;
                    }

                    if (item.Volume != null) {
                        myVolume += item.Volume;
                    }

                    //if (item.VolumetricWeight != null) {
                    //    myVolumetricWeight += item.VolumetricWeight;
                    //}

                    if (item.Weight != null) {
                        myGrossWeight += item.Weight;
                    }
                })

                warehouseEntryPM.TotalPieces = myQuantity;
                warehouseEntryPM.TotalVolume = AppTool.Round(myVolume, 3);
              //  warehouseEntryPM.VolumetricWeight = AppTool.Round(myVolumetricWeight, 3);

               
            }

          
        }
    }




   
    get TotalVolume() {
        var totalVolume: number = 0;
        if (this.warehouseEntryPM && this.warehouseEntryPM.TotalVolume) totalVolume = this.warehouseEntryPM.TotalVolume;
        return totalVolume;

    }
    

    get TotalGrossWeight() {
        var totalGrossWeight: number = 0;
        if (this.warehouseEntryPM && this.warehouseEntryPM.TotalGrossWeight) totalGrossWeight = this.warehouseEntryPM.TotalGrossWeight;
        return totalGrossWeight;

    }

    get TotalPieces() {
        var totalPieces: number = 0;
        if (this.warehouseEntryPM && this.warehouseEntryPM.TotalPieces) totalPieces = this.warehouseEntryPM.TotalPieces;
        return totalPieces;

    }
   
    get QuantityLabel() {
        var quantityLabel: string = "";
        if (this.IsLCLEntity) {
            quantityLabel = "Number of Packages";
        } else quantityLabel = "Number of Containers";

        return quantityLabel;

    }
 
    Start(args) {
        this.warehouseEntryPM = args.WarehouseEntryPM;
        this.ViewModelTrigger = args.ViewModelTrigger;
        
        if (this.warehouseEntryPM) {

            this.VolumeLabel = "Volume (" + this.warehouseEntryPM.VolumeUnitCode + ")";
            this.GrossWeightLabel = "Gross Weight (" + this.warehouseEntryPM.GrossWeightUnitCode + ")";
            this.DimensionsLabel = "Dim(L-W-H) (" + this.warehouseEntryPM.DimensionsUnitCode + ")";

            this.SetAttachedLabels();



            this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
            //this.TotalPieces = this.warehouseEntryPM.TotalPieces ? this.warehouseEntryPM.TotalPieces : 0;
            //this.TotalGrossWeight = this.warehouseEntryPM.TotalGrossWeight ? this.warehouseEntryPM.TotalGrossWeight : 0;
            //this.TotalVolume = this.warehouseEntryPM.TotalVolume ? this.warehouseEntryPM.TotalVolume : 0;
            this.warehouseEntryPM.WarehouseEntryPackages.forEach((item) => {
                var savedItem = new WarehouseEntryPackagePM(null);
                savedItem.PackageTypeId = item.PackageTypeId;
                savedItem.ContainerNumber = item.ContainerNumber;
                savedItem.Length = item.Length;
                savedItem.Height = item.Height;
                savedItem.Width = item.Width;
                savedItem.Volume = item.Volume;
                savedItem.Weight = item.Weight;
                savedItem.Description = item.Description;
                savedItem.Seal = item.Seal;
                savedItem.Harmonize = item.Harmonize;
                savedItem.Location = item.Location;
                savedItem.Dimensions = item.Dimensions;
                savedItem.Instock = item.Instock;
                savedItem.Quantity = item.Quantity;
                savedItem.ContainerNumberWarning = item.ContainerNumberWarning;
                savedItem.Id = item.Id;
                savedItem.CreateDate = item.CreateDate;
                savedItem.UpdateDate = item.UpdateDate;
                savedItem.CreatedByUserId = item.CreatedByUserId;
                savedItem.UpdatedByUserId = item.UpdatedByUserId;
                savedItem.WarehouseEntryId = item.WarehouseEntryId;
                savedItem.PackageTypeName = item.PackageTypeName;
                savedItem.IsContainer = item.IsContainer;
                savedItem.Make = item.Make;
                savedItem.Year = item.Year;
                savedItem.ChassisNumber = item.ChassisNumber;
                savedItem.RegistrationNumber = item.RegistrationNumber;
                savedItem.CountryId = item.CountryId;
                savedItem.Model = item.Model;
                savedItem.Color = item.Color;

                this.savedItems.push(savedItem);
                this.WarehouseEntryPackagesLists.push(item);

            });
        }

        //if (!this.IsFromFullWarehouseEntryComponent) {
        //    this.ComputeAndFullTotalPackage();
        //}


        this.ComputeAndFullTotalPackage();
    }
  

    AddPackage(isContainer: boolean) {

        var newWarehouseEntryPackagePM: WarehouseEntryPackagePM = new WarehouseEntryPackagePM(null);
        newWarehouseEntryPackagePM.Tenant = SessionLocator.TenantPM.Id;
        newWarehouseEntryPackagePM.CreatedByUserId = SessionLocator.LoggedUserId;
        newWarehouseEntryPackagePM.UpdatedByUserId = SessionLocator.LoggedUserId;
        newWarehouseEntryPackagePM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        newWarehouseEntryPackagePM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();

        newWarehouseEntryPackagePM.WarehouseEntryId = this.warehouseEntryPM.Id;
        newWarehouseEntryPackagePM.Id = "1-1";
        newWarehouseEntryPackagePM.IsContainer = isContainer;

        var title: string = isContainer ? "Add Container" : "Add Package";
        this.ShowAddPackageWindow(newWarehouseEntryPackagePM, true, title);

    }

    ShowAddPackageWindow(warehouseEntryPackagePM: WarehouseEntryPackagePM,  isNewEntity: boolean, title: string) {

        var windowArgs: any = {};
        windowArgs.IsNewEntity = isNewEntity;
        windowArgs.WarehouseEntryPackagePM = warehouseEntryPackagePM;
        windowArgs.WarehouseEntryPM = this.warehouseEntryPM;
        windowArgs.ViewModelTrigger = this;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 830;
        logWindow.Height = 550;
        logWindow.Title = title;
        windowArgs.AllPackageTypes = this.AllPackageTypes;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/AddEditWarehouseEntryPackagesAndContainers");
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.ComputeAndFullTotalPackage();
            }
        });


    }


    EditPackage(warehouseEntryPackagePM: WarehouseEntryPackagePM) {

        if (warehouseEntryPackagePM.Quantity == warehouseEntryPackagePM.Instock) {
            var title: string = !warehouseEntryPackagePM.IsContainer ? "Edit Package" : "Edit Container";

            this.ShowAddPackageWindow(warehouseEntryPackagePM, false, title);
        }
    }


    DeletePackage(item: WarehouseEntryPackagePM) {
        if(item.Quantity== item.Instock) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Delete this package");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    var index = this.WarehouseEntryPackagesLists.indexOf(item);
                    if (index != -1) this.WarehouseEntryPackagesLists.splice(index, 1);
                    this.ComputeAndFullTotalPackage();
                }
            });
        }
    }


    ComputeAndFullTotalPackage() {

        var totalPieces: number = 0;
        var totalVolume: number = 0;
        var totalGrossWeight: number = 0;

        if (this.WarehouseEntryPackagesLists && this.WarehouseEntryPackagesLists.length > 0) {
            this.WarehouseEntryPackagesLists.forEach((item) => {
                if (item.Quantity) totalPieces += item.Quantity;
                if (item.Volume) totalVolume += item.Volume;
                if (item.Weight) totalGrossWeight += item.Weight;
            });
        }

        this.warehouseEntryPM.WarehouseEntryPackages = this.WarehouseEntryPackagesLists;
        this.warehouseEntryPM.TotalPieces = totalPieces;
        this.warehouseEntryPM.TotalVolume = totalVolume;
        this.warehouseEntryPM.TotalGrossWeight = totalGrossWeight;

       
    }


    CancelButtonClicked() {

        this.ResetPackage();
        this.CloseButtonClicked();

    }

    ResetPackage() {
        this.warehouseEntryPM.WarehouseEntryPackages = this.savedItems;
    }

    CloseButtonClicked() {

        SessionLocator.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {

        this.SaveOnWarewarehouseEntryPM();
        SessionLocator.CurrentSession.CurrentWindow.Close("Refresh");
    }

    SaveOnWarewarehouseEntryPM() {
        if (this.warehouseEntryPM != null) {
            this.warehouseEntryPM.WarehouseEntryPackages = this.WarehouseEntryPackagesLists;

            if (this.WarehouseEntryPackagesLists.length == 0) {
                this.warehouseEntryPM.TotalVolume = 0;
                this.warehouseEntryPM.TotalGrossWeight = 0;
                this.warehouseEntryPM.TotalPieces = 0;
            } 

            if (this.warehouseEntryPM.TotalPieces == 0 || !this.warehouseEntryPM.TotalPieces) {
                this.warehouseEntryPM.TotalVolume = 0;
                this.warehouseEntryPM.TotalPieces = 0;
            }


            if (this.ViewModelTrigger != null) {
              //  this.ViewModelTrigger.SetPackagesDetailsEnable();
            }
        }
    }

    ReloadComponent(args:any) {

        this.WarehouseEntryPackagesLists = [];
        this.SelectedWarehouseEntryPackage = null;

        this.Start(args);
        
    }


}
