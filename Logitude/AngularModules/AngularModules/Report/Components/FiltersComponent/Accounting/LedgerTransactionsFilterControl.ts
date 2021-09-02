import { FeatureLocator } from './../../../../Infrastructure/Utilities/FeatureLocator';
import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { GLAccountPM } from '../../../../Accounting/EntityPMs/GLAccountPM';
import { CardListService } from '../../../../Common/Services/StandardLists/CardListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { GLAccountExtendedListService } from '../../../../Accounting/Services/ExtendedLists/GLAccountExtendedListService';
import { ReportsPreviewComponent } from '../../ReportsPreviewComponent';
import { EntityPartner } from '../../../../Infrastructure/DataContracts/EntityPartner';
import { CardExtendedPMService } from '../../../../Common/Services/ExtendedPMs/CardExtendedPMService';
import { AdvancedDatePickerResolverComponent } from '../../../../Infrastructure/Components/LogitudeComponents/AdvancedDatePickerResolverComponent';
import { ChartOfAccountPMService } from '../../../../Accounting/Services/StandardPMs/ChartOfAccountPMService';
import { ChartOfAccountPM } from '../../../../Accounting/EntityPMs/ChartOfAccountPM';
import { UserPM } from '../../../../Common/EntityPMs/UserPM';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { FullAccountingSettingList } from '../../../../Accounting/EntityLists/FullAccountingSettingList';


@Component({

    templateUrl: './LedgerTransactionsFilterControl.html',
})

export class LedgerTransactionsFilterControl extends BaseComponent implements OnInit
{
    public CurrencyFilters: any;

    ObjectTableName: string = "LedgerTransaction";
    public ReportsPreview: ReportsPreviewComponent;
    public RunReportTitle: string;
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public IsSchedulerReport: boolean = false;
    public GLAccountChanged: boolean = false;
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    isReady: boolean = false;
    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = false;
    public filterControlHight: string = "100";
    private CurrentSession = SessionLocator.SelectedSession;
    private entityListService: EntityListService = new EntityListService();
    public IsSalesmanRestricted: boolean = false;
    public SalesmanFilterItems: ApiQueryFilters;
    public ChartOfAccountTypeFilterItems: ApiQueryFilters;
    public GLAccountFilterItems: ApiQueryFilters;
    private chartOfAccountPMService: ChartOfAccountPMService = new ChartOfAccountPMService();
    private fullAccountingSetting: FullAccountingSettingList = new FullAccountingSettingList();

    constructor(private CD: ChangeDetectorRef)
    {
        super();

        this.InitLOVFilters();

        this.GetSalesmanFeature();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.GetResources();
        this.GetFullAccountingSettings();

    }
    GetFullAccountingSettings() {
        this.entityListService.getSingle(SessionLocator.Tenant.toString(), "FullAccountingSetting").then((res: any) => {
            this.CurrentSession.StopBusyIndicator();
            res.subscribe(myResponse => {
                if (myResponse != null) {

                    var res = myResponse.Result;
                    this.fullAccountingSetting = res;                   
                }
            })
        });
    }

    private GetResources()
    {
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) =>
        {
            this.entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe((response: any) =>
            {
                this.isReady = true;
                this.SetRunReportTitle();
            });
        });
    }

    private InitLOVFilters()
    {
        this.ChartOfAccountTypeFilterItems = new ApiQueryFilters();
        this.ChartOfAccountTypeFilterItems.addAdditionalFilter("CodeFilter", "3,4", null, null, "Exclude", false, false, false, "string", false, true);

        this.SalesmanFilterItems = new ApiQueryFilters();
        this.SalesmanFilterItems.addAdditionalFilter("IsSalesman", true, null, null, "Equals", false, false, false, "boolean", false, false);

        // GLAccount lov field filtera
        this.GLAccountFilterItems = new ApiQueryFilters();
        this.GLAccountFilterItems.addAdditionalFilter("AccountTypeCode", "4,5", null, null, "Exclude", false, false, false, "string", false, true);
    }
      private loggedUser: UserPM;

    GetSalesmanFeature()
    {
        var salesmanLedger = FeatureLocator.HasFeaturePermession("LedgerTransaction", "SalesmanLTRP");
        var isSalesmanRestrictionsEnabled =  !!salesmanLedger;
        console.log("[Salesman Ledger Transactions]", salesmanLedger);
         this.loggedUser = SessionLocator.LoggedUserPM;
        if (this.loggedUser.IsSalesman && isSalesmanRestrictionsEnabled) {
            this.IsSalesmanRestricted = true;
            this.Salesman = this.loggedUser.Id;
           this.GLAccountFilterItems.addAdditionalFilter("ConnectedToSalesmanId", this.loggedUser.Id, null, null, "Equals", true, true, false, "string", false, false);
        }

    }

    SetRunReportTitle()
    {
        if (this.isReady) {
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
            }
        }
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent)
    {
        this.ReportsPreview = myReportsPreview;
    }

    ngOnInit()
    {
        this.SetUIProperties();
        this.FillDefaultDateDetails();
    }

    FillDefaultDateDetails() {
        if (!this.IsSchedulerReport) {
            this.ToDate = AppTool.IsNullOrEmpty(this.ToDate) ? new Date() : this.ToDate;
            const today = new Date();
            const lastmonth = today.setMonth(today.getMonth() - 1);
            this.FromDate = AppTool.IsNullOrEmpty(this.FromDate) ? new Date(lastmonth) : this.FromDate;
        }
    }

    SetUIProperties()
    {
        this.UIProperties.SetRequired("FromDate", this.ObjectTableName, !this.FromDate);
        this.UIProperties.SetRequired("ToDate", this.ObjectTableName, !this.ToDate);

        if (this.IsSalesmanRestricted)
        {
            this.UIProperties.SetRequired("Salesman", "GLAccount", true);
            this.UIProperties.SetEnabled("Salesman", "GLAccount", false);
        }

    }

    //#region Filters

    //row 1
    private agingForDate: Date = null;
    public get AgingForDate() { return this.agingForDate; }
    public set AgingForDate(value: Date)
    {
        if (this.agingForDate != value) {
            this.agingForDate = value;

            this.ValidationErrorsList = [];
            this.ValidateDate();
        }
    }

    private customer: string;
    public get Customer() { return this.customer; }
    public set Customer(value: string)
    {
        if (this.customer != value) {
            this.customer = value;

            if (value)
                this.IsCategoryDisabled = true;
            else
                this.IsCategoryDisabled = false;
        }
    }

    ValidateDate()
    {
        var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {

            setTimeout(() =>
            {
                if (!this.IsOldDate("ToDate"))
                    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                if (!this.IsOldDate("FromDate"))
                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();
            }, 200);

        } else {
            setTimeout(() =>
            {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "");
                this.CD.detectChanges();
            }, 200);

        }
    }

    private IsOldDate(fieldName) {
        let isOldDate: boolean = false;
        const uiProperty = this.UIProperties.UIPropertyList.filter(uiProp => uiProp.FieldName == fieldName)[0];
        if (uiProperty)
            isOldDate = uiProperty.ValidationError == "Date time is too way in the past!" || uiProperty.ValidationError == "Invalid Date";
        
        return isOldDate;
    }

    private chartOfAccountId: string;

    get ChartOfAccountId() { return this.chartOfAccountId; }
    set ChartOfAccountId(value: string)
    {
        if (this.chartOfAccountId != value) {
            this.chartOfAccountId = value;


        }
    }
    
    private _ChartOfAccountsTypeCode : string;
    public get ChartOfAccountsTypeCode() : string {
        return this._ChartOfAccountsTypeCode;
    }
    public set ChartOfAccountsTypeCode(v : string) {
        this._ChartOfAccountsTypeCode = v;
    }
    
    private chartOfAccount: string;
    public get ChartOfAccount() { return this.chartOfAccount; }
    public set ChartOfAccount(value: string)
    {
        if (this.chartOfAccount != value) {
            this.chartOfAccount = value;
        }
    }

    private numberOfMonths: number;
    public get NumberOfMonths() { return this.numberOfMonths; }
    public set NumberOfMonths(value: number)
    {
        if (this.numberOfMonths != value) {
            this.numberOfMonths = value;
        }
    }


    //row 2

    private collector: string;
    public get Collector() { return this.collector; }
    public set Collector(value: string)
    {
        if (this.collector != value) {
            this.collector = value;
        }
    }

    private salesman: string;
    public get Salesman() { return this.salesman; }
    public set Salesman(value: string)
    {
        if (this.salesman != value) {
            this.salesman = value;
        }
    }

    //row 3

    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(value: string)
    {
        if (this.category1 != value) {
            this.category1 = value;
        }
    }


    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(value: string)
    {
        if (this.category2 != value) {
            this.category2 = value;
        }
    }

    private category3: string;
    public get Category3() { return this.category3; }
    public set Category3(value: string)
    {
        if (this.category3 != value) {
            this.category3 = value;
        }
    }

    private category4: string;
    public get Category4() { return this.category4; }
    public set Category4(value: string)
    {
        if (this.category4 != value) {
            this.category4 = value;
        }
    }

    //row 4
    private category5: string;
    public get Category5() { return this.category5; }
    public set Category5(value: string)
    {
        if (this.category5 != value) {
            this.category5 = value;
        }
    }

    private currenciesDetailed: boolean;
    public get CurrenciesDetailed() { return this.currenciesDetailed; }
    public set CurrenciesDetailed(value: boolean)
    {
        if (this.currenciesDetailed != value) {
            this.currenciesDetailed = value;
        }
    }


    //#endregion

    //#region Filter Methods
    public filterSelectedValue: string = 'filter_accounting';
    public _dateTypeCode: string = '1';
    FilterItemClicked(itemValue: string)
    {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterLines();
        }
    }
    FilterLines()
    {

        //Task 46666: Transaction Tab - date filter new design
        // <DateTypeCode>2</DateTypeCode> 1/2/3
        // Accounting- - code 1- חשבונםי
        // Due - code 2 - לגביה
        // Reference -code-3-  םסמכתם

        switch (this.filterSelectedValue) {
            case 'filter_accounting':
                this._dateTypeCode = '1';
                break;
            case 'filter_due':
                this._dateTypeCode = '2';
                break;
            case 'filter_reference':
                this._dateTypeCode = '3';
                break;
            default:
                break;
        }
    }
    //#endregion

    SetFilterSelectedValue()
    {
        switch (this._dateTypeCode) {
            case '1':
                this.filterSelectedValue = 'filter_accounting';
                break;
            case '2':
                this.filterSelectedValue = 'filter_due';
                break;
            case '3':
                this.filterSelectedValue = 'filter_reference';
                break;
            default:
                break;
        }
    }

    GetMainCustomerFieldName() {
        return 'GLAccountId';
    }

    IsPartnersChanged(SelectedTab) {
        if (SelectedTab == '2')
            this.GLAccountChanged = false;
        return this.GLAccountChanged;
    }

    GetQueryFilterItems()
    {
        var queryFilterItems = new Array<QueryFilterItem>();
        var queryFilterItem: QueryFilterItem;

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "FromDate";
        queryFilterItem.FieldDataType = 'Date';
        queryFilterItem.FieldValue = this.FromDate ? this.FromDate : null;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "ToDate";
        queryFilterItem.FieldDataType = 'Date';
        queryFilterItem.FieldValue = this.ToDate ? this.ToDate : null;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "GLAccountId";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.GLAccountId);
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "SalesmanUserId";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.salesman);
        queryFilterItem.Operator = "Equals";
        queryFilterItem.DisplayInList = true; // server code will take this value from DB.ObjectField.DisplayInList
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "ChartOfAccountId";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.ChartOfAccountId);
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "ChartOfAccountsTypeCode";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.ChartOfAccountsTypeCode);
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "CurrencyId";
        queryFilterItem.FieldValue = this.GetLookUpFieldValue(this.CurrencyId);
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "IncludeChildAccounts";
        queryFilterItem.FieldValue = this.IncludeChildAccounts ? true : false;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        //queryFilterItem = new QueryFilterItem();
        //queryFilterItem.FieldName = "SearchFields";
        //queryFilterItem.FieldValue = this.SearchFields ? this.SearchFields : null;
        //queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "DateTypeCode";
        queryFilterItem.FieldValue = this._dateTypeCode;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.FieldName = "IncludeRelatedCurrenciesAccount";
        queryFilterItem.FieldValue = this.IncludeRelatedCurrenciesAccount ? true : false;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem)

        queryFilterItem = new QueryFilterItem();
        this.IsReconciled = this.AttachedGLAccountCheckBox ? null : false;
        queryFilterItem.FieldName = "IsReconciled";
        queryFilterItem.Operator = "Equals";
        queryFilterItem.FieldValue = this.IsReconciled;
        queryFilterItems.push(queryFilterItem)

        // category
        var categoryIndex = null;
        var categoryValue = null;
        if (this.SelectedCategory) {
            categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category
            if (categoryIndex)
                categoryValue = this.DataContext[categoryIndex]; // select the value from the context
        }
        queryFilterItems.push(new QueryFilterItem("CategoryIndex", categoryIndex)); // 'Category1' , 'Category2' , ...
        queryFilterItems.push(new QueryFilterItem("CategoryValue", categoryValue));
  
        queryFilterItems.push(new QueryFilterItem("UseSecurityLevel", this.fullAccountingSetting.IsSecurityLevelActivated));
        return queryFilterItems;
    }

    GetLookUpFieldValue(field)
    {
        if (field) {
            if (field[0]["@nil"] != "true")
                return field;
        }
        return null
    }

    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>)
    { //For Scheduler Report
        this.IsSchedulerReport = true;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem =>
            {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }

    private SetFilterItem(queryFilterItem: QueryFilterItem)
    {
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "FromDate":
                    this.FromDate = queryFilterItem.FieldValue;
                    break;
                case "ToDate":
                    this.ToDate = queryFilterItem.FieldValue;
                    break;
                case "GLAccountId":
                    this.GLAccountId = queryFilterItem.FieldValue;
                    break;
                case "ChartOfAccountId":
                    this.ChartOfAccountId = queryFilterItem.FieldValue;
                    break;
                case "CurrencyId":
                    this.CurrencyId = queryFilterItem.FieldValue;
                    break;
                case "IncludeChildAccounts":
                    this.IncludeChildAccounts = queryFilterItem.FieldValue;
                    break;
                case "SearchFields":
                    this.SearchFields = queryFilterItem.FieldValue;
                    break;
                case "DateTypeCode":
                    this._dateTypeCode = queryFilterItem.FieldValue;
                    this.SetFilterSelectedValue();
                    break;
                case "IncludeRelatedCurrenciesAccount":
                    this.IncludeRelatedCurrenciesAccount = queryFilterItem.FieldValue;
                    break;
                case "IsReconciled":
                    this.IsReconciled = queryFilterItem.FieldValue;
                    this.AttachedGLAccountCheckBox = queryFilterItem.FieldValue;
                    break;
                case "SalesmanUserId":
                    this.Salesman = queryFilterItem.FieldValue;
                    break;
                case "CategoryIndex":
                    this.SelectedItemChanged(this.GetLookUpFieldValue(queryFilterItem.FieldValue));
                    break;
                case "CategoryValue":
                    //CategoryValue
                    break;
                case "ChartOfAccountsTypeCode":
                    this.ChartOfAccountsTypeCode = queryFilterItem.FieldValue;
                    break;
            }
        }
    }

    ValidateSelectedFilters()
    {
        this.ValidationErrorsList = [];
        var isValid: boolean = true;

        if (!this.GLAccountId && !this.ChartOfAccountId && !this.ChartOfAccountsTypeCode && !this.SelectedCategoryValue && !this.Salesman) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("GLTransactionReport.O.RequiredFields"));
            isValid = false;
        }

        if (!this.FromDate) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("GLAccounts.O.fromfieldrequired"));
            isValid = false;
        }
        if (!this.ToDate) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("GLAccounts.O.tofieldrequired"));
            isValid = false;
        }
       
        var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
            isValid = false;
        }
        
        if (this.ValidationErrorsList.length == 0)
            this.filterControlHight = "100";
        else
            this.filterControlHight = "80";
        isValid=  this.CheckIfChartOfAccountAndUserSecurityLevelAreMatched();
        return isValid;
    }


    public get SelectedCategoryValue(): string
    {
        if (this.SelectedCategory) {
            var categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category
            if (categoryIndex)
                return this.DataContext[categoryIndex]; // select the value from the context
        }
        return null;
    }


    PrepareContactList()
    {
        var cardExtendedPMService = new CardExtendedPMService();
        var glAccountId = this.GetLookUpFieldValue(this.GLAccountId);
        if (glAccountId != null) {
            cardExtendedPMService.GetAllConnectedPartnersByGLAccountId(glAccountId).subscribe((response: ServiceResponse) =>
            {
                if (!response.HasError) {
                    var allContacts = response.Result;
                    if (allContacts != null && allContacts.length > 0) {
                        allContacts.forEach(contact =>
                        {
                            if (!AppTool.IsNullOrEmpty(contact)) this.ReportsPreview.AddPartner(contact.PartnerName, contact.PartnerId);
                        });
                        this.ReportsPreview.PartnersObslist.reverse();
                    }
                }
            });
        }
    }

    RunButtonClicked()
    {
        this.SetUIProperties();

        var errors: string[] = [];
        var categoryValue = null;
        var categoryIndex = null;

        if (this.ValidateSelectedFilters()) {


            // // Selecting category
            // if (this.SelectedCategory) {
            //     categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category

            //     if (categoryIndex)
            //         categoryValue = this.DataContext[categoryIndex]; // select the value from the context
            // }
            // myFilterItems.push(new QueryFilterItem("CategoryIndex", categoryIndex)); // 'Category1' , 'Category2' , ...
            // myFilterItems.push(new QueryFilterItem("CategoryValue", categoryValue));

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = this.GetQueryFilterItems();

            this.RunReportEvent.emit(myReportFliter);

        }
    }

    //#region Category fields
    IsCategoryDisabled: boolean = false;
    CategoriesList: string[] = [
        'Category 1',
        'Category 2',
        'Category 3',
        'Category 4',
        'Category 5'
    ];
    SelectedCategory: string;
    SelectedItemChanged(item)
    {
        this.SelectedCategory = item;
    }
    //#endregion

    //#region Properties
    //OpenAmountHint: string = "";
    private openAmountHint: string;
    get OpenAmountHint() { return this.openAmountHint; }
    set OpenAmountHint(value: string)
    {
        if (this.openAmountHint != value) {
            this.openAmountHint = value;
        }
    }

    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date)
    {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.ValidateDate();
            if (!this.IsOldDate("FromDate"))
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, !value);
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date)
    {
        if (this.toDate != value) {
            this.toDate = value;
            this.ValidateDate();
            if (!this.IsOldDate("ToDate"))
            this.UIProperties.SetRequired("ToDate", this.ObjectTableName, !value);

        }
    }

    private currencyId: string;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(value: string)
    {
        if (this.currencyId != value) {
            this.currencyId = value;
        }
    }


    private attachedGLAccountCheckBox: boolean = true;
    get AttachedGLAccountCheckBox() { return this.attachedGLAccountCheckBox; }
    set AttachedGLAccountCheckBox(value: boolean)
    {
        if (this.attachedGLAccountCheckBox != value) {
            this.attachedGLAccountCheckBox = value;

        }
    }

    private splittedByCurrencyCheckBox: boolean = false;
    get SplittedByCurrencyCheckBox() { return this.splittedByCurrencyCheckBox; }
    set SplittedByCurrencyCheckBox(value: boolean)
    {
        if (this.splittedByCurrencyCheckBox != value) {
            this.splittedByCurrencyCheckBox = value;

        }
    }

    private _GLAccountId: string;
    get GLAccountId() { return this._GLAccountId; }
    set GLAccountId(value: string)
    {
        if (this._GLAccountId != value) {
            this.SetGLAccountChanged(this._GLAccountId);
            this._GLAccountId = value;
        }
    }


    private SetGLAccountChanged(value: string) {
        if (value != undefined)
            this.GLAccountChanged = true;
    }

    private securityLevel: any;
    private glaccountPM: any;
    
    get GLAccount() { return this.glaccountPM; }
    set GLAccount(value: any)
    {
        if (this.glaccountPM != value) {
            this.glaccountPM = value;
            if (this.glaccountPM) {
                if (this.glaccountPM.IsMultiCurrency) {

                    this.CurrencyId = !AppTool.IsNullOrEmpty(this.CurrencyId) && this.IsSchedulerReport ? this.CurrencyId : null;
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
                } else {
                    this.CurrencyId = !AppTool.IsNullOrEmpty(this.CurrencyId) && this.IsSchedulerReport ? this.CurrencyId : this.glaccountPM.CurrencyId;
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                }
             
                this.securityLevel = this.SetGLAccountChartOfAccountSecurityLevel(value.ChartOfAccountsId);
            }

        }
    }

    private  SetGLAccountChartOfAccountSecurityLevel(ChartOfAccountId: string) {
        this.chartOfAccountPMService.get(ChartOfAccountId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var chartOfAccount: ChartOfAccountPM = response.Result;
                this.securityLevel = chartOfAccount.ChartOfAccountSecurityLevel;
            }
        });
    }
    private CheckIfChartOfAccountAndUserSecurityLevelAreMatched() {
        if (this.GLAccount && !this.loggedUser.IsCustomerCare) {
            if (this.securityLevel == undefined) {
                this.securityLevel = 0;
            }
            if (this.securityLevel > this.loggedUser.SecurityLevel) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("ChartOfAccounts.O.SecurityLevelErrorMessage"));
                return false;
            }
            else return true;
        }
        else return true;
    }
   
    private _IsReconciled: boolean;
    public get IsReconciled(): boolean
    {
        return this._IsReconciled;
    }
    public set IsReconciled(v: boolean)
    {
        this._IsReconciled = v;
    }


    private _IncludeChildAccounts: boolean;
    public get IncludeChildAccounts(): boolean
    {
        return this._IncludeChildAccounts;
    }
    public set IncludeChildAccounts(v: boolean)
    {
        this._IncludeChildAccounts = v;
    }

    private _IncludeRelatedCurrenciesAccount: boolean;
    public get IncludeRelatedCurrenciesAccount(): boolean
    {
        return this._IncludeRelatedCurrenciesAccount;
    }
    public set IncludeRelatedCurrenciesAccount(v: boolean)
    {
        this._IncludeRelatedCurrenciesAccount = v;
    }


    private _SearchFields: string;
    public get SearchFields(): string
    {
        return this._SearchFields;
    }
    public set SearchFields(v: string)
    {
        this._SearchFields = v;
    }

    //#endregion


}
