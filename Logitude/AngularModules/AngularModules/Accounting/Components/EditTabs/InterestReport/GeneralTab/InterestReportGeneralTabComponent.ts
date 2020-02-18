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

@Component({
    moduleId: module.id,
    templateUrl: './InterestReportGeneralTabComponent.html',
})

export class InterestReportGeneralTabComponent extends BaseComponent implements OnInit{
    public EntityPM: InterestReportPM;
    public ObjectTableName: string = "InterestReport";
    public InterestReportLinesByDateList: ObservableCollection;
    public DataContext: InterestReportGeneralTabComponent = this;
    private _entityListService: EntityListService;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsNew: boolean = false;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
     constructor(public entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
         this.EntityPM = entityArgs.EntityPM;
         this.InterestReportLinesByDateList = new ObservableCollection([]);
         this._entityListService = new EntityListService();
         this.Listen();
         this.SetUIProperties();
    }
    ngOnInit() {
        this.BuildColumns();
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
    }
    LogWindowShow(itemComponent: InterestReportLinesByDatePM) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.Details");
        var myPath = "./Accounting/Components/Packages/EditTabs/InterestReport/GeneralTab/InterestReportLineByDateDetails/InterestReportLineByDateDetailsComponent";
        logWindow.Width = 850;
        logWindow.Height = 350;
        itemComponent.InterestReportId = this.EntityPM.Id;
        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
    }
     public DataSource = {
        pageSize: 50,
        rowCount: null,
        //sortingCol: "CreateDateTime",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        var filters = new ApiQueryFilters();
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.addAdditionalFilter("InterestReportId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        return this._entityListService.getByFilters("InterestReportLinesByDate", filters);//this.ledgerTransactionListExtendedService.getByFilters(filters);
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
    public columns: any[] = null;
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
        this.columns.push({
            FieldName: 'FromDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.FromDate"),//'FromDate',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ToDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.ToDate"), //'ToDate',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TotalInterestDays',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalInterestDays"), // 'TotalInterestDays',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TotalAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.TotalAmount"), // 'TotalAmount',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'AccumulatedAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.AccumulatedAmount"), // 'AccumulatedAmount',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StandardInterestPercentage',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.StandardInterestPercentage"), // 'StandardInterestPercentage',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ExceptionalInterestPercentage',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.ExceptionalInterestPercentage"), // 'ExceptionalInterestPercentage',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CreditInterestPercentage',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("InterestReportLinesByDate.F.CreditInterestPercentage"), // 'CreditInterestPercentage',
            Styles: { width: '100px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ShowDetails',
            DataTypeCode: 'Number',
            Display: ' ',  
            Styles: { width: '80px' },
            HtmlListComponentName: 'InterestReportLinesByDateListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportLinesByDateListTemplate',
            IsCustomTemplate: true
        });
    }
}
