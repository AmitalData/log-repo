import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { FreightCostTariffLineDetails } from "../../models/FreightCostTariffLineDetails";

Given("the user logged in and navigate to tariff workspace", () => {
    Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air freight cost with the following details", (dataTable) => {
    let tariffDetails = dataTable.hashes()[0] as TariffDetails;
    Actions.FillNewFreightCost("Air", tariffDetails);
});

Given("the user open the freight cost", () => {
    Actions.OpenLastCreatedTariff();
});

Given("add the following tariff lines", (dataTable) => {
    let freightCostTariffLineDetailsList = dataTable.hashes() as FreightCostTariffLineDetails[];
    Actions.AddFreightCostTariffLines("Air", freightCostTariffLineDetailsList);
});

Given("the following new values for the tariff lines", (dataTable) => {
    let freightCostTariffLineDetailsList = dataTable.hashes() as FreightCostTariffLineDetails[];
    Actions.EditFreightCostTariffLines("Air", freightCostTariffLineDetailsList);
});

When("create freight cost", () => {
    Actions.CreateTariff();
});

When("approve version", () => {
    Actions.ApproveTariffVersion();
});

When("copy version with start date {string}", (newVersionStartDate: string) => {
    Actions.CopyTariffVersion(newVersionStartDate);
});

When("open version history tab", () => {
    Actions.OpenVersionHistoryTab();
});

Then("the freight cost should create successfully", () => {
    Actions.ValidateCreateFreightCost();
});

Then("the version should approve successfully", () => {
    Actions.ValidateApproveTariffVersion();
});

Then("the version should copy successfully", () => {
    Actions.ValidateCopyTariffVersion();
});

Then("all approved versions should appear successfully", () => {
    Actions.ValidateApprovedVersionsAppear();
});