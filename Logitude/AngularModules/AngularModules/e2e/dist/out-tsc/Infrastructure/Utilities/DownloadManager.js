"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var ServiceHelper_1 = require("../../Infrastructure/Utilities/ServiceHelper");
var DownloadManager = /** @class */ (function () {
    function DownloadManager() {
    }
    DownloadManager.DownloadCommunicationLogXML = function (item) {
        if (item) {
            if (item.SecurityId) {
                this.DownloadPage("", item.SecurityId);
            }
            else {
                this.DownloadPage(item.DocumentId);
            }
        }
    };
    DownloadManager.DownloadTransferHeaderFile = function (fileName) {
        if (fileName) {
            var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
            var link = Tools_1.AppTool.GetLogitudeURL() + "WebPages/DownloadFileName.aspx?id=" + fileName + "&tempId=" + token;
            var win = window.open(link, '_blank');
            win.focus();
        }
    };
    DownloadManager.DownloadPage = function (id, securityId) {
        if (securityId === void 0) { securityId = null; }
        var url = !Tools_1.AppTool.IsNullOrEmpty(securityId) ? "securityId=" + securityId : "id=" + id;
        if (!Tools_1.AppTool.IsNullOrEmpty(id) && !Tools_1.AppTool.IsNullOrEmpty(securityId)) {
            url += ("~" + id);
        }
        var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
        var link = Tools_1.AppTool.GetLogitudeURL() + "WebPages/DownloadPage.aspx?" + url + "&tempId=" + token;
        var win = window.open(link, '_blank');
        win.focus();
    };
    return DownloadManager;
}());
exports.DownloadManager = DownloadManager;
//# sourceMappingURL=DownloadManager.js.map