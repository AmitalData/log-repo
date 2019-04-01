import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {GLAccountPMService} from '../../Services/StandardPMs/GLAccountPMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../Infrastructure/Tools';
import {NewGLAccountArgs} from '../../../Common/Args';
import {FullAccountingSettingPM} from '../../EntityPMs/FullAccountingSettingPM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'NewGLAccountComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './NewGLAccountComponent.html',
})

export class NewGLAccountComponent extends BaseComponent {
    public EntityPM: GLAccountPM;
    public DataContext: NewGLAccountComponent = this;
    public ObjectTableName: string = "GLAccount";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    public ChartOfAccountTypeFilterItems: ApiQueryFilters;
    public ParentsFilterItems: ApiQueryFilters;
    myService: GLAccountPMService;
    IsFromArgs: boolean = false;

    public AccountTypeCode = "1";


    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new GLAccountPM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.IsMultiCurrency = false;

        this.myService = new GLAccountPMService();

        this.InitLOVFilters();

        this.SetUIProperties();
        this.SelectDefaultValues();
    }

    InitLOVFilters() {

        //#region initialize query filters for ChartOfAccountType
        this.ChartOfAccountTypeFilterItems = new ApiQueryFilters();
        if (!this.IsFromArgs) {
            this.ChartOfAccountTypeFilterItems.addAdditionalFilter("CodeFilter", "3,4", null, null, "Exclude", false, false, false, "string", false, true);
            //this.ChartOfAccountTypeFilterItems.addAdditionalFilter("Code", "3,4", null, null, "Exclude", false, false, false, "string", false, true);
        } else {
            this.UIProperties.SetEnabled("ChartOfAccountsTypeCode", this.ObjectTableName, false);
            // customer
            this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);
        }
        //#endregion

        //#region initialize query filters for Parent Account
        this.ParentsFilterItems = new ApiQueryFilters();
        this.ParentsFilterItems.addAdditionalFilter("ChartOfAccountsId", this.ChartOfAccountsId, null, null, "Equals", false, false, false, "string", false, true);
        this.ParentsFilterItems.addAdditionalFilter("ParentAccountId", "Please Don't Erase Me", null, null, "IsNull", false, false, false, "string", false, true);
        //this.ParentsFilterItems.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "Exclude", false, false, false, "string");
        //#endregion
    }

    SetWindowArgs(args: NewGLAccountArgs) {
        if (args != null) {
            this.IsFromArgs = true;
            if (args.AccountType == "2") { // customer
                this.ChartOfAccountsTypeCode = args.ChartOfAccountType;
                this.DisplayNumber = args.DisplayNo;
                this.LocalName = args.LocalName;
                this.EnglishName = args.EnglishName;
                this.AccountTypeCode = args.AccountType;
                this.EntityPM.NewGLAccountCardId = args.CardId;
                this.EntityPM.RevenueExpenseType = args.RevenueExpenseType;
                this.InitLOVFilters();
            }
            else if (args.AccountType == "3") { // vendor
                this.ChartOfAccountsTypeCode = args.ChartOfAccountType;
                this.DisplayNumber = args.DisplayNo;
                this.LocalName = args.LocalName;
                this.EnglishName = args.EnglishName;
                this.AccountTypeCode = args.AccountType;
                this.EntityPM.NewGLAccountCardId = args.CardId;
                this.EntityPM.RevenueExpenseType = args.RevenueExpenseType;
                this.InitLOVFilters();
            }
        }
        this.UIProperties.SetEnabled("EnglishName",this.ObjectTableName,false);
    }

    //#region Properties
    get IsMultiCurrency() { return this.EntityPM.IsMultiCurrency == null ? false : this.EntityPM.IsMultiCurrency; }
    set IsMultiCurrency(value: boolean) {
        if (value == true) {
            this.EntityPM.IsMultiCurrency = value;
            this.ReconcileMethodCode = '0';
            this.CurrencyId = null;

            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
            this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, true, "");
            this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, false);
            //this.UIProperties.SetEnabled("ReconcileMethodCode", this.ObjectTableName, false);

            this.CD.detectChanges();

        } else if (value == false) {
            this.EntityPM.IsMultiCurrency = value;
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
            this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
            this.ReconcileMethodCode = null;

            this.CD.detectChanges();
        }
    }

    
    get IsVATExempt() { return this.EntityPM.IsVATExempt }
    set IsVATExempt(value: boolean) {
        if (this.EntityPM.IsVATExempt != value) {
            this.EntityPM.IsVATExempt = value;

        }
    }



    get RevaluationEnabled() { return this.EntityPM.RevaluationEnabled }
    set RevaluationEnabled(value: boolean) {
        if (this.EntityPM.RevaluationEnabled != value) {
            this.EntityPM.RevaluationEnabled = value;

        }}


    get ChartOfAccountsTypeCode() { return this.EntityPM.ChartOfAccountsTypeCode; }
    set ChartOfAccountsTypeCode(value: string) {
        if (this.EntityPM.ChartOfAccountsTypeCode != value) {
            this.EntityPM.ChartOfAccountsTypeCode = value;
            this.ChartOfAccountsId = null;
        }
        this.OnLovItemChanged(value);

        if (!AppTool.IsNullOrEmpty(value)) {
            if (value == "1") { // 1-Revenues
                this.RevenueExpenseType = "1";
                this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);
            } else if (value == "2") { // 2-Expenses
                this.RevenueExpenseType = "2";
                this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);
            } else {
                this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, true);
            }
        } else {
            this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, true);
        }
    }

    get ChartOfAccountsId() { return this.EntityPM.ChartOfAccountsId; }
    set ChartOfAccountsId(value: string) {
        if (this.EntityPM.ChartOfAccountsId != value) {
            this.EntityPM.ChartOfAccountsId = value;
            if (value != null) {
                this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "");
            } else {
                this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, false, "");
            }
            var filter = this.ParentsFilterItems.AdditionalFilters.find(d => d.FieldName == "ChartOfAccountsId");
            filter.FieldValue = value;

        }
    }

    get DisplayNumber() { return this.EntityPM.DisplayNumber; }
    set DisplayNumber(value: string) {
        if (this.EntityPM.DisplayNumber != value) {
            this.EntityPM.DisplayNumber = value;
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

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
            if(value != null)
                this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, true, "");
            else
                this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, false, "");

            if (value == SessionLocator.TenantPM.CurrencyId)
                this.ReconcileMethodCode = "0";

        }
    }

    get ReconcileMethodCode() { return this.EntityPM.ReconcileMethodCode; }
    set ReconcileMethodCode(value: string) {
        if (this.EntityPM.ReconcileMethodCode != value) {
            this.EntityPM.ReconcileMethodCode = value;
        }
    }

    get AutomaticReconcileId() { return this.EntityPM.AutomaticReconcileId; }
    set AutomaticReconcileId(value: string) {
        if (this.EntityPM.AutomaticReconcileId != value) {
            this.EntityPM.AutomaticReconcileId = value;
        }
    }

    get ParentAccountId() { return this.EntityPM.ParentAccountId; }
    set ParentAccountId(value: string) {
        if (this.EntityPM.ParentAccountId != value) {
            this.EntityPM.ParentAccountId = value;
        }
    }

    get Category1Id() { return this.EntityPM.Category1Id; }
    set Category1Id(value: string) {
        if (this.EntityPM.Category1Id != value) {
            this.EntityPM.Category1Id = value;
        }
    }

    get Category2Id() { return this.EntityPM.Category2Id; }
    set Category2Id(value: string) {
        if (this.EntityPM.Category2Id != value) {
            this.EntityPM.Category2Id = value;
        }
    }

    get Category3Id() { return this.EntityPM.Category3Id; }
    set Category3Id(value: string) {
        if (this.EntityPM.Category3Id != value) {
            this.EntityPM.Category3Id = value;
        }
    }

    get Category4Id() { return this.EntityPM.Category4Id; }
    set Category4Id(value: string) {
        if (this.EntityPM.Category4Id != value) {
            this.EntityPM.Category4Id = value;
        }
    }

    get Category5Id() { return this.EntityPM.Category5Id; }
    set Category5Id(value: string) {
        if (this.EntityPM.Category5Id != value) {
            this.EntityPM.Category5Id = value;
        }
    }

    get RevenueExpenseType() { return this.EntityPM.RevenueExpenseType; }
    set RevenueExpenseType(value: string) {
        if (this.EntityPM.RevenueExpenseType != value) {
            this.EntityPM.RevenueExpenseType = value;
            if (value == "3") {

                this.UIProperties.SetEnabled("IsVATExempt", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetEnabled("IsVATExempt", this.ObjectTableName, true);
            }

        }
    }

    //#endregion

    OkButtonClicked() {
        var errors: string[] = [];

        if (this.IsMultiCurrency) {
            if (this.ReconcileMethodCode != '0') {
                errors.push(TextCodeTranslator.Translate("GLAccounts.O.LocalCurrencyErr"));
                //errors.push("The reconcile method for multi currency GLAaccount must be local currency"); // need a textcode to enable translations to hebrew
            }
        }

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            this.SubmitChanges();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    SubmitChanges() {
        if (this.AccountTypeCode == "3") { // vendor
            this.EntityPM.ControlAccountId = this.FullAccountingSetting.VendorControlAccountId;
        }

        this.EntityPM.AccountTypeCode = AppTool.IsNullOrEmpty(this.AccountTypeCode) ? "1" : this.AccountTypeCode;
        this.EntityPM.Inactive = false;
        this.EntityPM.IsControlAccount = false;
        this.myService.insert(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);

     
        
    }

    OnLovItemChanged(item: any){
        if (item == null) {
            this.ChartOfAccountsId = null;
            this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
            this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, false);
            this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "Chart Of Accounts is requierd");
        } else {
            this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, true);
            this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, true);
            this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, false, "");
        }
    }

    private FullAccountingSetting: FullAccountingSettingPM = new FullAccountingSettingPM();
    SelectDefaultValues() {
        // Select default value for Automatic Reconcilation
        this.entityListService.getSingle(this.TenantPM.Id.toString(), "FullAccountingSetting").then((res: any) => {
            res.subscribe(myResponse => {
                if (myResponse != null) {

                    var res = myResponse.Result;
                    this.FullAccountingSetting = res;
                    this.AutomaticReconcileId = res["AutomaticReconcileMethodId"];
                    //console.log(res);

                }
            })
        });

    }

}
