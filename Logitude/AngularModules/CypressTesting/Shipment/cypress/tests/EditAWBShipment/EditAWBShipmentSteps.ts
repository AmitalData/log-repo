import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import { PackagesDetails } from "../../models/PackagesDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { ShipmentSelector } from "../../selectors/Selectors";

let ShipmentData: ShipmentDetails;
let ShipmentNumber: string;

Given("the user logged in and navigate to shipments workspace", () => {
    cy.Login();
    Actions.NavigatesToShipmentsWorkspace();
});

Given("a direct shipment with the following details", (dataTable) => {
    let shipmentDetails = dataTable.hashes()[0] as ShipmentDetails;
    ShipmentData = shipmentDetails;
    Actions.OpenNewShipmentWizard(ShipmentData.ShipmentLevel);
    Actions.FillShipmentWizardsFields(ShipmentData);
});

Given("main carriage airline is {string} with random flight number and MAWB", (airline) => {
    Actions.FillMainCarriage(airline);
});

When("create shipment", () => {
    Actions.CreateShipment(ShipmentData.ShipmentLevel);
});

Then("the shipment should create successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPostShipmentRequest", 200).then((interception) => {
        ShipmentNumber = interception.response.body.ShipmentNumber;
    });
});

Given("the user open the direct shipment", () => {
    Actions.OpenShipment(ShipmentNumber);
});

When("open the AWB wizard", () => {
    Actions.OpenAWBWizard(ShipmentData.ShipmentLevel);
});

Then("the overview tab should appear successfully", () => {
    BaseAssertion.AssertElementExist(ShipmentSelector.OverviewTabComponentInAWBWizard);
    BaseAssertion.AssertElementHaveClass(ShipmentSelector.OverviewTabInAWBWizard, "Selected");
});

Given("the user in the AWB wizard packages tab", () => {
    cy.Click(ShipmentSelector.PackagesTabInAWBWizard, null);
});

Given("add the follwing packages", (dataTable) => {
    let packagesDetailsList = dataTable.hashes() as PackagesDetails[];
    Actions.FillAWBWizardPackagesTab(packagesDetailsList);
});

When("save the AWB wizard", () => {
    Actions.UpdateShipment(BaseSelectors.SaveWizard);
});

Then("the shipment should update successfully", () => {
    BaseAssertion.AssertStatusCode("WaitPutShipmentRequest", 200);
});