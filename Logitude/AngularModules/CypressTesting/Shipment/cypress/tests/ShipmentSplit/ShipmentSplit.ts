import * as Actions from "../../actions/Actions";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ShipmentDetails } from "../../models/ShipmentDetails";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion"
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../selectors/Selectors";
import { PackagesDetails } from "cypress/models/PackagesDetails";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import { URLs } from "../../constants/URLs";
import * as Assists from "../../../../Base/cypress/assists/Assists";

let shipmentDetails: ShipmentDetails;
let shipmentNumber: string;
let FirstShipmentID: string;
let containerDetailsList

//#region  Create Direct Shipment
Given("the user logged in and navigates to shipments workspace", () => {
    cy.Login()
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
        FirstShipmentID = interception.response.body.Id;
    });
});
//#endregion

//#region Add Containers
Given("the user open the shipment and navigate to packages workspace", () => {
    Actions.OpenShipment(shipmentNumber);
    cy.Navigate(ShipmentSelectors.PackagesTab);
});

Given("a container with the following details", (dataTable) => {
    containerDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(shipmentDetails.TransportMode, containerDetailsList, shipmentDetails.ShipmentType);
});

When("save shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Split Shipment
When("split the shipment with the second container", () => {
    Actions.SplitShipment(containerDetailsList[1].ContainerNumber);
});

Then("the direct shipment should split successfully", () => {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetAll, RequestAliases.GetAll)
    BaseAssertion.AssertStatusCode(RequestAliases.SplitShipmentRequest, 200);

    //#region Validate for new shipment 
    Actions.ValidateShipmentNumber(shipmentNumber)
    Actions.ValidatePackageDetails(ShipmentSelectors.ShipmentPackagesTab, containerDetailsList[1], ShipmentSelectors.Shipment_GrossWeight, false)
    Actions.ValidateShipmentEventActions(ShipmentSelectors.ShipmentEventTab, "Split From Shipment: " + shipmentNumber)
    //#endregion

    //#region Validate for old shipment
    cy.BackButton("Shipment: " + shipmentNumber);
    Actions.ValidatePackageDetails(ShipmentSelectors.PackagesTab, containerDetailsList[0], ShipmentSelectors.PackageGrossWeight, false)
    //#endregion
});
//#endregion
