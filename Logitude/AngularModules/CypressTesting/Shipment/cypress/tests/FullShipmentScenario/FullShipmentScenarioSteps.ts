/// <reference types="cypress" />
// @ts-nocheck
import * as Actions from "../../actions/Actions"
import { ShipmentSelectors } from "../../selectors/Selectors"
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { ARInvoiceDetails } from "../../../../Accounting/cypress/models/ARInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { PayableDetails } from "cypress/models/PayableDetails"
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { APInvoiceDetails } from "../../../../Accounting/cypress/models/APInvoiceDetails"
import { APPaymentDetails } from "../../../../Accounting/cypress/models/APPaymentDetails"
import { ARPaymentDetails } from "../../../../Accounting/cypress/models/ARPaymentDetails"
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import { MainCarriageLeg } from "cypress/models/MainCarriageLeg";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import {AccountingSelectors} from '../../../../Accounting/cypress/selectors/Selectors';
import * as Assists from "../../../../Base/cypress/assists/Assists";

let ShipmentData: ShipmentDetails;
let packagesDetails: PackagesDetails[]
let APInvoiceData: APInvoiceDetails
let APPaymentData: APPaymentDetails
let APInvoiceNumber: string
let ARInvoiceNumber: string

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(ShipmentSelectors.ShipmentTab, null)
});
Given("a direct shipment with the following details", (dataTable) => {
    const shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentWizardsFields(ShipmentData);
});
When("create shipment", () => {
    Actions.CreateShipment(ShipmentData.ShipmentLevel);
});
Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        ShipmentData.ShipmentNumber = interception.response.body.ShipmentNumber;
    });
});
Given("the user fills {string} as ValueOfGoods and {string} as a MoveType", (ValueOfGoods, MoveType) => {
    Actions.OpenShipment(ShipmentData.ShipmentNumber);
    Actions.FillGeneralTab(ValueOfGoods, MoveType)
});

Given("the user add order package with the following details", (dataTable) => {
    //Actions.OpenShipment(ShipmentData.ShipmentNumber);
    packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillOrdersTab(packagesDetails)
});


Given("partners with following details", (dataTable) => {
    const partnersDetails = Assists.CreateInstance<PartnersDetails>(dataTable, true);
    Actions.FillPartnersTab(ShipmentData.Direction, ShipmentData.TransportMode, partnersDetails)
});

Given("a Package with the following details", (dataTable) => {
    packagesDetails = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(ShipmentData.TransportMode, packagesDetails)
});

Given("the user add new pickup", () => {
    Actions.FillPickupRouting()

});
Given("add delivery with {string} as a partner routing", (partner) => {
    Actions.FillDeliveryRouting(partner)

});
Given("add pre carriage from port {string} to port {string}", (fromPort, toPort) => {
    Actions.FillPreCarriageRouting(ShipmentData.TransportMode, fromPort, toPort)
});
Given("add on carriage from port {string} to port {string}", (fromPort, toPort) => {
    Actions.FillOnCarriageRouting(ShipmentData.TransportMode, fromPort, toPort)
});
When("save shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)

});
Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);

});
Given("a payable with the following details", (dataTable) => {
    const PayableData = Assists.CreateInstance<PayableDetails>(dataTable, true);
    Actions.FillPayablesTab(PayableData)
});

Given("an APInvoice with the following details and a random invoice number", (dataTable) => {
    APInvoiceData = Assists.CreateInstance<APInvoiceDetails>(dataTable, true);
    cy.Click(AccountingSelectors.ReceiveInvoiceButton, null);
    AccountingActions.FillAPInvoiceDetails(APInvoiceData)
});
When("receive APInvoice", () => {
    AccountingActions.ReceiveAPInvoice();
});
Then("the APInvoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200).then((interception) => {
        APInvoiceNumber = interception.request.body.invoiceNumber
    });

});

When("approve APInvoice", () => {
    AccountingActions.APApproveInvoice()
});
Then("the APInvoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);

});
Given("an APPayment with the following details", (dataTable) => {
    APPaymentData = Assists.CreateInstance<APPaymentDetails>(dataTable, true);
    cy.BackButton(BaseSelectors.ContainsShipment + ShipmentData.ShipmentNumber)
    cy.BackButton(BaseSelectors.ContainsOperations)
    AccountingActions.FillAPPayment(APPaymentData, APInvoiceNumber)
});
When("pay the APInvoice", () => {
    AccountingActions.PayAPInvoice()
});
Then("the APInvoice should pay successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.APPayments, 200)


});
Given("a receivable with the following details",
    (dataTable) => {
        const ReceivableData = Assists.CreateSet<ReceivableDetails>(dataTable);
        cy.BackButton(BaseSelectors.ContainsAccounting)
        cy.Click(BaseSelectors.OperationsMenu, null)
        cy.Click(ShipmentSelectors.ShipmentTab, null)
        Actions.OpenShipment(ShipmentData.ShipmentNumber);
        Actions.FillReceivablesTab(ReceivableData)
    });
Given("an ARInvoice with the following details",
    (dataTable) => {
        const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
        cy.Click(AccountingSelectors.CreateARInvoiceButton, null);
        AccountingActions.FillARInvoiceDetails(ARInvoiceData)
    });
When("create ARInvoice", () => {
    AccountingActions.CreateARInvoice()
});
Then("the ARInvoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("approve ARInvoice", () => {
    AccountingActions.ARApproveInvoice()
});
Then("the ARInvoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("set ARInvoice as sent", () => {
    AccountingActions.SetAsSentARInvoice()
});
Then("the ARInvoice should set as sent successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});
Given("an ARPayment with the following details", (dataTable) => {
    cy.Click(AccountingSelectors.ARPaymentTabInsideShipment,null)
    cy.Click(AccountingSelectors.NewARPayment, null)
    const ARPaymentDat = Assists.CreateInstance<ARPaymentDetails>(dataTable, true);
    AccountingActions.FillARPaymentDetails(ARPaymentDat)
});
When("pay the ARInvoice", () => {
    AccountingActions.PayARInvoice()
});
Then("the ARInvoice should pay successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
});

Given("a credit ARInvoice with the following details", (dataTable) => {
    cy.BackButton(BaseSelectors.ContainsBack)
    cy.BackButton(BaseSelectors.ContainsShipment + ShipmentData.ShipmentNumber)
    const ARInvoiceData = Assists.CreateInstance<ARInvoiceDetails>(dataTable, true);
    cy.Click(AccountingSelectors.CreateCreditNoteARInvoiceButton, null);
    AccountingActions.FillARInvoiceDetails(ARInvoiceData)
});
When("create credit ARInvoice", () => {
    AccountingActions.CreateARInvoice()

});
Then("the credit ARInvoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);

});

When("approve credit ARInvoice", () => {
    AccountingActions.ARApproveInvoice()

});
Then("the credit ARInvoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200).then((interception) => {
        ARInvoiceNumber = interception.response.body.InvoiceNumber
    })
});

When("set credit ARInvoice as sent", () => {
    Actions.SetAsSentARInvoice()

});
Then("the credit ARInvoice should set successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

Given("a credit ARPayment with the following details", (dataTable) => {
    const ARPaymentDat = Assists.CreateInstance<ARPaymentDetails>(dataTable, true);
    cy.BackButton(BaseSelectors.ContainsShipment+ ShipmentData.ShipmentNumber)
    cy.BackButton(BaseSelectors.ContainsOperations)
    AccountingActions.NewARPaymentFromAccounting(ARPaymentDat, ARInvoiceNumber)
});


When("send docs", () => {
    cy.BackButton(BaseSelectors.ContainsBack)
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(ShipmentSelectors.ShipmentTab, null)
    Actions.OpenShipment(ShipmentData.ShipmentNumber);
    Actions.SendDocs()

});
Then("the docs should send successfully", () => {
    BaseAssertion.AssertStatusCode("WaitSendDocs", 200)
});
When("upload docs", () => {
    cy.Click(ShipmentSelectors.DocsInTabb,null)
    cy.get(BaseSelectors.Row0).click();
    cy.Click(BaseSelectors.UploadDocumentdbtn,null);
    const fileName = 'Logitude.jpg'
    cy.DefineRequestWait(RestAPI.POST, "**/PostLogsList", "WaitUpload")
    cy.fixture(fileName).then(function (fileContent) {
        cy.get('input.upload').attachFile({ fileContent, fileName, mimetype: 'application/pdf' })
        cy.get('#FileUploadedSuccessfully').should('be.visible')
        cy.get('.RedButton').click()

    })
});

Then("the docs should upload successfully", () => {
    BaseAssertion.AssertStatusCode("WaitUpload", 200)

})
When("delete Attachment", () => {
    Actions.DeleteAttachment()

})
Then("the attachment should delete successfully", () => {
    BaseAssertion.AssertStatusCode("WaitDelete", 200)

})
Given("the user in the direct's shipment rounting tab", () => {
    cy.Click(ShipmentSelectors.RoutingsTab, null);
});

Given("edit Main Carriage Leg with the following details", (dataTable) => {
    let mainCarriageLeg = Assists.CreateInstance<MainCarriageLeg>(dataTable, true);
    Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
    cy.Click(ShipmentSelectors.ShipmentSaveButton, null);
});

When("close shipment operationally", ()=>{
    cy.Click(ShipmentSelectors.ShipmentMoreList, null,true);
    cy.Click(ShipmentSelectors.OperationalCloseButton, null);
    Actions.UpdateClosedShipment();
});

When("close shipment Accountly",()=>{
    cy.Click(ShipmentSelectors.ShipmentMoreList, null,true);
    cy.Click(ShipmentSelectors.AccountllyCloseButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should close successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});

When("reopen shipment Accountly",()=>{
    cy.Click(ShipmentSelectors.ShipmentMoreList, null,true);
    cy.Click(ShipmentSelectors.AccountllyReopenButton, null);
    Actions.UpdateClosedShipment();
});

When("reopen shipment operationally",()=>{
    cy.Click(ShipmentSelectors.ShipmentMoreList, null,true);
    cy.Click(ShipmentSelectors.OperationalReopenButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should Reopen successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200)
});