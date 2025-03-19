import {Component, OnInit, Output, EventEmitter,AfterViewInit,ChangeDetectorRef}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';


import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { ARInvoicePM } from 'Invoice/EntityPMs/ARInvoicePM';
import { ARInvoiceList } from 'Invoice/EntityLists/ARInvoiceList';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { ARInvoiceExtendedService } from 'Invoice/Services/ExtendedPMs/ARInvoiceExtendedService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';


@Component({
    templateUrl: './ARInvoiceSequenceListComponent.html',
    providers: []
})

export class ARInvoiceSequenceListComponent extends BaseComponent implements OnInit,AfterViewInit {
    @Output() onQueryChangeEvent = new EventEmitter();
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    public DataContext = this;
    public filterAgrs: ApiQueryFilters;

 
    // Services
    private _entityListService: EntityListService;
    public LogitudeGridExportToExcelComponent:LogitudeGridExportToExcelComponent;
    public aRInvoiceExtendedService : ARInvoiceExtendedService =  new ARInvoiceExtendedService();

    // Filters
    searchFieldFilter: FilterItem;

    public ItemsSource: ARInvoiceList[];

    public UsingLogGridV2:boolean= false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsExcelEnabled = true;
    public IsFutureOpenCheques= false;
    public IsUnpaidChecks= false;
    public TaxReportColumnsReady: EventEmitter<any> = new EventEmitter();

    constructor(private EntityResourceService: EntityResourceService, private CD: ChangeDetectorRef){
        super();
        debugger;
        this.EntityResourceService.getEntityResourceByTableName("ARInvoice").subscribe(response => {
            this.isLoaded = true;
            if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
            this.UsingLogGridV2 = false;//SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "LV2")[0]? true : false;
            this._entityListService = new EntityListService();
            this.LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent();
        });
       
    }
    
    SetWindowArgs() {

        this.LoadData();
        // const fromDate = new Date();
        // const toDate = new Date();
        // fromDate.setMonth(fromDate.getMonth() - 1);
        // const servicelink = "./Invoice/Services/ExtendedPMs/ARInvoiceExtendedService";
        // return new Promise((resolve, reject) => {
        //     SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
        //         resolve(service.getInvoiceSequenceStatus( fromDate, toDate));
        //     });
        // });
    }
    ngOnInit() {
        this.BuildColumns();
     }
 
 
     ngAfterViewInit() {
         this.CD.detectChanges();
     }

    

    LoadData() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters () });
    }

  


  
    //#region Data Source
    public columns: any[] = null;
    public QueryColumns: QueryColumnPM[] = [];


    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;

        }
    }

  private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
           

        }
  }
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'InvoiceSeries',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ARInvoice.F.InvoiceSeries"),
            Styles: { width: '85px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InvoiceSequenceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InvoiceSequenceListTemplate',
        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("InvoiceSeries",'Text',TextCodeTranslator.Translate("ARInvoice.F.InvoiceSeries")));

        this.columns.push({
            FieldName: 'InvoiceNumberPart',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ARInvoice.F.InvoiceNumberPart"), 
            Styles: { width: '110px' }, // TASK 47563
            IsCustomTemplate: true,
            HtmlListComponentName: 'InvoiceSequenceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InvoiceSequenceListTemplate',

        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("InvoiceNumberPart",'Text',TextCodeTranslator.Translate("ARInvoice.F.InvoiceNumberPart")));


        this.columns.push({
            FieldName: 'InvoiceDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate"), 
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InvoiceSequenceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InvoiceSequenceListTemplate',

        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("InvoiceDate", 'DateTime', TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate")));

        this.columns.push({
            FieldName: 'InvoiceNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ARInvoice.O.OriginalInvoiceNumber"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InvoiceSequenceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InvoiceSequenceListTemplate',

        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("InvoiceNumber", 'Text', TextCodeTranslator.Translate("ARInvoice.F.InvoiceNumber")));

        this.columns.push({
            FieldName: 'SequenceStatus',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ARInvoice.F.SequenceStatus"),
            Styles: { width: '85px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InvoiceSequenceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InvoiceSequenceListTemplate',

        });
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("SequenceStatus", 'Text', TextCodeTranslator.Translate("ARInvoice.F.SequenceStatus")));


    }

   
    DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingCol: "InvoiceNumber",
        sortingDir: "Descending",

        getRows: (skip: number, take: number, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, false, searchFields, filters); 
            debugger
            return tempo;
        },
    };

    @Output() MenuHeaderchangeevent = new EventEmitter();
 isLoaded: boolean = false;
    getRows(skip, take, getCount: boolean, searchfields?: string,filters:ApiQueryFilters=null) {
        debugger
        //this.CurrentSession.StartBusyIndicator("Loading...");
        const fromDate = new Date();
        const toDate = new Date();
        fromDate.setMonth(fromDate.getMonth() - 1);
        const servicelink = "./Invoice/Services/ExtendedPMs/ARInvoiceExtendedService";
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getInvoiceSequenceStatus( fromDate, toDate));
            });
        });
        // return this.aRInvoiceExtendedService.getInvoiceSequenceStatus(fromDate, toDate).subscribe((myResponse: ServiceResponse) => {
        //     this.CurrentSession.StopBusyIndicator();
        //     this.isLoaded = true;

        // });
}
    ExportToExcelClick()
    {
        this.LogitudeGridExportToExcelComponent.ExportToExcelExcute("ARInvoiceSequence",this.filterAgrs,this.QueryColumns,"SaveToMicrosoftExcel2007",true);
    }

    // private CreateApiQueryFilters(take: any, skip: any) {
    //     this.filterAgrs = new ApiQueryFilters();
    //     this.filterAgrs.PageSize = take;
    //     this.filterAgrs.PageIndex = skip;
    //     this.filterAgrs.GetAll = false;
    //     this.filterAgrs.GetCount = true;

    //     this.filterAgrs.addAdditionalFilter("InvoiceNumber", this.fromDate, this.toDate, null, "Between", false, false, false, "DateTime"); 


    // }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

   
}
