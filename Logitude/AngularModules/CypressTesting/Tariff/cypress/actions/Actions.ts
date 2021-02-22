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
import * as gr from '../../../Base/cypress/actions/GenerateRandoms'
import { SurchargeCostTariffLineDetails } from "cypress/models/SurchargeCostTariffLineDetails";
import { Tariff } from "../models/Tariff";
import { PriceCheckDetails } from "cypress/models/PriceCheckDetails";

export function LoginAndNavigateToTariffWorkspace() {
    cy.Login();
    cy.Click(BaseSelectors.TariffMenu, null);
}

export function FillNewFreightCost(freightCostType: string, tariffDetails: TariffDetails) {
    OpenNewFreightCostWizard(freightCostType);
    FillFreightCostWizardFields(freightCostType, tariffDetails);
}

export function FillNewSurchargeCost(surchargeCostType: string, tariffDetails: TariffDetails) {
    OpenNewSurchargeCostWizard(surchargeCostType);
    FillSurchargeCostWizardFields(tariffDetails);
}

export function FillNewSurchargeCostForUpdate(surchargeCostType: string, tariffDetails: TariffDetails) {
    OpenNewSurchargeCostWizard(surchargeCostType);
    OpenNewShippingLineWizard();
    tariffDetails.Seller = CreateNewShippingLine();
    FillSurchargeCostWizardFields(tariffDetails);
}

export function FillAllInCharges(chargeTypeDetailsList: ChargeTypeDetails[]) {
    cy.Click(TariffSelectors.EditTariffAllInChargesButton, null, true);
    cy.contains("Click the add button to add new lines");
    for (let i = 0; i < chargeTypeDetailsList.length; i++) {
        cy.Click(BaseSelectors.AddButton, null);
        if (chargeTypeDetailsList[i].Name) {
            cy.FillLogLov(TariffSelectors.TariffVersionAllInChargeType, chargeTypeDetailsList[i].Name, true);
        }
    }
    cy.Click(BaseSelectors.RedButton+TariffSelectors.Last, null);
}

export function FillSurcharges(surchargeDetailsList: SurchargeDetails[]) {
    for (let i = 0; i < surchargeDetailsList.length; i++) {
        if (i < 10) {
            if (surchargeDetailsList[i].Name) {
                cy.FillLogLov(TariffSelectors.TariffSurcharge(i + 1), surchargeDetailsList[i].Name, true);
            }
        }
    }
}

export function OpenLastCreatedTariff() {
    DefineRequestGetAllVersionsForTariff();
    cy.Click(BaseSelectors.FirstRecentEntityItem, null);
    AssertGetAllVersionsForTariff();
}

export function OpenGeneralTab() {
    cy.Click(BaseSelectors.DivElement, TariffSelectors.ContainsGeneral);
}

export function OpenVersionHistoryTab() {
    cy.Click(BaseSelectors.DivElement,TariffSelectors.ContainsVersionHistory);
}

export function OpenUpdateTab() {
    cy.Click(BaseSelectors.Button,TariffSelectors.ContainsUpdateSurcharges)
}

export function AddFreightCostTariffLines(freightCostType: string, freightCostTariffLineDetailsList: FreightCostTariffLineDetails[]) {
    FillFreightCostTariffLines(freightCostType, freightCostTariffLineDetailsList, true);
}

export function EditFreightCostTariffLines(freightCostType: string, freightCostTariffLineDetailsList: FreightCostTariffLineDetails[]) {
    FillFreightCostTariffLines(freightCostType, freightCostTariffLineDetailsList, false);
}

export function AddSurchargeCostTariffLines(surchargeTariffLineDetailsList: SurchargeCostTariffLineDetails[]) {
    FillSurchargeCostTariffLines(surchargeTariffLineDetailsList, true);
}

export function EditFreightCostGeneralTab(freightCostType: string, tariffDetails: TariffDetails) {
    FillTariffName(tariffDetails.Name);
    FillTariffContractNumber(tariffDetails.ContractNumber);
    FillTariffSeller(tariffDetails.Seller);
    if (freightCostType !== TariffSelectors.ContainsAir) {
        FillCurrency(tariffDetails.Currency);
    }
    if (freightCostType === TariffSelectors.ContainsAir) {
        FillTariffProduct(tariffDetails.Product);
    }
    FillTariffNotes(tariffDetails.Notes);
}

export function CreateTariff() {
    DefineRequestPostTariff();
    DefineRequestGetRecentTariffs();
    cy.Click(BaseSelectors.RedButton + TariffSelectors.Last, null);
}

export function UpdateTariff() {
    DefineRequestPutTariff();
    cy.Click(TariffSelectors.SaveTariff, null);
}

export function CreateUpdateTariff() {
    DefineRequestPostUpdateRequest();
    cy.Click(BaseSelectors.GreenButton,TariffSelectors.ContainsUpdate);
}

export function ApproveTariffVersion() {
    DefineRequestsForApproveOrCopyTariffVersion();
    cy.Click(BaseSelectors.GreenButton,TariffSelectors.ContainsApproveVersion);
}

export function CopyTariffVersion(newVersionStartDate: string) {
    cy.Click(TariffSelectors.TariffActionsToggleButton, null);
    cy.Click(TariffSelectors.TariffActionsToggleButtonItem, TariffSelectors.ContainsCopyIntoNewVersion);
    cy.FillDate(TariffSelectors.TariffStartDate, newVersionStartDate);
    DefineRequestsForApproveOrCopyTariffVersion();
    cy.Click(BaseSelectors.RedButton, null);
}

export function ValidateUpdateTariff() {
    AssertPutTariff();
}

export function ValidatePostUpdateTariff() {
    AssertPostUpdateTariff();
}

export function ValidateCreateFreightCost() {
    AssertPostTariff();
    AssertGetRecentTariffs();
}

export function ValidateCreateSurchargeCost() {
    let intercept = cy.wait("@" + RequestAliases.PostTariff);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            if (interception.response.body.ErrorMessage.indexOf(TariffSelectors.ContainsUniqueSellerError) !== -1) {
                cy.Click(BaseSelectors.Button,TariffSelectors.ContainsCancel);
            } else {
                throw new Error(TariffSelectors.ContainsTariffFailedError);
            }
        } else {
            if (interception.response.statusCode === 200) {
                AssertGetRecentTariffs();
            } else {
                throw new Error(TariffSelectors.ContainsTariffFailedError);
            }
        }
    })
}

export function ValidateApproveTariffVersion() {
    AssertApproveOrCopyTariffVersion();
}

export function ValidateCopyTariffVersion() {
    AssertApproveOrCopyTariffVersion();
}

export function ValidateApprovedVersionsAppear() {
    cy.Click(TariffSelectors.TariffVersionHistoryComboBox, null);
    BaseAssertion.AssertElementExist(TariffSelectors.TariffVersionHistoryComboBoxItem(1));
    BaseAssertion.AssertElementExist(TariffSelectors.TariffVersionHistoryComboBoxItem(2));
}

export function ValidateTariffPriceCheck(expectedPrice: string) {
    AssertTariffPriceCheck(expectedPrice);
}

export function ValidateUploadExcelFile() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostUploadExcelFile, 200)
}

export function BackToTariffWorkspace() {
    cy.Click(BaseSelectors.BackBottonBodyClass,TariffSelectors.ContainsTariffs);
}

export function OpenPriceCheckWizard(priceCheckType: string) {
    cy.Click(BaseSelectors.QueryLink, priceCheckType);
}

export function FillPriceCheckWizard(priceCheckType: string, priceCheck: PriceCheckDetails) {
    FillTariffLinePorts(priceCheck.FromPort, priceCheck.ToPort);
    FillTariffDate(TariffSelectors.PriceCheckDate, priceCheck.Date)
    if (priceCheckType === TariffSelectors.ContainsOceanFCL) {
        FillPriceCheckQuantity(1, priceCheck.Quantity1);
        FillPriceCheckQuantity(2, priceCheck.Quantity2);
        FillPriceCheckQuantity(3, priceCheck.Quantity3);
        FillPriceCheckQuantity(4, priceCheck.Quantity4);
        FillPriceCheckQuantity(5, priceCheck.Quantity5);
    } else {
        FillTariffChargeableWeight(priceCheck.ChargeableWeight)
    }
}

export function PriceCheckSearch() {
    DefineRequestPostAvailableTariffs();
    cy.Click(TariffSelectors.PriceCheckSearch,TariffSelectors.ContainsSearch);
}

export function FillUpdateSurcharges(tariffDetails: SurchargeCostTariffLineDetails) {
    FillUpdatePorts(TariffSelectors.FromPort, tariffDetails.FromPort);
    FillUpdatePorts(TariffSelectors.ToPort, tariffDetails.ToPort);
    FillTariffDate(TariffSelectors.TariffUpdateStartDate, tariffDetails.StartDate)
    FillUpdateSurchargesPrice(tariffDetails);
}

export function UploadExcelFile() {
    cy.Click(TariffSelectors.TariffActionsMenu,TariffSelectors.ContainsActions);
    const fileName = 'Tariff-1168-17-02-2021.xls'
    DefineRequestPostUploadExcelFile()
    cy.fixture(fileName,TariffSelectors.Binary)
        .then(Cypress.Blob.binaryStringToBlob)
        .then(fileContent => {
            cy.get(TariffSelectors.InputUpload).attachFile({ fileContent, fileName, mimeType: TariffSelectors.ExcelType, encoding: 'utf8' })
        })
}

function DefineRequestsForApproveOrCopyTariffVersion() {
    DefineRequestPutTariff();
    DefineRequestGetAllVersionsForTariff();
    DefineRequestGetTariffVersionLines();
    DefineRequestGetSingleTariff();
}

function AssertApproveOrCopyTariffVersion() {
    AssertPutTariff();
    AssertGetAllVersionsForTariff();
    AssertGetTariffVersionLines();
    AssertGetSingleTariff();
}

function FillFreightCostTariffLines(freightCostType: string, freightCostTariffLineDetailsList: FreightCostTariffLineDetails[], isNew: boolean) {
    for (let i = 0; i < freightCostTariffLineDetailsList.length; i++) {
        if (isNew) {
            cy.Click(BaseSelectors.AddButton, null);
        } else {
            cy.Click(TariffSelectors.TariffLineEditButton(i), null);
        }
        FillTariffLinePorts(freightCostTariffLineDetailsList[i].FromPort, freightCostTariffLineDetailsList[i].ToPort);
        if (freightCostType === TariffSelectors.ContainsOceanFCL) {
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

function FillSurchargeCostTariffLines(surchargeTariffLineDetailsList: SurchargeCostTariffLineDetails[], isNew: boolean) {
    for (let i = 0; i < surchargeTariffLineDetailsList.length; i++) {
        if (isNew) {
            cy.Click(BaseSelectors.AddButton, null);
        } else {
            cy.Click(TariffSelectors.TariffLineEditButton(i), null);
        }
        FillTariffLinePorts(surchargeTariffLineDetailsList[i].FromPort, surchargeTariffLineDetailsList[i].ToPort);
        FillTariffDate(TariffSelectors.TariffLineStartDate, surchargeTariffLineDetailsList[i].StartDate)
        cy.Click(BaseSelectors.RedButton, null);
    }
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
    FillTariffDate(TariffSelectors.TariffStartDate, tariffDetails.StartDate)
    if (freightCostType === TariffSelectors.ContainsAir) {
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

function FillUpdatePorts(PortSelector: string, PortData: string) {
    cy.Click(PortSelector, null)
    cy.FillLogLov(TariffSelectors.TariffUpdatePortSelector, PortData, false);
    cy.Click(BaseSelectors.Button, TariffSelectors.ContainsAdd)
    cy.Click(BaseSelectors.Button, TariffSelectors.ContainsClose, true)
}

function FillUpdateSurchargesPrice(tariffDetails: SurchargeCostTariffLineDetails) {
    cy.Click(TariffSelectors.TariffUpdateSurchargeCheckBox, null);
    FillTariffUpdatePrice(1, tariffDetails.Step1Price)
    FillTariffUpdatePrice(2, tariffDetails.Step2Price)
    FillTariffUpdatePrice(3, tariffDetails.Step3Price)
}

function FillTariffName(tariffName: string) {
    if (tariffName) {
        cy.FillLogTextBox(TariffSelectors.TariffName, tariffName);
    }
}

function FillTariffDate(dateSelector: string, date: string) {
    if (date) {
        cy.FillDate(dateSelector, date);
    }
}

function FillTariffContractNumber(contractNumber: string) {
    if (contractNumber) {
        cy.FillLogTextBox(TariffSelectors.TariffContractNumber, contractNumber);
    }
}

function FillTariffSeller(seller: string) {
    if (seller) {
        cy.FillLogLov(TariffSelectors.TariffSeller, seller, false);
    }
}

function FillCurrency(currency: string) {
    if (currency) {
        cy.FillLogLov(TariffSelectors.TariffCurrency, currency, true);
    }
}

function FillTariffNotes(notes: string) {
    if (notes) {
        cy.FillLogTextBox(TariffSelectors.TariffNotes, notes);
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

function FillTariffProduct(product: string) {
    if (product) {
        cy.FillLogLov(TariffSelectors.TariffProduct, product, true);
    }
}

function FillTariffLineMinPrice(minPrice: number) {
    if (minPrice) {
        cy.FillLogTextBox(TariffSelectors.TariffLineMinPrice, minPrice.toString());
    }
}

function FillTariffLinePorts(fromPort: string, toPort: string) {
    if (fromPort) {
        cy.FillLogLov(TariffSelectors.TariffLineFromPort, fromPort, false);
    }
    if (toPort) {
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

function FillPriceCheckQuantity(quantityNumber: number, quantity: number) {
    if (quantity) {
        cy.FillLogTextBox(TariffSelectors.PriceCheckQuantity(quantityNumber), quantity.toString());
    }
}

function FillSellerName(sellerName: string) {
    if (sellerName) {
        cy.FillLogTextBox(TariffSelectors.ShippingLineName, sellerName)
    }
}

function FillTariffUpdatePrice(stepNumber: number, price: string) {
    if (price) {
        cy.FillLogTextBox(TariffSelectors.TariffUpdatePrice(stepNumber), price);
    }
}

function FillTariffChargeableWeight(ChargeableWeight: string) {
    if (ChargeableWeight) {
        cy.FillLogTextBox(TariffSelectors.TariffChargeableWeight, ChargeableWeight)
    }
}

function DefineRequestGetAllVersionsForTariff() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetAllTariffVersionsForTariff, RequestAliases.GetAllTariffVersionsForTariff);
}

function DefineRequestPostTariff() {
    cy.DefineRequestWait(RestAPI.POST, Urls.Tariffs, RequestAliases.PostTariff);
}

function DefineRequestGetRecentTariffs() {
    cy.DefineRequestWait(RestAPI.GET, Urls.TariffDomainGetRecentTariffs, RequestAliases.GetRecentTariffs);
}

function DefineRequestPutTariff() {
    cy.DefineRequestWait(RestAPI.PUT, Urls.Tariffs, RequestAliases.PutTariff);
}

function DefineRequestPostUpdateRequest() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PostUpdateSurcharge, RequestAliases.PostUpdateRequest);
}

function DefineRequestGetTariffVersionLines() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetTariffVersionLines, RequestAliases.GetTariffVersionLines);
}

function DefineRequestGetSingleTariff() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetSingleTariff, RequestAliases.GetSingleTariff);
}

function DefineRequestPostAvailableTariffs() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PostAvailableTariffs, RequestAliases.PostAvailableTariffs);
}

function DefineRequestPostShippinglines() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PostShippingLine, RequestAliases.PostShippingline)
}

function DefineRequestPostUploadExcelFile() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PostUploadExcelFile, RequestAliases.PostUploadExcelFile)
}

function AssertPutTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTariff, 200);
}

function AssertPostUpdateTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostUpdateRequest, 200)
}

function AssertPostTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTariff, 200).then((interception) => {
        Tariff.Number = interception.response.body.TariffNumber;
    });
}

function AssertGetRecentTariffs() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetRecentTariffs, 200);
}

function AssertGetAllVersionsForTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetAllTariffVersionsForTariff, 200);
}

function AssertGetTariffVersionLines() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetTariffVersionLines, 200);
}

function AssertGetSingleTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetSingleTariff, 200);
}

function AssertTariffPriceCheck(expectedPrice: string) {
    BaseAssertion.AssertStatusCode(RequestAliases.PostAvailableTariffs, 200).then((interception) => {
        let actualPrice = interception.response.body.filter((t: { TariffNumber: string; }) => t.TariffNumber === Tariff.Number)[0].Price;
        assert.equal(actualPrice, expectedPrice);
        let indexOfTariff = interception.response.body.map(function (t: { TariffNumber: string; }) { return t.TariffNumber; }).indexOf(Tariff.Number);
        cy.get(TariffSelectors.PriceCheckResultTableRow).eq(indexOfTariff).find(BaseSelectors.DownArrowImage).click();
        cy.get(TariffSelectors.PriceCheckResultTableRow).eq(indexOfTariff).find(TariffSelectors.PriceCheckFreightResult).then((priceCell) => {
            assert.equal(priceCell.text().trim(), expectedPrice);
        });
        DefineRequestGetSingleTariff();
        cy.get(TariffSelectors.PriceCheckResultTableRow).eq(indexOfTariff).find(BaseSelectors.Hyperlink).contains(TariffSelectors.ContainsViewTariff).click();
        AssertGetSingleTariff();
        cy.get(TariffSelectors.TariffNumberShortTitleDiv).then((tariffNumberDiv) => {
            assert.equal(tariffNumberDiv.text().replace(":", "").trim(), Tariff.Number);
        });
    });
}

function OpenNewShippingLineWizard() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetCarrierViews, RequestAliases.GetCarrierViews);
    cy.DefineRequestWait(RestAPI.GET, Urls.GetEntityResource, RequestAliases.GetEntityResource);
    cy.get(TariffSelectors.TariffSeller).type(TariffSelectors.DownArrow);
    AssertEntityResource();
    cy.get(BaseSelectors.LogLOVFooterHyperLink).eq(0).click();
    AssertGetCarrierViews();
    cy.Click(BaseSelectors.Button,TariffSelectors.ContainsNewShippingLine);
}

function CreateNewShippingLine() {
    var Code = FillShippingLineCode();
    FillSellerName("SellerTest");
    DefineRequestPostShippinglines();
    cy.Click(BaseSelectors.RedButton +TariffSelectors.Last, null);
    ValidateShippingLine();
    return Code;
}

function FillShippingLineCode() {
    let code: string = gr.GenerateRandomNumberAndString(4);
    cy.get(TariffSelectors.ShippingLineCode).clear().type(code);
    cy.FillLogTextBox(TariffSelectors.ShippingLineSCACCode, code)
    cy.get(BaseSelectors.Label).contains(TariffSelectors.ContainsCode).click();
    cy.get(BaseSelectors.RedButton).then($btn => {
        if ($btn.is(TariffSelectors.Disabled)) {
            FillShippingLineCode();
        }
    })
    return code
}


function AssertEntityResource() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetEntityResource, 200);
}

function AssertGetCarrierViews() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetCarrierViews, 200);
}

function ValidateShippingLine() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostShippingline, 200)
}