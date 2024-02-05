import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { SharedManifestsSelectors } from "../../selectors/SharedManifestsSelectors/SharedManifestsSelectors"
import { SharedManifestsURLs } from "../../constants/SharedManifestsURLs/SharedManifestsURLs";
import { SharedManifestsRequestAliases } from "../../constants/SharedManifestsURLs/SharedManifestsRequestAliases";
import { DocumentsPermissionsDetails } from "../../models/SharedManifestsDetails/DocumentsPermissionsDetails"
import * as Actions from "../Actions"
import * as MaintenanceBaseActions from "../BaseActions"
import { SendDocs } from "../../../../Shipment/cypress/actions/Actions";
import * as Authentication from "./../../../../Base/cypress/commands/Authentication"
import { ValidateSingleErrorMessage } from "../BaseActions";
import { URLs } from './../../../../Shipment/cypress/constants/URLs';
import { RequestAliases } from '../../../../Base/cypress/constants/RequestAliases';
import { ShipmentContext } from '../../../../Shipment/cypress/models/ShipmentContext';
import { MainCarriageLegDetails } from "cypress/models/SharedManifestsDetails/MainCarriageLegDetails";

let SharedKey: string

export function ToOperations() {
    cy.get(SharedManifestsSelectors.OperationsTab).click()
}

export function ToSharedManifests() {
    cy.get(SharedManifestsSelectors.SharedManifests).click()
}
export function ToSharedManifestsDocumentsPermissions() {
    cy.get(SharedManifestsSelectors.DocumentsPermissions).click()
}
export function UpdateDocumentsPermissions(documentsPermissionsDetails: DocumentsPermissionsDetails, shipmentLevel: string) {

    cy.get('[data-cy="' + SharedManifestsSelectors.AirManifest + '_' + shipmentLevel + '_input"]')
    Actions.FillInputCheckBoxProcess('[data-cy="' + SharedManifestsSelectors.AirManifest + '_' + shipmentLevel + '_input"]',
        documentsPermissionsDetails.AirManifest)
    Actions.FillInputCheckBoxProcess('[data-cy="' + SharedManifestsSelectors.AirwaybillLabels + '_' + shipmentLevel + '_input"]',
        documentsPermissionsDetails.AirwaybillLabels)
    Actions.FillInputCheckBoxProcess('[data-cy="' + SharedManifestsSelectors.ArrivalNotice + '_' + shipmentLevel + '_input"]',
        documentsPermissionsDetails.ArrivalNotice)
}

export function SaveSharedManifestsDocumentsPermissions() {
    DefinePutSharedManifestsDocumentsPermissionsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePutSharedManifestsDocumentsPermissionsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedManifestsURLs.putupdatedocumenttypepmlists, SharedManifestsRequestAliases.updatedocumenttypepmlists);
}

export function AssertSaveSharedManifestsDocumentsPermissions() {
    AssertPutPartnersPermissions();
}
function AssertPutPartnersPermissions() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.updatedocumenttypepmlists, 200).then((interception) => {
    });
}
export function FillInvitation(email: string) {
    cy.get(SharedManifestsSelectors.AgentTHSharedLogistics).click()
    cy.get(SharedManifestsSelectors.SendInvitation).click()
    cy.get(SharedManifestsSelectors.Email).type(email)
}

export function SendInvitation() {
    DefinePostAgentSharedLogisticsKeyInvitationRequest();
    cy.get(SharedManifestsSelectors.Send).click()
    cy.get(SharedManifestsSelectors.Cancel).click()
}

function DefinePostAgentSharedLogisticsKeyInvitationRequest() {
    cy.DefineRequestWait(RestAPI.POST, SharedManifestsURLs.PostAgentSharedLogisticsKeyInvitation, SharedManifestsRequestAliases.PostAgentSharedLogisticsKeyInvitation);
}


export function AssertSendInvitation() {
    AssertPostAgentSharedLogisticsKeyInvitation();
}
function AssertPostAgentSharedLogisticsKeyInvitation() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.PostAgentSharedLogisticsKeyInvitation, 200).then((interception) => {
    });
}

export function GetSharedKey() {

    cy.get(SharedManifestsSelectors.SharedKey).invoke('val').then((Lable) => {
        cy.log(Lable.toString())
        SharedKey = (Lable.toString())
    })
}

export function LoginSecondTenant(customerCareUser = true) {
    let mode = Cypress.env("Mode");

    if (mode.toLowerCase() === "development") {
        DevelopmentModeLogin(customerCareUser)
    }
    else {
        ReleaseModeLogin(customerCareUser)
    }
}

function DevelopmentModeLogin(customerCareUser) {

    cy.fixture("Login.json").then(loginData => {
debugger
        let email = customerCareUser ? loginData.customerCareEmail : loginData.email;
        let password = customerCareUser ? loginData.customerCarePassword : loginData.password;
        let url = loginData.url;
        let tenant = customerCareUser ? loginData.secondTenant : null;
        Authentication.CompleteLoginProcess(email, password, url, tenant);
    });
}

function ReleaseModeLogin(customerCareUser) {

    let email = customerCareUser ? Cypress.env("CustomerCareEmail") : Cypress.env("Email");
    let password = customerCareUser ? Cypress.env("CustomerCarePassword") : Cypress.env("Password");
    let url = Cypress.env("Url");
    let tenant = customerCareUser ? Cypress.env("secondTenant") : null;
    Authentication.CompleteLoginProcess(email, password, url, tenant);

}
export function FillSharedKey() {
    cy.get(SharedManifestsSelectors.AgentTHSharedLogistics).click()
    cy.get(SharedManifestsSelectors.AcceptInvitation).click()
    cy.get(SharedManifestsSelectors.Key).type(SharedKey)
}

export function AcceptInvitation() {
    DefinePutAgentSharedLogisticsKeysRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePutAgentSharedLogisticsKeysRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedManifestsURLs.PutAgentSharedLogisticsKeys, SharedManifestsRequestAliases.putAgentSharedLogisticsKeys);
}


export function AssertAcceptInvitation() {
    AssertPutAgentSharedLogisticsKeys();
}
function AssertPutAgentSharedLogisticsKeys() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.putAgentSharedLogisticsKeys, 200).then((interception) => {
    });
}

export function SignOut() {
    cy.get(SharedManifestsSelectors.SignOut).click()
}

export function ShareManifestAction() {
    cy.get(SharedManifestsSelectors.SharingActions).click()
    cy.get(SharedManifestsSelectors.ShareManifest).click()

}
export function ExitShipment() {
    cy.get(SharedManifestsSelectors.BackBottonBody).click()
}
export function ValidateShareManifestErrorMessage(ErrorMessage: string) {
    MaintenanceBaseActions.ValidateSingleErrorMessage(ErrorMessage)
    cy.get(SharedManifestsSelectors.Cancel).click()
}

export function UpdateShipment(mainCarriageLeg: MainCarriageLegDetails) {
    AddMasterNumber(mainCarriageLeg.masterNumber, mainCarriageLeg.AgentCarrier)
    DeleteConsignee()
}

function AddMasterNumber(masterNumber: string, MainCarriageCarrier: string) {
    cy.get(SharedManifestsSelectors.ShipmentRoutings).click()
    cy.get(SharedManifestsSelectors.MainCarriage).click()
    SelectFirstDropDownListItem(SharedManifestsSelectors.MainCarriageCarrier, MainCarriageCarrier)
    cy.get(SharedManifestsSelectors.ShipmentMasterNo).type(masterNumber)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);

}
function DeleteConsignee() {
    cy.get(SharedManifestsSelectors.ShipmentPartners).click()
    cy.get(SharedManifestsSelectors.DeleteConsignee).click()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
export function SelectFirstDropDownListItem(selector: string, text: string) {
    cy.get(selector).click({ force: true }),
        cy.get(selector).type(text)
            .should('have.value', text)
    cy.get('.DropDownListItem:first').click()
}


export function SaveMasterShipment() {
    DefinePutMasterShipmentRequest();
    cy.get(SharedManifestsSelectors.ShipmentSave).click();
}

function DefinePutMasterShipmentRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedManifestsURLs.Shipment, SharedManifestsRequestAliases.PutShipment);
}

export function AssertSaveMasterShipment() {
    AssertPutMasterShipment();
}
function AssertPutMasterShipment() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.PutShipment, 200).then((interception) => {
    });
}
export function AddConsignee(code: string) {
    cy.get(SharedManifestsSelectors.ShipmentPartners).click()
    cy.get(SharedManifestsSelectors.AddPartner).click()
    cy.get(SharedManifestsSelectors.AddConsignee).click()
    SelectFirstDropDownListItem(SharedManifestsSelectors.ConsigneeId, code)
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

export function ShareManifest() {
    DefineGetSharedAgentManifestRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefineGetSharedAgentManifestRequest() {
    cy.DefineRequestWait(RestAPI.GET, SharedManifestsURLs.SharedAgentManifest, SharedManifestsRequestAliases.GetSharedAgentManifest);
}

export function AssertShareManifest() {
    AssertGetSharedAgentManifest();
    cy.contains('Your Manifest shared successfully')
    cy.get(SharedManifestsSelectors.Close).click()
}
function AssertGetSharedAgentManifest() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.GetSharedAgentManifest, 200).then((interception) => {
    });
}

export function SearchManifest(TransrportMode: string, ShipmentNumber: string) {
    cy.get('[data-cy="' + TransrportMode + ' Manifests"]').click()
    cy.get(SharedManifestsSelectors.Search).type(ShipmentNumber)

}
export function AssertManifestExist(MAWB: string) {
    cy.get(SharedManifestsSelectors.FirstRowMAWB).within(() => {
        cy.contains(MAWB)
    }).click()
}

export function CreateShipment(ShipmentNumber: string, MasterMainCarriageCarrier: string) {
    cy.get(SharedManifestsSelectors.Create).click()
    //BaseAssertion.AssertElementHaveValue(SharedManifestsSelectors.AgentReference, ShipmentNumber)
    cy.DefineRequestWait(RestAPI.POST, URLs.Shipment, RequestAliases.ShipmentRequest)
    cy.get(SharedManifestsSelectors.MasterMainCarriageCarrier).click()
    SelectFirstDropDownListItem(SharedManifestsSelectors.MasterMainCarriageCarrier, MasterMainCarriageCarrier)
    cy.get(SharedManifestsSelectors.CreateMaster).click()
}

export function AssertCreateShipment() {
    BaseAssertion.AssertStatusCode(RequestAliases.ShipmentRequest, 200).then((interception) => {
        //ShipmentContext.MasterNumber = interception.response.body.ShipmentNumber;
        assert.equal(interception.response.body.DirectionName, "Import", 'valus equal')
    })
    BaseAssertion.AssertMessageWindow("Your Shipment was successfully created.")
}

export function CancelManifest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedManifestsURLs.agentsharedmanifests, SharedManifestsRequestAliases.Putagentsharedmanifests);
    cy.get(SharedManifestsSelectors.CancelManifest).click()
}

export function AssertCancelManifest() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.Putagentsharedmanifests, 200).then((interception) => {
    });
}

export function MarkAsCompleted() {
    cy.DefineRequestWait(RestAPI.PUT, SharedManifestsURLs.agentsharedmanifests, SharedManifestsRequestAliases.Putagentsharedmanifests);
    cy.get(SharedManifestsSelectors.MarkAsCompleted).click()
}

export function AssertMarkAsCompleted() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.Putagentsharedmanifests, 200).then((interception) => {
    });
}
export function clearMasterNumber() {
    cy.get(SharedManifestsSelectors.ShipmentRoutings).click()
    cy.get(SharedManifestsSelectors.MainCarriage).click()
    cy.get(SharedManifestsSelectors.ShipmentMasterNo).clear()
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);

}

//#region Share Document
export function OpenSharedLogistic() {
    cy.get(SharedManifestsSelectors.GeneralMHSharedLogistics).click({ force: true })
    cy.get(SharedManifestsSelectors.DocumentsPermissions).click({ force: true })
}
export function SearchForAgentDocument(DocumentName: string) {
    cy.get(SharedManifestsSelectors.AgentView).click({ force: true })
    cy.FillLogTextBox(SharedManifestsSelectors.AgentSearch, DocumentName)

}

export function EditPermissions() {
    cy.get(SharedManifestsSelectors.CheckBox).click({ force: true })
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}
export function OpenDocTab(DocumentName: string) {
    cy.get(SharedManifestsSelectors.ShipmentTHDocsOut).click({ force: true })
    cy.wait(1000)
    cy.FillLogTextBox(SharedManifestsSelectors.SearchBox, DocumentName)
    cy.wait(1000)
}
export function BuildDocument() {
    cy.get(SharedManifestsSelectors.BuildDocsOut).click({ force: true })
    cy.wait(8000)
    cy.get(SharedManifestsSelectors.CloseDocument).click({ force: true })
    cy.wait(7000)

}
export function ShareDocument() {
    cy.get(SharedManifestsSelectors.SharingActions).click({ force: true })
    cy.get(SharedManifestsSelectors.ShareDocuments).click({ force: true })

}
export function ClickShare() {
    //cy.get(SharedManifestsSelectors.Share.).click({ force: true })
    cy.Click(SharedManifestsSelectors.Share, "Share", true);
    cy.get(SharedManifestsSelectors.MessageOK).click({ force: true })

}

export function OpenDocInTab(DocumentName: string) {
    cy.get(SharedManifestsSelectors.ShipmentTHDocsIn).click({ force: true })
    cy.FillLogTextBox(SharedManifestsSelectors.SearchBox, DocumentName)
}

export function UploadDoc() {
    cy.get('div').contains("Air Manifest.pdf")
}

