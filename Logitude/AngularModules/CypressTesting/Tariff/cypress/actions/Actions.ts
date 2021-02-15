import { TariffSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { TariffDetails } from "../models/TariffDetails";
import { ChargeTypeDetails } from "../models/ChargeTypeDetails";
import { SurchargeDetails } from "../models/SurchargeDetails";
import { FreightCostTariffLineDetails } from "../models/FreightCostTariffLineDetails";
import { Urls } from "../constants/Urls";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";

export function LoginAndNavigateToTariffWorkspace() {
    cy.Login();
    cy.Click(TariffSelectors.TariffMenu, null);
}

export function FillNewFreightCost(freightCostType: string, tariffDetails: TariffDetails) {
    OpenNewFreightCostWizard(freightCostType);
    FillFreightCostWizardFields(freightCostType, tariffDetails);
}

export function FillNewSurchargeCost(surchargeCostType: string, tariffDetails: TariffDetails) {
    OpenNewSurchargeCostWizard(surchargeCostType);
    FillSurchargeCostWizardFields(tariffDetails);
}

export function FillAllInCharges(chargeTypeDetailsList: ChargeTypeDetails[]) {
    cy.Click(TariffSelectors.EditTariffAllInChargesButton, null, true);
    cy.contains("Click the add button to add new lines");
    for (let i = 0; i < chargeTypeDetailsList.length; i++) {
        cy.Click(BaseSelectors.AddButton, null);
        if(chargeTypeDetailsList[i].Name){
            cy.FillLogLov(TariffSelectors.TariffVersionAllInChargeType, chargeTypeDetailsList[i].Name, true);
        }
    }
    cy.Click(BaseSelectors.RedButton + ":last", null);
}

export function FillSurcharges(surchargeDetailsList: SurchargeDetails[]) {
    for (let i = 0; i < surchargeDetailsList.length; i++) {
        if (i < 10) {
            if(surchargeDetailsList[i].Name){
                cy.FillLogLov(TariffSelectors.TariffSurcharge(i + 1), surchargeDetailsList[i].Name, true);
            }
        }
    }
}

export function CreateTariff() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Tariffs, RequestAliases.PostTariff);
    cy.DefineRequestWait(RestAPI.GET, Urls.TariffDomainGetRecentTariffs, RequestAliases.GetRecentTariffs);
    cy.Click(BaseSelectors.RedButton + ":last", null);
}

export function UpdateTariff() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Tariffs, RequestAliases.PutTariff);
    cy.Click(TariffSelectors.SaveTariff, null);
}

export function ValidateUpdateTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTariff, 200);
}

export function ValidateCreateFreightCost() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTariff, 200);
    cy.wait("@" + RequestAliases.GetRecentTariffs);
}

export function ValidateCreateSurchargeCost() {
    let intercept = cy.wait("@" + RequestAliases.PostTariff);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            if (interception.response.body.ErrorMessage.indexOf("Tariff surcharge seller should be unique") !== -1) {
                cy.Click(BaseSelectors.Button, "Cancel");
            } else {
                throw new Error("Created Tariff Failed");
            }
        } else {
            if (interception.response.statusCode !== 200) {
                throw new Error("Created Tariff Failed");
            }
        }
        cy.wait("@" + RequestAliases.GetRecentTariffs);
    })
}

export function OpenLastCreatedTariff() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetAllTariffVersionsForTariff, RequestAliases.GetAllTariffVersionsForTariff);
    cy.Click(BaseSelectors.FirstRecentEntityItem, null);
    cy.wait("@" + RequestAliases.GetAllTariffVersionsForTariff);
}

export function OpenDraftVersionTab() {
    cy.Click(BaseSelectors.SpanElement, "[Draft]");
}

export function OpenGeneralTab(){
    cy.Click(BaseSelectors.DivElement, "General");
}

export function AddFreightCostTariffLine(freightCostType: string, freightCostTariffLineDetailsList: FreightCostTariffLineDetails[]) {
    for (let i = 0; i < freightCostTariffLineDetailsList.length; i++) {
        cy.Click(BaseSelectors.AddButton, null);
        FillTariffLinePorts(freightCostTariffLineDetailsList[i].FromPort, freightCostTariffLineDetailsList[i].ToPort);
        if (freightCostType === "Ocean FCL") {
            FillTariffLineSurchargePrice(1, freightCostTariffLineDetailsList[i].Step1Price);
            FillTariffLineSurchargePrice(2, freightCostTariffLineDetailsList[i].Step2Price);
            FillTariffLineSurchargePrice(3, freightCostTariffLineDetailsList[i].Step3Price);
        } else {
            FillTariffLineMinPrice(freightCostTariffLineDetailsList[i].MinPrice);
            FillTariffLineStepPrice(1, freightCostTariffLineDetailsList[i].Step1Price);
            FillTariffLineStepPrice(2, freightCostTariffLineDetailsList[i].Step2Price);
            FillTariffLineStepPrice(3, freightCostTariffLineDetailsList[i].Step3Price);
            FillTariffLineStepPrice(4, freightCostTariffLineDetailsList[i].Step4Price);
            FillTariffLineStepPrice(5, freightCostTariffLineDetailsList[i].Step5Price);
            FillTariffLineStepPrice(6, freightCostTariffLineDetailsList[i].Step6Price);
        }
        FillTariffLineTransitTime(freightCostTariffLineDetailsList[i].TransitTime);
        FillTariffLineNotes(freightCostTariffLineDetailsList[i].Notes);
        cy.Click(BaseSelectors.RedButton, null);
    }
}

export function EditFreightCostGeneralTab(freightCostType: string, tariffDetails: TariffDetails){
    FillTariffName(tariffDetails.Name);
    FillTariffContractNumber(tariffDetails.ContractNumber);
    FillTariffSeller(tariffDetails.Seller);
    if(freightCostType !== "Air"){
        FillCurrency(tariffDetails.Currency);
    }
    if(freightCostType === "Air"){
        FillTariffProduct(tariffDetails.Product);
    }
    FillTariffNotes(tariffDetails.Notes);
}

function OpenNewFreightCostWizard(freightCostType: string) {
    cy.Click(TariffSelectors.NewFreightCostToggleButton, null);
    cy.Click(TariffSelectors.NewFreightCostToggleMenuButton, freightCostType);
}

function FillFreightCostWizardFields(freightCostType: string, tariffDetails: TariffDetails) {
    FillTariffName(tariffDetails.Name);
    FillTariffContractNumber(tariffDetails.ContractNumber);
    FillTariffSeller(tariffDetails.Seller);
    FillCurrency(tariffDetails.Currency);
    FillTariffStartDate(tariffDetails.StartDate);
    if(freightCostType === "Air"){
        FillTariffProduct(tariffDetails.Product);
    }
    FillTariffNotes(tariffDetails.Notes);
}

function OpenNewSurchargeCostWizard(surchargeCostType: string) {
    cy.Click(TariffSelectors.NewSurchargeCostToggleButton, null);
    cy.Click(TariffSelectors.NewSurchargeCostToggleMenuButton, surchargeCostType);
}

function FillSurchargeCostWizardFields(tariffDetails: TariffDetails) {
    FillTariffName(tariffDetails.Name);
    FillTariffContractNumber(tariffDetails.ContractNumber);
    FillTariffSeller(tariffDetails.Seller);
    FillCurrency(tariffDetails.Currency);
    FillTariffNotes(tariffDetails.Notes);
}

function FillTariffName(tariffName: string){
    if(tariffName){
        cy.FillLogTextBox(TariffSelectors.TariffName, tariffName);
    }
}

function FillTariffContractNumber(contractNumber: string){
    if(contractNumber){
        cy.FillLogTextBox(TariffSelectors.TariffContractNumber, contractNumber);
    }
}

function FillTariffSeller(seller: string){
    if(seller){
        cy.FillLogLov(TariffSelectors.TariffSeller, seller, false);
    }
}

function FillCurrency(currency: string){
    if(currency){
        cy.FillLogLov(TariffSelectors.TariffCurrency, currency, true);
    }
}

function FillTariffNotes(notes: string){
    if(notes){
        cy.FillLogTextBox(TariffSelectors.TariffNotes, notes);
    }
}

function FillTariffStartDate(startDate: string){
    if(startDate){
        cy.FillDate(TariffSelectors.TariffStartDate, startDate);
    }
}

function FillTariffLineStepPrice(stepNumber: number, price: number) {
    if (price) {
        cy.FillLogTextBox(TariffSelectors.TariffLineStepPrice(stepNumber), price.toString());
    }
}

function FillTariffLineSurchargePrice(surchargeNumber: number, price: number) {
    if (price) {
        cy.FillLogTextBox(TariffSelectors.TariffLineSurchargePrice(surchargeNumber), price.toString());
    }
}

function FillTariffProduct(product: string){
    if(product){
        cy.FillLogLov(TariffSelectors.TariffProduct, product, true);
    }
}

function FillTariffLineMinPrice(minPrice: number) {
    if (minPrice) {
        cy.FillLogTextBox(TariffSelectors.TariffLineMinPrice, minPrice.toString());
    }
}

function FillTariffLinePorts(fromPort: string, toPort: string) {
    if(fromPort){
        cy.FillLogLov(TariffSelectors.TariffLineFromPort, fromPort, false);
    }
    if(toPort){
        cy.FillLogLov(TariffSelectors.TariffLineToPort, toPort, false);
    }
}

function FillTariffLineTransitTime(transitTime: string) {
    if (transitTime) {
        cy.FillLogTextBox(TariffSelectors.TariffLineTransitTime, transitTime);
    }
}

function FillTariffLineNotes(notes: string) {
    if (notes) {
        cy.FillLogTextBox(TariffSelectors.TariffLineNotes, notes);
    }
}