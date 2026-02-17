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

@Component({
    moduleId: module.id,
    templateUrl: './LedgerTransactionsFilterControl.html',
})

export class LedgerTransactionsFilterControl extends BaseComponent implements OnInit {
    ObjectTableName: string = "LedgerTransaction";
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    isReady: boolean = false;
    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = false;
    constructor(private CD: ChangeDetectorRef) {
        super();



        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");


        // get requierd resources
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("LedgerTransaction").subscribe(response => { this.isReady = true; });
        });





    }

    ngOnInit() {
        this.SetUIProperties();

        //#region Fill Date Default Values
        var today = new Date();
        this.ToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        this.FromDate = new Date(lastmonth);
        //#endregion


    }

    SetUIProperties() {
        // this.UIProperties.SetRequired("AgingForDate", "GLAccount", true);

        this.UIProperties.SetRequired("GLAccountId", this.ObjectTableName, !this.GLAccountId);
        this.UIProperties.SetRequired("FromDate", this.ObjectTableName, !this.FromDate);
        this.UIProperties.SetRequired("ToDate", this.ObjectTableName, !this.ToDate);
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);

    }

    //#region Filters

    //row 1
    private agingForDate: Date = null;
    public get AgingForDate() { return this.agingForDate; }
    public set AgingForDate(value: Date) {
        if (this.agingForDate != value) {
            this.agingForDate = value;

            this.ValidationErrorsList = [];
            this.ValidateDate();
        }
    }

    private customer: string;
    public get Customer() { return this.customer; }
    public set Customer(value: string) {
        if (this.customer != value) {
            this.customer = value;

            if (value)
                this.IsCategoryDisabled = true;
            else
                this.IsCategoryDisabled = false;
        }
    }

    ValidateDate() {
        if (this.FromDate > this.ToDate) {

            setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();
            }, 200);

        } else {
            setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "");
                this.CD.detectChanges();
            }, 200);

        }
    }

    //private chartOfAccount: string;
    //public get ChartOfAccount() { return this.chartOfAccount; }
    //public set ChartOfAccount(value: string) {
    //    if (this.chartOfAccount != value) {
    //        this.chartOfAccount = value;
    //    }
    //}

    private numberOfMonths: number;
    public get NumberOfMonths() { return this.numberOfMonths; }
    public set NumberOfMonths(value: number) {
        if (this.numberOfMonths != value) {
            this.numberOfMonths = value;
        }
    }


    //row 2

    private collector: string;
    public get Collector() { return this.collector; }
    public set Collector(value: string) {
        if (this.collector != value) {
            this.collector = value;
        }
    }

    private salesman: string;
    public get Salesman() { return this.salesman; }
    public set Salesman(value: string) {
        if (this.salesman != value) {
            this.salesman = value;
        }
    }

    //row 3

    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(value: string) {
        if (this.category1 != value) {
            this.category1 = value;
        }
    }


    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(value: string) {
        if (this.category2 != value) {
            this.category2 = value;
        }
    }

    private category3: string;
    public get Category3() { return this.category3; }
    public set Category3(value: string) {
        if (this.category3 != value) {
            this.category3 = value;
        }
    }

    private category4: string;
    public get Category4() { return this.category4; }
    public set Category4(value: string) {
        if (this.category4 != value) {
            this.category4 = value;
        }
    }

    //row 4
    private category5: string;
    public get Category5() { return this.category5; }
    public set Category5(value: string) {
        if (this.category5 != value) {
            this.category5 = value;
        }
    }

    private currenciesDetailed: boolean;
    public get CurrenciesDetailed() { return this.currenciesDetailed; }
    public set CurrenciesDetailed(value: boolean) {
        if (this.currenciesDetailed != value) {
            this.currenciesDetailed = value;
        }
    }


    //#endregion

    //#region Filter Methods
    public filterSelectedValue: string = 'filter_accounting';
    public _dateTypeCode: string = '1';
    FilterItemClicked(itemValue: string) {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterLines();
        }
    }
    FilterLines() {

        //Task 46666: Transaction Tab - date filter new design
        // <DateTypeCode>2</DateTypeCode> 1/2/3
        // Accounting- - code 1- חשבונאי
        // Due - code 2 - לגביה
        // Reference -code-3-  אסמכתא

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

    RunButtonClicked() {
        this.SetUIProperties();

        var errors: string[] = [];
        var categoryValue = null;
        var categoryIndex = null;
        this.ValidationErrorsList = [];

        //#region requierd fields
        if (!this.GLAccountId) { errors.push(TextCodeTranslator.Translate("GLTransactionReport.O.GLAccountZrequierd")); }
        //#endregion

        //#region Date validation
        if(!this.FromDate)
            errors.push(TextCodeTranslator.Translate("GLAccounts.O.fromfieldrequired"));
        if(!this.ToDate)
            errors.push(TextCodeTranslator.Translate("GLAccounts.O.tofieldrequired"));

        if (this.FromDate > this.ToDate) {
            errors.push(TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
        }
        //#endregion

        if (errors.length == 0) {


            // // Selecting category
            // if (this.SelectedCategory) {
            //     categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category

            //     if (categoryIndex)
            //         categoryValue = this.DataContext[categoryIndex]; // select the value from the context
            // }

            var myFilterItems: QueryFilterItem[] = [];
            myFilterItems.push(new QueryFilterItem("FromDate", this.FromDate ? this.FromDate : null));
            myFilterItems.push(new QueryFilterItem("ToDate", this.ToDate ? this.ToDate : null));
            myFilterItems.push(new QueryFilterItem("GLAccountId", this.GLAccountId ? this.GLAccountId : null));
            myFilterItems.push(new QueryFilterItem("CurrencyId", this.CurrencyId ? this.CurrencyId : null));
            myFilterItems.push(new QueryFilterItem("IsReconciled", this.IsReconciled ? this.IsReconciled : null));
            myFilterItems.push(new QueryFilterItem("IncludeChildAccounts", this.IncludeChildAccounts ? this.IncludeChildAccounts : null));
            myFilterItems.push(new QueryFilterItem("SearchFields", this.SearchFields ? this.SearchFields : null));
            myFilterItems.push(new QueryFilterItem("DateTypeCode", this._dateTypeCode ? this._dateTypeCode : null));


            // myFilterItems.push(new QueryFilterItem("CategoryIndex", categoryIndex)); // 'Category1' , 'Category2' , ...
            // myFilterItems.push(new QueryFilterItem("CategoryValue", categoryValue));

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;

            this.RunReportEvent.emit(myReportFliter);

        } else {
            this.ValidationErrorsList = errors;
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
    SelectedItemChanged(item) {
        this.SelectedCategory = item;
    }
    //#endregion

    //#region Properties
    //OpenAmountHint: string = "";
    private openAmountHint: string;
    get OpenAmountHint() { return this.openAmountHint; }
    set OpenAmountHint(value: string) {
        if (this.openAmountHint != value) {
            this.openAmountHint = value;
        }
    }

    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.ValidateDate();
            this.UIProperties.SetRequired("FromDate", this.ObjectTableName, !value);
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.ValidateDate();
            this.UIProperties.SetRequired("ToDate", this.ObjectTableName, !value);

        }
    }

    private currencyId: string;
    get CurrencyId() { return this.currencyId; }
    set CurrencyId(value: string) {
        if (this.currencyId != value) {
            this.currencyId = value;
        }
    }


    private attachedGLAccountCheckBox: boolean = false;
    get AttachedGLAccountCheckBox() { return this.attachedGLAccountCheckBox; }
    set AttachedGLAccountCheckBox(value: boolean) {
        if (this.attachedGLAccountCheckBox != value) {
            this.attachedGLAccountCheckBox = value;

        }
    }

    private splittedByCurrencyCheckBox: boolean = false;
    get SplittedByCurrencyCheckBox() { return this.splittedByCurrencyCheckBox; }
    set SplittedByCurrencyCheckBox(value: boolean) {
        if (this.splittedByCurrencyCheckBox != value) {
            this.splittedByCurrencyCheckBox = value;

        }
    }

    private _GLAccountId: string;
    get GLAccountId() { return this._GLAccountId; }
    set GLAccountId(value: string) {
        if (this._GLAccountId != value) {
            this._GLAccountId = value;

            this.UIProperties.SetRequired("GLAccountId", this.ObjectTableName, !value);

        }
    }


    private _IsReconciled: boolean;
    public get IsReconciled(): boolean {
        return this._IsReconciled;
    }
    public set IsReconciled(v: boolean) {
        this._IsReconciled = v;
    }


    private _IncludeChildAccounts: boolean;
    public get IncludeChildAccounts(): boolean {
        return this._IncludeChildAccounts;
    }
    public set IncludeChildAccounts(v: boolean) {
        this._IncludeChildAccounts = v;
    }


    private _SearchFields: string;
    public get SearchFields(): string {
        return this._SearchFields;
    }
    public set SearchFields(v: string) {
        this._SearchFields = v;
    }




    //#endregion


}
