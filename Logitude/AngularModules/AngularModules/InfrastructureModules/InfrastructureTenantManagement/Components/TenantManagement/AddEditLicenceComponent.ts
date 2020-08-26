import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagementLicensePM} from '../../../../Infrastructure/EntityPMs/TenantManagementLicensePM';
import {PackageItem} from './TenantManagementGeneralTabComponent';
import { TenantManagementPM } from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    
    templateUrl: './AddEditLicenceComponent.html',
})

export class AddEditLicenceComponent extends BaseComponent {
    public EntityPM: TenantManagementLicensePM;
    public TenantManagementPM: TenantManagementPM;
    public DataContext: PackageItem;
    public MainDataContext: AddEditLicenceComponent = this;
    public ObjectTableName: string = "TenantManagementLicense";
    public ValidationErrorsList: string[] = [];
    public IsNewEntity: boolean;
    public IsMainPackage: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetDataContext(dataContext: PackageItem) {
        this.DataContext = dataContext;
        this.TenantManagementPM = dataContext.TenantManagementPM;
        this.EntityPM = dataContext.EntityPM;
        this.IsNewEntity = dataContext.IsNew;
        this.IsMainPackage = false;

        this.Clone();
    }

    SetWindowArgs(entityPM: TenantManagementPM) {
        this.TenantManagementPM = entityPM;
        this.IsNewEntity = false;
        this.IsMainPackage = true;
        this.ObjectTableName = "TenantManagement";

        this.SetUIProperties();
        this.Clone();
    }

    SetUIProperties() {
        this.SetUIProperties_NumberOfUsers();
        this.SetUIProperties_TotalPrice();
    }

    private SetUIProperties_NumberOfUsers() {        
        var isEditable = false;

        if (FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            if (this.TenantManagementPM.MainAdditionalPackageApplied || !this.TenantManagementPM.IsMultiPackage) {
                isEditable = true;
            }
        }

        this.UIProperties.SetEnabled("PackageCode", this.ObjectTableName, isEditable);
        this.UIProperties.SetEnabled("NumberOfUsers", this.ObjectTableName, isEditable);
        this.UIProperties.SetEnabled("FreeUsers", this.ObjectTableName, isEditable);
        this.UIProperties.SetEnabled("LicensePrice", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("NumberOfUsers", this.ObjectTableName, AppTool.IsNullOrZero(this.NumberOfUsers));
    }
    SetUIProperties_TotalPrice() {
        this.UIProperties.SetEnabled("TotalPrice", this.ObjectTableName, false);
    }

    get PackageCode() { return this.TenantManagementPM.PackageCode; }
    set PackageCode(newValue: string) {
        if (this.TenantManagementPM.PackageCode != newValue) {
            this.TenantManagementPM.PackageCode = newValue;

            if (newValue == "EAWB") {
                this.TenantManagementPM.IsAWBStockPrepaid = true;
            }
        }
    }

    get NumberOfUsers() { return this.TenantManagementPM.NumberOfUsers; }
    set NumberOfUsers(newValue: number) {
        if (this.TenantManagementPM.NumberOfUsers != newValue) {
            this.TenantManagementPM.NumberOfUsers = newValue;
            this.ComputeTotalPrice();
        }
    }

    get FreeUsers() { return this.TenantManagementPM.FreeUsers; }
    set FreeUsers(newValue: number) {
        if (this.TenantManagementPM.FreeUsers != newValue) {
            this.TenantManagementPM.FreeUsers = newValue;
        }
    }

    get LicensePrice() { return this.TenantManagementPM.LicensePrice; }
    set LicensePrice(newValue: number) {
        if (this.TenantManagementPM.LicensePrice != newValue) {
            this.TenantManagementPM.LicensePrice = newValue;
            this.ComputeTotalPrice();
        }
    }

    get TotalPrice() { return this.TenantManagementPM.TotalPrice; }
    set TotalPrice(newValue: number) {
        if (this.TenantManagementPM.TotalPrice != newValue) {
            this.TenantManagementPM.TotalPrice = newValue;
        }
    }

    ComputeTotalPrice() {
        if (this.NumberOfUsers && this.LicensePrice) {
            this.TotalPrice = this.NumberOfUsers * this.LicensePrice;
        }

        else {
            this.TotalPrice = null;
        }
    }

    CancelButtonClicked() {
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

        var numberOfUsers: number = 0;
        var freeUser: number = 0; 

        if (this.IsMainPackage) {
            numberOfUsers = this.NumberOfUsers;
            freeUser = this.FreeUsers;
        }

        else {
            numberOfUsers = this.DataContext.NumberOfUsers;
            freeUser = this.DataContext.FreeUsers;
        }

        if (AppTool.IsNullOrZero(numberOfUsers) && AppTool.IsNullOrZero(freeUser)) {
            errors.push("You should enter Number of Users or Free Users");

            //if (numberOfUser == 0) {
            //    errors.push("Number Of Users should not be zero");
            //}
            //else
            //    errors.push("Number Of Users is Required");
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
        if (this.IsMainPackage) {
            this.myCloner = new Cloner(this.MainDataContext);
            this.myCloner.AddField('PackageCode');
            this.myCloner.AddField('NumberOfUsers');
            this.myCloner.AddField('FreeUsers');
            this.myCloner.AddField('TotalPrice');
            this.myCloner.AddField('LicensePrice');            
            this.myCloner.AddEntity(this.TenantManagementPM);
        }

        else {
            this.myCloner = new Cloner(this.DataContext);
            this.myCloner.AddField('PackageCode');
            this.myCloner.AddField('NumberOfUsers');
            this.myCloner.AddField('FreeUsers');
            this.myCloner.AddField('TotalPrice');
            this.myCloner.AddField('Price');
            this.myCloner.AddEntity(this.EntityPM);
            this.myCloner.AddEntity(this.DataContext.TenantManagementPM);
        }
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
