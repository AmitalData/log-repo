import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { SharedManifestsSelectors } from "../../selectors/SharedManifestsSelectors/SharedManifestsSelectors"
import { SharedManifestsURLs } from "../../constants/SharedManifestsURLs/SharedManifestsURLs";
import { SharedManifestsRequestAliases } from "../../constants/SharedManifestsURLs/SharedManifestsRequestAliases";
import { DocumentsPermissionsDetails } from "../../models/SharedManifestsDetails/DocumentsPermissionsDetails"
import * as Actions from "../Actions"
import { SendDocs } from "../../../../Shipment/cypress/actions/Actions";
import * as Authentication from "./../../../../Base/cypress/commands/Authentication"

let SharedKey:string

export function ToOperations(){
cy.get(SharedManifestsSelectors.OperationsTab).click()
}

export function ToSharedManifests(){
cy.get(SharedManifestsSelectors.SharedManifests).click()
}
export function ToSharedManifestsDocumentsPermissions(){
    cy.get(SharedManifestsSelectors.DocumentsPermissions).click()
}
export function UpdateDocumentsPermissions(documentsPermissionsDetails:DocumentsPermissionsDetails,shipmentLevel:string ){

    cy.get('[data-cy="'+SharedManifestsSelectors.AirManifest+'_'+shipmentLevel+'_input"]')
    Actions.FillInputCheckBoxProcess('[data-cy="'+SharedManifestsSelectors.AirManifest+'_'+shipmentLevel+'_input"]', 
    documentsPermissionsDetails.AirManifest)
    Actions.FillInputCheckBoxProcess('[data-cy="'+SharedManifestsSelectors.AirwaybillLabels+'_'+shipmentLevel+'_input"]', 
    documentsPermissionsDetails.AirwaybillLabels)
    Actions.FillInputCheckBoxProcess('[data-cy="'+SharedManifestsSelectors.ArrivalNotice+'_'+shipmentLevel+'_input"]', 
    documentsPermissionsDetails.ArrivalNotice)
}

export function SaveSharedManifestsDocumentsPermissions(){
    DefinePutSharedManifestsDocumentsPermissionsRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
}

function DefinePutSharedManifestsDocumentsPermissionsRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedManifestsURLs.putupdatedocumenttypepmlists, SharedManifestsRequestAliases.updatedocumenttypepmlists);
}

export function AssertSaveSharedManifestsDocumentsPermissions(){
    AssertPutPartnersPermissions();
}
function AssertPutPartnersPermissions() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.updatedocumenttypepmlists, 200).then((interception) => {
    });
}
export function FillInvitation(email:string){
    cy.get(SharedManifestsSelectors.AgentTHSharedLogistics).click()
    cy.get(SharedManifestsSelectors.SendInvitation).click()
    cy.get(SharedManifestsSelectors.Email).type(email)
  }

  export function SendInvitation(){
    DefinePostAgentSharedLogisticsKeyInvitationRequest();
    cy.get(SharedManifestsSelectors.Send).click()
    cy.get(SharedManifestsSelectors.Cancel).click()
  }

function DefinePostAgentSharedLogisticsKeyInvitationRequest() {
    cy.DefineRequestWait(RestAPI.POST, SharedManifestsURLs.PostAgentSharedLogisticsKeyInvitation, SharedManifestsRequestAliases.PostAgentSharedLogisticsKeyInvitation);
}


export function AssertSendInvitation(){
    AssertPostAgentSharedLogisticsKeyInvitation();
}
function AssertPostAgentSharedLogisticsKeyInvitation() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.PostAgentSharedLogisticsKeyInvitation, 200).then((interception) => {
    });
}

export function GetSharedKey(){

    cy.get(SharedManifestsSelectors.SharedKey).invoke('val').then((Lable) => {
        cy.log(Lable.toString())
        SharedKey = (Lable.toString())
    })
}

export function LoginSecondTenant(){
     
    cy.fixture("Login.json").then(loginData => {
        let email = loginData.customerCareEmail;
        let password = loginData.customerCarePassword ;
        let url = loginData.url;
        let tenant = loginData.secondTenant ;
        Authentication.CompleteLoginProcess(email, password, url, tenant);
    });

}

  export function FillSharedKey(){
    cy.get(SharedManifestsSelectors.AgentTHSharedLogistics).click()
    cy.get(SharedManifestsSelectors.AcceptInvitation).click()
    cy.get(SharedManifestsSelectors.Key).type(SharedKey)
  }

  export function AcceptInvitation(){
    DefinePutAgentSharedLogisticsKeysRequest();
    cy.Click(BaseSelectors.RedButton + BaseSelectors.LastElement, null);
  }

function DefinePutAgentSharedLogisticsKeysRequest() {
    cy.DefineRequestWait(RestAPI.PUT, SharedManifestsURLs.PutAgentSharedLogisticsKeys, SharedManifestsRequestAliases.putAgentSharedLogisticsKeys);
}


export function AssertAcceptInvitation(){
    AssertPutAgentSharedLogisticsKeys();
}
function AssertPutAgentSharedLogisticsKeys() {
    BaseAssertion.AssertStatusCode(SharedManifestsRequestAliases.putAgentSharedLogisticsKeys, 200).then((interception) => {
    });
}

export function SignOut(){
    cy.get(SharedManifestsSelectors.SignOut).click()
}