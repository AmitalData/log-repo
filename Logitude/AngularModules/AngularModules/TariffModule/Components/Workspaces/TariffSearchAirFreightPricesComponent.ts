import { Component } from '@angular/core';
import { AppTool,DateTool } from '../../../Infrastructure/Tools';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TariffDomainService } from '../../../TariffModule/Services/TariffDomainService';
import { TariffSearchSummary } from '../../../TariffModule/Services/TariffDomainService';
import { Validator } from '../../../Infrastructure/Validators/Validator';

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
       }
    }

    private weight: number;
    get Weight() {
        return this.weight;
    }
    set Weight(value: number) {
        if (this.weight != value) {
            this.weight = value;
            this.SetUIProperties();
        }
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
            this.ValidationErrorsList.push("Weight is required");
        }

        if (AppTool.IsNullOrEmpty(this.WeightCode)) {
            this.ValidationErrorsList.push("Weight unit is required");
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorLoading();
            var computedWeight: number = this.ComputeWeightInKG(this.Weight);
            this.myDomainService.GetAvailableAirlineFreightTariffs(this.OriginPortId, this.DestinationPortId, this.Date, computedWeight).subscribe(res => {
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



