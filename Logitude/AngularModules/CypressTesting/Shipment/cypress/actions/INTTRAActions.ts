import { ShipmentSelectors } from '../selectors/Selectors';
import { GeneralSettingsDetails } from '../models/INTTRA/GeneralSettingsDetails';
import { InOutSettingsDetails } from '../models/INTTRA/InOutSettingsDetails';
import { BranchSettingsDetails } from '../models/INTTRA/BranchSettingsDetails';
import { RegistrationSettingsDetails } from '../models/INTTRA/RegistrationSettingsDetails';
import { RequiredToSendBookingShippingInstructions } from "../models/INTTRA/RequiredToSendBookingShippingInstructions";
import { BaseSelectors } from '../../../Base/cypress/selectors/BaseSelectors';
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { URLs } from '../constants/URLs';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI';
import { RequestAliases } from '../../../Base/cypress/constants/RequestAliases';
import { BaseURLs } from '../../../Base/cypress/constants/URLs';
import { ValidationMessageDetails } from '../../../Base/cypress/models/ValidationMessageDetails';


export function NavigateToMaintenanceMenu() {
    cy.Click(BaseSelectors.MaintenanceMenu, null);
}

export function OpenINTTRASettingsWizard() {
    cy.Click(BaseSelectors.MaintenanceTransmissionsTab, null);
    cy.DefineRequestWait(RestAPI.GET, URLs.GetINTTRASettings, RequestAliases.GetINTTRASettings);
    cy.Click(BaseSelectors.INTTRAMaintenanceItem, null);
    BaseAssertion.AssertStatusCode(RequestAliases.GetINTTRASettings, 200);
}

export function FillINTTRAGeneralSettings(generalSettingsDetails: GeneralSettingsDetails) {
    FillINTTRAMode(generalSettingsDetails.Mode);
    FillINTTRAId(generalSettingsDetails.INTTRAID);
    FillINTTRAAlias(generalSettingsDetails.Alias);
}

export function FillINTTRAInOutSettings(inOutSettingsDetails: InOutSettingsDetails, settingsType: string) {
    OpenFTPDetailWizard(settingsType);
    FillFTPDetailUserName(inOutSettingsDetails.UserName);
    FillFTPDetailPassword(inOutSettingsDetails.Password);
    FillFTPDetailHost(inOutSettingsDetails.Host);
    FillFTPDetailFolder(inOutSettingsDetails.Folder);
    cy.Click(ShipmentSelectors.SaveFTPDetailButton, null);
    BaseAssertion.AssertStatusCode(RequestAliases.SaveFTPDetails, 200);
}

export function FillINTTRABranchesSettings(branchSettingsDetailsList: BranchSettingsDetails[]) {
    for (let i = 0; i < branchSettingsDetailsList.length; i++) {
        let branchName = branchSettingsDetailsList[i].BranchName
        FillBranchINTTRAId(branchName, branchSettingsDetailsList[i].INTTRAID);
        FillBranchINTTRAAlias(branchName, branchSettingsDetailsList[i].PartyAlias);
        cy.get(BaseSelectors.LoggedUser).invoke('text').then(text => {
            var Contact = text.replace(/\s/g, "");
            FillBranchINTTRAContact(branchName,Contact);
        })
    }
}

export function FillINTTRARegistrationSettings(registrationSettingsDetailsList: RegistrationSettingsDetails[]) {
    for (let i = 0; i < registrationSettingsDetailsList.length; i++) {
        let branchName = registrationSettingsDetailsList[i].BranchName;
        let registrationCode = registrationSettingsDetailsList[i].RegistrationCode;
        let registrationCheckBoxSelector = ShipmentSelectors.INTTRARegistrationCheckBox(branchName, registrationCode);
        cy.get(registrationCheckBoxSelector).check({ force: true });
    }
}

export function SaveINTTRASettings() {
    cy.DefineRequestWait(RestAPI.PUT, URLs.PutINTTRASettings, RequestAliases.PutINTTRASettings);
    cy.Click(BaseSelectors.RedButton, null);
}

export function ValidateSaveINTTRASettings() {
    BaseAssertion.AssertStatusCode(RequestAliases.PutINTTRASettings, 200);
}

export function ValidateMessagesForSendingEBookingOrShippingInstructions(validationMessageDetailsList: ValidationMessageDetails[]) {
    cy.get(BaseSelectors.ValidationSummaryBlock).eq(1).find(BaseSelectors.ul)
        .within(() => {
            for (let i = 0; i < validationMessageDetailsList.length; i++) {
                cy.get(BaseSelectors.li).contains(validationMessageDetailsList[i].Message.trim()).should("exist");
            }
        });

    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsClose);
}

export function OpenSendBookingWizard() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetBookingMessageResultValidate, RequestAliases.GetBookingMessageResultValidate);
    cy.Click(BaseSelectors.Button, ShipmentSelectors.ContainsSendEBooking);
    BaseAssertion.AssertStatusCode(RequestAliases.GetBookingMessageResultValidate, 200);
}

export function OpenSendShippingInstructionsWizard() {
    cy.DefineRequestWait(RestAPI.GET, URLs.GetShippingInstructionMessageResultValidate, RequestAliases.GetShippingInstructionMessageResultValidate);
    cy.Click(BaseSelectors.Button, ShipmentSelectors.ContainsSendShippingInstructions);
    BaseAssertion.AssertStatusCode(RequestAliases.GetShippingInstructionMessageResultValidate, 200);
}

export function FillRequiredToSendBooking(requiredToSendBookingShippingInstructions: RequiredToSendBookingShippingInstructions) {
    FillRequiredToSendBookingInGeneralTab(requiredToSendBookingShippingInstructions);
    FillRequiredToSendBookingInOrdersTab(requiredToSendBookingShippingInstructions);
    FillRequiredToSendBookingInPartnersTab(requiredToSendBookingShippingInstructions);
    FillRequiredToSendBookingInRoutingsTab(requiredToSendBookingShippingInstructions);
}

export function FillRequiredToSendShippingInstructions(requiredToSendBookingShippingInstructions: RequiredToSendBookingShippingInstructions) {
    FillRequiredToSendBookingInGeneralTab(requiredToSendBookingShippingInstructions);
    FillRequiredToSendBookingInOrdersTab(requiredToSendBookingShippingInstructions);
    FillContainerNumberInPackagesTbe(requiredToSendBookingShippingInstructions);
}

export function SendBookingRequest() {
    cy.DefineRequestWait(RestAPI.GET, URLs.INTTRAWebServiceSendEBooking, RequestAliases.INTTRAWebServiceSendEBooking);
    cy.Click(BaseSelectors.Button, ShipmentSelectors.ContainsRequestBooking);
}

export function SendShippingInstructionsRequest() {
    cy.Click(BaseSelectors.RedButton, ShipmentSelectors.ContainsSend);
}

export function ValidateSendBookingRequest() {
    BaseAssertion.AssertStatusCode(RequestAliases.INTTRAWebServiceSendEBooking, 200);
}

export function ValidateSendInstructionsRequest() {
    BaseAssertion.AssertElementContain(ShipmentSelectors.ShippingInstructionsResultMessage, ShipmentSelectors.ContainsMessageHasBeenSentSuccessfully);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainClose);
    OpenSendBookingWizard();
}

export function ValidateBookingRequestStatus(status: string) {
    cy.get(ShipmentSelectors.INTTRABookingStatus).should("have.text", status);
    cy.Click(BaseSelectors.Button, BaseSelectors.ContainsClose)
    cy.Click(BaseSelectors.Backbutton, null, false);
}

function FillINTTRAMode(mode: string) {
    if (mode) {
        cy.ClickRadio(ShipmentSelectors.INTTRASettingsModeRadio(mode));
    }
}

function FillINTTRAId(id: string) {
    if (id) {
        cy.FillLogTextBox(ShipmentSelectors.INTTRASettingId, id);
    }
}

function FillINTTRAAlias(alias: string) {
    if (alias) {
        cy.FillLogTextBox(ShipmentSelectors.INTTRASettingAlias, alias);
    }
}

function FillFTPDetailUserName(username: string) {
    if (username) {
        cy.FillLogTextBox(ShipmentSelectors.FTPDetailUserName, username);
    }
}

function FillFTPDetailPassword(password: string) {
    if (password) {
        cy.FillLogTextBox(ShipmentSelectors.FTPDetailPassword, password);
    }
}

function FillFTPDetailHost(host: string) {
    if (host) {
        cy.FillLogTextBox(ShipmentSelectors.FTPDetailHost, host);
    }
}

function FillFTPDetailFolder(folder: string) {
    if (folder) {
        cy.FillLogTextBox(ShipmentSelectors.FTPDetailFolder, folder);
    }
}

function OpenFTPDetailWizard(settingsType: string) {
    let addButtonSelector = ShipmentSelectors.InOutSettingsHostAddButton(settingsType);
    let editButtonSelector = ShipmentSelectors.InOutSettingsHostEditButton(settingsType);
    cy.get(editButtonSelector)
        .then($editButton => {
            let isEditButtonDisabled = $editButton.is(":disabled");
            let requestType = isEditButtonDisabled ? RestAPI.POST : RestAPI.PUT;
            let openFTPDetailWizardSelector = isEditButtonDisabled ? addButtonSelector : editButtonSelector;
            cy.DefineRequestWait(requestType, URLs.FTPDetails, RequestAliases.SaveFTPDetails);
            cy.Click(openFTPDetailWizardSelector, null);
        });
}

function FillRequiredToSendBookingInGeneralTab(requiredToSendBookingShippingInstructions: RequiredToSendBookingShippingInstructions) {
    cy.Click(ShipmentSelectors.GeneralTab, null);
    if (requiredToSendBookingShippingInstructions.BranchName) {
        cy.FillLogLov(ShipmentSelectors.ShipmentBranch, requiredToSendBookingShippingInstructions.BranchName, true);
    }
    if (requiredToSendBookingShippingInstructions.MoveType) {
        cy.FillLogLov(ShipmentSelectors.ShipmentMoveType, requiredToSendBookingShippingInstructions.MoveType, true)
    }
}

function FillContainerNumberInPackagesTbe(requiredToSendBookingShippingInstructions: RequiredToSendBookingShippingInstructions) {
    cy.Click(ShipmentSelectors.PackagesTab, null);
    cy.Click(ShipmentSelectors.EditPackage, null)
    if (requiredToSendBookingShippingInstructions.ContainerNumber) {
        cy.FillLogTextBox(ShipmentSelectors.ContainerNumber, requiredToSendBookingShippingInstructions.ContainerNumber)
    }
    cy.Click(BaseSelectors.RedButton, null)
}

function FillRequiredToSendBookingInOrdersTab(requiredToSendBookingShippingInstructions: RequiredToSendBookingShippingInstructions) {
    cy.Click(ShipmentSelectors.OrdersTab, null);
    if (requiredToSendBookingShippingInstructions.ShippingLine) {
        cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageCarrierId, requiredToSendBookingShippingInstructions.ShippingLine, false);
    }
    if (requiredToSendBookingShippingInstructions.ContractNumber) {
        cy.FillLogTextBox(ShipmentSelectors.ShipmentINTTRAContractNumber, requiredToSendBookingShippingInstructions.ContractNumber);
    }
    if (requiredToSendBookingShippingInstructions.DescriptionOfGoods) {
        cy.FillLogTextBox(ShipmentSelectors.ShipmentDescriptionOfGoods, requiredToSendBookingShippingInstructions.DescriptionOfGoods);
    }
    if (requiredToSendBookingShippingInstructions.BookingConfirmationNumber) {
        cy.FillLogTextBox(ShipmentSelectors.ShipmentBookingConfirmationNumber, requiredToSendBookingShippingInstructions.BookingConfirmationNumber);
    }
}

function FillRequiredToSendBookingInPartnersTab(requiredToSendBookingShippingInstructions: RequiredToSendBookingShippingInstructions) {
    let shipperContact = requiredToSendBookingShippingInstructions.ShipperContact;
    if (shipperContact) {
        cy.Click(ShipmentSelectors.PartnersTab, null);
        cy.Click(ShipmentSelectors.EditShipper, null);
        cy.DefineRequestWait(RestAPI.GET, URLs.ContactViewsGetByFilters + shipperContact + "**", RequestAliases.ContactLogLovLoad);
        cy.get(ShipmentSelectors.ShipmentShipperContact).type("{selectall}" + shipperContact);
        cy.wait("@" + RequestAliases.ContactLogLovLoad).then((interception) => {
            if (interception.response.body.Result.filter((c: { EnglishName: string; }) => c.EnglishName.toLowerCase() === shipperContact.toLowerCase()).length === 0) {
                AddNewShipperContact(shipperContact);
            } else {
                cy.get(BaseSelectors.DropDownListItem).children().eq(0).click();
            }
        });
        cy.Click(ShipmentSelectors.PartnerOKButton, null);
    }
}

function AddNewShipperContact(shipperContact: string) {
    if (shipperContact) {
        cy.Click(ShipmentSelectors.AddShipperContactButton, null);
        cy.FillLogTextBox(ShipmentSelectors.ContactEmail, shipperContact + "@test.com")
        cy.FillLogTextBox(ShipmentSelectors.ContactEnglishName, shipperContact);
        cy.DefineRequestWait(RestAPI.POST, BaseURLs.Contacts, RequestAliases.PostContact);
        cy.Click(ShipmentSelectors.SaveShipperContactButton, null);
        BaseAssertion.AssertStatusCode(RequestAliases.PostContact, 200);
    }
}

function FillRequiredToSendBookingInRoutingsTab(requiredToSendBookingShippingInstructions: RequiredToSendBookingShippingInstructions) {
    cy.Click(ShipmentSelectors.RoutingsTab, null);
    cy.Click(ShipmentSelectors.EditRoutingMainCarriage, null);
    let ETDDate = requiredToSendBookingShippingInstructions.ETDDate;
    let ETDTime = requiredToSendBookingShippingInstructions.ETDTime;
    let vessel = requiredToSendBookingShippingInstructions.Vessel;
    if (ETDDate) {
        cy.FillDate(ShipmentSelectors.ShipmentMainCarriageETDDate, ETDDate);
    }
    if (ETDTime) {
        cy.FillLogTextBox(ShipmentSelectors.ShipmentMainCarriageETDTime, ETDTime);
    }
    if (vessel) {
        cy.FillLogLov(ShipmentSelectors.ShipmentMainCarriageVessel, vessel, true);
    }
    cy.Click(ShipmentSelectors.MainCarriageOKBtn, null);
}

function FillBranchINTTRAId(branchName: string, INTTRAId: string) {
    if (branchName && INTTRAId) {
        cy.FillLogTextBox(ShipmentSelectors.BranchINTTRAId(branchName), INTTRAId);
    }
}

function FillBranchINTTRAAlias(branchName: string, INTTRAAlias: string) {
    if (branchName && INTTRAAlias) {
        cy.FillLogTextBox(ShipmentSelectors.BranchINTTRAAlias(branchName), INTTRAAlias);
    }
}

function FillBranchINTTRAContact(branchName: string, INTTRAContact: string) {
    if (branchName && INTTRAContact) {
        cy.FillLogLov(ShipmentSelectors.BranchINTTRAContact(branchName), INTTRAContact, false, true);
    }
}