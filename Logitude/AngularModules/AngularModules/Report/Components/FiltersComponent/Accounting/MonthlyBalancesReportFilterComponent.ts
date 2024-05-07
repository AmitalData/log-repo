
import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../Filters/ReportFliter';
import { QueryFilterItem } from '../../Filters/QueryFilterItem';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AdvancedDatePickerResolverComponent } from '../../../../Infrastructure/Components/LogitudeComponents/AdvancedDatePickerResolverComponent';
import { reject } from 'q';
import { CodeNameClass } from 'Infrastructure/DataContracts/CodeNameClass';
import { TaxReportExtendedPMService } from 'Accounting/Services/ExtendedPMs/TaxReportExtendedPMService';
import { TaxReportPM } from 'Accounting/EntityPMs/TaxReportPM';
import { Operators } from 'Accounting/DataContracts/Operators';
import { ReportsPreviewComponent } from 'Report/Components/ReportsPreviewComponent';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { AppTool } from 'Infrastructure/Tools';

@Component({

    templateUrl: './MonthlyBalancesReportFilterComponent.html',
})

export class MonthlyBalancesReportFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ValidationErrorsList: string[] = [];

    public DataContext = this;
    isReady: boolean = false;
    public ObjectTableName: string = "GLAccount";
    public TenantPM: TenantPM = SessionLocator.TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public TaxReportLists: CodeNameClass[] = [];
    entityResourceService: EntityResourceService = new EntityResourceService();
    private taxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
    private TenatTaxReports: TaxReportPM[];
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    constructor(public entityListService: EntityListService, private CD: ChangeDetectorRef) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.entityResourceService.getEntityResourceByTableName("TaxDeductionReport").subscribe((response: any) => {
            this.entityResourceService.getEntityResourceByTableName("TaxReport").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => {
                    this.entityResourceService.getEntityResourceByTableName("ARInvoiceLine").subscribe((response: any) => {
                        this.entityResourceService.getEntityResourceByTableName("ARInvoice").subscribe((response: any) => {
                            this.entityResourceService.getEntityResourceByTableName("General").subscribe((response: any) => {
                        this.isReady = true;
                    });
                    });
                    });
                });
            });
        });
        this.DataContext.UIProperties.SetRequired("FromDate", this.ObjectTableName, true)
        this.DataContext.UIProperties.SetRequired("ToDate", this.ObjectTableName, true)

        this.GetTransmittedTaxReports();
    }
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var yesterdayDate = new Date().setDate(new Date().getDate() - 1);

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




    ValidateDate() {
        var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {

            setTimeout(() => {
                if (!this.IsOldDate("ToDate"))
                    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                this.errors = [];
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

    //-----------------------------------------------------------------------------1
    //#region DateFilter
    GetTransmittedTaxReports() {
        this.taxReportExtendedPMService.GetTenantTransmittedTaxReports().subscribe((response: any) => {
            this.TenatTaxReports = response.Result;
            this.CurrentSession.StopBusyIndicator();
            if (this.TenatTaxReports != null) {
                this.TenatTaxReports.forEach(p => {
                    this.TaxReportLists.push(new CodeNameClass(p.Id, this.FormatTaxReportDate(p.TaxReportMonth)));
                });

            }


        });
    }

    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.ValidateDate();
            this.DataContext.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
        }
    }

    private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.ValidateDate();
            this.DataContext.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);

        }
    }

    private notIncludedInAnyTaxReport: boolean = false;
    get NotIncludedInAnyTaxReport() { return this.notIncludedInAnyTaxReport; }
    set NotIncludedInAnyTaxReport(value: boolean) {
        if (this.notIncludedInAnyTaxReport != value) {
            this.notIncludedInAnyTaxReport = value;
            this.SelectedTaxReport = null;

        }
    }
    private selectedTaxReport: CodeNameClass;
    get SelectedTaxReport() { return this.selectedTaxReport; }
    set SelectedTaxReport(value: CodeNameClass) {
        if (this.selectedTaxReport != value) {
            this.selectedTaxReport = value;
        }
    }
    public _dateTypeCode: string = '1';
    private typeFilterDate: string = "Accountant";
    public get TypeFilterDate() { return this.typeFilterDate; }
    public set TypeFilterDate(value: string) {
        if (this.typeFilterDate != value) {
            this.typeFilterDate = value;
        }
    }
    FormatTaxReportDate(date: Date) {
        var newDate = new Date(date);
        var month: number = newDate.getMonth() + 1;
        var year: number = newDate.getFullYear();
        return month + "." + year;
    }
    TypeFilterDateClicked(itemValue: string) {
        if (this.TypeFilterDate != itemValue) {
            this.TypeFilterDate = itemValue;
            this.FilterLines()
        }
    }
    FilterLines() {

        switch (this.TypeFilterDate) {
            case 'Accountant':
                this._dateTypeCode = '1';
                break;
            case 'Creation':
                this._dateTypeCode = '2';
                break;
            case 'VATreport':
                this._dateTypeCode = '3';

                this.NotIncludedInAnyTaxReport = true;
                break;
            default:
                break;
        }


    }
    //----------------------------------------------------------------------------2
    //#endregion
    operatorsList =
        [{ Code: Operators.Equals, EnglishName: 'Equals', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Equals") },
        { Code: Operators.NotEqual, EnglishName: 'Not Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },
        { Code: Operators.LargerThan, EnglishName: 'Larger Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LargerThan") },
        { Code: Operators.LessThan, EnglishName: 'Less Than', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThan") },
        { Code: Operators.LessThanOrEqual, EnglishName: 'Less Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.LessThanOrEqual") },
        { Code: Operators.GreaterThanOrEqual, EnglishName: 'Greater Than Or Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.GreaterThanOrEqual") },
        { Code: Operators.Between, EnglishName: 'Between', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Between") },
        ];
    selectedAmountInvoiceOperator: { Code: string, EnglishName: string, LocalName: string };

    amountInvoiceFrom: number;
    get AmountInvoiceFrom() { return this.amountInvoiceFrom; }
    set AmountInvoiceFrom(value: number) {
        if (this.amountInvoiceFrom != value) {
            this.amountInvoiceFrom = value;
        }
    }
    amountInvoiceTo: number;
    get AmountInvoiceTo() { return this.amountInvoiceTo; }
    set AmountInvoiceTo(value: number) {
        if (this.amountInvoiceTo != value) {
            this.amountInvoiceTo = value;
        }
    }

    //---------------------------------------------------------------3

    private typeFilterReportsToVAT: string = "All";
    public get TypeFilterReportsToVAT() { return this.typeFilterReportsToVAT; }
    public set TypeFilterReportsToVAT(value: string) {
        if (this.typeFilterReportsToVAT != value) {
            this.typeFilterReportsToVAT = value;
        }
    }
    TypeFilterReportsToVATClicked(itemValue: string) {
        if (this.typeFilterReportsToVAT != itemValue) {
            this.typeFilterReportsToVAT = itemValue;
        }
    }
    //-----------------------------------------------------------------------------4
    private typeFilterIsExternal: string = "All";
    public get TypeFilterIsExternal() { return this.typeFilterIsExternal; }
    public set TypeFilterIsExternal(value: string) {
        if (this.typeFilterIsExternal != value) {
            this.typeFilterIsExternal = value;
        }
    }
    TypeFilterIsExternalClicked(itemValue: string) {
        if (this.typeFilterIsExternal != itemValue) {
            this.typeFilterIsExternal = itemValue;
        }
    }
    //-----------------------------------------------------------------------------5
    selectedAmountExamptOperator: { Code: string, EnglishName: string, LocalName: string };

    amountExamptFrom: number;
    get AmountExamptFrom() { return this.amountExamptFrom; }
    set AmountExamptFrom(value: number) {
        if (this.amountExamptFrom != value) {
            this.amountExamptFrom = value;
        }
    }
    amountExamptTo: number;
    get AmountExamptTo() { return this.amountExamptTo; }
    set AmountExamptTo(value: number) {
        if (this.amountExamptTo != value) {
            this.amountExamptTo = value;
        }
    }

    //-----------------------------------------------------------------------------6
    selectedAmountVatableOperator: { Code: string, EnglishName: string, LocalName: string };

    amountVatableFrom: number;
    get AmountVatableFrom() { return this.amountVatableFrom; }
    set AmountVatableFrom(value: number) {
        if (this.amountVatableFrom != value) {
            this.amountVatableFrom = value;
        }
    }
    amountVatableTo: number;
    get AmountVatableTo() { return this.amountVatableTo; }
    set AmountVatableTo(value: number) {
        if (this.amountVatableTo != value) {
            this.amountVatableTo = value;
        }
    }
    //-----------------------------------------------------------------------------7
    selectedAmountReportOperator: { Code: string, EnglishName: string, LocalName: string };

    amountReportFrom: number;
    get AmountReportFrom() { return this.amountReportFrom; }
    set AmountReportFrom(value: number) {
        if (this.amountReportFrom != value) {
            this.amountReportFrom = value;
        }
    }
    amountReportTo: number;
    get AmountReportTo() { return this.amountReportTo; }
    set AmountReportTo(value: number) {
        if (this.amountReportTo != value) {
            this.amountReportTo = value;
        }
    }

    //-----------------------------------------------------------------------------8
    operatorsList2 =
        [
            { Code: Operators.Equals, EnglishName: 'Equals', LocalName: TextCodeTranslator.Translate("Accounting.General.O.Equals") },
            { Code: Operators.NotEqual, EnglishName: 'Not Equal', LocalName: TextCodeTranslator.Translate("Accounting.General.O.NotEqual") },

        ];
    selectedCaseNumberOperator: { Code: string, EnglishName: string, LocalName: string };

    caseNumber: string;
    get CaseNumber() { return this.caseNumber; }
    set CaseNumber(value: string) {
        if (this.caseNumber != value) {
            this.caseNumber = value;
        }
    }


    //-----------------------------------------------------------------------------9



    private description: string;
    public get Description() { return this.description; }
    public set Description(value: string) {
        if (this.description != value) {
            this.description = value;
        }
    }

    RunReport() {
        this.ValidationErrorsList = [];


        var FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.TypeFilterDate == 'Accountant' || this.TypeFilterDate == 'Creation') {
            if (this.FromDate == null) {
                var FromDateValidation: string = FIELD_IS_REQUIERD.replace("%FieldName", "מתאריך");
                this.ValidationErrorsList.push(FromDateValidation);
            }

            if (this.ToDate == null) {
                var ToDateValidation: string = FIELD_IS_REQUIERD.replace("%FieldName", "עד תאריך");
                this.ValidationErrorsList.push(ToDateValidation);
            }

            if (this.FromDate != null && this.ToDate != null) {
                var FromDate = new Date(this.FromDate.getUTCFullYear(), this.FromDate.getUTCMonth(), this.FromDate.getUTCDate(), 0, 0, 0, 0);
                var ToDate = new Date(this.ToDate.getUTCFullYear(), this.ToDate.getUTCMonth(), this.ToDate.getUTCDate(), 0, 0, 0, 0);
                if (FromDate > ToDate) {
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
                }
            }
        }
        if (this.ValidationErrorsList.length == 0) {

            this.BuildReport();

        }
    }

    InitilaizeFilter() {
        this.queryFilterItems = new Array<QueryFilterItem>();
        //-----------------------------------------------------------------------------1

        switch (this.TypeFilterDate) {
            case 'Accountant':
                this.queryFilterItems.push(this.GetNewQueryFilterItem("InvoiceDate", this.FromDate, this.ToDate, "Date", "Between"));
                break;
            case 'Creation':
                this.queryFilterItems.push(this.GetNewQueryFilterItem("CreateDate", this.FromDate, this.ToDate, "Date", "Between"));
                break;
            case 'VATreport':
                if (this.SelectedTaxReport) {

                    this.queryFilterItems.push(this.GetNewQueryFilterItem("TaxReportId", this.SelectedTaxReport.Code, null, "string"));
                }
                else if (this.notIncludedInAnyTaxReport) {
                    this.queryFilterItems.push(this.GetNewQueryFilterItem("NotIncludedInAnyTaxReport", this.notIncludedInAnyTaxReport == null ? false : this.notIncludedInAnyTaxReport, null, "boolean"));
                }
                break;
            default:
                break;
        }
        //-----------------------------------------------------------------------------2

        this.BuildFilterByOperator("AmountInLocalCurrency", this.AmountInvoiceFrom, this.AmountInvoiceTo, "double", this.selectedAmountInvoiceOperator);
        //-----------------------------------------------------------------------------3

        switch (this.TypeFilterReportsToVAT) {
            case 'All':
                break;
            case 'Yes':
                this.queryFilterItems.push(this.GetNewQueryFilterItem("LineActionCode", "1", null, "string"));
                break
            case 'No':
                this.queryFilterItems.push(this.GetNewQueryFilterItem("LineActionCode", "2", "3", "string"));
                break;
        }
        //-----------------------------------------------------------------------------4
        switch (this.typeFilterIsExternal) {
            case 'All':
                break;
            case 'Externally':
                this.queryFilterItems.push(this.GetNewQueryFilterItem("IsExternalEntity", "1", null, "string"));
                break
            case 'Internal':
                this.queryFilterItems.push(this.GetNewQueryFilterItem("IsExternalEntity", "0", null, "string"));
                break;
        }
        //-----------------------------------------------------------------------------5 
        this.BuildFilterByOperator("TotalExamptFortaxReport", this.AmountExamptFrom, this.AmountExamptTo, "decimal", this.selectedAmountExamptOperator);

        //-----------------------------------------------------------------------------6
        this.BuildFilterByOperator("TotalVAT", this.AmountVatableFrom, this.AmountVatableTo, "decimal", this.selectedAmountVatableOperator);

        //-----------------------------------------------------------------------------7
        this.BuildFilterByOperator("TotalAmountForTaxReport", this.AmountReportFrom, this.AmountReportTo, "decimal", this.selectedAmountReportOperator);

        //-----------------------------------------------------------------------------8

        if (this.selectedCaseNumberOperator && !AppTool.IsNullOrEmpty(this.CaseNumber)) {
            this.queryFilterItems.push(this.GetNewQueryFilterItem("MainEntityReference", this.CaseNumber, null, "string", this.selectedCaseNumberOperator.Code));
        }

        //-----------------------------------------------------------------------------9
        if (!AppTool.IsNullOrEmpty(this.Description)) {
            this.queryFilterItems.push(this.GetNewQueryFilterItem("Description", this.Description, null));

        }

    }
    BuildFilterByOperator(FieldName, FieldValue, FieldValue2, Type, Operator: { Code: string, EnglishName: string, LocalName: string }) {
        if (!AppTool.IsNullOrEmpty(FieldValue) && !AppTool.IsNullOrEmpty(Operator)) {

            var FilterOperator = Operator.EnglishName.replace(/ /g, ''); // remove white spaces
            var num1 = FieldValue;
            var num2 = FieldValue2;
            this.queryFilterItems.push(this.GetNewQueryFilterItem(FieldName, num1, num2, Type, FilterOperator));


        }
    }
    BuildReport() {
        this.InitilaizeFilter();

        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";

        this.ReportsPreview.GenerateReport(this.reportFliter, true);
    }

    GetNewQueryFilterItem(FieldName: string, FieldValue: any, FieldValue2: any = null, FieldDataType: string = null, Operator: string = "Equals") {
        var queryFilterItem = new QueryFilterItem();
        queryFilterItem.DisplayInList = false;
        queryFilterItem.FieldName = FieldName;
        queryFilterItem.FieldValue = FieldValue;
        queryFilterItem.FieldValue2 = FieldValue2;
        queryFilterItem.Operator = Operator;
        queryFilterItem.FieldDataType = FieldDataType;

        return queryFilterItem;
    }
    ClearFields(){
        this.fromDate = null;
        this.toDate = null;
        this.notIncludedInAnyTaxReport = false;
        this.selectedTaxReport=null
        this._dateTypeCode= '1';
        this.typeFilterDate = "Accountant";

        this.selectedAmountInvoiceOperator = null
        this.amountInvoiceFrom = null;
        this.amountInvoiceTo = null;


        this.typeFilterReportsToVAT = "All";

        this.typeFilterIsExternal = "All";

        this.selectedAmountExamptOperator = null;
        this.amountExamptFrom  = null;
        this.amountExamptTo = null;

        this.selectedAmountVatableOperator = null
        this.amountVatableFrom = null;
        this.amountVatableTo = null;

        this.selectedAmountReportOperator = null
        this.amountReportFrom = null;
        this.amountReportTo = null;

        this.selectedCaseNumberOperator = null;
        this.caseNumber = "";
        this.description = "";

    }
}




