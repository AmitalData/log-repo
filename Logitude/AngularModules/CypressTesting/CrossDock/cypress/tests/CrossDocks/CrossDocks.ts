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
import { CrossDockDetails } from "../../models/CrossDockDetails";
import { LinkedShipmentDetails } from "../../models/LinkedShipmentDetails";
import { CrossdockSelectors } from "../../selectors/Selectors";
import { CrossDockContext } from "../../models/CrossDockContext";
import { LinkedReleaseDetails } from "../../models/LinkedReleaseDetails"
import * as Baseactions from "../../../../Base/cypress/actions/Actions"
import { DeliveryDetails } from "../../models/DeliveryDetails";
let shipmentDetails: ShipmentDetails;
let crossdockDetails: CrossDockDetails;

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
        CrossDockContext.ShipmentNumber = interception.response.body.ShipmentNumber;
    })
});

//#endregion

//#region Add packages
Given("the user open the shipment and navigate to packages tab", () => {
    ShipmentActions.OpenShipment(CrossDockContext.ShipmentNumber);
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

//#region Add cross dock entry/release
Given("the user in Connected Entities tab", () => {
    cy.Navigate(ShipmentSelectors.ConnectionsTab,true);
});

Given("a corss dock {string} with the following details", (crossDockType, dataTable) => {
    crossdockDetails = Assists.CreateInstance<CrossDockDetails>(dataTable, true);
    Actions.OpenNewCrossDock(crossDockType)
    Actions.FillCrossdockWizardsFields(crossdockDetails, crossDockType);
});

When("create cross dock entry", () => {
    Actions.CreateCrossdockEntry();
});

When("create cross dock release", () => {
    Actions.CreateCrossdockRelease();
});

Then("the cross dock entry should create successfully", () => {
    Actions.AssertCreateCrossdockEntry();
});

Then("the shipment should contain the linked entry details", (dataTable) => {
    let linkedEntryDetails = Assists.CreateInstance<CrossDockDetails>(dataTable, true);
    linkedEntryDetails = EntryNumberConversionMapping(linkedEntryDetails)
    Actions.ValidateLinkedEntryInShipment(linkedEntryDetails);
});

Then("the cross dock {string} should contain the linked shipment details", (crossDockType, dataTable) => {
    let linkedShipmentDetails = Assists.CreateInstance<LinkedShipmentDetails>(dataTable, true);
    linkedShipmentDetails = ShipmentNumberConversionMapping(linkedShipmentDetails);
    Actions.ValidateLinkedShipmentInCrossDock(crossDockType, linkedShipmentDetails);
});

Then("the cross dock release should create successfully", () => {
    Actions.AssertCreateCrossdockRelease();
});

Then("shipment should contain the linked release details", (dataTable) => {
    let linkedReleaseDetails = Assists.CreateInstance<CrossDockDetails>(dataTable, true);
    linkedReleaseDetails = ReleaseNumberConversionMapping(linkedReleaseDetails)
    Actions.ValidateLinkedReleaseInShipment(linkedReleaseDetails);
});

Then("the linked cross dock entry should contain this cross dock release details", (dataTable) => {
    let linkedReleaseDetails = Assists.CreateInstance<LinkedReleaseDetails>(dataTable, true);
    linkedReleaseDetails = ReleaseDetailsConversionMapping(linkedReleaseDetails)
    Actions.ValidateLinkedReleaseInEntry(linkedReleaseDetails);
    cy.BackButton(BaseSelectors.ContainsShipment + CrossDockContext.ShipmentNumber)
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
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, status)
});

Then("entry fields should be disable", () => {
    Actions.ValidateEntryDisableFields()
});

Then("linked entry status should be {string}", (status) => {
    cy.BackButton(BaseSelectors.ContainsShipment);
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossDockContext.EntryNumber), status)
});
//#endregion

//#region Edit entry
Given("the user open the created entry", () => {
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossDockContext.EntryNumber), null)
});

Given("fill the entry with the following details", (dataTable) => {
    let entryDetails = Assists.CreateInstance<CrossDockDetails>(dataTable, true);
    Actions.FillEntryDate(entryDetails);
});

When("save entry", () => {
    Actions.UpdateCrossdockEntry();
});

Then("the entry should update sucessfully", () => {
    Actions.AssertUpdateCrossdockEntry();
});

Then("entry status should be {string}", (status) => {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, status)
});

Then("entry date should be {string}", (date) => {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, date)
});

Then("the Warehouse Terminal in shipment routing tab should have the following details", (dataTable) => {
    let entryDetails = Assists.CreateInstance<CrossDockDetails>(dataTable, true);
    cy.BackButton(BaseSelectors.ContainsShipment + CrossDockContext.ShipmentNumber)
    Actions.ValidateRoutingsEntryFields(entryDetails)
});
//#endregion 
//#region Cancel Release
When("cancel the release", () => {
    Actions.CancelRelease();
});

Then("the release should cancel successfully", () => {
    Actions.AssertCancleRelease()
    
});

Then("release status should be {string}", (ReleaseStatus) => {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, status)
});

Then("release fields should be disabled", () => {
    Actions.ValidateReleaseDisableFields()
});

Then("linked release status should be {string}", (status) => {
    cy.BackButton(BaseSelectors.ContainsShipment);
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossDockContext.ReleaseNumber), status)
    Actions.NavigateToShipmentConnectedEntities();
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossDockContext.ReleaseNumber), status)
    cy.BackButton(BaseSelectors.ContainsShipment + CrossDockContext.ShipmentNumber)
});
//#endregion

//#region Edit release
Given("the user open the created release", () => {
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossDockContext.ReleaseNumber), null)
});

Given("fill the release with the following details", (dataTable) => {
    let releaseDetails = Assists.CreateInstance<CrossDockDetails>(dataTable, true);
    Actions.FillReleaseDate(releaseDetails);
});

When("save release", () => {
    Actions.UpdateCrossdockRelease();
});

Then("the release should update sucessfully", () => {
    Actions.AssertUpdateCrossdockRelease();
});

Then("release status should be {string}", (status) => {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, status)
});

Then("release date should be {string}", (date) => {
    BaseAssertion.AssertElementContain(BaseSelectors.HeaderScreen, date)
});

Then("the Warehouse Terminal in shipment routing tab should have the following release details", (dataTable) => {
    let releaseDetails = Assists.CreateInstance<CrossDockDetails>(dataTable, true);
    cy.BackButton(BaseSelectors.ContainsShipment + CrossDockContext.ShipmentNumber)
    Actions.ValidateRoutingsReleaseFields(releaseDetails)
});
//#endregion

//#region Add delivery
When("Add delivery", () => {
    cy.Navigate(ShipmentSelectors.ConnectionsTab);
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossDockContext.ReleaseNumber), null)
    Actions.AddDelivery()
});

Then("the delivery should add successfully", () => {
    Actions.AssertAddDelivery()
});

Then("the delivery leg should appear in the shipment routing tab with the following details", (dataTable) => {
    let deliveryDetails = Assists.CreateInstance<DeliveryDetails>(dataTable, true);
    cy.BackButton(BaseSelectors.ContainsShipment + CrossDockContext.ShipmentNumber)
    Actions.ValidateRoutingsReleaseDeliveryLegFields(deliveryDetails)
});
//#endregion
function EntryNumberConversionMapping(linkedEntryDetails: CrossDockDetails): CrossDockDetails {
    linkedEntryDetails.EntryNumber = linkedEntryDetails.EntryNumber.replace(/\"Created Entry Number\"/gi, CrossDockContext.EntryNumber);
    return linkedEntryDetails;
}
function ReleaseNumberConversionMapping(linkedEntryDetails: CrossDockDetails): CrossDockDetails {
    linkedEntryDetails.ReleaseNumber = linkedEntryDetails.ReleaseNumber.replace(/\"Created Release Number\"/gi, CrossDockContext.ReleaseNumber);
    return linkedEntryDetails;
}
function ShipmentNumberConversionMapping(linkedShipmentDetails: LinkedShipmentDetails): LinkedShipmentDetails {
    linkedShipmentDetails.ShipmentNumber = linkedShipmentDetails.ShipmentNumber.replace(/\"Created Shipment Number\"/gi, CrossDockContext.ShipmentNumber);
    return linkedShipmentDetails;
}
function ReleaseDetailsConversionMapping(linkedReleaseDetails: LinkedReleaseDetails) {
    linkedReleaseDetails.ShipmentNumber = linkedReleaseDetails.ShipmentNumber.replace(/\"Created Shipment Number\"/gi, CrossDockContext.ShipmentNumber);
    linkedReleaseDetails.ReleaseNumber = linkedReleaseDetails.ReleaseNumber.replace(/\"Created Release Number\"/gi, CrossDockContext.ReleaseNumber);
    return linkedReleaseDetails;
}