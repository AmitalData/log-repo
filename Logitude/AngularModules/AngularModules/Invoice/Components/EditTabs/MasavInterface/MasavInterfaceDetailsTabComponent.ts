import {Component,EventEmitter, Output}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { MasavInterfacePM } from 'Invoices/EntityPMs/MasavInterfacePM';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { APPaymentListService } from 'Invoice/Services/StandardLists/APPaymentListService';
import { APPaymentList } from 'Invoice/EntityLists/APPaymentList';
import { APPaymentExtendedService } from 'Invoice/Services/ExtendedPMs/APPaymentExtendedService';

@Component({
    
    templateUrl: './MasavInterfaceDetailsTabComponent.html',
})

export class MasavInterfaceDetailsTabComponent extends BaseComponent {
    public entityPM: MasavInterfacePM = null;
    public columns: any[] = null;
    public accountingPaymentMethodId :string = null;
    public masavInterfaceColumnsReady: EventEmitter<any> = new EventEmitter();
    private entityListService: EntityListService = new EntityListService();
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() menuHeaderchangeevent = new EventEmitter();
    private currentSession = SessionLocator.SelectedSession;
    public changeCheckBoxesState: EventEmitter<any> = new EventEmitter();
    public selectedLines: ObservableCollection = new ObservableCollection([]);
    apPaymentListService : APPaymentListService = new APPaymentListService();
    apPaymentExtendedService :APPaymentExtendedService = new APPaymentExtendedService();
    public markIsChecked: EventEmitter<any> = new EventEmitter();
    public readonly: boolean = false;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
        this.buildColumns(); 
        this.listen();   
        if(this.entityPM.StatusCode === MasavInterfaceStatus.Transmitted)
            this.selectedAll = true;
        
       this.readonly = this.entityPM?.StatusCode ===  MasavInterfaceStatus.Transmitted;
    }
   
    private listen() {
        this.currentSession.SessionEvent.subscribe(s => {
            if (s == "BackFromCard") {
                this.currentSession.StartBusyIndicatorLoading()
                this.refreshButtonClicked();
                this.currentSession.StopBusyIndicator()

            }
            if (s == "CancelMasavInterface") {
                this.currentSession.StartBusyIndicatorSaving()
                this.apPaymentExtendedService.updateMulti(this.selectedLines.Collection.map(c=>c.rowData.Id), null).subscribe((res)=>{
                       this.selectedAll = false  
                       this.currentSession.StopBusyIndicator()
                })
            }
            if (s == "TransmitterMasavInterface") {
                this.currentSession.StartBusyIndicatorSaving()
                this.apPaymentExtendedService.updateMulti(this.selectedLines.Collection.map(c=>c.rowData.Id), this.entityPM.Id).subscribe((res)=>{
                    
                    this.refreshButtonClicked();
                    this.currentSession.StopBusyIndicator()

                })
            }
        });
       this.currentSession.PseventRowSelectEvent.subscribe((res) => {
            this.onRowSelected(res);
        });

    }
    buildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: "Checked",
            DataTypeCode: 'Boolean',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '50px' },
            
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Invoice/Components/Templates/MasavInterfaceListTemplate',            
        });
        this.columns.push({
            FieldName: 'PaymentNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("APPayment.F.PaymentNo"),
            Styles: { width: '120px'},
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Invoice/Components/Templates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true

        });

        this.columns.push({
            FieldName: 'VendorName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("APPayment.F.VendorName"),
            Styles: { width: '180px'},
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Invoice/Components/Templates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });


        this.columns.push({
            FieldName: 'RegisterDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("APPayment.F.RegisterDate"),
            Styles: { width: '120px'},
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Invoice/Components/Templates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

      
        this.columns.push({
            FieldName: 'AmountInLocalCurrency',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("APPayment.F.AmountInLocalCurrency"),
            Styles: { width: '180px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Invoice/Components/Templates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
           
        });

        this.columns.push({
            FieldName: 'ApprovedDateTime',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("APPayment.F.ApprovedDateTime"),
            Styles: { width: '120px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Invoice/Components/Templates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true

        });
        this.columns.push({
            FieldName: "Errors",
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("General.O.ErrorsFound"),
            IsCustomTemplate: true,
            Styles: { width: '600px' },           
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Invoice/Components/Templates/MasavInterfaceListTemplate',            
        });

        
        this.masavInterfaceColumnsReady.emit(this.columns);

    }
    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "ApprovedDateTime",
        sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

      
         filters = this.getFilters();    

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetCount = true;
        filters.GetAll = false;

        if (sortingDir !== "") {
            filters.SortBy = sortingCol;
            filters.SortDirection = sortingDir;
        }
        else {
            filters.SortBy = "ApprovedDateTime";
            filters.SortDirection = "Descending";
        }

        return this.entityListService.getByFilters("APPayment", filters);

    }
    public selectLines(result: any) {        
        this.selectedLines.Clear();
        let Lines = result.map((res: APPaymentList) => ({ rowData: res, IsChecked: true, RowIndex: -1, ById: true }));         
        this.selectedLines.InsertCollection(Lines);
        this.changeCheckBoxesState.emit(Lines);
    }
    onRowSelected(rowData: any) {
        const row = rowData;
        const rowId = row.Id;
        const index = this.selectedLines.Collection.findIndex(c => c.rowData?.Id == row.Id);
        if (index < 0) {
            row.MasavInterfaceId = this.entityPM.Id;
            this.pushLine(row) 
        } 
        else {
            row.MasavInterfaceId = null;
            this.selectedLines.Remove(this.selectedLines.Collection.find(c => c.rowData?.Id == rowId));
        }
    }
    pushLine(rowData: any) {
        
        this.selectedLines.Insert({rowData: rowData, IsChecked: true, RowIndex: null, ById: true});
      
    }
    onDataLoaded() {
        if (this.selectedLines?.Collection && this.selectedLines.Collection.length > 0) {
            this.selectedLines.Collection.forEach(row => {
                
                if (row?.rowData?.IsChecked) {
                    this.pushLine(row?.rowData);
                }
            });
        }
        this.markIsChecked.emit({ SelectedLines: this.selectedLines });
        
        
    }
    refreshButtonClicked() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });       
    }

    public all: boolean;
    public get selectedAll(): boolean {
        return this.all;
    }
    public set selectedAll(value: boolean) {
        this.all = value;
        if (value) {                     
            this.getAllAPPayment();          

        } else {     
            this.selectedLines.Clear();  
            this.refreshButtonClicked();
    
        }
    }
    getAllAPPayment(){  
        this.currentSession.StartBusyIndicatorLoading();     
        var filters = this.getFilters();       
        filters.GetCount = false;
        filters.GetAll = true;      
        filters.SortBy = "ApprovedDateTime";
        filters.SortDirection = "Descending";         
        return this.apPaymentListService.getByFilters(filters).subscribe((res)=>{
            this.selectLines(res.Result);
            this.currentSession.StopBusyIndicator();     
        });
    }
    getFilters(){
        var filters = new ApiQueryFilters;
        if(this.entityPM.StatusCode === MasavInterfaceStatus.Transmitted )
            filters.addAdditionalFilter('MasavInterfaceId', this.entityPM.Id, null, null, 'Equals', false, false, false, 'string');    
        else {
            filters.addAdditionalFilter('MasavInterfaceId', '', null, null, 'IsNull', false, false, false, 'string');
            filters.addAdditionalFilter('StatusCode', 'AD', null, null, 'Equals', false, false, false, 'string');
            filters.addAdditionalFilter("ApprovedDateTime", this.entityPM.FromDate, this.entityPM.ToDate, null, "Between", false, false, false, "number");
            filters.addAdditionalFilter("PaymentMethodCode", "MS", null, null, 'Equals', false, true, false, 'string');
        }    
        return filters;
    }
}
export enum MasavInterfaceStatus {
    Transmitted = 'TR',
    Failed ='FD',
    Draft = 'DR',
    Cancelled ='CN'
  }
