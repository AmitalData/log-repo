import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {BankCodePM} from '../../EntityPMs/BankCodePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {BankCodePMService} from '../../Services/StandardPMs/BankCodePMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';

@Component({
    selector: 'NewBankCodeComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './NewBankCodeComponent.html',
})

export class NewBankCodeComponent extends BaseComponent{
    public EntityPM: BankCodePM;
    public DataContext: NewBankCodeComponent = this;
    public ObjectTableName: string = "BankCode";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: BankCodePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new BankCodePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new BankCodePMService();
        this.SetUIProperties();
        this.SelectDefaultValues();
    }

    // Properties
    get Inactive() { return this.EntityPM.Inactive == null ? false : this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

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

    SetUIProperties() {
    }
    
    SelectDefaultValues() {
        this.EntityPM.Inactive = false;
        
    }
     
}
