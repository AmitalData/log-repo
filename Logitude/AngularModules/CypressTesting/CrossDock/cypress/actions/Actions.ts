import { CrossDockURLs } from "../constants/URLs";
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { CrossdockSelectors } from "../selectors/Selectors"
import { CrossDockDetails } from "../models/CrossDockDetails";
import { CrossDockContext } from "../models/CrossDockContext";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { LinkedShipmentDetails } from "../models/LinkedShipmentDetails";
import { Constants } from "../../cypress/constants/Constants"
import * as Baseactions from "../../../Base/cypress/actions/Actions"
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import { LinkedReleaseDetails } from "../models/LinkedReleaseDetails";
import { URLs } from '../../../Shipment/cypress/constants/URLs';
import { ShipmentConstants } from "../../../Shipment/cypress/constants/constants"
import { DeliveryDetails } from "../models/DeliveryDetails";

export function OpenNewCrossDock(crossDockType: string) {
    cy.Click(CrossdockSelectors.CrossdockNewButton(crossDockType), null);
}
export function FillCrossdockWizardsFields(crossdockDetails: CrossDockDetails, crossDockType: string) {
    let txt;
    if (crossDockType == Constants.Entry) {
        FillCrossdockWizardsEntryFields(crossdockDetails);
    }
    else {
        FillCrossdockWizardsReleaseFields(crossdockDetails)
    }
}
function FillCrossdockWizardsEntryFields(crossdockEntryDetails: CrossDockDetails) {
    cy.FillLogLov(CrossdockSelectors.CrossdockWarehouseEntry, crossdockEntryDetails.Warehouse, true);
    FillCrossDockDate(CrossdockSelectors.CrossdockExpectedEntryDate, crossdockEntryDetails.ExpectedEntryDate);
    cy.FillLogTextBox(CrossdockSelectors.CrossdockExpectedEntryTime, crossdockEntryDetails.ExpectedEntryTime);
}
function FillCrossdockWizardsReleaseFields(crossdockReleaseDetails: CrossDockDetails) {
    cy.FillLogLov(CrossdockSelectors.CrossdockWarehouseRelease, crossdockReleaseDetails.Warehouse, true);
    FillCrossDockDate(CrossdockSelectors.CrossdockExpectedReleaseDate, crossdockReleaseDetails.ExpectedReleaseDate);
    cy.FillLogTextBox(CrossdockSelectors.CrossdockExpectedReleaseTime, crossdockReleaseDetails.ExpectedReleaseTime);
    AddPackageToCrossDockRelease();
}
function AddPackageToCrossDockRelease() {
    cy.Click(BaseSelectors.Button, Constants.ChoosePackage)
    cy.get(CrossdockSelectors.CheckBoxPackageWarehouseEntryNumber(CrossDockContext.EntryNumber)).check({ force: true });
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);

}
export function CreateCrossdockEntry() {
    DefinePostCrossdockEntryRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePostCrossdockEntryRequest() {
    cy.DefineRequestWait(RestAPI.POST, CrossDockURLs.WarehouseEntry, RequestAliases.PostCrossdockEntry);
}
export function AssertCreateCrossdockEntry() {
    AssertPostCrossdockEntry()
}
function AssertPostCrossdockEntry() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostCrossdockEntry, 200).then((interception) => {
        let responseBody = interception.response.body;
        CrossDockContext.EntryNumber = responseBody.EntryNumber;
    });
}
export function UpdateCrossdockEntry() {
    DefinePutCrossdockEntryRequest()
    cy.Click(CrossdockSelectors.WarehouseEntrySaveButton + BaseSelectors.LastElement, null,true);
}
function DefinePutCrossdockEntryRequest() {
    cy.DefineRequestWait(RestAPI.PUT, CrossDockURLs.WarehouseEntry, RequestAliases.PutCrossdockEntry);
}
export function AssertUpdateCrossdockEntry() {
    AssertPutCrossdockEntry()
}
function AssertPutCrossdockEntry() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCrossdockEntry, 200);
}
export function ValidateLinkedEntryInShipment(linkedEntryDetails: CrossDockDetails) {
    BaseAssertion.AssertElementContain(CrossdockSelectors.EntityNumberLink(CrossDockContext.EntryNumber), linkedEntryDetails.EntryNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossDockContext.EntryNumber), linkedEntryDetails.Status)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockDate(CrossDockContext.EntryNumber), linkedEntryDetails.EntryDate)
}

export function ValidateLinkedShipmentInCrossDock(crossDockType: string, linkedShipmentDetails: LinkedShipmentDetails) {
    if (crossDockType == Constants.Entry) {
        NavigateToShipmentConnectedEntities()
    }
    else {
        NavigateToShipmentConnectedReleases()
    }
    BaseAssertion.AssertElementContain(CrossdockSelectors.EntityNumberLink(CrossDockContext.ShipmentNumber), linkedShipmentDetails.ShipmentNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.WarehouseCustomerCell, linkedShipmentDetails.Customer)
    BaseAssertion.AssertElementContain(CrossdockSelectors.WarehouseFromCell, linkedShipmentDetails.From)
    BaseAssertion.AssertElementContain(CrossdockSelectors.WarehouseToCell, linkedShipmentDetails.To)
    BaseAssertion.AssertElementContain(CrossdockSelectors.WarehouseStatusCell, linkedShipmentDetails.Status)
}

export function CancleEntry() {
    DefinePutCancleEntryRequest()
    cy.Click(CrossdockSelectors.WarehouseEntryCancellButton, null);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function AssertCancleEntry() {
    AssertPutCancleEntry()
}

export function ValidateEntryDisableFields() {
    cy.Click(CrossdockSelectors.WarehouseEntryGeneralTab, null);
    BaseAssertion.AssertElementHaveClasss(CrossdockSelectors.CrossdockExpectedEntryDateDiv, BaseSelectors.HaveClass, CrossdockSelectors.ContainsDatePickerDisabled)
    cy.Click(CrossdockSelectors.WarehouseEntryPackagesTab + BaseSelectors.LastElement, null);
    BaseAssertion.AssertElementDisabled(CrossdockSelectors.WarehousePackageEditButton, BaseSelectors.BeDisabled)
}

export function NavigateToShipmentConnectedEntities() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetMenuButtonGrouppms, RequestAliases.GetWarehouseMenuButtonGroups);
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossDockContext.EntryNumber), null, true);
    BaseAssertion.AssertStatusCode(RequestAliases.GetWarehouseMenuButtonGroups, 200);
    cy.Navigate(CrossdockSelectors.ShipmentEntryConnectedEntities);
}
function NavigateToShipmentConnectedReleases() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetMenuButtonGrouppms, RequestAliases.GetWarehouseMenuButtonGroups);
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossDockContext.ReleaseNumber), null, true)
    BaseAssertion.AssertStatusCode(RequestAliases.GetWarehouseMenuButtonGroups, 200);
    cy.Navigate(CrossdockSelectors.ShipmentEntryConnectedReleases);
}

function DefinePutCancleEntryRequest() {
    cy.DefineRequestWait(RestAPI.PUT, CrossDockURLs.CancelWarehouseEntry, RequestAliases.PutCrossdockEntry);
}

function AssertPutCancleEntry() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCrossdockEntry, 200)
}
export function ValidateRoutingsEntryFields(entryDetails: CrossDockDetails) {
    cy.Navigate(ShipmentSelectors.RoutingsTab)
    cy.Click(ShipmentSelectors.EditWarehouseLegPickups, null)
    cy.get(ShipmentSelectors.ShipmentWarehouseLegExpectedEntryDate).should("have.value", GetTodayDate());
    cy.get(ShipmentSelectors.ShipmentWarehouseLegExpectedEntryTime).should("have.value", entryDetails.ExpectedEntryTime);
    cy.get(ShipmentSelectors.ShipmentWarehouseLegActualEntryDate).should("have.value", GetTodayDate());
    cy.get(ShipmentSelectors.ShipmentWarehouseLegActualEntryTime).should("have.value", entryDetails.ActualEntryTime);
    cy.Click(ShipmentSelectors.WarehouseOKBtn, null)
}
export function FillEntryDate(entryDetails: CrossDockDetails) {
    FillCrossDockDate(CrossdockSelectors.CrossdockActualEntryDate, entryDetails.ActualEntryDate)
    FillCrossDockDate(CrossdockSelectors.CrossdockActualEntryTime, entryDetails.ActualEntryTime)
}
export function CreateCrossdockRelease() {
    DefinePostCrossdockReleaseRequest()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePostCrossdockReleaseRequest() {
    cy.DefineRequestWait(RestAPI.POST, CrossDockURLs.WarehouseReleasExtended, RequestAliases.PostCrossdockRelease);
}
export function AssertCreateCrossdockRelease() {
    AssertPostCrossdockRelease()
}
function AssertPostCrossdockRelease() {
    BaseAssertion.AssertStatusCode(RequestAliases.PostCrossdockRelease, 200).then((interception) => {
        let responseBody = interception.response.body;
        CrossDockContext.ReleaseNumber = responseBody.ReleaseNumber;
    });
}
export function ValidateLinkedReleaseInShipment(linkedReleaseyDetails: CrossDockDetails) {
    BaseAssertion.AssertElementContain(CrossdockSelectors.EntityNumberLink(CrossDockContext.ReleaseNumber), linkedReleaseyDetails.ReleaseNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossDockContext.ReleaseNumber), linkedReleaseyDetails.Status)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockDate(CrossDockContext.ReleaseNumber), linkedReleaseyDetails.ReleaseDate)
}
export function ValidateLinkedReleaseInEntry(linkedReleaseDetails: LinkedReleaseDetails) {
    NavigateToShipmentConnectedEntities()
    BaseAssertion.AssertElementContain(CrossdockSelectors.EntityNumberLink(CrossDockContext.ReleaseNumber) + BaseSelectors.LastElement, linkedReleaseDetails.ReleaseNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.ShipmentNumber(CrossDockContext.ShipmentNumber) + BaseSelectors.LastElement, linkedReleaseDetails.ShipmentNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockConnectedTo(CrossDockContext.ReleaseNumber) + BaseSelectors.LastElement, linkedReleaseDetails.ConnectedTo)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossDockContext.ReleaseNumber) + BaseSelectors.LastElement, linkedReleaseDetails.Status)
}
export function CancelRelease() {
    DefinePutCancleReleaseRequest()
    cy.Click(CrossdockSelectors.WarehouseReleaseCancellButton, null);
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
function DefinePutCancleReleaseRequest() {
    cy.DefineRequestWait(RestAPI.PUT, CrossDockURLs.CancelWarehouseRelease, RequestAliases.PutCrossdockRelease);
}
export function AssertCancleRelease() {
    AssertPutCancleRelease()
}
function AssertPutCancleRelease() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCrossdockRelease, 200)
}
export function ValidateReleaseDisableFields() {
    cy.Click(CrossdockSelectors.WarehouseReleaseGeneralTab, null);
    BaseAssertion.AssertElementHaveClasss(CrossdockSelectors.CrossdockExpectedReleaseDateDiv, BaseSelectors.HaveClass, CrossdockSelectors.ContainsDatePickerDisabled)
    BaseAssertion.AssertElementHaveClasss(CrossdockSelectors.CrossdockActualReleaseDateDiv, BaseSelectors.HaveClass, CrossdockSelectors.ContainsDatePickerDisabled)
    BaseAssertion.AssertElementHaveClasss(CrossdockSelectors.CrossdockExpectedReleaseTimeDiv, BaseSelectors.HaveClass, CrossdockSelectors.ContainsDatePickerDisabled)
    BaseAssertion.AssertElementHaveClasss(CrossdockSelectors.CrossdockActualReleaseTimeDiv, BaseSelectors.HaveClass, CrossdockSelectors.ContainsDatePickerDisabled)
}
export function UpdateCrossdockRelease() {
    DefinePutCrossdockReleaseRequest()
    cy.Click(CrossdockSelectors.WarehouseReleaseSaveButton + BaseSelectors.LastElement, null);
}
function DefinePutCrossdockReleaseRequest() {
    cy.DefineRequestWait(RestAPI.PUT, CrossDockURLs.WarehouseReleases, RequestAliases.PutCrossdockRelease);
}
export function AssertUpdateCrossdockRelease() {
    AssertPutCrossdockRelease()
}
function AssertPutCrossdockRelease() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutCrossdockRelease, 200);
}
export function ValidateRoutingsReleaseFields(releaseDetails: CrossDockDetails) {
    cy.Navigate(ShipmentSelectors.RoutingsTab)
    cy.Click(ShipmentSelectors.EditWarehouseLegPickups, null)
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.ShipmentWarehouseLegExpectedReleaseDate,GetTodayDate())
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.ShipmentWarehouseLegExpectedReleaseTime,releaseDetails.ExpectedReleaseTime)
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.ShipmentWarehouseLegActualReleaseDate,GetTodayDate())
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.ShipmentWarehouseLegActualReleaseTime,releaseDetails.ActualReleaseTime)
    cy.Click(ShipmentSelectors.WarehouseOKBtn, null)
}
export function AddDelivery() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.DefineRequestWait(RestAPI.GET, URLs.CardViews, RequestAliases.CardViewsRequest)
    cy.DefineRequestWait(RestAPI.GET, URLs.AddressViews, RequestAliases.AddressViewsRequest)
    cy.Click(CrossdockSelectors.WarehouseReleaseCreateDeliveryButton, null)
    BaseAssertion.AssertStatusCode(RequestAliases.CardViewsRequest, 200)
    BaseAssertion.AssertStatusCode(RequestAliases.AddressViewsRequest, 200)
    cy.Click(BaseSelectors.SaveCloseButton, null)
}
export function AssertAddDelivery() {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
}
export function ValidateRoutingsReleaseDeliveryLegFields(deliveryDetails: DeliveryDetails) {
    cy.Navigate(ShipmentSelectors.RoutingsTab)
    BaseAssertion.AssertElementContain(ShipmentSelectors.RoutingDeliveryLeg, CrossDockContext.ShipmentNumber)
    cy.get(ShipmentSelectors.RoutingDeliveryLeg).should("contain.text", deliveryDetails.ATDDepartureDate)
    cy.Click(ShipmentSelectors.EditDelivery, null)
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.DeliveryToPartnerName,deliveryDetails.ToPartner)
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.PickUpDeliveryETDDate, GetTodayDate())
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.PickUpDeliveryETDTime, deliveryDetails.ETDDepartureTime)
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.PickUpDeliveryATDDate, GetTodayDate())
    BaseAssertion.AssertElementHaveValue(ShipmentSelectors.PickUpDeliveryATDTime, deliveryDetails.ATDDepartureTime)
}
export function FillReleaseDate(releaseDetails: CrossDockDetails) {
    FillCrossDockDate(CrossdockSelectors.CrossdockActualReleaseDate, releaseDetails.ActualReleaseDate)
    FillCrossDockDate(CrossdockSelectors.CrossdockActualReleaseTime, releaseDetails.ActualReleaseTime)
}
export function FillCrossDockDate(selector:string,date:string){
    if(date=="Today"){
        cy.FillDate(selector , GetTodayDate())
    }
    else{
        cy.FillDate(selector , date)
    }
}
 
function GetTodayDate(){ 
    var dateString =  new Date().toLocaleDateString("en-US", { timeZone: "Asia/Jerusalem"})  
    var currentDateArray = dateString.split("/");
    if(parseInt(currentDateArray[0]) > 0 && parseInt(currentDateArray[0]) < 10){
    return "0" + currentDateArray[1] + "/" + "0" + currentDateArray[0] + "/" + currentDateArray[2];
    }
   return currentDateArray[1] + "/" + currentDateArray[0] + "/" + currentDateArray[2];
}