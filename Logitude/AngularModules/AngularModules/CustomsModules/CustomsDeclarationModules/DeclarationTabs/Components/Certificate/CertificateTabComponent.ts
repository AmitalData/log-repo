

declare var window: any;
import {Component, AfterViewInit, OnInit, ChangeDetectorRef, Output, EventEmitter}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult} from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';

import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {DeclarationErrorView} from '../../../../../Customs/EntityPMs/Extended/DeclarationErrorView';
import {DeclarationConstraintPM} from '../../../../../Customs/EntityPMs/DeclarationConstraintPM';
import {DeclarationEventManager} from '../../../../../Customs/Utilities/DeclarationEventManager';

import {DeclarationWebService} from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import {ConstraintApprovalRequestParams} from '../../../../../Customs/DataContract/RequestParams/ConstraintApprovalRequestParams';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import { VendorInsertUpdateDeleteMessageRequestParams, OperationTypes } from '../../../../../Customs/DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {DeclarationMessagesService} from '../../../../../Customs/Services/WebServices/DeclarationMessagesService';
import {CertificateTicket} from '../../../../../Customs/DataContract/CertificateTicket';
import {MultiCertificatesService} from '../../../../../Customs/Services/Others/MultiCertificatesService';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CertificateConnectedItem} from '../../../../../Customs/DataContract/CertificateConnectedItem';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {ConfirmationTypePM} from  '../../../../../Customs/EntityPMs/ConfirmationTypePM';

@Component({
    moduleId: module.id,
    templateUrl: './CertificateTabComponent.html',
})

export class CertificateTabComponent extends BaseComponent implements OnInit {
    public DeclarationPM: DeclarationPM;
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: any = this;
    public CurrentEditComponentId: string;
    public IsDisplayOnly: boolean = false;
    public ShowStorageStatusMessage: boolean = false;
    public DisplayOnlyMessage: string = "";
    ResponseData: INF_MSG_GenericResponseData;
    selecteCertificate: CertificateTicket;
    SelectedItem: CertificateTicketListItem = null;
    multiCertificatesService: MultiCertificatesService = new MultiCertificatesService();
    //Services
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    //private declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    public connectedItems: ObservableCollection;
    public ExcludedItems: ObservableCollection;

    SelectedItemsCountText: string;
    SelectedItemsCount: number;;
    public IsVisible: boolean;
    showTemplate: boolean = false;
    CertificateTicketsList: CertificateTicketListItem[] = [];
    preventSelect: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public CD: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();
        this.SelectedItemsCount = 0;
        this.CurrentSession.SubscriptionAdd(
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
                    if (res == "certificate") {
                        this.preventSelect = true;
                    }
                })
            )
        );
        
        //SessionLocator.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
        //    SessionLocator.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe(response => {


        this.DeclarationPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
       
        this.BuildColumns();
        this.Listen();
        console.log("Declaration", this.DeclarationPM);
        this.connectedItems = new ObservableCollection([]);
        this.ExcludedItems = new ObservableCollection([]);
        this.DisplayOnlyCheck();

        this.CheckDeclarationInvoices();

        //  this.GetCertificates(null);

        //this.CurrentSession.SelectItemEvent.subscribe((res) => {
        //    if (res.selected) {
        //        //  this.showTemplate = true;

        //        if (!this.connectedItems.Collection.includes(res.Data))
        //            this.connectedItems.Insert(res.data);
        //        this.SelectedItemsCount += 1;
        //        if (this.dataCount != null) {
        //            if (this.SelectedItemsCount == this.dataCount) {
        //                //    this.IsSelected = true;
        //            }
        //            this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();

        //        }
        //    }
        //    else {
        //        this.connectedItems.Remove(res.data);

        //        this.SelectedItemsCount -= 1;
        //        if (this.IsSelected) this.IsSelected = false;
        //        if (this.dataCount != null) {
        //            this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();

        //        }
        //    }

        //    if (this.SelectedItemsCount > 0) {
        //        this.IsVisible = true;
        //    }
        //    else {
        //        this.IsVisible = false;
        //    }

        //});


        //});
        //});
    }

    ThereIsNoInvoices: boolean = false;
    NoInvoicesMessage: string;
    CheckDeclarationInvoices() {
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
        this.multiCertificatesService.DeclarationHasInvoices(this.DeclarationPM.Id).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!response.Result) {

                if (!this.IsDisplayOnly) {
                    this.ThereIsNoInvoices = true;
                    this.NoInvoicesMessage = TextCodeTranslator.Translate("Customs.Declaration.O.EnterAtLeastInvoice");
                }
            }


            else {
                this.ReloadCertificateTickets(true);
            }

        });
    }
    filterAgrs: ApiQueryFilters;

    activeItem: CertificateTicketListItem;



    ngOnInit() {
        //  this.BuildColumns();
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.DeclarationPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                })
            );;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.DeclarationPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //   this.GetCertificates(null);
                        this.DisplayOnlyCheck();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DECR") {
                            this.activeItem = null;
                            this.selecteCertificate = null;
                            //   this.ReloadCertificateTickets(true);
                            this.DisplayOnlyCheck();
                            this.ConfirmationTypeCode = null;
                            this.CheckDeclarationInvoices();
                        }
                    }
                })
            );
        }
    }

    IsFirstTime: boolean = true;
    ReloadCertificateTickets(isFirstTime: boolean) {
        this.IsFirstTime = isFirstTime;
        this.GetCertificates(null);
    }
    
    ConfirmationTypesFilterItems: ApiQueryFilters;
    GetCertificates(message: any) {
        if (message == "ok" || message == null) {
            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Loading"));
            this.connectedItems.Collection = [];
            this.ExcludedItems.Collection = [];
            this.ThereIsNoInvoices = false;
            this.IsVisible = false;
            this.SelectedItemsCountText = null;
            this.SelectedItemsCount = 0;
          
            this.IsSelected = false;
            var confirmation = null;
            if (this.ConfirmationType) {
                confirmation= this.ConfirmationType.Code;
            }
            this.declarationWebService.GetCertificateTickets(this.DeclarationPM.Id, confirmation, this.selectedInvoiceNumber, this.SelectedCounterKey, this.DemandState)
                .subscribe((response: ServiceResponse) => {
                    console.log("[Response] GetCertificateTickets: ", response);
                    this.CertificateTicketsList = [];
                    this.ConfirmationTypesFilterItems = new ApiQueryFilters();
                    var reqConfirmationCodes: string = "";
                    var res = response.Result;
                    if (res) {
                     
                        if (res.length > 0) {
                            if (!AppTool.IsNullOrEmpty(res)) {

                                this.IsCheckBoxVisible = true;

                                for (var i = 0; i < res.length; i++) {

                                 
                                    reqConfirmationCodes = reqConfirmationCodes + res[i].ReqConfirmationTypeCode + ',';
                                  

                                    var item: CertificateTicketListItem = new CertificateTicketListItem(res[i], this);
                                    //if (i == 0) {
                                    //    if (this.activeItem == null) {
                                    //        this.activeItem = item;
                                    //    }
                                    //}
                                    if (item.AttachmentTypeCode == "4") {
                                        item.ExepmtVisibility = true;
                                    } else {
                                        item.ConfirmationVisibility = true;
                                    }


                                    this.CertificateTicketsList.push(item);

                                }

                                if (reqConfirmationCodes != null) {
                                    reqConfirmationCodes = reqConfirmationCodes.substr(0, reqConfirmationCodes.length - 1);
                                }
                                this.ConfirmationTypesFilterItems.addAdditionalFilter("Code", reqConfirmationCodes, null, null, "InListExact", false, false, false, "string", false, true);

                                if (this.activeItem != null) {
                                    this.activeItem = this.CertificateTicketsList.filter(d => d.AttachmentTypeCode == this.activeItem.AttachmentTypeCode && d.ReqConfirmationTypeCode == this.activeItem.ReqConfirmationTypeCode && d.ResConfirmationTypeCode == this.activeItem.ResConfirmationTypeCode && d.CustomsAttachmentId == this.activeItem.CustomsAttachmentId && d.CertificateNumber == this.activeItem.CertificateNumber)[0];
                                }
                                if (this.activeItem == null) {
                                    this.activeItem = this.CertificateTicketsList[0];
                                }

                                if (this.selecteCertificate != null) {
                                    var selected = this.CertificateTicketsList.filter(d => d.AttachmentTypeCode == this.selecteCertificate.AttachmentTypeCode && d.ReqConfirmationTypeCode == this.selecteCertificate.ReqConfirmationTypeCode && d.ResConfirmationTypeCode == this.selecteCertificate.ResConfirmationTypeCode && d.CustomsAttachmentId == this.selecteCertificate.CustomsAttachmentId && d.CertificateNumber == this.selecteCertificate.CertificateNumber)[0];
                                }
                                if (selected != null) this.selecteCertificate = selected.ticket;
                                if (selected == null) {
                                    this.selecteCertificate = this.CertificateTicketsList[0].ticket;
                                }

                                
                             



                                this.GetCertificatesCompleted();
                            }
                        }
                        else {
                            this.selecteCertificate = null;
                            this.IsCheckBoxVisible = false;

                        }
                    }
                     this.LoadConnectedItems(null);
                    this.CurrentSession.StopBusyIndicator();
                });
        }
    }

    LoadConnectedItems(message: string) {
        this.preventSelect = false;
        if (message == "ok" || message == null) {

            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }
    }


    GetCertificatesCompleted() {
        /*** put you code here, alaa ***/


        if (this.SelectedInvoiceNumber == null) {
            this.FillInvoiceNumbersList();
        }

    }


    //#region Invoice ComboBox
    SelectedInvoiceReqConfirmation: string;
    InvoicesSelectionChanged(selectedItem) {
        if (selectedItem != null) {
            this.SelectedInvoiceNumber = selectedItem.InvoiceNumber;
            this.SelectedCounterKey = selectedItem.InvoiceCounterKey;
            this.SelectedInvoiceReqConfirmation = selectedItem.ReqConfirmationTypeCode;
            this.ReloadCertificateTickets(false);
        }
    }
    InvoicesNumbersList: any[];
    FillInvoiceNumbersList() {
        this.declarationWebService.GetDeclarationInvoicesNumbers(this.DeclarationPM.Id)
            .subscribe((response: ServiceResponse) => {
                console.log("[Response] GetDeclarationInvoicesNumbers: ", response);
                var res = response.Result;
                if (!AppTool.IsNullOrEmpty(res)) {
                    this.InvoicesNumbersList = [];
                    this.InvoicesNumbersList = res;


                }
            });


    }
    //#endregion

    //#region DemandState Filter Methods
    public DemandStateFilterSelectedValue: string = 'All';
    DemandStateFilterItemClicked(itemValue: string) {
        if (this.DemandStateFilterSelectedValue != itemValue) {
            this.DemandStateFilterSelectedValue = itemValue;
            this.DemandState = itemValue == 'All' ? null : itemValue;
            this.ReloadCertificateTickets(false);
        }
    }
    //#endregion

    //#region LevelSelection Filter Methods
    public LevelSelectionFilterSelectedValue: string = 'DeclarationConect';
    LevelSelectionFilterItemClicked(itemValue: string) {
        if (this.LevelSelectionFilterSelectedValue != itemValue) {
            this.LevelSelectionFilterSelectedValue = itemValue;
            if (itemValue != "Invoice") {
                this.SelectedInvoiceNumber = null;
                this.SelectedCounterKey = null;
                this.ReloadCertificateTickets(false);
            }
        }
    }
    //#endregion

    //#region Filter Methods
    // public FilterSelectedValue: string = null;
    //FilterItemClicked(itemValue: string) {
    //    if (this.FilterSelectedValue != itemValue) {
    //        this.FilterSelectedValue = itemValue;
    //    }
    //}
    //#endregion

    //#region Properties

    private SelectedCounterKey: number = null;
    private DemandState: string = null;

    private selectedInvoiceNumber: string = null;
    public get SelectedInvoiceNumber() { return this.selectedInvoiceNumber }
    public set SelectedInvoiceNumber(newValue: string) {
        this.selectedInvoiceNumber = newValue;
    }



    //#endregion

    RefreshEntity() {
        //if (this.IsDisplayOnly) {
            if (this.CurrentSession.CurrentEditComponent) {
                this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        //}
        if (!this.ThereIsNoInvoices) {
            this.activeItem = null;
            this.selecteCertificate = null;
            this.ReloadCertificateTickets(false);
            this.preventSelect = false;
        }
    }
    timerToken: any;
    DisplayOnlyCheck() {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;

        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            //this.SetScreenFieldsEditability();
            this.timerToken = setTimeout(() => {
                DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            }, 200);
            return;
        }
        else if (this.DeclarationPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.DeclarationPM.StorageStatusName;
        }
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.DeclarationPM).subscribe((response: ServiceResponse) => {
            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
            this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (this.DeclarationPM.StorageStatusCode) {
                this.ShowStorageStatusMessage = true;
                this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.DeclarationPM.StorageStatusName;
            }
            this.timerToken = setTimeout(() => {
                DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            }, 200);
        });
    }


    public columns: any[] = null;

    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: "",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            IsCheckBox: true
            // HtmlListComponentName: 'CertificateCheckBoxComponent',
            // HtmlListComponentUrl: './Customs/Components/ListTemplates/CertificateCheckBoxComponent',
        });


        this.columns.push({
            FieldName: 'InvoiceNumber',
            DataTypeCode: 'String',
            Display: 'מספר חשבון',
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'SequenceNumeric',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.SequenceNumeric"),
            Styles: { width: '30px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemCode"),
            Styles: { width: '70px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'ClassificationCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ClassificationCode"),
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'TradeAgreementName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.TradeAgreementCode"),
            Styles: { width: '80px' },

            IsCustomTemplate: true
        });


        this.columns.push({
            FieldName: 'OriginCountryName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.OriginCountryName"),
            Styles: { width: '90px' },
            IsCustomTemplate: true
        });



        this.columns.push({
            FieldName: "CatalogNumber",
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.CatalogNumber"),


            IsCustomTemplate: true,
            Styles: { width: '80px' },
            HtmlListComponentName: 'CertificateTextBoxComponent',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CertificateTextBoxComponent',
        });


    }

    DataSource = {

        pageSize: 10,
        rowCount: null,
        sortingCol: "InvoiceNumber",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);

            return tempo;


        },

    };

    ignoreCount: boolean = false;
    IsCheckBoxVisible: boolean = false;
    private isSelected: boolean;
    public get IsSelected() { return this.isSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
        //this.SelectedItemsCount = 0;
        if (this.isSelected) {
            this.dataCount = this.DataSource.rowCount;
            this.SelectedItemsCount = this.dataCount;
            if (this.dataCount) {
                this.SelectedItemsCountText = "נבחרו " + this.dataCount.toString() + " פריטים מתוך " + this.dataCount.toString();
            }
            this.IsVisible = true;
            this.ignoreCount = true;
            if (this.selecteCertificate != null) {
                this.selecteCertificate.IsAllSelected = true;
            }
        }
        else {
            this.SelectedItemsCount = 0;
            this.IsVisible = false;
            if (this.selecteCertificate != null) {
                this.selecteCertificate.IsAllSelected = false;
            }
        }

        //    //this.CurrentSession.ConnectedItemSelectedEvent.emit({ Count: "All" });
        //    this.SelectedItemsCount = this.dataCount;
        //    this.selecteCertificate.IsAllSelected = true;
        //    this.SelectedItemsCountText = "נבחרו " + this.dataCount.toString() + " פריטים מתוך " + this.dataCount.toString();
        //    this.IsVisible = true;
        //}
        //else {
        //    if (this.dataCount == this.SelectedItemsCount) {
        //       // this.CurrentSession.ConnectedItemSelectedEvent.emit({ Count: "None" });
        //        this.SelectedItemsCount = 0;
        //        this.SelectedItemsCountText = null;
        //        if (this.selecteCertificate) {
        //            this.selecteCertificate.IsAllSelected = false;
        //        }
        //        this.IsVisible = false;
        //    }
        //}
    }

    onCheckBoxChecked($event) {

        if ($event.IsChecked) {
            if (!this.connectedItems.Collection.includes($event.rowData))
                this.connectedItems.Insert($event.rowData);
            if (this.IsSelected) {
                if (this.ExcludedItems.Collection.includes($event.rowData)) {
                    this.ExcludedItems.Remove($event.rowData);
                }
            }
            //if (this.ignoreCount == false) {
            this.SelectedItemsCount += 1;
            //}
            this.dataCount = this.DataSource.rowCount;
            if (this.dataCount != null) {
                this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();

            }
            if (this.SelectedItemsCount == this.dataCount) {
                this.IsSelected = true;
            }
        }
        else {
            this.connectedItems.Remove($event.rowData);
            if (this.IsSelected) {
                if (!this.ExcludedItems.Collection.includes($event.rowData)) {
                    this.ExcludedItems.Insert($event.rowData);
                }
            }
            //if (this.ignoreCount == false) {
            this.SelectedItemsCount -= 1;
            //}
            //if (this.IsSelected) this.IsSelected = false;
            if (this.dataCount != null) {
                this.SelectedItemsCountText = "נבחרו " + (this.SelectedItemsCount).toString() + " פריטים מתוך " + this.dataCount.toString();

            }
        }

        if (this.SelectedItemsCount > 0) {
            this.IsVisible = true;
        }
        else {
            this.IsVisible = false;
        }
        this.CD.detectChanges();
    }
    ViewInitCompleted($event) {
        /// this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }

    public SelectedRow: any = null;
    OnRowSelected(CurrentRow) {
        if (this.preventSelect == false) {
            //if (!this.showTemplate) {
            this.SelectedRow = CurrentRow.rowData;
            this.CurrentSession.StartBusyIndicatorLoading();

            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe(response => {

                this.declarationWebService
                    .GetSupplierInvoiceWithSpecificItemByCounterKey(this.SelectedRow.DeclarationId, this.SelectedRow.InvoiceCounterKey, this.SelectedRow.SequenceNumeric)
                    .subscribe((response: ServiceResponse) => {
                        console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);
                        this.preventSelect = true;

                        var supplierInvoicePM = response.Result;


                        if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

                            this.CurrentSession.StartBusyIndicatorLoading();
                            var windowArgs: any = {};
                     
                        
                            windowArgs.EntityPM = supplierInvoicePM;
                          
                            windowArgs.declarationPM = this.DeclarationPM;

                           
                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 1030;
                            logWindow.Height = 600;

                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.DeclarationPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.DeclarationPM.DeclarationNumber;

                            }
                            else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.DeclarationPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;

                            }
                            else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.DeclarationPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;

                            }
                            else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.DeclarationPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                            }
                            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                          
                     
                            logWindow.ShowCloseButton = false;
                            logWindow.WindowArgs = windowArgs;


                            logWindow.WindowClosed.subscribe(($event: any) => {
                                this.LoadConnectedItems($event);
                                this.CD.reattach();
                                this.RefreshEntity();
                            });
                            this.CD.detach();
                          logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                            this.CurrentSession.StopBusyIndicator();

                        }
                        else {
                            var window = new MessageWindow();
                            window.Show("There is no invoice with such key in this declaration!!");
                            this.preventSelect = false;
                        }
                        this.CurrentSession.StopBusyIndicator();
                    });



            });
            //}
            //this.showTemplate = false;
        }
        else {
            //this.preventSelect = false;
        }

    }

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;

        if (this.SelectedInvoiceNumber != null) {
            filters.addAdditionalFilter("InvoiceNumber", this.SelectedInvoiceNumber, null, null, "Equals", false, false, false, "string");
        }

        if (this.selecteCertificate) {
            if (this.ConfirmationType) {
                return this.multiCertificatesService.getPromiseByFilters(filters, this.DeclarationPM.Id, this.selecteCertificate.AttachmentTypeCode, this.ConfirmationType.Code, this.selecteCertificate.CertificateExemptionTypeCode, this.selecteCertificate.CertificateNumber, this.selecteCertificate.ResConfirmationTypeCode);

            }
            else {

                return this.multiCertificatesService.getPromiseByFilters(filters, this.DeclarationPM.Id, this.selecteCertificate.AttachmentTypeCode, this.selecteCertificate.ReqConfirmationTypeCode, this.selecteCertificate.CertificateExemptionTypeCode, this.selecteCertificate.CertificateNumber, this.selecteCertificate.ResConfirmationTypeCode);

            }
        }
        //else if (this.ConfirmationType != null){
        //    return this.multiCertificatesService.getPromiseByFilters(filters, this.DeclarationPM.Id, null, this.ConfirmationType.Code, null, null,null);

        //}
       
        else {
            return this.multiCertificatesService.getPromiseByFilters(filters, this.DeclarationPM.Id, "10", null, null, null, null);
        }

    }
    dataCount: number;

    CreateMethod() {
        var windowArgs: any = {};
        windowArgs.Ticket = this.selecteCertificate;
        windowArgs.IsAllSelected = this.IsSelected;
        windowArgs.IsNew = true;
        windowArgs.DeclarationId = this.DeclarationPM.Id;
        windowArgs.ConnectedItems = this.connectedItems.Collection;
        windowArgs.ExcludedItems = this.ExcludedItems.Collection;
        windowArgs.Parent = this;

        var logWindow = new LogitudeWindow();
        if (this.selecteCertificate.ReqConfirmationTypeName != null) {
            logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.RequestedCerticate") + " - " + this.selecteCertificate.ReqConfirmationTypeName + " " + this.selecteCertificate.ReqConfirmationTypeCode;
        }

        else {
            logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.RequestedCerticate");
        }




        logWindow.Width = 550;
        logWindow.Height = 300;

        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "ok") {
                this.GetCertificates($event);
                this.CD.reattach();
                this.RefreshEntity();
            }
        });
        this.CD.detach();
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Certificate/CreateEditTicketComponent');


    }

    MoveMethod() {
        var windowArgs: any = {};
        var certificates: any[] = [];
        windowArgs.Ticket = this.selecteCertificate;

        windowArgs.IsAllSelected = this.IsSelected;
        windowArgs.certificateList = this.CertificateTicketsList.filter(item => item.ticket != this.selecteCertificate && item.ReqConfirmationTypeCode == this.selecteCertificate.ReqConfirmationTypeCode);
        windowArgs.ConnectedItems = this.connectedItems.Collection;
        windowArgs.ExcludedItems = this.ExcludedItems.Collection;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "העבר לאישור";


        logWindow.Width = 900;
        logWindow.Height = 600;

        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.GetCertificates($event);
            this.CD.reattach();
            this.RefreshEntity();
        });
        this.CD.detach();
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Certificate/CertificateSelectionComponent');


    }

    confirmationType: ConfirmationTypePM;
    originalCertificateList: CertificateTicketListItem[] = [];

    get ConfirmationType() { return this.confirmationType; }
    set ConfirmationType(value: ConfirmationTypePM) {

        if (this.confirmationType != value) {
            this.confirmationType = value;
            if (value) {
                this.ConfirmationTypeCode = value.Code; 
                }
            this.GetCertificates(null);

        }
       
    }

    confirmationTypeCode: string;
    get ConfirmationTypeCode() { return this.confirmationTypeCode; }
    set ConfirmationTypeCode(value: string)
    {
        this.confirmationTypeCode = value;
    }
}

export class CertificateTicketListItem extends BaseComponent {

    ticket: CertificateTicket;
    ObjectTableName: string = "Customs.SupplierInvioceItemCertificat";
    DataContext: any = this;
    parent: CertificateTabComponent;
    multiCertificatesService: MultiCertificatesService = new MultiCertificatesService();

    constructor(entity: CertificateTicket, Parent: CertificateTabComponent) {

        super();
        this.ticket = entity;
        this.parent = Parent;
        this.IsDisplayOnly = this.parent.IsDisplayOnly;
        //   this.parent.selecteCertificate = entity;
        if (!AppTool.IsNullOrEmpty(entity.ReqConfirmationTypeName)) {

            this.Title = entity.ReqConfirmationTypeName + " " + entity.ReqConfirmationTypeCode;
        }
        this.FilterSelectedValue = this.ticket.AttachmentTypeCode;
        if (this.FilterSelectedValue == "4") {
            this.ConfirmationVisibility = false;
            this.ExepmtVisibility = true;
        }
        else {
            this.ConfirmationVisibility = true;
            this.ExepmtVisibility = false;
        }
    }

    Title: string;
    public IsDisplayOnly: boolean;
    private confirmationVisibility: boolean;
    get ConfirmationVisibility() { return this.confirmationVisibility; }
    set ConfirmationVisibility(newValue: boolean) {
        this.confirmationVisibility = newValue;
    }
    private exepmtVisibility: boolean;
    get ExepmtVisibility() { return this.exepmtVisibility; }
    set ExepmtVisibility(newValue: boolean) {
        this.exepmtVisibility = newValue;
    }
    get CertificateNumber() { return this.ticket.CertificateNumber; }
    get ReqConfirmationTypeCode() { return this.ticket.ReqConfirmationTypeCode; }
    get ResConfirmationTypeName() { return this.ticket.ResConfirmationTypeName; }
    get AttachmentTypeCode() { return this.ticket.AttachmentTypeCode; }
    get ReqConfirmationTypeName() { return this.ticket.ReqConfirmationTypeName; }
    get AttachmentTypeName() {
        return this.ticket.AttachmentTypeName;
    }

    get CertificateExemptionTypeName() { return this.ticket.CertificateExemptionTypeName; }
    get CertificateExemptionTypeCode() { return this.ticket.CertificateExemptionTypeCode; }
    get CustomsAttachmentId() { return this.ticket.CustomsAttachmentId; }
    get ResConfirmationTypeCode() { return this.ticket.ResConfirmationTypeCode; }


    FilterSelectedValue: string;
    CertificateItemClicked(item: CertificateTicketListItem) {
        this.parent.SelectedItem = this;
        this.parent.IsVisible = false;
        this.parent.selecteCertificate = this.ticket;
        this.parent.activeItem = item;

        this.parent.SelectedItemsCountText = null;
        this.parent.SelectedItemsCount = 0;

        this.parent.IsSelected = false;
        this.parent.selecteCertificate.IsAllSelected = false;
        this.parent.LoadConnectedItems(null);


    }

    EditTicket(item: CertificateTicketListItem) {
        if (!AppTool.IsNullOrEmpty(item) && !this.parent.IsDisplayOnly) {
            //this.multiCertificatesService.GetCertificateConnectedItems(this.parent.DeclarationPM.Id, this.AttachmentTypeCode, this.ReqConfirmationTypeCode, this.ticket.CertificateExemptionTypeCode, this.CertificateNumber, this.ticket.ResConfirmationTypeCode)
            //    .subscribe((response: ServiceResponse) => {
            //if (!response.HasError) {
            var windowArgs: any = {};
            this.ticket.IsAllSelected = true;
            windowArgs.IsAllSelected = true;
            windowArgs.Ticket = this.ticket;
            windowArgs.DeclarationId = this.parent.DeclarationPM.Id;
            windowArgs.Items = [];
            windowArgs.Parent = this.parent;

            var logWindow = new LogitudeWindow();
            if (item.ticket.ReqConfirmationTypeName != null) {
                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.RequestedCerticate") + " - " + this.ticket.ReqConfirmationTypeName + " " + this.ticket.ReqConfirmationTypeCode;
            }

            else {
                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.RequestedCerticate");
            }


            logWindow.Width = 550;
            logWindow.Height = 300;

            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "ok") {
                    this.ReloadCertificates($event);
                    this.parent.CD.reattach();
                    this.parent.RefreshEntity();
                }
            });
            this.parent.CD.detach();
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Certificate/CreateEditTicketComponent');
                    //}


                //});



        }
    }


    ReloadCertificates(message: string) {
        if (message == "ok") {
            this.parent.GetCertificates("ok");
        }
    }
}

