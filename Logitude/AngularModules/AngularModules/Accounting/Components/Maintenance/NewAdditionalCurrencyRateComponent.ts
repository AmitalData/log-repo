import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {AdditionalCurrencyRatePM} from '../../EntityPMs/AdditionalCurrencyRatePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AdditionalCurrencyRatePMService} from '../../Services/StandardPMs/AdditionalCurrencyRatePMService';

@Component({
    selector: 'NewAdditionalCurrencyRateComponent',
    
    templateUrl: './NewAdditionalCurrencyRateComponent.html',
})

export class NewAdditionalCurrencyRateComponent extends BaseComponent{
    public EntityPM: AdditionalCurrencyRatePM;
    public DataContext: NewAdditionalCurrencyRateComponent = this;
    public ObjectTableName: string = "AdditionalCurrencyRate";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: AdditionalCurrencyRatePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new AdditionalCurrencyRatePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new AdditionalCurrencyRatePMService();

    }

    // Properties
    get Value() { return this.EntityPM.Value; }
    set Value(value: number) {
        if (this.EntityPM.Value != value) {
            this.EntityPM.Value = value;
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
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
        
        this.myService.insert(this.EntityPM).subscribe((myResult:any) => {

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
