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
import {WarehouseEntryPM} from '../../Warehouse/EntityPMs/WarehouseEntryPM';
import {WarehouseEntryPackagePM} from '../../Warehouse/EntityPMs/WarehouseEntryPackagePM';
import {NewWarehouseEntryComponent} from '../../Warehouse/Components/NewWarehouseEntryComponent';
import {PackageTypeList} from '../../Common/EntityLists/PackageTypeList';
import {EventTypeArgs} from '../../Infrastructure/DataContracts/EventTypeArgs';
import {PackageTypeListService} from '../../Common/Services/StandardLists/PackageTypeListService';
import {ClassLevelValidator} from '../../Infrastructure/Validators/ClassLevelValidator';
import {ObservableCollection} from '../../Infrastructure/Utilities/ObservableCollection';
import {WarehouseEntryPackageItem} from '../../Warehouse/Components/AddEditWarehouseEntryPackagesAndContainers';



@Component({
    
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
    IsCancelled: boolean = false;
    ObjectTableName: string = "WarehouseEntryPM";
    VolumeLabel: string;
    GrossWeightLabel: string;
    DimensionsLabel: string;


    HeightLabel: string;
    WidthLabel: string;
    LengthLabel: string;

    VolumetricWeightLabel: string;
    IsNewEntity: boolean = false;
    public ItemsSource: ObservableCollection;
    IsEditMode: boolean = false;
    IsCFSWarehouse: boolean = false;
    IsFromFullWarehouseEntryComponent: boolean = false;

    myPackageTypeService: PackageTypeListService;
    savedItems: WarehouseEntryPackagePM[] = [];

    ShowAddPackageButton: boolean = false;
    DisableAddPackageButton: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.myPackageTypeService = new PackageTypeListService();
        this.ItemsSource = new ObservableCollection([]);

    }

    ngOnInit(

    ) {


    }

    SetWindowArgs(args: any) {
      


        this.IsFromFullWarehouseEntryComponent = args.IsFromFullWarehouseEntryComponent;
        this.IsEditMode = args.IsEditMode;
        this.ShowPackageSummary = args.ShowPackageSummary;
        this.ShowAddPackageButton = args.ShowAddPackageButton;
        this.IsCFSWarehouse = args.IsCFSWarehouse;
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



    BuildItemsSource() {

 

        var itemsCollection: WarehouseEntryPackageItem[] = [];

        this.WarehouseEntryPackagesLists.forEach((item) => {
            itemsCollection.push(new WarehouseEntryPackageItem(item,this));
        })

        this.ItemsSource.InsertCollection(itemsCollection);
        
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

    get ChargeableWeightUnitCode() { return this.warehouseEntryPM.ChargeableWeightUnitCode; }
    set ChargeableWeightUnitCode(newValue: string) {
        if (this.warehouseEntryPM.ChargeableWeightUnitCode != newValue) {
            this.warehouseEntryPM.ChargeableWeightUnitCode = newValue;
            this.ComputeDimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }

   

    get Ratio() { return this.warehouseEntryPM.Ratio; }
    set Ratio(newValue: number) {
        if (this.warehouseEntryPM.Ratio != newValue) {
            this.warehouseEntryPM.Ratio = newValue;
            this.OnWarehouseEntryRatioChanged(this.warehouseEntryPM);
        }
    }




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
        this.VolumeLabel = "Volume (" + this.warehouseEntryPM.VolumeUnitCode + ")";
        this.GrossWeightLabel = "Gross Weight (" + this.warehouseEntryPM.GrossWeightUnitCode + ")";
        this.DimensionsLabel = "Dim(L-W-H) (" + this.warehouseEntryPM.DimensionsUnitCode + ")";
        this.VolumetricWeightLabel = "Volumetric Weight (" + this.warehouseEntryPM.ChargeableWeightUnitCode + ")";
        this.ChargeableWeightUnitCodeLabel = "Chargeable Weight (" + this.warehouseEntryPM.ChargeableWeightUnitCode + ")";
        this.DimensionsUnitLable = " (" + this.warehouseEntryPM.DimensionsUnitCode + ")";
        this.WidthLabel = "W (" + this.DimensionsUnitCode + ")";
        this.HeightLabel = "H (" + this.DimensionsUnitCode + ")";
        this.LengthLabel = "L (" + this.DimensionsUnitCode + ")";


    }

    OnQuantityChange(item: any) {
        this.OnQuantityLostFocus(item.Quantity, item.EntityPM.ShipmentPackageId);
    }

    OnQuantityLostFocus(quantity: any, shipmentPackageId: any) {
        if (this.warehouseEntryPM.DirectionId == "I") {
            this.WarehouseEntryPackagesLists.filter(entryPackage => {
                if (entryPackage.ShipmentPackageId == shipmentPackageId) {
                    if (entryPackage.OldQuantity < quantity)
                        entryPackage.OverManifest = quantity - entryPackage.OldQuantity;
                    else entryPackage.OverManifest = 0;
                    this.CurrentSession.FireEvent({ Name: 'QuantityChanged', DataContext: this.DataContext })
                }
            });
        }
    }

    private ComputeGrossWeigh_Kg_Ton() {

    }

    public RecalculateShipmentFields(warehouseEntryPM: WarehouseEntryPM) {

        if (warehouseEntryPM != null) {

            if (warehouseEntryPM.Ratio == null) {
                warehouseEntryPM.Ratio = AppTool.GetRatio(warehouseEntryPM.DirectionId, warehouseEntryPM.TransportModeId, warehouseEntryPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
            }

            if (warehouseEntryPM.WarehouseEntryPackages.length == 0) {
                warehouseEntryPM.TotalPieces = null;
                warehouseEntryPM.TotalGrossWeight = null;
                warehouseEntryPM.TotalVolume = null;
                warehouseEntryPM.TotalVolumetricWeight = null;
         
            }

            else {

                var myQuantity: number = 0;
                var myVolume: number = 0;
                var myGrossWeight: number = 0;
                var myVolumetricWeight: number = 0;

                warehouseEntryPM.WarehouseEntryPackages.forEach((item) => {

            
                    item.Volume = AppTool.ComputePackageVolume(item.Quantity, item.Width, item.Height, item.Length, item.Weight, warehouseEntryPM.Ratio, warehouseEntryPM.DimensionsUnitCode, warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.GrossWeightUnitCode);

                    item.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(item.Quantity, item.Width, item.Height, item.Length, item.Volume, item.Weight, warehouseEntryPM.Ratio, warehouseEntryPM.DimensionsUnitCode, warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.GrossWeightUnitCode, warehouseEntryPM.ChargeableWeightUnitCode);

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

                warehouseEntryPM.TotalPieces = myQuantity;
                warehouseEntryPM.TotalVolume = AppTool.Round(myVolume, 3);
                warehouseEntryPM.TotalVolumetricWeight = AppTool.Round(myVolumetricWeight, 3);
            }
        }
    }


    public  OnWarehouseEntryRatioChanged(warehouseEntryPM: WarehouseEntryPM) {
        if (warehouseEntryPM) {
            if (warehouseEntryPM.Ratio == null) {
                warehouseEntryPM.Ratio = AppTool.GetRatio(warehouseEntryPM.DirectionId, warehouseEntryPM.TransportModeId, warehouseEntryPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
            }
            // ShipmentPackages
            if (warehouseEntryPM.WarehouseEntryPackages.length == 0) {

                warehouseEntryPM.TotalGrossWeight = null;
                warehouseEntryPM.TotalVolume = null;
                warehouseEntryPM.TotalVolumetricWeight = null;
            }

            else {
                warehouseEntryPM.WarehouseEntryPackages.forEach((item) => {
                    if (item.Volume) {
                        item.VolumetricWeight = AppTool.GetWeightFromVolume(warehouseEntryPM.VolumeUnitCode, warehouseEntryPM.ChargeableWeightUnitCode, item.Volume, warehouseEntryPM.Ratio);
                    }
                });
                warehouseEntryPM.TotalVolumetricWeight = AppTool.Round(ArrayTool.Sum(warehouseEntryPM.WarehouseEntryPackages, "VolumetricWeight"), 3);

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
    get TotalVolumetricWeight() {
        var iResult: number = 0;

        if (this.warehouseEntryPM && this.warehouseEntryPM.TotalVolumetricWeight) {
            iResult = this.warehouseEntryPM.TotalVolumetricWeight;
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
 
    Start(args) {
        this.warehouseEntryPM = args.WarehouseEntryPM;
        this.IsCancelled = this.warehouseEntryPM.StatusCode == "CAEA" ? true : false;
        this.ViewModelTrigger = args.ViewModelTrigger;
        
        if (this.warehouseEntryPM) {

            if (this.warehouseEntryPM.DirectionId == "I" && !AppTool.IsNullOrEmpty(this.warehouseEntryPM.ConnectedTo)) {
                this.DisableAddPackageButton = true;
            }
            if (this.warehouseEntryPM.Ratio == null) {
                var isDirty = this.warehouseEntryPM.IsDirty;
                this.warehouseEntryPM.Ratio = AppTool.GetRatio(this.warehouseEntryPM.DirectionId, this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);

                if (!isDirty) this.warehouseEntryPM.IsDirty = false;
            }



            this.SetAttachedLabels();


            //SetDefultPackage

            if (this.warehouseEntryPM.WarehouseEntryPackages.length == 0 && this.warehouseEntryPM.DirectionId != 'I') {
                var i = 0;
                while (i < 5) {
                    var warehouseEntryPackagePM = new WarehouseEntryPackagePM(null);
                    warehouseEntryPackagePM.Tenant = SessionLocator.TenantPM.Id;
                    warehouseEntryPackagePM.CreatedByUserId = SessionLocator.LoggedUserId;
                    warehouseEntryPackagePM.UpdatedByUserId = SessionLocator.LoggedUserId;
                    warehouseEntryPackagePM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                    warehouseEntryPackagePM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
                    warehouseEntryPackagePM.WarehouseEntryId = this.warehouseEntryPM.Id;
                    warehouseEntryPackagePM.IsContainer = false;
                    this.warehouseEntryPM.WarehouseEntryPackages.push(warehouseEntryPackagePM);
                    i += 1;
                }
            }
          

            this.IsLCLEntity = AppTool.IsLCLEntity(this.warehouseEntryPM.TransportModeId, this.warehouseEntryPM.ShipmentTypeId);
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
                savedItem.ReleasesNumber = item.ReleasesNumber;
                this.savedItems.push(savedItem);
                this.WarehouseEntryPackagesLists.push(item);

            });
        }

  
        this.BuildItemsSource();
        this.ComputeAndFullTotalPackage(true);
    }





    public SelectedRow: WarehouseEntryPackageItem = null;
    OnRowSelected(itemComponent: WarehouseEntryPackageItem) {
        this.SelectedRow = itemComponent;

    }




    AddPackage(isContainer: boolean) {


        var warehouseEntryPackagePM = new WarehouseEntryPackagePM(null)
        warehouseEntryPackagePM.Tenant = SessionLocator.TenantPM.Id;
        warehouseEntryPackagePM.CreatedByUserId = SessionLocator.LoggedUserId;
        warehouseEntryPackagePM.UpdatedByUserId = SessionLocator.LoggedUserId;
        warehouseEntryPackagePM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        warehouseEntryPackagePM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        warehouseEntryPackagePM.WarehouseEntryId = this.warehouseEntryPM.Id;
        warehouseEntryPackagePM.IsContainer = isContainer;
        var warehouseEntryPackageItem: WarehouseEntryPackageItem = new WarehouseEntryPackageItem(warehouseEntryPackagePM, this);

        var title: string = isContainer ? "Add Container" : "Add Package";
        this.ShowAddPackageWindow(warehouseEntryPackageItem, true, title);

    }

    ShowAddPackageWindow(warehouseEntryPackageItem: WarehouseEntryPackageItem,  isNewEntity: boolean, title: string) {

        var windowArgs: any = {};
        windowArgs.IsNewEntity = isNewEntity;
        windowArgs.WarehouseEntryPackageItem = warehouseEntryPackageItem;
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
                this.BuildItemsSource();
            }
        });


    }


    EditPackage(WarehouseEntryPackageItem: WarehouseEntryPackageItem) {

        if (WarehouseEntryPackageItem.Quantity == WarehouseEntryPackageItem.Instock) {
            var title: string = !WarehouseEntryPackageItem.IsContainer ? "Edit Package" : "Edit Container";

            this.ShowAddPackageWindow(WarehouseEntryPackageItem, false, title);
        }
    }


    DeletePackage(item: WarehouseEntryPackageItem) {
        if(item.Quantity== item.Instock) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Delete this package");
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    var index = this.WarehouseEntryPackagesLists.indexOf(item.EntityPM);
                    if (index != -1) this.WarehouseEntryPackagesLists.splice(index, 1);
                    this.ComputeAndFullTotalPackage();
                    this.BuildItemsSource();
                }
            });
        }
    }


    ComputeAndFullTotalPackage(firstTime: boolean = false) {

        var totalPieces: number = 0;
        var totalVolume: number = 0;
        var totalGrossWeight: number = 0;
        var totalVolumetricWeight: number = 0;

        if (this.WarehouseEntryPackagesLists && this.WarehouseEntryPackagesLists.length > 0) {
            this.WarehouseEntryPackagesLists.forEach((item) => {
                if (item.Quantity) totalPieces += item.Quantity;
                if (item.Volume) totalVolume += item.Volume;
                if (item.Weight) totalGrossWeight += item.Weight;
                if (item.VolumetricWeight) totalVolumetricWeight += item.VolumetricWeight;
            });
        }


        this.warehouseEntryPM.WarehouseEntryPackages = this.WarehouseEntryPackagesLists;
        this.warehouseEntryPM.TotalPieces = totalPieces;
        this.warehouseEntryPM.TotalVolume = totalVolume;
        this.warehouseEntryPM.TotalGrossWeight =  totalGrossWeight;
        this.warehouseEntryPM.TotalVolumetricWeight = totalVolumetricWeight;
        if (firstTime && this.warehouseEntryPM.IsDirty) this.warehouseEntryPM.IsDirty = false;
       
    }


    CancelButtonClicked() {

        this.ResetPackage();
        this.CloseButtonClicked();

    }

    ResetPackage() {
        this.warehouseEntryPM.WarehouseEntryPackages = this.savedItems;
    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {

        this.SaveOnWarewarehouseEntryPM();
        this.CurrentSession.CurrentWindow.Close("Refresh");
    }

    SaveOnWarewarehouseEntryPM() {
        if (this.warehouseEntryPM != null) {
            //this.warehouseEntryPM.WarehouseEntryPackages = this.WarehouseEntryPackagesLists.filter(d => d.Quantity>0);

            if (this.WarehouseEntryPackagesLists.length == 0) {
                this.warehouseEntryPM.TotalVolume = 0;
                this.warehouseEntryPM.TotalGrossWeight = 0;
                this.warehouseEntryPM.TotalPieces = 0;
                this.warehouseEntryPM.TotalVolumetricWeight = 0;
            } 

            if (this.warehouseEntryPM.TotalPieces == 0 || !this.warehouseEntryPM.TotalPieces) {
                this.warehouseEntryPM.TotalVolume = 0;
                this.warehouseEntryPM.TotalPieces = 0;
                this.warehouseEntryPM.TotalVolumetricWeight = 0;
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
