import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { FreightCostTariffLineDetails } from "../../models/FreightCostTariffLineDetails";
import { PriceCheckDetails } from "cypress/models/PriceCheckDetails";

Given("the user logged in and navigate to tariff workspace", () => {
  Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an ocean FCL freight cost with the following details", (dataTable) => {
  let tariffDetails = dataTable.hashes()[0] as TariffDetails;
  Actions.FillNewFreightCost("Ocean FCL", tariffDetails);
});

Given("the user open the freight cost", () => {
  Actions.OpenLastCreatedTariff();
});

Given("add the following tariff line", (dataTable) => {
  let freightCostTariffLineDetailsList = dataTable.hashes() as FreightCostTariffLineDetails[];
  Actions.AddFreightCostTariffLines("Ocean FCL", freightCostTariffLineDetailsList);
});

Given("the user back into tariff workspace and open price check wizard", () => {
  Actions.BackToTariffWorkspace();
  Actions.OpenPriceCheckWizard("Ocean FCL");
});

Given("fill the following price check details", (dataTable) => {
  let oceanFCLPriceCheckDetails = dataTable.hashes()[0] as PriceCheckDetails;
  Actions.FillPriceCheckWizard("Ocean FCL",oceanFCLPriceCheckDetails)

});

When("create freight cost", () => {
  Actions.CreateTariff();
});

When("approve version", () => {
  Actions.ApproveTariffVersion();
});

When("search about prices", () => {
  Actions.PriceCheckSearch();
});

Then("the freight cost should create successfully", () => {
  Actions.ValidateCreateFreightCost();
});

Then("the version should approve successfully", () => {
  Actions.ValidateApproveTariffVersion();
});

Then("ocean FCL price should equal {string}", (expectedPrice: string) => {
  Actions.ValidateTariffPriceCheck(expectedPrice);
});