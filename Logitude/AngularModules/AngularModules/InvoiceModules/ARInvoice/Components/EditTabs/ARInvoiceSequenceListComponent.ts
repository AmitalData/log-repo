import {Component, OnInit, Output, EventEmitter,AfterViewInit,ChangeDetectorRef, ComponentRef}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';


import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { ARInvoicePM } from 'Invoice/EntityPMs/ARInvoicePM';
import { ARInvoiceList } from 'Invoice/EntityLists/ARInvoiceList';
import { ARInvoiceExtendedService } from 'Invoice/Services/ExtendedPMs/ARInvoiceExtendedService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';


@Component({
    templateUrl: './ARInvoiceSequenceListComponent.html',
    providers: []
})

export class ARInvoiceSequenceListComponent extends BaseComponent implements OnInit,AfterViewInit {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    public DataContext = this;

 
    // Services
    public LogitudeGridExportToExcelComponent:LogitudeGridExportToExcelComponent;
    public aRInvoiceExtendedService : ARInvoiceExtendedService =  new ARInvoiceExtendedService();
    public ComponentRef: ComponentRef<ARInvoiceSequenceListComponent>;
    public ValidationErrorsList: string[] = [];


    public ItemsSource: ARInvoiceList[];


    constructor(private EntityResourceService: EntityResourceService, private CD: ChangeDetectorRef){
        super();
        this.EntityResourceService.getEntityResourceByTableName("ARInvoice").subscribe(response => {
            this.LogitudeGridExportToExcelComponent = new LogitudeGridExportToExcelComponent();
        });
       
    }
    
   
    ngOnInit() {
        var lastmonth = new Date();
        lastmonth.setMonth(lastmonth.getMonth() - 1);

        if (lastmonth.getFullYear() < new Date().getFullYear()) {
            const currentYear = new Date().getFullYear();

            this.FromDate = new Date(Date.UTC(currentYear, 0, 1)); // Set to the first day of the year
        } else {
            this.FromDate = lastmonth;
        }


        this.ToDate = new Date();
        this.BuildColumns();

     }


     BackButtonClicked() {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
        }


    }
 
 
     ngAfterViewInit() {
         this.CD.detectChanges();
     }

     RefreshBtnClick() {

        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({});
        }, 10);
    }

  
    //#region Data Source
    public columns: any[] = null;
    public QueryColumns: QueryColumnPM[] = [];


    private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate !== value) {
            this.fromDate = value;
        if(this.toDate !== undefined && this.toDate !== null)
        {
            this.Validate();
        }
            

        }
    }

  private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate !== value) {
            this.toDate = value;
            if(this.toDate !== undefined && this.toDate !== null)
            {
                this.Validate(); 
            }
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
            Styles: { width: '110px' },
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

    Validate() {
        this.ValidationErrorsList = [];

        const fromYear = this.FromDate.getFullYear();
        const toYear = this.ToDate.getFullYear();
        if (fromYear !== toYear) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("ARInvoice.O.DatesMustBeInTheSameYear"));
        }
        else if(this.FromDate > this.ToDate) {

            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
        }
        if(this.ValidationErrorsList.length === 0)
        {
            this.RefreshBtnClick();
        }
        
    }
   
    DataSource = {
        pageSize: 50,
        rowCount: null,


        getRows: () => {
            var tempo = this.getRows(); 
            return tempo;
        },
    };

    @Output() MenuHeaderchangeevent = new EventEmitter();
    getRows() {

        const servicelink = "./Invoice/Services/ExtendedPMs/ARInvoiceExtendedService";
        return new Promise((resolve, reject) => {
            SessionLocator.DynamicLoader.GetInstance(servicelink).then((service: any) => {
                resolve(service.getInvoiceSequenceStatus(this.fromDate, this.toDate));
            });
        });
        
}
    ExportToExcelClick()
    {
        var url = ServiceHelper.GetLogitudeURL() + 'api/ARInvoiceExtended/GetInvoiceSequenceStatus2Excel?' + '&fromDate=' + this.fromDate.toISOString() + '&toDate=' + this.toDate.toISOString() + '&tenant=' + SessionLocator.Tenant;
        window.open(url);
    }

  
   
}
