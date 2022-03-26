
import { Given, When, Then, And } from "cypress-cucumber-preprocessor/steps";
import * as Assists from "../../../../../Base/cypress/assists/Assists";
import * as SharedManifestsActions from "../../../actions/SharedManifestsActions/SharedManifestsActions";
import * as Actions from "../../../actions/Actions";
import { ContactDetails } from "../../../models/ContactDetails";
import { BaseSelectors } from "../../../../../Base/cypress/selectors/BaseSelectors";
import { MaintenanceSelectors } from "../../../selectors/Selectors";
import { DocumentsPermissionsDetails } from "../../../models/SharedManifestsDetails/DocumentsPermissionsDetails";

//#region variables


//#endregion
Given("the user logged in", () => { 
    cy.Login()
});
Given("the user navigates to operations", () => { 
    SharedManifestsActions.ToOperations()
});
Given("chooses shared manifests tab", () => { 
    SharedManifestsActions.ToSharedManifests()
});
Given("goes to Documents Permissions", () => { 
SharedManifestsActions.ToSharedManifestsDocumentsPermissions()
});
Given("update Documents Permissions for {string} shipments as follows", (shipmentLevel,dataTable) => {
    
    let documentsPermissionsDetails = Assists.CreateInstance<DocumentsPermissionsDetails>(dataTable, true);
    SharedManifestsActions.UpdateDocumentsPermissions(documentsPermissionsDetails,shipmentLevel)

});

When("the user save documents permissions changes", () => { 
    SharedManifestsActions.SaveSharedManifestsDocumentsPermissions()
});
Then("the documents permissions should updated successfully", () => { 
    SharedManifestsActions.AssertSaveSharedManifestsDocumentsPermissions()
});