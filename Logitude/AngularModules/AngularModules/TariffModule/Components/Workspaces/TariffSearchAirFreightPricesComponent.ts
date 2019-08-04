import { Component } from '@angular/core';
import { AppTool,DateTool } from '../../../Infrastructure/Tools';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TariffDomainService, SurchargeSummary, TariffSummery } from '../../../TariffModule/Services/TariffDomainService';
import { TariffSearchSummary } from '../../../TariffModule/Services/TariffDomainService';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { CurrencyList } from '../../../Common/EntityLists/CurrencyList';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';


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
    public AvailableTariffs: Array<TariffSearchSummary>= [];
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myDomainService = new TariffDomainService();
        this.SetUIProperties();
        this.Date = DateTool.GetCurrentDateAsUtc();
        this.CalculateDefaultCurrency(); 
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
            this.originPortId = args['FromPort'];
            this.destinationPortId = args['ToPort'];
            this.date = args['BetweenDate'];
            this.weight = args['ChargeableWeight'];
            this.ChargeableWeight = args['ChargeableWeight'];
            this.weightCode = args['ChargeableWeightUnit'];
            this.grossWeight = args['GrossWeight'];
            this.grossWeightCode = args['GrossWeightUnit'];
            this.volume = args['Volume'];
            this.volumeUnitCode = args['VolumeUnit'];
            this.SetUIProperties();
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
    

    private weightCode: string="KG";
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



    private ComputeChargeableWeight_Kg() {
        var weigh_Kg: number = null;
        var weigh_Ton: number = null;

        if (this.Weight != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.WeightCode)) {
                switch (this.WeightCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }
                    case "MT": { factorOfConvert = 1000; break; }
                }
            }

            weigh_Kg = this.Weight * factorOfConvert;
        }

        if (weigh_Kg != null) {
            weigh_Kg = AppTool.Round(weigh_Kg, 3);
        }
        this.Weight = weigh_Kg;
    }


    private ComputeVolumetricWeight() {
        //this.volume = AppTool.ComputePackageVolume(null, null, null, null, this.ChargeableWeight, this.Ratio, null, this.VolumeUnitCode, this.GrossWeightCode);

        this.weight = AppTool.ComputePackageVolumetricWeight(null, null, null, null, this.Volume, this.ChargeableWeight, this.Ratio, null, this.VolumeUnitCode, this.GrossWeightCode, this.WeightCode);
    }


    

    private chargeableWeight: number;
    get ChargeableWeight() {
        return this.chargeableWeight;
    }
    set ChargeableWeight(value: number) {
        if (this.chargeableWeight != value) {
            this.chargeableWeight = value;
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
            this.SetUIProperties();

        }
    }

    private ComputeVolume() {
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

        if (this.ValidationErrorsList.length == 0) {
            if (AppTool.IsNullOrEmpty(this.GrossWeight)) {
                this.GrossWeight = this.Weight;
            }

            if (AppTool.IsNullOrEmpty(this.Volume)) {
                this.ComputeVolume();
            }

            this.CurrentSession.StartBusyIndicatorLoading();
            this.myDomainService.GetAvailableAirlineFreightTariffs(this.OriginPortId, this.DestinationPortId, this.Date, this.Weight, this.WeightCode, this.GrossWeight, this.GrossWeightCode, this.Volume, this.VolumeUnitCode, this.CurrencyId).subscribe(res => {
                if (!res.HasError) {
                    if (res.Result) {
                        this.AvailableTariffs = res.Result;
                    }
                }
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    private ComputeWeightInKG(weight: number) {
        var weigh_Kg: number = null;
        var weigh_Ton: number = null;

        if (this.Weight != null) {
            var factorOfConvert: number = 1;

            if (!AppTool.IsNullOrEmpty(this.WeightCode)) {
                switch (this.WeightCode.toUpperCase()) {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }
                    case "MT": { factorOfConvert = 1000; break; }
                }
            }

            weigh_Kg = this.Weight * factorOfConvert;
        }

        if (weigh_Kg != null) {
            weigh_Kg = AppTool.Round(weigh_Kg, 3);
        }
        return weigh_Kg;    
    }

    PriceClick(item: TariffSearchSummary) {
        if (item) {
            var editWindow = new LogitudeWindow();
            editWindow.ShowHeaderButtons = true;
            editWindow.Title = "Price Check";
            editWindow.Height = 770;
            editWindow.Width = 1500;
            editWindow.ShowEditComponent(item.Id, "Tariff", item.VersionId);
        }

    }


    private SetUIProperties() {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DestinationPortId));
        this.UIProperties.SetRequired("Date", null, AppTool.IsNullOrEmpty(this.Date));
        this.UIProperties.SetRequired("Weight", null, AppTool.IsNullOrEmpty(this.Weight));


    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}



