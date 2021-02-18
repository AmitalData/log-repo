import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { SurchargeDetails } from "../../models/SurchargeDetails";

Given("the user logged in and navigate to tariff workspace", () => {
  Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an air surcharge cost with the following details", (dataTable) => {
  let tariffDetails = dataTable.hashes()[0] as TariffDetails;
  Actions.FillNewSurchargeCost("Air", tariffDetails);
});

Given("an ocean LCL surcharge cost with the following details", (dataTable) => {
  let tariffDetails = dataTable.hashes()[0] as TariffDetails;
  Actions.FillNewSurchargeCost("Ocean LCL", tariffDetails);
});

Given("an ocean FCL surcharge cost with the following details", (dataTable) => {
  let tariffDetails = dataTable.hashes()[0] as TariffDetails;
  Actions.FillNewSurchargeCost("Ocean FCL", tariffDetails);
});

Given("add the follwing surcharges", (dataTable) => {
  let surchargeDetailsList = dataTable.hashes() as SurchargeDetails[];
  Actions.FillSurcharges(surchargeDetailsList);
});

When("create surcharge cost", () => {
  Actions.CreateTariff();
});

Then("the surcharge cost should create successfully", () => {
  Actions.ValidateCreateSurchargeCost();
});