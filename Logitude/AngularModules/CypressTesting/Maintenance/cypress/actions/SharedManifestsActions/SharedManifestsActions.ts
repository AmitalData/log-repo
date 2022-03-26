import { BaseSelectors } from "../../../../Base/cypress/selectors/BaseSelectors";
import * as BaseAssertion from "../../../../Base/cypress/actions/Assertion";
import { RestAPI } from "../../../../Base/cypress/constants/RestAPI";
import * as BaseActions from "../../../../Base/cypress/actions/Actions";
import { SharedManifestsSelectors } from "../../selectors/SharedManifestsSelectors/SharedManifestsSelectors"
import { SharedManifestsURLs } from "../../constants/SharedManifestsURLs/SharedManifestsURLs";
import { SharedManifestsRequestAliases } from "../../constants/SharedManifestsURLs/SharedManifestsRequestAliases";
import { DocumentsPermissionsDetails } from "../../models/SharedManifestsDetails/DocumentsPermissionsDetails"
import * as Actions from "../Actions"


let CustomerCode:string

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