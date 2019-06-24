import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {PaymentChequePM} from '../../EntityPMs/PaymentChequePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {PaymentChequePMService} from '../../Services/StandardPMs/PaymentChequePMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../Infrastructure/Tools';
import {BankAccountPM} from '../../EntityPMs/BankAccountPM';
import {PaymentChequeValidator} from '../../Validators/PaymentChequeValidator';
import {CurrencyRatesService, LastRate} from '../../../Common/Services/CurrencyRatesService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';



@Component({
    selector: 'NewPaymentChequeComponent',
    moduleId: module.id,
 
    templateUrl: './NewPaymentChequeComponent.html',
})



export class NewPaymentChequeComponent extends BaseComponent
{
    public DataContext = this;
    public ObjectTableName: string = "PaymentCheque";
    entityPM: PaymentChequePM = new PaymentChequePM();
    public ValidationErrorsList: string[] = [];
    public TenantPM: TenantPM;
    PaymentChequePMService: PaymentChequePMService = new PaymentChequePMService();
    EntityResourceService: EntityResourceService = new EntityResourceService();
    visible: boolean = false;
    public filterAgrs: ApiQueryFilters;
    Height: number = 50;
    IsForignAmountVisibile = true;
    paymentChequeValidator: PaymentChequeValidator = new PaymentChequeValidator();
    CurrencyRatesService: CurrencyRatesService = new CurrencyRatesService();
    LastRatesList: LastRate[] = [];
    LocalAmountFieldLabel: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService)
    {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityResourceService.getEntityResourceByTableName("PaymentCheque").subscribe((response: any) => {
        this.visible = true; this.LocalAmountFieldLabel = TextCodeTranslator.Translate("PaymentCheque.F.LocalAmount") + " " + "(" + SessionLocator.LocalCurrencyCode + ")";
});
        this.SetFilters();
        this.entityPM.Tenant = this.TenantPM.Id;
        this.CurrencyId =  SessionLocator.LocalCurrencyId;
        this.ValueDate = new Date();
        this.CurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.LocalCurrencyId, this.ValueDate).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;

              
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        });


    }

    SetFilters()
    {
        this.filterAgrs = new ApiQueryFilters();
        this.filterAgrs.addAdditionalFilter("AccountTypeCode", "4,5", null, null, "Exclude", false, false, false, "string");

    }

    get PayToGLAccountId() { return this.entityPM.PayToGLAccountId; }
    set PayToGLAccountId(value: string) {
        if (this.entityPM.PayToGLAccountId != value) {
            this.entityPM.PayToGLAccountId = value;
        }
    }

    get PayToName() { return this.entityPM.PayToName; }
    set PayToName(value: string) {
        if (this.entityPM.PayToName != value) {
            this.entityPM.PayToName = value;
        }
    }

    get CurrencyId() { return this.entityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.entityPM.CurrencyId != value) {
            this.entityPM.CurrencyId = value;
            //if (!AppTool.IsNullOrEmpty(value)) {
            //    this.CurrencyRatesService.GetCurrenciesExchangeRateByValueDate(this.CurrencyId, new Date()).subscribe((myResponse: ServiceResponse) => {

            //        if (!myResponse.HasError) {
            //            this.LastRatesList = myResponse.Result;
            //            var rate: number = null;
            //            var rateDate: Date = null;
            //            var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.bankAccount.GLAccountCurrencyId)[0];
            //            if (lastRate != null) {
            //                rate = lastRate.Rate;
            //                this.entityPM.ExchangeRate = rate;
            //                this.ForeignAmount = this.LocalAmount / rate;
            //            }

            //        }

            //        //else {
            //        //    this.CurrentSession.StopBusyIndicator();
            //        //}
            //    });

           // }
        }
    }

    get LocalAmount() { return this.entityPM.LocalAmount; }
    set LocalAmount(value: number) {
        if (this.entityPM.LocalAmount != value) {
            this.entityPM.LocalAmount = value;

            if (value && this.BankAccount) {
                var rate: number = null;
                var rateDate: Date = null;
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.bankAccount.GLAccountCurrencyId)[0];
                if (lastRate != null) {
                   
                    rate = lastRate.Rate;
                    this.entityPM.ExchangeRate = rate;
                    this.ForeignAmount = this.LocalAmount / rate;
                }
            }
            else {
                this.ForeignAmount = null;
            }
        }
    }
    get ForeignAmount() { return this.entityPM.ForeignAmount; }
    set ForeignAmount(value: number) {
        if (this.entityPM.ForeignAmount != value) {
            this.entityPM.ForeignAmount = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, true);

            }
        }
    }
    get Notes() { return this.entityPM.Notes; }
    set Notes(value: string) {
        if (this.entityPM.Notes != value) {
            this.entityPM.Notes = value;
        }
    }
  public bankName: string = "LocalName";
  get BankName() { return this.bankName; }
  set BankName(value: string) {
    if (this.bankName != value) {
      this.bankName = value;
    }
  }


 
    get ValueDate() { return this.entityPM.ValueDate; }
    set ValueDate(value: Date) {
        if (this.entityPM.ValueDate != value) {
            this.entityPM.ValueDate = value;
        }
  }
  
  get BankAccountId() { return this.entityPM.BankAccountId; }
  set BankAccountId(value: string) {
    if (this.entityPM.BankAccountId != value) {
      this.entityPM.BankAccountId = value;
    }
  }

    isAccountValid: boolean = true;
    private account: GLAccountPM;
    get Account() { return this.account; }
    set Account(value: GLAccountPM) {
        if (this.account != value) {
            this.account = value;
            if (value != null) {
                if (value.AccountTypeCode == "3") {
                    this.isAccountValid = false;
                    this.UIProperties.SetValidity("PayToGLAccountId", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.VendorsGLAccount")); 
                }
                else {
                    this.UIProperties.SetValidity("PayToGLAccountId", this.ObjectTableName, true,null); 

                    this.isAccountValid = true;
                    this.PayToName = value.LocalName;
                }
            }
        }
    }
    private bankAccount: BankAccountPM;
    get BankAccount() { return this.bankAccount; }
    set BankAccount(value: BankAccountPM) {
        if (this.bankAccount != value) {
            this.bankAccount = value;
           
      
      
            if (this.bankAccount) {

          if (this.bankAccount.LocalName) {
          this.BankName = "LocalName";
               }
            else {
          this.BankName = "EnglishName";
                }
                this.CurrencyId = this.bankAccount.GLAccountCurrencyId;
                if (this.bankAccount.GLAccountCurrencyId == SessionLocator.LocalCurrencyId) {
                    this.UIProperties.SetVisibility("ForeignAmount", this.ObjectTableName, false);
                    this.UIProperties.SetVisibility("CurrencyId", this.ObjectTableName, false);
                    // this.Height = 0;
                    this.IsForignAmountVisibile = false;

                }
                else {
                    this.UIProperties.SetVisibility("ForeignAmount", this.ObjectTableName, true);
                    this.UIProperties.SetVisibility("CurrencyId", this.ObjectTableName, true);
                    //this.Height = 0;
                    this.IsForignAmountVisibile = true;
                    this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, true);
                    if (this.LocalAmount) {
                        var rate: number = null;
                        var rateDate: Date = null;
                        var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.bankAccount.GLAccountCurrencyId)[0];
                        if (lastRate != null) {
                            rate = lastRate.Rate;
                            this.entityPM.ExchangeRate = rate;
                            this.ForeignAmount = this.LocalAmount / rate;
                        }
                    }
                }
            }
            else {
                    this.UIProperties.SetVisibility("ForeignAmount", this.ObjectTableName, true);
                    this.UIProperties.SetVisibility("CurrencyId", this.ObjectTableName, true);
                    //this.Height = 0;
                    this.IsForignAmountVisibile = true;
                    this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, true);
                    this.ForeignAmount = null;
 }
        }
    }
    FIELD_IS_REQUIERD: string = null;
    OkButtonClicked() {
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
        this.entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.ValidationErrorsList = [];
        this.CheckCurrency();
     
        if (!this.isAccountValid) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.VendorsGLAccount"));

        }


        if (this.ValidationErrorsList.length == 0) {
            
            var errors: string[] = [];

            Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
            
           if (errors.length == 0) {



                this.SubmitChanges();
            } else {
                this.ValidationErrorsList = errors;
            }
        }
    }
    SubmitChanges() {

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.Saving"));

        this.entityPM.BankAccountGLAccountId = this.BankAccount.DeferredGLAccountId;
        this.entityPM.PaymentChequeStatusCode = "1";
        this.PaymentChequePMService.insert(this.entityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                var entity = mm.Result;
                this.CurrentSession.CloseCurrentWindowEmit("ok");// this.CurrentSession.CloseCurrentWindowEmit("ok");

                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.CancelButtonClicked();
                        });
                    });
                this.CurrentSession.StopBusyIndicator();

               
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    CheckCurrency() {
        if (this.BankAccount) {
            if (SessionLocator.LocalCurrencyId != this.BankAccount.GLAccountCurrencyId) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.DifferentBankCurrency"));
            }

        }

        if(this.Account){
            if (!this.Account.IsMultiCurrency) {
             
                if (this.Account.CurrencyId != SessionLocator.LocalCurrencyId) {
                      //  this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.DifferentCurrencies"));
                    } 
                
            }
        }


    }




    CancelButtonClicked()
    {
        this.CurrentSession.CloseCurrentWindow();

    }
}
