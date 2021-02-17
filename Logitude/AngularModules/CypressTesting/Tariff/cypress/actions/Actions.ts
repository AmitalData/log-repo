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
import { TariffLine } from "cypress/models/TariffLine";


export function LoginAndNavigateToTariffWorkspace() {
    cy.Login();
    cy.Click(TariffSelectors.TariffMenu, null);
}

export function LoginAndNavigateToShippingLineWorkspace() {
    cy.Login();
    NavigateToMaintenance();
    NavigateToShippingLine();
}

export function FillNewFreightCost(freightCostType: string, tariffDetails: TariffDetails) {
    OpenNewFreightCostWizard(freightCostType);
    FillFreightCostWizardFields(freightCostType, tariffDetails);
}

export function FillNewSurchargeCost(surchargeCostType: string, tariffDetails: TariffDetails) {
    OpenNewSurchargeCostWizard(surchargeCostType);
    FillSurchargeCostWizardFields(tariffDetails);
}

export function FillNewOceanFCLSurchargesCost( surchargeCostType: string ,tariffDetails: TariffDetails){
    OpenNewSurchargeCostWizard(surchargeCostType);
    cy.FillLogTextBox(TariffSelectors.TariffName, tariffDetails.Name);
    cy.FillLogLov(TariffSelectors.TariffSeller, tariffDetails.Seller, false);
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
    cy.Click(BaseSelectors.RedButton + ":last", null);
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
    cy.Click(BaseSelectors.DivElement, "General");
}

export function OpenVersionHistoryTab() {
    cy.Click(BaseSelectors.DivElement, "Version History");
}

export function AddFreightCostTariffLines(freightCostType: string, freightCostTariffLineDetailsList: FreightCostTariffLineDetails[]) {
    FillFreightCostTariffLines(freightCostType, freightCostTariffLineDetailsList, true);
}

export function EditFreightCostTariffLines(freightCostType: string, freightCostTariffLineDetailsList: FreightCostTariffLineDetails[]) {
    FillFreightCostTariffLines(freightCostType, freightCostTariffLineDetailsList, false);
}

export function EditFreightCostGeneralTab(freightCostType: string, tariffDetails: TariffDetails) {
    FillTariffName(tariffDetails.Name);
    FillTariffContractNumber(tariffDetails.ContractNumber);
    FillTariffSeller(tariffDetails.Seller);
    if (freightCostType !== "Air") {
        FillCurrency(tariffDetails.Currency);
    }
    if (freightCostType === "Air") {
        FillTariffProduct(tariffDetails.Product);
    }
    FillTariffNotes(tariffDetails.Notes);
}

export function CreateTariff() {
    DefineRequestPostTariff();
    DefineRequestGetRecentTariffs();
    cy.Click(BaseSelectors.RedButton + ":last", null);
}

export function UpdateTariff() {
    DefineRequestPutTariff();
    cy.Click(TariffSelectors.SaveTariff, null);
}

export function CreateUpdateTariff() {
    cy.DefineRequestWait(RestAPI.POST, Urls.PostUpdateSurcharge, RequestAliases.PostUpdateRequest);
    cy.Click(BaseSelectors.GreenButton, "Update")
}

export function ApproveTariffVersion() {
    DefineRequestsForApproveOrCopyTariffVersion();
    cy.Click(BaseSelectors.GreenButton, "Approve Version");
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

export function ValidateCreateFreightCost() {
    AssertPostTariff();
    AssertGetRecentTariffs();
}

export function ValidateCreateSurchargeCost() {
    let intercept = cy.wait("@" + RequestAliases.PostTariff);
    intercept.then((interception) => {
        if (interception.response.statusCode === 400) {
            if (interception.response.body.ErrorMessage.indexOf("Tariff surcharge seller should be unique") !== -1) {
                cy.Click(BaseSelectors.Button, "Cancel");
            } else {
                throw new Error("Create Tariff Failed");
            }
        } else {
            if (interception.response.statusCode === 200) {
                AssertGetRecentTariffs();
            } else {
                throw new Error("Create Tariff Failed");
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
    if (freightCostType === "Air") {
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

function FillTariffName(tariffName: string) {
    if (tariffName) {
        cy.FillLogTextBox(TariffSelectors.TariffName, tariffName);
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

function FillTariffStartDate(startDate: string) {
    if (startDate) {
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

function DefineRequestGetTariffVersionLines() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetTariffVersionLines, RequestAliases.GetTariffVersionLines);
}

function DefineRequestGetSingleTariff() {
    cy.DefineRequestWait(RestAPI.GET, Urls.GetSingleTariff, RequestAliases.GetSingleTariff);
}

export function AssertPutTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutTariff, 200);
}

export function AssertPostUpdateTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostUpdateRequest, 200)
}

function AssertPostTariff() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostTariff, 200);
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

function NavigateToMaintenance() {
    cy.Click("#GeneralMHMaintenance", null)
    cy.Click("#MaintenanceItemMTSL", null);
}

function NavigateToShippingLine() {
    cy.DefineRequestWait(RestAPI.GET, Urls.CarrierViews, RequestAliases.GetCarrierViews)
    cy.Click(BaseSelectors.Button, "Add");
    BaseAssertion.AssertStatusCode(RequestAliases.GetCarrierViews, 200)
    cy.Click(BaseSelectors.Button, "New Shipping Line");
}

export function CreateSeller() {
    var Code = FillShippingLineCode();
    cy.FillLogTextBox(TariffSelectors.ShippingLineName, "SellerTest")
    cy.DefineRequestWait(RestAPI.POST, Urls.ShippingLine, RequestAliases.PostShippinglinesRequest)
    cy.Click(BaseSelectors.RedButton, "Ok");
    return Code;
}

function FillShippingLineCode() {
    let code: string = gr.GenerateRandomNumberAndString(4);
    cy.get(TariffSelectors.ShippingLineCode).clear().type(code);
    cy.FillLogTextBox(TariffSelectors.ShippingLineSCACCode,code)
    cy.get(BaseSelectors.Label).contains("Code:").click();
    cy.get(BaseSelectors.RedButton).then($btn => {
        if ($btn.is(":disabled")) {
            FillShippingLineCode();
        }
    });
    return code
}

export function FillTariffLine(tariffDetails : TariffLine){
    cy.Click(TariffSelectors.AddButton, null);
    cy.FillLogLov(TariffSelectors.TariffLineFromPort, tariffDetails.FromPort, false)
    cy.FillLogLov(TariffSelectors.TariffLineToPort, tariffDetails.ToPort, false)
    cy.FillDate(TariffSelectors.TariffLineStartDate, tariffDetails.StartDate);
}

export function FillUpdateSurcharges(tariffDetails:TariffLine){
    cy.Click(BaseSelectors.Button, "Update Surcharges")
    FillUpdatePorts(TariffSelectors.FromPort ,tariffDetails.FromPort);
    FillUpdatePorts(TariffSelectors.ToPort ,tariffDetails.ToPort);
    cy.FillDate(TariffSelectors.TariffUpdateStartDate, tariffDetails.StartDate);
}

function FillUpdatePorts(PortSelector : string , PortData:string){
    cy.Click(PortSelector, null)
    cy.FillLogLov(TariffSelectors.TariffUpdatePortSelector, PortData, false);
    cy.Click(BaseSelectors.Button, "Add")
    cy.Click(BaseSelectors.Button, "Close", true)
}

export function FillUpdatePrice(tariffDetails:TariffLine ){
    cy.Click(TariffSelectors.TariffUpdateSurchargeCheckBox, null);
    FillTariffUpdatePrice(1 , tariffDetails.Step1Price )
    FillTariffUpdatePrice(2 , tariffDetails.Step2Price )
    FillTariffUpdatePrice(3 , tariffDetails.Step3Price )
}

function FillTariffUpdatePrice(stepNumber: number, price: string) {
    if (price) {
        cy.FillLogTextBox(TariffSelectors.TariffUpdatePrice(stepNumber), price);
    }
}

export function UploadExcelFile(){
    cy.Click(TariffSelectors.TariffActionsMenu, "Actions");
    const fileName = 'Tariff-1168-17-02-2021.xls'
    cy.DefineRequestWait(RestAPI.POST, Urls.PostUploadExcelFile, RequestAliases.WaitUpload)
    cy.fixture(fileName,'binary')
    .then(Cypress.Blob.binaryStringToBlob)
    .then(fileContent => {
      cy.get('input.upload').attachFile({ fileContent, fileName, mimeType:'application/vnd.ms-excel',encoding:'utf8' })
    })
}
