declare var System: any;
declare var window: any;
import { Component, OnInit, EventEmitter, Output, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { PhysicalCheckWebService } from '../../../../../Customs/Services/WebServices/PhysicalCheckWebService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from              '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from    '../../../../../Infrastructure/Utilities/ObservableCollection';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
//import { PhysicalCheckList } from '../../../../../Customs/EntityLists/PhysicalCheckList';
//import { PhysicalCheckPMService } from '../../../../../Customs/Services/StandardPMs/PhysicalCheckPMService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ApiQueryFilters } from     '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from   '../../../../../Infrastructure/Services/EntityListService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({

    moduleId: module.id,
    templateUrl: './DeclarationTaxesTabComponent.html',
})

export class DeclarationTaxesTabComponent implements OnInit, OnDestroy {
    public EntityPM: DeclarationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    private _entityListService: EntityListService;

    public CurrentEditComponentId: string;
    //public physicalCheckList: PhysicalCheckList[] = [];

    private _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService;

    //private physicalCheckPMService: PhysicalCheckPMService = new PhysicalCheckPMService;
    IsVisible: boolean = false;
    CurrencyRatesView: ObservableCollection;
    TaxesObslist: ObservableCollection;


    TaxBaseAmountTotal: number = 0;
    TaxToPayTotal: number = 0;
    FooterMethods: number = 0;
    TotalAmountTotal: number = 0;
    DeferredTaxAmountTotal: number = 0;
    IsCloseButtonVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {

        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsTax").subscribe(response => {

                    this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsExchangeRate").subscribe(response => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationTax").subscribe(response => {
                            this.EntityPM = this.entityArgs.EntityPM;
                            this.ObjectTableName = this.entityArgs.ObjectTableName;
                            this._entityListService = new EntityListService();
                          
                            this.Listen();
                            this.CurrencyRatesView = new ObservableCollection([]);
                            this.TaxesObslist = new ObservableCollection([]);
                            
                            this.OnEditTabSelected();
                            this.BuildColumns();
                        });
                    });
                });
            });
        });
    }
    ngOnDestroy() {
        console.log("DeclarationTaxesTabComponent:ngOnDestroy");
        this.entityArgs = null;
        this.CD = null;
    }
    SetWindowArgs(args: any) {
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsTax").subscribe(response => {

                    this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsExchangeRate").subscribe(response => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationTax").subscribe(response => {
                            this.EntityPM = args.EntityPM;
                            this.EntityId = this.EntityPM.Id;
                            this.CIFValue = this.EntityPM.CIFValue;
                            this.ObjectTableName = this.entityArgs.ObjectTableName;
                            this._entityListService = new EntityListService();
                            this.Listen();
                            this.CurrencyRatesView = new ObservableCollection([]);
                            this.TaxesObslist = new ObservableCollection([]);
                            this.IsCloseButtonVisibile = true;
                            
                            this.OnEditTabSelected();
                            this.BuildColumns();
                            this.CD.detectChanges();
                        });
                    });
                });
            });
        });
    }


    ngOnInit() {
        if (this.entityArgs.EntityPM) {
            this.EntityPM = this.entityArgs.EntityPM;
        }
     
    }
    EntityId: string;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.OnEditTabSelected();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DETX") {
                            this.OnEditTabSelected();
                        }
                    }
                })
            );
        }
    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];          
        this.columns.push({
            FieldName: 'ClassificationCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ClassificationCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'InvoiceNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoice.F.InvoiceNumber"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });


        this.columns.push({
            FieldName: 'TaxTypeName',//'TaxTypeCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TaxTypeCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });          
        this.columns.push({
            FieldName: 'TradeAgreementTypeCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TradeAgreementTypeCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });    

        this.columns.push({
            FieldName: 'TaxBaseAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TaxBaseAmount"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            //<TextBlock VerticalAlignment= "Center" Text= "{Binding TaxBaseAmount,StringFormat=\{0:N2\}}" />
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
        }); 

        this.columns.push({
            FieldName: 'TaxRate',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TaxRate"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
            IsCustomTemplate: true
        });   
        this.columns.push({
            FieldName: 'TaxToPay',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TaxAmount"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',

            IsCustomTemplate: true
        }); 

        this.columns.push({
            FieldName: 'TaxAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.DeclarationTax.F.TaxToPay"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',

            IsCustomTemplate: true
        }); 

        this.columns.push({
            FieldName: 'DeferedTaxAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.DeferedTaxAmount"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',

            IsCustomTemplate: true
        }); 

        this.columns.push({
            FieldName: 'EditButton',
            DataTypeCode: 'String',
            Display: '',// TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.DeferedTaxAmount"),
            Styles: { width: '30px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
        });

    }
    GetSupplierInvoiceCurrencyAsync() {
        this.CurrencyRatesView.Clear();
        this._DeclarationExtendedListService.GetCurrenciesCodesForDeclaration(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe(
            (res) => {

                let aSupplierInvoiceCurrency = res.Result as SupplierInvoiceCurrency[];
                this.CurrencyRatesView.InsertCollection(aSupplierInvoiceCurrency);
                this.IsVisible = true;
            }
            );
    }

    public OnEditTabSelected() {
        //if (tabCode == "DETX") {
        if (this.EntityPM != null) {
        this.ViewInitCompleted({});
        this.TaxesObslist.Clear();
     
            this.TaxesObslist.InsertCollection(this.EntityPM.DeclarationTaxes);
            this.GetSupplierInvoiceCurrencyAsync();
            this.RefreshProperties();
            this.ReloadTaxes();


            //return;

            //}

            this.BuildTaxesObsList();
            this.RefreshProperties();
        }
            //this.loaded = true;
        //}
    }
    BuildTaxesObsList() {
        this.DeferredTaxAmountTotal =this.TotalAmountTotal =this.TaxToPayTotal =this.TaxBaseAmountTotal = 0;
        this.TaxesObslist.Collection.forEach(
            (item) => {
                this.TaxBaseAmountTotal += item.TaxBaseAmount;
                this.TaxToPayTotal += item.TaxToPay;
                this.TotalAmountTotal += item.TotalAmount;
                this.DeferredTaxAmountTotal += item.DeferredTaxAmount
            }
        );
        let headerH = 27;
        let rowH = 26;
        //if ((this.TaxesObslist.Length * 30) + 30 < 123) {
        //    this.FooterMethods = (this.TaxesObslist.Length * 30) + 30;
        //}
        //else {
        //    this.FooterMethods = 123;
        //}
        //this.FooterMethods += 10;
        let top: number = headerH + (this.TaxesObslist.Length * 26) + 2;
        this.FooterMethods = top;
        console.log(this.FooterMethods);
        
    }
    ShowErrorMessage: boolean = false;
    RefreshProperties() {
        this.ShowErrorMessage = false;
        if (this.EntityPM.IsChanged && this.EntityPM.DeclarationTaxes.length > 0) {
            console.log("ShowErrorMessage");
            this.ShowErrorMessage = true;
            this.ErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationTaxChanged");
        }
        else {
            //MessageBorderVisibility = Visibility.Collapsed;
            this.ErrorMessage = null;
        }
        if (this.EntityPM.DeclarationTaxes.length > 0) {
            this.ShowColumnFooter = true;
        }
    }
    _IsReloading: boolean = false;
    private ReloadTaxes() {
        if (!this._IsReloading) {
            //this._DeclarationExtendedListService.      
            //GetSingleDeclarationByNumber

            //.GetDeclarationTaxesByDeclarationId
            //LoadOperation op = context.Load(context.GetDeclarationTaxesByDeclarationIdQuery(entityPM.Id, TenantContext.Current.Id));
            //op.Completed += op_Completed;

            this._IsReloading = true;
        }
    }





    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }


    ErrorMessage: string = null;
    ShowColumnFooter: boolean;
    get ErrorMessageNotUsed() {

        if (this.EntityPM.IsChanged && this.EntityPM.DeclarationTaxes.length > 0) {

            return TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationTaxChanged");
        }
        else {
            //MessageBorderVisibility = Visibility.Collapsed;
            return null;
        }
    }
    public get CIFValue() {
        return this.EntityPM == null ? null : this.EntityPM.CIFValue;
    }
    public set CIFValue(value: number) {
        this.EntityPM.CIFValue = value;
    }
    public get DealValueWithoutFactor() { return this.EntityPM == null ? null : this.EntityPM.DealValueWithoutFactor; }
    get TotalTax() { return this.EntityPM == null ? null : this.EntityPM.TotalTax; }
    get PlatformFee() { return this.EntityPM == null ? null : this.EntityPM.PlatformFee; }
    get DealValue() { return this.EntityPM == null ? null : this.EntityPM.DealValue; }
    get LoadingFactor() { return this.EntityPM == null ? null : this.EntityPM.LoadingFactor; }
    


    public SelectedRow2: any = null;
    OnRowSelected2(CurrentRow) {
        this.SelectedRow2 = CurrentRow.rowData;
    }
    public ItemsSource: ObservableCollection;
    ViewInitCompleted($event) {
        //this.SelectedRow = this.ItemsSource.Collection[0];
        //this.OnRowSelected(this if (this.EntityPM) {
            this.filterAgrs = new ApiQueryFilters();

            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
       
    }
    
    DataSource = {

        pageSize: 10,
        rowCount: null,
        sortingCol: "ClassificationCode",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };
    isOk = true;
    ///public SelectedRow: //SupplierInvoicePM 
    any = null;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {



        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "ClassificationCode";
        filters.SortDirection = "Ascending";


        if (this.EntityPM) {
            filters.addAdditionalFilter("DeclarationId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        }

        else {
            filters.addAdditionalFilter("DeclarationId", this.EntityId, null, null, "Equals", false, false, false, "string");
        }
        
        return this._entityListService.getExtendedByFilters("Customs.SupplierInvoiceItemsTax", filters);//this.ledgerTransactionListExtendedService.getByFilters(filters);


    }


    filterAgrs: ApiQueryFilters; 

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }

}
export class SupplierInvoiceCurrency {

    public Id: string;
    public InvoiceCurrencyId: string;
    public ExchangeRate: number;

}
