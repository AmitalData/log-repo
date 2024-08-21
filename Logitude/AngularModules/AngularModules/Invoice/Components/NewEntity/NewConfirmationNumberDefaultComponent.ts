import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { BankAccountLitePM } from '../../EntityPMs/BankAccountLitePM';
import { BankAccountLitePMService } from '../../Services/StandardPMs/BankAccountLitePMService';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ConfirmationNumberStatusPM } from 'Invoice/EntityPMs/ConfirmationNumberStatusPM';
import { ConfirmationNumberDefaultPM } from 'Invoice/EntityPMs/ConfirmationNumberDefaultPM';
import { Data } from '@angular/router';
import { ConfirmationNumberDefaultPMService } from 'Invoice/Services/StandardPMs/ConfirmationNumberDefaultPMService';


@Component({

    templateUrl: './NewConfirmationNumberDefaultComponent.html',
})

export class NewConfirmationNumberDefaultComponent extends BaseComponent implements OnInit {

    public DataContext: NewConfirmationNumberDefaultComponent = this;
    public ObjectTableName: string = "ConfirmationNumberDefault";
    public EntityPM: ConfirmationNumberDefaultPM = new ConfirmationNumberDefaultPM();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new ConfirmationNumberDefaultPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
    }

    ngOnInit() {

    }

    get FromDate() { return this.EntityPM.FromDate; }
    set FromDate(newValue: Date) {
        if (this.EntityPM.FromDate != newValue) {
            this.EntityPM.FromDate = newValue;
        }
    }

    get AmountForConfirmationNumber() { return this.EntityPM.AmountForConfirmationNumber; }
    set AmountForConfirmationNumber(newValue: number) {
        if (this.EntityPM.AmountForConfirmationNumber != newValue) {
            this.EntityPM.AmountForConfirmationNumber = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive !== newValue) {
            this.EntityPM.InActive = newValue;
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
            var myService: ConfirmationNumberDefaultPMService = new ConfirmationNumberDefaultPMService();
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
