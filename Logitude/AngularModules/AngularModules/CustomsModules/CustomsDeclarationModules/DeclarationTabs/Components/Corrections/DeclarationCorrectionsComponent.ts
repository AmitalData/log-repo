

declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool, DateTool} from '../../../../../Infrastructure/Tools';
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
import {ConsignmentPM} from '../../../../../Customs/EntityPMs/ConsignmentPM';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {AmendmentView} from '../../../../../Customs/EntityPMs/Extended/AmendmentView';
import { GeneralDataView } from '../../../../../Customs/EntityPMs/Extended/GeneralDataView';
import { error } from '../../../../../Customs/EntityPMs/Extended/AmendmentView';
import {DeclarationCorrectionView} from '../../../../../Customs/EntityPMs/Extended/DeclarationCorrectionView';
import {DeclarationConstraintPM} from '../../../../../Customs/EntityPMs/DeclarationConstraintPM';
import {DeclarationEventManager} from '../../../../../Customs/Utilities/DeclarationEventManager';

import {DeclarationWebService} from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import {DeclarationPMService} from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import {ConstraintApprovalRequestParams} from '../../../../../Customs/DataContract/RequestParams/ConstraintApprovalRequestParams';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import {VendorInsertUpdateDeleteMessageRequestParams, OperationTypes} from '../../../../../Customs/DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {DeclarationMessagesService} from '../../../../../Customs/Services/WebServices/DeclarationMessagesService';
import {SendRequestVIA} from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from '../../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: '././DeclarationCorrectionsComponent.html',
})

export class DeclarationCorrectionsComponent extends BaseComponent {
    public EntityPM: DeclarationPM;
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: any = this;
    public CurrentEditComponentId: string;
    public IsDisplayOnly: boolean = false;
    public DisplayOnlyMessage: string = "";
    public IsAmendmentDeficitInitiatedEnabled: boolean=false;
    public IsNoAmendmentsMsgVisible: boolean = false;
    ResponseData: INF_MSG_GenericResponseData;
    public IsOldAmendment: boolean;

    //Grids data
    AdditionalInformationlist: ObservableCollection = new ObservableCollection([]);
    AmendmentViewsList: ObservableCollection = new ObservableCollection([]);
    ReferenceList: ObservableCollection = new ObservableCollection([]);

    //Services
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService;
    private declarationPMService: DeclarationPMService = new DeclarationPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    TabsSource: any[] = [];
    SelectedTab: string = "";

    public get AmendmentRequestNumber() { return this.EntityPM ? this.EntityPM.AmendmentRequestNumber : null; }
    public set AmendmentRequestNumber(newValue: string) { this.EntityPM.AmendmentRequestNumber = newValue; }

    public get AmendmentissueDate() {

        if (this.EntityPM != null) {
            if (this.EntityPM.AmendmentissueDate != null) {
                var myFormats = DateTool.GetDateFormats(this.EntityPM.AmendmentissueDate);
                return myFormats.DateString + " " + myFormats.ShortTimeString;
            }
        }
        return null;


    }
    public set AmendmentissueDate(newValue: string) {  }


    public get AmendmentDeficitInitiated() { return this.EntityPM ? this.EntityPM.AmendmentDeficitInitiated : null; }
    public set AmendmentDeficitInitiated(newValue: boolean) { this.EntityPM.AmendmentDeficitInitiated = newValue; }


    public get AmendmentRejectionReasonName() { return this.EntityPM ? this.EntityPM.AmendmentRejectionReasonName : null; }
    public set AmendmentRejectionReasonName(newValue: string) { this.EntityPM.AmendmentRejectionReasonName = newValue; }


    public get VersionId() { return this.EntityPM ? this.EntityPM.VersionId : null; }
    public set VersionId(newValue: string) { this.EntityPM.VersionId = newValue; }


    public get AmendDeficitInitiatedReasTo() { return this.EntityPM ? this.EntityPM.AmendDeficitInitiatedReasTo : null; }
    public set AmendDeficitInitiatedReasTo(newValue: string) { this.EntityPM.AmendDeficitInitiatedReasTo = newValue; }

    public get AmendmentRemarks() { return this.EntityPM ? this.EntityPM.AmendmentRemarks : null; }
    public set AmendmentRemarks(newValue: string) { this.EntityPM.AmendmentRemarks = newValue; }

    LayoutDirection: string = 'ltr';


    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();

        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe((response:any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe((response:any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response:any) => {
                        this.EntityPM = this.entityArgs.EntityPM;
                        this.ObjectTableName = this.entityArgs.ObjectTableName;
                        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
                        this.Listen();
                        this.IsOldAmendment = this.entityArgs.EditComponent.SelectedTab.Code == "DCCO";
                        console.log("Declaration", this.EntityPM);
                        this.BuildTabs();

                        this.ReloadDeclarationCorrection();

   
                        this.UIProperties.SetEnabled("AmendmentRequestNumber", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("AmendmentissueDate", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("VersionId", this.ObjectTableName, false);
                        this.UIProperties.SetEnabled("AmendmentRejectionReasonName", this.ObjectTableName, false);

                        this.DisplayOnlyCheck();

                     });
                });
            });
        });
 

    }
    SelectionChanged(tab: any) {

        this.TabsSource.forEach(item => { // reset selection
            item.isSelected = false;
        });

        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab); return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    }
    BuildTabs() {
        this.SelectedTab = "Details";
        this.TabsSource.push({ Name: "Details", isSelected: true, Header: TextCodeTranslator.Translate("Customs.Declaration.O.CorrectionStatement") });
        this.TabsSource.push({ Name: "References", isSelected: false, Header: TextCodeTranslator.Translate("Customs.Declaration.O.References") });
        this.TabsSource.push({ Name: "Errors", isSelected: false, Header: TextCodeTranslator.Translate("Customs.Declaration.O.Errors") });

    }


    DisplayOnlyCheck() {

         var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
             if (this.EntityPM.AmendmentMessage != null && this.EntityPM.AmendmentMessage != "") {
                {
                     this.DisplayOnlyMessage = this.EntityPM.AmendmentMessage;
                    if (this.EntityPM.IsAmendmentDisplayOnly) this.IsDisplayOnly = this.EntityPM.IsAmendmentDisplayOnly;
                }
            }
             else {
                if (!this.EntityPM.AmendmentDeficitInitiated) this.UIProperties.SetEnabled("AmendDeficitInitiatedReasTo", this.ObjectTableName, false);

            }            this.SetScreenFieldsEditability();

            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
 
    }
    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("AmendmentRemarks", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AmendDeficitInitiatedReasTo", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AmendmentDeficitInitiated", this.ObjectTableName, !this.IsDisplayOnly);
       
        if ( !this.AmendmentDeficitInitiated)
            this.UIProperties.SetEnabled("AmendDeficitInitiatedReasTo", this.ObjectTableName, false);


    }


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
                        //this.DisplayOnlyCheck();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
            this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCCR") {
                        //this.DisplayOnlyCheck();
                        this.ReloadDeclarationCorrection();
                    }
                }
                })
            );;
        }
    }

    AmendmentDeficitInitiatedChecked(checked) {
        if (checked) {
            this.UIProperties.SetEnabled("AmendDeficitInitiatedReasTo", this.ObjectTableName, true);
            
        }

        else {
            this.UIProperties.SetEnabled("AmendDeficitInitiatedReasTo", this.ObjectTableName, false);
            this.AmendDeficitInitiatedReasTo = "";
        }
    }
    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    public IsDescriptionVisible: boolean = false;
    private description: string;
    public get Description() { return this.description }
    public set Description(newValue: string) {
        this.description = newValue;
    }

    //#region Get screen DATA
    SelectedGeneralIndex: number;
    GeneralData: GeneralDataView[] = [];
    ReloadDeclarationCorrection() {
        this.CurrentSession.StartBusyIndicatorLoading();

        //[1] GetDeclarationCorrections();
        this.declarationWebService.GetDeclarationCorrection(this.EntityPM.Id).subscribe((myServiceResponse: ServiceResponse) => {
                console.log("[Response] GetDeclarationConstraints : ", myServiceResponse.Result);
                var res: DeclarationCorrectionView = myServiceResponse.Result;

                if (!AppTool.IsNullOrEmpty(res)) {

                    this.GeneralData = [];
                    var amendmentViewsList = [];
                    var referenceList = [];

                    //sort data
                    var data = res.GeneralDataViews ? res.GeneralDataViews.sort((a, b) => { return (a.Version < b.Version) ? 1 : -1 }) : null

                    //build version list
                    this.BuildGeneralData(data); 

                    // select amendment for the first version
                    var general = this.GeneralData[0];
                    this.AmendmentViewsList = new ObservableCollection([]);
                    this.AdditionalInformationlist.InsertCollection(general.AdditionalInformation);
                    general.AmendmentViews.forEach(el => {
                        amendmentViewsList.push(el);
                    });

                     this.AmendmentViewsList.InsertCollection(amendmentViewsList);

                    this.GetResources(this.AmendmentViewsList.Collection);




                this.ReferenceList = new ObservableCollection([]);
                    if (!AppTool.IsNullOrEmpty(general.ReferenceViews)) {
                        general.ReferenceViews.forEach(el => {
                            referenceList.push(el);
                        });
                    }
                  

                    this.ReferenceList.InsertCollection(referenceList);

 

                    this.BuildSystemMessage(general.SystemMessageViews);

                } else {
                    if (!this.EntityPM.IsAmendment) this.IsNoAmendmentsMsgVisible = true;
                }
                this.CurrentSession.StopBusyIndicator();

            });
    }

    BuildGeneralData(data: GeneralDataView[]) {
        if (data) {

            var dateStr = TextCodeTranslator.Translate("Customs.Declaration.O.Date");
            var timeStr = TextCodeTranslator.Translate("Customs.Declaration.O.Time");
            var versionStr = TextCodeTranslator.Translate("Customs.Declaration.O.Version");

            this.VersionsList = [];

            data.forEach(el => {
                this.GeneralData.push(el);

                var myFormats = DateTool.GetDateFormats(el.CorrectionDate);
                var dateValue = myFormats.ShortDateString;
                var timeValue = myFormats.ShortTimeString;

                this.VersionsList.push(dateStr + ' ' + dateValue + ' ' + timeStr + ' ' + timeValue + ' ' + versionStr + ' ' + el.Version);
                this.SelectedVersion = this.VersionsList[0];
            });
        } else {
            console.log("No data to build versions list!!!!", data);
        }
    }

    GeneralDataSelectionChanged(selectedIndex: number) {
        var selectedGeneral = this.GeneralData[selectedIndex]; // new selected version

        this.AmendmentViewsList = new ObservableCollection([]);
        this.AdditionalInformationlist.InsertCollection(selectedGeneral.AdditionalInformation);
        selectedGeneral.AmendmentViews.forEach(el => {
            this.AmendmentViewsList.Insert(el);
        });

        this.GetResources(this.AmendmentViewsList.Collection);
    }

    BuildSystemMessage(data: error[]) {

        this.Description = null;
        for (var error of data) {

            if (error.ListVersionID == "A") {
                if (!this.Description) this.Description = "";
                this.Description += error.MessageError + ", ";
            }
        }

        if (this.Description) {
            this.IsDescriptionVisible = true;
            this.Description = this.Description.replace(/,\s*$/, ""); //remove last comma
        }
    }
    //#endregion

    //#region Version DDL
    VersionsList: string[] = [];
    public SelectedVersion: string = '';
    FilterItemClicked(itemValue: string) {
        if (this.SelectedVersion != itemValue) {
            this.SelectedVersion = itemValue;

            this.GeneralDataSelectionChanged(this.VersionsList.indexOf(this.SelectedVersion));
        }
    }
    //#endregion

    OpenAmendment(amendment) {
        console.log("open amendment: ", amendment);
        //selectedLine = amendment;

        this.EditEntity(amendment);

    }


    EditEntity(amendmentView: AmendmentView) {


        if (AppTool.IsNullOrEmpty(amendmentView)) {
            console.warn("[!] There is no Amendment View!");
        } else {
            this.CurrentSession.StartBusyIndicatorLoading();
            switch (amendmentView.EntityName.toLowerCase()) {

                case "declaration":
                case "consignment":
                    {
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 1000;
                        logWindow.Height = 700;
                        logWindow.ShowCloseButton = true;
                        logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
                        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/DeclarationGeneralComponent');
                        logWindow.WindowArgs = {
                            AmendmentView: amendmentView,
                            entityArgs: this.entityArgs,
                            entityPM: this.EntityPM,
                            IsDisplayOnly: this.IsDisplayOnly,
                        };
                        this.CurrentSession.StopBusyIndicator();
                        break;
                    }

                case "supplierinvoice": {

                    this.declarationWebService
                        .GetSupplierInvoiceBySequenceNumber(this.EntityPM.Id, +amendmentView.LineNumber, 0, 500)
                        .subscribe((response: ServiceResponse) => {
                            console.log("[Response] GetSupplierInvoiceBySequenceNumber: ", response);


                            var supplierInvoicePM = response.Result;


                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

                                this.CurrentSession.StartBusyIndicatorLoading();
                                var windowArgs: any = {};
                                windowArgs.EntityPM = supplierInvoicePM;
                                windowArgs.declarationPM = this.EntityPM;
                                windowArgs.AmendmentView = amendmentView;

                                var logWindow = new LogitudeWindow();
                                logWindow.Width = 1030;
                                logWindow.Height = 600;

                                if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

                                }
                                else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;

                                }
                                else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;

                                }
                                else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                                }
                                windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                                logWindow.ShowCloseButton = false;
                                logWindow.WindowArgs = windowArgs;
                                logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
                                logWindow.WindowClosed.subscribe(($event: any) => {

                                });
                              logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                                this.CurrentSession.StopBusyIndicator();

                            }
                            else {
                                this.CurrentSession.StopBusyIndicator();
                                var window = new MessageWindow();
                                window.Show("There is no invoice with such key in this declaration!!");
                            }
                        });

                    break;
                }

                case "supplierinvoiceitem":
                    {
                        if (AppTool.IsNullOrEmpty(amendmentView.LineNumber)) {
                            console.log("No line number", amendmentView);
                            return;
                        }
                        var lines = amendmentView.LineNumber.split(',');

                        var invSequence = +lines[0];
                        var itemSequence = +lines[1];

                        this.declarationWebService
                            .GetSupplierInvoiceWithItemBySequenceNumber(this.EntityPM.Id, invSequence, itemSequence, 0, 500)
                            .subscribe((response: ServiceResponse) => {
                                console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);


                                var supplierInvoicePM = response.Result;


                                if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

                                    this.CurrentSession.StartBusyIndicatorLoading();
                                    var windowArgs: any = {};
                                    windowArgs.EntityPM = supplierInvoicePM;
                                    windowArgs.declarationPM = this.EntityPM;
                                    windowArgs.AmendmentView = amendmentView;

                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 1030;
                                    logWindow.Height = 600;

                                    if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

                                    }
                                    else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;

                                    }
                                    else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;

                                    }
                                    else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                                    }
                                    windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                                    logWindow.ShowCloseButton = false;
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
                                    this.CurrentSession.StopBusyIndicator();

                                  logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                                    this.CurrentSession.StopBusyIndicator();

                                }
                                else {
                                    this.CurrentSession.StopBusyIndicator();
                                    var window = new MessageWindow();
                                    window.Show("There is no invoice with such key in this declaration!!");
                                }
                            });

                        break;
                    }

                case "supplierinvioceitemscertificate":
                    {
                        if (AppTool.IsNullOrEmpty(amendmentView.LineNumber)) {
                            console.log("No line number", amendmentView);
                            return;
                        }
                        var lines = amendmentView.LineNumber.split(',');

                        var invSequence = +lines[0];
                        var itemSequence = +lines[1];

                        this.declarationWebService
                            .GetSupplierInvoiceWithItemBySequenceNumber(this.EntityPM.Id, invSequence, itemSequence, 0, 500)
                            .subscribe((response: ServiceResponse) => {
                                console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);


                                var supplierInvoicePM: SupplierInvoicePM = response.Result;

                                if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

                                    this.CurrentSession.StartBusyIndicatorLoading();

                                    var invoiceItem = supplierInvoicePM.SupplierInvoiceItems.find(d => d.SequenceNumeric == amendmentView.ParentLine);

                                    if (!AppTool.IsNullOrEmpty(invoiceItem)) {

                                        // open certificate
                                        var windowArgs: any = {};
                                        windowArgs.SupplierInvoiceItemPM = invoiceItem;
                                        windowArgs.AmendmentView = amendmentView;
                                        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                                        windowArgs.InvoiceNumber = supplierInvoicePM.InvoiceNumber;
                                        windowArgs.SupplierInvoicePM = supplierInvoicePM;
                                        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");

                                        var logWindow = new LogitudeWindow();
                                        logWindow.Width = 1000;
                                        logWindow.Height = 600;
                                        //if (supplierInvoicePM.ClassificationCode != null) {
                                        //    logWindow.Title = "אישורים לפרט מכס" + " " + supplierInvoicePM.ClassificationCode;
                                        //}
                                        //else {
                                        //    logWindow.Title = "אישורים לפרט מכס";
                                        //}
                                        logWindow.ShowCloseButton = false;
                                        logWindow.WindowArgs = windowArgs;
                                        logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
                                        this.CurrentSession.StopBusyIndicator();

                                      logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent');
                                        //end open certificate
                                    }
                                    this.CurrentSession.StopBusyIndicator();

                                }
                                else {
                                    this.CurrentSession.StopBusyIndicator();
                                    var window = new MessageWindow();
                                    window.Show("There is no invoice with such key in this declaration!!");
                                }
                            });

                        break;
                    }

                default:
                    {
                        this.CurrentSession.StopBusyIndicator();
                        var window = new MessageWindow();
                        window.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                        break;
                    }
            }
        }
    }
    GetEditedScreenTitle(entityName: string, view: AmendmentView) {

        var title = "";
        switch (entityName.toLowerCase()) {
            case "declaration":
            case "consignment":
                {
                    title = TextCodeTranslator.Translate("Customs.Declaration");
                    break;
                }
            case "supplierinvoice":
                {
                    var declaration = this.EntityPM;
                    var supplierInvoicePM = declaration.SupplierInvoices.find(d => d.DeclarationId == declaration.Id && d.SequenceNumeric == view.Line);

                    if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
                        title = supplierInvoicePM.InvoiceNumber + "-" + declaration.DeclarationNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                    }
                    else if ((AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && !AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
                        title = declaration.DeclarationNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                    }
                    if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && (AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
                        title = supplierInvoicePM.InvoiceNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                    }
                    if ((AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && (AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
                        title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                    }
                    break;
                }
            case "supplierinvoiceitem":
                {
                    title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
                    break;
                }

            case "supplierinvioceitemscertificate":
                {
                    title = TextCodeTranslator.Translate("Customs.Declaration.O.Certificates");
                    break;
                }
        }
        return title;
    }


    // row selected
    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }

    //#region Get resources
    isResourcesLoaded: boolean = false;
    arrayLength = 0;
    GetResources(amendmentArray: any[]) {
        var tables = [];
        if (!AppTool.IsNullOrEmpty(amendmentArray)) {

            this.arrayLength = amendmentArray.length;
            amendmentArray.forEach((el) => {
                if (!AppTool.IsNullOrEmpty(el.FieldNameTextCode)) {

                    var splittedWords = el.FieldNameTextCode.split('.');
                    var objectTableName = splittedWords[0] + "." + splittedWords[1];

                    //#region Get resources
                    if (objectTableName == 'Customs.SupplierInvioceItemsCertificate') {
                        objectTableName = 'Customs.SupplierInvioceItemCertificat';
                    }
                    console.log("Get resources for ===> ", objectTableName);
                    this.EntityResourceService.getEntityResourceByTableName(objectTableName).subscribe((response:any) => {
                        if (this.arrayLength != 1) {
                            this.arrayLength--;
                        }
                        else {
                            //this.LoadConstriantsList(errors);
                        }
                    });
                    //#endregion 

                } else {
                    this.arrayLength--;
                    console.log("No FieldNameTextCode", el);
                }
            });

        }

    }
    //#endregion

    //#region XML Errors
    
    //EditEntity(amendmentView: DeclarationErrorView) {


    //    if (AppTool.IsNullOrEmpty(amendmentView)) {
    //        console.warn("[!] There is no declaraion error for the constraint!");
    //    } else {
    //        this.CurrentSession.StartBusyIndicatorLoading();
    //        switch (amendmentView.EntityName.toLowerCase()) {

    //            case "declaration":
    //            case "consignment":
    //                {
    //                        var logWindow = new LogitudeWindow();
    //                        logWindow.Width = 1000;
    //                        logWindow.Height = 700;
    //                        logWindow.ShowCloseButton = true;
    //                        logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
    //                        logWindow.Show('./Customs/Components/Declaration/EditTabs/General/DeclarationGeneralComponent');
    //                        logWindow.WindowArgs = {
    //                            DeclarationError: amendmentView,
    //                            entityArgs: this.entityArgs,
    //                            entityPM: this.EntityPM,
    //                            IsDisplayOnly: this.IsDisplayOnly,
    //                        };
    //                        this.CurrentSession.StopBusyIndicator();
    //                    break;
    //                }

    //            case "supplierinvoice": {

    //                this.declarationWebService
    //                    .GetSupplierInvoiceBySequenceNumber(amendmentView.DeclarationId, +amendmentView.LineNumber, 0, 500)
    //                    .subscribe((response: ServiceResponse) => {
    //                        console.log("[Response] GetSupplierInvoiceBySequenceNumber: ", response);


    //                        var supplierInvoicePM = response.Result;


    //                        if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

    //                            this.CurrentSession.StartBusyIndicatorLoading();
    //                            var windowArgs: any = {};
    //                            windowArgs.EntityPM = supplierInvoicePM;
    //                            windowArgs.declarationPM = this.EntityPM;
    //                            windowArgs.DeclarationError = amendmentView;

    //                            var logWindow = new LogitudeWindow();
    //                            logWindow.Width = 1030;
    //                            logWindow.Height = 600;

    //                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

    //                            }
    //                            else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;

    //                            }
    //                            else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;

    //                            }
    //                            else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

    //                            }
    //                            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
    //                            logWindow.ShowCloseButton = false;
    //                            logWindow.WindowArgs = windowArgs;
    //                            logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
    //                            logWindow.WindowClosed.subscribe(($event: any) => {

    //                            });
    //                            logWindow.Show('./Customs/Components/Declaration/EditTabs/SupplierInvoices/AddEditSupplierInvoiceComponent');
    //                            this.CurrentSession.StopBusyIndicator();

    //                        }
    //                        else {
    //                            var window = new MessageWindow();
    //                            window.Show("There is no invoice with such key in this declaration!!");
    //                        }
    //                    });

    //                break;
    //            }

    //            case "supplierinvoiceitem":
    //                {
    //                    if (AppTool.IsNullOrEmpty(amendmentView.LineNumber)) {
    //                        console.log("No line number", amendmentView);
    //                        return;
    //                    }
    //                    var lines = amendmentView.LineNumber.split(',');

    //                    var invSequence = +lines[0];
    //                    var itemSequence = +lines[1];

    //                    this.declarationWebService
    //                        .GetSupplierInvoiceWithItemBySequenceNumber(amendmentView.DeclarationId, invSequence, itemSequence, 0, 500)
    //                        .subscribe((response: ServiceResponse) => {
    //                            console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);


    //                            var supplierInvoicePM = response.Result;


    //                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

    //                                this.CurrentSession.StartBusyIndicatorLoading();
    //                                var windowArgs: any = {};
    //                                windowArgs.EntityPM = supplierInvoicePM;
    //                                windowArgs.declarationPM = this.EntityPM;
    //                                windowArgs.DeclarationError = amendmentView;

    //                                var logWindow = new LogitudeWindow();
    //                                logWindow.Width = 1030;
    //                                logWindow.Height = 600;

    //                                if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

    //                                }
    //                                else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;

    //                                }
    //                                else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;

    //                                }
    //                                else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
    //                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

    //                                }
    //                                windowArgs.IsDisplayOnly = this.IsDisplayOnly;
    //                                logWindow.ShowCloseButton = false;
    //                                logWindow.WindowArgs = windowArgs;
    //                                logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
    //                                this.CurrentSession.StopBusyIndicator();

    //                                logWindow.Show('./Customs/Components/Declaration/EditTabs/SupplierInvoices/AddEditSupplierInvoiceComponent');
    //                                this.CurrentSession.StopBusyIndicator();

    //                            }
    //                            else {
    //                                var window = new MessageWindow();
    //                                window.Show("There is no invoice with such key in this declaration!!");
    //                            }
    //                        });

    //                    break;
    //                }

    //            case "supplierinvioceitemscertificate":
    //                {
    //                    if (AppTool.IsNullOrEmpty(amendmentView.LineNumber)) {
    //                        console.log("No line number", amendmentView);
    //                        return;
    //                    }
    //                    var lines = amendmentView.LineNumber.split(',');

    //                    var invSequence = +lines[0];
    //                    var itemSequence = +lines[1];

    //                    this.declarationWebService
    //                        .GetSupplierInvoiceWithItemBySequenceNumber(amendmentView.DeclarationId, invSequence, itemSequence, 0, 500)
    //                        .subscribe((response: ServiceResponse) => {
    //                            console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);


    //                            var supplierInvoicePM: SupplierInvoicePM = response.Result;

    //                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

    //                                this.CurrentSession.StartBusyIndicatorLoading();

    //                                var invoiceItem = supplierInvoicePM.SupplierInvoiceItems.find(d => d.SequenceNumeric == amendmentView.ParentLine);

    //                                if (!AppTool.IsNullOrEmpty(invoiceItem)) {

    //                                    // open certificate
    //                                    var windowArgs: any = {};
    //                                    windowArgs.SupplierInvoiceItemPM = invoiceItem;
    //                                    windowArgs.DeclarationError = amendmentView;
    //                                    windowArgs.IsDisplayOnly = this.IsDisplayOnly;
    //                                    windowArgs.InvoiceNumber = supplierInvoicePM.InvoiceNumber;
    //                                    windowArgs.SupplierInvoicePM = supplierInvoicePM;
    //                                    var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");

    //                                    var logWindow = new LogitudeWindow();
    //                                    logWindow.Width = 1000;
    //                                    logWindow.Height = 600;
    //                                    //if (supplierInvoicePM.ClassificationCode != null) {
    //                                    //    logWindow.Title = "אישורים לפרט מכס" + " " + supplierInvoicePM.ClassificationCode;
    //                                    //}
    //                                    //else {
    //                                    //    logWindow.Title = "אישורים לפרט מכס";
    //                                    //}
    //                                    logWindow.ShowCloseButton = false;
    //                                    logWindow.WindowArgs = windowArgs;
    //                                    logWindow.Title = this.GetEditedScreenTitle(amendmentView.EntityName, amendmentView);
    //                                    this.CurrentSession.StopBusyIndicator();

    //                                    logWindow.Show('./Customs/Components/Declaration/EditTabs/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent');
    //                                    //end open certificate
    //                                }
    //                                this.CurrentSession.StopBusyIndicator();

    //                            }
    //                            else {
    //                                var window = new MessageWindow();
    //                                window.Show("There is no invoice with such key in this declaration!!");
    //                            }
    //                        });

    //                    break;
    //                }

    //            default:
    //                {
    //                    var window = new MessageWindow();
    //                    window.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
    //                    break;
    //                }
    //        }
    //    }
    //}
    //ShowXMLErrors(error) {
    //    //if (!AppTool.IsNullOrEmpty(error.Field)) {
    //    //    this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvoice", false, error.Description);
    //    //}

    //    var errors = [];
    //    if (!AppTool.IsNullOrEmpty(error.Description)) {
    //        var xmlErrors: any[] = error.Description.split(/,|:/);
    //        for (var xmlError of xmlErrors) {
    //            errors.push(xmlError);
    //        }
    //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
    //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
    //    }
    //    if (error.EntityName != null) {
    //        if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
    //            //if (OnShowXMLErrors != null) {
    //            //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
    //            //}
    //        }
    //    }
    //}
    //GetEditedScreenTitle(entityName: string, amendmentView: DeclarationErrorView) {

    //    var title = "";
    //    switch (entityName.toLowerCase()) {
    //        case "declaration":
    //        case "consignment":
    //            {
    //                title = TextCodeTranslator.Translate("Customs.Declaration");
    //                break;
    //            }
    //        case "supplierinvoice":
    //            {
    //                var declaration = this.EntityPM;
    //                var supplierInvoicePM = declaration.SupplierInvoices.find(d => d.DeclarationId == declaration.Id && d.SequenceNumeric == amendmentView.Line);

    //                if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
    //                    title = supplierInvoicePM.InvoiceNumber + "-" + declaration.DeclarationNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

    //                }
    //                else if ((AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && !AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
    //                    title = declaration.DeclarationNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

    //                }
    //                if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && (AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
    //                    title = supplierInvoicePM.InvoiceNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

    //                }
    //                if ((AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && (AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
    //                    title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

    //                }
    //                break;
    //            }
    //        case "supplierinvoiceitem":
    //            {
    //                title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
    //                break;
    //            }

    //        case "supplierinvioceitemscertificate":
    //            {
    //                title = TextCodeTranslator.Translate("Customs.Declaration.O.Certificates");
    //                break;
    //            }
    //    }
    //    return title;
    //}

    //#endregion

    GetFieldName(item) {
        var translation = TextCodeTranslator.Translate(item.FieldNameTextCode);
        if (AppTool.IsNullOrEmpty(translation)) {
            return item.Field;
        }
        return translation;
    }

}

