import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {QuoteOPPM} from '../../EntityPMs/QuoteOPPM';
import {QuoteOPPackagePM} from '../../EntityPMs/QuoteOPPackagePM';
import {PackageTypeList} from '../../../Common/EntityLists/PackageTypeList';
import {PackageTypeListService} from '../../../Common/Services/StandardLists/PackageTypeListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

@Component({
    
    templateUrl: './QuoteDimensionsComponent.html',
})

export class QuoteDimensionsComponent {
    public EntityPM: QuoteOPPM;
    public DataContext: QuoteDimensionsComponent = this;
    public ObjectTableName: string = "Quote";
    public ItemsSource: DimensionsPackageItem[] = [];
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public IsPackageTypeVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {  
             
    }

    SetWindowArgs(entityPM: QuoteOPPM) {
        this.EntityPM = entityPM;
        this.IsPackageTypeVisible = this.EntityPM.TransportModeId == "A" ? false : true;

        this.entityResourceService.getEntityResourceByTableName("QuotePackage", 0).subscribe((response1: any) => {
            this.IsResourcesReady = true;
            this.SaveData(); 
            this.SetLabels();
            this.BuildData();
        });
    }    

    private savedPackages: QuoteOPPackagePM[] = [];
    private savedVolume: number = null;
    private savedGrossWeight: number = null;
    private savedChargeableWeight: number = null;
    private savedVolumetricWeight: number = null;
    private savedNumberOfPackages: number = null;
    private savedPickupDeliveryChargeableWeight: number = null;
    private savedPickupDeliveryVolumetricWeight: number = null;
    SaveData() {
        this.EntityPM.QuotePackages.forEach(item => {
            var newItem = new QuoteOPPackagePM(null);
            newItem.Id = item.Id;
            newItem.GrossWeight = item.GrossWeight;
            newItem.Height = item.Height;
            newItem.Length = item.Length;
            newItem.PackageTypeId = item.PackageTypeId;
            newItem.PackageTypeName = item.PackageTypeName;
            newItem.Quantity = item.Quantity;
            newItem.QuoteOPId = item.QuoteOPId;
            newItem.Tenant = item.Tenant;
            newItem.Volume = item.Volume;
            newItem.VolumetricWeight = item.VolumetricWeight;
            newItem.Width = item.Width;

            this.savedPackages.push(newItem);
        });

        this.savedVolume = this.EntityPM.Volume;
        this.savedGrossWeight = this.EntityPM.GrossWeight;
        this.savedChargeableWeight = this.EntityPM.ChargeableWeight;
        this.savedVolumetricWeight = this.EntityPM.VolumetricWeight;
        this.savedNumberOfPackages = this.EntityPM.NumberOfPackages;
        this.savedPickupDeliveryChargeableWeight = this.EntityPM.PickupDeliveryChargeableWeight;
        this.savedPickupDeliveryVolumetricWeight = this.EntityPM.PickupDeliveryVolumetricWeight;
    }
   
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public ChargeableWeightLabel: string;
    public VolumetricWeightLabel: string;
    private SetLabels() {
        this.VolumeLabel = TextCodeTranslator.Translate("Quote.F.Volume").replace('%VolumeCode', this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Quote.F.GrossWeight").replace('%GrossWeightCode', this.EntityPM.GrossWeightUnitCode);
        this.ChargeableWeightLabel = TextCodeTranslator.Translate("Quote.F.ChargeableWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate("Quote.F.VolumetricWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
    }
    
    private BuildData() {
        this.ItemsSource = [];

        var list: QuoteOPPackagePM[] = new Array<QuoteOPPackagePM>();
        this.EntityPM.QuotePackages.forEach((item) => {
            list.push(item);
        });

        if (list.length < 5) {
            for (var i = list.length; i < 5; i++) {

                var item: QuoteOPPackagePM = new QuoteOPPackagePM(null);
                item.Tenant = this.EntityPM.Tenant;
                item.QuoteOPId = this.EntityPM.Id;
                list.push(item);
            }
        }

        list.forEach((item) => {
            var itemViewModel: DimensionsPackageItem = new DimensionsPackageItem(item, this);
            this.ItemsSource.push(itemViewModel);
        })
    }

    get NumberOfPackages() { return AppTool.IsNullOrZero(this.EntityPM.NumberOfPackages) ? 0 : this.EntityPM.NumberOfPackages; }
    set NumberOfPackages(newVaule: number) {
        this.EntityPM.NumberOfPackages = newVaule;
    }

    get GrossWeight() { return AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = AppTool.Round(newValue, 3);
            this.ComputeGrossWeigh_Kg_Ton();
        }
    }

    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    set GrossWeightUnitCode(newValue: string) {
        if (this.EntityPM.GrossWeightUnitCode != newValue) {
            this.EntityPM.GrossWeightUnitCode = newValue;
            this.ComputeGrossWeigh_Kg_Ton();
        }
    }

    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }
    set ChargeableWeightUnitCode(newValue: string) {
        if (this.EntityPM.ChargeableWeightUnitCode != newValue) {
            this.EntityPM.ChargeableWeightUnitCode = newValue;
            this.ComputeChargeableWeight_Kg();
        }
    }

    private ComputeGrossWeigh_Kg_Ton() {
        var weigh_Kg: number = null;
        var weigh_Ton: number = null;

        if (this.GrossWeight != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
                switch (this.GrossWeightUnitCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }
                    case "MT": { factorOfConvert = 1000; break; }
                }
            }

            weigh_Kg = this.GrossWeight * factorOfConvert;
        }

        if (weigh_Kg != null) {
            weigh_Kg = AppTool.Round(weigh_Kg, 3);

            weigh_Ton = weigh_Kg / 1000;
        }

        if (weigh_Ton != null) {
            weigh_Ton = AppTool.Round(weigh_Ton, 3);
        }

        this.EntityPM.GrossWeightInKG = weigh_Kg;
        this.EntityPM.GrossWeightPerTon = weigh_Ton;
    }
    private ComputeChargeableWeight_Kg() {
        var weigh_Kg: number = null;
        var weigh_Ton: number = null;

        if (this.ChargeableWeight != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.ChargeableWeightUnitCode)) {
                switch (this.ChargeableWeightUnitCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }
                    case "MT": { factorOfConvert = 1000; break; }
                }
            }

            weigh_Kg = this.ChargeableWeight * factorOfConvert;
        }

        if (weigh_Kg != null) {
            weigh_Kg = AppTool.Round(weigh_Kg, 3);
        }
        this.EntityPM.ChargeableWeightInKG = weigh_Kg;
    }

    get PickupDeliveryVolumetricWeight() { return this.EntityPM.PickupDeliveryVolumetricWeight == null ? 0 : this.EntityPM.PickupDeliveryVolumetricWeight; }
    set PickupDeliveryVolumetricWeight(newValue: number) {
        if (this.EntityPM.PickupDeliveryVolumetricWeight != newValue) {
            this.EntityPM.PickupDeliveryVolumetricWeight = AppTool.Round(newValue, 2);
        }
    }

    get PickupDeliveryChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.PickupDeliveryChargeableWeight) ? null : this.EntityPM.PickupDeliveryChargeableWeight; }
    set PickupDeliveryChargeableWeight(newValue: number) {
        if (this.EntityPM.PickupDeliveryChargeableWeight != newValue) {
            this.EntityPM.PickupDeliveryChargeableWeight = AppTool.Round(newValue, 2);
        }
    }

    get Volume() { return AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 2);
            this.ComputeVolume_CBM();
        }
    }

    private ComputeVolume_CBM() {
        var volume_CBM: number = null;

        if (this.Volume != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.EntityPM.VolumeUnitCode)) {
                switch (this.EntityPM.VolumeUnitCode.toUpperCase()) {
                    case "CBM": { factorOfConvert = 1; break; }
                    case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                    case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                }
            }

            volume_CBM = this.Volume / factorOfConvert;
        }

        if (volume_CBM != null) {
            volume_CBM = AppTool.Round(volume_CBM, 3);
        }
        this.EntityPM.VolumeInCBM = volume_CBM;
    }

    get VolumetricWeight() { return AppTool.IsNullOrZero(this.EntityPM.VolumetricWeight) ? 0 : this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 2);
        }
    }
    
    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            this.EntityPM.ChargeableWeight = AppTool.Round(newValue, 2);
            this.ComputeChargeableWeight_Kg();
        }
    }
    
    public ComputeTotals() {
        if (this.EntityPM.QuotePackages.length == 0) {
            this.Volume = null;
            this.GrossWeight = null;
            this.ChargeableWeight = null;
            this.VolumetricWeight = null;
            this.NumberOfPackages = null;
            this.PickupDeliveryChargeableWeight = null;
            this.PickupDeliveryVolumetricWeight = null;
        }

        else {
            var myNumberOfPackages: number = 0;
            var myVolume: number = 0;
            var myGrossWeight: number = 0;
            var myVolumetricWeight: number = 0;

            this.EntityPM.QuotePackages.forEach((item) => {

                if (!AppTool.IsNullOrEmpty(item.Quantity)) {
                    myNumberOfPackages += item.Quantity;
                }

                if (!AppTool.IsNullOrEmpty(item.Volume)) {
                    myVolume += item.Volume;
                }

                if (!AppTool.IsNullOrEmpty(item.VolumetricWeight)) {
                    myVolumetricWeight += item.VolumetricWeight;
                }

                if (!AppTool.IsNullOrEmpty(item.GrossWeight)) {
                    myGrossWeight += item.GrossWeight;
                }
            })
        }

        this.NumberOfPackages = myNumberOfPackages;
        this.Volume = myVolume;
        this.VolumetricWeight = myVolumetricWeight;
        this.PickupDeliveryVolumetricWeight = AppTool.ComputePackageVolumetricWeight(null, null, null, null, this.Volume, this.GrossWeight, this.EntityPM.PickupDeliveryRatio, this.EntityPM.DimensionsUnitCode, this.EntityPM.VolumeUnitCode, this.GrossWeightUnitCode, this.EntityPM.PickupDeliveryCWeightUnitCode);
        this.PickupDeliveryChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.PickupDeliveryVolumetricWeight, this.GrossWeightUnitCode, this.EntityPM.PickupDeliveryCWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);

        this.GrossWeight = myGrossWeight;
        this.ChargeableWeight = AppTool.CalculateChargeableWeight(this.GrossWeight, this.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
    }

    AddPackageClicked() {
        var itemPM = new QuoteOPPackagePM(null);
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.QuoteOPId = this.EntityPM.Id;

        var logeWindow = new LogitudeWindow();
        logeWindow.Title = TextCodeTranslator.Translate("Quote.B.AddPackage");
        logeWindow.WindowArgs = new DimensionsPackageItem(itemPM, this);
        logeWindow.Show("./Quote/Components/NewEntity/NewQuoteAddEditDimensionsComponent");
        logeWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.ComputeTotals();
            }
        });
    }
    DeletePackage(itemViewModel: DimensionsPackageItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Quote.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var itemIndex = this.ItemsSource.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    this.ItemsSource.splice(itemIndex, 1);
                }

                this.EntityPM.RemoveQuoteOPPackage(itemViewModel.EntityPM);

                this.ComputeTotals();
            }
        });
    }   

    CancelButtonClicked() {
        this.EntityPM.QuotePackages = [];

        this.savedPackages.forEach(item => {
            this.EntityPM.AddQuoteOPPackage(item);
        });

        this.EntityPM.Volume = this.savedVolume;
        this.EntityPM.GrossWeight = this.savedGrossWeight;
        this.EntityPM.ChargeableWeight = this.savedChargeableWeight;
        this.EntityPM.VolumetricWeight = this.savedVolumetricWeight;
        this.EntityPM.NumberOfPackages = this.savedNumberOfPackages;
        this.EntityPM.PickupDeliveryChargeableWeight = this.savedPickupDeliveryChargeableWeight;
        this.EntityPM.PickupDeliveryVolumetricWeight = this.savedPickupDeliveryVolumetricWeight;

        this.CurrentSession.CloseCurrentWindow();
    }
    
    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }
}
export class DimensionsPackageItem extends BaseComponent {
    public EntityPM: QuoteOPPackagePM;
    public QuoteOPPM: QuoteOPPM;
    public DataContext: DimensionsPackageItem = this;   
    public ObjectTableName: string = "QuotePackage";
    public IsWindowMode: boolean = false;
    public IsPackageTypeVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityPM: QuoteOPPackagePM, public fatherComponent: QuoteDimensionsComponent) {
        super();
        this.EntityPM = entityPM;
        this.QuoteOPPM = fatherComponent.EntityPM;
        this.IsPackageTypeVisible = fatherComponent.IsPackageTypeVisible;
        this.SetUIProperties();
    }
    
    public SetUIProperties() {
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;

        if (this.Quantity > 0) {
            isFieldEnabled = true;
            isVolumeEnabled = true;
            isDimensionEnabled = true;

            if (this.Height != null || this.Width != null || this.Length != null) {
                isVolumeEnabled = false;
            }

            else if (this.Volume != null) {
                isDimensionEnabled = false;
            }
        }

        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("PackageTypeId", this.ObjectTableName, isFieldEnabled);

        if (this.IsPackageTypeVisible) {
            this.UIProperties.SetRequired("PackageTypeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.PackageTypeId) ? true : false);
            this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, AppTool.IsNullOrZero(this.GrossWeight) ? true : false);
        }
    }
    
    get PackageTypeId() { return this.EntityPM.PackageTypeId; }
    set PackageTypeId(newValue: string) {
        if (this.EntityPM.PackageTypeId != newValue) {
            this.EntityPM.PackageTypeId = newValue;

            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.EntityPM.PackageTypeName = null;
            }

            else {

                var packageTypeService = new PackageTypeListService();                
                packageTypeService.getSingleFromCache(newValue).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var myPackageTypeList: PackageTypeList = myResponse.Result;
                        if (myPackageTypeList != null) {
                            this.EntityPM.PackageTypeName = myPackageTypeList.EnglishName;
                        }
                    }
                });
            }            
        }
    }

    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    set PackageTypeName(newValue: string) {
        if (this.EntityPM.PackageTypeName != newValue) {
            this.EntityPM.PackageTypeName = newValue;
        }
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = AppTool.Round(newValue, 0);

            var itemIndex = this.QuoteOPPM.QuotePackages.indexOf(this.EntityPM);

            if (AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                this.Height = null;
                this.Length = null;
                this.Width = null;
                this.Volume = null;
                this.GrossWeight = null;
                this.VolumetricWeight = null;

                if (!this.IsWindowMode) {
                    if (itemIndex > -1) {
                        this.QuoteOPPM.RemoveQuoteOPPackage(this.EntityPM);
                    }
                }
            }

            else {
                if (!this.IsWindowMode) {
                    if (itemIndex == -1) {
                        this.QuoteOPPM.AddQuoteOPPackage(this.EntityPM);
                    }
                }
            }

            this.SetUIProperties();
            this.ComputeVolume();
            this.fatherComponent.ComputeTotals();
        }
    }

    get Length() { return this.EntityPM.Length; }
    set Length(newValue: number) {
        if (this.EntityPM.Length != newValue) {
            this.EntityPM.Length = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Width() { return this.EntityPM.Width; }
    set Width(newValue: number) {
        if (this.EntityPM.Width != newValue) {
            this.EntityPM.Width = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Height() { return this.EntityPM.Height; }
    set Height(newValue: number) {
        if (this.EntityPM.Height != newValue) {
            this.EntityPM.Height = AppTool.Round(newValue, 2);
            this.ComputeVolume();
            this.SetUIProperties();
        }
    }

    get Dimensions() {
        var myDimensions: string;

        if (this.Length == null && this.Width == null && this.Height == null) {
            myDimensions = " - - ";
        }

        else {
            var myLength: number = 0;
            var myWidth: number = 0;
            var myHeight: number = 0;

            if (this.Length != null) {
                myLength = this.Length;
            }

            if (this.Width != null) {
                myWidth = this.Width;
            }

            if (this.Height != null) {
                myHeight = this.Height;
            }

            myDimensions = myLength + "-" + myWidth + "-" + myHeight;
        }

        return myDimensions;
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 2);
            this.ComputeVolumetricWeight();
            this.SetUIProperties();
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 2);
            this.fatherComponent.ComputeTotals();
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        var myValue: number = AppTool.Round(newValue, 2);
        if (this.EntityPM.GrossWeight != myValue) {
            this.EntityPM.GrossWeight = myValue

            this.SetUIProperties();
            this.fatherComponent.ComputeTotals();
        }
    }

    OnGrossWeightLostFocus(input: number) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.QuoteOPPM.GrossWeightUnitCode, this.QuoteOPPM.ChargeableWeightUnitCode, this.EntityPM.GrossWeight);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.QuoteOPPM.ChargeableWeightUnitCode, this.QuoteOPPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.QuoteOPPM.Ratio);
                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
            }
        }
    }

    private ComputeVolume() {
        if (this.QuoteOPPM.Ratio == null) {
            this.QuoteOPPM.Ratio = AppTool.GetRatio(this.QuoteOPPM.DirectionId, this.QuoteOPPM.TransportModeId, this.QuoteOPPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        if (this.QuoteOPPM.PickupDeliveryRatio == null) {
            this.QuoteOPPM.PickupDeliveryRatio = AppTool.GetPickupDeliveryRatio(this.QuoteOPPM.ShipmentTypeId);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.GrossWeight, this.QuoteOPPM.Ratio, this.QuoteOPPM.DimensionsUnitCode, this.QuoteOPPM.VolumeUnitCode, this.QuoteOPPM.GrossWeightUnitCode);
    }
    private ComputeVolumetricWeight() {
        if (this.QuoteOPPM.Ratio == null) {
            this.QuoteOPPM.Ratio = AppTool.GetRatio(this.QuoteOPPM.DirectionId, this.QuoteOPPM.TransportModeId, this.QuoteOPPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        if (this.QuoteOPPM.PickupDeliveryRatio == null) {
            this.QuoteOPPM.PickupDeliveryRatio = AppTool.GetPickupDeliveryRatio(this.QuoteOPPM.ShipmentTypeId);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.GrossWeight, this.QuoteOPPM.Ratio, this.QuoteOPPM.DimensionsUnitCode, this.QuoteOPPM.VolumeUnitCode, this.QuoteOPPM.GrossWeightUnitCode, this.QuoteOPPM.ChargeableWeightUnitCode);
    }
}
