import { Component, OnInit, AfterViewInit, Output, EventEmitter} from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { DateTool, AppTool } from 'Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { CargoTrackingExtendedPMService, CargoTrackingArgs } from 'Accounting/Services/ExtendedPMs/CargoTrackingExtendedPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { CargoTrackingIncrementalStatExtendedListService } from 'Accounting/Services/ExtendedLists/CargoTrackingIncrementalStatExtendedListService';
import { CargoTrackingIncrementalStatService } from 'Accounting/Services/Others/CargoTrackingIncrementalStatService';
 

@Component({
    
    templateUrl: './CargoTrackingIncrementalStatistics.html',
})

export class CargoTrackingIncrementalStatistics extends BaseComponent implements OnInit  {
    private _entityListService: CargoTrackingIncrementalStatExtendedListService;
    private _CargoTrackingIncrementalStatService:CargoTrackingIncrementalStatService ;
    private CurrentSession = SessionLocator.SelectedSession;
    public _CargoTrackingIncrementalArgs:CargoTrackingIncrementalArgs;
    public ValidationErrorsList: string[] = [];
    public DataContext: any = this;
    public ObjectTableName: string = "CargoTrackingIncrementalStat";
    public Main_Filter:ApiQueryFilters;
    @Output() onQueryChangeEvent = new EventEmitter();
    constructor(){
        super();
        this._CargoTrackingIncrementalArgs = new CargoTrackingIncrementalArgs();
        this._CargoTrackingIncrementalStatService = new CargoTrackingIncrementalStatService();
        this._entityListService = new CargoTrackingIncrementalStatExtendedListService();
    }
 
    public GridHeaderText:string = "Incremental Records Details";

    ngOnInit() {
        this.InitializeDate();
        this.GetCargoTrackingIncrementalData();
        this.BuildColumns();
    }
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
    public DataSource = {
        pageSize: 50,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        this.Main_Filter = this.ImplementFilter(take,skip);
        return this._entityListService.getByFiltersForVirtualization(this.Main_Filter); 
    }

    private ImplementFilter(take,skip):ApiQueryFilters{
        var filters = new ApiQueryFilters();
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.addAdditionalFilter("Start_End_Date", this.FromDate, this.ToDate, null, "Between", true, false, false, "DateTime"); 
        return filters;
    }
    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'Id',
            DataTypeCode: 'Number',
            Display: 'Number', 
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'StartDate',
            DataTypeCode: 'DateTime',
            Display: "Start Date",
            Styles: { width: '160px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'EndDate',
            DataTypeCode: 'DateTime',
            Display: 'EndDate',  
            Styles: { width: '160px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Shipments',
            DataTypeCode: 'Number',
            Display: 'Shipments',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'Cards',
            DataTypeCode: 'Number',
            Display: 'Cards',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'Ports',
            DataTypeCode: 'Number',
            Display: 'Ports',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'Countries',
            DataTypeCode: 'Number',
            Display: 'Countries',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'ShipmentComputedFields',
            DataTypeCode: 'Number',
            Display: 'ShipmentComputedFields',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'ShipmentMasterDatas',
            DataTypeCode: 'Number',
            Display: 'ShipmentMasterDatas',  
            Styles: { width: '100px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'TransportModes',
            DataTypeCode: 'Number', 
            Display: 'TransportModes',  
            Styles: { width: '130px' },
            HtmlListComponentName: 'CargoTrackingIncrementalStatListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/CargoTrackingIncrementalStatListTemplate',
            IsCustomTemplate: true
        });
    }
    GetCargoTrackingIncrementalData() {
        this.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this._CargoTrackingIncrementalStatService.GetCargoTrackingIncrementalData().subscribe((response: ServiceResponse) => {
        this.CurrentSession.StopBusyIndicator();
          var mm: ServiceResponse = response;
          if (!mm.HasError) {
              this._CargoTrackingIncrementalArgs = mm.Result;
          }
          else {
           this.ValidationErrorsList.push(mm.ErrorsArray[0]);
          }
    
        });
    }
    ReloadScreen() {
             
            this.onQueryChangeEvent.emit({ Filters: this.Main_Filter }); // refresh grid
    }
    ValidateDate(fieldName: any) {
        var date1 = DateTool.GetDateFromDate(this.FromDate, true);
        var date2 = DateTool.GetDateFromDate(this.ToDate, true);
        var diffDays =0;
        if(this.FromDate && this.ToDate){
            diffDays = date2.getDate() - date1.getDate(); 
        }
        if (DateTool.GetDateFromDate(this.FromDate, true) > DateTool.GetDateFromDate(this.ToDate, true)) {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, "''To date'' field must be greater than or equal to ''From date'' field");
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, "''From date'' field must be less than or equal to ''To date'' field");
        }
        else if (diffDays > 7){
            this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, "The date range should be less than or equal to 7 days.");
            this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, "The date range should be less than or equal to 7 days.");
        }
         else {
 
            this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, null);
            this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, null);
            this.ReloadScreen();
        }
           
        
    }
    InitializeDate(){
  
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var Day = new Date().getDate();
        this.ToDate = this.SetDate(Year, month, Day);
        this.FromDate = this.SetDate(Year, month, Day);
        this.FromDate.setUTCDate(this.ToDate.getDate() - 7);
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
    public FromDateText:string="Date From: ";
    public ToDateText:string="To: ";
    public WaitingSinceText:string = " waiting since ";
    public Parag1:string ="Incremental last run on: ";
    public Parag2:string ="Records wating for incremental update: ";
    public Parag3:string ="Oldest update still waiting: ";
    public Parag4:string ="Total records updated in last 10 minutes: ";

    RefreshButtonClicked() {
        this.GetCargoTrackingIncrementalData();
        this.ReloadScreen();
    }
}

export class CargoTrackingIncrementalArgs {
    public  RecordsWating :number;
    public  IncrementalLastRun:Date;
    public  OldestUpdateStillWaiting:string;
    public  TotalUpdatedLast10Minutes:number;
    public  DateOfOldestUpdateStillWaiting:Date;
}



 
