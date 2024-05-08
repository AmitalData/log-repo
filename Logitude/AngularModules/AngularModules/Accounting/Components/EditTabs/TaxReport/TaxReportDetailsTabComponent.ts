import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TaxReportPM } from '../../../EntityPMs/TaxReportPM';
import { TaxReportLinePM } from '../../../EntityPMs/TaxReportLinePM';
import { RatesTableExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { TaxReportLineStatusListService } from '../../../Services/StandardLists/TaxReportLineStatusListService';
import { TaxReportLineExtendedListService } from '../../../Services/ExtendedLists/TaxReportLineExtendedListService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TaxReportExtendedPMService } from '../../../Services/ExtendedPMs/TaxReportExtendedPMService';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { TaxReportLineTransmitStatusListService } from 'Accounting/Services/StandardLists/TaxReportLineTransmitStatusListService';
import { HttpResponse } from '@angular/common/http';

declare var window: any;

@Component({

    templateUrl: './TaxReportDetailsTabComponent.html',
})

export class TaxReportDetailsTabComponent extends BaseComponent implements OnInit {

    public EntityPM: TaxReportPM = null;
    public ObjectTableName = "TaxReport";
    public DataContext = this;
    public isRTL: boolean = false;
    public showLocals: boolean = false;
    public LogitudeGridExportToExcelComponent: LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent();

    private _entityListService: EntityListService = new EntityListService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _TaxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
    private _TaxReportLineStatusListService: TaxReportLineStatusListService = new TaxReportLineStatusListService();
    private _TaxReportLineExtendedListService: TaxReportLineExtendedListService = new TaxReportLineExtendedListService();
    private taxReportLineTransmitStatusListService: TaxReportLineTransmitStatusListService = new TaxReportLineTransmitStatusListService();
    public TaxReportColumnsReady: EventEmitter<any> = new EventEmitter();
    public QueryColumns: QueryColumnPM[] = [];
    IsTesterButtonVisibile: boolean = false;
    ReportLines: ObservableCollection;
    OriginalReportLines: ObservableCollection;
    isReady: boolean = false;
    ShowErrorMsg: boolean = false;
    errorsCount: number = 0;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, public CD: ChangeDetectorRef) {
        super();

        var table = window.ObjectTables.filter(d => d.Name === 'TaxReport')[0];

        this.IsTesterButtonVisibile = FeatureLocator.Features.filter(f => (f.Code == "TaxReport.Features.TestButton") && f.ObjectTableId == table.Id)[0] ? true : false;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;

        this.EntityPM = entityArgs.EntityPM;

        this.SetUIProperty();

        this.Listen();
    }

    // Events
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    // Filters
    dateFilter: FilterItem;
    searchFieldFilter: FilterItem;

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.ReloadScreen();
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    }
    NewLineButtonClicked() {
        this.CurrentSession.StartBusyIndicator("Loading...");
        this._TaxReportExtendedPMService.CreateNewTaxReportLine(this.EntityPM).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            this.EntityPM = myResult.Result;

            this.ReloadScreen();

        });



    }
    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("TaxReport").subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("TaxReportLine").subscribe((response: any) => {
                this._entityResourceService.getEntityResourceByTableName("TaxReportLineTransmitStatus").subscribe((response: any) => {
                    this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => {
                        this._entityResourceService.getEntityResourceByTableName("JournalLine").subscribe((response: any) => {
                            this.isReady = true;


                            // this.GetStatuses();
                            // this.FillGrids();

                            // this.BuildColumns();
                            this.ReloadScreen();
                            // this.ReloadData();
                        });
                    });
                });
            });
        });

    }

    ReloadScreen() {
        
        this.BuildColumns();
        this.buildQueryColumns();
        this.GetStatuses();
        this.GetTransmitStatuses();
        
        this.CD.detectChanges();
        // this.FillGrids();
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        this.GetReportCounter();
        
    }

    public Export2ExcelClicked() {
        this.LogitudeGridExportToExcelComponent.ExportToExcelExcute('TaxReportLine', this.ListFilters, this.QueryColumns);
    }
    SetUIProperty() {
        this.UIProperties.SetEnabled("VatNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OutputTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxableOutputAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("LastUpdateDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ExemptTaxableOutput", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxReportMonth", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TaxableOutputAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EquipmentInputsTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("OtherInputsTaxAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("AmountForPayRefund", this.ObjectTableName, false);
    }

    //#region Properties
    //VatNumber
    //TaxableOutputAmount
    //LastUpdateDate
    //ExemptTaxableOutput
    //TaxReportMonth
    //TaxableOutputAmount
    //EquipmentInputsTaxAmount
    //OtherInputsTaxAmount
    //AmountForPayRefund

    get VatNumber() { return this.EntityPM.VatNumber; }
    set VatNumber(value: string) {
        if (this.EntityPM.VatNumber != value) {
            this.EntityPM.VatNumber = value;
        }
    }

    get OutputTaxAmount() { return this.EntityPM.OutputTaxAmount; }
    set OutputTaxAmount(value: number) {
        if (this.EntityPM.OutputTaxAmount != value) {
            this.EntityPM.OutputTaxAmount = value;
        }
    }

    get TaxableOutputAmount() { return this.EntityPM.TaxableOutputAmount; }
    set TaxableOutputAmount(value: number) {
        if (this.EntityPM.TaxableOutputAmount != value) {
            this.EntityPM.TaxableOutputAmount = value;
        }
    }

    get LastUpdateDate() { return this.EntityPM.LastUpdateDate; }
    set LastUpdateDate(value: Date) {
        if (this.EntityPM.LastUpdateDate != value) {
            this.EntityPM.LastUpdateDate = value;
        }
    }

    get ExemptTaxableOutput() { return this.EntityPM.ExemptTaxableOutput; }
    set ExemptTaxableOutput(value: number) {
        if (this.EntityPM.ExemptTaxableOutput != value) {
            this.EntityPM.ExemptTaxableOutput = value;
        }
    }

    get TaxReportMonth() { return this.EntityPM.TaxReportMonth; }
    set TaxReportMonth(value: Date) {
        if (this.EntityPM.TaxReportMonth != value) {
            this.EntityPM.TaxReportMonth = value;
        }
    }

    get EquipmentInputsTaxAmount() { return this.EntityPM.EquipmentInputsTaxAmount; }
    set EquipmentInputsTaxAmount(value: number) {
        if (this.EntityPM.EquipmentInputsTaxAmount != value) {
            this.EntityPM.EquipmentInputsTaxAmount = value;
        }
    }

    get OtherInputsTaxAmount() { return this.EntityPM.OtherInputsTaxAmount; }
    set OtherInputsTaxAmount(value: number) {
        if (this.EntityPM.OtherInputsTaxAmount != value) {
            this.EntityPM.OtherInputsTaxAmount = value;
        }
    }

    get AmountForPayRefund() { return this.EntityPM.AmountForPayRefund; }
    set AmountForPayRefund(value: number) {
        if (this.EntityPM.AmountForPayRefund != value) {
            this.EntityPM.AmountForPayRefund = value;
        }
    }


    //#endregion

    //#region Filtering Methods
    TaxableTransactionsCount: number = 0;
    ExemptTransactionsCount: number = 0;
    AllTransactionsCount: number = 0;
    InputsEquipmentsCount: number = 0;
    InputsOtherCount: number = 0;
    AllCount: number = 0;

    public FilterSelectedValue: string = 'All';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.FilterLines();
        }
    }
    ListFilters: ApiQueryFilters = new ApiQueryFilters();
    FilterLines() {

        // var filteredLines = [];
        // filteredLines = this.OriginalReportLines.Collection;
        var filters = new ApiQueryFilters;

        //search
        if (!AppTool.IsNullOrEmpty(this.searchText))
            filters.addAdditionalFilter("SearchFields", this.searchText, null, null, "Contains", false, false, false, "string");
        // filteredLines = filteredLines.filter(d => d.SearchFields.toLowerCase().includes(this.searchText.toLowerCase()));

        //update filters count
        //this.TaxableTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount > 0).length;
        //this.ExemptTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount == 0).length;
        //this.AllTransactionsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O").length;
        //this.InputsEquipmentsCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == true).length;
        //this.InputsOtherCount = filteredLines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == false).length;
        //this.AllCount = filteredLines.length;


        //toggle filters
        switch (this.FilterSelectedValue) {
            case "TaxableTransactions": {
                filters.addAdditionalFilter("OutputOrInput", "O", null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("VatAmount", 0, null, null, "NotEqual", false, false, false, "string");
                break;
            }
            case "ExemptTransactions": {
                filters.addAdditionalFilter("OutputOrInput", "O", null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("VatAmount", 0, null, null, "Equals", false, false, false, "string");
                break;
            }
            case "AllTransactions": {
                filters.addAdditionalFilter("OutputOrInput", "O", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "InputsEquipments": {
                filters.addAdditionalFilter("OutputOrInput", "I", null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("IsEquipment", true, null, null, "Equals", false, false, false, "string");
                break;
            }
            case "InputsOther": {
                filters.addAdditionalFilter("OutputOrInput", "I", null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("IsEquipment", false, null, null, "Equals", false, false, false, "string");
                break;
            }
            case "All": {
                break;
            }
        }


        //filter statuses
        if (this.SelectedStatusItems.length > 0) {
            
            var statusesListString = "";

            this.SelectedStatusItems.forEach(item => { statusesListString += item + ","; });
            statusesListString = statusesListString.slice(0, -1); // trim last comma
            filters.addAdditionalFilter("StatusCode", statusesListString, null, null, "InList", false, false, false, "string");
            //filteredLines = filteredLines.filter(d => this.SelectedStatusItems.includes(d.StatusCode));
        }


        if (this.SelectedTransmitStatusItems.length > 0)
            this.AddTransmitStatusFilter(filters);


        this.ListFilters = filters;
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });


    }

    private timerToken: any;
    searchText: string = "";

    AddTransmitStatusFilter(filters: ApiQueryFilters) {
        filters.addAdditionalFilter("TransmitStatusCode", this.SelectedTransmitStatusItems.join(','), null, null, "InList", false, false, false, "string");
    }

    TextChanged(searchtext) {

        this.timerToken = setTimeout(() => {
            this.searchText = searchtext;
            this.FilterLines();
        }, 500);


        if (!AppTool.IsNullOrEmpty(searchtext)) {

            this.timerToken = setTimeout(() => {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
            }, 700);

        } else {
            this.searchFieldFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }
    //#endregion

    //#region Data
    FillGrids() {
        // var lines = [];

        // this.OriginalReportLines = new ObservableCollection([]);

        // if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
        //     for (let item of this.ReportLines.Collection.sort((a, b) => { return (a.Line === b.Line) ? 0 : (a.Line < b.Line) ? -1 : 1 })) {
        //         lines.push(item));
        //     }
        // }

        // // this.ReportLines.InsertCollection(lines);
        // this.OriginalReportLines.InsertCollection(lines);

        //calculate sums
        //this.TaxableTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount > 0).length;
        //this.ExemptTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O" && d.TaxReportLinePM.VatAmount == 0).length;
        //this.AllTransactionsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "O").length;
        //this.InputsEquipmentsCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == true).length;
        //this.InputsOtherCount = lines.filter((d: ReportLineModel) => d.TaxReportLinePM.OutputOrInput == "I" && d.TaxReportLinePM.IsEquipment == false).length;
        //this.AllCount = lines.length;

        var lines = this.ReportLines.Collection;
        this.errorsCount = lines.filter((d) => d.TaxReportLinePM.StatusCode != "6" && d.TaxReportLinePM.TransmitStatusCode == "1").length;
        this.ShowErrorMsg = this.errorsCount > 0;

    }

    public TransmitStatuses = [];
    public SelectedTransmitStatusItems = [];
    StatusItems = [];
    SelectedStatusItems = [];
    CheckSelectedStatusItems(status){
        
        return this.SelectedStatusItems?.some(a => a == status.Code) ?? false
    }
    GetStatuses() {
        this._TaxReportLineStatusListService.getAll().subscribe((myResult: any) => {
            this.StatusItems = myResult.Result;
        });
    }
    PushStatus(status) {
        this.SelectedStatusItems.push(status.Code);
        this.FilterLines();
    }
    PopStatus(status) {
        var itemIndex = this.SelectedStatusItems.indexOf(status.Code);
        if (itemIndex > -1)
            this.SelectedStatusItems.splice(itemIndex, 1);
        this.FilterLines();
    }
    GetLinesWithErrorsCount() {
        this._TaxReportExtendedPMService.getErrorsCount(this.EntityPM.Id).subscribe((myResult: ServiceResponse) => {
            var __errorsCount = myResult.Result;
            this.ShowErrorMsg = __errorsCount >= 1;
            this.errorsCount = __errorsCount;
        });
    }

    GetTransmitStatuses() {
        this.taxReportLineTransmitStatusListService.getAll().subscribe((response: any) => {
            this.TransmitStatuses = response.Result;
        });
    }
    PushTransmitStatus(status) {
        
        this.SelectedTransmitStatusItems.push(status.Code);
        this.FilterLines();
    }
    PopTransmitStatus(status) {
        
        var itemIndex = this.SelectedTransmitStatusItems.indexOf(status.Code);
        if (itemIndex > -1)
            this.SelectedTransmitStatusItems.splice(itemIndex, 1);
        this.FilterLines();
    }
    //#endregion

    //#region Data
    public columns: any[] = null;

    ReloadData() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        this.GetReportCounter();
    }

    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: 'TransmitStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Accounting.O.Included"),
            Styles: { width: '70px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true

        });

        this.columns.push({
            FieldName: 'Line',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.Line"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            ServerSideSortable: true
        });


        this.columns.push({
            FieldName: 'LineTypeCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.LineTypeCode"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            ServerSideSortable: true
        });


        this.columns.push({
            FieldName: 'VatNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.VatNumber"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

        this.columns.push({
            FieldName: 'Reference',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.Reference"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true

        });

        this.columns.push({
            FieldName: 'ReferecneGroup',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.ReferecneGroup"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

        this.columns.push({
            FieldName: 'ReferenceDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.ReferenceDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });



        //ameerah
        this.columns.push({
            FieldName: 'SubTotalInLocalCurrency',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("ARInvoice.F.SubTotalInLocalCurrency"),
            Styles: { width: '160px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        // this
        this.columns.push({
            FieldName: 'TotalInvoiceAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.TotalInvoiceAmount"),
            Styles: { width: '160px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'VatAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.VatAmount"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

        this.columns.push({
            FieldName: SessionLocator.LoggedUserPM.DontShowLocal ? 'StatusEnglishName' : 'StatusLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.StatusEnglishName"),
            Styles: { width: '300px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.JournalNumber"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'ConfirmationNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.ConfirmationNumber"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'Buttons;' + this.EntityPM.StatusCode,
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            ServerSideSortable: true,
            IsCustomTemplate: true,
        });

        this.columns.push({
            FieldName: 'IsManuallyChanged',
            DataTypeCode: 'boolean',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true

        });
        this.columns.push({
            FieldName: 'IsExternalLine',
            DataTypeCode: 'String',
            //  Display: TextCodeTranslator.Translate("TaxReportLine.F.IsExternalLine"),
            Styles: { width: '40px' },
            HtmlListComponentName: 'TaxReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/TaxReportListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
        });
        this.TaxReportColumnsReady.emit(this.columns);
        //this.CustomColumnsReady.emit(this.columns);
    }
    buildQueryColumns() {
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("TransmitStatusCode", 'Text', TextCodeTranslator.Translate("Accounting.O.Included")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Line", 'Text', TextCodeTranslator.Translate("TaxReportLine.F.Line")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("LineTypeCode", 'Text', TextCodeTranslator.Translate("TaxReportLine.F.LineTypeCode")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("VatNumber", 'Text', TextCodeTranslator.Translate("TaxReportLine.F.VatNumber")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Reference", 'Text', TextCodeTranslator.Translate("TaxReportLine.F.Reference")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ReferecneGroup", 'Text', TextCodeTranslator.Translate("TaxReportLine.F.ReferecneGroup")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ReferenceDate", 'DateTime', TextCodeTranslator.Translate("TaxReportLine.F.ReferenceDate")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("TotalInvoiceAmount", 'Decimal', TextCodeTranslator.Translate("TaxReportLine.F.TotalInvoiceAmount")));
        //ameerah
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("SubTotalInLocalCurrency", 'Decimal', TextCodeTranslator.Translate("ARInvoice.F.SubTotalInLocalCurrency")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("VatAmount", 'Decimal', TextCodeTranslator.Translate("TaxReportLine.F.VatAmount")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.
            GetQueryColumn(SessionLocator.LoggedUserPM.DontShowLocal ? 'StatusEnglishName' : 'StatusLocalName', 'Text', TextCodeTranslator.Translate("TaxReportLine.F.StatusEnglishName")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("JournalNumber", 'Text', TextCodeTranslator.Translate("TaxReportLine.F.JournalNumber")));
    }
    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "Line",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

        //#region Filters
        if (this.ListFilters)
            var filters = this.ListFilters;
        else
            var filters = new ApiQueryFilters;

        //add report id
        filters.addAdditionalFilter('TaxReportId', this.EntityPM.Id, null, null, 'Equals', false, false, false, 'string');

        //if (this.dateFilter) {
        //     filters.AdditionalFilters.push(this.dateFilter);
        // } else {
        //     return;
        // }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetCount = true;

        if (sortingDir != "") {
            filters.SortBy = sortingCol;
            filters.SortDirection = sortingDir;
        }
        else {
            filters.SortBy = "Line";
            filters.SortDirection = "Ascending";
        }

        // filters.addAdditionalFilter("BankAccountId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");

        //#endregion

        return this._entityListService.getExtendedByFilters("TaxReportLine", filters);

    }
    reportCounters: TaxReportLinesCounter;
    GetReportCounter() {
        this._TaxReportExtendedPMService.GetReportLinesCounter(this.EntityPM.Id).subscribe((myResult: ServiceResponse) => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                var result = myResult.Result;
                this.reportCounters = result.Result;
                console.log("GetReportLinesCounter", mm);
            }
            else {
            }
        });

        this.GetLinesWithErrorsCount();

    }

    //#endregion

    RefreshButtonClicked() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        //this.ListFilters = new ApiQueryFilters();
        //this.FilterSelectedValue = 'All';
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        this.GetReportCounter();
    }

    DownloadPa() {
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("LineTypeCode", 'I', null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter('TaxReportId', this.EntityPM.Id, null, null, 'Equals', false, false, false, 'string');
        filters.GetAll = true;
        this._TaxReportLineExtendedListService.DownloadPaFile(filters).subscribe(response => {
            const blob = response.blob;
            const downloadUrl = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadUrl;
            link.download = response.filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }, error => {
            console.error('Error downloading the file:', error);
        });
    }

    EditLine(entity) {
        if (entity) {
            var windowTitle = TextCodeTranslator.Translate("Accounting.O.EditLine") + " " + entity.Line;

            var windowArgs: any = {};
            windowArgs.TaxReportPM = this.EntityPM;
            windowArgs.TaxReportLinePM = entity;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 450;
            logWindow.Height = 400;
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe((event: any) => {
                if (event == "ok")
                    this.ReloadScreen();
            });
            logWindow.Show('./Accounting/Components/EditTabs/TaxReport/EditTaxReportLine/EditTaxReportLineComponent');
        }




    }

    GetErrorMsg() {
        var msg = TextCodeTranslator.Translate("Accounting.O.TaxReportErrorMsg");
        return msg.replace("#Number", this.errorsCount.toString());
    }

}

export class TaxReportLinesCounter {
    TaxableTransactions: number;
    ExcemptTransactions: number;
    AllTransaxtions: number;
    InputEquipments: number;
    InputOthers: number;
    All: number;
}
