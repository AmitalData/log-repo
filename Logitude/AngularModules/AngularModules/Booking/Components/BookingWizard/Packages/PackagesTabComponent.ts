import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {BookingPM} from '../../../EntityPMs/BookingPM';
import {BookingPackagePM} from '../../../EntityPMs/BookingPackagePM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {BookingWizardComponent} from '../BookingWizardComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {BookingTool} from '../../../Tools';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
@Component({
    selector: 'PackagesTabComponent',
    moduleId: module.id,
    templateUrl: './PackagesTabComponent.html',
})

export class PackagesTabComponent extends BaseComponent {
    public EntityPM: BookingPM;
    public Wizard: BookingWizardComponent;
    public DataContext: PackagesTabComponent = this;
    public ObjectTableName: string;
    public ShipmentLevelCode: string;
    public ItemsSource: BookingWizardPackageItem[];
    public TabSummaryAreaHeight: number = 100;
    public TenantPM: TenantPM;
    public IsVisible: boolean = false;
    private firstDigit: string = ",";
    private secondDigit: string = ".";

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor() {
        super();
        this.TenantPM = InfraSettings.TenantPM;
        this.setDigits();

    }
    
    InitTab(wizard: BookingWizardComponent) {
        this._entityResourceService.getEntityResourceByTableName("BookingPackage", 0).subscribe((response1: any) => {
            this.IsVisible = true;
            this.Wizard = wizard;
            this.EntityPM = this.Wizard.EntityPM;
            this.ObjectTableName = this.Wizard.ObjectTableName;
            this.SetLabels();
            this.BuildData();
            this.SetUIProperties();
            this.Listen();
            this.Validate();
        });
    }

    RefreshTab() {
        this.SetUIProperties();
        this.Validate();
        this.BuildData();
    }
      
    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.RefreshTab();
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.BuildData();
                    this.RefreshTab();
                }
            });
        }
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    public IsTotalsFieldEnabled: boolean = true;
    public IsChooseDescriptionOfGoodsVisible: boolean = false;
    public IsChooseDescriptionOfGoodsEnabled: boolean = false;
    public IsTemperatureSensitiveHelpVisible: boolean = false;
    SetUIProperties() {
        this.IsEditingEnabled = BookingTool.IsEditingFieldsEnabled_Others(this.EntityPM);
        var isFieldEnabled = false;

        if (this.IsEditingEnabled) {
            if (this.EntityPM.BookingPackages.length > 0) {
                isFieldEnabled = true;
            }
        }

        this.ItemsSource.forEach(item => {
            item.SetUIProperties();
        });

        this.IsTotalsFieldEnabled = isFieldEnabled;

        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isFieldEnabled);

        // Ayman: Task 28676
        //this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, false);

        if (this.EntityPM.ZeroIsDescOfGoodsFromList) {
            this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, false);
            this.UIProperties.SetVisibility("DescriptionOfGoodsService", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("DescriptionOfGoodsService", this.ObjectTableName, false);

            this.IsChooseDescriptionOfGoodsEnabled = isFieldEnabled;
            this.IsChooseDescriptionOfGoodsVisible = true;
        }
        else {
            this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetVisibility("DescriptionOfGoodsService", this.ObjectTableName, false);

            this.IsChooseDescriptionOfGoodsEnabled = false;
            this.IsChooseDescriptionOfGoodsVisible = false;
        }  

        if (this.EntityPM.IsTemperatureSensitive) {
            this.IsTemperatureSensitiveHelpVisible = true;
        }
        else {
            this.IsTemperatureSensitiveHelpVisible = false;
        }        
    }

    // Validate
    public ShowWarning_GrossWeight: boolean = false;
    public ShowWarning_DescriptionOfGoods: boolean = false;
    private FireWizardEvent() {
        this.Validate();
        this.Wizard.ValidateScreen_PAC();
    }
    private Validate() {
        var isShowWarning_GrossWeight = false;
        var isShowWarning_DescriptionOfGoods = false;

        if (AppTool.IsNullOrZero(this.GrossWeight)) {
            isShowWarning_GrossWeight = true;
        }

        if (AppTool.IsNullOrEmpty(this.DescriptionOfGoods)) {
            isShowWarning_DescriptionOfGoods = true;
        }

        if (!isShowWarning_DescriptionOfGoods) {
            var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "DescriptionOfGoods")[0];
            if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.DescriptionOfGoods)) {
                isShowWarning_DescriptionOfGoods = true;
            }
        }

        this.ShowWarning_GrossWeight = isShowWarning_GrossWeight;
        this.ShowWarning_DescriptionOfGoods = isShowWarning_DescriptionOfGoods;
    }

    // Properties    
    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
            this.FireWizardEvent();
        }
    }

    get DescriptionOfGoodsService() { return this.EntityPM.DescriptionOfGoodsService; }
    set DescriptionOfGoodsService(newValue: string) {
        if (this.EntityPM.DescriptionOfGoodsService != newValue) {
            this.EntityPM.DescriptionOfGoodsService = newValue;
        }
    }

    get IsDangerous() { return this.EntityPM.IsDangerous; }
    set IsDangerous(newValue: boolean) {
        if (this.EntityPM.IsDangerous != newValue) {
            this.EntityPM.IsDangerous = newValue;

            if (!newValue) {
                this.EntityPM.DangerousClassNumber = null;
                this.EntityPM.DangerousUnNumber = null;
                this.EntityPM.DangerousPackagingGroup = null;
                this.EntityPM.DangerousIMDGCode = null;
                this.EntityPM.DangerousFlashPoint = null;
                this.EntityPM.DangerousMaterialDescription = null;
            }
        }
    }

    get Volume() { return AppTool.IsNullOrZero(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 3);
        }
    }

    get VolumetricWeight() { return AppTool.IsNullOrZero(this.EntityPM.VolumetricWeight) ? 0 : this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 3);
        }
    }

    get NumberOfPackages() { return AppTool.IsNullOrZero(this.EntityPM.NumberOfPackages) ? 0 : this.EntityPM.NumberOfPackages; }
    set NumberOfPackages(newVaule: number) {
        this.EntityPM.NumberOfPackages = newVaule;
    }

    get GrossWeight() { return AppTool.IsNullOrZero(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {

        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = AppTool.Round(newValue, 3);
            this.FireWizardEvent();
        }
    }

    get ChargeableWeight() { return AppTool.IsNullOrZero(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            this.EntityPM.ChargeableWeight = AppTool.Round(newValue, 3);
        }
    }
    
    // Labels
    public VolumeColumnHeader: string;
    public WeightColumnHeader: string;
    public DimensionsColumnHeader: string;
    public VolumetricWeightColumnHeader: string;
    public VolumeLabel: string;
    public VolumetricWeightLabel: string;
    private SetLabels() {
        this.VolumeColumnHeader = TextCodeTranslator.Translate("Booking.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator.Translate("Booking.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Booking.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator.Translate("Booking.O.Packages.VolWeight").replace("%UnitCode", this.EntityPM.ChargeableWeightUnitCode);

        this.VolumeLabel = TextCodeTranslator.Translate("Booking.O.Packages.Volume").replace('%UnitCode', this.EntityPM.VolumeUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate("Booking.O.Packages.VolWeight").replace('%UnitCode', this.EntityPM.ChargeableWeightUnitCode);
    }

    // BuildData
    public BuildData() {
        this.ItemsSource = [];

        var list: BookingPackagePM[] = new Array<BookingPackagePM>();
        this.EntityPM.BookingPackages.forEach((item) => {
            list.push(item);
        });

        if (list.length < 5) {
            for (var i = list.length; i < 5; i++) {

                var item: BookingPackagePM = new BookingPackagePM(null);
                item.Tenant = this.EntityPM.Tenant;
                item.BookingId = this.EntityPM.Id;
                list.push(item);
            }
        }

        list.sort((a, b) => { return (a === b) ? 0 : a ? -1 : 1 }).forEach((item) => {
            var itemViewModel: BookingWizardPackageItem = new BookingWizardPackageItem(item, false, this);
            this.ItemsSource.push(itemViewModel);
            itemViewModel.SetUIProperties();
        })
    }

    private setDigits() {
        switch (SessionLocator.TenantPM.NumberFormatCode) {
            case "CD": {
                this.firstDigit = ",";
                this.secondDigit = ".";
                break;
            }

            case "DC": {
                this.firstDigit = ".";
                this.secondDigit = ",";
                break;
            }

            case "AD": {
                this.firstDigit = "'";
                this.secondDigit = ".";
                break;
            }

            default:
                {
                    this.firstDigit = ",";
                    this.secondDigit = ".";
                    break;
                }
        }
    }

    GrossWeightLostFocus(input: any) {

        var valueComputed: number = 0;
        var valueInserted: number = 0;

        this.EntityPM.BookingPackages.forEach((item) => {
            if (!AppTool.IsNullOrEmpty(item.Weight)) {
                valueComputed += item.Weight;
            }
        });

        if (!AppTool.IsNullOrEmpty(input)) {
            if (this.firstDigit == ".") {
                input = input.replace(/\./g, '');
                input = input.replace(/,/g, ".");
            }

            else if (this.firstDigit == "'") {
                input = input.replace(/'/g, '');
            }
            else {
                input = AppTool.Replace(input, ",", "");
            }
            valueInserted = Number(input);
        }

        valueComputed = valueComputed == 0 ? null : valueComputed;
        valueInserted = valueInserted == 0 ? null : valueInserted;
        this.EntityPM.GrossWeightEdited = !(valueComputed == valueInserted);
        this.GrossWeight = valueInserted;
        this.ComputeTotals();
    }

    ChargeableWeightLostFocus(input: any) {

        var valueComputed: number = 0;
        var valueInserted: number = 0;

        valueComputed = AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionCode, this.EntityPM.TransportModeCode);

        if (!AppTool.IsNullOrEmpty(input)) {
            if (this.firstDigit == ".") {
                input = input.replace(/\./g, '');
                input = input.replace(/,/g, ".");
            }

            else if (this.firstDigit == "'") {
                input = input.replace(/'/g, '');
            }
            else {
                input = AppTool.Replace(input, ",", "");
            }
            valueInserted = Number(input);
        }

        valueComputed = valueComputed == 0 ? null : valueComputed;
        valueInserted = valueInserted == 0 ? null : valueInserted;
        this.EntityPM.ChargeableWeightEdited = !(valueComputed == valueInserted);
        this.ChargeableWeight = AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionCode, this.EntityPM.TransportModeCode);
        this.ComputeTotals();
    }
    
    ResetGrossWeightEdited() {
        this.EntityPM.GrossWeightEdited = false;
        this.ComputeTotals();
    }
    ResetChargeableWeightEdited() {
        this.EntityPM.ChargeableWeightEdited = false;
        this.ComputeTotals();
    }
    ResetTotalEditedValues() {
        this.EntityPM.GrossWeightEdited = false;
        this.EntityPM.ChargeableWeightEdited = false;
    }
    
    ComputeTotals() {
        this.SetUIProperties();

        if (this.EntityPM.BookingPackages.length == 0) {
            this.EntityPM.NumberOfPackages = null;
            this.EntityPM.GrossWeight = null;
            this.EntityPM.Volume = null;
            this.EntityPM.VolumetricWeight = null;
            this.EntityPM.ChargeableWeight = null;
            this.EntityPM.AWBCommodityItemNumber = null;
            this.EntityPM.GrossWeightEdited = false;
            this.EntityPM.ChargeableWeightEdited = false;
        }

        else {

            var myQuantity: number = 0;
            var myVolume: number = 0;
            var myGrossWeight: number = 0;
            var myVolumetricWeight: number = 0;

            this.EntityPM.BookingPackages.forEach((item) => {

                if (!AppTool.IsNullOrEmpty(item.Quantity)) {
                    myQuantity += item.Quantity;
                }

                if (!AppTool.IsNullOrEmpty(item.Volume)) {
                    myVolume += item.Volume;
                }

                if (!AppTool.IsNullOrEmpty(item.VolumetricWeight)) {
                    myVolumetricWeight += item.VolumetricWeight;
                }

                if (!AppTool.IsNullOrEmpty(item.Weight)) {
                    myGrossWeight += item.Weight;
                }
            })
        }

        this.NumberOfPackages = myQuantity;
        this.Volume = myVolume;
        this.VolumetricWeight = myVolumetricWeight;

        if (!this.EntityPM.GrossWeightEdited) {
            this.GrossWeight = AppTool.Round(myGrossWeight, 3);
        }

        if (!this.EntityPM.ChargeableWeightEdited) {
            this.EntityPM.ChargeableWeight = AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionCode, this.EntityPM.TransportModeCode);
        }

        this.SetUIProperties();
        this.FireWizardEvent();
    }

    public AddPackage() {
        this._entityResourceService.getEntityResourceByTableName("BookingPackage", 0).subscribe((response: any) => {
            var itemPM = new BookingPackagePM(null);
            itemPM.BookingId = this.EntityPM.Id;
            itemPM.Tenant = this.EntityPM.Tenant;
            var itemViewModel = new BookingWizardPackageItem(itemPM, true, this);
            this.RunPackageWindow(itemViewModel, "Add Package line");
        });
    };
    public EditPackage(itemViewModel: BookingWizardPackageItem) {
        this.RunPackageWindow(itemViewModel, "Edit Package line");
    }
    public DeletePackage(itemViewModel: BookingWizardPackageItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Booking.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var itemIndex = this.ItemsSource.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    this.ItemsSource.splice(itemIndex, 1);
                }

                var index = this.EntityPM.BookingPackages.indexOf(itemViewModel.EntityPM);
                if (index > -1) {
                    this.EntityPM.RemoveBookingPackage(itemViewModel.EntityPM);
                }

                this.ComputeTotals();
                this.SetUIProperties();
                this.FireWizardEvent();
            }
        });
    }
    private RunPackageWindow(itemComponent: BookingWizardPackageItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./Booking/Components/BookingWizard/Packages/AddEditPackageComponent');
    }

    public EditDangerouse() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Booking.O.Packages.EditDangerousGoods");
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnLogitudeWindowClosed($event));
        logitudeWindow.Show('./Booking/Components/BookingWizard/Packages/DangerousPackageComponent');
    }
    OnLogitudeWindowClosed(message: string) {
        if (message == "ok") {
            this.IsDangerous = this.EntityPM.IsDangerous;
        }
    }

    public ChooseDescriptionOfGoods() {
        this._entityResourceService.getEntityResourceByTableName("AWBDescriptionOfGoods", 0).subscribe((response1: any) => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = "AWB Description of Goods Search";
            logitudeWindow.WindowArgs = this.EntityPM;
            logitudeWindow.Show('./Booking/Components/BookingWizard/Packages/ChooseDescriptionOfGoodsComponent');
            logitudeWindow.WindowClosed.subscribe(($event: any) => {
                this.SetUIProperties();
                this.Validate();
                this.Wizard.ValidateScreen_GEN();
                if (this.Wizard.PageChild_GEN != null) {
                    this.Wizard.PageChild_GEN.Validate_AWBSpecialHandlingCodes();
                }
            });
        });
    }
}
export class BookingWizardPackageItem extends BaseComponent {
    public DataContext: BookingWizardPackageItem = this;
    public EntityPM: BookingPackagePM;
    public BookingPM: BookingPM;
    public ObjectTableName: string = "BookingPackage";
    public IsNewEntity: boolean;
    public IsWindowMode: boolean;
    constructor(entityPM: BookingPackagePM, isNew: boolean, public fatherComponent: PackagesTabComponent) {
        super();
        this.EntityPM = entityPM;
        this.BookingPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.SetUIProperties();
    }

    public SetUIProperties() {
        var isEditingEnabled = BookingTool.IsEditingFieldsEnabled_Others(this.BookingPM);
        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;

        if (isEditingEnabled) {
            if (this.Quantity > 0 || this.hasValue) {
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
        }

        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isFieldEnabled);

        this.Validate();
    }

    private hasValue: boolean;
    public HasValue(hasValue: boolean) {
        this.hasValue = hasValue;
        this.SetUIProperties();
    }

    public ShowWarning_Dimensions: boolean = false;
    public ShowWarning_GrossWeight: boolean = false;
    private Validate() {
        this.ShowWarning_Dimensions = false;
        this.ShowWarning_GrossWeight = false;

        if (!AppTool.IsNullOrZero(this.Quantity)) {
            if (AppTool.IsNullOrZero(this.Weight)) {
                this.ShowWarning_GrossWeight = true;
            }

            if (AppTool.IsNullOrZero(this.Volume)) {
                if (AppTool.IsNullOrZero(this.Height) || AppTool.IsNullOrZero(this.Width) || AppTool.IsNullOrZero(this.Length)) {
                    this.ShowWarning_Dimensions = true;
                }
            }
        }
    }
    
    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = AppTool.Round(newValue, 0);

            var itemIndex = this.BookingPM.BookingPackages.indexOf(this.EntityPM);

            if (AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                this.Height = null;
                this.Length = null;
                this.Width = null;
                this.Volume = null;
                this.VolumetricWeight = null;
                this.Weight = null;

                if (!this.IsWindowMode) {
                    if (itemIndex > -1) {
                        this.BookingPM.RemoveBookingPackage(this.EntityPM);
                    }
                }
            }

            else {
                if (!this.IsWindowMode) {
                    if (itemIndex == -1) {
                        this.BookingPM.AddBookingPackage(this.EntityPM);
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
            myDimensions = "";
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
            this.EntityPM.Volume = AppTool.Round(newValue, 3);
            this.ComputeVolumetricWeight();
            this.SetUIProperties();
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 3);
            this.fatherComponent.ComputeTotals();
        }
    }

    get Weight() { return this.EntityPM.Weight; }
    set Weight(newValue: number) {
        var myValue: number = AppTool.Round(newValue, 3);

        if (this.EntityPM.Weight != myValue) {
            this.EntityPM.Weight = myValue

            this.SetUIProperties();
            this.fatherComponent.ResetTotalEditedValues();
            this.fatherComponent.ComputeTotals();
        }
    }

    OnGrossWeightLostFocus(input: number) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {

            if (this.BookingPM.TransportModeCode == "A") {
                if (this.Width == null || this.Height == null || this.Length == null) {
                    this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.BookingPM.GrossWeightUnitCode, this.BookingPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                    this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.BookingPM.ChargeableWeightUnitCode, this.BookingPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.BookingPM.Ratio);

                    this.SetUIProperties();
                    this.fatherComponent.ResetTotalEditedValues();
                    this.fatherComponent.ComputeTotals();
                }
            }
        }
    }

    private ComputeVolume() {

        if (this.BookingPM.Ratio == null) {
            this.BookingPM.Ratio = AppTool.GetRatio(this.BookingPM.DirectionCode, this.BookingPM.TransportModeCode, null, this.fatherComponent.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.BookingPM.Ratio, this.BookingPM.DimensionsUnitCode, this.BookingPM.VolumeUnitCode, this.BookingPM.GrossWeightUnitCode);
    }
    private ComputeVolumetricWeight() {
        if (this.BookingPM.Ratio == null) {
            this.BookingPM.Ratio = AppTool.GetRatio(this.BookingPM.DirectionCode, this.BookingPM.TransportModeCode, null, this.fatherComponent.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.BookingPM.Ratio, this.BookingPM.DimensionsUnitCode, this.BookingPM.VolumeUnitCode, this.BookingPM.GrossWeightUnitCode, this.BookingPM.ChargeableWeightUnitCode);
    }
}
