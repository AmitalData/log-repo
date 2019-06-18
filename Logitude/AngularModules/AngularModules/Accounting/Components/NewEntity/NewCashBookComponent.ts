import {Component, ChangeDetectorRef, OnInit} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {CashBookPM} from '../../EntityPMs/CashBookPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CashBookPMService} from '../../Services/StandardPMs/CashBookPMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'NewCashBookComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './NewCashBookComponent.html',
})

export class NewCashBookComponent extends BaseComponent implements OnInit {
    public EntityPM: CashBookPM;
    public DataContext: NewCashBookComponent = this;
    public ObjectTableName: string = "CashBook";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: CashBookPMService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public GLAccountsFilterItems: ApiQueryFilters;

    public isRTL: boolean = false;

    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService, public entityResourceService: EntityResourceService) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        // Create new cashbook
        this.EntityPM = new CashBookPM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.Inactive = false;
        this.EntityPM.TotalAmount = 0;

        this.myService = new CashBookPMService();
        this.SetUIProperties();
        this.SelectDefaultValues();
        this._entityResourceService.getEntityResourceByTableName("CashBook").subscribe((response: any) => { });

        this.InitLOVFilters();

    }

    InitLOVFilters() {
        // initialize query filters for Accounts
        this.GLAccountsFilterItems = new ApiQueryFilters();
        // this.GLAccountsFilterItems.addAdditionalFilter("IsMultiCurrency", true, null, null, "Equals", false, false, false, "boolean");
        // this.GLAccountsFilterItems.addAdditionalFilter("CurrencyId", this.CurrencyId, "OOORRR", null, "Equals", false, false, false, "string");
        this.GLAccountsFilterItems.addAdditionalFilter("SingleAndMultiCurrencyAccount", this.CurrencyId, null, null, "Equals", true, false, false, "string");
    }

    ngOnInit() {
    }

    //#region Properties

    //get Code() { return this.EntityPM.Id; }
    //set Code(value: string) {
    //    if (this.EntityPM.Id != value) {
    //        this.EntityPM.Id = value;
    //    }
    //}

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

    get CashBookTypeCode() { return this.EntityPM.CashBookTypeCode; }
    set CashBookTypeCode(value: string) {
        if (this.EntityPM.CashBookTypeCode != value) {
            this.EntityPM.CashBookTypeCode = value;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
            this.CheckCurrency();
        }

        // var filter = this.GLAccountsFilterItems.AdditionalFilters.find(d => d.FieldName == "CurrencyId");
        var filter = this.GLAccountsFilterItems.AdditionalFilters.find(d => d.FieldName == "SingleAndMultiCurrencyAccount");
        filter.FieldValue = this.CurrencyId;

    }

    get AccountId() { return this.EntityPM.AccountId; }
    set AccountId(value: string) {
        if (this.EntityPM.AccountId != value) {
            this.EntityPM.AccountId = value;
        }
    }

    get BranchId() { return this.EntityPM.BranchId; }
    set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;
        }
    }

    private account: GLAccountPM;
    get Account() { return this.account; }
    set Account(value: GLAccountPM) {
        if (this.account != value) {
            this.account = value;
            this.CheckCurrency();
        }
    }

    //#endregion

    OkButtonClicked() {
        this.CheckCurrency();
        if (this.ValidationErrorsList.length == 0) {

            var errors: string[] = [];

            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

            if (errors.length == 0) {

                //if (AppTool.IsNullOrEmpty(this.BranchId)) {
                //    this.ValidationErrorsList.push("Branch fields is requierd");
                //}

                this.SubmitChanges();
            } else {
                this.ValidationErrorsList = errors;
            }
        }
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    SubmitChanges() {
        this.myService.insert(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                var entity = mm.Result;
                SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");

                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    SessionLocator.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.CancelButtonClicked();
                        });
                    });
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                SessionLocator.CurrentSession.StopBusyIndicator();
            }
        });
    }

    SetUIProperties() {
        //this.UIProperties.SetEnabled("CashBooksId", this.ObjectTableName, false);
        //this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("BranchId", this.ObjectTableName, true);
    }

    SelectDefaultValues() {
        this.EntityPM.CreateDate = new Date();
        this.EntityPM.UpdateDate = new Date();
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
    }

    OnLovItemChanged(item: any) {
        if (!AppTool.IsNullOrEmpty(item)) {
            this.Account = item;
        }
    }

    CheckCurrency() {
        if (!AppTool.IsNullOrEmpty(this.Account) && !AppTool.IsNullOrEmpty(this.CurrencyId)) {
            if (this.Account.CurrencyId != this.CurrencyId) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.GLAccountcurnotmatchcashbookcur"));// "The GLAccount currency does not match the cashbook currency!");
            } else {
                this.ValidationErrorsList = [];
            }
        }
    }



}
