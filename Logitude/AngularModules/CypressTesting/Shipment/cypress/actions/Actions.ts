import * as gr from "../../../Base/cypress/Actions/GenerateRandoms"
import { Selectors } from "../selectors/Selectors"
import { ShipmentDetails } from "../models/ShipmentDetails";
import { PartnersDetails } from "cypress/models/PartnersDetails";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { PayableDetails } from "cypress/models/PayableDetails";
import { ReceivableDetails } from "cypress/models/ReceivableDetails"
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails"
import { ARInvoiceDetails } from "cypress/models/ARInvoiceDetails"
import * as BaseAssertion from "../../../Base/cypress/actions/Assertion";
import { ShipmentMapping } from "../mapping/ShipmentMapping"
import { PackagesDetails } from "cypress/models/PackagesDetails";

export function OpenNewShipmentWizard(shipmentLevel: string) {
    cy.Click(Selectors.NewShipmentToggleButton, null)
    cy.Click(Selectors.NewShipmentToggleButtonItem, shipmentLevel)
}

export function FillShipmentWizardsFields(shipmentDetails: ShipmentDetails) {
    if (!IsMaster(shipmentDetails.ShipmentLevel)) {
        FillDirectAndHouseFields(shipmentDetails);
    } else {
        FillMasterFields(shipmentDetails);
    }
}

export function CreateShipment(shipmentLevel: string) {
    let createSelector = IsMaster(shipmentLevel) ? Selectors.CreateMasterShipmentButton : Selectors.CreateShipmentButton;
    cy.DefineRequestWait("POST", "**/shipment", "WaitPostShipmentRequest")
    cy.Click(createSelector, null)
}

export function UpdateShipment(saveButtonSelector: string, saveButtonSelectorContains?: string ) {
    cy.DefineRequestWait("PUT", "**/shipment", "WaitPutShipmentRequest")
    cy.Click(saveButtonSelector, saveButtonSelectorContains)
}

export function OpenShipment(ShipmentNumber: string) {
    cy.SelectQuickSearchFirstElement(Selectors.ShipmentSearchBar, ShipmentNumber)
}

export function CancelShipment() {
    cy.Click(Selectors.ShipmentMoreList, null);
    cy.Click(Selectors.CancelShipmentButton, null);
    UpdateShipment(Selectors.ConfirmActionButton);
}

export function ConnectOrDisconnectShipment() {
    UpdateShipment(BaseSelectors.RedButton, "Yes");
}

export function FillGeneralTab(GrossWeight: string, MoveType: string) {
    cy.Click(Selectors.GeneralTab, null)
    cy.FillLogTextBox(Selectors.ShipmentGrossWeight, GrossWeight)
    cy.SelectLogLovElement(Selectors.ShipmentMoveType, true, MoveType)
}

export function FillOrdersTab(packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(Selectors.OrdersTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(Selectors.OrdersAddPackage, null)

        cy.FillLogTextBox(Selectors.OrderPackageQuantity, packagesDetails[i].Quantity.toString())
        if (hasPacakageType(shipmentType)) {
            cy.SelectLogLovElement(Selectors.OrderPackageType, true, packagesDetails[i].PackageType)
        }
        if (!IsFCL(shipmentType) && !IsFTL(shipmentType)) {
            cy.FillLogTextBox(Selectors.OrderPackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(Selectors.OrderPackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(Selectors.OrderPackageHeight, packagesDetails[i].Height.toString())
        }
        cy.FillLogTextBox(Selectors.OrderPackageGrossWeight, packagesDetails[i].GrossWeight.toString())
        cy.Click(Selectors.OrderOKButton, null)
    }

}
export function FillPartnersTab(direction: string, transportMode: string, partnersDetails: PartnersDetails) {
    cy.Click(Selectors.PartnersTab, null)

    if (IsImport(direction)) {
        AddPartner(Selectors.AddShipperButton, Selectors.ShipmentShipper, partnersDetails.Shipper)
    }
    if (IsExport(direction) || IsDrop(direction) || (IsDomestic(direction) && !IsInland(transportMode))) {
        AddPartner(Selectors.AddConsigneeButton, Selectors.ShipmentConsignee, partnersDetails.Consignee)
    }
    AddPartner(Selectors.AddAgentButton, Selectors.ShipmentAgent, partnersDetails.Agent)
    if ((IsImport(direction) && IsAir(transportMode)) || (IsDomestic(direction) && IsAir(transportMode))) {
        AddPartner(Selectors.AddIssuingCarrierAgentButton, Selectors.ShipmentIssuingCarrierAgent)
    }
    AddPartner(Selectors.AddCustomsAgentExportButton, Selectors.ShipmentCustomAgentExport, partnersDetails.CustomsAgentExport)
    AddPartner(Selectors.AddCustomsAgentImportButton, Selectors.ShipmentCustomAgentImport, partnersDetails.CustomsAgentImport)
    AddPartner(Selectors.AddNotify1Button, Selectors.ShipmentNotify1, partnersDetails.Notify1)
    AddPartner(Selectors.AddNotify2Button, Selectors.ShipmentNotify2, partnersDetails.Notify2)
    AddPartner(Selectors.AddShipperNotExporterButton, Selectors.ShipmentShipperNotExporter, partnersDetails.ShipperNotExporter)
    AddPartner(Selectors.AddConsigneeNotImporterButton, Selectors.ShipmentConsigneeNotImporter, partnersDetails.ConsigneeNotImporter)
    AddPartner(Selectors.AddFreightForwarderButton, Selectors.ShipmentFreightForwarder, partnersDetails.FreightForwarder)
    AddPartner(Selectors.AddColoaderButton, Selectors.ShipmentColoader, partnersDetails.Coloader)
    AddPartner(Selectors.AddCustomClearancePointButton, Selectors.ShipmentCustomClearancePoint, partnersDetails.CustomClearancePoint)
    AddPartner(Selectors.AddConsolidatorButton, Selectors.ShipmentConsolidator, partnersDetails.Consolidator)
    AddPartner(Selectors.AddReleasingAgentButton, Selectors.ShipmentReleasingAgent, partnersDetails.ReleasingAgent)
}

export function FillPackageTab(transportMode: string, packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(Selectors.PackagesTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(Selectors.AddPackage, null)
        if (hasPacakageType(shipmentType)) {
            cy.SelectLogLovElement(Selectors.PackageType, true, packagesDetails[i].PackageType)
        }
        if (!IsFCL(shipmentType) && !IsFTL(shipmentType)) {
            cy.FillLogTextBox(Selectors.PackageQuantity, packagesDetails[i].Quantity.toString())
            cy.FillLogTextBox(Selectors.PackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(Selectors.PackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(Selectors.PackageHeight, packagesDetails[i].Height.toString())
        }
        cy.get(Selectors.PackageWeight).type(packagesDetails[i].GrossWeight.toString());

        if (IsAir(transportMode)) {
            cy.Click(Selectors.AirPackageOKButton, null)
        } else {
            cy.Click(Selectors.OceanPackageOKButton, null)
        }
    }
}
export function FillHouseInShipmentsTab(Shipper: string) {
    cy.SelectLogLovElement(Selectors.ShipmentCustomer, true, Shipper)
}

export function FillReceivablesTab(receivableDetails: ReceivableDetails) {
    cy.Click(Selectors.ReceivablesTab, null)
    cy.Click(Selectors.AddNewReceivableLine, null)
    cy.SelectLogLovElement(Selectors.ReceivableChargesType, true, receivableDetails.ChargesType)
    cy.SelectLogLovElement(Selectors.ReceivableMeasurement, true, receivableDetails.UOM)
    cy.get(Selectors.ReceivableQuantity).type(receivableDetails.Quantity.toString());
    cy.get(Selectors.ReceivableUnitPrice).type(receivableDetails.UnitPrice.toString());
    cy.SelectLogLovElement(Selectors.ReceivableCurrency, true, receivableDetails.Currency)
    cy.get(Selectors.ShipmentReceivableRate).clear().type(receivableDetails.ExchangeRate.toString());
    cy.Click(Selectors.AddReceivableOkButton, null)
}

export function FillPickupRouting() {
    cy.Click(Selectors.RoutingsTab, null)
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait("GET", "**/cardviews/**", "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", "**/addressviews/**", "WaitAddressViewsRequest")
    cy.Click(Selectors.PickUp, null)
    BaseAssertion.AssertStatusCode("WaitCardViewsRequest", 200)
    BaseAssertion.AssertStatusCode("WaitAddressViewsRequest", 200)
    cy.Click(Selectors.SaveClose, null)
}

export function EditMainCarriageLegs(Airline: string) {
    cy.Click(Selectors.EditRoutingMainCarriage, null)
    cy.SelectLogLovElement(Selectors.ShipmentMainCarriageCarrierId, false, Airline)
    cy.FillRandomNumber(Selectors.ShipmentFlightNumber, 100, 999)
    cy.FillRandomNumber(Selectors.ShipmentMAWB, 10000000, 99999999)
    cy.Click(Selectors.ShipmentDateMaincarriageATD, null)
    cy.Click(BaseSelectors.Button, "Today")
    cy.Click(Selectors.MainCarriageOKBtn, null);
    cy.Click(Selectors.ShipmentSaveButton, null);
}

export function UpdateClosedShipment() {
    cy.DefineRequestWait("PUT", "**/shipment", "WaitPutShipmentRequest")
    cy.Click(BaseSelectors.RedButton, "Confirm");
}

export function FillDeliveryRouting(partner: string) {
    cy.Click(Selectors.RoutingsTab, null)
    cy.Click(Selectors.RoutingToggle, null)
    cy.DefineRequestWait("GET", "**/cardviews/**", "WaitCardViewsRequest")
    cy.DefineRequestWait("GET", "**/addressviews/**", "WaitAddressViewsRequest")
    cy.Click(Selectors.Delivery, null)
    BaseAssertion.AssertStatusCode("WaitCardViewsRequest", 200)
    BaseAssertion.AssertStatusCode("WaitAddressViewsRequest", 200)
    cy.get(Selectors.ShipmentPickUpDeliveryToPartnerCard).then((input) => {
        if (input.text() === "" || input.text() === null) {
            cy.SelectLogLovElement(Selectors.ShipmentPickUpDeliveryToPartnerCard, false, partner)
        }
    })
    cy.Click(Selectors.SaveClose, null)

}

export function FillPreCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(Selectors.RoutingsTab, null)
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.PreCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(Selectors.PreCarriage, null)
            cy.FillLogLov(Selectors.ShipmentPreCarriageTransportMode, transportMode, true)
            cy.SelectLogLovElement(Selectors.ShipmentPreCarriageFromPort, false, fromPort)
            cy.SelectLogLovElement(Selectors.ShipmentPreCarriageToPort, false, toPort)
            cy.Click(Selectors.PreCarriageOKBtn, null)
        } else {
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillOnCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(Selectors.RoutingsTab, null)
    cy.Click(Selectors.RoutingToggle, null)
    cy.get(Selectors.OnCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(Selectors.OnCarriage, null)
            cy.FillLogLov(Selectors.ShipmentOnCarriageTransportMode, transportMode, true)
            cy.SelectLogLovElement(Selectors.ShipmentOnCarriageFromPort, false, fromPort)
            cy.SelectLogLovElement(Selectors.ShipmentOnCarriageToPort, false, toPort)
            cy.Click(Selectors.OnCarriageOKBtn, null)
        } else {
            cy.Click(Selectors.RoutingsTab, null)
        }
    })
}

export function FillPayablesTab(payableDetails: PayableDetails) {
    cy.Click(Selectors.PayablesTab, null)
    cy.Click(Selectors.AddNewPayableLine, null)
    cy.SelectLogLovElement(Selectors.ShipmentPayableChargesType, true, payableDetails.ChargesType);
    cy.SelectLogLovElement(Selectors.ShipmentPayableMeasurement, true, payableDetails.UOM)
    if (payableDetails.Quantity) {
        cy.FillLogTextBox(Selectors.ShipmentPayableQuantity,payableDetails.Quantity.toString());
    }
    if (payableDetails.UnitPrice) {
        cy.FillLogTextBox(Selectors.ShipmentPayableUnitPrice,payableDetails.UnitPrice.toString());
    }
    cy.SelectLogLovElement(Selectors.ShipmentPayableCurrency, true, payableDetails.Currency)
    cy.Click(Selectors.AddPayableOkButton, null)
}

export function CreateAnewShipment(shipmentLevel: string) {
    var shipmentDetails = {
        ShipmentLevel: shipmentLevel,
        Direction: "Export",
        TransportMode: "Air",
        Shipper: "Shipper1",
        MainCarriageToPort: "LHR",
        MainCarriageFromPort: "MIA"
    } as ShipmentDetails;

    OpenNewShipmentWizard(shipmentDetails.ShipmentLevel);
    FillShipmentWizardsFields(shipmentDetails);
    CreateShipment(shipmentDetails.ShipmentLevel)
}

export function CopyShipment(shipmentLevel: string) {
    cy.Click(Selectors.ShipmentMoreList, null)
    cy.Click(Selectors.CopyShipmentButton, null)
    CreateShipment(shipmentLevel);
}

export function FillAPInvoiceDetails(aPInvoiceDetails: APInvoiceDetails) {
    var generatedInvoiceNumber = "AP" + gr.GenerateRandomNumber(10000, 99999).toString();
    cy.SelectLogLovElement(Selectors.APInvoiceVendor, false, aPInvoiceDetails.Vendor);
    cy.FillLogTextBox(Selectors.APInvoiceInvoiceNumber,generatedInvoiceNumber)
    cy.FillLogTextBox(Selectors.APInvoiceAmountInInvoice,aPInvoiceDetails.InvoiceAmount.toString());
    cy.SelectLogLovElement(Selectors.APInvoiceInvoiceCurrency, true, aPInvoiceDetails.InvoiceCurrency);
    cy.FillLogTextBox(Selectors.APInvoiceInvoiceExchangeRate,aPInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(Selectors.APInvoiceInvoiceDate, aPInvoiceDetails.InvoiceDate)
    cy.SelectLogLovElement(Selectors.APInvoicePaymentTerm, true, aPInvoiceDetails.PaymentTerms);
    cy.FillDate(Selectors.APInvoiceDueDate, aPInvoiceDetails.DueDate)
    cy.Click(Selectors.OkCreateAPInvoiceButton, null);
    cy.Click(Selectors.APInvoiceLineCheckBox, null)
    cy.SelectLogLovElement(Selectors.APInvoiceVatType, true, aPInvoiceDetails.VATType)
    cy.Click(Selectors.VatTypeApplyToAll, null)
}

export function ReceiveAPInvoice() {
    cy.DefineRequestWait("POST", "**/apinvoices", "WaitPostAPInvoicesRequest")
    cy.Click(Selectors.APInvoiceSaveButton, null)
}

export function APApproveInvoice() {
    cy.DefineRequestWait("PUT", "**/apinvoices", "WaitPutAPInvoicesRequest")
    cy.Click(Selectors.APInvoiceApproveButton, null)
}

export function APInvoiceCancelApproval() {
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.DefineRequestWait("PUT", "**/apinvoices", "WaitPutAPInvoicesRequest")
    cy.Click(Selectors.APInvoiceCancelApprovalButton, null)
}

export function VoidAPInvoice() {
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.Click(Selectors.APInvoiceVoidButton, null)
    cy.DefineRequestWait("PUT", "**/apinvoices", "WaitPutAPInvoicesRequest")
    cy.Click(Selectors.ConfirmWindowYes, null);
}

export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    var generatedInvoiceNumber = "AR" + gr.GenerateRandomNumber(10000, 99999).toString();
    cy.get(".ComboBox").click();
    cy.get(".FillParent").find(".TextTrimming").contains("Customer").click()
    cy.SelectLogLovElement(Selectors.ARInvoiceInvoiceCurrency, true, aRInvoiceDetails.InvoiceCurrency)
    cy.get(Selectors.ARInvoiceExchangeRate).clear().type(aRInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(Selectors.ARInvoiceInvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.SelectLogLovElement(Selectors.ARInvoicePaymentTerm, true, aRInvoiceDetails.PaymentTerms)
    cy.FillDate(Selectors.ARInvoiceDueDate, aRInvoiceDetails.DueDate)
    cy.get(Selectors.ARInvoiceVatNumber).clear().type(aRInvoiceDetails.VATNo)
    cy.SelectLogLovElement(Selectors.ARInvoiceBranch, true, aRInvoiceDetails.Branch)
    cy.Click(Selectors.OkCreateARInvoiceButton, null);
    cy.SelectLogLovElement(Selectors.ARInvoiceVatType, true, aRInvoiceDetails.VATType)
    cy.Click(Selectors.VatTypeApplyToAll, null)
}

export function CreateARInvoice() {
    cy.DefineRequestWait("POST", "**/arinvoices", "WaitPostARInvoicesRequest")
    cy.Click(Selectors.ARInvoiceSaveButton, null)
}

export function ARApproveInvoice() {
    cy.DefineRequestWait("PUT", "**/arinvoices", "WaitPutARInvoicesRequest")
    cy.Click(Selectors.ARInvoiceApproveButton, null)
}

export function SetAsSentARInvoice() {
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.Click(Selectors.ARInvoiceSetAsSentButton, null)
    cy.DefineRequestWait("PUT", "**/arinvoices", "WaitPutARInvoicesRequest")
    cy.Click("button", "Confirm");
}

export function VoidARInvoice() {
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.Click(Selectors.ARInvoiceVoidButton, null)
    cy.DefineRequestWait("PUT", "**/arinvoices", "WaitPutARInvoicesRequest")
    cy.Click(Selectors.ConfirmWindowYes, null);
}

export function CancelDraftARInvoice() {
    cy.Click(Selectors.InvoiceMoreList, null)
    cy.DefineRequestWait("PUT", "**/arinvoices", "WaitPutARInvoicesRequest")
    cy.Click(Selectors.ARInvoiceCancelDraftButton, null)
    cy.Click(Selectors.ConfirmWindowYes, null)
}


function AddPartner(partnerTypeId: string, partnerFieldId: string, partner?: string) {
    cy.Click("label", "Add Partners")

    cy.get(partnerTypeId).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(partnerTypeId, null)
            let partnerFieldSelector = "addeditpartnercomponent input[id^='" + partnerFieldId.replace("#", "") + "']"
            if (partner) {
                cy.SelectLogLovElement(partnerFieldSelector, false, partner)
            }
            else {
                cy.SelectLogLovFirstElement(partnerFieldSelector, false)
            }
            cy.Click(Selectors.PartnerOKButton, null)
        }
    })
}


function FillDirectAndHouseFields(shipmentDetails: ShipmentDetails) {
    FillMainFields(shipmentDetails);
    FillShipperAndConsignee(shipmentDetails);
    FillMainCarriagePorts(shipmentDetails);
}

function FillMasterFields(shipmentDetails: ShipmentDetails) {
    FillMainFields(shipmentDetails);
    FillMasterAgent(shipmentDetails);
    FillMainCarriagePorts(shipmentDetails);
}

function FillMainFields(shipmentDetails: ShipmentDetails) {
    FillDirection(shipmentDetails);
    FillTransportMode(shipmentDetails);
    FillShipmentType(shipmentDetails);
}

function FillDirection(shipmentDetails: ShipmentDetails) {
    let directionRadioSelector = "input[id^='DirectionRadio_'][id$='" + ShipmentMapping.GetDirectionCode(shipmentDetails.Direction) + "']";
    cy.ClickRadio(directionRadioSelector);
    //cy.ClickRadio("#DirectionRadio_0" + ShipmentMapping.GetDirectionCode(shipmentDetails.Direction))
}

function FillTransportMode(shipmentDetails: ShipmentDetails) {
    let transportModeRadioSelector = "input[id^='TransportModeRadio_'][id$='" + ShipmentMapping.GetTransportModeCode(shipmentDetails.TransportMode) + "']";
    cy.ClickRadio(transportModeRadioSelector);
    //cy.ClickRadio("#TransportModeRadio_0" + ShipmentMapping.GetTransportModeCode(shipmentDetails.TransportMode))
}

function FillShipmentType(shipmentDetails: ShipmentDetails) {
    if (shipmentDetails.ShipmentType) {
        if (IsGroupage(shipmentDetails.ShipmentType)) {
            let shipmentTypeRadioSelector = "input[id^='ShipmentTypeRadio_'][id$='MyG" + ShipmentMapping.GetTransportModeCode(shipmentDetails.TransportMode) + "']";
            cy.ClickRadio(shipmentTypeRadioSelector);
            //cy.ClickRadio("#ShipmentTypeRadio_0MyG" + ShipmentMapping.GetTransportModeCode(shipmentDetails.TransportMode));
        } else {
            let shipmentTypeRadioSelector = "input[id^='ShipmentTypeRadio_'][id$='" + shipmentDetails.ShipmentType + "']";
            cy.ClickRadio(shipmentTypeRadioSelector);
            //cy.ClickRadio("#ShipmentTypeRadio_0" + shipmentDetails.ShipmentType)
        }
    }
}

function FillShipperAndConsignee(shipmentDetails: ShipmentDetails) {
    if (IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        cy.FillLogLov(Selectors.ShipmentShipper, shipmentDetails.Shipper, false)
        cy.FillLogLov(Selectors.ShipmentConsignee, shipmentDetails.Consignee, false)
    } else {
        if (IsImport(shipmentDetails.Direction)) {
            cy.FillLogLov(Selectors.ShipmentConsignee, shipmentDetails.Consignee, false)
        } else {
            cy.FillLogLov(Selectors.ShipmentShipper, shipmentDetails.Shipper, false)
        }
    }
}

function FillMainCarriagePorts(shipmentDetails: ShipmentDetails) {
    if (!IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        let fromPortSelector = IsMaster(shipmentDetails.ShipmentLevel) ? Selectors.MasterMainCarriageFromPort : Selectors.ShipmentMainCarriageFromPort;
        let toPortSelector = IsMaster(shipmentDetails.ShipmentLevel) ? Selectors.MasterMainCarriageToPort : Selectors.ShipmentMainCarriageToPort;
        cy.FillLogLov(fromPortSelector, shipmentDetails.MainCarriageFromPort, false)
        cy.FillLogLov(toPortSelector, shipmentDetails.MainCarriageToPort, false)
    }
}

function FillMasterAgent(shipmentDetails: ShipmentDetails) {
    cy.FillLogLov(Selectors.MasterAgent, shipmentDetails.Agent, false)
}

function IsImport(direction: string) {
    return (direction === "Import" || direction === "I");
}

function IsExport(direction: string) {
    return (direction === "Export" || direction === "E");
}

function IsDrop(direction: string) {
    return (direction === "Drop" || direction === "R");
}

function IsDomestic(direction: string) {
    return (direction === "Domestic" || direction === "D");
}

function IsAir(transportMode: string) {
    return (transportMode === "Air" || transportMode === "A");
}

function IsInland(transportMode: string) {
    return (transportMode === "Inland" || transportMode === "I");
}

function IsOcean(transportMode: string) {
    return (transportMode === "Ocean" || transportMode === "O");
}

function IsMaster(shipmentLevel: string) {
    return (shipmentLevel === "Master" || shipmentLevel === "C");
}

function IsInlandDomestic(direction: string, transportMode: string) {
    return (IsDomestic(direction) && IsInland(transportMode));
}

function IsFCL(shipmentType: string) {
    return shipmentType === "FCLD";
}

function IsFTL(shipmentType: string) {
    return shipmentType === "FTL";
}

function IsLCL(shipmentType: string) {
    return shipmentType === "LCLD";
}

function IsLTL(shipmentType: string) {
    return shipmentType === "LTL";
}

function IsGroupage(shipmentType: string) {
    return shipmentType === "Groupage";
}
function hasPacakageType(shipmentType: string) {
    return IsFCL(shipmentType) || IsFTL(shipmentType) || IsLCL(shipmentType) || IsLTL(shipmentType);
}