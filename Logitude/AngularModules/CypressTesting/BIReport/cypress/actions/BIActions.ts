import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { BIReportSelectors } from "../selectors/BIReportSelectors";
import { BIfolderDetails } from "../models/BIfolderDetails";
import { BIReportDetails } from "../models/BIReportDetails";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';

const AddColumn = (columnName, columnSel, filterSel, index) => {
    console.log("index",index)
    let searchSel = `${BIReportSelectors.SearchDWQueryBuilderSearchFields}_0_${index}`;
    cy.FillLogTextBox(searchSel, columnName);
    cy.Click(columnSel, null, true)
    cy.Click(filterSel, null, true)
}

export function NavigatesBIReportWorkspace() {
    cy.Click(BaseSelectors.Report, null)
    cy.get(BIReportSelectors.BI).contains("BI").click()
}

export function FillBIFoldertDetails(BifolderDetails: BIfolderDetails) {
    cy.Click(BIReportSelectors.NewBIReportFolder, null, true);
    cy.FillLogTextBox(BIReportSelectors.BIReportFolderName, BifolderDetails.Name)
    cy.FillLogTextBox(BIReportSelectors.BIReportFolderDescription, BifolderDetails.Description);
}

export const SearchBIFolder = (BIReportFolderName) => {
    cy.wait(1000)
    cy.get('input[placeholder*="Search"]:last').type(BIReportFolderName)
    //cy.FillLogTextBox(BIReportSelectors.FolderSearch, BIReportFolderName);
    cy.Click(BIReportSelectors.SearchBIReportFolder, null, true);
}

export function FillBIReportDetails(BiReportDetails: BIReportDetails) {
    cy.Click(BIReportSelectors.NewButtonBIReport, null, true);
    cy.FillLogTextBox(BIReportSelectors.BIReportName, BiReportDetails.Name)
    cy.SelectComboDropDownListItem(BIReportSelectors.BIReportFact, BiReportDetails.FactTable, 0)
}


export function AddCoulmnAndFilter(columnName) {
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnShipmentNumber,
        BIReportSelectors.AddQBRootFilterShipmentNumber, 0)
}

export function EditCoulmnAndFilter(columnName) {
    cy.wait(10000)
    cy.Click(BIReportSelectors.EditQueryBuilder, null, true)
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnCustomer,
        BIReportSelectors.AddQBRootFilterCustomer, 1)
}

export function OpenBIFolder() {
    cy.wait(5000)
    cy.Click(BIReportSelectors.EditBackbutton, null, true);
}

export function AddCoulmnAndFiltershipmentcharge(columnName) {
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnBranch,
        BIReportSelectors.AddQBRootFilterBranch, 2)
}

export function EditCoulmnAndFiltershipmentcharge(columnName) {
    cy.wait(10000)
    cy.Click(BIReportSelectors.EditQueryBuilder, null, true)
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnShipper,
        BIReportSelectors.AddQBRootFilterShipper, 3)
}


//Master
export function FillMasterBIReportDetails(BiReportDetails: BIReportDetails) {
    cy.Click(BIReportSelectors.NewButtonBIReport, null, true);
    cy.FillLogTextBox(BIReportSelectors.BIReportName, BiReportDetails.Name)
    // cy.get(BIReportSelectors.BIReportFact).click({force: true});
    cy.SelectComboDropDownListItem(BIReportSelectors.BIReportFact, BiReportDetails.FactTable, 0)
}

export function AddCoulmnAndFilterMaster(columnName) {
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnMaster,
        BIReportSelectors.AAddQBRootFilterMaster, 4)
}

export function EditCoulmnAndFilterMaster(columnName) {
    cy.wait(10000)
    cy.Click(BIReportSelectors.EditQueryBuilder, null, true)
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnMasterShipmentNumber,
        BIReportSelectors.AddQBRootFilterMasterShipmentNumber, 5)
}

export function OpenMasterBIFolder() {
    cy.wait(5000)
    cy.Click(BIReportSelectors.MasterEditBackbutton, null, true);
}


//ARInvoices
export function FillARInvoicesBIReportDetails(BiReportDetails: BIReportDetails) {
    cy.Click(BIReportSelectors.NewButtonBIReport, null, true);
    cy.FillLogTextBox(BIReportSelectors.BIReportName, BiReportDetails.Name)
    // cy.get(BIReportSelectors.BIReportFact).click({force: true});
    cy.SelectComboDropDownListItem(BIReportSelectors.BIReportFact, BiReportDetails.FactTable, 0)
}

export function AddCoulmnAndFilterARInvoices(columnName) {
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnARInvoiceType,
        BIReportSelectors.AddQBRootFilterARInvoiceType, 6)
}

export function EditCoulmnAndFilterARInvoices(columnName) {
    cy.wait(10000)
    cy.Click(BIReportSelectors.EditQueryBuilder, null, true)
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnInvoiceBranch,
        BIReportSelectors.AddQBRootFilterInvoiceBranch, 7)
}

export function OpenARInvoicesBIFolder() {
    cy.wait(5000)
    cy.Click(BIReportSelectors.ARInvoicesEditBackbutton, null, true);
}

//Quotes
export function FillQuoteBIReportDetails(BiReportDetails: BIReportDetails) {
    cy.Click(BIReportSelectors.NewButtonBIReport, null, true);
    cy.FillLogTextBox(BIReportSelectors.BIReportName, BiReportDetails.Name)
    // cy.get(BIReportSelectors.BIReportFact).click({force: true});
    cy.SelectComboDropDownListItem(BIReportSelectors.BIReportFact, BiReportDetails.FactTable, 0)
}

export function AddCoulmnAndFilterQuote(columnName) {
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnQuoteNumber,
        BIReportSelectors.AddQBRootFilterQuoteNumber, 8)
}

export function EditCoulmnAndFilterQuote(columnName) {
    cy.wait(10000)
    cy.Click(BIReportSelectors.EditQueryBuilder, null, true)
    AddColumn(columnName, BIReportSelectors.AddQBRootColumnIsQuoteDataExternal,
        BIReportSelectors.AddQBRootFilterIsQuoteDataExternal, 9)
}

export function OpenQuoteBIFolder() {
    cy.wait(5000)
    cy.Click(BIReportSelectors.QuoteEditBackbutton, null, true);
}
