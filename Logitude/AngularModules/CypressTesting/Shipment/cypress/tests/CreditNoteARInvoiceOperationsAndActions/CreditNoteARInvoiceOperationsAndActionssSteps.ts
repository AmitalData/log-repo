import * as Actions from "../../actions/Actions"
import { ShipmentSelectors } from "../../selectors/Selectors"
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { ARInvoiceDetails } from "../../../../Accounting/cypress/models/ARInvoiceDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import {AccountingSelectors} from '../../../../Accounting/cypress/selectors/Selectors'

let ShipmentData: ShipmentDetails;
let shipmentNumber: string;

Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
    Actions.NavigatesToShipmentsWorkspace()
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
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentNumber = interception.response.body.ShipmentNumber;
    });

    Given("a receivable with the following details", (dataTable) => {
        const ReceivableData = dataTable.hashes() as ReceivableDetails[];
        Actions.OpenShipment(shipmentNumber)
        Actions.FillReceivablesTab(ReceivableData)
        Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
    });
    Given("a credit ARInvoice with a random invoice number and the following details",
        (dataTable) => {
            const ARInvoiceData = dataTable.hashes()[0] as ARInvoiceDetails
            cy.Click(AccountingSelectors.CreateCreditNoteARInvoiceButton, null);
            AccountingActions.FillARInvoiceDetails(ARInvoiceData)
        });
});
When("create invoice", () => {
    AccountingActions.CreateARInvoice()
});
Then("the invoice should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("approve invoice", () => {
    AccountingActions.ARApproveInvoice()
});
Then("the invoice should approve successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("set invoice as sent", () => {
    AccountingActions.SetAsSentARInvoice()
});
Then("the invoice should set as sent successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});

When("void invoice", () => {
    AccountingActions.VoidARInvoice()
});
Then("the invoice should void successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
});
