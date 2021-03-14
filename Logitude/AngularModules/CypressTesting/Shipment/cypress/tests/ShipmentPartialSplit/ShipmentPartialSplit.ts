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
let packageDetailsList, partialSplitDetails, expectedPackagesDetailList

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

//#region Add Packages
Given("the user open the shipment and navigate to packages workspace", () => {
    Actions.OpenShipment(shipmentNumber);
    cy.Navigate(ShipmentSelectors.PackagesTab);
});

Given("a package with the following details", (dataTable) => {
    packageDetailsList = Assists.CreateSet<PackagesDetails>(dataTable);
    Actions.FillPackageTab(shipmentDetails.TransportMode, packageDetailsList, shipmentDetails.ShipmentType);
});

When("save shipment", () => {
    Actions.UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
});

Then("the direct shipment should save successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
});
//#endregion

//#region Partial Split Shipment
When("partial split the shipment with the following details", (dataTable) => {
    partialSplitDetails = Assists.CreateInstance<PackagesDetails>(dataTable, true);
    Actions.PartialSplitShipment(partialSplitDetails);
});

Then("the direct shipment should split into two shipment with packages with the following details", (dataTable) => {
    let expectedPackagesDetailList = Assists.CreateSet<PackagesDetails>(dataTable);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetAll, RequestAliases.GetAll)
    BaseAssertion.AssertStatusCode(RequestAliases.SplitShipmentRequest, 200);

    //#region Validate for new shipment 
    Actions.ValidateShipmentNumber(shipmentNumber)
    Actions.ValidatePackageDetails(ShipmentSelectors.ShipmentPackagesTab, expectedPackagesDetailList[0], ShipmentSelectors.Shipment_GrossWeight, true)
    Actions.ValidateShipmentEventActions(ShipmentSelectors.ShipmentEventTab, "Split From Shipment: " + shipmentNumber)
    //#endregion

    //#region Validate for old shipment
    cy.BackButton("Shipment: " + shipmentNumber);
    Actions.ValidatePackageDetails(ShipmentSelectors.PackagesTab, expectedPackagesDetailList[1], ShipmentSelectors.PackageGrossWeight, true)
    //#endregion

});
//#endregion


