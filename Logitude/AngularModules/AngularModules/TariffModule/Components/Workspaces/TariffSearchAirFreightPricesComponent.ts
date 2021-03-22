import { Component } from '@angular/core';
import { AppTool, DateTool, ArrayTool } from '../../../Infrastructure/Tools';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TariffDomainService, TariffSearchSummary, TariffSearchArgs } from '../../../TariffModule/Services/TariffDomainService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { CurrencyList } from '../../../Common/EntityLists/CurrencyList';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { ShipmentPM } from '../../../Shipment/EntityPMs/ShipmentPM';
import { QuoteChargePM } from '../../../Quote/EntityPMs/QuoteChargePM';
import { QuoteChargeItem } from '../../../QuoteModules/QuoteCharges/Components/LCLChargesComponent';
import { FCLQuoteChargeItem } from '../../../QuoteModules/QuoteCharges/Components/FCLChargesComponent';
import { ShipmentPayablePM } from '../../../Shipment/EntityPMs/ShipmentPayablePM';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ShipmentGenerator, ByPckageType, ShipmentTool } from '../../../Shipment/Tools';
import { ChargesTypeList } from '../../../Common/EntityLists/ChargesTypeList';
import { ChargesTypeListService } from '../../../Common/Services/StandardLists/ChargesTypeListService';
import { ShipmentPayableItem } from '../../../ShipmentModules/ShipmentTabs/Components/Payables/PayablesTabComponent';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { PackageTypeList } from '../../../Common/EntityLists/PackageTypeList';
import { PackageTypeListService } from '../../../Common/Services/StandardLists/PackageTypeListService';
import { TariffProductListService } from '../../Services/StandardLists/TariffProductListService';
import { TariffProductList } from '../../EntityLists/TariffProductList';
import { TariffSettingPM } from '../../EntityPMs/TariffSettingPM';
import { QuoteChargesBehaviours } from '../../../QuoteModules/QuoteCharges/Behaviours/QuoteChargesBehaviours';

@Component({

    templateUrl: './TariffSearchAirFreightPricesComponent.html',
})

export class TariffSearchAirFreightPricesComponent extends BaseComponent {
    public ObjectTableName = "TariffLine";
    public DataContext: TariffSearchAirFreightPricesComponent = this;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    private myDomainService: TariffDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    public AvailableTariffs: Array<TariffSearchSummary> = [];
    public IsGeneratePayablesVisible: boolean = false;
    private IsShipment: boolean;
    private IsQuote: boolean;
    private FatherComponent: any;
    private myChargesTypeListService: ChargesTypeListService;
    private dimenstionShipment: ShipmentPM;
    public IsPickedFromWizard: boolean = false;
    public TariffType: string;
    public FreightLabel: string;
    public OriginDependencyFilterValue: string = "A";
    public DestinationDependencyFilterValue = "A";
    private packageTypeListService: PackageTypeListService;
    public IsFirstTime: boolean = true;
    public IsViewSurchargesClickedEnabled: boolean = true;
    public PortDisplayMemberPath = "Code";

    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myDomainService = new TariffDomainService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.SetUIProperties();
        this.Date = DateTool.GetCurrentDateAsUtc();        
        this.GetTariffProducts();

        this.dimenstionShipment = new ShipmentPM();
        this.packageTypeListService = new PackageTypeListService();
        this.GetTariffSettings();
    }

    private ContainerDefaults = "";
    private GetTariffSettings() {
        var tariffDomainService = new TariffDomainService();
        tariffDomainService.GetTenantTariffSetting().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var tariffSetting: TariffSettingPM = myResponse.Result;

                if (tariffSetting != null) {
                    this.ContainerDefaults = tariffSetting.ContainerDefaults;
                    this.CurrencyId = tariffSetting.DefaultCurrencyId;                    
                }

                this.LoadContainers();
                this.CalculateDefaultCurrency();
            }
        });
    }

    private allPackageTypes: PackageTypeList[];
    private LoadContainers() {
        this.packageTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.allPackageTypes = myResponse.Result;
                this.FillContainersIds();
            }
        });
    }

    private FillContainersIds() {
        if (!this.IsShipment && !this.IsQuote) {
            if (!AppTool.IsNullOrEmpty(this.ContainerDefaults)) {
                var containersArray: string[] = this.ContainerDefaults.split(',');

                if (containersArray.length > 0) {
                    var index: number = 1;

                    containersArray.forEach(item => {
                        var packageType: PackageTypeList = this.allPackageTypes.filter(d => d.Code == item.trim())[0];
                        if (packageType != null) {
                            this['ContainerType' + index + 'Id'] = packageType.Id;
                        }

                        index++;
                    });
                }
            }
        }
    }

    private GetTariffProducts() {
        var service: TariffProductListService = new TariffProductListService();
        service.getAllFromCache().subscribe((res: any) => {
            if (!res.HasError) {
                if (res.Result) {
                    var myResult: TariffProductList[] = res.Result;

                    var generalProduct = myResult.filter(d => d.Code == "GEN")[0];
                    this.TariffProductId = generalProduct == null ? null : generalProduct.Id;
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    private CalculateDefaultCurrency() {
        if (AppTool.IsNullOrEmpty(this.CurrencyId)) {
            var CurrencyList: CurrencyList[] = [];
            var myService: CurrencyListService = new CurrencyListService();
            myService.getAllFromCache().subscribe((myResult: ServiceResponse) => {
                if (myResult) {
                    CurrencyList = myResult.Result;
                    var usdCurrency = CurrencyList.filter(c => c.Code == "USD" && c.Tenant == SessionLocator.Tenant)[0];
                    if (usdCurrency != null) {
                        this.CurrencyId = usdCurrency.Id;
                    }
                }
            });
        }
    }
    SetPortsDependencyFilterValue() {
        if (this.TariffType == "OLC" || this.TariffType == "OFC") {
            this.OriginDependencyFilterValue = "O";
            this.DestinationDependencyFilterValue = "O";
            this.PortDisplayMemberPath = "CombinedCode";

        }
    }
    private currencyId: string;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(newValue: string) {
        if (this.currencyId != newValue) {
            this.currencyId = newValue;
        }
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            var isAutorun = false;
            if (args['IsShipment'] || args['IsQuote']) {
                this.IsGeneratePayablesVisible = true;
                isAutorun = true;
                if (args['IsShipment']) {
                    this.IsShipment = args['IsShipment'];
                } else {
                    this.IsQuote = args['IsQuote'];
                }
            }
            if (args['FatherComponent']) {
                this.FatherComponent = args['FatherComponent'];
            }
            if (args['FromPort']) {
                this.originPortId = args['FromPort'];
            }
            if (args['ToPort']) {
                this.destinationPortId = args['ToPort'];
            }
            if (args['BetweenDate']) {
                this.date = args['BetweenDate'];
            }
            if (args['ChargeableWeight']) {
                this.weight = args['ChargeableWeight'];
                this.ChargeableWeight = args['ChargeableWeight'];
            }
            if (args['ChargeableWeightUnit']) {
                this.weightCode = args['ChargeableWeightUnit'];
            }
            if (args['GrossWeight']) {
                this.GrossWeight = args['GrossWeight'];
            }
            if (args['GrossWeightUnit']) {
                this.GrossWeightCode = args['GrossWeightUnit'];
            }
            if (args['Volume']) {
                this.volume = args['Volume'];
            }
            if (args['VolumeUnit']) {
                this.volumeUnitCode = args['VolumeUnit'];
            }
            if (args['TariffType']) {
                this.TariffType = args['TariffType'];
            }
            this.SetLabels();
            this.SetUIProperties();
            this.SetPortsDependencyFilterValue();
            this.SetContainersInitialValues();
            if (isAutorun) {
                this.SearchButtonClicked();
            }
        }
    }

    SetContainersInitialValues(): any {
        if (this.TariffType == "OFC") {
            if (this.IsShipment) {
                this.SetContainersInitialValues_Shipments();
            }
            else {
                this.SetContainersInitialValues_Quotes();
            }
        }
    }
    private SetContainersInitialValues_Shipments() {
        this.BCNTGrouped = ShipmentTool.GetByPckageTypeGrouped(this.FatherComponent.EntityPM);
        this.ContainerType1Id = this.BCNTGrouped[0] != null ? this.BCNTGrouped[0].PackageTypeId : null;
        this.ContainerType2Id = this.BCNTGrouped[1] != null ? this.BCNTGrouped[1].PackageTypeId : null;
        this.ContainerType3Id = this.BCNTGrouped[2] != null ? this.BCNTGrouped[2].PackageTypeId : null;
        this.ContainerType4Id = this.BCNTGrouped[3] != null ? this.BCNTGrouped[3].PackageTypeId : null;
        this.ContainerType5Id = this.BCNTGrouped[4] != null ? this.BCNTGrouped[4].PackageTypeId : null;
        this.Quantity1 = this.BCNTGrouped[0] != null ? this.BCNTGrouped[0].Quantity : null;
        this.Quantity2 = this.BCNTGrouped[1] != null ? this.BCNTGrouped[1].Quantity : null;
        this.Quantity3 = this.BCNTGrouped[2] != null ? this.BCNTGrouped[2].Quantity : null;
        this.Quantity4 = this.BCNTGrouped[3] != null ? this.BCNTGrouped[3].Quantity : null;
        this.Quantity5 = this.BCNTGrouped[4] != null ? this.BCNTGrouped[4].Quantity : null;
    }
    private SetContainersInitialValues_Quotes() {
        this.ContainerType1Id = this.FatherComponent.EntityPM.PackageType1Id;
        this.ContainerType2Id = this.FatherComponent.EntityPM.PackageType2Id;
        this.ContainerType3Id = this.FatherComponent.EntityPM.PackageType3Id;
        this.ContainerType4Id = this.FatherComponent.EntityPM.PackageType4Id;
        this.ContainerType5Id = this.FatherComponent.EntityPM.PackageType5Id;
        this.Quantity1 = this.FatherComponent.EntityPM.PackageType1Quantity;
        this.Quantity2 = this.FatherComponent.EntityPM.PackageType2Quantity;
        this.Quantity3 = this.FatherComponent.EntityPM.PackageType3Quantity;
        this.Quantity4 = this.FatherComponent.EntityPM.PackageType4Quantity;
        this.Quantity5 = this.FatherComponent.EntityPM.PackageType5Quantity;
    }
    private SetLabels() {
        this.FreightLabel = "Air Freight";
        if (this.TariffType == "OLC") {
            this.FreightLabel = "Ocean Freight";
        }
        else if (this.TariffType == "OFC") {
            this.FreightLabel = "Ocean FCL";
        }
    }

    private originPortId: string;
    get OriginPortId() {
        return this.originPortId;
    }
    set OriginPortId(value: string) {
        if (this.originPortId != value) {
            this.originPortId = value;
            this.SetUIProperties();
        }
    }

    private destinationPortId: string;
    get DestinationPortId() {
        return this.destinationPortId;
    }
    set DestinationPortId(value: string) {
        if (this.destinationPortId != value) {
            this.destinationPortId = value;
            this.SetUIProperties();
        }
    }

    private date: Date;
    get Date() {
        return this.date;
    }
    set Date(value: Date) {
        if (this.date != value) {
            this.date = value;
            this.SetUIProperties();
        }
    }

    private weightCode: string = "KG";
    get WeightCode() {
        return this.weightCode;
    }
    set WeightCode(value: string) {
        if (this.weightCode != value) {
            this.weightCode = value;
            //  this.ComputeChargeableWeight_Kg();
            this.ComputeVolume();
            this.ComputeVolumetricWeight();
            this.SetUIProperties();

        }
    }

    private grossWeightCode: string = "KG";
    get GrossWeightCode() {
        return this.grossWeightCode;
    }
    set GrossWeightCode(value: string) {
        if (this.grossWeightCode != value) {
            this.grossWeightCode = value;
            this.ComputeVolume();
            this.ComputeVolumetricWeight();
            this.SetUIProperties();

        }
    }

    private Ratio: number = 6.00;

    private volumeUnitCode: string = "CBM";
    get VolumeUnitCode() {
        return this.volumeUnitCode;
    }
    set VolumeUnitCode(value: string) {
        if (this.volumeUnitCode != value) {
            this.volumeUnitCode = value;
            this.ComputeVolume();
            this.ComputeVolumetricWeight();
            this.SetUIProperties();

        }
    }

    private chargeableWeight: number;
    get ChargeableWeight() {
        return this.chargeableWeight;
    }
    set ChargeableWeight(value: number) {
        if (this.chargeableWeight != value) {
            this.chargeableWeight = value;
            this.ComputeChargeableWeight_Kg();
        }
    }

    private weight: number;
    get Weight() {
        return this.weight;
    }
    set Weight(value: number) {
        if (this.weight != value) {
            this.weight = value;
            this.ChargeableWeight = value;
            this.SetUIProperties();
        }
    }

    private volume: number;
    get Volume() {
        return this.volume;
    }
    set Volume(value: number) {
        if (this.volume != value) {
            this.volume = value;
            this.ComputeVolumetricWeight();
            this.ComputeVolume_CBM();
            this.SetUIProperties();
        }
    }

    private grossWeight: number;
    get GrossWeight() {
        return this.grossWeight;
    }
    set GrossWeight(value: number) {
        if (this.grossWeight != value) {
            this.grossWeight = value;
            this.ComputeVolumetricWeight();
            this.ComputeGrossWeigh_Kg_Ton();
            this.SetUIProperties();
        }
    }

    private grossWeightPerTon: number;
    get GrossWeightPerTon() {
        return this.grossWeightPerTon;
    }
    set GrossWeightPerTon(value: number) {
        if (this.grossWeightPerTon != value) {
            this.grossWeightPerTon = value;
        }
    }

    private grossWeightInKG: number;
    get GrossWeightInKG() {
        return this.grossWeightInKG;
    }
    set GrossWeightInKG(value: number) {
        if (this.grossWeightInKG != value) {
            this.grossWeightInKG = value;
        }
    }

    private chargeableWeightInKG: number;
    get ChargeableWeightInKG() {
        return this.chargeableWeightInKG;
    }
    set ChargeableWeightInKG(value: number) {
        if (this.chargeableWeightInKG != value) {
            this.chargeableWeightInKG = value;
        }
    }

    private volumeInCBM: number;
    get VolumeInCBM() {
        return this.volumeInCBM;
    }
    set VolumeInCBM(value: number) {
        if (this.volumeInCBM != value) {
            this.volumeInCBM = value;
        }
    }


    private productId: string;
    get TariffProductId() {
        return this.productId;
    }
    set TariffProductId(value: string) {
        if (this.productId != value) {
            this.productId = value;
        }
    }

    ComputeGrossWeigh_Kg_Ton() {
        var weigh_Kg: number = null;
        var weigh_Ton: number = null;

        if (this.GrossWeight != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.GrossWeightCode)) {
                switch (this.GrossWeightCode.toUpperCase()) {
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

        this.GrossWeightInKG = weigh_Kg;
        this.GrossWeightPerTon = weigh_Ton;
    }
    ComputeChargeableWeight_Kg() {
        var weigh_Kg: number = null;
        if (this.ChargeableWeight != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.WeightCode)) {
                switch (this.WeightCode.toUpperCase()) {
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
        this.ChargeableWeightInKG = weigh_Kg;
    }
    ComputeVolume_CBM() {
        var volume_CBM: number = null;

        if (this.Volume != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
                switch (this.VolumeUnitCode.toUpperCase()) {
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
        this.VolumeInCBM = volume_CBM;
    }

    FillDimensionsClicked() {
        this.entityResourceService.getEntityResourceByTableName("Shipment").subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe((res2: any) => {
                var logeWindow = new LogitudeWindow();
                logeWindow.Width = 850;
                logeWindow.Title = "Fill Dimensions";
                this.dimenstionShipment.TransportModeId = "A";
                this.dimenstionShipment.ChargeableWeightUnitCode = this.WeightCode;
                this.dimenstionShipment.VolumeUnitCode = this.VolumeUnitCode;
                this.dimenstionShipment.GrossWeightUnitCode = this.GrossWeightCode;
                logeWindow.WindowArgs = this.dimenstionShipment;
                logeWindow.Show("./TariffModule/Components/Workspaces/WizardDimensionsComponent");
                logeWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.weight = this.dimenstionShipment.OrderChargeableWeight;
                        this.grossWeight = this.dimenstionShipment.OrderGrossWeight;
                        this.volume = this.dimenstionShipment.BookingVolume;
                        if (!AppTool.IsNullOrZero(this.dimenstionShipment.OrderChargeableWeight) || !AppTool.IsNullOrZero(this.dimenstionShipment.OrderGrossWeight) || !AppTool.IsNullOrZero(this.dimenstionShipment.BookingVolume)) {
                            this.IsPickedFromWizard = true;
                        }
                        else {
                            this.IsPickedFromWizard = false;
                            this.weight = null;
                            this.grossWeight = null;
                            this.volume = null;
                            this.chargeableWeight = null;
                        }
                    }


                    this.SetUIProperties();
                });
            });
        });
    }

    ContainerType1Id: string;
    ContainerType2Id: string;
    ContainerType3Id: string;
    ContainerType4Id: string;
    ContainerType5Id: string;
    Quantity1: number;
    Quantity2: number;
    Quantity3: number;
    Quantity4: number;
    Quantity5: number;

    FillContainersClicked() {
        var logeWindow = new LogitudeWindow();
        logeWindow.Title = "Fill Containers";
        logeWindow.WindowArgs = { ContainerType1Id: this.ContainerType1Id, ContainerType2Id: this.ContainerType2Id, ContainerType3Id: this.ContainerType3Id, ContainerType4Id: this.ContainerType4Id, ContainerType5Id: this.ContainerType5Id };
        logeWindow.Show("./TariffModule/Components/Workspaces/AddTariffContainersComponent");
        logeWindow.ComponentLoaded.subscribe(s => {
            logeWindow.WindowClosed.subscribe(d => {
                if (d == "ok") {
                    this.ContainerType1Id = s.ContainerType1Id;
                    this.ContainerType2Id = s.ContainerType2Id;
                    this.ContainerType3Id = s.ContainerType3Id;
                    this.ContainerType4Id = s.ContainerType4Id;
                    this.ContainerType5Id = s.ContainerType5Id;
                }
            });
        });
    }

    ComputeVolumetricWeight() {
        this.weight = AppTool.ComputePackageVolumetricWeight(null, null, null, null, this.Volume, this.ChargeableWeight, this.Ratio, null, this.VolumeUnitCode, this.GrossWeightCode, this.WeightCode);
    }
    ComputeVolume() {
        this.volume = AppTool.ComputePackageVolume(null, null, null, null, this.ChargeableWeight, this.Ratio, null, this.VolumeUnitCode, this.GrossWeightCode);
    }

    ShowTariffclicked(item: TariffSearchSummary) {
        item.IsShown = !item.IsShown;

        if (item.IsShown) {
            item.MoreLessDetailsLabel = "Less Details";
        }

        else {
            item.MoreLessDetailsLabel = "More Details";
        }
    }

    ExpandNotesClicked(notes: string) {
        var windowArgs: any = {};
        windowArgs.TextValue = notes;
        windowArgs.DisplayMode = true;

        var wind = new LogitudeWindow();
        wind.Width = 960;
        wind.Height = 570;
        wind.WindowArgs = windowArgs;
        wind.Title = "Remarks";
        wind.Show("./Infrastructure/Component/LogitudeComponents/MultilineTextBoxWindow");
    }

    SearchButtonClicked() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.OriginPortId)) {
            this.ValidationErrorsList.push("From port is required");
        }

        if (AppTool.IsNullOrEmpty(this.DestinationPortId)) {
            this.ValidationErrorsList.push("To port is required");
        }

        if (AppTool.IsNullOrEmpty(this.Date)) {
            this.ValidationErrorsList.push("Date is required");
        }

        if (this.TariffType != "OFC") {
            if (AppTool.IsNullOrEmpty(this.Weight)) {
                this.ValidationErrorsList.push("Chargeable Weight is required");
            }

            if (AppTool.IsNullOrEmpty(this.WeightCode)) {
                this.ValidationErrorsList.push("Chargeable Weight unit is required");
            }
        }

        if (this.TariffType == "OFC") { // validate the containers
            if (AppTool.IsNullOrEmpty(this.ContainerType1Id) && AppTool.IsNullOrEmpty(this.ContainerType2Id) && AppTool.IsNullOrEmpty(this.ContainerType3Id) && AppTool.IsNullOrEmpty(this.ContainerType4Id) && AppTool.IsNullOrEmpty(this.ContainerType5Id)) {
                this.ValidationErrorsList.push("You have to fill at least one Container type");
            }

            this.ValidateContainerTypes_Duplicate();
        }

        if (this.ValidationErrorsList.length == 0) {
            if (AppTool.IsNullOrEmpty(this.GrossWeight)) {
                this.GrossWeight = this.Weight;
            }

            if (AppTool.IsNullOrEmpty(this.Volume)) {
                this.ComputeVolume();
            }

            this.CurrentSession.StartBusyIndicatorLoading();
            var tariffSearchArgs = new TariffSearchArgs();
            tariffSearchArgs.OriginPortId = this.OriginPortId;
            tariffSearchArgs.DestinationPortId = this.DestinationPortId;
            tariffSearchArgs.Date = ServiceHelper.GetDateString(this.Date);
            tariffSearchArgs.Weight = this.Weight;
            tariffSearchArgs.WeightCode = this.WeightCode;
            tariffSearchArgs.GrossWeight = this.GrossWeight;
            tariffSearchArgs.GrossWeightCode = this.GrossWeightCode;
            tariffSearchArgs.Volume = this.Volume;
            tariffSearchArgs.VolumeUnitCode = this.VolumeUnitCode;
            tariffSearchArgs.CurrencyId = this.CurrencyId;
            tariffSearchArgs.TariffType = this.TariffType;
            tariffSearchArgs.ContainerType1Id = this.ContainerType1Id;
            tariffSearchArgs.ContainerType2Id = this.ContainerType2Id;
            tariffSearchArgs.ContainerType3Id = this.ContainerType3Id;
            tariffSearchArgs.ContainerType4Id = this.ContainerType4Id;
            tariffSearchArgs.ContainerType5Id = this.ContainerType5Id;
            tariffSearchArgs.Quantity1 = this.Quantity1;
            tariffSearchArgs.Quantity2 = this.Quantity2;
            tariffSearchArgs.Quantity3 = this.Quantity3;
            tariffSearchArgs.Quantity4 = this.Quantity4;
            tariffSearchArgs.Quantity5 = this.Quantity5;
            tariffSearchArgs.ProductId = this.TariffProductId;

            this.myDomainService.GetAvailableAirlineFreightTariffs(tariffSearchArgs).subscribe((res: any) => {
                if (!res.HasError) {
                    if (res.Result) {
                        this.loadedResults = res.Result;
                        this.AssignResultToItemsSource();
                    }
                }
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    private loadedResults: Array<TariffSearchSummary> = [];
    private AssignResultToItemsSource() {
        this.NoDataMessage = "No results found matching your search. Please refine your search, or enter more tariffs to the system";
        this.IsFirstTime = false;

        if (!AppTool.IsNullOrEmpty(this.SearchText)) {
            this.AvailableTariffs = this.loadedResults.filter(f => f.SellerName.toUpperCase().indexOf(this.SearchText.toUpperCase()) > -1);
        }

        else {
            this.AvailableTariffs = this.loadedResults;
        }
    }

    ValidateContainerTypes_Duplicate() {
        for (var firstIndex = 1; firstIndex <= 5; firstIndex++) {
            for (var secondIndex = firstIndex + 1; secondIndex <= 5; secondIndex++) {
                var comparedContainer = "ContainerType" + firstIndex + "Id";
                var targetContainer = "ContainerType" + secondIndex + "Id";
                if (this[comparedContainer] != null && this[comparedContainer] == this[targetContainer]) {
                    var packageType: PackageTypeList = this.allPackageTypes.filter(d => d.Id == this[targetContainer + ""])[0];
                    if (packageType) {
                        this.ValidationErrorsList.push("Container type " + packageType.EnglishName + " is duplicated");
                    }
                }
            }
        }
    }

    PriceClick(item: TariffSearchSummary) {
        if (item) {
            var editWindow = new LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            editWindow.Title = "Price Check";
            editWindow.Height = 770;
            editWindow.Width = 1500;

            var code = item.VersionId;
            if (!AppTool.IsNullOrEmpty(item.LineId)) {
                code = code + "," + item.LineId;
            }

            var argumentsPriceCheck = { VersionId: item.VersionId, LineId: item.LineId, ChargeableWeightInKG: this.chargeableWeightInKG };
            editWindow.EditComponentArguments = argumentsPriceCheck;
            editWindow.ShowEditComponent(item.TariffId, "Tariff");
        }
    }

    ViewSurchargesClicked(item: TariffSearchSummary) {
        if (item && item.SurchargesWithoutAllIn != null && item.SurchargesWithoutAllIn.length != 0) {
            var surcharge = item.SurchargesWithoutAllIn[0];
            var editWindow = new LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            editWindow.Title = "Price Check";
            editWindow.Height = 770;
            editWindow.Width = 1500;

            var code = surcharge.VersionId;
            if (!AppTool.IsNullOrEmpty(surcharge.LineId)) {
                code = code + "," + surcharge.LineId;
            }

            editWindow.ShowEditComponent(surcharge.TariffId, "Tariff", code);
        }
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
        this.UIProperties.SetRequired("Date", null, AppTool.IsNullOrEmpty(this.Date));
        this.UIProperties.SetRequired("Weight", null, AppTool.IsNullOrEmpty(this.Weight));

        if (this.IsPickedFromWizard || this.IsShipment || this.IsQuote) {
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("GrossWeightCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("WeightCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OriginPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DestinationPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ContainerType1Id", null, false);
            this.UIProperties.SetEnabled("ContainerType2Id", null, false);
            this.UIProperties.SetEnabled("ContainerType3Id", null, false);
            this.UIProperties.SetEnabled("ContainerType4Id", null, false);
            this.UIProperties.SetEnabled("ContainerType5Id", null, false);
            this.UIProperties.SetEnabled("Quantity1", null, false);
            this.UIProperties.SetEnabled("Quantity2", null, false);
            this.UIProperties.SetEnabled("Quantity3", null, false);
            this.UIProperties.SetEnabled("Quantity4", null, false);
            this.UIProperties.SetEnabled("Quantity5", null, false);

        }
        else {
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("GrossWeightCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("OriginPortId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("DestinationPortId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ContainerType1Id", null, true);
            this.UIProperties.SetEnabled("ContainerType2Id", null, true);
            this.UIProperties.SetEnabled("ContainerType3Id", null, true);
            this.UIProperties.SetEnabled("ContainerType4Id", null, true);
            this.UIProperties.SetEnabled("ContainerType5Id", null, true);
            this.UIProperties.SetEnabled("Quantity1", null, true);
            this.UIProperties.SetEnabled("Quantity2", null, true);
            this.UIProperties.SetEnabled("Quantity3", null, true);
            this.UIProperties.SetEnabled("Quantity4", null, true);
            this.UIProperties.SetEnabled("Quantity5", null, true);

        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private Generator: ShipmentGenerator;
    private TariffList_Shipment: ShipmentPayablePM[];
    private TariffList_Quote: QuoteChargePM[];
    GeneratePayablesClicked(item: TariffSearchSummary) {
        if (this.IsShipment) {
            this.GenerateShipmentPayablesFromTariff(item);
        }
        else {
            this.GenerateQuoteChargesFromTariff(item);
        }
    }

    // LCL/FCLShipment
    private BCNTGrouped: ByPckageType[] = [];
    private ValidateShipmentTariffContainers() {
        var isValid = true;
        if (this.TariffType == "OFC") {
            var checkIfShipmentHasTariffContainers = this.BCNTGrouped.filter(a => a.PackageTypeId == this.ContainerType1Id ||
                a.PackageTypeId == this.ContainerType2Id ||
                a.PackageTypeId == this.ContainerType3Id ||
                a.PackageTypeId == this.ContainerType4Id ||
                a.PackageTypeId == this.ContainerType5Id);

            if (checkIfShipmentHasTariffContainers == null || (checkIfShipmentHasTariffContainers != null && checkIfShipmentHasTariffContainers.length == 0)) {
                isValid = false;
                var messageWindow = new MessageWindow();
                messageWindow.Show("Tariff container types are not found in the shipment.");
            }
        }
        return isValid;
    }

    GenerateShipmentPayablesFromTariff(item: TariffSearchSummary) {
        var isValid = this.ValidateExistPayablesConnectedToTariff(item);
        if (isValid) {
            isValid = this.ValidateTariffClosedLines();
            var isShipmentHasTariffContainers = this.ValidateShipmentTariffContainers();
            if (isValid && isShipmentHasTariffContainers) {
                this.TariffList_Shipment = [];
                this.Generator = new ShipmentGenerator(this.FatherComponent.EntityPM, this.FatherComponent.AllRates);

                var notes = null;
                if (!AppTool.IsNullOrEmpty(item.AllIn)) {
                    notes = "Includes the following charges as all-in: " + item.AllIn;
                }

                // FCL Shipment 
                if (this.TariffType == "OFC") {
                    var teuPrice: number = 0;
                    // Generate FCL Frieght
                    var shipmentContainer = this.BCNTGrouped.filter(f => f.PackageTypeId == this.ContainerType1Id)[0];
                    if (this.ContainerType1Id && shipmentContainer) {
                        this.AddNewTariffPayable(item, notes, this.ContainerType1Id);
                        if (item != null && item.SurchargesWithoutAllIn != null) {
                            item.SurchargesWithoutAllIn.filter(a => a.UnitOfMesurmentCode != 'FIXD' && a.UnitOfMesurmentCode != 'BTEU').forEach(surcharge => {
                                this.AddNewTariffPayable(surcharge, null, this.ContainerType1Id);
                            });
                        }
                    }
                    shipmentContainer = this.BCNTGrouped.filter(f => f.PackageTypeId == this.ContainerType2Id)[0];
                    if (this.ContainerType2Id && shipmentContainer) {
                        this.AddNewTariffPayable(item, notes, this.ContainerType2Id);
                        if (item != null && item.SurchargesWithoutAllIn != null) {
                            item.SurchargesWithoutAllIn.filter(a => a.UnitOfMesurmentCode != 'FIXD' && a.UnitOfMesurmentCode != 'BTEU').forEach(surcharge => {
                                this.AddNewTariffPayable(surcharge, null, this.ContainerType2Id);
                            });
                        }
                    }
                    shipmentContainer = this.BCNTGrouped.filter(f => f.PackageTypeId == this.ContainerType3Id)[0];
                    if (this.ContainerType3Id && shipmentContainer) {
                        this.AddNewTariffPayable(item, notes, this.ContainerType3Id);
                        if (item != null && item.SurchargesWithoutAllIn != null) {
                            item.SurchargesWithoutAllIn.filter(a => a.UnitOfMesurmentCode != 'FIXD' && a.UnitOfMesurmentCode != 'BTEU').forEach(surcharge => {
                                this.AddNewTariffPayable(surcharge, null, this.ContainerType3Id);
                            });
                        }
                    }
                    shipmentContainer = this.BCNTGrouped.filter(f => f.PackageTypeId == this.ContainerType4Id)[0];
                    if (this.ContainerType4Id && shipmentContainer) {
                        this.AddNewTariffPayable(item, notes, this.ContainerType4Id);
                        if (item != null && item.SurchargesWithoutAllIn != null) {
                            item.SurchargesWithoutAllIn.filter(a => a.UnitOfMesurmentCode != 'FIXD' && a.UnitOfMesurmentCode != 'BTEU').forEach(surcharge => {
                                this.AddNewTariffPayable(surcharge, null, this.ContainerType4Id);
                            });
                        }
                    }
                    shipmentContainer = this.BCNTGrouped.filter(f => f.PackageTypeId == this.ContainerType5Id)[0];
                    if (this.ContainerType5Id && shipmentContainer) {
                        this.AddNewTariffPayable(item, notes, this.ContainerType5Id);
                        if (item != null && item.SurchargesWithoutAllIn != null) {
                            item.SurchargesWithoutAllIn.filter(a => a.UnitOfMesurmentCode != 'FIXD' && a.UnitOfMesurmentCode != 'BTEU').forEach(surcharge => {
                                this.AddNewTariffPayable(surcharge, null, this.ContainerType5Id);
                            });
                        }
                    }

                    // Generate one line for TEU & Fixed 
                    item.SurchargesWithoutAllIn.filter(a => a.UnitOfMesurmentCode == 'FIXD' || a.UnitOfMesurmentCode == 'BTEU').forEach(surcharge => {
                        this.AddNewTariffPayable(surcharge, null, null);
                    });
                }

                else {
                    // Generate Air Frieght
                    this.AddNewTariffPayable(item, notes);
                    // Generate Surcharges
                    if (item != null && item.SurchargesWithoutAllIn != null) {
                        item.SurchargesWithoutAllIn.forEach(surcharge => {
                            this.AddNewTariffPayable(surcharge);
                        });
                    }
                }

                var isDuplicate = this.CheckTariffPayablesDuplicate();
                if (isDuplicate) {
                    // override
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Show("This generate will update on the existing lines.");
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.CurrentSession.StartBusyIndicatorLoading();
                            this.OverrideTariffPayablesOfShipment();
                        }
                        if (confirmWindow.No) {
                            //nothing
                        }
                    });
                }
                else {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this.AssignTariffPayablesToShipment();
                }
            }
        }
    }

    OverrideTariffPayablesOfShipment(): any {
        this.TariffList_Shipment.forEach(payable => {
            var existsPayable: ShipmentPayablePM = this.FatherComponent.EntityPM.ShipmentPayables.filter(d => d.ChargesTypeId == payable.ChargesTypeId && d.MeasurementId == payable.MeasurementId && (d.TariffId == payable.TariffId || d.TariffId == null))[0];
            if (existsPayable != null) {
                this.FatherComponent.EntityPM.RemovePayable(existsPayable);
            }
        });
        this.AssignTariffPayablesToShipment();
    }

    AssignTariffPayablesToShipment(): any {
        this.TariffList_Shipment.forEach(shipmentPayable => {
            this.FatherComponent.EntityPM.AddPayable(shipmentPayable);
            var payableItem = new ShipmentPayableItem(shipmentPayable, this.FatherComponent, false);
            this.FatherComponent.ItemsSource.Insert(payableItem);
            payableItem.ChargesTypeId = shipmentPayable.ChargesTypeId;
            payableItem.MeasurementId = shipmentPayable.MeasurementId;
            payableItem.CurrencyId = shipmentPayable.CurrencyId;
            payableItem.UnitPrice = shipmentPayable.UnitPrice;
            payableItem.MinAmount = shipmentPayable.MinAmount;
            payableItem.ComputeTotalAmount();
        });
        this.ReloadTariffPayables();
    }

    ReloadTariffPayables() {
        this.FatherComponent.OnEntityDataGenerated();
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }

    CheckTariffPayablesDuplicate(): any {
        var isDuplicate = false;
        this.TariffList_Shipment.forEach(payable => {
            var existsPayable: ShipmentPayablePM = this.FatherComponent.EntityPM.ShipmentPayables.filter(d => d.ChargesTypeId == payable.ChargesTypeId && d.MeasurementId == payable.MeasurementId && (d.TariffId == payable.TariffId || d.TariffId == null))[0];
            if (existsPayable != null) {
                isDuplicate = true;
            }
        });
        return isDuplicate;
    }

    AddNewTariffPayable(newRecord: any, notes = null, packageId = null) {
        this.myChargesTypeListService.getSingleFromCache(newRecord.ChargeTypeId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var chargesType: ChargesTypeList = myResponse.Result;
                var shipmentPayable: ShipmentPayablePM = this.Generator.GeneratePayablesFromTariff(chargesType);
                shipmentPayable.TariffId = newRecord.TariffId;
                shipmentPayable.TariffNumber = newRecord.TariffNumber;
                shipmentPayable.TariffLineId = newRecord.LineId;
                shipmentPayable.TariffVersion = newRecord.VersionId != null ? newRecord.VersionId.toString() : newRecord.VersionId;
                shipmentPayable.CurrencyId = newRecord.CurrencyId;
                this.Generator.GetCurrencyCode(shipmentPayable);
                shipmentPayable.Rate = this.Generator.GetCurrencyRate(shipmentPayable.CurrencyId);
                shipmentPayable.ProfitCurrencyExchangeRate = this.Generator.GetCurrencyRate(this.FatherComponent.EntityPM.ProfitCurrencyId);
                shipmentPayable.MeasurementId = newRecord.UnitOfMesurmentId;
                shipmentPayable.MeasurementCode = newRecord.UnitOfMesurmentCode;

                var newQuantity = 1;
                var expectedAmount;
                if (packageId != null) {
                    this.CreateNewPayableFromTariffCharge_FCL(packageId, shipmentPayable);
                    var containerPrice = 0;
                    if (newRecord.ContainersPrices) {
                        var container = newRecord.ContainersPrices.filter(d => d.TariffId == newRecord.TariffId && d.ContainerId == packageId)[0];
                        if (container) {
                            containerPrice = container.Price;
                            newQuantity = container.Quantity;
                        }
                    }
                    expectedAmount = containerPrice;
                }
                else {
                    expectedAmount = newRecord.ActualPrice;
                    newQuantity = this.GetQuantity(newRecord.UnitOfMesurmentCode)
                }

                if (!AppTool.IsNullOrZero(expectedAmount)) {
                    shipmentPayable.TariffId = newRecord.TariffId;
                    shipmentPayable.TariffNumber = newRecord.TariffNumber;
                    shipmentPayable.TariffVersion = newRecord.VersionId != null ? newRecord.VersionId.toString() : newRecord.VersionId;
                }

                var rate = this.Generator.GetCurrencyRate(newRecord.CurrencyId);
                var expectedAmountLocal = expectedAmount * rate;

                var profitCurrencyExchangeRate = this.Generator.GetCurrencyRate(this.FatherComponent.EntityPM.ProfitCurrencyId);
                var expectedAmountProfit = expectedAmountLocal / profitCurrencyExchangeRate;
                shipmentPayable.ExpectedAmount = AppTool.Round(expectedAmount, 2);

                if (newQuantity != null) {

                    shipmentPayable.Quantity = AppTool.Round(newQuantity, 3);
                    if (newRecord.UnitOfMesurmentCode == "PRVL" || newRecord.UnitOfMesurmentCode == "PRFR") {
                        var price = shipmentPayable.ExpectedAmount * 100;
                        shipmentPayable.UnitPrice = AppTool.Round(price / shipmentPayable.Quantity, 3);
                    }
                    else {
                        shipmentPayable.UnitPrice = expectedAmount != null ? AppTool.Round(expectedAmount / newQuantity, 3) : null;
                    }
                }

                shipmentPayable.ExpectedAmountLocal = AppTool.Round(expectedAmountLocal, 2);
                shipmentPayable.ExpectedAmountInProfitCurrency = AppTool.Round(expectedAmountProfit, 2);
                shipmentPayable.OpenAmount = shipmentPayable.ExpectedAmount;
                shipmentPayable.OpenAmountInLocalCurrency = shipmentPayable.ExpectedAmountLocal;
                shipmentPayable.OpenAmountInProfitCurrency = shipmentPayable.ExpectedAmountInProfitCurrency;
                shipmentPayable.AccountedAmount = 0;
                shipmentPayable.AccountedAmountInLocalCurrency = 0;
                shipmentPayable.AccountedAmountInProfitCurrency = 0;
                shipmentPayable.ShipmentPayableLineStatusCode = "EMPT";
                shipmentPayable.ShipmentPayableAmountTypeCode = "ACCU";
                shipmentPayable.ShipmentId = this.FatherComponent.EntityPM.Id;
                shipmentPayable.ShipmentNumber = this.FatherComponent.EntityPM.ShipmentNumber;
                shipmentPayable.CreateDate = DateTool.GetCurrentDateAsUtc();
                shipmentPayable.Tenant = this.FatherComponent.EntityPM.Tenant;
                shipmentPayable.CreatedByUserId = SessionLocator.LoggedUserId;
                shipmentPayable.UpdateDate = DateTool.GetCurrentDateAsUtc();
                shipmentPayable.UpdateByUserId = SessionLocator.LoggedUserId;
                shipmentPayable.Notes = notes;
                shipmentPayable.VendorId = newRecord.SellerId;
                shipmentPayable.VendorName = newRecord.SellerName;
                shipmentPayable.MinAmount = newRecord.IsDifferentCurrency ? newRecord.ActualMinPrice : newRecord.MinPrice;
                this.TariffList_Shipment.push(shipmentPayable);
            }
        });

        this.OnAmountChanged(); 
    }

    OnAmountChanged() {
        this.TariffList_Shipment.filter(f => f.ChargesGroupCode != "FRT" && f.MeasurementCode == "PRFR").forEach(item => {
            var newQuantity = this.GetQuantity(item.MeasurementCode);
            var price = item.ExpectedAmount * 100;
            item.UnitPrice = AppTool.Round(price / newQuantity, 3);
        });
    }

    private CreateNewPayableFromTariffCharge_FCL(myPackageTypeId: string, newItemPM: ShipmentPayablePM) {
        if (!AppTool.IsNullOrEmpty(myPackageTypeId)) {
            var itemGrouped: ByPckageType = this.BCNTGrouped.filter(f => f.PackageTypeId == myPackageTypeId)[0];
            if (itemGrouped != null) {
                newItemPM.MeasurementId = itemGrouped.MeasurementId;
                newItemPM.MeasurementCode = itemGrouped.MeasurementCode;
                newItemPM.MeasurementShortName = itemGrouped.MeasurementShortName;
            }
        }
    }

    ValidateExistPayablesConnectedToTariff(item: TariffSearchSummary) {
        var isValid = true;
        var existsPayableOnAirFreight: ShipmentPayablePM = this.FatherComponent.EntityPM.ShipmentPayables.filter(d => d.TariffId != null && d.TariffId != item.TariffId && d.ChargesTypeId == item.ChargeTypeId)[0];
        var existsPayableOnSurcharges: ShipmentPayablePM[] = [];
        item.SurchargesWithoutAllIn.forEach(surcharge => {
            var payable = this.FatherComponent.EntityPM.ShipmentPayables.filter(d => d.TariffId != null && d.TariffId != surcharge.TariffId && d.ChargesTypeId == surcharge.ChargeTypeId)[0];
            if (payable) {
                existsPayableOnSurcharges.push(payable);
            }
        });

        if (existsPayableOnAirFreight || (existsPayableOnSurcharges != null && existsPayableOnSurcharges.length > 0)) {
            var messageWindow = new MessageWindow();
            isValid = false;
            messageWindow.Show("Can't have more than one tariff connected to the same line.");
        }
        return isValid;
    }
    ValidateTariffClosedLines() {
        var isValid = true;
        var closedPayablesLine: ShipmentPayablePM = this.FatherComponent.EntityPM.ShipmentPayables.filter(d => d.AccountedAmount != null && d.AccountedAmount != 0)[0];
        if (closedPayablesLine) {
            var messageWindow = new MessageWindow();
            isValid = false;
            messageWindow.Show("Can't connect a tariff to this shipment due to closed lines.");
        }
        return isValid;
    }
    GetQuantity(measurementCode): any {
        var myQuantity: number = null;
        switch (measurementCode) {
            case "GRWT": { myQuantity = this.GrossWeight; break; }
            case "CHWT": { myQuantity = this.ChargeableWeight; break; }
            case "VOLU": { myQuantity = this.Volume; break; }
            case "BTEU": { myQuantity = this.FatherComponent.EntityPM.TEU; break; }
            case "FIXD": { myQuantity = 1; break; }
            case "PRVL": { myQuantity = this.FatherComponent.EntityPM.ValueOfGoods; break; }
            case "PRFR": { myQuantity = ArrayTool.Sum(this.TariffList_Shipment.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentPayableParentId)), "ExpectedAmount"); break; }
            case "GWTN": { myQuantity = this.GrossWeightPerTon; break; }
            case "CWKG": { myQuantity = this.ChargeableWeightInKG; break; }
            case "GWKG": { myQuantity = this.GrossWeightInKG; break; }
            case "QTY": { myQuantity = this.FatherComponent.EntityPM.NumberOfPackages != null ? this.FatherComponent.EntityPM.NumberOfPackages : null; break; }
            case "VCBM": { myQuantity = this.VolumeInCBM; break; }
            default: { break; }
        }
        return myQuantity;
    }

    // Quote Work
    GenerateQuoteChargesFromTariff(item: TariffSearchSummary) {
        var isValid = this.ValidateExistChargesConnectedToTariff(item);
        if (isValid) {
            this.TariffList_Quote = [];

            var isOFC = false;
            // FCL Quote 
            if (this.TariffType == "OFC") {
                isOFC = true;
            }


            this.AddNewTariffQuoteCharge(item, isOFC, false);
            if (item != null && item.SurchargesWithoutAllIn != null) {
                item.SurchargesWithoutAllIn.forEach(surcharge => {
                    this.AddNewTariffQuoteCharge(surcharge, isOFC, true);
                });
            }
            // Generate AllIn Surcharges
            if (item != null && item.AllInSurcharges != null) {
                item.AllInSurcharges.forEach(surcharge => {
                    this.AddNewTariffQuoteCharge(surcharge, false, true);
                });
            }


            var isDuplicate = this.CheckTariffChargesDuplicate();
            if (isDuplicate) {
                // override
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("This generate will update on the existing lines.");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.CurrentSession.StartBusyIndicatorLoading();
                        this.OverrideTariffQuoteCharges();
                    }
                    if (confirmWindow.No) {
                        //nothing
                    }
                });
            }
            else {
                var freightChrage = this.CheckFreightDuplicate();
                if (freightChrage) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("Freight Charge already added.");
                }
                else {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this.AssignTariffChargesToQuote();
                }
            }
        }
    }

    CheckFreightDuplicate(): any {
        var isfreighExists = false;
        if (this.FatherComponent.EntityPM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT").length > 0) {
            isfreighExists = true;
        }
        return isfreighExists;
    }
    OverrideTariffQuoteCharges(): any {
        this.TariffList_Quote.forEach(item => {
            var charge: QuoteChargePM = this.FatherComponent.EntityPM.QuoteCharges.filter(d => d.ChargesTypeId == item.ChargesTypeId && d.CostCurrencyId == item.CostCurrencyId && d.CostMeasurementId == item.CostMeasurementId && (d.TariffId == item.TariffId || d.TariffId == null))[0];
            if (charge != null) {
                this.FatherComponent.EntityPM.RemoveQuoteChargePM(charge);
                item.ChargesGroupCode = charge.ChargesGroupCode;
            }
        });
        this.AssignTariffChargesToQuote();
    }

    AssignTariffChargesToQuote() {
        if (this.TariffType == "OFC") {
            this.AssignTariffChargesToQuote_FCL();
        }
        else {
            this.AssignTariffChargesToQuote_LCL();
        }
    }

    AssignTariffChargesToQuote_LCL() {
        this.TariffList_Quote.forEach(item => {
            this.FatherComponent.EntityPM.AddQuoteChargePM(item);
            var chargeItem: QuoteChargeItem = new QuoteChargeItem(item, this.FatherComponent, false);
            chargeItem.ChargesTypeId = item.ChargesTypeId;
            chargeItem.CostMeasurementId = item.CostMeasurementId;
            chargeItem.CostCurrencyId = item.CostCurrencyId;
            chargeItem.CostTotalAmount = item.CostTotalAmount;
            chargeItem.CostMinAmount = item.CostMinAmount;
            chargeItem.CostExchangeRate = item.CostExchangeRate;
            chargeItem.SaleMeasurementId = item.SaleMeasurementId;
            chargeItem.SaleCurrencyId = item.SaleCurrencyId;
            chargeItem.SaleExchangeRate = item.SaleExchangeRate;
            chargeItem.ChargesGroupCode = item.ChargesGroupCode;
            chargeItem.IsAllIN = false;
            var costAmount: number = item.CostTotalAmount;
            chargeItem.SetCostQuantity();
            chargeItem.SetSaleQuantity();
            var costQuantity: number = chargeItem.CostQuantity;

        if (costQuantity != null && costQuantity != 0) {
            if (chargeItem.CostMeasurementCode == "PRVL" || chargeItem.CostMeasurementCode == "PRFR") {
                chargeItem.CostUnitPrice = (costAmount / costQuantity) * 100;
            }
            else {
                chargeItem.CostUnitPrice = (costAmount / costQuantity);

            }
            chargeItem.SaleUnitPrice = chargeItem.CostUnitPrice;
            }
            chargeItem.ComputeSalePrice();
            chargeItem.ComputeSaleAmounts();
            chargeItem.SetUIProperties_AllIn();
            this.FatherComponent.ItemsSource.Insert(chargeItem);
        });
        this.ReloadTariffCharges();
    }

    AssignTariffChargesToQuote_FCL() {
        this.TariffList_Quote.forEach(item => {
            this.FatherComponent.EntityPM.AddQuoteChargePM(item);
            var chargeItem: FCLQuoteChargeItem = new FCLQuoteChargeItem(item, this.FatherComponent, false);
            var chargeId = item.ChargesTypeId;
            chargeItem.ChargesTypeId = chargeId;
            chargeItem.CostMeasurementId = item.CostMeasurementId;
            chargeItem.CostCurrencyId = item.CostCurrencyId;
            chargeItem.CostTotalAmount = item.CostTotalAmount;
            chargeItem.CostMinAmount = item.CostMinAmount;
            chargeItem.CostExchangeRate = item.CostExchangeRate;
            chargeItem.SaleMeasurementId = item.SaleMeasurementId;
            chargeItem.SaleCurrencyId = item.SaleCurrencyId;
            chargeItem.SaleExchangeRate = item.SaleExchangeRate;
            chargeItem.ChargesGroupCode = item.ChargesGroupCode;
            chargeItem.IsAllIN = false;
            chargeItem.CostContainerType1UnitPrice = item.CostContainerType1UnitPrice;
            chargeItem.CostContainerType2UnitPrice = item.CostContainerType2UnitPrice;
            chargeItem.CostContainerType3UnitPrice = item.CostContainerType3UnitPrice;
            chargeItem.CostContainerType4UnitPrice = item.CostContainerType4UnitPrice;
            chargeItem.CostContainerType5UnitPrice = item.CostContainerType5UnitPrice;
            var costAmount: number = item.CostTotalAmount;
            chargeItem.CostQuantity = item.CostQuantity;
            chargeItem.SaleQuantity = item.CostQuantity;

            var costQuantity: number = chargeItem.CostQuantity;
            if (costQuantity != null && costQuantity != 0) {
            if (chargeItem.CostMeasurementCode == "PRVL" || chargeItem.CostMeasurementCode == "PRFR") {
                chargeItem.CostUnitPrice = (costAmount / costQuantity) * 100;
            }
            else {
                chargeItem.CostUnitPrice = (costAmount / costQuantity);
            }
                chargeItem.SaleUnitPrice = chargeItem.CostUnitPrice;
            }
            chargeItem.ComputeSalePrice();
            chargeItem.ComputeSaleAmounts();
            chargeItem.ComputeCostInSalePrice();  
            chargeItem.ComputeCostInSalePrice1();
            chargeItem.ComputeCostInSalePrice2();
            chargeItem.ComputeCostInSalePrice3();
            chargeItem.ComputeCostInSalePrice4();
            chargeItem.ComputeCostInSalePrice5();
            chargeItem.ComputeCostInSaleAmount();
            chargeItem.SetUIProperties_AllIn();
            this.FatherComponent.ItemsSource.Insert(chargeItem);
        });
        this.ReloadTariffCharges();
    }
    ReloadTariffCharges() {
        this.FatherComponent.BuildItemsSource();
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();

    }
    CheckTariffChargesDuplicate(): any {
        var isDuplicate = false;
        this.TariffList_Quote.forEach(item => {
            var isChargeExists: QuoteChargePM = this.FatherComponent.EntityPM.QuoteCharges.filter(d => d.ChargesTypeId == item.ChargesTypeId && d.CostCurrencyId == item.CostCurrencyId && d.CostMeasurementId == item.CostMeasurementId && (d.TariffId == item.TariffId || d.TariffId == null))[0];
            if (isChargeExists != null) {
                isDuplicate = true;
            }
        });
        return isDuplicate;
    }
    AddNewTariffQuoteCharge(item: any, isOFC: boolean, isSurcharge : boolean) {
        this.myChargesTypeListService.getSingleFromCache(item.ChargeTypeId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var chargesType: ChargesTypeList = myResponse.Result;
                var chargePM = new QuoteChargePM(this.FatherComponent.EntityPM);
                chargePM.ChargesTypeId = item.ChargeTypeId;
                chargePM.ChargesTypeCode = chargesType.Code;
                chargePM.ChargesTypeName = chargesType.EnglishName;
                chargePM.ChargesTypeLocalName = chargesType.LocalName;
                chargePM.TariffId = item.TariffId;
                chargePM.TariffNumber = item.TariffNumber;
                chargePM.TariffLineId = item.LineId;
                chargePM.TariffVersion = item.VersionId != null ? item.VersionId.toString() : item.VersionId;
                chargePM.Tenant = this.FatherComponent.EntityPM.Tenant;
                chargePM.QuoteId = this.FatherComponent.EntityPM.Id;
                chargePM.UpdatedByUserId = SessionLocator.LoggedUserId;
                chargePM.MarkUpTypeCode = "F";
                chargePM.MarkUpValue = 0;
                chargePM.QuoteTypeCode = this.FatherComponent.EntityPM.QuoteTypeCode;
                chargePM.CostCurrencyId = item.CurrencyId;
                chargePM.CostCurrencyCode = this.FatherComponent.Behaviours.GetCurrencyCode(item.CurrencyId);
                chargePM.CostExchangeRate = this.FatherComponent.Behaviours.GetCurrencyRate(item.CurrencyId);

                chargePM.SaleCurrencyId = this.FatherComponent.Behaviours.GetSaleCurrencyOnChargeTypeChanged(chargesType, chargePM);
                chargePM.SaleCurrencyCode = this.FatherComponent.Behaviours.GetCurrencyCode(chargePM.SaleCurrencyId);
                chargePM.SaleExchangeRate = this.FatherComponent.Behaviours.GetCurrencyRate(chargePM.SaleCurrencyId);
                chargePM.ChargesGroupCode = chargesType.ChargesGroupCode;

                var measurementCode = item.UnitOfMesurmentCode;
                var quantity;
                var measurementId = item.UnitOfMesurmentId;

                if (!item.IsAllIn) {
                    chargePM.CostMinAmount = AppTool.Round(item.IsDifferentCurrency ? item.ActualMinPrice : item.MinPrice, 3);
                    var costAmount = AppTool.Round(item.ActualPrice, 3);
                    chargePM.CostTotalAmount = costAmount;
                    if (isOFC) {
                        if (!isSurcharge) {
                            var bcntCharge = this.FatherComponent.Behaviours.AllMeasurements.filter(d => d.Code == "BCNT")[0];
                            measurementCode = bcntCharge.Code;
                            measurementId = bcntCharge.Id;
                        }

                        if (measurementCode != 'FIXD' && measurementCode != 'BTEU') {
                            this.FillQuoteFCLCharges(this.ContainerType1Id, chargePM, item);
                            this.FillQuoteFCLCharges(this.ContainerType2Id, chargePM, item);
                            this.FillQuoteFCLCharges(this.ContainerType3Id, chargePM, item);
                            this.FillQuoteFCLCharges(this.ContainerType4Id, chargePM, item);
                            this.FillQuoteFCLCharges(this.ContainerType5Id, chargePM, item);
                        }
                        else {
                            if (measurementCode == 'FIXD') {
                                chargePM.CostUnitPrice = item.ActualPrice;
                                chargePM.CostQuantity = 1;
                            }
                            else {
                                 chargePM.CostUnitPrice= item.ContainersPrices[0].Price_WithoutQuantity;
                                chargePM.CostQuantity = this.FatherComponent.EntityPM.TEU;
                            }
                        }
                    }
                }
              
                chargePM.CostMeasurementCode = measurementCode;
                chargePM.SaleMeasurementCode = measurementCode;
                chargePM.CostMeasurementId = measurementId;
                chargePM.SaleMeasurementId = measurementId;
                chargePM.VendorId = item.SellerId;
                chargePM.VendorName = item.SellerName;
                chargePM.IsCostAllIn = item.IsAllIn;
                this.TariffList_Quote.push(chargePM);
            }
        });
    }

    FillQuoteFCLCharges(packageId: string, chargePM: QuoteChargePM, item: any) {
        var costAmount: number = AppTool.Round(item.ActualPrice, 3);
        var quantity: number;

        if (item.ContainersPrices) {
            var container = item.ContainersPrices.filter(d => d.TariffId == item.TariffId && d.ContainerId == packageId)[0];
            if (container) {
                quantity = container.Quantity;
                costAmount = container.Price_WithoutQuantity;
            }

            else {
                costAmount = 0;
            }
        }
  
        if (packageId == this.FatherComponent.EntityPM.PackageType1Id) {
            chargePM.CostContainerType1UnitPrice = costAmount;
        }
        else if (packageId == this.FatherComponent.EntityPM.PackageType2Id) {
            chargePM.CostContainerType2UnitPrice = costAmount;
        }
        else if (packageId == this.FatherComponent.EntityPM.PackageType3Id) {
            chargePM.CostContainerType3UnitPrice = costAmount;
        }
        else if (packageId == this.FatherComponent.EntityPM.PackageType4Id) {
            chargePM.CostContainerType4UnitPrice = costAmount;
        }
        else if (packageId == this.FatherComponent.EntityPM.PackageType5Id) {
            chargePM.CostContainerType5UnitPrice = costAmount;
        }
        else {
            chargePM.CostTotalAmount = costAmount;
        }

        chargePM.CostQuantity = quantity;
    }

    ValidateExistChargesConnectedToTariff(item: TariffSearchSummary) {
        var isValid = true;
        var existsPayableOnAirFreight: QuoteChargePM = this.FatherComponent.EntityPM.QuoteCharges.filter(d => d.TariffId != null && d.TariffId != item.TariffId && d.ChargesTypeId == item.ChargeTypeId)[0];
        var quoteChargesOnSurcharges: QuoteChargePM[] = [];
        item.SurchargesWithoutAllIn.forEach(surcharge => {
            var charge = this.FatherComponent.EntityPM.QuoteCharges.filter(d => d.TariffId != null && d.TariffId != surcharge.TariffId && d.ChargesTypeId == surcharge.ChargeTypeId)[0];
            if (charge) {
                quoteChargesOnSurcharges.push(charge);
            }
        });

        if (existsPayableOnAirFreight || (quoteChargesOnSurcharges != null && quoteChargesOnSurcharges.length > 0)) {
            var messageWindow = new MessageWindow();
            isValid = false;
            messageWindow.Show("Can't have more than one tariff connected to the same line.");
        }


        return isValid;
    }

    public IsFiltersExtended: boolean = false;
    public IsFiltersHidden: boolean = true;
    public FiltersHeight: number = 90;
    public NoDataMessage: string = "Please enter data to get up-to-date results";
    ExtendedFiltersClicked(action: string) {
        if (action == "show") {
            this.IsFiltersExtended = true;
            this.IsFiltersHidden = false;
            this.FiltersHeight = 130;
        }

        else {
            this.IsFiltersExtended = false;
            this.IsFiltersHidden = true;
            this.FiltersHeight = 90;
        }
    }

    //FlexibleDate

    get GrossWeightFilterVisible() { return AppTool.IsNullOrZero(this.GrossWeight) ? false : true }
    get VolumeFilterVisible() { return AppTool.IsNullOrZero(this.Volume) ? false : true }

    private fromPrice: number;
    get FromPrice() {
        return this.fromPrice;
    }
    set FromPrice(value: number) {
        if (this.fromPrice != value) {
            this.fromPrice = value;
        }
    }

    private toPrice: number;
    get ToPrice() {
        return this.toPrice;
    }
    set ToPrice(value: number) {
        if (this.toPrice != value) {
            this.toPrice = value;
        }
    }

    private searchText: string = null;
    public get SearchText() { return this.searchText; }
    public set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
        }
    }

    SearchTextChanged(text: string) {
        this.SearchText = text;
        this.AssignResultToItemsSource();
    }
}
