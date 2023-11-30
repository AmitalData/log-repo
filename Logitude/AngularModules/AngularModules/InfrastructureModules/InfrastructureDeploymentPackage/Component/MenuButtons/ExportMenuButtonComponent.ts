import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { DeploymentPackageExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DeploymentPackageExtendedPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { DeploymentPackagePMService } from '../../../../Infrastructure/Services/StandardPMs/DeploymentPackagePMService';
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
    private deploymentPackagePMService: DeploymentPackagePMService;

    constructor() {
        super();
        this.DeploymentPackageExtendedPMService = new DeploymentPackageExtendedPMService();
        this.deploymentPackagePMService = new DeploymentPackagePMService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.IsExported = this.EntityPM.IsExported;
        this.LoadMissingPackageDependiencies();
    }

    LoadMissingPackageDependiencies() {
        if (this.IsExported) return;
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

        this.CurrentSession.StartBusyIndicator("Exporting ... ");

        this.EntityPM.IsExported = true;

        this.deploymentPackagePMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
            if (response.HasError) return;
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            this.IsExported = true;
            this.CurrentSession.StopBusyIndicator();
        });
    }

    DownloadFile() {
        DownloadManager.DownloadPage(this.EntityPM.DocumentId, null, true);
    }
}
