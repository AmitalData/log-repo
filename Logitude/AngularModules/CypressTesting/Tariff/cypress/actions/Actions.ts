import { TariffSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { TariffDetails } from "../models/TariffDetails";
import { ChargeTypeDetails } from "../models/ChargeTypeDetails";
import { SurchargeDetails } from "../models/SurchargeDetails";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";

export function LoginAndNavigateToTariffWorkspace() {
    cy.Login();
    cy.Click(TariffSelectors.TariffMenu, null);
}

export function OpenNewFreightCostWizard(freightCostType: string) {
    cy.Click(TariffSelectors.NewFreightCostToggleButton, null);
    cy.Click(TariffSelectors.NewFreightCostToggleMenuButton, freightCostType);
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

export function OpenNewSurchargeCostWizard(surchargeCostType: string) {
    cy.Click(TariffSelectors.NewSurchargeCostToggleButton, null);
    cy.Click(TariffSelectors.NewSurchargeCostToggleMenuButton, surchargeCostType);
}

export function FillSurchargeCostWizardFields(tariffDetails: TariffDetails){
    cy.FillLogTextBox(TariffSelectors.TariffName, tariffDetails.Name);
    cy.FillLogLov(TariffSelectors.TariffSeller, tariffDetails.Seller, false);
}

export function FillSurcharges(surchargeDetailsList: SurchargeDetails[]){
    for (let i = 0; i < surchargeDetailsList.length; i++) {
        if(i < 10){
            cy.FillLogLov(TariffSelectors.TariffSurcharge(i + 1), surchargeDetailsList[i].Name, true);
        }
    }
}

export function CreateTariff() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Tariffs, RequestAliases.PostTariff);
    cy.Click(BaseSelectors.RedButton + ":last", null);
}

export function ValidateCreatedFreightCost(){
    BaseAssertion.AssertStatusCode(RequestAliases.PostTariff, 200);
}

export function ValidateCreatedSurchargeCost() {
    let intercept = cy.wait("@" + RequestAliases.PostTariff);
    intercept.then((interception) => {
        if(interception.response.statusCode === 400){
            if(interception.response.body.ErrorMessage.indexOf("Tariff surcharge seller should be unique") !== -1){
                cy.Click(BaseSelectors.Button, "Cancel");
            }else{
                throw new Error("Created Tariff Failed");
            }
        }else{
            if(interception.response.statusCode !== 200){
                throw new Error("Created Tariff Failed");
            }
        }
    })
}