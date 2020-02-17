import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantAddOnPM} from '../../../../Infrastructure/EntityPMs/TenantAddOnPM';
import {AddOnItem} from './TenantManagementGeneralTabComponent';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAddOnComponent.html',
})

export class AddEditAddOnComponent {
    public EntityPM: TenantAddOnPM;
    public DataContext: AddOnItem;
    public ObjectTableName: string = "TenantAddOn";
    public ValidationErrorsList: string[] = [];
    public IsNewEntity: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }
    
    SetDataContext(dataContext: AddOnItem) {
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
            if (this.DataContext.fatherComponent.EntityPM.AddOns.filter(d => d.PackageCode == this.DataContext.PackageCode)[0]) {
                errors.push("Same package already exists");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            if(this.IsNewEntity) {
                this.IsNewEntity = false;

                this.DataContext.fatherComponent.EntityPM.AddTenantAddOnPM(this.DataContext.EntityPM);

                if (this.DataContext.fatherComponent.AddOnsList.indexOf(this.DataContext) == -1) {
                    this.DataContext.fatherComponent.AddOnsList.push(this.DataContext);
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
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.TenantManagementPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
