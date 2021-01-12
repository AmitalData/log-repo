import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool , DateTool} from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters, FilterItem } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { InterestReportExtendedListService, PDFDocumentInvoices } from '../../../Accounting/Services/ExtendedLists/InterestReportExtendedListService';
import { InterestReportArguments, SelectItem } from '../../DataContracts/InterestReportArgs';
import { InterestReportEventManager } from '../../Utilities/InterestReportEventManager';
import { BatchTaskExecutionListService } from 'Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from 'Infrastructure/EntityLists/BatchTaskExecutionList';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { BatchPrintServiceHelper } from './BatchPrintServiceHelper';
 


@Component({
  selector: 'BatchPrintComponent',
    providers: [EntityListService],
  templateUrl: './BatchPrintComponent.html',
})
export class BatchPrintComponent extends BaseComponent implements AfterViewInit, OnInit{
    entityListService = new EntityListService();
    DataContext: any = this;
    LoadGrids: boolean = false;
    public isRTL: boolean = false;
    private ExcludedItems: ObservableCollection;
    private interestReportExtendedListService: InterestReportExtendedListService = new InterestReportExtendedListService();
    private CurrentSession = SessionLocator.SelectedSession;
    public SelectedItemsCountText: string = null;
    public ValidationErrorsList: string[] = [];
    public BatchId:string;
    public _BatchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    public bteList: BatchTaskExecutionList;
    public timer: any;
    public timerInterval: number = 1000;
    public ObjectTableName: string = "ARInvocie";
    public PrintButtonText: string = TextCodeTranslator.Translate("ARInvoice.B.Print");
    public BatchPrintServiceHelper:BatchPrintServiceHelper;

    constructor(private CD: ChangeDetectorRef) {
    super();
    if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    this.ExcludedItems = new ObservableCollection([]);
    this.selectedItems = new ObservableCollection([]);
    this.BatchPrintServiceHelper = new BatchPrintServiceHelper();
    this.AttachReportWithEachInvoice = true;
   // this.Listen();
 }
  @Output() onQueryChangeEvent = new EventEmitter();
  public FireCheckBoxChecked: EventEmitter<any> = new EventEmitter();
  @Output() MenuHeaderchangeevent = new EventEmitter();
  //public MarkIsChecked: EventEmitter<any> = new EventEmitter();
    public ColumnsReady: EventEmitter<any> = new EventEmitter();
    public MarkIsChecked: EventEmitter<any> = new EventEmitter();
  ngOnInit() {
    this.InitializeDate();
    this.BuildColumns();
    this.ReloadData();

  }

  
  OnSortInvoked(event){
    // this.TransactionSelectedLines = new ObservableCollection([]);
    // this.ExtPageSelectedLines = new ObservableCollection([]);
}

private selectedItems:ObservableCollection;
    ReloadData() {
      
      this.SelectedItemsCount = 0;
        this.selectedItems.Clear();
        this.SetCreateInvoiceButtonText();
        this.AllSelected = false;
    this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid

  }
 
  ngAfterViewInit() {
    var t = setTimeout(() => {
      this.LoadGrids = true;
    }, 100);
  }
    public Columns: any[] = null;
    BuildColumns() {
        this.Columns = [];
        // if (this.ShowPrintedInvoice) {
        //     this.Columns.push({
        //         FieldName: "Select",
        //         DataTypeCode: 'String',
        //         Display: '',
        //         IsCustomTemplate: true,
        //         Styles: { width: '27px' },
        //         HtmlListComponentName: 'InterestInvoiceListTemplate',
        //         HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',
        //     });
        // }
        // else {

      this.Columns.push({
                FieldName: "Select",
                DataTypeCode: 'String',
                Display: '',
                IsCustomTemplate: true,
                Styles: { width: '27px' },             
                 IsCheckBox: true,
            });
        // }
      this.Columns.push({
          FieldName: 'InvoiceNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("InterestReport.CH.ARInvoiceNumberListLable"),
            Styles: { width: '130px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            HtmlListComponentName: 'InterestInvoiceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',

        });

      this.Columns.push({
          FieldName: 'InvoiceDate',
            DataTypeCode: 'Date',
            Display: TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate"),
            Styles: { width: '130px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            HtmlListComponentName: 'InterestInvoiceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',
        });

      this.Columns.push({
        FieldName: 'StatusName',
        DataTypeCode: 'String',
        Display: TextCodeTranslator.Translate("ARInvoice.F.StatusName"),
        Styles: { width: '120px' },
        ServerSideSortable: true,
        HtmlListComponentName: 'InterestInvoiceListTemplate',
        HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',
        IsCustomTemplate: true
      });

     this.Columns.push({
            FieldName: 'BillToName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("ARInvoice.F.BillToId"),
            Styles: { width: '150px' },
            ServerSideSortable: true,
            HtmlListComponentName: 'InterestInvoiceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',
            IsCustomTemplate: true
        });


     this.Columns.push({
          FieldName: 'InvoiceCurrencyCode',
            DataTypeCode: 'String',
          Display: TextCodeTranslator.Translate("ARInvoice.F.InvoiceCurrencyCode"), 
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            HtmlListComponentName: 'InterestInvoiceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',
           
        });


        this.Columns.push({
          FieldName: 'AmountInInvoiceCurrency',
            DataTypeCode: 'number',
          Display: TextCodeTranslator.Translate("ARInvoice.F.AmountInInvoiceCurrency"), 
            Styles: { width: '130px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            HtmlListComponentName: 'InterestInvoiceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',
           
        });


        
      this.Columns.push({
        FieldName: 'AmountDue',
          DataTypeCode: 'number',
          Display: TextCodeTranslator.Translate("ARInvoice.F.AmountDue"),
          Styles: { width: '130px' },
          IsCustomTemplate: true,
          ServerSideSortable: true,
          HtmlListComponentName: 'InterestInvoiceListTemplate',
          HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',
      });


      this.Columns.push({
         FieldName: 'IsPrinted',
          DataTypeCode: 'Boolean',
          Display: TextCodeTranslator.Translate("ARInvoice.F.IsPrinted"),
          Styles: { width: '40px' },
          IsCustomTemplate: true,
          ServerSideSortable: true,
          HtmlListComponentName: 'InterestInvoiceListTemplate',
          HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestInvoiceListTemplate',
          ColumnHeaderTemplateName: 'PrintedListHeaderTemplate',
          ColumnHeaderTemplateUrl: './Accounting/Components/ListTemplates/PrintedListHeaderTemplate',
      });
        
        
        this.ColumnsReady.emit(this.Columns); 
        this.CurrentSession.InterestReportCheckBoxCheckedEvent.subscribe(($event) => {
            if (!AppTool.IsNullOrEmpty($event)) {
                var row = $event.line;
                var rowId = $event.line.Id;
                var RowIndex = $event.RowIndex;
                var isChecked = $event.isChecked;

                this.onCheckBoxChecked($event);
                this.FireCheckBoxChecked.emit({ rowData: row, IsChecked: isChecked, RowIndex: RowIndex });
                this.MarkIsChecked.emit({ MyRecord: row,AllSelected:this.AllSelected , ExcludedLines:this.ExcludedItems});
              }
        });

    }
    private timerToken: any;
    ValidateDate(fieldName: any) {
        this.ValidationErrorsList = [];
        if (DateTool.GetDateFromDate(this.FromDate, true) > DateTool.GetDateFromDate(this.ToDate, true)) {
            if (fieldName == null) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
            }
            this.timerToken = setTimeout(() => {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.FromDateMustBeLTT"));
                this.CD.detectChanges();
            }, 200);
        }
         else {
            this.timerToken = setTimeout(() => {

            this.UIProperties.SetValidity("ToDate", null, true, "");
            this.UIProperties.SetValidity("FromDate", null, true, "");
            }, 200);
                this.ReloadData();
        }
           
        
    }
  public DataCount: number;
  private EnabledDataCount: number;
    OnDataLoaded(result) {
        if (result) {
          result = new ObservableCollection(result);
          this.DataCount = this.DataSource.rowCount;
          this.EnabledDataCount = result.Collection.filter(d => d.InterestReportStatusCode != "8").length;
          this.SelectedItemsCountText = "selected 0 of " + this.DataCount;
        } 
        var selectedLines = this.AllSelected?result: this.selectedItems;
        this.MarkIsChecked.emit({ SelectedLines:selectedLines,AllSelected: this.AllSelected, ExcludedLines:this.ExcludedItems});
    }
     today: Date = new Date();
     lastmonth:any = this.today.setDate(this.today.getDay() - 30);
  
  private fromDate: Date;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.ValidateDate("FromDate");

        }
    }

  private toDate: Date;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.ValidateDate("ToDate");
           

        }
  }
  private showPrintedInvoice: boolean = false;
  public get ShowPrintedInvoice() { return this.showPrintedInvoice; }
  public set ShowPrintedInvoice(value: boolean) {
    if (this.showPrintedInvoice != value) {
        this.showPrintedInvoice = value;
 
        this.BuildColumns();
        this.ValidateDate(null);

    }
    }
    private attachReportWithEachInvoice: boolean = false;
    public get AttachReportWithEachInvoice() { return this.attachReportWithEachInvoice; }
    public set AttachReportWithEachInvoice(value: boolean) {
      if (this.attachReportWithEachInvoice != value) {
          this.attachReportWithEachInvoice = value;
       }
      }

    public IsSelectedItemsTextVisibile: boolean = false;
    public IsSelectAllEnabled: boolean = true;
  private allSelected: boolean = false; 
  public get AllSelected() { return this.allSelected; }
  public set AllSelected(value: boolean) {
    if (this.allSelected != value) {
      this.allSelected = value;
      //InterestReportEventManager.SelectAllEvent.emit({
      //  value
      //});
      //InterestReportEventManager.AllSelected = value;
        if (value) {
            this.IsSelectedItemsTextVisibile = true;
            this.SelectedItemsCountText = "selected " + this.DataSource.rowCount + " of " + this.DataSource.rowCount;
            this.ExcludedItems.Clear();
            this.SelectedItemsCount = this.DataSource.rowCount;

        } else {
            this.IsSelectedItemsTextVisibile = false;
            this.SelectedItemsCount = 0;
            this.selectedItems.Clear();
            this.ExcludedItems.Clear();
            this.MarkIsChecked.emit({ SelectedLines:this.selectedItems,AllSelected: value, ExcludedLines:this.ExcludedItems});
        }
        this.SetCreateInvoiceButtonText();
    }
  }
  DataSource = {
    pageSize: 30,
    rowCount: null,
    sortingCol: "InvoiceDate",
    sortingDir: "Descending",
    getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
      var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
      return tempo;
    },
  };

  getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
    var filters = new ApiQueryFilters;
   
    filters.SortBy = sortingCol;
    filters.SortDirection = sortingDir;
    filters.PageSize = take;
    filters.PageIndex = skip + 1; // decremented 1 in the service
    filters.GetAll = true;
    filters.GetCount = true;
   
    if (!this.ShowPrintedInvoice) {
      filters.addAdditionalFilter("IsPrinted", false, null, null, "Equal", false, false, false, "Boolean")
    }
    filters.addAdditionalFilter("InvoiceDate", this.fromDate, this.toDate, null, "Between", false, false, false, "DateTime"); 
    filters.addAdditionalFilter("ARInvoiceTypeCode", "IT", null ,null, "Equal", false, false, false, "string"); 
    filters.SortBy = sortingCol;
    filters.SortDirection = sortingDir;


    return this.entityListService.getByFilters("ARInvoice", filters);
  }


    Refresh() {
        this.ValidateDate(null);
   // this.ReloadData();

    }
  
  public SelectedItemsCount: number=0;
    onCheckBoxChecked($event:any)
    {
        var isChecked: boolean = $event.IsChecked == undefined ? $event.isChecked : $event.IsChecked;

        var rowData = $event.rowData == undefined ? $event.line : $event.rowData;
        if (isChecked) {

            if (!this.selectedItems.Collection.includes(rowData)) {
                this.selectedItems.Insert(rowData);

              this.SelectedItemsCount += 1;
              this.DataCount = this.DataSource.rowCount;
              if (this.DataCount != null) {
                  this.SelectedItemsCountText = "selected " + (this.SelectedItemsCount).toString() + " of " + this.DataCount.toString();

              }

              if (this.AllSelected) {
                  if (this.ExcludedItems.Collection.includes(rowData.Id)) {
                      this.ExcludedItems.Remove(rowData.Id);
                  }
              }
          }
      }
    
    else {
           //this.AllSelected= false;
            this.selectedItems.Remove(this.selectedItems.Collection.find(c => c.Id == rowData.Id));
      this.SelectedItemsCount -= 1;

      if (this.DataCount != null) {
        this.SelectedItemsCountText = "selected " + (this.SelectedItemsCount).toString() + " of " + this.DataCount.toString();

      }
        if (this.AllSelected) {
            if (!this.ExcludedItems.Collection.includes(rowData.Id)) {
                this.ExcludedItems.Insert(rowData.Id);
          }
        }
      if (this.SelectedItemsCount == 0) {
          this.IsSelectedItemsTextVisibile = false;
        this.AllSelected = false;
      }

    } this.SetCreateInvoiceButtonText();
  }
  
  PrintInterestInvoice(){
    this.ValidationErrorsList = [];
    if (this.SelectedItemsCount == 0) {
      this.ValidationErrorsList.push(TextCodeTranslator.Translate("InterestReport.O.SelectAtLeastOnLine"));
    } else { 

var interestReportArgs: InterestReportArguments=  this.FillInterestReportArgs();  

 
  this.BatchPrintServiceHelper.PrintAllInterestInvoices(interestReportArgs);
        
   
    }

  }


  CreateInvoiceButtonClicked(interestReportArgs: InterestReportArguments) {
     this.interestReportExtendedListService.PutBatchPrint(interestReportArgs).subscribe((response: ServiceResponse) => {
      this.CurrentSession.StopBusyIndicator();
      var mm: ServiceResponse = response;
      if (!mm.HasError) {
        let file = new Blob([mm.Result], { type: 'application/pdf' });
        let url =  URL.createObjectURL(file);       

        this.ShowBtatchPrintConfirmComponent(url);
       
      }
      else {
        // if(this.newWindow){
        //   this.newWindow.blur();
        //   this.newWindow.close();
        // }
        if(mm.ErrorsArray){
          var msg = new MessageWindow();
          msg.RTL = this.isRTL;
          msg.Width = 400;
          msg.Show(mm.ErrorsArray[0]);
      }
      }

    });
  
}

// private OpenWindow(){
//   this.newWindow = window.open('', '_blank');//OPEN WINDOW FIRST ON SUBMIT THEN POPULATE PDF
//   this.newWindow.document.write("<html><head> </head><body> <style> body { padding: 10em 0; } .loading { font-size: 84px; font-family: 'Montserrat', sans-serif; font-weight: 800; text-align: center; } .loading span { display: inline-block; margin: 0 -0.05em; } .loading07 span { position: relative; color: rgba(0, 0, 0, 0.2); } .loading07 span::after { position: absolute; top: 0; left: 0; content: attr(data-text); color: #grey; opacity: 0; transform: scale(1.5); animation: loading07 3s infinite; } .loading07 span:nth-child(2)::after { animation-delay: 0.1s; } .loading07 span:nth-child(3)::after { animation-delay: 0.2s; } .loading07 span:nth-child(4)::after { animation-delay: 0.3s; } .loading07 span:nth-child(5)::after { animation-delay: 0.4s; } .loading07 span:nth-child(6)::after { animation-delay: 0.5s; } .loading07 span:nth-child(7)::after { animation-delay: 0.6s; } @keyframes loading07 { 0%, 75%, 100% { transform: scale(1.5); opacity: 0; } 25%, 50% { transform: scale(1); opacity: 1; } } </style> <section> <div class='loading loading07'> <span data-text='I'>I</span> <span data-text='n'>n</span> <span data-text=''> </span> <span data-text='P'>P</span> <span data-text='r'>r</span> <span data-text='o'>o</span> <span data-text='g'>g</span> <span data-text='r'>r</span> <span data-text='e'>e</span> <span data-text='s'>s</span> <span data-text='s'>s</span> <span data-text='.'>.</span> <span data-text='.'>.</span> <span data-text='.'>.</span> </div> </section> </body></html>");
 
// }

GetNumberOfDocumentNotPrinted(isReportsAttached: boolean) {
  this.AttachReportWithEachInvoice = isReportsAttached ? true: false;
  this.ValidationErrorsList = [];
  if (this.SelectedItemsCount == 0) {
    this.ValidationErrorsList.push(TextCodeTranslator.Translate("InterestReport.O.SelectAtLeastOnLine"));
  } else { 
    //  this.OpenWindow();
    var interestReportArgs: InterestReportArguments=  this.FillInterestReportArgs();  
    this.interestReportExtendedListService.GetNumberOfDocumentNotPrinted(interestReportArgs).subscribe((response: ServiceResponse) => {
    this.CurrentSession.StopBusyIndicator();
    var mm: ServiceResponse = response;
    if (!mm.HasError) {
    var pDFDocumentInvoices:PDFDocumentInvoices = mm.Result;
    if((!AppTool.IsNullOrEmpty(pDFDocumentInvoices.ARInvoiceNumbersNotPrinted) && pDFDocumentInvoices.ARInvoiceNumbersNotPrinted.length>0 ) ||  (!AppTool.IsNullOrEmpty(pDFDocumentInvoices.InterestReportNumbersNotPrinted) && pDFDocumentInvoices.InterestReportNumbersNotPrinted.length >0 )){
      // if(this.newWindow){
      //   this.newWindow.blur();
      //   this.newWindow.close();
      // }
      this.ShowBtatchPrintWarningComponent(pDFDocumentInvoices,interestReportArgs);
    }
    else{
      this.CurrentSession.StartBusyIndicatorLoading();
      setTimeout(() =>  this.CreateInvoiceButtonClicked(interestReportArgs),200);
    }
    
    }
    else {
      // if(this.newWindow){
      //   this.newWindow.blur();
      //   this.newWindow.close();
      // }
      if(mm.ErrorsArray){
        var msg = new MessageWindow();
        msg.RTL = this.isRTL;
        msg.Width = 400;
        msg.Show(mm.ErrorsArray[0]);
    }
    }

  });
    }
    this.DropdownClose();
    }


// public newWindow:any;

 ShowBtatchPrintWarningComponent(DataContext:PDFDocumentInvoices,interestReportArgs: InterestReportArguments) {
  var logWindow = new LogitudeWindow();
  logWindow.Title = TextCodeTranslator.Translate("General.O.Warning");
  var myPath = "./Accounting/Components/Packages/Others/BtatchPrintWarningComponent";
  logWindow.Width = 650;
  logWindow.Height = 260;
  DataContext.Document =  this.SelectedItemsCount;
  logWindow.DataContext = DataContext;
  logWindow.IsShowCloseButton = true;
  logWindow.Show(myPath);
  logWindow.WindowClosed.subscribe(s => {
      if (s!=null) {
        // this.OpenWindow();
         this.CurrentSession.StartBusyIndicatorLoading();
          this.CreateInvoiceButtonClicked(interestReportArgs);
      }
  })
}

ShowBtatchPrintConfirmComponent(URL) {
  var logWindow = new LogitudeWindow();
  logWindow.Title = TextCodeTranslator.Translate("InterestReport.O.BatchPrint");
  var myPath = "./Accounting/Components/Packages/Others/BtatchPrintConfirmComponent";
  logWindow.Width = 560;
  logWindow.Height = 160;
  logWindow.DataContext = URL;
  logWindow.Show(myPath);
  logWindow.IsShowCloseButton = true;
  logWindow.WindowClosed.subscribe(s => {
    if (s!=null) {
      this.Refresh();
    }
  
  });
  // logWindow.WindowClosed.subscribe(s => {
  //     if (s!=null) {
  //       if(s=="View"){
  //          window.open(URL,  '_blank');
  //        }
  //       else{
  //         var a = document.createElement("a");
  //         a.href = URL;
  //         a.download = "InterestInvoices.pdf"; 
  //         a.click();
  //          window.URL.revokeObjectURL(URL);        
          
  //         }
    
  //     }
  // })
}

  InitializeDate(){
  
  var month = new Date().getMonth();
  var Year = new Date().getFullYear();
  var Day = new Date().getDate();
  this.ToDate = this.SetDate(Year, month, Day);
  this.FromDate = this.SetDate(Year, month, Day);
  this.FromDate.setUTCDate(this.ToDate.getDate() - 30);
}

 
  SetDate(year: number, month: number, day: number) {
    var date = new Date();
    date.setUTCFullYear(year);
    date.setUTCMonth(month);
    date.setUTCDate(day);
    date.setUTCHours(0);
    date.setUTCMinutes(0);
    date.setUTCSeconds(0);
    date.setUTCMilliseconds(0);

    return date;
}
 
  
SetCreateInvoiceButtonText(){
if (this.SelectedItemsCount > 0) {
this.PrintButtonText = TextCodeTranslator.Translate("ARInvoice.B.Print") + "(" + this.SelectedItemsCount + ")";
 }
else{
this.PrintButtonText = TextCodeTranslator.Translate("ARInvoice.B.Print");
}
}


public SelectedItems :SelectItem[]=[];


  FillInterestReportArgs() {
    var interestReportArgs: InterestReportArguments = new InterestReportArguments();
    interestReportArgs.AllSelected = this.AllSelected;
    interestReportArgs.FromDate = this.FromDate;
    interestReportArgs.ToDate = this.ToDate;
    interestReportArgs.SelectedIds = [];
    interestReportArgs.ExcludedIds = [];
    interestReportArgs.Entities = [];
    interestReportArgs.SelectedItems = [];
    interestReportArgs.ShowPrintedInvoice = this.showPrintedInvoice;
    interestReportArgs.AttachReportWithEachInvoice = this.AttachReportWithEachInvoice;
    interestReportArgs.Tenant =SessionLocator.TenantPM.Id;
    this.selectedItems.Collection.forEach((item) => {
      interestReportArgs.SelectedIds.push(item.Id);
     });
    interestReportArgs.SelectedItems = this.SelectedItems;
    interestReportArgs.ExcludedIds = this.ExcludedItems.Collection;
    return interestReportArgs;
    }

    CancelButtonClicked() {
        this.SelectedItemsCount = 0;
        this.selectedItems.Clear();
        this.CurrentSession.CloseCurrentWindow();
    }

    getScreenHeight() { return self.innerHeight; }
    dropdownDisplay: string = 'none';
    DropdowndisplayToggle() {
        var item = document.getElementById("printbutton");
        var itemRect = item.getBoundingClientRect();

        let DDLHeight =22.5;//    height: 22px; * 3 +30 
        let Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
        if (itemRect.bottom + DDLHeight < this.getScreenHeight()) {//this.PaintTop = true                
            document.getElementById("dropdowmenu").style.top = (itemRect.bottom - DDLHeight - Extra) + 'px';
        }
      
        if (this.dropdownDisplay == 'none') {
            this.dropdownDisplay = 'block';
        }
        else {
            this.dropdownDisplay = 'none';
        }
    }

    DropdownClose() {
        this.dropdownDisplay = 'none';
    }
 
}



