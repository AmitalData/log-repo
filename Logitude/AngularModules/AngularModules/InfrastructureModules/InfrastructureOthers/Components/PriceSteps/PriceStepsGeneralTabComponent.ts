import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DateTool } from '../../../../Infrastructure/Tools';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { PriceStepsPM } from '../../../../Infrastructure/EntityPMs/PriceStepsPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TariffDomainService } from '../../../../TariffModule/Services/TariffDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TariffSettingPM } from '../../../../TariffModule/EntityPMs/TariffSettingPM';

@Component({
    selector: 'PriceStepsGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './PriceStepsGeneralTabComponent.html',
})

export class PriceStepsGeneralTabComponent extends BaseComponent implements OnInit {
    public DataContext: PriceStepsGeneralTabComponent = this;
    public ObjectTableName: string = "PriceSteps";
    public EntityPM: PriceStepsPM;
    public PriceStepsText: string;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.InitializeEntity();
    }

    InitializeEntity() {
        this.EntityPM = this.entityArgs.EntityPM;
        var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
    }

    ngOnInit() {
        this.GetTenantTariffSetting();
    }
    
    EditPriceSteps() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Price Steps";
        logWindow.WindowArgs = this.Steps;
        logWindow.Show("./TariffModule/Components/NewEntity/TariffPriceStepsComponent");
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d != "cancel") {
                    var steps = s.DefaultPriceSteps;
                    this.EntityPM.Steps = steps;
                    this.Steps = steps;
                    this.PriceStepsText = this.GetPriceSteps(this.Steps);
                }
            });
        });
    }
    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get Steps() { return this.EntityPM.Steps; }
    set Steps(value: string) {
        if (this.EntityPM.Steps != value) {
            this.EntityPM.Steps = value;
        }
    }

    GetPriceSteps(steps: string) {
        if (steps) {
            var stpesWithSpaces = steps.split(',').join(', ');
            return stpesWithSpaces;
        }
    }

    GetTenantTariffSetting() {
        var tariffDomainService = new TariffDomainService();
        tariffDomainService.GetTenantTariffSetting().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var entity: TariffSettingPM = myResponse.Result;
                this.EntityPM.Steps = entity.DefaultPriceSteps;
                this.PriceStepsText = this.GetPriceSteps(entity.DefaultPriceSteps);
            }
        });
    }
}
