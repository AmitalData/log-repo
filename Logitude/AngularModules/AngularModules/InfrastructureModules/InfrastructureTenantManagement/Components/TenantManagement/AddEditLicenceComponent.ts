import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagementLicensePM} from '../../../../Infrastructure/EntityPMs/TenantManagementLicensePM';
import {PackageItem} from './TenantManagementGeneralTabComponent';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditLicenceComponent.html',
})

export class AddEditLicenceComponent {
    public EntityPM: TenantManagementLicensePM;
    public DataContext: PackageItem;
    public ObjectTableName: string = "TenantManagementLicense";
    public ValidationErrorsList: string[] = [];
    public IsNewEntity: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(dataContext: PackageItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNewEntity = dataContext.IsNew;

        this.Clone();
    }

    CancelButtonClicked() {
        //this.DataContext.ResetOldData();
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.IsNewEntity) {
            if (this.DataContext.fatherComponent.EntityPM.TenantManagementLicenses.filter(d => d.PackageCode == this.DataContext.PackageCode)[0]) {
                errors.push("Same package already exists");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            if (this.IsNewEntity) {
                this.IsNewEntity = false;

                this.DataContext.fatherComponent.EntityPM.AddTenantManagementLicensePM(this.DataContext.EntityPM);

                if (this.DataContext.fatherComponent.PackagesList.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.PackagesList.push(this.DataContext);
                }
            }

            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('PackageCode');
        this.myCloner.AddField('NumberOfUsers');
        this.myCloner.AddField('FreeUsers');
        this.myCloner.AddField('Price');
        this.myCloner.AddField('TotalPrice');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.TenantManagementPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
