import * as Actions from "../../actions/Actions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { ContactDetails } from "../../models/ContactDetails";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { CardDetails } from "../../../cypress/models/CardDetails";
import { CardGeneralTabDetails } from "cypress/models/CardGeneralTabDetails";
import { CardBillingTabDetails } from "cypress/models/CardBillingTabDetails";
import { Constants } from '../../constants/Constants'

let vendorDetails: CardDetails
let contactDetails: ContactDetails
let cardGeneralTabDetails:CardGeneralTabDetails
let cardBillingTabDetails:CardBillingTabDetails
//#region Create new vendor
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.VendorMaintenanceItem)
});

Given("a vendor with the following details", (dataTable) => {
    vendorDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    Actions.OpenNewWizard("Vendor");
    Actions.FillVendorDetails(vendorDetails)
});

Given("a vendor contact with the following details", (dataTable) => {
    let contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    Actions.FillVendorContactDetails(contactDetails)
});

When("create vendor", () => {
    Actions.CreateVendor();
});

Then("the vendor should create successfully", () => {
    Actions.AssertCreateVendor()
});

//#endregion

//#region Search for the vendor by code
When("search vendor", () => {
    Actions.SearchVendor();
});

Then("the vendor should appear successfully", () => {
    Actions.AssertSearchVendor(vendorDetails.CompanyName);
});

//#endregion

//#region Open the vendor
When("open vendor", () => {
   Actions.OpenCard(Constants.Vendor);
});

Then("the vendor should open successfully", () => {
    Actions.AssertOpenVendor(); 
});
//#endregion

//#region Edit the vendor
Given("the user fill the following vendor details", (dataTable) => {
    let vendorDetails = Assists.CreateInstance<CardGeneralTabDetails>(dataTable, true);
    Actions.FillVendorGeneralTab(vendorDetails)
});

Given("fill the following vendor Billing details", (dataTable) => {
    let vendorBillingDetails = Assists.CreateInstance<CardBillingTabDetails>(dataTable, true);
    cy.Navigate(MaintenanceSelectors.VendorBillingTab);
    Actions.FillVendorBillingTab(vendorBillingDetails);
});

When("save vendor", () => {
    Actions.UpdateVendor()
});

Then("the vendor should update successfully", () => {
    Actions.AssertUpdateVendor()
});

//#endregion

//#region Save and close the vendor
When("save and close vendor", () => {
    Actions.CloseSaveVendor(); 
});

Then("the vendor should close successfully", () => {
    Actions.AssertCloseSaveVendor();
});

//#endregion