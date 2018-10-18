import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {Category2PM} from '../../EntityPMs/Category2PM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Category2PMService} from '../../Services/StandardPMs/Category2PMService';

@Component({
    selector: 'NewCategory2Component',
    moduleId: module.id,
    templateUrl: './NewCategory2Component.html',
})

export class NewCategory2Component extends BaseComponent{
    public EntityPM: Category2PM;
    public DataContext: NewCategory2Component = this;
    public ObjectTableName: string = "Category2";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: Category2PMService;

    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new Category2PM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new Category2PMService();

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
