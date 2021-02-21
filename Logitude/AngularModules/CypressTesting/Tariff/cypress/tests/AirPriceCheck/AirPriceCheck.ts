import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { FreightCostTariffLineDetails } from "cypress/models/FreightCostTariffLineDetails";
import { PriceCheckDetails } from "cypress/models/PriceCheckDetails";

//#region Air Create Freight Cost 
Given("the user logged in and navigate to tariff workspace", () => {
    Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air freight cost with the following details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffDetails;
    Actions.FillNewFreightCost("Air", tariffDetails);
});

When("create freight cost", () => {
    Actions.CreateTariff();
});

Then("the freight cost should create successfully", () => {
    Actions.ValidateCreateFreightCost(); 
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
When("approve version", () => {
    Actions.ApproveTariffVersion();
});

Then("the version should approve successfully", () => {
    Actions.ValidateApproveTariffVersion();
});
//#endregion

//#region Check Air Price 
Given("the user back into tariff workspace and open price check wizard", () => {
    Actions.BackToTariffWorkspace();
    Actions.OpenPriceCheckWizard("Air");
});

Given("fill the following price check details", (dataTable) => {
    let airPriceCheckDetails = dataTable.hashes()[0] as PriceCheckDetails;
    Actions.FillPriceCheckWizard("Air",airPriceCheckDetails)

});

When("search about prices", () => {
    Actions.PriceCheckSearch();
});

Then("air price should equal {string}", (ExpectedResult) => {
    Actions.ValidateTariffPriceCheck(ExpectedResult);
});
//#endregion