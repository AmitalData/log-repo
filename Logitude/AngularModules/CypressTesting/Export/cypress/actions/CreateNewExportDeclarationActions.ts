import { CreateNewExportDeclarationSelectors } from '../selectors/CreateNewExportDeclarationSelectors';
import { CreateNewExportDeclarationDetails } from "cypress/models/CreateNewExportDeclarationDetails";
import { BaseExportSelectors } from "../selectors/BaseExportSelectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'


export function NavigatesExportWizerd() {
    
    cy.Click(BaseExportSelectors.ExportDeclaration, null);
}

export function CreateNewExportDeclaration(createNewExportDeclarationDetails: CreateNewExportDeclarationDetails) {
    
    cy.Click(CreateNewExportDeclarationSelectors.NewDeclarationButton, null);
    cy.Click(CreateNewExportDeclarationSelectors.Airlabel, null);
    cy.Click(CreateNewExportDeclarationSelectors.ExportFileNum, null);
    cy.get(CreateNewExportDeclarationSelectors.ExportFileNum).type('565652');
    cy.Click(CreateNewExportDeclarationSelectors.Customer, null);
    cy.FillLogLov(CreateNewExportDeclarationSelectors.Customer, createNewExportDeclarationDetails.Customer, true)
    
}

export function Compare(createNewExportDeclarationDetails: CreateNewExportDeclarationDetails) {
    
    cy.get('.ShortTitleControl').should('contain', '565652');

}

export function FillNewExportDeclaration(createNewExportDeclarationDetails: CreateNewExportDeclarationDetails){
    debugger

   cy.FillLogLov(CreateNewExportDeclarationSelectors.DeclarationOfficeHandlCode,createNewExportDeclarationDetails.DeclarationOfficeHandlCode, true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.ExportDeclarationOfficeCode,createNewExportDeclarationDetails.ExportDeclarationOfficeCode, true);
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.ExporterNumber,createNewExportDeclarationDetails.ExporterNumber,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.DeclarationTypeCode,createNewExportDeclarationDetails.DeclarationTypeCode,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.ProcedureCurrentCode,createNewExportDeclarationDetails.ProcedureCurrentCode,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.DeclarationDocumentTypeCode,createNewExportDeclarationDetails.DeclarationDocumentTypeCode,true);
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.ClientSearch,createNewExportDeclarationDetails.ClientSearch,true);
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.DeclarationDocumentId,createNewExportDeclarationDetails.DeclarationDocumentId,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.DestinationCountryCode,createNewExportDeclarationDetails.DestinationCountryCode,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.AutonomyRegionTypeCode,createNewExportDeclarationDetails.AutonomyRegionTypeCode,true);
   cy.Click(CreateNewExportDeclarationSelectors.IsExporterConfirmation,createNewExportDeclarationDetails.IsExporterConfirmation,true)
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.RecipientName,createNewExportDeclarationDetails.RecipientName,true);
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.RecipientAddress,createNewExportDeclarationDetails.RecipientAddress,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.RecipientIssueCountryCode,createNewExportDeclarationDetails.RecipientIssueCountryCode,true);
   //cy.FillLogLov(CreateNewExportDeclarationSelectors.TransportType,createNewExportDeclarationDetails.TransportType,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.CargoType,createNewExportDeclarationDetails.CargoType,true);
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.FirstCargoID,createNewExportDeclarationDetails.FirstCargoID,true);
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.SecondCargoID,createNewExportDeclarationDetails.SecondCargoID,true);
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.ThirdCargoID,createNewExportDeclarationDetails.ThirdCargoID,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.FinalDestinationPortCode,createNewExportDeclarationDetails.FinalDestinationPortCode,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.LoadingPortCode,createNewExportDeclarationDetails.LoadingPortCode,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.UnloadingPortCode,createNewExportDeclarationDetails.UnloadingPortCode,true);
   cy.FillLogTextBox(CreateNewExportDeclarationSelectors.CargoDescription ,createNewExportDeclarationDetails.CargoDescription,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.StorageSite,createNewExportDeclarationDetails.StorageSite,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.RecieverWareHouse,createNewExportDeclarationDetails.RecieverWareHouse,true);
   cy.FillLogLov(CreateNewExportDeclarationSelectors.InternalTransition,createNewExportDeclarationDetails.InternalTransition,true);
   cy.Click(CreateNewExportDeclarationSelectors.IsDangerousGoods,createNewExportDeclarationDetails.IsDangerousGoods,true)
   cy.get(CreateNewExportDeclarationSelectors.CheckBox).click();

 }

 export function SaveDeclaretion() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Declarations, RequestAliases.DeclarationRequest)
    cy.Click(CreateNewExportDeclarationSelectors.Save,null);
    
}



export function AssertSaveDeclaretion() {
    
    BaseAssertion.AssertStatusCode(RequestAliases.DeclarationRequest, 200);
    
}



