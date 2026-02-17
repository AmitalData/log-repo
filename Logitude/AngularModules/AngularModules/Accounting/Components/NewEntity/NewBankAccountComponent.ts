import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {BankAccountPM} from '../../EntityPMs/BankAccountPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {BankAccountPMService} from '../../Services/StandardPMs/BankAccountPMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'NewBankAccountComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './NewBankAccountComponent.html',
})

export class NewBankAccountComponent extends BaseComponent{
    public EntityPM: BankAccountPM;
    public DataContext: NewBankAccountComponent = this;
    public ObjectTableName: string = "BankAccount";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: BankAccountPMService;
    public GLAccountsFilterItems: ApiQueryFilters;

    

public isRTL: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new BankAccountPM();
        this.EntityPM.Inactive = false;
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new BankAccountPMService();
        this.SetUIProperties();
        this.SelectDefaultValues();
        this.InitLOVFilters();
    }

    InitLOVFilters() {
        // initialize query filters for Accounts
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("ChartOfAccountsTypeCode", "5", null, null, "Equals", false, false, false, "string");
        this.GLAccountsFilterItems.addAdditionalFilter("IsMultiCurrency", false, null, null, "Equals", false, false, false, "string");
    }

    //#region Properties
    get AccountNumber() { return this.EntityPM.AccountNumber; }
    set AccountNumber(value: string) {
        if (this.EntityPM.AccountNumber != value) {
            this.EntityPM.AccountNumber = value;
        }
    }

    get GLAccountId() { return this.EntityPM.GLAccountId; }
    set GLAccountId(value: string) {
        if (this.EntityPM.GLAccountId != value) {
            this.EntityPM.GLAccountId = value;

            //if (!this.DeferredGLAccountId)
                //this.DeferredGLAccountId = value;
        }
    }

    get DeferredGLAccountId() { return this.EntityPM.DeferredGLAccountId; }
    set DeferredGLAccountId(value: string) {
        if (this.EntityPM.DeferredGLAccountId != value) {
            this.EntityPM.DeferredGLAccountId = value;
        }
    }

    get TransferGLAcccountId() { return this.EntityPM.TransferGLAcccountId; }
    set TransferGLAcccountId(value: string) {
        if (this.EntityPM.TransferGLAcccountId != value) {
            this.EntityPM.TransferGLAcccountId = value;
        }
    }

    get BankId() { return this.EntityPM.BankId; }
    set BankId(value: string) {
        if (this.EntityPM.BankId != value) {
            this.EntityPM.BankId = value;
        }
    }

    get BranchNumber() { return this.EntityPM.BranchNumber; }
    set BranchNumber(value: string) {
        if (this.EntityPM.BranchNumber != value) {
            this.EntityPM.BranchNumber = value;
        }
    }

    glAccount: GLAccountPM;
    get GLAccount() { return this.glAccount; }
    set GLAccount(value: GLAccountPM) {
        if (this.glAccount != value) {
            this.glAccount = value;
        }
    }

    deferredGLAccount: GLAccountPM;
    get DeferredGLAccount() { return this.deferredGLAccount; }
    set DeferredGLAccount(value: GLAccountPM) {
        if (this.deferredGLAccount != value) {
            this.deferredGLAccount = value;
        }
    }

    transferGLAcccount: GLAccountPM;
    get TransferGLAcccount() { return this.transferGLAcccount; }
    set TransferGLAcccount(value: GLAccountPM) {
        if (this.transferGLAcccount != value) {
            this.transferGLAcccount = value;
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
    //#endregion

    OkButtonClicked() {
        var errors: string[] = [];
        //this.EntityPM.EnglishName = "­­ ";
        //this.EntityPM.BranchAddress = "­­ ";
        //}

        // Class Validator
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        // Custom Validation
        if (!AppTool.IsNullOrEmpty(this.GLAccount) && !AppTool.IsNullOrEmpty(this.DeferredGLAccount)) {
            if (this.GLAccount.IsMultiCurrency || this.DeferredGLAccount.IsMultiCurrency) {
                errors.push("The GLAccount and Difffered GLAccount must be single currency");
            }
            else if (this.GLAccount.CurrencyId != this.DeferredGLAccount.CurrencyId) {
                errors.push(TextCodeTranslator.Translate("BankAccounts.O.CurrencyGLAccountAndDefferredMustSame")); // The currencies of the GLAccount must be the same
            }
        }

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
        //this.UIProperties.SetEnabled("BankAccountsId", this.ObjectTableName, false);
        //this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
    }
    
    SelectDefaultValues() {
        this.EntityPM.Inactive = false;
        
    }
     
}
