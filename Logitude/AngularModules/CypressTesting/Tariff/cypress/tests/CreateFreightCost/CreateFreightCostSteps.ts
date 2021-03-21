import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { ChargeTypeDetails } from "../../models/ChargeTypeDetails";
import * as Assists from "../../../../Base/cypress/assists/Assists";

Given("the user logged in and navigate to tariff workspace", () => {
  Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air freight cost with the following details", (dataTable) => {
  let tariffDetails = Assists.CreateInstance<TariffDetails>(dataTable, true);
  Actions.FillNewFreightCost("Air", tariffDetails);
});

Given("an ocean LCL freight cost with the following details", (dataTable) => {
  let tariffDetails = Assists.CreateInstance<TariffDetails>(dataTable, true);
  Actions.FillNewFreightCost("Ocean LCL", tariffDetails);
});

Given("an ocean FCL freight cost with the following details", (dataTable) => {
  let tariffDetails = Assists.CreateInstance<TariffDetails>(dataTable, true);
  Actions.FillNewFreightCost("Ocean FCL", tariffDetails);
});

Given("the following All-In charges", (dataTable) => {
  let chargeTypeDetailsList = Assists.CreateSet<ChargeTypeDetails>(dataTable);
  Actions.FillAllInCharges(chargeTypeDetailsList);
});

When("create freight cost", () => {
  Actions.CreateTariff();
});

Then("the freight cost should create successfully", () => {
  Actions.ValidateCreateFreightCost();
});