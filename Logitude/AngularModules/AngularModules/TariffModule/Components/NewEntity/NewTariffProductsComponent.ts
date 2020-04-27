import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TariffProductPMService } from '../../Services/StandardPMs/TariffProductPMService';
import { TariffProductPM } from '../../EntityPMs/TariffProductPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';

@Component({
    selector: 'NewTariffProductsComponent',
    templateUrl: './NewTariffProductsComponent.html',
})

export class NewTariffProductsComponent extends BaseComponent implements OnInit {
    public EntityPM: TariffProductPM;
    public ObjectTableName: string = "TariffProduct";
    public TenantPM: TenantPM;
    public DataContext: NewTariffProductsComponent = this;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    ngOnInit() {
        this.CreateTariffProduct();
    }

    CreateTariffProduct() {
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new TariffProductPM();
        this.EntityPM.Tenant = this.TenantPM.Id;
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }


    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

  
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.Submit();
        }
    }

    Submit() {
        var service: TariffProductPMService = new TariffProductPMService();
        service.insert(this.EntityPM).subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                if (!myResult.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}
