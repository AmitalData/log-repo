import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { ShipmentSelectors } from "../../selectors/Selectors";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import * as AccountingActions from '../../../../Accounting/cypress/actions/Actions';
import { AccountingSelectors } from "../../../../Accounting/cypress/selectors/Selectors";
import { APInvoiceDetails } from "../../../../Accounting/cypress/models/APInvoiceDetails"


//#region variables
let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
//#endregion

Given("the user logged in and navigates to shipments workspace", (maintenanceItem) => {
    cy.Login();
    Actions.NavigatesToShipmentsWorkspace()
  });

//#region Create direct export air shipment
Given("the user navigates to shipments workspace", () => {
    Actions.NavigatesToShipmentsWorkspace()
  });
  
  Given("a direct shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
  });
  
  When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
  });
  
  Then("the direct should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
      shipmentNumber = interception.response.body.ShipmentNumber;
    })
  });
  //#endregion
  //#region Create APInvoice
  Given("open the shipment and navigates to payable wizard", () => {
    Actions.OpenShipment(shipmentNumber)
    cy.Click(ShipmentSelectors.PayablesTab, null)
  });

  Given("creates APInvoice with a random invoice number and the following details",
  (dataTable) => {
    const APInvoiceData = Assists.CreateInstance<APInvoiceDetails>(dataTable, true);
    cy.Click(AccountingSelectors.ReceiveInvoiceButton, null);
    AccountingActions.FillunexpectedAPInvoiceDetails(APInvoiceData)
  });

When("create invoice", () => {
  //  AccountingActions.ReceiveunexpectedAPInvoice();
});

Then("the invoice should create successfully", () => {
//   BaseAssertion.AssertStatusCode(RequestAliases.InvoiceDomain, 200);
  });
 //#endregion
 Given("the user creates an AP invoice line", () => {
     
 }) ;