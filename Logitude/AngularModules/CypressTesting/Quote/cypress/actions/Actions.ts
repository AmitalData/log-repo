import { QuoteSelectors } from "../selectors/Selectors";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { QuoteURLs } from "../constants/URLs";
import { QuickSearchDetails } from "../../../Base/cypress/models/QuickSearchDetails";
import { QuoteDetails } from "../models/QuoteDetails";
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import * as Conditions from "../../../Shipment/cypress/actions/Conditions";
import { PackagesDetails } from "../../../Shipment/cypress/models/PackagesDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { EventTypeDetails } from "../../../Base/cypress/models/EventTypeDetails";

//#region Navigate and open
export function NavigatesToSQuotesWorkspace() {
    cy.Click(QuoteSelectors.QuotesTab, null);
}

export function OpenQuote(QuoteNumber: string) {
    var quickSearchDetails = {
        Selector: QuoteSelectors.QuoteSearch,
        Parent: QuoteSelectors.QuoteSearchParent,
        ParentClass: QuoteSelectors.QuoteSearchParentClass,
        WaitURL: QuoteURLs.QuoteViews,
        Value: QuoteNumber,
        RequestAliase: RequestAliases.QuickSearchDataLoaded
    } as QuickSearchDetails;

    cy.SelectQuickSearchFirstElement(quickSearchDetails);
}
//#endregion

//#region Create Quote
export function FillQuoteFields(quoteDetails: QuoteDetails) {
    cy.Click(QuoteSelectors.NewQuote, null);
    FillMainFields(quoteDetails);
    FillCustomerType(quoteDetails.Direction)
    FillShipperAndConsignee(quoteDetails);
    FillMainCarriagePorts(quoteDetails);
}

function FillMainFields(quoteDetails: QuoteDetails) {
    FillDirection(quoteDetails.Direction);
    FillTransportMode(quoteDetails.TransportMode);
    FillShipmentType(quoteDetails.ShipmentType, quoteDetails.TransportMode);
}

function FillDirection(Direction: string) {
    let directionRadioSelector = ShipmentSelectors.DirectionRadio(Direction);
    cy.ClickRadio(directionRadioSelector);
}


function FillTransportMode(TransportMode: string) {
    let transportModeRadioSelector = ShipmentSelectors.TransportModeRadio(TransportMode);
    cy.ClickRadio(transportModeRadioSelector);
}

function FillShipmentType(ShipmentType: string, TransportMode: string) {
    if (ShipmentType) {
        let shipmentTypeRadioSelector: string;
        if (Conditions.IsGroupage(ShipmentType)) {
            shipmentTypeRadioSelector = ShipmentSelectors.GroupageShipmentTypeRadio(TransportMode);
        } else {
            shipmentTypeRadioSelector = ShipmentSelectors.ShipmentTypeRadio(ShipmentType);
        }
        cy.ClickRadio(shipmentTypeRadioSelector);
    }
}

function FillCustomerType(direction: string) {
    if (Conditions.IsImport(direction)) {
        cy.FillLogLov(QuoteSelectors.QuoteCustomerType, "Consignee", true)
    } else {
        cy.FillLogLov(QuoteSelectors.QuoteCustomerType, "Shipper", true)
    }
}

function FillShipperAndConsignee(quoteDetails: QuoteDetails) {
    if (Conditions.IsInlandDomestic(quoteDetails.Direction, quoteDetails.TransportMode)) {
        cy.FillLogLov(QuoteSelectors.QuoteShipper, quoteDetails.Shipper, false)
        cy.FillLogLov(QuoteSelectors.QuoteConsignee, quoteDetails.Consignee, false)
    } else {
        if (Conditions.IsImport(quoteDetails.Direction)) {
            cy.FillLogLov(QuoteSelectors.QuoteConsignee, quoteDetails.Consignee, false)
        } else {
            cy.FillLogLov(QuoteSelectors.QuoteShipper, quoteDetails.Shipper, false)
        }
    }
}

function FillMainCarriagePorts(quoteDetails: QuoteDetails) {
    if (!Conditions.IsInlandDomestic(quoteDetails.Direction, quoteDetails.TransportMode)) {
        cy.FillLogLov(QuoteSelectors.QuoteFromPort, quoteDetails.MainCarriageFromPort, false)
        cy.FillLogLov(QuoteSelectors.QuoteToPort, quoteDetails.MainCarriageToPort, false)
    }
}

export function CreateQuote() {
    cy.DefineRequestWait(RestAPI.POST, QuoteURLs.Quotes, RequestAliases.Quotes)
    cy.Click(QuoteSelectors.CreateQuote, null)
}
//#endregion

//#region Update Quote
export function FillPackageTab(packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(QuoteSelectors.PackagesTab, null)
    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(QuoteSelectors.AddPackage, null)
        cy.FillLogTextBox(QuoteSelectors.PackageQuantity, packagesDetails[i].Quantity.toString())

        if (Conditions.IsLCL(shipmentType) || Conditions.IsLTL(shipmentType)) {
            cy.FillLogLov(QuoteSelectors.PackageType, packagesDetails[i].PackageType, true)
        }
        if (packagesDetails[i].Volume) {
            cy.FillLogTextBox(QuoteSelectors.PackageVolume, packagesDetails[i].Volume.toString())
        } else {
            cy.FillLogTextBox(QuoteSelectors.PackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(QuoteSelectors.PackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(QuoteSelectors.PackageHeight, packagesDetails[i].Height.toString())
        }
        cy.get(QuoteSelectors.PackageWeight).type(packagesDetails[i].GrossWeight.toString());
        cy.Click(QuoteSelectors.OkAddPackage, null);
    }
}

export function FillExpectedOrderDetailsDimensions(packagesDetails: PackagesDetails[]) {
    cy.Click(BaseSelectors.HyperlinkButtonControl, BaseSelectors.ContainsFillDimensions, true)
    for (let i = 0; i < packagesDetails.length; i++) {
        cy.FillLogTextBox(QuoteSelectors.PackageLineSelector(QuoteSelectors.PackageQuantity, i), packagesDetails[i].Quantity.toString())

        if (packagesDetails[i].Volume) {
            cy.FillLogTextBox(QuoteSelectors.PackageLineSelector(QuoteSelectors.PackageVolume, i), packagesDetails[i].Volume.toString())
        } else {
            cy.FillLogTextBox(QuoteSelectors.PackageLineSelector(QuoteSelectors.PackageLength, i), packagesDetails[i].Length.toString())
            cy.FillLogTextBox(QuoteSelectors.PackageLineSelector(QuoteSelectors.PackageWidth, i), packagesDetails[i].Width.toString())
            cy.FillLogTextBox(QuoteSelectors.PackageLineSelector(QuoteSelectors.PackageHeight, i), packagesDetails[i].Height.toString())
        }
        cy.get(QuoteSelectors.PackageLineSelector(QuoteSelectors.PackageWeight, i)).type(packagesDetails[i].GrossWeight.toString());
    }
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function OpenQuoteAction(action:string, note:string){
    cy.Click(BaseSelectors.MenuButtons, null, true);
    cy.Click(QuoteSelectors.QuotationActionsButton(action), null,true);
    FillActionNote(note)
}

function FillActionNote(note:string){
    cy.FillLogTextBox(QuoteSelectors.QuoteEventNote, note)
    UpdateQuote(BaseSelectors.ConfrimApproved)
}

export function UpdateQuote(selector:string) {
    cy.DefineRequestWait(RestAPI.PUT, QuoteURLs.Quotes, RequestAliases.Quotes);
    cy.Click(selector, null);
}
//#endregion

//#region Quotation open, update, print and send
export function OpenQuotation() {
    cy.Click(QuoteSelectors.Quotation, null);
    WaitQuotationLoading();
}

export function WaitQuotationLoading() {
    cy.DefineRequestWait(RestAPI.GET, QuoteURLs.QuoteGetsingle, RequestAliases.QuoteGetsingle)
    BaseAssertion.AssertStatusCode(RequestAliases.QuoteGetsingle, 200)
}

export function PrintQuotation() {
    cy.DefineWindowOpen(RequestAliases.PrintQuotationWindowOpen);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainPrint);
}

export function AddDataFieldToQuotationIntroduction(dataField: string) {
    cy.Click(QuoteSelectors.EditQuotationIntroduction, null)
    cy.Click(QuoteSelectors.AddDataField, null)
    cy.Click(QuoteSelectors.QuotationDataFields(dataField), null, true)
    cy.Click(QuoteSelectors.QuotationEditOkButton, null)
}

export function UpdateQuotation() {
    cy.DefineRequestWait(RestAPI.PUT, QuoteURLs.QuoteTemplateSections, RequestAliases.UpdateQuotation)
    cy.Click(QuoteSelectors.SaveQuotation, null)
}

export function SendQuotationToLoggedInUser() {

    cy.GetLoggedInUser().then(email => {
        FillCustomerEmail(email);
        SentToCustomer();
    });
}

function FillCustomerEmail(email: string) {
    cy.Click(QuoteSelectors.SendOption, null)
    cy.Click(QuoteSelectors.SendToCustomer, null, true)
    cy.get(QuoteSelectors.EmailSearchTextBox).type(email + '{downarrow}{enter}')
}

function SentToCustomer() {
    cy.DefineRequestWait(RestAPI.POST, QuoteURLs.PostSendhtmlDocument, RequestAliases.SentToCustomer)
    cy.Click(QuoteSelectors.SendMessageButton, null)
}
//#endregion

//#region Copy quote
export function CopyQuote(action:string){
    cy.DefineRequestWait(RestAPI.GET, QuoteURLs.GetQuoteSettings, RequestAliases.GetQuoteSettings);
    cy.Click(BaseSelectors.MenuButtons, null, true);
    cy.Click(QuoteSelectors.QuotationActionsButton(action), null);
    BaseAssertion.AssertStatusCode(RequestAliases.GetQuoteSettings, 200);
    CreateQuote()
}

export function QuoteConversionEventsMapping(eventDetailsList: EventTypeDetails[] ,QuoteNumber :string): EventTypeDetails[]{
    for (let i = 0; i < eventDetailsList.length; i++) {
        eventDetailsList[i].Notes = eventDetailsList[i].Notes.replace(/\"OldQuoteNumber\"/gi, QuoteNumber);
    }
    return eventDetailsList;
}

export function ValidatePackageCells(expectedOrderDetails:PackagesDetails[]){
    for(let i = 0 ; i<expectedOrderDetails.length;i++){
        BaseAssertion.AssertElementContain(BaseSelectors.CellWithRowAndCol("1",i.toString()),expectedOrderDetails[i].Quantity.toString())
        BaseAssertion.AssertElementContain(BaseSelectors.CellWithRowAndCol("2",i.toString()),Dimensions_L_W_H(expectedOrderDetails[i]))
        BaseAssertion.AssertElementContain(BaseSelectors.CellWithRowAndCol("4",i.toString()),expectedOrderDetails[i].GrossWeight.toString())
    }
}

function Dimensions_L_W_H(expectedOrderDetails:PackagesDetails){
    return expectedOrderDetails.Length+"-"+expectedOrderDetails.Width+"-"+expectedOrderDetails.Height
}

//#endregion