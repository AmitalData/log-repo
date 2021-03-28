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
        cy.FillLogLov(CrossdockSelectors.CrossdockWarehouseEntry, crossdockDetails.Warehouse, true);
        cy.FillDate(CrossdockSelectors.CrossdockExpectedEntryDate, crossdockDetails.ExpectedEntryDate);
        cy.FillLogTextBox(CrossdockSelectors.CrossdockExpectedEntryTime, crossdockDetails.ExpectedEntryTime);
    }
    else {
        cy.FillLogLov(CrossdockSelectors.CrossdockWarehouseRelease, crossdockDetails.Warehouse, true);
        cy.FillDate(CrossdockSelectors.CrossdockExpectedReleaseDate, crossdockDetails.ExpectedReleaseDate);
        cy.FillLogTextBox(CrossdockSelectors.CrossdockExpectedReleaseTime, crossdockDetails.ExpectedReleaseTime);
        AddPackageToCrossDockRelease();
    }
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
    cy.Click(CrossdockSelectors.WarehouseEntrySaveButton + BaseSelectors.LastElement, null);
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
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossDockContext.EntryNumber), null, true)
    cy.Navigate(CrossdockSelectors.ShipmentEntryConnectedEntities);
}
function NavigateToShipmentConnectedReleases() {
    cy.Click(CrossdockSelectors.EntityNumberLink(CrossDockContext.ReleaseNumber), null, true)
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
    cy.get(ShipmentSelectors.ShipmentWarehouseLegExpectedEntryDate).should("have.value", entryDetails.ExpectedEntryDate);
    cy.get(ShipmentSelectors.ShipmentWarehouseLegExpectedEntryTime).should("have.value", entryDetails.ExpectedEntryTime);
    cy.get(ShipmentSelectors.ShipmentWarehouseLegActualEntryDate).should("have.value", entryDetails.ActualEntryDate);
    cy.get(ShipmentSelectors.ShipmentWarehouseLegActualEntryTime).should("have.value", entryDetails.ActualEntryTime);
    cy.Click(ShipmentSelectors.WarehouseOKBtn, null)
}
export function FillEntryDate(entryDetails: CrossDockDetails) {
    cy.FillDate(CrossdockSelectors.CrossdockActualEntryDate, entryDetails.ActualEntryDate)
    cy.FillDate(CrossdockSelectors.CrossdockActualEntryTime, entryDetails.ActualEntryTime)
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
    BaseAssertion.AssertElementContain(CrossdockSelectors.EntityNumberLink(CrossDockContext.ReleaseNumber)+BaseSelectors.LastElement, linkedReleaseDetails.ReleaseNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.ShipmentNumber(CrossDockContext.ShipmentNumber)+BaseSelectors.LastElement, linkedReleaseDetails.ShipmentNumber)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockConnectedTo(CrossDockContext.ReleaseNumber)+BaseSelectors.LastElement, linkedReleaseDetails.ConnectedTo)
    BaseAssertion.AssertElementContain(CrossdockSelectors.CrossdockStatus(CrossDockContext.ReleaseNumber)+BaseSelectors.LastElement, linkedReleaseDetails.Status)
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
    cy.get(ShipmentSelectors.ShipmentWarehouseLegExpectedReleaseDate).should("have.value", releaseDetails.ExpectedReleaseDate);
    cy.get(ShipmentSelectors.ShipmentWarehouseLegExpectedReleaseTime).should("have.value", releaseDetails.ExpectedReleaseTime);
    cy.get(ShipmentSelectors.ShipmentWarehouseLegActualReleaseDate).should("have.value", releaseDetails.ActualReleaseDate);
    cy.get(ShipmentSelectors.ShipmentWarehouseLegActualReleaseTime).should("have.value", releaseDetails.ActualReleaseTime);
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
    cy.get(ShipmentSelectors.DeliveryToPartnerName).should("have.value", deliveryDetails.ToPartner)
    cy.get(ShipmentSelectors.PickUpDeliveryETDDate).should("have.value", deliveryDetails.ETDDepartureDate)
    cy.get(ShipmentSelectors.PickUpDeliveryETDTime).should("have.value", deliveryDetails.ETDDepartureTime)
    cy.get(ShipmentSelectors.PickUpDeliveryATDDate).should("have.value", deliveryDetails.ATDDepartureDate)
    cy.get(ShipmentSelectors.PickUpDeliveryATDTime).should("have.value", deliveryDetails.ATDDepartureTime)
}
export function FillReleaseDate(releaseDetails: CrossDockDetails) {
    cy.FillDate(CrossdockSelectors.CrossdockActualReleaseDate, releaseDetails.ActualReleaseDate)
    cy.FillDate(CrossdockSelectors.CrossdockActualReleaseTime, releaseDetails.ActualReleaseTime)
}