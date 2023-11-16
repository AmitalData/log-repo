import {Component, ChangeDetectorRef}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {GLAccountValidator} from '../../../Validators/GLAccountValidator';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { GLAccountExtendedPMService } from '../../../Services/ExtendedPMs/GLAccountExtendedPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { FullAccountingSettingPM } from '../../../EntityPMs/FullAccountingSettingPM';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';

@Component({

    templateUrl: './GLAccountGeneralTabComponent.html',
    providers: [GLAccountExtendedListService]
})

export class GLAccountGeneralTabComponent extends BaseComponent {
    public oldCurrency: string = null;
    public oldIsMultiCurrency: boolean = null;
    public EntityPM: GLAccountPM = null;
    public ObjectTableName = "GLAccount";
    public DataContext = this;
    public DisableGLAccount: boolean = false;
    public IsEditMode: boolean = false;
    public IsCustomerAccount: boolean = false;
    public ChartOfAccountTypeFilterItems: ApiQueryFilters;
    public ParentsFilterItems: ApiQueryFilters;
    public IsVendor: boolean = false;
    public EnableFollowUpData: boolean = false;
    public IsVendorChartOfAccount: boolean = false;
    public isRTL: boolean = false;
    public TenantPM: TenantPM;
    public AccountHasATransactions: boolean = false;

    private _GLAccountExtendedListService = new GLAccountExtendedListService();
    private gLAccountExtendedPMService = new GLAccountExtendedPMService();
    private FullAccountingSetting: FullAccountingSettingPM = new FullAccountingSettingPM();

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        // Set Entity
        this.EntityPM = entityArgs.EntityPM;
        this.oldCurrency = this.EntityPM.CurrencyId;
        this.oldIsMultiCurrency = this.EntityPM.IsMultiCurrency;
        this.TenantPM = SessionLocator.TenantPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");


        //#region initialize query filters for ChartOfAccountType
        this.ChartOfAccountTypeFilterItems = new ApiQueryFilters();
        this.GetConnectedCards(this.EntityPM.Id).then((connectedCards: CardList[]) => {
            var firstConnectedCard = connectedCards[0];

                if (firstConnectedCard && firstConnectedCard.PartnerTypeId == "AC") {
                this.ChartOfAccountTypeFilterItems.addAdditionalFilter("CodeFilter", "5,6", null, null, "Exclude", false, false, false, "string", false, true);
            }
            else {
                this.ChartOfAccountTypeFilterItems.addAdditionalFilter("Code", "3,4", null, null, "Exclude", false, false, false, "string", false, true);
            }
        });


        this.CheckIfAccountHasTransactions();



        //this.ChartOfAccountTypeFilterItems.addAdditionalFilter("Code", "4", null, null, "NotEqual", false, false, false, "string", false, true);
        //#endregion



        this.SetupFiels();
        this.SetUIProperties();

        this.Listen();
    }
    private CheckIfAccountHasTransactions()
    {
        this._GLAccountExtendedListService.GetAccountTransactionsCount(this.EntityPM.Id).subscribe(res =>
        {
            var count = res.Result;
            this.AccountHasATransactions = count > 0;
            console.log("[GetAccountTransactionsCount]", res);

            this.SetUIProperties();

        });
    }

  SetupFiels(){
    if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {// Edit Mode
        if (this.ChartOfAccountsTypeCode == "4") { this.IsVendorChartOfAccount = true;}

      this.IsEditMode = true;

      this.IsVendor = false;
        if (this.EntityPM.AccountTypeCode == "5" || this.EntityPM.AccountTypeCode == "4") {
            this.DisableGLAccount = true;
            this.SetFieldsEditablility(false);
        } else if (this.EntityPM.AccountTypeCode == "2" || this.EntityPM.AccountTypeCode == "3")
        {
            this.IsCustomerAccount = true;
            this.UIProperties.SetEnabled("ChartOfAccountsTypeCode", this.ObjectTableName, false);
            // this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, false);
            // this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, false);
        } else if(this.EntityPM.AccountTypeCode == "1" || this.EntityPM.AccountTypeCode == "3"){
            this.EnableFollowUpData = true;
        }
         if (this.EntityPM.AccountTypeCode == "3") {
          this.IsVendor = true;

      }

        //#region initialize query filters for Parent Account
        this.ParentsFilterItems = new ApiQueryFilters();
        this.ParentsFilterItems.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "Exclude", false, false, false, "string", false, true);
        this.ParentsFilterItems.addAdditionalFilter("ChartOfAccountsId", this.EntityPM.ChartOfAccountsId, null, null, "Equals", false, false, false, "string", false, true);
        this.ParentsFilterItems.addAdditionalFilter("ParentAccountId", "Please Don't Erase Me", null, null, "IsNull", false, false, false, "string", false, true);


        if (this.IsVendor) {
            this.ParentsFilterItems.addAdditionalFilter("AccountTypeCode", "3", null, null, "Equals", false, false, false, "string", false, true);
        } else if (this.IsCustomerAccount) {
            this.ParentsFilterItems.addAdditionalFilter("AccountTypeCode", "2", null, null, "Equals", false, false, false, "string", false, true);
        }
        //#endregion

        if (this.ChartOfAccountsTypeCode == "1") { // 1-Revenues
          this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);

        } else if (this.ChartOfAccountsTypeCode == "2") { // 2-Expenses
          this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);

        } else {
          this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, true);

      }



    }
  }
  GetConnectedCards(accountId: string)
    {

        return new Promise(resolve =>
        {
            this.entityArgs.EditComponent.StartBusyIndicatorLoading();
            this.gLAccountExtendedPMService.GetConnectedCardsForGLAccount(accountId)
                .subscribe((myResponse: ServiceResponse) =>
                {
                    this.entityArgs.EditComponent.StopBusyIndicator();
                    var connectedCards = myResponse.Result;
                    if (connectedCards)
                        resolve(connectedCards);
                });
        });
    }

    SelectDefaultValues() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityListService.getSingle(this.TenantPM.Id.toString(), "FullAccountingSetting").then((res: any) => {
        this.CurrentSession.StopBusyIndicator();
            res.subscribe(myResponse => {
                if (myResponse != null) {

                    var res = myResponse.Result;
                    this.FullAccountingSetting = res;
                    if(this.EntityPM.ChartOfAccountsTypeCode=="4")
                    this.EntityPM.ControlAccountId = this.FullAccountingSetting.VendorControlAccountId;
                    if(this.EntityPM.ChartOfAccountsTypeCode=="3")
                    this.EntityPM.ControlAccountId = this.FullAccountingSetting.CustomerControlAccountId;
                }
            })
        });

    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private TabSelectedEvent: any = null;
    public CurrentEditComponentId: string;
    Listen() {


        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            //
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetupFiels();
                        this.SetUIProperties();
                    }
                });
            }

            //
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetupFiels();
                        this.SetUIProperties();
                        console.log("Entity Reloaded");
                    }
                });
            }

        }
    }
    //DownloadButtonClicked() {
    //    this._GLAccountExtendedListService.CalculateFututreCheques().subscribe((myResult:any) => {




    //    });
    //}
    //#region Properties
    get IsMultiCurrency() { return this.EntityPM.IsMultiCurrency == null ? false : this.EntityPM.IsMultiCurrency; }
    set IsMultiCurrency(value: boolean) {

        if (this.EntityPM.IsMultiCurrency != value) {
            this.EntityPM.IsMultiCurrency = value;

            if (value != this.oldIsMultiCurrency)
                GLAccountValidator.ValidateIsMultiCurrency(this.EntityPM);

            if (value) {
                // Change UI Property
                this.CurrencyId = null;
                this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, false);
                this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, true, "");
                this.ReconcileMethodCode = '0';
                this.CD.detectChanges();



            } else {
                this.EntityPM.IsMultiCurrency = value;
                this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
                this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
                this.ReconcileMethodCode = null;

                this.CD.detectChanges();
            }
        }
    }
    get RevaluationEnabled() { return this.EntityPM.RevaluationEnabled }
    set RevaluationEnabled(value: boolean) {
        if (this.EntityPM.RevaluationEnabled != value) {
            this.EntityPM.RevaluationEnabled = value;

        }
  }

    get IsVATExempt() { return this.EntityPM.IsVATExempt }
    set IsVATExempt(value: boolean) {
        if (this.EntityPM.IsVATExempt != value) {
            this.EntityPM.IsVATExempt = value;

        }
    }



  get IsEquipmentVendor() { return this.EntityPM.IsEquipmentVendor }
  set IsEquipmentVendor(value: boolean) {
    if (this.EntityPM.IsEquipmentVendor != value) {
      this.EntityPM.IsEquipmentVendor = value;

    }
    }

    get ExcludeFromDeductionReport() { return this.EntityPM.ExcludeFromDeductionReport }
    set ExcludeFromDeductionReport(value: boolean) {
        if (this.EntityPM.ExcludeFromDeductionReport != value) {
            this.EntityPM.ExcludeFromDeductionReport = value;

        }
    }

    get Smallcashbook() { return this.EntityPM.Smallcashbook }
    set Smallcashbook(value: boolean) {
        if (this.EntityPM.Smallcashbook != value) {
            this.EntityPM.Smallcashbook = value;

        }
    }


    get ReportingAsAnotherDocument() { return this.EntityPM.ReportingAsAnotherDocument }
    set ReportingAsAnotherDocument(value: boolean) {
        if (this.EntityPM.ReportingAsAnotherDocument != value) {
            this.EntityPM.ReportingAsAnotherDocument = value;

        }
    }

    IsMultiCurrencyCheckboxEnabled: boolean = true;
    get ChartOfAccountsTypeCode() { return this.EntityPM.ChartOfAccountsTypeCode; }
    set ChartOfAccountsTypeCode(value: string) {
        if (this.EntityPM.ChartOfAccountsTypeCode != value) {
            this.EntityPM.ChartOfAccountsTypeCode = value;
            this.ChartOfAccountsId = null;

            if (!AppTool.IsNullOrEmpty(value)) {
                if (value == "4") {
                    this.IsVendorChartOfAccount = true;
                    this.SelectDefaultValues();
                    this.EntityPM.RevenueExpenseType = "3";}
                if (value == "3") {
                    this.SelectDefaultValues();
                    this.EntityPM.RevenueExpenseType = "3";}

               else if (value == "1" || value == "2"){ // 1-Revenues, 2-Expenses
                    if (value == "1") {
                        this.EntityPM.RevenueExpenseType = "1";
                    }
                    else {
                        this.EntityPM.RevenueExpenseType = "2";
                    }
                    // disable fields
                    this.IsMultiCurrency = true;
                    this.CurrencyId = null;
                    this.IsMultiCurrencyCheckboxEnabled = false;
                    this.UIProperties.SetEnabled("IsMultiCurrency", this.ObjectTableName, false);
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);

                    // disable fields
                    this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, false);

                    // set type
                    this.RevenueExpenseType = value;
                } else {
                    // enable fields
                    if (this.EntityPM.RevenueExpenseType == "3") {
                        this.EntityPM.RevenueExpenseType = "1";
                    }
                    this.IsMultiCurrency = false;
                    this.IsMultiCurrencyCheckboxEnabled = true;
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, true);
                }

            } else {
                this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, true);
            }
            if(!this.EntityPM.IsControlAccount) this.EntityPM.AccountTypeCode = null;
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
                filter.FieldValue = this.EntityPM.ChartOfAccountsId;
        }
    }

    get DisplayNumber() { return this.EntityPM.DisplayNumber; }
    set DisplayNumber(value: string) {
        if (this.EntityPM.DisplayNumber != value) {
            this.EntityPM.DisplayNumber = value;
        }
    }
 get NameForPrintingCheques() { return this.EntityPM.NameForPrintingCheques; }
    set NameForPrintingCheques(value: string) {
        if (this.EntityPM.NameForPrintingCheques != value) {
            this.EntityPM.NameForPrintingCheques = value;
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

            // Check transactions
            GLAccountValidator.ValidateCurrency(this.EntityPM, this.oldCurrency);

            if (value != null) {
                this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, true, "");
            }
            else {
                this.UIProperties.SetValidity("CurrencyId", this.ObjectTableName, false, "");
                this.EntityPM.CurrencyCode = null;
            }

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

    get FollowupDate() { return this.EntityPM.FollowupDate; }
    set FollowupDate(value: Date) {
        if (this.EntityPM.FollowupDate != value) {
            this.EntityPM.FollowupDate = value;
        }
    }

    get FollowupNotes() { return this.EntityPM.FollowupNotes; }
    set FollowupNotes(value: string) {
        if (this.EntityPM.FollowupNotes != value) {
            this.EntityPM.FollowupNotes = value;
        }
    }

    get PaymentTerms() { return this.EntityPM.PaymentTerms; }
    set PaymentTerms(value: string) {
        if (this.EntityPM.PaymentTerms != value) {
            this.EntityPM.PaymentTerms = value;
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

    get AllowEditChequePayToName() { return this.EntityPM.AllowEditChequePayToName; }
    set AllowEditChequePayToName(value: boolean) {
        if (this.EntityPM.AllowEditChequePayToName != value) {
            this.EntityPM.AllowEditChequePayToName = value;
                    }
    }
    //#endregion


    SetUIProperties() {
        if (!this.EntityPM || this.DisableGLAccount)
            return;

        if (this.EntityPM.IsMultiCurrency) {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        }
        if (this.EntityPM.ChartOfAccountsTypeCode) {
            this.ChartOfAccountsTypeCode = this.EntityPM.ChartOfAccountsTypeCode;
        }
        if (this.EntityPM.ChartOfAccountsId) {
            this.ChartOfAccountsId = this.EntityPM.ChartOfAccountsId;
            this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "");
        }

        if (this.EntityPM.ParentCurrencyId != null) {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
            this.IsMultiCurrencyCheckboxEnabled=  false;
        }

        if (this.EntityPM.RevenueExpenseType == "3") {


            this.UIProperties.SetEnabled("IsVATExempt", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetEnabled("IsVATExempt", this.ObjectTableName, true);
        }

        if (this.ChartOfAccountsTypeCode == "1" || this.ChartOfAccountsTypeCode == "2"){ // 1-Revenues, 2-Expenses
            this.IsMultiCurrencyCheckboxEnabled = false;
            this.UIProperties.SetEnabled("IsMultiCurrency", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        }

        if(this.AccountHasATransactions)
            this.UIProperties.SetEnabled("ChartOfAccountsTypeCode", this.ObjectTableName, false);

    }

    SetFieldsEditablility(enable) {
        this.UIProperties.SetEnabled("IsMultiCurrency", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ChartOfAccountsTypeCode", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("DisplayNumber", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ReconcileMethodCode", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("AutomaticReconcileId", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("ParentAccountId", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category1Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category2Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category3Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category4Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("Category5Id", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("RevenueExpenseType", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("FollowupDate", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("FollowupNotes", this.ObjectTableName, enable);
        this.UIProperties.SetEnabled("PaymentTerms", this.ObjectTableName, enable);
    }


    OnLovItemChanged(item: any) {
        if (item == null) {
            this.ChartOfAccountsId = null;
            if (!this.DisableGLAccount) {
                this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
                this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, false);
                this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "Chart Of Accounts is requierd");
            }
        } else {
            if (!this.DisableGLAccount) {
                this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, true);
                if (!this.EntityPM.ChartOfAccountsId) {
                    this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, false, "");
                    this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, true);
                }
            }
        }
    }

    GetDisplayMemberPath(){
        var showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        return showLocal ? "LocalName" : "EnglishName";
    }

}
