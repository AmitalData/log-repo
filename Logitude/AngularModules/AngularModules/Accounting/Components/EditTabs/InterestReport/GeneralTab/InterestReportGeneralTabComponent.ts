import { Component, OnInit} from '@angular/core';
import { InterestReportPM } from '../../../../EntityPMs/InterestReportPM';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../../Infrastructure/Locators/ObjectsLocator';
import { InterestReportLinesByDatePM } from '../../../../EntityPMs/InterestReportLinesByDatePM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';

@Component({
    
    templateUrl: './InterestReportGeneralTabComponent.html',
})

export class InterestReportGeneralTabComponent extends BaseComponent implements OnInit{
    public EntityPM: InterestReportPM;
    public ObjectTableName: string = "InterestReport";
    public InterestReportLinesByDateList: ObservableCollection;
    public DataContext: InterestReportGeneralTabComponent = this;
    private _entityListService: EntityListService;
    private CurrentSession = SessionLocator.SelectedSession;
    public  NoDataTextCode:string=null;
    public filterAgrs: ApiQueryFilters;
    public IsNew: boolean = false;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
    public LogitudeGridExportToExcelComponent:LogitudeGridExportToExcelComponent;
    public GLAccountsFilterItems: ApiQueryFilters;

     constructor(public entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent(); 
        this.EntityPM = entityArgs.EntityPM;
         this.FillNoDataTextCodeValue();
         this.InterestReportLinesByDateList = new ObservableCollection([]);
         this._entityListService = new EntityListService();
         this.Listen();
         this.SetUIProperties()
         ;
         this.InitLOVFilters();
    }
    ngOnInit() {
        this.BuildColumns();
    }
    InitLOVFilters() {
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("IsMultiCurrency", false, null, null, "Equals", false, false, false, "string");
    }
    private FillNoDataTextCodeValue(){
        switch(this.EntityPM.InterestReportStatusCode){
            case "5":{
                   this.NoDataTextCode=  "InterestReport.O.ReportinProgress";
                break;
            }
            case "6":{
                this.NoDataTextCode=  "InterestReport.O.ReportCreationFailed";
                break;
            }
            default :{
                this.NoDataTextCode=  null;
                break;
            }
        }
    }
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }
    SetUIProperties() {
        this.UIProperties.SetEnabled("CustomerId", "InterestReport", false);
        this.UIProperties.SetEnabled("OpenBalance", "InterestReport", false);
        this.UIProperties.SetEnabled("InterestCalculationDate", "InterestReport", false);
        this.UIProperties.SetEnabled("GLAccountInterestCreditLimit", "InterestReport", false);
        this.UIProperties.SetEnabled("CreditAllotmentPercentage", "InterestReport", false);
    }
     public DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };



    LogWindowShow() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("InterestReport.O.EditOpenBalance");
        var myPath = "./Accounting/Components/Packages/EditTabs/InterestReport/GeneralTab/InterestReportEditOpenBalanceComponent/InterestReportEditOpenBalanceComponent";
        logWindow.Width = 350;
        logWindow.Height = 160;
        logWindow.DataContext = this.EntityPM.OpenBalance ;
        logWindow.Show(myPath);
        logWindow.WindowClosed.subscribe(s => {
            if (s!=null) {
               this.EntityPM.OpenBalance = s;
            }
        })
    }

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
       this.filterAgrs = new ApiQueryFilters();
       this.filterAgrs.PageSize = take;
       this.filterAgrs.PageIndex = skip;
       this.filterAgrs.GetAll = false;
       this.filterAgrs.GetCount = true;
       this.filterAgrs.addAdditionalFilter("InterestReportId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        return this._entityListService.getByFilters("InterestReportLinesByDate", this.filterAgrs); 
    }

    EditCalculationDate(){
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("InterestReport.O.EditCalculationDate");
        var myPath = "./Accounting/Components/Packages/EditTabs/InterestReport/GeneralTab/InterestReportEditCalculationDate/InterestReportEditCalculationDateComponent";
        logWindow.Width = 350;
        logWindow.Height = 160;
        logWindow.DataContext = this.EntityPM.InterestCalculationDate ;
        logWindow.Show(myPath);
        logWindow.WindowClosed.subscribe(s => {
            if (s!=null) {
              this.InterestCalculationDate = s;
              //this.CurrentSession.CurrentEditComponent.SaveChanges();fffff
            }
        })
    }

    get CustomerId() {
        if (this.EntityPM != null) {
            return this.EntityPM.CustomerId;
        }
        else
            return null;
    }
    set CustomerId(newValue: string) {
        if (this.EntityPM.CustomerId != newValue) {
            this.EntityPM.CustomerId = newValue;
        }
    }
    get IsFirstReport() {
        if (this.EntityPM != null) {
            return this.EntityPM.IsFirstReport;
        }
        else
            return null;
    }
    set IsFirstReport(newValue: boolean) {
        if (this.EntityPM.IsFirstReport != newValue) {
            this.EntityPM.IsFirstReport = newValue;
        }
    }
    get OpenBalance() {
        if (this.EntityPM != null) {
            return this.EntityPM.OpenBalance;
        }
        else
            return null;
    }
    set OpenBalance(newValue: number) {
        if (this.EntityPM.OpenBalance != newValue) {
            this.EntityPM.OpenBalance = newValue;
        }
    }
    get InterestCalculationDate() {
        if (this.EntityPM != null) {
            return this.EntityPM.InterestCalculationDate;
        }
        else
            return null;
    }
    set InterestCalculationDate(newValue: Date) {
        if (this.EntityPM.InterestCalculationDate != newValue) {
            this.EntityPM.InterestCalculationDate = newValue;
        }
    }
    get GLAccountInterestCreditLimit() {
        if (this.EntityPM != null) {
            return this.EntityPM.GLAccountInterestCreditLimit;
        }
        else
            return null;
    }
    set GLAccountInterestCreditLimit(newValue: number) {
        if (this.EntityPM.GLAccountInterestCreditLimit != newValue) {
            this.EntityPM.GLAccountInterestCreditLimit = newValue;
        }
    }

    get IsDraftReport() {
        if (this.EntityPM != null && this.EntityPM.InterestReportStatusCode == '1') {
            return true;
        }
        else
            return false;
    }
    get CreditAllotmentPercentage() {
        if (this.EntityPM != null) {
            return this.EntityPM.CreditAllotmentPercentage;
        }
        else
            return null;
    }
    
    public columns: any[] = null;
    public QueryColumns: QueryColumnPM[] = [];
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'LineNumber',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.LineNumber"),//'LineNumber',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("LineNumber",'Number',TextCodeTranslator.Translate("InterestReportLinesByDate.F.LineNumber")));
        this.columns.push({
            FieldName: 'FromDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.FromDate"),//'FromDate',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("FromDate",'DateTime',TextCodeTranslator.Translate("InterestReportLinesByDate.F.FromDate")));

        this.columns.push({
            FieldName: 'ToDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.ToDate"), //'ToDate',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ToDate",'DateTime',TextCodeTranslator.Translate("InterestReportLinesByDate.F.ToDate")));

        this.columns.push({
            FieldName: 'TotalInterestDays',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalInterestDays"), // 'TotalInterestDays',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("TotalInterestDays",'Number',TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalInterestDays")));

        this.columns.push({
            FieldName: 'TotalAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalAmount"), // 'TotalAmount',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("TotalAmount",'Number',TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalAmount")));

        this.columns.push({
            FieldName: 'AccumulatedAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.AccumulatedAmount"), // 'AccumulatedAmount',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("AccumulatedAmount",'Number',TextCodeTranslator.Translate("InterestReportLinesByDate.F.AccumulatedAmount")));

        this.columns.push({
            FieldName: 'TotalInterest',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalInterest"), // 'AccumulatedAmount',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("TotalInterest",'Number',TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalInterest")));

        this.columns.push({
            FieldName: 'ShowDetails',
            DataTypeCode: 'Number', 
            Display: ' ',  
            Styles: { width: '110px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'IsOpenBalanceLine',
            DataTypeCode: '', 
            Display: ' ',  
            Styles: { width: '30px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("IsOpenBalanceLine",'Boolean',TextCodeTranslator.Translate("InterestReportLinesByDate.F.IsOpenBalanceLine")));

    }

    public ExportToExcelClick(){
        this.LogitudeGridExportToExcelComponent.ExportToExcelExcute("InterestReportLinesByDate",this.filterAgrs,this.QueryColumns,"SaveToMicrosoftExcel2007",true);
    }
    
}
