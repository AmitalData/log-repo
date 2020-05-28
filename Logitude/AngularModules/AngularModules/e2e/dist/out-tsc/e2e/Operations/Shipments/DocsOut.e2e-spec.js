"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Login_po_1 = require("../../Login/Login.po");
var protractor_1 = require("protractor");
var DocOutSenario_1 = require("./DocOutSenario");
describe('DocsOut', function () {
    // let login: LoginComp = new LoginComp();
    //let docsOutTab: DocsOutTabComponent = new DocsOutTabComponent();
    //let sendMailPopup: SendMailPopup = new SendMailPopup();
    //let NewDirectShipment: OperationsComp = new OperationsComp();
    //let printDocOut: PrintDocOut = new PrintDocOut();
    //let helper = new FieldsHelper();
    var DocOutSenarios = new DocOutSenario_1.DocOutSenario();
    var page = new Login_po_1.LoginComp();
    beforeEach(function () {
        protractor_1.browser.driver.manage().window().maximize();
        protractor_1.browser.ignoreSynchronization = true;
    });
    /*it('Send Docs Out of Shipment To A User Successfully', function () {
  
      NewDirectShipment.DoOperations();
      helper.WaitBusyIndicator();
      docsOutTab.DocsOutTab();
      docsOutTab.QuickSearchDocOut('BRCL-S-DocsOut', 'BRCL-L-DocsOut', 'BOOKING REQUEST');
      //here we need busy indicater
      sendMailPopup.sendEmailToFirstUser('Booking request', 'SearchFieldsId_0_1');
    });
  
   it('Failing Sending Email', function () {
      //add subject to doc
      docsOutTab.QuickSearchDocOut('BRCL-S-DocsOut', 'BRCL-L-DocsOut', 'BOOKING REQUEST ');
      sendMailPopup.sendEmailToFirstUser('ExceptionTest', 'SearchFieldsId_0_2');
      sendMailPopup.isSendingFailed('SendDocumentFailedDiv');
    });*/
    it('Successfully Printing Document', function () {
        DocOutSenarios.SuccessfullyPrintingDocument();
        //NewDirectShipment.DoOperations();
        //helper.WaitBusyIndicator();
        //docsOutTab.DocsOutTab();
        //docsOutTab.QuickSearchDocOut('ETO-P-DocsOut', 'ETO-L-DocsOut', 'Export Trucking Order');
        // printDocOut.isPrintingCompleted('BuildDocumentSucceededDiv', true);
    });
    it('Failing Printing Document', function () {
        DocOutSenarios.FailingPrintingDocument();
        // docsOutTab.QuickSearchDocOut('FTDT-P-DocsOut', 'FTDT-L-DocsOut', 'Failure Test Document');
        // printDocOut.isPrintingCompleted('BuildDocumentFailedDiv', false);
    });
});
//# sourceMappingURL=DocsOut.e2e-spec.js.map