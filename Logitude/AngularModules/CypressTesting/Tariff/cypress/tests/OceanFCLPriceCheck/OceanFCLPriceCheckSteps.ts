import { Given, When, Then } from "cypress-cucumber-preprocessor/steps";
import * as Actions from "../../actions/Actions";
import { TariffDetails } from "../../models/TariffDetails";
import { ChargeTypeDetails } from "../../models/ChargeTypeDetails";
import { FreightCostTariffLineDetails } from "../../models/FreightCostTariffLineDetails";
import { OceanFCLPriceCheckDetails } from "../../models/OceanFCLPriceCheckDetails";

let FreightCostTariffLines: FreightCostTariffLineDetails[];
let OceanFCLPriceCheck: OceanFCLPriceCheckDetails;
let TariffNumber: string;

Given("the user logged in and navigate to tariff workspace", () => {
  Actions.LoginAndNavigateToTariffWorkspace();
});

Given("an ocean FCL freight cost with the following details", (dataTable) => {
  let tariffDetails = dataTable.hashes()[0] as TariffDetails;
  Actions.FillNewFreightCost("Ocean FCL", tariffDetails);
});

Given("the following All-In charges", (dataTable) => {
  let chargeTypeDetailsList = dataTable.hashes() as ChargeTypeDetails[];
  Actions.FillAllInCharges(chargeTypeDetailsList);
});

Given("the user open the freight cost", () => {
  Actions.OpenLastCreatedTariff();
});

Given("add the following tariff line", (dataTable) => {
  let freightCostTariffLineDetailsList = dataTable.hashes() as FreightCostTariffLineDetails[];
  FreightCostTariffLines = freightCostTariffLineDetailsList;
  //Actions.AddFreightCostTariffLines("Ocean FCL", freightCostTariffLineDetailsList);
});

Given("the user back into tariff workspace and open price check wizard", () => {
  //Actions.BackToTariffWorkspace();
  Actions.OpenPriceCheckWizard("Ocean FCL");
});

Given("fill the following price check details", (dataTable) => {
  let oceanFCLPriceCheckDetails = dataTable.hashes()[0] as OceanFCLPriceCheckDetails;
  OceanFCLPriceCheck = oceanFCLPriceCheckDetails;
  Actions.FillOceanFCLPriceCheckWizard(oceanFCLPriceCheckDetails);
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
  TariffNumber = Actions.ValidateCreateFreightCost();
  cy.log(TariffNumber);
});

Then("the version should approve successfully", () => {
  Actions.ValidateApproveTariffVersion();
});

Then("prices should calculate correctly", () => {

  let oceanFCLPrice = Actions.CalculateOceanFCLPrice(OceanFCLPriceCheck, FreightCostTariffLines);

  cy.get(".LogitudeScrollViewer.LogitudeSmallScrollViewer > table > tr").each((row, index, list) => {

    if(row.text().indexOf("More Details") !== -1){
      
      cy.get(".LogitudeScrollViewer.LogitudeSmallScrollViewer > table > tr").eq(index).find("img[src='./Images/Buttons/downarrow.png']").click();

      cy.DefineRequestWait("GET", "**/tariffs/getsingle?**", "GetSingleTariff");

      cy.get(".LogitudeScrollViewer.LogitudeSmallScrollViewer > table > tr").eq(index).find(".hyperlink").contains("View Tariff").click();

      cy.wait("@GetSingleTariff");
      
      cy.get(".LogitudeWindow:last .ShortTitleDiv:first").then((tariffNumberDiv) => {
        let isDesiredTariff = false;

        if(tariffNumberDiv.text().replace(":", "").trim() === TariffNumber){
          isDesiredTariff = true;
        }

        cy.get("img[src='./Images/WindowIcons/WindowClose.png']").click();

        cy.get(".LogitudeScrollViewer.LogitudeSmallScrollViewer > table > tr").eq(index).find("img[src='./Images/Buttons/uparrow.png']").click();

        if(isDesiredTariff){
          //assert
          cy.log("OK");
        }

      });
      

    }

  });


});