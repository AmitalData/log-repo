import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DateTool, AppTool } from '../../../../Infrastructure/Tools';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { PriceStepPM } from '../../../../Infrastructure/EntityPMs/PriceStepPM';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { TariffDomainService } from '../../../../TariffModule/Services/TariffDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TariffSettingPM } from '../../../../TariffModule/EntityPMs/TariffSettingPM';
import { TenantPM } from "../../../../Common/EntityPMs/TenantPM";
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { PriceStepPMService } from '../../../../Infrastructure/Services/StandardPMs/PriceStepPMService';

@Component({
    selector: 'PriceStepsGeneralTabComponent',
    
    templateUrl: './PriceStepsGeneralTabComponent.html',
})

export class PriceStepsGeneralTabComponent extends BaseComponent implements OnInit {
    public DataContext: PriceStepsGeneralTabComponent = this;
    public ObjectTableName: string = "PriceStep";
    public EntityPM: PriceStepPM;
    public TenantPM: TenantPM;
    public PriceStepsText: string;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    public isNewEntity: boolean = false;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.TenantPM = SessionLocator.TenantPM;
        this.Listen();
    }

    ngOnInit() {
        if (this.EntityPM == null) {
            this.InitializeNewEntity();
        } else {
            this.GetPriceSteps(this.Steps);
        }
    }

    public PriceStepId: string; 
    SetWindowArgs(args) {
        this.PriceStepId = args;
        this.GetSinglePriceSteps();
    }

    GetSinglePriceSteps() {
        var service: PriceStepPMService = new PriceStepPMService();
        service.get(this.PriceStepId).subscribe((result: ServiceResponse) => {
            if (result) {
                if (!result.HasError) {
                    this.EntityPM = result.Result;
                    this.GetPriceSteps(this.Steps);
                    this.SetUIProperties_Steps();
                }
            }
        });
    }

    InitializeNewEntity() {
        this.EntityPM = new PriceStepPM();
        var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.Steps = "";
        this.isNewEntity = true;
    }
    
    EditPriceSteps() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Price Steps";
        logWindow.WindowArgs = this.Steps;
        logWindow.Show("./TariffModule/Components/NewEntity/TariffPriceStepsComponent");
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d && d != "cancel") {
                    var steps = s.DefaultPriceSteps;
                    this.Steps = steps;
                    this.GetPriceSteps(this.Steps);
                }
            });
        });
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get UnitOfMeasurementCode() { return this.EntityPM.UnitOfMeasurementCode; }
    set UnitOfMeasurementCode(value: string) {
        if (this.EntityPM.UnitOfMeasurementCode != value) {
            this.EntityPM.UnitOfMeasurementCode = value;
        }
    }

    get Steps() { return this.EntityPM.Steps; }
    set Steps(value: string) {
        if (this.EntityPM.Steps != value) {
            this.EntityPM.Steps = value;
            this.SetUIProperties_Steps();
        }
    }

    SetUIProperties_Steps() {
        this.UIProperties.SetRequired("Steps", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Steps));
    }
    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }

    GetPriceSteps(steps: string) {
        if (steps) {
            var stpesWithSpaces = steps.split(',').join(', ');
            this.PriceStepsText = stpesWithSpaces;
        }
    }

    InactiveChecked(checked: boolean) {
        if (checked) {
            this.Inactive = true;
        }
        else {
            this.Inactive = false;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, this.ValidationErrorsList);
        if (this.ValidationErrorsList.length == 0) {
            if (this.PriceStepId == null) {
                this.SubmitCreatingPriceSteps();
            }
            else {
                this.SubmitUpdatingPriceSteps();
            }
            
        }
    }

    SubmitCreatingPriceSteps() {
        var service: PriceStepPMService = new PriceStepPMService();

        service.insert(this.EntityPM).subscribe((result: ServiceResponse) => {
            if (result) {
                if (!result.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    this.ValidationErrorsList = result.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
    SubmitUpdatingPriceSteps() {
        var service: PriceStepPMService = new PriceStepPMService();
        service.update(this.EntityPM).subscribe((result: ServiceResponse) => {
            if (result) {
                if (!result.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    this.ValidationErrorsList = result.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}
