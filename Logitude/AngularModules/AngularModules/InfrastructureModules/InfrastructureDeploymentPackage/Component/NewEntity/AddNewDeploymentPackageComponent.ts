import { Component, QueryList, ViewChild, ViewChildren, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { DeploymentPackagePMService } from '../../../../Infrastructure/Services/StandardPMs/DeploymentPackagePMService';
import { FilterClass } from '../../../../Shipment/Components/NewEntity/NewShipmentComponent';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';

@Component({

    templateUrl: './AddNewDeploymentPackageComponent.html',
})


export class AddNewDeploymentPackageComponent extends BaseComponent {
    public EntityPM: DeploymentPackagePM;
    public DataContext: AddNewDeploymentPackageComponent = this;
    public ObjectTableName: string = "DeploymentPackage";
    public deploymentPackageService: DeploymentPackagePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public DirectionsList: FilterClass[] = [];
    public SessionIndex: number;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild("EXPORT", { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

    constructor() {
        super();
        this.deploymentPackageService = new DeploymentPackagePMService();
        this.EntityPM = new DeploymentPackagePM();
        this.SessionIndex = this.CurrentSession.SessionIndex;
        this.BuildDirectionsList();
        this.RunComponent();
    }

    BuildDirectionsList() {
        this.DirectionsList = [];
        this.DirectionsList.push(new FilterClass("E", "Export"));
        this.DirectionsList.push(new FilterClass("I", "Import"));
        this.DirectionId = "E";
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (!this.AllLocations || this.AllLocations.toArray().length == 0) {
            this.RunComponentTimer();
            return;
        }
        this.isLoaderReady = true;
        this.ChangeComponent();

    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    ChangeComponent() {
        if (!this.DirectionId || !this.isLoaderReady) return;
        this.ShowSelectedDirectionComponent();
    }

    ShowSelectedDirectionComponent() {

        let selectedDirectionCode = (this.DirectionId && this.DirectionId == "I") ? "IMPORT" : "EXPORT";
        let importDeplymentPackageComponentPath = "./InfrastructureModules/InfrastructureDeploymentPackage/Component/NewEntity/NewImportDeploymentPackageComponent";
        let exportDeplymentPackageComponentPath = "./InfrastructureModules/InfrastructureDeploymentPackage/Component/NewEntity/NewExportDeploymentPackageComponent";
        let selectedDirectionComponentPath = (this.DirectionId && this.DirectionId == "I") ? importDeplymentPackageComponentPath : exportDeplymentPackageComponentPath;

        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == selectedDirectionCode)[0];

        if (myLocation != null) {
            myLocation.viewContainerRef.clear();
        }

        SessionLocator.DynamicLoader.Load(selectedDirectionComponentPath, myLocation.viewContainerRef)
            .then(cmpRef => {
                this.SelectedDirectionComponent = cmpRef.instance;
                cmpRef.instance.EntityPM = this.EntityPM;
                cmpRef.instance.AddNewDeploymentPackageComponent = this;
                cmpRef.instance.Run();
            });
    }

    public SelectedDirectionComponent: any;

    get IsDirectionSelectionDisabled() {
        return this.SelectedDirectionComponent && (this.SelectedDirectionComponent.IsUploadInProgress || this.SelectedDirectionComponent.UploadedSuccessfully);
    }

    get IsControlButtonsDisabled() {
        return this.SelectedDirectionComponent && this.SelectedDirectionComponent.IsUploadInProgress;
    }

    get IsNextButtonClicked() {
        return this.SelectedDirectionComponent && this.SelectedDirectionComponent.IsNextClicked;
    }

    get HasErrorsWhileImporting() {
        return this.SelectedDirectionComponent && this.SelectedDirectionComponent.HasErrorsWhileImporting;
    }
    get DirectionId() { return this.EntityPM.DirectionId; }
    set DirectionId(newValue: string) {
        if (this.EntityPM.DirectionId == newValue) return;
        this.EntityPM.DirectionId = newValue;
        this.ChangeComponent();
    }


    CancelButtonClicked() {
        this.SelectedDirectionComponent.CancelButtonClicked();
    }

    public CreateButtonClicked() {
        this.SelectedDirectionComponent.ValidateDeploymentPackage();
        if (this.SelectedDirectionComponent.ValidationErrorsList.length > 0) return;
        this.CurrentSession.StartBusyIndicator("Saving ...");
        this.CreateDeploymentPackage();
    }

    CreateDeploymentPackage() {

        this.deploymentPackageService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
            if (!response.HasError && response.Result && this.EntityPM.DirectionId == "I") {
                this.SelectedDirectionComponent.StartCheckDeploymentPackageDeployViaWorkerRoleTimer(response.Result.PackageExecutionLogId);
            }
            if (!response.HasError && this.EntityPM.DirectionId == "E") {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindow();
                return;
            }
            if (response.HasError && response.ErrorsArray?.length > 0 && this.EntityPM.DirectionId == "I") {
                this.CurrentSession.StopBusyIndicator();
                this.ShowMessage(response.ErrorsArray[0]);
                return;
            }
            
            if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                this.SelectedDirectionComponent.ValidationErrorsList.push(response.ErrorsArray[0]);
                this.CurrentSession.StopBusyIndicator();
            }

        });

    }
    public ShowMessage(message: string) {

        const messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }
    NextButtonClicked() {
        if (!this.SelectedDirectionComponent) return;
        this.SelectedDirectionComponent.NextButtonClicked();
    }

    DeployButtonClicked() {
        if (!this.SelectedDirectionComponent) return;
        this.SelectedDirectionComponent.DeployButtonClicked();
    }

    PreviousButtonClicked() {
        if (!this.SelectedDirectionComponent) return;
        this.SelectedDirectionComponent.IsNextClicked = false;
    }
}
