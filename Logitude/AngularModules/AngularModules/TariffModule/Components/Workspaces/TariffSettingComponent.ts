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
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    
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
    public IsAirEditBtnEnabled = false;
    public IsLCLEditBtnEnabled = false;
    public AirDefaultStepsName: string;
    public LCLDefaultStepsName: string;

    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myService = new TariffSettingPMService();
        this.myDomainService = new TariffDomainService();
        this.GetSingletariffSetting();
        this.EntityPM = new TariffSettingPM();
    }

    private GetSingletariffSetting() {
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

                    if (AppTool.IsNullOrEmpty(this.DefaultCurrencyId)) {
                        this.DefaultCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
                    }

                    this.BuildItemsSource();
                    this.IsResourcesReady = true;
                    this.SetUIPropertiesForEditButtons();
                    this.SetUIPropertiesOfFields();
                }
            });
        });
    }
    private SetUIPropertiesForEditButtons() {
        this.IsLCLEditBtnEnabled = false;
        this.IsAirEditBtnEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.AirDefaultStepsId)) {
            this.IsAirEditBtnEnabled = true;
        }
        if (!AppTool.IsNullOrEmpty(this.LCLDefaultStepsId)) {
            this.IsLCLEditBtnEnabled = true;
        }
    }

    private SetUIPropertiesOfFields() {
        this.UIProperties.SetRequired("AirDefaultStepsId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.AirDefaultStepsId));
        this.UIProperties.SetRequired("LCLDefaultStepsId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.LCLDefaultStepsId));
        this.UIProperties.SetEnabled("ContainerDefaults", this.ObjectTableName, false);
        this.UIProperties.SetRequired("DefaultCurrencyId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.DefaultCurrencyId));
    }

    get DefaultWarningPercentage() {
        if (this.EntityPM != null) {
            return this.EntityPM.DefaultWarningPercentage;
        }
    }
    set DefaultWarningPercentage(value: number) {
        if (this.EntityPM.DefaultWarningPercentage != value) {
            this.EntityPM.DefaultWarningPercentage = value;
        }
    }

    get LCLDefaultStepsId() {
        if (this.EntityPM != null) {
            return this.EntityPM.LCLDefaultStepsId;
        }
    }
    set LCLDefaultStepsId(value: string) {
        if (this.EntityPM.LCLDefaultStepsId != value) {
            this.EntityPM.LCLDefaultStepsId = value;
            this.SetUIPropertiesForEditButtons();
            this.SetUIPropertiesOfFields();
        }
    }

    get ContainerDefaults() {
        if (this.EntityPM != null) {
            return this.EntityPM.ContainerDefaults;
        }
    }
    set ContainerDefaults(value: string) {
        if (this.EntityPM.ContainerDefaults != value) {
            this.EntityPM.ContainerDefaults = value;
        }
    }

    get AirDefaultStepsId() {
        if (this.EntityPM != null) {
            return this.EntityPM.AirDefaultStepsId;
        }
    }
    set AirDefaultStepsId(value: string) {
        if (this.EntityPM.AirDefaultStepsId != value) {
            this.EntityPM.AirDefaultStepsId = value;
            this.SetUIPropertiesForEditButtons();
            this.SetUIPropertiesOfFields();
        }
    }

    get DefaultPriceSteps() { return this.EntityPM.DefaultPriceSteps; }
    set DefaultPriceSteps(value: string) {
        if (this.EntityPM.DefaultPriceSteps != value) {
            this.EntityPM.DefaultPriceSteps = value;

        }
    }

    get DefaultCurrencyId() { return this.EntityPM.DefaultCurrencyId; }
    set DefaultCurrencyId(value: string) {
        if (this.EntityPM.DefaultCurrencyId != value) {
            this.EntityPM.DefaultCurrencyId = value;

            this.SetUIPropertiesOfFields();
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
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (!this.EntityPM.IsDirty && !AppTool.IsNullOrEmpty(this.AirDefaultStepsId) && !AppTool.IsNullOrEmpty(this.LCLDefaultStepsId)) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            var errors: string[] = [];
            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

            if (this.DefaultWarningPercentage == null || this.DefaultWarningPercentage > 100 || this.DefaultWarningPercentage < 0) {
                errors.push("Warning Percentage must be between 0-100");
            }

            var isValidSort: boolean = true;
            var SortedItemStep: number = 0;

            this.ItemsSource.filter(f => f.Step != null).sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 }).forEach(item => {
                if (SortedItemStep == 0) {
                    SortedItemStep = item.Step;
                }

                else {
                    if (Number(item.Step) <= Number(SortedItemStep)) {
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

            if (AppTool.IsNullOrEmpty(this.AirDefaultStepsId)) {
                errors.push("Air Default Steps field is required");
            }

            if (AppTool.IsNullOrEmpty(this.LCLDefaultStepsId)) {
                errors.push("LCL Default Steps field is required");
            }

            if (AppTool.IsNullOrEmpty(this.DefaultCurrencyId)) {
                errors.push("Default Currency field is required");
            }

            this.ValidationErrorsList = this.ValidationErrorsList.concat(errors);

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

    EditPriceStepsClicked(type: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Price Steps";
        if (type == "LCL") {
            logWindow.WindowArgs = this.LCLDefaultStepsId;
        }
        else if (type == "Air") {
            logWindow.WindowArgs = this.AirDefaultStepsId;
        }
        this.ShowPriceStepsWindow(logWindow, type);
    }

    AddPriceStepsClicked(type: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Price Steps";
        logWindow.WindowArgs = null;
        this.ShowPriceStepsWindow(logWindow, type);
    }

    ShowPriceStepsWindow(logWindow: LogitudeWindow, type: string) {
        logWindow.Show("./InfrastructureModules/InfrastructureOthers/Components/PriceSteps/PriceStepsGeneralTabComponent");
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d && d != "cancel") {
                    if (type == "LCL") {
                        this.LCLDefaultStepsId = s.EntityPM.Id;
                        this.EntityPM.LCLUnitOfMeasurementCode = s.EntityPM.UnitOfMeasurementCode;

                    }
                    else if (type == "Air") {
                        this.AirDefaultStepsId = s.EntityPM.Id;
                        this.EntityPM.AirUnitOfMeasurementCode = s.EntityPM.UnitOfMeasurementCode;

                    }
                    this.CurrentSession.SessionEvent.emit("TariffStepsRefresh");
                }
            });
        });
    }

    EditContainerDefaultsClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Container Defaults";
        logWindow.WindowArgs = this.EntityPM;
        logWindow.Show("./TariffModule/Components/Workspaces/ContainerDefaultsComponent");
        logWindow.WindowClosed.subscribe(d => {
            if (d && d != "cancel") {
                
            }
        });
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
