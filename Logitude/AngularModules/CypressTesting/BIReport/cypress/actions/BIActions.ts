import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import { BIReportSelectors } from "../selectors/BIReportSelectors";
import { BIfolderDetails } from "../models/BIfolderDetails";
import { BIReportDetails } from "../models/BIReportDetails";
import * as gr from '../../../Base/cypress/actions/GenerateRandoms';




export function NavigatesBIReportWorkspace() {
    cy.Click(BaseSelectors.Report, null)
    cy.get(BIReportSelectors.BI).contains("BI").click()
}

export function FillBIFoldertDetails(BifolderDetails:BIfolderDetails) {
    cy.Click(BIReportSelectors.NewBIReportFolder, null, true);
    cy.FillLogTextBox(BIReportSelectors.BIReportFolderName, BifolderDetails.Name)
    cy.FillLogTextBox(BIReportSelectors.BIReportFolderDescription, BifolderDetails.Description);
   }

   export const SearchBIFolder = (BIReportFolderName)=> {
    cy.FillLogTextBox(BIReportSelectors.FolderSearch,BIReportFolderName);
    cy.Click(BIReportSelectors.SearchBIReportFolder, null, true);
    

   }

export function OpenBIFolder() {
 //need to open the last folder 
    cy.get(BIReportSelectors.BI).contains("BI").click()
}

export function FillBIReportDetails(BiReportDetails:BIReportDetails) {
    cy.Click(BIReportSelectors.NewButtonBIReport, null, true);
    cy.FillLogTextBox(BIReportSelectors.BIReportName, BiReportDetails.Name)
    cy.get(BIReportSelectors.BIReportFact).click({force: true});
    cy.Click(BIReportSelectors.BIReportFactType, BiReportDetails.FactTable, true)
    
   }


