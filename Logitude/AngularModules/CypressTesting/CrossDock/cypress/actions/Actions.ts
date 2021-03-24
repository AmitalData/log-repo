import { URLs } from "../constants/URLs";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import {CrossdockSelectors} from "../selectors/Selectors"
import { CrossdockDetails } from "../models/CrossdockDetails";
import { CrossdockContext } from "../models/CrossdockContext";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { LinkedShipmentDetails } from "../models/LinkedShipmentDetails";


export function FillCrossdockWizardsFields(crossdockDetails : CrossdockDetails,crossDockType:string){
    cy.Click(CrossdockSelectors.CrossdockNewButton(crossDockType), null);
    cy.FillLogLov(CrossdockSelectors.CrossdockWarehouse , crossdockDetails.Warehouse,true);
    cy.FillDate(CrossdockSelectors.CrossdockExpectedEntryDate , crossdockDetails.ExpectedEntryDate);
    cy.FillLogTextBox(CrossdockSelectors.CrossdockExpectedEntryTime,crossdockDetails.ExpectedEntryTime);
}

export function CreateCrossdock(){
    DefinePostCrossdockRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCreateCrossdock(){
    AssertPostCrossdock()
}

export function UpdateCrossdock(){
    DefinePutCrossdockRequest()
    cy.Click("#WarehouseEntry-Save"+BaseSelectors.LastElement, null);
}

export function AssertUpdateCrossdock(){
    AssertPutCrossdock()
}

export function ValidateLinkedEntryInShipment(linkedEntryDetails: CrossdockDetails) {
    BaseAssertion.AssertElementContain(CrossdockSelectors.EntityNumberLink(CrossdockContext.EntryNumber),linkedEntryDetails.EntryNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossdockContext.EntryNumber),linkedEntryDetails.Status)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockDate(CrossdockContext.EntryNumber),linkedEntryDetails.EntryDate)
}

export function ValidateLinkedShipmentInEntry(linkedShipmentDetails: LinkedShipmentDetails) {
    NavigateToShipmentEntryConnectedEntities()
    BaseAssertion.AssertElementContain(CrossdockSelectors.EntityNumberLink(CrossdockContext.ShipmentNumber),linkedShipmentDetails.ShipmentNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.WarehouseCustomerCell,linkedShipmentDetails.Customer)
    BaseAssertion.AssertElementContain(CrossdockSelectors.WarehouseFromCell,linkedShipmentDetails.From)
    BaseAssertion.AssertElementContain(CrossdockSelectors.WarehouseToCell,linkedShipmentDetails.To)
    BaseAssertion.AssertElementContain(CrossdockSelectors.WarehouseStatusCell,linkedShipmentDetails.Status)
}

export function CancleEntry(){
    DefinePutCancleEntryRequest()
    cy.Click(CrossdockSelectors.WarehouseEntryCancellButton, null);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCancleEntry(){
    AssertPutCancleEntry()
}

export function ValidateEntryDisableFields() {
    cy.Click(CrossdockSelectors.WarehouseEntryGeneralTab, null);
    BaseAssertion.AssertElementHaveClasss(CrossdockSelectors.CrossdockExpectedEntryDateDiv,BaseSelectors.HaveClass, CrossdockSelectors.ContainsDatePickerDisabled)
    cy.Click(CrossdockSelectors.WarehouseEntryPackagesTab+BaseSelectors.LastElement,null);
    BaseAssertion.AssertElementDisabled(CrossdockSelectors.WarehousePackageEditButton, BaseSelectors.BeDisabled)
}

function NavigateToShipmentEntryConnectedEntities() {
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossdockContext.EntryNumber), null,true)
    cy.Navigate(CrossdockSelectors.ShipmentEntryConnectedEntities);
}

function DefinePostCrossdockRequest() {
    cy.DefineRequestWait(RestAPI.POST, URLs.WarehouseEntry, RequestAliases.PostCrossdock);
}

function AssertPostCrossdock() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostCrossdock, 200).then((interception) => {
        let responseBody = interception.response.body;
        CrossdockContext.EntryNumber = responseBody.EntryNumber;
    });
}

function DefinePutCancleEntryRequest() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.CancelWarehouseEntry, RequestAliases.PutCrossdock);
}

function AssertPutCancleEntry() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCrossdock, 200)
}

function DefinePutCrossdockRequest() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.WarehouseEntry, RequestAliases.PutCrossdock);
}

function AssertPutCrossdock() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCrossdock, 200);
}


