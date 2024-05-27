import {Component, OnInit, Output, EventEmitter, ChangeDetectorRef}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../Filters/ReportFliter';
import {QueryFilterItem} from '../../Filters/QueryFilterItem';
import {ReportsPreviewComponent} from '../../ReportsPreviewComponent';
import { AdvancedDatePickerResolverComponent } from 'Infrastructure/Components/LogitudeComponents/AdvancedDatePickerResolverComponent';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';

@Component({
    
    templateUrl: './CustomsCollateralFilterComponent.html',
})

export class CustomsCollateralFilterComponent extends BaseComponent {

    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Customs.CustomsCollateral";
    public DataContext: CustomsCollateralFilterComponent = this;
    TransportFilter_A: string;
    TransportFilter_O: string;
    TransportFilter_I: string;
    errors: any[];
    isReady: boolean = false;

    constructor(private EntityResourceService: EntityResourceService, private CD: ChangeDetectorRef) {
        super();
         
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.isReady = true;
            }); 
        });

        this.DataContext.UIProperties.SetRequired("FromDate", this.ObjectTableName, true)
        this.DataContext.UIProperties.SetRequired("ToDate", this.ObjectTableName, true)

        this.TransportFilter_A = "TransportFilter_A";
        this.TransportFilter_O = "TransportFilter_O";
        this.TransportFilter_I = "TransportFilter_I";
    }

    ngOnInit() {

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;        
    }

    public UserId: string; 
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

    private selectedTransportModeId: string = "All";
    public get SelectedTransportModeId() { return this.selectedTransportModeId; }
    public set SelectedTransportModeId(value: string) {
        if (this.selectedTransportModeId != value) {
            this.selectedTransportModeId = value;
        }
    }

    private importExport: string = "All";
    public get ImportExport() { return this.importExport; }
    public set ImportExport(value: string) {
        if (this.importExport != value) {
            this.importExport = value;
        }
    }

    private customerId: string;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(value: string) {
        if (this.customerId != value) {
            this.customerId = value;
        }
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


    itemClicked(itemValue: string) {
        this.SelectedTransportModeId = itemValue;
    }


    itemMouseOver(itemValue: string) {
        if (this.SelectedTransportModeId != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I.png");
                    break;
                }
            }
        }
    }

    itemMouseLeave(itemValue: string) {
        if (this.SelectedTransportModeId != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    }

    DirectionClicked(itemValue: string) {
        if (this.importExport != itemValue) {
            this.importExport = itemValue;
        }
    }


    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    RunReport() {
        this.ValidationErrorsList = [];


        var FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        
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
        if (this.ValidationErrorsList.length == 0) {

            this.BuildReport();

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

    InitilaizeFilter() {
       
        this.queryFilterItems = new Array<QueryFilterItem>();
        //-----------------------------------------------------------------------------1
        this.queryFilterItems.push(this.GetNewQueryFilterItem("CreateDate", this.FromDate, this.ToDate, "Date", "Between"));
        
        //-----------------------------------------------------------------------------2

        if(this.SelectedTransportModeId != 'All') {
            this.queryFilterItems.push(this.GetNewQueryFilterItem("TransportModeId", this.SelectedTransportModeId, null, "string"));
        }
        //-----------------------------------------------------------------------------3
        if(AppTool.IsNullOrEmpty(this.ImportExport != 'All')) {
            this.queryFilterItems.push(this.GetNewQueryFilterItem("ImportExport", this.ImportExport, null, "string"));
        }
        
        if(!AppTool.IsNullOrEmpty(this.CustomerId)) {
            this.queryFilterItems.push(this.GetNewQueryFilterItem("Customer", this.CustomerId, null, "string"));
        }
        
        
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

}
