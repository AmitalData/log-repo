import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {AdditionalCurrencyRatePM} from '../../EntityPMs/AdditionalCurrencyRatePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AdditionalCurrencyRatePMService } from '../../Services/StandardPMs/AdditionalCurrencyRatePMService';
import { AdditionalCurrencyRateValidator } from 'Accounting/Validators/AdditionalCurrencyRateValidator';

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
    myservice: AdditionalCurrencyRatePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new AdditionalCurrencyRatePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myservice = new AdditionalCurrencyRatePMService();
    }

    // Properties
    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get Rate() { return this.EntityPM.Rate; }
    set Rate(value: number) {
        if (this.EntityPM.Rate != value) {
            this.EntityPM.Rate = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    async SubmitChanges() {

        const canContinue = await AdditionalCurrencyRateValidator.CheckIdenticalRateValue(this.EntityPM);
        if (canContinue) {
            this.myservice.insert(this.EntityPM).subscribe((myResult:any) => {

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
}
