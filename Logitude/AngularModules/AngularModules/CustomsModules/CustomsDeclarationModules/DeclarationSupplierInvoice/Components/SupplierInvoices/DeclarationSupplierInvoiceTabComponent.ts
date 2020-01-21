import {Component, OnInit, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {SupplierInvoiceExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {SupplierInvoiceList} from '../../../../../Customs/EntityLists/SupplierInvoiceList';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {SupplierInvoiceItemPM} from '../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import {SupplierInvoicePMService} from '../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {EntityListService} from '../../../../../Infrastructure/Services/EntityListService';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {CustomsDocumentPointerService} from '../../../../../Customs/Services/Others/CustomsDocumentPointerService';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {DeclarationPMService} from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
declare var window: any;
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';


@Component({
    selector: 'DeclarationSupplierInvoiceTabComponent',
    moduleId: module.id,
    templateUrl: './DeclarationSupplierInvoiceTabComponent.html',
    providers: [DeclarationExtendedListService]
 })

export class DeclarationSupplierInvoiceTabComponent extends BaseComponent implements OnInit{

    public EntityPM: DeclarationPM;
    public ObjectTableName: string = null;
    public DataContext: any = this;
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public declarationPMService: DeclarationPMService = new DeclarationPMService();

    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService;
    customsDocumentPointerService: CustomsDocumentPointerService;
    supplierInvoicePMService: SupplierInvoicePMService;
    public ItemsSource: ObservableCollection;
    public Invoices: SupplierInvoicePM[];
    public InvoiceItems: ObservableCollection;
    public itemsList: SupplierInvoiceItemPM[];
    public IsVisible = false;
    private _entityListService: EntityListService;
    public IsDisplayOnly: boolean = false;
    public ShowStorageStatusMessage: boolean = false;
    public DisplayOnlyMessage: string = "";
    LayoutDirection: string = 'ltr';
    NumberOfLoadedItems: number = 500;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    IsDisplayMessage: boolean;
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef, public declarationExtendedListService: DeclarationExtendedListService) {
        super();
       // this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe(response => {
        this.customsDocumentPointerService = new CustomsDocumentPointerService();
            this.ItemsSource = new ObservableCollection([]);
            this.InvoiceItems = new ObservableCollection([]);
            
            this.Listen();
            this._entityListService = new EntityListService();
            //this.EntityPM = this.entityArgs.EntityPM;
            this.supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
            //this.supplierInvoicePMService = new SupplierInvoicePMService();
            //this.ObjectTableName = this.entityArgs.ObjectTableName;
            //this.getSupplierInvoices();
            
        //});
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
        this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe(response => {
        this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsMod").subscribe(response => {
                this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemProcesType").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsConDeclar").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsDescript").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsSerialNum").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsProdIdent").subscribe(response => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsLevy").subscribe(response => {
                this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe(response => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(response => {
     

                    

                this.IsVisible = true;
                //this.ItemsSource = new ObservableCollection([]);
                //this.InvoiceItems = new ObservableCollection([]);
                this.BuildColumns();

                
                this.supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
                this.supplierInvoicePMService = new SupplierInvoicePMService();
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                //this.EntityPM = this.entityArgs.EntityPM;
                //this.getSupplierInvoices();
                //this.DisplayOnlyCheck();
                this.ReloadMyScreen();

                
            });
            });
            });
            });
            });
            });
        });
        });
        });
            });
            });
        });
    }

    IsAccumulatedMessageText: string;
    
    public CurrentEditComponentId: string;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.ReloadMyScreen();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {


                    if (isLoadSuccess) {
                        setTimeout(() => {
                            this.ReloadMyScreen();
                        });
                    }

                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEIN") {

                            this.ReloadMyScreen();
                        }
                    }
                })
            );
        }
    }
    ReloadMyScreen() { ///DSV - After Sending to Customs - Enter SUpplierInvoice and Getting Optimistic Concurancy error"
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.getSupplierInvoices();
        this.DisplayOnlyCheck();
    }

    public columns: any[] = null;

    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName:  'SequenceNumeric',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.SequenceNumeric"),
            Styles: { width: '50px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemCode"), 
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ClassificationCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ClassificationCode"), 
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
       
        this.columns.push({
            FieldName: 'TradeAgreementName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.TradeAgreementName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'InvoiceQuantity',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.InvoiceQuantity"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'DeclarationSupplierInvoiceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationSupplierInvoiceListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemPrice',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemPrice"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'DeclarationSupplierInvoiceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationSupplierInvoiceListTemplate',
            IsCustomTemplate: true
        });
      
        this.columns.push({
            FieldName: 'OriginCountryName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.OriginCountryName"), 
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });

       
    }

    ViewInitCompleted($event) {
        this.SelectedRow = this.ItemsSource.Collection[0];
        this.OnRowSelected(this.SelectedRow);
    }

    getSupplierInvoices() {

        //this.ItemsSource.Clear();
        this.Invoices = this.EntityPM.SupplierInvoices;
        this.ItemsSource.InsertCollection(this.Invoices, true);
        //this.ItemsSource.Collection = this.Invoices;
        //this.ItemsSource.Length = this.Invoices.length;

        //Select last selected row, or first
        if (this.SelectedRowB4Refresh) {

            if (this.SelectedRowB4Refresh == this.lastDeletedItem) { //deleted item
                this.OnRowSelected(this.Invoices[0]);
            } else {
                var selectedInvoiceKey = this.SelectedRowB4Refresh.InvoiceCounterKey;
                var selectedInvoice = this.Invoices.filter(d => d.InvoiceCounterKey == selectedInvoiceKey)[0];
                this.OnRowSelected(selectedInvoice);
            }

        }
        else
            this.OnRowSelected(this.Invoices[0]);
        
        
    }


    public SelectedRow: SupplierInvoicePM = null;
    public SelectedRowB4Refresh: SupplierInvoicePM = null;
    OnRowSelected(itemComponent: SupplierInvoicePM) {
        
        this.SelectedRow = itemComponent;
        this.SelectedRowB4Refresh = this.SelectedRow;
        this.filterAgrs = new ApiQueryFilters();

        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        if (itemComponent) {
            if (itemComponent.IsAccumalated) {
                this.IsAccumulated = true;
                this.IsAccumulatedMessageText = TextCodeTranslator.Translate("Customs.Declaration.O.IsAccumulated");
            }
            else {
                this.IsAccumulated = false;
            }
        }
        //this.DataSource.pageSize = 10;
        //this.DataSource.rowCount = 10;
        //this.DataSource.sortingDir = "Ascending";
        //this.DataSource.getRows(0, 10, "SequenceNumeric", "Ascending", true, null, null);
        

   
    }

   


    DataSource = {
       
        pageSize: 10,
        rowCount: null,
        sortingCol: "SequenceNumeric",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
           
                var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        
     

            if (filters == null) {
                filters = new ApiQueryFilters();
            }

            filters.PageSize = take;
            filters.PageIndex = skip;
            filters.GetAll = false;
            filters.GetCount = true;
            filters.SortBy = "SequenceNumeric";
            filters.SortDirection = "Ascending";
            if (this.SelectedRow) {

                if (this.SelectedRow.IsAccumalated) {
                    filters.addAdditionalFilter("IsParent", true, null, null, "Equals", false, false, false, "boolean");

                }
                filters.addAdditionalFilter("DeclarationId", this.SelectedRow.DeclarationId, null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("CounterKey", this.SelectedRow.InvoiceCounterKey, null, null, "Equals", false, false, false, "number");

            }
            else {
                filters.addAdditionalFilter("DeclarationId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
      //          filters.addAdditionalFilter("CounterKey", null, null, null, "Equals", false, false, false, "number");

            }
            return this._entityListService.getExtendedByFilters("Customs.SupplierInvoiceItem", filters);//this.ledgerTransactionListExtendedService.getByFilters(filters);

           
        }
      

    filterAgrs: ApiQueryFilters; 

    private timerToken: any;
    IsPrimarySupplierInvoiceChecked(checked: boolean, item: SupplierInvoicePM) {
   
        if (!checked) {
            if (this.EntityPM.PrimaryInvoiceCounterKey == item.InvoiceCounterKey.toString()) {
                this.timerToken = setTimeout(() => {

                    var invoice = this.EntityPM.SupplierInvoices.filter(d => d.InvoiceCounterKey == item.InvoiceCounterKey)[0];
                    invoice.IsPrimarySupplierInvoice = true;
                }, 1);
               
            }
        }
        else {
            if (this.EntityPM.PrimaryInvoiceCounterKey != item.InvoiceCounterKey.toString()) {
                this.EntityPM.PrimaryInvoiceCounterKey = item.InvoiceCounterKey.toString();
                item.IsPrimarySupplierInvoice = true;
                for (var i = 0; i < this.EntityPM.SupplierInvoices.length; i++) {
             
                    if (this.EntityPM.SupplierInvoices[i].InvoiceCounterKey.toString() != this.EntityPM.PrimaryInvoiceCounterKey) {

                        this.EntityPM.SupplierInvoices[i].IsPrimarySupplierInvoice = false;
                    }
                    else {
                        this.EntityPM.SupplierInvoices[i].IsPrimarySupplierInvoice = true;
                    }

                }

               
       
            }
        }
        
    }

    EditButtonClicked(item: SupplierInvoicePM) {

      

        var errors = [];
        Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", errors);

        for (let item of this.EntityPM.Consignments) {
            for (let line of item.ConsignmentPackages) {
                if (line.MarksNumbers == null && line.PackageMeasureQualifierCode == null && line.PackageQuantity == null && line.PackageTypeCode == null && line.GrossMassMeasure == null) {
                   var errorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.EmptyConsignmentPackage");
                    if (!AppTool.IsNullOrEmpty(errorMessage)) {
                        errors.push(errorMessage);

                    }
                }

            }
        }

      
        if (errors.length > 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }


        else {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicator("");
                this.declarationPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                    var declaration = response.Result;
                    this.CurrentSession.StopBusyIndicator();
                    if (!AppTool.IsNullOrEmpty(declaration)) {
                        if (!AppTool.IsNullOrEmpty(item)) {
                            this.EditInvoice(item);

                        }
                    }

                });
            }
            else {
                this.EditInvoice(item);
            }

            
        }

    
             
    }

    EditInvoice(item: SupplierInvoicePM) {
        this.CurrentSession.StartBusyIndicator("");
        var supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();

        this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.Id, item.InvoiceCounterKey, 0, this.NumberOfLoadedItems, "parent").subscribe(response => {

                var windowArgs: any = {};
                windowArgs.EntityPM = response.Result;
                windowArgs.declarationPM = this.EntityPM;
                windowArgs.NumberOfLoadedItems = this.NumberOfLoadedItems;
                var windowTitle = "Supplier Invoice";

            var logWindow = new LogitudeWindow();
            logWindow.Width = 1017;// this changed By Rabaia for Task No. 54930; Dont change it back before calling me. //995; // don't change this width!
            logWindow.Height = 600;

                if (!AppTool.IsNullOrEmpty(item.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                    windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + item.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

                }
                else if (AppTool.IsNullOrEmpty(item.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                    windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;

                }
                else if (!AppTool.IsNullOrEmpty(item.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                    windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + item.InvoiceNumber;

                }
                else if (AppTool.IsNullOrEmpty(item.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                    windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                }
                windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                logWindow.ShowCloseButton = false;
                logWindow.WindowArgs = windowArgs;
                this.CD.detach();
                logWindow.WindowClosed.subscribe((event: any) => {
                    if (event != 'cancel') {

                        this.RefreshEntity();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                    else {
                        this.ReloadMyScreen();
                    }
                    this.CD.reattach();

                });
                logWindow.IsHideHeader = true;
          logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                this.CurrentSession.StopBusyIndicator();
            });
        
        }

    DeleteButtonClicked(item: SupplierInvoicePM) {

        var confirmWindow = new ConfirmWindow();
        //confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        //confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        confirmWindow.Width = 300;
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");

        confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteInvoice"));
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.customsDocumentPointerService.GetCheckForPointers(item.DeclarationId, item.InvoiceCounterKey).subscribe((myResponse: ServiceResponse) => {
                    var exist = myResponse;
                    if (myResponse.Result) {
                        var confirmWindow = new ConfirmWindow();

                        //     confirmWindow.DisplayWariningIconImage();
                        confirmWindow.Width = 400;

                        confirmWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                        confirmWindow.Height = 190;
                        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
                        confirmWindow.ShowWarningImage = true;
                        confirmWindow.ShowNoButton
                        confirmWindow.Show(TextCodeTranslator.Translate("Customs.General.O.InvoiceRelatedPoiner"));
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {
                                this.DeleteSelected(item);
                            } else if (confirmWindow.No) {

                            }
                        });
                    }

                    else {
                        this.DeleteSelected(item);
                    }

                });

            } else if (confirmWindow.No) {
             
            }
        });

        
        
}


   
    lastDeletedItem: SupplierInvoicePM;
    DeleteSelected(item: SupplierInvoicePM) {
        this.CurrentSession.StartBusyIndicator("");
        this.lastDeletedItem = item;

        var SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            SaveCompletedEvent.unsubscribe();
            this.supplierInvoiceExtendedPMService.delete(item.DeclarationId, item.InvoiceCounterKey).subscribe((myResponse: ServiceResponse) => {

                if (!myResponse.HasError) {
                    this.ItemsSource.Remove(item);
                    //if (Number(this.EntityPM.PrimaryInvoiceCounterKey) == item.InvoiceCounterKey) {
                    //    this.EntityPM.PrimaryInvoiceCounterKey = this.ItemsSource.Collection[0].InvoiceCounterKey;
                    //}

                    //   this.getSupplierInvoices();
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.StopBusyIndicator();

                }


            });
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges();
     
       

    }

    Add() {
        if (this.IsDisplayOnly) return;

        var errors = [];


        Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", errors);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;

        if (errors.length > 0) {
            //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }
        else {

            if (this.EntityPM.IsDirty) {
                this.declarationPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                    var declaration = response.Result;

                    if (!AppTool.IsNullOrEmpty(declaration))
                        this.NewInvoice();
                    else
                        console.log("[!] No response for saving declaration, adding inice aborted.", response);
                });
            } else {
                this.NewInvoice();
            }

           
        }
    }
    accumulationFeature: any;
    NewInvoice() {
        var itemPM = new SupplierInvoicePM();
        itemPM.DeclarationId = this.EntityPM.Id;
        itemPM.Tenant = SessionLocator.Tenant;
        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];
        this.accumulationFeature = FeatureLocator.Features.filter(f => (f.Code == "ACCUMULATION") && f.ObjectTableId == table.Id)[0];
        if (this.accumulationFeature == null) {
            itemPM.AccumalationStateCode = "3";
        }
        else {
            itemPM.AccumalationStateCode = "1";
        }
        itemPM.InvoiceCounterKey = 0;
     

        // [!] I think its not nessecary  !!!
        //var itemList = new SupplierInvoiceList();
        //itemPM.DeclarationId = this.EntityPM.Id;
        //itemPM.Tenant = SessionLocator.Tenant;

        var windowArgs: any = {};

        if (this.EntityPM.SupplierInvoices.length > 0) {
            windowArgs.InsuranceAmountEnabled = true;
            windowArgs.InsuranceCurrencyEnabled = true;
            windowArgs.PercentageEnabled = true;
            itemPM.IsPrimarySupplierInvoice = true;
        } else {

            windowArgs.InsuranceAmountEnabled = false;
            windowArgs.InsuranceCurrencyEnabled = false;
            windowArgs.PercentageEnabled = false;
        }


        // Show Window
        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.NewInvoice");
        windowArgs.EntityPM = itemPM;
        windowArgs.declarationPM = this.EntityPM;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        windowArgs.IsNewEntity = true;
        windowArgs.NumberOfLoadedItems = 0;
        windowArgs.WindowTitle = windowTitle;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1017;// this changed By Rabaia for Task No. 54930; Dont change it back before calling me. //995; // don't change this width!
        logWindow.Height = 600;
      //  logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CD.reattach();
            
            this.RefreshEntity();
            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            this.getSupplierInvoices();
        });
        this.CD.detach();
        logWindow.IsHideHeader = true;
      logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');


            //}
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
      
    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            //this.SetScreenFieldsEditability();
            //DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            if (this.EntityPM.SupplierInvoices != null) {
                for (var i = 0; i < this.EntityPM.SupplierInvoices.length; i++) {
                    this.EntityPM.SupplierInvoices[i].UIProperties.SetEnabled("IsPrimarySupplierInvoice", "Customs.SupplierInvoice", !this.IsDisplayOnly);
                }
            }
            return;
        }
        else if (this.EntityPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
        }
        else {
            this.InitDisplayOnlyMessage();
        }
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe((response: any) => {
            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
            this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (this.EntityPM.StorageStatusCode) {
                this.ShowStorageStatusMessage = true;
                this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
            }
            else {
                this.InitDisplayOnlyMessage();
            }
            if (this.EntityPM.SupplierInvoices != null) {
                for (var i = 0; i < this.EntityPM.SupplierInvoices.length; i++) {
                    this.EntityPM.SupplierInvoices[i].UIProperties.SetEnabled("IsPrimarySupplierInvoice", "Customs.SupplierInvoice", !this.IsDisplayOnly);
                }
            }
        });
    }
    IsAccumulated: boolean = false;
    public SelectedRow2: any = null;
    OnRowSelected2(CurrentRow) {
        this.SelectedRow2 = CurrentRow.rowData;
        
    }

    InitDisplayOnlyMessage() {
        if (this.EntityPM.IsAmendment && (this.EntityPM.AmendmentStatus == "2")) {
            this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.IsAmendment") + ' ' + this.EntityPM.AmendmentStatusName;
            this.IsDisplayMessage = true;
        }
        else if (this.EntityPM.IsAmendment == false) {
            this.declarationExtendedListService.GetDeclarationAmendmentsById(this.EntityPM.Id).subscribe
                (data => {
                    if (data.Result == null || data.Result.length <= 0) return;

                    data.Result = data.Result.sort((obj1, obj2) => {
                        if (obj1.amendmentissueDate > obj2.amendmentissueDate) {
                            return 1;
                        }

                        if (obj1.amendmentissueDate < obj2.amendmentissueDate) {
                            return -1;
                        }

                        return 0;
                    });
                    var temp = false;

                    data.Result.forEach((item) => {
                        if ((temp == false) && (item.AmendmentStatus == "3" || item.AmendmentStatus == "1" || item.AmendmentStatus == "6" || item.AmendmentStatus == "4" || item.AmendmentStatus == "2")) {
                            {
                                this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.ExistsAmendments") + ' ' + item.AmendmentStatusName;
                                this.IsDisplayMessage = true;
                                temp = true;

                            }
                        }

                    });
                    //this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.ExistsAmendments") + ' ' + data.Result[0].AmendmentStatusName;
                    //this.IsDisplayMessage = true;

                }


                );

        }
    }
}



//export class SupplierInvoiceLine extends BaseComponent {
//    public SupplierInvoicePM: SupplierInvoicePM = null;
//    public ObjectTableName = "Customs.SupplierInvoice";
//    public DataContext = this;


//    constructor(private invoice: SupplierInvoicePM,
//        private parent: DeclarationSupplierInvoiceTabComponent) {
//        super();
//        this.EntityPM = this.parent.EntityPM;
//        this.SupplierInvoicePM = invoice;
       
//    }

//    get DeclarationId() { return this.invoice.DeclarationId; }
//    set DeclarationId(value: string) {
//        if (this.invoice.DeclarationId != value) {
//            this.invoice.DeclarationId = value;
//        }
//    }

//    get InvoiceCounterKey() { return this.invoice.InvoiceCounterKey; }
//    set InvoiceCounterKey(value: number) {
//        if (this.invoice.InvoiceCounterKey != value) {
//            this.invoice.InvoiceCounterKey = value;
//        }
//    }

//    get SequenceNumeric() { return this.invoice.SequenceNumeric; }
//    set SequenceNumeric(value: number) {
//        if (this.invoice.SequenceNumeric != value) {
//            this.invoice.SequenceNumeric = value;
//        }
//    }
//    get InvoiceNumber() { return this.invoice.InvoiceNumber; }
//    set InvoiceNumber(value: string) {
//        if (this.invoice.InvoiceNumber != value) {
//            this.invoice.InvoiceNumber = value;
//        }
//    }

//    get IssueDate() { return this.invoice.IssueDate; }
//    set IssueDate(value: Date) {
//        if (this.invoice.IssueDate != value) {
//            this.invoice.IssueDate = value;
//        }
//    }
//    get IncotermCode() { return this.invoice.IncotermCode; }
//    set IncotermCode(value: string) {
//        if (this.invoice.IncotermCode != value) {
//            this.invoice.IncotermCode = value;
//        }
//    }

//    get InvoiceCurrencyTypeCode() { return this.invoice.InvoiceCurrencyTypeCode; }
//    set InvoiceCurrencyTypeCode(value: string) {
//        if (this.invoice.InvoiceCurrencyTypeCode != value) {
//            this.invoice.InvoiceCurrencyTypeCode = value;
//        }
//    }
//    get IssueCountryName() { return this.invoice.IssueCountryName; }
//    set IssueCountryName(value: string) {
//        if (this.invoice.IssueCountryName != value) {
//            this.invoice.IssueCountryName = value;
//        }
//    }

//    get VendorName() { return this.invoice.VendorName; }
//    set VendorName(value: string) {
//        if (this.invoice.VendorName != value) {
//            this.invoice.VendorName = value;
//        }
//    }
//    get PreferenceDocumentTypeName() { return this.invoice.PreferenceDocumentTypeName; }
//    set PreferenceDocumentTypeName(value: string) {
//        if (this.invoice.PreferenceDocumentTypeName != value) {
//            this.invoice.PreferenceDocumentTypeName = value;
//        }
//    }

   

//    get InsruanceCurrencyTypeCode() { return this.invoice.InsruanceCurrencyTypeCode; }
//    set InsruanceCurrencyTypeCode(value: string) {
//        if (this.invoice.InsruanceCurrencyTypeCode != value) {
//            this.invoice.InsruanceCurrencyTypeCode = value;
//        }
//    }

//    get InsuranceAmount() { return this.invoice.InsuranceAmount; }
//    set InsuranceAmount(value: number) {
//        if (this.invoice.InsuranceAmount != value) {
//            this.invoice.InsuranceAmount = value;
//        }
//    }
//    get InvoiceAmount() { return this.invoice.InvoiceAmount; }
//    set InvoiceAmount(value: number) {
//        if (this.invoice.InvoiceAmount != value) {
//            this.invoice.InvoiceAmount = value;
//        }
//    }

//    private isPrimarySupplierInvoice: boolean = false;
//    get IsPrimarySupplierInvoice() { return this.invoice.IsPrimarySupplierInvoice; }
//    set IsPrimarySupplierInvoice(value: boolean) {
//        if (this.invoice.IsPrimarySupplierInvoice != value) {
//            this.invoice.IsPrimarySupplierInvoice = value;
          
//        }
//    }
        
      







//}
