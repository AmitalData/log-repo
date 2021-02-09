/// <reference types="cypress" />
// @ts-nocheck
import * as Actions from "../../actions/Actions"
import { Selectors } from "../../selectors/Selectors"
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors"
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { ARInvoiceDetails } from "cypress/models/ARInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { PayableDetails } from "cypress/models/PayableDetails"
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails"
import { APPaymentDetails } from "cypress/models/APPaymentDetails"
import { ARPaymentDetails } from "cypress/models/ARPaymentDetails"
let ShipmentData: ShipmentDetails;
let packagesDetails: PackagesDetails[]
let APInvoiceData: APInvoiceDetails
let APPaymentData: APPaymentDetails
let APInvoiceNumber: string
let ARInvoiceNumber: string

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(Selectors.ShipmentTab, null)
});
Given("a direct shipment with the following details", (dataTable) => {
    const shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentWizardsFields(ShipmentData);
});
When("create shipment", () => {
    Actions.CreateShipment(ShipmentData.ShipmentLevel);
});
Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        ShipmentData.ShipmentNumber = interception.response.body.ShipmentNumber;
    });
});
Given("the user fills {string} as GrossWeight and {string} as a MoveType", (GrossWeight, MoveType) => {
    Actions.OpenShipment(ShipmentData.ShipmentNumber);
    Actions.FillGeneralTab(GrossWeight, MoveType)
});

Given("the user add order package with the following details", (dataTable) => {
    //Actions.OpenShipment(ShipmentData.ShipmentNumber);
    packagesDetails = dataTable.hashes() as PackagesDetails[];
    Actions.FillOrdersTab(packagesDetails)
});


Given("partners with following details", (dataTable) => {
    const partnersDetails = dataTable.hashes()[0] as PartnersDetails;
    Actions.FillPartnersTab(ShipmentData.Direction, ShipmentData.TransportMode, partnersDetails)
});

Given("a Package with the following details", (dataTable) => {
    packagesDetails = dataTable.hashes() as PackagesDetails[];
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
    Actions.UpdateShipment(Selectors.ShipmentSaveButton)

});
Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200);

});
Given("a payable with the following details", (dataTable) => {
    const PayableData = dataTable.hashes()[0] as PayableDetails;
    Actions.FillPayablesTab(PayableData)
});

Given("an APInvoice with the following details and a random invoice number", (dataTable) => {
    APInvoiceData = dataTable.hashes()[0] as APInvoiceDetails
    cy.Click(Selectors.ReceiveInvoiceButton, null);
    Actions.FillAPInvoiceDetails(APInvoiceData)
});
When("receive APInvoice", () => {
    Actions.ReceiveAPInvoice();
});
Then("the APInvoice should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostAPInvoicesRequest", 200).then((interception) => {
        APInvoiceNumber = interception.request.body.invoiceNumber
    });

});

When("approve APInvoice", () => {
    Actions.APApproveInvoice()
});
Then("the APInvoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutAPInvoicesRequest", 200);

});
Given("an APPayment with the following details", (dataTable) => {
    APPaymentData = dataTable.hashes()[0] as APPaymentDetails
    cy.BackButton("Shipment: " + ShipmentData.ShipmentNumber)
    cy.BackButton("Operations")
    Actions.FillAPPayment(APPaymentData, APInvoiceNumber)
});
When("pay the APInvoice", () => {
    Actions.PayAPInvoice()
});
Then("the APInvoice should pay successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutAPPayments", 200)


});
Given("a receivable with the following details",
    (dataTable) => {
        const ReceivableData = dataTable.hashes() as ReceivableDetails;
        cy.BackButton("Accounting")
        cy.Click(BaseSelectors.OperationsMenu, null)
        cy.Click(Selectors.ShipmentTab, null)
        Actions.OpenShipment(ShipmentData.ShipmentNumber);
        Actions.FillReceivablesTab(ReceivableData)
    });
Given("an ARInvoice with the following details",
    (dataTable) => {
        const ARInvoiceData = dataTable.hashes()[0] as ARInvoiceDetails
        cy.Click(Selectors.CreateARInvoiceButton, null);
        Actions.FillARInvoiceDetails(ARInvoiceData)
    });
When("create ARInvoice", () => {
    Actions.CreateARInvoice()
});
Then("the ARInvoice should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostARInvoicesRequest", 200);
});

When("approve ARInvoice", () => {
    Actions.ARApproveInvoice()
});
Then("the ARInvoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutARInvoicesRequest", 200);
});

When("set ARInvoice as sent", () => {
    Actions.SetAsSentARInvoice()
});
Then("the ARInvoice should set as sent successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutARInvoicesRequest", 200);
});
Given("an ARPayment with the following details", (dataTable) => {
    cy.Click(Selectors.ARPaymentTabInsideShipment)
    cy.Click(Selectors.NewARPayment, null)
    const ARPaymentDat = dataTable.hashes()[0] as ARPaymentDetails
    Actions.FillARPaymentDetails(ARPaymentDat)
});
When("pay the ARInvoice", () => {
    Actions.PayARInvoice()
});
Then("the ARInvoice should pay successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutARPayments", 200)
});

Given("a credit ARInvoice with the following details", (dataTable) => {
    cy.BackButton("Back")
    cy.BackButton("Shipment: " + ShipmentData.ShipmentNumber)
    const ARInvoiceData = dataTable.hashes()[0] as ARInvoiceDetails
    cy.Click(Selectors.CreateCreditNoteARInvoiceButton, null);
    Actions.FillARInvoiceDetails(ARInvoiceData)
});
When("create credit ARInvoice", () => {
    Actions.CreateARInvoice()

});
Then("the credit ARInvoice should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostARInvoicesRequest", 200);

});

When("approve credit ARInvoice", () => {
    Actions.ARApproveInvoice()

});
Then("the credit ARInvoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutARInvoicesRequest", 200).then((interception) => {
        ARInvoiceNumber = interception.response.body.InvoiceNumber
    })
});

When("set credit ARInvoice as sent", () => {
    Actions.SetAsSentARInvoice()

});
Then("the credit ARInvoice should set successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutARInvoicesRequest", 200);
});

Given("a credit ARPayment with the following details", (dataTable) => {
    const ARPaymentDat = dataTable.hashes()[0] as ARPaymentDetails
    cy.BackButton("Shipment: " + ShipmentData.ShipmentNumber)
    cy.BackButton("Operations")
    Actions.NewARPaymentFromAccounting(ARPaymentDat, ARInvoiceNumber)
});


When("send docs", () => {
    cy.BackButton("Back")
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(Selectors.ShipmentTab, null)
    Actions.OpenShipment(ShipmentData.ShipmentNumber);
    Actions.SendDocs()

});
Then("the docs should send successfully", () => {
    BaseAssertion.AssertStatusCode("WaitSendDocs", 200)
});
When("upload docs", () => {
    cy.Click(Selectors.DocsInTabb)
    cy.get(BaseSelectors.Row0).click();
    cy.Click(BaseSelectors.UploadDocumentdbtn);
    const fileName = 'Logitude.jpg'
    cy.DefineRequestWait("POST", "**/PostLogsList", "WaitUpload")
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
Given("the user in the direct's shipment rounting tab",()=>{
    cy.Click(Selectors.RoutingsTab, null);
});

Given("edit Main Carriage Leg with the follwing details",(dataTable)=>{
    let mainCarriageLeg = dataTable.hashes()[0] as MainCarriageLeg;
    Actions.EditMainCarriageLegs(mainCarriageLeg.Airline);
}); 

When("close shipment operationally",()=>{
    cy.Click(Selectors.ShipmentMoreList, null,true);
    cy.Click(Selectors.OperationalCloseButton, null);
    Actions.UpdateClosedShipment();
});

When("close shipment Accountly",()=>{
    cy.Click(Selectors.ShipmentMoreList, null,true);
    cy.Click(Selectors.AccountllyCloseButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should close successfully",()=>{
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200)
});

When("reopen shipment Accountly",()=>{
    cy.Click(Selectors.ShipmentMoreList, null,true);
    cy.Click(Selectors.AccountllyReopenButton, null);
    Actions.UpdateClosedShipment();
});

When("reopen shipment operationally",()=>{
    cy.Click(Selectors.ShipmentMoreList, null,true);
    cy.Click(Selectors.OperationalReopenButton, null);
    Actions.UpdateClosedShipment();
});

Then("the shipment should Reopen successfully",()=>{
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200)
});