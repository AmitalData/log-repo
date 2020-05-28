"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Login_po_1 = require("../../Login/Login.po");
var Operations_po_1 = require("./NewEntity/Operations.po");
var DocOutSenario = /** @class */ (function () {
    // private printDocOut: PrintDocOut = new PrintDocOut();
    // private helper = new FieldsHelper();
    function DocOutSenario() {
        this.login = new Login_po_1.LoginComp();
        //  private docsOutTab: DocsOutTabComponent = new DocsOutTabComponent();
        // private sendMailPopup: SendMailPopup = new SendMailPopup();
        this.NewDirectShipment = new Operations_po_1.OperationsComp();
    }
    DocOutSenario.prototype.SuccessfullyPrintingDocument = function () {
        // this.NewDirectShipment.DoOperations();
        //this.helper.WaitBusyIndicator();
        //this.docsOutTab.DocsOutTab();
        //this.docsOutTab.QuickSearchDocOut('ETO-P-DocsOut', 'ETO-L-DocsOut', 'Export Trucking Order');
        //this.printDocOut.isPrintingCompleted('BuildDocumentSucceededDiv', true);
    };
    DocOutSenario.prototype.FailingPrintingDocument = function () {
        //this.docsOutTab.QuickSearchDocOut('FTDT-P-DocsOut', 'FTDT-L-DocsOut', 'Failure Test Document');
        //this.printDocOut.isPrintingCompleted('BuildDocumentFailedDiv', false);
    };
    return DocOutSenario;
}());
exports.DocOutSenario = DocOutSenario;
//# sourceMappingURL=DocOutSenario.js.map