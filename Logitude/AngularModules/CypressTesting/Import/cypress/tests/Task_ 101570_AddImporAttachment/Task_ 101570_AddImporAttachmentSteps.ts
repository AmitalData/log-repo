import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as AddImpotAttachmentActions from '../../actions/AddImpotAttachmentActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { AddImportAttachmentDetails } from '../../models/AddImportAttachmentDetails';
import { AddImportAttachmentSelectors } from "../../selectors/AddImportAttachmentSelectors";



//#region Create new Export Invoice
Given("the user logged in and navigates to Import workspace", () => {
    cy.Login();
    Actions.NavigatesImportDeclarationWorkspace();
});

Given("Search for file and enter to Customs Attachments", (dataTable) => {
  
    let addImportAttachmentDetails = Assists.CreateInstance<AddImportAttachmentDetails>(dataTable, true);
    AddImpotAttachmentActions.FillSearchField(addImportAttachmentDetails);
  
});

Given("Add new attachment", (dataTable) => {
let addImportAttachmentDetails = Assists.CreateInstance<AddImportAttachmentDetails>(dataTable, true);
AddImpotAttachmentActions.AddFSIE(addImportAttachmentDetails);


});

Given("the user delete the Attachment", (dataTable) => {
    
cy.wait(200);
cy.get('.ConnectedTicketItem iconbutton').invoke('removeAttr','class').invoke('removeProp','class');

  
});


When("disconected the Attachment", () => {
    cy.Click(AddImportAttachmentSelectors.Disconnect, null);
        
 });


Then("the ticket will be empty", () => {
    
    cy.contains('button', 'שאילתא למסמכים').should('be.visible');
 
   
});


//#endregion