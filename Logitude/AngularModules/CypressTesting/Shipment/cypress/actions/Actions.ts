import { ShipmentSelectors } from '../../../Shipment/cypress/selectors/Selectors';
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
import { BaseURLs } from '../../../Base/cypress/constants/URLs';
import { QuickSearchDetails } from '../../../Base/cypress/models/QuickSearchDetails';
import * as BaseActions from '../../../Base/cypress/actions/Actions';
import { EventDetails } from '../models/EventDetails';
import { EventTypeDetails } from '../models/EventTypeDetails';
import { WarehouseStorage } from 'cypress/models/WarehouseStorage';
import { ShipmentContext } from '../models/ShipmentContext';

let ShipmentNumber = null;
export function NavigatesToEventsTab() {
    cy.DefineRequestWait(RestAPI.GET, URLs.TraceEventsDomain, RequestAliases.GetTraceEvent);
    cy.Click(ShipmentSelectors.EventsTab, null)
    BaseAssertion.AssertStatusCode(RequestAliases.GetTraceEvent,200)
}
export function ClickOnAddEventButton() {
    NavigatesToEventsTab();
    cy.Click(BaseSelectors.Button, ShipmentSelectors.ContainAddEvent, null)
}
export function FillEventDetails(eventDetails: EventDetails) {
    ClickOnAddEventButton()
    cy.FillLogLov(ShipmentSelectors.EventType, eventDetails.EventType, true)
    cy.FillDate(ShipmentSelectors.EventDate, eventDetails.EventDate)
    cy.FillLogTextBox(ShipmentSelectors.EventTime, eventDetails.EventTime)
    cy.FillLogTextBox(ShipmentSelectors.TraceEventNotes, eventDetails.EventNotes)
}
export function AddEvent(){
    cy.DefineRequestWait(RestAPI.GET, URLs.TraceEventsDomain, RequestAliases.GetTraceEvent);
    cy.DefineRequestWait(RestAPI.GET, URLs.ShipmentGetSingle, RequestAliases.ShipmentGetSingle);
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
}
export function AssertAddEvent() {
    BaseAssertion.AssertStatusCode(RequestAliases.GetTraceEvent, 200).then((interception) => {
        expect(interception.response.body.Notes,)
    })
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentGetSingle,200)
}
export function AssertExceptionResolved (EventNote:string){
    BaseAssertion.AssertStatusCode( RequestAliases.ShipmentRequest,200).then((interception) => {
        expect(interception.response.body.ExceptionResolvedDescription,EventNote)
    })  
}
export function AssertEventAppearInEventTab(EventType:string) {
cy.get(ShipmentSelectors.EventItemBox).contains(EventType)
}
export function CheckHasException(HasExceptionValue:string){
  cy.get(BaseSelectors.HeaderScreen).find(BaseSelectors.HeaderScreenLable)
  .contains(ShipmentSelectors.ContainHasException).next(BaseSelectors.td).should('contain.text',HasExceptionValue)  
}
export function ClickOnExceptionResolved(ExceptionResolvedNote:string){
    cy.Click(ShipmentSelectors.ShipmentMoreList,null)
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(ShipmentSelectors.ShipmentExceptionResolved,null)
    cy.FillLogTextBox(ShipmentSelectors.EventNotes,ExceptionResolvedNote)
    cy.Click(ShipmentSelectors.ConfirmActionButton,null)
}
export function RefreshEventTab(){
    cy.get(BaseSelectors.CurvedEditArea).find(BaseSelectors.Refresh).click()
}
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
    cy.Click(saveButtonSelector, saveButtonSelectorContains, false)
}
export function UpdateMaster(){
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.PutShipment)
}

//export function GetHouseNumber() : any {
    //BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception)=>{
    //ShipmentNumber=interception.response.body.ShipmentNumber;
    //ShipmentNumber= interception.response.body.House;
    //return ShipmentNumber
    ////})

//}
//export function openHouseShipment(){
    //cy.Click(ShipmentSelectors.NewTapItem, null);
    //cy.Click(ShipmentSelectors.ShipmentWorkspace, null)
   //OpenShipment(ShipmentContext.HouseNumber)

//}
export function openHouseShipment(){
    cy.Navigate(ShipmentSelectors.ShipmentsTab);
    cy.get("hyperlink").find("button").click()
}
export function OpenShipment(shipmentNumber: string) {
    cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetMenuButtonGroups, RequestAliases.WaitLoadShipmentMenuButtons);

    var quickSearchDetails = {
        Selector: ShipmentSelectors.ShipmentSearchBar,
        
       // ShipmentSearchBar
        Parent: ShipmentSelectors.ShipmentSearchParent,
        ParentClass: ShipmentSelectors.ShipmentSearchParentClass,
        WaitURL: BaseURLs.GetQuickSearch(shipmentNumber),
        Value: shipmentNumber,
        RequestAliase: RequestAliases.QuickSearchDataLoaded
    } as QuickSearchDetails;

    cy.SelectQuickSearchFirstElement(quickSearchDetails);

    BaseAssertion.AssertStatusCode(RequestAliases.WaitLoadShipmentMenuButtons, 200);
}
export function SplitShipment(packageNumber: string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.SplitShipmentButton, null);
    cy.Click(ShipmentSelectors.SplitButton(packageNumber), null);
    cy.DefineRequestWait(RestAPI.PUT, URLs.SplitShipment, RequestAliases.SplitShipmentRequest)
    cy.Click(BaseSelectors.RedButton, ShipmentSelectors.ContainsSplit);
}

export function PartialSplitShipment(packagesDetails: PackagesDetails) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.SplitShipmentButton, null);
    cy.Click(ShipmentSelectors.PackagePartialSplit, null);
    cy.DefineRequestWait(RestAPI.PUT, URLs.SplitShipment, RequestAliases.SplitShipmentRequest)
    FillPartialSplitWizard(packagesDetails);
    cy.Click(BaseSelectors.RedButton, ShipmentSelectors.ContainsSplit);
}

export function CancelShipment(note: string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.CancelShipmentButton, null);
    cy.FillLogTextBox(ShipmentSelectors.ShipmentEventNote, note)
    UpdateShipment(ShipmentSelectors.ConfirmActionButton);
}
export function OperationalCloseShipment() {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.OperationalCloseButton, null);
    
    UpdateShipment(ShipmentSelectors.ConfirmActionButton);
}


export function ReactiveShipment(note: string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.ReactivateShipmentButton, null);
    cy.FillLogTextBox(ShipmentSelectors.ShipmentEventNote, note)
    UpdateShipment(ShipmentSelectors.ConfirmActionButton);
}
export function RepoenShipment(note: string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(ShipmentSelectors.OperationalReopenButton, null);
    cy.FillLogTextBox(ShipmentSelectors.ShipmentEventNote, note)
    //UpdateShipment(ShipmentSelectors.ConfirmActionButton);
}

export function ValidateCancelIconExist(IsCancelled: boolean) {
    if (IsCancelled) {
        cy.get(ShipmentSelectors.ShortTitleControl).should(BaseSelectors.Exist)
    } else {
        cy.get(ShipmentSelectors.ShortTitleControl).should(BaseSelectors.NotExist)
    }
}
export function ValidateCloseIconExist(IsClosed: boolean) {
    if (IsClosed) {
        cy.get(ShipmentSelectors.ShortTitleControl).should(BaseSelectors.Exist)
    } else {
        cy.get(ShipmentSelectors.ShortTitleControl).should(BaseSelectors.NotExist)
    }
}

export function ValidateShipmentEventActions(eventSelector: string, excpectedMSG: string) {
    cy.DefineRequestWait(RestAPI.GET, URLs.TraceEventsDomain, RequestAliases.GetTraceEvent);
    cy.Click(eventSelector, null);
    BaseAssertion.AssertStatusCode(RequestAliases.GetTraceEvent, 200).then((interception) => {
        // let actualMSG = interception.response.body[0].Notes;
        // assert.equal(actualMSG, excpectedMSG);
        let IndexOfEvent = interception.response.body.map(function (t: { Notes: string; }) { return t.Notes; }).indexOf(excpectedMSG);
        assert.notEqual(IndexOfEvent, "-1")
    });
}

export function ValidateShipmentFields(IsCanceled: boolean) {
    cy.Click(ShipmentSelectors.GeneralTab, null);
    EditGeneralField();
    if (IsCanceled) {
        BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentValueOfGoods, BaseSelectors.BeEmpty)
    } else {
        cy.get(ShipmentSelectors.ShipmentValueOfGoods).then(($shipmentValueOfGoods) => {
            const shipmentValueOfGoods = $shipmentValueOfGoods.val()
             expect(shipmentValueOfGoods).to.be.oneOf(['123.00','123'])
    })
        
    }
    CheckIfDisable(ShipmentSelectors.OrdersTab, ShipmentSelectors.ShipmentBookingNumberOfPackages, IsCanceled);
    CheckIfDisable(ShipmentSelectors.OrdersTab, ShipmentSelectors.ShipmentMainCarriageCarrierId, IsCanceled);
    CheckIfHaveClass(ShipmentSelectors.PartnersTab, ShipmentSelectors.PartnerToggle, "ToggleButtonDisabled", IsCanceled);
    CheckIfDisable(ShipmentSelectors.PartnerEditShipper, BaseSelectors.RedButton, IsCanceled);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsCancel);
    CheckIfDisable(ShipmentSelectors.PackagesTab, ShipmentSelectors.AddPackage, IsCanceled);
    CheckIfHaveClass(ShipmentSelectors.RoutingsTab, ShipmentSelectors.RoutingToggle, 'ToggleButtonDisabled', IsCanceled)
    CheckIfDisable(ShipmentSelectors.EditRoutingMainCarriage, BaseSelectors.RedButton, IsCanceled);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsCancel);
    CheckIfDisable(ShipmentSelectors.PayablesTab, BaseSelectors.AddButton, IsCanceled);
    CheckIfDisable(ShipmentSelectors.ReceivablesTab, BaseSelectors.AddButton, IsCanceled);
}
export function ValidateCloseShipmentFields(IsClosed: boolean) {
    cy.Click(ShipmentSelectors.GeneralTab, null);
    EditGeneralField();
    if (IsClosed) {
        BaseAssertion.AssertElementDisabled(ShipmentSelectors.ShipmentValueOfGoods, BaseSelectors.BeEmpty)
    } else {
        cy.get(ShipmentSelectors.ShipmentValueOfGoods).then(($shipmentValueOfGoods) => {
            const shipmentValueOfGoods = $shipmentValueOfGoods.val()
             expect(shipmentValueOfGoods).to.be.oneOf(['123.00','123'])
    })
        
    }
    CheckIfDisable(ShipmentSelectors.OrdersTab, ShipmentSelectors.ShipmentBookingNumberOfPackages, IsClosed);
    CheckIfDisable(ShipmentSelectors.OrdersTab, ShipmentSelectors.ShipmentMainCarriageCarrierId, IsClosed);
    CheckIfHaveClass(ShipmentSelectors.PartnersTab, ShipmentSelectors.PartnerToggle, "ToggleButtonDisabled", IsClosed);
    CheckIfDisable(ShipmentSelectors.PartnerEditShipper, BaseSelectors.RedButton, IsClosed);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsClose);
    CheckIfDisable(ShipmentSelectors.PackagesTab, ShipmentSelectors.AddPackage, IsClosed);
    CheckIfHaveClass(ShipmentSelectors.RoutingsTab, ShipmentSelectors.RoutingToggle, 'ToggleButtonDisabled', IsClosed)
    CheckIfDisable(ShipmentSelectors.EditRoutingMainCarriage, BaseSelectors.RedButton, IsClosed);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsClose);
    CheckIfDisable(ShipmentSelectors.PayablesTab, BaseSelectors.AddButton, IsClosed);
    CheckIfDisable(ShipmentSelectors.ReceivablesTab, BaseSelectors.AddButton, IsClosed);
}
export function ValidatePackageDetails(tabSelector: string, partialSplitDetails: PackagesDetails, grossWeightSelector: string, isPackage: boolean) {
    cy.Navigate(tabSelector)
    if (isPackage) {
        GetCellAssertion("1", partialSplitDetails.Quantity.toString());
        GetCellAssertion("3", partialSplitDetails.Volume.toString());
        GetCellAssertion("5", partialSplitDetails.GrossWeight.toString());
    }
    cy.get(grossWeightSelector).then(($grossWeightSelector) => {
        const grossWeight = $grossWeightSelector.val()
         expect(grossWeight).to.be.oneOf([parseFloat(partialSplitDetails.GrossWeight.toString()).toString(), parseInt(partialSplitDetails.GrossWeight.toString()).toString()])
    
})
}

export function ValidateShipmentNumber(OldShipmentNumber: string) {
    BaseAssertion.AssertStatusCode(RequestAliases.GetAll, 200)
    cy.get(ShipmentSelectors.ShipmentNumberInTitle + BaseSelectors.LastElement).invoke('text').then((text) => {
        assert.notEqual(OldShipmentNumber + ":", text.trim())
    });
}

function GetCellAssertion(cellNumber: string, ValueToCompare: string) {
    cy.get(ShipmentSelectors.PackageGrid(cellNumber) + BaseSelectors.LastElement).children('div').children('div').invoke('text').then((text) => {
        assert.equal(ValueToCompare, text.trim())
    })
}

function EditGeneralField() {
    cy.FillLogTextBox(ShipmentSelectors.ShipmentValueOfGoods, "123");
    UpdateShipment(ShipmentSelectors.ShipmentSaveButton)
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200);
}

function CheckIfDisable(tabSelector: string, fieldSelector: string, IsDisable: boolean) {
    cy.Click(tabSelector, null);
    BaseAssertion.AssertElementDisabled(fieldSelector, IsDisable ? BaseSelectors.BeDisabled : BaseSelectors.NotBeDisabled)
}

function CheckIfHaveClass(tabSelector: string, fieldSelector: string, classValue: string, IsHaveCLass: boolean) {
    cy.Click(tabSelector, null);
    BaseAssertion.AssertElementHaveClasss(fieldSelector, IsHaveCLass ? BaseSelectors.HaveClass : BaseSelectors.NotHaveClass, classValue)
}

function FillPartialSplitWizard(packagesDetails: PackagesDetails) {
    cy.FillLogTextBox(ShipmentSelectors.PackageQuantity, packagesDetails.Quantity.toString())
    cy.get(ShipmentSelectors.PackageVolume).type(packagesDetails.Volume.toString());
    cy.get(ShipmentSelectors.PackageWeight).type(packagesDetails.GrossWeight.toString());
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function ConnectOrDisconnectShipment() {
    UpdateShipment(BaseSelectors.RedButton, "Yes");
}
//#endregion
//#region General Tab
export function FillGeneralTab(ValueOfGoods: string, MoveType: string) {
    cy.Click(ShipmentSelectors.GeneralTab, null)
    cy.FillLogTextBox(ShipmentSelectors.ShipmentValueOfGoods, ValueOfGoods)
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
    // for (let i = 0; i < packagesDetails.length; i++) {
    //     packagesDetails[i].ContainerNumber = packagesDetails[i].ContainerNumber == 'Random' ? GetGeneratedRandomContainerNumber() : packagesDetails[i].ContainerNumber;
    // }
    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelectors.AddPackage, null)
        if (Conditions.HasPacakageType(shipmentType)) {
            cy.FillLogLov(ShipmentSelectors.PackageType, packagesDetails[i].PackageType, true)
            if(packagesDetails[i].ContainerNumber){
                cy.FillLogTextBox(ShipmentSelectors.ContainerNumber, packagesDetails[i].ContainerNumber)
            }
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

        if(packagesDetails[i].ChargeableWeight) {
            cy.FillLogTextBox(ShipmentSelectors.PackageChargeableWeight, packagesDetails[i].ChargeableWeight.toString(), true)
            cy.FillLogTextBox(ShipmentSelectors.PackageChargeableWeight, packagesDetails[i].ChargeableWeight.toString(), true)
        }
    }
}

export function AddInsidePackage(packagesDetails: PackagesDetails[]) {
    cy.Click(ShipmentSelectors.PackagesTab, null)
    for (let i = 0; i < packagesDetails.length; i++) {
        cy.Click(ShipmentSelectors.AddInsidePackage, null)
        cy.FillLogLov(ShipmentSelectors.InsidePackageType, packagesDetails[i].PackageType, true)
        cy.FillLogTextBox(ShipmentSelectors.InsidePackageQuantity, packagesDetails[i].Quantity.toString())
        cy.get(ShipmentSelectors.InsidePackageWeight).type(packagesDetails[i].GrossWeight.toString());
        cy.get(ShipmentSelectors.InsidePackageDescription).type(packagesDetails[i].Description.toString());
    }
    cy.Click("#OKInsidePackage", null)
}
//#endregion

//#region House Shipment Tab
export function CreateNewAttachedHouse(Shipper:string){
    cy.Click(ShipmentSelectors.NewAttachedHouse,null);
    FillHouseInShipmentsTab(Shipper);
    CreateShipment("House");
}

function FillHouseInShipmentsTab(Shipper: string) {
    cy.FillLogLov(ShipmentSelectors.ShipmentCustomer, Shipper, true)
}

export function CheckBusyIndicator(){
    cy.get(ShipmentSelectors.ComponentBusyIndicator).should(BaseSelectors.NotExist);
}

export function CheckHouseCheckBox(){
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.get(ShipmentSelectors.HouseCheckBox(ShipmentContext.HouseNumber)).find("input").then($InActiveStatesCheckBox => {
        if (!($InActiveStatesCheckBox.is(':checked'))) {
            cy.get(ShipmentSelectors.HouseCheckBox(ShipmentContext.HouseNumber)).find("input").check({ force: true })
        }
    })
}

export function UncheckHouseCheckBox(){
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.get(ShipmentSelectors.HouseCheckBox(ShipmentContext.HouseNumber)).find("input").then($InActiveStatesCheckBox => {
        if ($InActiveStatesCheckBox.is(':checked')) {
            cy.get(ShipmentSelectors.HouseCheckBox(ShipmentContext.HouseNumber)).find("input").uncheck({ force: true })
        }
    })
}

export function ValidateCheckHouseCheckBox(){
    cy.get(ShipmentSelectors.HouseCheckBox(ShipmentContext.HouseNumber)).find("input").then($InActiveStatesCheckBox => {
       var CheckBoxStatus = $InActiveStatesCheckBox.is(':checked')
       assert.equal(CheckBoxStatus , true);
    })
}

//#endregion

//#region Receivables Tab
export function FillReceivablesTab(receivableDetails: ReceivableDetails[], HaveAccountingSystem?: boolean) {
    cy.Click(ShipmentSelectors.ReceivablesTab, null)
    for (let i = 0; i < receivableDetails.length; i++) {
        cy.Click(ShipmentSelectors.AddNewReceivableLine, null)
        cy.FillLogLov(ShipmentSelectors.ReceivableChargesType, receivableDetails[i].ChargesType, true)
        cy.FillLogLov(ShipmentSelectors.ReceivableMeasurement, receivableDetails[i].UOM, true)
        cy.get(ShipmentSelectors.ReceivableQuantity).type(receivableDetails[i].Quantity.toString());
        cy.get(ShipmentSelectors.ReceivableUnitPrice).type(receivableDetails[i].UnitPrice.toString());
        cy.FillLogLov(ShipmentSelectors.ReceivableCurrency, receivableDetails[i].Currency, true)
        if (HaveAccountingSystem) {
            BaseActions.ClearExternalIDFromShipmentLevel(BaseSelectors.ChargesType)
            BaseActions.ClearExternalIDFromShipmentLevel(BaseSelectors.Currency)
        }
        cy.FillLogTextBox(ShipmentSelectors.ShipmentReceivableRate, receivableDetails[i].ExchangeRate.toString());
        cy.Click(ShipmentSelectors.AddReceivableOkButton, null)
    }
}

export function GenerateReceivablesFromPayables() {
    cy.Click(ShipmentSelectors.ReceivablesTab, null)
    cy.Click(ShipmentSelectors.ReceivableFromPayables, null)
    cy.get(BaseSelectors.CheckBoxLine).eq(0).click()
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
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
export function EditMainCarriageLegsFromToport(Gateway :string, Destination :string) {
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null)
    
    cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageFromPort,Gateway, false)
    cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageToPort,Destination, false)
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null);

    
   
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

export function AssertRoutingLegAppeared(legName:string , ContainerNumber:string){
    legName=legName.replace(/\s/g, "");
    BaseAssertion.AssertElementContain(ShipmentSelectors.legBoxItem(legName),ContainerNumber )
}

export function CalculateStorage() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetAllByFilter, RequestAliases.GetAll)
    cy.Click(BaseSelectors.Button, ShipmentSelectors.ContainsCalculateStorage, true);
    BaseAssertion.AssertStatusCode(RequestAliases.GetAll, 200)
}

export function AssertStorageFee(expectedStorageFeeValue: string) {
    // cy.get(ShipmentSelectors.StorageFeeResult).invoke('text').then((text) => {
    //     if (text.trim() != expectedStorageFeeValue) {
    //         AssertStorageFee(expectedStorageFeeValue)
    //     }
    // })
    cy.get(ShipmentSelectors.StorageFeeResult).should("be.visible")
    cy.get(ShipmentSelectors.StorageFeeResult).then(($StorageFee) => {
        const StorageFee = $StorageFee.text().toString()
         cy.log(StorageFee)
         expect(StorageFee.replace(/\s/g, '')).to.be.eq(expectedStorageFeeValue)
})
cy.Click(BaseSelectors.RedButton+BaseSelectors.LastElement,BaseSelectors.ContainsOK);

}

export function ValidateStoragePricing(AmountList:WarehouseStorage[],expectedWeight:string){
    cy.get(BaseSelectors.Hyperlink).contains(ShipmentSelectors.ContainsStoragePricing).click();
    for (let i = 0; i < AmountList.length; i++) {
        BaseAssertion.AssertElementTextEqual(BaseSelectors.CellWithRowAndCol(BaseSelectors.ColNo5, (i + 1).toString()),AmountList[i].Amount,BaseSelectors.td)
    }
    BaseAssertion.AssertElementContain(BaseSelectors.LogitudeScrollViewer,ShipmentSelectors.ContainsWeight+expectedWeight)
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
    cy.FillLogTextBox(ShipmentSelectors.ShipmentPayableRate, payableDetails.ExchangeRate.toString());

    if (payableDetails.Vendor) {
        cy.FillLogLov(ShipmentSelectors.ShipmentPayableVendor, payableDetails.Vendor, true)
        BaseAssertion.AssertElementHaveValue(ShipmentSelectors.ShipmentPayableVendor,payableDetails.Vendor)
      //  cy.DefineRequestWait(RestAPI.GET, '**/cardviews/**', 'cardviews')
        cy.Click(ShipmentSelectors.AddPayableOkButton, null)
       // BaseAssertion.AssertStatusCode('cardviews', 200)
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

//#region AMANAC 
export function NavigatesTocustomSettingsInMaintenance(maintenanceSearchValue: string) {
    cy.Click(BaseSelectors.MaintenanceMenu, null);
    cy.FillLogTextBox(BaseSelectors.NullSearch, maintenanceSearchValue);
    cy.Click(BaseSelectors.CustomsSettings, null);
}

export function UpdateLocalCustomsInterface(localCustomsInterfaceValue: string) {
    cy.get(BaseSelectors.typeCheckbox).check({ force: true });
    cy.FillLogLov(BaseSelectors.LocalCustomsInterfaceCode, localCustomsInterfaceValue, true);
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}

export function NavigatesToAMANACWorkspace() {
    cy.Click(BaseSelectors.OperationsMenu, null);
    cy.Click(ShipmentSelectors.AMANACTab, null);
}

export function AMANACView(TransportMode: string, AMANACView: string) {
    AMANACView = AMANACView.replace(/\s/g, "");
    cy.get(ShipmentSelectors.AMANACView(TransportMode, AMANACView)).children().first().click();
}

export function AMANACMarkeShipmentAs(MarkAs: string, ShipmentNumber: string) {
    MarkAs = MarkAs.replace(/\s/g, "");
    MarkAs = MarkAs.replace(/fortransfer/g, "");
    //SearchAShipmentInNullSearch(ShipmentNumber);
    cy.Click(ShipmentSelectors.AMANACMarkeShipmentAs(MarkAs, ShipmentNumber), null)
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsClose)
}

export function AMANACExportAShipment(ShipmentNumber: string) {
    cy.Click(ShipmentSelectors.CheckAll, null);
    //SearchAShipmentInNullSearch(ShipmentNumber);
    cy.Click(ShipmentSelectors.CheckShipment(ShipmentNumber), null);
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsExport);
}

export function SearchAShipmentInNullSearch(ShipmentNumber: string) {
    cy.DefineRequestWait(RestAPI.GET, URLs.ShipmentviewsGetbyfilters(ShipmentNumber), RequestAliases.ShipmentviewsGetbyfilters)
    cy.FillLogTextBox(BaseSelectors.NullSearch, ShipmentNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentviewsGetbyfilters, 200);
}

export function FillShippingLineInOrdersTab(ShippingLine: string) {
    cy.Click(ShipmentSelectors.OrdersTab, null);
    cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageCarrierId, ShippingLine, true);
}

export function FillVoyageNoVesselInRoutingsTab(VoyageNo: string, Vessel: string) {
    NavigateToEditMAinCarriage()
    cy.FillLogTextBox(ShipmentSelectors.MainCarrigeVoyageNo, VoyageNo)
    cy.FillLogLov(ShipmentSelectors.MainCarrigeVessel, Vessel, true)
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
}

export function FillMAWBInRoutingsTab() {
    NavigateToEditMAinCarriage()
    cy.FillRandomNumber(ShipmentSelectors.ShipmentMAWB, 10000000, 99999999);
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
}

export function EditPackageGrossWeightPackagesTab(GrossWeight: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)
    cy.Click(ShipmentSelectors.EditPackage, null)
    cy.FillLogTextBox(ShipmentSelectors.PackageWeight, GrossWeight)
    cy.Click(BaseSelectors.RedButton, null)
}

export function Retransfer() {
    cy.Click(ShipmentSelectors.CustomsTab, null);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsSendtoCustoms)
    cy.Click(ShipmentSelectors.CustomsTransmissionsRetransfer, null)
    cy.Click(ShipmentSelectors.CloseCustomsTransmissions, null)
}

export function FormatDate(date: string): string{
    var dateString = date == 'Today' ? new Date().toLocaleDateString("en-US", { timeZone: "Asia/Jerusalem"}) : date
    var currentDateArray = dateString.split("/");
    return currentDateArray[1] + " " + GetMonth(currentDateArray[0]) + " " + currentDateArray[2];
}

function GetMonth(monthNum: string) {
    switch (monthNum) {
      case "1": return "Jan"; 
      case "2": return "Feb"; 
      case "3": return "Mar"; 
      case "4": return "Apr"; 
      case "5": return "May"; 
      case "6": return "June"; 
      case "7": return "July"; 
      case "8": return "Aug"; 
      case "9": return "Sept"; 
      case "10": return "Oct"; 
      case "11": return "Nov"; 
      case "12": return "Dec"; 
    }
  }
//#endregion

//#region Containers
export function NavigatesToAContainersWorkspace() {
    cy.Click(BaseSelectors.OperationsMenu, null);
    cy.Click(ShipmentSelectors.ContainersTab, null);
}

export function ContainersView(containerView: string) {
    containerView = containerView.replace(/\s/g, "");
    cy.get(ShipmentSelectors.ContainersView(containerView)).first().click();
}

export function GetGeneratedRandomContainerNumber(): string {
    return GenerateRandoms.GetValidContainerNumber(GenerateRandoms.GenerateRandomString(4, true) + GenerateRandoms.GenerateRandomNumber(1000000, 9999999));
}

export function AddDeliveryFollowUpActualDepartureDateAndTime(date: string, time: string, ContainerNumber: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)

    cy.Click(ShipmentSelectors.AddContainerDelivery(ContainerNumber), null);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsAddFollowup);
    cy.FillDate(ShipmentSelectors.DeliveryATDDate, date);
    cy.FillLogTextBox(ShipmentSelectors.DeliveryATDTime, time);
}

export function AddDeliveryContainerDeliveryActualDepartureDateAndTime(date: string, time: string, ContainerNumber: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)

    cy.Click(ShipmentSelectors.AddContainerDelivery(ContainerNumber), null);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsAddContainerDelivery);
    cy.FillDate(ShipmentSelectors.PickUpDeliveryATDDate, date);
    cy.FillLogTextBox(ShipmentSelectors.PickUpDeliveryATDTime, time);
}

export function AddDeliveryActualArrivalDateAndTime(date: string, time: string, ContainerNumber: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)

    cy.Click(ShipmentSelectors.EditContainerDelivery(ContainerNumber), null)
    cy.FillDate(ShipmentSelectors.DeliveryATADate, date);
    cy.FillLogTextBox(ShipmentSelectors.DeliveryATATime, time);
}

export function AddContainerReturnFollowUpActualDepartureDateAndTime(date: string, time: string, ContainerNumber: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)

    cy.Click(ShipmentSelectors.AddContainerReturn(ContainerNumber), null)
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsAddFollowup);
    cy.FillDate(ShipmentSelectors.EmptyContainerReturnATDDate, date);
    cy.FillLogTextBox(ShipmentSelectors.EmptyContainerReturnATDTime, time);
}

export function AddContainerReturnContainerDeliveryActualDepartureDateAndTime(date: string, time: string, ContainerNumber: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)

    cy.Click(ShipmentSelectors.AddContainerReturn(ContainerNumber), null)
    cy.Click(BaseSelectors.Button,BaseSelectors.ContainsAddEmptyContainerReturn);
    cy.FillDate(ShipmentSelectors.PickUpDeliveryATDDate, date);
    cy.FillLogTextBox(ShipmentSelectors.PickUpDeliveryATDTime, time);
    cy.FillLogLov(ShipmentSelectors.ShipmentPickUpDeliveryToPartnerCard,"TestConsigneeImport",true)
}

export function AddContainerReturnActualArrivalDateAndTime(date: string, time: string, ContainerNumber: string) {
    cy.Click(ShipmentSelectors.PackagesTab, null)

    cy.Click(ShipmentSelectors.EditContainerReturn(ContainerNumber), null)
    cy.FillDate(ShipmentSelectors.EmptyContainerReturnATADate, date);
    cy.FillLogTextBox(ShipmentSelectors.EmptyContainerReturnATATime, time);
}

export function SearchAContainer(ContainerNumber: string) {
    cy.FillLogTextBox(BaseSelectors.SearchField, ContainerNumber);
    cy.DefineRequestWait(RestAPI.GET, URLs.ContainerFollowUpViewsGetbyfilters, RequestAliases.ContainerFollowUpViewsGetbyfilters)
    BaseAssertion.AssertStatusCode(RequestAliases.ContainerFollowUpViewsGetbyfilters, 200)
    cy.DefineRequestWait(RestAPI.GET, URLs.ContainerFollowUpViewsGetbyfilters, RequestAliases.ContainerFollowUpViewsGetbyfilters)
    BaseAssertion.AssertStatusCode(RequestAliases.ContainerFollowUpViewsGetbyfilters, 200)
}
export function UpdateFollowUp() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
}

export function UpdateContainerDelivery(status: string) {
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.ShipmentRequest)
    if (status == "new") {
        cy.Click(BaseSelectors.SaveButton, null)
    } else {
        cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
    }
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
    FillCustomerType(shipmentDetails);
    FillShipperAndConsignee(shipmentDetails);
    FillMainCarriagePorts(shipmentDetails);
}

function FillMasterFields(shipmentDetails: ShipmentDetails) {
    FillMainFields(shipmentDetails);
    //FillCustomerType(shipmentDetails);
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

function FillCustomerType(shipmentDetails: ShipmentDetails) { 
    if (Conditions.IsImport(shipmentDetails.Direction)) {
       // cy.FillLogLov(ShipmentSelectors.ShipmentCustomerType, "Consignee", true)
      cy.SelectDropDownListItem(ShipmentSelectors.LogLovShipmentCustomer,"Consignee")
    } else {
       // cy.FillLogLov(ShipmentSelectors.ShipmentCustomerType, "Shipper", true)
        cy.SelectDropDownListItem(ShipmentSelectors.LogLovShipmentCustomer,"Shipper")

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

function NavigateToEditMAinCarriage() {
    cy.Click(ShipmentSelectors.RoutingsTab, null);
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null)
}



//#region Shipment Conversions

export function OpenFclAndLclConversionWizard(intoShipmentType: string) {
    let convertShipmentSelector = intoShipmentType === "LCL" ? ShipmentSelectors.ConvertToLCL : ShipmentSelectors.ConvertToFCL;
    OpenConversionWizard(convertShipmentSelector);
}

export function OpenDirectAndHouseConversionWizard(intoShipmentLevel: string) {
    let convertShipmentSelector = intoShipmentLevel === "House" ? ShipmentSelectors.ConvertToHouse : ShipmentSelectors.ConvertToDirect;
    OpenConversionWizard(convertShipmentSelector);
}

export function OpenDirectionConversionWizard() {
    OpenConversionWizard(ShipmentSelectors.ConvertShipmentDirection);
}

export function FillDirectionConversionWizard(shipmentDetails: ShipmentDetails) {
    FillDirection(shipmentDetails.Direction);
    FillShipperAndConsignee(shipmentDetails);
}

export function FillEventNotesWizard(notes: string) {
    if (notes) {
        cy.FillLogTextBox(BaseSelectors.EventNotes, notes);
    }
}

export function ConvertShipmentDirection() {
    ConvertShipment();
}

export function ConvertShipmentType() {
    ConvertShipment();
}

export function ValidateShipmentTypeInHeaderScreen(expectedShipmentType: string) {
    if (expectedShipmentType) {
        cy.get(ShipmentSelectors.ShipmentTypeValue).should("have.text", expectedShipmentType);
    }
}

export function ValidateShipmentDirectionInShortTitle(expectedDirection: string) {
    if (expectedDirection) {
        let directionIconSelector = ShipmentSelectors.ShortTitleDirectionIcon(expectedDirection);
        cy.get(directionIconSelector).should("exist");
    }
}

export function ValidateShipmentNumberInShortTitle(expectedShipmentNumber: string){
    if(expectedShipmentNumber){
        cy.get(ShipmentSelectors.ShipmentNumberInTitle).then((shipmentNumberDiv) => {
            assert.equal(shipmentNumberDiv.text().replace(":", "").trim(), expectedShipmentNumber);
        });
    }
}

export function ValidateEventsTab(expectedEventDetailsList: EventTypeDetails[]) {
    cy.get(ShipmentSelectors.EventsTab).then(($eventTab) => {
        cy.DefineRequestWait(RestAPI.GET, BaseURLs.GetTraceEventsForEntity, RequestAliases.GetTraceEventsForEntity);
        if ($eventTab.hasClass("SelectedMenuItem")) {
            cy.Click(ShipmentSelectors.ShipmentEventsRefreshButton, null);
        } else {
            cy.Click(ShipmentSelectors.EventsTab, null);
        }
        BaseAssertion.AssertStatusCode(RequestAliases.GetTraceEventsForEntity, 200);
        for (let i = 0; i < expectedEventDetailsList.length; i++) {
            let expectedEvent = expectedEventDetailsList[i].Event;
            let expectedNotes = expectedEventDetailsList[i].Notes;
    
            if (expectedEvent) {
                cy.contains(expectedEvent).eq(0).should("exist");
            }
    
            if (expectedEvent && expectedNotes) {
                cy.contains(expectedEvent).eq(0).parents(BaseSelectors.EventItemBox).within(() => {
                    cy.get(BaseSelectors.textarea).should("have.value", expectedNotes);
                });
            }
        }
    });
}

export function ValidateAddButtonInPackagesTab(expectedButtonContains: string){
    if(expectedButtonContains){
        cy.Click(ShipmentSelectors.PackagesTab, null);
        cy.get(ShipmentSelectors.AddPackage).should("have.text", expectedButtonContains);
    }
}

export function ValidatePartnerInPartnersTab(partnerType: string, partnerName: string){
    if(partnerType && partnerName){
        cy.Click(ShipmentSelectors.PartnersTab, null);
        let partnerBoxItemSelector = ShipmentSelectors.PartnerBoxItem(partnerType);
        cy.get(partnerBoxItemSelector).should("exist");
        cy.get(partnerBoxItemSelector).within(() => {
            cy.get(ShipmentSelectors.PartnerName).then((partnerNameDiv) => {
                assert.equal(partnerNameDiv.text().trim(), partnerName);
            });
        });
    }
}

function OpenConversionWizard(convertButtonSelector: string) {
    cy.Click(ShipmentSelectors.ShipmentMoreList, null, true);
    cy.Click(convertButtonSelector, null);
}

function ConvertShipment(){
    cy.DefineRequestWait(RestAPI.PUT, URLs.Shipment, RequestAliases.PutShipment);
    cy.Click(BaseSelectors.RedButton + ":last", null);
}

//#endregion