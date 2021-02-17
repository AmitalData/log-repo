import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {AppTool, ArrayTool, FormatTool} from '../../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentDomainService} from '../../../../../Shipment/Services/ShipmentDomainService';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { ShipmentCommodityPM } from '../../../../../Shipment/EntityPMs/ShipmentCommodityPM';
import { CommodityPackagePM } from '../../../../../Shipment/EntityPMs/CommodityPackagePM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { forEach } from 'cypress/types/lodash';

@Component({    
    selector: 'AWBPackagesTabComponent',
    templateUrl: './AWBPackagesTabComponent.html',
})

export class AWBPackagesTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: AWBPackagesTabComponent = this;
    public ObjectTableName: string;
    public ShipmentLevelCode: string;
    public ItemsSource: AWBWizardPackageItem[];
    public ItemsSourceOfCommodities: ObservableCollection;
    public TabSummaryAreaHeight: number = 150;
    private DomainService: ShipmentDomainService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    private firstDigit: string = ",";
    private secondDigit: string = ".";
    IsMultipleCommoditiesVisible: boolean = false;
    constructor() {
        super();
        this.DomainService = new ShipmentDomainService();
        this.ItemsSource = [];
        this.ItemsSourceOfCommodities = new ObservableCollection([]);
        this.setDigits();       
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.ShipmentLevelCode = this.Wizard.ShipmentLevelCode;
        this.SetLabels();
        this.BuildData();
        this.Listen();
        this.Validate();
        this.SetUIProperties();

        if (this.EntityPM.ShipmentLevelCode != 'H') {
            var hasToggleFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "AMC" && d.TenantNumber == SessionLocator.Tenant)[0]
            if (hasToggleFeature) {
                this.IsMultipleCommoditiesVisible = true;
            }
        }
    }

    RefreshTab() {
        this.Validate();
        this.SetUIProperties();
        this.SetRebuildButton();
        this.SetGenerateButton();
    }
    
    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.BuildData();
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

    private ChargeableWeightPasted: boolean = false;
    ChargeableWeightPaste($event) {
        this.ChargeableWeightPasted = true;
    }

    private GrossWeightPasted: boolean = false;
    GrossWeightPaste($event) {
        this.GrossWeightPasted = true;
    }


    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    public IsTotalsFieldEnabled: boolean = false;
    SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);

        var isFieldEnabled = this.IsEditingEnabled;
        var isFieldVisible = !this.IsMultipleCommodities;
        var tabSummaryAreaHeight = 150;

        if (isFieldEnabled) {
            isFieldEnabled = false;

            if (this.EntityPM.IsMultipleCommodities) {
                tabSummaryAreaHeight = 120;

                if (this.EntityPM.ShipmentCommodities != null) {
                    if (this.EntityPM.ShipmentCommodities.length > 0) {
                        isFieldEnabled = true;
                    }
                }
            }

            else {
                tabSummaryAreaHeight = 150;

                if (this.EntityPM.ShipmentPackages != null) {
                    if (this.EntityPM.ShipmentPackages.length > 0) {
                        isFieldEnabled = true;
                    }
                }
            }
        }

        this.TabSummaryAreaHeight = tabSummaryAreaHeight;
        this.IsTotalsFieldEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("ChargeableWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("AWBCommodityItemNumber", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("IsDangerous", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("DescriptionOfGoods", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("SLAC", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetVisibility("AWBCommodityItemNumber", this.ObjectTableName, isFieldVisible);
        this.UIProperties.SetVisibility("DescriptionOfGoods", this.ObjectTableName, isFieldVisible);
        this.UIProperties.SetVisibility("SLAC", this.ObjectTableName, isFieldVisible);

        this.ItemsSource.forEach(item => {
            item.SetUIProperties();
        });
    }

    // Validate
    public ShowWarning_GrossWeight: boolean = false;
    public ShowWarning_ChargeableWeight: boolean = false;
    public ShowWarning_AWBCommodityItemNumber: boolean = false;
    public ShowWarning_DescriptionOfGoods: boolean = false;
    FireWizardEvent() {
        this.Wizard.ValidateScreen_PAC();
        this.Wizard.ValidateScreen_FRE();
        this.Wizard.ValidateScreen_GEN();
    }
    private Validate() {
        if (!this.Wizard.IsImportWizard) {
            var isShowWarning_GrossWeight = false;
            var isShowWarning_ChargeableWeight = false;
            var isShowWarning_AWBCommodityItemNumber = false;
            var isShowWarning_DescriptionOfGoods = false;

            if (AppTool.IsNullOrZero(this.GrossWeight)) {
                isShowWarning_GrossWeight = true;
            }

            if (this.Wizard.IsFWB) {
                if (AppTool.IsNullOrZero(this.ChargeableWeight)) {
                    isShowWarning_ChargeableWeight = true;
                }
            }

            else {
                if (AppTool.IsNullOrEmpty(this.DescriptionOfGoods)) {
                    isShowWarning_DescriptionOfGoods = true;
                }
            }

            if (!this.IsMultipleCommodities) {
                if (this.Wizard.IsFWB) {

                    if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.MainCarriageCarrierCode == "AR") {
                        if (AppTool.IsNullOrEmpty(this.DescriptionOfGoods)) {
                            isShowWarning_DescriptionOfGoods = true;
                        }
                    }

                    if (!FormatTool.Validate_CommodityNo(this.AWBCommodityItemNumber)) {
                        isShowWarning_AWBCommodityItemNumber = true;
                    }

                    if (!isShowWarning_AWBCommodityItemNumber) {
                        var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "AWBCommodityItemNumber")[0];
                        if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.AWBCommodityItemNumber)) {
                            isShowWarning_AWBCommodityItemNumber = true;
                        }
                    }
                }

                else {
                    if (AppTool.IsNullOrEmpty(this.DescriptionOfGoods)) {
                        isShowWarning_DescriptionOfGoods = true;
                    }
                }

                if (!isShowWarning_DescriptionOfGoods) {
                    var myFieldRule = this.Wizard.AirlineRulesList.filter(d => d.RuleFieldName == "DescriptionOfGoods")[0];
                    if (!AppTool.IsAirlineRuleFieldValid(myFieldRule, this.DescriptionOfGoods)) {
                        isShowWarning_DescriptionOfGoods = true;
                    }
                }
            }
        }

        this.ShowWarning_GrossWeight = isShowWarning_GrossWeight;
        this.ShowWarning_ChargeableWeight = isShowWarning_ChargeableWeight;
        this.ShowWarning_AWBCommodityItemNumber = isShowWarning_AWBCommodityItemNumber;
        this.ShowWarning_DescriptionOfGoods = isShowWarning_DescriptionOfGoods;
    }

    // Labels
    public VolumeColumnHeader: string;
    public WeightColumnHeader: string;
    public DimensionsColumnHeader: string;
    public VolumetricWeightColumnHeader: string;
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public ChargeableWeightLabel: string;
    public VolumetricWeightLabel: string;
    private SetLabels() {
        this.VolumeColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace("%UnitCode", this.EntityPM.VolumeUnitCode);
        this.WeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace("%UnitCode", this.EntityPM.GrossWeightUnitCode);
        this.DimensionsColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace("%UnitCode", this.EntityPM.DimensionsUnitCode);
        this.VolumetricWeightColumnHeader = TextCodeTranslator.Translate("Shipment.O.Packages.VolWeight").replace("%UnitCode", this.EntityPM.ChargeableWeightUnitCode);

        this.VolumeLabel = TextCodeTranslator.Translate("Shipment.F.Volume").replace('%VolumeCode', this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Shipment.F.GrossWeight").replace('%GrossWeightCode', this.EntityPM.GrossWeightUnitCode);
        this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate("Shipment.F.VolumetricWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
    }

    // Rebuild
    public IsRebuildButtonVisible: boolean = false;
    public SetRebuildButton() {
        var isRebuildButtonVisible = false;

        if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0 && this.EntityPM.ShipmentPackages.length > 0) {
            isRebuildButtonVisible = true;
        }

        this.IsRebuildButtonVisible = isRebuildButtonVisible;
    }
    RebuildClicked() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Rebuild Packages?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                if (this.EntityPM.ShipmentPackages.length > 0) {
                    this.EntityPM.ShipmentPackages = [];
                    this.EntityPM.IsDirty = true;
                    this.ItemsSource = [];
                    this.ComputeTotals();
                    this.SetUIProperties();
                    this.Validate();
                    this.FireWizardEvent();
                    this.SetRebuildButton();
                    this.SetGenerateButton(); 
                }

                //var list = this.EntityPM.ShipmentPackages.filter(f => f.OriginalShipmentPackageId != null);
                //list.forEach(item => {
                //    this.EntityPM.RemovePackage(item);
                //});

                this.GenerateClicked();
            }
        });
    }

    // Generate
    public IsGeneratingVisible: boolean = false;
    public IsBuildFromShipmentsVisible: boolean = false;
    public IsNoPackagesLoadedTextVisible: boolean = false;
    public BuildFromShipmentsLabel: string = null;
    public SetGenerateButton() {
        var isGeneratingVisible = false;
        var isBuildFromShipmentsVisible = false;

        if (this.IsMultipleCommodities == false) {
            if (this.ItemsSource.length == 0) {
                isGeneratingVisible = true;
            }

            if (this.EntityPM.ShipmentLevelCode == "C") {
                if (this.EntityPM.ShipmentConsoleShipments.length > 0) {
                    isBuildFromShipmentsVisible = true;
                }
            }
        }

        this.BuildFromShipmentsLabel = "Build From " + this.EntityPM.ShipmentConsoleShipments.length + " Shipments";
        this.IsGeneratingVisible = isGeneratingVisible;
        this.IsBuildFromShipmentsVisible = isBuildFromShipmentsVisible;
    }
    GenerateClicked() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.DomainService.GetShipmentConsolidationPackages(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();

            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var allPackages: ShipmentPackagePM[] = myResponse.Result;

                    if (allPackages.length == 0) {
                        this.IsNoPackagesLoadedTextVisible = true;
                    }

                    else {
                        this.IsNoPackagesLoadedTextVisible = false;

                        allPackages.forEach(item => {

                            var matchedItem = this.EntityPM.ShipmentPackages.filter(f => f.Height == item.Height && f.Width == item.Width && f.Length == item.Length && f.PackageTypeId == item.PackageTypeId)[0];
                            if (matchedItem != null) {

                                // Quantity
                                if (AppTool.IsNullOrEmpty(matchedItem.Quantity)) {
                                    matchedItem.Quantity = item.Quantity;
                                }

                                else {
                                    matchedItem.Quantity = matchedItem.Quantity + item.Quantity;
                                }

                                // Weight
                                if (AppTool.IsNullOrEmpty(matchedItem.Weight)) {
                                    matchedItem.Weight = item.Weight;
                                }

                                else {
                                    matchedItem.Weight = matchedItem.Weight + item.Weight;
                                }

                                // Volume
                                if (AppTool.IsNullOrEmpty(matchedItem.Width) || AppTool.IsNullOrEmpty(matchedItem.Height) || AppTool.IsNullOrEmpty(matchedItem.Length)) {
                                    matchedItem.Volume = (matchedItem.Weight * this.EntityPM.Ratio) / 1000;
                                    matchedItem.VolumetricWeight = matchedItem.Weight;
                                }

                                else {
                                    matchedItem.Volume = (matchedItem.Width * matchedItem.Height * matchedItem.Length * matchedItem.Quantity) / 1000000;
                                    matchedItem.VolumetricWeight = (matchedItem.Volume * 1000) / this.EntityPM.Ratio;
                                }
                            }

                            else {
                                var newPackage = new ShipmentPackagePM(this.EntityPM);
                                newPackage.ShipmentId = this.EntityPM.Id;
                                newPackage.ClassNumber = item.ClassNumber;
                                newPackage.ContainerNumber = item.ContainerNumber;
                                newPackage.Description = item.Description;
                                newPackage.FlashPoint = item.FlashPoint;
                                newPackage.Harmonize = item.Harmonize;
                                newPackage.Height = item.Height;
                                newPackage.IMDGCode = item.IMDGCode;
                                newPackage.IsContainer = item.IsContainer;
                                newPackage.IsDangerous = item.IsDangerous;
                                newPackage.Length = item.Length;
                                newPackage.MarksAndNumbers = item.MarksAndNumbers;
                                newPackage.MaterialDescription = item.MaterialDescription;
                                newPackage.PackageTypeId = item.PackageTypeId;
                                newPackage.PackageTypeName = item.PackageTypeName;
                                newPackage.PackagingGroup = item.PackagingGroup;
                                newPackage.Quantity = item.Quantity;
                                newPackage.ShipperSeal = item.ShipperSeal;
                                newPackage.CarrierSeal = item.CarrierSeal;
                                newPackage.SOC = item.SOC;
                                newPackage.Tare = item.Tare;
                                newPackage.Temperature = item.Temperature;
                                newPackage.Tenant = item.Tenant;
                                newPackage.UnNumber = item.UnNumber;
                                newPackage.Ventilation = item.Ventilation;
                                newPackage.Volume = item.Volume;
                                newPackage.VolumetricWeight = item.VolumetricWeight;
                                newPackage.Weight = item.Weight;
                                newPackage.Width = item.Width;
                                newPackage.OriginalShipmentPackageId = item.Id;
                                this.EntityPM.AddPackage(newPackage);
                            }
                        });
                    }

                    this.BuildData();
                    this.ComputeTotals();
                }
            }
        });
    }

    // Properties
    get AWBCommodityItemNumber() { return this.EntityPM.AWBCommodityItemNumber; }
    set AWBCommodityItemNumber(newValue: string) {
        if (this.EntityPM.AWBCommodityItemNumber != newValue) {
            this.EntityPM.AWBCommodityItemNumber = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }

    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
            this.Validate();
            this.FireWizardEvent();
        }
    }

    get SLAC() { return this.EntityPM.SLAC; }
    set SLAC(newValue: string) {
        if (this.EntityPM.SLAC != newValue) {
            this.EntityPM.SLAC = newValue;
        }
    }

    get IsDangerous() { return this.EntityPM.IsDangerous; }
    set IsDangerous(newValue: boolean) {
        if (this.EntityPM.IsDangerous != newValue) {
            this.EntityPM.IsDangerous = newValue;
            this.Validate();
            this.FireWizardEvent();

            if (newValue == false) {
                this.EntityPM.DangerousClassNumber = null;
                this.EntityPM.DangerousUnNumber = null;
                this.EntityPM.DangerousPackagingGroup = null;
                this.EntityPM.DangerousIMDGCode = null;
                this.EntityPM.DangerousFlashPoint = null;
                this.EntityPM.DangerousMaterialDescription = null;
            }
        }
    }

    get Volume() { return AppTool.IsNullOrEmpty(this.EntityPM.Volume) ? 0 : this.EntityPM.Volume; }
    set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = AppTool.Round(newValue, 3);
        }
    }

    get VolumetricWeight() { return AppTool.IsNullOrEmpty(this.EntityPM.VolumetricWeight) ? 0 : this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = AppTool.Round(newValue, 3);
        }
    }

    get NumberOfPackages() { return AppTool.IsNullOrEmpty(this.EntityPM.NumberOfPackages) ? 0 : this.EntityPM.NumberOfPackages; }
    set NumberOfPackages(newVaule: number) {
        if (this.EntityPM.NumberOfPackages != newVaule) {
            this.EntityPM.NumberOfPackages = newVaule;
        }
    }

    get GrossWeight() { return AppTool.IsNullOrEmpty(this.EntityPM.GrossWeight) ? 0 : this.EntityPM.GrossWeight; }
    set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = AppTool.Round(newValue, 3);            
            this.Validate();
            this.FireWizardEvent();
        }
    }

    get ChargeableWeight() { return AppTool.IsNullOrEmpty(this.EntityPM.ChargeableWeight) ? 0 : this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            this.EntityPM.ChargeableWeight = AppTool.Round(newValue, 3);
            this.Validate();
            this.ChargeableWeight_Kg();
            this.FireWizardEvent();
            this.ComputeAWBChargeAmount();
        }
    }

    ChargeableWeight_Kg() {
        var weigh_Kg: number = null;
 
        if (this.ChargeableWeight != null) {
            var factorOfConvert: number = 1;
            if (!AppTool.IsNullOrEmpty(this.EntityPM.ChargeableWeightUnitCode)) {
                switch (this.EntityPM.ChargeableWeightUnitCode.toUpperCase()) {
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

    get AWBChargeAmount() { return this.EntityPM.AWBChargeAmount; }
    set AWBChargeAmount(newValue: number) {
        if (this.EntityPM.AWBChargeAmount != newValue) {
            this.EntityPM.AWBChargeAmount = AppTool.Round(newValue, 3);
            this.ComputeAWBFrieghtAmount();
        }
    }

    private ComputeAWBChargeAmount() {
        this.AWBChargeAmount = ShipmentTool.ComputeAWBChargeAmount(this.EntityPM.RateClassCode, this.EntityPM.AWBChargeRate, this.EntityPM.ChargeableWeight);
    }
    private ComputeAWBFrieghtAmount() {
        var computedAmount = this.AWBChargeAmount;
        var totaAmount = this.EntityPM.AWBFreightAmountPrepaid + this.EntityPM.AWBFreightAmountCollect;

        var recompute = true;
        var isPrepaidHasAmount = (this.EntityPM.AWBFreightAmountPrepaid != 0 && this.EntityPM.AWBFreightAmountPrepaid != null);
        var isCollectHasAmount = (this.EntityPM.AWBFreightAmountCollect != 0 && this.EntityPM.AWBFreightAmountCollect != null);

        if (!AppTool.IsNullOrEmpty(this.EntityPM.FreightPrepaidCollectId)) {
            if (isPrepaidHasAmount && isCollectHasAmount && (computedAmount == totaAmount)) {
                recompute = false;
            }
        }

        if (recompute) {
            if (this.EntityPM.FreightPrepaidCollectId == "P") {
                this.EntityPM.AWBFreightAmountCollect = 0;
                this.EntityPM.AWBFreightAmountPrepaid = computedAmount == null ? 0 : computedAmount;
            }

            else if (this.EntityPM.FreightPrepaidCollectId == "C") {
                this.EntityPM.AWBFreightAmountPrepaid = 0;
                this.EntityPM.AWBFreightAmountCollect = computedAmount == null ? 0 : computedAmount;
            }
        }

        //this.FireAWBErrorsEvent();
    }

    private setDigits() {
        this.firstDigit = ",";
        this.secondDigit = ".";
        //switch (SessionLocator.TenantPM.NumberFormatCode) {
        //    case "CD": {
        //        this.firstDigit = ",";
        //        this.secondDigit = ".";
        //        break;
        //    }

        //    case "DC": {
        //        this.firstDigit = ".";
        //        this.secondDigit = ",";
        //        break;
        //    }

        //    case "AD": {
        //        this.firstDigit = "'";
        //        this.secondDigit = ".";
        //        break;
        //    }

        //    default:
        //        {
        //            this.firstDigit = ",";
        //            this.secondDigit = ".";
        //            break;
        //        }
        //}
    }

    GrossWeightLostFocus(input: any) {

        var valueComputed: number = 0;
        var valueInserted: number = 0;

        this.EntityPM.ShipmentPackages.forEach((item) => {
            if (!AppTool.IsNullOrEmpty(item.Weight)) {
                valueComputed += item.Weight;
            }
        });

        // if (!AppTool.IsNullOrEmpty(input)) {
        //     if (this.firstDigit == ".") {
        //         if (!this.GrossWeightPasted) {
        //             input = input.replace(/\./g, '');
        //         }
        //         input = input.replace(/,/g, ".");
        //     }

        //     else if (this.firstDigit == "'") {
        //         input = input.replace(/'/g, '');
        //     }
        //     else {
        //         input = AppTool.Replace(input, ",", "");
        //     }
        //     valueInserted = Number(input);
        // }

        valueComputed = valueComputed == 0 ? null : valueComputed;
        valueInserted = AppTool.GetNumberFromText(input);
        this.EntityPM.GrossWeightEdited = !(valueComputed == valueInserted);
        this.GrossWeight = valueInserted;        
        this.ComputeTotals();
    }
    ChargeableWeightLostFocus(input: any) {

        var valueComputed: number = 0;
        var valueInserted: number = 0;

        valueComputed = AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);

        // if (!AppTool.IsNullOrEmpty(input)) {
        //     if (this.firstDigit == ".") {
        //         if (!this.ChargeableWeightPasted) {
        //             input = input.replace(/\./g, '');
        //         }
        //         input = input.replace(/,/g, ".");
        //     }

        //     else if (this.firstDigit == "'") {
        //         input = input.replace(/'/g, '');
        //     }
        //     else {
        //         input = AppTool.Replace(input, ",", "");
        //     }
        //     valueInserted = Number(input);
        // }

        valueComputed = valueComputed == 0 ? null : valueComputed;
        valueInserted = AppTool.GetNumberFromText(input);
        this.EntityPM.ChargeableWeightEdited = !(valueComputed == valueInserted);
        this.ChargeableWeight = AppTool.RoundChargeableWeight(valueInserted, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
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

        if (this.IsMultipleCommodities) {
            if (this.ItemsSourceOfCommodities.Length == 0) {
                this.EntityPM.NumberOfPackages = null;
                this.EntityPM.GrossWeight = null;
                this.EntityPM.Volume = null;
                this.EntityPM.VolumetricWeight = null;
                this.EntityPM.ChargeableWeight = null;
                this.EntityPM.AWBCommodityItemNumber = null;
                this.EntityPM.GrossWeightEdited = false;
                this.EntityPM.ChargeableWeightEdited = false;
                this.EntityPM.AWBChargeAmount = null;
            }

            else {
                if (this.EntityPM.ShipmentCommodities.length == 0) {
                    this.EntityPM.GrossWeightEdited = false;
                    this.EntityPM.ChargeableWeightEdited = false;
                }

                this.NumberOfPackages = ArrayTool.Sum(this.ItemsSourceOfCommodities.Collection, 'NumberOfPackages');
                this.Volume = ArrayTool.Sum(this.ItemsSourceOfCommodities.Collection, 'Volume');
                this.VolumetricWeight = ArrayTool.Sum(this.ItemsSourceOfCommodities.Collection, 'VolumetricWeight');

                if (!this.EntityPM.GrossWeightEdited) {
                    this.GrossWeight = ArrayTool.Sum(this.ItemsSourceOfCommodities.Collection, 'GrossWeight');
                }

                if (!this.EntityPM.ChargeableWeightEdited) {
                    this.ChargeableWeight = ArrayTool.Sum(this.ItemsSourceOfCommodities.Collection, 'ChargeableWeight');
                }

                this.EntityPM.AWBChargeAmount = ArrayTool.Sum(this.ItemsSourceOfCommodities.Collection, 'ChargeAmount');
            }
        }

        else {
            if (this.EntityPM.ShipmentPackages.length == 0) {
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

                this.EntityPM.ShipmentPackages.forEach((item) => {

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
                this.ChargeableWeight = AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.EntityPM.GrossWeightUnitCode, this.EntityPM.ChargeableWeightUnitCode, this.EntityPM.DirectionId, this.EntityPM.TransportModeId);
            }
        }

        this.SetUIProperties();
        this.Validate();
        this.FireWizardEvent();
        this.ComputeAWBChargeAmount();
    }
    ChooseCommodityClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 775;
        logitudeWindow.Height = 570;
        logitudeWindow.Title = TextCodeTranslator.TranslateTablePlural("Commodity") + " Search";
        logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, FieldName: 'AWBCommodityItemNumber' };
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBChooseCommodityComponent");
        logitudeWindow.WindowClosed.subscribe(s => {
            this.SetUIProperties();
            this.Validate();
            this.FireWizardEvent();
        });
    }
    EditDangerouse() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = TextCodeTranslator.Translate("Shipment.O.Packages.EditDangerousGoods");
        logitudeWindow.WindowArgs = this.EntityPM;
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBDangerousPackageComponent");
    }

    AddLineClicked() {
        if (this.IsMultipleCommodities) {
            this.AddCommodity();
        }

        else {
            this.AddPackage();
        }
    }

    AddPackage() {
        var itemPM = new ShipmentPackagePM(null);
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.Tenant = this.EntityPM.Tenant;
        var itemViewModel = new AWBWizardPackageItem(itemPM, true, this);
        this.RunPackageWindow(itemViewModel, TextCodeTranslator.Translate("ShipmentPackage.O.AddPackage"));
    }
    EditPackage(itemComponent: AWBWizardPackageItem) {
        this.RunPackageWindow(itemComponent, TextCodeTranslator.Translate("ShipmentPackage.O.EditPackage"));
    }
    DeletePackage(itemComponent: AWBWizardPackageItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Shipment.M.DeleteThisPackage"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                if (itemComponent.EntityPM.ShipmentPackageItems != null) {
                    itemComponent.EntityPM.ShipmentPackageItems = [];
                }

                if (itemComponent.EntityPM.InsideShipmentPackages != null) {
                    itemComponent.EntityPM.InsideShipmentPackages = [];
                }

                this.EntityPM.RemovePackage(itemComponent.EntityPM);

                var itemIndex = this.ItemsSource.indexOf(itemComponent);
                if (itemIndex > -1) {
                    this.ItemsSource.splice(itemIndex, 1);
                }

                this.ComputeTotals();                
                this.SetUIProperties();
                this.Validate();
                this.FireWizardEvent();
                this.SetRebuildButton();
                this.SetGenerateButton();                
            }
        });
    }
    RunPackageWindow(itemComponent: AWBWizardPackageItem, windowTitle: string) {
        this._entityResourceService.getEntityResourceByTableName(itemComponent.ObjectTableName).subscribe(response=> {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = windowTitle;
            logitudeWindow.DataContext = itemComponent;
            logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBAddEditPackageComponent");
        });
    }

    AddCommodity() {
        var validationResult: string = ShipmentTool.ValidateAddingCommoditiesCount(this.EntityPM);

        if (AppTool.IsNullOrEmpty(validationResult)) {
            var itemPM = new ShipmentCommodityPM(null);
            itemPM.Tenant = SessionLocator.Tenant
            itemPM.ShipmentId = this.EntityPM.Id;
            itemPM.RateClassCode = "Q";
            var itemViewModel = new ShipmentCommodityItem(itemPM, true, this);
            this.RunCommodityWindow(itemViewModel, "Add Commodity line");
        }

        else {
            var messageWindow = new MessageWindow();
            messageWindow.Show(validationResult);
        }
    }
    EditCommodityClicked(itemComponent: ShipmentCommodityItem) {
        this.RunCommodityWindow(itemComponent, "Edit Commodity line");
    }
    DeleteCommodityClicked(itemComponent: ShipmentCommodityItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Commodity?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                if (itemComponent.EntityPM.CommodityPackages != null) {
                    itemComponent.EntityPM.CommodityPackages = [];
                }

                this.EntityPM.RemoveCommodity(itemComponent.EntityPM);

                this.BuildData();

                this.ComputeTotals();
                this.SetUIProperties();
                this.Validate();
                this.FireWizardEvent();
                this.SetRebuildButton();
                this.SetGenerateButton();
            }
        });
    }
    RunCommodityWindow(itemComponent: ShipmentCommodityItem, windowTitle: string) {
        this._entityResourceService.getEntityResourceByTableName(itemComponent.ObjectTableName).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = windowTitle;
            logitudeWindow.DataContext = itemComponent;
            logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBAddEditCommodityComponent");
        });
    }

    // Single | Multiple Commodities
    // Commodities
    public SelectedRow: ShipmentCommodityItem = null;
    OnRowSelected(itemComponent: ShipmentCommodityItem) {
        this.SelectedRow = itemComponent;
    }
    OnRowLoaded(Row: any) {

        var isExpandaple = false;

        if (Row) {
            var item: ShipmentCommodityItem = Row.rowData;
            if (item) {
                if (item.EntityPM.CommodityPackages.length > 0) {
                    isExpandaple = true;
                }
            }

            Row.SetExpandaple(isExpandaple);
        }
    }

    SetMultipleCommodities(newValue: boolean) {
        this.IsMultipleCommodities = newValue;
    }
    get IsMultipleCommodities() { return this.EntityPM.IsMultipleCommodities; }
    set IsMultipleCommodities(newValue: boolean) {
        if (this.EntityPM.IsMultipleCommodities != newValue) {
            this.EntityPM.IsMultipleCommodities = newValue;
            this.SetUIProperties();
            this.ChangeDataStructure();
            this.BuildData();
        }
    }
    private ChangeDataStructure() {
        this.Validate();
        this.FireWizardEvent();


        //  Order by - Sort Id
        // var myBaseCommodityPM: ShipmentCommodityPM = this.EntityPM.ShipmentCommodities.filter(d => d.IsFirstLine).OrderBy(d => d.Id)[0];

        var baseCommodityPM: ShipmentCommodityPM = this.EntityPM.ShipmentCommodities.filter(d => d.IsFirstLine)[0];
        if (!baseCommodityPM) {
            baseCommodityPM = new ShipmentCommodityPM(this.EntityPM);
            baseCommodityPM.Tenant = SessionLocator.Tenant;
            baseCommodityPM.ShipmentId = this.EntityPM.Id;
            baseCommodityPM.DescriptionOfGoods = this.EntityPM.DescriptionOfGoods;
            baseCommodityPM.CommodityNumber = this.EntityPM.AWBCommodityItemNumber;
            baseCommodityPM.RateClassCode = this.EntityPM.RateClassCode;
            baseCommodityPM.ChargeRate = this.EntityPM.AWBChargeRate;
            baseCommodityPM.GrossWeight = this.EntityPM.GrossWeight;
            baseCommodityPM.Volume = this.EntityPM.Volume;
            baseCommodityPM.VolumetricWeight = this.EntityPM.VolumetricWeight;
            baseCommodityPM.NumberOfPackages = this.EntityPM.NumberOfPackages;
            baseCommodityPM.ChargeableWeight = this.EntityPM.ChargeableWeight;
            baseCommodityPM.ChargeAmount = this.EntityPM.AWBChargeAmount;
            baseCommodityPM.IsFirstLine = true;
        }

        if (this.IsMultipleCommodities) {

            baseCommodityPM.CommodityPackages = [];

            this.EntityPM.ShipmentPackages.forEach((item: ShipmentPackagePM) => {
                var newItem: CommodityPackagePM = new CommodityPackagePM(baseCommodityPM);
                newItem.ShipmentId = item.ShipmentId;
                newItem.ShipmentNumber = item.ShipmentNumber;
                newItem.CommodityId = baseCommodityPM.Id;
                newItem.Tenant = item.Tenant;
                newItem.Description = item.Description;
                newItem.Height = item.Height;
                newItem.Length = item.Length;
                newItem.PackageTypeId = item.PackageTypeId;
                newItem.Quantity = item.Quantity;
                newItem.Volume = item.Volume;
                newItem.VolumetricWeight = item.VolumetricWeight;
                newItem.Weight = item.Weight;
                newItem.Width = item.Width;
                baseCommodityPM.AddCommodityPackagePM(newItem);
            });

            baseCommodityPM.DescriptionOfGoods = this.EntityPM.DescriptionOfGoods;
            baseCommodityPM.CommodityNumber = this.EntityPM.AWBCommodityItemNumber;
            baseCommodityPM.RateClassCode = this.EntityPM.RateClassCode;
            baseCommodityPM.ChargeRate = this.EntityPM.AWBChargeRate;
            baseCommodityPM.GrossWeight = this.EntityPM.GrossWeight;
            baseCommodityPM.Volume = this.EntityPM.Volume;
            baseCommodityPM.VolumetricWeight = this.EntityPM.VolumetricWeight;
            baseCommodityPM.ChargeableWeight = this.EntityPM.ChargeableWeight;
            baseCommodityPM.NumberOfPackages = this.EntityPM.NumberOfPackages;
            baseCommodityPM.ChargeAmount = this.EntityPM.AWBChargeAmount;

            this.EntityPM.RateClassCode = null;
            this.EntityPM.AWBChargeRate = null;
            this.EntityPM.AWBCommodityItemNumber = null;
            this.EntityPM.DescriptionOfGoods = null;
            this.EntityPM.ShipmentPackages = [];
            this.EntityPM.ShipmentCommodities = [];
            this.EntityPM.AddCommodity(baseCommodityPM);
        }

        else {
            var list: ShipmentPackagePM[] = [];

            this.EntityPM.ShipmentCommodities.forEach((item: ShipmentCommodityPM) => {
                item.CommodityPackages.forEach((child: CommodityPackagePM) => {
                    var newItem: ShipmentPackagePM = new ShipmentPackagePM(this.EntityPM);
                    newItem.ShipmentId = child.ShipmentId;
                    newItem.ShipmentNumber = child.ShipmentNumber;
                    newItem.CommodityId = null;
                    newItem.Tenant = child.Tenant;
                    newItem.Description = child.Description;
                    newItem.Height = child.Height;
                    newItem.Length = child.Length;
                    newItem.PackageTypeId = child.PackageTypeId;
                    newItem.Quantity = child.Quantity;
                    newItem.Volume = child.Volume;
                    newItem.VolumetricWeight = child.VolumetricWeight;
                    newItem.Weight = child.Weight;
                    newItem.Width = child.Width;
                    list.push(newItem);
                });
            });

            this.EntityPM.RateClassCode = baseCommodityPM.RateClassCode;
            this.EntityPM.AWBChargeRate = baseCommodityPM.ChargeRate;
            this.EntityPM.AWBCommodityItemNumber = baseCommodityPM.CommodityNumber;
            this.EntityPM.DescriptionOfGoods = baseCommodityPM.DescriptionOfGoods;
            this.EntityPM.ShipmentPackages = [];
            this.EntityPM.ShipmentCommodities = [];
            this.EntityPM.AddCommodity(baseCommodityPM);

            list.forEach(item => {
                this.EntityPM.AddPackage(item);
            });
        }
    }

    public BuildData() {
        this.ItemsSource = [];
        this.ItemsSourceOfCommodities.Clear();

        if (this.IsMultipleCommodities) {
            var itemsCollection: ShipmentCommodityItem[] = [];

            this.EntityPM.ShipmentCommodities.forEach((item) => {
                itemsCollection.push(new ShipmentCommodityItem(item, false, this));
            });

            this.ItemsSourceOfCommodities.InsertCollection(itemsCollection);
        }

        else {
            var list: ShipmentPackagePM[] = new Array<ShipmentPackagePM>();
            this.EntityPM.ShipmentPackages.forEach((item) => {
                list.push(item);
            });

            if (this.EntityPM.ShipmentLevelCode != "C") {
                if (list.length < 5) {
                    for (var i = list.length; i < 5; i++) {

                        var item: ShipmentPackagePM = new ShipmentPackagePM(null);
                        item.Tenant = this.EntityPM.Tenant;
                        item.ShipmentId = this.EntityPM.Id;
                        item.IsAWBWizardDefault = true;
                        list.push(item);
                    }
                }
            }

            list./*sort((a, b) => { return (a === b) ? 0 : a ? 1 : 1 }).*/forEach((item) => {
                var itemViewModel: AWBWizardPackageItem = new AWBWizardPackageItem(item, false, this);
                this.ItemsSource.push(itemViewModel);
                itemViewModel.SetUIProperties();
            })
        }

        this.SetRebuildButton();
        this.SetGenerateButton();
    }
}
export class AWBWizardPackageItem extends BaseComponent {
    public DataContext: AWBWizardPackageItem = this;
    public EntityPM: ShipmentPackagePM;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "ShipmentPackage";
    public IsNewEntity: boolean = false;
    public IsWindowMode: boolean = false;
    constructor(entityPM: ShipmentPackagePM, isNew: boolean, public fatherComponent: AWBPackagesTabComponent) {
        super();
        this.EntityPM = entityPM;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.SetUIProperties();
    }

    public IsEditingEnabled: boolean = false;
    public SetUIProperties() {

        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        if (this.IsEditingEnabled) {
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

        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled );
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isFieldEnabled);
    }

    private hasValue: boolean;
    public HasValue(hasValue: boolean) {
        this.hasValue = hasValue;
        this.SetUIProperties();
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = AppTool.Round(newValue, 0);

            var itemIndex = this.ShipmentPM.ShipmentPackages.indexOf(this.EntityPM);

            if (AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                this.Height = null;
                this.Length = null;
                this.Width = null;
                this.Volume = null;
                this.VolumetricWeight = null;
                this.Weight = null;

                if (!this.IsWindowMode) {
                    if (itemIndex > -1) {
                        this.ShipmentPM.RemovePackage(this.EntityPM);
                    }
                }
            }

            else {
                if (!this.IsWindowMode) {
                    if (itemIndex == -1) {
                        this.ShipmentPM.AddPackage(this.EntityPM);
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

            this.fatherComponent.ResetTotalEditedValues();
            this.fatherComponent.ComputeTotals();
        }
    }

    OnGrossWeightLostFocus(input1: number) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);

                this.SetUIProperties();
                this.fatherComponent.ResetTotalEditedValues();
                this.fatherComponent.ComputeTotals();
            }
        }
    }

    private ComputeVolume() {

        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    }
    private ComputeVolumetricWeight() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    }
}
export class ShipmentCommodityItem extends BaseComponent {
    public DataContext: ShipmentCommodityItem = this;
    public EntityPM: ShipmentCommodityPM;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "ShipmentCommodity";
    public IsNewEntity: boolean = false;
    public IsWindowMode: boolean = false;
    public ItemsSource: CommodityPackageItem[] = [];
    constructor(entityPM: ShipmentCommodityPM, isNew: boolean, public fatherComponent: AWBPackagesTabComponent) {
        super();
        this.EntityPM = entityPM;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.SetUIProperties();
        this.BuildItemsSource();
    }

    IsEditingEnabled: boolean = false;
    SetUIProperties() {

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        var isFieldEnabled = false;
        if (this.IsEditingEnabled) {
            if (this.ItemsSource.length == 0) {
                isFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("ChargeAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("NumberOfPackages", this.ObjectTableName, isFieldEnabled);

        this.SetRateClassUIProperties();
    }
    SetRateClassUIProperties() {
        var rateClassGroupCode: string = ShipmentTool.GetRateClassGroupCode(this.RateClassCode);

        if (rateClassGroupCode == "S") {
            this.ChargeRate = null;
            this.UIProperties.SetEnabled("AWBChargeRate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AWBChargeAmount", this.ObjectTableName, true);

        }

        else {
            this.UIProperties.SetEnabled("AWBChargeRate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("AWBChargeAmount", this.ObjectTableName, false);
        }
    }

    get IsFirstLine() { return this.EntityPM.IsFirstLine; }
    set IsFirstLine(value: boolean) {
        if (this.EntityPM.IsFirstLine != value) {
            this.EntityPM.IsFirstLine = value;
        }
    }

    get CommodityNumber() { return this.EntityPM.CommodityNumber; }
    set CommodityNumber(value: string) {
        if (this.EntityPM.CommodityNumber != value) {
            this.EntityPM.CommodityNumber = value;
            this.fatherComponent.FireWizardEvent();
        }
    }

    get RateClassCode() { return this.EntityPM.RateClassCode; }
    set RateClassCode(value: string) {
        if (this.EntityPM.RateClassCode != value) {
            this.EntityPM.RateClassCode = value;
            this.ComputeAWBChargeAmount();
            this.SetRateClassUIProperties();
            this.fatherComponent.FireWizardEvent();
        }
    }

    get ChargeableWeight() { return this.EntityPM.ChargeableWeight; }
    set ChargeableWeight(value: number) {
        if (this.EntityPM.ChargeableWeight != value) {
            this.EntityPM.ChargeableWeight = AppTool.Round(value, 3);
            this.ComputeAWBChargeAmount();
            this.fatherComponent.ComputeTotals();
        }
    }

    get ChargeRate() { return this.EntityPM.ChargeRate; }
    set ChargeRate(value: number) {
        if (this.EntityPM.ChargeRate != value) {
            this.EntityPM.ChargeRate = AppTool.Round(value, 3);
            this.fatherComponent.FireWizardEvent();
            this.ComputeAWBChargeAmount();
        }
    }

    private ComputeAWBChargeAmount() {
        this.ChargeAmount = ShipmentTool.ComputeAWBChargeAmount(this.RateClassCode, this.ChargeRate, this.ChargeableWeight);
    }

    get ChargeAmount() { return this.EntityPM.ChargeAmount; }
    set ChargeAmount(value: number) {
        if (this.EntityPM.ChargeAmount != value) {
            this.EntityPM.ChargeAmount = AppTool.Round(value, 3);
            this.ComputeAWBFrieghtAmount();
        }
    }

    private ComputeAWBFrieghtAmount() {
        var computedAmount = this.ChargeAmount;
        var totaAmount = this.ShipmentPM.AWBFreightAmountPrepaid + this.ShipmentPM.AWBFreightAmountCollect;

        var recompute = true;
        var isPrepaidHasAmount = (this.ShipmentPM.AWBFreightAmountPrepaid != 0 && this.ShipmentPM.AWBFreightAmountPrepaid != null);
        var isCollectHasAmount = (this.ShipmentPM.AWBFreightAmountCollect != 0 && this.ShipmentPM.AWBFreightAmountCollect != null);

        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.FreightPrepaidCollectId)) {
            if (isPrepaidHasAmount && isCollectHasAmount && (computedAmount == totaAmount)) {
                recompute = false;
            }
        }

        if (recompute) {
            if (this.ShipmentPM.FreightPrepaidCollectId == "P") {
                this.ShipmentPM.AWBFreightAmountCollect = 0;
                this.ShipmentPM.AWBFreightAmountPrepaid = computedAmount == null ? 0 : computedAmount;
            }

            else if (this.ShipmentPM.FreightPrepaidCollectId == "C") {
                this.ShipmentPM.AWBFreightAmountPrepaid = 0;
                this.ShipmentPM.AWBFreightAmountCollect = computedAmount == null ? 0 : computedAmount;
            }
        }

        this.fatherComponent.FireWizardEvent();
    }

    get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    set DescriptionOfGoods(value: string) {
        if (this.EntityPM.DescriptionOfGoods != value) {
            this.EntityPM.DescriptionOfGoods = value;
        }
    }

    get Volume() { return this.EntityPM.Volume; }
    set Volume(value: number) {
        if (this.EntityPM.Volume != value) {
            this.EntityPM.Volume = AppTool.Round(value, 3);
            this.fatherComponent.ComputeTotals();
        }
    }

    get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    set VolumetricWeight(value: number) {
        if (this.EntityPM.VolumetricWeight != value) {
            this.EntityPM.VolumetricWeight = AppTool.Round(value, 3);
            this.fatherComponent.ComputeTotals();
        }
    }

    get GrossWeight() { return this.EntityPM.GrossWeight; }
    set GrossWeight(value: number) {
        if (this.EntityPM.GrossWeight != value) {
            this.EntityPM.GrossWeight = AppTool.Round(value, 3);
            this.fatherComponent.ComputeTotals();
        }
    }

    get NumberOfPackages() { return this.EntityPM.NumberOfPackages; }
    set NumberOfPackages(value: number) {
        if (this.EntityPM.NumberOfPackages != value) {
            this.EntityPM.NumberOfPackages = value;
            this.fatherComponent.ComputeTotals();
        }
    }

    BuildItemsSource() {

        this.ItemsSource = [];

        this.EntityPM.CommodityPackages.forEach((item: CommodityPackagePM) => {
            this.ItemsSource.push(new CommodityPackageItem(item, this, false));
        });
    }

    ComputeTotals() {
        if (this.EntityPM.CommodityPackages.length == 0) {
            this.EntityPM.Volume = null;
            this.EntityPM.GrossWeight = null;
            this.EntityPM.VolumetricWeight = null;
            this.EntityPM.ChargeableWeight = null;
            this.EntityPM.NumberOfPackages = null;
        }

        else {
            this.EntityPM.NumberOfPackages = ArrayTool.Sum(this.EntityPM.CommodityPackages, 'Quantity');
            this.EntityPM.Volume = ArrayTool.Sum(this.EntityPM.CommodityPackages, 'Volume');
            this.EntityPM.VolumetricWeight = ArrayTool.Sum(this.EntityPM.CommodityPackages, 'VolumetricWeight');
            this.EntityPM.GrossWeight = ArrayTool.Sum(this.EntityPM.CommodityPackages, 'Weight');
            this.EntityPM.ChargeableWeight = AppTool.CalculateChargeableWeight(this.EntityPM.GrossWeight, this.EntityPM.VolumetricWeight, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId);
        }

        if (this.EntityPM.Volume == 0) {
            this.EntityPM.Volume = null;
        }

        if (this.EntityPM.GrossWeight == 0) {
            this.EntityPM.GrossWeight = null;
        }

        if (this.EntityPM.VolumetricWeight == 0) {
            this.EntityPM.VolumetricWeight = null;
        }

        if (this.EntityPM.ChargeableWeight == 0) {
            this.EntityPM.ChargeableWeight = null;
        }

        if (this.EntityPM.NumberOfPackages == 0) {
            this.EntityPM.NumberOfPackages = null;
        }

        this.SetUIProperties();
        this.ComputeAWBChargeAmount();
        this.fatherComponent.ComputeTotals();
    }

    AddCommodityPackageClicked() {
        var newItem: CommodityPackagePM = new CommodityPackagePM(null);
        newItem.ShipmentId = this.ShipmentPM.Id;
        newItem.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
        newItem.CommodityId = this.EntityPM.Id;
        newItem.Tenant = SessionLocator.Tenant;
        var itemComponent = new CommodityPackageItem(newItem, this, true);
        this.RunCommodityPackageWindow(itemComponent, "Add Commodity Package");

    }
    EditCommodityPackageClicked(itemComponent: CommodityPackageItem) {
        this.RunCommodityPackageWindow(itemComponent, "Edit Commodity Package");
    }
    DeleteCommodityPackageClicked(itemComponent: CommodityPackageItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Package?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                this.EntityPM.RemoveCommodityPackagePM(itemComponent.EntityPM);

                this.BuildItemsSource();
                this.ComputeTotals();

                if (this.EntityPM.CommodityPackages.length == 0) {
                    this.fatherComponent.BuildData();
                }
            }
        });
    }
    RunCommodityPackageWindow(itemComponent: CommodityPackageItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/Packages/AWBAddEditCommodityPackageComponent");
    }

}
export class CommodityPackageItem extends BaseComponent {
    public DataContext: CommodityPackageItem = this;
    public EntityPM: CommodityPackagePM;
    public ShipmentPM: ShipmentPM;
    public CommodityPM: ShipmentCommodityPM;
    public ObjectTableName: string = "ShipmentPackage";
    public IsNewEntity: boolean = false;
    public IsWindowMode: boolean = false;
    constructor(entityPM: CommodityPackagePM, public fatherComponent: ShipmentCommodityItem, isNew: boolean) {
        super();

        this.EntityPM = entityPM;
        this.ShipmentPM = fatherComponent.ShipmentPM;
        this.CommodityPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.SetUIProperties();
    }

    private hasValue: boolean;
    public HasValue(hasValue: boolean) {
        this.hasValue = hasValue;
        this.SetUIProperties();
    }

    public IsEditingEnabled: boolean = false;
    public SetUIProperties() {

        var isFieldEnabled = false;
        var isVolumeEnabled = false;
        var isDimensionEnabled = false;

        this.IsEditingEnabled = this.fatherComponent.IsEditingEnabled;

        if (this.IsEditingEnabled) {
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

        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("VolumetricWeight", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Length", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Width", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Height", this.ObjectTableName, isDimensionEnabled);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isVolumeEnabled);
        this.UIProperties.SetEnabled("Weight", this.ObjectTableName, isFieldEnabled);
    }

    get Quantity() { return this.EntityPM.Quantity; }
    set Quantity(newValue: number) {
        if (this.EntityPM.Quantity != newValue) {
            this.EntityPM.Quantity = AppTool.Round(newValue, 0);

            var itemIndex = this.fatherComponent.EntityPM.CommodityPackages.indexOf(this.EntityPM);
            
            if (AppTool.IsNullOrZero(this.EntityPM.Quantity)) {
                this.Height = null;
                this.Length = null;
                this.Width = null;
                this.Volume = null;
                this.VolumetricWeight = null;
                this.Weight = null;

                if (!this.IsWindowMode) {
                    if (itemIndex > -1) {
                        this.fatherComponent.EntityPM.RemoveCommodityPackagePM(this.EntityPM);
                    }
                }
            }

            else {
                if (!this.IsWindowMode) {
                    if (itemIndex == -1) {
                        this.fatherComponent.EntityPM.AddCommodityPackagePM(this.EntityPM);
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

            this.fatherComponent.ComputeTotals();
        }
    }

    OnGrossWeightLostFocus(input1: number) {
        if (AppTool.IsNullOrEmpty(this.EntityPM.Volume)) {
            if (this.Width == null || this.Height == null || this.Length == null) {
                this.EntityPM.VolumetricWeight = AppTool.GetWeightFromWeight(this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode, this.EntityPM.Weight);
                this.EntityPM.Volume = AppTool.GetVolumeFromWeight(this.ShipmentPM.ChargeableWeightUnitCode, this.ShipmentPM.VolumeUnitCode, this.EntityPM.VolumetricWeight, this.ShipmentPM.Ratio);

                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
            }
        }
    }

    private ComputeVolume() {

        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.Volume = AppTool.ComputePackageVolume(this.Quantity, this.Width, this.Height, this.Length, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode);
    }
    private ComputeVolumetricWeight() {
        if (this.ShipmentPM.Ratio == null) {
            this.ShipmentPM.Ratio = AppTool.GetRatio(this.ShipmentPM.DirectionId, this.ShipmentPM.TransportModeId, this.ShipmentPM.ShipmentTypeId, SessionLocator.TenantPM.CountryCode);
        }

        this.VolumetricWeight = AppTool.ComputePackageVolumetricWeight(this.Quantity, this.Width, this.Height, this.Length, this.Volume, this.Weight, this.ShipmentPM.Ratio, this.ShipmentPM.DimensionsUnitCode, this.ShipmentPM.VolumeUnitCode, this.ShipmentPM.GrossWeightUnitCode, this.ShipmentPM.ChargeableWeightUnitCode);
    }
}
