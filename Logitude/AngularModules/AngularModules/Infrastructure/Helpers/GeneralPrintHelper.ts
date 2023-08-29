
declare var System: any;
declare var window: any;
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../DataContracts/ApiQueryFilters';
import { SessionLocator } from '../Utilities/SessionLocator';
import { SessionInfo } from '../Utilities/SessionInfo';
import { LogitudeWindow } from '../../Controls/Windows/LogitudeWindow';

import { DocumentTypePM } from '../../Common/EntityPMs/DocumentTypePM';
import { DocumentOutPM } from '../../Common/EntityPMs/DocumentOutPM';
import { MessageWindow } from '../../Controls/Windows/MessageWindow';
import { DocumentTypeListService } from '../../Common/Services/StandardLists/DocumentTypeListService';
import { DocumentTypePMExtendedService } from '../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';

import { DocumentTypeList } from '../../Common/EntityLists/DocumentTypeList';
import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { DocumentOutPMService } from '../../Common/Services/ExtendedPMs/DocumentOutPMService';
import { DocsOutDataViewModel } from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';
import { ARInvoicePMService } from '../../Invoice/Services/StandardPMs/ARInvoicePMService';
import { ARInvoicePM } from '../../Invoice/EntityPMs/ARInvoicePM';


export class GeneralPrintHelper {
    public ObjectTableName: string;
    public CurrentObjectTableId: string;
    ChildObjectTableId: string;
    ChildObjectTableName: string;

    public DocumentTypeCode: string;
    public ChildEntityId: string;

    ChildReference: string;
    public EntityId: string;
    documentTypeList: DocumentTypeList;
    documentOutPM: DocumentOutPM;
    documentTypePM: DocumentTypePM;
    documentTypePMService: DocumentTypePMExtendedService;
    documentOutPMService: DocumentOutPMService;
    public IsLoadPrintControl: boolean = true;
    public IsStartPrint: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(objecttablename: string, documentTypeCode: string, entityId: string, childEntityId: string, childReference: string, childObjectTableId: string) {
        this.ObjectTableName = objecttablename;
        if (!AppTool.IsNullOrEmpty(documentTypeCode)) {
            this.DocumentTypeCode = documentTypeCode.toUpperCase();
        }

        this.CurrentObjectTableId = window.ObjectTables.filter(d => d.Name == objecttablename)[0].Id;
        this.EntityId = entityId == "null" || !entityId ? "" : entityId;
        this.ChildEntityId = childEntityId == "null" || !childEntityId ? "" : childEntityId;
        this.ChildObjectTableId = childObjectTableId == "null" || !childObjectTableId ? "" : childObjectTableId;
        this.ChildReference = childReference == "null" || !childReference ? "" : childReference;

        if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId)) {
            this.ChildObjectTableName = window.ObjectTables.filter(d => d.Id == this.ChildObjectTableId)[0].Name;
        }

        this.documentTypePMService = new DocumentTypePMExtendedService();
        this.documentOutPMService = new DocumentOutPMService();


    }


    ShowPrintControl(documentTypeTemplate: string = null, StatusCode: string = null, ApprovedDate: Date = null, showController: boolean = true) {
        if (this.IsStartPrint) return;
        let apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
        let documentTypeListService = new DocumentTypeListService();
        documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.documentTypeList = myResult.filter(d => d.Code.toUpperCase() == this.DocumentTypeCode)[0];
                if (this.documentTypeList) {
                    if (this.documentTypeList.DocumentTypeDefaultReportTemplateId) {
                        this.GetDocumentOut(documentTypeTemplate, StatusCode, ApprovedDate, showController);

                    }
                    else this.ShowMessage("Document type of code " + this.DocumentTypeCode + " has no default template");
                }

                else {
                    if (this.ObjectTableName == "APPayment") {
                        this.ShowMessage("There is no document type for A/P Payment please go to maintenance and add it!");
                    }
                    else if (this.ObjectTableName == "ARPayment") {
                        this.ShowMessage("There is no document type for A/R Payment please go to maintenance and add it!");
                    }

                    else this.ShowMessage("Document type of code " + this.DocumentTypeCode + " not exists");
                }
            }
        });

    }

    GetDocumentOut(documentTypeTemplate: string = null, StatusCode: string = null, ApprovedDate: Date = null, showController: boolean = true) {
        
        var signHSM=!showController
        if (this.IsStartPrint) return;
        this.IsStartPrint = true;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(this.EntityId, SessionInfo.LoggedUserTenant, this.ChildEntityId, this.documentTypeList.Id).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.documentOutPM = myResult;

                if (!this.documentOutPM) {
                    this.documentOutPMService.getCreateDocumentOut(this.documentTypeList.Id, this.EntityId, this.ChildEntityId, this.ChildReference, this.CurrentObjectTableId, SessionInfo.LoggedUserTenant, documentTypeTemplate,signHSM).subscribe((res: any) => {
                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                this.documentOutPM = myResult;

                                this.LoadDocumentTypePm(StatusCode, ApprovedDate, showController);
                            }
                        }

                        else {
                            this.CurrentSession.StopBusyIndicator();
                            this.IsStartPrint = false;
                        }

                    });
                }
                else {
                    this.LoadDocumentTypePm(StatusCode, ApprovedDate, showController);
                }
            }

            else {
                this.IsStartPrint = false;
                this.CurrentSession.StopBusyIndicator();
            }
        });

    }

    public LoadDocumentTypePm(StatusCode: string = null, ApprovedDate: Date = null, showController: boolean = true) {
        this.documentTypePMService.getSingleDocumentType(this.documentTypeList.Id, this.documentOutPM.Id, SessionInfo.LoggedUserTenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentTypePM = myResult;
                    if (showController)
                        this.LoadPrintControl(StatusCode, ApprovedDate);
                    else {
                        
                        this.IsStartPrint = false;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            }

            else {
                this.CurrentSession.StopBusyIndicator();
                this.IsStartPrint = false;
            }
        });
    }

    LoadPrintControl(StatusCode: string = null, ApprovedDate: Date = null) {

        this.IsStartPrint = false;
        this.CurrentSession.StopBusyIndicator();
        var documentOutPmLists = new Array<DocumentOutPM>();
        documentOutPmLists.push(this.documentOutPM);
        var SelectedInternalDocument = new DocsOutDataViewModel(this.documentTypePM, this.EntityId, this.documentOutPM.ChildEntityId, this.CurrentObjectTableId, this.ChildObjectTableId, this.documentOutPM.ChildEntityReference,
            documentOutPmLists, null, null, null);

        SelectedInternalDocument.IsNotFromDocsOutListOpenPrintControl = true;
        SelectedInternalDocument.IsAWBWizard = false;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 760;

        var heightwindwo: number = this.documentOutPM.IssuedDate ? 552 : 502;
        logitudeWindow.Height = heightwindwo;
        logitudeWindow.DataContext = SelectedInternalDocument;
        logitudeWindow.Title = "Print " + this.documentTypePM.Name;
        logitudeWindow.WindowArgs = { "statusCode": StatusCode, "ApprovedDate": ApprovedDate };
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/PrintDocumentComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if (this.CurrentSession.CurrentEditComponent) {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                //if (this.ChildObjectTableName == "ARInvoice" || this.ObjectTableName == "ARInvoice") {
                //    this.UpdateInvoicePrintProperties();
                //}

                //else {
                //    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                //}
            }
        });
    }

    private UpdateInvoicePrintProperties() {
        var invoiceId: string;
        if (this.ChildObjectTableName == "ARInvoice") {
            invoiceId = this.ChildEntityId;
        }
        else if (this.ObjectTableName == "ARInvoice") {
            invoiceId = this.EntityId;
        }

        if (!AppTool.IsNullOrEmpty(invoiceId)) {
            var service: ARInvoicePMService = new ARInvoicePMService();
            service.get(invoiceId).subscribe((res: any) => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var invoice: ARInvoicePM = pmResponse.Result;
                    if (invoice) {
                        invoice.PrintByUserId = invoice.IssuedByUserId;
                        invoice.PrintDate = DateTool.GetCurrentDateAsUtc();

                        switch (invoice.StatusCode) {
                            case "AD":
                            case "VD":
                            case "PD":
                            case "PP":
                            case "AR":
                            case "AC":
                                {
                                    if (invoice.IsFromInterestBatchInvoice == false) {
                                        invoice.IsPrinted = true;
                                    }

                                    break;
                                }
                        }

                        service.update(invoice).subscribe((res: any) => {
                            var pmResponse: ServiceResponse = res;
                            if (!pmResponse.HasError) {
                                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            }
                        });
                    }
                }
            });
        }
    }

    public ShowMessage(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }
}
