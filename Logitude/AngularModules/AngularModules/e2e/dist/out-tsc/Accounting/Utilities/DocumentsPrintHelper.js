"use strict";
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
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var DocumentTypePMExtendedService_1 = require("../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var ExportDocumentService_1 = require("../../Common/Services/DocumentServices/ExportDocumentService");
var DocumentOutPMService_1 = require("../../Common/Services/ExtendedPMs/DocumentOutPMService");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var DownloadManager_1 = require("../../Infrastructure/Utilities/DownloadManager");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var DocumentsPrintHelper = /** @class */ (function () {
    function DocumentsPrintHelper(ObjectTableName, EntityId, Tenant) {
        this.ObjectTableName = ObjectTableName;
        this.EntityId = EntityId;
        this.Tenant = Tenant;
        this._documentOutPMService = new DocumentOutPMService_1.DocumentOutPMService();
        this._documentTypePMService = new DocumentTypePMExtendedService_1.DocumentTypePMExtendedService();
        this._exportDocumentService = new ExportDocumentService_1.ExportDocumentService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    DocumentsPrintHelper.prototype.BuildAndPrintDocument = function (documentTypeCategoryCode) {
        this.documentTypeCategoryCode = documentTypeCategoryCode;
        if (documentTypeCategoryCode)
            this.BuildDocument();
        else
            console.error("[DocumentsPrintHelper] documentTypeCategoryCode is not set!");
    };
    DocumentsPrintHelper.prototype.BuildDocument = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.BuildingDocument"));
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        var objectTableId = objectTable.Id;
        //1
        //Get document type
        this._documentTypePMService.GetDocumentTypeByCode(this.documentTypeCategoryCode, this.Tenant).subscribe(function (response) {
            var documentType = response.Result;
            console.log("[DocumentsPrintHelper] _documentTypePMService.GetDocumentTypeByCode", response);
            if (documentType) {
                //2
                //Get document copy
                var documentTypeCopy = documentType.DocumentTypeCopies[0];
                //3
                //Get document out
                _this._documentOutPMService.getCreateDocumentOut(documentType.Id, _this.EntityId, null, null, objectTableId, _this.Tenant).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var documentout = pmResponse.Result;
                        console.log("[DocumentsPrintHelper] _documentOutPMService.getCreateDocumentOut", response);
                        if (documentout) {
                            var documentOutCopy = documentout.DocumentOutCopies[0];
                            _this.documentOutPM = documentout;
                            //if (documentOutCopy) {
                            //4
                            //Export to pdf
                            _this._exportDocumentService.getDocumentPdfFile(documentType.Id, _this.EntityId, objectTableId, null, null, documentout.Id, documentout.Tenant, documentTypeCopy.Id, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
                                var pmResponse = res;
                                if (!pmResponse.HasError) {
                                    console.log("[DocumentsPrintHelper] _exportDocumentService.getDocumentPdfFile", pmResponse);
                                    var myResult = pmResponse.Result;
                                    if (myResult != null) {
                                        if (documentOutCopy) {
                                            //5
                                            //view page
                                            var documentName = documentOutCopy.Tenant + "~" + documentOutCopy.Id;
                                            documentName = documentName + "~" + documentOutCopy.DocumentId + "~" + SessionLocator_1.SessionLocator.LoggedUserId;
                                            _this.ViewPage(documentOutCopy.Id, documentOutCopy.DocoumentTypeCopyName, documentout);
                                        }
                                        else {
                                            _this.BuildDocument(); // resend the request, the method [getCreateDocumentOut] does not create document out copy!!
                                            console.warn("[DocumentsPrintHelper] Cannot find document out copy, resend request...");
                                        }
                                    }
                                    else
                                        _this.StopBusyIndicator();
                                }
                                else {
                                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                        console.error(pmResponse.ErrorsArray[0]);
                                    }
                                    _this.StopBusyIndicator();
                                }
                            });
                            //} else {
                            //    console.warn("Cannot find document out copy, resend request...");
                            //    //this.CurrentSession.StopBusyIndicator();
                            //    this.BuildDocument(); // resend the request, the method [getCreateDocumentOut] does not create document out copy!!
                            //}
                        }
                        else {
                            console.error("[DocumentsPrintHelper] Cannot create document out!", res);
                            _this.CurrentSession.StopBusyIndicator();
                        }
                    }
                });
            }
        });
    };
    DocumentsPrintHelper.prototype.ViewPage = function (documentName, docoumentTypeCopyName, documentOut) {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, docoumentTypeCopyName + " Viewing");
        DownloadManager_1.DownloadManager.DownloadPage(documentName, documentOut.SecurityId);
        this.StopBusyIndicator();
    };
    DocumentsPrintHelper.prototype.StartBusyIndicator = function (message) {
        this.CurrentSession.StartBusyIndicator(message);
    };
    DocumentsPrintHelper.prototype.StopBusyIndicator = function () {
        this.CurrentSession.StopBusyIndicator();
    };
    return DocumentsPrintHelper;
}());
exports.DocumentsPrintHelper = DocumentsPrintHelper;
//# sourceMappingURL=DocumentsPrintHelper.js.map