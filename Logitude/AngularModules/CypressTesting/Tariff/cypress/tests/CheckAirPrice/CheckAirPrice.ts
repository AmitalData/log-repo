import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { ChargeTypeDetails } from "../../models/ChargeTypeDetails";
import { FreightCostTariffLineDetails } from "cypress/models/FreightCostTariffLineDetails";
import { AirPriceCheck } from "cypress/models/AirPriceCheck";
import { TariffSelectors } from "../../selectors/Selectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../../Base/cypress/constants/RequestAliases";

var CreatedTariffNumber;
//#region Air Create Freight Cost 
Given("the user logged in and navigate to tariff workspace", () => {
    Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air freight cost with the following details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffDetails;
    Actions.FillNewFreightCost("Air", tariffDetails);
});

Given("the follwing All-In charges", (dataTable) => {
    let chargeTypeDetailsList = dataTable.hashes() as ChargeTypeDetails[];
    Actions.FillAllInCharges(chargeTypeDetailsList);
});

When("create freight cost", () => {
    Actions.CreateTariff();
});

Then("the freight cost should create successfully", () => {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTariff, 200).then((interception) => {
        CreatedTariffNumber = interception.response.body.TariffNumber;
    })
    Actions.AssertGetRecentTariffs();
});
//#endregion

//#region Add Tariff Line 
Given("the user open the freight cost", () => {
    Actions.OpenLastCreatedTariff();
});

Given("add the follwing tariff line", (dataTable) => {
    let freightCostTariffLineDetailsList = dataTable.hashes() as FreightCostTariffLineDetails[];
    Actions.AddFreightCostTariffLines("Air", freightCostTariffLineDetailsList);
});
When("update freight cost", () => {
    Actions.UpdateApprovedTariff();
});

Then("the freight cost should update successfully", () => {
    Actions.ValidateUpdateTariff();
});
//#endregion

Given("the user in the air's price check workspace", () => {
    cy.Click(TariffSelectors.TariffEditBackbutton, null)
    cy.Click(TariffSelectors.PriceCheckQuery, "Air");
});

When("enter the following details", (dataTable) => {
    let priceCheck = dataTable.hashes()[0] as AirPriceCheck;
    Actions.FillAirPriceCheck(priceCheck);
});

Then("the result should be {string}", (ExpectedResult) => {
   Actions.PriceCheckAssertion(CreatedTariffNumber,ExpectedResult)
});