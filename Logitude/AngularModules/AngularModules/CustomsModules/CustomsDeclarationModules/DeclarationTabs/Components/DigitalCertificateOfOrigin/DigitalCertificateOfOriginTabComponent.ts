import { Component, OnInit, Output, EventEmitter, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomsDocumentPointerService } from '../../../../../Customs/Services/Others/CustomsDocumentPointerService';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
declare var window: any;
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ImageParameter } from '../../../../../Infrastructure/DataContracts/ImageParameter';
import { Guid } from '../../../../../Infrastructure/Utilities/Guid';
import { variable } from '@angular/compiler/src/output/output_ast';
import { CertificateOfOriginPMService } from 'Customs/Services/StandardPMs/CertificateOfOriginPMService';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
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
    customsDocumentPointerService: CustomsDocumentPointerService
    public UploadFileId: string = Guid.NewRandomString();
    filterImageParameter: ImageParameter;
    certificateOfOriginPMService: CertificateOfOriginPMService;


    public ItemsSource: ObservableCollection;
    public CertificateOfOrigins: CertificateOfOriginPM[];
    public CertificateOfOriginItems: ObservableCollection;
    public itemsList: CertificateOfOriginPM[];
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
    
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef, public declarationExtendedListService: DeclarationExtendedListService) {
        super();

        this.ItemsSource = new ObservableCollection([]);
        this.CertificateOfOriginItems = new ObservableCollection([]);

        this.Listen();
        this._entityListService = new EntityListService();

        this.certificateOfOriginPMService = new CertificateOfOriginPMService();

    }


    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
            this.entityResourceService.getEntityResourceByTableName("Customs.CertificateOfOrigin").subscribe((response: any) => {
                this.entityResourceService.getEntityResourceByTableName("Customs.CertificateOfOriginInvoice").subscribe((response: any) => {
                    this.entityResourceService.getEntityResourceByTableName("Customs.CertificateOfOriginItem").subscribe((response: any) => {
                        var multiUpdateFeature = FeatureLocator.HasFeaturePermession("Customs.Declaration", "MultiUpdate");
                        if (multiUpdateFeature) {
                            this.MultiUpdate = true;
                        }
                        var isOcrFeature = FeatureLocator.HasFeaturePermession("Customs.Declaration", "OCR");
                        if (isOcrFeature) {
                            this.IsOcr = true;
                        }



                        this.IsVisible = true;



                        this.certificateOfOriginPMService = new CertificateOfOriginPMService();
                        this.certificateOfOriginPMService = new CertificateOfOriginPMService();
                        this.ObjectTableName = this.entityArgs.ObjectTableName;


                        this.ReloadMyScreen();


                    });
                });
            });
        });
    }


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
    ReloadMyScreen() { ///DSV - After Sending to Customs - Enter and Getting Optimistic Concurancy error"
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.getCertificateOfOrigins();
        this.DisplayOnlyCheck();
    }

    AddNewCertificateOfOrigin() {
        debugger
        
        //TODO: add new CertificateOfOrigin
        var args: any = {
            Declaration: this.EntityPM,
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 700;
        // logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.DigitalCertificateOfOrigin");
        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.CertificateOfOrigin");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        // logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOrigin/CertificateOfOrigin');
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/CertificateOfOriginComponent');


        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });

        
       
    }
    
    ViewInitCompleted($event) {
        this.SelectedRow = this.ItemsSource.Collection[0];
        this.OnRowSelected(this.SelectedRow);
    }

    SendMsgCooStatusCode() {
     
    }
    CopyOfCertificate() {
     
    }

    getCertificateOfOrigins() {
      
        const filters = new ApiQueryFilters();    
      
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "SequenceNumeric";
        filters.SortDirection = "Ascending";
        filters.addAdditionalFilter("DeclarationNumber", this.EntityPM.DeclarationNumber, null, null, "Equals", false, false, false, "string");
   
        this._entityListService.getByFilters("Customs.CertificateOfOrigin", filters).then((myResult:any) => {
            console.log("Response: ", myResult);
            if (myResult == null) {
                this.CertificateOfOrigins =  [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError && myResponse.Result) {
                    this.ItemsSource = myResponse.Result;
                    this.CertificateOfOrigins =  myResponse.Result;        
                    this.ItemsSource.InsertCollection(this.CertificateOfOrigins, true);
                }
            }
        });


        //Select last selected row, or first
        if (this.SelectedRowB4Refresh) {

            // if (this.SelectedRowB4Refresh == this.lastDeletedItem) { //deleted item
            //     this.OnRowSelected(this.CertificateOfOrigins[0]);
            // } else {
            //     var selectedInvoiceKey = this.SelectedRowB4Refresh.;
            //     var selectedInvoice = this.CertificateOfOrigins.filter(d => d.InvoiceCounterKey == selectedInvoiceKey)[0];
            //     this.OnRowSelected(selectedInvoice);
            // }

        }
        // else
        //     this.OnRowSelected(this.CertificateOfOrigins[0]);
    }

    public SelectedRow: CertificateOfOriginPM = null;
    public SelectedRowB4Refresh: CertificateOfOriginPM = null;
    OnRowSelected(itemComponent: CertificateOfOriginPM) {

        this.SelectedRow = itemComponent;
        this.SelectedRowB4Refresh = this.SelectedRow;
        this.filterAgrs = new ApiQueryFilters();

        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        if (itemComponent) {
        
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

            // if (this.SelectedRow.IsAccumalated) {
            //     filters.addAdditionalFilter("IsParent", true, null, null, "Equals", false, false, false, "boolean");
            // }
            // filters.addAdditionalFilter("DeclarationId", this.SelectedRow.DeclarationId, null, null, "Equals", false, false, false, "string");
            // filters.addAdditionalFilter("CounterKey", this.SelectedRow.InvoiceCounterKey, null, null, "Equals", false, false, false, "number");
        }
        else {
            filters.addAdditionalFilter("DeclarationNumber", this.EntityPM.DeclarationNumber, null, null, "Equals", false, false, false, "string");
        }
        return this._entityListService.getByFilters("Customs.CertificateOfOrigin", filters);
    }


    filterAgrs: ApiQueryFilters;
    
    public CertificateOfOriginComprehensiveUpdate: CertificateOfOriginPM[] = [];
    IsComprehensiveUpdateChecked(checked: boolean, item: CertificateOfOriginPM) {

        if (checked) {
            this.CertificateOfOriginComprehensiveUpdate.push(item);
        }
       
    }

    private _entityResourceService: EntityResourceService = new EntityResourceService();

    


    EditButtonClicked(item: CertificateOfOriginPM) {
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
                            this.EditCertificateOfOrigin(item);

                        }
                    }

                });
            }
            else {
                this.EditCertificateOfOrigin(item);
            }
        }



    }

    EditCertificateOfOrigin(item: CertificateOfOriginPM) {   
        this.CurrentSession.StartBusyIndicator("");

        // this.certificateOfOriginPMService.get(this.EntityPM.Id).subscribe((response: any) => {
        //     var windowArgs: any = {};
        //     windowArgs.EntityPM = response.Result;
        //     windowArgs.declarationPM = this.EntityPM;
        //     windowArgs.NumberOfLoadedItems = this.NumberOfLoadedItems;
        //     var windowTitle = "Certificate Of Origin";

        //     var logWindow = new LogitudeWindow();
        //     logWindow.Width = 1017;// this changed By Rabaia for Task No. 54930; Dont change it back before calling me. //995; // don't change this width!
        //     logWindow.Height = 600;
        //     var textCodeTitle = "Customs.Declaration.O.EditCertificateOfOrigin";


        //     if (this.EntityPM.Direction == "E") {
        //         textCodeTitle = "Customs.Declaration.O.ExporterEditCertificateOfOrigin";
        //     }

            
        //     windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        //     logWindow.ShowCloseButton = false;
        //     logWindow.WindowArgs = windowArgs;
        //     this.CD.detach();
        //     logWindow.WindowClosed.subscribe((event: any) => {
        //         if (event != 'cancel') {
        //             this.CertificateOfOriginComprehensiveUpdate = [];

        //             this.RefreshEntity();
        //             this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        //         }
        //         else {
        //             this.ReloadMyScreen();
        //         }
        //         this.CD.reattach();

        //     });
        //     logWindow.IsHideHeader = true;

        //     // TODO: #101459 -change to other new component
        //     logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditCertificateOfOriginComponent');

        //     this.CurrentSession.StopBusyIndicator();
        // });

    }

    DeleteButtonClicked(item: CertificateOfOriginPM) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");

        confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteInvoice"));
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                
            } else if (confirmWindow.No) {

            }
        });
    }


    lastDeletedItem: CertificateOfOriginPM;
    DeleteSelected(item: CertificateOfOriginPM) {
        this.CurrentSession.StartBusyIndicator("");
        this.lastDeletedItem = item;

        var SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            SaveCompletedEvent.unsubscribe();
            // this.certificateOfOriginPMService.delete(item.DeclarationId, item.InvoiceCounterKey).subscribe((myResponse: ServiceResponse) => {

            //     if (!myResponse.HasError) {
            //         this.ItemsSource.Remove(item);
            //         this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            //         this.CurrentSession.StopBusyIndicator();
            //     }
            // });
        });

        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }

   
    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

 
    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

    }

    DisplayOnlyCheck() {
        // TODO: can look on example function from file- DeclarationSupplierInvoiceTabComponent.ts
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
            // SetEnabled
            
            return;
        }
        else if (this.EntityPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            //this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
        }
        
    }

 
}

