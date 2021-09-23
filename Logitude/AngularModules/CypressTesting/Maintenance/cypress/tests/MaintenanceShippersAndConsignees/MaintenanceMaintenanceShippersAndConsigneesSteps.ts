import * as Actions from "../../actions/Actions";
import * as BaseActions from "../../actions/BaseActions";
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import { CardDetails } from "../../models/CardDetails";
import { ContactDetails } from "../../models/ContactDetails";
import { MaintenanceSelectors } from "../../selectors/Selectors";
import { Constants } from "../../constants/Constants";

let code = null
//#region Create new vessel
Given("the user logged in and open {string} in maintenance menu", (maintenanceItemName) => {
    cy.Login()
    Actions.OpenMaintenanceItemFromMaintenanceMenu(maintenanceItemName, MaintenanceSelectors.MaintenanceItemShipperAndConsignee)
});

Given("a Shipper-Consignee with the following details", (dataTable) => {
    Actions.OpenNewWizard("Customer");
    let cardDetails = Assists.CreateInstance<CardDetails>(dataTable, true);
    Actions.FillCardDetails(cardDetails, null);
});

Given("a Shipper-Consignee contact with the following details", (dataTable) => {
    let contactDetails = Assists.CreateInstance<ContactDetails>(dataTable, true);
    Actions.FillCardContactDetails(contactDetails)
});

When("create Shipper-Consignee", () => {
    Actions.CreateCard();
});

Then("the Shipper-Consignee should create successfully", () => {
    Actions.AssertCreateCard(Constants.Customer);
});
//#endregion

//#region Search for the Shipper-Consignee by code
When("search Shipper-Consignee", () => {
    code = Actions.getCardCode()
    BaseActions.Search(code)
});

Then("the Shipper-Consignee should appear successfully", () => {
    BaseActions.AssertSearch(code);
});
//#endregion

//#region Open the Shipper-Consignee
When("open Shipper-Consignee", () => {
    Actions.OpenCard(Constants.Customer)
});

Then("the Shipper-Consignee should open successfully", () => {
    Actions.AssertOpenCard()
});
//#endregion

//#region Edit the Shipper-Consignee
Given("a {string} as LocalName", (localName) => {
    Actions.FillCustomerLocalName(localName);
});

When("save Shipper-Consignee", () => {
    Actions.UpdateCustomer()
});

Then("the Shipper-Consignee should update successfully", () => {
    Actions.AssertUpdateCustomer()
});
//#endregion