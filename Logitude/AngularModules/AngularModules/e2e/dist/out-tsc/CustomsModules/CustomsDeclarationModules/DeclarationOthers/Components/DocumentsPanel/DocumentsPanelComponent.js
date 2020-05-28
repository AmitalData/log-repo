"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ServiceHelper_1 = require("../../../../../Infrastructure/Utilities/ServiceHelper");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var ControlsIdCounter_1 = require("../../../../../Infrastructure/Utilities/ControlsIdCounter");
var RelatedDocumentViewModel_1 = require("../../../../CustomsDocuments/Components/RelatedDocumentViewModel");
var CustDocRelatedDocsWebService_1 = require("../../../../../Customs/Services/WebServices/CustDocRelatedDocsWebService");
var AmitalGatewayUtil_1 = require("../../../../../Infrastructure/Utilities/AmitalGatewayUtil");
var ImageLibraryService_1 = require("../../../../../Common/Services/Others/ImageLibraryService");
var CustomDocumentViewerService_1 = require("../../../../../Customs/Services/WebServices/CustomDocumentViewerService");
var CustDocMetaDataValuesWebService_1 = require("../../../../../Customs/Services/WebServices/CustDocMetaDataValuesWebService");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var DocumentsPanelComponent = /** @class */ (function () {
    function DocumentsPanelComponent() {
        this.custDocRelatedDocsWebService = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
        this._ImageLibraryService = new ImageLibraryService_1.ImageLibraryService();
        this._CustomDocumentViewerService = new CustomDocumentViewerService_1.CustomDocumentViewerService();
        this.custDocsMetadataWebService = new CustDocMetaDataValuesWebService_1.CustDocMetaDataValuesWebService();
        this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Documents DDL
        this.IsDocsPanelVisible = false;
        //#endregion
        this.DocumentFilterSelectedValue = "customs";
        var counter = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter("DocumentListWrapperId");
        this.DocumentListWrapperId = "DocumentListWrapperId" + counter;
    }
    DocumentsPanelComponent.prototype.Run = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
    };
    DocumentsPanelComponent.prototype.DocumentsIconClicked = function () {
        this.IsDocsPanelVisible = !this.IsDocsPanelVisible;
        if (this.IsDocsPanelVisible == true)
            this.LoadDocuments();
    };
    DocumentsPanelComponent.prototype.LoadDocuments = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var objecttable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
        this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.EntityPM.Id, null, objecttable.Id, "I", this.EntityPM.CustomFileNo, this.DocumentFilterSelectedValue)
            .subscribe(function (response) {
            console.log("[response] GetDocumentsFilingsForRelatedDocuments:", response);
            _this.CurrentSession.StopBusyIndicator();
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                _this.RelatedDocuments = [];
                var relatedDocs;
                relatedDocs = response.Result;
                for (var i = 0; i < relatedDocs.length; i++) {
                    //var ticket = this.CustomsDocumentsTickets.filter(d => d.DocumentsFilingId == relatedDocs[i].Id)[0];
                    //var values: CustomsDocumentMetaDataValuePM[] = this.MetadataValues.filter(d => d.CustomsDocumentId == relatedDocs[i].Id);
                    var values = null;
                    //if (!ticket) {
                    var relatedDocViewModel = new RelatedDocumentViewModel_1.RelatedDocumentViewModel(relatedDocs[i], values, true);
                    _this.RelatedDocuments.push(relatedDocViewModel);
                    //}
                }
                //Load first document
                //this.TicketItemClicked(this.RelatedDocuments[0]);
                //this.IsDocsPanelVisible = true;
            }
        });
    };
    DocumentsPanelComponent.prototype.DownloadDocumentFile = function (documentsFilingId) {
        var _this = this;
        this.custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(documentsFilingId).subscribe(function (resp) {
            var documentFiling = resp.Result;
            _this._ImageLibraryService.DownloadFile(documentFiling.DocumentId, documentFiling.Extension, documentFiling.Folder, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var documentName = SessionLocator_1.SessionLocator.Tenant + "_" + documentFiling.DocumentId;
                var token = ServiceHelper_1.ServiceHelper.GetLoggedUserToken();
                var uri = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/Downloadpage.aspx?id=" + documentName + "&token=" + token;
                if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                    AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(uri);
                    return;
                }
                var win = window.open(uri);
            });
        });
    };
    DocumentsPanelComponent.prototype.DocumentFilterItemClicked = function (value) {
        this.DocumentFilterSelectedValue = value;
        this.LoadDocuments();
        //this.GetRelatedDocuments();
    };
    DocumentsPanelComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentsPanelComponent',
            templateUrl: "DocumentsPanelComponent.html",
        }),
        __metadata("design:paramtypes", [])
    ], DocumentsPanelComponent);
    return DocumentsPanelComponent;
}());
exports.DocumentsPanelComponent = DocumentsPanelComponent;
//# sourceMappingURL=DocumentsPanelComponent.js.map