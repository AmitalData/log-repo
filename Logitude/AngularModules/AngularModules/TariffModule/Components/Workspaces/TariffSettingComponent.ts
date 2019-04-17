import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { TariffSettingPM } from '../../../TariffModule/EntityPMs/TariffSettingPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TariffDomainService } from '../../../TariffModule/Services/TariffDomainService';
import { TariffSettingPMService } from '../../../TariffModule/Services/StandardPMs/TariffSettingPMService';
import { Validator } from '../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './TariffSettingComponent.html',
})

export class TariffSettingComponent extends BaseComponent {
    public EntityPM: TariffSettingPM;
    public ObjectTableName = "TariffSetting";
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: TariffSettingStep[] = [];
    private myService: TariffSettingPMService;
    private myDomainService: TariffDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();

        this.myService = new TariffSettingPMService();
        this.myDomainService = new TariffDomainService();

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res1: any) => {
            this.myDomainService.GetTenantTariffSetting().subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.EntityPM = myResponse.Result;

                    if (!this.EntityPM) {
                        this.EntityPM = new TariffSettingPM();
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                    }

                    this.BuildItemsSource();
                    this.IsResourcesReady = true;
                }
            });
        });
    }

    get DefaultPriceSteps() { return this.EntityPM.DefaultPriceSteps; }
    set DefaultPriceSteps(value: string) {
        if (this.EntityPM.DefaultPriceSteps != value) {
            this.EntityPM.DefaultPriceSteps = value;
        }
    }

    BuildItemsSource() {
        var Steps: string[] = [];

        if (this.DefaultPriceSteps) {
            Steps = this.DefaultPriceSteps.split(',');
        }

        Steps.forEach(item => {
            this.ItemsSource.push(new TariffSettingStep(item, this));
        });

        if (Steps.length < 8) {
            for (var i = Steps.length; i < 8; i++) {
                this.ItemsSource.push(new TariffSettingStep(null, this));
            }
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            var errors: string[] = [];
            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

            //

            this.ValidationErrorsList = errors;

            if (errors.length == 0) {

                this.CurrentSession.StartBusyIndicatorSaving();

                if (this.EntityPM.Id) {
                    this.myService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            this.CurrentSession.CloseCurrentWindow();
                        }
                    });
                }

                else {
                    this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            this.CurrentSession.CloseCurrentWindow();
                        }
                    });
                }
            }
        }
    }
}

class TariffSettingStep {
    constructor(iStep: string, private father: TariffSettingComponent) {
        this.step = +iStep;
    }

    private step: number = null;
    get Step() { return this.step; }
    set Step(value: number) {
        if (this.step != value) {
            this.step = value;
        }
    }

    DeleteClicked() {
        this.Step = null;
    }
}
