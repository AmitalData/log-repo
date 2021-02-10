import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { ChargeTypeDetails } from "../../models/ChargeTypeDetails";

Given("the user logged in and navigate to tariff workspace", () => {
    Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air freight cost with the following details", (dataTable) => {
    FillNewFreightCost("Air", dataTable);
});

Given("an ocean LCL freight cost with the following details", (dataTable) => {
  FillNewFreightCost("Ocean LCL", dataTable);
});

Given("an ocean FCL freight cost with the following details", (dataTable) => {
  FillNewFreightCost("Ocean FCL", dataTable);
});

Given("add the follwing All-In charges", (dataTable) => {
  let chargeTypeDetailsList = dataTable.hashes() as ChargeTypeDetails[];
  Actions.FillAllInCharges(chargeTypeDetailsList);
});

When("create freight cost", () => {
    Actions.CreateTariff();
});

Then("the freight cost should create successfully", () => {
  Actions.ValidateCreatedTariff();
});


function FillNewFreightCost(freightCostType: string, dataTable: any){
  let tariffDetails = dataTable.hashes()[0] as TariffDetails;
  Actions.OpenNewFreightCostWizard(freightCostType);
  Actions.FillFreightCostWizardFields(freightCostType, tariffDetails);
}