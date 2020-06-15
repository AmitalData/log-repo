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



@Component({
  selector: 'BatchInvoicesComponent',
    providers: [EntityListService],
  templateUrl: './BatchInvoicesComponent.html',
})
export class BatchInvoicesComponent extends BaseComponent {
    entityListService = new EntityListService();
     DataContext: any = this;
    LoadGrids: boolean = false;
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
  public CreateInvoiceText: string = TextCodeTranslator.Translate("InterestReport.O.CreateInvoice");
  constructor() {
    super();
    this.ExcludedItems = new ObservableCollection([]);
    this.selectedItems = new ObservableCollection([]);
    this.Listen();
 }
  @Output() onQueryChangeEvent = new EventEmitter();
  public FireCheckBoxChecked: EventEmitter<any> = new EventEmitter();
  @Output() MenuHeaderchangeevent = new EventEmitter();

  ngOnInit() {
    this.BuildColumns();
    this.ReloadData();

  }
    ReloadData() {
      this.SelectedItemsCount = 0;
      this.selectedItems.Clear();
    this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid

  }
  Listen() {
 this.CurrentSession.InterestReportCheckBoxCheckedEvent.subscribe(($event) => {
            if (!AppTool.IsNullOrEmpty($event)) {
                var row = $event.line;
                var rowId = $event.line.Id;
                var RowIndex = $event.RowIndex;
                var isChecked = $event.isChecked;
               
                this.onCheckBoxChecked(isChecked, row, RowIndex );


            }
        });
      
  }

  
  ngAfterViewInit() {
    var t = setTimeout(() => {
      this.LoadGrids = true;
    }, 100);
  }
    public Columns: any[] = null;
    BuildColumns() {
        this.Columns = [];

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

        this.Columns.push({
          FieldName: 'ReportNumber',
            DataTypeCode: 'String',
          Display: TextCodeTranslator.Translate("InterestReport.F.ReportNumber"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
            HtmlListComponentName: 'InterestReportListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportListTemplate',

        });

        this.Columns.push({
          FieldName: 'GLAccountLocalName',
            DataTypeCode: 'String',
          Display: TextCodeTranslator.Translate("InterestReport.F.GLAccountLocalName"), // 'Source',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
        });

      this.Columns.push({
        FieldName: 'InterestCalculationDate',
        DataTypeCode: 'Date',
        Display: TextCodeTranslator.Translate("InterestReport.F.InterestCalculationDate"),
        Styles: { width: '120px' },
        HtmlListComponentName: 'InterestReportListTemplate',
        HtmlListComponentUrl: './Accounting/Components/ListTemplates/InterestReportListTemplate',
        IsCustomTemplate: true
      });
        this.Columns.push({
          FieldName: 'GLAccountInterestCreditLimit',
            DataTypeCode: 'number',
          Display: TextCodeTranslator.Translate("InterestReport.F.GLAccountInterestCreditLimit"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
           
        });
        this.Columns.push({
          FieldName: 'TotalAmount',
            DataTypeCode: 'number',
          Display: TextCodeTranslator.Translate("InterestReport.F.TotalAmount"), 
            Styles: { width: '80px' },
            IsCustomTemplate: true
            ,
           
        });
        this.Columns.push({
          FieldName: 'InterestReportStatusName',
            DataTypeCode: 'String',
          Display: TextCodeTranslator.Translate("InterestReport.F.InterestReportStatusName"), 
            Styles: { width: '80px' },
            IsCustomTemplate: true
            ,
           
        });
       
    }
  public DataCount: number;
  private EnabledDataCount: number;
    OnDataLoaded(result) {
        if (result) {
          this.DataCount = result.length;
          this.EnabledDataCount = result.filter(d => d.InterestReportStatusCode != "8").length;
          this.SelectedItemsCountText = "selected 0 of " + this.DataCount;
        } 
    }
     today: Date = new Date();
     lastmonth:any = this.today.setDate(this.today.getDay() - 30);
  
  private fromDate: Date = DateTool.NextDay(DateTool.GetCurrentDateTimeAsUtc(), -30);
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
          
        }
    }

  private toDate: Date = new Date();
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            

        }
  }
  private showInProgressReports: boolean = false;
  public get ShowInProgressReports() { return this.showInProgressReports; }
  public set ShowInProgressReports(value: boolean) {
    if (this.showInProgressReports != value) {
      this.showInProgressReports = value;
     // this.ReloadData();

    }
  }
  private allSelected: boolean = false; 
  public get AllSelected() { return this.allSelected; }
  public set AllSelected(value: boolean) {
    if (this.allSelected != value) {
      this.allSelected = value;
      InterestReportEventManager.SelectAllEvent.emit({
        value
      });
      InterestReportEventManager.AllSelected = value;
    if(value)
      this.SelectedItemsCountText = "selected "+ this.DataCount +" of " +this.DataCount;
      else this.SelectedItemsCountText = "selected 0 of " + this.DataCount;
        this.SelectedItemsCount = this.DataCount;
    }
  }
  DataSource = {
    pageSize: 30,
    rowCount: null,
    sortingCol: "InterestCalculationDate",
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
    var fromDate = new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0);
    var toDate = new Date(this.ToDate.getFullYear(), this.ToDate.getMonth(), this.ToDate.getDate(), 0, 0, 0);
    filters.addAdditionalFilter("InterestCalculationDate", fromDate, toDate, null, "Between", false, false, false, "DateTime"); 
    if (this.ShowInProgressReports) {
      filters.addAdditionalFilter("InterestReportStatusCode", "8", null, null, "Equals", false, false, false, "string"); 
    }
    else filters.addAdditionalFilter("InterestReportStatusCode", "1,8,9", null, null, "InList", false, false, false, "string"); 
    
    filters.SortBy = sortingCol;
    filters.SortDirection = sortingDir;


    return this.entityListService.getExtendedByFilters("InterestReport", filters);
  }


  Refresh() {
    this.ReloadData();

    }
    private selectedItems: ObservableCollection;
  private SelectedItemsCount: number=0;
  onCheckBoxChecked(IsChecked:boolean, row:any, rowIndex:any)
  {
    if (IsChecked) {

      if (!this.selectedItems.Collection.includes(row)) {
        this.selectedItems.Insert(row);

              this.SelectedItemsCount += 1;
              this.DataCount = this.DataSource.rowCount;
              if (this.DataCount != null) {
                  this.SelectedItemsCountText = "selected " + (this.SelectedItemsCount).toString() + " of " + this.DataCount.toString();

              }

              if (this.AllSelected) {
                  if (this.ExcludedItems.Collection.includes(row.Id)) {
                      this.ExcludedItems.Remove(row.Id);
                  }
              }
          }
      }
    
    else {
      var removedIndex = null;
      for (var i = 0; i < this.selectedItems.Collection.length; i++) {
       
          removedIndex = i;
          break;
        
      }

      if (removedIndex != null) {

        this.selectedItems.RemoveFromIndex(removedIndex);
      }

     
      this.SelectedItemsCount -= 1;

      if (this.DataCount != null) {
        this.SelectedItemsCountText = "selected " + (this.SelectedItemsCount).toString() + " of " + this.DataCount.toString();

      }
        if (this.AllSelected) {
          if (!this.ExcludedItems.Collection.includes(row.Id)) {
            this.ExcludedItems.Insert(row.Id);
          }
        }
      if (this.SelectedItemsCount == 0) {
          this.SelectedItemsCountText = "selected 0 of " + this.DataCount.toString();
        this.AllSelected = false;
      }

    } this.SetCreateInvoiceButtonText();
  }
  CreateInvoiceButtonClicked() {
    this.ValidationErrorsList = [];
    if (this.SelectedItemsCount == 0) {
      this.ValidationErrorsList.push(TextCodeTranslator.Translate("InterestReport.O.SelectAtLeastOnLine"));
    } else {
      var interestReportArgs: InterestReportArguments=  this.FillInterestReportArgs();  
      this.CurrentSession.StartBusyIndicator("");
      this.interestReportExtendedListService.PutInterestReortStatus(interestReportArgs).subscribe((response: ServiceResponse) => {
        // this.CurrentSession.StopBusyIndicator();
        var mm: ServiceResponse = response;
        if (!mm.HasError) {
            this.BatchId= mm.Result;
            this.timer = setInterval(() => {
              this.GetBTE();
          }, this.timerInterval);
        }
        else {

        }

      });

    }
  }
SetCreateInvoiceButtonText(){
if (this.SelectedItemsCount > 0) {
this.CreateInvoiceText = TextCodeTranslator.Translate("InterestReport.O.CreateInvoice") + "(" + this.SelectedItemsCount + ")";
 }
else{
this.CreateInvoiceText = TextCodeTranslator.Translate("InterestReport.O.CreateInvoice");
}
}
  FillInterestReportArgs() {
    var interestReportArgs: InterestReportArguments = new InterestReportArguments();
    interestReportArgs.AllSelected = this.AllSelected;
    interestReportArgs.FromDate = this.FromDate;
    interestReportArgs.ToDate = this.ToDate;
    interestReportArgs.SelectedIds = [];
    interestReportArgs.ExcludedIds = [];
    interestReportArgs.Tenant =SessionLocator.TenantPM.Id;
    this.selectedItems.Collection.forEach((item) => {
      interestReportArgs.SelectedIds.push(item.Id);
    });
    interestReportArgs.ExcludedIds = this.ExcludedItems.Collection;
    return interestReportArgs;
  }
CancelButtonClicked(){
    this.CurrentSession.CloseCurrentWindow();
}


GetBTE() {
  this._BatchTaskExecutionListService.getSingle(this.BatchId).subscribe((myResult:any) => {
      console.log("[_BatchTaskExecutionListService.getSingle]", myResult);
      var mm: ServiceResponse = myResult;
      if (!mm.HasError) {
          this.bteList = mm.Result;
          if (this.bteList.StatusCode == "D") // D- Done
          {
              //stop timer
              if (this.timer) {
                  clearInterval(this.timer);
               }
               this.SelectedItemsCount=0;
               this.SetCreateInvoiceButtonText();
               this.ReloadData();
               this.AllSelected = false;
               this.CurrentSession.StopBusyIndicator();
          }
          else if (this.bteList.StatusCode == "F") // F- Failed
          {
               this.CurrentSession.StopBusyIndicator();
              if (this.timer) {
                  clearInterval(this.timer);
              }
          }
      }
      else {
      }
  });

}
}
