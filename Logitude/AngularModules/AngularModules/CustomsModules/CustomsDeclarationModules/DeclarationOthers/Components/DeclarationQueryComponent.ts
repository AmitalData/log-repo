import {Component, Output, EventEmitter}  from '@angular/core';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../../../Infrastructure/Services/EntityListService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {DeclarationExtendedListService} from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import {DeclarationPM} from '../../../../Customs/EntityPMs/DeclarationPM';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';


@Component({
    moduleId: module.id,
    templateUrl: './DeclarationQueryComponent.html',
})


export class DeclarationQueryComponent extends BaseComponent {

    entityListService: EntityListService = new EntityListService();
    DataContext: any = this;
    @Output() onQueryChangeEvent = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    IsDisplayOnly: any; // html component requires this property. AOT

    declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    EntityPM: DeclarationPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.CurrentSession.StartBusyIndicatorLoading();
        
        this.BuildColumns();
        //this.onQueryChangeEvent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }

    SetWindowArgs(args) {
        this.EntityPM = args.DeclarationPM;
        this.CustomerId = this.EntityPM.CustomerId;

        var today = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1)
        this.TaxationDateTime = new Date(lastmonth);
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
    }
    
    private customerId: string;
    get CustomerId() { return this.customerId; }
    set CustomerId(value: string) {
        if (this.customerId != value) {
            this.customerId = value;
          
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }


    private departmentId: string;
    get DepartmentId() { return this.departmentId; }
    set DepartmentId(value: string) {
        if (this.departmentId != value) {
            this.departmentId = value;

            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    private declarationOfficeCode: string;
    get DeclarationOfficeCode() { return this.declarationOfficeCode; }
    set DeclarationOfficeCode(value: string) {
        if (this.declarationOfficeCode != value) {
            this.declarationOfficeCode = value;

            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }
    
    private paymentDate: Date;
    get PaymentDate() { return this.paymentDate; }
    set PaymentDate(value: Date) {
        if (this.paymentDate != value) {
            this.paymentDate = value;
           
           
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    private taxationDateTime: Date;
    get TaxationDateTime() { return this.taxationDateTime; }
    set TaxationDateTime(value: Date) {
        if (this.taxationDateTime != value) {
           
            this.taxationDateTime = value;
           
           
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    searchValue: string;
    Search(value: string) {
        this.searchValue = value;
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });

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
        var filters = new ApiQueryFilters();

        filters.GetAll = false;
        filters.GetCount = true;
        filters.PageSize = 20;

        if (!AppTool.IsNullOrEmpty(this.TaxationDateTime)) {


            var todayDate = new Date();
            var fromDate = new Date(this.TaxationDateTime.getFullYear(), this.TaxationDateTime.getMonth(), this.TaxationDateTime.getDate(), 0, 0, 0);
            var todayDate = new Date(todayDate.setHours(23, 59, 59, 59));

            console.log("TaxationDateTime: ", fromDate, todayDate);
            filters.addAdditionalFilter("TaxationDateTime", fromDate, todayDate, null, "Between", false, false, false, "Date", false);
        }

        if (!AppTool.IsNullOrEmpty(this.PaymentDate)) {
            var fromDate = new Date();
            fromDate.setUTCDate(this.PaymentDate.getUTCDate());
            fromDate.setUTCMonth(this.PaymentDate.getUTCMonth());
            fromDate.setUTCFullYear(this.PaymentDate.getUTCFullYear());
            fromDate.setHours(0);
            fromDate.setMinutes(0);
            fromDate.setSeconds(0);
            fromDate.setMilliseconds(0);

            var filterValue: Date = new Date();
            filterValue.setUTCDate(this.PaymentDate.getUTCDate());
            filterValue.setUTCMonth(this.PaymentDate.getUTCMonth());
            filterValue.setUTCFullYear(this.PaymentDate.getUTCFullYear());
            filterValue.setHours(23);
            filterValue.setMinutes(59);

            filters.addAdditionalFilter("PaymentDate", fromDate, filterValue, null, "Between", false, false, false, "Date", false);
        }
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {

            filters.addAdditionalFilter("CustomerId", this.CustomerId, null, null, "Equals", false, false, false, "string");

        }

        if (!AppTool.IsNullOrEmpty(this.DepartmentId)) {

            filters.addAdditionalFilter("DepartmentId", this.DepartmentId, null, null, "Equals", false, false, false, "string");

        }
        if (!AppTool.IsNullOrEmpty(this.DeclarationOfficeCode)) {

            filters.addAdditionalFilter("DeclarationOfficeCode", this.DeclarationOfficeCode, null, null, "Equals", false, false, false, "string");

        }

        if (!AppTool.IsNullOrEmpty(this.searchValue)) {

            filters.addAdditionalFilter("SearchFields", this.searchValue, null, null, "Contains", false, false, false, "string");

        }
        filters.addAdditionalFilter("CustomFileNo", this.EntityPM.CustomFileNo, null, null, "NotEqual", false, false, false, "string");
        this.CurrentSession.StopBusyIndicator();
        return this.entityListService.getByFilters("Customs.Declaration",filters);
    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'TaxationDateTime',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.TaxationDateTime'),
            Styles: { width: '120px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.CustomFileNo'),
            Styles: { width: '80px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',

            IsCustomTemplate: true

        });
        this.columns.push({
            FieldName: 'CustomerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.CustomerName'),
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true

        });
        this.columns.push({
            FieldName: 'DeclarationOfficeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationOfficeName'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',

        });
        this.columns.push({
            FieldName: 'DeclarationNumber',
            DataTypeCode: 'string',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationNumber'),
            Styles: { width: '105px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'ProcedureCurrentName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.ProcedureCurrentName'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
        });

        this.columns.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationStatusTypeName'),
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',

        });

       

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    OkButtonClicked() {
        if (this.SelectedRow) {
            let confirm = new ConfirmWindow();
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    this.declarationExtendedListService.GetSupplierInvoiceItemsCount(this.SelectedRow.Id).subscribe((response: any) => {
                        if (response) {
                            if (!response.HasError) {

                                if (response.Result) {
                                    var msg = new MessageWindow();

                                    msg.Show("ההצהרה מכילה יותר מ-1000 פריטים, לא ניתן להעתיק אותה");


                                }
                                else {
                                    this.CurrentSession.StartBusyIndicator("");
                                    this.declarationExtendedListService
                                        .PutCopyDeclaration(this.SelectedRow.Id, this.EntityPM.Id, this.EntityPM.Tenant)
                                        .subscribe((response: any) => {

                                            if (response) {
                                                if (!response.HasError) {


                                                    //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                                    this.CurrentSession.StopBusyIndicator();
                                                    this.CurrentSession.CloseCurrentWindowEmit(null);
                                                }
                                            }
                                        });
                                }

                            }
                        }
                    });



                }
            });
            confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CopyData") + " " + this.SelectedRow.CustomFileNo + " " + TextCodeTranslator.Translate("Customs.Declaration.O.ToFile") + " " + this.EntityPM.CustomFileNo + " ?");
        } else {
            let msg = new MessageWindow();
            msg.RTL = true;
            msg.Show("נא לבחר הצהרה");
        }
      
    }


    public SelectedRow: any = null;

    OnRowSelected(item) { 
        this.SelectedRow = item.rowData; 
    }
}
