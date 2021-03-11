import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { FreightCostTariffLineDetails } from "../../models/FreightCostTariffLineDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";

Given("the user logged in and navigate to tariff workspace", () => {
    Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air freight cost with the following details", (dataTable) => {
    let tariffDetails = Assists.CreateInstance<TariffDetails>(dataTable, true);
    Actions.FillNewFreightCost("Air", tariffDetails);
});

Given("the user open the freight cost", () => {
    Actions.OpenLastCreatedTariff();
});

Given("add the following tariff lines", (dataTable) => {
    let freightCostTariffLineDetailsList = Assists.CreateSet<FreightCostTariffLineDetails>(dataTable);
    Actions.AddFreightCostTariffLines("Air", freightCostTariffLineDetailsList);
});

Given("the user in general tab", () => {
    Actions.OpenGeneralTab();
});

Given("the freight cost with new following details", (dataTable) => {
    let tariffDetails = Assists.CreateInstance<TariffDetails>(dataTable, true);
    Actions.EditFreightCostGeneralTab("Air", tariffDetails);
});

When("create freight cost", () => {
    Actions.CreateTariff();
});

When("update freight cost", () => {
    Actions.UpdateTariff();
});

Then("the freight cost should create successfully", () => {
    Actions.ValidateCreateFreightCost();
});

Then("the freight cost should update successfully", () => {
    Actions.ValidateUpdateTariff();
});