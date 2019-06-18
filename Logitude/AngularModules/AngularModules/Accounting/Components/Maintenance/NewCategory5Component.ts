import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {Category5PM} from '../../EntityPMs/Category5PM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Category5PMService} from '../../Services/StandardPMs/Category5PMService';

@Component({
    selector: 'NewCategory5Component',
    moduleId: module.id,
    templateUrl: './NewCategory5Component.html',
})

export class NewCategory5Component extends BaseComponent{
    public EntityPM: Category5PM;
    public DataContext: NewCategory5Component = this;
    public ObjectTableName: string = "Category5";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: Category5PMService;

    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new Category5PM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new Category5PMService();

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
