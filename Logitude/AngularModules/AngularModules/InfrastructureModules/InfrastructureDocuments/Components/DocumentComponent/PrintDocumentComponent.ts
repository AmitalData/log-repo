declare var window: any;
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DocumentCopiesViewModel } from './DocsOut/ViewModel/DocumentCopiesViewModel';
import { DocsOutDataViewModel } from './DocsOut/ViewModel/DocsOutDataViewModel';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { DocumentOutPMService } from '../../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import { DocumentTypePMExtendedService } from '../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import { ExportDocumentService } from '../../../../Common/Services/DocumentServices/ExportDocumentService';
import { DocumentTypeTemplateListExtendedService } from '../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import { DocumentTypeCustomFieldService } from '../../../../Common/Services/ExtendedPMs/DocumentTypeCustomFieldService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { HtmlEditorService } from '../../../../Common/Services/DocumentServices/HtmlEditorService';
import { DocumentOutPM } from '../../../../Common/EntityPMs/DocumentOutPM';
import { DocumentTypePM } from '../../../../Common/EntityPMs/DocumentTypePM';
import { DocumentTypeCustomFieldPM } from '../../../../Common/EntityPMs/DocumentTypeCustomFieldPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DocumentTypeTemplateViewModel } from './DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { DocumentCustomFieldsArgs } from './DocsOut/Filters/DocumentCustomFieldsArgs';
import { FroalaEditorFilters } from './DocsOut/Filters/FroalaEditorFilters';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ServiceLocator } from '../../../../Infrastructure/Locators/ServiceLocator';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { ExportDocumentArgs } from '../../../../Infrastructure/DataContracts/ExportDocumentArgs';
import { DocumentsExecutionLogList } from '../../../../Common/EntityLists/DocumentsExecutionLogList';
import { DocumentsExecutionLogListExtendedService } from '../../../../Common/Services/ExtendedLists/DocumentsExecutionLogListExtendedService';
import { interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';
declare var Base64ToString: any;
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ObjectsLocator } from "../../../../Infrastructure/Locators/ObjectsLocator";


@Component({
    selector: 'PrintDocument',
    templateUrl: './PrintDocumentView.html',
    providers: [DocumentTypePMExtendedService, DocumentTypeCustomFieldService, DocumentOutPMService, ExportDocumentService, DocumentTypeTemplateListExtendedService, HtmlEditorService],
    inputs: ['DataContext']
})

export class PrintDocumentComponent extends BaseComponent implements OnInit {
    @Output() OnCloseWindow = new EventEmitter();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public ItemsSource: DocumentCopiesViewModel[];
    public AddedDocumentTypeCopyViewModels: DocumentCopiesViewModel[];
    public RemovedDocumentTypeCopyViewModels: DocumentCopiesViewModel[];
    public Items: DocumentCopiesViewModel[];
    HtmlEditEditor: string;
    public documentCopieViewModelSelected: DocumentCopiesViewModel;
    public DocumentTypeCustomFieldLists: DocumentTypeCustomFieldPM[];
    public Title: string;
    BuildButtonIsEnabled: boolean = true;
    IsCancelHtmlDocumentBluid: boolean;
    IsCancelStimulDocumentBluid: boolean;
    public LastBuildDate: Date;
    public CurrentDocumentTypeTemplateList: DocumentTypeTemplateViewModel;

    public IsRefreshPrintConrol: boolean;
    public DocumentTypeTemplateLists: DocumentTypeTemplateViewModel[];
    public TargetEntityName: string = "Shipment";
    public idArray: string[];
    public DataContext: DocsOutDataViewModel;
    public CurrentDocumentOut: DocumentOutPM;
    public DocumentTypeload: DocumentTypePM;
    public lastCount: number = 0;
    public EntityId: string;
    public isAWBWizard: boolean;
    public HtmlEditorData: string;
    IsShowDocumentCustomFields: boolean;
    public BusyIndicatorText: string;
    public DocumentCustomFieldsArgs: DocumentCustomFieldsArgs;
    PopupSendScreenWidth: string;
    PopupSendScreenHeight: string;
    LastBuildDateVisible: boolean;
    public PrintAllCopiesBtnVisible: boolean;
    public PrintAllCopiesBtnDisable: boolean;
    IsBuildDocumentViaWorkerRole: boolean = false;

    IsEnableEditDocument: boolean = false;
    IsEnableManageDocument: boolean = false;
    IsAWBPackage: boolean = false;
    public IsAccountingActivated = false;
    private statusCode: String;
    private ApprovedDate: Date;
    public SignatureFaild: boolean = false;
    public NoSignature: boolean = false;
    public SignatureSuccess: boolean = false;

    public SelectedAsDefaultBtnVisible: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    private documentsExecutionLogListExtendedService: DocumentsExecutionLogListExtendedService;
    IsTemplateDisabled: boolean = false;
    public EntityPM: any = null;
    constructor(public _documentTypeCustomFieldService: DocumentTypeCustomFieldService, public _documentOutPMService: DocumentOutPMService, public _documentTypePMService: DocumentTypePMExtendedService, public _exportDocumentService: ExportDocumentService, public _documentTypeTemplateListExtendedService: DocumentTypeTemplateListExtendedService, public _htmlEditorService: HtmlEditorService) {
        super();

        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        if (FeatureLocator.HasFeaturePermession("DocumentType", "EDITPRINTEDDOCUMENTS")) {
            this.IsEnableEditDocument = true;
        }
        this.CheckManageDocumentFeature();
        this.CheckAWBPackage();
    }

    ngOnInit() {

        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        var IsFromInterestBatchInvoice = false;

        if (this.EntityPM.IsFromInterestBatchInvoice) {
            IsFromInterestBatchInvoice = true;
        }
        if (ObjectsLocator.GlobalSetting.WorkEnvironment === 'cloud' && IsFromInterestBatchInvoice == false && SessionLocator.TenantPM.AccountingActivated && this.DataContext.invoiceType != "IT") {
            this.UpdateDocumentsAutomatically();
        }
    }

    UpdateDocumentsAutomatically() {
        this.CurrentDocumentOut = this.DataContext.CurrentDocument;
        this.LoadCopiesControl();

        this._documentOutPMService.getSingleDocumentOutPM(this.DataContext.CurrentDocument.Id,
            this.DataContext.CurrentDocument.Tenant).subscribe((res: any) => {
                const pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    const myResult = pmResponse.Result;
                    if (myResult) {
                        this.CurrentDocumentOut = myResult;
                        this.DataContext.CurrentDocument = myResult;
                        this.UpdateDocument();
                    }
                }

            });



    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.statusCode = args.statusCode;
            this.ApprovedDate = args.ApprovedDate;

        }

    }


    CheckManageDocumentFeature() {
        if (FeatureLocator.HasFeaturePermession("DocumentType", "MANAGEDOCUMENTTEMPLATES")) {
            this.IsEnableManageDocument = true;
        }
    }
    ObjectTableId: string;
    ObjectTableName: string;

    IsNoTemplateDefult: boolean = false;
    InitializeCopeisControl() {

        if (!this.CurrentDocumentOut.DocumentTemplateEditorTool && !this.IsQuotationDocument) {
            this.IsNoTemplateDefult = true;
        }

        this.GetTemplates();


        // this.IsLoading = true;

        if (this.CurrentDocumentOut.DocumentTemplateEditorTool == "R") {
            this.IsShowDocumentCustomFields = false;
            this.PrintAllCopiesBtnDisable = true;
        }
        else {
            this.BuildButtonIsEnabled = true;
            this.PrintAllCopiesBtnDisable = false;
        }




        if (this.DataContext.DocumentTypePM.IsDocumentOneTimePrintLimited) {
            this.SelectedAsDefaultBtnVisible = false;
            this.BuildButtonIsEnabled = false;
        }

        this.LoadCopiesControl();


        this.LoadDocumentCustomFields();

        if (this.CurrentDocumentOut.IssuedDate) {
            this.LastBuildDate = this.CurrentDocumentOut.IssuedDate;//.toString();
            if (!this.isAWBWizard) {
                this.LastBuildDateVisible = true;
            }
        }
        else {
            this.LastBuildDateVisible = false;
        }



    }

    public LoadDocumentCustomFields() {

        this.DocumentCustomFieldsArgs = new DocumentCustomFieldsArgs();
        this.DocumentCustomFieldsArgs.EditCustomField = false
        this.DocumentCustomFieldsArgs.ObjectTableId = this.ObjectTableId;
        this.DocumentCustomFieldsArgs.DocumentTypeId = this.DataContext.DocumentTypePM.Id;
        this.DocumentCustomFieldsArgs.EntityId = this.EntityId;

        if (!this.IsSystemAdditionalPrintingFields) {

            this._documentTypeCustomFieldService.getDocumentTypeCustomFieldsByDocument(this.CurrentDocumentOut.Tenant, this.DocumentCustomFieldsArgs.DocumentTypeId).subscribe((res: any) => {


                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {

                        this.DocumentTypeCustomFieldLists = myResult;
                        if (this.DocumentTypeCustomFieldLists.length > 0) {

                            this.DocumentCustomFieldsArgs.DocumentTypeCustomFieldLists = this.DocumentTypeCustomFieldLists;
                            this.IsShowDocumentCustomFields = true;


                        }
                        else {
                            this.IsShowDocumentCustomFields = false;

                        }
                    }
                    else {
                        this.IsShowDocumentCustomFields = false;

                    }

                }
                else this.StopBusyIndicator();








            });
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.PrintingFieldsScreenCode)) {

                this.DocumentCustomFieldsArgs.ScreenCode = this.PrintingFieldsScreenCode;
                this.DocumentCustomFieldsArgs.ObjectTableName = this.ObjectTableName;
                this.DocumentCustomFieldsArgs.EntityPM = this.DataContext.EntityPM;
                this.IsShowDocumentCustomFields = true;

            }
        }

    }

    CheckDocumentTemplate() {
        if (!AppTool.IsNullOrEmpty(this.DataContext.DocumentTypePM.Code)) {
            switch (this.DataContext.DocumentTypePM.Code.toUpperCase()) {
                case "740":
                case "714":
                case "716":
                case "716SD":
                case "784":
                case "781":
                case "999S":
                case "999M":
                case "999C":
                case "740L":
                case "740HL":
                case "CMR":
                case "SCMR":
                case "785A":
                case "785O":
                case "PAO":
                case "PAA":
                case "CPA":
                case "CPO":
                case "CPI":
                case "CPIO":
                case "CPE":
                case "PROF":
                case "APP":
                case "ARP":
                case "LCLL":
                case "SFBL":
                case "PALI":
                case "PALN":
                case "740PP":
                case "714PP":
                case "BCS":
                case "IFI":
                case "GAPS":
                case "DOR":
                case "COO":
                case "ARNT":
                case "PGDF":
                case "DORE":
                case "TBOL":
                case "REOR":
                case "TML":
                case "LCOT":
                case "SBOL":
                case "MBOL":
                case "DEOR":
                case "BCO":
                case "SELE":
                case "DELI":
                case "EXCU":
                case "890":
                case "999P":
                case "999MP":
                case "AVISC":
                case "LAL":
                case "ESU":
                case "860":
                case "865":
                case "852":
                case "PROD":
                case "ATME":
                case "OPPA":
                case "DRA":
                case "SVDF":
                case "CA":
                case "CRCT":
                case "CRCD":
                case "CRCC":
                case "PCRC":
                case "999CI":
                case "JRPR":
                case "HORD":
                case "CDE":
                case "CDR":
                case "OPPB":
                case "SOPI":
                case "INVS":
                case "ETO":
                case "ITO":
                case "SSN":
                case "CRCW":
                case "CRCO":
                case "CRCI":
                case "CRCCU":
                case "CRCCM":
                case "CRCCB":
                case "OMBC":
                case "WHL":
                case "DESCH":
                case "WESL":
                case "SBOLP":
                case "CRR":
                case "BDE":
                case "WELB":
                case "SHCO":
                case "INMA":
                case "ABOCO":
                case "SHCMR":
                case "TEST":
                case "NCR":
                case "782":
                case "ARINV":
                case "CARICOM":
                    return true;

                default:
                    return false;
            }
        }
        else return false;
    }

    showDialog(pageScreen: string) {

        if (this.IsNoTemplateFound || this.IsNoTemplateDefult) {
            return;
        }

        if (!this.CheckDocumentTemplate() && this.CurrentDocumentOut.DocumentTemplateEditorTool == "S") {
            return;
        }


        ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.DocumentTypeload.Name + " Building");

        if (!this.CheckDocumentTemplate() && this.CurrentDocumentOut.DocumentTemplateEditorTool == "S") {
            return;
        }


        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();


        logWindow.Title = "Edit Document";

        if (pageScreen == "EditDocument") {
            windowArgs.ModePage = "StimaulEdit";
            if (this.isAWBWizard) {
                windowArgs.ModePage = "AWBWizardEdit";
            }
        }
        else if (pageScreen == "AdditionalPrintingFields") {
            this.IsShowDocumentCustomFields = false;
        } else {
            logWindow.Title = "Manage Template";
        }


        windowArgs.PageType = pageScreen;
        windowArgs.DataViewModel = this;
        windowArgs.WindowHeight = window.innerHeight - 100;
        windowArgs.WindowWidth = window.innerWidth - 100;
        windowArgs.CurrentDocument = this.CurrentDocumentOut;
        windowArgs.DocumentTypePM = this.DocumentTypeload;
        var documentTypeTemplate = this.DocumentTypeTemplateLists.filter(d => d.Id == this.DataContext.CurrentDocument.DocumentTemplateId)[0];
        windowArgs.Subject = documentTypeTemplate ? documentTypeTemplate.Subject : "";
        windowArgs.EntityId = this.EntityId;
        windowArgs.DocumentTypeCopyId = this.DataContext.documentOutCopyId;
        windowArgs.DocumentTypeCustomFieldLists = this.DocumentTypeCustomFieldLists;

        windowArgs.ChildEntityId = this.ChildEntityId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;
        windowArgs.ChildReference = this.ChildReference;
        windowArgs.ObjectTableId = this.ObjectTableId;

        logWindow.WindowArgs = windowArgs;
        logWindow.Width = windowArgs.WindowWidth;
        logWindow.Height = windowArgs.WindowHeight;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/EditDocumentComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (this.IsRefreshPrintConrol) {

                this.UpdateDocument();
                this.IsRefreshPrintConrol = false;

            }
        });



    };
    IsSendClose: boolean = false;
    showSendControlDialog(documentCopie: DocumentCopiesViewModel) {



        if (documentCopie.CurrentDocumentOutCopy) {

            this.IsSendClose = false;
            ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, documentCopie.CurrentDocumentOutCopy.DocoumentTypeCopyName + " Sending");

            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var percentagewidthwindow = widthwindow * 0.252;
            var percentageHeightwindow = heighthwindow * 0.1764705;
            var sendWindowHeight = heighthwindow - percentageHeightwindow;
            var sendWindowWidth = widthwindow - percentagewidthwindow;
            if (sendWindowWidth < 1000) sendWindowWidth = 1000;
            if (sendWindowHeight < 600) sendWindowHeight = 600;
            this.DataContext.documentOutCopyId = documentCopie.CurrentDocumentOutCopy.Id;
            this.DataContext.ModeSendDocument = "Send";
            this.DataContext.PageRequestSendComponent = "PrintDocumentComponent";
            var logWindow = new LogitudeWindow();
            logWindow.Width = this.DataContext.WindowWidth = sendWindowWidth;
            logWindow.Height = this.DataContext.WindowHeight = sendWindowHeight;
            logWindow.DataContext = this.DataContext;


            logWindow.Title = "Send Message";
            logWindow.NotifyOnClose = true;
            logWindow.IsShowCloseButton = true;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SendDocumentComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {

                if ($event == "SendEnd") {
                    if (documentCopie.CurrentDocumentOutCopy.DocumentTypeCopyId == documentCopie.CurrentDocumentType.LimitedPrintCopyId && documentCopie.CurrentDocumentType.IsDocumentOneTimePrintLimited) {
                        documentCopie.IsPrintButtonEnabled = false;
                        var loggedContactName = SessionLocator.LoggedUserPM.EnglishName;
                        documentCopie.PrintedByMessage = "This document is already printed by " + loggedContactName;
                    }
                    if (!this.IsSendClose) {
                        this.IsSendClose = true;
                        this.CurrentSession.FireEvent("RefreshDocumentOutSend");
                    }

                }
            });

        }
    }

    SetDataContext(dataContext: any) {

        this.DataContext = dataContext;
        this.setArguments(this.DataContext);
    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }

    IsNoTemplateFound: boolean = false;
    GetTemplates() {

        this.CurrentSession.StartBusyIndicatorLoading();
        this._documentTypeTemplateListExtendedService.getDocumentTypeTemplateListsForDocumentType(this.DataContext.DocumentTypePM.Id, this.DataContext.DocumentTypePM.Tenant).subscribe((res: any) => {
            this.DocumentTypeTemplateLists = new Array<DocumentTypeTemplateViewModel>();

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    // if ((this.ObjectTableName == "ARInvoice") && entityPM.IsFromInterestBatchInvoice && entityPM.IsPrinted == false) {
                    //     myResult.filter(d => d.InActive == false && d.IsDefault == true).forEach((item) => {
                    //         if (item.TemplateType == "P") {
                    //             this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                    //         }
                    //     });    
                    // } else {
                    myResult.filter(d => d.InActive == false).forEach((item) => {
                        if (item.TemplateType == "P") {
                            this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                        }
                    });
                    // }

                    if (!this.IsNoTemplateFound && !this.IsQuotationDocument) {
                        if (this.DocumentTypeTemplateLists.length == 0) {
                            this.ShowMessage(TextCodeTranslator.Translate("DocsOut.M.NoTemplatesFound"));
                            this.IsNoTemplateFound = true;
                        }
                        else {
                            if (this.IsNoTemplateDefult) this.ShowMessage("Please select template as default");

                        }
                    }

                    if (this.DocumentTypeTemplateLists.length > 0) {

                        var item = this.DocumentTypeTemplateLists.filter(r => r.Id == this.CurrentDocumentOut.DocumentTemplateId)[0];

                        if (item == null) item = this.DocumentTypeTemplateLists.filter(r => r.Id == this.DataContext.DocumentTypePM.DocumentTypeDefaultReportTemplateId)[0];
                        if (item == null) item = this.DocumentTypeTemplateLists[0];

                        this.CurrentDocumentTypeTemplateList = item;


                    }
                }

            }

            else {
                this.StopBusyIndicator();
                var messageError: string;
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    messageError = pmResponse.ErrorsArray[0];
                }
                this.ShowMessage(messageError);
            }


        });

    }

    LoadDocumentTemplateStimulSoftData() {

        this.CurrentDocumentOut.DocumentTemplateId = this.CurrentDocumentTypeTemplateList.Id;
        this.CurrentDocumentOut.DocumentTemplateEditorTool = this.CurrentDocumentTypeTemplateList.EditorTool;
        this._documentOutPMService.putDocumentOut(this.CurrentDocumentOut).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.DataContext.CurrentDocument = this.CurrentDocumentOut = myResult;
                    ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.DocumentTypeload.Name + " Building");
                    this.BuildCurrentCopies(this.Items, "");
                }
            }
        });

    }

    alertselected(selectedTemplate) {

        this.CurrentDocumentTypeTemplateList = selectedTemplate;
        //stimal start

        if (this.CurrentDocumentTypeTemplateList.EditorTool == "S" && this.CurrentDocumentTypeTemplateList.TemplateType == "P") {
            if (!this.IsCancelStimulDocumentBluid) {
                if (this.CurrentDocumentOut != null) {
                    var editableFieldsBody: string = this.CurrentDocumentOut.EditableFields ? Base64ToString(this.CurrentDocumentOut.EditableFields) : null;
                    if (editableFieldsBody && editableFieldsBody.indexOf("<Items isList='true' count='0' />") == -1) {
                        var confirmWindow: ConfirmWindow = new ConfirmWindow();
                        confirmWindow.Width = 400;
                        confirmWindow.Show("Do you want to lose the data you have entered manually to your edited template?");
                        confirmWindow.YesButtonText = "Yes";
                        confirmWindow.NoButtonText = "No";
                        confirmWindow.WindowClosed.subscribe((event: any) => {
                            if (confirmWindow.Yes) {
                                this.CurrentDocumentOut.EditableFields = null;
                                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
                                this._documentOutPMService.putDocumentOut(this.CurrentDocumentOut).subscribe((res: any) => {
                                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                                    this.LoadDocumentTemplateStimulSoftData();
                                });
                            }
                            else this.LoadDocumentTemplateStimulSoftData();
                        });

                    } else this.LoadDocumentTemplateStimulSoftData();

                }
            }
            else {

                this.IsCancelStimulDocumentBluid = false;
            }

            //LoadDocumentCustomFieldsControl();
        }


        // html
        if (this.CurrentDocumentTypeTemplateList.EditorTool == "R" && this.CurrentDocumentTypeTemplateList.TemplateType == "P") {

            if (!this.IsCancelHtmlDocumentBluid) {
                //  this.IsCancelCloseEditWindow = true;

                this.CurrentDocumentOut.DocumentTemplateId = this.CurrentDocumentTypeTemplateList.Id;
                this.DataContext.CurrentDocument = this.CurrentDocumentOut;
                this._documentOutPMService.putDocumentOut(this.CurrentDocumentOut).subscribe((res: any) => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            this.CurrentDocumentOut = myResult;
                            this.ReBluidHtmlDocument(this.DocumentTypeload.DocumentTypeCopies[0].Id);
                        }

                    } else this.StopBusyIndicator();

                });

            }
            else {
                this.IsCancelHtmlDocumentBluid = false;
            }

        }



    }

    OnmMouseOver(item: DocumentCopiesViewModel) {

        this.Items.forEach((item) => { item.VisiblePrint = false; });
        item.VisiblePrint = true;

    }

    OnmMouseleave(item: DocumentCopiesViewModel) {

        this.Items.forEach((item) => { item.VisiblePrint = false; });


    }


    SortItemSource() {

        if (this.Items) {

            var entityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            if ((this.ObjectTableName == "ARInvoice") && entityPM.IsFromInterestBatchInvoice) {
                var originalCopy = this.Items.filter(x => x.IsOriginal == true)[0];
                if (originalCopy) {
                    if (originalCopy.IsPrintButtonEnabled) {
                        //this.Items = this.Items.filter(x => x.IsOriginal == true);
                        this.IsTemplateDisabled = false;
                    } else {
                        this.IsTemplateDisabled = true;
                    }
                }
            }
            
            if (this.ObjectTableName == "ARInvoice" && (this.SignatureFaild ||this.SignatureSuccess))
                this.Items = this.Items.filter(x => x.IsOriginal == true)
            this.Items = this.Items.sort(d => d.IndexOrder);
        }
    }

    LoadCopiesControl() {


        this.ItemsSource = new Array<DocumentCopiesViewModel>();

        this._documentTypePMService.getSingleDocumentType(this.DataContext.DocumentTypePM.Id, this.CurrentDocumentOut.Id, this.CurrentDocumentOut.Tenant).subscribe((res: any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.DocumentTypeload = myResult;
                    if (this.DocumentTypeload != null) {
                        this.DataContext.DocumentTypePM = myResult;
                        if (this.DataContext.DocumentTypePM.DocumentTypeCopies != null) {

                            this.DocumentTypeload.DocumentTypeCopies.forEach((item) => {
                                this.ItemsSource.push(new DocumentCopiesViewModel(item, this.CurrentDocumentOut, this.EntityId, this.ChildEntityId, this.ObjectTableId, this.ChildObjectTableId, this.DocumentTypeload, this.ChildReference));
                            });

                            if (ObjectsLocator.GlobalSetting.WorkEnvironment === 'cloud' && SessionLocator.TenantPM.AccountingActivated) {
                                this.ItemsSource = this.ItemsSource.filter((value, index, self) =>
                                    index === self.findIndex((t) => (
                                        t.Id === value.Id
                                    ))
                                )
                            }

                            var item = this.ItemsSource.filter(d => d.IsSelected)[0];
                            var anySelected = false;
                            if (!item) {
                                this.ItemsSource.forEach((item) => {
                                    item.IsHideSetSelectedAsDefaultBtn = true;
                                    item.IsSelected = item.IsSelectedByDefault;
                                    this.SelectedAsDefaultBtnVisible = false;
                                    anySelected = true;
                                });

                            }

                            this.Items = this.ItemsSource;
                            this.SortItemSource();

                        }

                    }


                    if (this.ItemsSource.length == 1) {
                        this.PrintAllCopiesBtnVisible = false;
                    }
                    else {

                        if (this.ItemsSource.length > 1) {
                            this.PrintAllCopiesBtnVisible = true;
                        }

                    }

                    this.CopiesControlLoaded(this.ItemsSource);

                }
            }


        });


    }


    compareDate(firstDate: Date, secondDate: Date) {
        const lastPrintDate = new Date(firstDate);
        const approvedDate = new Date(secondDate);

        if (lastPrintDate.getFullYear() < approvedDate.getFullYear()) {
            return true;
        } else if (lastPrintDate.getFullYear() == approvedDate.getFullYear()) {


            if (lastPrintDate.getMonth() < approvedDate.getMonth()) {
                return true;
            } else if (lastPrintDate.getMonth() == approvedDate.getMonth()) {

                if (lastPrintDate.getDate() < approvedDate.getDate()) {
                    return true;
                } else if (lastPrintDate.getDate() == approvedDate.getDate()) {

                    if (lastPrintDate.getHours() < approvedDate.getHours()) {
                        return true;
                    } else if (lastPrintDate.getHours() == approvedDate.getHours()) {

                        if (lastPrintDate.getMinutes() < approvedDate.getMinutes()) {
                            return true;
                        } else if (lastPrintDate.getMinutes() == approvedDate.getMinutes()) {

                            if (lastPrintDate.getSeconds() < approvedDate.getSeconds()) {
                                return true;
                            } else {
                                return false;
                            }

                        } else {
                            return false;
                        }

                    } else {
                        return false;
                    }

                } else {
                    return false;
                }

            } else {
                return false;
            }


        } else {
            return false;
        }
    }
    CopiesControlLoaded(copies: Array<DocumentCopiesViewModel>) {


        if (copies != null) {


            if (copies.length == 1) {
                this.PrintAllCopiesBtnVisible = false;
                this.SelectedAsDefaultBtnVisible = false;
            }
            else if (copies.length > 1) {
                this.PrintAllCopiesBtnVisible = true;

            }



            if (FeatureLocator.IsPackage_EAWB()) {
                copies.forEach(d => d.IsSelectedByDefault = true);
            }


            if (this.DocumentTypeload.IsDocumentOneTimePrintLimited) {
                copies.forEach((item) => {




                    item.CurrentDocumentTypeCopy.IsSelectedByDefault = true;
                    var LastPrintDateLessThenApprovedDate = null;

                    if (item.CurrentDocumentOutCopy != null) {

                        LastPrintDateLessThenApprovedDate = this.compareDate(item.CurrentDocumentOutCopy.LastPrintDate, this.ApprovedDate);

                    }

                    if (this.statusCode != null && this.IsAccountingActivated) {

                        if (this.statusCode == "DR" || (this.statusCode != "DR" && (LastPrintDateLessThenApprovedDate))) {
                            item.IsPrintButtonEnabled = true;
                            item.PrintedByMessage = '';
                        }
                    }


                });
            }



            this.lastCount = copies.filter(d => d.CurrentDocumentTypeCopy.IsSelectedByDefault).length;

            if ((this.CurrentDocumentOut.DocumentOutCopies.length == 0 || this.CurrentDocumentOut.NeedsRebuild)) {

                var editorToolCode = null;


                if (this.CurrentDocumentOut.DocumentTemplateEditorTool != null && this.CurrentDocumentOut.DocumentTemplateEditorTool != "") {
                    editorToolCode = this.CurrentDocumentOut.DocumentTemplateEditorTool;

                }


                else if (this.DataContext.DocumentTypePM.DocumentTypeDefaultEditorTool != null && this.DataContext.DocumentTypePM.DocumentTypeDefaultEditorTool != "") {
                    editorToolCode = this.DocumentTypeload.DocumentTypeDefaultEditorTool;
                }

                if (editorToolCode) {
                    if (editorToolCode == "S") this.BuildCurrentCopies(copies, "New");
                    if (editorToolCode == "R") {

                        this.Items = new Array<DocumentCopiesViewModel>();
                        this.Items = copies;
                        this.ReBluidHtmlDocument(this.CurrentDocumentOut.DocumentTypeId);
                    }
                }
                else {

                    this.StopBusyIndicator();
                }
            }

            else {
                this.Items = new Array<DocumentCopiesViewModel>();
                this.Items = copies;
                this.StopBusyIndicator();

            }



            this.SortItemSource();


        }

    }


    HeaderHeight: number;
    FooterHeight: number;
    ReBluidHtmlDocument(documentTypeCopyId: string) {
        this.IsDocumentBuildSucceeded = false;
        this.IsDocumentBuildFailed = false;

        var documentTypeId = this.CurrentDocumentOut.DocumentTypeId;
        var shipmentId = this.CurrentDocumentOut.EntityId;

        var item = null;
        if (this.DocumentTypeTemplateLists) item = this.DocumentTypeTemplateLists.filter(d => d.Id == this.CurrentDocumentOut.DocumentTemplateId)[0];

        if (item && !AppTool.IsNullOrEmpty(item.HtmlResolve)) {
            this.HeaderHeight = item.TemplateHeaderHeight;
            this.FooterHeight = item.TemplateFooterHeight;
            this.HtmlEditorData = item.HtmlResolve;
            this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);
            this.SaveReportData(documentTypeCopyId);
            item.HtmlResolve = null;
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this._htmlEditorService.getEditorHtmlData(this.CurrentDocumentOut.Id, shipmentId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, false, this.CurrentDocumentOut.DocumentTemplateId, "", "Edit").subscribe((res: any) => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.HtmlEditorData = "<header>" + "<height>" + "<div style='display:none'>" + myResult.HeaderHeight + "</div></height>" + myResult.HeaderHtml + "</header>" + myResult.Htmlstring + "<footer>" + "<height>" + "<div style='display:none'>" + myResult.FooterHeight + "</div></height>" + myResult.FooterHtml + "</footer>";
                        this.HeaderHeight = myResult.HeaderHeight;
                        this.FooterHeight = myResult.FooterHeight;
                    }
                    this.StopBusyIndicator();
                    this.CurrentSession.StartBusyIndicator("Building document...");
                    this.SaveReportData(documentTypeCopyId);

                } else this.CurrentSession.StopBusyIndicator();



            });



        }
    }

    docIds: string;

    SaveReportData(documentTypeCopyId: string) {

        var filter = new FroalaEditorFilters();
        filter.DocumentOutId = this.DataContext.CurrentDocument.Id;
        filter.DocumentTypeCopyId = documentTypeCopyId;
        filter.Tenant = SessionInfo.LoggedUserTenant;
        filter.HtmlString = this.HtmlEditorData;
        filter.HeaderHeight = this.HeaderHeight;
        filter.FooterHeight = this.FooterHeight;
        filter.EntityId = this.EntityId;
        filter.ChildEntityId = this.ChildEntityId;
        filter.DocumentTypeId = this.DataContext.DocumentTypePM.Id;

        this._htmlEditorService.saveEditedReportToServer(filter).subscribe((res: any) => {

            this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.docIds = myResult;
                    this.idArray = this.docIds.split(',');

                    this.AddedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
                    this.RemovedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
                    var anySelected = false;

                    this.Items.forEach((copy) => {
                        if (!this.DocumentTypeload.IsDocumentOneTimePrintLimited) {

                            if (copy.IsSelected) {
                                anySelected = true;

                                this.AddedDocumentTypeCopyViewModels.push(copy);
                            }

                            if (copy.Exists && !copy.IsSelected) {

                                this.RemovedDocumentTypeCopyViewModels.push(copy);
                            }
                        }

                        else {
                            copy.IsSelected = true;
                            anySelected = true;

                            this.AddedDocumentTypeCopyViewModels.push(copy);
                        }

                    });


                    if (anySelected) {
                        this.lastCount = this.AddedDocumentTypeCopyViewModels.length;
                        var numberOfCopy = this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected).length;
                        if (numberOfCopy > 0) {
                            this.CurrentDocumentOut.XamlDocumentId = this.idArray[1];
                            this.CurrentDocumentOut.IssuedByUserId = SessionInfo.LoggedUserId;
                            this.CurrentDocumentOut.IsChangeIssuedDate = true;
                            this.SaveContext();
                        }
                        else this.StopBusyIndicator();
                    }
                    else {

                        this.StopBusyIndicator();
                    }


                }
                else {

                    this.StopBusyIndicator();


                }

            }
            else this.StopBusyIndicator();



        });

    }

    BuildCurrentCopies(copies: Array<DocumentCopiesViewModel>, mode: string) {
        if (!this.ObjectTableName) {
            this.StopBusyIndicator();
            return;
        }
        this.IsDocumentBuildSucceeded = false;
        this.IsDocumentBuildFailed = false;

        this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);

        this.AddedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
        this.RemovedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();

        var anySelected = false;


        copies.forEach((copy) => {
            if (!this.DocumentTypeload.IsDocumentOneTimePrintLimited) {
                if (copy.IsSelected || copies.length == 1) {
                    anySelected = true;

                    if (copies.length == 1) {
                        copy.IsSelected = true;
                        copy.CurrentDocumentTypeCopy.IsSelectedByDefault = true;
                        copy.IsDiableSelctedDocumentTypeCopy = true;
                    }
                    else {
                        copy.IsDiableSelctedDocumentTypeCopy = false;
                    }
                    this.AddedDocumentTypeCopyViewModels.push(copy);
                }
                if (copy.Exists && !copy.IsSelected) {

                    var index = this.RemovedDocumentTypeCopyViewModels.indexOf(copy, 0);
                    if (index) {
                        this.RemovedDocumentTypeCopyViewModels.splice(index, 1);
                    }

                }
            }

            else {
                copy.IsSelected = true;
                anySelected = true;
                this.AddedDocumentTypeCopyViewModels.push(copy);
            }


        });
        if (anySelected) {

            this.lastCount = this.AddedDocumentTypeCopyViewModels.length;

            var numberOfCopy = this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected).length;
            var count: number = 0;
            var copiesIds: string[] = [];

            this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected).forEach((copy) => {
                if (!this.IsBuildDocumentViaWorkerRole) {

                    this._exportDocumentService.getDocumentPdfFile(this.DataContext.DocumentTypePM.Id, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId, this.CurrentDocumentOut.Id, this.CurrentDocumentOut.Tenant, copy.CurrentDocumentTypeCopy.Id, SessionLocator.LoggedUserId).subscribe((res: any) => {
                        count += 1;
                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;

                            if (myResult != null) {
                                copy.Status = "Success";
                                copy.Exists = true;

                                if (numberOfCopy == count) {
                                    this.UpdateDocumentOutData();
                                    this.SaveContext();
                                }
                            }
                            else this.StopBusyIndicator();




                        } else {
                            var messageError: string;
                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                messageError = pmResponse.ErrorsArray[0];
                            }

                            this.ShowMessage(messageError);
                            this.StopBusyIndicator();
                        }

                    });
                }

            });

            if (this.IsBuildDocumentViaWorkerRole) {
                this.BliudDocumentViewWorkerRole(this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected));
            }


            if (mode == "New" && this.AddedDocumentTypeCopyViewModels) {

                var copies = new Array<DocumentCopiesViewModel>();
                this.Items.forEach((copy) => {
                    var item = this.AddedDocumentTypeCopyViewModels.filter(d => d.Id == copy.Id)[0];
                    if (item) copies.push(item);
                    else copies.push(copy);

                });

                this.Items = copies;

                this.SortItemSource();
            }




        }

        else {

            this.StopBusyIndicator();
            this.ShowMessage(TextCodeTranslator.Translate("DocsOut.M.SelectCopyThenRebuild"));
        }


    }



    UpdateDocumentOutData() {
        if (this.CurrentDocumentOut) {
            this.CurrentDocumentOut.Issued = true;
            this.CurrentDocumentOut.NeedsRebuild = false;
            this.DataContext.Issued = true;
            this.CurrentDocumentOut.IssuedByUserId = SessionInfo.LoggedUserId;
            this.CurrentDocumentOut.IsChangeIssuedDate = true;
        }
    }


    IsDocumentBuildSucceeded: boolean = false;
    IsDocumentBuildFailed: boolean = false;




    LoadDocumentOut() {
        this._documentOutPMService.getSingleDocumentOutPM(this.DataContext.CurrentDocument.Id, this.DataContext.CurrentDocument.Tenant).subscribe((res: any) => {
            this.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.HasFollowUp = this.DataContext.CurrentDocument.HasFollowUp;
                    this.CurrentDocumentOut = myResult;
                    this.DataContext.CurrentDocument = myResult;
                    if (this.AddedDocumentTypeCopyViewModels != null) {
                        this.AddedDocumentTypeCopyViewModels.forEach((copy) => {
                            copy.RefereshDocumentOutCopies(this.CurrentDocumentOut);
                        });

                        this.DataContext.HasFile = true;
                        if (this.DataContext.Issued != true) {
                            this.DataContext.Issued = true;
                        }
                        this.LastBuildDate = this.CurrentDocumentOut.IssuedDate;
                        this.DataContext.IssuedDate = this.CurrentDocumentOut.IssuedDate;
                        this.DataContext.IssuedByUserName = this.CurrentDocumentOut.IssuedByUserName;
                        ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.DataContext.DocumentTypePM.Name + " Built");

                        if (this.DataContext.IsNotFromDocsOutListOpenPrintControl) {
                            this.CurrentSession.FireEvent("RefreshDocumentOutPrint");
                        }

                        this.IsDocumentBuildSucceeded = true;
                        if (this.CurrentSession.CurrentEditComponent) this.CurrentSession.CurrentEditComponent.ReloadEntityPM();


                    }
                }

            }
            else this.StopBusyIndicator();


        });

    }


    SaveContext() {


        this._documentOutPMService.putDocumentOut(this.CurrentDocumentOut).subscribe((res: any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myResult = pmResponse.Result;
                this.LoadDocumentOut();



            } else this.StopBusyIndicator();




        });

    }

    BliudDocumentViewWorkerRole(documentTypeCopyLists: DocumentCopiesViewModel[]) {
        if (documentTypeCopyLists.length > 0) {
            var exportDocumentArgs = new ExportDocumentArgs();
            exportDocumentArgs.DocumentTypeId = this.DataContext.DocumentTypePM.Id;
            exportDocumentArgs.EntityId = this.EntityId;
            exportDocumentArgs.ObjectTableId = this.ObjectTableId;
            exportDocumentArgs.ChildEntityId = this.ChildEntityId;
            exportDocumentArgs.ChildObjectTableId = this.ChildObjectTableId;
            exportDocumentArgs.CurrentDocumentOutId = this.CurrentDocumentOut.Id;
            exportDocumentArgs.LoggedContactId = SessionLocator.LoggedUserId;
            exportDocumentArgs.Tenant = SessionLocator.Tenant;
            exportDocumentArgs.DocumentTypeName = this.DataContext.DocumentTypePM.Name;
            exportDocumentArgs.DocumentTypeTemplateId = this.CurrentDocumentOut.DocumentTemplateId;
            exportDocumentArgs.CurrentDocumentTypeCode = this.DataContext.DocumentTypePM.Code;
            exportDocumentArgs.ObjectTableName = this.ObjectTableName;
            exportDocumentArgs.DocumentTemplateEditorTool = this.CurrentDocumentOut.DocumentTemplateEditorTool;

            exportDocumentArgs.DocumentTypeCopyIdsList = documentTypeCopyLists.map(function (a) { return a.Id; });
            this._exportDocumentService.BuildDocumentViaWorkerRole(exportDocumentArgs).subscribe((myResponse: ServiceResponse) => {
                var result: any = myResponse.Result;
                if (!myResponse.HasError && result) {
                    this.StartCheckDocumentBuildViaWorkerRoleTimer(result, documentTypeCopyLists);
                } else {

                    this.StopBusyIndicator();


                    var messageError: string;
                    if (myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                        messageError = myResponse.ErrorsArray[0];
                    }
                    this.ShowMessage(messageError);
                }

            });
        }
    }



    initializeStartCheckDocumentBuildViaWorkerRoleTimer() {
        return interval(250).pipe(timeInterval());
    }



    private StartCheckDocumentBuildViaWorkerRoleTimerTimersub: any = null;
    IsStartCheckDocumentBuildViaWorkerRoleTimer: boolean = false;
    StartCheckDocumentBuildViaWorkerRoleTimer(documentExecutionLogId, documentTypeCopyLists) {
        if (this.IsStartCheckDocumentBuildViaWorkerRoleTimer) {
            this.StartCheckDocumentBuildViaWorkerRoleTimerTimersub.unsubscribe();
        }

        this.IsStartCheckDocumentBuildViaWorkerRoleTimer = true;
        this.StartCheckDocumentBuildViaWorkerRoleTimerTimersub = this.initializeStartCheckDocumentBuildViaWorkerRoleTimer().subscribe(respose => {


            if (!this.IsStartCheckDocumentBuildViaWorkerRoleTimer) {
                this.StartCheckDocumentBuildViaWorkerRoleTimerTimersub.unsubscribe();
                this.IsStartCheckDocumentBuildViaWorkerRoleTimer = false;
                return;
            }


            if (this.IsStartCheckDocumentBuildViaWorkerRoleTimer) {

                if (this.documentsExecutionLogListExtendedService == null) {
                    this.documentsExecutionLogListExtendedService = new DocumentsExecutionLogListExtendedService();
                }

                this.CurrentSession.StartBusyIndicator("Loading ...");

                this.documentsExecutionLogListExtendedService.GetDocumentsExecutionLogList(documentExecutionLogId).subscribe((res: any) => {
                    var pmResponse: ServiceResponse = res;
                    var documentsExecutionLogList: DocumentsExecutionLogList = res.Result;

                    if (this.IsStartCheckDocumentBuildViaWorkerRoleTimer) {
                        if (pmResponse.HasError || !documentsExecutionLogList || (documentsExecutionLogList && (documentsExecutionLogList.StatusCode == "D" || documentsExecutionLogList.StatusCode == "F" || documentsExecutionLogList.StatusCode == "T"))) {
                            this.StartCheckDocumentBuildViaWorkerRoleTimerTimersub.unsubscribe();
                            this.IsStartCheckDocumentBuildViaWorkerRoleTimer = false;
                            this.StopBusyIndicator();
                        }

                        if (!pmResponse.HasError) {

                            if (documentsExecutionLogList) {
                                if (documentsExecutionLogList.StatusCode == "F" || documentsExecutionLogList.StatusCode == "T") {
                                    this.ShowMessage(documentsExecutionLogList.ExceptionMessage);
                                }
                                else if (documentsExecutionLogList.StatusCode == "D") {
                                    this.StopBusyIndicator();

                                    documentTypeCopyLists.forEach((copy) => {
                                        copy.Status = "Success";
                                        copy.Exists = true;
                                    });


                                    this.LoadDocumentOut();


                                    //this.CurrentDocumentOut.Issued = true;
                                    //this.CurrentDocumentOut.NeedsRebuild = false;
                                    //this.DataContext.Issued = true;
                                    //this.CurrentDocumentOut.IssuedByUserId = SessionInfo.LoggedUserId;
                                    //this.CurrentDocumentOut.IsChangeIssuedDate = true;
                                    //this.SaveContext();
                                }
                            }
                            else {
                                this.ShowMessage("Documents execution Log not found");
                            }

                        }
                        else {

                            var messageError: string;
                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                messageError = pmResponse.ErrorsArray[0];
                            }
                            this.ShowMessage(messageError);





                        }

                    }

                });

            }
        });

    }

    PrintMethod(item: DocumentCopiesViewModel) {
        if (item.CurrentDocumentOutCopy) {


            var copyId: string = item.CurrentDocumentOutCopy.Id;
            var documentName = item.CurrentDocumentOutCopy.Tenant + "~" + item.CurrentDocumentOutCopy.Id;
            if (this.DocumentTypeload.IsDocumentOneTimePrintLimited && this.DataContext.DocumentTypePM.LimitedPrintCopyId == item.CurrentDocumentOutCopy.DocumentTypeCopyId) {
                documentName = documentName + "~" + item.CurrentDocumentOutCopy.DocumentId + "~" + SessionInfo.LoggedUserId;

            }

            this.ViewPage(item.CurrentDocumentOutCopy.DocoumentTypeCopyName, copyId);
            if(this.ObjectTableName == "ARInvoice" && this.NoSignature && item.IsOriginal){
                item.IsPrintButtonEnabled = false;
                var loggedContactName = SessionLocator.LoggedUserPM.EnglishName;
                item.PrintedByMessage = "This document is already printed by " + loggedContactName;
            }
            else if (item.CurrentDocumentOutCopy.DocumentTypeCopyId == item.CurrentDocumentType.LimitedPrintCopyId && item.CurrentDocumentType.IsDocumentOneTimePrintLimited) {

                if (this.IsAccountingActivated && this.statusCode != "DR") {
                    item.IsPrintButtonEnabled = false;
                    var loggedContactName = SessionLocator.LoggedUserPM.EnglishName;
                    item.PrintedByMessage = "This document is already printed by " + loggedContactName;
                } else if (!this.IsAccountingActivated) {
                    item.IsPrintButtonEnabled = false;
                    var loggedContactName = SessionLocator.LoggedUserPM.EnglishName;
                    item.PrintedByMessage = "This document is already printed by " + loggedContactName;
                }

            }

        }
    }


    ViewPage(docoumentTypeCopyName: string, id: string) {

        ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, docoumentTypeCopyName + " Viewing");

        DownloadManager.DownloadPage(id, this.CurrentDocumentOut.SecurityId, false, this.ObjectTableName);

    }


    public setArguments(item: DocsOutDataViewModel) {

        this.IsBuildDocumentViaWorkerRole = true;
        this._entityResourceService.getEntityResourceByTableName("DocsOut").subscribe((response: any) => {


            if (!item.DocumentTypePM) {
                this.CurrentSession.StartBusyIndicator("Loading...");

                this._documentTypePMService.GetSinglePMWithOutInclude(item.Id, SessionLocator.Tenant).subscribe((res: any) => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        item.DocumentTypePM = pmResponse.Result;
                    }
                    this.CurrentSession.StopBusyIndicator();
                    this.Start(item);
                });

            }
            else this.Start(item);

        });
    }

    ChildEntityId: string;
    ChildObjectTableId: string;
    ChildReference: string;
    IsQuotationDocument: boolean = false;
    IsSystemAdditionalPrintingFields: boolean;
    PrintingFieldsScreenCode: string;
    BuildingDocumentText: string = "Building document...";
    Start(item: DocsOutDataViewModel) {
        this.DataContext = item;
        var buildingDocumentText: string = TextCodeTranslator.Translate("Accounting.General.O.BuildingDocument");

        if (!AppTool.IsNullOrEmpty(buildingDocumentText)) {
            this.BuildingDocumentText = buildingDocumentText;
        }


        this.ChildEntityId = item.ChildEntityId ? item.ChildEntityId : "";
        this.ChildObjectTableId = item.ChildObjectTableId ? item.ChildObjectTableId : "";
        this.ChildReference = item.ChildReference ? item.ChildReference : "";
        this.ObjectTableId = item.CurrentObjectTableId;
        this.CurrentDocumentOut = this.DataContext.CurrentDocument;
        this.DocumentTypeTemplateLists = new Array<DocumentTypeTemplateViewModel>();
        this.EntityId = item.EntityId;
        this.Title = "Print " + this.DataContext.DocumentTypePM.Name;
        this.isAWBWizard = this.DataContext.IsAWBWizard;
        this.IsSystemAdditionalPrintingFields = this.DataContext.DocumentTypePM.IsSystemAdditionalPrintingFields;
        this.PrintingFieldsScreenCode = this.DataContext.DocumentTypePM.PrintingFieldsScreenCode;

        var table = window.ObjectTables.filter(d => d.Id == this.ObjectTableId)[0];


        if (table) {
            this.ObjectTableId = table.Id;
            this.ObjectTableName = table.Name;

        }

        if ((this.ObjectTableName == "ARInvoice" || item.ChildObjectTableName == "ARInvoice") && (this.EntityPM.IsSigned==2)) {
            this.SignatureFaild = true;
        }
        else if ((this.ObjectTableName == "ARInvoice" || item.ChildObjectTableName == "ARInvoice") && (this.EntityPM.IsSigned==0||this.EntityPM.isSigned==null)) {  
            this.NoSignature = true;
        }
        else if ((this.ObjectTableName == "ARInvoice" || item.ChildObjectTableName == "ARInvoice") ) {
            this.SignatureSuccess = true;
        }
        if (this.ObjectTableName == "Quote" && item.DocumentTypeCode == "QUOTE") {

            this.IsQuotationDocument = true;
        }



        if (this.isAWBWizard) {
            this.PrintAllCopiesBtnVisible = false;
            this.SelectedAsDefaultBtnVisible = false;
            this.LastBuildDateVisible = false;

        }

        if (this.DataContext.DocumentTypePM.IsReadOnly) {
            this.PrintAllCopiesBtnVisible = false;
            this.SelectedAsDefaultBtnVisible = false;
            this.LastBuildDateVisible = false;

        }


        if (this.DataContext.DocumentTypePM.IsDocumentOneTimePrintLimited) {
            this._documentOutPMService.getSingleDocumentOutPM(this.CurrentDocumentOut.Id, this.CurrentDocumentOut.Tenant).subscribe((res: any) => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        myResult.HasFollowUp = this.CurrentDocumentOut.HasFollowUp;
                        this.CurrentDocumentOut = myResult;
                        this.InitializeCopeisControl();
                    }

                }


            });
        }

        else {

            this.InitializeCopeisControl();
        }



    }
    CheckAWBPackage() {
        var myCodes: string[] = [];
        myCodes.push("EAWB");
        this.IsAWBPackage = FeatureLocator.IsPackageOneOf(myCodes);
    }


    public UpdateDocument() {

        this.CurrentSession.CurrentWindow = this.CurrentSession.Windows.filter(d => d.Title == "Print " + this.DataContext.DocumentTypePM.Name)[0];

        ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.DocumentTypeload.Name + " Building");

        this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);

        if (this.CurrentDocumentOut.DocumentTemplateEditorTool == "S") {
            this.BuildCurrentCopies(this.Items, "");
        }

        else if (this.CurrentDocumentOut.DocumentTemplateEditorTool == "R") {
            this.ReBluidHtmlDocument(this.DocumentTypeload.DocumentTypeCopies[0].Id);

        }
    }

    PrintAllCopiesBtnClick() {

        var currentCount = this.Items.filter(d => d.IsSelected).length;
        if (this.DataContext.DocumentTypePM.IsDocumentOneTimePrintLimited) {
            if (this.IsAccountingActivated && this.statusCode != "DR") {
                this.Items.forEach((item) => {

                    if (item.CurrentDocumentOutCopy && item.CurrentDocumentType) {
                        if (item.CurrentDocumentOutCopy.DocumentTypeCopyId == item.CurrentDocumentType.LimitedPrintCopyId && AppTool.IsNullOrEmpty(item.PrintedByMessage)) {

                            var loggedContactName = SessionLocator.LoggedUserPM.EnglishName;
                            item.PrintedByMessage = "This document is already printed by " + loggedContactName;

                        }
                    }
                });
            } else if (!this.IsAccountingActivated) {
                this.Items.forEach((item) => {

                    if (item.CurrentDocumentOutCopy && item.CurrentDocumentType) {
                        if (item.CurrentDocumentOutCopy.DocumentTypeCopyId == item.CurrentDocumentType.LimitedPrintCopyId && AppTool.IsNullOrEmpty(item.PrintedByMessage)) {

                            var loggedContactName = SessionLocator.LoggedUserPM.EnglishName;
                            item.PrintedByMessage = "This document is already printed by " + loggedContactName;

                        }
                    }
                });
            }



        }

        if (currentCount != this.lastCount) {

            this.ShowMessage(TextCodeTranslator.Translate("DocsOut.M.RebuildThenPrintAgain"));
        }
        else {
            this.PrintAllDocs();
        }


    }

    public ShowMessage(message: string) {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message ? message : "error");
        this.IsDocumentBuildFailed = true;
    }
    SetSelectedAsDefaultBtnClick() {


        this.Items.forEach((item) => {
            item.CurrentDocumentTypeCopy.IsSelectedByDefault = item.IsSelected;
        });



        this._documentTypePMService.putDocumentType(this.DataContext.DocumentTypePM).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.DataContext.DocumentTypePM = myResult;
                    this.BuildCurrentCopies(this.Items, "");

                }

            }


        });

    }

    PrintAllDocs() {
        var token = ServiceHelper.GetLDocumentDownloadToken();
        window.open(ServiceHelper.GetLogitudeURL() + "WebPages/MergeAllPage.aspx?securityId=" + this.CurrentDocumentOut.SecurityId + "~" + SessionInfo.LoggedUserId + "&tempId=" + token);
    }
    IsSelect: boolean;
    public DocumentCopySelectedChange(item: DocumentCopiesViewModel, value: any) {

        item.IsSelected = value;

        this.SelectedAsDefaultBtnVisible = true;
        item.IsHideSetSelectedAsDefaultBtn = false;


    }

    StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();

    }

    RetrySignature(item:any) {
        this.CurrentSession.StartBusyIndicator("Retrying signature...");
        this._documentOutPMService.PutRetrySignature(item?.CurrentDocumentOut?.Id,item?.CurrentDocumentOutCopy?.Tenant).subscribe((res: any) => {
           
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
             }
            this.CurrentSession.CloseCurrentWindow();
            this.CurrentSession.StopBusyIndicator();

        })
    }
}
