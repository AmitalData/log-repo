import { Component } from '@angular/core';
import { AppTool, DateTool, ArrayTool } from '../../../Infrastructure/Tools';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TariffDomainService} from '../../../TariffModule/Services/TariffDomainService';
import { TariffSearchSummary, TariffSearchArgs } from '../../../TariffModule/Services/TariffDomainService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { CurrencyList } from '../../../Common/EntityLists/CurrencyList';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { ShipmentPM } from '../../../Shipment/EntityPMs/ShipmentPM';
import { ShipmentPayablePM } from '../../../Shipment/EntityPMs/ShipmentPayablePM';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import {  ShipmentGenerator } from '../../../Shipment/Tools';
import { ChargesTypeList } from '../../../Common/EntityLists/ChargesTypeList';
import { ChargesTypeListService } from '../../../Common/Services/StandardLists/ChargesTypeListService';
import { ShipmentPayableItem } from '../../../ShipmentModules/ShipmentTabs/Components/Payables/PayablesTabComponent';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';


@Component({
    moduleId: module.id,
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
    private ShipmentPM: ShipmentPM;
    private FatherComponent: any;
    private myChargesTypeListService: ChargesTypeListService;
    private dimenstionShipment: ShipmentPM;
    public IsPickedFromWizard: boolean = false;
    public TariffType: string;
    public FreightLabel: string;
    public OriginDependencyFilterValue: string = "A";
    public DestinationDependencyFilterValue = "A";

    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myDomainService = new TariffDomainService();
        this.myChargesTypeListService = new ChargesTypeListService();
        this.SetUIProperties();
        this.Date = DateTool.GetCurrentDateAsUtc();
        this.CalculateDefaultCurrency();
        this.dimenstionShipment = new ShipmentPM();
    }

    private CalculateDefaultCurrency() {
        var CurrencyList: CurrencyList[] = [];
        var myService: CurrencyListService = new CurrencyListService();
        myService.getAllFromCache().subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                CurrencyList = myResult.Result;
                var usdCurrency = CurrencyList.filter(c => c.Code == "USD" && c.Tenant == SessionLocator.Tenant)[0];
                if (usdCurrency != null) {
                    this.CurrencyId = usdCurrency.Id;
                }
                else {
                    this.CurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
                }
            }
        });
    }
    SetPortsDependencyFilterValue() {
        if (this.TariffType == "OLC" || this.TariffType == "OFC") {
            this.OriginDependencyFilterValue = "O";
            this.DestinationDependencyFilterValue = "O";
        }
    }
    private currencyId: string;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(newValue: string) {
        if (this.currencyId != newValue) {
            this.currencyId = newValue;
            this.CalculatePriceByCurrency();
        }
    }
    private CalculatePriceByCurrency() {

    }

    SetWindowArgs(args: any) {
        if (args != null) {
            var isAutorun = false; 
            if (args['ShipmentPM']) {
                this.ShipmentPM = args['ShipmentPM'];
                this.IsGeneratePayablesVisible = true;
                isAutorun = true;
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
           
            if (isAutorun) {
                this.SearchButtonClicked();
            }
        }
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
        //this.volume = AppTool.ComputePackageVolume(null, null, null, null, this.ChargeableWeight, this.Ratio, null, this.VolumeUnitCode, this.GrossWeightCode);
        this.weight = AppTool.ComputePackageVolumetricWeight(null, null, null, null, this.Volume, this.ChargeableWeight, this.Ratio, null, this.VolumeUnitCode, this.GrossWeightCode, this.WeightCode);
    }
    ComputeVolume() {
        this.volume = AppTool.ComputePackageVolume(null, null, null, null, this.ChargeableWeight, this.Ratio, null, this.VolumeUnitCode, this.GrossWeightCode);
    }

    ShowTariffclicked(item: TariffSearchSummary) {
        item.IsShown = !item.IsShown;
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

        if (AppTool.IsNullOrEmpty(this.Weight)) {
            this.ValidationErrorsList.push("Chargeable Weight is required");
        }

        if (AppTool.IsNullOrEmpty(this.WeightCode)) {
            this.ValidationErrorsList.push("Chargeable Weight unit is required");
        }

        if (this.TariffType == "OFC") { // validate the containers
            if (AppTool.IsNullOrEmpty(this.ContainerType1Id) && AppTool.IsNullOrEmpty(this.ContainerType2Id) && AppTool.IsNullOrEmpty(this.ContainerType3Id) && AppTool.IsNullOrEmpty(this.ContainerType4Id) && AppTool.IsNullOrEmpty(this.ContainerType5Id)) {
                this.ValidationErrorsList.push("You have to fill at least one Container type");
            }
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

            this.myDomainService.GetAvailableAirlineFreightTariffs(tariffSearchArgs).subscribe(res => {
                if (!res.HasError) {
                    if (res.Result) {
                        this.AvailableTariffs = res.Result;
                    }
                }
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    PriceClick(item: TariffSearchSummary) {
        if (item) {
            var editWindow = new LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            editWindow.Title = "Price Check";
            editWindow.Height = 770;
            editWindow.Width = 1500;
            editWindow.ShowEditComponent(item.TariffId, "Tariff", item.VersionId);
        }
    }

    ViewSurchargesClicked(item: TariffSearchSummary) {
        if (item && item.Surcharges != null) {
            var surcharge = item.Surcharges[0];
            var editWindow = new LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            editWindow.Title = "Price Check";
            editWindow.Height = 770;
            editWindow.Width = 1500;
            editWindow.ShowEditComponent(surcharge.TariffId, "Tariff", surcharge.VersionId);
        }
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
        this.UIProperties.SetRequired("Date", null, AppTool.IsNullOrEmpty(this.Date));
        this.UIProperties.SetRequired("Weight", null, AppTool.IsNullOrEmpty(this.Weight));

        if (this.IsPickedFromWizard) {
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("GrossWeightCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("WeightCode", this.ObjectTableName, false);

        }
        else {
            this.UIProperties.SetEnabled("Weight", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("Volume", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("GrossWeightCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("WeightCode", this.ObjectTableName, true);
           
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private Generator: ShipmentGenerator;
    private TariffPayables: ShipmentPayablePM[];
    GeneratePayablesClicked(item: TariffSearchSummary) {
        var isValid = this.ValidateExistConnectedTariff(item);
        if (isValid) {
            isValid = this.ValidateTariffClosedLines();
            if (isValid) {
                this.TariffPayables = [];
                this.Generator = new ShipmentGenerator(this.FatherComponent.EntityPM, this.FatherComponent.AllRates);
                // Generate Air Frieght
                var notes = null;
                if (!AppTool.IsNullOrEmpty(item.AllIn)) {

                    notes = "Includes the following charges as all-in: " + item.AllIn;
                }

                this.AddNewTariffPayable(item, notes);
                // Generate Surcharges
                if (item != null && item.Surcharges != null) {
                    item.Surcharges.forEach(surcharge => {
                        this.AddNewTariffPayable(surcharge);
                    });
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
        this.TariffPayables.forEach(payable => {
            var existsPayable: ShipmentPayablePM = this.ShipmentPM.ShipmentPayables.filter(d => d.ChargesTypeId == payable.ChargesTypeId && d.MeasurementId == payable.MeasurementId && (d.TariffId == payable.TariffId || d.TariffId == null))[0];
            if (existsPayable != null) {
                this.ShipmentPM.RemovePayable(existsPayable);
            }
        });
        this.AssignTariffPayablesToShipment();
    }
    AssignTariffPayablesToShipment(): any {
        this.TariffPayables.forEach(shipmentPayable => {
            this.ShipmentPM.AddPayable(shipmentPayable);
            
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
        this.TariffPayables.forEach(payable => {
            var existsPayable: ShipmentPayablePM = this.ShipmentPM.ShipmentPayables.filter(d => d.ChargesTypeId == payable.ChargesTypeId && d.MeasurementId == payable.MeasurementId && (d.TariffId == payable.TariffId || d.TariffId == null))[0];
            if (existsPayable != null) {
                isDuplicate = true;
            }
        });
        return isDuplicate;
    }
    AddNewTariffPayable(newRecord: any, notes = null) {
        this.myChargesTypeListService.getSingleFromCache(newRecord.ChargeTypeId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var chargesType: ChargesTypeList = myResponse.Result;
                var shipmentPayable: ShipmentPayablePM = this.Generator.GeneratePayablesFromTariff(chargesType);
                shipmentPayable.TariffId = newRecord.TariffId;
                shipmentPayable.TariffNumber = newRecord.TariffNumber;
                shipmentPayable.TariffVersion = newRecord.VersionId != null ? newRecord.VersionId.toString() : newRecord.VersionId;
                shipmentPayable.CurrencyId = newRecord.CurrencyId;
                this.Generator.GetCurrencyCode(shipmentPayable);
                shipmentPayable.Rate = this.Generator.GetCurrencyRate(shipmentPayable.CurrencyId);
                shipmentPayable.ProfitCurrencyExchangeRate = this.Generator.GetCurrencyRate(this.ShipmentPM.ProfitCurrencyId);
                shipmentPayable.MeasurementId = newRecord.UnitOfMesurmentId;
                shipmentPayable.MeasurementCode = newRecord.UnitOfMesurmentCode;
                var nweQuantity = this.GetQuantity(newRecord.UnitOfMesurmentCode);
                var expectedAmount = newRecord.ActualPrice;
                var rate = this.Generator.GetCurrencyRate(newRecord.CurrencyId);
                var expectedAmountLocal = expectedAmount * rate;

                var profitCurrencyExchangeRate = this.Generator.GetCurrencyRate(this.ShipmentPM.ProfitCurrencyId);
                var expectedAmountProfit = expectedAmountLocal / profitCurrencyExchangeRate;

                if (nweQuantity != null) {
                    shipmentPayable.UnitPrice = newRecord.ActualPrice != null ? AppTool.Round(newRecord.ActualPrice / nweQuantity, 3) : null;
                    shipmentPayable.Quantity = AppTool.Round(nweQuantity, 3);
                }

                shipmentPayable.ExpectedAmount = AppTool.Round(expectedAmount, 2);
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
                shipmentPayable.ShipmentId = this.ShipmentPM.Id;
                shipmentPayable.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
                shipmentPayable.CreateDate = DateTool.GetCurrentDateAsUtc();
                shipmentPayable.Tenant = this.ShipmentPM.Tenant;
                shipmentPayable.CreatedByUserId = SessionLocator.LoggedUserId;
                shipmentPayable.UpdateDate = DateTool.GetCurrentDateAsUtc();
                shipmentPayable.UpdateByUserId = SessionLocator.LoggedUserId;
                shipmentPayable.Notes = notes;
                shipmentPayable.VendorId = newRecord.SellerId;
                shipmentPayable.VendorName = newRecord.SellerName;
                this.TariffPayables.push(shipmentPayable);

                var payableItem = new ShipmentPayableItem(shipmentPayable, this.FatherComponent, false);
                this.FatherComponent.ItemsSource.Insert(payableItem);
                payableItem.ChargesTypeId = shipmentPayable.ChargesTypeId;
                payableItem.MeasurementId = shipmentPayable.MeasurementId;
                payableItem.CurrencyId = shipmentPayable.CurrencyId;
                payableItem.UnitPrice = shipmentPayable.UnitPrice;
                payableItem.MinAmount = newRecord.MinPrice;
            }
        });
    }

    ValidateExistConnectedTariff(item: TariffSearchSummary) {
        var isValid = true;
        var existsPayableOnAirFreight: ShipmentPayablePM = this.ShipmentPM.ShipmentPayables.filter(d => d.TariffId != null && d.TariffId != item.TariffId && d.ChargesTypeId == item.ChargeTypeId)[0];
        var existsPayableOnSurcharges: ShipmentPayablePM [] = []; 
        item.Surcharges.forEach(surcharge => {
            var payable = this.ShipmentPM.ShipmentPayables.filter(d => d.TariffId != null && d.TariffId != surcharge.TariffId && d.ChargesTypeId == surcharge.ChargeTypeId)[0];
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
        var closedPayablesLine: ShipmentPayablePM = this.ShipmentPM.ShipmentPayables.filter(d => d.AccountedAmount != null && d.AccountedAmount != 0)[0];
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
            case "BTEU": { myQuantity = this.ShipmentPM.TEU; break; }
            case "FIXD": { myQuantity = 1; break; }
            case "PRVL": { myQuantity = this.ShipmentPM.ValueOfGoods; break; }
            case "PRFR": { myQuantity = ArrayTool.Sum(this.ShipmentPM.ShipmentPayables.filter(d => d.ChargesGroupCode == "FRT" && AppTool.IsNullOrEmpty(d.ShipmentPayableParentId)), "ExpectedAmount"); break; }
            case "GWTN": { myQuantity = this.GrossWeightPerTon; break; }
            case "CWKG": { myQuantity = this.ChargeableWeightInKG; break; }
            case "GWKG": { myQuantity = this.GrossWeightInKG; break; }
            case "QTY": { myQuantity = this.ShipmentPM.NumberOfPackages != null ? this.ShipmentPM.NumberOfPackages: null; break; }
            case "VCBM": { myQuantity = this.VolumeInCBM; break; }
            default: { break; }
        }
        return myQuantity;
    }

}



