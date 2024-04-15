declare var System: any;
declare var window: any;
import {ServiceResponse} from '../DataContracts/ServiceResponse';

import {SessionLocator} from '../Utilities/SessionLocator';
import {SessionInfo} from '../Utilities/SessionInfo';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {DocumentTypePM} from '../../Common/EntityPMs/DocumentTypePM';
import {DocumentOutPM} from '../../Common/EntityPMs/DocumentOutPM';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {DocumentTypePMExtendedService} from '../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {DocumentOutPMService} from '../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {DocsOutDataViewModel} from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';
import {HtmlEditorService} from '../../Common/Services/DocumentServices/HtmlEditorService';
import {FroalaEditorFilters} from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/FroalaEditorFilters';
import {ExportDocumentService} from '../../Common/Services/DocumentServices/ExportDocumentService';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator'
import {DocumentCopiesViewModel} from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentCopiesViewModel';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {ShareDocument, ShipmentShareDocumentsData} from '../../Common/DataContracts/ShipmentShareDocumentsData';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {DocumentsFilingExtendedPMService} from '../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {DocumentsFilingPMService} from '../../Common/Services/StandardPMs/DocumentsFilingPMService';

export class SharedDocumentHelper {


    ChildEntityId: string;
    ChildReference: string;
    EntityId: string;
    TiggerViewModel: any;
    documentOutPM: DocumentOutPM;
    documentTypePM: DocumentTypePM;
    documentTypePMExtendedService: DocumentTypePMExtendedService;
    documentOutPMService: DocumentOutPMService;
    htmlEditorService: HtmlEditorService
    exportDocumentService: ExportDocumentService;

   documentsFilingExtendedPMService: DocumentsFilingExtendedPMService
   documentsFilingPMService: DocumentsFilingPMService;
    ObjectTableId: string;
    ShareDocument: ShareDocument;
    ShipmentShareDocumentsData: ShipmentShareDocumentsData;
    IsBuildDocumentRunning: boolean = false;
    IsUploadDocumentRunning: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

        this.documentTypePMExtendedService = new DocumentTypePMExtendedService();
        this.documentOutPMService = new DocumentOutPMService();
        this.htmlEditorService = new HtmlEditorService();
        this.exportDocumentService = new ExportDocumentService();


        this.documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this.documentsFilingPMService = new DocumentsFilingPMService();

        this.ObjectTableId = window.ObjectTables.filter(d => d.Name == "Shipment")[0].Id;
    }



    BuildDocument(shareDocument: ShareDocument) {
        if (!this.IsBuildDocumentRunning) {
            this.IsBuildDocumentRunning = true;
            this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);

            this.ShareDocument = shareDocument;

            if (shareDocument.DocumentOutPM) {
                this.LoadCopies();
            }
            else if (!AppTool.IsNullOrEmpty(shareDocument.DocumentOutId)) {
                this.documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(this.ShareDocument.EntityId, SessionInfo.LoggedUserTenant, "", this.ShareDocument.DocumentTypeId).subscribe((res:any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        this.ShareDocument.DocumentOutPM = myResult;
                        this.LoadCopies();
                    }
                    else this.StopBusyIndicator();
                });

            }
            else {
                this.documentOutPMService.getCreateDocumentOut(this.ShareDocument.DocumentTypeId, this.ShareDocument.EntityId, this.ChildEntityId, this.ChildReference, this.ObjectTableId, SessionInfo.LoggedUserTenant).subscribe((res: any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            this.ShareDocument.DocumentOutPM = myResult;
                            this.ShareDocument.DocumentOutId = myResult.Id;
                            this.LoadCopies();

                        } else this.StopBusyIndicator();


                    } else this.StopBusyIndicator();


                });
            }
        }
    }


     ItemsSource: DocumentCopiesViewModel[];
     AddedDocumentTypeCopyViewModels: DocumentCopiesViewModel[];
     RemovedDocumentTypeCopyViewModels: DocumentCopiesViewModel[];
     Items: DocumentCopiesViewModel[];
     lastCount: number;
     LoadCopies() {

         this.ItemsSource = new Array<DocumentCopiesViewModel>();
        if (this.ShareDocument.DocumentOutPM != null) {
            this.documentTypePMExtendedService.getSingleDocumentType(this.ShareDocument.DocumentTypeId, this.ShareDocument.DocumentOutPM.Id, SessionLocator.Tenant).subscribe((res:any) => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.documentTypePM = myResult;

                        if (this.documentTypePM != null) {

                            if (this.documentTypePM.DocumentTypeCopies != null) {
                                this.documentTypePM.DocumentTypeCopies.forEach((item) => {

                                    this.ItemsSource.push(new DocumentCopiesViewModel(item, this.ShareDocument.DocumentOutPM, this.ShareDocument.EntityId, "", this.ObjectTableId, "", this.documentTypePM, ""));
                                });

                                var item = this.ItemsSource.filter(d => d.IsSelected)[0];
                                var anySelected = false;
                                if (!item) {
                                    this.ItemsSource.forEach((item) => {
                                        item.IsSelected = item.IsSelectedByDefault;
                                        anySelected = true;
                                    });

                                }

                                this.Items = this.ItemsSource;

                            }

                        }

                        this.CopiesControlLoaded(this.ItemsSource);

                    }
                    else this.StopBusyIndicator();
                } else {
                    this.StopBusyIndicator();
                }



            });
        }

    }


    CopiesControlLoaded(copies: Array<DocumentCopiesViewModel>) {


        if (copies != null) {

            if (FeatureLocator.IsPackage_EAWB()) {
                copies.forEach(d => d.IsSelectedByDefault = true);
            }


            if (this.documentTypePM.IsDocumentOneTimePrintLimited) {
                copies.forEach((item) => { item.CurrentDocumentTypeCopy.IsSelectedByDefault = true; });
            }

            this.lastCount = copies.filter(d => d.CurrentDocumentTypeCopy.IsSelectedByDefault).length;



            var editorToolCode = null;


            if (!AppTool.IsNullOrEmpty(this.ShareDocument.DocumentOutPM.DocumentTemplateEditorTool)) {
                editorToolCode = this.ShareDocument.DocumentOutPM.DocumentTemplateEditorTool;

            }
            else if (!AppTool.IsNullOrEmpty(this.documentTypePM.DocumentTypeDefaultEditorTool)) {
                editorToolCode = this.documentTypePM.DocumentTypeDefaultEditorTool;
            }

            if (editorToolCode) {
                if (editorToolCode == "S") this.BuildCurrentCopies(copies);
                if (editorToolCode == "R") {

                    this.Items = new Array<DocumentCopiesViewModel>();
                    this.Items = copies;
                    this.ReBluidHtmlDocument();
                }
            }
            else {
                this.ShowMessage("No templates found for this document!");
                this.StopBusyIndicator();
            }
        }

    }



    BuildCurrentCopies(copies: Array<DocumentCopiesViewModel>) {


        this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);

        this.AddedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
        this.RemovedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();


        var anySelected = false;


        copies.forEach((copy) => {
            if (!this.documentTypePM.IsDocumentOneTimePrintLimited) {
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
            this.AddedDocumentTypeCopyViewModels.filter(d => d.IsSelected).forEach((copy) => {

                this.exportDocumentService.getDocumentPdfFile(this.documentTypePM.Id, this.ShareDocument.EntityId, this.ObjectTableId, "", "", this.ShareDocument.DocumentOutPM.Id, this.ShareDocument.DocumentOutPM.Tenant, copy.CurrentDocumentTypeCopy.Id, SessionLocator.LoggedUserId).subscribe((res:any) => {
                    count += 1;
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;

                        if (myResult != null) {
                            copy.Status = "Success";
                            copy.Exists = true;

                            if (numberOfCopy == count) {
                                this.ShareDocument.DocumentOutPM.NeedsRebuild = false;
                                this.ShareDocument.DocumentOutPM.Issued = true;
                                this.ShareDocument.DocumentOutPM.IssuedByUserId = SessionLocator.LoggedUserId;
                                this.ShareDocument.DocumentOutPM.IsChangeIssuedDate = true;
                           
                                this.SaveContext();

                          
                            }
                        }
                        else this.StopBusyIndicator();


                    } else {
                        if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                            this.ShowMessage(pmResponse.ErrorsArray[0]);
                        }
                        this.StopBusyIndicator();
                    }

                });


            });
            if (this.AddedDocumentTypeCopyViewModels) {

                var copies = new Array<DocumentCopiesViewModel>();
                this.Items.forEach((copy) => {
                    var item = this.AddedDocumentTypeCopyViewModels.filter(d => d.Id == copy.Id)[0];
                    if (item) copies.push(item);
                    else copies.push(copy);

                });

                this.Items = copies;
            }
        }

        else {

            this.StopBusyIndicator();
            this.ShowMessage("Please select a copy then rebuild!");
        }


    }


    SaveContext() {

        this.documentOutPMService.putDocumentOut(this.ShareDocument.DocumentOutPM).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myResult = pmResponse.Result;
                this.documentOutPMService.getSingleDocumentOutPM(this.ShareDocument.DocumentOutPM.Id, this.ShareDocument.DocumentOutPM.Tenant).subscribe((res:any) => {
                
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {

                            this.ShareDocument.DocumentOutPM = myResult;
                   
                            this.ShareDocument.Included = true;
                            this.ShareDocument.LastUpdateDate = this.ShareDocument.DocumentOutPM.IssuedDate;
                            this.ShareDocument.SecurityId = this.ShareDocument.DocumentOutPM.SecurityId;
                       
                            this.ShareDocument.ActionButtonLabel = "Update";

                            if (this.ShareDocument.DocumentOutPM && this.ShareDocument.DocumentOutPM.DocumentOutCopies) {
                                var documentOutCopy: any = this.ShareDocument.DocumentOutPM.DocumentOutCopies.filter(d => d.DocumentTypeCopyId== this.ShareDocument.DocumentTypeCopyId)[0];
                                if (documentOutCopy) {
                                    this.ShareDocument.DocumentId = documentOutCopy.Id;
                                }
                            }



                            ServiceLocator.SendTotangoUserActivity("Shipment", this.ShareDocument.DocumentTypeName + " Built");

                            this.CurrentSession.FireEvent("RefreshDocumentOutPrint");
                            if (!this.ShareDocument.IsReady) {
                                this.ShareDocument.IsReady = true;
                                this.documentOutPMService.GetCalculatedFileNameForDocumentOutCopy(this.ShareDocument.DocumentOutPM.Id, this.ShareDocument.DocumentTypeCopyId).subscribe((res:any) => {
                                    this.StopBusyIndicator();
                                    var pmResponse: ServiceResponse = res;
                                    if (!pmResponse.HasError) {
                                        var myResult = pmResponse.Result;
                                        if (myResult) {
                                            this.ShareDocument.FileName = myResult;
                                        }
                                    }
                                });
                            } else {

                                this.StopBusyIndicator();
                            }

                        }
                    } else this.StopBusyIndicator();

                });



            } else this.StopBusyIndicator();




        });

    }


    HeaderHeight: number;
    FooterHeight: number;
    HtmlEditorData: string;

    ReBluidHtmlDocument() {

        var documentTypeCopyId = this.ShareDocument.DocumentTypeId;
        var documentTypeId = this.ShareDocument.DocumentTypeId;
        var shipmentId = this.ShareDocument.EntityId;


        this.CurrentSession.StartBusyIndicatorLoading();
        this.htmlEditorService.getEditorHtmlData(this.ShareDocument.DocumentOutPM.Id, shipmentId, this.ObjectTableId, "", "", SessionLocator.Tenant, SessionLocator.LoggedUserId, false, this.ShareDocument.DocumentOutPM.DocumentTemplateId, "", "Edit").subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.HtmlEditorData = "<header>" + "<height>" + "<div style='display:none'>" + myResult.HeaderHeight + "</div></height>" + myResult.HeaderHtml + "</header>" + myResult.Htmlstring + "<footer>" + "<height>" + "<div style='display:none'>" + myResult.FooterHeight + "</div></height>" + myResult.FooterHtml + "</footer>";
                    this.HeaderHeight = myResult.HeaderHeight;
                    this.FooterHeight = myResult.FooterHeight;
                }
                this.StopBusyIndicator();
                this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);
                this.SaveReportData(documentTypeCopyId);

            } else this.StopBusyIndicator();



        });




    }

    docIds: string;
    BuildingDocumentText: string = "Building document...";
    SaveReportData(documentTypeCopyId: string) {

        var filter = new FroalaEditorFilters();
        filter.DocumentOutId = this.ShareDocument.DocumentOutPM.Id;
        filter.DocumentTypeCopyId = documentTypeCopyId;
        filter.Tenant = SessionLocator.Tenant;
        filter.HtmlString = this.HtmlEditorData;
        filter.HeaderHeight = this.HeaderHeight;
        filter.FooterHeight = this.FooterHeight;
        filter.EntityId = this.ShareDocument.EntityId;
        filter.ChildEntityId = "";
        filter.DocumentTypeId = this.ShareDocument.DocumentTypeId;
        var idArray: any[];
        this.htmlEditorService.saveEditedReportToServer(filter).subscribe((res:any) => {

            this.CurrentSession.StartBusyIndicator(this.BuildingDocumentText);

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.docIds = myResult;
                    idArray = this.docIds.split(',');

                    this.AddedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
                    this.RemovedDocumentTypeCopyViewModels = new Array<DocumentCopiesViewModel>();
                    var anySelected = false;

                    this.Items.forEach((copy) => {
                        if (!this.documentTypePM.IsDocumentOneTimePrintLimited) {

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
                            this.ShareDocument.DocumentOutPM.XamlDocumentId = idArray[1];
                            this.ShareDocument.DocumentOutPM.IssuedByUserId = SessionLocator.LoggedUserId;
                            this.ShareDocument.DocumentOutPM.IsChangeIssuedDate = true;

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



    public ShowMessage(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }
    

    StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
        this.IsBuildDocumentRunning = false;
    }



    UploadButton(sareDocument: ShareDocument, shipmentShareDocumentsData: ShipmentShareDocumentsData, tiggerViewModel : any) {
        if (!this.IsUploadDocumentRunning) {

            if (sareDocument && shipmentShareDocumentsData) {
                this.IsUploadDocumentRunning = true;
                this.TiggerViewModel = tiggerViewModel;
                this.ShareDocument = sareDocument;
                this.ShipmentShareDocumentsData = shipmentShareDocumentsData;
                this.ShareDocument.EntityId = shipmentShareDocumentsData.EntityId;
  
                if (AppTool.IsNullOrEmpty(this.ShareDocument.DocumentsFilingId)) {
                    this.CreateDocumentsFilingPM();
                } else {

                    if (this.ShareDocument.IsReady && this.ShareDocument.ActionButtonLabel == "Additional") {
                        var shareDocument: ShareDocument = new ShareDocument();
                        shareDocument.DirectionCode = this.ShareDocument.DirectionCode;
                        shareDocument.DocumentTypeId = this.ShareDocument.DocumentTypeId;
                        shareDocument.DocumentTypeCode = this.ShareDocument.DocumentTypeCode;
                        shareDocument.DocumentTypeName = this.ShareDocument.DocumentTypeName;
                        shareDocument.EntityId = this.ShareDocument.EntityId;
                        this.CreateDocumentsFilingPM(shareDocument);
                    } else {
                        if (!this.ShareDocument.DocumentsFilingPM) {
                            this.LoadDocumentFiling();
                        } else {
                 
                            this.ShowAttachExternal();
                        }

                    }


                }
            }
        }
    }

    CreateDocumentsFilingPM(shareDocument: ShareDocument = null) {

        if (!shareDocument) shareDocument = this.ShareDocument;
        this.documentsFilingExtendedPMService.CreateDocumentsFiling(shareDocument.DocumentTypeId, shareDocument.EntityId, "", "", this.ObjectTableId, "I", SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    shareDocument.DocumentsFilingPM = myResult;
                    shareDocument.DocumentsFilingId = myResult.Id;
                    this.ShowAttachExternal(shareDocument);

                }
            }

        });



    }
    LoadDocumentFiling() {

        this.documentsFilingPMService.get(this.ShareDocument.DocumentsFilingId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.ShareDocument.DocumentsFilingPM = myResult;
                    this.ShareDocument.DocumentsFilingId = myResult.Id;
                  
                    this.ShowAttachExternal();

                }
            }

        });


    }


    ShowAttachExternal(shareDocument: ShareDocument = null) {
        if (!shareDocument) shareDocument = this.ShareDocument;

        var windowArgs: any = {};
        windowArgs.EntityId = shareDocument.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.RequsetPageName = "SharedDocument";
        windowArgs.CurrentDocument = shareDocument.DocumentsFilingPM;
        windowArgs.TiggerViewModel = this;
        windowArgs.Entity = shareDocument;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 300;
        logitudeWindow.Title = "File Uploading";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/AttachDocs/AttachmentUploaderComponent");
        logitudeWindow.WindowClosed.subscribe(($event: any) => {

            this.IsUploadDocumentRunning = false;
        });



    }

    OnUploadComplete(item: ShareDocument) {
        if (item) {
            if (item.DocumentsFilingPM) {
                item.Extension = item.DocumentsFilingPM.FileExtension;
                item.FileSize = item.DocumentsFilingPM.FileSize;
                item.DocumentId = item.DocumentsFilingPM.DocumentId;
                item.LastShareDate = item.DocumentsFilingPM.LastShareDate;
                item.LastUpdateDate = item.DocumentsFilingPM.UpdateDate;
                item.SecurityId = item.DocumentsFilingPM.SecurityId;

                if (!AppTool.IsNullOrEmpty(item.DocumentsFilingPM.FileName) ) {
                    item.FileName = item.DocumentsFilingPM.FileName.toLowerCase().replace("." + item.Extension, "");
                }
                item.IsReady = true;
                item.Included = true;
                item.ActionButtonLabel = "Additional";
                if (this.ShipmentShareDocumentsData && this.ShipmentShareDocumentsData.ShareDocuments) {

                    if (!this.ShipmentShareDocumentsData.ShareDocuments.filter(d => d.SecurityId == item.SecurityId)[0]) {
                        this.ShipmentShareDocumentsData.ShareDocuments.push(item);

                        if (this.TiggerViewModel) {
                            this.TiggerViewModel.ShareDocumentSelected = item;
                            this.TiggerViewModel.SortItemSource(this.ShipmentShareDocumentsData.ShareDocuments);
                        }
                    }

                }
                this.CurrentSession.FireEvent("RefreshDocIn");
            }
        }

    }
}
