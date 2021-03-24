import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import * as ShipmentActions from "../../../../Shipment/cypress/actions/Actions"
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import * as Actions from "../../actions/Actions"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentDetails } from "../../../../Shipment/cypress/models/ShipmentDetails";
import { PackagesDetails } from "../../../../Shipment/cypress/models/PackagesDetails";
import { ShipmentSelectors } from "../../../../Shipment/cypress/selectors/Selectors";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { CrossdockDetails } from "../../models/CrossdockDetails";
import { LinkedShipmentDetails } from "../../models/LinkedShipmentDetails";
import { CrossdockSelectors } from "../../selectors/Selectors";
import { CrossdockContext } from "../../models/CrossdockContext";

let shipmentDetails: ShipmentDetails;
let crossdockDetails: CrossdockDetails;

//#region Create direct export air shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login();
    ShipmentActions.NavigatesToShipmentsWorkspace();
});

Given("a shipment with the following details", (dataTable) => {
    shipmentDetails = Assists.CreateInstance<ShipmentDetails>(dataTable, true);
    ShipmentActions.OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    ShipmentActions.FillShipmentWizardsFields(shipmentDetails);
});

When("create shipment", () => {
    ShipmentActions.CreateShipment(shipmentDetails.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        CrossdockContext.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});

//#endregion

//#region Add packages
Given("the user open the shipment and navigate to packages workspace", () => {
    ShipmentActions.OpenShipment(CrossdockContext.ShipmentNumber);
    cy.Navigate(ShipmentSelectors.PackagesTab);
});

Given("a package with the following details", (dataTable) => {
    let packageDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
    ShipmentActions.FillPackageTab(shipmentDetails.TransportMode, packageDetailsList, shipmentDetails.ShipmentType);
});

When("save shipment", () => {
    ShipmentActions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)

});

Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Add cross dock entry
Given("the user in Connected Entities workspace", () => {
    cy.Navigate(ShipmentSelectors.ConnectionsTab);
});

Given("a corss dock {string} with the following details", (crossDockType, dataTable) => {
    crossdockDetails = Assists.CreateInstance<CrossdockDetails>(dataTable, true);
    Actions.FillCrossdockWizardsFields(crossdockDetails ,crossDockType );
});

When("create cross dock entry", () => {
    Actions.CreateCrossdock();
});

Then("the cross dock entry should create successfully", () => {
    Actions.AssertCreateCrossdock();
});

Then("shipment will contain the linked entry details", (dataTable) => {
    let linkedEntryDetails = Assists.CreateInstance<CrossdockDetails>(dataTable, true);
    linkedEntryDetails = EntryNumberConversionMapping(linkedEntryDetails)
    Actions.ValidateLinkedEntryInShipment(linkedEntryDetails);
});

Then("cross dock entry will contain the linked shipment details", (dataTable) => {
    let linkedShipmentDetails = Assists.CreateInstance<LinkedShipmentDetails>(dataTable, true);
    linkedShipmentDetails = ShipmentNumberConversionMapping(linkedShipmentDetails);
    Actions.ValidateLinkedShipmentInEntry(linkedShipmentDetails);
});
//#endregion

//#region Cancel entry
When("cancel the entry", () => {
    Actions.CancleEntry();
});

Then("the entry should cancel successfully", () => {
    Actions.AssertCancleEntry()
});

Then("entry status should be {string}", (status) => {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen , status)
});

Then("entry fields will be disable", () => {
    Actions.ValidateEntryDisableFields()
});

Then("linked entry status should be {string}", (status) => {
    cy.BackButton(BaseSelectors.ContainsShipment);
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossdockContext.EntryNumber),status)

});
//#endregion

//#region Edit entry
Given("the user open the created entry", () => {
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossdockContext.EntryNumber), null)
});

Given("fill the entry with the following details", (dataTable) => {
    let entryDetails = Assists.CreateInstance<CrossdockDetails>(dataTable, true);
    cy.FillDate(CrossdockSelectors.CrossdockActualEntryDate , entryDetails.ActualEntryDate)
    cy.FillDate(CrossdockSelectors.CrossdockActualEntryTime , entryDetails.ActualEntryTime)
});

When("save entry", () => {
    Actions.UpdateCrossdock(); 
});

Then("the entry should update sucessfully", () => {
    Actions.AssertUpdateCrossdock();
});
//#endregion

function EntryNumberConversionMapping(linkedEntryDetails: CrossdockDetails): CrossdockDetails {
    linkedEntryDetails.EntryNumber = linkedEntryDetails.EntryNumber.replace(/\"Created Entry Number\"/gi, CrossdockContext.EntryNumber);
    return linkedEntryDetails;
}

function ShipmentNumberConversionMapping(linkedShipmentDetails: LinkedShipmentDetails): LinkedShipmentDetails {
    linkedShipmentDetails.ShipmentNumber = linkedShipmentDetails.ShipmentNumber.replace(/\"Created Shipment Number\"/gi, CrossdockContext.ShipmentNumber);
    return linkedShipmentDetails;
}