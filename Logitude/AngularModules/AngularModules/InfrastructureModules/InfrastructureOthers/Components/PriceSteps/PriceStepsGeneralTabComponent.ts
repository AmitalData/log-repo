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
import { TenantPM } from "../../../../Common/EntityPMs/TenantPM";
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { PriceStepsPMService } from '../../../../Infrastructure/Services/StandardPMs/PriceStepsPMService';

@Component({
    selector: 'PriceStepsGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './PriceStepsGeneralTabComponent.html',
})

export class PriceStepsGeneralTabComponent extends BaseComponent implements OnInit {
    public DataContext: PriceStepsGeneralTabComponent = this;
    public ObjectTableName: string = "PriceSteps";
    public EntityPM: PriceStepsPM;
    public TenantPM: TenantPM;
    public PriceStepsText: string;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.TenantPM = SessionLocator.TenantPM;
        this.Listen();
    }

    InitializeNewEntity() {
        this.EntityPM = new PriceStepsPM();
        var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
    }

    ngOnInit() {
        this.GetTenantTariffSetting();
        if (this.EntityPM == null) {
            this.InitializeNewEntity();
        } else {
            this.UpdateEntity();
        }
    }

    UpdateEntity() {
        var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
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

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                    //this.SetUIProperties();
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    //this.SetUIProperties();
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


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);

        //if (AppTool.IsNullOrEmpty(this.Description)) {
        //    errors.push("Description is required");
        //}
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreatingPriceSteps();
        }
    }

    SubmitCreatingPriceSteps() {
        var service: PriceStepsPMService = new PriceStepsPMService();

        service.insert(this.EntityPM).subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                if (!myResult.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}
