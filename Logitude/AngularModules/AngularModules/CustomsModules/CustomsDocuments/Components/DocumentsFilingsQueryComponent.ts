import {Component, AfterViewInit, ChangeDetectorRef, OnInit, Input, Output,EventEmitter, ViewChildren, QueryList}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {DocumentsFilingPM}  from '../../../Common/EntityPMs/DocumentsFilingPM';
import {CustomsDocumentPM} from '../../../Customs/EntityPMs/CustomsDocumentPM';
import {CustomsDocumentsTicketPM} from '../../../Customs/EntityPMs/CustomsDocumentsTicketPM';
import {CustomsDocumentPointerPM} from '../../../Customs/EntityPMs/CustomsDocumentPointerPM';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {CustomsDocumentsTicketPMService} from '../../../Customs/Services/StandardPMs/CustomsDocumentsTicketPMService';
import {CustomsDocumentPMService} from '../../../Customs/Services/StandardPMs/CustomsDocumentPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters, FilterItem} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {DocumentsFilingViewsExtService} from '../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService';
import {DocumentsFilingList}  from '../../../Common/EntityLists/DocumentsFilingList';
import { Declaration } from 'typescript';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { CardExtendedPMService } from '../../../Common/Services/ExtendedPMs/CardExtendedPMService';
import { MultiSelectLOVComponent } from '../../../Infrastructure/Components/LogitudeComponents/MultiSelectLOVComponent';

@Component({
    providers: [CardExtendedPMService], 
    templateUrl: './DocumentsFilingsQueryComponent.html',
})

export class DocumentsFilingsQueryComponent extends BaseComponent {
    //***********************properties*************************//
    DataContext = this;
  IsDisplayOnly: boolean = false;
    public AllowPointerEvents: any = 'all';
   searchOrExportFile:string='שדות חיפוש:';
   SearchText:string;
   @ViewChildren(MultiSelectLOVComponent)
    public myViewChildrenMultiSelectLOVComponent: QueryList<MultiSelectLOVComponent> = null;


    private documentTypeId: string;
    get DocumentTypeId() { return this.documentTypeId; }
    set DocumentTypeId(value: string) {
        if (this.documentTypeId != value) {
            this.documentTypeId = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.DocumentTypeIdFilter = new FilterItem("DocumentTypeId", value, null, null, "Equals", false, false, false, "string", false);
            } else {
                this.DocumentTypeIdFilter = null
            }
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }


    _CustomersList: any[] = [];
    get CustomersList() { return this._CustomersList; }
    set CustomersList(value) {
        if (this._CustomersList != value) {
            this._CustomersList = value;
        }
    }

    private fromCreateDate: Date;
    get FromCreateDate() { return this.fromCreateDate; }
    set FromCreateDate(value: Date) {
        if (this.fromCreateDate != value) {
            this.fromCreateDate = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                if (this.ToCreateDate) {
                    var filterValue: Date = new Date();
                    filterValue.setUTCDate(this.ToCreateDate.getUTCDate());
                    filterValue.setUTCMonth(this.ToCreateDate.getUTCMonth());
                    filterValue.setUTCFullYear(this.ToCreateDate.getUTCFullYear());
                    filterValue.setHours(23);
                    filterValue.setMinutes(59);
                    this.FromCreateDateFilter = new FilterItem("CreateDate", value, this.ToCreateDate, null, "Between", false, false, false, "datetime", false);
                }
                else {
                    this.FromCreateDateFilter = new FilterItem("CreateDate", value, null, null, "GreaterThanOrEqual", false, false, false, "datetime", false);
                }
            } else {
                this.FromCreateDateFilter = null
            }
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }


    SelectedValueChangedEmit() {
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });

    }

    private customsDocId: string;
    get CustomsDocId() { return this.customsDocId; }
    set CustomsDocId(value: string) {
        if (this.customsDocId != value) {
            this.customsDocId = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.CustomsDocIdFilter = new FilterItem("CustomsDocId", value, null, null, "StartsWith", false, false, false, "string", false);
            } else {
                this.CustomsDocIdFilter = null
            }
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    private toCreateDate: Date;
    get ToCreateDate() { return this.toCreateDate; }
    set ToCreateDate(value: Date) {
        if (this.toCreateDate != value) {
            this.toCreateDate = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                var filterValue: Date = new Date();
                filterValue.setUTCDate(value.getUTCDate());
                filterValue.setUTCMonth(value.getUTCMonth());
                filterValue.setUTCFullYear(value.getUTCFullYear());
                filterValue.setHours(23);
                filterValue.setMinutes(59);

                if (this.FromCreateDate) {
                    this.FromCreateDateFilter = new FilterItem("CreateDate", this.FromCreateDate, filterValue, null, "Between", false, false, false, "datetime", false);
                }
                else {
                    this.ToCreateDateFilter = new FilterItem("CreateDate", filterValue, null, null, "LessThanOrEqual", false, false, false, "datetime", false);
                }
            } else {
                this.ToCreateDateFilter = null
            }
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }
    DocumentTypeIdFilter: FilterItem;
    FromCreateDateFilter: FilterItem;
    CustomsDocIdFilter: FilterItem;
    ToCreateDateFilter: FilterItem;
    SearchFieldsFilter: FilterItem;
    @Output() onQueryChangeEvent = new EventEmitter();
    entityListService: EntityListService;
    preventSelect: boolean = false;
    SelectedItem: any;
    declarationPM: DeclarationPM;
    isLoad: boolean = false;
    

    //**********************************************************//
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public cardExtendedPMService: CardExtendedPMService, private _CD: ChangeDetectorRef) {
        super();
        this.BuildColumns();
        this.CurrentSession.SubscriptionAdd(
            this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
                if (res == "document") {
                    this.preventSelect = true;
                }
            })
        );

    }

    SetWindowArgs(windowArgs) {
        
        this.declarationPM = windowArgs;
       
        if (this.declarationPM != null && this.declarationPM.Direction == 'E' && (!AppTool.IsNullOrEmpty(this.declarationPM.ImporterCode) && !windowArgs.IsClose)) {

            this.cardExtendedPMService.GetAllCardsByVatNumber(this.declarationPM.ImporterCode).subscribe(data => {
                this.CustomersList = data.Result;

                this.myViewChildrenMultiSelectLOVComponent.first.Invalidate();
                this.myViewChildrenMultiSelectLOVComponent.last.Invalidate();

                this._CD.detectChanges();
                this.isLoad = true;


            });


        }
        else {

            this.isLoad = true;
        }

        this.entityListService = new EntityListService();

        if(this.declarationPM != null && this.declarationPM.Direction == 'E' && windowArgs.IsClose) {
            this.SearchText = this.declarationPM?.ExportFile;
            this.SearchFieldsFilter = new FilterItem("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string", false);
        }
        this.searchOrExportFile=this.declarationPM?.Direction == 'E'?'חיפוש /תיק יצוא:':this.searchOrExportFile;
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit(this.SelectedItem.Id);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    private timerToken: any;
    TextChanged(searchtext) {
        if (searchtext != null || searchtext != undefined) {

            this.timerToken = setTimeout(() => {
                this.SearchFieldsFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
            }, 700);

        } else {
            this.SearchFieldsFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
        }
    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        //sortingCol: "CreateDateTime",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

  getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
    this.AllowPointerEvents = 'none';
        var filters = new ApiQueryFilters;
        var hasFileFilter = new FilterItem("HasFile", true, null, null, "Equals", false, false, false, "string", false);
        filters.AdditionalFilters.push(hasFileFilter);
        if (this.CustomsDocIdFilter) {
            filters.AdditionalFilters.push(this.CustomsDocIdFilter);
        }
        if (this.DocumentTypeIdFilter) {
            filters.AdditionalFilters.push(this.DocumentTypeIdFilter);
        }
        if (this.FromCreateDateFilter) {
            filters.AdditionalFilters.push(this.FromCreateDateFilter);
        }
        if (this.ToCreateDateFilter) {
            filters.AdditionalFilters.push(this.ToCreateDateFilter);
        }
        if (this.SearchFieldsFilter) {
            filters.AdditionalFilters.push(this.SearchFieldsFilter);
        }

       if (this.CustomersList) {

          var CustomersListString = "";

          if (this.CustomersList.length > 0) {

              this.CustomersList.forEach(item => { CustomersListString += item["Code"] + ","; });//Id: "1-3697"
              CustomersListString = CustomersListString.slice(0, -1); // trim last comma

              // filters.AdditionalFilters.push(this.SearchFieldsFilter);
              filters.addAdditionalFilter("ExternalEntityReference", CustomersListString, null, null, "InListExact", false, false, false, "string", this.CustomersList.length == 0);

          }
      }



        filters.PageSize = 30;
        filters.PageIndex = 0; // decremented 1 in the service
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;

        //filters.addAdditionalFilter("AccountingDate", true, null, null, "Between", false, false, false, "datetime");
        return new Promise((resolve, reject) => {
            var service: DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService();
            resolve(service.getByFilters(filters));
        });
        //return this.entityListService.getByFilters("DocumentsFiling",filters);//this.ledgerTransactionListExtendedService.getByFilters(filters);
    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'Extension',
            DataTypeCode: 'text',
            Display: '',
            Styles: { width: '33px'},
            HtmlListComponentName: 'DocumentsFilingTemplateComponent',
            HtmlListComponentUrl: './Customs/Components/Templates/DocumentsFilingTemplateComponent',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Extension'
        });
        this.columns.push({
            FieldName: 'Code',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('DocumentsFiling.F.Code'),
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Code'
           
        });
        this.columns.push({
            FieldName: 'DocumentTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.CustomsDocument.CH.NameListLable'),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'DocumentTypeName'

        });
        this.columns.push({
            FieldName: 'Description',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('DocumentsFiling.F.Description'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            ServerSideSortable: true,
            SortByName: 'Description'

        });
        this.columns.push({
            FieldName: 'CreateDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate('DocumentsFiling.F.CreateDate'),
            Styles: { width: '105px' },
            HtmlListComponentName: 'DocumentsFilingTemplateComponent',
            HtmlListComponentUrl: './Customs/Components/Templates/DocumentsFilingTemplateComponent',
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CreateDate'
        });

        this.columns.push({
            FieldName: 'OwnerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('DocumentsFiling.F.OwnerName'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            ServerSideSortable: true,
            SortByName: 'OwnerName'

        });

        this.columns.push({
            FieldName: 'ExternalEntityReference',
            DataTypeCode: 'String',
            Display: 'מספר ישות',//TextCodeTranslator.Translate('DocumentsFiling.F.ExternalEntityReference'),;
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ExternalEntityReference'


        });

        this.columns.push({
            FieldName: 'CustomsDocId',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.CustomsDocument.F.CustomsDocId'),//TextCodeTranslator.Translate('DocumentsFiling.F.ExternalEntityReference'),;
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CustomsDocId'


        });

    }

    OnRowSelected(item) {
        if (!this.preventSelect) {
            this.SelectedItem = item.rowData;
            this.CurrentSession.CloseCurrentWindowEmit(this.SelectedItem.Id);
        }
        else {
            this.preventSelect = false;
        }
    }

    OnDataLoaded(rows: any[]) {
      this.CurrentSession.StopBusyIndicator();
      this.AllowPointerEvents = 'all';
    }
}
