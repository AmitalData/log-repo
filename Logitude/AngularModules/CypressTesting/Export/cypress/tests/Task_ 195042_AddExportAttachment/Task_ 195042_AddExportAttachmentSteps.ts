import { Given, When, Then } from 'cypress-cucumber-preprocessor/steps';
import * as AddExportAttachmentActions from '../../actions/AddExportAttachmentActions';
import * as Actions from '../../actions/Actions';
import * as Assists from "../../../../Base/cypress/assists/Assists";
import { AddExportAttachmentDetails } from '../../models/AddExportAttachmentDetails';
import { AddExportAttachmentSelectors } from "../../selectors/AddExportAttachmentSelectors";



//#region Create new Export Invoice
Given("the user logged in and navigates to Export workspace", () => {
    cy.Login();
    Actions.NavigatesExportsWizerd();
});

Given("Search for file and enter to Customs Attachments", (dataTable) => {
  
    let addExportAttachmentDetails = Assists.CreateInstance<AddExportAttachmentDetails>(dataTable, true);
    AddExportAttachmentActions.FillSearchField(addExportAttachmentDetails);
  
});

Given("Add new attachment", (dataTable) => {
let addExportAttachmentDetails = Assists.CreateInstance<AddExportAttachmentDetails>(dataTable, true);
AddExportAttachmentActions.AddFSIE(addExportAttachmentDetails);


});

Given("the user delete the Attachment", (dataTable) => {
    
cy.wait(200);
cy.get('.ConnectedTicketItem iconbutton').invoke('removeAttr','class').invoke('removeProp','class');

  
});


When("disconected the Attachment", () => {
    cy.Click(AddExportAttachmentSelectors.Disconnect, null);
        
 });


Then("the ticket will be empty", () => {
    
    cy.contains('button', 'שאילתא למסמכים').should('be.visible');
 
   
});


//#endregion