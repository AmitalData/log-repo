import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { SupplierInvoiceExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { SupplierInvoicePM } from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { SupplierInvoiceList } from '../../../../../Customs/EntityLists/SupplierInvoiceList';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { SupplierInvoiceItemPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import { SupplierInvoicePMService } from '../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomsDocumentPointerService } from '../../../../../Customs/Services/Others/CustomsDocumentPointerService';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
declare var window: any;
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { SupplierInvoiceService } from '../../../../../Customs/Services/Others/SupplierInvoiceService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ImageParameter } from '../../../../../Infrastructure/DataContracts/ImageParameter';
import { Guid } from '../../../../../Infrastructure/Utilities/Guid';
import { List } from 'Infrastructure/DataContracts/Dashboard/List';
import { forEach } from 'cypress/types/lodash';
import { OcrDocumentExtendedListService } from 'Customs/Services/ExtendedLists/OcrDocumentExtendedListService';
import { OcrDocumentPM } from 'Customs/EntityPMs/OcrDocumentPM';
import { OcrDocumentPMService } from 'Customs/Services/StandardPMs/OcrDocumentPMService';
import { variable } from '@angular/compiler/src/output/output_ast';
declare var attachmentUploader, ResultAsArray: any;

@Component({
    selector: 'DigitalCertificateOfOriginTabComponent',

    templateUrl: './DigitalCertificateOfOriginTabComponent.html',
    providers: [DeclarationExtendedListService]
})

export class DigitalCertificateOfOriginTabComponent extends BaseComponent implements OnInit {
    public onQueryChangeEvent: any;

    public EntityPM: DeclarationPM;
    public ObjectTableName: string = null;
    public DataContext: any = this;
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public declarationPMService: DeclarationPMService = new DeclarationPMService();
    public UploadFileId: string = Guid.NewRandomString();
    filterImageParameter: ImageParameter;
    File: any;
    FileData: number;
    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService;
    customsDocumentPointerService: CustomsDocumentPointerService;
    supplierInvoicePMService: SupplierInvoicePMService;


    public ItemsSource: ObservableCollection;
    public Invoices: SupplierInvoicePM[];
    public InvoiceItems: ObservableCollection;
    public itemsList: SupplierInvoiceItemPM[];
    public IsVisible = false;
    public MultiUpdate = false;
    public IsOcr = false;

    private _entityListService: EntityListService;
    public IsDisplayOnly: boolean = false;
    public ShowStorageStatusMessage: boolean = false;
    public DisplayOnlyMessage: string = "";
    LayoutDirection: string = 'ltr';
    NumberOfLoadedItems: number = 500;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    private CurrentSession = SessionLocator.SelectedSession;
    IsDisplayMessage: boolean;
    showMultiUpdateWindowBtn: boolean = false;
    showUploadInvoicesFromCsvBtn: boolean = false;
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef, public declarationExtendedListService: DeclarationExtendedListService) {
        super();
        // this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe((response:any) => {
        this.customsDocumentPointerService = new CustomsDocumentPointerService();
        this.ItemsSource = new ObservableCollection([]);
        this.InvoiceItems = new ObservableCollection([]);

        this.Listen();
        this._entityListService = new EntityListService();
        //this.EntityPM = this.entityArgs.EntityPM;
        this.supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();

    }


    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsMod").subscribe((response: any) => {
                        this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemProcesType").subscribe((response: any) => {
                            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsConDeclar").subscribe((response: any) => {
                                this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsDescript").subscribe((response: any) => {
                                    this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsSerialNum").subscribe((response: any) => {
                                        this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsProdIdent").subscribe((response: any) => {
                                            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsLevy").subscribe((response: any) => {
                                                this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe((response: any) => {
                                                    this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe((response: any) => {
                                                        var multiUpdateFeature = FeatureLocator.HasFeaturePermession("Customs.Declaration", "MultiUpdate");
                                                        if (multiUpdateFeature) {
                                                            this.MultiUpdate = true;
                                                        }
                                                        var isOcrFeature = FeatureLocator.HasFeaturePermession("Customs.Declaration", "OCR");
                                                        if (isOcrFeature) {
                                                            this.IsOcr = true;
                                                        }




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

        this.showMultiUpdateWindowBtn = FeatureLocator.HasFeaturePermession("Customs.Declaration", "MultiUpdateClassificationCodeWindow")
        this.showUploadInvoicesFromCsvBtn = FeatureLocator.HasFeaturePermession("Customs.Declaration", "UploadExportInvoicesFromCsv")
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
                            if (FeatureLocator.HasFeaturePermession("Customs.Declaration", "OCR")) {
                                if (this.EntityPM.IsDirty) {
                                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                                    const unsub = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                        this.ReloadMyScreen();
                                        unsub.unsubscribe();

                                    });
                                }
                                else {
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                    this.ReloadMyScreen();
                                }

                            }
                            else
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
            FieldName: 'SequenceNumeric',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.SequenceNumeric"),
            Styles: { width: '50px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemCode"),
            Styles: { width: '100px', direction: 'ltr' },
            HtmlListComponentName: 'DigitalCertificateOfOriginListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DigitalCertificateOfOriginListTemplate',
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
            HtmlListComponentName: 'DigitalCertificateOfOriginListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DigitalCertificateOfOriginListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemPrice',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemPrice"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'DigitalCertificateOfOriginListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DigitalCertificateOfOriginListTemplate',
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
        this.Invoices = this.EntityPM.SupplierInvoices;
        this.ItemsSource.InsertCollection(this.Invoices, true);

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

    public SupplierInvoiceComprehensiveUpdate: SupplierInvoicePM[] = [];
    IsComprehensiveUpdateChecked(checked: boolean, item: SupplierInvoicePM) {

        if (checked) {
            this.SupplierInvoiceComprehensiveUpdate.push(item);
        }
        else {
            this.SupplierInvoiceComprehensiveUpdate = this.SupplierInvoiceComprehensiveUpdate.filter(i => i.InvoiceNumber !== item.InvoiceNumber);
        }
    }

    private _entityResourceService: EntityResourceService = new EntityResourceService();

    OpenOcrDefult() {
        this.DataSource;
        this._entityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceExportDefault", 0).subscribe((response: any) => {
            var windowTitle = TextCodeTranslator.Translate("Customs.Consignment.O.ComprehensiveUpdate");
            var logWindow = new LogitudeWindow();
            var args: any = {
                EntityPM: this.EntityPM,
                IsFromSupplierInvoice: true,
                SupplierInvoiceComprehensiveUpdate: this.SupplierInvoiceComprehensiveUpdate
            };
            logWindow.WindowArgs = args;
            logWindow.Width = 800;
            logWindow.Height = 500;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.WindowArgs = args;
            logWindow.Show('./Common/Components/Maintenance/OcrDefaultsSettingsComponent');
            logWindow.WindowClosed.subscribe((event: any) => {
                if (event == "update") {
                    this.RefreshEntity();
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SupplierInvoiceComprehensiveUpdate = [];
                }
                this.CD.reattach();
            });
        });
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

        this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.Id, item.InvoiceCounterKey, 0, this.NumberOfLoadedItems, "parent").subscribe((response: any) => {
            var windowArgs: any = {};
            windowArgs.EntityPM = response.Result;
            windowArgs.declarationPM = this.EntityPM;
            windowArgs.NumberOfLoadedItems = this.NumberOfLoadedItems;
            var windowTitle = "Supplier Invoice";

            var logWindow = new LogitudeWindow();
            logWindow.Width = 1017;// this changed By Rabaia for Task No. 54930; Dont change it back before calling me. //995; // don't change this width!
            logWindow.Height = 600;
            var textCodeTitle = "Customs.Declaration.O.EditInvoice";


            if (this.EntityPM.Direction == "E") {
                textCodeTitle = "Customs.Declaration.O.ExporterInvoice";
            }

            if (!AppTool.IsNullOrEmpty(item.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate(textCodeTitle) + " " + item.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

            }
            else if (AppTool.IsNullOrEmpty(item.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate(textCodeTitle) + " " + this.EntityPM.DeclarationNumber;

            }
            else if (!AppTool.IsNullOrEmpty(item.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate(textCodeTitle) + " " + item.InvoiceNumber;

            }
            else if (AppTool.IsNullOrEmpty(item.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate(textCodeTitle);

            }
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            this.CD.detach();
            logWindow.WindowClosed.subscribe((event: any) => {
                if (event != 'cancel') {
                    this.SupplierInvoiceComprehensiveUpdate = [];

                    this.RefreshEntity();
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                else {
                    this.ReloadMyScreen();
                }
                this.CD.reattach();

            });
            logWindow.IsHideHeader = true;
            
            // TODO: #101459 -change to other new component
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');

            this.CurrentSession.StopBusyIndicator();
        });

    }

    DeleteButtonClicked(item: SupplierInvoicePM) {
        var confirmWindow = new ConfirmWindow();
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

                                if (this.EntityPM.Direction == 'E' && this.IsOcr) {
                                    this.supplierInvoiceExtendedPMService.GetDocumentFilingIdForForInvoice(item.DeclarationId, item.InvoiceCounterKey, true).subscribe((response) => {
                                        var docId = response?.Result?.Result;
                                        var count = response?.Result?.Count ?? 0;
                                        if (docId && count <= 1) {
                                            var ocrDocumentExtendedListService: OcrDocumentExtendedListService = new OcrDocumentExtendedListService();

                                            ocrDocumentExtendedListService.GetOcrDocumentByDocumentFilingId(SessionLocator.Tenant, docId).subscribe((response) => {
                                                if (response?.Result?.NotConnect) {
                                                    var ocrDocumentPM: OcrDocumentPM = response.Result;
                                                    ocrDocumentPM.NotConnect = false;
                                                    var ocrDocumentPMService: OcrDocumentPMService = new OcrDocumentPMService();
                                                    ocrDocumentPMService.update(ocrDocumentPM).subscribe(() => {
                                                        this.DeleteSelected(item);
                                                    });
                                                }
                                                else {
                                                    this.DeleteSelected(item);
                                                }
                                            });
                                        }
                                        else {
                                            this.DeleteSelected(item);
                                        }

                                    });
                                }
                                else {
                                    this.DeleteSelected(item);
                                }
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
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }
    
    OpenMultiUpdateWindow() {
        var args: any = {
            Declaration: this.EntityPM,
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 320;
        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.MultiUpdate");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        // TODO: #101459 -change to other component
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/MultiUpdateComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }

    OpenUpLoadFile() {
        document.getElementById(this.UploadFileId).click();
    }
    
    UploadFile(event: any) {
        var FileExtension: string
        var file: any = attachmentUploader(this.UploadFileId);
        if (file) {
            var temp = file.name.split('.');
            FileExtension = temp[temp.length - 1];
            this.File = file;
            if (FileExtension != "csv") {
                this.ShowMessage("חובה קובץ CSV");
                return;
            }


            if (FileExtension && FileExtension.length > 10) {
                this.ShowMessage("File extension should be less than or equal 10 characters");
            }
            else {

                this.filterImageParameter = new ImageParameter();
                this.filterImageParameter.Key = Guid.newGuid();
                this.filterImageParameter.IsFirstTry = true;
                this.filterImageParameter.Extension = FileExtension;
                this.filterImageParameter.UploadMode = "Block";
                this.filterImageParameter.FileSize = file.size;
                this.filterImageParameter.Tenant = SessionLocator.Tenant;
                this.ArrayBufferToBase64(file, this);

            }
        }
    }

    ArrayBufferToBase64(file: any, viewmodel: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.filterImageParameter.Base64String = window.btoa(binary);
        };
        reader.onabort = function (e) {

        };
        reader.onloadend = function (e) {
            viewmodel.CreateExportSupplierInviocesFromFile();

        };
        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }

    CreateExportSupplierInviocesFromFile() {
        var supplierInvoiceService = new SupplierInvoiceService();

        supplierInvoiceService.PutExportSupplierInviocesFromFileRequest(this.filterImageParameter, SessionLocator.Tenant, this.EntityPM.Id)
            .subscribe((myServiceResponse: ServiceResponse) => {

                if (myServiceResponse.HasError) {
                    this.ShowMessage(myServiceResponse.ErrorsArray[0]);
                }
                else {
                    this.ShowMessage(myServiceResponse.Result);
                }
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.DeleteFileButtonClicked();
            });
    }

    @ViewChild('myInput')
    myInputVariable: ElementRef;
    DeleteFileButtonClicked() {
        this.myInputVariable.nativeElement.value = "";
        this.filterImageParameter = null;
        this.File = null;
    }

    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
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
    notToCheckFeature: boolean = false;
    NewInvoice() {
        var itemPM = new SupplierInvoicePM();
        itemPM.DeclarationId = this.EntityPM.Id;
        itemPM.Tenant = SessionLocator.Tenant;
        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

        if (this.notToCheckFeature) {
            itemPM.AccumalationStateCode = "3";
        }
        else {

            this.accumulationFeature = FeatureLocator.HasFeaturePermession("Customs.Declaration", "ACCUMULATION")
            if (this.accumulationFeature == null) {
                itemPM.AccumalationStateCode = "3";
            }
            else {
                itemPM.AccumalationStateCode = "1";
            }
        }
        itemPM.InvoiceCounterKey = 0;



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

        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CD.reattach();

            this.RefreshEntity();
            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            this.getSupplierInvoices();
        });
        this.CD.detach();
        logWindow.IsHideHeader = true;
        // TODO: #101459 -change to other component
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.EntityPM.AmendmentMessage != null && this.EntityPM.AmendmentMessage != "") {
            {
                this.IsDisplayMessage = true;

                this.DisplayOnlyMessage = this.EntityPM.AmendmentMessage;
                if (this.EntityPM.IsAmendmentDisplayOnly) this.IsDisplayOnly = this.EntityPM.IsAmendmentDisplayOnly;
            }
        }

        else if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;

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
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe((response: any) => {
            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
            this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (this.EntityPM.AmendmentMessage != null && this.EntityPM.AmendmentMessage != "") {
                {
                    this.IsDisplayMessage = true;

                    this.DisplayOnlyMessage = this.EntityPM.AmendmentMessage;
                    if (this.EntityPM.IsAmendmentDisplayOnly) this.IsDisplayOnly = this.EntityPM.IsAmendmentDisplayOnly;
                }
            }

            else if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (this.EntityPM.StorageStatusCode) {
                this.ShowStorageStatusMessage = true;
                this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
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

}

