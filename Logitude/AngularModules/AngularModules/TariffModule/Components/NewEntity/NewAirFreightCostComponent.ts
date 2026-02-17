import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { TariffPM } from '../../EntityPMs/TariffPM';
import { TariffPMService } from '../../Services/StandardPMs/TariffPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'NewAirFreightCostComponent',
    moduleId: module.id,
    templateUrl: './NewAirFreightCostComponent.html',
})

export class NewAirFreightCostComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "Tariff";
    public EntityPM: TariffPM;
    public SelectedLocationFilter: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new TariffPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.TypeCode = "AFC";
    }

    get Name() {
        return this.EntityPM.Name;
    }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get StartDate() {
        return this.EntityPM.StartDate;
    }
    set StartDate(value: Date) {
        if (this.EntityPM.StartDate != value) {
            this.EntityPM.StartDate = value;
        }
    }

    get ExpirationDate() {
        return this.EntityPM.ExpirationDate;
    }
    set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;
        }
    }


    get Description() {
        return this.EntityPM.Description;
    }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }



    get CurrencyId() {
        return this.EntityPM.CurrencyId;
    }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
        }
    }


    get SellerId() {
        return this.EntityPM.SellerId;
    }
    set SellerId(value: string) {
        if (this.EntityPM.SellerId != value) {
            this.EntityPM.SellerId = value;
        }
    }


    get ContractNumber() {
        return this.EntityPM.ContractNumber;
    }
    set ContractNumber(value: number) {
        if (this.EntityPM.ContractNumber != value) {
            this.EntityPM.ContractNumber = value;
        }
    }



    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[]=[];
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.StartDate != null && this.ExpirationDate != null) {
            if (this.ExpirationDate < this.StartDate) {
                this.ValidationErrorsList.push("Expiration date must be less than start date");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Creating...");
            var myService: TariffPMService = new TariffPMService();
            myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit('OK');
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}
