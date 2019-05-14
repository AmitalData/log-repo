import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {BankAccountLitePM} from '../../EntityPMs/BankAccountLitePM';
import {BankAccountLitePMService} from '../../Services/StandardPMs/BankAccountLitePMService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Component({
    moduleId: module.id,
    templateUrl: './NewBankAccountLiteComponent.html',
})

export class NewBankAccountLiteComponent extends BaseComponent implements OnInit {

    public DataContext: NewBankAccountLiteComponent = this;
    public ObjectTableName: string = "BankAccountLite";
    public EntityPM: BankAccountLitePM = new BankAccountLitePM();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new BankAccountLitePM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
    }

    ngOnInit() {

    }

    get AccountNumber() { return this.EntityPM.AccountNumber; }
    set AccountNumber(newValue: string) {
        if (this.EntityPM.AccountNumber != newValue) {
            this.EntityPM.AccountNumber = newValue;
        }
    }

    get BankCode() { return this.EntityPM.BankCode; }
    set BankCode(newValue: string) {
        if (this.EntityPM.BankCode != newValue) {
            this.EntityPM.BankCode = newValue;
        }
    }

    get BranchNumber() { return this.EntityPM.BranchNumber; }
    set BranchNumber(newValue: string) {
        if (this.EntityPM.BranchNumber != newValue) {
            this.EntityPM.BranchNumber = newValue;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(newValue: string) {
        if (this.EntityPM.CurrencyId != newValue) {
            this.EntityPM.CurrencyId = newValue;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var myService: BankAccountLitePMService = new BankAccountLitePMService();
            myService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
                if (response != null) {
                    if (!response.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        this.ValidationErrorsList = response.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            });
        }
    }

}
