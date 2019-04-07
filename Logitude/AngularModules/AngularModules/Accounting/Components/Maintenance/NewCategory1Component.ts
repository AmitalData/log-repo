import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {Category1PM} from '../../EntityPMs/Category1PM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {Category1PMService} from '../../Services/StandardPMs/Category1PMService';

@Component({
    selector: 'NewCategory1Component',
    moduleId: module.id,
    templateUrl: './NewCategory1Component.html',
})

export class NewCategory1Component extends BaseComponent{
    public EntityPM: Category1PM;
    public DataContext: NewCategory1Component = this;
    public ObjectTableName: string = "Category1";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: Category1PMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new Category1PM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new Category1PMService();

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
        this.CurrentSession.CloseCurrentWindow();
    }
    SubmitChanges() {
        
        this.myService.insert(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit("ok");
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
