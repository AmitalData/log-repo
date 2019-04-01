import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {AutomaticReconcileMethodPM} from '../../EntityPMs/AutomaticReconcileMethodPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AutomaticReconcileMethodPMService} from '../../Services/StandardPMs/AutomaticReconcileMethodPMService';

@Component({
    selector: 'AutoRecoMethodComponent',
    moduleId: module.id,
    templateUrl: './AutoRecoMethodComponent.html',
})

export class AutoRecoMethodComponent extends BaseComponent{
    public EntityPM: AutomaticReconcileMethodPM;
    public DataContext: AutoRecoMethodComponent = this;
    public ObjectTableName: string = "AutomaticReconcileMethod";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: AutomaticReconcileMethodPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new AutomaticReconcileMethodPM();
        this.EntityPM.Inactive = false;
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new AutomaticReconcileMethodPMService();

    }

    //#region Properties
    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get AutomaticReconcile1() { return this.EntityPM.AutomaticReconcile1; }
    set AutomaticReconcile1(value: string) {
        if (this.EntityPM.AutomaticReconcile1 != value) {
            this.EntityPM.AutomaticReconcile1 = value;
        }
    }

    get AutomaticReconcile2() { return this.EntityPM.AutomaticReconcile2; }
    set AutomaticReconcile2(value: string) {
        if (this.EntityPM.AutomaticReconcile2 != value) {
            this.EntityPM.AutomaticReconcile2 = value;
        }
    }

    get AutomaticReconcile3() { return this.EntityPM.AutomaticReconcile3; }
    set AutomaticReconcile3(value: string) {
        if (this.EntityPM.AutomaticReconcile3 != value) {
            this.EntityPM.AutomaticReconcile3 = value;
        }
    }
    //#endregion
   
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
