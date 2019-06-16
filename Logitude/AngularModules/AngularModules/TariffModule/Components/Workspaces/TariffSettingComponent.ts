import { Component } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { TariffSettingPM } from '../../../TariffModule/EntityPMs/TariffSettingPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TariffDomainService } from '../../../TariffModule/Services/TariffDomainService';
import { TariffSettingPMService } from '../../../TariffModule/Services/StandardPMs/TariffSettingPMService';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { TenantPMService } from '../../../Common/Services/StandardPMs/TenantPMService';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';

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
    private TenantPM: TenantPM = new TenantPM();

    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.GetTenantPMMethod();
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

    private GetTenantPMMethod() {
        var myService: TenantPMService = new TenantPMService();
        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            this.TenantPM = response.Result;
           
        });
    }


    get DefaultWarningPercentage() {
        if (this.TenantPM != null) {
            return this.TenantPM.DefaultWarningPercentage;
        }
    }
    set DefaultWarningPercentage(value: number) {
        if (this.TenantPM.DefaultWarningPercentage != value) {
            this.TenantPM.DefaultWarningPercentage = value;
        }
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

        var index: number = 0;
        Steps.forEach(item => {
            this.ItemsSource.push(new TariffSettingStep(item, index, this));
            index++;
        });

        if (Steps.length < 8) {
            for (var i = Steps.length; i < 8; i++) {
                this.ItemsSource.push(new TariffSettingStep(null, index, this));
                index++;
            }
        }
    }
    BuildDefaultPriceSteps() {

        var iDefaultPriceSteps: string = null;

        this.ItemsSource.filter(f => f.Step != null).sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 }).forEach(item => {
            if (AppTool.IsNullOrEmpty(iDefaultPriceSteps)) {
                iDefaultPriceSteps = "" + item.Step;
            }

            else {
                iDefaultPriceSteps += "," + item.Step;
            }
        });

        this.DefaultPriceSteps = iDefaultPriceSteps;
    }

    CancelButtonClicked() {
        this.TenantPM = null;
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        this.UpdateTenant();
        if (!this.EntityPM.IsDirty && !this.TenantPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            var errors: string[] = [];
            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

            var isValidSort: boolean = true;
            var SortedItemStep: number = 0;

            this.ItemsSource.filter(f => f.Step != null).sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 }).forEach(item => {
                if (SortedItemStep == 0) {
                    SortedItemStep = item.Step;
                }

                else {
                    if (item.Step <= SortedItemStep) {
                        isValidSort = false;
                    }

                    else {
                        SortedItemStep = item.Step;
                    }
                }
            });

            if (!isValidSort) {
                errors.push("Price steps must be sorted");
            }

            this.ValidationErrorsList =  this.ValidationErrorsList.concat(errors);

            if (this.ValidationErrorsList.length == 0) {

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

    UpdateTenant() {
        if (this.DefaultWarningPercentage == null || this.DefaultWarningPercentage > 100 || this.DefaultWarningPercentage < 0) {
            this.ValidationErrorsList.push("Warning Percentage must be between 0-100");
        }
        else {
            var myService: TenantPMService = new TenantPMService();
            this.TenantPM.DefaultWarningPercentage = this.DefaultWarningPercentage;
            myService.update(this.TenantPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse) {
                    if (!myResponse.HasError) {
                        InfraSettings.TenantPM = this.TenantPM;
                    }
                }
            });
        }
    }
}

class TariffSettingStep extends BaseComponent {
    public Index: number;
    public DataContext = this;
    constructor(iStep: string, index: number, private father: TariffSettingComponent) {
        super();

        this.Index = index;

        if (iStep) {
            this.step = +iStep;
        }
    }

    private step: number = null;
    get Step() { return this.step; }
    set Step(value: number) {
        if (this.step != value) {
            this.step = value;
            this.father.BuildDefaultPriceSteps();
        }
    }

    DeleteClicked() {
        this.Step = null;
    }
}
