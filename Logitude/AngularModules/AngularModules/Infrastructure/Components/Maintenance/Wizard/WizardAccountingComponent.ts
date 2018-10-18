import {Component} from '@angular/core';
import {BaseComponent} from '../../LogitudeComponents/BaseComponent';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {AppTool} from '../../../Tools';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './WizardAccountingComponent.html',
})

export class WizardAccountingComponent extends BaseComponent {
    public EntityPM: TenantPM = null;
    public DataContext = this;
    public ObjectTableName: string = "Tenant";
    constructor() {
        super();
    }

    InitializeComponent(tenantPM: TenantPM) {
        this.EntityPM = tenantPM;
        
        if (AppTool.IsNullOrEmpty(this.PaymentTermId)) {
            var myService = new PaymentTermListService();
            myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var data: PaymentTermList[] = myResponse.Result;
                    var list: PaymentTermList = data.filter(f => f.EnglishName == "Net 30")[0];
                    if (list) {
                        this.PaymentTermId = list.Id;
                    }

                    else {
                        myService.getAll().subscribe((myResponse2: ServiceResponse) => {
                            if (!myResponse2.HasError) {
                                data = myResponse.Result;
                                list = data.filter(f => f.EnglishName == "Net 30")[0];

                                if (list) {
                                    this.PaymentTermId = list.Id;
                                }
                            }
                        });
                    }
                }
            });
        }
    }
    Validate(errors: string[]) {
        
        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.CurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Tenant.F.CurrencyId")));
        }

        if (AppTool.IsNullOrEmpty(this.ProfitCurrencyId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Tenant.F.ProfitCurrencyId")));
        }

        if (AppTool.IsNullOrEmpty(this.ProfitCurrencyRate) || this.ProfitCurrencyRate <= 0) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Tenant.F.ProfitCurrencyRate")));
        }

        return errors;
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;

            var myRate: number = null;
            var isRateEnabled: boolean = true;

            if (!AppTool.IsNullOrEmpty(value)) {
                if (value == this.ProfitCurrencyId) {
                    myRate = 1;
                    isRateEnabled = false;
                }
            }

            this.ProfitCurrencyRate = myRate;
            this.UIProperties.SetEnabled("ProfitCurrencyRate", this.ObjectTableName, isRateEnabled);
        }
    }

    get ProfitCurrencyId() { return this.EntityPM.ProfitCurrencyId; }
    set ProfitCurrencyId(value: string) {
        if (this.EntityPM.ProfitCurrencyId != value) {
            this.EntityPM.ProfitCurrencyId = value;

            var myRate: number = null;
            var isRateEnabled: boolean = true;

            if (!AppTool.IsNullOrEmpty(value)) {
                if (value == this.CurrencyId) {
                    myRate = 1;
                    isRateEnabled = false;
                }
            }

            this.ProfitCurrencyRate = myRate;
            this.UIProperties.SetEnabled("ProfitCurrencyRate", this.ObjectTableName, isRateEnabled);
        }
    }

    get ProfitCurrencyRate() { return this.EntityPM.ProfitCurrencyRate; }
    set ProfitCurrencyRate(value: number) {
        if (this.EntityPM.ProfitCurrencyRate != value) {
            this.EntityPM.ProfitCurrencyRate = value;
        }
    }

    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(value: string) {
        if (this.EntityPM.VatNumber != value) {
            this.EntityPM.VatNumber = value;
        }
    }

    get STDVatPercentage() { return this.EntityPM.STDVatPercentage; }
    set STDVatPercentage(value: number) {
        if (this.EntityPM.STDVatPercentage != value) {
            this.EntityPM.STDVatPercentage = value;
        }
    }

    get PaymentTermId() { return this.EntityPM.PaymentTermId; }
    set PaymentTermId(value: string) {
        if (this.EntityPM.PaymentTermId != value) {
            this.EntityPM.PaymentTermId = value;
        }
    }
}