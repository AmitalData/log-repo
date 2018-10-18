import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {Category3PM} from '../../EntityPMs/Category3PM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Category3PMService} from '../../Services/StandardPMs/Category3PMService';

@Component({
    selector: 'NewCategory3Component',
    moduleId: module.id,
    templateUrl: './NewCategory3Component.html',
})

export class NewCategory3Component extends BaseComponent{
    public EntityPM: Category3PM;
    public DataContext: NewCategory3Component = this;
    public ObjectTableName: string = "Category3";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: Category3PMService;

    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new Category3PM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new Category3PMService();

    }

    // Properties
    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get InActive() { return this.EntityPM.Inactive; }
    set InActive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }

    // Commands
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            this.SubmitChanges();
        }
    }
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
    SubmitChanges() {
        
        this.myService.insert(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                SessionLocator.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
