import * as Actions from "../../actions/Actions"
import { ShipmentSelectors } from "../../selectors/Selectors"
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { PayableDetails } from "cypress/models/PayableDetails"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { ShipmentDetails } from "cypress/models/ShipmentDetails";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";

//#region Variables
let shipmentDetails: ShipmentDetails;
//#endregion

//#region AMANAC Setup
Given("the user logged in and navigates to {string} in maintenance menu", (maintenanceSearchValue) => {
    cy.Login()
    Actions.NavigatesTocustomSettingsInMaintenance(maintenanceSearchValue);
});

When("set local customs interface to {string}", (localCustomsInterfaceValue) => {
    Actions.UpdateLocalCustomsInterface(localCustomsInterfaceValue);
});

Then("the AMANAC workspace should appear in operations menu", () => {
    cy.Click(BaseSelectors.OperationsMenu, null);
    cy.get("#AMANAC").should('exist');
});
//#endregion

//#region Create direct export air shipment
Given("the user in shipment workspace", () => {
    Actions.NavigatesToShipmentsWorkspace()
});

Given("a shipment with the following details", (dataTable) => {
    shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    Actions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    Actions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
    Actions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        shipmentDetails.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});
//#endregion

//#region Marked as blocked for transfer
When("marke the shipment as blocked for transfer", () => {
    Actions.NavigatesToAMANACWorkspace();
    Actions.AMANACView("New Transfer");
    Actions.AMANACmarketheshipmentasblocked(shipmentDetails.ShipmentNumber);
});

Then("the shipment should appear in the {string} view in the AMANAC workspace", (AMANACview) => {
    //Actions.AMANACView(AMANACview);
    cy.get("Button[id^='ReportID']").eq(1).click()

    cy.DefineRequestWait(RestAPI.GET, "**/shipmentviews/getbyfilters?**"+shipmentDetails.ShipmentNumber+"**", "shipmentviews1")
   //cy.FillLogLov("#null_Search", ShipmentNumber, true);
   cy.get("#null_Search").type("{selectall}" + shipmentDetails.ShipmentNumber,{delay:5})
  // BaseAssertion.AssertStatusCode("shipmentviews1", 200);
   //cy.DefineRequestWait(RestAPI.GET, "**/shipmentviews/getbyfilters?**", "shipmentviews2")

   BaseAssertion.AssertStatusCode("shipmentviews1", 200);

    cy.get("td[data-cy^=ShipmentNumber_" + shipmentDetails.ShipmentNumber +"]").should('contain',shipmentDetails.ShipmentNumber)
    cy.Click(".Button", "Close")
});

Then("AMANAC Status should be {string}", (AMANACstatus) => {
    Actions.NavigatesToShipmentsWorkspace()
    Actions.OpenShipment(shipmentDetails.ShipmentNumber);
    cy.Click("#ShipmentTHCustoms", null);
    //expect(cy.get(".SummayRow").contains("Status:")).to.contain(AMANACstatus)
});
//#endregion