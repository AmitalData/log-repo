import { QuoteSelectors } from "../selectors/Selectors";
import { RestAPI } from "../../../Base/cypress/constants/RestAPI";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { QuoteURLs } from "../constants/URLs";
import { QuickSearchDetails } from "../../../Base/cypress/models/QuickSearchDetails";
import { QuoteDetails } from "../models/QuoteDetails";
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import * as Conditions from "../../../Shipment/cypress/actions/Conditions";
import { PackagesDetails } from "../../../Shipment/cypress/models/PackagesDetails";

export function NavigatesToSQuotesWorkspace() {
    cy.Click(QuoteSelectors.QuotesTab, null);
}

export function FillQuoteFields(quoteDetails: QuoteDetails) {
    cy.Click(QuoteSelectors.NewQuote, null);
    FillMainFields(quoteDetails);
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

export function OpenQuote(QuoteNumber: string) {
    var quickSearchDetails = {
        Selector: QuoteSelectors.QuoteSearch,
        Parent: QuoteSelectors.QuoteSearchParent,
        ParentClass: QuoteSelectors.QuoteSearchParentClass,
        WaitURL: QuoteURLs.QuoteViews,
        Value: QuoteNumber
    } as QuickSearchDetails;

    cy.SelectQuickSearchFirstElement(quickSearchDetails);
}

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

export function UpdateQuote() {
    cy.DefineRequestWait(RestAPI.PUT, QuoteURLs.Quotes, RequestAliases.Quotes);
    cy.Click(QuoteSelectors.QuoteSave, null);
}