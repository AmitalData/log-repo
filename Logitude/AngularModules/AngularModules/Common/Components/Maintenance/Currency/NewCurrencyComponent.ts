import {Component, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CurrencyList} from '../../../EntityLists/CurrencyList';
import {CommonDomainService} from '../../../Services/CommonDomainService';
import {CurrencyListService} from '../../../Services/StandardLists/CurrencyListService';
import {TenantPM} from '../../../EntityPMs/TenantPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';

@Component({
    selector: 'NewCurrencyComponent',
    
    templateUrl: './NewCurrencyComponent.html',
})

export class NewCurrencyComponent extends BaseComponent implements AfterViewInit {

    public DataContext: NewCurrencyComponent = this;
    public LabelColumnWidth: number = 135;
    public ControlColumnWidth: number = 230;
    public TenantPM: TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    IsAccountingActivated: boolean = false;

    constructor() {
        super();
        this.TenantPM = InfraSettings.TenantPM;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.LoadCurrencyListMethod();
        this.SetUIProperties();
    }

    ngAfterViewInit() {
        //this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("CurrencyId", null, AppTool.IsNullOrEmpty(this.CurrencyId));
        this.UIProperties.SetRequired("RateDate", null, AppTool.IsNullOrEmpty(this.RateDate));
        this.UIProperties.SetRequired("CurrencyRate", null, AppTool.IsNullOrEmpty(this.CurrencyRate));

        if(this.IsAccountingActivated) {
            this.UIProperties.SetRequired("Unit", null, AppTool.IsNullOrEmpty(this.Unit));
        }
       
    }

    public CurrencyList: CurrencyList[] = [];
    private LoadCurrencyListMethod() {
        var myService: CurrencyListService = new CurrencyListService();
        myService.getAll().subscribe((myResult: ServiceResponse) => {
            if (myResult) {
                this.CurrencyList = myResult.Result;
            }
        });
    }

    // Properties 
    private currencyId: string;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(newValue: string) {
        if (this.currencyId != newValue) {
            this.currencyId = newValue;
            this.SetUIProperties();
        }
    }

    private currencyRate: number;
    get CurrencyRate() { return this.currencyRate; }
    set CurrencyRate(newValue: number) {
        if (this.currencyRate != newValue) {
            this.currencyRate = newValue;
            this.SetUIProperties();
        }
    }

    private unit: number = 1;
    get Unit() { return this.unit; }
    set Unit(newValue: number) {
        if (this.unit != newValue) {
            this.unit = newValue;
            this.SetUIProperties();
        }
    }

    private rateDate: Date = DateTool.GetCurrentDateTimeAsUtc();
    get RateDate() { return this.rateDate; }
    set RateDate(newValue: Date) {
        if (this.rateDate != newValue) {
            this.rateDate = newValue;
            this.SetUIProperties();
        }
    }

    private seletedCurrency: CurrencyList;
    get SeletedCurrency() { return this.seletedCurrency; }
    set SeletedCurrency(newValue: CurrencyList) {
        if (this.seletedCurrency != newValue) {
            this.seletedCurrency = newValue;
        }
    }

    //Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.CurrencyId)) {
            errors.push("Currency Field is Required");
        }

        if (this.RateDate == null) {
            errors.push("Exchage Rate Date Field is Required");
        }

        if (this.CurrencyRate == null || this.CurrencyRate <= 0) {
            errors.push("Exchange Rate Field is Required");
        }

        if(this.IsAccountingActivated) {

            if (this.Unit == null || this.Unit <= 0) {
                errors.push("Unit Field is Required");
            }
        }
       

        var ratedate = DateTool.GetDateParts(this.RateDate).DateObject;
        ratedate = DateTool.TruncateTime(ratedate);
        var today = DateTool.GetCurrentDateAsUtc();
        if (ratedate.valueOf() > today.valueOf()) {
            errors.push("Can't create currency with future date");
        }

        if (this.SeletedCurrency != null) {
            var list = this.CurrencyList.filter(c => c.Code == this.SeletedCurrency.Code && c.Tenant == this.TenantPM.Id)[0];
            if (list != null) {
                errors.push("This currency already exists");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreatingCurrency();
        }
    }

    SubmitCreatingCurrency() {
        var myService: CommonDomainService = new CommonDomainService();
        var month = this.RateDate.getMonth() + 1;
        var year = this.RateDate.getFullYear();
        var day = this.RateDate.getDate();

        var hour = this.RateDate.getHours();
        var minute = this.RateDate.getMinutes();
        var sec = this.RateDate.getSeconds();
        var millsec = this.RateDate.getMilliseconds();

        var myDate = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec + "." + millsec;

        myService.CopyCurrencyToTenant(this.CurrencyId, this.CurrencyRate, myDate,this.Unit).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var newCreatedCurrency: CurrencyList = myResponse.Result;
                this.CurrentSession.CloseCurrentWindowEmit(newCreatedCurrency.Id);
            }

            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
