import { ShipmentSelectors } from '../selectors/Selectors';
import { ShipmentDetails } from '../models/ShipmentDetails';
import { PartnersDetails } from 'cypress/models/PartnersDetails';
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import { PayableDetails } from 'cypress/models/PayableDetails';
import { ReceivableDetails } from 'cypress/models/ReceivableDetails';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import * as GenerateRandoms from '../../../Base/cypress/actions/GenerateRandoms';
import { PackagesDetails } from 'cypress/models/PackagesDetails';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import * as Conditions from "../actions/Conditions";
import { AccountingURLs } from '../../../Accounting/cypress/constants/URLs';
import { AccountingSelectors } from '../../../Accounting/cypress/selectors/Selectors';
import { BaseURLs } from '../../../Base/cypress/constants/URLs';
import { QuickSearchDetails } from '../../../Base/cypress/models/QuickSearchDetails';

//#region ShipmentsWorkspace
export function NavigatesToShipmentsWorkspace() {
    cy.Click(BaseSelectors.OperationsMenu, null)
    cy.Click(ShipmentSelectors.ShipmentTab, null)
}

export function OpenNewShipmentWizard(shipmentLevel: string) {
    cy.Click(ShipmentSelectors.NewShipmentToggleButton, null)
    cy.Click(ShipmentSelectors.NewShipmentToggleButtonItem, shipmentLevel)
}
//#endregion
//#region Create Shipment
export function FillShipmentWizardsFields(shipmentDetails: ShipmentDetails) {
    if (!Conditions.IsMaster(shipmentDetails.ShipmentLevel)) {
        FillDirectAndHouseFields(shipmentDetails);
    } else {
        FillMasterFields(shipmentDetails);
    }
}

export function CreateShipment(shipmentLevel: string) {
    let createSelector = Conditions.IsMaster(shipmentLevel) ? ShipmentSelectors.CreateMasterShipmentButton : ShipmentSelectors.CreateShipmentButton;
    cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(createSelector, null)
}
//#endregion
//#region Open And UpdateShipment
export function UpdateShipment(saveButtonSelector: string, saveButtonSelectorContains?: string) {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(saveButtonSelector, saveButtonSelectorContains)
}

export function OpenShipment(shipmentNumber: string) {
    cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetMenuButtonGroups, RequestAliases.WaitLoadShipmentMenuButtons);

    var quickSearchDetails = {
        Selector: ShipmentSelectors.ShipmentSearchBar,
        Parent: ShipmentSelectors.ShipmentSearchParent,
        ParentClass: ShipmentSelectors.ShipmentSearchParentClass,
        WaitURL: BaseURLs.GetQuickSearch,
        Value: shipmentNumber
    } as QuickSearchDetails;

    cy.SelectQuickSearchFirstElement(quickSearchDetails);
    
    BaseAssertion.AssertStatusCode(RequestAliases.WaitLoadShipmentMenuButtons, 200);
}

export function CancelShipment(note :string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.CancelShipmentButton, null);
    cy.FillLogTextBox(ShipmentSelectors.ShipmentEventNote ,note)
    UpdateShipment(ShipmentSelectors.ConfirmActionButton);
}

export function ReactiveShipment(note :string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.ReactivateShipmentButton, null);
    cy.FillLogTextBox(ShipmentSelectors.ShipmentEventNote , note)
    UpdateShipment(ShipmentSelectors.ConfirmActionButton);
}

export function ValidateCancelIconExist(IsCancelled:boolean){
    if(IsCancelled){
        cy.get(ShipmentSelectors.ShortTitleControl).should(BaseSelectors.Exist)
    }else{
        cy.get(ShipmentSelectors.ShortTitleControl).should(BaseSelectors.NotExist)
    }
}

export function ValidateShipmentEventActions(excpectedMSG : string){
    cy.DefineRequestWait(RestAPI.GET,URLs.TraceEventsDomain,RequestAliases.GetTraceEvent);
    cy.Click(ShipmentSelectors.Events,null);
    BaseAssertion.AssertStatusCode(RequestAliases.GetTraceEvent, 200).then((interception) => {
        let actualMsg = interception.response.body[0].Notes;
        assert.equal(actualMsg, excpectedMSG);
    });
}

export function ValidateShipmentFields(IsCanceled:boolean){
    cy.Click(ShipmentSelectors.GeneralTab,null);
    EditGeneralField();
    if(IsCanceled){
        BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentValueOfGoods,BaseSelectors.BeEmpty)
    }else{
        BaseAssertion.AssertElementHaveValue(ShipmentSelectors.ShipmentValueOfGoods,'123.00')
    }
    CheckIfDisable(ShipmentSelectors.OrdersTab,ShipmentSelectors.ShipmentBookingNumberOfPackages,IsCanceled);
    CheckIfDisable(ShipmentSelectors.OrdersTab,ShipmentSelectors.ShipmentMainCarriageCarrierId,IsCanceled);
    CheckIfHaveClass(ShipmentSelectors.PartnersTab,ShipmentSelectors.PartnerToggle,"ToggleButtonDisabled",IsCanceled);
    CheckIfDisable(ShipmentSelectors.PartnerEditShipper,BaseSelectors.RedButton,IsCanceled);
    cy.Click(BaseSelectors.Button,BaseSelectors.ContainsCancel);
    CheckIfDisable(ShipmentSelectors.PackagesTab,ShipmentSelectors.AddPackage,IsCanceled);
    CheckIfHaveClass(ShipmentSelectors.RoutingsTab,ShipmentSelectors.RoutingToggle,'ToggleButtonDisabled',IsCanceled)
    CheckIfDisable(ShipmentSelectors.EditRoutingMainCarriage,BaseSelectors.RedButton,IsCanceled);
    cy.Click(BaseSelectors.Button,BaseSelectors.ContainsCancel);
    CheckIfDisable(ShipmentSelectors.PayablesTab,BaseSelectors.AddButton,IsCanceled);
    CheckIfDisable(ShipmentSelectors.ReceivablesTab,BaseSelectors.AddButton,IsCanceled);
}

function EditGeneralField(){
    cy.FillLogTextBox(ShipmentSelectors.ShipmentValueOfGoods,"123");
    UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
}

function CheckIfDisable(tabSelector:string , fieldSelector:string , IsDisable:boolean){
    cy.Click(tabSelector,null);
    BaseAssertion.AssertElementDisabled(fieldSelector,IsDisable?BaseSelectors.BeDisabled:BaseSelectors.NotBeDisabled)
}

function CheckIfHaveClass(tabSelector:string , fieldSelector:string ,classValue:string ,IsHaveCLass:boolean){
    cy.Click(tabSelector,null);
    BaseAssertion.AssertElementHaveClasss(fieldSelector,IsHaveCLass?BaseSelectors.HaveClass:BaseSelectors.NotHaveClass,classValue)
}

export function ConnectOrDisconnectShipment() {
    UpdateShipment(BaseSelectors.RedButton, "Yes");
}
//#endregion
//#region General Tab
export function FillGeneralTab(GrossWeight: string, MoveType: string) {
    cy.Click(ShipmentSelectors.GeneralTab, null)
    cy.FillLogTextBox(ShipmentSelectors.ShipmentGrossWeight, GrossWeight)
    cy.FillLogLov(ShipmentSelectors.ShipmentMoveType, MoveType, true)
}
//#endregion
//#region Order Tab
export function FillOrdersTab(packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(ShipmentSelectors.OrdersTab, null)

    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelectors.OrdersAddPackage, null)

        cy.FillLogTextBox(ShipmentSelectors.OrderPackageQuantity, packagesDetails[i].Quantity.toString())
        if (Conditions.HasPacakageType(shipmentType)) {
            cy.FillLogLov(ShipmentSelectors.OrderPackageType, packagesDetails[i].PackageType, true)
        }
        if (!Conditions.IsFCL(shipmentType) && !Conditions.IsFTL(shipmentType)) {
            cy.FillLogTextBox(ShipmentSelectors.OrderPackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(ShipmentSelectors.OrderPackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(ShipmentSelectors.OrderPackageHeight, packagesDetails[i].Height.toString())
        }
        cy.FillLogTextBox(ShipmentSelectors.OrderPackageGrossWeight, packagesDetails[i].GrossWeight.toString())
        cy.Click(ShipmentSelectors.OrderOKButton, null)
    }
}
//#endregion
//#region Partner Tab
export function FillPartnersTab(direction: string, transportMode: string, partnersDetails: PartnersDetails) {
    cy.Click(ShipmentSelectors.PartnersTab, null)

    if (Conditions.IsImport(direction)) {
        AddPartner(ShipmentSelectors.AddShipperButton, ShipmentSelectors.ShipmentShipper, partnersDetails.Shipper)
    }
    if (Conditions.IsExport(direction) || Conditions.IsDrop(direction) || (Conditions.IsDomestic(direction) && !Conditions.IsInland(transportMode))) {
        AddPartner(ShipmentSelectors.AddConsigneeButton, ShipmentSelectors.ShipmentConsignee, partnersDetails.Consignee)
    }
    AddPartner(ShipmentSelectors.AddAgentButton, ShipmentSelectors.ShipmentAgent, partnersDetails.Agent)
    if ((Conditions.IsImport(direction) && Conditions.IsAir(transportMode)) || (Conditions.IsDomestic(direction) && Conditions.IsAir(transportMode))) {
        AddPartner(ShipmentSelectors.AddIssuingCarrierAgentButton, ShipmentSelectors.ShipmentIssuingCarrierAgent)
    }
    AddPartner(ShipmentSelectors.AddCustomsAgentExportButton, ShipmentSelectors.ShipmentCustomAgentExport, partnersDetails.CustomsAgentExport)
    AddPartner(ShipmentSelectors.AddCustomsAgentImportButton, ShipmentSelectors.ShipmentCustomAgentImport, partnersDetails.CustomsAgentImport)
    AddPartner(ShipmentSelectors.AddNotify1Button, ShipmentSelectors.ShipmentNotify1, partnersDetails.Notify1)
    AddPartner(ShipmentSelectors.AddNotify2Button, ShipmentSelectors.ShipmentNotify2, partnersDetails.Notify2)
    AddPartner(ShipmentSelectors.AddShipperNotExporterButton, ShipmentSelectors.ShipmentShipperNotExporter, partnersDetails.ShipperNotExporter)
    AddPartner(ShipmentSelectors.AddConsigneeNotImporterButton, ShipmentSelectors.ShipmentConsigneeNotImporter, partnersDetails.ConsigneeNotImporter)
    AddPartner(ShipmentSelectors.AddFreightForwarderButton, ShipmentSelectors.ShipmentFreightForwarder, partnersDetails.FreightForwarder)
    AddPartner(ShipmentSelectors.AddColoaderButton, ShipmentSelectors.ShipmentColoader, partnersDetails.Coloader)
    AddPartner(ShipmentSelectors.AddCustomClearancePointButton, ShipmentSelectors.ShipmentCustomClearancePoint, partnersDetails.CustomClearancePoint)
    AddPartner(ShipmentSelectors.AddConsolidatorButton, ShipmentSelectors.ShipmentConsolidator, partnersDetails.Consolidator)
    AddPartner(ShipmentSelectors.AddReleasingAgentButton, ShipmentSelectors.ShipmentReleasingAgent, partnersDetails.ReleasingAgent)
}
//#endregion
//#region Package Tab
export function FillPackageTab(transportMode: string, packagesDetails: PackagesDetails[], shipmentType?: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)
    for (let i = 0; i < packagesDetails.length; i++) {
    packagesDetails[i].ContainerNumber = packagesDetails[i].ContainerNumber == 'Random' ? GetGeneratedRandomContainerNumber() : packagesDetails[i].ContainerNumber;
        cy.Click(ShipmentSelectors.AddPackage, null)
        if (Conditions.HasPacakageType(shipmentType)) {
            cy.FillLogLov(ShipmentSelectors.PackageType, packagesDetails[i].PackageType, true)
            cy.FillLogTextBox(ShipmentSelectors.ContainerNumber, packagesDetails[i].ContainerNumber)
        }
        if (!Conditions.IsFCL(shipmentType) && !Conditions.IsFTL(shipmentType)) {
            cy.FillLogTextBox(ShipmentSelectors.PackageQuantity, packagesDetails[i].Quantity.toString())
            cy.FillLogTextBox(ShipmentSelectors.PackageLength, packagesDetails[i].Length.toString())
            cy.FillLogTextBox(ShipmentSelectors.PackageWidth, packagesDetails[i].Width.toString())
            cy.FillLogTextBox(ShipmentSelectors.PackageHeight, packagesDetails[i].Height.toString())
        }
        cy.get(ShipmentSelectors.PackageWeight).type(packagesDetails[i].GrossWeight.toString());

        if (Conditions.IsAir(transportMode)) {
            cy.Click(ShipmentSelectors.AirPackageOKButton, null)
        } else {
            cy.Click(ShipmentSelectors.OceanPackageOKButton, null)
        }
    }
}
//#endregion
//#region House Shipment Tab
export function FillHouseInShipmentsTab(Shipper: string) {
    cy.FillLogLov(ShipmentSelectors.ShipmentCustomer, Shipper, true)
}
//#endregion
//#region Receivables Tab
export function FillReceivablesTab(receivableDetails: ReceivableDetails[]) {
    cy.Click(ShipmentSelectors.ReceivablesTab, null)
    for (let i = 0; i < receivableDetails.length; i++) {
        cy.Click(ShipmentSelectors.AddNewReceivableLine, null)
        cy.FillLogLov(ShipmentSelectors.ReceivableChargesType, receivableDetails[i].ChargesType, true)
        cy.FillLogLov(ShipmentSelectors.ReceivableMeasurement, receivableDetails[i].UOM, true)
        cy.get(ShipmentSelectors.ReceivableQuantity).type(receivableDetails[i].Quantity.toString());
        cy.get(ShipmentSelectors.ReceivableUnitPrice).type(receivableDetails[i].UnitPrice.toString());
        cy.FillLogLov(ShipmentSelectors.ReceivableCurrency, receivableDetails[i].Currency, true)
        cy.FillLogTextBox(ShipmentSelectors.ShipmentReceivableRate,receivableDetails[i].ExchangeRate.toString());

        cy.Click(ShipmentSelectors.AddReceivableOkButton, null)
    }
}
export function GenerateReceivablesFromPayables(profit?:boolean) {
    cy.Click(ShipmentSelectors.ReceivablesTab, null)
    cy.Click(ShipmentSelectors.ReceivableFromPayables, null)
    cy.get(BaseSelectors.CheckBoxLine).eq(0).click()
    cy.Click(BaseSelectors.RedButton, "Ok")
    // if(profit){
    // cy.Click("#Edit",null)
    // cy.get(ShipmentSelectors.ReceivableUnitPrice).clear().type("20");
    // cy.Click(ShipmentSelectors.AddReceivableOkButton, null)

    // }
    UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
}
//#endregion
//#region Routing Tab
export function FillPickupRouting() {
    cy.Click(ShipmentSelectors.RoutingsTab, null, true)
    cy.Click(ShipmentSelectors.RoutingToggle, null, true)
    cy.DefineRequestWait(RestAPI.GET, URLs.CardViews, RequestAliases.CardViewsRequest)
    cy.DefineRequestWait(RestAPI.GET, URLs.AddressViews, RequestAliases.AddressViewsRequest)
    cy.Click(ShipmentSelectors.PickUp, null)
    BaseAssertion.AssertStatusCode(RequestAliases.CardViewsRequest, 200)
    BaseAssertion.AssertStatusCode(RequestAliases.AddressViewsRequest, 200)
    cy.Click(ShipmentSelectors.SaveClose, null)
}

export function EditMainCarriageLegs(Airline: string) {
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null)
    cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageCarrierId, Airline, false)
    cy.FillRandomNumber(ShipmentSelectors.ShipmentFlightNumber, 100, 999)
    cy.FillRandomNumber(ShipmentSelectors.ShipmentMAWB, 10000000, 99999999)
    cy.Click(ShipmentSelectors.ShipmentDateMaincarriageATD, null)
    cy.Click(BaseSelectors.Button, "Today")
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
    //cy.Click(ShipmentSelectors.ShipmentSaveButton, null);
}

export function AddMainCarriageATDDateAndTime(date: string, time: string) {
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null);
    cy.FillDate(ShipmentSelectors.MainCarriageATDDate, date);
    cy.FillLogTextBox(ShipmentSelectors.MainCarriageATDTime, time);
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
}

export function AddMainCarriageATADateAndTime(date: string, time: string) {
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null);
    cy.FillDate(ShipmentSelectors.MainCarriageATADate, date);
    cy.FillLogTextBox(ShipmentSelectors.MainCarriageATATime, time);
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
}

export function FillDeliveryRouting(partner: string) {
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    cy.Click(ShipmentSelectors.RoutingToggle, null, true)
    cy.DefineRequestWait(RestAPI.GET, URLs.CardViews, RequestAliases.CardViewsRequest)
    cy.DefineRequestWait(RestAPI.GET, URLs.AddressViews, RequestAliases.AddressViewsRequest)
    cy.Click(ShipmentSelectors.Delivery, null)
    BaseAssertion.AssertStatusCode(RequestAliases.CardViewsRequest, 200)
    BaseAssertion.AssertStatusCode(RequestAliases.AddressViewsRequest, 200)

    cy.FillLogLov(ShipmentSelectors.ShipmentPickUpDeliveryToPartnerCard, partner, false)

    cy.Click(ShipmentSelectors.SaveClose, null)
}

export function FillPreCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    cy.Click(ShipmentSelectors.RoutingToggle, null)
    cy.get(ShipmentSelectors.PreCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(ShipmentSelectors.PreCarriage, null)
            cy.FillLogLov(ShipmentSelectors.ShipmentPreCarriageTransportMode, transportMode, true)
            cy.FillLogLov(ShipmentSelectors.ShipmentPreCarriageFromPort, fromPort, false)
            cy.FillLogLov(ShipmentSelectors.ShipmentPreCarriageToPort, toPort, false)
            cy.Click(ShipmentSelectors.PreCarriageOKBtn, null)
        } else {
            cy.Click(ShipmentSelectors.RoutingsTab, null)
        }
    })
}

export function FillOnCarriageRouting(transportMode: string, fromPort: string, toPort: string) {
    cy.Click(ShipmentSelectors.RoutingsTab, null)
    cy.Click(ShipmentSelectors.RoutingToggle, null)
    cy.get(ShipmentSelectors.OnCarriage).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(ShipmentSelectors.OnCarriage, null)
            cy.FillLogLov(ShipmentSelectors.ShipmentOnCarriageTransportMode, transportMode, true)
            cy.FillLogLov(ShipmentSelectors.ShipmentOnCarriageFromPort, fromPort, false)
            cy.FillLogLov(ShipmentSelectors.ShipmentOnCarriageToPort, toPort, false)
            cy.Click(ShipmentSelectors.OnCarriageOKBtn, null)
        } else {
            cy.Click(ShipmentSelectors.RoutingsTab, null)
        }
    })
}
//#endregion
//#region Payables Tab
export function FillPayablesTab(payableDetails: PayableDetails) {
    cy.Click(ShipmentSelectors.PayablesTab, null)
    cy.Click(ShipmentSelectors.AddNewPayableLine, null)
    cy.FillLogLov(ShipmentSelectors.ShipmentPayableChargesType, payableDetails.ChargesType, true);
    cy.FillLogLov(ShipmentSelectors.ShipmentPayableMeasurement, payableDetails.UOM, true)
    cy.FillLogTextBox(ShipmentSelectors.ShipmentPayableQuantity, payableDetails.Quantity.toString());
    cy.FillLogTextBox(ShipmentSelectors.ShipmentPayableUnitPrice, payableDetails.UnitPrice.toString());
    cy.FillLogLov(ShipmentSelectors.ShipmentPayableCurrency, payableDetails.Currency, true)
    cy.FillLogTextBox(ShipmentSelectors.ShipmentPayableRate,payableDetails.ExchangeRate.toString());

    if (payableDetails.Vendor) {
        cy.FillLogLov(ShipmentSelectors.ShipmentPayableVendor, payableDetails.Vendor, true)
        cy.DefineRequestWait(RestAPI.GET, '**/cardviews/**', 'cardviews')
        cy.Click(ShipmentSelectors.AddPayableOkButton, null)
        BaseAssertion.AssertStatusCode('cardviews', 200)
    } else {
        cy.Click(ShipmentSelectors.AddPayableOkButton, null)
    }
}
//#endregion
//#region Copy Shipment
export function CopyShipment(shipmentLevel: string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true)
    cy.Click(ShipmentSelectors.CopyShipmentButton, null)
    CreateShipment(shipmentLevel);
}
//#endregion
//#region Update Closed Shipment
export function UpdateClosedShipment() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(BaseSelectors.RedButton, "Confirm");
}
//#endregion
//#region Docs Out Tab
export function SendDocs() {
    cy.Click(ShipmentSelectors.DocsOutTab, null)
    cy.FillLogTextBox(BaseSelectors.SearchField, "Flight Update")
    cy.get("#FU-L-DocsOut").click()
    cy.get("#FU-S-DocsOut").click()
    cy.FillLogTextBox(ShipmentSelectors.EmailSearchInput, "abd@logitudeworld.com{enter}")
    cy.DefineRequestWait(RestAPI.POST, URLs.HtmlEditor, "WaitSendDocs")
    cy.Click(ShipmentSelectors.SendMessageButton, null)
}
//#endregion
//#region Docs In Tab
export function DeleteAttachment() {
    cy.DefineRequestWait(RestAPI.GET, URLs.DocumentsFilingExtended, "WaitDelete")
    cy.contains("Delete Attachment").click()
}
//#endregion

//#region Containers
export function NavigatesToAContainersWorkspace() {
    cy.Click(BaseSelectors.OperationsMenu, null);
    cy.Click(ShipmentSelectors.ContainersTab, null);
}

export function ContainersView(containerView: string) {
    containerView = containerView.replace(/\s/g, "");
    cy.get(ShipmentSelectors.ContainersView(containerView)).children().first().click();
}

function GetGeneratedRandomContainerNumber(): string{
    var RandomString = GenerateRandoms.GenerateRandomString(4, true)
    var RandomNumber = GenerateRandoms.GenerateRandomNumber(100000, 999999)
    var CheckDigit = 
    return RandomString + RandomNumber + CheckDigit;
}
//#endregion
export function FillMainCarriage(airline: string) {
    cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageCarrierId, airline, false);
    cy.FillRandomNumber(ShipmentSelectors.ShipmentFlightNumber, 100, 999);
    cy.FillRandomNumber(ShipmentSelectors.ShipmentMAWB, 10000000, 99999999);
}

export function OpenAWBWizard(shipmentLevel: string) {
    cy.Click(BaseSelectors.Button, shipmentLevel + " AWB Wizard");
}

export function FillAWBWizardPackagesTab(packagesDetails: PackagesDetails[]) {
    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelectors.AddPackageLineInAWBWizard, null);
        cy.FillLogTextBox(ShipmentSelectors.PackageQuantityInAWBWizard, packagesDetails[i].Quantity.toString());
        cy.FillLogTextBox(ShipmentSelectors.PackageLengthInAWBWizard, packagesDetails[i].Length.toString());
        cy.FillLogTextBox(ShipmentSelectors.PackageWidthInAWBWizard, packagesDetails[i].Width.toString());
        cy.FillLogTextBox(ShipmentSelectors.PackageHeightInAWBWizard, packagesDetails[i].Height.toString());
        cy.FillLogTextBox(ShipmentSelectors.PackageWeightInAWBWizard, packagesDetails[i].GrossWeight.toString());
        cy.Click(BaseSelectors.OKBtn, null);
    }
}

function AddPartner(partnerTypeId: string, partnerFieldId: string, partner?: string) {
    cy.Click("label", "Add Partners")
    cy.get(partnerTypeId).then((btn) => {
        if (!btn.is('[disabled]')) {
            cy.Click(partnerTypeId, null)
            let partnerFieldSelector = "addeditpartnercomponent input[id^='" + partnerFieldId.replace("#", "") + "']"
            cy.FillLogLov(partnerFieldSelector, partner, false)
            cy.Click(ShipmentSelectors.PartnerOKButton, null)
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
    FillDirection(shipmentDetails.Direction);
    FillTransportMode(shipmentDetails.TransportMode);
    FillShipmentType(shipmentDetails.ShipmentType, shipmentDetails.TransportMode);
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

function FillShipperAndConsignee(shipmentDetails: ShipmentDetails) {
    if (Conditions.IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        cy.FillLogLov(ShipmentSelectors.ShipmentShipper, shipmentDetails.Shipper, false)
        cy.FillLogLov(ShipmentSelectors.ShipmentConsignee, shipmentDetails.Consignee, false)
    } else {
        if (Conditions.IsImport(shipmentDetails.Direction)) {
            cy.FillLogLov(ShipmentSelectors.ShipmentConsignee, shipmentDetails.Consignee, false)
        } else {
            cy.FillLogLov(ShipmentSelectors.ShipmentShipper, shipmentDetails.Shipper, false)
        }
    }
}

function FillMainCarriagePorts(shipmentDetails: ShipmentDetails) {
    if (!Conditions.IsInlandDomestic(shipmentDetails.Direction, shipmentDetails.TransportMode)) {
        let fromPortSelector = Conditions.IsMaster(shipmentDetails.ShipmentLevel) ? ShipmentSelectors.MasterMainCarriageFromPort : ShipmentSelectors.ShipmentMainCarriageFromPort;
        let toPortSelector = Conditions.IsMaster(shipmentDetails.ShipmentLevel) ? ShipmentSelectors.MasterMainCarriageToPort : ShipmentSelectors.ShipmentMainCarriageToPort;
        cy.FillLogLov(fromPortSelector, shipmentDetails.MainCarriageFromPort, false)
        cy.FillLogLov(toPortSelector, shipmentDetails.MainCarriageToPort, false)
    }
}

function FillMasterAgent(shipmentDetails: ShipmentDetails) {
    cy.FillLogLov(ShipmentSelectors.MasterAgent, shipmentDetails.Agent, false)
}