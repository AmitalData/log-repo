import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { DeploymentPackageExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DeploymentPackageExtendedPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
declare var window: any;

@Component({

    templateUrl: './ExportMenuButtonComponent.html',
})

export class ExportMenuButtonComponent extends BaseComponent {
    public EntityPM: DeploymentPackagePM;
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: ExportMenuButtonComponent = this;
    public IsExported: boolean;
    public IsValidForExport: boolean;
    public ValidationErrorsList: string[] = [];
    public MissingPackageDependiencies: [] = [];
    private DeploymentPackageExtendedPMService: DeploymentPackageExtendedPMService;
    constructor() {
        super();
        this.DeploymentPackageExtendedPMService = new DeploymentPackageExtendedPMService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.LoadMissingPackageDependiencies();
    }

    LoadMissingPackageDependiencies() {
        this.IsValidForExport = false;
        this.ValidationErrorsList = [];
        this.MissingPackageDependiencies = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.DeploymentPackageExtendedPMService.ValidateDependiencies(this.EntityPM).subscribe((serviceResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (serviceResponse.HasError) {
                this.ValidationErrorsList.push(serviceResponse.ErrorsArray?.toString());
                return;
            }

            this.FillMissingPackageDependiencies(serviceResponse.Result);
        });
    }
    FillMissingPackageDependiencies(Result: any) {
        if (!Result) return;
        if (Result.IsValid) {
            this.IsValidForExport = true;
            return;
        }
        this.IsValidForExport = false;
        this.MissingPackageDependiencies = Result.MissingDependiencies;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

    ExportButtonClicked() {
        this.ValidationErrorsList = [];
        if (!this.IsValidForExport || this.IsExported) {
            this.ValidationErrorsList.push("Not Valid For Export");
            return;
        }
        this.IsExported = true;
        //this.CurrentSession.CloseCurrentWindowEmit("Exported");
    }
}
