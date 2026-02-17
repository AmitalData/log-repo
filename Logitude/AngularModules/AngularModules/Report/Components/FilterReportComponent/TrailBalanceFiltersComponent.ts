

declare var System: any;
declare var window: any;
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow'
import { ReportFliter } from '../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../Components/Filters/QueryFilterItem';
import { Component, OnInit, Output, ElementRef } from '@angular/core';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { AppTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';

@Component({
  moduleId: module.id,
  selector: 'TrailBalanceFiltersComponent',
  templateUrl: './TrailBalanceFiltersComponent.html',
  inputs: ['ReportsPreview']
})




export class TrailBalanceFiltersComponent extends BaseComponent
{
  public DataContext: any = this;
  public ObjectTableName: string = "GLAccount";
    public ReportsPreview: ReportsPreviewComponent;     
    GLAccountHtmlinputId: string;
    ChartofaccountHtmlinputId: string;
    chartofaccounttypeHtmlinputId: string;
    reportFliter: ReportFliter;
    queryFilterItems: QueryFilterItem[];
  public ValidationErrorsList: string[] = [];
  IsDisplayOnly: boolean = false;
 // IsCategoryDisabled: boolean = false;
  queryFilterItem: QueryFilterItem;
  public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
  Name: string;
  //  Level: string = "GLAccount";
  CustomerDetailedControlList: CodeNameClass[];
  VendorDetailedControlList: CodeNameClass[];

    constructor() {
        super();
        this.chartofaccounttypeHtmlinputId = Guid.newGuid();
        this.GLAccountHtmlinputId = Guid.newGuid();
      this.ChartofaccountHtmlinputId = Guid.newGuid();
      if (this.showLocal) {
        this.Name = "LocalName";


      }
      else {

        this.Name = "Name";
      }




      this.VendorDetailedControlList = [];

      this.VendorDetailedControlList.push(new CodeNameClass("1", "Show", "הצג פירוט"));
      this.VendorDetailedControlList.push(new CodeNameClass("2", "Dont show", "ללא פירוט"));
      this.VendorDetailedControlFilter = this.VendorDetailedControlList[1];
        this.CustomerDetailedControlList = [];

        this.CustomerDetailedControlList.push(new CodeNameClass("1", "Show", "הצג פירוט"));
      this.CustomerDetailedControlList.push(new CodeNameClass("2", "Dont show", "ללא פירוט"));
      this.CustomerDetailedControlFilter = this.CustomerDetailedControlList[1];
    }

 
  private vendorDetailedControlFilter: CodeNameClass;
  get VendorDetailedControlFilter() { return this.vendorDetailedControlFilter; }
  set VendorDetailedControlFilter(value: CodeNameClass) {
    if (this.vendorDetailedControlFilter != value) {
      this.vendorDetailedControlFilter = value;
    }
  }
  private customerDetailedControlFilter: CodeNameClass;
  get CustomerDetailedControlFilter() { return this.customerDetailedControlFilter; }
  set CustomerDetailedControlFilter(value: CodeNameClass) {
    if (this.customerDetailedControlFilter != value) {
      this.customerDetailedControlFilter = value;
    }
  }

  InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
    this.ReportsPreview = myReportsPreview;



   
    //this.BuildFilterList();
  }
  private fromDate: Date;
  public get FromDate() { return this.fromDate; }
  public set FromDate(value: Date) {
    if (this.fromDate != value) {
      this.fromDate = value;
    }
  }

  private level: string = "ChartOfAccountType";
  public get Level() { return this.level; }
  public set Level(value: string) {
    if (this.level != value) {
      this.level = value;
      if (this.level == "ChartOfAccount" || this.level == "ChartOfAccountType")
      {
        this.UIProperties.SetEnabled("ChartOfAccountId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Category1", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Category2", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Category3", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Category4", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Category5", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Category4", this.ObjectTableName, false);
        this.IsDisplayOnly = true;
        this.IsCategoryDisabled = true;



      }

   else{
          this.UIProperties.SetEnabled("ChartOfAccountId", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Category1", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Category2", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Category3", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Category4", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Category5", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("Category4", this.ObjectTableName, true);
        this.IsDisplayOnly = false;
        this.IsCategoryDisabled = false;


       }

    
    }
}

private toDate: Date;
  public get ToDate() { return this.toDate; }
  public set ToDate(value: Date) {
    if (this.toDate != value) {
      this.toDate = value;
    }
  }

  private useBalanceFilter: boolean;
  get UseBalanceFilter() { return this.useBalanceFilter; }
  set UseBalanceFilter(value: boolean) {
    if (this.useBalanceFilter != value) {
      this.useBalanceFilter = value;
    }
  }

  private useCustomerFilter: boolean;
  get UseCustomerFilter() { return this.useCustomerFilter; }
  set UseCustomerFilter(value: boolean) {
    if (this.useCustomerFilter != value) {
      this.useCustomerFilter = value;
    }
  }

  private useVendorFilter: boolean;
  get UseVendorFilter() { return this.useVendorFilter; }
  set UseVendorFilter(value: boolean) {
    if (this.useVendorFilter != value) {
      this.useVendorFilter = value;
    }
  }

  private useZeroBalanceFilter: boolean;
  get UseZeroBalanceFilter() { return this.useZeroBalanceFilter; }
  set UseZeroBalanceFilter(value: boolean) {
    if (this.useZeroBalanceFilter != value) {
      this.useZeroBalanceFilter = value;
    }
  }

  private currencyFilter: boolean;
  get CurrencyFilter() { return this.currencyFilter; }
  set CurrencyFilter(value: boolean) {
    if (this.currencyFilter != value) {
      this.currencyFilter = value;
    }
  }

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
  
  private chartOfAccountId: string;
  public get ChartOfAccountId() { return this.chartOfAccountId; }
  public set ChartOfAccountId(value: string) {
    if (this.chartOfAccountId != value) {
        this.chartOfAccountId = value;
      if (value != null) {
        this.IsCategoryDisabled = true;
      }
      else {
        this.IsCategoryDisabled = false;
      }
    }
  }

  

    
  
  public FilterSelectedValue: string = 'ChartOfAccountType';
  FilterItemClicked(itemValue: string) {
    if (this.FilterSelectedValue != itemValue) {
      this.FilterSelectedValue = itemValue;
      this.Level = itemValue;

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
    //#endregion    RunReport() {
        this.ValidationErrorsList = [];
       
        if (this.ToDate == null) {
            var FIELD_IS_REQUIERD: string = null;
            FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");


          var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.General.O.ToDate"));
            this.ValidationErrorsList.push(s);
        }

      if (this.FromDate == null) {
        var FIELD_IS_REQUIERD: string = null;
        FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");


        var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Accounting.O.FromDate"));
        this.ValidationErrorsList.push(s);
      }
      if (this.ToDate < this.FromDate) {


        this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
      }


        if (this.ValidationErrorsList.length == 0) {

            this.queryFilterItems = new Array<QueryFilterItem>();


            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
          if (!this.Level) this.Level = "ChartOfAccountType";
            this.queryFilterItems.push(new QueryFilterItem("Level", this.Level));
            //if (!this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITHOUT") {
            //    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "0"));
            //    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "0"));
            //}

            //else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITHOUT") {
            //    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "2"));
            //}
            //else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITH") {
            //    this.queryFilterItems.push(new QueryFilterItem("CardFilter", "1"));
            //}
               this.queryFilterItems.push(new QueryFilterItem("FromDate", this.FromDate, "Date"));
          this.queryFilterItems.push(new QueryFilterItem("Category1", this.Category1, "String"));
          this.queryFilterItems.push(new QueryFilterItem("Category2", this.Category2, "String"));
          this.queryFilterItems.push(new QueryFilterItem("Category3", this.Category3, "String"));
          this.queryFilterItems.push(new QueryFilterItem("Category4", this.Category4, "String"));
          this.queryFilterItems.push(new QueryFilterItem("Category5", this.Category5, "String"));
          this.queryFilterItems.push(new QueryFilterItem("CurrencyDetailed", this.CurrencyFilter, "boolean"));
          if (this.CustomerDetailedControlFilter != null && this.CustomerDetailedControlFilter.Code == "1") {

            this.queryFilterItems.push(new QueryFilterItem("Customer", true, "boolean"));

          }
          if (this.VendorDetailedControlFilter != null && this.VendorDetailedControlFilter.Code == "1") {

            this.queryFilterItems.push(new QueryFilterItem("Vendor", true, "boolean"));

          }
          this.queryFilterItems.push(new QueryFilterItem("UseBalanceFilter", this.UseBalanceFilter, "boolean"));
          this.queryFilterItems.push(new QueryFilterItem("ChartOfAccountId", this.ChartOfAccountId, "String"));





            this.reportFliter = new ReportFliter();
            //this.reportFliter.Level = this.Level;
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;

            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.CleanPartnersObslist();




            this.ReportsPreview.GenerateReport(this.reportFliter, true);


        }

    }

}
