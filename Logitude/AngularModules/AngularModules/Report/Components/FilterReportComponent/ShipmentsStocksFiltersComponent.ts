

declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {AppTool} from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    moduleId: module.id,
    selector: 'ShipmentsStocksFiltersComponent',
    templateUrl: './ShipmentsStocksFiltersComponent.html',
    inputs: ['ReportsPreview']
})

export class ShipmentsStocksFiltersComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;




    public ValidationErrorsList: string[] = [];
    reportFliter: ReportFliter;
    ToDate: Date;
    public HeightControl: string;
    FromDate: Date;

    public myForm: FormGroup;
    queryFilterItems: QueryFilterItem[];

    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public DataContext: ShipmentsStocksFiltersComponent = this;
    constructor(fb: FormBuilder) {
        super();
        this.myForm = fb.group({});

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
        this.RunReport(false);
    }

    ngOnInit() {

    }



    daysInMonth(aDate: Date) {

        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }



    private includeShipmentsDetails: boolean = false;
    public get IncludeShipmentsDetails() { return this.includeShipmentsDetails; }
    public set IncludeShipmentsDetails(value: boolean) {
        if (this.includeShipmentsDetails != value) {
            this.includeShipmentsDetails = value;
        }
    }

    RunReport(isloading: boolean) {





        this.ValidationErrorsList = [];

        if (this.ToDate == null) {
            var FIELD_IS_REQUIERD: string = null;
            FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            var fieldName = TextCodeTranslator.Translate("Accounting.General.O.ToDate");
            if (!fieldName) fieldName = "To Date";

            var s: string = FIELD_IS_REQUIERD.replace("%FieldName", fieldName);
            this.ValidationErrorsList.push(s);
        }

        if (this.FromDate == null) {
            var FIELD_IS_REQUIERD: string = null;
            FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            var fieldName = TextCodeTranslator.Translate("Accounting.General.O.FromDate");
            if (!fieldName) fieldName = "From Date";
            var s: string = FIELD_IS_REQUIERD.replace("%FieldName", fieldName);
            this.ValidationErrorsList.push(s);
        }

        if (this.ToDate < this.FromDate) {
            var messageError = TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate");
            if (!messageError) messageError = "to date must be greater than from date";
            this.ValidationErrorsList.push(messageError);
        }


        if (this.ValidationErrorsList.length == 0) {



            this.queryFilterItems = new Array<QueryFilterItem>();

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItems.push(new QueryFilterItem("IncludeShipmentsDetails", this.IncludeShipmentsDetails));



            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;


            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);

        }
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



}
