import { TariffSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { TariffDetails } from "../models/TariffDetails";
import { ChargeTypeDetails } from "../models/ChargeTypeDetails";
import { SurchargeDetails } from "../models/SurchargeDetails";
import { Urls } from "../constants/Urls";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";

export function LoginAndNavigateToTariffWorkspace() {
    cy.Login();
    cy.Click(TariffSelectors.TariffMenu, null);
}

export function OpenNewFreightCostWizard(freightCostType: string) {
    cy.Click(TariffSelectors.NewFreightCostToggleButton, null);
    cy.Click(BaseSelectors.ToggleMenuButton, freightCostType);
}

export function FillFreightCostWizardFields(freightCostType: string, tariffDetails: TariffDetails){
    cy.FillLogTextBox(TariffSelectors.TariffName, tariffDetails.Name);
    cy.FillLogLov(TariffSelectors.TariffSeller, tariffDetails.Seller, false);
    cy.FillDate(TariffSelectors.TariffStartDate, tariffDetails.StartDate);
    if(freightCostType === "Air"){
        cy.FillLogLov(TariffSelectors.TariffProduct, tariffDetails.Product, true);
    }
}

export function FillAllInCharges(chargeTypeDetailsList: ChargeTypeDetails[]){
    cy.Click(TariffSelectors.EditTariffAllInChargesButton, null, true);
    cy.contains("Click the add button to add new lines");
    for (let i = 0; i < chargeTypeDetailsList.length; i++) {
        cy.Click(BaseSelectors.AddButton, null);
        cy.FillLogLov(TariffSelectors.TariffVersionAllInChargeType, chargeTypeDetailsList[i].Name, true);
    }
    cy.Click(BaseSelectors.RedButton + ":last", null);
}

export function CreateTariff() {
    cy.DefineRequestWait("POST", Urls.Tariffs, "WaitPostTariff");
    cy.Click(BaseSelectors.RedButton + ":last", null);
}

export function ValidateCreatedTariff() {
    BaseAssertion.AssertStatusCode("WaitPostTariff", 200);
}


































export function OpenNewSurchargeCostWizard(surchargeCostType: string) {
    cy.Click(TariffSelectors.NewSurchargeCostToggleButton, null);
    cy.Click(BaseSelectors.ToggleMenuButton, surchargeCostType);
}

export function FillSurchargeCostWizardFields(tariffDetails: TariffDetails){
    cy.FillLogTextBox(TariffSelectors.TariffName, tariffDetails.Name);
    cy.FillLogLov(TariffSelectors.TariffSeller, tariffDetails.Seller, false);
}

export function FillSurcharges(surchargeDetailsList: SurchargeDetails[]){
    for (let i = 1; i <= surchargeDetailsList.length; i++) {
        if(i <= 10){
            cy.FillLogLov(TariffSelectors.TariffSurcharge(i), surchargeDetailsList[i].Name, true);
            cy.FillLogLov(TariffSelectors.TariffSurchargeMeasurement(i), surchargeDetailsList[i].MeasurementUnit, true);
        }
    }
}