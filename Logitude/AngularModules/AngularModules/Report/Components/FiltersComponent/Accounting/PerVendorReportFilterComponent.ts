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
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { FullAccountingSettingPM } from '../../../../Accounting/EntityPMs/FullAccountingSettingPM';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { GLAccountListService } from '../../../../Accounting/Services/StandardLists/GLAccountListService';
import { ChartOfAccountListService } from '../../../../Accounting/Services/StandardLists/ChartOfAccountListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AdvancedDatePickerResolverComponent } from '../../../../Infrastructure/Components/LogitudeComponents/AdvancedDatePickerResolverComponent';
import { GLAccountPMService } from 'Accounting/Services/StandardPMs/GLAccountPMService';
import { reject } from 'q';
import { GLAccountPM } from 'Accounting/EntityPMs/GLAccountPM';

@Component({

    templateUrl: './PerVendorReportFilterComponent.html',
})

export class PerVendorReportFilterComponent extends BaseComponent {
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    isReady: boolean = false;

    public CardFilterItems: ApiQueryFilters;
    public ObjectTableName: string = "Card";
    public VendorLovSizeForFullAccounting: number = 550
    public TenantPM: TenantPM = SessionLocator.TenantPM;
    VendorGLAccount: GLAccountPM;
    private CurrentSession = SessionLocator.SelectedSession;

    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    public VendorDependencyFilter1Value: string = 'VD';
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    constructor(public entityListService: EntityListService, private CD: ChangeDetectorRef) {
        super();
        this.TenantPM = SessionLocator.TenantPM;

        this.InitComponent();

    }

    private InitComponent() {
        this.GetResources();
        this.InitLOVFilters();
        this.SetGLaccountFilterEnability();
        this.SetMonthFilterDefaults();
        //this.FillAgingMethodList();
    }

    private SetMonthFilterDefaults() {
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var day = new Date().getDate();


        this.FromDate = this.SetDate(Year, month - 1, day);
        this.ToDate = new Date();
    }
    SetDate(year: number, month: number, day: number) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    }
    InitLOVFilters() {
        // initialize query filters for Accounts
        this.CardFilterItems = new ApiQueryFilters();
        // this.CardFilterItems.addAdditionalFilter("PartnerTypeId", "VD", null, null, "Equals", false, false, true, "string");
    }

    private GetResources() {
        this.entityResourceService.getEntityResourceByTableName("TaxDeductionReport").subscribe((response: any) => { this.isReady = true; });
    }
    private vendor: CardList;
    public get Vendor() { return this.vendor; }
    public set Vendor(value: CardList) {
        if (this.vendor != value) {
            this.vendor = value;
            this.getVendorGlAccount();
        }
    }

    getVendorGlAccount(){
        if(this.vendor && this.vendor.GLAccountId) {
            this.GetGLAccount(this.vendor.GLAccountId);
        }
    }

    ValidateDate() {
        var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {

            setTimeout(() => {
                if (!this.IsOldDate("ToDate"))
                    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                if (!this.IsOldDate("FromDate"))
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

    private IsOldDate(fieldName) {
        let isOldDate: boolean = false;
        const uiProperty = this.UIProperties.UIPropertyList.filter(uiProp => uiProp.FieldName == fieldName)[0];
        if (uiProperty)
            isOldDate = uiProperty.ValidationError == "Date time is too way in the past!" || uiProperty.ValidationError == "Invalid Date";

        return isOldDate;
    }

    //#endregion
    private errors: string[] = [];
    RunButtonClicked() {

        this.errors = [];
        this.ValidationErrorsList = [];
        if (this.VendorFilterSelectedValue == "Vendor" && !this.vendor) {
            this.errors.push("No vendor has been selected");
            this.errors.push(TextCodeTranslator.Translate("GLTransactionReport.O.RequiredFields"));
        }

        if (this.VendorFilterSelectedValue == "Vendor" && this.vendor && this.vendor.GLAccountId == null) {
            this.errors.push("The selected vendor doesn't have GlAccount");
        }

        if(this.VendorFilterSelectedValue == "Vendor" && this.VendorGLAccount && this.VendorGLAccount.ExcludeFromDeductionReport) {
            this.errors.push("The selected vendor is excluded from deduction report");
        }
        if (this.errors.length == 0) {


            var myFilterItems: QueryFilterItem[] = [];
            myFilterItems.push(new QueryFilterItem("FromDate", this.FromDate, "Date"));
            myFilterItems.push(new QueryFilterItem("ToDate", this.ToDate, "Date"));

            if (this.VendorFilterSelectedValue != "All") {
                myFilterItems.push(new QueryFilterItem("Vendor", this.Vendor.GLAccountId, "string"));
                myFilterItems.push(new QueryFilterItem("CardId", this.VendorGLAccount.CardId, "string"));
            }

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;

            this.RunReportEvent.emit(myReportFliter);

        } else {
            this.ValidationErrorsList = this.errors;
        }
    }


    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.ValidateDate();
        }
    }

    private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.ValidateDate();

        }
    }

    
    private vendorId: Date;
    public get VendorId() { return this.vendorId; }
    public set VendorId(value: Date) {
        if (this.vendorId != value) {
            this.vendorId = value;
        }
    }

    private vendorFilterSelectedValue: string = "All";
    public get VendorFilterSelectedValue() { return this.vendorFilterSelectedValue; }
    public set VendorFilterSelectedValue(value: string) {
        if (this.vendorFilterSelectedValue != value) {
            this.vendorFilterSelectedValue = value;
            this.SetGLaccountFilterEnability();
        }
    }
    SetGLaccountFilterEnability() {
        if (this.VendorFilterSelectedValue == "All") {
            this.Vendor = null;
            this.VendorId = null;
            this.VendorGLAccount = null;
            this.UIProperties.SetEnabled("VendorId", "Card", false);
        }
        else {
            this.UIProperties.SetEnabled("VendorId", "Card", true);
        }
    }
    VendorFilterItemClicked(myCode: string) {
        this.VendorFilterSelectedValue = myCode;
    }

    FilterChanged() {

        //this.Customer = null;
        //this.Salesman = null;
        //this.Collector = null;
        //this.ChartOfAccountsId_Dummy = null;
        //this.UIProperties.SetValidity("ChartOfAccountsId", "GLAccount", true, "");
        //this.UIProperties.SetRequired("ChartOfAccountsId", "GLAccount", false);


        //switch (this.filterSelectedValue) {
        //    case 'filter_customer':
        //        this.AccountTypeCode = '2';
        //        this.ChartOfAccountsTypeCode = '3';
        //        break;
        //    case 'filter_vendor':
        //        this.AccountTypeCode = '3';
        //        this.ChartOfAccountsTypeCode = '4';
        //        break;
        //    default:
        //        break;


        //this.SetUIProperties();
        //this.ValidateDate();

    }

    GetGLAccount(id: string)
    {
        return new Promise(resolve =>
        {

            var service = new GLAccountPMService();
            service.get(id).subscribe((response:any) =>
            {
                var result: ServiceResponse = response;
                if (!result.HasError) {
                    this.VendorGLAccount = result.Result;
                    resolve(this.VendorGLAccount);
                }
                else {
                    console.error(result.ErrorsArray);
                    reject();
                }
            });


        });
    }

}




