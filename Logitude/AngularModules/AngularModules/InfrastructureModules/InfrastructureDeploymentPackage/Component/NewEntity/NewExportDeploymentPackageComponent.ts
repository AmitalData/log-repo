import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeploymentPackagePM } from '../../../../Infrastructure/EntityPMs/DeploymentPackagePM';
import { AddNewDeploymentPackageComponent } from './AddNewDeploymentPackageComponent';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';

@Component({

    templateUrl: './NewExportDeploymentPackageComponent.html',
})

export class NewExportDeploymentPackageComponent extends BaseComponent {
    public EntityPM: DeploymentPackagePM;
    public ValidationErrorsList: string[] = [];
    public DataContext: NewExportDeploymentPackageComponent = this;
    public ObjectTableName: string = "DeploymentPackage";
    private CurrentSession = SessionLocator.SelectedSession;
    public AddNewDeploymentPackageComponent: AddNewDeploymentPackageComponent;
    constructor() {
        super();
        this.UIProperties.SetRequired("Code", this.ObjectTableName, true);
        this.UIProperties.SetRequired("Name", this.ObjectTableName, true);
    }

    Run() {
        this.ClearScreenFields();
        this.SetDefaultFields();
    }

    private ClearScreenFields() {
        this.Code = "";
        this.Name = "";
        this.Description = "";
        this.ValidationErrorsList = [];
    }
    private SetDefaultFields() {
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreatedBy = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedBy = SessionLocator.LoggedUserId;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.DirectionId = "E";
    }

    public get Code() { return this.EntityPM.Code }
    public set Code(value: string) {
        if (this.EntityPM.Code == value) return;
        this.EntityPM.Code = value;
    }

    public get Name() { return this.EntityPM.Name }
    public set Name(value: string) {
        if (this.EntityPM.Name == value) return;
        this.EntityPM.Name = value;
        this.Code = this.Code = AppTool.Replace(value?.toLowerCase(), " ", "_");
    }

    public get Description() { return this.EntityPM.Description }
    public set Description(value: string) {
        if (this.EntityPM.Description == value) return;
        this.EntityPM.Description = value;
    }

    public ValidateDeploymentPackage() {

        let errors = [];

        if (!this.EntityPM.Code)
            errors.push("Code Field is Required");

        if (!this.EntityPM.Name)
            errors.push("Name Field is Required");

        if (this.EntityPM.Code && this.EntityPM.Code.length > 100)
            errors.push("Maximum length of Code Field is 100");

        if (this.EntityPM.Name && this.EntityPM.Name.length > 100)
            errors.push("Maximum length of Name Field is 100");

        this.ValidationErrorsList = errors;

    }

    public CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
