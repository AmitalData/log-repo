/*
 *********************************
 **    DocumentsPrintHelper     **
 *********************************
 * Steps in server:
 *    1- Declare DataProvider for document
 *    2- Declare the Service
 *    3- add case in GetReportDocument method inside [ExportDocumentHelper.cs]
 *    4- Create the DocumentType in maintenance for tenant zero, check [copy] check box to copy it to all tenants
 *
 * Steps in client:
 *    1- Add case in file [NewDocumentTypeComponent]
 *    2- add case in validator [DocumentTypeClassLevelValidator.Shared.cs]
 *
 *  -- Abdullah
 */   

declare var window: any;
import { EventEmitter} from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {AppTool} from '../../Infrastructure/Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypePM} from '../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypePMExtendedService} from '../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {ExportDocumentService} from '../../Common/Services/DocumentServices/ExportDocumentService';
import {DocumentOutPMService} from '../../Common/Services/ExtendedPMs/DocumentOutPMService';
import { DocumentOutPM } from '../../Common/EntityPMs/DocumentOutPM';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {DownloadManager} from '../../Infrastructure/Utilities/DownloadManager';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';

export class DocumentsPrintHelper {
    private documentOutPM: DocumentOutPM;
    private documentTypeCategoryCode: string;

    private _documentOutPMService: DocumentOutPMService = new DocumentOutPMService();
    private _documentTypePMService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();
    private _exportDocumentService: ExportDocumentService = new ExportDocumentService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private ObjectTableName: string, private EntityId: string, private Tenant: number) {

    }

    public BuildAndPrintDocument(documentTypeCategoryCode: string) {
        this.documentTypeCategoryCode = documentTypeCategoryCode;
        if (documentTypeCategoryCode)
            this.BuildDocument();
        else
            console.error("[DocumentsPrintHelper] documentTypeCategoryCode is not set!");
    }


    private BuildDocument() {
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.BuildingDocument"));
        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
        var objectTableId = objectTable.Id;


        //1
        //Get document type
        this._documentTypePMService.GetDocumentTypeByCode(this.documentTypeCategoryCode, this.Tenant).subscribe((response: ServiceResponse) => {
            var documentType: DocumentTypePM = response.Result;
            console.log("[DocumentsPrintHelper] _documentTypePMService.GetDocumentTypeByCode", response)
            if (documentType) {

                //2
                //Get document copy
                var documentTypeCopy =
                    documentType.DocumentTypeCopies[0];




                //3
                //Get document out
                this._documentOutPMService.getCreateDocumentOut(documentType.Id, this.EntityId, null, null, objectTableId, this.Tenant).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var documentout: DocumentOutPM = pmResponse.Result;
                        console.log("[DocumentsPrintHelper] _documentOutPMService.getCreateDocumentOut", response)
                        if (documentout) {

                            var documentOutCopy = documentout.DocumentOutCopies[0];
                            this.documentOutPM = documentout;

                            //if (documentOutCopy) {
                            //4
                            //Export to pdf
                            this._exportDocumentService.getDocumentPdfFile(documentType.Id, this.EntityId, objectTableId, null, null, documentout.Id, documentout.Tenant, documentTypeCopy.Id, SessionLocator.LoggedUserId).subscribe(res => {
                                var pmResponse: ServiceResponse = res;
                                if (!pmResponse.HasError) {
                                    console.log("[DocumentsPrintHelper] _exportDocumentService.getDocumentPdfFile", pmResponse)
                                    var myResult = pmResponse.Result;

                                    if (myResult != null) {

                                        if (documentOutCopy) {
                                            //5
                                            //view page
                                            var documentName = documentOutCopy.Tenant + "~" + documentOutCopy.Id;
                                            documentName = documentName + "~" + documentOutCopy.DocumentId + "~" + SessionLocator.LoggedUserId;
                                            this.ViewPage(documentOutCopy.Id, documentOutCopy.DocoumentTypeCopyName, documentout);
                                        } else {
                                            this.BuildDocument(); // resend the request, the method [getCreateDocumentOut] does not create document out copy!!
                                            console.warn("[DocumentsPrintHelper] Cannot find document out copy, resend request...");
                                        }



                                    }
                                    else
                                        this.StopBusyIndicator();

                                } else {
                                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                        console.error(pmResponse.ErrorsArray[0]);
                                    }
                                    this.StopBusyIndicator();
                                }

                            });
                            //} else {
                            //    console.warn("Cannot find document out copy, resend request...");
                            //    //this.CurrentSession.StopBusyIndicator();
                            //    this.BuildDocument(); // resend the request, the method [getCreateDocumentOut] does not create document out copy!!
                            //}




                        } else {
                            console.error("[DocumentsPrintHelper] Cannot create document out!", res);
                            this.CurrentSession.StopBusyIndicator();
                        }
                    }


                });



            }
        });



    }

    private ViewPage(documentName: string, docoumentTypeCopyName: string, documentOut: DocumentOutPM) {

        ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, docoumentTypeCopyName + " Viewing");

        DownloadManager.DownloadPage(documentName,documentOut.SecurityId);
        this.StopBusyIndicator();

    }

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
}
