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
import { InterestReportExtendedListService } from '../../../Accounting/Services/ExtendedLists/InterestReportExtendedListService';
import { InterestReportArguments } from '../../DataContracts/InterestReportArgs';
import { InterestReportEventManager } from '../../Utilities/InterestReportEventManager';
import { BatchTaskExecutionListService } from 'Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from 'Infrastructure/EntityLists/BatchTaskExecutionList';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { InterestLastBatchServicePM } from 'Accounting/EntityPMs/InterestLastBatchServicePM';
 
 



@Component({
  selector: 'BatchInvoicesComponent',
    providers: [EntityListService],
  templateUrl: './BatchInvoicesComponent.html',
})
export class BatchInvoicesComponent extends BaseComponent implements AfterViewInit, OnInit{
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
    public ObjectTableName: string = "InterestReport";
    public CreateInvoiceText: string = TextCodeTranslator.Translate("InterestReport.O.CreateInvoice");
    constructor(private CD: ChangeDetectorRef) {
    super();
    if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    this.ExcludedItems = new ObservableCollection([]);
    this.selectedItems = new ObservableCollection([]);
   // this.Listen();
 }
  @Output() onQueryChangeEvent = new EventEmitter();
  public FireCheckBoxChecked: EventEmitter<any> = new EventEmitter();
  @Output() MenuHeaderchangeevent = new EventEmitter();
 public MarkIsChecked: EventEmitter<any> = new EventEmitter();
    public ColumnsReady: EventEmitter<any> = new EventEmitter();
  ngOnInit() {
    this.InitializeDate();
    this.BuildColumns();
    this.ReloadData();

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
        if (this.ShowInProgressReports) {
            this.Columns.push({
                FieldName: "Select",
                DataTypeCode: 'String',
                Display: '',
                IsCustomTemplate: true,
                Styles: { width: '27px' },
                HtmlListComponentName: 'InterestReportListTemplate',
                HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportListTemplate',

                // IsCheckBox: true,
            });
        }
        else {

            this.Columns.push({
                FieldName: "Select",
                DataTypeCode: 'String',
                Display: '',
                IsCustomTemplate: true,
                Styles: { width: '27px' },             
                 IsCheckBox: true,
            });
        }
        this.Columns.push({
          FieldName: 'ReportNumber',
            DataTypeCode: 'String',
          Display: TextCodeTranslator.Translate("InterestReport.F.ReportNumber"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
           
            HtmlListComponentName: 'InterestReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportListTemplate',
            ServerSideSortable: true,
            SortByName: 'ReportNumber'
        });

        this.Columns.push({
          FieldName: 'GLAccountLocalName',
            DataTypeCode: 'String',
          Display: TextCodeTranslator.Translate("InterestReport.F.GLAccountLocalName"), // 'Source',
            Styles: { width: '300px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'GLAccountLocalName'
        });

      this.Columns.push({
        FieldName: 'InterestCalculationDate',
        DataTypeCode: 'Date',
        Display: TextCodeTranslator.Translate("InterestReport.F.InterestCalculationDate"),
        Styles: { width: '120px' },
        HtmlListComponentName: 'InterestReportListTemplate',
        HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportListTemplate',
        IsCustomTemplate: true,
        ServerSideSortable: true,
        SortByName: 'InterestCalculationDate'
      });
        this.Columns.push({
          FieldName: 'GLAccountInterestCreditLimit',
            DataTypeCode: 'number',
          Display: TextCodeTranslator.Translate("InterestReport.F.GLAccountInterestCreditLimit"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
           SortByName: 'GLAccountInterestCreditLimit'
        });
        this.Columns.push({
          FieldName: 'TotalAmount',
            DataTypeCode: 'number',
          Display: TextCodeTranslator.Translate("InterestReport.F.TotalAmount"), 
            Styles: { width: '100px' },
            IsCustomTemplate: true ,
             ServerSideSortable: true,
            SortByName: 'TotalAmount'
        });
        this.Columns.push({
          FieldName: 'InterestReportStatusName',
            DataTypeCode: 'String',
          Display: TextCodeTranslator.Translate("InterestReport.F.InterestReportStatusName"), 
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InterestReportListTemplate',
          HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportListTemplate',
           ServerSideSortable: true,
            SortByName: 'InterestReportStatusName'
           
        }); this.ColumnsReady.emit(this.Columns); 
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
          
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustBeGTF"));
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.FromDateMustBeLTT"));
                this.CD.detectChanges();
           
        }
         else {
          

            this.UIProperties.SetValidity("ToDate", null, true, "");
            this.UIProperties.SetValidity("FromDate", null, true, "");
         
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
   var selectedLines:ObservableCollection = this.AllSelected?result: this.selectedItems;

 
  this.MarkIsChecked.emit({ SelectedLines:selectedLines,AllSelected: this.AllSelected , ExcludedLines:this.ExcludedItems });

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
  private showInProgressReports: boolean = false;
  public get ShowInProgressReports() { return this.showInProgressReports; }
  public set ShowInProgressReports(value: boolean) {
    if (this.showInProgressReports != value) {
        this.showInProgressReports = value;
        if (value) {

            this.IsSelectAllEnabled = false;
        }
        else this.IsSelectAllEnabled = true;
        this.BuildColumns();
        this.ValidateDate(null);

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
         this.MarkIsChecked.emit({ SelectedLines:this.selectedItems,AllSelected: value, ExcludedLines:this.ExcludedItems });

        }
        this.SetCreateInvoiceButtonText();
    }
  }
  DataSource = {
    pageSize: 30,
    rowCount: null,
    sortingCol: "ReportNumber",
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
    // var fromDate = new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0);
    // var toDate = new Date(this.ToDate.getFullYear(), this.ToDate.getMonth(), this.ToDate.getDate(), 0, 0, 0);
    // console.log(fromDate);
    // console.log(toDate);

    filters.addAdditionalFilter("InterestCalculationDate", this.fromDate, this.toDate, null, "Between", false, false, false, "DateTime"); 
    if (this.ShowInProgressReports) {
      filters.addAdditionalFilter("InterestReportStatusCode", "1,9,8", null, null, "InList", false, false, false, "string"); 
    }
    else filters.addAdditionalFilter("InterestReportStatusCode", "1,9", null, null, "InList", false, false, false, "string"); 
    
    filters.SortBy = sortingCol;
    filters.SortDirection = sortingDir;


    return this.entityListService.getExtendedByFilters("InterestReport", filters);
  }


    Refresh() {
        this.ValidateDate(null);
   // this.ReloadData();

    }
    private SelectingItem(rowData: any) {
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
    private UnSelectingItem(rowData:any) {
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
    }
  public SelectedItemsCount: number=0;
    onCheckBoxChecked($event: any) {
        var isChecked: boolean = $event.IsChecked == undefined ? $event.isChecked : $event.IsChecked;
        var rowData = $event.rowData == undefined ? $event.line : $event.rowData;
        if (isChecked) {
            this.SelectingItem(rowData);
        }

        else {
            this.UnSelectingItem(rowData);
        }

        this.SetCreateInvoiceButtonText();
    }


    IsCloseWithoutInvoice: boolean = false;
    CloseWithoutInvoice() {
        if (this.SelectedItemsCount == 0) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("InterestReport.O.SelectAtLeastOnLine"));
        }
        else {
            this.ConfirmClosingWithoutInvoice();
        }
      
    }
    CreateInvoiceButtonClicked() {
        this.IsCloseWithoutInvoice = false;;
        this.UpdateInterestReportsStatuses();
     
  }
    UpdateInterestReportsStatuses() {
        var interestReportArgs: InterestReportArguments = this.FillInterestReportArgs();
        this.CurrentSession.StartBusyIndicatorLoading();
        this.interestReportExtendedListService.PutInterestReortStatus(interestReportArgs).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            var mm: ServiceResponse = response;
            if (!mm.HasError) {
                this.BatchId = mm.Result;
                this.CancelButtonClicked();
            }
            else {
                if (mm.ErrorsArray) {
                    var msg = new MessageWindow();
                    msg.RTL = this.isRTL;
                    msg.Width = 400;
                    msg.Show(mm.ErrorsArray[0]);
                }
            }

        });
    }
  ShowInvoiceDateForBatchInvoiceComponent(){
    var logWindow = new LogitudeWindow();
    logWindow.Title = TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate");
    var myPath = "./Accounting/Components/Packages/Others/InvoiceDateForBatchInvoicesComponent";
    logWindow.Width = 360;
    logWindow.Height = 160;
    //logWindow.DataContext = "" ;
    logWindow.Show(myPath);
    //logWindow.IsShowCloseButton=true;
    logWindow.WindowClosed.subscribe(s => {
        if (s!=null) {
           this.InvoiceDate = new Date(s);
           this.CreateInvoiceButtonClicked(); 
        }
    })
    //InvoiceDateForBatchInvoicesComponent
  }
   CheckNumberOfInterestReportInvoicingWithoutInvoice(){
    this.ValidationErrorsList = [];
    if (this.SelectedItemsCount == 0) {
      this.ValidationErrorsList.push(TextCodeTranslator.Translate("InterestReport.O.SelectAtLeastOnLine"));
    } else {
      var interestReportArgs: InterestReportArguments=  this.FillInterestReportArgs();  
      this.CurrentSession.StartBusyIndicatorLoading();
      this.interestReportExtendedListService.CheckNumberOfInterestReportInvoicingWithoutInvoice(interestReportArgs).subscribe((response: ServiceResponse) => {
      this.CurrentSession.StopBusyIndicator();
        var mm: ServiceResponse = response;
        if (!mm.HasError) {
        var NumberOfReportsWithoutInvoices = mm.Result;
        if(NumberOfReportsWithoutInvoices>0){
          this.ShowWarninngAboutReportsWithoutInvoice(NumberOfReportsWithoutInvoices,interestReportArgs);
         }
         else{
         // this.CreateInvoiceButtonClicked();
         this.ShowInvoiceDateForBatchInvoiceComponent();
         }
        }
        else {
          if(mm.ErrorsArray){
            var msg = new MessageWindow();
            msg.RTL = this.isRTL;
            msg.Width = 400;
            msg.Show(mm.ErrorsArray[0]);
        }
        }

      });

    }

  }
 
  // GetInterestLastBatchServiceByTenant(){
  //   this.CurrentSession.StartBusyIndicatorLoading();
  //   this.interestReportExtendedListService.GetInterestLastBatchServiceByTenant().subscribe((response: ServiceResponse) => {
  //     this.CurrentSession.StopBusyIndicator();
  //       var mm: ServiceResponse = response;
  //       if (!mm.HasError) {
  //       //  var InterestLastBatchService:InterestLastBatchServicePM  = mm.Result;
  //       //  if(!InterestLastBatchService || (InterestLastBatchService  && !InterestLastBatchService.CreateInvoicesBatchId)){
  //          this.CheckNumberOfInterestReportInvoicingWithoutInvoice();
  //       //  }
  //       //  else if(InterestLastBatchService && InterestLastBatchService.CreateInvoicesBatchId){
  //       //   this.CheckBatchTaskExcecutingAndCreateInvoices(InterestLastBatchService.CreateInvoicesBatchId);
  //       //  }
  //       }
  //       else {
  //         if(mm.ErrorsArray){
  //           var msg = new MessageWindow();
  //           msg.RTL = this.isRTL;
  //           msg.Width = 400;
  //           msg.Show(mm.ErrorsArray[0]);
  //       }
  //       }

  //     });
  // }



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

    ConfirmClosingWithoutInvoice() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 390;
        var NumberIdsSelected: number = 0;
       
        confirmWindow.Show( TextCodeTranslator.Translate("InterestReport.O.CloseAllSelectedWithoutInvoice"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.IsCloseWithoutInvoice = true;
                this.UpdateInterestReportsStatuses();                
                
            }
        });
    }
ShowWarninngAboutReportsWithoutInvoice(NumberOfReportsWithoutInvoices:number,interestReportArgs: InterestReportArguments) {
      var confirmWindow = new ConfirmWindow();
      confirmWindow.Width = 390;
      var NumberIdsSelected:number=0;
      if(interestReportArgs.AllSelected){
          NumberIdsSelected = this.DataCount - interestReportArgs.ExcludedIds.length;
      }
      else{
          NumberIdsSelected = interestReportArgs.SelectedIds.length;
      }
      confirmWindow.Show(  NumberOfReportsWithoutInvoices+" "+TextCodeTranslator.Translate("InterestReport.O.OutOf")+" " + this.SelectedItemsCount + " " +TextCodeTranslator.Translate("InterestReport.O.SelectedReportsWillNotHaveAnInvoice"));
      confirmWindow.WindowClosed.subscribe((event: any) => {
          if (confirmWindow.Yes) {
              
             if(NumberOfReportsWithoutInvoices == this.SelectedItemsCount){
              this.CreateInvoiceButtonClicked();
             }
             else{
              this.ShowInvoiceDateForBatchInvoiceComponent();
             }
             
          } else if (confirmWindow.No) {

          }
      });
}  
    public CloseWithoutInvoiceText: string = TextCodeTranslator.Translate("InterestReport.O.CloseWithoutInvoice");
  
SetCreateInvoiceButtonText(){
if (this.SelectedItemsCount > 0) {
    this.CreateInvoiceText = TextCodeTranslator.Translate("InterestReport.O.CreateInvoice") + "(" + this.SelectedItemsCount + ")";
    this.CloseWithoutInvoiceText = TextCodeTranslator.Translate("InterestReport.O.CloseWithoutInvoice")+ "(" + this.SelectedItemsCount + ")";
 }
else{
    this.CreateInvoiceText = TextCodeTranslator.Translate("InterestReport.O.CreateInvoice");
    this.CloseWithoutInvoiceText = TextCodeTranslator.Translate("InterestReport.O.CloseWithoutInvoice");
}
}
  FillInterestReportArgs() {
    var interestReportArgs: InterestReportArguments = new InterestReportArguments();
    interestReportArgs.AllSelected = this.AllSelected;
    interestReportArgs.FromDate = this.FromDate;
    interestReportArgs.ToDate = this.ToDate;
      interestReportArgs.InvoiceDate = this.InvoiceDate;
      interestReportArgs.CloseWithoutInvoice = this.IsCloseWithoutInvoice;
    interestReportArgs.SelectedIds = [];
    interestReportArgs.ExcludedIds = [];
    interestReportArgs.Tenant =SessionLocator.TenantPM.Id;
    this.selectedItems.Collection.forEach((item) => {
      interestReportArgs.SelectedIds.push(item.Id);
    });
    interestReportArgs.ExcludedIds = this.ExcludedItems.Collection;
    return interestReportArgs;
  }
    CancelButtonClicked() {
        this.SelectedItemsCount = 0;
        this.selectedItems.Clear();
        this.CurrentSession.CloseCurrentWindow();

}

private invoiceDate:Date; 
get InvoiceDate(){
    return this.invoiceDate;
}
set InvoiceDate(val: Date){
     this.invoiceDate=val;
}

CheckBatchTaskExcecutingAndCreateInvoices( BatchId:string) {
  this._BatchTaskExecutionListService.getSingle( BatchId).subscribe((myResult:any) => {
      console.log("[_BatchTaskExecutionListService.getSingle]", myResult);
      var mm: ServiceResponse = myResult;
      if (!mm.HasError) {
          this.bteList = mm.Result;
          if (this.bteList.StatusCode == "D" || this.bteList.StatusCode == "F") // D- Done
          {  
            //this.CreateInvoiceButtonClicked();
            this.ShowInvoiceDateForBatchInvoiceComponent();
          }
          else{
            var msg = new MessageWindow();
            msg.RTL = this.isRTL;
            msg.Width = 400;
            msg.Show(TextCodeTranslator.Translate("InterestReport.O.AnotherBatchInvoiceStillInProgress"));

          }
 
      }
      else {
      }
  });


    }


}



